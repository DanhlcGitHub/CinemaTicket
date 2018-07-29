/// <reference path="bootstrap.min.js" />
$(document).ready(function () {

});


var myApp = angular.module("homeModule", []);
var filmController = function ($scope, $http) {
    $scope.currentGroupCinemaIndex = 0;// which group is focus
    $scope.currentCinemaIndex = 0; // which cinema is focus
    $scope.pageIdex = 0;
    $scope.maxItem = 8;
    $scope.resellEmail;
    $scope.resellData;
    $scope.userKey = "userKey";
    $scope.userData = LocalStorageManager.loadDataFromStorage($scope.userKey);
    $scope.isBackgroundLoadDone = false;

    if ($scope.userData == undefined) {
        $scope.userData = "";
    }
    $('#myModalTrailer').on("hidden.bs.modal", function () {
        $('#myIframe').prop('src', "");
    });

    $('#validateModal').on("hidden.bs.modal", function () {
        $('body').addClass('modal-open');
    });

    $('#myModalRegister').on("hidden.bs.modal", function () {
        $('body').addClass('modal-open');
    });

    $('#myModalLogin').on("hidden.bs.modal", function () {
        $('body').addClass('modal-open');
    });

    $http({
        method: "POST",
        url: "Schedule/LoadCinemaBelongToGroup"
    })
    .then(function (response) {
        $scope.scheduleList = response.data;
        $scope.currentCinemaList = response.data[$scope.currentGroupCinemaIndex].cinemas;
        //$scope.currentFilmInScheduleList = $scope.currentCinemaList[$scope.currentCinemaIndex].films;
        $http({
            method: "POST",
            url: "Schedule/GetFilmByCinemaAndDate",
            params: { cinemaId: $scope.currentCinemaList[0].id }
        })
        .then(function (response) {
            $scope.currentFilmInScheduleList = response.data;
        });
    });

    // background load
    $http({
        method: "POST",
        url: "schedule/LoadScheduleGroupByCinema"
    })
    .then(function (response) {
        console.log("Background load done");
        $scope.scheduleList = response.data;
        $scope.currentCinemaList = response.data[$scope.currentGroupCinemaIndex].cinemas;
        $scope.isBackgroundLoadDone = true;
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
        if ($scope.currentCinemaIndex.length != 0) {
            if ($scope.isBackgroundLoadDone == false) {
                $http({
                    method: "POST",
                    url: "Schedule/GetFilmByCinemaAndDate",
                    params: { cinemaId: $scope.currentCinemaList[0].id }
                })
                .then(function (response) {
                    $scope.currentFilmInScheduleList = response.data;
                });
            } else {
                $scope.currentFilmInScheduleList = $scope.currentCinemaList[0].films;
            }
        }
    };
    $scope.onclickCinema = function (index) {
        for (var i = 0 ; i < $scope.currentCinemaList.length; i++) {
            var anId = "cinema" + i;
            document.getElementById(anId).style.borderColor = "white";
        }
        var elementId = "cinema" + index;
        document.getElementById(elementId).style.border = "1px solid red";

        $scope.currentCinemaIndex = index;

        if ($scope.isBackgroundLoadDone == false) {
            $http({
                method: "POST",
                url: "Schedule/GetFilmByCinemaAndDate",
                params: { cinemaId: $scope.currentCinemaList[$scope.currentCinemaIndex].id }
            })
            .then(function (response) {
                $scope.currentFilmInScheduleList = response.data;
            });
        } else {
            $scope.currentFilmInScheduleList = $scope.currentCinemaList[$scope.currentCinemaIndex].films;
        }
        //$scope.currentFilmInScheduleList = $scope.currentCinemaList[$scope.currentCinemaIndex].films;
    };
    $scope.openTrailerDialog = function (url) {
        console.log(url);
        if (url.includes("imdb")) {
            $scope.openInNewTab(url);
        } else {
            $('#myIframe').prop('src', url);
            $('#myModalTrailer').modal();
        }
    };
    $scope.openInNewTab = function(url) {
        var win = window.open(url, '_blank');
        win.focus();
    }

    $scope.loadUpcommingMovie = function (event) {
        $("#showingMovieId").attr('class', 'nav-link text-muted');
        $("#upcommingMovieId").attr('class', 'nav-link text-danger');
        $scope.pageIdex = 0;
        $scope.FilmData = $.grep($scope.AllFilmData, function (v) {
            return v.filmStatus === 2;
        });
        $scope.CurrentFilmList = $scope.FilmData.slice($scope.pageIdex * $scope.maxItem, (($scope.pageIdex + 1) * $scope.maxItem));
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
            $('#validateModal').modal();
            $("#modalMessage").html("Sai định dạng email!");
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
                $('#validateModal').modal();
                $("#modalMessage").html("Mã xác nhận không đúng.");
            } else {
                document.getElementById("confirmResellEmailBlock").style.display = "none";
                $("#resellTicketList").slideToggle("slow");
                $scope.resellData = response.data;
                if ($scope.resellData.length > 5) {
                    $('#resellTicketList').css("height", "400px");
                }
                console.log("resell ticket data");
                console.log(response.data);
            }
        });

    };
    $scope.postSellingTicket = function (index) {
        var ticketId = $scope.resellData[index].ticketId;
        var inputId = "resellDescription" + index;
        var description = document.getElementById(inputId).value;
        $http({
            method: "POST",
            url: "Ticket/PostSellingTicket",
            params: { ticketId: ticketId, description: description }
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
                params: { ticketId: ticketId, buyerEmail: buyerEmail, sellerEmail: $scope.resellEmail }
            })
            .then(function (response) {
                if (response.data.isSuccess == "true") {
                    $scope.resellData[index].status = response.data.status;
                    $scope.resellData[index].statusvn = response.data.statusvn;
                } else {
                    $('#validateModal').modal();
                    $("#modalMessage").html("Xảy ra lỗi kết nối, vui lòng kiểm tra kết nối!");
                }
                
            });
        } else {
            $('#validateModal').modal();
            $("#modalMessage").html("Email không hợp lệ.");
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
                $('#validateModal').modal();
                $("#modalMessage").html("Xuất chiếu đã hết hạn");
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
                        $scope.submitChooseTicketForm(scheduleId);
                    } else {
                        $("#myModalChooseRoom").modal();
                    }
                });
            }
            /*if (response.data.valid == "false") {
                $('#validateModal').modal();
                $("#modalMessage").html("Xuất chiếu đã hết hạn");
            } else {
                var cinemaId = $scope.currentCinemaList[$scope.currentCinemaIndex].id;
                console.log("go to choose seat");
                console.log("filmId " + filmId);
                console.log("timeId " + timeId);
                console.log("cinemaId " + cinemaId);

                var param1 = "<input type='hidden' name='filmId' value='" + filmId + "' />";
                var param2 = "<input type='hidden' name='timeId' value='" + timeId + "' />";
                var param3 = "<input type='hidden' name='cinemaId' value='" + cinemaId + "' />";

                document.getElementById('goToChooseTicketAndSeatForm').innerHTML = param1 + param2 + param3;
                document.getElementById('goToChooseTicketAndSeatForm').submit();
            }*/
        });
    };
    /*================ Login & Register Part*/
    $scope.showLogin = function () {
        $("#myModalRegister").modal('hide');
        $("#myModalLogin").modal();
        $('body').addClass('modal-open');
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
                url: "Login/CheckLogin",
                params: { username: username, password: password }
            })
            .then(function (response) {
                console.log(response.data);
                var status = response.data.status;
                if (status == "valid") {
                    $('#validateModal').modal();
                    $("#myModalLogin").modal('hide');
                    $("#modalMessage").html("Đăng nhập thành công!");
                    $('#loginForm')[0].reset();
                    
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
                url: "Login/CheckRegister",
                params: { username: username, password: password, phone: phone, email: email }
            })
            .then(function (response) {
                console.log(response.data);
                var status = response.data.status;
                if (status == "valid") {
                    $('#validateModal').modal();
                    $("#myModalRegister").modal('hide');
                    $("#modalMessage").html("Tạo tài khoản thành công!");
                    $('#registerForm')[0].reset();
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
function validatePhone(inputphone) {
    return inputphone.match(/\d/g).length === 10;
}