$(document).ready(function () {
    $('#navigation').on('click', '.nav-item', setSidebarLinksActiveBasedOnLinksClicked);
});

function setSidebarLinksActiveBasedOnLinksClicked() {
    $('#navigation').find('.nav-link').removeClass('active');
    $(this).find('.nav-link').addClass('active');
}