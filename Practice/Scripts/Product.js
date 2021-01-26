var app = angular.module('myApp', []);

app.controller('viewctrl', function ($scope) {
    $scope.users = function () {
        //$location.path('/somenewpath');
        //$location.replace();
        $('#products').hide();
        $('#users').show();
    }
    $scope.products = function () {
        //$location.path('/somenewpathtwo');ss
        //$location.replace();
        $('#products').show();
        $('#users').hide();
    }
})

app.controller('productListCtrl', function ($scope, $http) {
    //$('#users').hide();
    alert('hi');
    $scope.totalShow = 10;
    $scope.pageNo = 1;



    $scope.search = function () {
        var value = $scope.value.toLowerCase();
        $("#products tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
        });
    }

    $scope.loadProducts = function () {
        var req = {
            url: "/Home/GetProductsList",
            method: "POST",
            header: "Content-Type:application/json",
            data: {
                totalShow: $scope.totalShow,
                pageNo: $scope.pageNo
            }
        };
        $http(req)
            .then(function (response) {
                if (response.status == 200) {
                    $scope.products = JSON.parse(response.data);
                }
            });
    }
    $('document').ready(function () {
        $scope.loadProducts();
    })
    $scope.openModal = function () {
        $("#editmodal").modal("show");
        $scope.data = null;
        $scope.show = "Create";
        $scope.button = "success";
        $scope.f = "createProduct";
        $scope.msg = "Create New Product";
    }

    $scope.editModal = function (product) {
        $scope.product = product;
        var req = {
            url: "/Home/GetProductInfoById",
            method: "POST",
            header: "Content-Type:application/json",
            data: {
                id: product.productId
            }
        };
        $http(req)
            .then(function (response) {
                if (response.status == 200) {
                    if (response.data) {
                        var data = JSON.parse(response.data);
                        $scope.data = data;
                        $scope.show = "Update";
                        $scope.button = "primary";
                        $scope.f = "updateProduct";
                        $scope.msg = "Update Product";
                        $("#editmodal").modal("show");
                    }
                    else {
                        alert("Something Went Wrong!!!");
                    }
                }
            });
    }

    $scope.deleteModal = function (product) {
        $("#deleteModal").modal("show");
        $scope.product = product;
    }

    $scope.deleteProduct = function (pId) {
        var req = {
            url: "/Home/DeleteProductById",
            method: "POST",
            header: "Content-Type:application/json",
            data: {
                id: $scope.product.productId
            }
        };
        $http(req)
            .then(function (response) {
                if (response.status == 200) {
                    if (response.data) {
                        var index = $scope.products.indexOf($scope.product);
                        $scope.products.splice(index, 1);
                        //delete $scope.products[index];
                        //console.log($scope.products);
                        $("#deleteModal").modal("hide");
                        //window.location.href = "/home";
                    }
                    else {

                    }
                }
            });
    }

    $scope.addOrUpdate = function () {
        if ($scope.f === "createProduct") {
            var req = {
                url: "/Home/CreateNewProduct",
                method: "POST",
                header: "Content-Type:application/json",
                data: {
                    userId: id,
                    productName: $scope.data.productName,
                    quantity: $scope.data.quantity,
                    price: $scope.data.price
                }
            };
            $http(req)
                .then(function (response) {
                    if (response.status == 200) {
                        if (response.data) {
                            $("#editmodal").modal("hide");
                            $scope.products.push(JSON.parse(response.data));
                            //window.location.href = "/home";
                        }
                        else {

                        }
                    }
                });
        }
        else {
            var req = {
                url: "/Home/UpdateProduct",
                method: "POST",
                header: "Content-Type:application/json",
                data: {
                    productId: $scope.product.productId,
                    userId: id,
                    productName: $scope.data.productName,
                    quantity: $scope.data.quantity,
                    price: $scope.data.price
                }
            };
            $http(req)
                .then(function (response) {
                    if (response.status == 200) {
                        if (response.data) {
                            //console.log(response.data);
                            var index = $scope.products.indexOf($scope.product);
                            $scope.products[index] = JSON.parse(response.data);
                            $("#editmodal").modal("hide");
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