using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using UTUtil;

public partial class Public_uploadfile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Stream stream = Request.InputStream;

        byte[] NameLength = new byte[4];
        stream.Read(NameLength, 0, 4);
        int Length = BitConverter.ToInt32(NameLength, 0);

        byte[] NameByte = new byte[Length];
        stream.Read(NameByte, 0, Length);

        string paraminfo = System.Text.Encoding.Default.GetString(NameByte);
        Dictionary<string, string> dt = new Dictionary<string, string>();
        UtilFunc.GetParaToDictory(paraminfo, dt, ";", true);
        string filename = dt["filename"];
        string uploadFolder = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "uploadfiles");
        string destpath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "apppage\\");
        if (!Directory.Exists(uploadFolder))
        {
            Directory.CreateDirectory(uploadFolder);
        }
        uploadFolder = Path.Combine(uploadFolder, filename);
        FileStream targetStream = null;
        try
        {
            using (targetStream = new FileStream(uploadFolder, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                byte[] buffer = new byte[4048];
                int count = 0;
                long lFileSize = 0;
                while ((count = stream.Read(buffer, 0, 4048)) > 0)
                {
                    lFileSize = lFileSize + count;
                    targetStream.Write(buffer, 0, count);
                }
                targetStream.Flush();
                targetStream.Dispose();
                stream.Close();
            }
            string message = "";
            UTUtil.MyZip.UnZipFile(uploadFolder, destpath, out message);
        }
        catch (Exception ex)
        {

        }
    }
}