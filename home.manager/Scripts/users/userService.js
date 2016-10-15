angular.module('manager')
    .service('userService', [
        '$http', function ($http) {

            var doHttp = function (http, controller, link, data, headers) {
                return (typeof headers == "undefined") ?
                $http({ method: http, url: '/' + controller + link, data: data }) :
                $http({ method: http, url: '/' + controller + link, data: data, headers: headers })
                .success(function (responce) { return responce; })
                .error(function (responce, status) { console.log("Error (" + status + ") in the request to", controller + link); });
            }

            return {
                getUsers: function () { return doHttp('Account', 'Account', '/GetUsers'); },
                addUser: function (item) { return doHttp('POST', 'Account', '/AddUser', item); },
                updateUser: function (item) { return doHttp('POST', 'Account', '/UpdateUser', item); },
                deleteUser: function (item) { return doHttp('POST', 'Account', '/DeleteUser', item); },
                manageUser: function (item, token) {
                    return doHttp('POST', 'Account', '/Manage', item, { 'RequestVerificationToken': token });
                }
            }
        }
    ]);

