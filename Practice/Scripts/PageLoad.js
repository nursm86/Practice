$('document').ready(function () {
    $.ajax({
        url: "/home/products", success: function (result) {
            //console.log(result);
            $("#products").html(result);
        }
    });
})
var app = angular.module('myApp', []);

app.controller('viewCtrl', function ($scope) {
    $scope.users = function () {
        //$location.path('/someNewPath');
        //$location.replace();
        $.ajax({
            url: "/user/index", success: function (result) {
                //console.log(result);
                $("#products").html(result);
            }
        });
        //$('#products').hide();
        //$('#users').show();
    }
    $scope.products = function () {
        //$location.path('/someNewPathtwo');
        //$location.replace();
        $.ajax({
            url: "/home/products", success: function (result) {
                //console.log(result);
                $("#products").html(result);
            }
        });
        //$('#products').show();
        //$('#users').hide();
    }
    $scope.users();
});
//

//app.controller('viewCtrl', function ($scope, $http) {

//    $scope.loadIndexPage = function () {
        
//        var req = {
//            url: "/Home/Products",
//            method: "GET"
//        };
//        $http(req)
//            .then(function (response) {
//                if (response.status == 200) {
//                    $scope.html = response.data;
//                }
//            });
//    }
//    $scope.loadIndexPage();
//});