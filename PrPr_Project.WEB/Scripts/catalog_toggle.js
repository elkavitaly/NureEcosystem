$(document).ready(function() {
    $(".catalog_item_arrow_holder").on("click", function() {
        $(this).parent().toggleClass("catalog_item_fullsize");
        $(this).children().toggleClass("pe-7s-angle-up");
        $(this).children().toggleClass("pe-7s-angle-down");
    });
});