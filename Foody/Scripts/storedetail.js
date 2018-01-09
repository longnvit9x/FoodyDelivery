var baseUrl = $("#BaseUrl").data("baseurl");
var storeID = $("#BaseUrl").data("storeid");
$(window).resize(function () {
    if ($('.modal.in').length != 0) {
        setModalMaxHeight($('.modal.in'));
    }
});

$(document).ready(function () {
    loadGioHang();
});

function loadGioHang() {
    var url = baseUrl + "giohang/getGioHang";
    var data = {
        StoreID: storeID
    }
    executeGetNew(url, data, getCartSuccess, getCartError);
}

function getCartSuccess(data) {
    if (data != -1) {
        showCart(data);
      }
}

function showCart(data) {
    var soPhan = 0;
    data.forEach(n=> soPhan += n.SoLuong);
    $('#cart-soluong').html(soPhan);
    var htmlFoodOrder = "";
    for (var i = 0; i < data.length; i++) {
        var foodOrder = data[i];
        htmlFoodOrder += "<div class='row' style='width:100%'><div class='col-md-3'>"
           + "<img src='" + foodOrder.FoodImage + "' width='60' height='60'></div>"
           + "<div class='col-md-6' style='margin:0; padding:0'>"
           + "<p style='margin:0'>" + foodOrder.FoodName
           + "</p><p style='margin:0'>" + foodOrder.FoodSize + foodOrder.FoodType
           + "</p><p style='margin:0'>" + foodOrder.ThanhTien + " đ"
           + "</p></div><div class='col-md-3 spinner-soluong'><div class='input-group number-spinner'><span class='input-group-btn data-dwn'>"
           + "<button id='btnDouwnCart_" + i + "' class='btn btn-default btn-info btn-soluong-small' data-dir='dwn'><span class='glyphicon glyphicon-minus'></span></button></span>"
           + "<input type='text' id='soLuongCart_" + i + "' class='form-control text-center input-soluong-small' value='" + foodOrder.SoLuong + "' min='1' max='10'>"
           + " <span class='input-group-btn data-up '>"
           + "<button id='btnUpCart_" + i + "' class='btn btn-default btn-info btn-soluong-small' data-dir='up'><span class='glyphicon glyphicon-plus'></span></button></span></div></div></div>"
    }
    $('#list-order').html(htmlFoodOrder);
    var tongTien = 0;
    data.forEach(n=> tongTien += n.ThanhTien);
    $('#cart-tongtien').html(tongTien + " đ");
}
function getCartError() {
    display("Đã xảy ra lỗi", "Không thể tải dữ liệu!");
}

function btnAddCart(FoodID) {
    var url = baseUrl + "store/getFoodDetail";
    var data = { FoodID: FoodID };
    executeGetNew(url, data, getFoodDetailSuccess, getFoodDetailError);
}
var glFood;
var foodSizeID = -1;
var foodTypeID = -1;
var soLuong = 1;
function getFoodDetailSuccess(data) {
    glFood = data;
    soLuong = 1;
    $("#imgFood").attr("src", data.FoodImage);
    $("#foodName").html(data.FoodName);
    $("#foodPrice").html(data.Price + 'đ');
    $("#soLuong").val(soLuong);
    var htmlFoodSize = "";

    for (var i = 0; i < data.FoodSizes.length; i++) {
        foodSizeID = data.FoodSizes[0].FoodSizeID;
        var foodSize = data.FoodSizes[i];
        htmlFoodSize += "<div class='row form-check' style='width:100%'>"
                       + "<div class='col-md-8'><input id='cbx_" + foodSize.FoodSizeID + "' foodsizeid='" + foodSize.FoodSizeID + "' type='radio' name='radio'" + ((i == 0) ? 'checked' : '') + "><span>" + foodSize.SizeName
                       + "</span></div><div class='col-md-4 food-price'><span>+" + foodSize.PriceSize + "đ</span></div></div>"

    }
    $("#lstFoodSize").html((htmlFoodSize != "") ? htmlFoodSize : "Không có dữ liệu");

    var htmlFoodType = "";
    for (var i = 0; i < data.FoodTypes.length; i++) {
        foodTypeID = data.FoodTypes[0].FoodTypeID;
        var foodType = data.FoodTypes[i];
        htmlFoodType += "<div class='row form-check' style='width:100%'>"
                       + "<div class='col-md-8'><input id='cbx_" + foodType.FoodTypeID + "' foodtypeid='" + foodType.FoodTypeID + "' type='radio' name='radio'" + ((i == 0) ? 'checked' : '') + "><span>" + foodSize.SizeName
                       + "</span></div><div class='col-md-4 food-price'><span>+" + foodType.PriceType + "đ</span></div></div>"

    }
    $("#lstFoodType").html((htmlFoodType != "") ? htmlFoodType : "Không có dữ liệu");
    $("#mdFoodDetail").modal("show");
    tongGia(foodSizeID, foodTypeID, soLuong);
    $('#soLuong').on('change', function () {
        soLuong = this.value;
        tongGia(foodSizeID, foodTypeID, soLuong);
    });
    $('#btnDouwn').click(function () {
        setTimeout(function () {
            soLuong = $("#soLuong").val();
            tongGia(foodSizeID, foodTypeID, soLuong);
        }, 1000);
    })
    $('#btnUp').click(function () {
        setTimeout(function () {
            soLuong = $("#soLuong").val();
            tongGia(foodSizeID, foodTypeID, soLuong);
        }, 1000);
    })
    for (var i = 0; i < data.FoodSizes.length; i++) {
        $('#cbx_' + data.FoodSizes[0].FoodSizeID).checked = true;
        var foodSize = data.FoodSizes[i];
        $('#cbx_' + foodSize.FoodSizeID).on('click change', function () {
            if (this.checked) {
                foodSizeID = this.getAttribute('foodsizeid');
                tongGia(foodSizeID, foodTypeID, soLuong);
            }
        });
    }
    for (var i = 0; i < data.FoodTypes.length; i++) {
        $('#cbx_' + data.FoodTypes[0].FoodTypeID).checked = true;
        var foodType = data.FoodTypes[i];
        $('#cbx_' + foodType.FoodTypeID).on('click change', function () {
            if (this.checked) {
                foodTypeID = this.getAttribute('foodtypeid');
                tongGia(foodSizeID, foodTypeID, soLuong);
            }
        });
    }
}

