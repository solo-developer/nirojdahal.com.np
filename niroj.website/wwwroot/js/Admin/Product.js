$(document).ready(function () {
    $('.btn-img-preview').on('change', function () {
        let imageContainer = $(this).closest('.img-preview-container');
        let imageElement = imageContainer.find('img');
        imagePreview(imageElement, this);
    });

    $('form').on('submit', function (e) {
        e.preventDefault();
        $('.template').remove();
        $(this).off('submit');
        $(this).submit();
    });
});