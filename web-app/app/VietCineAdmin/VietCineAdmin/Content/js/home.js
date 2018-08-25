$(document).ready(function () {
    validationManager.filmValidation();
    $("#posterPicture").change(function () {
        console.log("here");
        readURL(this, "#imagePosterPicture");
    });
    $("#additionPicture").change(function () {
        readURL(this, "#imageAdditionPicture");
    });
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
    $scope.ShowAddFilmModal = function () {
        $('#imagePosterPicture').attr('src', 'https://placeidentity.gr/wp-content/themes/aloom/assets/images/default.jpg');
        $('#imageAdditionPicture').attr('src', 'https://placeidentity.gr/wp-content/themes/aloom/assets/images/default.jpg');
        $("#modal-film").modal();
    }

    $scope.clickDetail = function (index) {
        $scope.FilmNowSelected = $scope.FilmNowShowing[index];

        $('#datepicker').datepicker({ dateFormat: 'dd/mm/yy' });

        $('#datepicker').datepicker('setDate', $scope.FilmNowSelected.dateRelease);
        $("input[name=restricted][value=" + $scope.FilmNowSelected.restricted + "]").attr('checked', 'checked');
        $("#filmId").val($scope.FilmNowSelected.filmId);
        $("#filmName").val($scope.FilmNowSelected.name);
        $("#filmLength").val($scope.FilmNowSelected.filmLength);
        $("#author").val($scope.FilmNowSelected.author);
        $("#movieGenre").val($scope.FilmNowSelected.movieGenre);
        $("#actorList").val($scope.FilmNowSelected.actorList);
        $("#countries").val($scope.FilmNowSelected.countries);
        $("#trailerLink").val($scope.FilmNowSelected.trailerLink);
        //$("#divFilePicture").hide();
        $("#filmStatus").val($scope.FilmNowSelected.filmStatus);
        $("#filmContent").val($scope.FilmNowSelected.filmContent);
    };


    $scope.clickDisable = function (index) {

        $scope.FilmNowSelected = $scope.FilmNowShowing[index];
        var filmId = $scope.FilmNowSelected.filmId;

        $http({
            method: "POST",
            url: "/Home/DiableFilm",
            params: {
                filmId: filmId
            }
        }).then(function (response) {
            alert("Success!");
            $scope.FilmNowShowing = response.data;
        });
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
                var posterPicturePath = "";
                if (posterPicture != "") {
                    var arrStr = posterPicture.split('\\');
                    posterPicturePath = arrStr[arrStr.length - 1];
                }

                var additionPicture = $("#additionPicture").val();
                var additionPicturePath = "";
                if (additionPicture != "") {
                    var arrStr = additionPicture.split('\\');
                    additionPicturePath = arrStr[arrStr.length - 1];
                }

                var filmStatus = $("#filmStatus").val()
                var filmContent = $("#filmContent").val();

                $http({
                    method: "POST",
                    url: "/Home/CreateFilm",
                    params: {
                        filmId: filmId, filmName: filmName, dateRelease: dateRelease, restricted: restricted, filmLength: filmLength,
                        author: author, movieGenre: movieGenre, actorList: actorList,
                        countries: countries, trailerLink: trailerLink, posterPicture: posterPicturePath, additionPicture: additionPicturePath,
                        filmStatus: filmStatus, filmContent: filmContent
                    }
                }).then(function (response) {
                    $scope.FilmNowShowing = response.data;

                    $scope.clearForm();
                    $("#validateModal").modal();
                    $("#modalMessage").html("Successfull!");

                    $('#modal-film').modal('hide');

                });
            }
        });
    };

    $scope.clearForm = function () {
        $('#datepicker').datepicker().datepicker('setDate', new Date());
        $("input[name=restricted][value=" + 0 + "]").attr('checked', 'checked');
        $("#filmId").val("0");
        $("#filmName").val("");
        $("#filmLength").val("");
        $("#author").val("");
        $("#movieGenre").val("");
        $("#actorList").val("");
        $("#countries").val("");
        $("#trailerLink").val("");
        $("#divFilePicture").show();
        $("#filmStatus").val("1");
        $("#filmContent").val("");
        $("#posterPicture").val("");
        $("#additionPicture").val("");
    };
    

    $scope.uploadPicture = function (id) {
        var filmId = $("#filmId").val();
        id = "#" + id;
        var formData = new FormData();
        var files = $(id).get(0).files;

        // Add the uploaded image content to the form data collection
        if (files.length > 0) {
            formData.append("imageUpload", files[0]);
            var fileName = files[0].name;
            jQuery.ajax({
                url: '/Home/SaveImage',
                data: formData,
                cache: false,
                contentType: false,
                processData: false,
                method: 'POST',
                success: function (data) {
                    alert("Upload image " + data.message);
                    console.log("filmId: " + filmId);
                    if (filmId != 0) {
                        $http({
                            method: "POST",
                            url: "/Home/UpdateImageLink",
                            params: {
                                filmId: filmId,
                                fileName: fileName,
                                type: id
                            }
                        }).then(function (response) {
                            $scope.FilmNowShowing = response.data;
                        });
                    };
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert("Some error occur, can't upload image!");
                }
            });
        } else {
            $("#validateModal").modal();
            $("#modalMessage").html("No file selected");
        }
    };
}

myApp.controller("homeController", homeController);

var validationManager = {
    filmValidation: function () {
        $("#form-film").validate({
            rules: {
                filmName: {
                    required: true
                },
                filmLength: {
                    required: true,
                    min : 1
                },
                movieGenre: {
                    required: true
                },
                filmStatus: {
                    required: true,
                },
            },
            messages: {
                filmName: {
                    required: 'Please enter film name!'
                },
                filmLength: {
                    required: 'Please enter film length!',
                    min: 'Film length must be greater than 0!'
                },
                movieGenre: {
                    required: 'Please enter film genre !'
                },
                filmStatus: {
                    required: 'Please enter film status!',
                },
            }
        });
    }
}

function readURL(input,elementId) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $(elementId).attr('src', e.target.result)
                .width(150)
                .height(150);

        }
        reader.readAsDataURL(input.files[0]);
    }
}

