var app = angular.module('myApp', []);

app.controller('productListCtrl', function ($scope, $http) {
    $scope.totalShow = 10;
    $scope.pageNo = 1;



    $scope.search = function () {
        var value = $scope.value.toLowerCase();
        $("#products tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
        });
    }

    $scope.getAllProductList = function () {

        //$http.get($scope.getAllProductUrl, {
        //    params: {},
        //    success: function (result, status) {
        //        alert(status);
        //        if (result.status == 200) {
        //            $scope.products = JSON.parse(result.data);
        //        }
        //    },
        //    error: function (result, status) {
        //        console.log(result);
        //        alert("Something went Wrong!!!");
        //    }
        //});

        //$http.get($scope.getAllProductUrl, {
        //    params: {}
        //}).success(function (result, status) {
        //    console.log(result);
        //    $scope.products = JSON.parse(result);
        //}).error(function (result, status) {
        //    alert("Something Went Wrong");
        //});

        var req = {
            url: $scope.getAllProductUrl,
            method: "GET",
            header: "Content-Type:application/json",
        };
        $http(req)
            .then(function (response) {
                if (response.status == 200) {
                    if (response.data) {
                        $scope.products = JSON.parse(response.data);
                    }
                    else {
                        alert("something went Wrong!!!");
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
            url: $scope.deleteProductByIdUrl,
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

    $scope.Init = function (
        getProductByIdUrl
        , deleteProductByIdUrl
        , getAllProductUrl
    ) {
        $scope.getProductByIdUrl = getProductByIdUrl;
        $scope.deleteProductByIdUrl = deleteProductByIdUrl;
        /*bind extra url if need*/
        $scope.getAllProductUrl = getAllProductUrl;
        //$scope.saveCountryListUrl = saveCountryListUrl;
        //$scope.getCountryByIdUrl = getCountryByIdUrl;
        //$scope.getCountryDataExtraUrl = getCountryDataExtraUrl;
        //$scope.saveCountryUrl = saveCountryUrl;
        $scope.getAllProductList();

        //$scope.loadPage();
    };

    $scope.convertToDate = function (stringDate) {
        var dateOut = new Date(stringDate);
        dateOut.setDate(dateOut.getDate());
        return dateOut;
    };
});