//var app = angular.module('myApp', []);
app.controller('userListCtrl', function ($scope, $http) {
    $scope.search = function () {
        var value = $scope.value.toLowerCase();
        $("#users tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
        });
    }
    $scope.show = "Add New User";
    $scope.button = "success";
    $scope.f = "createUser";
    $scope.loadUsers = function () {
        var req = {
            url: "/User/GetUsersList",
            method: "POST",
            header: "Content-Type:application/json",
        };
        $http(req)
            .then(function (response) {
                if (response.status == 200) {
                    $scope.users = JSON.parse(response.data);
                }
            });
    }
    $scope.loadUsers();

    $scope.addNewUser = function () {
        $('#userName').focus();
        $scope.data = null;
        $scope.show = "Add New User";
        $scope.button = "success";
        $scope.f = "createUser";
    }

    $scope.editModal = function (user) {
        $scope.user = user;
        var req = {
            url: "/User/GetUserInfoById",
            method: "POST",
            header: "Content-Type:application/json",
            data: {
                id: user.userId
            }
        };
        $http(req)
            .then(function (response) {
                if (response.status == 200) {
                    if (response.data) {
                        var data = JSON.parse(response.data);
                        $scope.data = data;
                        $scope.show = "Update User";
                        $scope.button = "primary";
                        $scope.f = "updateUser";
                        $('#userName').focus();
                    }
                    else {
                        alert("Something Went Wrong!!!");
                    }
                }
            });
    }

    $scope.deleteModal = function (user) {
        $("#deleteUserModal").modal("show");
        $scope.user = user;
    }

    $scope.deleteUser = function (pId) {
        var req = {
            url: "/User/DeleteUserById",
            method: "POST",
            header: "Content-Type:application/json",
            data: {
                id: $scope.user.userId
            }
        };
        $http(req)
            .then(function (response) {
                if (response.status == 200) {
                    if (response.data) {
                        var index = $scope.users.indexOf($scope.user);
                        $scope.users.splice(index, 1);
                        //delete $scope.products[index];
                        //console.log($scope.products);
                        $("#deleteUserModal").modal("hide");
                        //window.location.href = "/home";
                    }
                    else {

                    }
                }
            });
    }

    $scope.addOrUpdate = function () {
        if ($scope.f === "createUser") {
            var req = {
                url: "/User/CreateNewUser",
                method: "POST",
                header: "Content-Type:application/json",
                data: {
                    userName: $scope.data.userName,
                    password: $scope.data.password
                }
            };
            $http(req)
                .then(function (response) {
                    if (response.status == 200) {
                        if (response.data) {
                            //console.log(response.data);
                            $scope.users.push(JSON.parse(response.data));
                            $scope.data = null;
                            //window.location.href = "/home";
                        }
                        else {

                        }
                    }
                });
        }
        else {
            var req = {
                url: "/User/UpdateUser",
                method: "POST",
                header: "Content-Type:application/json",
                data: {
                    userId: $scope.user.userId,
                    userName: $scope.data.userName,
                    password: $scope.data.password
                }
            };
            //console.log($scope.user);
            $http(req)
                .then(function (response) {
                    if (response.status == 200) {
                        if (response.data) {
                            //console.log(response.data);
                            var index = $scope.users.indexOf($scope.user);
                            $scope.users[index] = JSON.parse(response.data);
                            $scope.addNewUser();
                            //window.location.href = "/home";
                        }
                        else {

                        }
                    }
                });
        }
    }

    $scope.convertToDate = function (stringDate) {
        var dateOut = new Date(stringDate);
        dateOut.setDate(dateOut.getDate());
        return dateOut;
    };
});