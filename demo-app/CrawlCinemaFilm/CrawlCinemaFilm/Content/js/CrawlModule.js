$(document).ready(function () {

});

var crawlModule = angular.module("crawlModule", []);
var crawlController = function ($scope, $http) {

    $http({
        method: "POST",
        url: "/Partner/GetGroupCinemaInfor",
        params: { groupIdStr: $("#groupId").val(), }
    })
   .then(function (response) {
       $scope.groupCinemaInfor = response.data;
   });

}

crawlModule.controller("crawlController", crawlController);


