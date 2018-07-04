using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Web;

namespace CRMOZ.Web.Common
{
    public static class MediaStore
    {
        // Ham lay duong dan tuyet doi cua file
        public static InfoSaveFile GetAbsolutetPath(string folderName, string fileName)
        {
            InfoSaveFile infoSaveFile = new InfoSaveFile();
            //Lay ra duong dan tuyet doi toi thu muc chua file
            string rootPath = HttpContext.Current.Server.MapPath("/" + folderName);
            // Tao thu muc co ten la ngay thang nam hien tai
            string newFolder = string.Format("{0:ddMMyyyy}", DateTime.Now);
            // Lay ra duong dan tuyet doi cua thu muc vua moi tao
            var folderPath = Path.Combine(rootPath, newFolder);

            // Duong dan tuyet doi cua file
            var imagePath = Path.Combine(folderPath, fileName);

            //Neu thu muc chua ton tai
            if (!Directory.Exists(folderPath))
            {
                try
                {
                    // Tao moi thu muc
                    Directory.CreateDirectory(folderPath);
                }
                catch (Exception ex)
                {
                    infoSaveFile.FileUrl = string.Empty;
                    infoSaveFile.AbsolutePath = string.Empty; ;
                    infoSaveFile.Message = ex.Message;
                    infoSaveFile.Status = false;
                }
            }
            string fileUrl = string.Format("/{0}/{1}/{2}", folderName, string.Format("{0:ddMMyyyy}", DateTime.Now), fileName);
            infoSaveFile.FileUrl = fileUrl;
            infoSaveFile.AbsolutePath = imagePath;
            infoSaveFile.Message = string.Empty;
            infoSaveFile.Status = true;
            return infoSaveFile;
        }

        // Ham sesize image
        public static InfoSaveFile SaveResizeImage(HttpPostedFileBase file, InfoSaveFile infoSaveFile, int width)
        {
            try
            {
                //Lay ra hinh anh
                Image img = Image.FromStream(file.InputStream);

                // lấy chiều rộng và chiều cao ban đầu của ảnh
                int originalW = img.Width;
                int originalH = img.Height;
                if (originalW >= width)
                {
                    // lấy chiều rộng và chiều cao mới tương ứng với chiều rộng truyền vào của ảnh 
                    //(nó sẽ giúp ảnh của chúng ta sau khi resize vần giứ được độ cân đối của tấm ảnh
                    int resizedW = width;
                    int resizedH = (originalH * resizedW) / originalW;

                    using (Bitmap b = new Bitmap(resizedW, resizedH))
                    {
                        Graphics g = Graphics.FromImage((Image)b);
                        g.InterpolationMode = InterpolationMode.Bicubic;    // Specify here
                        g.DrawImage(img, 0, 0, resizedW, resizedH);
                        g.Dispose();

                        b.Save(infoSaveFile.AbsolutePath);
                        img.Dispose();
                        b.Dispose();

                        infoSaveFile.AbsolutePath = string.Empty;
                        infoSaveFile.Message = "Lưu hình ảnh thành công!";
                        infoSaveFile.Status = true;
                    }
                }
                else
                {
                    file.SaveAs(infoSaveFile.AbsolutePath);
                    infoSaveFile.AbsolutePath = string.Empty;
                    infoSaveFile.Message = "Lưu hình ảnh thành công!";
                    infoSaveFile.Status = true;
                }

                return infoSaveFile;

            }
            catch (Exception ex)
            {
                infoSaveFile.AbsolutePath = string.Empty;
                infoSaveFile.Message = ex.Message;
                infoSaveFile.Status = false;
                return infoSaveFile;
            }

        }

        // Ham luu file
        public static InfoSaveFile SaveFile(HttpPostedFileBase file, string folder)
        {
            InfoSaveFile infoSaveFile = null;
            if (file != null && file.ContentLength > 0)
            {
                string fileName = file.FileName;
                // Random va tao ten file moi
                string randomName = Guid.NewGuid() + Path.GetExtension(fileName);
                // Lay ra duong dan tuyet doi cua file
                infoSaveFile = GetAbsolutetPath(folder, randomName);

                if (infoSaveFile.Status == true)
                {
                    try
                    {
                        file.SaveAs(infoSaveFile.AbsolutePath);
                        infoSaveFile.AbsolutePath = string.Empty;
                        infoSaveFile.Message = "Lưu file thành công!";
                    }
                    catch (Exception ex)
                    {
                        infoSaveFile.AbsolutePath = string.Empty;
                        infoSaveFile.Message = ex.Message;
                        infoSaveFile.Status = false;
                        return infoSaveFile;
                    }
                }

                return infoSaveFile;
            }

            infoSaveFile = new InfoSaveFile() { FileUrl = string.Empty, AbsolutePath = string.Empty, Message = "Chưa có file nào được chọn!", Status = false };
            return infoSaveFile;
        }

        // Ham luu hinh anh
        public static InfoSaveFile SaveImage(HttpPostedFileBase file, string folder, int with)
        {
            InfoSaveFile infoSaveFile = null;
            if (file != null && file.ContentLength > 0)
            {
                string fileName = file.FileName;

                // Random va tao ten file moi
                string randomName = Guid.NewGuid() + Path.GetExtension(fileName);

                infoSaveFile = GetAbsolutetPath(folder, randomName);

                if (infoSaveFile.Status == true)
                {
                    // Resize image va luu lai
                    infoSaveFile = SaveResizeImage(file, infoSaveFile, with);
                    return infoSaveFile;
                }

                return infoSaveFile;
            }

            infoSaveFile = new InfoSaveFile() { FileUrl = string.Empty, AbsolutePath = string.Empty, Message = "Chưa có file nào được chọn!", Status = false };
            return infoSaveFile;
        }

        public static InfoSaveFile RemoveFile(string url)
        {
            InfoSaveFile infoSaveFile = new InfoSaveFile();
            try
            {
                //Lay ra duong dan tuyet doi toi thu muc chua file
                string filePath = HttpContext.Current.Server.MapPath(url);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                infoSaveFile.FileUrl = url;
                infoSaveFile.AbsolutePath = string.Empty;
                infoSaveFile.Message = "Xóa file thành công!";
                infoSaveFile.Status = true;
                return infoSaveFile;
            }
            catch (Exception ex)
            {
                infoSaveFile.FileUrl = string.Empty;
                infoSaveFile.AbsolutePath = string.Empty;
                infoSaveFile.Message = ex.Message;
                infoSaveFile.Status = false;
                return infoSaveFile;
            }
        }
    }
}