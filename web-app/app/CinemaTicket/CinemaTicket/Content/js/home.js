/// <reference path="bootstrap.min.js" />
$(function () {
    //FilmManager.loadFilmList();
    //FilmManager.loadCinema();
    //console.log("run it");
});

var myApp = angular.module("homeModule", []);
var filmController = function ($scope, $http) {
    $scope.currentGroupCinemaIndex = 0;// which group is focus
    $scope.currentCinemaIndex = 0; // which cinema is focus
    $scope.pageIdex = 0;
    $scope.maxItem = 8;
    $http({
        method: "POST",
        url: "Schedule/LoadScheduleGroupByCinema"
    })
    .then(function (response) {
        $scope.scheduleList = response.data;
        $scope.currentCinemaList = response.data[$scope.currentGroupCinemaIndex].cinemas;
        $scope.currentFilmInScheduleList = $scope.currentCinemaList[$scope.currentCinemaIndex].films;
        console.log("currentCinemaList");
        console.log($scope.currentCinemaList);
    });

    $http({
        method: "POST",
        url: "Film/LoadAvailableFilm"
    })
    .then(function (response) {
        $scope.AllFilmData = response.data;
        $scope.FilmData = $.grep($scope.AllFilmData, function (v) {
            return v.filmStatus === 1;
        });
        //$scope.test1();
        $scope.CurrentFilmList = $scope.FilmData.slice($scope.pageIdex * $scope.maxItem, (($scope.pageIdex + 1) * $scope.maxItem))
    });

    $scope.onclickGroupCinema = function (index) {
        $scope.currentGroupCinemaIndex = index;
        $scope.currentCinemaIndex = 0;
        $scope.currentCinemaList = $scope.scheduleList[$scope.currentGroupCinemaIndex].cinemas;
        $scope.currentFilmInScheduleList = $scope.currentCinemaList[$scope.currentCinemaIndex].films;
        console.log("group click");
        console.log($scope.currentCinemaList);
        console.log("------------------------------------");
        console.log($scope.currentFilmInScheduleList);
        console.log("currentGroupCinemaIndex: " + $scope.currentGroupCinemaIndex);
        console.log("currentCinemaIndex: " + $scope.currentCinemaIndex);
    };
    $scope.onclickCinema = function (index) {
        $scope.currentCinemaIndex = index;
        $scope.currentFilmInScheduleList = $scope.currentCinemaList[$scope.currentCinemaIndex].films;
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
    $scope.loadUpcommingMovie = function (event) {
        $("#showingMovieId").attr('class', 'nav-link text-muted');
        $("#upcommingMovieId").attr('class', 'nav-link text-danger');
        $scope.pageIdex = 0;
        $scope.FilmData = $.grep($scope.AllFilmData, function (v) {
            return v.filmStatus === 2;
        });
        $scope.CurrentFilmList = $scope.FilmData.slice($scope.pageIdex * $scope.maxItem, (($scope.pageIdex + 1) * $scope.maxItem));
        event.preventdefault();
        return false;
    },
    $scope.loadShowingMovie = function () {
        $("#showingMovieId").attr('class', 'nav-link text-danger');
        $("#upcommingMovieId").attr('class', 'nav-link text-muted');
        $scope.pageIdex = 0;
        $scope.FilmData = $.grep($scope.AllFilmData, function (v) {
            return v.filmStatus === 1;
        });
        $scope.CurrentFilmList = $scope.FilmData.slice($scope.pageIdex * $scope.maxItem, (($scope.pageIdex + 1) * $scope.maxItem));
        event.preventdefault();
        return false;
    },
    $scope.gotoFilmDetail = function (id) {
        window.location.href = 'home/FilmDetail?filmId='+id;
    },
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
    saveToLocalStorage: function (data, dataKey) {
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