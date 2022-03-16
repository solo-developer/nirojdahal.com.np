$(document).ready(function () {

    $('.btn-add-blog-category').on('click', function () {

        getSaveUpdateView();
    });

    $('.btn-edit-category').on('click', function () {
        let categoryId = $(this).closest('tr').find('.category-id').val();
        getSaveUpdateView(categoryId);
    });

    $('.category-submit-btn').on('click', function () {
        saveUpdateBlogCategory();
    });
});

async function getSaveUpdateView(categoryId = "") {
    const response = await fetch(`/admin/blog-category/save-update-view?category_id=${categoryId}`, {
        dataType: 'json'
    });
    var jsonData = await response.json();
    var data = JSON.parse(jsonData).data;

    showModal(data, 'Blog Category', false);
    reInitEvents();
}

function reInitEvents() {
    $('.category-submit-btn').off('click').on('click', saveUpdateBlogCategory);
}

async function saveUpdateBlogCategory() {
    let roleName = $('.category-name').val();
    if (roleName.length == 0) {
        ShowToastMessage("info", "Category Name is required.")
        return;
    }

    let isNameDuplicate = await checkDuplicateName();
    if (isNameDuplicate) {
        ShowToastMessage("info", "Blog Category with same name already exists.");
        return;
    }
    var data = $('.category-form').serialize();
    $.ajax({
        url: "/admin/blog-category/save",
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
            ShowToastMessage("error", "Failed to save/update blog category.");
        }
    });
}

async function checkDuplicateName() {
    let roleName = $('.category-name').val();

    const response = await fetch(`/admin/blog-category/check-duplicate-name?Title=${roleName}`, {
        dataType: 'json'
    });
    var jsonData = await response.json();
    var data = JSON.parse(jsonData).data;

    return data.isDuplicate;

}
