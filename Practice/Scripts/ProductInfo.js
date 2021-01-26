var app = angular.module('myApp', []);

app.controller('productInfo', function ($scope, $http) {

    if (paramId == 0) {
        $scope.show = "Create";
        $scope.button = "success";
        $scope.msg = "Add new Product";
    }
    else {
        $scope.show = "Update";
        $scope.button = "primary";
        $scope.msg = "Update Product";
    }

    $scope.productInfo = function () {
        var req = {
            url: $scope.getProductByIdUrl,
            method: "POST",
            header: "Content-Type:application/json",
            data: {
                id: paramId
            }
        };
        $http(req)
            .then(function (response) {
                if (response.status == 200) {
                    if (response.data) {
                        var data = JSON.parse(response.data);
                        $scope.data = data;
                    }
                    else {
                        alert("Something Went Wrong!!!");
                    }
                }
            });
    }

    $scope.addOrUpdate = function () {
        if (paramId == 0) {
            var req = {
                url: $scope.createNewProductUrl,
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
                            
                            window.location.href = "/Product";
                        }
                        else {

                        }
                    }
                });
        }
        else {
            var req = {
                url: $scope.editProductUrl,
                method: "POST",
                header: "Content-Type:application/json",
                data: {
                    productId: paramId,
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
                            //var index = $scope.products.indexOf($scope.product);
                            //$scope.products[index] = JSON.parse(response.data);
                            //$("#editmodal").modal("hide");
                            window.location.href = "/product";
                        }
                        else {

                        }
                    }
                });
        }
    }

    $scope.Init = function (
        getProductByIdUrl
        , deleteProductByIdUrl
        , getAllProductUrl,
        createNewProductUrl,
        editProductUrl
    ) {
        $scope.getProductByIdUrl = getProductByIdUrl;
        $scope.deleteProductByIdUrl = deleteProductByIdUrl;
        /*bind extra url if need*/
        $scope.getAllProductUrl = getAllProductUrl;
        $scope.createNewProductUrl = createNewProductUrl;
        $scope.editProductUrl = editProductUrl;
        $scope.productInfo();
    };

    $scope.convertToDate = function (stringDate) {
        var dateOut = new Date(stringDate);
        dateOut.setDate(dateOut.getDate());
        return dateOut;
    };
});