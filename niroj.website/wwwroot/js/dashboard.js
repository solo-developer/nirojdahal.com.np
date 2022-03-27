$(document).ready(function () {
    $("#my-works-container").load("/github/public", function () { });
    $("#skills-container").load("/skills", function () { });
    $('#blog-content-container').load("/blogs/dashboard-section", function () { });

    lazyLoadImages();
})