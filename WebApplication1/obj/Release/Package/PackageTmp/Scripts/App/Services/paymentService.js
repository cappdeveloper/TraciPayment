'use strict';

app.factory('paymentService', ['$http', function ($http) {
    return {
        getPayments: function (currentPage, pageSize) {
            return $http.post("/PaymentScreen/GetPayments?page=" + currentPage + "&pagesize=" + pageSize);
        },
        savePayment: function (payment) {
            return $http.post("/PaymentScreen/SavePayment", payment);
        },
        deletePayment: function (paymentKey) {
            return $http.post("/PaymentScreen/Deletepayment?paymentKey=" + paymentKey);
        },
        getPayment: function (paymentKey) {
            return $http.post("/PaymentScreen/GetPayment?paymentKay=" + paymentKey);
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
            return $http.post("/PaymentScreen/DeletePaymentAccount?paymentAccountKey=" + paymentAccountKey);
        },
        deletePaymentProgram: function (paymentProgramKey) {
            return $http.post("/PaymentScreen/DeletePaymentProgram?paymentProgramKey=" + paymentProgramKey);
    }
    };
}]);