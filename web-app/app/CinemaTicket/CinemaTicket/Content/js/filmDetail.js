/// <reference path="bootstrap.min.js" />
$(function () {
});

var myApp = angular.module("detailFilmModule", []);
var filmController = function ($scope, $http) {
    $scope.transferTicketData;
    $scope.groupIndex = 0;
    $scope.dateIndex = 0;
    $scope.groupCinemaList;
    $scope.dateList;
    $scope.currentData; // use to show cinema and time and digtyp information
    $scope.userKey = "userKey";
    $scope.userData = LocalStorageManager.loadDataFromStorage($scope.userKey);
    $scope.isBackgroundLoadDone = false;
    $scope.isDateLoadDone = false;
    $scope.isGroupLoadDone = false;

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
            $scope.isGroupLoadDone = true;
        });

        $http({
            method: "POST",
            url: "/Schedule/GetSeventDateFromNow",
        })
        .then(function (response) {
            $scope.dateList = response.data;
            $scope.isDateLoadDone = true;
        });

        $http({//fast load
            method: "POST",
            url: "/Schedule/LoadScheduleGroupByCinemaForFilmDetail",
            params: { filmId: $("#filmId").val() }
        })
        .then(function (response) {
            $scope.data = response.data;
            //$scope.currentData = $scope.data[$scope.groupIndex].dates[$scope.dateIndex].cinemas;
            // load small part GetSubScheduleFilmDetail

            var timer = setInterval(function () {
                if ($scope.isDateLoadDone == true && $scope.isGroupLoadDone == true) {
                    //auto change seat status from buying to available
                    $http({
                        method: "POST",
                        url: "/Schedule/GetSubScheduleFilmDetail",
                        params: {
                            filmIdStr: $("#filmId").val(), selectDateStr: $scope.dateList[$scope.dateIndex].fullDate,
                            groupIdStr: $scope.groupCinemaList[$scope.groupIndex].id,
                        }
                    })
                    .then(function (response) {
                        $scope.currentData = response.data;
                    });
                    clearInterval(timer);
                }
                console.log("still alive");
            }, 1000);

            // load backgroup
            $http({
                method: "POST",
                url: "/Schedule/LoadScheduleGroupByCinemaForFilmDetailBackGround",
                params: { filmId: $("#filmId").val() }
            })
            .then(function (response) {
                console.log(response.data);
                $scope.isBackgroundLoadDone = true;
                $scope.data = response.data;
                $scope.currentData = $scope.data[$scope.groupIndex].dates[$scope.dateIndex].cinemas;
                console.log("load background done");
            });
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
        if ($scope.isBackgroundLoadDone == true) {
            $scope.currentData = $scope.data[$scope.groupIndex].dates[$scope.dateIndex].cinemas;
        } else {
            if ($scope.isDateLoadDone == true && $scope.isGroupLoadDone == true) {
                $http({
                    method: "POST",
                    url: "/Schedule/GetSubScheduleFilmDetail",
                    params: {
                        filmIdStr: $("#filmId").val(), selectDateStr: $scope.dateList[$scope.dateIndex].fullDate,
                        groupIdStr: $scope.groupCinemaList[$scope.groupIndex].id,
                    }
                })
                .then(function (response) {
                    $scope.currentData = response.data;
                });
            }
        }
    };
    $scope.dateClickHandler = function (index) {
        for (var i = 0 ; i < 7; i++) {
            var anId = "dateofweek" + i;
            document.getElementById(anId).style.color = "black";
        }
        var elementId = "dateofweek" + index;
        document.getElementById(elementId).style.color = "red";

        $scope.dateIndex = index;

        if ($scope.isBackgroundLoadDone == true) {
            console.log("here");
            console.log($scope.data);
            console.log($scope.data[$scope.groupIndex].dates);
            console.log($scope.data[$scope.groupIndex].dates[$scope.dateIndex]);
            $scope.currentData = $scope.data[$scope.groupIndex].dates[$scope.dateIndex].cinemas;
        } else {
            if ($scope.isDateLoadDone == true && $scope.isGroupLoadDone == true) {
                $http({
                    method: "POST",
                    url: "/Schedule/GetSubScheduleFilmDetail",
                    params: {
                        filmIdStr: $("#filmId").val(), selectDateStr: $scope.dateList[$scope.dateIndex].fullDate,
                        groupIdStr: $scope.groupCinemaList[$scope.groupIndex].id,
                    }
                })
                .then(function (response) {
                    $scope.currentData = response.data;
                });
            }
        }
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
                $http({
                    method: "POST",
                    url: "/home/CheckDupplicateRoom",
                    params: { filmId: filmId, timeId: timeId, cinemaId: cinemaId, selectDate: selectDate }
                })
                .then(function (response) {
                    $scope.transferTicketData = response.data;
                    console.log($scope.transferTicketData);
                    if ($scope.transferTicketData.length == 1) {
                        var scheduleId = $scope.transferTicketData[0].scheduleId;
                        var availableSeat = $scope.transferTicketData[0].availableSeat;
                        if (availableSeat != 0) {
                            $scope.submitChooseTicketForm(scheduleId);
                        } else {
                            $("#validateModal").modal();
                            $("#modalMessage").html("Suất chiếu đã hết ghế!");
                        }
                    } else {
                        $("#myModalChooseRoom").modal();
                    }
                });
            }
        });
    };

    $scope.submitChooseTicketForm = function (scheduleId) {
        var param1 = "<input type='hidden' name='scheduleIdStr' value='" + scheduleId + "' />";
        document.getElementById('goToChooseTicketAndSeatForm').innerHTML = param1;
        document.getElementById('goToChooseTicketAndSeatForm').submit();
    }
    /*================ Login & Register Part*/
    $scope.validateUsername = function (str) {
        return /^[a-z][a-z0-9_.-]{4,19}$/i.test(str);
    };
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
        } else if (!$scope.validateUsername(username)) {
            $('#validateModal').modal();
            $("#modalMessage").html("Tài khoản sai phải từ 5 kí tự trở lên và không chứa kí tự đặc biệt!");
        } else if (!$scope.validateUsername(password)) {
            $('#validateModal').modal();
            $("#modalMessage").html("Mật khẩu phải từ 5 kí tự trở lên và không chứa kí tự đặc biệt!");
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
        } else if (!$scope.validateUsername(password)) {
            $('#validateModal').modal();
            $("#modalMessage").html("Mật khẩu phải từ 5 kí tự trở lên và không chứa kí tự đặc biệt!");
        } else if (!$scope.validateUsername(username)) {
            $('#validateModal').modal();
            $("#modalMessage").html("Tài khoản phải từ 5 kí tự trở lên và không chứa kí tự đặc biệt!");
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
        $http({
            method: "POST",
            url: "/Login/Logout"
        })
        .then(function (response) {
            console.log(response);
            if (response.data.status == "ok") {
                $('#validateModal').modal();
                $("#modalMessage").html("Bạn đã đăng xuất!");
            }
        });
    }
    $scope.gohome = function () {
        document.getElementById('gotoHomeForm').submit();
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
    return /\(?([0-9]{3})\)?([ .-]?)([0-9]{3})\2([0-9]{4})/.test(inputphone);
}