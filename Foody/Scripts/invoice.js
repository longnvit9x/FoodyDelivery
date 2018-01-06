var baseUrl = $("#BaseUrl").data("baseurl");
var storeID = $("#BaseUrl").data("storeid");
$(document).ready(function ($) {
    loadHoaDon();
});

function loadHoaDon() {
    var url = baseUrl + "invoice/getAll";
    var data = { StoreID: storeID };
    executeGetNew(url, data, loadHoaDonSuccess, loadHoaDonError);
}
function loadHoaDonSuccess(data) {
    populateHoaDon(data);
}

function loadHoaDonError(data) {
    console.log("error");
}

function populateHoaDon(HoaDon) {
    for (var i = 0; i < HoaDon.length; i++) {
        (HoaDon[i].Status == 0) ? addSelect("#DonMoi_", HoaDon[i].InvoiceID) : removeSelect("#DonMoi_", HoaDon[i].InvoiceID);
        (HoaDon[i].Status == 1) ? addSelect("#XacThuc_", HoaDon[i].InvoiceID) : removeSelect("#XacThuc_", HoaDon[i].InvoiceID);
        (HoaDon[i].Status == 2) ? addSelect("#CheBien_", HoaDon[i].InvoiceID) : removeSelect("#CheBien_", HoaDon[i].InvoiceID);
        (HoaDon[i].Status == 3) ? addSelect("#VanChuyen_", HoaDon[i].InvoiceID) : removeSelect("#VanChuyen_", HoaDon[i].InvoiceID);
        (HoaDon[i].Status == 4) ? addSelect("#HoanTat_", HoaDon[i].InvoiceID) : removeSelect("#HoanTat_", HoaDon[i].InvoiceID);
        (HoaDon[i].Status == 5) ? addSelect("#HuyBo_", HoaDon[i].InvoiceID) : removeSelect("#HuyBo_", HoaDon[i].InvoiceID);
    }
}

function addSelect(selector, id) {
    $(selector + id).addClass("stt-invoice");
}
function removeSelect(selector, id) {
    $(selector + id).removeClass("stt-invoice");
}

var sttchange;
var ivIDChange;
function updateSTT(invoiceID, status) {
    sttchange = status;
    ivIDChange = invoiceID
    var url = baseUrl + "invoice/SuaSTTHoaDon";
    var data = {
        InvoiceID: invoiceID,
        Status: status
    };
    executePut(url, JSON.stringify(data), updateSTTSuccess, updateSTTError);
}

function updateSTTSuccess(data) {
    if (data) {
        $('#message').html(ivIDChange+" cập nhật thành công");
        (sttchange == 0) ? addSelect("#DonMoi_", ivIDChange) : removeSelect("#DonMoi_", ivIDChange);
        (sttchange == 1) ? addSelect("#XacThuc_", ivIDChange) : removeSelect("#XacThuc_", ivIDChange);
        (sttchange == 2) ? addSelect("#CheBien_", ivIDChange) : removeSelect("#CheBien_", ivIDChange);
        (sttchange == 3) ? addSelect("#VanChuyen_", ivIDChange) : removeSelect("#VanChuyen_", ivIDChange);
        (sttchange == 4) ? addSelect("#HoanTat_", ivIDChange) : removeSelect("#HoanTat_", ivIDChange);
        (sttchange == 5) ? addSelect("#HuyBo_", ivIDChange) : removeSelect("#HuyBo_", ivIDChange);
    }
}

function updateSTTError(data) {
    $('#message').html(ivIDChange + " cập nhật thất bại");
}