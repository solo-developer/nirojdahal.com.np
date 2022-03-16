$(document).ready(function () {
    $('#btn-image-preview').on('change', function () {

        imagePreview($('.preview'), this);
    });

    let sortableArea = $("#sortable");
    if (sortableArea.length > 0) {
        sortableArea.sortable();
        sortableArea.disableSelection();
    }

    $('.order-team').on('click', sortTeam);
});

async function sortTeam() {
    event.preventDefault();
    let listItems = $('#sortable>li');
    if (listItems.length == 0) {
        ShowToastMessage("info", "There are no members to sort.");
        return;
    }

    let sortedData = [];
    let positionIndex = 1;
    listItems.each(function () {
        let id = $(this).find('.member-id').val();
        sortedData.push({
            MemberId: id,
            DisplayOrder: positionIndex
        });

        positionIndex++;
    });
    const response = await fetch(`/admin/team/order`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(sortedData)
    });
    try {
        var jsonData = await response.json();

        if (isErrorResponse(jsonData)) {

            ShowToastMessage('error', 'Failed to sort members.');
        }
        else {
            ShowToastMessage('success', 'Members sorted successfully.');
        }
    } catch (e) {
        ShowToastMessage('error', 'Failed to sort members.');

    }
}