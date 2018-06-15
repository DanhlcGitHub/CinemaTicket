var myApp = angular.module("seatModule", []);
var seatController = function ($scope, $http) {
    $scope.quantityDataKey = "quantityDataKey";
    $scope.totalAmountKey = "totalAmoutKey";
    $scope.totalAmount = 0;
    $scope.quantityData;
    $scope.orderData; // contain scheduleId, seatId List
    $scope.matrix = []; // 
    $scope.matrixX = 0;
    $scope.matrixY = 0;
    $scope.totalTicket = 0;
    $scope.countClick = 0;
    $scope.choosedList = [];
    $scope.middeSeatFlag = false;
    $scope.alpha = ["A", "B", "C", "D", "E", "F", "J", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U"];

    // init data
    $scope.totalAmount = LocalStorageManager.loadDataFromStorage($scope.totalAmountKey);
    $scope.quantityData = LocalStorageManager.loadDataFromStorage($scope.quantityDataKey);
    for (var i = 0; i < $scope.quantityData.length; i++) {
        $scope.totalTicket += $scope.quantityData[i].userChoose;
    }
    console.log("init");
    console.log($scope.totalAmount);
    console.log($scope.quantityData);
    // window click---------------------
    $http({
        method: "POST",
        url: "/Seat/FindAllSeatByScheduleId",
        params: { scheduleId: $("#scheduleId").val() }
    })
    .then(function (response) {
        $scope.data = response.data.seats;
        console.log("data receive");
        console.log(response);
        $scope.matrixX = response.data.matrixX;
        $scope.matrixY = response.data.matrixY;
        $scope.displayData($scope.matrixX, $scope.matrixY);
        console.log("after display");
        console.log($scope.matrix);
    });

    $http({
        method: "POST",
        url: "/ChooseTicket/GetChooseTicketData",
        params: { scheduleId: $("#scheduleId").val() }
    })
    .then(function (response) {
        $scope.scheduleData = response.data;
    });
    $scope.onclickSeat = function (seatId, px, py) {
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
        $scope.middeSeatFlag  = $scope.validMiddleSeat();
        if ($scope.middeSeatFlag == true) alert("khong the de trong ghe o giua");
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
        for (var i = 0; i < $scope.data.length; i++) {
            //console.log("x: " + $scope.data[i].px + " y: " + $scope.data[i].py);
            //console.log("-----------------------------------------------------" + i);
            var seatObj = {
                isNull: false,
                className: 'btn-seat',
                isChoosing: false,
                seatId: $scope.data[i].id,
                px: parseInt($scope.data[i].px),
                py: $scope.data[i].py
            }

            $scope.matrix[seatObj.py].seats[seatObj.px] = seatObj;
        }
    };
    $scope.checkout = function () {
        // check condition
        if ($scope.choosedList.length == $scope.totalTicket) {
            if ($scope.middeSeatFlag == true) {
                alert("Bạn không được để trống ghế ở giữa");
            } else {
                if ($("#emailId").val() == "" || $("#phoneId").val() == "") {
                    alert("Thông tin E-mail và Điện thoại không được bỏ trống!");
                } else {
                    // check format email and phone
                    if (validateEmail($("#emailId").val()) && validatePhone($("#phoneId").val())) {
                        console.log("success!");
                        // gửi qua bên kia list choosed list
                        // gửi qua bên kia schedule; mỗi scheduleId + 1 seat -> 1 order detail duy nhất
                    } else {
                        alert("Email và phone sai format!");
                    }
                }
            }
        } else {
            alert("Bạn chưa chọn đủ " + $scope.totalTicket + " vé!");
        }
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
