'use strict';
var container = $(".displaynone");

app.controller('programController', ['$scope', '$filter', 'programService', function ($scope, $filter, programService) {

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
            $scope.GetPrograms($scope.CurrentPage, $scope.PageSize);
        }
    };
    /// End paging

    // method will get all Programs
    $scope.GetPrograms = function (page, pageSize) {
        //paging
        $scope.PageSize = parseInt(pageSize);
        $scope.CurrentPage = page; //paging

        $scope.Programs = [];
        programService.getPrograms(page, pageSize).success(function (data) {
            $scope.Programs = data.Programs;

            //begin paging
            $scope.ProgramCount = data.ProgramCount;
            $scope.PageCount = Math.ceil(data.ProgramCount / $scope.PageSize);
            if (data.ProgramCount > 0) {
                $scope.FirstRecord = (($scope.CurrentPage - 1) * $scope.PageSize) + 1;
                $scope.LastRecord = ($scope.Programs.length == $scope.PageSize ? $scope.PageSize * $scope.CurrentPage : $scope.ProgramCount);
            }
            //end paging

            $scope.PageMode = $scope.Mode.ListMode;
        }).error(function () {
            alert("Some error occured while getting Programs.")
        });
    }

    // method will display Program in add/edit mode
    $scope.AddEditProgram = function (programKey) {
        $scope.Program = {};
        clearValidations();
        $scope.PageMode = $scope.Mode.AddEditMode;
        if (programKey != undefined) {
            $scope.GetProgram(programKey);
        }
    }

    // method will get specific program with programKey
    $scope.GetProgram = function (programKey) {
        programService.getProgram(programKey).success(function (data) {
            $scope.Program = data;
        }).error(function () {
            alert("Some error occured while getting Program.")
        });
    }

    /// method will take you back to listing
    $scope.CancelProgram = function () {
        if ($scope.Program.ProgramKey == undefined)
            $scope.PageMode = $scope.Mode.ListMode;
        else
            $scope.GetPrograms($scope.CurrentPage, $scope.PageSize);
    }

    /// method will add/update Program
    $scope.SaveUpdateProgram = function () {

        if (!$("#ProgramForm").valid())
            return;

        clearValidations();
        programService.saveUpdateProgram($scope.Program).success(function (data) {
            if (data.ProgramKey != undefined) {
                if ($scope.Program.ProgramKey == undefined) {
                    $scope.Program = data;
                    $scope.Programs.push($scope.Program);
                    alert("Program added successfully.");
                }
                else {
                    //var programObj = $filter('filter')($scope.Programs, { ProgramKey: $scope.Program.ProgramKey });
                    //if (programObj != null) {
                    //    programObj[0].ProgramName = data.ProgramName;
                    //    programObj[0].ProgramNote = data.ProgramNote;

                    //    $scope.PageMode = $scope.Mode.ListMode;
                    //}
                    $scope.GetPrograms($scope.CurrentPage, $scope.PageSize);
                    alert("Program updated successfully.");
                }
            }

        }).error(function () {
            alert("Some error occured while saving Program.")
        });
    }

    ///method will delete specific Program with ProgramKey
    $scope.DeleteProgram = function (programKey, index) {
        programService.deleteProgram(programKey).then(function (response) {
            if (response.data.Success) {
                $scope.GetPrograms($scope.CurrentPage, $scope.PageSize);
            }
            alert(response.data.Message);
        },
        function (response) {
            alert('Some error occured while deleting the Program.');
        });
    }

    var initial = function () {
        $scope.Programs = [];
        $scope.Program = {};
        $scope.PageMode = $scope.Mode.ListMode;

        //Paging
        $scope.ProgramCount = 0;
        $scope.PageSize = 10;
        $scope.CurrentPage = 1;
        $scope.PageCount = 0;
        //paging

        $scope.GetPrograms($scope.CurrentPage, $scope.PageSize);
    }

    initial();

    var clearValidations = function () {
        $('.input-validation-error').removeClass('input-validation-error');
        $('.field-validation-error').removeClass('field-validation-error').addClass('field-validation-valid');
    };

}]);
