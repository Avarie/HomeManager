angular.module('manager')
    .service('settingsService', [
        '$http', function ($http) {

            var doHttp = function (http, controller, link, data) {
                return $http({ method: http, url: '/' + controller + link, data: data })
                .success(function (responce) { return responce; })
                .error(function (responce, status) { console.log("Error (" + status + ") in the request to", controller + link); });
            }

            this.getApi = function (controller) {

                if (typeof controller == "undefined") {
                    console.log("Error: Controller is not defined.");
                    return null;
                };

                return {
                    getItems: function (id, category, search) { return doHttp('POST', controller, '/GetItems', { id: id, category: category, search: search }); },
                    getCategories: function () { return doHttp('GET', controller, '/GetCategories'); },
                    deleteItem: function (item) { return doHttp('POST', controller, '/Delete', item); },
                    updateItem: function (item) { return doHttp('POST', controller, '/Update', item); },
                }
            };

            this.getExpenseApi = function (controller) {

                var api = this.getApi(controller);

                api.getStatsData = function () { return doHttp('GET', controller, '/GetStatsData'); }
                api.getSubCategoryNames = function (id) { return doHttp('POST', controller, '/GetSubCategoryNames', { categoryId: id }); }

                return api;
            };

            this.getCategoryApi = function (controller) {

                if (typeof controller == "undefined") {
                    console.log("Error: Controller is not defined.");
                    return null;
                };

                return {
                    getCategories: function (manage) { return doHttp('POST', controller, '/GetCategories', { withManageData: manage || false }); },
                    updateCategory: function (item) { return doHttp('POST', controller, '/UpdateCategory', item); },
                    deleteCategory: function (item) { return doHttp('POST', controller, '/DeleteCategory', item); }
                }
            };

            this.getSettingsApi = function () {

                var api = {
                    Expenses: this.getCategoryApi("Expenses"),
                    Contacts: this.getCategoryApi("Contacts"),
                    Security: this.getCategoryApi("Security"),
                    Notes: this.getCategoryApi("Notes"),
                    Title: {
                        getTitle: function () { return doHttp('GET', 'Settings', '/GetTitle'); },
                        saveTitle: function (title) { return doHttp('POST', 'Settings', '/SaveTitle', { title: title }); }
                    },
                    ExpensesItems: {
                        getCategories: function(item) { return doHttp('POST', 'Settings', '/GetExpenseSubCategories', item); },
                        updateCategory: function (item, id) { return doHttp('POST', 'Settings', '/AddOrUpdateExpenseSubCategory', { subCategory: item, id: id }); },
                        deleteCategory: function (item) { return doHttp('POST', 'Settings', '/DeleteExpenseSubCategory', item); },
                        mergeCategory: function(item) { return doHttp('POST', 'Settings', '/MergeSubCategory', item); }
                    }
                };

                api.Expenses.getCategories = function (item) { return doHttp('POST', 'Settings', '/GetExpenseCategories', item); };

                return api;
            };

            this.getChartsApi = function () {
                return { getTableData: function (m) { return doHttp('POST', 'Chart', '/GetData', { months: m }); } };
            };
        }
    ]);

