$(document).ready(function () {

});

var managerModule = angular.module("managerModule", []);
var managerController = function ($scope, $http) {
    $scope.cinemaInfor;
    $scope.cinemaId = $("#cinemaId").val();
    
    $scope.showDashboard = function () {
        $("#dashboardManager").show();
        $("#basicAdd").hide();
        $("#advantageAdd").hide();

        $scope.$broadcast('viewDashBoardEvent');
    }
    $scope.showDashboard();

    $scope.basicAdd = function () {
        $("#basicAdd").show();
        $("#advantageAdd").hide();
        $("#dashboardManager").hide();
        

        $scope.$broadcast('basicAddScheduleEvent');
    };

    $scope.advantageAdd = function () {
        $("#basicAdd").hide();
        $("#advantageAdd").show();
        $("#dashboardManager").hide();

        $scope.$broadcast('advantageAddScheduleEvent');
    };

    $http({
        method: "POST",
        url: "/CinemaManager/GetCinemaInfor",
        params: { cinemaIdStr: $("#cinemaId").val() }
    })
   .then(function (response) {
       $scope.cinemaInfor = response.data;
   });

    
    
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

managerModule.controller("managerController", managerController);
angular.module("CombineModule", ["managerModule", "managerScheduleModule", "dashboardModule"]);

