function AddItemToCart(ItemId, UnitPrice, Quantity) {
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "/Cart/AddToCart/" + ItemId + "/" + UnitPrice + "/" + Quantity,
        success: function (res) {
            $("#cartCounter").text(res.count);
        }, error: function (error) {

        }
    })
}

//This will update the and item quantity 
function updateQuantity(id, CurrentQuantity, quantity) {
    if (parseInt(CurrentQuantity) >= 1 && parseInt(quantity) > -1) {
        $.ajax({
            type: "PUT",
            contentType: "application/json; charset=utf-8",
            url: '/Cart/UpdateQuantity/' + id + "/" +quantity,
            success: function (res) {
                $("#cartCounter").text(res.count); //cartCounter is the id of the element where you want to display the count
            }, error: function (error) {

            }
        })
    }
}
