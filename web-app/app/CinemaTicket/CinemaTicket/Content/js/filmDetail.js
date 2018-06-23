/// <reference path="bootstrap.min.js" />
$(function () {
});

var myApp = angular.module("detailFilmModule", []);
var filmController = function ($scope, $http) {
    $scope.groupIndex = 0;
    $scope.dateIndex = 0;
    $scope.groupCinemaList;
    $scope.dateList;
    $scope.currentData; // use to show cinema and time and digtyp information

    angular.element(document).ready(function () {
        $http({
            method: "POST",
            url: "/Film/LoadFilmById",
            params: { filmId: $("#filmId").val() }
                })
           .then(function (response) {
               $scope.filmData = response.data;
               console.log($scope.filmData);
        });

        $http({
            method: "POST",
            url: "/Cinema/LoadGroupCinema",
        })
        .then(function (response) {
            $scope.groupCinemaList = response.data;
        });

        $http({
            method: "POST",
            url: "/Schedule/GetSeventDateFromNow",
        })
        .then(function (response) {
            $scope.dateList = response.data;
        });

        $http({
            method: "POST",
            url: "/Schedule/LoadScheduleGroupByCinemaForFilmDetail",
            params: { filmId: $("#filmId").val() }
        })
        .then(function (response) {
            $scope.data = response.data;
            $scope.currentData = $scope.data[$scope.groupIndex].dates[$scope.dateIndex].cinemas;
               console.log($scope.data);
        });
    });
    $scope.groupClickHandler = function (index) {
        $scope.groupIndex = index;
        $scope.currentData = $scope.data[$scope.groupIndex].dates[$scope.dateIndex].cinemas;
        console.log("current data");
        console.log($scope.currentData);
    };
    $scope.dateClickHandler = function (index) {
        $scope.dateIndex = index;
        $scope.currentData = $scope.data[$scope.groupIndex].dates[$scope.dateIndex].cinemas;
        console.log("current data");
        console.log($scope.data[$scope.groupIndex].dates[$scope.dateIndex].cinemas);
    };
    $scope.openTrailerDialog = function (url) {
        console.log(url);
        var modal = document.getElementById('myModal');
        var myIframe = document.getElementById('myIframe');
        modal.style.display = "block";
        var span = document.getElementsByClassName("close")[0];
        $('#myIframe').prop('src', url);

        // When the user clicks on <span> (x), close the modal
        span.onclick = function () {
            modal.style.display = "none";
        }

        // When the user clicks anywhere outside of the modal, close it
        window.onclick = function (event) {
            if (event.target == modal) {
                modal.style.display = "none";
                $('#myIframe').prop('src', "");
            }
        }
    };
    $scope.gotoChooseTicket = function (filmId, timeId, cinemaId) {
        var selectDate = $scope.dateList[$scope.dateIndex].fullDate;
        console.log("filmId " + filmId);
        console.log("timeId " + timeId);
        console.log("cinemaId " + cinemaId);
        console.log("selectDate " + selectDate);

        var param1 = "<input type='hidden' name='filmId' value='" + filmId + "' />";
        var param2 = "<input type='hidden' name='timeId' value='" + timeId + "' />";
        var param3 = "<input type='hidden' name='cinemaId' value='" + cinemaId + "' />";
        var param4 = "<input type='hidden' name='selectDate' value='" + selectDate + "' />";
        document.getElementById('goToChooseTicketForm').innerHTML = param1 + param2 + param3 + param4;
        document.getElementById('goToChooseTicketForm').submit();
    }
}

myApp.controller("filmController", filmController);

/* console.log(window.location.hostname);
               console.log(window.location.href);
               console.log(window.location.pathname);
               console.log(window.location.href.replace(window.location.pathname, ''));*/