var btnTimeout = 100000;
function executePostNew(strUrl, inputData, successFunc, errorFunc, doneFunc, validateFunc, async) {
    executeAjaxNew(strUrl, inputData, successFunc, errorFunc, doneFunc, "POST", validateFunc, async);
}
function executePostNewNon(strUrl, inputData, successFunc, errorFunc, doneFunc, validateFunc, async) {
    executeAjaxNewNoresponse(strUrl, inputData, successFunc, errorFunc, doneFunc, "POST", validateFunc, async);
}
function executePutNew(strUrl, inputData, successFunc, errorFunc, doneFunc, validateFunc, async) {
    executeAjaxNewNoresponse(strUrl, inputData, successFunc, errorFunc, doneFunc, "PUT", validateFunc, async);
}

function executePut(strUrl, inputData, successFunc, errorFunc, doneFunc, validateFunc, async) {
    executeAjaxNew(strUrl, inputData, successFunc, errorFunc, doneFunc, "PUT", validateFunc, async);
}

function executeGetNew(strUrl,inputdata, successFunc, errorFunc, doneFunc, validateFunc, async) {
    executeAjaxNew(strUrl, inputdata, successFunc, errorFunc, doneFunc, "GET", validateFunc, async);
}
function executeDeleteNew(strUrl, successFunc, errorFunc, doneFunc, validateFunc) {
    executeAjaxNewNoresponse(strUrl, '', successFunc, errorFunc, doneFunc, "DELETE", validateFunc);
}

function executeAjaxNew(strUrl, inputData, successFunc, errorFunc, doneFunc, type, validateFunc, async) {
       //Validate if needed
    if (typeof validateFunc != 'undefined' && !validateFunc(inputData)) {
        return;
    }
    setTimeout(function () {
        $.ajax({
            type: type,
            contentType: "application/json;charset=utf-8",
            url: strUrl,
            data: inputData,
            dataType: 'json',
            timeout: btnTimeout,
            async: false,
            success: function (data) {
                console.log("SUCCESS: ", data);
                successFunc(data);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                if (typeof errorFunc != 'undefined') {
                    errorFunc(jqXHR, textStatus, errorThrown);
                }
            },
            done: function (e) {
                if (typeof doneFunc != 'undefined') {
                    doneFunc(e);
                }
                console.log("DONE");
            }
        });
    }, 1);
}

function executeAjaxNewNoresponse(strUrl, inputData, successFunc, errorFunc, doneFunc, type, validateFunc) {
    //Validate if needed
    if (typeof validateFunc != 'undefined') {
        validateFunc(inputData);
        return;
    }
    setTimeout(function () {
        $.ajax({
            type: type,
            contentType: "application/json;charset=utf-8",
            url: strUrl,
            data: inputData,
            timeout: btnTimeout,
            success: function (data) {
                console.log("SUCCESS: ", data);
                successFunc(data);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                if (typeof errorFunc != 'undefined') {
                    errorFunc(jqXHR, textStatus, errorThrown);
                }
            },
            done: function (e) {
                if (typeof doneFunc != 'undefined') {
                    doneFunc(e);
                }
                console.log("DONE");
            }
        });
    }, 1);
}

function executeAjaxUpload(strUrl, inputData, successFunc, errorFunc, doneFunc, validateFunc) {

    //Validate if needed
    if (typeof validateFunc != 'undefined') {
        validateFunc(inputData);
        return;
    }

    $.ajax({
        type: "POST",
        data: inputData,
        enctype: 'multipart/form-data',
        processData: false, // tell jQuery not to process the data
        contentType: false, // tell jQuery not to set contentType
        cache: false,
        url: strUrl,
        dataType: 'json',
        timeout: btnTimeout,
        success: function (data) {
            console.log("SUCCESS: ", data);
            successFunc(data);
        },
        error: function (e) {
            if (typeof errorFunc != 'undefined') {
                errorFunc(e);
            }
            console.log("ERROR: ", e);
        },
        done: function (e) {
            if (typeof doneFunc != 'undefined') {
                doneFunc(e);
            }
            console.log("DONE");
        }
    });
}

function display(header,msg) {
    $('#txtMsgError').html(msg);
    $('#txtHeaderError').html(header);
    $('#errorModalGlobal').modal('show');
}