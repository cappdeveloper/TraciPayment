'use strict';
var container = $(".displaynone");

app.controller('budgetController', ['$scope', '$filter', 'budgetService', function ($scope, $filter, budgetService) {

    $scope.Mode = { ListMode: 1, AddEditMode: 2 };

    /// Begin Pagin
    $scope.FirstRecord = 0; $scope.LastRecord = 10; $scope.GoToPage = '';
    $scope.NextPage = function () {
        if ($scope.CurrentPage < $scope.PageCount) {
            $scope.SetPage($scope.CurrentPage + 1);
        }
    };

    $scope.PrevPage = function () {
        if ($scope.CurrentPage > 1) {
            $scope.SetPage($scope.CurrentPage - 1);
        }
    };

    $scope.SetPage = function (n) {
        n = parseInt(n);
        if ($scope.CurrentPage != n && n > 0) {
            $scope.CurrentPage = n;
            $scope.GetBudgets($scope.CurrentPage, $scope.PageSize);
        }
    };
    /// End paging

    // method will get data of drop downs
    $scope.GetBudgetViewData = function () {
        $scope.Accounts = [];
        $scope.Terms = [];
        $scope.Programs = [];
        budgetService.getBudgetViewData().success(function (data) {
            $scope.Accounts = data.Accounts;
            $scope.Terms = data.Terms;
            $scope.Programs = data.Programs;

            container.show();
        }).error(function () {
            alert("Some error occured while getting data.")
        });
    }

    // method will get all Budgets
    $scope.GetBudgets = function (page, pageSize) {
        //paging
        $scope.PageSize = parseInt(pageSize);
        $scope.CurrentPage = page; //paging

        $scope.Budgets = [];
        budgetService.getBudgets(page, pageSize).success(function (data) {
            $scope.Budgets = data.Budgets;

            //begin paging
            $scope.BudgetCount = data.BudgetCount;
            $scope.PageCount = Math.ceil(data.BudgetCount / $scope.PageSize);
            if (data.BudgetCount > 0) {
                $scope.FirstRecord = (($scope.CurrentPage - 1) * $scope.PageSize) + 1;
                $scope.LastRecord = ($scope.Budgets.length == $scope.PageSize ? $scope.PageSize * $scope.CurrentPage : $scope.BudgetCount);
            }
            //end paging

            $scope.PageMode = $scope.Mode.ListMode;
        }).error(function () {
            alert("Some error occured while getting Budgets.")
        });
    }

    // method will display Budget in add/edit mode
    $scope.AddEditBudget = function (budgetKey) {
        $scope.Budget = {};
        clearValidations();
        $scope.PageMode = $scope.Mode.AddEditMode;
        if (budgetKey != undefined) {
            $scope.GetBudget(budgetKey);
        }
    }

    // method will get specific budget with budgetkey
    $scope.GetBudget = function (budgetKey) {
        budgetService.getBudget(budgetKey).success(function (data) {
            $scope.Budget = data;
        }).error(function () {
            alert("Some error occured while getting Budget.")
        });
    }

    /// method will take you back to listing
    $scope.CancelBudget = function () {
        if ($scope.Budget.BudgetKey == undefined)
            $scope.PageMode = $scope.Mode.ListMode;
        else
            $scope.GetBudgets($scope.CurrentPage, $scope.PageSize);
    }

    /// method will add/update Budget
    $scope.SaveUpdateBudget = function () {

        if (!$("#BudgetForm").valid())
            return;

        clearValidations();
        budgetService.saveUpdateBudget($scope.Budget).success(function (data) {
            if (data.BudgetKey != undefined) {
                //if ($scope.Budget.BudgetKey == undefined) {
                //    $scope.Budget = data;
                //    $scope.Budgets.push($scope.Budget);
                //    alert("Budget added successfully.");
                //}
                //else {
                $scope.GetBudgets($scope.CurrentPage, $scope.PageSize);
                //    alert("Budget updated successfully.");
                //}
            }

        }).error(function () {
            bootbox.alert("Some error occured while saving Budget.")
        });
    }

    ///method will delete specific Budget with budgetKey
    $scope.DeleteBudget = function (budgetKey, index) {

        bootbox.dialog({
            message: "Are you sure you want to delete this Budget ?",
            title: "Delete Budget",
            className: "bootbox-alert",
            buttons: {
                yes: {
                    label: "Yes",
                    className: "btn-danger",
                    callback: function () {
                        budgetService.deleteBudget(budgetKey).then(function (response) {
                            if (response.data.Success) {
                                $scope.GetBudgets($scope.CurrentPage, $scope.PageSize);
                            }
                            bootbox.alert(response.data.Message);
                        },
         function (response) {
             bootbox.alert('Some error occured while deleting the Budget.');
         });
                    }
                },
                no: {
                    label: "No",
                    className: "btn-default",
                    callback: function () {
                        bootbox.hideAll();
                        return false;
                    }
                },
            }
        });


    }

    var initial = function () {
        $scope.Budgets = [];
        $scope.Accounts = [];
        $scope.Terms = [];
        $scope.Programs = [];
        $scope.Budget = {};
        $scope.PageMode = $scope.Mode.ListMode;

        //Paging
        $scope.BudgetCount = 0;
        $scope.PageSize = 10;
        $scope.CurrentPage = 1;
        $scope.PageCount = 0;
        //paging

        $scope.GetBudgetViewData();

        $scope.GetBudgets($scope.CurrentPage, $scope.PageSize);
    }

    initial();

    var clearValidations = function () {
        $('.input-validation-error').removeClass('input-validation-error');
        $('.field-validation-error').removeClass('field-validation-error').addClass('field-validation-valid');
    };

}]);
