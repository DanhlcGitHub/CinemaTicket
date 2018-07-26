$(document).ready(function () {
    
});

var logout = function () {
   /* $http({
        method: "POST",
        url: "/Utility/logout",
    })
   .then(function (response) {
       if (response.data.status == "ok") {
           document.getElementById("gohomeForm").submit();
       }
   });
   */
    $.ajax({
        type: 'POST',
        url: "/home/logout",
        success: function (response) {
            if (response.status == "ok") {
                document.getElementById("gohomeForm").submit();
            }
        }
    });
};