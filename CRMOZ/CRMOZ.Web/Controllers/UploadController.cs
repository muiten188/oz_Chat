using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRMOZ.Web.Controllers
{
    public class UploadController : Controller
    {
        [HttpPost]
        public JsonResult UploadImage()
        {
            string strUrl = string.Empty;
            foreach (string fileName in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[fileName];
                string fName = file.FileName;
                if (file != null && file.ContentLength > 0)
                {
                    var originalDirectory = new DirectoryInfo(string.Format("{0}Uploads", Server.MapPath(@"\")));
                    string pathString = originalDirectory.ToString();

                    bool isExists = Directory.Exists(pathString);
                    if (!isExists)
                        Directory.CreateDirectory(pathString);

                    string RandomName = Guid.NewGuid() + Path.GetExtension(fName);
                    var path = string.Format("{0}\\{1}", pathString, RandomName);
                    bool result = SaveResizeImage(Image.FromStream(file.InputStream), 200, path);
                    if (result == false)
                    {
                        file.SaveAs(path);
                    }

                    strUrl = "/Uploads/" + RandomName;
                }
            }

            return Json(new { data = strUrl }, JsonRequestBehavior.AllowGet);
        }

        public bool SaveResizeImage(Image img, int width, string path, string oldPath = "")
        {
            try
            {
                // lấy chiều rộng và chiều cao ban đầu của ảnh
                int originalW = img.Width;
                int originalH = img.Height;
                if (originalW >= width)
                {
                    // lấy chiều rộng và chiều cao mới tương ứng với chiều rộng truyền vào của ảnh (nó sẽ giúp ảnh của chúng ta sau khi resize vần giứ được độ cân đối của tấm ảnh
                    int resizedW = width;
                    int resizedH = (originalH * resizedW) / originalW;
                    Bitmap b = new Bitmap(resizedW, resizedH);
                    Graphics g = Graphics.FromImage((Image)b);
                    g.InterpolationMode = InterpolationMode.Bicubic;    // Specify here
                    g.DrawImage(img, 0, 0, resizedW, resizedH);
                    g.Dispose();
                    b.Save(path);
                    img.Dispose();
                    b = null;

                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}