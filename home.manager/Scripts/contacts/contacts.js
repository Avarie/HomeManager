angular.module('manager')
    .controller('ContactsController', [
        '$scope', '$filter', '$http', '$modal', 'settingsService', 'Str',
        function ($scope, $filter, $http, $modal, $db, str) {

            var api = $db.getApi("Contacts");

            $scope.current = {
                user: 0,
                category: 0,
                search: ""
            };

            $scope.contactsGridData = [];
            $scope.selectedcontacts = [];

            $scope.getCurrentContact = function () {
                var contact = "";
                if ($scope.selectedcontacts.length == 0) return contact;
                var c = $scope.selectedcontacts[0];
                
                for (var l in c.ContactLines) {
                    var i = c.ContactLines[l];
                    contact += i.Name;
                    if (i.Description != null || i.Description != "")
                        contact += " (" + i.Description + ") ";
                }

                contact += ", " + c.Name ;
                if (c.Description != null || c.Description != "") {
                    contact += ", (" + c.Description + ") ";
                }

                return contact;
            }

            api.getCategories().success(function (data) {
                $scope.categories = data;
            });

            var contactsTemplate = '<table >' +
                '<tr ng-repeat="c in row.entity[col.field]">' +
                '<td class="data" title="{{c.Description}}">{{c.Name}}</td>' +
                '<td class="description">{{c.Description}}</td></tr></table>';

            $scope.contactsGrid = {
                data: 'contactsGridData',
                multiSelect: false,
                selectedItems: $scope.selectedcontacts,
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
                    { field: 'Name', displayName: str.grid_Name },
                    {
                        field: 'ContactLines',
                        displayName: str.grid_Contacts,
                        cellClass: 'contactLines',
                        cellTemplate: contactsTemplate
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
                            '<button class="btn btn-warning btn-xs" ng-click="addContact(true,$index)" title="' + str.action_Edit + '">' +
                            '<span class="glyphicon glyphicon-pencil"></span>' +
                            '</button>' +
                            '<button class="btn btn-danger btn-xs" type="button" ng-click="deleteContact($index)" title="' + str.action_Remove + '">' +
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
                    .success(function (data) { $scope.contactsGridData = data; });
            };

            $scope.update();

            $scope.addContact = function (editable) {
                var contact = {};
                var category = {};
                var title;

                if (editable) {
                    angular.copy(this.row.entity, contact);
                    category = _.find($scope.categories,
                        function (c) { return c.Id == contact.Category.Id; }
                        );
                    title = str.ttl_EditContact;
                } else {
                    contact = { ContactLines: [] };
                    title = str.ttl_AddContact;
                }

                $modal.open({
                    templateUrl: 'addContact.html',
                    controller: [
                        '$scope', '$modalInstance', function ($mscope, $modalInstance) {
                            $mscope.Title = title;
                            $mscope.c = contact;
                            $mscope.c.Category = category;
                            $mscope.categories = $scope.categories;

                            function clean() { $mscope.line = ''; $mscope.description = ''; }

                            $mscope.removeLine = function (c) {
                                _.remove($mscope.c.ContactLines, function (r) { return r == c; });
                            };

                            $mscope.addLine = function (c, d) {
                                $mscope.c.ContactLines.push( { Name: c, Description: d } );
                                clean();
                            };

                            clean();

                            $mscope.ok = function () { $modalInstance.close($mscope.c); };
                            $mscope.cancel = function () { $modalInstance.dismiss('cancel'); };
                        }
                    ]
                }).result.then(function (item) { api.updateItem(item).then($scope.update); });
            };

            $scope.deleteContact = function () {
                var exp = this.row.entity;
                $modal.open({
                    templateUrl: 'deleteContact.html',
                    controller: [
                        '$scope', '$modalInstance', function ($mscope, $modalInstance) {
                            $mscope.Title = str.ttl_DeleteContact;
                            $mscope.c = exp;
                            $mscope.ok = function () { $modalInstance.close(); };
                            $mscope.cancel = function () { $modalInstance.dismiss('cancel'); };
                        }
                    ]
                }).result.then(function() { api.deleteItem(exp).then($scope.update); });
            };
        }
    ]);
