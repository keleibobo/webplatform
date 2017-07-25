using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using UTDtBaseSvr;

/// <summary>
/// Summary description for TableNav
/// </summary>
/// 
namespace AppCode
{
    public class TableNav
    {
        int mainCount = 0;
        public string nav = "";
        Dictionary<String, List<object>> childreninfos = new Dictionary<string, List<object>>();
        Dictionary<string, string> namedes = new Dictionary<string, string>();
        public string homepage = "";
        private string _appname = "";
        private string _roles = "";
        public string style = ReadConfig.TheReadConfig["style"];


        public TableNav(string appname, string roles)
        {
            //
            // TODO: Add constructor logic here
            //
            this._appname = appname;
            this._roles = roles;
            toHTML();
        }

       


        public void toHTML()
        {
            List<object> menuList = InitModel.GetUserMenuByRoleID(this._appname, this._roles);
            Dictionary<String, List<object>> menuinfos = new Dictionary<string, List<object>>();
            childreninfos = new Dictionary<string, List<object>>();
            if (menuList != null)
                foreach (UserMenuInfo umi in menuList)
                {
                    namedes.Add(umi.name,umi.desc);
                    ///层级
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
                    ///父子
                    pid = umi.parentname;
                    if (childreninfos.ContainsKey(pid))
                    {
                        List<object> infoList = childreninfos[pid];
                        infoList.Add(umi);
                    }
                    else
                    {
                        List<object> infoList = new List<object>();
                        infoList.Add(umi);
                        childreninfos.Add(pid, infoList);
                        
                    }
                }

            if (menuinfos.ContainsKey("1"))
            {
                List<object> menu = menuinfos["1"];
                DataTable mainMenuTable = toDataTable(menu);
             //   nav=toMenubutton(childreninfos);
                nav = toTR(mainMenuTable);//第一级菜单
                nav += toChild(mainMenuTable);//子菜单
            }
        }

        private string toMenubutton(Dictionary<string, List<object>> childreninfos)
        {
           
            List<object> menues = childreninfos[""];
            StringBuilder childmenu = new StringBuilder();
            StringBuilder sb = new StringBuilder();
            StringBuilder script = new StringBuilder();
            sb.Append("<div style='padding:5px;border:1px solid #ddd;background-color: #A9DFE7'>");
            script.Append("<script type='text/javascript'>");
            foreach (UserMenuInfo  menu in menues)
            {
           
                if (childreninfos.ContainsKey(menu.name))
                {
                    childmenu.Append(getChildMenu(childreninfos, menu.name));
         
                  
                    sb.Append(string.Format("<a  id='a{0}'>{1}</a>", menu.name, menu.desc));

                    string click = @"
var ddlMenu = $('#a{0}').menubutton({{ menu: '#{0}0' }}); 

$(ddlMenu.menubutton('options').menu).menu({{
            onClick: function (item) {{
                alert(item.text);
 {1}
            }}
}})
";
                    //string selectNodeName = "";
                    //if (menu.parentname.Equals(""))
                    //{
                    //    selectNodeName = "&nbsp;>&nbsp;" + System.Web.HttpUtility.UrlEncode(menu.desc); //只有一级
                    //}
                    //else
                    //{
                    //    selectNodeName = String.Format("&nbsp;>&nbsp;{1}&nbsp;>>&nbsp;{0}", System.Web.HttpUtility.UrlEncode(menu.desc), System.Web.HttpUtility.UrlEncode(menu.parentname));
                    //}
                    //string surl = String.Format("\"{0}?type={1}&id={2}&SelectNodeName={3}\"", menu.link, menu.name, System.Web.HttpUtility.UrlEncode(menu.id), selectNodeName);

                    //string clickfunc = String.Format("menuclick('{0}','{1}')", menu.name, surl);
             
                    string clickfunc = String.Format("f_menuclick('{0}',this,'&nbsp;>&nbsp;{1}&nbsp;>>&nbsp;{2}')", menu.name, menu.parentname, menu.desc);
                 
                    script.Append(string.Format(click,menu.name,clickfunc));

                }
                else
                {
      

                    sb.Append(string.Format("<a href='{0}' class='easyui-linkbutton' data-options=\"plain:true\">{1}</a>", menu.link, menu.desc));

                }
              
               
            }


            sb.Append("</div>");
            sb.Append(childmenu);
            sb.Append(script).Append("</script>");
            return sb.ToString();
        }

