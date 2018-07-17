$(document).ready(function () {

});

var managerModule = angular.module("managerModule", []);
var managerController = function ($scope, $http) {
    $scope.cinemaInfor;
    $scope.cinemaId = $("#cinemaId").val();
   /* $("#empManagement").hide();
    $("#cinemaManagement").show();*/

   /* $scope.$on('roomCallRefreshEvent', function (event) {
        $('#viewSeatModal').modal('hide');
        $scope.$broadcast('reloadCinemaEvent');
    });*/

    $http({
        method: "POST",
        url: "/CinemaManager/GetCinemaInfor",
        params: { cinemaIdStr: $("#cinemaId").val() }
    })
   .then(function (response) {
       $scope.cinemaInfor = response.data;
   });

    $scope.scheduleCustomAdd = function () {
        /*$("#empManagement").show();
        $("#cinemaManagement").hide();*/
        var groupId = $scope.groupId;
        $scope.$broadcast('customScheduleEvent', groupId);
    };
    
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

