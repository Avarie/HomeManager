angular.module('manager')
    .controller('MainController', ['$scope', 'settingsService', function ($scope, $db) {

        $db.getSettingsApi().Title.getTitle().success(function (data) {
            $scope.title = data;
        });

    }]);