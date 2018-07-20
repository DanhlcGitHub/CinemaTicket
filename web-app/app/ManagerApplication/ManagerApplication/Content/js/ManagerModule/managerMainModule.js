$(document).ready(function () {

});

var managerModule = angular.module("managerModule", []);
var managerController = function ($scope, $http) {
    $scope.cinemaInfor;
    $scope.cinemaId = $("#cinemaId").val();

    $scope.basicAdd = function () {
        $("#basicAdd").show();
        $("#advantageAdd").hide();

        $scope.$broadcast('basicAddScheduleEvent');
    };
    $scope.basicAdd();

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
angular.module("CombineModule", ["managerModule", "managerScheduleModule"]);

