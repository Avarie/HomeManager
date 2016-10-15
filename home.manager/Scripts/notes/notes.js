angular.module('manager')
    .controller('NotesController', [
        '$scope', '$filter', '$http', '$modal', 'settingsService', 'Str',
        function ($scope, $filter, $http, $modal, $db, str) {

            var api = $db.getApi("Notes");

            $scope.current = {
                user: 0,
                category: 0,
                search: ""
            };

            $scope.notesGridData = [];
            $scope.selectednotes = [];

            api.getCategories().success(function (data) {
                $scope.categories = data;
            });


            var linkTemplate = '<div class="ngCellText" ng-class="col.colIndex()">' +
                '<a href="public/note/?id={{row.getProperty(\'PublicId\')}}">' +
                '{{row.getProperty(col.field)}}' +
                '</a></div>';

            $scope.notesGrid = {
                data: 'notesGridData',
                multiSelect: false,
                selectedItems: $scope.selectednotes,
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
                    {
                        field: 'Name',
                        displayName: str.grid_Name,
                        cellTemplate: linkTemplate
                    },
                    {
                        field: 'Description',
                        displayName: str.grid_Description,
                        enableCellEdit: true
                    },
                    { field: 'Owner.UserName', displayName: str.grid_Owner, width: '80px' },
                    {
                        displayName: str.grid_Actions,
                        cellTemplate: '<div style="text-align:center;">' +
                            '<button class="btn btn-warning btn-xs" ng-click="addNote(true,$index)" title="' + str.action_Edit + '">' +
                            '<span class="glyphicon glyphicon-pencil"></span>' +
                            '</button>' +
                            '<button class="btn btn-danger btn-xs" type="button" ng-click="deleteNotes($index)" title="' + str.action_Remove + '">' +
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
                    .success(function (data) { $scope.notesGridData = data; });
            };

            $scope.update();

            $scope.addNote = function (editable) {
                var note = {};
                var category = {};
                var title;

                if (editable) {
                    angular.copy(this.row.entity, note);
                    category = note.Category == null ? null :
                        _.find($scope.categories,
                        function (c) { return c.Id == note.Category.Id; }
                        );
                    title = str.ttl_EditNotes;
                } else {
                    title = str.ttl_AddNotes;
                }

                $modal.open({
                    templateUrl: 'addNote.html',
                    controller: [
                        '$scope', '$modalInstance', function ($mscope, $modalInstance) {
                            $mscope.Title = title;
                            $mscope.c = note;
                            $mscope.originalContent = note.Content;
                            $mscope.c.Category = category;
                            $mscope.categories = $scope.categories;

                            $mscope.IsNoteChanged = function (content) {
                                return (typeof $mscope.c == "undefined") ? false : $mscope.originalContent != content;
                            }

                            $mscope.save = function () {
                                api.updateItem($mscope.c).success(function (data) {
                                    $mscope.originalContent = $mscope.c.Content;
                                });
                            };
                            $mscope.ok = function () { $modalInstance.close($mscope.c); };
                            $mscope.cancel = function () { $modalInstance.dismiss('cancel'); };
                        }
                    ],
                    size: 'lg' // lg-900, sm - 300, default - 600
                }).result.then(function(item) { api.updateItem(item).then($scope.update); });
            };

            $scope.deleteNotes = function () {
                var exp = this.row.entity;
                $modal.open({
                    templateUrl: 'deleteNote.html',
                    controller: [
                        '$scope', '$modalInstance', function ($mscope, $modalInstance) {
                            $mscope.Title = str.ttl_DeleteNotes;
                            $mscope.c = exp;
                            $mscope.ok = function () { $modalInstance.close(); };
                            $mscope.cancel = function () { $modalInstance.dismiss('cancel'); };
                        }
                    ]
                }).result.then(function() { api.deleteItem(exp).then($scope.update); });
            };

        }
    ]);