function tongGia(fsID, ftID, sl) {
    var soLuong = sl;
    var priceFood = glFood.Price;
    var priceSize = 0;
    var priceType = 0;
    if (glFood.FoodSizes.length > 0) {
        priceSize = glFood.FoodSizes.find(n=> n.FoodSizeID == fsID).PriceSize;
    }
    if (glFood.FoodTypes.length > 0) {
        priceType = glFood.FoodTypes.find(n=> n.FoodTypeID == ftID).PriceType;
    }
    $('#tongGia').html((priceFood + priceSize + priceType) * soLuong + " đ");
}

function getFoodDetailError() {
    display("Đã xảy ra lỗi", "Không thể tải dữ liệu!");
}


function addFood() {
    var url = baseUrl + "giohang/addFood";
    var data = {
        FoodID: glFood.FoodID,
        FoodSizeID: foodSizeID,
        FoodTypeID: foodTypeID,
        soluong: soLuong
    };
    executePostNew(url, JSON.stringify(data), addFoodSuccess, addFoodError);
}

function addFoodSuccess(data) {
    if (data != null) {
        showCart(data);
        $("#mdFoodDetail").modal("hide");
    }
}

function addFoodError() {
    display("Đã xảy ra lỗi", "Không thêm món ăn!");
}

function deleteCart() {
    var url = baseUrl + "giohang/deleteCart";
    var data = {
        StoreID: storeID
    };
    executePostNew(url, JSON.stringify(data), deleteCartSuccess, deleteCartError);
}

function deleteCartSuccess(data) {
    if (data) {
        var cart = [];
        showCart(cart);
    }
}

function deleteCartError() {
    display("Đã xảy ra lỗi", "Không thêm xóa giỏ hàng!");
}

function datHang() {
    $('#mdDatHang').modal('show');
}

function setModalMaxHeight(element) {
    this.$element = $(element);
    this.$content = this.$element.find('.modal-content');
    var borderWidth = this.$content.outerHeight() - this.$content.innerHeight();
    var dialogMargin = $(window).width() < 768 ? 20 : 60;
    var contentHeight = $(window).height() - (dialogMargin + borderWidth);
    var headerHeight = this.$element.find('.modal-header').outerHeight() || 0;
    var footerHeight = this.$element.find('.modal-footer').outerHeight() || 0;
    var maxHeight = contentHeight - (headerHeight + footerHeight);

    this.$content.css({
        'overflow': 'hidden'
    });

    this.$element
      .find('.modal-body').css({
          'max-height': maxHeight,
          'overflow-y': 'auto'
      });
}

$('.modal').on('show.bs.modal', function () {
    $(this).show();
    setModalMaxHeight(this);
});

$(function () {
    var action;
    $(".number-spinner button").mousedown(function () {
        btn = $(this);
        input = btn.closest('.number-spinner').find('input');
        btn.closest('.number-spinner').find('button').prop("disabled", false);

        if (btn.attr('data-dir') == 'up') {
            action = setInterval(function () {
                if (input.attr('max') == undefined || parseInt(input.val()) < parseInt(input.attr('max'))) {
                    input.val(parseInt(input.val()) + 1);
                } else {
                    btn.prop("disabled", true);
                    clearInterval(action);
                }
            }, 50);
        } else {
            action = setInterval(function () {
                if (input.attr('min') == undefined || parseInt(input.val()) > parseInt(input.attr('min'))) {
                    input.val(parseInt(input.val()) - 1);
                } else {
                    btn.prop("disabled", true);
                    clearInterval(action);
                }
            }, 50);
        }
    }).mouseup(function () {
        clearInterval(action);
    });
});