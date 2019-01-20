var CartController = function () {
   var cachedObj = {
        colors: [],
        sizes: [],
    }
    this.initialize = function () {
    $.when(loadColors(),
            loadSizes())
    .then(function(){   
            loadData();
            });
      
        registerEvents();
    }

    function registerEvents() {
        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            $.ajax({
                url: '/Cart/RemoveFromCart',
                type: 'post',
                data: {
                    productId: id
                },
                success: function () {
                    shoponline.notify('Removing product is successful.', 'success');
                    loadHeaderCart();
                    loadData();
                }
            });
        });
        $('body').on('keyup', '.txtQuantity', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            var q = $(this).val();
            if (q > 0) {
                $.ajax({
                    url: '/Cart/UpdateCart',
                    type: 'post',
                    data: {
                        productId: id,
                        quantity: q
                    },
                    success: function () {
                        shoponline.notify('Update quantity is successful', 'success');
                        loadHeaderCart();
                        loadData();
                    }
                });
            } else {
                shoponline.notify('Your quantity is invalid', 'error');
            }

        });

        $('body').on('change', '.ddlColorId', function (e) {
            e.preventDefault();
            var id = parseInt($(this).closest('tr').data('id'));
            var colorId = $(this).val();
            var q = $(this).closest('tr').find('.txtQuantity').first().val();
            var sizeId = $(this).closest('tr').find('.ddlSizeId').first().val();

            if (q > 0) {
                $.ajax({
                    url: '/Cart/UpdateCart',
                    type: 'post',
                    data: {
                        productId: id,
                        quantity: q,
                        color:colorId,
                        size:sizeId
                    },
                    success: function () {
                        shoponline.notify('Update quantity is successful', 'success');
                        loadHeaderCart();
                        loadData();
                    }
                });
            } else {
                shoponline.notify('Your quantity is invalid', 'error');
            }

        });

        $('body').on('change', '.ddlSizeId', function (e) {
            e.preventDefault();
            var id = parseInt($(this).closest('tr').data('id'));
            var sizeId = $(this).val();
            var q = parseInt($(this).closest('tr').find('.txtQuantity').first().val());
            var colorId = parseInt($(this).closest('tr').find('.ddlColorId').first().val());
            if (q > 0) {
                $.ajax({
                    url: '/Cart/UpdateCart',
                    type: 'post',
                    data: {
                        productId: id,
                        quantity: q,
                        color:colorId,
                        size:sizeId
                    },
                    success: function () {
                        shoponline.notify('Update quantity is successful', 'success');
                        loadHeaderCart();
                        loadData();
                    }
                });
            } else {
                shoponline.notify('Your quantity is invalid', 'error');
            }

        });
        $('#btnClearAll').on('click', function (e) {
            e.preventDefault();
            $.ajax({
                url: '/Cart/ClearCart',
                type: 'post',
                success: function () {
                    shoponline.notify('Clear cart is successful', 'success');
                    loadHeaderCart();
                    loadData();
                }
            });
        });
    }
    function loadColors() {
        return $.ajax({
            type: "GET",
            url: "/Cart/GetColors",
            dataType: "json",
            success: function (response) {
                cachedObj.colors = response;
            },
            error: function () {
                shoponline.notify('Has an error in progress', 'error');
            }
        });
    }

    function loadSizes() {
        return $.ajax({
            type: "GET",
            url: "/Cart/GetSizes",
            dataType: "json",
            success: function (response) {
                cachedObj.sizes = response;
            },
            error: function () {
                shoponline.notify('Has an error in progress', 'error');
            }
        });
    }
    function getColorOptions(selectedId) {
        var colors = "<select class='form-control ddlColorId'>";
        $.each(cachedObj.colors, function (i, color) {
            if (selectedId === color.id)
                colors += '<option value="' + color.id + '" selected="select">' + color.name + '</option>';
            else
                colors += '<option value="' + color.id + '">' + color.name + '</option>';
        });
        colors += "</select>";
        return colors;
    }

    function getSizeOptions(selectedId) {
        var sizes = "<select class='form-control ddlSizeId'>";
        $.each(cachedObj.sizes, function (i, size) {
            if (selectedId === size.id)
                sizes += '<option value="' + size.id + '" selected="select">' + size.name + '</option>';
            else
                sizes += '<option value="' + size.id + '">' + size.name + '</option>';
        });
        sizes += "</select>";
        return sizes;
    }
    function loadHeaderCart() {
        $("#headerCart").load("/AjaxContent/HeaderCart");
    }
    function loadData() {
        $.ajax({
            url: '/Cart/GetCart',
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                var template = $('#template-cart').html();
                var render = "";
                var totalAmount = 0;
                $.each(response, function (i, item) {
                    render += Mustache.render(template,
                        {
                            ProductId: item.product.id,
                            ProductName: item.product.name,
                            Image: item.product.image,
                            Price: shoponline.formatNumber(item.price, 0),
                            Quantity: item.quantity,
                            Colors: getColorOptions(item.color == null? 0: item.color.id),
                            Sizes:getSizeOptions(item.Size == null? "": item.size.id),
                            Amount: shoponline.formatNumber(item.price * item.quantity, 0),
                            Url: '/' + item.product.seoAlias + "-p." + item.product.id + ".html"
                        });
                    totalAmount += item.price * item.quantity;
                });
                $('#lblTotalAmount').text(shoponline.formatNumber(totalAmount, 0));
                if (render !== "")
                    $('#table-cart-content').html(render);
                else
                    $('#table-cart-content').html('You have no product in cart');
            }
        });
        return false;
    }
}