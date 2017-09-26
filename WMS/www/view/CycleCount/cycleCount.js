appControllers.controller('cycleCountCtrl', [
    'ENV',
    '$scope',
    '$stateParams',
    '$state',
    '$cordovaKeyboard',
    '$cordovaBarcodeScanner',
    '$cordovaToast',
    'PopupService',
    'ApiService',
    function (
        ENV,
        $scope,
        $stateParams,
        $state,
        $cordovaKeyboard,
        $cordovaBarcodeScanner,
        $cordovaToast,
        PopupService,
        ApiService) {
        $scope.Aeaw1 = {
            MasterJobNo: '',
        };
        $scope.Ae1 = {};
        $scope.Detail = {
            Aemt1: {},
            PidS: {},
            Aemt1S: {},
            Scan: {
                PID_NO: ''
            }
        };
        $scope.refreshAeaw = function (MAwbNo) {
            if (is.not.undefined(MAwbNo) && is.not.empty(MAwbNo)) {
                var objUri = ApiService.Uri(true, '/api/wms/awaw1/MAwbNo');
                objUri.addSearch('MAwbNo', MAwbNo);
                ApiService.Get(objUri, false).then(function success(result) {
                    $scope.Aeaw1s = result.data.results;
                    if ($scope.Aeaw1s !== null && $scope.Aeaw1s.length > 0) {
                        $scope.Ae1.selected = $scope.Aeaw1s[0];
                        // $scope.ShowAeaw();
                    } else {
                        $scope.Aeaw1s = [];
                    }
                });
            }
        };

        $scope.ShowAeaw = function (MAwbNo) {
            if (is.not.undefined(MAwbNo) && is.not.empty(MAwbNo)) {
                var objUri = ApiService.Uri(true, '/api/wms/awaw1/MasterJobNo');
                objUri.addSearch('MAwbNo', MAwbNo);
                ApiService.Get(objUri, true).then(function success(result) {
                    $scope.Aeaw1 = result.data.results[0];
                    getPid($scope.Aeaw1.MasterJobNo);
                });
            } else {
                $scope.Detail.PidS='';
                $scope.Detail.Aemt1S='';
            }
            if (!ENV.fromWeb) {
                $cordovaKeyboard.close();
            }
        };

        var getPid = function (MasterJobNo) {
            if (MasterJobNo !== '') {
                var objUri = ApiService.Uri(true, '/api/wms/awaw1/Pid');
                objUri.addSearch('MasterJobNo', MasterJobNo);
                objUri.addSearch('FromAeawFlag', 'Y');
                ApiService.Get(objUri, true).then(function success(result) {
                    $scope.Detail.PidS = result.data.results;
                });

                objUri = ApiService.Uri(true, '/api/wms/awaw1/Pid');
                objUri.addSearch('MasterJobNo', MasterJobNo);
                objUri.addSearch('FromAeawFlag', 'N');
                ApiService.Get(objUri, true).then(function success(result) {
                    $scope.Detail.Aemt1S = result.data.results;
                });
            }

        };

        $scope.enter = function (ev, type) {
            if (is.equal(ev.keyCode, 13)) {
                if (is.equal(type, 'PID_NO') && is.not.empty($scope.Detail.Scan.PID_NO)) {
                    if (blnVerifyInput('PID_NO')) {
                    }
                }
                if (!ENV.fromWeb) {
                    $cordovaKeyboard.close();
                }
            }
        };

        $scope.openCam = function (type) {
            if (!ENV.fromWeb) {
                if (is.equal(type, 'PID_NO')) {
                    $cordovaBarcodeScanner.scan().then(function (imageData) {
                        $scope.Detail.Scan.PID_NO = imageData.text;
                        if (blnVerifyInput('PID_NO')) {
                        }
                    }, function (error) {
                        $cordovaToast.showShortBottom(error);
                    });
                }
            }
        };

        var insertAEMT1 = function () {
            $scope.Detail.Aemt1.MAwbNo = $scope.Aeaw1.MAwbNo;
            $scope.Detail.Aemt1.PID_NO = $scope.Detail.Scan.PID_NO;
            $scope.Detail.Aemt1.UserID = sessionStorage.getItem('UserId').toString();
            var arrAemt1 = [];
            arrAemt1.push($scope.Detail.Aemt1);
            var jsonData = {
                "UpdateAllString": JSON.stringify(arrAemt1)
            };
            var objUri = ApiService.Uri(true, '/api/wms/Aemt1/Insert');
            ApiService.Post(objUri, jsonData, true).then(function success(result) {
                PopupService.Alert(null, 'This PID No is vailed under this MAWB').then(
                    $scope.ShowAeaw($scope.Detail.Aemt1.MAwbNo)
                );

            });

        };
        var blnVerifyInput = function (type) {
            var blnPass = true;
            if (is.equal(type, 'PID_NO')) {
                if ($scope.Detail.PidS.length > 0) {
                    for (var i = 0; i < $scope.Detail.PidS.length; i++) {
                        if ($scope.Detail.Scan.PID_NO !== $scope.Detail.PidS[i].PID_NO) {
                            blnPass = false;
                        } else {
                            blnPass = true;
                            break;
                        }
                    }
                } else {
                    blnPass = false;
                }
            }
            if (blnPass) {
                insertAEMT1();
            } else {
                PopupService.Alert(null, 'This PID No is not under this MAWB').then();
            }
            $scope.Detail.Scan.PID_NO = '';
            return blnPass;

        };

        $scope.clearInput = function (type) {
            if (is.equal(type, 'PID_NO')) {
                if ($scope.Detail.Scan.PID_NO.length > 0) {
                    $scope.Detail.Scan.PID_NO = '';
                    $('#txt-PID_NO').focus();
                }
            }
        };

        $scope.showDate = function (utc) {
            return moment(utc).format('DD-MMM-YYYY');
        };
        $scope.GoToDetail = function (Imcc1) {
            if (Imcc1 !== null) {
                $state.go('cycleCountDetail', {
                    'CustomerCode': Imcc1.CustomerCode,
                    'TrxNo': Imcc1.TrxNo,
                }, {
                    reload: true
                });
            }
        };
        $scope.returnMain = function () {
            $state.go('index.main', {}, {
                reload: true
            });
        };
    }
]);

