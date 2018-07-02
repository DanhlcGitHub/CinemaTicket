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
    $scope.userKey = "userKey";
    $scope.userData = LocalStorageManager.loadDataFromStorage($scope.userKey);
    if ($scope.userData == undefined) {
        $scope.userData = "";
    }
    $('#myModalTrailer').on("hidden.bs.modal", function () {
        $('#myIframe').prop('src', "");
    });

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
        for (var i = 0 ; i < $scope.groupCinemaList.length; i++) {
            var anId = "groupcinema" + i;
            document.getElementById(anId).style.color = "black";
        }
        var elementId = "groupcinema" + index;
        document.getElementById(elementId).style.color = "red";

        $scope.groupIndex = index;
        $scope.currentData = $scope.data[$scope.groupIndex].dates[$scope.dateIndex].cinemas;
        console.log("current data");
        console.log($scope.currentData);
    };
    $scope.dateClickHandler = function (index) {
        for (var i = 0 ; i < $scope.groupCinemaList.length; i++) {
            var anId = "dateofweek" + i;
            document.getElementById(anId).style.color = "black";
        }
        var elementId = "dateofweek" + index;
        document.getElementById(elementId).style.color = "red";

        $scope.dateIndex = index;
        $scope.currentData = $scope.data[$scope.groupIndex].dates[$scope.dateIndex].cinemas;
        console.log("current data");
        console.log($scope.data[$scope.groupIndex].dates[$scope.dateIndex].cinemas);
    };
    $scope.openTrailerDialog = function (url) {
        console.log(url);
        $('#myIframe').prop('src', url);
        $('#myModalTrailer').modal();
    };
    $scope.gotoChooseTicket = function (filmId, timeId, cinemaId, startTime) {
        var selectDate = $scope.dateList[$scope.dateIndex].fullDate;
        $http({
            method: "POST",
            url: "/utility/CompareScheduleTimeForSelectedDate",
            params: { startTime: startTime, selectDate: selectDate }
        })
        .then(function (response) {
            if (response.data.valid == "false") {
                alert("Xuất chiếu đã hết hạn");
            } else {
                console.log("filmId " + filmId);
                console.log("timeId " + timeId);
                console.log("cinemaId " + cinemaId);
                console.log("selectDate " + selectDate);

                var param1 = "<input type='hidden' name='filmId' value='" + filmId + "' />";
                var param2 = "<input type='hidden' name='timeId' value='" + timeId + "' />";
                var param3 = "<input type='hidden' name='cinemaId' value='" + cinemaId + "' />";
                var param4 = "<input type='hidden' name='selectDate' value='" + selectDate + "' />";
                document.getElementById('goToChooseTicketAndTicketForm').innerHTML = param1 + param2 + param3 + param4;
                document.getElementById('goToChooseTicketAndTicketForm').submit();
            }
        }
    );
    };
    /*================ Login & Register Part*/
    $scope.showLogin = function () {
        $("#myModalRegister").modal('hide');
        $("#myModalLogin").modal();
    };
    $scope.showRegister = function () {
        $("#myModalLogin").modal('hide');
        $("#myModalRegister").modal();
    };
    $scope.login = function () {
        var username = $("#login_username").val();
        var password = $("#login_password").val();
        if (username == "" || password == "") {
            $('#validateModal').modal();
            $("#modalMessage").html("Bạn chưa nhập tài khoản và mật khẩu!");
        } else {
            $http({
                method: "POST",
                url: "/Login/CheckLogin",
                params: { username: username, password: password }
            })
            .then(function (response) {
                console.log(response.data);
                var status = response.data.status;
                if (status == "valid") {
                    $('#validateModal').modal();
                    $("#myModalLogin").modal('hide');
                    $("#modalMessage").html("Đăng nhập thành công!");
                    $scope.userData = response.data;
                    LocalStorageManager.saveToLocalStorage(response.data, $scope.userKey);
                } else if (status == "notValid") {
                    $('#validateModal').modal();
                    $("#modalMessage").html("Đăng nhập thất bại, sai tên đăng nhập hoặc mật khẩu!");
                }
            });
        }
    };
    $scope.register = function () {
        var username = $("#register_username").val();
        var password = $("#register_password").val();
        var phone = $("#register_phone").val();
        var email = $("#register_email").val();
        if (username == "" || password == "" || phone == "" || email == "") {
            $('#validateModal').modal();
            $("#modalMessage").html("Bạn chưa điền đầy đủ thông tin!");
        } else if (!validateEmail(email)) {
            $('#validateModal').modal();
            $("#modalMessage").html("Sai định dạng email");
        } else if (!validatePhone(phone)) {
            $('#validateModal').modal();
            $("#modalMessage").html("Số điện thoại phải từ 10-11 chữ số");
        } else if (username.length < 6 || username.length > 20 || password.length < 6 || password.length > 20) {
            $('#validateModal').modal();
            $("#modalMessage").html("Tên đăng nhập và mật khẩu phải từ 6 kí tự và bé hơn 20 kí tự");
        } else {
            $http({
                method: "POST",
                url: "/Login/CheckRegister",
                params: { username: username, password: password, phone: phone, email: email }
            })
            .then(function (response) {
                console.log(response.data);
                var status = response.data.status;
                if (status == "valid") {
                    $('#validateModal').modal();
                    $("#myModalRegister").modal('hide');
                    $("#modalMessage").html("Tạo tài khoản thành công!");
                    $scope.userData = response.data;
                    LocalStorageManager.saveToLocalStorage(response.data, $scope.userKey);
                } else if (status == "notValid") {
                    $('#validateModal').modal();
                    $("#modalMessage").html("Tên đăng nhập đã tồn tại!");
                }
            });
        }
    };
    $scope.logout = function () {
        LocalStorageManager.removeDataFromStorage($scope.userKey);
        $scope.userData = "";
    };
}
myApp.controller("filmController", filmController);

/* console.log(window.location.hostname);
               console.log(window.location.href);
               console.log(window.location.pathname);
               console.log(window.location.href.replace(window.location.pathname, ''));*/
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
function validatePhone(inputphone) {
    return inputphone.match(/\d/g).length === 10;
}