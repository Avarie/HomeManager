
angular.module('manager')
    .controller('ExpensesController', [
        '$scope', '$http', '$modal', 'settingsService', 'Str',
        function ($scope, $http, $modal, $db, str) {

            String.prototype.replaceAll = function (search, replacement) {
                var target = this;
                return target.replace(new RegExp(search, 'g'), replacement);
            };

            var api = $db.getExpenseApi("Expenses");

            $scope.expensesData = [];
            $scope.selectedExpenses = [];

            $scope.current = {
                user: 0,
                category: 0,
                search: ""
            };

            $scope.$watchCollection('selectedExpenses', function () {
                $scope.selected = angular.copy($scope.selectedExpenses[0]);
            });

            $scope.expensesGrid = {
                data: 'expensesData',
                multiSelect: false,
                selectedItems: $scope.selectedExpenses,
                enableColumnResize: true,
                enableCellSelection: true,
                enableRowSelection: true,
                enableCellEdit: false,
                enableRowEdit: false,
                columnDefs: [
                    { field: 'Date', displayName: str.grid_Date, width: '130px', cellFilter: 'date:\'yyyy-MM-dd HH:mm\'' },
                    { field: 'Category.Name', displayName: str.grid_Category },
                    { field: 'SubCategory.Name', displayName: str.grid_Item },
                    { field: 'SpentMoney', displayName: str.grid_Spent, width: '80px', cellFilter: 'number:2' },
                    { field: 'Description', displayName: str.grid_Description },
                    { field: 'Owner.UserName', displayName: str.grid_Owner, width: '80px' },
                    {
                        displayName: str.grid_Actions,
                        cellTemplate: '<div style="text-align:center;"><button class="btn btn-warning btn-xs" ng-click="editExpense($index)" title="' + str.action_Edit + '"><span class="glyphicon glyphicon-pencil"></span></button><button class="btn btn-danger btn-xs" type="button" ng-click="deleteExpense($index)" title="' + str.action_Remove + '"><span class="glyphicon glyphicon-remove"></span></button></div>',
                        width: '80px',
                        enableCellEdit: false
                    }
                ]
            };

            $scope.update = function () {
                api.getItems($scope.current.user, $scope.current.category, $scope.current.search)
                    .success(function (data) { $scope.expensesData = data; });

                api.getStatsData().success(function (data) { $scope.stats = data; });
            };

            api.getCategories().success(function (data) { $scope.categories = data; });

            $scope.update();

            $scope.addExpense = function (size) {
                $modal.open({
                    templateUrl: 'addExpense.html',
                    controller: [
                        '$scope', '$modalInstance', function ($mscope, $modalInstance) {
                            $mscope.Title = str.ttl_AddNewExpense;
                            $mscope.exp = {};
                            $mscope.categories = $scope.categories;
                            $mscope.category = $scope.categories[0];

                            $mscope.updateSubCategories = function (c) {
                                api.getSubCategoryNames(c.Id).success(function (data) { $mscope.subCategories = data; });
                            };

                            $mscope.updateSubCategories($mscope.category);

                            $mscope.addNew = function () { return $scope.addNew($mscope); };
                            $mscope.evil = function (fn) { return $scope.evil(fn); }
                            $mscope.isInvalid = function(exp) { return $scope.isInvalid(exp); }

                            $mscope.ok = function () {
                                $mscope.exp.Category = $mscope.category;
                                $mscope.exp.SpentMoney = $mscope.evil($mscope.exp.SpentMoney).value;
                                $modalInstance.close($mscope.exp);
                            };
                            $mscope.cancel = function () { $modalInstance.dismiss('cancel'); };
                        }
                    ],
                    size: size
                }).result.then(function (data) { api.updateItem(data).then($scope.update); });

            };

            $scope.evil = function (fn) {
                if (angular.isUndefined(fn)) return;

                try {
                    return {
                        value: Math.round(new Function('return ' + fn.replaceAll(",", "."))() * 100) / 100,
                        error: false
                    };
                } catch (e) {
                    return {
                        value: fn,
                        error: true
                    };
                }
            }

            $scope.isInvalid = function (exp) {
                return (
                    angular.isUndefined(exp.SubCategory) ||
                    angular.isUndefined(exp.SubCategory.Name) ||
                    angular.isUndefined(exp.SpentMoney) ||
                    $scope.evil(exp.SpentMoney).error);
            }

            $scope.addNew = function (mscope) {
                if ($scope.isInvalid(mscope.exp)) return;
                mscope.exp.Category = mscope.category;
                mscope.exp.SpentMoney = mscope.evil(mscope.exp.SpentMoney).value;

                api.updateItem(mscope.exp).then(function () {
                    $scope.update();
                    mscope.exp = {};
                });
            };

            $scope.deleteExpense = function () {
                var exp = this.row.entity;
                $modal.open({
                    templateUrl: 'deleteExpense.html',
                    controller: [
                        '$scope', '$modalInstance', function ($mscope, $modalInstance) {
                            $mscope.Title = 'Delete expense ?';
                            $mscope.exp = exp;
                            $mscope.ok = function () { $modalInstance.close(); };
                            $mscope.cancel = function () { $modalInstance.dismiss('cancel'); };
                        }
                    ]
                }).result.then(function () { api.deleteItem(exp).then($scope.update); });
            };

            $scope.editExpense = function () {
                var exp = this.row.entity;
                $modal.open({
                    templateUrl: 'addExpense.html',
                    controller: [
                        '$scope', '$modalInstance', function ($mscope, $modalInstance) {
                            $mscope.Title = str.ttl_EditExpense;
                            $mscope.exp = exp;
                            $mscope.category = _.find($scope.categories, function (c) {
                                return c.Id == exp.Category.Id;
                            });

                            $mscope.updateSubCategories = function (c, init) {
                                api.getSubCategoryNames(c.Id).success(function (data) {
                                    $mscope.subCategories = data;
                                    if (!init) $mscope.subCategory = '';
                                });
                            };

                            $mscope.addNew = function () { return $scope.addNew($mscope); };
                            $mscope.evil = function (fn) { return $scope.evil(fn); }
                            $mscope.isInvalid = function (item) { return $scope.isInvalid(item); }

                            $mscope.updateSubCategories($mscope.category, true);
                            $mscope.categories = $scope.categories;

                            $mscope.ok = function () {
                                $mscope.exp.CategoryId = $mscope.category.Id;
                                $modalInstance.close($mscope.exp);
                            };

                            $mscope.cancel = function () { $modalInstance.dismiss('cancel'); };
                        }
                    ]
                }).result.then(function (data) { api.updateItem(data).then($scope.update); });

            };
        }
    ])
.directive('onEnter', function () {
    return function (scope, element, attrs) {
        element.bind("keydown keypress", function (event) {
            if (event.which === 13) {
                scope.$apply(function () {
                    scope.$eval(attrs.onEnter);
                });

                event.preventDefault();
            }
        });
    };
});
