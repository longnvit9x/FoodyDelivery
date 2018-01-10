var baseUrl = $("#BaseUrl").data("baseurl");
var storeID = $("#BaseUrl").data("storeid");
var htmlPicture = "";
var globalFilePicture = [];
var fileMime = {};
var listFile = [];
function performClick(elemId) {
    $('#' + elemId).bind('change', function () {
        var fileType = this.files[0].type;

        selectedDataImage = null;
        if (fileType.startsWith('image')) {
            var fileSize = this.files[0].size;
            var fileName = this.files[0].name;
            if (fileSize > 1000000000) {
                fileSize = Math.ceil(fileSize / 1000000000) + "GB";
            } else if (fileSize > 1000000) {
                fileSize = Math.ceil(fileSize / 1000000) + "MB";
            } else if (fileSize > 1000) {
                fileSize = Math.ceil(fileSize / 1000) + "KB";
            } else if (fileSize > 1) {
                fileSize = Math.ceil(fileSize) + "B";
            } else {
                fileSize = "Unidentified";
            }
            selectedFile = this.files[0];
            selectedFile.toJSON = function () {
                return {
                    'lastModified': selectedFile.lastModified,
                    'lastModifiedDate': selectedFile.lastModifiedDate,
                    'name': selectedFile.name,
                    'size': selectedFile.size,
                    'type': selectedFile.type
                };
            }
            //fileMime.lastModified = selectedFile.lastModified;
            //fileMime.lastModifiedDate = selectedFile.lastModifiedDate;
            //fileMime.name = selectedFile.name;
            //fileMime.size = selectedFile.size;
            //fileMime.type = selectedFile.type;
            var readerBase64 = new FileReader();
            var src = URL.createObjectURL(selectedFile);
            readerBase64.readAsDataURL(selectedFile);
            readerBase64.onload = function () {
                selectedDataImage = readerBase64.result;
                var filedata = {
                    fileName: selectedFile.name,
                    fileSize: selectedFile.size,
                    fileType: selectedFile.type,
                    dataImage: selectedDataImage
                };
                if (listFile.filter(n=> n.dataImage == filedata.dataImage).length == 0)
                listFile.push(filedata);
            };
            if (globalFilePicture.filter(n=>n == this.files[0]).length == 0) {
                globalFilePicture.push(this.files[0]);
                htmlPicture = htmlPicture + " <li class='item-picture'> <div class='c-pic'>"
                     + "<div class='btn-remove'>"
                      + "<i class='fa fa-times'></i> </div>"
                       + "<img src='" + src + "' class='picture-upload'/>"
                     + "  </div> </li>";
            }

        }
        $("#listpicture").html(htmlPicture);

    });
    var elem = document.getElementById(elemId);
    if (elem && document.createEvent) {
        var evt = document.createEvent("MouseEvents");
        evt.initEvent("click", true, false);
        elem.dispatchEvent(evt);
    }
}

function submitBinhLuan() {
    var url = baseUrl + "post/createBinhluan";
    var data = {
        Title: $("#title").val(),
        Content: $("#comment").val(),
        Rating:($("#rating-value").html()!=null)?$("#rating-value").html():0,
        StoreID: storeID,
        ListFileUpload: listFile
    };
    executePostNew(url, JSON.stringify(data), createBinhLuanSuccess, createBinhLuanError);
}

function createBinhLuanSuccess(data) {
    if (data) {
         htmlPicture = "";
         globalFilePicture = [];
         fileMime = {};
         listFile = [];
         $("#listpicture").html(htmlPicture);
    } else
        $('#message-binhluan').val("Đăng bài thất bại");
}

function createBinhLuanError() {
    $('#message-binhluan').val("Đăng bài thất bại");
}


