$(document).ready(function () {

});

var partnerModule = angular.module("partnerModule", []);
var partnerController = function ($scope, $http) {
    $scope.groupCinemaInfor;
    $scope.groupId = $("#groupId").val();
    $("#empManagement").hide();
    $("#cinemaManagement").show();

    $scope.$on('roomCallRefreshEvent', function (event) {
        $('#viewSeatModal').modal('hide');
        $scope.$broadcast('reloadCinemaEvent');
    });

    $http({
        method: "POST",
        url: "/Partner/GetGroupCinemaInfor",
        params: { groupIdStr: $("#groupId").val(), }
    })
   .then(function (response) {
       $scope.groupCinemaInfor = response.data;
   });

    $scope.manageEmployee = function () {
        $("#empManagement").show();
        $("#cinemaManagement").hide(); 
        $("#dashboardManagement").hide();
        var groupId = $scope.groupId;
        $scope.$broadcast('reloadEmployeeEvent', groupId);
    };

    $scope.manageCinema = function () {
        $("#empManagement").hide();
        $("#dashboardManagement").hide();
        $("#cinemaManagement").show();
        $scope.$broadcast('reloadCinemaEvent');
    };

    $scope.manageDashboard = function () {
        $("#empManagement").hide();
        $("#cinemaManagement").hide();
        $("#dashboardManagement").show();
        $scope.$broadcast('manageDashboardEvent');
    };

    $scope.manageDashboard();
    
    $scope.logout = function () {
        $http({
            method: "POST",
            url: "/Utility/logout",
        })
       .then(function (response) {
           if (response.data.status == "ok") {
               document.getElementById("gohomeForm").submit();
           }
       });
    };
}

partnerModule.controller("partnerController", partnerController);
angular.module("CombineModule", ["partnerModule", "partnerRoomModule", "partnerEmployeeModule",
    "partnerCinemaModule", "partnerReportModule", "partnerDashboardModule"]);

