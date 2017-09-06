'use strict';

app.factory('accountService', ['$http', function ($http) {
    return {
        getAccountViewData: function () {
            return $http.post("/ManageAccount/GetAccountViewData");
        },
        getAccounts: function (currentPage, pageSize) {
            return $http.post("/ManageAccount/GetAccounts?page=" + currentPage + "&pagesize=" + pageSize);
        },
        saveUpdateAccount: function (account) {
            return $http.post("/ManageAccount/SaveUpdateAccount", account);
        },
        getAccount: function (accountKey) {
            return $http.post("/ManageAccount/GetAccount?accountKey=" + accountKey);
        },
        deleteAccount: function (accountKey) {
            return $http.post("/ManageAccount/DeleteAccount?accountKey=" + accountKey);
        }
    };
}]);