angular.module('manager')
    .controller('UsersController', [
        '$scope', '$http', '$modal', 'userService', function($scope, $http, $modal, $db) {

            $scope.usersData = [];
            $scope.selectedUsers = [];

            $scope.$watchCollection('selectedUsers', function() {
                $scope.selected = angular.copy($scope.selectedUsers[0]);
            });

            var selCatTempl = '<select ng-cell-input ng-class="\'colt\' + $index" ng-model="COL_FIELD" data-placeholder="- Select Category -"><option ng-repeat="st in categories">{{st.Name}}</option></select>';

            $scope.usersGrid = {
                data: 'usersData',
                multiSelect: false,
                selectedItems: $scope.selectedUsers,
                enableColumnResize: true,
                enableCellSelection: true,
                enableRowSelection: true,
                enableCellEdit: false,
                enableRowEdit: false,
                columnDefs: [
//                    { field: 'UserId', displayName: 'Id', width: '5%', enableCellEdit: false },
//                    { field: 'UserName', displayName: 'Name', width: '25%', enableCellEdit: false },
                    { field: 'UserId', displayName: 'Id', width: '5%', enableCellEdit: false },
                    { field: 'UserName', displayName: 'Name', width: '25%', enableCellEdit: false },
                    { field: 'Comment', displayName: 'Comment', enableCellEdit: false },
                    {
                        displayName: 'Actions',
                        cellTemplate: '<div style="text-align:center;"><button class="btn btn-warning btn-xs" ng-click="editUser($index)" title="Edit"><span class="glyphicon glyphicon-pencil"></span></button><button class="btn btn-danger btn-xs" type="button" ng-click="deleteUser($index)" title="Remove"><span class="glyphicon glyphicon-remove"></span></button></div>',
                        width: '10  %',
                        enableCellEdit: false
                    }
                ]
            };

            $scope.update = function() {
                $db.getUsers().success(function(data) { $scope.usersData = data; });
            };

            $scope.update();

            $scope.addUser = function(size) {
                $modal.open({
                        templateUrl: 'addUser.html',
                        controller: [
                            '$scope', '$modalInstance', function($mscope, $modalInstance) {
                                $mscope.Title = 'Add new user';
                                $mscope.name = '';
                                $mscope.password = '';
                                $mscope.comment = '';

                                $mscope.ok = function() {
                                    $modalInstance.close({
                                        UserName: $mscope.name,
                                        Password: $mscope.password,
                                        ConfirmPassword: $mscope.confirm,
                                        Comment: $mscope.comment
                                    });
                                };
                                $mscope.cancel = function() { $modalInstance.dismiss('cancel'); };
                            }
                        ],
                        size: size
                    }).result
                    .then(function(item) {
                        $db.addUser(item).then(function(response) {
                            $scope.update();
                        });
                    }, function() { /*console.log("Cancel"); */ });

            };

            $scope.editUser = function() {
                var exp = this.row.entity;
                $modal.open({
                        templateUrl: 'addUser.html',
                        controller: [
                            '$scope', '$modalInstance', function($mscope, $modalInstance) {
                                $mscope.Title = 'Edit user';

                                $mscope.name = exp.UserName;
                                $mscope.password = '';
                                $mscope.comment = exp.Comment;

                                $mscope.ok = function() {
                                    $modalInstance.close({
                                        UserName: $mscope.name,
                                        NewPassword: $mscope.password,
                                        ConfirmPassword: $mscope.confirm,
                                        Comment: $mscope.comment
                                    });
                                };
                                $mscope.cancel = function() { $modalInstance.dismiss('cancel'); };
                            }
                        ]
                    }).result
                    .then(function(item) {
                        $db.updateUser(item).then(function(response) {
                            $scope.update();
                        });
                    }, function() { /*console.log("Cancel"); */ });

            };


            $scope.deleteUser = function() {
                var exp = this.row.entity;

                $modal.open({
                        templateUrl: 'deleteUser.html',
                        controller: [
                            '$scope', '$modalInstance', function($mscope, $modalInstance) {
                                $mscope.name = exp.UserName;

                                $mscope.ok = function() {
                                    $modalInstance.close({ id: exp.UserId });
                                };
                                $mscope.cancel = function() { $modalInstance.dismiss('cancel'); };
                            }
                        ]
                    }).result
                    .then(function(item) {
                        $db.deleteUser(item).then(function (response) {
                            $scope.update();
                        });
                    }, function() { /*console.log("Cancel"); */ });


            };

        }
    ]);