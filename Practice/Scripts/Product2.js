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

    $scope.loadProducts = function () {
        var req = {
            url: $scope.getAllProductUrl,
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


        //$scope.loadPage();
    };

    $scope.convertToDate = function (stringDate) {
        var dateOut = new Date(stringDate);
        dateOut.setDate(dateOut.getDate());
        return dateOut;
    };
});