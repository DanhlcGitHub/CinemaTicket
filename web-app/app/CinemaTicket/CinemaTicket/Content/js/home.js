/// <reference path="bootstrap.min.js" />
$(function () {
    //FilmManager.loadFilmList();
    //FilmManager.loadCinema();
    //console.log("run it");
});

var myApp = angular.module("homeModule", []);
var filmController = function ($scope, $http) {
    $scope.currentCinemaIndex = 0;
    $scope.pageIdex = 0;
    $scope.maxItem = 8;
    $http({
        method: "POST",
        url: "home/LoadCinema"
    })
    .then(function (response) {
        $scope.scheduleList = response.data;
        $scope.currentCinemaList = response.data[$scope.currentCinemaIndex].cinemas;
    });

    $http({
        method: "POST",
        url: "home/LoadAvailableFilm"
    })
    .then(function (response) {
        $scope.FilmData = response.data;
        $scope.CurrentFilmList = $scope.FilmData.slice($scope.pageIdex * $scope.maxItem, (($scope.pageIdex + 1) * $scope.maxItem))
        console.log("filmdata");
        console.log($scope.FilmData);
        console.log("CurrentFilmList");
        console.log($scope.CurrentFilmList);
    });

    $scope.onclickGroupCinema = function (index) {
        $scope.currentCinemaList = $scope.scheduleList[$scope.currentCinemaIndex].cinemas;
    };
    $scope.previous = function () {
        $scope.pageIdex--;
        if ($scope.pageIdex < 0) $scope.pageIdex = Math.ceil($scope.FilmData.length / $scope.maxItem) - 1;
        $scope.CurrentFilmList = $scope.FilmData.slice($scope.pageIdex * $scope.maxItem, (($scope.pageIdex + 1) * $scope.maxItem));
    };
    $scope.next = function () {
        $scope.pageIdex++;
        if ($scope.pageIdex >= (Math.ceil($scope.FilmData.length / $scope.maxItem))) $scope.pageIdex = 0;
        $scope.CurrentFilmList = $scope.FilmData.slice($scope.pageIdex * $scope.maxItem, (($scope.pageIdex + 1) * $scope.maxItem));
    };
}

myApp.controller("filmController", filmController);

var LocalStorageManager = {
    saveToLocalStorage: function (data,dataKey) {
        var dataString = JSON.stringify(data);
        localStorage.setItem(dataKey, dataString);
    },
    loadDataFromStorage: function (dataKey) {
        // read from local storage
        if (localStorage.getItem(dataKey)) {
            var dataString = localStorage.getItem(dataKey);
            return JSON.parse(dataString);
        }
    },
    removeDataFromStorage: function (dataKey) {
        localStorage.removeItem(dataKey);
    }
}