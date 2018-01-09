var baseUrl = $("#BaseUrl").data("baseurl");
$(document).ready(function ($) {
    loadStore();
});

function loadStore() {
    var url = baseUrl + "Administrator/AdminStore/getAll";
    executeGetNew(url,null, loadStoreSuccess, loadStoreError);
}
function loadStoreSuccess(data) {
    populateStore(data);
}

function loadStoreError(data) {
    console.log("error");
}

function populateStore(Store) {
    for (var i = 0; i < Store.length; i++) {
        (Store[i].Status == 0) ? addSelect("#ChuaXacThuc_", Store[i].StoreID) : removeSelect("#ChuaXacThuc_", Store[i].StoreID);
        (Store[i].Status == 1) ? addSelect("#XacThuc_", Store[i].StoreID) : removeSelect("#XacThuc_", Store[i].StoreID);
    }
}

function addSelect(selector, id) {
    $(selector + id).addClass("stt-store");
}
function removeSelect(selector, id) {
    $(selector + id).removeClass("stt-store");
}


var sttchange;
var stIDChange;
function updateSTT(storeID, status) {
    sttchange = status;
    stIDChange = storeID
    var url = baseUrl + "Administrator/AdminStore/SuaSTTStore";
    var data = {
        StoreID: storeID,
        Status: status
    };
    executePut(url, JSON.stringify(data), updateSTTSuccess, updateSTTError);
}

function updateSTTSuccess(data) {
    if (data) {
        $('#message').html(stIDChange + " cập nhật thành công");
        (sttchange == 0) ? addSelect("#ChuaXacThuc_", stIDChange) : removeSelect("#ChuaXacThuc_", stIDChange);
        (sttchange == 1) ? addSelect("#XacThuc_", stIDChange) : removeSelect("#XacThuc_", stIDChange);
        display("Thông báo", stIDChange + " cập nhật thành công")
    }
}

function updateSTTError(data) {
    display("Xảy ra lỗi", stIDChange + " cập nhật thất bại")
}