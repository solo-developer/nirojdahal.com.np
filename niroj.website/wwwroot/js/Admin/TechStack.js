$(document).ready(function () {
    IconPicker.Init({
        jsonUrl: './iconpicker-1.5.0.json',
        searchPlaceholder: 'Search Icon',
        showAllButton: 'Show All',
        cancelButton: 'Cancel',
        noResultsFound: 'No results found.', // v1.5.0 and the next versions
        borderRadius: '20px', // v1.5.0 and the next versions
    });
    IconPicker.Run('#GetIconPicker');

    $(document).on('click', '.remove-row', function () {
        let row = $(this).closest('.tech-row').remove();
    });

    $('#add-tech-row').click(async function () {
        const response = await fetch(`/admin/tech-stack/new-tech-stack-row`);
        var jsonData = await response.json();
        var data = JSON.parse(jsonData).data;

        $('.tech-list-table>tbody').prepend(data);
    });
});
document.addEventListener("icon-selected", function (e) {
    let className = e.detail.className;
    let element = $('#selectedIcons');
    let value = element.val();
    if (value) {
        value += ",";
    }
    value = (value ?? "") + className;
    element.val(value);
});