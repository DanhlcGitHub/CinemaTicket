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
    $scope.resellEmail;
    $scope.resellData;
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
        for (var i = 0 ; i < $scope.scheduleList.length; i++) {
            var anId = "groupcinema" + i;
            document.getElementById(anId).style.opacity = 0.4;
        }
        var elementId = "groupcinema" + index;
        document.getElementById(elementId).style.opacity = 1;

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
        for (var i = 0 ; i < $scope.currentCinemaList.length; i++) {
            var anId = "cinema" + i;
            document.getElementById(anId).style.borderColor = "white";
        }
        var elementId = "cinema" + index;
        document.getElementById(elementId).style.border = "1px solid red";

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
        window.location.href = 'home/FilmDetail?filmId=' + id;
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
    $scope.resellTicketSearch = function () {
        $scope.resellEmail = document.getElementById("resellEmailTxt").value;
        document.getElementById("findResellTicketBtn").disabled = true;
        if (validateEmail($scope.resellEmail)) {
            $http({
                method: "POST",
                url: "Ticket/SendResellConfirmCode",
                params: { email: $scope.resellEmail }
            })
            .then(function (response) {
                $("#confirmResellEmailBlock").slideToggle("slow");
                document.getElementById("inputEmailResellEmailBlock").style.display = "none"
            });
        } else {
            document.getElementById("findResellTicketBtn").disabled = false;
            alert("Wrong email format!");
        }
    };
    //checkResellConfirmCode
    $scope.checkResellConfirmCode = function () {
        var confirmCode = document.getElementById("resellConfirmCodeTxt").value;
        console.log("confirmCode: " + confirmCode);
        $http({
            method: "POST",
            url: "Ticket/GetTicketListBelongToMail",
            params: { confirmCode: confirmCode, email: $scope.resellEmail }
        })
        .then(function (response) {
            if (response.data.isWrong == "true") {
                alert("Mã xác nhận không đúng.");
            } else {
                document.getElementById("confirmResellEmailBlock").style.display = "none";
                $("#resellTicketList").slideToggle("slow");
                $scope.resellData = response.data;
                console.log("resell ticket data");
                console.log(response.data);
            }
        });

    };
    $scope.postSellingTicket = function (index) {
        var ticketId = $scope.resellData[index].ticketId;
        $http({
            method: "POST",
            url: "Ticket/PostSellingTicket",
            params: { ticketId: ticketId }
        })
        .then(function (response) {
            $scope.resellData[index].status = response.data.status;
            $scope.resellData[index].statusvn = response.data.statusvn;
        });
    };
    $scope.resellTicket = function (index) {
        var inputId = "customerEmail" + index;
        var ticketId = $scope.resellData[index].ticketId;
        var buyerEmail = document.getElementById(inputId).value;
        if (validateEmail(buyerEmail)) {
            $http({
                method: "POST",
                url: "Ticket/ResellTicket",
                params: { ticketId: ticketId, buyerEmail : buyerEmail, sellerEmail : $scope.resellEmail }
            })
            .then(function (response) {
                $scope.resellData[index].status = response.data.status;
                $scope.resellData[index].statusvn = response.data.statusvn;
            });
        } else {
            alert("Email không hợp lệ.");
        }
    };
    $scope.gotoChooseTicket = function (filmId, timeId, startTime) {
        $http({
            method: "POST",
            url: "utility/CompareScheduleTimeForToday",
            params: { startTime: startTime }
        })
        .then(function (response) {
            if (response.data.valid == "false") {
                alert("Xuất chiếu đã hết hạn");
            } else {
                var cinemaId = $scope.currentCinemaList[$scope.currentCinemaIndex].id;
                console.log("go to choose seat");
                console.log("filmId " + filmId);
                console.log("timeId " + timeId);
                console.log("cinemaId " + cinemaId);

                var param1 = "<input type='hidden' name='filmId' value='" + filmId + "' />";
                var param2 = "<input type='hidden' name='timeId' value='" + timeId + "' />";
                var param3 = "<input type='hidden' name='cinemaId' value='" + cinemaId + "' />";

                document.getElementById('goToChooseTicketForm').innerHTML = param1 + param2 + param3;
                document.getElementById('goToChooseTicketForm').submit();
            }
        });
    };
    $scope.getPath = function () {

    }
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

function validateEmail(inputemail) {
    var regEmail = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return inputemail.match(regEmail);
}