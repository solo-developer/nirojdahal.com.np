String.prototype.insertAfter = function (index, string) {
    if (index > 0) {
        return this.substring(0, index + 1) + string + this.substr(index + 1);
    }
    return string + this;
};


$.fn.dataTable.Buttons.defaults.dom.button.className = 'btn btn-white btn-sm';
$(document).ready(function () {
    initializeDatatable();
    $('.clone-add-row').on('click', appendRow);
    $('.remove-row').on('click', removeRow);
    $('.delete').on('click', serverCenteredDelete);
    let fileFolder = $('.file-folder-name').val();
    if (fileFolder) {
        CKEDITOR.replace('editor', {
            height: 500,
            extraPlugins: 'uploadimage',
            filebrowserBrowseUrl: '',
            filebrowserImageBrowseUrl: '',
            filebrowserUploadUrl: `/admin/attachment/upload-file?file_type=${fileFolder}`,
            filebrowserImageUploadUrl: `/admin/attachment/upload-file?file_type=${fileFolder}`
        });
    }
    
})

function initializeDatatable() {
    $('.tbl-datatable').DataTable({
        pageLength: 25,
        responsive: true,
        dom: '<"html5buttons"B>lTfgitp',
        buttons: [
            { extend: 'copy' },
            { extend: 'csv' },
            { extend: 'excel', title: 'excel-download' },
            { extend: 'pdf', title: 'pdf-download' },

            {
                extend: 'print',
                customize: function (win) {
                    $(win.document.body).addClass('white-bg');
                    $(win.document.body).css('font-size', '10px');

                    $(win.document.body).find('table')
                        .addClass('compact')
                        .css('font-size', 'inherit');
                }
            }
        ]

    });
}

function isErrorResponse(response) {
    if (response.error) {
        return true;
    }
    return false;
}

function imagePreview(destinationSelector, input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            destinationSelector.attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
}

function ShowToastMessage(type, message) {
    $.notify(message,type);
}

function showModal(content, modalTitle, showModalFooter = true) {
    $('.modal-footer').css('display', 'none');
    if (showModalFooter) {
        $('.modal-footer').css('display', 'flex');
    }

    $('#modal-container').find('.modal-body').html(`${content}`);
    $('#modal-container').find('.modal-title').html(`${modalTitle}`);

    $('#modal-container').modal('show');
}

function appendRow() {
    let btn = $(this);
    let refTable = btn.data('tableref');
    let guid = getRandomGuid();
    let template = $(`${refTable}`).find('.template');

    let clonedRow = template.clone(true, true);

    clonedRow.find('.clear-on-clone').val('');
    let rowIdentifier = clonedRow.find('.row-id');
    rowIdentifier.val(guid);
    clonedRow.find('.is-post-param').each(function () {
        let name = $(this).attr('name');
        let newName = name.insertAfter(name.indexOf("["), guid);
        $(this).attr('name', newName);
    });


    clonedRow.css('display', '');
    clonedRow.removeClass('template');
    $(`${refTable}>tbody`).prepend(clonedRow);
}

function getRandomGuid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}

function removeRow() {
    $(this).closest('tr').remove();
}

function serverCenteredDelete() {
    event.preventDefault();

    let deletionConfirmed = confirm('Are you sure to delete?');
    if (deletionConfirmed) {
        let url = $(this).attr('href');
        if (url) {
            window.location.href = url;
        }
    }

}