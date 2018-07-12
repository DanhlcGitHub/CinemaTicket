$(document).ready(function () {
    openChooseTicket();
});

function openChooseTicket() {
    document.getElementById("chooseSeatAreaContent").style.display = "none";
    document.getElementById("checkoutSidebar").style.display = "none";
    document.getElementById("chooseTicketContent").style.display = "block";
    document.getElementById("filmInforSidebar").style.display = "block";
    document.getElementById("chooseTicketContent").style.marginLeft = "25%";
    document.getElementById("topbar").style.marginLeft = "25%";
    document.getElementById("filmInforSidebar").style.width = "25%";
    document.getElementById("chooseTicketContent").style.marginRight = "0%";
    document.getElementById("topbar").style.marginRight = "0%";
    $("#chooseTicketStep").css({ 'color': 'red' });
    $("#chooseSeatStep").css({ 'color': 'black' });
}


var myApp = angular.module("chooseTicketModule", []);
var chooseTicketController = function ($scope, $http) {
    $scope.scheduleId;
    $scope.totalAmount = 0;
    $scope.userKey = "userKey";
    $scope.userData = LocalStorageManager.loadDataFromStorage($scope.userKey);
    if ($scope.userData == undefined) {
        $scope.userData = "";
    }

    angular.element(document).ready(function () {
        console.log("init");
        $scope.scheduleId = $("#scheduleId").val();
        // Load Film base on scheduleId
        $http({
            method: "POST",
            url: "/ChooseTicket/GetChooseTicketData",
            params: { scheduleId: $scope.scheduleId }
        })
        .then(function (response) {
            $scope.chooseTicketData = response.data;
            console.log($scope.chooseTicketData);

            document.getElementById("filmInforSidebar").style.backgroundImage = "url('" + $scope.chooseTicketData.img + "')";
            $scope.calculateTotalAmout();

            //set data for service

        });
    });

    $scope.clickPlusButton = function (index) {
        var sum = $scope.calculateSum();
        if (sum >= 10) {
            alert("Bạn Chỉ Được Mua 10 Vé!");
        } else {
            $scope.chooseTicketData.typeOfSeats[index].userChoose++;
            $scope.calculateTotalAmout();
        }
        //$("#chooseSeatButtonId").attr("disabled", "");
        console.log("index " + index + ";plus: " + $scope.chooseTicketData.typeOfSeats[index].userChoose);
    };

    $scope.clickMinusButton = function (index) {
        if ($scope.chooseTicketData.typeOfSeats[index].userChoose > 0) {
            $scope.chooseTicketData.typeOfSeats[index].userChoose--;
            //$("#chooseSeatButtonId").attr("disabled", "");
            var sum = $scope.calculateSum();
            if (sum <= 0) {
                console.log("go here");
                //$("#chooseSeatButtonId").attr("disabled","disabled");
            }
            $scope.calculateTotalAmout();
        }
    };
    $scope.calculateSum = function () {
        var sum = 0;
        for (var i = 0; i < $scope.chooseTicketData.typeOfSeats.length; i++) {
            sum += $scope.chooseTicketData.typeOfSeats[i].userChoose;
        }
        console.log(sum);
        return sum;
    };
    $scope.calculateTotalAmout = function () {
        $scope.totalAmount = 0;
        for (var i = 0; i < $scope.chooseTicketData.typeOfSeats.length; i++) {
            $scope.totalAmount += (parseInt($scope.chooseTicketData.typeOfSeats[i].userChoose)
                * parseInt($scope.chooseTicketData.typeOfSeats[i].price));
        }
    };
    $scope.gotoChooseSeat = function () {
        var sum = $scope.calculateSum();
        if (sum > 0) {
            // animation
            document.getElementById("filmInforSidebar").style.display = "none";
            document.getElementById("chooseSeatAreaContent").style.display = "block";
            document.getElementById("chooseTicketContent").style.display = "none";
            document.getElementById("checkoutSidebar").style.display = "block";
            document.getElementById("chooseSeatAreaContent").style.marginRight = "25%";
            document.getElementById("topbar").style.marginRight = "25%";
            document.getElementById("checkoutSidebar").style.width = "25%";
            document.getElementById("chooseSeatAreaContent").style.marginLeft = "0%";
            document.getElementById("topbar").style.marginLeft = "0%";
            $("#chooseTicketStep").css({ 'color': 'black' });
            $("#chooseSeatStep").css({ 'color': 'red' });
            // data handler
            $scope.totalTicket = 0;
            $scope.quantityData = $scope.chooseTicketData.typeOfSeats;
            for (var i = 0; i < $scope.quantityData.length; i++) {
                $scope.totalTicket += $scope.quantityData[i].userChoose;
            }
            $scope.calculateTotalAmout();
            console.log("amount: " + $scope.totalAmount);
            $scope.totalAmoutUSD = Math.round((parseFloat($scope.totalAmount) / $scope.exchangeRate) * 100) / 100;
        } else {
            alert("Bạn chưa chọn ghế nào!");
        }
    }

    //================================================= choose Seat Part
    $scope.seatData;
    $scope.Math = window.Math;
    $scope.quantityDataKey = "quantityDataKey";
    $scope.totalAmountKey = "totalAmoutKey";
    $scope.totalAmount = 0;
    $scope.totalAmoutUSD = 0;
    $scope.quantityData;
    $scope.orderData; // contain scheduleId, seatId List
    $scope.matrix = []; // 
    $scope.matrixX = 0;
    $scope.matrixY = 0;
    $scope.totalTicket = 0;
    $scope.countClick = 0;
    $scope.choosedList = [];
    $scope.middeSeatFlag = false;
    $scope.email;
    $scope.phone;
    $scope.ticketData;
    $scope.currentCart = [];
    $scope.countDown = 300;
    $scope.exchangeRate = 23000;
    $scope.isActionClick = false;

    $scope.alpha = ["A", "B", "C", "D", "E", "F", "J", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U"];
    $scope.TicketStatusEnum = Object.freeze({ "available": "btn-seat", "buyed": "btn-seat-buyed", "buying": "btn-seat-buyed", "resell": "btn-seat-resell" });
    // init data
    
    // beforeunload---------------------
    window.addEventListener('beforeunload', function (e) {
        //auto change seat status from buying to available
        if ($scope.ticketData != "" && $scope.ticketData != null) {
            $http({
                method: "POST",
                url: "/Ticket/BuyingToAvailable",
                params: { ticketListStr: JSON.stringify($scope.ticketData) }
            })
            .then(function (response) {
                $scope.ticketData = response.data;
            });
        }
    });

    $("#paypal-button-container").click(function () {
        console.log("paypal clicked");
    });

    $http({
        method: "POST",
        url: "/Seat/FindAllSeatByScheduleId",
        params: { scheduleId: $("#scheduleId").val() }
    })
    .then(function (response) {
        $scope.seatData = response.data.seats;//seat data
        $scope.matrixX = response.data.matrixX;
        $scope.matrixY = response.data.matrixY;
        $scope.displayData($scope.matrixX, $scope.matrixY);
        $scope.scheduleId = $("#scheduleId").val();
    });

    $http({
        method: "POST",
        url: "/ChooseTicket/GetChooseTicketData",
        params: { scheduleId: $("#scheduleId").val() }
    })
    .then(function (response) {
        $scope.scheduleData = response.data;
    });
    $scope.onclickSeat = function (seatId, px, py, seatStatus) {
        if (seatStatus == "available") {
            console.log(px + ":" + px);
            var index = 0;
            index = $scope.countClick % $scope.totalTicket;
            $scope.choosedList[index] = {
                isNull: false,
                className: 'btn-seat-choosing',
                seatId: seatId,
                px: px,
                py: py
            };
            $scope.countClick++;
            $scope.displayData($scope.matrixX, $scope.matrixY);
            for (var i = 0 ; i < $scope.choosedList.length ; i++) {
                var seat = $scope.choosedList[i];
                $scope.matrix[seat.py].seats[seat.px] = seat;
            }
            $scope.middeSeatFlag = $scope.validMiddleSeat();
            if ($scope.middeSeatFlag == true) {
                $("#validateModal").modal();
                $("#modalMessage").html("Không thể để trống ghế ở giữa!");
            }
        }
    };
    $scope.validMiddleSeat = function () {
        for (var i = 0 ; i < $scope.matrix.length; i++) {
            var row = $scope.matrix[i];
            for (var j = 0 ; j < row.seats.length - 2; j++) {
                var col1 = row.seats[j];
                var col2 = row.seats[j + 1];
                var col3 = row.seats[j + 2];
                if (col1.className == 'btn-seat-choosing' && col2.className != 'btn-seat-choosing'
                     && col3.className == 'btn-seat-choosing') {
                    return true;
                }
            }
        }
        return false;
    };
    $scope.displayData = function (matrixX, matrixY) {
        for (var i = 0 ; i < matrixY ; i++) {
            var rowObj = {
                py: i,
                alphaName: $scope.alpha[i],
                seats: [],
            };
            $scope.matrix[i] = rowObj;
        }
        //init null seat
        for (var i = 0 ; i < matrixY ; i++) {
            for (var j = 0 ; j < matrixX ; j++) {
                var seatObj = {
                    isNull: true,
                    className: 'btn-seat-hidden',
                    seatId: -1,
                    px: -1,
                    py: -1
                }
                $scope.matrix[i].seats[j] = seatObj;
            }
        }
        for (var i = 0; i < $scope.seatData.length; i++) {
            //console.log("x: " + $scope.chooseTicketData[i].px + " y: " + $scope.chooseTicketData[i].py);
            //console.log("-----------------------------------------------------" + i);
            var seatObj = {
                isNull: false,
                className: $scope.TicketStatusEnum[$scope.seatData[i].seatStatus],
                isChoosing: false,
                seatId: $scope.seatData[i].id,
                seatStatus: $scope.seatData[i].seatStatus,
                resellDescription: $scope.seatData[i].resellDescription,
                px: parseInt($scope.seatData[i].px),
                py: $scope.seatData[i].py,
                resellDescription: $scope.seatData[i].resellDescription,
                emailOwner: $scope.seatData[i].emailOwner,
            }

            $scope.matrix[seatObj.py].seats[seatObj.px] = seatObj;
        }
    };
    $scope.countController = function () {
        //$scope.countDown = 30;
        var timer = setInterval(function () {
            if ($scope.countDown == 0) {
                //auto change seat status from buying to available
                $http({
                    method: "POST",
                    url: "/Ticket/BuyingToAvailable",
                    params: { ticketListStr: JSON.stringify($scope.ticketData) }
                })
                .then(function (response) {
                    console.log("ticket data after time out");
                    $scope.ticketData = response.data;
                });
                if ($scope.isActionClick == false) {
                    $('#confirmTicketModal').modal('hide');
                    $('#backdropModal').modal();
                    $("#backdropMessage").html("Hết thời gian đặt vé, bấm nút bên dưới để tiếp tục!");
                }
                clearInterval(timer);
                return;
            }
            $scope.countDown--;
            $scope.$apply()
        }, 1000);
    };
    $scope.checkout = function () {
        // check condition
        if ($scope.choosedList.length == $scope.totalTicket) {
            if ($scope.middeSeatFlag == true) {
                $("#validateModal").modal();
                $("#modalMessage").html("Bạn không được để trống ghế ở giữa");
            } else {
                $scope.email = $("#emailId").val();
                $scope.phone = $("#phoneId").val();
                if ($scope.email == "" || $scope.phone == "") {
                    $("#validateModal").modal();
                    $("#modalMessage").html("Thông tin E-mail và Điện thoại không được bỏ trống!");
                } else {
                    // check format email and phone
                    if (validateEmail($("#emailId").val()) && validatePhone($("#phoneId").val())) {
                        console.log("success!");
                        // get ticketId, check status, show thanh toan dialog
                        $http({
                            method: "POST",
                            url: "/Ticket/GetTicketList",
                            params: {
                                choosedList: JSON.stringify($scope.choosedList),
                                scheduleIdStr: $scope.scheduleId
                            }
                        })
                        .then(function (response) {
                            console.log("ticket data");
                            $scope.ticketData = response.data;
                            console.log(response.data);
                            if (!$scope.IsSeatStillAvailable()) {//not available
                                $("#validateModal").modal();
                                $("#modalMessage").html("Loại vé bạn chọn đã hết hoặc không đủ số lượng ghế trống!");
                            } else {
                                //change available to buying
                                $http({
                                    method: "POST",
                                    url: "/Ticket/ChangeAvailableToBuying",
                                    params: { ticketListStr: JSON.stringify($scope.ticketData) }
                                })
                               .then(function (response) {
                                   console.log("ticket data after update");
                                   $scope.ticketData = response.data;
                               });
                                $scope.countController();
                                $scope.renderPaypalButton();
                                $scope.openConfirmDialog();
                            }
                        });
                    } else {
                        $("#validateModal").modal();
                        $("#modalMessage").html("Email và phone sai format!");
                    }
                }
            }
        } else {
            $("#validateModal").modal();
            $("#modalMessage").html("Bạn chưa chọn đủ " + $scope.totalTicket + " vé!");
        }
    };
    $scope.openConfirmDialog = function () {
        //$scope.countController = 300;
        $('#confirmTicketModal').modal();
        var span = document.getElementsByClassName("close")[0];

        // When the user clicks on <span> (x), close the modal
        span.onclick = function () {
            $('#confirmTicketModal').modal('hide');
            $scope.countDown = 0;
            $scope.isActionClick = true;
            $('#backdropModal').modal();
            $("#backdropMessage").html("Bạn đã hủy đặt vé, bấm nút bên dưới để tiếp tục!");
            // reload page
        }
    };
    $scope.renderPaypalButton = function () {
        //<insert production client id>//AXWr3Vji-q_UZFmrhTIxcSMBctnfsofDOxwsi3_llRLpDzwJ83NtZzt7wT5Sg_eB916xA5eC6c7O1APa
        paypal.Button.render({

            env: 'sandbox', // sandbox | production

            client: {
                sandbox: 'AWT16aVyr2SNJR2uBG46HHvz-DIY98lIP3aPAO-sUs36sOvtN9Ay3H0z-4e8cj4qZyNR5Aj3qrsxw0W3',
                production: '<insert production client id>'
            },
            commit: true, // Show a 'Pay Now' button
            payment: function (data, actions) {
                return actions.payment.create({
                    payment: {
                        transactions: [
                            {
                                amount: { total: $scope.totalAmoutUSD, currency: 'USD' }
                            }
                        ]
                    }
                });
            },

            onAuthorize: function (data, actions) {
                return actions.payment.execute().then(function () {
                    window.alert('Payment Complete!');
                });
                return actions.payment.get().then(function (data) {
                    if ($scope.countDown !== 0) { // !timeout
                        return actions.payment.execute().then(function () {
                            // Show a thank-you note
                            $http({
                                method: "POST",
                                url: "/Ticket/MakeOrder",
                                params: {
                                    ticketListStr: JSON.stringify($scope.ticketData),
                                    email: $scope.email,
                                    phone: $scope.phone,
                                    filmName: $scope.scheduleData.filmName,
                                    cinemaName: $scope.scheduleData.cinemaName,
                                    date: $scope.scheduleData.date,
                                    roomName: $scope.scheduleData.roomName,
                                    startTime: $scope.scheduleData.startTime,
                                }
                            })
                               .then(function (response) {
                                   console.log("ticket data after update MakeOrder");
                                   $scope.ticketData = response.data;
                               });
                            $('#confirmTicketModal').modal('hide');
                            $('#backdropModal').modal();
                            $("#backdropMessage").html("Đặt vé thành công, bấm nút bên dưới để tiếp tục!");
                        });
                    } else {
                        $('#confirmTicketModal').modal('hide');
                        $('#backdropModal').modal();
                        $("#backdropMessage").html("Đã hết thời gian đặt vé, Đặt vé thất bại, bấm nút bên dưới để tiếp tục!");

                    };
                });
            },
            onError: function (err) {
                console.log("some error occur");
                console.log(err);
            }
        }, '#paypal-button-container');
    };
    $scope.BackToChooseTicket = function () {
        var param1 = "<input type='hidden' name='scheduleId' value='" + $scope.scheduleId + "' />";
        document.getElementById('BackToChooseTicketForm').innerHTML = param1;
        document.getElementById('BackToChooseTicketForm').submit();
    }
    $scope.IsSeatStillAvailable = function () {
        for (var i = 0 ; i < $scope.ticketData.length; i++) {
            var aTicket = $scope.ticketData[i];
            if (aTicket.ticketStatus == "buying") {
                return false;//not available
            }
        }
        return true;//still available
    }
    $scope.getSeatClassName = function (seatStatus) {
        if (seatStatus == "available") {
            return "";
        }
    };
    $scope.openChooseTicket = function () {
        document.getElementById("chooseSeatAreaContent").style.display = "none";
        document.getElementById("checkoutSidebar").style.display = "none";
        document.getElementById("chooseTicketContent").style.display = "block";
        document.getElementById("filmInforSidebar").style.display = "block";
        document.getElementById("chooseTicketContent").style.marginLeft = "25%";
        document.getElementById("topbar").style.marginLeft = "25%";
        document.getElementById("filmInforSidebar").style.width = "25%";
        document.getElementById("chooseTicketContent").style.marginRight = "0%";
        document.getElementById("topbar").style.marginRight = "0%";
        $("#chooseTicketStep").css({ 'color': 'red' });
        $("#chooseSeatStep").css({ 'color': 'black' });
    },
    $scope.goHome = function () {
        document.getElementById('gotoHomeForm').submit();
    };
    /*================ Login  Part*/
    $scope.showLogin = function () {
        $("#myModalLogin").modal();
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
    $scope.logout = function () {
        LocalStorageManager.removeDataFromStorage($scope.userKey);
        $scope.userData = "";
    }
};
myApp.controller("chooseTicketController", chooseTicketController);

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
    },

}

function validateEmail(inputemail) {
    var regEmail = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return inputemail.match(regEmail);
}
function validatePhone(inputphone) {
    return inputphone.match(/\d/g).length === 10;
}
