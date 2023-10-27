var files;
var fileList;

function uploadFiles(formData, onSuccess, onError){
    $.ajax({
        url: '/home/upload',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: onSuccess,
        error: onError
    });
}
