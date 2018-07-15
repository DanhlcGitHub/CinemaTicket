var myApp = angular.module("homeModule", []);
var homeController = function ($scope, $http) {
    $scope.FilmNowShowing;
  
  
    $http({
        method: "GET",
        url: "/Home/GetFilmNowShowing"
    }).then(function (response) {
        $scope.FilmNowShowing = response.data;
       
    });

    $scope.clickDetail = function (index) {
        $scope.FilmNowSelected = $scope.FilmNowShowing[index];
        document.getElementById("filmId").value = $scope.FilmNowSelected.filmId;
        document.getElementById("filmName").value = $scope.FilmNowSelected.name;
        $('#datepicker').datepicker().datepicker('setDate', $scope.FilmNowSelected.dateRelease);

        document.getElementById("restricted").value = $scope.FilmNowSelected.restricted;
        document.getElementById("filmLength").value = $scope.FilmNowSelected.filmLength;
        document.getElementById("digTypeId").value = $scope.FilmNowSelected.digTypeId;
        document.getElementById("author").value = $scope.FilmNowSelected.author;
        document.getElementById("movieGenre").value = $scope.FilmNowSelected.movieGenre;
        document.getElementById("actorList").value = $scope.FilmNowSelected.actorList;
        document.getElementById("countries").value = $scope.FilmNowSelected.countries;
        document.getElementById("trailerLink").value = $scope.FilmNowSelected.trailerLink;
        document.getElementById("posterPicture").value = $scope.FilmNowSelected.posterPicture;
        document.getElementById("additionPicture").value = $scope.FilmNowSelected.additionPicture;
        if ($scope.FilmNowSelected.filmStatus == 1) {
            document.getElementById("filmStatus").value = "Now Showing";

        } else {
            document.getElementById("filmStatus").value = "Coming Soon";
        }
        document.getElementById("filmContent").value = $scope.FilmNowSelected.filmContent;

    };

    $scope.createOrUpdateFilm = function () {
        var filmId = document.getElementById("filmId").value;
        var filmName = document.getElementById("filmName").value;
        var dateRelease = document.getElementById("datepicker").value;
        var restricted = document.getElementById("restricted").value;
        var filmLength = document.getElementById("filmLength").value;
        var digTypeId = document.getElementById("digTypeId").value;
        var author = document.getElementById("author").value;
        var movieGenre = document.getElementById("movieGenre").value;
        var actorList = document.getElementById("actorList").value;
        var countries = document.getElementById("countries").value;
        var trailerLink = document.getElementById("trailerLink").value;
        var posterPicture = document.getElementById("posterPicture").value;
        var additionPicture = document.getElementById("additionPicture").value;
        var filmStatus = document.getElementById("filmStatus").value;
        var filmContent = document.getElementById("filmContent").value;

        $http({
            method: "POST",
            url: "/Home/CreateFilm",
            params: {
                filmId: filmId, filmName: filmName, dateRelease: dateRelease, restricted: restricted, filmLength: filmLength,
                digTypeId: digTypeId, author: author, movieGenre: movieGenre, actorList: actorList,
                countries: countries, trailerLink: trailerLink, posterPicture: posterPicture, additionPicture: additionPicture,
                filmStatus: filmStatus, filmContent: filmContent
            }
        }).then(function (response) {
            $scope.FilmNowShowing = response.data;

            $scope.clearForm();

            $('#modal-film').modal('hide');

            alert("Successful!");
        });
    };

    $scope.clearForm = function () {
        document.getElementById("filmName").value = "";
        document.getElementById("datepicker").value = "";
        document.getElementById("restricted").value = "";
        document.getElementById("filmLength").value = "";
        document.getElementById("digTypeId").value = "";
        document.getElementById("author").value = "";
        document.getElementById("movieGenre").value = "";
        document.getElementById("actorList").value = "";
        document.getElementById("countries").value = "";
        document.getElementById("trailerLink").value = "";
        document.getElementById("posterPicture").value = "";
        document.getElementById("additionPicture").value = "";
        document.getElementById("filmStatus").value = "";
        document.getElementById("filmContent").value = "";

    }

}

myApp.controller("homeController", homeController);
