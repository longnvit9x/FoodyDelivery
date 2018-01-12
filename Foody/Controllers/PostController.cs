using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foody.Models;
using Foody.Models.Dto;
using Foody.Models.Entity;
using System.IO;
using System.Globalization;

namespace Foody.Controllers
{
    public class PostController : Controller
    {
        private FoodDeliveryEntities db;
        public PostController()
        {
            db = new FoodDeliveryEntities();
        }

        public ActionResult BaiVietStore(string StoreID)
        {
            var stID = Guid.Parse(StoreID);
            var lstPost = db.Posts.Where(n => n.StoreID == stID)
                .OrderByDescending(n => n.PostTime).ToList();
            ViewBag.StoreID = StoreID;
            var storeAdmin = 0;
            if (Session["TaiKhoan"] != null)
            {
                Customer cus = (Customer)Session["TaiKhoan"];
                ViewBag.Avatar = cus.FileData.URL;
                ViewBag.CustomerID = cus.CustomerID;
                var store = db.Stores.Where(n => n.StoreID == stID).SingleOrDefault();
                if (store != null && store.CustomerID.Equals(cus.CustomerID))
                {
                    storeAdmin = 1;
                }
            }
            ViewBag.StoreAdmin = storeAdmin;
            return PartialView(lstPost);
        }

        public ActionResult BinhLuan(string StoreID)
        {
            var stID = Guid.Parse(StoreID);
            ViewBag.StoreID = StoreID;
            ViewBag.StoreName = db.Stores.Where(n => n.StoreID == stID).Select(n => n.StoreName).SingleOrDefault();
            return PartialView();
        }

        public ActionResult ThongTinDanhGia(string StoreID)
        {
            ViewBag.StoreID = StoreID;
            return PartialView();
        }

        // ajax
        [HttpPost]
        public JsonResult createBinhluan(BinhLuanEntity binhluan)
        {
            if (binhluan != null && Session["FullName"] != null)
            {
                try
                {
                    Customer cus = (Customer)Session["TaiKhoan"];
                    Post post = new Post
                    {
                        Title = binhluan.Title,
                        Content = binhluan.Content,
                        Rating = binhluan.Rating,
                        StoreID = Guid.Parse(binhluan.StoreID),
                        CustomerID = cus.CustomerID,
                        Like = 0,
                        View = 0,
                        PostTime = DateTime.Now,
                        Status = 0,
                        Hide = 0
                    };
                    if (ModelState.IsValid)
                    {
                        db.Entry(post).State = System.Data.Entity.EntityState.Added;
                        db.SaveChanges();

                    }
                    foreach (var file in binhluan.ListFileUpload)
                    {
                        FileData fileData = saveBase64FileData(file.dataImage, file.fileName, file.fileSize, file.fileType);
                        if (fileData != null)
                        {
                            PostImage postImage = new PostImage
                            {
                                PostID = post.PostID,
                                FileID = fileData.FileID,
                                URL = fileData.URL
                            };
                            db.Entry(postImage).State = System.Data.Entity.EntityState.Added;
                            db.SaveChanges();
                        }
                    }
                    return Json(true);
                }
                catch
                {
                    return Json(false);
                }
            }
            return Json(false);
        }

        public FileData saveBase64FileData(string base64Encoded, string filename, int fileSize, string mimeType)
        {
            try
            {
                String strFileName = DateTime.Today.ToString("ddMMyyyy") + "_" + filename;
                //Convert Base64 Encoded string to Byte Array.
                string convert = base64Encoded.Replace("data:image/png;base64,", String.Empty);
                byte[] imageBytes = Convert.FromBase64String(convert);
                //Save the Byte Array as Image File.
                string filePath = Server.MapPath("/Data/Originals");
                //using (System.IO.File).WriteAllBytes(filePath, imageBytes) ;
                MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                // Convert byte[] to Image
                ms.Write(imageBytes, 0, imageBytes.Length);
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                image.Save(filePath + "/" + strFileName, System.Drawing.Imaging.ImageFormat.Png);
                FileData fileData = new FileData
                {
                    FileType = mimeType,
                    FileName = strFileName,
                    FileSize = fileSize,
                    URL = "/Data/Originals/" + strFileName
                };
                db.Entry(fileData).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();
                return fileData;
            }
            catch
            {
                return null;
            }
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static byte[] Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return base64EncodedBytes;
        }

