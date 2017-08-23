'use strict';

app.factory('personService', ['$http', function ($http) {
    return {
        getPersons: function (currentPage, pageSize) {
            return $http.post("/Person/GetPersons?page=" + currentPage + "&pagesize=" + pageSize);
        },
        saveUpdatePerson: function (person) {
            return $http.post("/Person/SaveUpdatePerson", person);
        },
        deletePerson: function (personKey) {
            return $http.post("/Person/DeletePerson?personKey=" + personKey);
        }
    };
}]);