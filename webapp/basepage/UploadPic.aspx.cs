using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
 

using System.Web.UI;
using System.Web.UI.WebControls;
using AppCode;

public partial class Public_UploadPic : System.Web.UI.Page
{
    string sdirpath;
    protected void Page_Load(object sender, EventArgs e)
    {
        sdirpath=Request["DirPath"];
    }
    private string data(string spath)
    {
        HttpFileCollection MyFileColl = Request.Files;
        if (MyFileColl.Count > 0)
        {
            string UploadFileMsg = Upload(MyFileColl, spath);
            if (UploadFileMsg == "上传成功")//判断上传的文件是否为空
            {
                for (int iLoop = 0; iLoop < MyFileColl.Count; iLoop++)
                {
                    if (MyFileColl[iLoop].ContentLength > 0)
                    {
                        HttpPostedFile postedFile = MyFileColl[iLoop];
                        string sFileName = System.IO.Path.GetFileName(postedFile.FileName);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "11", "<script type='text/javascript'>parent.fileUpload('" + sFileName + "');</script>");
                        return sFileName;
                    }
                }
            }
            else
            {
                return UploadFileMsg;
            }
        }
        return "";
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        string spath = Server.MapPath("~/").Replace(@"\", @"/") + sdirpath;
        string filename = data(spath);
        labFileName.Text = filename;
        img1.ImageUrl = "photo.aspx?imgsrc=" + HttpUtility.UrlEncode(sdirpath + filename);
        img1.Visible = true;
    }

    /// <summary>
    /// 通用多个文件上传函数
    /// </summary>
    /// <param name="MyFileCollection">要上传的文件群(MyFileCollection=Request.Files)</param>
    /// <param name="sPath">保存上传文件的路径</param>
    /// <returns>返回文件是否上传成功</returns>	
    public static string Upload(HttpFileCollection MyFileCollection, string sPath)
    {
        string NewFile = "";
        string filename = "";
        HttpContext context = HttpContext.Current;
        string sNewPath = context.Server.MapPath(sPath);
        try
        {

            for (int iFile = 0; iFile < MyFileCollection.Count; iFile++)
            {
                filename = MyFileCollection[iFile].FileName;
                if ((MyFileCollection[iFile].ContentLength / 1024) > 15000)
                {

                }
                if (MyFileCollection[iFile].ContentLength > 0)
                {
                    int i = filename.LastIndexOf("\\");
                    NewFile = filename.Substring(i).ToLower();
                    if (NewFile.IndexOf(".jpg") < 0 && NewFile.IndexOf(".gif") < 0 && NewFile.IndexOf(".png") < 0 && NewFile.IndexOf(".bmp") < 0)
                    {
                        return "上传失败，只能上传图片文件";
                    }
                    MyFileCollection[iFile].SaveAs(sNewPath + NewFile);
                }
            }
        }
        catch (Exception Err)
        {
            return "上传失败," + Err.Message.ToString() + sNewPath;

        }
        return "上传成功";
    }
}