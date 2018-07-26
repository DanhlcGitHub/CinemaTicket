$(document).ready(function () {
    validationManager.filmValidation();
});

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
        
        $('#datepicker').datepicker().datepicker('setDate', $scope.FilmNowSelected.dateRelease);
        $("#restricted").val($scope.FilmNowSelected.restricted);
        $("#filmId").val($scope.FilmNowSelected.filmId);
        $("#filmName").val($scope.FilmNowSelected.name);
        $("#filmLength").val($scope.FilmNowSelected.filmLength);
        $("#author").val($scope.FilmNowSelected.author);
        $("#movieGenre").val($scope.FilmNowSelected.movieGenre);
        $("#actorList").val($scope.FilmNowSelected.actorList);
        $("#countries").val($scope.FilmNowSelected.countries);
        $("#trailerLink").val($scope.FilmNowSelected.trailerLink);
        $("#divPosterPicture").hide();
        $("#divAdditionPicture").hide();
        $("#filmStatus").val($scope.FilmNowSelected.filmStatus);
        $("#filmContent").val($scope.FilmNowSelected.filmContent);
    };

    $scope.createOrUpdateFilm = function () {

        $("#form-film").submit(function (e) {
            e.preventDefault(e);
            var valid = $("#form-film").valid();
            if (valid == true) {
                var filmId = $("#filmId").val();
                var filmName = $("#filmName").val();
                var dateRelease = $("#datepicker").val();
                var restricted = $("input[name='restricted']:checked").val();
                var filmLength = $("#filmLength").val();
                var author = $("#author").val();
                var movieGenre = $("#movieGenre").val();
                var actorList = $("#actorList").val();
                var countries = $("#countries").val();
                var trailerLink = $("#trailerLink").val();
                var posterPicture = $("#posterPicture").val();
                var additionPicture = $("#additionPicture").val();
                var filmStatus = $("#filmStatus").val()
                var filmContent = $("#filmContent").val();

                $http({
                    method: "POST",
                    url: "/Home/CreateFilm",
                    params: {
                        filmId: filmId, filmName: filmName, dateRelease: dateRelease, restricted: restricted, filmLength: filmLength,
                        author: author, movieGenre: movieGenre, actorList: actorList,
                        countries: countries, trailerLink: trailerLink, posterPicture: posterPicture, additionPicture: additionPicture,
                        filmStatus: filmStatus, filmContent: filmContent
                    }
                }).then(function (response) {
                    $scope.FilmNowShowing = response.data;

                    $scope.clearForm();

                    $('#modal-film').modal('hide');

                    alert("Success!");
                });
            }
        });
    };

    $scope.clearForm = function () {
        $("#form-film")[0].reset();
    }
    
}

myApp.controller("homeController", homeController);

var validationManager = {
    filmValidation: function () {
        $("#form-film").validate({
            rules: {
                filmName: {
                    required: true
                },
                filmLength : {
                    required: true,
                    min: 0
                },
                author : {
                    required: true
                },
                movieGenre : {
                    required: true
                },
                actorList : {
                    required: true
                },
                countries : {
                    required: true
                },
                filmContent : {
                    required: true
                }

            },
            messages: {
                filmName: {
                    required: 'Please enter film name!',
                },
                filmLength: {
                    required: 'Please enter film length!',
                    min: 'Film length must be greater than 0!'
                },
                author: {
                    required: 'Please enter author!',
                },
                movieGenre: {
                    required: 'Please enter film genre!',
                },
                actorList: {
                    required: 'Please enter actors!',
                },
                countries: {
                    required: 'Please enter countries!',
                },
                filmContent: {
                    required: 'Please enter film content',
                }
            }
        });
    }
}

