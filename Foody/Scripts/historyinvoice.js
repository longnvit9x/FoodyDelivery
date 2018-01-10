var baseUrl = $("#BaseUrl").data("baseurl");
function huyDon(invoiceID, status) {
    sttchange = status;
    ivIDChange = invoiceID
    var url = baseUrl + "invoice/SuaSTTHoaDon";
    var data = {
        InvoiceID: invoiceID,
        Status: status
    };
    executePut(url, JSON.stringify(data), huyDonSuccess, huyDonError);
}

function huyDonSuccess(data) {
    if (data) {
        removeStt("#XacThuc_", ivIDChange);
        removeStt("#CheBien_", ivIDChange);
        $("#HuyBo_" + ivIDChange).css('display', 'block');
        $('#btnHuyDon_' + ivIDChange).css('display', 'none');
        display("Hủy đơn hàng","Thành công..!")
    }
}

function huyDonError(data) {
    display("Hủy đơn hàng","Thành thất bại..!")
}

function removeStt(selector, id) {
    $(selector + id).removeClass("donhang-status-red");
}