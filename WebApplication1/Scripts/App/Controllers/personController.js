'use strict';
var container = $(".displaynone");

app.controller('personController', ['$scope', '$filter', 'personService', function ($scope, $filter, personService) {

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
            $scope.GetPersons($scope.CurrentPage, $scope.PageSize);
        }
    };
    /// End paging

    /// <summary>
    /// Method will get all Persons
    /// <summary>
    $scope.GetPersons = function (page, pageSize) {
        //paging
        $scope.PageSize = parseInt(pageSize);
        $scope.CurrentPage = page; //paging

        $scope.Persons = [];
        personService.getPersons(page, pageSize).success(function (data) {
            $scope.Persons = data.Persons;

            //begin paging
            $scope.PersonCount = data.PersonCount;
            $scope.PageCount = Math.ceil(data.PersonCount / $scope.PageSize);
            if (data.PersonCount > 0) {
                $scope.FirstRecord = (($scope.CurrentPage - 1) * $scope.PageSize) + 1;
                $scope.LastRecord = ($scope.Persons.length == $scope.PageSize ? $scope.PageSize * $scope.CurrentPage : $scope.PersonCount);
            }
            //end paging

            container.show();
            $scope.PageMode = $scope.Mode.ListMode;
        }).error(function () {
            alert("Some error occured while getting Persons.")
        });
    }

    // method will display person in edit mode 
    $scope.EditPerson = function (person) {
        $scope.Person = person;
        $scope.PageMode = $scope.Mode.AddEditMode;
    }

    //Convert date to javascript date
    var ToJavaScriptDate = function (value) {
        if (value == null || value == undefined) {
            return "";
        }
        return moment(value).format("DD/MM/YYYY");
    }

    ///method will take you back to listing
    $scope.CancelPerson = function () {
        $scope.PageMode = $scope.Mode.ListMode;
    }

    /// method will update person
    $scope.SaveUpdatePerson = function () {

        if (!$("#PersonForm").valid())
            return;

        clearValidations();
        personService.saveUpdatePerson($scope.Person).success(function (data) {
            var personObj = $filter('filter')($scope.Persons, { PersonKey: $scope.Person.PersonKey });
            if (personObj != null) {
                personObj[0].PersonName = data.PersonName;
                personObj[0].PersonAddress = data.PersonAddress;
                personObj[0].PersonCity = data.PersonCity;
                personObj[0].PersonStateKey = data.PersonStateKey;
                personObj[0].PersonZipCode = data.PersonZipCode;
                personObj[0].PersonNote = data.PersonNote;
                personObj[0].VendorFederalEIN = data.VendorFederalEIN;
                personObj[0].VendorDefaultTerms = data.VendorDefaultTerms;
            }
            alert("Person updated successfully.");
            $scope.PageMode = $scope.Mode.ListMode;
        }).error(function () {
            alert("Some error occured while saving the payment.")
        });
    }

    ///method will delete specific person with id
    $scope.DeletePerson = function (personKey, index) {
        personService.deletePerson(personKey).then(function (response) {
            if (response.data.Success) {
                $scope.GetPersons($scope.CurrentPage, $scope.PageSize);
            }
            alert(response.data.Message);
        },
        function (response) {
            alert('Some error occured while deleting the Person.');
        });
    }

    var initial = function () {
        $scope.Persons = [];
        $scope.Person = {};
        $scope.PageMode = $scope.Mode.ListMode;

        //Paging
        $scope.PersonCount = 0;
        $scope.PageSize = 10;
        $scope.CurrentPage = 1;
        $scope.PageCount = 0;
        //paging

        $scope.GetPersons($scope.CurrentPage, $scope.PageSize);
    }

    initial();

    var clearValidations = function () {
        $('.input-validation-error').removeClass('input-validation-error');
        $('.field-validation-error').removeClass('field-validation-error').addClass('field-validation-valid');
    };

}]);
