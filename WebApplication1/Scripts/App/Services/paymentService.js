app.factory('paymentService', ['$http', function ($http) {
    return {
        getPaymentDllData: function () {
            return $http.post("/PaymentScreen/GetPaymentDllData");
        },
        savePayment: function (payment) {
            return $http.post("/PaymentScreen/SavePayment", payment);
        },
        savePerson: function (person) {
            return $http.post("/PaymentScreen/SavePerson", person);
        },
        savePaymentAccount: function (account) {
            return $http.post("/PaymentScreen/SavePaymentAccount", account);
        },
        savePaymentProgram: function (program) {
            return $http.post("/PaymentScreen/SavePaymentProgram", program);
        },
        deletePaymentAccount: function (paymentAccountKey) {
            return $http.post("/PaymentScreen/DeletePaymentAccount" + '?paymentAccountKey=' + paymentAccountKey);
        },
        deletePaymentProgram: function (paymentProgramKey) {
            return $http.post("/PaymentScreen/DeletePaymentProgram" + '?paymentProgramKey=' + paymentProgramKey);
    }
    };
}]);