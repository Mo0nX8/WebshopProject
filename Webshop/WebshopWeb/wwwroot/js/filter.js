var $ = jQuery.noConflict();
$(document).ready(function () {

    if ($("#priceSlider").length)
    {
        $("#priceSlider").slider(
            {
            range: true,
            min: 0,
            max: 2000000,
            values: [$('#minPrice').val() || 0, $('#maxPrice').val() || 2000000],
            slide: function (event, ui) {
                $("#minPrice").val(ui.values[0]);
                $("#maxPrice").val(ui.values[1]);
            },
            stop: function () {
                $("#filterForm").submit();
            }
        });
    } 
    
});
