
app.controller('leaveRecordsCtrl', function ($scope, $http) {
    $scope.isDataAvailable = false;
    $scope.noDataAvailable2 = true;
    $scope.noButton = true;
    $scope.button = "primary";
    $scope.msg = "Edit Leave Allocation";

    $scope.deleteModal = function(id) {
        $("#deleteModalForAllocation").modal("show");
        $scope.delId = id;
    }

    $scope.deleteModalForApplication = function(id) {
        $("#deleteModalForApplication").modal("show");
        $scope.delId = id;
    }

    $scope.deleteLeaveApplicationById = function() {
        $http.get($scope.getDeleteEmpLeaveAllocationByIdUrl,{
            params: { "id": $scope.delId }
        }).success(function (result, status) {
            var data = JSON.parse(result);
            $("#deleteModalForApplication").modal("hide");
            $scope.loadData();
            $scope.getAllEmpLeaveApplicationById();
            //$scope.SalaryRecord.EffectiveDate = new Date($scope.SalaryRecord.EffectiveDate).toISOString().slice(0, 10);;
        }).error(function (result, status) {
            console.log(result);
            alert("Something Went Wrong!!!");
        });
    }
    $scope.deleteLeaveAllocationById = function() {
        $http.get($scope.getDeleteEmpLeaveRecordByIdUrl, {
            params: {
                "id": $scope.delId,
                "employeeId" : $scope.id
            }
        }).success(function (result, status) {
            var data = JSON.parse(result);
            $("#deleteModalForAllocation").modal("hide");
            $scope.loadData();
            $scope.getAllEmpLeaveApplicationById();
            //$scope.SalaryRecord.EffectiveDate = new Date($scope.SalaryRecord.EffectiveDate).toISOString().slice(0, 10);;
        }).error(function (result, status) {
            console.log(result);
            alert("Something Went Wrong!!!");
        });
    }

    $scope.getLeaveRecord = function (id) {
        $http.get($scope.getEmpLeaveRecordByIdUrl, {
            params: { "id": id }
        }).success(function (result, status) {
            var data = JSON.parse(result);
            $("#editModalForAllocation").modal("show");
            $scope.data= data;
            $scope.button = "primary";
            $scope.show = "Edit Leave Record";
            //$scope.SalaryRecord.EffectiveDate = new Date($scope.SalaryRecord.EffectiveDate).toISOString().slice(0, 10);;
        }).error(function (result, status) {
            console.log(result);
            alert("Something Went Wrong!!!");
        });
    }

    $scope.addNewLeaveRecord = function () {
        $scope.data = null;
        $scope.leaveRecordId = 0;
        $("#editModalForAllocation").modal("show");
        $scope.button = "success";
        $scope.show = "Add Leave Allocation";
        $scope.msg = "Add New Leave Allocation";
    }

    $scope.addNewLeaveApplication = function () {
        if ($scope.allocationId != undefined) {
            $scope.data = null;
            $scope.leaveApplicationId = 0;
            $("#editModalForApplication").modal("show");
            $scope.button = "success";
            $scope.show = "Add Leave Application";
            $scope.msg = "Add New Leave Allocation";
        }
        else {
            alert("Please Select a Leave Allocation first");
        }
    }

    $scope.updateTotal = function () {
        $scope.data.TotalLeave = $scope.data.OtherLeave +
            $scope.data.SickLeave +
            $scope.data.VacationLeave;
    }

    $scope.loadData = function () {
        //$scope.getSalaryRecordByUserId();
        $scope.getAllEmpLeaveAllocationById();
    }

    $scope.getAllEmpLeaveAllocationById = function () {
        $http.get($scope.getAllEmpLeaveAllocationByIdUrl, {
            params: { "id": $scope.id }
        }).success(function (result, status) {
            var data = JSON.parse(result);
            //console.log(result);
            if (data.length > 0) {
                if (data.length == 1) {
                    $scope.isOne = false;
                } else {
                    $scope.isOne = true;
                }
                $scope.isDataAvailable = true;
                $scope.LeaveRecords = data;
            } else {
                $scope.errormsg = "";
                $scope.isData = false;
                $scope.isDataAvailable = true;
                $scope.noDataAvailable = true;
            }
        }).error(function (result, status) {
            console.log(result);
            alert("Something Went Wrong!!!");
        });
    }

    $scope.getLeaveApplicationById = function (id) {
        $scope.leaveApplicationId = id;
        $http.get($scope.getLeaveApplicationByIdUrl, {
            params: { "id": id }
        }).success(function (result, status) {
            var data = JSON.parse(result);
            //console.log(data);
            $("#editModalForApplication").modal("show");
            $scope.data = data;
            $scope.button = "primary";
            $scope.show = "Edit Leave Application";
            //$scope.SalaryRecord.EffectiveDate = new Date($scope.SalaryRecord.EffectiveDate).toISOString().slice(0, 10);;
        }).error(function (result, status) {
            console.log(result);
            alert("Something Went Wrong!!!");
        });
    }

    $scope.getAllEmpLeaveApplicationById = function () {
        $scope.noButton = false;
        $http.get($scope.getAllEmpLeaveApplicationByIdUrl, {
            params: {
                "leaveAllocationId": $scope.allocationId,
                "employeeId": $scope.id
            }
        }).success(function (result, status) {
            var data = JSON.parse(result);
            //console.log(data);
            if (data.length > 0) {
                $scope.noDataAvailable2 = false;
                $scope.isDataAvailable = true;
                $scope.LeaveApplications = data;
            } else {
                $scope.errormsg = "";
                $scope.isData = false;
                $scope.isDataAvailable = true;
                $scope.noDataAvailable2 = true;
            }
        }).error(function (result, status) {
            console.log(result);
            alert("Something Went Wrong!!!");
        });
    }

    $scope.saveLeaveAllocation = function () {
        if ($scope.leaveRecordId == 0) {
            $scope.data.EmpLeaveAllocationDetailId = $scope.leaveRecordId;
            $scope.data.EmployeeId = $scope.id;
        }
        $http.post($scope.saveEmpLeaveRecordUrl + "/", $scope.data).
            success(function (result, status) {
                //console.log(result);
                var data = JSON.parse(result);
                //console.log(result);
                if (data.error == "") {
                    alert("Operation SuccessFully");
                    $("#editModalForAllocation").modal("hide");
                    $scope.loadData();
                    $scope.noDataAvailable = false;
                } else {
                    //console.log(data);
                    alert(data.error);
                    $scope.error = data.error;
                }

            }).
            error(function (result, status) {
                console.log(result);
                alert("Something Went Wrong!!!");
            });
    }

    $scope.saveLeaveApplication = function() {
        if ($scope.leaveApplicationId == 0) {
            //alert($scope.allocationId);
            $scope.data.EmpLeaveApplicationDetailId = 0;
            $scope.data.LeaveAllocationId = $scope.allocationId;
            $scope.data.EmployeeId = $scope.id;
        }
        $http.post($scope.saveEmpLeaveAllocationUrl + "/", $scope.data).
            success(function (result, status) {
                //console.log(result);
                var data = JSON.parse(result);
                if (data.error == "") {
                    alert("Operation SuccessFully");
                    $("#editModalForApplication").modal("hide");
                    $scope.loadData();
                    $scope.getAllEmpLeaveApplicationById();
                    $scope.noDataAvailable = false;
                } else {
                    //console.log(data);
                    alert(data.error);
                    $scope.error = data.error;
                }

            }).
            error(function (result, status) {
                console.log(result);
                alert("Something Went Wrong!!!");
            });
    } 

    $scope.loadApplications = function (allocation) {
        $scope.selected = allocation;
        $scope.allocationId = allocation.EmpLeaveAllocationDetailId;
        $scope.getAllEmpLeaveApplicationById();
    }

    $scope.Init = function (
        getAllEmpLeaveAllocationByIdUrl,
        getEmpLeaveRecordByIdUrl,
        getDeleteEmpLeaveRecordByIdUrl,
        saveEmpLeaveRecordUrl,
        getAllEmpLeaveApplicationByIdUrl,
        getLeaveApplicationByIdUrl,
        getDeleteEmpLeaveAllocationByIdUrl,
        saveEmpLeaveAllocationUrl
    ) {
        $scope.getAllEmpLeaveAllocationByIdUrl = getAllEmpLeaveAllocationByIdUrl;
        $scope.getEmpLeaveRecordByIdUrl = getEmpLeaveRecordByIdUrl;
        $scope.getDeleteEmpLeaveRecordByIdUrl = getDeleteEmpLeaveRecordByIdUrl;
        $scope.saveEmpLeaveRecordUrl = saveEmpLeaveRecordUrl;
        $scope.getAllEmpLeaveApplicationByIdUrl = getAllEmpLeaveApplicationByIdUrl;
        $scope.getLeaveApplicationByIdUrl = getLeaveApplicationByIdUrl;
        $scope.getDeleteEmpLeaveAllocationByIdUrl = getDeleteEmpLeaveAllocationByIdUrl;
        $scope.saveEmpLeaveAllocationUrl = saveEmpLeaveAllocationUrl;
        //Data Loading..
        //$scope.loadPage();
        if ($scope.id == 0) {
            $scope.isNew = true;
        } else {
            $scope.isNew = false;
        }
    }
});