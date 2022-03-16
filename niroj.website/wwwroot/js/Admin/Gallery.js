$(document).ready(function () {
    $('#btn-image-preview').on('change', function () {

        imagePreview($('.preview'), this);
    });
});