$(document).ready(function () {

});

var managerScheduleModule = angular.module("managerScheduleModule", []);
var scheduleController = function ($scope, $http) {
   

    $scope.test = function () {
        $http({
            method: "POST",
            url: "/CinemaManager/GetScheduleByDateFilm",
            params: { cinemaIdStr:"1", selectedDateStr: "2018-07-12", filmIdStr : "1" }
            //cinemaIdStr,string selectedDateStr, string filmIdStr
        })
       .then(function (response) {
           console.log(response);
       });
    };
}

managerScheduleModule.controller("scheduleController", scheduleController);

