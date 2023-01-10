function ShowImageProduct(imageUpload, previewImage) {
    if (imageUpload.files && imageUpload.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $(previewImage).attr('src', e.target.result);
        }
        reader.readAsDataURL(imageUpload.files[0]);
    }
}