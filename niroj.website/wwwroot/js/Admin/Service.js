$(document).ready(function () {
    let fileFolder = $('.file-folder-name').val();
    CKEDITOR.replace('service-content', {
        height: 500,
        extraPlugins : 'uploadimage',
        filebrowserBrowseUrl: '',
        filebrowserImageBrowseUrl: '',
        filebrowserUploadUrl: `/admin/attachment/upload-file?file_type=${fileFolder}`,
        filebrowserImageUploadUrl: `/admin/attachment/upload-file?file_type=${fileFolder}`
    });
});