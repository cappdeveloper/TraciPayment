'use strict';
var container = $(".displaynone");

app.controller('accountController', ['$scope', '$filter', 'accountService', function ($scope, $filter, accountService) {

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
            $scope.GetAccounts($scope.CurrentPage, $scope.PageSize);
        }
    };
    /// End paging

    // method will get data of drop downs
    $scope.GetAccountViewData = function () {
        $scope.AccounTypes = [];
        accountService.getAccountViewData().success(function (data) {
            $scope.AccounTypes = data.AccountTypes;

            container.show();
        }).error(function () {
            alert("Some error occured while getting data.")
        });
    }

    // method will get all Accounts
    $scope.GetAccounts = function (page, pageSize) {
        //paging
        $scope.PageSize = parseInt(pageSize);
        $scope.CurrentPage = page; //paging

        $scope.Accounts = [];
        accountService.getAccounts(page, pageSize).success(function (data) {
            $scope.Accounts = data.Accounts;

            //begin paging
            $scope.AccountCount = data.AccountCount;
            $scope.PageCount = Math.ceil(data.AccountCount / $scope.PageSize);
            if (data.AccountCount > 0) {
                $scope.FirstRecord = (($scope.CurrentPage - 1) * $scope.PageSize) + 1;
                $scope.LastRecord = ($scope.Accounts.length == $scope.PageSize ? $scope.PageSize * $scope.CurrentPage : $scope.AccountCount);
            }
            //end paging

            $scope.PageMode = $scope.Mode.ListMode;
        }).error(function () {
            alert("Some error occured while getting Accounts.")
        });
    }

    // method will display Account in add/edit mode
    $scope.AddEditAccount = function (accountKey) {
        $scope.Account = {};
        clearValidations();
        $scope.PageMode = $scope.Mode.AddEditMode;
        if (accountKey != undefined) {
            $scope.GetAccount(accountKey);
        }
    }

    // method will get specific Account with AccountKey
    $scope.GetAccount = function (accountKey) {
        accountService.getAccount(accountKey).success(function (data) {
            $scope.Account = data;
        }).error(function () {
            alert("Some error occured while getting Account.")
        });
    }

    /// method will take you back to listing
    $scope.CancelAccount = function () {
        if ($scope.Account.AccountKey == undefined)
            $scope.PageMode = $scope.Mode.ListMode;
        else
            $scope.GetAccounts($scope.CurrentPage, $scope.PageSize);
    }

    /// method will add/update Account
    $scope.SaveUpdateAccount = function () {

        if (!$("#AccountForm").valid())
            return;

        clearValidations();
        accountService.saveUpdateAccount($scope.Account).success(function (data) {
            if (data.AccountKey != undefined) {
                if ($scope.Account.AccountKey == undefined) {
                    $scope.Account = data;
                    $scope.Accounts.push($scope.Account);
                    alert("Account added successfully.");
                }
                else {
                    //var budgetObj = $filter('filter')($scope.Accounts, { AccountKey: $scope.Account.AccountKey });
                    //if (budgetObj != null) {
                    //    budgetObj[0].AccountName = data.AccountName;
                    //    budgetObj[0].GLAccountCode = data.GLAccountCode;
                    //    budgetObj[0].AccountTypeKey = data.AccountTypeKey;
                    //    budgetObj[0].AccountParentKey = data.AccountParentKey;
                    //    budgetObj[0].AccountNote = data.AccountNote;

                    //    var acctTypeObj = $filter('filter')($scope.AccounTypes, { AccountTypeKey: $scope.Account.AccountTypeKey });

                    //    budgetObj[0].AccountTypeName = acctTypeObj[0].AccountTypeName;

                    //    $scope.PageMode = $scope.Mode.ListMode;
                    //}
                    $scope.GetAccounts($scope.CurrentPage, $scope.PageSize);
                    alert("Account updated successfully.");
                }
            }
        }).error(function () {
            alert("Some error occured while saving Account.")
        });
    }

    ///method will delete specific Account with accountKey
    $scope.DeleteAccount = function (accountKey, index) {
        accountService.deleteAccount(accountKey).then(function (response) {
            if (response.data.Success) {
                $scope.GetAccounts($scope.CurrentPage, $scope.PageSize);
            }
            alert(response.data.Message);
        },
        function (response) {
            alert('Some error occured while deleting the Account.');
        });
    }

    var initial = function () {
        $scope.Accounts = [];
        $scope.AccounTypes = [];
        $scope.Account = {};
        $scope.PageMode = $scope.Mode.ListMode;

        //Paging
        $scope.AccountCount = 0;
        $scope.PageSize = 10;
        $scope.CurrentPage = 1;
        $scope.PageCount = 0;
        //paging

        $scope.GetAccountViewData();

        $scope.GetAccounts($scope.CurrentPage, $scope.PageSize);
    }

    initial();

    var clearValidations = function () {
        $('.input-validation-error').removeClass('input-validation-error');
        $('.field-validation-error').removeClass('field-validation-error').addClass('field-validation-valid');
    };

}]);
