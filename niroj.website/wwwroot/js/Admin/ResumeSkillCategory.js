$(document).ready(function () {

    $('.btn-add-category').on('click', function () {

        getSaveUpdateView(0);
    });

    $('.btn-edit-category').on('click', function () {
        let catId = $(this).closest('tr').find('.cat-id').val();
        getSaveUpdateView(catId);
    });

    $(document).on('click', '.resume-skill-category-submit-btn', function () {
        saveUpdate();
    });
});

async function getSaveUpdateView(catId = 0) {
    const response = await fetch(`/admin/resume-skill-category/save-update-view?id=${catId}`);
    var jsonData = await response.json();
    var data = JSON.parse(jsonData).data;

    showModal(data, 'Resume Skill Category', false);
    reInitEvents();
}

function reInitEvents() {
    $('.category-submit-btn').off('click').on('click', saveUpdate);
}

async function saveUpdate() {
    let categoryName = $('.resume-skill-category-name').val();
    if (categoryName.length == 0) {
        toastr.info("Skill Category name is required.");
        return;
    }

    let isDuplicateCategory = await checkDuplicateName();
    if (isDuplicateCategory) {
        toastr.info("Skill Category with same name already exists.");
        return;
    }
    var data = $('.resume-skill-category-form').serialize();
    $.ajax({
        url: "/admin/resume-skill-category/save",
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
            ShowToastMessage("error", "Failed to save/update skill category.");
        }
    });
}

async function checkDuplicateName() {
    let categoryName = $('.category-name').val();

    const response = await fetch(`/admin/tag/check-duplicate-name?Name=${categoryName}`);
    var jsonData = await response.json();
    var data = JSON.parse(jsonData).data;

    return data.isDuplicate;

}
