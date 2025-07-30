$(function () {

    //#region Metodo para cargar archivos
    var DrZone = new Dropzone(("#demo-upload"), {
        url: "/CFDI/ReadCfidVoucher",
        autoDiscover: false,
        acceptedFiles: ".xml",
        uploadMultiple: true,
        parallelUploads: 100,
        dictInvalidFileType: "Este tipo de archivo no es valido",
        autoProcessQueue: false,
        addRemoveLinks: true,
        dictDuplicateFile: "No se pueden subir archivos duplicados",
        preventDuplicates: true,
        dictRemoveFile: "Remover",
        init: function () {
            this.on("sending", function (file, xhr, formData) {

                if (IsNullOrUndefined($scope.LoadFilesParameters.CompanyId)) {
                    swal("", "Favor de seleccionar la empresa", "warning");
                    return;
                }

                $scope.errors = [];
                if (!IsNullOrUndefined($scope.LoadFilesParameters.Item))
                    $scope.LoadFilesParameters.ItemId = $scope.LoadFilesParameters.Item.Id;
                $scope.$apply();

                angular.forEach($scope.LoadFilesParameters, function (value, key) {
                    formData.append(key, JSON.stringify(value, (key, value) => {
                        if (!IsNullOrUndefined(value)) return value;
                    }));
                });
            });
            this.on("error", function (data, errorMessage, xhr) {
                var httpResponse = null;
                if (!IsNullOrUndefined(data.xhr))
                    httpResponse = JSON.parse(data.xhr.response)

                $scope.errors = [];
                $(".alertError").show();
                $('.dz-error-message span').text(typeof errorMessage == "object" ? "Error" : errorMessage);
                $(".errMessage").text("Error");

                if (!IsNullOrUndefined(httpResponse))
                    $scope.errors = httpResponse.Message.split(";");
                $scope.$apply();

                $("#quote_upload_close").prop("disabled", false);
                $('#quote_upload_clear').prop("disabled", false);

            });
            this.on("complete", function (data, file, response) {
                if (data.status != "error") {
                    var httpResponse = null;

                    if (!IsNullOrUndefined(data.xhr))
                        httpResponse = JSON.parse(data.xhr.response)

                    if (httpResponse.Status == 200) {
                        setTimeout(function () {
                            $("#quote_upload_close").prop("disabled", false);
                            $('#quote_upload_clear').prop("disabled", false);
                            DrZone.removeAllFiles();
                            $scope.PreviewReaderModel = httpResponse.Data;
                            $scope.supplierMessages = [];
                            for (var i = 0; i < $scope.PreviewReaderModel.PreviewReaderEntries.length; i++) {
                                if ($scope.PreviewReaderModel.PreviewReaderEntries[i].SupplierNull || $scope.PreviewReaderModel.PreviewReaderEntries[i].SupplierReactivate) {
                                    $scope.supplierMessages.push($scope.PreviewReaderModel.PreviewReaderEntries[i]);
                                }
                                $scope.PreviewReaderModel.PreviewReaderEntries[i].AutomaticalQuantity = 0;
                                for (var j = 0; j < $scope.PreviewReaderModel.PreviewReaderEntries[i].ConceptEntries.length; j++) {
                                    if (!IsNullOrUndefined($scope.PreviewReaderModel.PreviewReaderEntries[i].ConceptEntries[j].SaeItem.Id)) {
                                        $scope.PreviewReaderModel.PreviewReaderEntries[i].Automatical = true;
                                        $scope.PreviewReaderModel.PreviewReaderEntries[i].AutomaticalQuantity = $scope.PreviewReaderModel.PreviewReaderEntries[i].AutomaticalQuantity + 1
                                    } else {
                                        if (!$scope.PreviewReaderModel.PreviewReaderEntries[i].Automatical) {
                                            $scope.PreviewReaderModel.PreviewReaderEntries[i].Check = false;
                                        }

                                    }
                                }

                                $scope.PreviewReaderSelected = $scope.PreviewReaderModel.PreviewReaderEntries[i];
                                $scope.PreviewReaderSelected.DocumentDate = angular.copy(new Date($filter('date')($scope.PreviewReaderModel.PreviewReaderEntries[i].DocumentDate.substr(6, 13), 'MM/dd/yyyy')));
                                $scope.PreviewReaderSelected.ApplyDiscount = $scope.applyOptions[0];

                                /* if (!$scope.PreviewReaderSelected.CaptureDiscount)*/
                                $scope.Recalculate();
                            }

                            $scope.ShowPreviewReaderModel();
                            $scope.$apply();
                            if ($scope.supplierMessages.length > 0) {
                                $('#m_modal_14').modal({ backdrop: 'static', keyboard: false, show: false });
                                $("#m_modal_14").modal("show");
                            }
                            $scope.GetDocumentSeries();
                        }, 1500);
                    }
                }
            });


        }
    });
    $('#quote_upload_clear').click(function () {
        $scope.errors = [];
        //$scope.LoadFilesParameters = new LoadFilesParameters();
        //$scope.LoadFilesParameters.CompanyId = $scope.currentCompany.Id;
        //$scope.$apply();
        DrZone.removeAllFiles();
        $("#quote_upload_submit").prop("disabled", false);
    });
    $('#quote_upload_submit').click(function () {
        if (DrZone.getQueuedFiles().length >= 1) {
            if (IsNullOrUndefined($scope.LoadFilesParameters.Item) && $scope.LoadFilesParameters.ItemOption == "3") {
                swal("", "Favor de seleccionar el producto", "warning");
            } else {
                DrZone.processQueue();
                $("#quote_upload_submit").prop("disabled", true);
                $('#quote_upload_clear').prop("disabled", true);
                $("#quote_upload_close").prop("disabled", true);
            }
        } else {
            swal("", "Favor de cargar un archivo", "warning");
        }

    });
    $('#quote_upload_close').click(function () {
        $("#LoadFiles").modal("hide");
        DrZone.removeAllFiles();
    })
    //#endregion

});