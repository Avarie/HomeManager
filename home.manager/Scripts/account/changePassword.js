angular.module('manager')
    .controller('AccountController', [
        '$scope', 'userService', function ($scope, userService) {
            $scope.c = {};
            $scope.message = "";

            $scope.update = function () {
                userService.manageUser($scope.c, $scope.antiForgeryToken).success(function (data) {
                    if (typeof data.Errors != "undefined" && data.Errors.length > 0) {
                        $scope.message = '';
                        for (var error in data.Errors) {
                            $scope.message += data.Errors[error].ErrorMessage + '\n';
                        }
                    } else {
                        $scope.message = data.Message;
                    }
                    $scope.c = {};
                }).error(function () {
                    $scope.c = {};
                    $scope.message = 'Unexpected Error';
                });
            };
        }
    ]);
