'use strict';
var container = $(".displaynone");

app.controller('paymentController', ['$scope', '$filter', 'paymentService', function ($scope, $filter, paymentService) {

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
            $scope.GetPayments($scope.CurrentPage, $scope.PageSize);
        }
    };
    /// End paging

    /// <summary>
    /// Method will get all payments and data of all dlls 
    /// <summary>
    $scope.GetPayments = function (page, pageSize) {
        //paging
        $scope.PageSize = parseInt(pageSize);
        $scope.CurrentPage = page; //paging

        $scope.Payments = [];
        $scope.PeymentTypes = [];
        $scope.Accounts = [];
        $scope.Programs = [];
        $scope.Persons = [];
        paymentService.getPayments(page, pageSize).success(function (data) {
            $scope.Payments = data.Payments;
            $scope.Payments.PaymentToClient=data.Payments.PaymentToClient;
            $scope.PeymentTypes = data.PaymentTypes;
            $scope.Persons = data.Persons;
            $scope.Programs = data.Programs;
            $scope.Accounts = data.Accounts;
         
            //begin paging
            $scope.PaymentCount = data.PaymentCount;
            $scope.PageCount = Math.ceil(data.PaymentCount / $scope.PageSize);
            if (data.PaymentCount > 0) {
                $scope.FirstRecord = (($scope.CurrentPage - 1) * $scope.PageSize) + 1;
                $scope.LastRecord = ($scope.Payments.length == $scope.PageSize ? $scope.PageSize * $scope.CurrentPage : $scope.PaymentCount);
            }
            //end paging

            $scope.PageMode = $scope.Mode.ListMode;
        }).error(function () {
            alert("Some error occured while getting dropdown's data.")
        });
    }

    // method will display the payment in add/edit mode
    $scope.AddEditPayment = function (paymentKey) {
        $scope.Payment = {};
        $scope.PaymentAccounts = [];
        $scope.PaymentPrograms = [];
        $scope.AccountTotalAmount = 0;
        $scope.ProgramTotalAmount = 0;
        clearValidations();
        $scope.PageMode = $scope.Mode.AddEditMode;
        if (paymentKey != undefined) {
            $scope.GetPayment(paymentKey);
        }
    }

    $scope.GetPayment = function (paymentKey,viewDetail) {
        paymentService.getPayment(paymentKey).success(function (data) {
            $scope.Payment = data;
            $scope.Payment.PaymentDate = ToJavaScriptDate($scope.Payment.PaymentDate);
            $scope.PaymentAccounts = data.PaymentAccountsModel;
            $scope.PaymentPrograms = data.PaymentProgramsModel;
            GetProgramTotalAmount();
            GetAccountTotalAmount();
            if (viewDetail != undefined) {
                $("#PaymentDetailModal").modal("toggle");
            }
        }).error(function () {
            alert("Some error occured while getting Payment.")
        });
    }

    //Convert date to javascript date
    var ToJavaScriptDate = function (value) {
        if (value == null || value == undefined) {
            return "";
        }
        return moment(value).format("DD/MM/YYYY");
    }

    ///method will take you back to payment listing
    $scope.CancelPayment = function () {
        //$scope.PageMode = $scope.Mode.ListMode;
        $scope.GetPayments($scope.CurrentPage, $scope.PageSize);
    }

    /// <summary>
    /// Method will save/update parameter detail
    /// <summary>
    $scope.SavePayment = function () {

        if (!$("#PaymentForm").valid())
            return;

        clearValidations();
        paymentService.savePayment($scope.Payment).success(function (data) {
            if (data.PaymentKey != undefined) {
                if ($scope.Payment.PaymentKey == undefined) {
                    alert("Payment added successfully.");
                    $scope.Payment.PaymentKey = data.PaymentKey;
                }
                else {
                    alert("Payment updated successfully.");
                    var paymentObj = $filter('filter')($scope.Payments, { PaymentKey: $scope.Payment.PaymentKey });
                    if (paymentObj != null) {
                        paymentObj[0].PaymentDate = data.PaymentDate;
                        paymentObj[0].PaymentCheckNumber = data.PaymentCheckNumber;
                        paymentObj[0].PaymentTypeKey = data.PaymentTypeKey;
                        paymentObj[0].PaymentTo = data.PaymentTo;
                        paymentObj[0].PaymentVendorInvoiceNumber = data.PaymentVendorInvoiceNumber;
                        paymentObj[0].PaymentNote = data.PaymentNote;
                    }
                }
            }
        }).error(function () {
            alert("Some error occured while saving the payment.")
        });
    }

    ///method will delete specific payment
    $scope.DeletePayment = function (paymentId, index) {
        paymentService.deletePayment(paymentId).then(function (response) {
            if (response.data) {
                //$scope.Payments.splice(index, 1);
                $scope.GetPayments($scope.CurrentPage, $scope.PageSize);
                alert('Payment deleted successfully');
            }
        },
        function (response) {
            alert('Some error occured while deleting the payment.');
        });

    }

    // method will display the person popup allowing you to add a new person
    $scope.AddNewPerson = function () {
        $scope.Person = {};
        clearValidations();
        $("#PersonModal").modal("toggle");
    }

    /// <summary>
    /// method will save detail of person 
    /// <summary>
    $scope.SavePerson = function () {
        if (!$("#PersonForm").valid())
            return;

        clearValidations();

        paymentService.savePerson($scope.Person).success(function (data) {
            if (data.PersonKey != undefined) {
                $scope.Persons.push(data);
                $scope.Payment.PaymentTo = data.PersonKey;
                $("#PersonModal").modal("toggle");
            }
        }).error(function () {
            alert("Some error occured while adding the person.")
        });
    }

    /// method will display payment account popup, allowing you to add new account for payment
    $scope.AddNewPaymentAccount = function () {
        if ($scope.Payment.PaymentKey == undefined) {
            alert('Please save payment details first.')
        }
        else {
            $scope.PaymentAccount = {};
            clearValidations();
            $("#PaymentAmountModal").modal("toggle");
        }
    }

    /// <summary>
    /// method will save detail of person 
    /// <summary>
    $scope.SavePaymentAccount = function () {

        if (!$("#PaymentAmountForm").valid())
            return;

        clearValidations();
        $scope.PaymentAccount.PaymentKey = $scope.Payment.PaymentKey;
        paymentService.savePaymentAccount($scope.PaymentAccount).success(function (data) {
            if (data.PaymentAccountKey != undefined) {
                var accountObj = $filter('filter')($scope.Accounts, { AccountKey: $scope.PaymentAccount.AccountKey });
                if (accountObj != null) {
                    data.AccountName = accountObj[0].AccountName;
                }
                $scope.PaymentAccounts.push(data);
                GetAccountTotalAmount();
                $("#PaymentAmountModal").modal("toggle");
            }
        }).error(function () {
            alert("Some error occured while saving the payment account.")
        });
    }

    ///method will delete he specific payment account
    $scope.DeletePaymentAccount = function (paymentAccountId, index) {
        paymentService.deletePaymentAccount(paymentAccountId).then(function (response) {
            if (response.data)
                $scope.PaymentAccounts.splice(index, 1);
            GetAccountTotalAmount();
        },
        function (response) {
            alert('Some error occured while deleting the account.')
        });

    }

    /// method will display payment program popup, allowing you to add new account for payment
    $scope.AddNewPaymentProgram = function () {
        if ($scope.Payment.PaymentKey == undefined) {
            alert('Please save payment details first.')
        }
        else {
            $scope.PaymentProgram = {};
            clearValidations();
            $("#PaymentProgramModal").modal("toggle");
        }
    }

    /// <summary>
    /// method will save payment program
    /// <summary>
    $scope.SavePaymentProgram = function () {
        if (!$("#PaymentAmountForm").valid())
            return;

        clearValidations();
        $scope.PaymentProgram.PaymentKey = $scope.Payment.PaymentKey;
        paymentService.savePaymentProgram($scope.PaymentProgram).success(function (data) {
            if (data.PaymentProgramKey != undefined) {
                var progObj = $filter('filter')($scope.Programs, { ProgramName: $scope.PaymentProgram.AccountKey });
                if (progObj != null) {
                    data.ProgramName = progObj[0].ProgramName;
                }
                $scope.PaymentPrograms.push(data);
                GetProgramTotalAmount();
                $("#PaymentProgramModal").modal("toggle");
            }
        }).error(function () {
            alert("Some error occured while saving the payment program.")
        });
    }

    ///method will delete he specific payment program
    $scope.DeletePaymentProgram = function (paymentProgramId, index) {
        paymentService.deletePaymentProgram(paymentProgramId).then(function (response) {
            if (response.data)
                $scope.PaymentPrograms.splice(index, 1);
            GetProgramTotalAmount();
        },
        function (response) {
            alert('Some error occured while deleting the program.')
        });

    }

    var GetProgramTotalAmount = function () {

        $scope.ProgramTotalAmount = 0;
        angular.forEach($scope.PaymentPrograms, function (value, key) {
            $scope.ProgramTotalAmount = $scope.ProgramTotalAmount + value.PaymentProgramAmount;
        });
    }

    var GetAccountTotalAmount = function () {

        $scope.AccountTotalAmount = 0;
        angular.forEach($scope.PaymentAccounts, function (value, key) {
            $scope.AccountTotalAmount = $scope.AccountTotalAmount + value.PaymentAccountAmount;
        });
    }

    var initial = function () {
        $scope.PeymentTypes = [];
        $scope.Persons = [];
        $scope.Accounts = [];
        $scope.Programs = [];
        $scope.Payment = {};
        $scope.Person = {};
        $scope.PaymentAccount = {};
        $scope.PaymentPrograms = [];
        $scope.PaymentAccounts = [];
        $scope.PageMode = $scope.Mode.ListMode;
        $scope.Payments = [];
        $scope.AccountTotalAmount = 0;
        $scope.ProgramTotalAmount = 0;
        ////Paging
        $scope.PaymentCount = 0;
        $scope.PageSize = 10;
        $scope.CurrentPage = 1;
        $scope.PageCount = 0;
        //paging

        $scope.GetPayments($scope.CurrentPage, $scope.PageSize);
    }
    initial();

    var clearValidations = function () {
        $('.input-validation-error').removeClass('input-validation-error');
        $('.field-validation-error').removeClass('field-validation-error').addClass('field-validation-valid');
    };

}]);
