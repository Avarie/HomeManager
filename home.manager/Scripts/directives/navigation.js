angular.module('manager')
    .directive("navigation", ['userService', '$upload', function ($dbu, $upload) {
        return {
            templateUrl: "manager/directives/navigation",
            restrict: "E",
            replace: true,
            scope: {
                addNew: "&",
                categoryEnabled: "=",
                categories: "=",
                update: "&",
                current: "=",
                mode: "@"
            },
            link: function (scope) {
                $dbu.getUsers().success(function (data) { scope.users = data; });

                scope.files = [];

                scope.$watch('files', function (n, o) {
                    if (typeof n == "undefined") return;
                    for (var i = 0; i < scope.files.length; i++) {
                        var file = scope.files[i];
                        scope.upload = $upload.upload({
                            url: 'Documents/Upload',
                            file: file
                        })
                            .progress(function (evt) { })
                            .success(function (data, status, headers, config) {
                                scope.update();
                            });
                    }
                });
            }
        };
    }]);