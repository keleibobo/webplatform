using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Data.SqlClient;
using System.Web.UI;

/// <summary>
///MyImage 的摘要说明
/// </summary>AppCode
namespace AppCode
{
    public static class MyImage
    {
        static MyImage()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
        public static int iWidth = 358;
        public static int iHeight = 441;
        static int iQty = 70;
        /// <summary>   
        /// 保存JPG时用   
        /// </summary>   
        /// <param name="mimeType">文件类型</param>   
        /// <returns>得到指定mimeType的ImageCodecInfo</returns>   
        private static ImageCodecInfo GetCodecInfo(string mimeType)
        {
            ImageCodecInfo[] CodecInfo = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo ici in CodecInfo)
            {
                if (ici.MimeType == mimeType) return ici;
            }
            return null;
            
        }
        public static long CompressPicture(byte[] bSrc, out byte[] bPhoto)
        {
            bPhoto = null;
            try
            {
                System.IO.MemoryStream ms1 = new System.IO.MemoryStream(bSrc);
                Bitmap bmpSrc = new Bitmap(ms1);
                Bitmap bmp1 = new Bitmap(bmpSrc, iWidth, iHeight);
                bmp1.SetResolution(350, 350);
                // 压缩图片
                EncoderParameter p;
                EncoderParameters ps;
                ps = new EncoderParameters(1);
                p = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, iQty);
                ps.Param[0] = p;
                MemoryStream ms2 = new MemoryStream();
                bmp1.Save(ms2, GetCodecInfo("image/jpeg"), ps);
                bPhoto = ms2.ToArray();
                return ms2.Length;
            }
            catch{}
            return 0;
        }
        public static long SmallPic(byte[] bSrc, out byte[] bPhoto,int Width,int Height)
        {
            bPhoto = null;
            try
            {
                System.IO.MemoryStream ms1 = new System.IO.MemoryStream(bSrc);
                Bitmap bmpSrc = new Bitmap(ms1);
                Bitmap bmp1 = new Bitmap(bmpSrc, Width, Height);
                //bmp1.SetResolution(350, 350);
                // 压缩图片
                EncoderParameter p;
                EncoderParameters ps;
                ps = new EncoderParameters(1);
                p = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, iQty);
                ps.Param[0] = p;
                MemoryStream ms2 = new MemoryStream();
                bmp1.Save(ms2, GetCodecInfo("image/jpeg"), ps);
                bPhoto = ms2.ToArray();
                return ms2.Length;
            }
            catch (Exception e)
            {
                string s = e.Message;
            }
            return 0;
        }

        public static void SmallPic(string sSrc, out byte[] bPhoto, int Width, int Height)
        {
            Image img = Image.FromFile(sSrc);
            MemoryStream ms = new MemoryStream();
            img.Save(ms, ImageFormat.Jpeg);
            byte[] bytes = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(bytes, 0, Convert.ToInt32(ms.Length));
            SmallPic(bytes, out bPhoto, Width,Height);
        }

    }
}
