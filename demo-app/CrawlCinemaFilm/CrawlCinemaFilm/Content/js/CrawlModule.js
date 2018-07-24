$(document).ready(function () {
});

var crawlModule = angular.module("crawlModule", []);
var crawlController = function ($scope, $http) {
    $scope.CrawlFilmData;
    $scope.currentMonth = null;
    $scope.addedFilmList = [];
    $scope.currentFilmList;
    $scope.currentYear = null;
    $scope.selectedMonth = null;
    $scope.showingText = "N/A";
    $("#crawlLoader").hide();
    $("#showFilmArea").hide();

    $scope.months = [{ num: "01", name: "1st | Januaray" }, { num: "02", name: "2nd | February" }, { num: "03", name: "3rd | Math" },
                     { num: "04", name: "4th | April" }, { num: "05", name: "5th | May" }, { num: "06", name: "6th | June" },
                        { num: "07", name: "7th | July" }, { num: "08", name: "8th | August" }, { num: "09", name: "9th | September" },
                        { num: "10", name: "10th | October" }, { num: "11", name: "11th | November" }, { num: "12", name: "12th | December" }];



    $http({
        method: "POST",
        url: "/Crawl/GetCurrentMonth",
    })
   .then(function (response) {
       $scope.currentMonth = response.data.month;
       $scope.currentYear = response.data.year;
       $scope.months = $.grep($scope.months, function (o) {
           return (o.num >= $scope.currentMonth);
       });
       var monthParam = $scope.currentYear + "-" + $scope.formattingMonth($scope.currentMonth);
       
       console.log(monthParam);
       $scope.crawlFilmData(monthParam);
       //$("#monthPicker select").val($scope.formattingMonth($scope.currentMonth));
   });

    $('#monthPicker').on('change', function (e) {
        var optionSelected = $("option:selected", this);
        var valueSelected = this.value;
        $scope.selectedMonth = valueSelected;
        var contentIdentity = "#monthPicker option[value='" + valueSelected + "']";
        $scope.showingText = $(contentIdentity).text();
        var monthParam = $scope.currentYear + "-" + $scope.selectedMonth;
        console.log(monthParam); 
        
        $scope.crawlFilmData(monthParam);
    });
   
    $scope.getCurrentFilmList = function () {
        $http({
            method: "POST",
            url: "/Crawl/LoadAvailableFilm",
        })
       .then(function (response) {
           $scope.currentFilmList = response.data;
       });
    };

    $scope.getCurrentFilmList();
    $scope.crawlFilmData = function (monthParam) {
        $("#showFilmArea").hide();
        $("#crawlLoader").show();
        $('#monthPicker').prop('disabled', 'disabled');
        $http({
            method: "POST",
            url: "/Crawl/CrawlFilmData",
            params: { monthParam: monthParam, }
        })
       .then(function (response) {
           $("#crawlLoader").hide();
           console.log(response.data);
           //$scope.CrawlFilmData = response.data;
           $scope.CrawlFilmData = $scope.filterCrawlList(response.data);
           $('#monthPicker').prop('disabled', false);
           $("#showFilmArea").show();

       });
    };

    $scope.addFilmToList = function (aFilm) {
        //check dupplicate
        if (!$scope.isDupplicateFilm(aFilm)) {
            $scope.addedFilmList.push(aFilm);

            //remove to $scope.CrawlFilmData 
            $scope.CrawlFilmData = $.grep($scope.CrawlFilmData, function (o) {
                return (o.name != aFilm.name);
            });
        } else {
            alert("System detect that this film already exist!");
        }
    };

    $scope.removeFilmToList = function (aFilm) {
        $scope.addedFilmList = $.grep($scope.addedFilmList, function (o) {
            return (o.name != aFilm.name);
        });
        if ($scope.addedFilmList.length == 0) {
            $("#addedFilmModal").modal('hide');
        }
        $scope.CrawlFilmData.push(aFilm);
    };

    $scope.isDupplicateFilm = function (aFilm) {
        //compare with addedlist
        /*for (var i = 0 ; i < $scope.addedFilmList.length; i++) {
            var filmItem = $scope.addedFilmList[i];
            if (filmItem.name == aFilm.name) return true;
        }*/
        
        for (var i = 0 ; i < $scope.currentFilmList.length; i++) {
            var filmItem = $scope.currentFilmList[i];
            if (filmItem.name == aFilm.name) return true;
        }
        //compare with currentlist

        return false;
    };

    $scope.filterCrawlList = function (dataList) {
        for (var i = dataList.length- 1 ; i >=0; i--) {
            var item = dataList[i];
            if ($scope.isDupplicateFilm(item)) {
                dataList.splice(i, 1);
            }
        }
        return dataList;
    };

    $scope.saveFilmToDB = function () {
        $.ajax({
            type: 'POST',
            url: "/Crawl/SaveFilmToDB",
            data: { addedFilmList: JSON.stringify($scope.addedFilmList) },
            success: function (response) {
                $scope.addedFilmList = [];
                $scope.getCurrentFilmList();
                $scope.cancelFilmList = response;
                console.log($scope.cancelFilmList);
                if ($scope.cancelFilmList.length != 0) {
                    $("#addedFilmModal").modal('hide');
                    $("#cancelFilmModal").modal();
                }
                else {
                    alert("All Item was added Success!");
                }
                    
            }
        });
    }

    $scope.viewAddedList = function () {
        if ($scope.addedFilmList.length != 0)
            $("#addedFilmModal").modal();
        else
            alert("No item added!");
    }

    $scope.formattingMonth =  function (target) {
        return target < 10 ? '0' + target : target;
    }
}

crawlModule.controller("crawlController", crawlController);


