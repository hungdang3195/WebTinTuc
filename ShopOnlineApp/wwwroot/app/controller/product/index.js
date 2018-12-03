var productController = function () {
    this.initialize = function () {
        loadData();
    }

    function registerEvent() {


    }
    function loadData() {
        var template = $('#table-template').html();
        var render = "";
        $.ajax({
            type: 'GET',
            url: '/Admin/Product/GetAll',
            dataType: 'json',
            success: (response) => {
                debugger;
                    $.each(response,
                        function (i, item) {
                            render += Mustache.render(template,
                                {
                                    Name: item.name,
                                    Image: item.image == null
                                        ? '<img src="/admin-side/images/user.png" width=25'
                                        : '<img src="' + item.image + '" width=25 />',
                                    //CategoryName: item.ProductCategory.Name,
                                    Price: shoponline.formatNumber(item.price, 0),
                                    CreatedDate: shoponline.dateTimeFormatJson(item.dateCreated),
                                    Status: shoponline.getStatus(item.status)
                                });

                            if (render !== "") {
                                $('#tbl-content').html(render);
                            }
                        });
            },
            error: function (status) {
                console.log(status);
                shoponline.notify('Cannot loading data', 'error');
            }
        });

    }

}