appControllers.controller('cycleCountDetailCtrl', [
    'ENV',
    '$scope',
    '$stateParams',
    '$state',
    '$http',
    '$timeout',
    '$ionicHistory',
    '$ionicLoading',
    '$ionicPopup',
    '$ionicModal',
    '$cordovaKeyboard',
    '$cordovaToast',
    '$cordovaBarcodeScanner',
    'SqlService',
    'ApiService',
    'PopupService',
    function (
        ENV,
        $scope,
        $stateParams,
        $state,
        $http,
        $timeout,
        $ionicHistory,
        $ionicLoading,
        $ionicPopup,
        $ionicModal,
        $cordovaKeyboard,
        $cordovaToast,
        $cordovaBarcodeScanner,
        SqlService,
        ApiService,
        PopupService) {
        var popup = null;
        var hmImcc2 = new HashMap();
        var Imcc2dataResults = new Array();
        $scope.Detail = {
            Customer: $stateParams.CustomerCode,
            TrxNo: $stateParams.TrxNo,
            NextStatus: '',
            Imcc2: {
                setColorPacking: '',
                setColorWhole: '',
                setColorLoose: ''
            },
            Impr1: {
                ProductCode: '',
                ProductDescription: ''
            },
            Imcc2s: {},
            Imcc2sDb: {}
        };

        $scope.returnList = function () {
            if ($ionicHistory.backView()) {
                $ionicHistory.goBack();
            } else {
                $state.go('cycleCountList', {}, {
                    reload: false
                });
            }
        };

        $scope.showPrev = function () {
            var intRow = $scope.Detail.Imcc2.RowNum - 1;
            if ($scope.Detail.Imcc2s.length > 0 && intRow > 0 && is.equal($scope.Detail.Imcc2s[intRow - 1].RowNum, intRow)) {
                // $scope.clearInput();
                showImcc2(intRow - 1);
            } else {
                PopupService.Info(popup, 'Already the first one');
            }
        };
        var checkDimensionQty = function (DimensionFlag) {
            if (DimensionFlag !== "") {
                if (DimensionFlag === '1') {
                    if ($scope.Detail.Imcc2.PackingQtyTempValue < 1) {
                        PopupService.Info(popup, 'Packing Qty Must Be Greater Than Zero ');
                        $scope.Detail.NextStatus = true;
                    } else {
                        $scope.Detail.NextStatus = false;
                    }
                } else if (DimensionFlag === '2') {
                    if ($scope.Detail.Imcc2.WholeQtyTempValue < 1) {
                        PopupService.Info(popup, 'Whole Qty Must Be Greater Than Zero ');
                        $scope.Detail.NextStatus = true;
                    } else {
                        $scope.Detail.NextStatus = false;
                    }
                } else if (DimensionFlag === '3') {
                    if ($scope.Detail.Imcc2.LooseQtyTempValue < 1) {
                        PopupService.Info(popup, 'Loose Qty Must Be Greater Than Zero ');
                        $scope.Detail.NextStatus = true;
                    } else {
                        $scope.Detail.NextStatus = false;
                    }
                } else {}
            }
        };

        $scope.showNext = function (comfirmLastRecord) {
            var DimensionFlag = $scope.Detail.Imcc2.DimensionFlag;
            if (comfirmLastRecord === 1) {
                var intRow = $scope.Detail.Imcc2.RowNum + 1;
                if ($scope.Detail.Imcc2s.length > 0 && $scope.Detail.Imcc2s.length >= intRow && is.equal($scope.Detail.Imcc2s[intRow - 1].RowNum, intRow)) {
                    // $scope.clearInput();
                    checkDimensionQty(DimensionFlag);
                    if ($scope.Detail.NextStatus === false) {
                        var obj = {
                            PackingQtyTempValue: $scope.Detail.Imcc2.PackingQtyTempValue,
                            WholeQtyTempValue: $scope.Detail.Imcc2.WholeQtyTempValue,
                            LooseQtyTempValue: $scope.Detail.Imcc2.LooseQtyTempValue
                        };
                        var strFilter = 'TrxNo=' + $scope.Detail.Imcc2.TrxNo + ' And LineItemNo=' + $scope.Detail.Imcc2.LineItemNo;
                        SqlService.Update('Imcc2_CycleCount', obj, strFilter).then(function (res) {
                            $scope.Detail.Imcc2s[intRow - 2].PackingQtyTempValue = $scope.Detail.Imcc2.PackingQtyTempValue;
                            $scope.Detail.Imcc2s[intRow - 2].WholeQtyTempValue = $scope.Detail.Imcc2.WholeQtyTempValue;
                            $scope.Detail.Imcc2s[intRow - 2].LooseQtyTempValue = $scope.Detail.Imcc2.LooseQtyTempValue;
                            showImcc2(intRow - 1);
                            // SqlService.Select('Imcc2_CycleCount', '*').then(function (results) {
                            //     if (results.rows.length > 0) {
                            //         for (var i = 0; i < results.rows.length; i++) {
                            //             var Imcc2_CycleCount = results.rows.item(i);
                            //             Imcc2dataResults = Imcc2dataResults.concat(Imcc2_CycleCount);
                            //             $scope.Detail.Imcc2s = Imcc2dataResults;
                            //         }
                            //         showImcc2(intRow - 1);
                            //     }
                            //     $ionicLoading.hide();
                            // }, function (res) {
                            //     $ionicLoading.hide();
                            // });

                        });

                    }
                } else {
                    PopupService.Info(popup, 'Already the last one');
                }
            } else if (comfirmLastRecord === 2) {
                checkDimensionQty(DimensionFlag);
                if ($scope.Detail.NextStatus === false) {
                    var obj = {
                        PackingQtyTempValue: $scope.Detail.Imcc2.PackingQtyTempValue,
                        WholeQtyTempValue: $scope.Detail.Imcc2.WholeQtyTempValue,
                        LooseQtyTempValue: $scope.Detail.Imcc2.LooseQtyTempValue
                    };
                    var strFilter = 'TrxNo=' + $scope.Detail.Imcc2.TrxNo + ' And LineItemNo=' + $scope.Detail.Imcc2.LineItemNo;
                    SqlService.Update('Imcc2_CycleCount', obj, strFilter).then(function (res) {});
                }
            }
        };
        var showImcc2 = function (row) {
            if (row !== null && $scope.Detail.Imcc2s.length >= row) {
                $scope.Detail.Imcc2 = {
                    RowNum: $scope.Detail.Imcc2s[row].RowNum,
                    TrxNo: $scope.Detail.Imcc2s[row].TrxNo,
                    LineItemNo: $scope.Detail.Imcc2s[row].LineItemNo,
                    WarehouseCode: $scope.Detail.Imcc2s[row].WarehouseCode,
                    StoreNo: $scope.Detail.Imcc2s[row].StoreNo,
                    ProductTrxNo: $scope.Detail.Imcc2s[row].ProductTrxNo,
                    ProductCode: $scope.Detail.Imcc2s[row].ProductCode,
                    Description: $scope.Detail.Imcc2s[row].Description,
                    PackingQtyTempValue: $scope.Detail.Imcc2s[row].PackingQtyTempValue,
                    WholeQtyTempValue: $scope.Detail.Imcc2s[row].WholeQtyTempValue,
                    LooseQtyTempValue: $scope.Detail.Imcc2s[row].LooseQtyTempValue,
                    DimensionFlag: $scope.Detail.Imcc2s[row].DimensionFlag,
                    PackingUomCode: $scope.Detail.Imcc2s[row].PackingUomCode,
                    LooseUomCode: $scope.Detail.Imcc2s[row].LooseUomCode,
                    WholeUomCode: $scope.Detail.Imcc2s[row].WholeUomCode,
                };

                setColor($scope.Detail.Imcc2s[row].DimensionFlag);
            }
            if (is.equal(row, $scope.Detail.Imcc2s.length - 1)) {
                $scope.Detail.blnNext = false;
            } else {
                $scope.Detail.blnNext = true;
            }
        };
        var setColor = function (DimensionFlag) {
            var Color = "blue";
            if (DimensionFlag !== '') {
                if (DimensionFlag === '1') {
                    $scope.Detail.Imcc2.setColorPacking = {
                        "color": Color
                    };
                } else if (DimensionFlag === '2') {
                    $scope.Detail.Imcc2.setColorWhole = {
                        "color": Color
                    };
                } else if (DimensionFlag === '3') {
                    $scope.Detail.Imcc2.setColorLoose = {
                        "color": Color
                    };
                } else {}
            }
        };
        $scope.Confirm = function () {
            $scope.showNext(2);
            if ($scope.Detail.NextStatus === true) {} else if ($scope.Detail.NextStatus === false) {
                SqlService.Select('Imcc2_CycleCount', '*').then(function (results) {
                    if (results.rows.length > 0) {
                        for (var i = 0; i < results.rows.length; i++) {
                            var Imcc2_CycleCount = results.rows.item(i);
                            Imcc2_CycleCount.UserId = sessionStorage.getItem('UserId').toString();
                            Imcc2dataResults = Imcc2dataResults.concat(Imcc2_CycleCount);
                        }
                        var jsonData = {
                            "UpdateAllString": JSON.stringify(Imcc2dataResults)
                        };
                        var objUri = ApiService.Uri(true, '/api/wms/imcc2/confirm');
                        ApiService.Post(objUri, jsonData, true).then(function success(result) {
                            PopupService.Info(null, 'Confirm Success', '').then(function (res) {
                                $scope.returnList();
                            });
                        });
                    }
                    $ionicLoading.hide();
                }, function (res) {
                    $ionicLoading.hide();
                });
            }
        };
        var GetImcc2ProductTrxNo = function (TrxNo) {
            var objUri = ApiService.Uri(true, '/api/wms/imcc2');
            objUri.addSearch('TrxNo', TrxNo);
            ApiService.Get(objUri, true).then(function success(result) {
                $scope.Detail.Imcc2s = result.data.results;
                if ($scope.Detail.Imcc2s !== null && $scope.Detail.Imcc2s.length > 0) {
                    SqlService.Delete('Imcc2_CycleCount').then(function (res) {
                        for (var i = 0; i < $scope.Detail.Imcc2s.length; i++) {
                            var objImcc2 = $scope.Detail.Imcc2s[i];
                            SqlService.Insert('Imcc2_CycleCount', objImcc2).then();
                        }
                        showImcc2(0);
                    });
                } else {
                    PopupService.Info(null, 'Imcc2 Not Record', '').then(function (res) {
                        $scope.returnList();
                    });
                }

            });
        };
        GetImcc2ProductTrxNo($scope.Detail.TrxNo);
    }
]);
