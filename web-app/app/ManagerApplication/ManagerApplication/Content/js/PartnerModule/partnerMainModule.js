$(document).ready(function () {

});

var partnerModule = angular.module("partnerModule", []);
var partnerController = function ($scope, $http) {
    $scope.groupCinemaInfor;
    $scope.groupId = 3;
    $("#empManagement").hide();
    $("#cinemaManagement").show();

    $scope.$on('roomCallRefreshEvent', function (event) {
        $('#viewSeatModal').modal('hide');
        $scope.$broadcast('reloadCinemaEvent');
    });

    $http({
        method: "POST",
        url: "/Partner/GetGroupCinemaInfor",
        params: { groupIdStr: $scope.groupId, }
    })
   .then(function (response) {
       $scope.groupCinemaInfor = response.data;
   });

    $scope.manageEmployee = function () {
        $("#empManagement").show();
        $("#cinemaManagement").hide();
        var groupId = $scope.groupId;
        $scope.$broadcast('reloadEmployeeEvent', groupId);
    };

    $scope.manageCinema = function () {
        $("#empManagement").hide();
        $("#cinemaManagement").show();
        $scope.$broadcast('reloadCinemaEvent');
    };
    
}

partnerModule.controller("partnerController", partnerController);
angular.module("CombineModule", ["partnerModule", "partnerRoomModule", "partnerEmployeeModule",
    "partnerCinemaModule", "partnerReportModule"]);

