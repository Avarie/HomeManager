angular.module('manager')
    .controller('SecurityController', [
        '$scope', '$filter', '$http', '$modal', 'settingsService', 'Str',
        function ($scope, $filter, $http, $modal, $db, str) {

            var api = $db.getApi("Security");

            $scope.current = {
                user: 0,
                category: 0,
                search: ""
            };

            $scope.itemsGridData = [];

            api.getCategories().success(function (data) { $scope.categories = data; });

            var linkTemplate = '<div class="ngCellText" ng-class="col.colIndex()">' +
                '<a href="{{row.getProperty(col.field)}}" target="_blank">' +
                    '{{row.getProperty(col.field)}}' +
                    '</a>' +
                '</div>';

            $scope.securityItemsGrid = {
                data: 'itemsGridData',
                multiSelect: false,
                selectedItems: $scope.selectedItems,
                enableColumnResize: true,
                enableCellSelection: false,
                enableRowSelection: true,
                enableCellEdit: false,
                enableRowEdit: false,
                columnDefs: [
                    {
                        field: 'Date',
                        displayName: str.grid_Date,
                        cellFilter: 'date:\'yyyy-MM-dd\'',
                        width: '100px'
                    },
                    { field: 'Category.Name', displayName: str.grid_Category },
                    { field: 'Name', displayName: str.grid_Title },
                    {
                        field: 'Link',
                        displayName: str.grid_Link,
                        cellTemplate: linkTemplate
                    },
                    { field: 'Description', displayName: str.grid_Description },

                    { field: 'Owner.UserName', displayName: str.grid_Owner, width: '80px' },
                    {
                        displayName: str.grid_Actions,
                        cellTemplate: '<div style="text-align:center;">' +
                            '<button class="btn btn-warning btn-xs" ng-click="addSecurityItem(true,$index)" title="' + str.action_Edit + '">' +
                            '<span class="glyphicon glyphicon-pencil"></span>' +
                            '</button>' +
                            '<button class="btn btn-danger btn-xs" type="button" ng-click="deleteSecurityItem($index)" title="' + str.action_Remove + '">' +
                            '<span class="glyphicon glyphicon-remove"></span>' +
                            '</button>' +
                            '</div>',
                        width: '80px',
                        enableCellEdit: false
                    }
                ],
            };

            $scope.update = function () {
                api.getItems($scope.current.user, $scope.current.category, $scope.current.search)
                    .success(function (data) { $scope.itemsGridData = data; });
            };

            $scope.update();

            $scope.addSecurityItem = function (editable) {
                var item = {};
                var category = {};
                var title;

                if (editable) {
                    angular.copy(this.row.entity, item);
                    category = _.find($scope.categories,
                        function (c) { return c.Id == item.Category.Id; }
                        );
                    title = str.ttl_EditItem;
                } else {
                    item = { ContactLines: [] };
                    title = str.ttl_AddItem;
                }

                $modal.open({
                    templateUrl: 'addSecurityItem.html',
                    controller: [
                        '$scope', '$modalInstance', function ($mscope, $modalInstance) {
                            $mscope.Title = title;
                            $mscope.c = item;
                            $mscope.c.Category = category;
                            $mscope.categories = $scope.categories;

                            $mscope.ok = function () { $modalInstance.close($mscope.c); };
                            $mscope.cancel = function () { $modalInstance.dismiss('cancel'); };
                        }
                    ]
                }).result.then(function (data) { api.updateItem(data).then($scope.update); });
            };

            $scope.deleteSecurityItem = function () {
                var item = this.row.entity;

                $modal.open({
                    templateUrl: 'deleteSecurityItem.html',
                    controller: [
                        '$scope', '$modalInstance', function ($mscope, $modalInstance) {
                            $mscope.Title = str.ttl_DeleteItem;
                            $mscope.c = item;
                            $mscope.ok = function () { $modalInstance.close(); };
                            $mscope.cancel = function () { $modalInstance.dismiss('cancel'); };
                        }
                    ]
                }).result.then(function() { api.deleteItem(item).then($scope.update); });
            };
        }
    ]);
