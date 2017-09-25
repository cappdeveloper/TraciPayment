//Angular App
var app = angular.module('app', []);
app.config(['$httpProvider', function ($httpProvider) {
   
    $httpProvider.interceptors.push(["$q", "$location",  function ($q, $location) {

        return {

           
        };
    }]);
}]);

