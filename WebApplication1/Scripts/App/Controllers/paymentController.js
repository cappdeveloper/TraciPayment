'use strict';
var container = $(".displaynone");

app.controller('paymentController', ['$scope', '$filter', 'paymentService', function ($scope, $filter, paymentService) {

    /// <summary>
    /// Method will get data of all dlls 
    /// <summary>
    $scope.GetPaymentDllData = function () {
        $scope.PeymentTypes = [];
        $scope.Accounts = [];
        $scope.Programs = [];
        $scope.Persons = [];
        paymentService.getPaymentDllData().success(function (data) {
            $scope.PeymentTypes = data.PaymentTypes;
            $scope.Persons = data.Persons;
            $scope.Programs = data.Programs;
            $scope.Accounts = data.Accounts;
        }).error(function () {
            alert("Some error occured while getting dropdown's data.")
        });
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
                alert("Payment added successfully.")
                $scope.Payment.PaymentKey = data.PaymentKey;
            }
        }).error(function () {
            alert("Some error occured while saving the payment.")
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
                $scope.PaymentAccounts.push(data);
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
        $scope.PaymentProgram.PaymentKey =  $scope.Payment.PaymentKey;
        paymentService.savePaymentProgram($scope.PaymentProgram).success(function (data) {
            if (data.PaymentProgramKey != undefined) {
                $scope.PaymentPrograms.push(data);
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
        },
        function (response) {
            alert('Some error occured while deleting the program.')
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
        $scope.GetPaymentDllData();
    }
    initial();

    var clearValidations = function () {
        $('.input-validation-error').removeClass('input-validation-error');
        $('.field-validation-error').removeClass('field-validation-error').addClass('field-validation-valid');
    };

}]);