        [HttpPost]
        public JsonResult like(LikeEntity likeData)
        {
            if (likeData != null && Session["FullName"] != null)
            {
                try
                {
                    Customer cus = (Customer)Session["TaiKhoan"];
                    if (likeData.isLike)
                    {
                        PostLike postLike = new PostLike
                        {
                            PostID = likeData.PostID,
                            CustomerID = cus.CustomerID,
                            LikeTime = DateTime.Now
                        };
                        if (ModelState.IsValid)
                        {
                            db.Entry(postLike).State = System.Data.Entity.EntityState.Added;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        var like = db.PostLikes.Where(n => n.PostID == likeData.PostID && n.CustomerID == cus.CustomerID).SingleOrDefault();
                        if (like == null)
                        {
                            return Json(false);
                        }
                        db.Entry(like).State = System.Data.Entity.EntityState.Deleted;
                        db.SaveChanges();
                    }

                    return Json(true);
                }
                catch
                {
                    return Json(false);
                }
            }
            return Json(false);
        }

        [HttpPost]
        public JsonResult comment(CommentEntity commentData)
        {
            if (commentData != null && Session["FullName"] != null)
            {
                try
                {
                    Customer cus = (Customer)Session["TaiKhoan"];
                    PostComment postComment = new PostComment
                    {
                        PostID = commentData.PostID,
                        CustomerID = cus.CustomerID,
                        CommentContent = commentData.CommentContent,
                        CommentTime = DateTime.Now,
                        CommentLike = 0,
                    };
                    if (ModelState.IsValid)
                    {
                        db.Entry(postComment).State = System.Data.Entity.EntityState.Added;
                        db.SaveChanges();
                    }
                    CommentDto comDto = new CommentDto()
                    {
                        PostCommentID= postComment.PostCommentID,
                        Avatar = cus.FileData.URL,
                        FullName = cus.FullName,
                        CommentContent = postComment.CommentContent,
                        CommentLike = (int)postComment.CommentLike,
                        CommentTime = postComment.CommentTime.ToString()
                    };
                    return Json(comDto);
                }
                catch
                {
                    return Json(null);
                }
            }
            return Json(null);
        }

        [HttpPost]
        public JsonResult deleteComment(int PostCommentID)
        {
            if (PostCommentID != 0 && Session["TaiKhoan"] != null)
            {
                try
                {
                    Customer cus = (Customer)Session["TaiKhoan"];
                    PostComment postComment = db.PostComments.Where(n => n.PostCommentID == PostCommentID).SingleOrDefault();
                    if (postComment != null)
                    {
                        db.Entry(postComment).State = System.Data.Entity.EntityState.Deleted;
                        db.SaveChanges();
                        return Json(PostCommentID);
                    }
                }
                catch
                {
                    return Json(null);
                }
            }
            return Json(null);
        }

        [HttpPost]
        public JsonResult deletePost(int PostID)
        {
            if (PostID != 0 && Session["TaiKhoan"] != null)
            {
                try
                {
                    Customer cus = (Customer)Session["TaiKhoan"];
                    Post post = db.Posts.Where(n => n.PostID == PostID).SingleOrDefault();
                    if (post != null)
                    {
                        List<PostComment> postComment = db.PostComments.Where(n => n.PostID == PostID).ToList();
                        foreach (var comment in postComment)
                        {
                            db.Entry(comment).State = System.Data.Entity.EntityState.Deleted;
                        }
                        List<PostLike> postLike = db.PostLikes.Where(n => n.PostID == PostID).ToList();
                        foreach (var like in postLike)
                        {
                            db.Entry(like).State = System.Data.Entity.EntityState.Deleted;
                        }
                        List<PostImage> postImage = db.PostImages.Where(n => n.PostID == PostID).ToList();
                        foreach (var image in postImage)
                        {
                            db.Entry(image).State = System.Data.Entity.EntityState.Deleted;
                        }
                        db.Entry(post).State = System.Data.Entity.EntityState.Deleted;
                        db.SaveChanges();
                        return Json(PostID);
                    }
                }
                catch
                {
                    return Json(null);
                }
            }
            return Json(null);
        }

        [HttpPost]
        public JsonResult hidePost(int PostID)
        {
            if (PostID != 0 && Session["TaiKhoan"] != null)
            {
                try
                {
                    Customer cus = (Customer)Session["TaiKhoan"];
                    Post post = db.Posts.Where(n => n.PostID == PostID).SingleOrDefault();
                    if (post != null)
                    {
                        post.Hide = 1;
                        db.Entry(post).State = System.Data.Entity.EntityState.Modified;
                        var postLike = db.PostLikes.Where(n => n.PostID == PostID).ToList();
                        db.SaveChanges();
                        return Json(PostID);
                    }
                }
                catch
                {
                    return Json(null);
                }
            }
            return Json(null);
        }
    }
}