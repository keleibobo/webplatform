using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SVGControlDefine
/// </summary>
/// 
namespace AppCode{
public class SVGControlDefine
{
	public SVGControlDefine(string appname)
	{
		//
		// TODO: Add constructor logic here
		//

        string temp = "";
        temp = ReadConfig.TheReadConfig["refreshTree"];
        if (temp != null && !temp.Equals(""))
        {
           refTree = Convert.ToInt32(temp);
        }

        temp = ReadConfig.TheReadConfig["refreshData"];
        if (temp != null && !temp.Equals(""))
        {
            refData = Convert.ToInt32(temp);
        }

        string ifLogin = ReadConfig.TheReadConfig["iflogin"];
        if (ifLogin.Equals("false"))
        {
            loginId = ReadConfig.TheReadConfig["appname"];
        }
        else
        {
            loginId = appname;
        }
        showtype = ReadConfig.TheReadConfig["showtype"];

	}


    public string appname = "";


    public string loginId = "joyo-j"; //默认 test
    public string strFileName = "";
   
    public String lasttime = "";
    public int refData = 0, refTree = 0;
    public string showtype;

  //  public string ID = "18";

 //   public string[] treeInfo;//导航信息/所有svg文件信息

 //   public string sval = "郭家岗变控制室"; //加载的svg文件 名 加载页面时由js调用后台
}
}