        public string getChildMenu(Dictionary<string, List<object>> childreninfos,string pname)
        {
            StringBuilder sb = new StringBuilder();
            List<object> menues=childreninfos[pname];

          

            sb.Append(String.Format("<div id='{0}0' style='width:150px'>",pname));

            foreach(UserMenuInfo menu in menues){
                if(childreninfos.ContainsKey(menu.name)){
                    sb.Append(String.Format("<div><span>{0}</span>{1}</div>",menu.desc,getChildMenu(childreninfos,menu.name)));
                
                }else{
                    string selectNodeName = "";
                    if (menu.parentname.Equals(""))
                    {
                        selectNodeName = "&nbsp;>&nbsp;" + System.Web.HttpUtility.UrlEncode(menu.desc); //只有一级
                    }
                    else
                    {
                        selectNodeName = String.Format("&nbsp;>&nbsp;{1}&nbsp;>>&nbsp;{0}", System.Web.HttpUtility.UrlEncode(menu.desc), System.Web.HttpUtility.UrlEncode(menu.parentname));
                    }

                    string surl = String.Format("\"{0}?type={1}&id={2}&SelectNodeName={3}\"", menu.link, menu.name, System.Web.HttpUtility.UrlEncode(menu.id), selectNodeName);

                  
                    sb.Append(String.Format("<div name={1}>{0}</div>",menu.desc,surl)); //
                }
            }
            sb.Append("</div>");
            return sb.ToString();
        }

        public String toTR(DataTable MenuTable)
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            mainCount = MenuTable.Rows.Count;
            sb.Append("<table id='Menu1' class='Menu1_2' cellpadding='0' cellspacing='0' border='0' style='height:37px'>");
            sb.Append("<tr>");
            foreach (DataRow dr in MenuTable.Rows)
            {
                sb.Append("<td><img src='../images/"+style+"/j_line2.gif' width='12' height='19' /> </td> ");
                String td = toTD(dr, i);
                sb.Append(td);
                i++;
            }

            sb.Append("<td><img src='../images/"+style+"/j_line2.gif' width='12' height='19' /> </td></tr></table>");

            return sb.ToString(); ;
        }

        public string toTD(DataRow dr, int i)
        {
            StringBuilder td = new StringBuilder();
            string onclick = " onclick=\" f_menuclick('{0}',this,'&nbsp;>&nbsp;{1}nbsp;>>&nbsp;{2}')\"";
            onclick = String.Format(onclick, dr["name"], dr["parentname"], dr["Des"]);
            string link = dr["link"].ToString();//"../public/query.aspx";//
            string selectNodeName = "";
            if (dr["parentname"].ToString().Equals(""))
            {
                selectNodeName = "&nbsp;>&nbsp;" + System.Web.HttpUtility.UrlEncode(dr["Des"].ToString()); //只有一级
            }
            else
            {
                selectNodeName = String.Format("&nbsp;>&nbsp;{1}&nbsp;>>&nbsp;{0}", System.Web.HttpUtility.UrlEncode(dr["Des"].ToString()), System.Web.HttpUtility.UrlEncode(dr["parentname"].ToString()));
            }
            int l = dr["Des"].ToString().Length;
            string surl = String.Format(" surl=\"{0}?type={1}&id={2}&SelectNodeName={3}\"", link, dr["name"].ToString(), System.Web.HttpUtility.UrlEncode(dr["id"].ToString()), selectNodeName);
            td.Append(String.Format("<td onmouseover=\"Menu_HoverStatic(this,'{0}')\" style='width: 120px' id='Menu1n{1}' ", mainCount, i))
                .Append(onclick)
                //  dr["name"].ToString()',this,'<%="&nbsp;>&nbsp;" + dr["parentname"].ToString() + "&nbsp;>>&nbsp;" + dr["Des"].ToString()%>')"
                   .Append(surl)//<%=%>

                    .Append(" align='center' onmouseout=\"Menu_Unhover(this)\" onkeyup=\"Menu_Key(this)\">")//onmouseout="Menu_Unhover(this)" onkeyup="Menu_Key(this)" id="<%= "Menu1n"+i.ToString() %>">

                     //  <%= dr["Des"].ToString()%>
                      .Append(String.Format("<table cellpadding='0' cellspacing='0' border='0'> <tr>  <td style='white-space: nowrap;'> <a class='list2' href='#'>{0}</a></td></tr></table> </td>", dr["Des"]));


            return td.ToString();
        }

        public static string onclick(DataRow dr)
        {
            string rs = "";


            string onclick = " onclick=\" f_menuclick('{0}',this,'&nbsp;>&nbsp;{1}nbsp;>>&nbsp;{2}')\"";
            onclick = String.Format(onclick, dr["name"], dr["parentname"], dr["Des"]);
            string link = dr["link"].ToString();//"../public/query.aspx";//
            string selectNodeName = "";
            if (dr["parentname"].ToString().Equals(""))
            {
                selectNodeName = "&nbsp;>&nbsp;" + System.Web.HttpUtility.UrlEncode(dr["Des"].ToString()); //只有一级
            }
            else
            {
                selectNodeName = String.Format("&nbsp;>&nbsp;{1}&nbsp;>>&nbsp;{0}", System.Web.HttpUtility.UrlEncode(dr["Des"].ToString()), System.Web.HttpUtility.UrlEncode(dr["parentname"].ToString()));
            }
            int l = dr["Des"].ToString().Length;
            string surl = String.Format(" surl=\"{0}?type={1}&id={2}&SelectNodeName={3}\"", link, dr["name"].ToString(), System.Web.HttpUtility.UrlEncode(dr["id"].ToString()), selectNodeName);

            rs = onclick + surl;
            return rs;
        }

