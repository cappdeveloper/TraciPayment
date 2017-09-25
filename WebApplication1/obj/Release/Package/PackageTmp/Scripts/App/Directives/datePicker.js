app.directive('setDatepicker', function () {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, elem, attrs, ngModel) {
            elem.datepicker({
                autoclose: true,
                todayHighlight: true,
                showmonths: true,
                dateFormat: "DD/MM/YYYY"
            });
        }
    };
});