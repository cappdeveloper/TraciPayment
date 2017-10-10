'use strict';

app.factory('programService', ['$http', function ($http) {
    return {
        getPrograms: function (currentPage, pageSize) {
            return $http.post("/ManageProgram/GetPrograms?page=" + currentPage + "&pagesize=" + pageSize);
        },
        saveUpdateProgram: function (program) {
            return $http.post("/ManageProgram/SaveUpdateProgram", program);
        },
        getProgram: function (programKey) {
            return $http.post("/ManageProgram/GetProgram?programKey=" + programKey);
        },
        deleteProgram: function (programKey) {
            return $http.post("/ManageProgram/DeleteProgram?programKey=" + programKey);
        }
    };
}]);