let skip = 8;
let productsCount = $("#loadMore").next().val();
$(document).on("click", "#loadMore", function () {
    $.ajax({
        url: "/Products/LoadMore/",
        type: "get",
        data: {
            "skipCount":skip
        },
         success: function (res) {
             $("#myProducts").append(res)
             skip += 8;

             if (productsCount <= skip) {
                 $("#loadMore").remove()
             }
             
        }
    });
});