        public String toChild(DataTable mainMenuTable)
        {
            DataTable sub2Table = new DataTable();///test 子菜单
            StringBuilder rs = new StringBuilder();


            int j = 0;
            StringBuilder sb = new StringBuilder();
            foreach (DataRow drmain in mainMenuTable.Rows)
            {

                sb.Append(String.Format("<div id='Menu1n{0}Items' class='Menu1_0'>", j.ToString()));
                j++;
                sb.Append(String.Format("<table style='width: 148' border='0' width='120px' cellpadding='0' cellspacing='0' bgcolor='#FFFFFF' class='TextBorder1' id='TwoMenu'>", j));
                string pname = drmain["name"].ToString();
                if (childreninfos.ContainsKey(pname))
                {
                    sub2Table = toDataTable(childreninfos[pname]);
                }
                else
                {
                    sub2Table = new DataTable();

                }

                foreach (DataRow dr in sub2Table.Rows) //.Select("id like '" + drmain["id"].ToString() + "%'")
                {


                    string onclick = String.Format(" onclick=\"f_menuclick('{0}',this,'&nbsp;>&nbsp;{1}&nbsp;>>&nbsp;{2}')\"", dr["name"], dr["parentname"], dr["Des"]);
                    string surl = String.Format(" surl=\"{0}?type={1}&id={4}&SelectNodeName=&nbsp;>&nbsp;{3}&nbsp;>>&nbsp;{2} \">", dr["link"].ToString(), dr["name"].ToString(), System.Web.HttpUtility.UrlEncode(dr["Des"].ToString()), System.Web.HttpUtility.UrlEncode(dr["parentname"].ToString()), System.Web.HttpUtility.UrlEncode(dr["id"].ToString()));
                    string tr = String.Format("<tr id='Menu1n{0}' submenunum='{1}' ", dr["id"], dr["icount"]);
                    tr += " onmouseover=\"Menu_HoverDynamic(this)\" onmouseout=\"Menu_Unhover(this)\" onkeyup=\"Menu_Key(this)\" " + onclick + surl;
                    sb.Append(tr);

                    sb.Append(" <td valign='top'><table style='width: 100%; height: 24px' border='0' cellpadding='0' cellspacing='0'> <tr><td style='white-space: nowrap; width: 100%; padding-left: 10px; padding-right: 10px;'>");
                    sb.Append(String.Format("<a class='list01' href='#'>{0}</a></td>", dr["Des"]));

                    if (dr["icount"].ToString() != "0")
                    {
                        sb.Append(" <td> <img src='../images/"+style+"/dot4.gif' width='8' height='8' border='0' /></td>");
                    }
                    //       i++;
                    sb.Append("</tr></table> </td></tr>");
                }

                if (childreninfos.ContainsKey(pname))
                {
                    sb.Append(String.Format("</table>"));
                }
                else
                {
                    sb = new StringBuilder();
                }
                sb.Append(" </div><input type='hidden' id='style' value=" + style + " />");
                rs.Append(sb);
            }
            return rs.ToString();
        }


        public DataTable toDataTable(List<object> menuList)
        {
            menuList.Sort(new MenuInfoComparer());
            DataTable dt = new DataTable();
            //  dt.Columns.Add("KeyID");
            dt.Columns.Add("ID");
            dt.Columns.Add("Des");
            //  dt.Columns.Add("usertype");
            dt.Columns.Add("rolecanusefunction");
            dt.Columns.Add("rolecannotusefunction");
            dt.Columns.Add("parentid");
            dt.Columns.Add("pathlevel");
            dt.Columns.Add("showorder");
            dt.Columns.Add("Name");
            dt.Columns.Add("PageType");
            dt.Columns.Add("link");
            dt.Columns.Add("icount"); //??
            dt.Columns.Add("parentname");
            foreach (UserMenuInfo umi in menuList)
            {
                DataRow dr = dt.NewRow();
                dr["ID"] = umi.id;
                dr["Des"] = umi.desc;
                dr["rolecanusefunction"] = umi.rolecanusefunction;
                dr["parentid"] = umi.parentid;
                dr["pathlevel"] = umi.pathlevel;
                dr["showorder"] = umi.showorder;
                dr["Name"] = umi.name;
                dr["PageType"] = umi.pagetype;
                dr["link"] = umi.link;
                dr["icount"] = 0;
                dr["parentname"] = "";
                if (namedes.ContainsKey(umi.parentname))
                {
                    dr["parentname"] = namedes[umi.parentname];
                }
                dt.Rows.Add(dr);
            }

            return dt;
        }


    }



}