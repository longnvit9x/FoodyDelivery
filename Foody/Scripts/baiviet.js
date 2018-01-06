$(document).ready(function () {


});

var likeChange;
var PostIDLike;
function btnLike(postID) {
    PostIDLike = postID;
    likeChange = !$("#iconLike_" + PostIDLike).hasClass("fa-thumbs-up");
    var url = baseUrl + "post/like";
    var data = {
        PostID: PostIDLike,
        isLike: likeChange
    };
    executePostNew(url, JSON.stringify(data), likeSuccess, likeError);
}

function likeSuccess(data) {

    if (data) {
        var number = 0;
        if ($("#numberLike_" + PostIDLike).html() != null) {
            number = parseInt($("#numberLike_" + PostIDLike).html());
        }
        if (likeChange) {
            $("#iconLike_" + PostIDLike).addClass("fa-thumbs-up").removeClass("fa-thumbs-o-up");
            $("#numberLike_" + PostIDLike).html(number + 1);
        } else {
            $("#iconLike_" + PostIDLike).addClass("fa-thumbs-o-up").removeClass("fa-thumbs-up");
            $("#numberLike_" + PostIDLike).html(number - 1);
        }
    }
}

function likeError() {
    $('#message-binhluan').val("Like thất bại");
}

function btnBinhluan(PostID) {
    $("#txtComment_" + PostID).focus();
}
var PostIDChange;
function submitComment(postID) {
    if (this.event.defaultPrevented) {
        return;
    }
    PostIDChange = postID;
    var keycode = (this.event.keyCode ? this.event.keyCode : this.event.which);
    if (keycode == 13) {
        var url = baseUrl + "post/comment";
        var data = {
            PostID: PostIDChange,
            CommentContent: $("#txtComment_" + postID).val()
        };
        executePostNew(url, JSON.stringify(data), commentSuccess, commentError);
        return true;
    }
    return false;
}

function commentSuccess(data) {
    if (data != null) {
        var htmlComment = $("#listComment_" + PostIDChange).html();
        var addhtml = "<div class='comment'>" +
                      " <img src=" + data.Avatar + " alt='' />  <div class='content'>" +
                      "<h3><a href=''>" + data.FullName + "</a></h3><p>" +
                              data.CommentContent + "</p><span><time>" + data.CommentTime
                              + " </time><a href='#'>" + data.CommentLike + " thích</a></span></div></div>";
        $("#listComment_" + PostIDChange).html(htmlComment + addhtml);
        $("#txtComment_" + PostIDChange).val('');
        var numberCM = 0;
        if ($("#numberLike_" + PostIDLike).html() != null) {
            numberCM = parseInt($("#numberComment_" + PostIDChange).html());
        }
        $("#numberComment_" + PostIDChange).html(numberCM + 1);
    }
}

function commentError() {
    $('#message-binhluan').val("Bình luận thất bại");
}

