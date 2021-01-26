var app = angular.module('myApp', []);

app.controller('userCtrl', function ($scope, $http) {
    $scope.login = function () {
        var req = {
            url: "/login/validate",
            method: "POST",
            header: "Content-Type:application/json",
            data: {
                userName: $scope.username,
                password: $scope.password
            }
        };
        $http(req)
            .then(function (response) {
                if (response.status == 200) {
                    if (response.data == -1) {
                        $scope.errmsg = "UserName or Password is Wrong!!!";
                        $scope.c = "red";
                    }
                    else {
                        Cookies.set("uname", $scope.username);
                        Cookies.set("pass", $scope.password);
                        Cookies.set("id", response.data);
                        window.location.href = '/product';
                    }
                }
            });
    }
});