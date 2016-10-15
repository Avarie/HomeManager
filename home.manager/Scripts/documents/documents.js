angular.module('manager')
    .controller('DocumentsController', [
        '$scope', '$filter', '$http', '$modal', 'settingsService', 'Str', '$upload',
        function ($scope, $filter, $http, $modal, $db, str, $upload) {

            var api = $db.getApi("Documents");

            $scope.documentsDataGrid = [];
            $scope.selectedDocuments = [];
            $scope.searchLine = '';

            $scope.current = {
                user: 0,
                search: ""
            };

            var linkTemplate = '<div class="ngCellText" ng-class="col.colIndex()">' +
                '<a href="Public/Document/?name={{row.getProperty(col.field)}}">' +
                '{{row.getProperty(col.field)}}' +
                '</a></div>';

            $scope.documentsGrid = {
                data: 'documentsDataGrid',
                multiSelect: false,
                selectedItems: $scope.selectedDocuments,
                enableColumnResize: true,
                enableCellSelection: false,
                enableRowSelection: true,
                enableCellEdit: false,
                enableRowEdit: false,
                columnDefs: [
                    {
                        field: 'CreatedTime',
                        displayName: str.grid_Added,
                        cellFilter: 'date:\'yyyy-MM-dd\'',
                        width: '100px'
                    },
                    {
                        field: 'FileName',
                        displayName: str.grid_Name,
                        cellTemplate: linkTemplate
                    },
                    { field: 'ContentLength', displayName: str.grid_Length, width: '80px' },
                    { field: 'ContentType', displayName: str.grid_Type, width: '80px' },
                    {
                        field: 'Description',
                        displayName: str.grid_Description,
                        enableCellEdit: true
                    },
                    { field: 'Owner.UserName', displayName: str.grid_Owner, width: '80px' },
                    {
                        displayName: str.grid_Actions,
                        cellTemplate: '<div style="text-align:center;">' +
                            '<button class="btn btn-danger btn-xs" type="button" ng-click="delete($index)" title="' + str.action_Remove + '">' +
                            '<span class="glyphicon glyphicon-remove"></span><' +
                            '/button></div>',
                        width: '80px',
                        enableCellEdit: false
                    }
                ]
            }; 

            $scope.$on('ngGridEventEndCellEdit', function (evt) { api.updateItem(evt.targetScope.row.entity).then($scope.update); });

            $scope.delete = function() {
                var item = this.row.entity;
                $modal.open({
                        templateUrl: 'deleteDocument.html',
                        controller: [
                            '$scope', '$modalInstance', function($mscope, $modalInstance) {
                                $mscope.d = item;
                                $mscope.ok = function() { $modalInstance.close({ Id: item.Id, }); };
                                $mscope.cancel = function() { $modalInstance.dismiss('cancel'); };
                            }
                        ]
                    }).result
                    .then(function () { api.deleteItem(item).then($scope.update); });
            };

            $scope.update = function () {
                api.getItems($scope.current.user, 0, $scope.current.search).success(function(data) { $scope.documentsDataGrid = data; });
            };

            $scope.update();

        }
    ]);
