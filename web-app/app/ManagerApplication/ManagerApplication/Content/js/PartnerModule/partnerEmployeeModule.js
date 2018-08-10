
var partnerEmployeeModule = angular.module("partnerEmployeeModule", []);
var partnerEmployeeController = function ($scope, $http) {
    $scope.employeeData;
    $scope.allCinemaData;
    $scope.currentEmployeeList;
    $scope.currentPageEmpData;
    $scope.empMaxItem = 5;
    $scope.empPageIndex = 0;
    $scope.empTotalPage = 0;
    $scope.currentEmployee;
    $scope.empPagingArray;
    $scope.isAddnewEmp = true;
    $scope.currentSelectedEmployeeIndex = 0;
    /* =============== event area =============== */
    $scope.$on('reloadEmployeeEvent', function (event, groupId) {
        $scope.getAllEmployee();
        $scope.getAllCinema(groupId);
    });

    /* ----------------------------------------------*/

    $("#empForm").submit(function (e) {
        e.preventDefault(e);
        if ($scope.isAddnewEmp == true) {
            $scope.addNewEmployee();
        } else {//update
            $scope.updateEmployee();
        }
    });

    $scope.getAllCinema = function (groupId) {
        $http({
            method: "POST",
            url: "/Partner/GetAllCinemaInGroup",
            params: { groupIdStr: $("#groupId").val(), }
        })
        .then(function (response) {
            $scope.allCinemaData = response.data;
        });
    };


    $scope.getAllEmployee = function () {
        $http({
            method: "POST",
            url: "/Partner/GetAllEmployee",
            params: { groupIdStr: $("#groupId").val(), }
        })
       .then(function (response) {
           $scope.empPageIndex = 0;
           $scope.isAddnewEmp = true;
           $scope.currentEmployee = {};
           $scope.employeeData = response.data;
           $scope.currentEmployeeList = response.data;
           $scope.currentPageEmpData = $scope.currentEmployeeList.slice($scope.empPageIndex * $scope.empMaxItem, (($scope.empPageIndex + 1) * $scope.empMaxItem));
           $scope.empTotalPage = Math.ceil($scope.currentEmployeeList.length / $scope.empMaxItem);
           $scope.empPagingArray = new Array($scope.empTotalPage);
           $scope.updateEmpTrackingArray();
       });
    };

    $scope.getAllEmployee();

    $scope.empPreviousClick = function () {
        $scope.empPageIndex--;
        if ($scope.empPageIndex < 0) $scope.empPageIndex = Math.ceil($scope.currentEmployeeList.length / $scope.empMaxItem) - 1;
        $scope.currentPageEmpData = $scope.currentEmployeeList.slice($scope.empPageIndex * $scope.empMaxItem, (($scope.empPageIndex + 1) * $scope.empMaxItem));
        $scope.updateEmpTrackingArray();
    };

    $scope.empNextClick = function () {
        $scope.empPageIndex++;
        if ($scope.empPageIndex >= (Math.ceil($scope.currentEmployeeList.length / $scope.empMaxItem))) $scope.empPageIndex = 0;
        $scope.currentPageEmpData = $scope.currentEmployeeList.slice($scope.empPageIndex * $scope.empMaxItem, (($scope.empPageIndex + 1) * $scope.empMaxItem));
        $scope.updateEmpTrackingArray();
    };

    $scope.empPageClick = function (index) {
        $scope.empPageIndex = index;
        $scope.currentPageEmpData = $scope.currentEmployeeList.slice($scope.empPageIndex * $scope.empMaxItem, (($scope.empPageIndex + 1) * $scope.empMaxItem));
        $scope.updateEmpTrackingArray();
    };

    $scope.employeeDetail = function (index) {
        $scope.currentSelectedEmployeeIndex = index;
        var validator = $("#empForm").validate();
        
        validator.resetForm();
        $("#inputEmpUsername").attr("disabled", "disabled");
        $scope.isAddnewEmp = false;
        $scope.currentEmployee = $scope.currentPageEmpData[index];
    };

    $scope.showAddEmpForm = function () {
        $('#empForm')[0].reset();
        var validator = $("#empForm").validate();
        
        validator.resetForm();
        $("#inputEmpUsername").removeAttr("disabled");
        $scope.isAddnewEmp = true;
        $scope.currentEmployee = {};
    };

    $scope.updateEmployee = function () {
        if ($("#empForm").valid()) {
            inputEmpUsername
            var empName = $("#inputEmpName").val();
            var empEmail = $("#inputEmpEmail").val();
            var empPhone = $("#inputEmpPhone").val();
            var empObj = {};
            empObj.username = $scope.currentEmployee.username;
            empObj.name = empName;
            empObj.email = empEmail;
            empObj.phone = empPhone;
            $http({
                method: "POST",
                url: "/Partner/UpdateEmployee",
                params: { empObj: empObj, }
            })
            .then(function (response) {
                $http({
                    method: "POST",
                    url: "/Partner/GetAllEmployee",
                    params: { groupIdStr: $("#groupId").val(), }
                })
               .then(function (response) {
                   $('#empForm')[0].reset();
                   $scope.employeeData = response.data;
                   $scope.currentEmployeeList = response.data;
                   $scope.currentPageEmpData = $scope.currentEmployeeList.slice($scope.empPageIndex * $scope.empMaxItem, (($scope.empPageIndex + 1) * $scope.empMaxItem));
                   $scope.empTotalPage = Math.ceil($scope.currentEmployeeList.length / $scope.empMaxItem);
                   $scope.empPagingArray = new Array($scope.empTotalPage);
                   $scope.updateEmpTrackingArray();
                   $scope.currentEmployee = $scope.currentPageEmpData[$scope.currentSelectedEmployeeIndex];
               });
                $("#validateModal").modal();
                $("#modalMessage").html("Update success!");
            });
        }
    };
    $scope.validateUsername = function (str) {
        return /^[a-z][a-z0-9_.-]{4,19}$/i.test(str);
    };
    $scope.addNewEmployee = function () {
        var empUsername = $("#inputEmpUsername").val();
        if (!$scope.validateUsername(empUsername)) {
            $("#inputEmpUsernameErr").css("display", "inline").fadeOut(4000);
        } else if ($("#empForm").valid()) {
            var empName = $("#inputEmpName").val();
            var empEmail = $("#inputEmpEmail").val();
            var empPhone = $("#inputEmpPhone").val();
            
            var empPassword = $("#inputEmpPassword").val();
            var cinemaId = $('#inputEmpSelectCinema').val();
            var cinemaName = $('#inputEmpSelectCinema :selected').text();

            var empObj = {};
            empObj.username = empUsername;
            empObj.password = empPassword;
            empObj.name = empName;
            empObj.email = empEmail;
            empObj.phone = empPhone;
            empObj.cinemaId = cinemaId;
            empObj.cinemaName = cinemaName;
            $http({
                method: "POST",
                url: "/Partner/IsEmpExist",
                params: { username: empUsername, }
            })
            .then(function (response) {
                console.log(response);
                console.log(response.data);
                if (response.data == "False") {
                    $http({
                        method: "POST",
                        url: "/Partner/AddNewEmployee",
                        params: { empObj: empObj, }
                    })
                    .then(function (response) {
                        $('#empForm')[0].reset();
                        $scope.getAllEmployee();
                        $("#validateModal").modal();
                        $("#modalMessage").html(response.data.message);
                    });
                } else {
                    $("#validateModal").modal();
                    $("#modalMessage").html("this username already exist!");
                    $('#inputEmpUsername').focus();
                }
            });
        }
    };

    $scope.deleteEmployee = function (index) {
        var emp = $scope.currentPageEmpData[index];
        var confirmDialog = confirm("Are you sure want to delete this employee ?")
        if (confirmDialog) {
            var username = emp.username;
            $http({
                method: "POST",
                url: "/Partner/DeleteEmployee",
                params: { username: username, }
            })
            .then(function (response) {
                $('#empForm')[0].reset();
                $scope.getAllEmployee();
                $("#validateModal").modal();
                $("#modalMessage").html("delete success!");
            });
        }
        else {
            //some code
        }
    };

    $scope.updateEmpTrackingArray = function () {
        for (var i = 0 ; i < $scope.empPagingArray.length; i++) {
            $scope.empPagingArray[i] = "page-item";
        }
        $scope.empPagingArray[$scope.empPageIndex] = "page-item active";
    };
}

partnerEmployeeModule.controller("partnerEmployeeController", partnerEmployeeController);





