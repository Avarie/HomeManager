// Main configuration file. Sets up AngularJS module and routes and any other config objects

var appRoot = angular.module('manager',
    ['ngRoute',
        'ngGrid',
        'autocomplete',
        'angularFileUpload',
        'ui.bootstrap',
        'textAngular',
        'ngResource']);     //Define the main module

var resources = {};
var xmlhttp = getXmlHttp();

// using sync method to get resources before starting angular app.
xmlhttp.open("GET", "Settings/GetResources", false);
xmlhttp.send();
if (xmlhttp.status == 200) {
    var data = JSON.parse(xmlhttp.response);
    for (var d in data) { resources[data[d].Key] = data[d].Value; }
}

appRoot
    .value('Str', resources)
    .config(['$routeProvider', function ($routeProvider) {

        //Setup routes to load partial templates from server. TemplateUrl is the location for the server view (Razor .cshtml view)
        $routeProvider
            .when('/home', { templateUrl: 'manager/home/main', controller: 'MainController' })
            .when('/settings', { templateUrl: 'manager/settings/settings', controller: 'SettingsController' })
            .when('/documents', { templateUrl: 'manager/documents/documents', controller: 'DocumentsController' })
            .when('/contacts', { templateUrl: 'manager/contacts/contacts', controller: 'ContactsController' })
            .when('/notes', { templateUrl: 'manager/notes/notes', controller: 'NotesController' })
            .when('/security', { templateUrl: 'manager/security/security', controller: 'SecurityController' })

            .when('/expenses', { templateUrl: 'manager/expenses/expenses', controller: 'ExpensesController' })
            .when('/charts', { templateUrl: 'manager/charts/charts', controller: 'ChartsController' })
//            .when('/login', { templateUrl: '/account/login', controller: 'ExpensesController' })
            .when('/manage', { templateUrl: 'manager/account/changePassword', controller: 'AccountController' })
            .when('/users', { templateUrl: 'manager/users/users', controller: 'UsersController' })
            .otherwise({ redirectTo: '/home' })
        ;
    }])
    .controller('ManagerController',
    [
        '$scope',
        '$route',
        '$routeParams',
        '$location',
        function ($scope, $route, $routeParams, $location) {
        $scope.$on('$routeChangeSuccess', function (e, current, previous) {
            $scope.activeViewPath = $location.path();
        });
        }]);

function getXmlHttp() {
    var xmlhttp;
    try {
        xmlhttp = new ActiveXObject("Msxml2.XMLHTTP");
    } catch (e) {
        try {
            xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
        } catch (E) {
            xmlhttp = false;
        }
    }
    if (!xmlhttp && typeof XMLHttpRequest != 'undefined') {
        xmlhttp = new XMLHttpRequest();
    }
    return xmlhttp;
}

