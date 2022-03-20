$(document).ready(function () {
    $("#treeview").hummingbird();
    $('.role-checkbox-item').on('click', loadPermissionsOfRole);
    $("#checkAll").click(function () {
        event.preventDefault();
        // $("#treeview").hummingbird("checkAll");
        $('#treeview').find('input').each(function () {
            $(this).prop('checked', true);
        });
    });
    $("#uncheckAll").click(function () {
        event.preventDefault();
        //$("#treeview").hummingbird("uncheckAll");
        $('#treeview').find('input').each(function () {
            $(this).prop('checked', false);
        });
    });
    $("#collapseAll").click(function () {
        event.preventDefault();
        $("#treeview").hummingbird("collapseAll");
    });
    $("#expandAll").click(function () {
        event.preventDefault();
        $("#treeview").hummingbird("expandAll");
    });

    $('.role-permission-save-btn').on('click', saveRolePermissions);
});

async function loadPermissionsOfRole() {
    let roleId = $(this).val();
    UncheckRolesExcept(roleId);
    const response = await fetch(`/user-management/role-permission/permissions-of-role?role_id=${roleId}`, {
        dataType: 'json'
    });
    var jsonData = await response.json();
    var permissions = JSON.parse(jsonData).data;
    $('#treeview').find('input[type="checkbox"]').attr('checked', false);

    permissions.forEach(function (item, index) {
        $("#treeview").hummingbird("checkNode", { attr: "data-name", name: item, expandParents: true });
    });
}



function UncheckRolesExcept(roleId) {
    $('.role-container>input').each(function () {
        if ($(this).val() != roleId) {
            $(this).prop("checked", false);
        }
    });
}

async function saveRolePermissions() {
    let roleId = '';
    $('.role-container>input').each(function () {
        if ($(this).is(":checked")) {
            roleId = $(this).val();
        }
    });

    if (roleId.length == 0) {
        ShowToastMessage('info', 'At least a role is to be checked.');
        return;
    }

    let permissions = [];
    $('#treeview').find('input').each(function () {
        if ($(this).data('isheader') == 'False' && $(this).is(':checked')) {
            permissions.push($(this).data('name'));
        }
    });
    //const formData = new FormData();
    //formData.append('RoleId', roleId);
    //formData.append('Permissions', permissions);
    let data = {
        RoleId: roleId,
        Permissions: permissions
    };
    const response = await fetch(`/user-management/role-permission/save`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    });
    try {
        var jsonData = await response.json();

        if (isErrorResponse(jsonData)) {

            ShowToastMessage('error', 'Failed to save role permission detail.');
        }
        else {
            ShowToastMessage('success', 'Role Permission Saved Successfully.');
        }
    } catch (e) {
        ShowToastMessage('error', 'Failed to save role permission detail.');

    }

}