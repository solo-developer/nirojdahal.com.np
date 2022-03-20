$(document).ready(function () {
    
    $('.btn-add-role').on('click', function () {

        getSaveUpdateView();
    });

    $('.btn-edit-role').on('click', function () {
        let roleId = $(this).closest('tr').find('.role-id').val();
        getSaveUpdateView(roleId);
    });

    $('.role-submit-btn').on('click', function () {
        saveUpdateBlogCategory();
    });
});

async function getSaveUpdateView(roleId = "") {
    const response = await fetch(`/user-management/role/save-update-view?role_id=${roleId}`);
    var jsonData = await response.json();
    var data = JSON.parse(jsonData).data;

    showModal(data, 'Role', false);
    reInitEvents();
}

function reInitEvents() {
    $('.role-submit-btn').off('click').on('click', saveUpdateBlogCategory);
}

async function saveUpdateBlogCategory() {
    let roleName = $('.role-name').val();
    if (roleName.length == 0) {
        ShowToastMessage("info","Role name is required.");
        return;
    }

    let isDuplicateRole = await checkDuplicateName();
    if (isDuplicateRole) {
        ShowToastMessage("info","Role with same name already exists.");
        return;
    }
    var data = $('.role-form').serialize();
    $.ajax({
        url: "/user-management/role/save",
        type: "POST",
        dataType: 'json',
        data: data,
        success: function (response) {
            if (JSON.parse(response).data.success == true) {
                window.location.reload();
            }
            else {
                ShowToastMessage("error",response.error);
            }
        },
        error: function () {
            ShowToastMessage("error","Failed to save/update role.");
        }
    });
}

async function checkDuplicateName() {
    let roleName = $('.role-name').val();

    const response = await fetch(`/user-management/role/check-duplicate-name?Name=${roleName}`);
    var jsonData = await response.json();
    var data = JSON.parse(jsonData).data;

    return data.isDuplicate;

}
