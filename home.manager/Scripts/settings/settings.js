angular.module('manager')
    .controller('SettingsController', [
        '$scope', '$http', '$modal', 'settingsService', 'Str', function ($scope, $http, $modal, $db, Str) {

            $scope.tabs = {};
            $scope.data = getInitialItems();
            $scope.selected = getInitialItems();;

            var api = $db.getSettingsApi();

            $scope.categoryGrid = getGrid('Expenses', {
                displayName: Str.grid_Actions,
                cellTemplate: '<div style="text-align:center;">' +
                    '<button class="btn btn-danger btn-xs" ng-if="this.row.entity.Amount==0" ' +
                    'ng-click="removeCategory(\'Expenses\')" title="'+Str.action_Remove+'">' +
                    '<span class="glyphicon glyphicon-remove"></span></button></div>',
                width: '10%',
                enableCellEdit: false
            });

            $scope.subCategoryGrid = getGrid('ExpensesItems', {
                displayName: Str.grid_Actions,
                cellTemplate: '<div style="text-align:center;"><button class="btn btn-warning btn-xs" ng-if="this.row.entity.Amount!=0" ng-click="mergeSubCategory($index)" title="' + Str.setting_local_MergeItems + '"><span class="glyphicon glyphicon-export"></span></button><button class="btn btn-danger btn-xs" ng-if="this.row.entity.Amount==0" ng-click="removeExpenseItem($index)" title="Remove"><span class="glyphicon glyphicon-remove"></span></button></div>',
                width: '10%'
            });

            $scope.contactCategoryGrid = getGrid('Contacts');
            $scope.securityCategoryGrid = getGrid('Security');
            $scope.noteCategoryGrid = getGrid('Notes');

            $scope.update = function (type) {
                api[type].getCategories(true).then(function (r) { $scope.data[type] = r.data; });
            };

            $scope.$watchCollection("selected.Expenses", function (n) {
                if (typeof n == "undefined" || n.length == 0) return;
                api.ExpensesItems.getCategories({ category: n[0].Id }).then(function (r) {
                    $scope.data['ExpensesItems'] = r.data;
                });
            });

            $scope.removeExpenseItem = function () {
                var id = $scope.selected['Expenses'][0].Id;
                api.ExpensesItems.deleteCategory(this.row.entity).then(function () {
                    api.ExpensesItems.getCategories({ category: id }).then(function (r) {
                        $scope.data['ExpensesItems'] = r.data;
                    });;
                });
            };

            $scope.$on('ngGridEventEndCellEdit', function (evt) {

                var item = evt.targetScope.row.entity;
                if (!(item.Id)) return;

                switch (evt.targetScope.gridId) {
                    case evt.targetScope.categoryGrid.gridId:
                        api.Expenses.updateCategory(item).then($scope.update('Expenses'));
                        break;

                    case evt.targetScope.subCategoryGrid.gridId:
                        var id = $scope.selected['Expenses'][0].Id;
                        api.ExpensesItems.updateCategory(item, id).then(function () {
                            api.ExpensesItems.getCategories({ category: id }).then(function (r) {
                                $scope.data['ExpensesItems'] = r.data;
                            });;
                        });
                        break;

                    case evt.targetScope.contactCategoryGrid.gridId:
                        api.Contacts.updateCategory(item).then($scope.update('Contacts'));
                        break;

                    case evt.targetScope.securityCategoryGrid.gridId:
                        api.Security.updateCategory(item).then($scope.update('Security'));
                        break;

                    case evt.targetScope.noteCategoryGrid.gridId:
                        api.Notes.updateCategory(item).then($scope.update('Notes'));
                        break;

                    default: break;
                }
            });

            $scope.saveTitle = function(title) {
                api.Title.saveTitle(title).then( function () { $scope.originalTitle = title; });
            }

            api.Title.getTitle().success(function (data) {
                $scope.originalTitle = data;
                $scope.title = data;
            });

            $scope.IsTitleChanged = function (title) { return (typeof $scope.title == "undefined") ? false : $scope.originalTitle != title; }

            $scope.mergeSubCategory = function () {
                var exp = this.row.entity;
                $modal.open({
                    templateUrl: 'mergeModal.html',
                    controller: [
                        '$scope', '$modalInstance', function ($mscope, $modalInstance) {
                            $mscope.title = Str.setting_local_MergeItems;
                            $mscope.name = exp.Name;
                            $mscope.desc = exp.Description;
                            $mscope.category = '';
                            $mscope.subCategory = '';

                            api.Expenses.getCategories().then(function (response) {
                                $mscope.categories = response.data;
                            });

                            $mscope.updateSubCategories = function (c) {
                                return api.ExpensesItems.getCategories({ category: c.Id })
                                    .success(function (sc) { $mscope.subCategories = sc; });
                            };

                            $mscope.ok = function () {
                                console.log($scope);
                                $modalInstance.close({
                                    oldCategoryId: $scope.selected.Expenses[0].Id,  
                                    oldSubCategoryId: exp.Id,
                                    newCategoryId: $mscope.category.Id,
                                    newSubCategoryId: $mscope.subCategory.Id
                                });
                            };
                            $mscope.cancel = function () { $modalInstance.dismiss('cancel'); };
                        }
                    ]
                }).result
                    .then(function (item) {
                        api.ExpensesItems.mergeCategory(item).then(function () {
                            api.ExpensesItems.getCategories({ category: $scope.selected.Expenses[0].Id }).then(function (response) {
                                $scope.data['ExpensesItems'] = response.data;
                            });
                        });
                    });
            };

            $scope.addSubCategory = function () {
                var id = $scope.selected['Expenses'][0].Id;
                getNewCategoryModal().then(function (item) {
                    api.ExpensesItems.updateCategory(item, id)
                        .then(function () {
                            api.ExpensesItems.getCategories({ category: id })
                                .then(function (r) { $scope.data['ExpensesItems'] = r.data; });;
                        });
                });
            };

            $scope.addCategory = function (type) {
                getNewCategoryModal().then(function (item) {
                    api[type].updateCategory(item).then(function () { $scope.update(type); });
                });
            };

            function getNewCategoryModal() {
                return $modal.open({
                    templateUrl: 'addCategoryModal.html',
                    controller: [
                        '$scope', '$modalInstance', function ($mscope, $modalInstance) {
                            $mscope.title = Str.nav_addnew;
                            $mscope.name = '';
                            $mscope.description = '';
                            $mscope.ok = function () {
                                $modalInstance.close({ Name: $mscope.name, Description: $mscope.description });
                            };
                            $mscope.cancel = function () { $modalInstance.dismiss('cancel'); };
                        }
                    ],
                    //size: size
                }).result;
            }

            $scope.removeCategory = function (type) {
                api[type].deleteCategory(this.row.entity)
                    .then(function () {
                        $scope.selected[type] = [];
                        $scope.update(type);
                    });
            };

            function getGrid(type, actions) {

                var columnDefs = [
//                    { field: 'Id', displayName: 'Id', width: '10%', enableCellEdit: false },
                    { field: 'Name', displayName: Str.grid_Name, width: '50%' },
                    { field: 'Description', displayName: Str.grid_Description }
                ];

                if (actions) {
                    columnDefs.push({
                        field: 'Amount',
                        displayName: Str.grid_Amount,
                        width: '20%',
                        cellTemplate: '<div style="text-align:center;"  class="ngCellText">{{row.getProperty(col.field)}}</div>',
                        enableCellEdit: false
                    });
                    columnDefs.push(actions);
                } else {
                    columnDefs.push({
                        displayName: Str.grid_Actions,
                        cellTemplate: '<div style="text-align:center;">' +
                            '<button class="btn btn-danger btn-xs" ng-if="this.row.entity.IsEmpty" ng-click="removeCategory(\'' + type + '\')" title="' + Str.action_Remove + '"><span class="glyphicon glyphicon-remove"></span></button></div>',
                        width: '20%',
                        enableCellEdit: false
                    });
                }

                return {
                    data: 'data[\'' + type + '\']',
                    multiSelect: false,
                    selectedItems: $scope.selected[type],
                    enableColumnResize: true,
                    enableCellSelection: true,
                    enableRowSelection: true,
                    enableCellEdit: true,
                    columnDefs: columnDefs
                }
            };

            function getInitialItems() {
                return {
                    Contacts: [],
                    Expenses: [],
                    ExpensesItems: [],
                    Notes: [],
                    Security: []
                };
            }
        }
    ])
