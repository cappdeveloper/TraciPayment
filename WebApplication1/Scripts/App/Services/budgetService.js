'use strict';

app.factory('budgetService', ['$http', function ($http) {
    return {
        getBudgetViewData: function () {
            return $http.post("/Budget/GetBudgetViewData");
        },
        getBudgets: function (currentPage, pageSize) {
            return $http.post("/Budget/GetBudgets?page=" + currentPage + "&pagesize=" + pageSize);
        },
        saveUpdateBudget: function (budget) {
            return $http.post("/Budget/SaveUpdateBudget", budget);
        },
        getBudget: function (budgetKey) {
            return $http.post("/Budget/GetBudget?budgetKey=" + budgetKey);
        },
        deleteBudget: function (budgetKey) {
            return $http.post("/Budget/DeleteBudget?budgetKey=" + budgetKey);
        }
    };
}]);