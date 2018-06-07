/// <reference path="bootstrap.min.js" />
$(function () {
    //FilmManager.loadFilmList();
    //FilmManager.loadCinema();
    //console.log("run it");
});

var myApp = angular.module("homeModule", []);
var filmController = function ($scope, $http) {
    $scope.currentCinemaIndex = 0;
    $http({
        method: "POST",
        url: "home/LoadCinema"
    })
    .then(function (response) {
        $scope.scheduleList = response.data;
        $scope.currentCinemaList = response.data[$scope.currentCinemaIndex].cinemas;
    })

    $http({
        method: "POST",
        url: "home/LoadAvailableFilm"
    })
    .then(function (response) {
        $scope.FilmData = response.data;
    })

    $scope.onclickGroupCinema = function (index) {
        $scope.currentCinemaList = $scope.scheduleList[$scope.currentCinemaIndex].cinemas;
    }
    $scope.getNumber = function (num) {
        return new Array(Math.ceil(num));
    }
}

myApp.controller("filmController", filmController);

var FilmManager = {
    FilmData : [],
    FilmDataKey: 'FilmDataKey',
    pageIdex: 0,
    maxItem : 8,
    loadFilmList: function () {
        $.ajax({
            url: 'home/LoadAvailableFilm',
            type: 'POST',
            contentType: 'application/json;',
            success: function (response) {
                LocalStorageManager.saveToLocalStorage(response, FilmManager.FilmDataKey);
                FilmManager.displayFilm(response);
            }
        });
    },
    loadCinema: function () {
        $.ajax({
            url: 'home/LoadCinema',
            type: 'POST',
            contentType: 'application/json;',
            success: function (response) {
                console.log("cinema list");
                console.log(response);
                //FilmManager.displayGroupCinema(response);
            }
        });
    },
    displayFilm: function (filmList) {
        $("#filmContainer").html("");
        var size = (this.pageIdex + 1) * this.maxItem;
        if (size >= filmList.length) {
            size = filmList.length;
        }
        for (var i = this.pageIdex * this.maxItem  ; i < size; i++) {
            var aFilm = filmList[i];
            var containerDiv = document.createElement("div");
            containerDiv.className = "col-sm-3";

            var img = document.createElement("img");
            img.src = aFilm.img;
            img.className = "img-movie rounded";
            img.alt = aFilm.name;
            img.style.height = '320px';

            var child1 = document.createElement("div");
            child1.className = "top-right";
            child1.style.width = "50px";
            child1.style.height = "40px";

            var child2 = document.createElement("div");
            child2.className = "text-center";
            child2.textContent = aFilm.imdb;

            var child3 = document.createElement("div");
            child3.className = "d-inline-flex";
            child3.style.float = 'left';
            
            for (var j = 0 ; j < aFilm.imdb / 2; j++) {
                var img1 = document.createElement("img");
                img1.className = "icon_star_rating";
                img1.src = "Content/img/film/star_full.png";
                child3.appendChild(img1);
            }
            var h6 = document.createElement("h6");
            h6.className = "mt-2";
            h6.textContent = aFilm.name;

            var small = document.createElement("small");
            small.className = "text-muted";
            small.textContent = aFilm.length + " phút";

            child1.appendChild(child2);
            child1.appendChild(child3);

            containerDiv.appendChild(img);
            if(aFilm.imdb!=0)
                containerDiv.appendChild(child1);
            containerDiv.appendChild(h6);
            containerDiv.appendChild(small);

            $("#filmContainer").append(containerDiv);
        }
    },
    displayCinema: function(){

    },
    previous: function () {
        if ($.isEmptyObject(this.FilmData)) {
            this.FilmData = LocalStorageManager.loadDataFromStorage(this.FilmDataKey);
        }
        this.pageIdex--;
        if (this.pageIdex < 0) this.pageIdex = Math.ceil(this.FilmData.length / this.maxItem) -1;
        this.displayFilm(this.FilmData);
    },
    next: function () {
        if ($.isEmptyObject(this.FilmData)) {
            this.FilmData = LocalStorageManager.loadDataFromStorage(this.FilmDataKey);
        }
        console.log(this.FilmData.length);
        this.pageIdex++;
        if (this.pageIdex >= (Math.ceil(this.FilmData.length / this.maxItem))) this.pageIdex = 0;
        this.displayFilm(this.FilmData);
    },
    test: function () {
        $scope.test = "dkm";
    }
}

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