var myApp = angular.module("seatModule", []);
var seatController = function ($scope, $http) {
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
    $scope.countDown = 60;
    $scope.exchangeRate = 23000;

    $scope.alpha = ["A", "B", "C", "D", "E", "F", "J", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U"];
    $scope.TicketStatusEnum = Object.freeze({ "available": "btn-seat", "buyed": "btn-seat-buyed", "buying": "btn-seat-buyed" });
    // init data

    $scope.totalAmount = LocalStorageManager.loadDataFromStorage($scope.totalAmountKey);
    $scope.quantityData = LocalStorageManager.loadDataFromStorage($scope.quantityDataKey);
    for (var i = 0; i < $scope.quantityData.length; i++) {
        $scope.totalTicket += $scope.quantityData[i].userChoose;
    }
    console.log("init");
    console.log($scope.totalAmount);
    $scope.totalAmoutUSD = Math.round((parseFloat($scope.totalAmount) / $scope.exchangeRate) * 100) / 100;
    console.log($scope.quantityData);
    // beforeunload---------------------
    window.addEventListener('beforeunload', function (e) {
        /* $http({
             method: "POST",
             url: "/Ticket/ChangeTicketStatus",
             params: { ticketList: $("#scheduleId").val() }
         })
         .then(function (response) {
             $scope.scheduleData = response.data;
         });*/
        alert("Before unload");
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
            if ($scope.middeSeatFlag == true) alert("khong the de trong ghe o giua");
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
            //console.log("x: " + $scope.data[i].px + " y: " + $scope.data[i].py);
            //console.log("-----------------------------------------------------" + i);
            var seatObj = {
                isNull: false,
                className: $scope.TicketStatusEnum[$scope.seatData[i].seatStatus],
                isChoosing: false,
                seatId: $scope.seatData[i].id,
                seatStatus: $scope.seatData[i].seatStatus,
                px: parseInt($scope.seatData[i].px),
                py: $scope.seatData[i].py
            }

            $scope.matrix[seatObj.py].seats[seatObj.px] = seatObj;
        }
    };
    $scope.countController = function () {
        var timer = setInterval(function () {
            if ($scope.countDown == 0) {
                clearInterval($scope.countController);
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
                alert("Bạn không được để trống ghế ở giữa");
            } else {
                $scope.email = $("#emailId").val();
                $scope.phone = $("#phoneId").val();
                if ($scope.email == "" || $scope.phone == "") {
                    alert("Thông tin E-mail và Điện thoại không được bỏ trống!");
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
                                alert("Loại vé bạn chọn đã hết hoặc không đủ số lượng ghế trống!");
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
                        alert("Email và phone sai format!");
                    }
                }
            }
        } else {
            alert("Bạn chưa chọn đủ " + $scope.totalTicket + " vé!");
        }
    };
    $scope.openConfirmDialog = function () {
        var modal = document.getElementById('myModal');
        modal.style.display = "block";
        var span = document.getElementsByClassName("close")[0];

        // When the user clicks on <span> (x), close the modal
        span.onclick = function () {

            if ($scope.countDown === 0) {//time out
                //$scope.BackToChooseTicket();
                //change status from buying to available
                $http({
                    method: "POST",
                    url: "/Ticket/BuyingToAvailable",
                    params: { scheduleId: $("#scheduleId").val() }
                })
                .then(function (response) {
                    $scope.scheduleData = response.data;
                });
                // reload page
            } else {
                modal.style.display = "none";
            }
        }

        // When the user clicks anywhere outside of the modal, close it
        window.onclick = function (event) {
            if (event.target == modal) {

            }
        }
    };
    $scope.openSuccessDialog = function () {
        var sucessDialog = document.getElementById('successDialog');
        sucessDialog.style.display = "block";
    };
    $scope.renderPaypalButton = function () {
        paypal.Button.render({

            env: 'sandbox', // sandbox | production

            client: {
                sandbox: 'AWT16aVyr2SNJR2uBG46HHvz-DIY98lIP3aPAO-sUs36sOvtN9Ay3H0z-4e8cj4qZyNR5Aj3qrsxw0W3',
                production: 'AXWr3Vji-q_UZFmrhTIxcSMBctnfsofDOxwsi3_llRLpDzwJ83NtZzt7wT5Sg_eB916xA5eC6c7O1APa'
            },
            commit: false, // Show a 'Pay Now' button
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
                                    phone: $scope.phone
                                }
                            })
                               .then(function (response) {
                                   console.log("ticket data after update MakeOrder");
                                   $scope.ticketData = response.data;
                               });
                            var modal = document.getElementById('myModal');
                            modal.style.display = "none";
                            $scope.openSuccessDialog();
                        });
                    } else {
                        alert("Time out"); //$scope.BackToChooseTicket();
                    };
                });
            },
            onError: function (err) {
                console.log("some error occur");
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
    $scope.goHome = function () {
        document.getElementById('gotoHomeForm').submit();
    };
}

myApp.controller("seatController", seatController);

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
