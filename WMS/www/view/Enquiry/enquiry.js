appControllers.controller('EnquiryListCtrl', [
    'ENV',
    '$scope',
    '$stateParams',
    '$state',
    '$cordovaKeyboard',
    '$cordovaBarcodeScanner',
    '$cordovaToast',
    '$ionicModal',
    '$ionicPopup',
    'ionicDatePicker',
    'ApiService',
    'PopupService',
    function (
        ENV,
        $scope,
        $stateParams,
        $state,
        $cordovaKeyboard,
        $cordovaBarcodeScanner,
        $cordovaToast,
        $ionicModal,
        $ionicPopup,
        ionicDatePicker,
        ApiService,
        PopupService) {
        $scope.Rcbp1 = {};
        $scope.Rcbp1ForConsinnee = {};
        $scope.Detail = {
            ONHANDNO: '',
            location: '',
            Trucker: '',
            VisibleDetailFlag: 'N',
            ChargeType: {
                NewItem: 'PP',
            },
            pushPublication: {
                checked: false
            },
            pushHazardous: {
                checked: false
            },
            pushClassified: {
                checked: false
            },
            pushExercise: {
                checked: false
            },
            ONHAND_D: {},
            OH_PID_D_S: {},
            OH_PID_D: {
                RowNum: 0,
                LineItemNo: 0,
                TRK_BILL_NO: '',
                PACK_TYPE: '',
                PID_NO: '',
                UnNo: '',
                GROSS_LB: 0,
                LENGTH: 0,
                WIDTH: 0,
                HEIGHT: 0
            },
            Add_OH_PID_D: {
                RowNum: 0,
                LineItemNo: 0,
                TRK_BILL_NO: '',
                PACK_TYPE: '',
                PID_NO: '',
                UnNo: '',
                GROSS_LB: 0,
                LENGTH: 0,
                WIDTH: 0,
                HEIGHT: 0
            },
            Rcdg1: {},
            Rcdg1s: {},
            PidUnGrid: {},
            PidUnGrids: {},
        };

        $scope.ChargeType = [{
            text: 'Prepaid',
            value: 'PP'
        }, {
            text: 'Collect',
            value: 'CC'
        }, {
            text: 'To Follow',
            value: 'TF'
        }];
        var CheckPush = function () {
            if ($scope.Detail.ONHAND_D.PUB_YN === 'Y') {
                $scope.Detail.pushPublication.checked = true;
            } else {
                $scope.Detail.pushPublication.checked = false;
            }
            if ($scope.Detail.ONHAND_D.HAZARDOUS_YN === 'Y') {
                $scope.Detail.pushHazardous.checked = true;
            } else {
                $scope.Detail.pushHazardous.checked = false;
            }
            if ($scope.Detail.ONHAND_D.CLSF_YN === 'Y') {
                $scope.Detail.pushClassified.checked = true;
            } else {
                $scope.Detail.pushClassified.checked = false;
            }
            if ($scope.Detail.ONHAND_D.ExerciseFlag === 'Y') {
                $scope.Detail.pushExercise.checked = true;
            } else {
                $scope.Detail.pushExercise.checked = false;
            }
        };
        var CheckLocation = function () {
            if ($scope.Detail.ONHAND_D.LOC_CODE === 'L1') {
                $scope.Detail.location = 'Location1';
            } else if ($scope.Detail.ONHAND_D.LOC_CODE === 'L2') {
                $scope.Detail.location = 'Location2';
            } else {
                $scope.Detail.location = '';
            }
        };

        var CheckTrucker = function () {
            if ($scope.Detail.ONHAND_D.TRK_CODE === 'F') {
                $scope.Detail.Trucker = 'Fedex';
            } else if ($scope.Detail.ONHAND_D.TRK_CODE === 'T') {
                $scope.Detail.Trucker = 'Others';
            } else {
                $scope.Detail.Trucker = '';
            }
        };
        var CheckChargeType = function () {
            if ($scope.Detail.ONHAND_D.TRK_CHRG_TYPE === 'PP') {
                $scope.Detail.ChargeType.NewItem = 'PP';
            } else if ($scope.Detail.ONHAND_D.TRK_CHRG_TYPE === 'CC') {
                $scope.Detail.ChargeType.NewItem = 'CC';
            } else if ($scope.Detail.ONHAND_D.TRK_CHRG_TYPE === 'TF') {
                $scope.Detail.ChargeType.NewItem = 'TF';
            } else {
                $scope.Detail.ChargeType.NewItem = '';
            }
        };
        $scope.findOnhand = function () {
            if (is.not.undefined($scope.Detail.ONHANDNO) && is.not.empty($scope.Detail.ONHANDNO)) {
                var objUri = ApiService.Uri(true, '/api/wms/ONHAND_D');
                objUri.addSearch('strONHAND_NO', $scope.Detail.ONHANDNO);
                ApiService.Get(objUri, false).then(function success(result) {
                    $scope.Detail.ONHAND_D = result.data.results[0];
                    if (is.not.undefined($scope.Detail.ONHAND_D)) {
                        $scope.Detail.VisibleDetailFlag = 'Y';
                        $scope.Detail.ONHAND_D.ONHAND_date = checkDate($scope.Detail.ONHAND_D.ONHAND_date);
                        $scope.Detail.ONHAND_D.PICKUP_SUP_datetime = checkDate($scope.Detail.ONHAND_D.PICKUP_SUP_datetime);
                        $scope.Rcbp1.selected = {
                            BusinessPartyCode: $scope.Detail.ONHAND_D.SHP_CODE,
                            BusinessPartyName: $scope.Detail.ONHAND_D.ShipperName
                        };
                        $scope.Rcbp1ForConsinnee.selected = {
                            BusinessPartyCode: $scope.Detail.ONHAND_D.CNG_CODE,
                            BusinessPartyName: $scope.Detail.ONHAND_D.ConsigneeName
                        };
                        CheckPush();
                        CheckLocation();
                        CheckTrucker();
                        CheckChargeType();
                    } else {
                        PopupService.Info(null, 'Please Enter The Current OhandNo').then();
                    }
                });
            } else {
                $scope.Detail.VisibleDetailFlag = 'N';
                PopupService.Info(null, 'Please Enter OhandNo').then();
            }
        };
        // $scope.findOnhand();
        $scope.refreshRcbp1 = function (BusinessPartyName) {
            if (is.not.undefined(BusinessPartyName) && is.not.empty(BusinessPartyName)) {
                var objUri = ApiService.Uri(true, '/api/wms/rcbp1');
                objUri.addSearch('BusinessPartyName', BusinessPartyName);
                ApiService.Get(objUri, false).then(function success(result) {
                    $scope.Rcbp1s = result.data.results;
                });
            }
        };
        $scope.refreshRcbp1ForConsinnee = function (BusinessPartyName) {
            if (is.not.undefined(BusinessPartyName) && is.not.empty(BusinessPartyName)) {
                var objUri = ApiService.Uri(true, '/api/wms/rcbp1');
                objUri.addSearch('BusinessPartyName', BusinessPartyName);
                ApiService.Get(objUri, false).then(function success(result) {
                    $scope.Rcbp1ForConsinnees = result.data.results;
                });
            }
        };
        $scope.pushChange = function (Type) {
            if (Type === 1) {
                console.log('Push Notification Change', $scope.Detail.pushPublication.checked);
            } else if (Type === 2) {
                console.log('Push Notification Change', $scope.Detail.pushHazardous.checked);
            } else if (Type === 3) {
                console.log('Push Notification Change', $scope.Detail.pushClassified.checked);
            } else if (Type === 4) {
                console.log('Push Notification Change', $scope.Detail.pushExercise.checked);
            } else {
                console.log();
            }

        };
        $scope.OnDatePicker = function () {
            var ipObj1 = {
                callback: function (val) { //Mandatory
                    //  console.log('Return value from the datepicker popup is : ' + val, new Date(val));
                    $scope.Detail.ONHAND_D.ONHAND_date = moment(new Date(val)).format('YYYY-MM-DD');
                },
                to: new Date(),
            };
            ionicDatePicker.openDatePicker(ipObj1);
        };

        $scope.OnDatePickerForVendorShipDate = function () {
            var ipObj1 = {
                callback: function (val) { //Mandatory
                    //  console.log('Return value from the datepicker popup is : ' + val, new Date(val));
                    $scope.Detail.ONHAND_D.PICKUP_SUP_datetime = moment(new Date(val)).format('YYYY-MM-DD');
                },
                to: new Date(),
            };
            ionicDatePicker.openDatePicker(ipObj1);
        };
        $scope.EditOff = function () {
            $("input").attr("disabled", true);
        };

        $scope.EditOn = function () {
            $("input").attr("disabled", false);
        };
        $scope.showDate = function (utc) {
            return moment(utc).format('DD-MMM-YYYY');
        };
        $scope.returnMain = function () {
            $state.go('index.main', {}, {
                reload: true
            });
        };
        $scope.findOH_PID_D = function (Type) {
            if (is.not.undefined($scope.Detail.ONHANDNO) && is.not.empty($scope.Detail.ONHANDNO)) {
                var objUri = ApiService.Uri(true, '/api/wms/OH_PID_D');
                objUri.addSearch('strONHAND_NO', $scope.Detail.ONHANDNO);
                ApiService.Get(objUri, false).then(function success(result) {
                    $scope.Detail.OH_PID_D_S = result.data.results;
                    if (Type !== 'Update') {
                        if (is.array($scope.Detail.OH_PID_D_S) && is.not.empty($scope.Detail.OH_PID_D_S)) {
                            showPid(0, 'Delete');
                        } else {
                            // PopupService.Info(null, 'This OH_PID_D has no Record Please Add').then(function (res) {
                            // });
                        }
                    }
                    $scope.UpdateTotal();
                });
            } else {
                PopupService.Info(null, 'Please First Create Onhand').then();
            }
        };

        // start PId Page
        $ionicModal.fromTemplateUrl('scan.html', {
            scope: $scope,
            animation: 'slide-in-up'
        }).then(function (modal) {
            $scope.modal = modal;
        });
        $scope.$on('$destroy', function () {
            $scope.modal.remove();
        });
        $scope.openModal = function () {
            if (is.not.undefined($scope.Detail.ONHANDNO) && is.not.empty($scope.Detail.ONHANDNO)) {
                $scope.modal.show();
                $scope.findOH_PID_D('');
            } else {
            
                PopupService.Info(null, 'Please First Create Onhand', '').then(function (res) {});
            }
            // $ionicLoading.show();
        };

        $scope.closeModal = function () {
            $scope.modal.hide();
            $scope.updateLineItem('Update');

        };

        $scope.DeleteLine = function (LineItemNo) {
            if (LineItemNo > 0) {
                var objUri = ApiService.Uri(true, '/api/wms/OH_PID_D/DeleteLineItem');
                objUri.addSearch('strONHAND_NO', $scope.Detail.ONHANDNO);
                objUri.addSearch('LineItemNo', LineItemNo);
                ApiService.Get(objUri, false).then(function success(result) {
                    var results = result.data.results;
                    if (is.not.empty(results)) {
                        $scope.Detail.OH_PID_D = "";
                        $scope.findOH_PID_D('Delete');
                    } else {}
                });

            }
        };
        $scope.updateLineItem = function (Type) {
            var arrOH_PID_D = [];
            var jsonData = '';
            var objUri = '';
            if (Type === 'Update') {
                if ($scope.Detail.OH_PID_D_S.length > 0) {
                    $scope.Detail.OH_PID_D.ONHAND_NO = $scope.Detail.ONHANDNO;
                    arrOH_PID_D.push($scope.Detail.OH_PID_D);
                    jsonData = {
                        "UpdateAllString": JSON.stringify(arrOH_PID_D)
                    };
                    objUri = ApiService.Uri(true, '/api/wms/OH_PID_D/updateLineItem');
                    ApiService.Post(objUri, jsonData, true).then(function success(result) {
                        if (is.not.empty(result)) {
                            $scope.findOH_PID_D(Type);
                        }
                    });
                }
            } else {
                // var length = 0;
                // $scope.Detail.OH_PID_D_S.ONHAND_NO = $scope.Detail.ONHANDNO;
                // for (var i = 0; i < $scope.Detail.OH_PID_D_S.length; i++) {
                //     length = i;
                //     arrOH_PID_D.push($scope.Detail.OH_PID_D_S[i]);
                //     jsonData = {
                //         "UpdateAllString": JSON.stringify(arrOH_PID_D)
                //     };
                //     objUri = ApiService.Uri(true, '/api/wms/OH_PID_D/updateLineItem');
                //     ApiService.Post(objUri, jsonData, true).then(function success(result) {
                //     });
                // }
            }
        };

        var showPid = function (row, Type) {
            if (Type !== 'Delete') {
                $scope.updateLineItem('Update');
            }
            if (row !== null && $scope.Detail.OH_PID_D_S.length >= row) {
                $scope.Detail.OH_PID_D = {
                    RowNum: $scope.Detail.OH_PID_D_S[row].RowNum,
                    LineItemNo: $scope.Detail.OH_PID_D_S[row].LineItemNo,
                    TRK_BILL_NO: $scope.Detail.OH_PID_D_S[row].TRK_BILL_NO,
                    PACK_TYPE: $scope.Detail.OH_PID_D_S[row].PACK_TYPE,
                    PID_NO: $scope.Detail.OH_PID_D_S[row].PID_NO,
                    UnNo: $scope.Detail.OH_PID_D_S[row].UnNo,
                    GROSS_LB: $scope.Detail.OH_PID_D_S[row].GROSS_LB,
                    LENGTH: $scope.Detail.OH_PID_D_S[row].LENGTH,
                    WIDTH: $scope.Detail.OH_PID_D_S[row].WIDTH,
                    HEIGHT: $scope.Detail.OH_PID_D_S[row].HEIGHT,
                    UnNo02: $scope.Detail.OH_PID_D_S[row].UnNo02,
                    UnNo03: $scope.Detail.OH_PID_D_S[row].UnNo03,
                    UnNo04: $scope.Detail.OH_PID_D_S[row].UnNo04,
                    UnNo05: $scope.Detail.OH_PID_D_S[row].UnNo05,
                    UnNo06: $scope.Detail.OH_PID_D_S[row].UnNo06,
                    UnNo07: $scope.Detail.OH_PID_D_S[row].UnNo07,
                    UnNo08: $scope.Detail.OH_PID_D_S[row].UnNo08,
                    UnNo09: $scope.Detail.OH_PID_D_S[row].UnNo09,
                    UnNo10: $scope.Detail.OH_PID_D_S[row].UnNo10,
                    Remark: $scope.Detail.OH_PID_D_S[row].Remark,
                };
                getPidUngrid($scope.Detail.OH_PID_D_S[row]);
            }

            $scope.Detail.blnNext = true;
            // if (is.equal(row, $scope.Detail.OH_PID_D_S.length - 1)) {
            //     $scope.Detail.blnNext = false;
            // } else {
            //     $scope.Detail.blnNext = true;
            // }
        };

        var getPidUngrid = function (PidRows) {
            arrPidUnGrid = new Array();
            $scope.Detail.PidUnGrids = '';
            $scope.Detail.PidUnGrid = {
                UnNo: PidRows.UnNo01,
                DGClass: PidRows.DgClass01,
                DGDescription: PidRows.DgDescription01
            };
            arrPidUnGrid.push($scope.Detail.PidUnGrid);
            $scope.Detail.PidUnGrid = {
                UnNo: PidRows.UnNo02,
                DGClass: PidRows.DgClass02,
                DGDescription: PidRows.DgDescription02
            };
            arrPidUnGrid.push($scope.Detail.PidUnGrid);
            $scope.Detail.PidUnGrid = {
                UnNo: PidRows.UnNo03,
                DGClass: PidRows.DgClass03,
                DGDescription: PidRows.DgDescription03
            };
            arrPidUnGrid.push($scope.Detail.PidUnGrid);
            $scope.Detail.PidUnGrid = {
                UnNo: PidRows.UnNo04,
                DGClass: PidRows.DgClass04,
                DGDescription: PidRows.DgDescription04
            };
            arrPidUnGrid.push($scope.Detail.PidUnGrid);
            $scope.Detail.PidUnGrid = {
                UnNo: PidRows.UnNo05,
                DGClass: PidRows.DgClass05,
                DGDescription: PidRows.DgDescription05
            };
            arrPidUnGrid.push($scope.Detail.PidUnGrid);
            $scope.Detail.PidUnGrid = {
                UnNo: PidRows.UnNo06,
                DGClass: PidRows.DgClass06,
                DGDescription: PidRows.DgDescription06
            };
            arrPidUnGrid.push($scope.Detail.PidUnGrid);
            $scope.Detail.PidUnGrid = {
                UnNo: PidRows.UnNo07,
                DGClass: PidRows.DgClass07,
                DGDescription: PidRows.DgDescription07
            };
            arrPidUnGrid.push($scope.Detail.PidUnGrid);
            $scope.Detail.PidUnGrid = {
                UnNo: PidRows.UnNo08,
                DGClass: PidRows.DgClass08,
                DGDescription: PidRows.DgDescription08
            };
            arrPidUnGrid.push($scope.Detail.PidUnGrid);
            $scope.Detail.PidUnGrid = {
                UnNo: PidRows.UnNo09,
                DGClass: PidRows.DgClass09,
                DGDescription: PidRows.DgDescription09
            };
            arrPidUnGrid.push($scope.Detail.PidUnGrid);
            $scope.Detail.PidUnGrid = {
                UnNo: PidRows.UnNo10,
                DGClass: PidRows.DgClass10,
                DGDescription: PidRows.DgDescription10
            };
            arrPidUnGrid.push($scope.Detail.PidUnGrid);
            $scope.Detail.PidUnGrids = arrPidUnGrid;
        };

        $scope.showPrev = function () {
            var intRow = $scope.Detail.OH_PID_D.RowNum - 1;
            if ($scope.Detail.OH_PID_D_S.length > 0 && intRow > 0 && is.equal($scope.Detail.OH_PID_D_S[intRow - 1].RowNum, intRow)) {
                // $scope.clearInput();
                showPid(intRow - 1, '');
            } else {
                PopupService.Info(null, 'Already the first one');
            }
        };
        $scope.showNext = function () {
            var intRow = $scope.Detail.OH_PID_D.RowNum + 1;
            if ($scope.Detail.OH_PID_D_S.length > 0 && $scope.Detail.OH_PID_D_S.length >= intRow && is.equal($scope.Detail.OH_PID_D_S[intRow - 1].RowNum, intRow)) {
                // $scope.clearInput();
                showPid(intRow - 1, '');
            } else {
                PopupService.Info(null, 'Already the last one');
            }
        };

        // End Pid Page

        // start Add Pid Page
        $ionicModal.fromTemplateUrl('PID.html', {
            scope: $scope,
            animation: 'slide-in-up'
        }).then(function (modal) {
            $scope.modalPID = modal;
        });
        $scope.$on('$destroy', function () {
            $scope.modalPID.remove();
        });

        $scope.UpdateTotal = function () {
            if ($scope.Detail.OH_PID_D_S.length > 0) {
                var TotalWeight = 0;
                for (var i = 0; i < $scope.Detail.OH_PID_D_S.length; i++) {
                    TotalWeight = TotalWeight + $scope.Detail.OH_PID_D_S[i].GROSS_LB;
                }
                $scope.Detail.ONHAND_D.TotalWeight = TotalWeight;
            }
        };
        $scope.closeModalAddPID = function () {
            $scope.modalPID.hide();
            $scope.findOH_PID_D('');
        };
        $scope.openModalAddPID = function () {
            if (is.not.undefined($scope.Detail.ONHANDNO) && is.not.empty($scope.Detail.ONHANDNO)) {
                $scope.updateLineItem('Update');
                $scope.Detail.Add_OH_PID_D.TRK_BILL_NO = '';
                $scope.Detail.Add_OH_PID_D.PACK_TYPE = '';
                $scope.Detail.Add_OH_PID_D.PID_NO = '';
                $scope.Detail.Add_OH_PID_D.UnNo = '';
                $scope.Detail.Add_OH_PID_D.GROSS_LB = 0;
                $scope.Detail.Add_OH_PID_D.LENGTH = 0;
                $scope.Detail.Add_OH_PID_D.WIDTH = 0;
                $scope.Detail.Add_OH_PID_D.HEIGHT = 0;
                $scope.Detail.Add_OH_PID_D.Remark = '';
                $scope.Detail.Rcdg1s = '';
                $scope.Detail.Add_OH_PID_D.UnNo01 = '';
                $scope.Detail.Add_OH_PID_D.UnNo02 = '';
                $scope.Detail.Add_OH_PID_D.UnNo03 = '';
                $scope.Detail.Add_OH_PID_D.UnNo04 = '';
                $scope.Detail.Add_OH_PID_D.UnNo05 = '';
                $scope.Detail.Add_OH_PID_D.UnNo06 = '';
                $scope.Detail.Add_OH_PID_D.UnNo07 = '';
                $scope.Detail.Add_OH_PID_D.UnNo08 = '';
                $scope.Detail.Add_OH_PID_D.UnNo09 = '';
                $scope.Detail.Add_OH_PID_D.UnNo10 = '';
                arrRcdg1 = new Array();
                $scope.modalPID.show();
            } else {
                // PopupService.Info(null, 'Please First Create Onhand', '').then(function (res) {});
            }
        };

        $scope.refreshRcdg1UnNo = function (UnNo) {
            if (is.not.undefined(UnNo) && is.not.empty(UnNo)) {
                var objUri = ApiService.Uri(true, '/api/wms/rcdg1/UnNo');
                objUri.addSearch('UnNo', UnNo);
                objUri.addSearch('UnNoFlag', 'N');
                ApiService.Get(objUri, false).then(function success(result) {
                    $scope.Rcdg1s = result.data.results;
                });
            }
        };
        $scope.ShowRcdg1 = function (UnNo) {
            var objUri = ApiService.Uri(true, '/api/wms/rcdg1/UnNo');
            objUri.addSearch('UnNo', UnNo);
            objUri.addSearch('UnNoFlag', 'Y');
            ApiService.Get(objUri, false).then(function success(result) {
                $scope.Detail.Rcdg1 = result.data.results[0];
            });
            if (!ENV.fromWeb) {
                $cordovaKeyboard.close();
            }
        };
        $scope.addLine = function () {
            PopupService.Confirm(null, 'Confirm', 'Are you sure to Add PID?').then(function (res) {
                if (res) {
                    if (is.not.undefined($scope.Detail.ONHANDNO) && is.not.empty($scope.Detail.ONHANDNO)) {
                        var arrOH_PID_D = [];
                        $scope.Detail.Add_OH_PID_D.ONHAND_NO = $scope.Detail.ONHANDNO;
                        //add UnNo start
                        for (var i = 0; i < $scope.Detail.Rcdg1s.length; i++) {
                            if (i === 0) {
                                $scope.Detail.Add_OH_PID_D.UnNo01 = $scope.Detail.Rcdg1s[i].UnNo;
                            } else if (i === 1) {
                                $scope.Detail.Add_OH_PID_D.UnNo02 = $scope.Detail.Rcdg1s[i].UnNo;
                            } else if (i === 2) {
                                $scope.Detail.Add_OH_PID_D.UnNo03 = $scope.Detail.Rcdg1s[i].UnNo;
                            } else if (i === 3) {
                                $scope.Detail.Add_OH_PID_D.UnNo04 = $scope.Detail.Rcdg1s[i].UnNo;
                            } else if (i === 4) {
                                $scope.Detail.Add_OH_PID_D.UnNo05 = $scope.Detail.Rcdg1s[i].UnNo;
                            } else if (i === 5) {
                                $scope.Detail.Add_OH_PID_D.UnNo06 = $scope.Detail.Rcdg1s[i].UnNo;
                            } else if (i === 6) {
                                $scope.Detail.Add_OH_PID_D.UnNo07 = $scope.Detail.Rcdg1s[i].UnNo;
                            } else if (i === 7) {
                                $scope.Detail.Add_OH_PID_D.UnNo08 = $scope.Detail.Rcdg1s[i].UnNo;
                            } else if (i === 8) {
                                $scope.Detail.Add_OH_PID_D.UnNo09 = $scope.Detail.Rcdg1s[i].UnNo;
                            } else if (i === 9) {
                                $scope.Detail.Add_OH_PID_D.UnNo10 = $scope.Detail.Rcdg1s[i].UnNo;
                            }
                            //add UnNo end

                        }
                        arrOH_PID_D.push($scope.Detail.Add_OH_PID_D);

                        var jsonData = {
                            "UpdateAllString": JSON.stringify(arrOH_PID_D)
                        };
                        var objUri = ApiService.Uri(true, '/api/wms/OH_PID_D/create');
                        ApiService.Post(objUri, jsonData, true).then(function success(result) {
                            var results = result.data.results;
                            if (is.not.empty(results)) {
                                $scope.closeModalAddPID();
                            } else {}
                        });

                    } else {
                        PopupService.Info(null, 'Please First Create Onhand', '').then(function (res) {});
                    }
                } else {}
            });

        };

        $scope.AddUnNo = function () {
            var myPopup = $ionicPopup.show({
                templateUrl: 'popup-UnNo.html',
                title: 'Add UnNo',
                scope: $scope,
                buttons: [{
                    text: 'Cancel',
                    onTap: function (e) {}
                }, {
                    text: 'Add',
                    type: 'button-positive',
                    onTap: function (e) {
                        if ($scope.Detail.Rcdg1.UnNo !== '' && is.not.undefined($scope.Detail.Rcdg1.UnNo)) {
                            arrRcdg1.push($scope.Detail.Rcdg1);
                            $scope.Detail.Rcdg1s = arrRcdg1;
                            $scope.Detail.Rcdg1 = '';
                        }
                    }
                }]
            });

        };

        // End Add Pid Page


    }
]);
