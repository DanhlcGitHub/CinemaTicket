var partnerRoomModule = angular.module("partnerRoomModule", []);
var roomController = function ($scope, $http) {
    /*====================== receive event area ====================*/
    $scope.$on('viewRoomEvent', function (event, infor) {
        console.log(infor);
        $scope.currentCinema = infor.currentCinema;
        $scope.viewRoom(infor.currentRoom);
    });

    $scope.$on('addRoomEvent', function (event, infor) {
        $scope.currentCinema = infor.currentCinema;
    });

    $("#seatForm").submit(function (e) {
        e.preventDefault(e);
        $scope.initAddRoom();
    });
    /*====================== seat area ====================*/
    $scope.matrix = []; //
    $scope.seatData;
    $scope.currentCapacity = 0;
    $scope.currentCinema;
    $scope.isAddRoom;
    $scope.currentRoom;
    $scope.matrixX = 0;
    $scope.matrixY = 0;
    $scope.maxAdditionSize = 3;
    $scope.currentMode = "multipleSelectMode";//singleSelectMode; multipleSelectMode; multipleDeleteMode
    $scope.initAddRoomData = function () {
        $scope.baseSizeX = 0;
        $scope.baseSizeY = 0;
        $scope.maxSizeX = 0;
        $scope.maxSizeY = 0;
        $scope.currentSizeX = 0;
        $scope.currentSizeY = 0;
        $scope.startPoint = {};
        $scope.endPoint = {};
        $scope.countClick = 0;
    };
    $scope.initAddRoomData();
    $scope.selectGroupMode = true;

    console.log("room running now");

    $scope.alpha = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U"];

    $http({
        method: "POST",
        url: "/Partner/GetTypeOfSeat",
        params: { groupIdStr: $("#groupId").val(), }
    })
    .then(function (response) {
        $scope.typeOfSeatList = response.data;
        console.log($scope.typeOfSeatList);
    });

    $scope.viewRoom = function (currentRoom) {
        $scope.isAddRoom = false;
        $scope.currentRoom = currentRoom;
        //$scope.$apply();
        var roomId = $scope.currentRoom.id;
        console.log(roomId);
        $http({
            method: "POST",
            url: "/Partner/FindAllSeatByRoomId",
            params: { roomIdStr: roomId }
        })
        .then(function (response) {
            $scope.seatData = response.data;//seat data
            $scope.matrixX = $scope.currentRoom.matrixSizeX;
            $scope.matrixY = $scope.currentRoom.matrixSizeY;
            $scope.showRoomData($scope.matrixX, $scope.matrixY);
            $('#viewSeatModal').modal();
        });

        
    };

    $scope.showRoomData = function (matrixX, matrixY) {
        $scope.currentMode = "null";
        var seatClass = "btn-seat";
        if ($scope.isAddRoom == false) seatClass = "btn-seat-disable";
        $scope.initMatrixData(matrixX, matrixX);
        for (var i = 0; i < $scope.seatData.length; i++) {
            $scope.seatData[i].seatClass = seatClass;
        }
        $scope.fillDataToMatrixForView();
    };

    $scope.initMatrixData = function (matrixX, matrixY) {
        var hiddenClass = "btn-seat-hidden";
        if ($scope.isAddRoom == false) hiddenClass = "btn-seat-hidden-true";
        $scope.matrix = [];
        for (var i = 0 ; i < matrixY ; i++) {
            var rowObj = {
                py: i,
                //alphaName: $scope.alpha[i],
                alphaName: "#",
                seats: [],
            };
            $scope.matrix[i] = rowObj;
        }
        //init null seat
        for (var i = 0 ; i < matrixY ; i++) {
            for (var j = 0 ; j < matrixX ; j++) {
                var seatObj = {
                    className: hiddenClass,
                    px: j,
                    py: i,
                    locationX: j
                }
                $scope.matrix[i].seats[j] = seatObj;
            }
        }
    };

    $scope.initAddRoom = function () {
        $scope.isAddRoom = true;
        $scope.currentMode = "multipleSelectMode";
        $scope.matrix = [];
        $scope.seatData = [];
        $scope.initAddRoomData();

        $scope.currentCapacity = $("#inputCapacity").val();
        console.log($scope.currentCapacity);
        $scope.baseSizeY = Math.ceil(Math.sqrt($scope.currentCapacity));
        $scope.baseSizeX = Math.ceil($scope.currentCapacity / $scope.baseSizeY);
        $scope.maxSizeX = $scope.baseSizeX + $scope.maxAdditionSize;
        $scope.maxSizeY = $scope.baseSizeY + $scope.maxAdditionSize;
        $scope.currentSizeX = $scope.baseSizeX;
        $scope.currentSizeY = $scope.baseSizeY;
        $scope.initMatrixData($scope.currentSizeX, $scope.currentSizeY);
        $scope.$apply();
        $("#seatAreaId").show();
    };

    $scope.fillDataToMatrix = function () {
        for (var i = 0; i < $scope.seatData.length; i++) {
            var seatObj = {
                className: $scope.seatData[i].seatClass,
                px: parseInt($scope.seatData[i].px),
                locationX: parseInt($scope.seatData[i].px),
                py: $scope.seatData[i].py,
            }
            $scope.matrix[seatObj.py].seats[seatObj.px] = seatObj;
        }
    }

    $scope.fillDataToMatrixForView = function () {
        for (var i = 0; i < $scope.seatData.length; i++) {
            var seatObj = {
                className: $scope.seatData[i].seatClass,
                px: parseInt($scope.seatData[i].px),
                locationX: parseInt($scope.seatData[i].locationX),
                py: $scope.seatData[i].py,
                locationY: parseInt($scope.seatData[i].locationY),
            }
            $scope.matrix[seatObj.py].alphaName = $scope.alpha[seatObj.locationY - 1];//locationY -1
            $scope.matrix[seatObj.py].seats[seatObj.px] = seatObj;
        }
    }

    $scope.addSeatToList = function (x, y) {
        var item = $scope.findSeat(x, y);
        if (item == null) {
            var obj = {};
            obj.seatClass = "btn-seat";
            obj.px = x;
            obj.py = y;
            $scope.seatData.push(obj);
        }
    };

    $scope.removeSeatToList = function (item) {
        $scope.seatData = $.grep($scope.seatData, function (o) {
            return !(o.px == item.px && o.py == item.py);
        });
    };

    $scope.addGroupSeat = function () {
        if ($scope.countClick % 2 == 0) {
            $scope.handleClickPoint();
            var startPoint = $scope.startPoint;
            var endPoint = $scope.endPoint;
            for (var y = startPoint.py; y <= endPoint.py; y++) {
                for (var x = startPoint.px; x <= endPoint.px; x++) {
                    var item = $scope.findSeat(x, y);
                    if (item == null && $scope.seatData.length < $scope.currentCapacity) {
                        $scope.addSeatToList(x, y);
                    }
                }
            }
        }
    };

    $scope.removeGroupSeat = function () {
        if ($scope.countClick % 2 == 0) {
            $scope.handleClickPoint();
            var startPoint = $scope.startPoint;
            var endPoint = $scope.endPoint;
            for (var y = startPoint.py; y <= endPoint.py; y++) {
                for (var x = startPoint.px; x <= endPoint.px; x++) {
                    var item = $scope.findSeat(x, y);
                    if (item != null) {
                        $scope.removeSeatToList(item);
                    }
                }
            }
        }
    };

    $scope.handleClickPoint = function () {
        var startPoint = $scope.startPoint;
        var endPoint = $scope.endPoint;
        if ((startPoint.px > endPoint.px) && (startPoint.py < endPoint.py)) {
            var temp = startPoint.px;
            startPoint.px = endPoint.px;
            endPoint.px = temp;
        }
        else if ((startPoint.py > endPoint.py) && (startPoint.px < endPoint.px)) {
            var temp = startPoint.py;
            startPoint.py = endPoint.py;
            endPoint.py = temp;
        } else if (((startPoint.px > endPoint.px) && (startPoint.py > endPoint.py)) ||
                     ((startPoint.px == endPoint.px) && (startPoint.py > endPoint.py)) ||
                        ((startPoint.px > endPoint.px) && (startPoint.py == endPoint.py))) {
            var temp = startPoint;
            startPoint = endPoint;
            endPoint = temp;
        }
        $scope.startPoint = startPoint;
        $scope.endPoint = endPoint;
    };

    $scope.findSeat = function (x, y) {
        for (var i = 0 ; i < $scope.seatData.length; i++) {
            var item = $scope.seatData[i];
            if (item.px == x && item.py == y) return item;
        }
        return null;
    };

    $scope.changeMode = function (mode) {
        $scope.countClick = 0;
        $scope.startPoint = {};
        $scope.endPoint = {};
        $scope.currentMode = mode;
    };

    $scope.setValuePoint = function (px, py) {
        if ($scope.countClick % 2 == 1) {
            $scope.startPoint.px = px;
            $scope.startPoint.py = py;
        } else if ($scope.countClick % 2 == 0) {
            $scope.endPoint.px = px;
            $scope.endPoint.py = py;
        }
    }

    $scope.singleSelect = function (px, py) {
        var item = $scope.findSeat(px, py);
        if (item != null) {
            $scope.removeSeatToList(item);
        } else {
            if($scope.seatData.length < $scope.currentCapacity){
                $scope.addSeatToList(px, py);
            } else {
                alert("can't add, full seat added");
            }
        }
    };

    $scope.onclickSeat = function (px, py) {//singleSelectMode; multipleSelectMode; multipleDeleteMode
        console.log($scope.currentMode);
        $scope.countClick++;
        $scope.setValuePoint(px, py);
        if ($scope.currentMode == "singleSelectMode") {
            $scope.singleSelect(px, py);
            $scope.initMatrixData($scope.currentSizeX, $scope.currentSizeY);
            $scope.fillDataToMatrix();
        } else if ($scope.currentMode == "multipleSelectMode") {
            $scope.addGroupSeat();
            $scope.initMatrixData($scope.currentSizeX, $scope.currentSizeY);
            $scope.fillDataToMatrix();
        } else if ($scope.currentMode == "multipleDeleteMode") {
            $scope.removeGroupSeat();
            $scope.initMatrixData($scope.currentSizeX, $scope.currentSizeY);
            $scope.fillDataToMatrix();
        }
    };

    $scope.addRow = function () {
        if ($scope.currentSizeY < $scope.maxSizeY) {
            $scope.currentSizeY++;
            $scope.initMatrixData($scope.currentSizeX, $scope.currentSizeY);
            $scope.fillDataToMatrix();
        } else {
            alert("Row limit to " + $scope.maxSizeY + " with capacity " + $scope.currentCapacity);
        }
    }

    $scope.addColumn = function () {
        if ($scope.currentSizeX < $scope.maxSizeX) {
            $scope.currentSizeX++;
            $scope.initMatrixData($scope.currentSizeX, $scope.currentSizeY);
            $scope.fillDataToMatrix();
        } else {
            alert("Column limit to " + $scope.maxSizeX + " with capacity " + $scope.currentCapacity);
        }
    }

    $scope.minusColumn = function () {
        if ($scope.currentSizeX > $scope.baseSizeX) {
            $scope.removeSeatInColumn($scope.currentSizeX);
            $scope.currentSizeX -= 1;
            $scope.initMatrixData($scope.currentSizeX, $scope.currentSizeY);
            $scope.fillDataToMatrix();
        } else {
            alert("Column must greater than " + $scope.baseSizeX + " with capacity " + $scope.currentCapacity);
        }
    };

    $scope.minusRow = function () {
        if ($scope.currentSizeY > $scope.baseSizeY) {
            $scope.removeSeatInRow($scope.currentSizeY);
            $scope.currentSizeY -= 1;
            $scope.initMatrixData($scope.currentSizeX, $scope.currentSizeY);
            $scope.fillDataToMatrix();
        } else {
            alert("Row must greater than " + $scope.baseSizeY + " with capacity " + $scope.currentCapacity);
        }
    };

    $scope.removeSeatInRow = function (y) {
        $scope.seatData = $.grep($scope.seatData, function (o) {
            return !(o.py == (y - 1));
        });
    };

    $scope.removeSeatInColumn = function (x) {
        $scope.seatData = $.grep($scope.seatData, function (o) {
            return !(o.px == (x - 1));
        });
    };

    $scope.manipulateLocationInfor = function () {
        console.log("==================manipulate===============");
        var locationY = 0;
        for (var y = 0 ; y <= $scope.currentSizeY; y++) {
            var row = $.grep($scope.seatData, function (o) {
                return (o.py == (y - 1));
            });
            row.sort(function (a, b) {
                if (a.px > b.px) { return 1 }
                if (a.px < b.px) { return -1 }
                return 0;
            });
            //delete this row
            $scope.removeSeatInRow(y);
            //add this row :v
            if (row.length > 0) {
                locationY++;
                for (var i = 0; i < row.length; i++) {
                    row[i].locationX = i + 1;
                    row[i].locationY = locationY;
                    row[i].typeOfSeatId = $scope.typeOfSeatList[0].id;
                    $scope.seatData.push(row[i]);
                }
            }
        }
        console.log($scope.seatData);
    }

    $scope.reviewSeat = function () {
        $scope.manipulateLocationInfor();
        $scope.initMatrixData($scope.currentSizeX, $scope.currentSizeY);
        for (var i = 0; i < $scope.seatData.length; i++) {
            var seatObj = {
                className: $scope.seatData[i].seatClass,
                px: parseInt($scope.seatData[i].px),
                py: $scope.seatData[i].py,
                locationX: $scope.seatData[i].locationX,
                locationY: $scope.seatData[i].locationY,
            }
            $scope.matrix[seatObj.py].alphaName = $scope.alpha[seatObj.locationY - 1];
            $scope.matrix[seatObj.py].seats[seatObj.px] = seatObj;
        }
    }

    $scope.save = function () {
        console.log("save");
        if ($scope.currentCapacity != $scope.seatData.length) {
            alert("not select enought seat");
        } else {
            $scope.saveRoomToDB();
        }
    }

    $scope.saveRoomToDB = function () {
        $scope.manipulateLocationInfor();
        console.log($scope.currentCinema);
        var dataStr = {
            roomData: {
                cinemaId: $scope.currentCinema.cinemaId,
                name: $("#inputRoomName").val(),
                digType: 1,
                matrixSizeX: $scope.currentSizeX,
                matrixSizeY: $scope.currentSizeY,
                capacity: $scope.currentCapacity
            },
            seatData: $scope.seatData
        }

        $.ajax({
            type: 'POST',
            url: "/Partner/SaveRoom",
            data: { roomInfor: JSON.stringify(dataStr) },
            success: function () {
                $scope.$emit('roomCallRefreshEvent');
                alert("success!");
            }
        });
    };
}

partnerRoomModule.controller("roomController", roomController);