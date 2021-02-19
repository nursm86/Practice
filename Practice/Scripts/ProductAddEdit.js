var app = angular.module('myApp', []);

app.controller('productInfo', function ($scope, $http) {

    $scope.getProductById = function () {
        var req = {
            url: $scope.getProductByIdUrl,
            method: "POST",
            header: "Content-Type:application/json",
            data: {
                id: $scope.productId
            }
        };
        $http(req)
            .then(function (response) {
                if (response.status == 200) {
                    if (response.data) {
                        //console.log(data);
                        var data = JSON.parse(response.data);
                        $scope.data = data;
                    }
                    else {
                        alert("Something Went Wrong!!!");
                    }
                }
            });
    }

    $scope.loadProductExtraData = function () {
        var req = {
            url: $scope.getProductDataExtraUrl,
            method: "POST",
            header: "Content-Type:application/json",
        };
        $http(req)
            .then(function (response) {
                if (response.status == 200) {
                    if (response.data) {
                        console.log(response.data.dealers);
                        $scope.categories = JSON.parse(response.data[0].Value);
                        $scope.dealers = JSON.parse(response.data[1].Value);
                    }
                    else {
                        alert("Something Went Wrong!!!");
                    }
                }
            });
    }

    $scope.saveProduct = function () {
        $scope.data.productId = $scope.productId;
        $scope.data.userId = id;
        var req = {
            url: $scope.saveProductUrl,
            method: "POST",
            header: "Content-Type:application/json",
            data: {
                p : $scope.data
            }
        };
        $http(req)
            .then(function (response) {
                if (response.status == 200) {
                    var data = JSON.parse(response.data);
                    if (data.errors == "") {
                        window.location.href = window.location.href = $scope.productListUrl;
                    } else {
                        $scope.errors = data.errors;
                    }
                }
            });
    }

    $scope.Init = function (
        productId,
        getProductByIdUrl
        , deleteProductByIdUrl
        , getAllProductUrl,
        saveProductUrl,
        getProductDataExtraUrl,
        productListUrl
    ) {
        $scope.productId = productId;
        $scope.getProductByIdUrl = getProductByIdUrl;
        $scope.deleteProductByIdUrl = deleteProductByIdUrl;
        $scope.productListUrl = productListUrl;
        /*bind extra url if need*/
        $scope.getAllProductUrl = getAllProductUrl;
        $scope.saveProductUrl = saveProductUrl;
        $scope.getProductDataExtraUrl = getProductDataExtraUrl;
        $scope.loadProductExtraData();
        if ($scope.productId == 0) {
            $scope.show = "Create";
            $scope.button = "success";
            $scope.msg = "Add new Product";
        }
        else {
            $scope.show = "Update";
            $scope.button = "primary";
            $scope.msg = "Update Product";
            $scope.getProductById();
        }
    };

    $scope.convertToDate = function (stringDate) {
        var dateOut = new Date(stringDate);
        dateOut.setDate(dateOut.getDate());
        return dateOut;
    };
});