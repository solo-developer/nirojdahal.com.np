$(document).ready(function () {

    $('.btn-add-tag').on('click', function () {

        getSaveUpdateView(0);
    });

    $('.btn-edit-tag').on('click', function () {
        let tagId = $(this).closest('tr').find('.tag-id').val();
        getSaveUpdateView(tagId);
    });

    $('.tag-submit-btn').on('click', function () {
        saveUpdate();
    });
});

async function getSaveUpdateView(tagId = 0) {
    const response = await fetch(`/admin/tag/save-update-view?tag_id=${tagId}`);
    var jsonData = await response.json();
    var data = JSON.parse(jsonData).data;

    showModal(data, 'Tag', false);
    reInitEvents();
}

function reInitEvents() {
    $('.tag-submit-btn').off('click').on('click', saveUpdate);
}

async function saveUpdate() {
    let tagName = $('.tag-name').val();
    if (tagName.length == 0) {
        toastr.info("Tag name is required.");
        return;
    }

    let isDuplicateTag = await checkDuplicateName();
    if (isDuplicateTag) {
        toastr.info("Tag with same name already exists.");
        return;
    }
    var data = $('.tag-form').serialize();
    $.ajax({
        url: "/admin/tag/save",
        type: "POST",
        dataType: 'json',
        data: data,
        success: function (response) {
            if (JSON.parse(response).data.success == true) {
                window.location.reload();
            }
            else {
                ShowToastMessage("error", response.error);
            }
        },
        error: function () {
            ShowToastMessage("error", "Failed to save/update tag.");
        }
    });
}

async function checkDuplicateName() {
    let tagName = $('.tag-name').val();

    const response = await fetch(`/admin/tag/check-duplicate-name?Name=${tagName}`);
    var jsonData = await response.json();
    var data = JSON.parse(jsonData).data;

    return data.isDuplicate;

}
