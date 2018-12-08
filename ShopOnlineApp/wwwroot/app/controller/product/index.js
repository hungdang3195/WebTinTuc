var productController = function () {
    this.initialize = function () {
        loadDataCategory();
        loadData();
        registerEvent();
    }

    function registerEvent() {
        $('#ddlShowPage').on('change', function () {
            shoponline.configs.pageSize = $(this).val();
            shoponline.configs.pageIndex = 1;
            loadData(true);
        });
        $('#btnSearch').on('click',
            function() {
                loadData(true);
            });


    }
    function loadDataCategory() {
        $.ajax({
            type: 'GET',
            url: '/Admin/ProductCategory/GetAll',
            dataType: 'json',
            success: (response) => {
                var result = "<option value=''>---Chọn danh mục---</option>";
                $.each(response,
                    function (i, item) {
                        var temp = Object.assign({}, item);
                        result += "<option value='" + temp.id + "'>" + temp.name + "</option>";
                    });
                $('#ddlCategorySearch').html(result);

                console.log(result);
            },
            error: function (status) {
                console.log(status);
                shoponline.notify('Không thể load danh mục sản phẩm ', 'lỗi');
            }
        });
    }
    function loadData(isPageChanged) {
        var template = $('#table-template').html();
        var render = "";
        $.ajax({
            type: 'GET',
            url: '/Admin/Product/GetAllPaging',
            data: {
                categoryId: $('#ddlCategorySearch').val(),
                pageIndex: shoponline.configs.pageIndex,
                pageSize: shoponline.configs.pageSize
            },
            dataType: 'json',
            success: (response) => {
                    $.each(response.data.items,
                        function (i, item) {
                            render += Mustache.render(template,
                                {
                                    Name: item.name,
                                    Image: item.image === null
                                        ? '<img src="/admin-side/images/user.png" width=25'
                                        : '<img src="' + item.image + '" width=25 />',
                                    //CategoryName: item.ProductCategory.Name,
                                    Price: shoponline.formatNumber(item.price, 0),
                                    CreatedDate: shoponline.dateTimeFormatJson(item.dateCreated),
                                    Status: shoponline.getStatus(item.status)
                                });
                            $('#lblTotalRecords').text(response.data.rowCount);
                            if (render !== "") {
                                $('#tbl-content').html(render);
                            }
                            wrapPaging(response.data.rowCount, function () {
                                loadData();
                            }, isPageChanged);
                        });
            },
            error: function (status) {
                console.log(status);
                shoponline.notify('Không thể load dữ liệu', 'lỗi');
            }
        });


        function wrapPaging(recordCount, callBack, changePageSize) {
            var totalsize = Math.ceil(recordCount / shoponline.configs.pageSize);
            //Unbind pagination if it existed or click change pagesize
            if ($('#paginationUL a').length === 0 || changePageSize === true) {
                $('#paginationUL').empty();
                $('#paginationUL').removeData("twbs-pagination");
                $('#paginationUL').unbind("page");
            }
            //Bind Pagination Event
            $('#paginationUL').twbsPagination({
                totalPages: totalsize,
                visiblePages: 7,
                first: 'Đầu',
                prev: 'Trước',
                next: 'Tiếp',
                last: 'Cuối',
                onPageClick: function (event, p) {
                    shoponline.configs.pageIndex = p;
                    setTimeout(callBack(), 200);
                }
            });
        }

    };

    function test() {
        alert("test merge code11233 develope");
        alert("test merge code admin sua nhe12121");
    }
}