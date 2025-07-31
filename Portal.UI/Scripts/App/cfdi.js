$(function () {

    var $dropZone = $('#drop-zone');
    var $fileInput = $('#file-input');

    //#region Control de drag and drop
    $dropZone.on('dragover', function (e) {
        e.preventDefault();
        $dropZone.addClass('dragover');
    });

    $dropZone.on('dragleave', function (e) {
        e.preventDefault();
        $dropZone.removeClass('dragover');
    });

    $dropZone.on('drop', function (e) {
        e.preventDefault();
        $dropZone.removeClass('dragover');

        var files = e.originalEvent.dataTransfer.files;
        if (files.length > 0) {
            UploadFile(files[0]);
        }
    });

    $dropZone.on('click', function () {
        $fileInput.trigger('click');
    });

    $fileInput.on('change', function () {
        if (this.files.length > 0) {
            UploadFile(this.files[0]);
        }
    });
    //#endregion

    //#region Funciones
    function UploadFile(file) {
        $('#status').text('');

        var fileName = file.name.toLowerCase();
        if (!fileName.endsWith('.xml')) {
            $('#status').text('Solo se permiten archivos XML');
            return;
        }

        var formData = new FormData();
        formData.append('file', file);

        $.ajax({
            url: '/CFDI/ReadCfidVoucher',
            type: 'POST',
            data: formData,
            contentType: false,
            processData: false,
            beforeSend: function () {
                Swal.fire({
                    title: 'Cargando...',
                    html: 'Procesando archivo',
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    didOpen: () => {
                        Swal.showLoading();
                    }
                });
            },
            success: function (response) {
                Swal.close();
                Swal.fire({
                    title: '¡Correcto!',
                    text: "Se obtuvo correctamente el archivo",
                    icon: 'success',
                    confirmButtonText: 'OK'
                }).then((result) => {
                    if (result.isConfirmed) {
                        var blob = new Blob([response], { type: 'application/pdf' });
                        var blobUrl = window.URL.createObjectURL(blob);
                        window.open(blobUrl, '_blank');
                        $('#file-input').val('');
                    }
                });
            },
            error: function (error) {
                Swal.close();
                var errorMessage = error.responseJSON.Message || 'Ocurrió un error inesperado.';
                Swal.fire({
                    title: '¡Error!',
                    text: errorMessage,
                    icon: 'error',
                    confirmButtonText: 'OK'
                }).then(() => {
                    $('#file-input').val('');
                });
            }
        });
    }
    //#endregion


});