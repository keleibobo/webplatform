using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UTDtBaseSvr;
using System.Text;

/// <summary>
/// Summary description for ULNav
/// </summary>
/// 
namespace AppCode
{
    public class ULNav
    {
        List<object> menuList = null;
        Dictionary<string, List<object>> menuchildren = new Dictionary<string, List<object>>();
        public string nav = "";
        private string _appname = "";
        private string _rolename = "";
        public ULNav(string appname,string rolename)
        {
            this._appname = appname;
            this._rolename = rolename;
            menuList = InitModel.GetUserMenuByRoleID(this._appname, this._rolename);
            toHTML();
        }

        public void toHTML()
        {
            Dictionary<String, List<object>> menuinfos = new Dictionary<string, List<object>>();

            menuinfos = new Dictionary<string, List<object>>();
            if (menuList != null)
                foreach (UserMenuInfo umi in menuList)
                {
                    string pid = umi.pathlevel;
                    if (menuinfos.ContainsKey(pid))
                    {
                        List<object> infoList = menuinfos[pid];
                        infoList.Add(umi);
                    }
                    else
                    {
                        List<object> infoList = new List<object>();
                        infoList.Add(umi);
                        menuinfos.Add(pid, infoList);
                    }
                }

          
            if (menuList != null)
                foreach (UserMenuInfo umi in menuList)
                {
                    string pid = umi.parentname;
                    if (menuchildren.ContainsKey(pid))
                    {
                        List<object> infoList = menuchildren[pid];
                        infoList.Add(umi);
                    }
                    else
                    {
                        List<object> infoList = new List<object>();
                        infoList.Add(umi);
                        menuchildren.Add(pid, infoList);
                    }
                }


            if (menuinfos.ContainsKey("1"))
            {
                List<object> menu = menuinfos["1"];
                nav = toLI(menu, "topnav");
            }
        }


        public String toLI(List<object> menuList,string navclass)
        {
            if (menuList == null) return "";
            menuList.Sort(new MenuInfoComparer());
            StringBuilder sb = new StringBuilder();
            sb.Append(String.Format("<ul class='{0}'>",navclass));
            
            foreach (UserMenuInfo umi in menuList)
            {
                string name = umi.name;
                string parentname = umi.parentname;
                string Des = umi.desc;
                string link = umi.link;
                string id = umi.id;
                string onclick = String.Format(" onclick=\"f_menuclick('{0}',this,'&nbsp;>&nbsp;{1}&nbsp;>>&nbsp;{1}')\" ", name, parentname, Des);
                string surl = String.Format(" surl=\"{0}?type={1}&id={4}&SelectNodeName=&nbsp;>&nbsp;{3}&nbsp;>>&nbsp;{2} \" ", link, name, System.Web.HttpUtility.UrlEncode(Des), System.Web.HttpUtility.UrlEncode(parentname), System.Web.HttpUtility.UrlEncode(id));
                string mouse = "";// " onmouseover=\"Menu_HoverDynamic(this)\" onmouseout=\"Menu_Unhover(this)\" onkeyup=\"Menu_Key(this)\" ";
 
               // sb.Append(String.Format("<li><a href='{0}'>{1}</a>",umi.link,umi.desc));
                sb.Append(String.Format("<li {2} {3} {4}  style='white-space: nowrap;'><a class='list2' href='{0}'>{1}</a>", "#", Des, mouse, onclick, surl));
                ///子
                ///
                string pname = umi.name;

                if (!pname.Equals("")&&menuchildren.ContainsKey(pname))
                {
                    sb.Append(toLI(menuchildren[pname], "subnav"));
                }
                sb.Append("</li>");
            }
            sb.Append("</ul>");

            return sb.ToString();
        }
    }
}