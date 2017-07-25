using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UTDtBaseSvr;
using UTUtil;

/// <summary>
/// Summary description for SetLayout
/// </summary>
public class SetLayout
{
    public static string Navtitle = "";
    public static string Layout(List<BusinessComponentLayoutCall> LayoutList)
	{
        int maxColumn = 0;
        int maxRow = 0;
        string innerhtml = "";
        bool crossColumn = false;
        bool crossRow = false;
        GetParam(ref maxRow, ref maxColumn, ref crossColumn,ref crossRow, LayoutList);
        if (maxColumn == 1)
        {
            if (maxRow == 1)
            {
                innerhtml = "<div data-options=\"region:'center'\" id='layoutcenter' style='width:100%;height:100%;' ><div id='0BaseSvr.BusinessComponentLayoutCall'></div>{0}</div>";
                string divStr = GetChildDiv(LayoutList, maxColumn, maxRow);
                innerhtml = string.Format(innerhtml, divStr);
            }
            else if (maxRow == 2)
            {

                innerhtml += "<div data-options=\"region:'center'\" id='layoutcenter' style='width:100%;height:100%;' ><div id='0BaseSvr.BusinessComponentLayoutCall'></div>{0}</div>";
                string divStr = GetVChildDiv(LayoutList, maxRow, maxColumn);
                innerhtml = string.Format(innerhtml, divStr);
            }
        }
        else if (maxColumn == 2)
        {
            string title = "";
            if (!crossColumn)
            {
                foreach (BusinessComponentLayoutCall businessComponentLayoutCall in LayoutList)
                {
                    if (businessComponentLayoutCall.columncount == "1" && businessComponentLayoutCall.rowcount == "1")
                    {
                        Dictionary<string, string> dtconfig = new Dictionary<string, string>();
                        UtilFunc.GetParaToDictory(businessComponentLayoutCall.extendparam, dtconfig, ";", false);
                        if (dtconfig.ContainsKey("autohide"))
                        {
                            title = dtconfig["autohide"];
                        }
                    }
                }
                if (string.IsNullOrEmpty(title))
                {
                    innerhtml = "<div data-options=\"region:'west',split:true\" title='' id='layoutwest'  style='width:270px;height:100%;overflow:hidden;'>{0}</div>";
                }
                else
                {
                    innerhtml = "<div data-options=\"region:'west',split:true,collapsed:true,\" title='" + title + "' id='layoutwest' conditionhide='true'  style='width:270px;height:100%;overflow:hidden;'>{0}</div>";
                }
                string divStr = GetChildDiv(LayoutList, 1, maxRow);
                innerhtml = string.Format(innerhtml, divStr);

                innerhtml += "<div data-options=\"region:'center'\" id='layoutcenter' style='width:100%;height:100%;overflow:hidden;' ><div id='0BaseSvr.BusinessComponentLayoutCall'></div>{0}</div>"; 
                divStr = GetChildDiv(LayoutList, 2, maxRow);
                innerhtml = string.Format(innerhtml, divStr);
            }
        }
        else if (maxColumn == 3)
        {
            if (!crossRow)
            {
                innerhtml = "<div data-options=\"region:'west',split:true\" title='' id='layoutwest' style='width:270px;height:100%;'>{0}</div>";
                string divStr = GetChildDiv(LayoutList, 1, maxRow);
                innerhtml = string.Format(innerhtml, divStr);

                innerhtml += "<div data-options=\"region:'center'\" id='layoutcenter' style='width:100%;height:100%;' ><div id='0BaseSvr.BusinessComponentLayoutCall'></div>{0}</div>";
                divStr = GetChildDiv(LayoutList, 2, maxRow);
                innerhtml = string.Format(innerhtml, divStr);

                innerhtml += "<div data-options=\"region:'east'\" id='layouteast' style='width:270px;height:100%;' >{0}</div>";
                divStr = GetChildDiv(LayoutList, 3, maxRow);
                innerhtml = string.Format(innerhtml, divStr);
            }
        }

        if (innerhtml.Equals(""))
        {
            innerhtml = "<div data-options=\"region:'center'\" id='layoutcenter' style='width:100%;height:100%;' ></div>";
        }

        return innerhtml;
	}

    public static void GetParam(ref int MaxRow, ref int MaxColumn,ref bool CrossColumn,ref bool CrossRow ,List<BusinessComponentLayoutCall> LayoutList)
    {
        foreach (BusinessComponentLayoutCall bcltc in LayoutList)
        {
            if (!bcltc.rowcount.Contains(','))
            {
                if (Convert.ToInt16(bcltc.rowcount) > MaxRow)
                    MaxRow = Convert.ToInt16(bcltc.rowcount);
            }
            else
            {
                CrossRow = true;
                string[] rowlist = bcltc.rowcount.Split(',');
                if (Convert.ToInt16(rowlist[rowlist.Length-1]) > MaxRow)
                    MaxRow = Convert.ToInt16(rowlist[rowlist.Length - 1]);
            }
            if (!bcltc.columncount.Contains(','))
            {
                if (Convert.ToInt16(bcltc.columncount) > MaxColumn)
                    MaxColumn = Convert.ToInt16(bcltc.columncount);
            }
            else
            {
                CrossColumn = true;
                string[] columncountlist = bcltc.columncount.Split(',');
                if (Convert.ToInt16(columncountlist[columncountlist.Length - 1]) > MaxColumn)
                    MaxColumn = Convert.ToInt16(bcltc.columncount);
            }
        }
    }

    public static string GetChildDiv(List<BusinessComponentLayoutCall> LayoutList, int clounm,int RowCount)
    {
        
        float height = 100 / RowCount;
        List<string> divlist = new List<string>();
        string div = "";
        string lastid = "";

        LayoutList.Sort(new LayoutCompare());
        for (int i = 0; i < RowCount; i++)
        {
            foreach (BusinessComponentLayoutCall bcltc in LayoutList)
            {
                if (bcltc.columncount.Contains(clounm.ToString()) && bcltc.rowcount.Contains((i + 1).ToString()))
                {
                    if (div.IndexOf(string.Format("id='{0}Base", bcltc.componentid)) < 0)
                    {
                        if (bcltc.rowcount.Split(',').Length == RowCount)
                        {
                            height = 100;
                        }

                        divlist.Add(string.Format("<div id='{0}{1}' style='width:100%;'></div>", bcltc.componentid, bcltc.GetType()));
                        div += string.Format("<div id='{0}{1}' style='width:100%;'></div>", bcltc.componentid,bcltc.GetType());
                        lastid = bcltc.componentid;
                    }
                }
            }
        }
       return div;
    }

    public static bool Getcollapsed(List<BusinessComponentLayoutCall> LayoutList, int clounm, int RowCount)
    {
        List<string> divlist = new List<string>();
        bool collapsed = false;

        LayoutList.Sort(new LayoutCompare());
        for (int i = 0; i < RowCount; i++)
        {
            foreach (BusinessComponentLayoutCall bcltc in LayoutList)
            {
                if (bcltc.columncount.Contains(clounm.ToString()) && bcltc.rowcount.Contains((i + 1).ToString()))
                {
                    
                }
            }
        }
        return collapsed;
    }

    private static string  toLayout(List<string> divlist,string lastid){
        string rs = "";
      //  string rs = "";

        if(divlist.Count==1){
            rs=divlist[0];
        }else

            if (divlist.Count == 2)
            {
                rs = "<div class='easyui-layout layout' id='innerlayout' data-options='fit:true' style='width:100%; height:100% '>";
                rs += String.Format("<div data-options=\"region:'north',border:false\"  style='width:100%;height:50px;background:#A2FFFF' >{0}</div>", "<div id='0BaseSvr.BusinessComponentLayoutCall'></div>");

                rs += String.Format("<div data-options=\"region:'center',border:false\"  style='width:100%;overflow:hidden' >{0}</div>", divlist[0]);
                rs += String.Format("<div data-options=\"region:'south',border:false\" style='width:100%;height:60px;' >{0}</div>", divlist[1]);
              //  rs += string.Format("<script type='text/javascript'>lastdiv('{0}')</script>",lastid);
                rs += "</div>";
            }
            else //未用
            {
                rs = "<div class='easyui-layout layout' data-options='fit:true' style='width:100%; height:100% '>";
                rs += String.Format("<div data-options=\"region:'north',border:false\"  style='width:100%;height:50px;background:#A2FFFF' >{0}</div>", "<div id='0BaseSvr.BusinessComponentLayoutCall'></div>");


                string temp = "";
                for (int i = 0; i < divlist.Count - 1; i++)
                {
                    temp += divlist[i];
                }

                rs += String.Format("<div data-options=\"region:'center',border:false\"  style='width:100%;' >{0}</div>", temp);
                rs += String.Format("<div data-options=\"region:'south',border:false\" style='width:100%;height:75px;' >{0}</div>", divlist[divlist.Count - 1]);

                    rs += "</div>";
     
            }

      
        return rs;
    }

    public static string GetVChildDiv(List<BusinessComponentLayoutCall> LayoutList, int Row, int ColunmCount)
    {

        float height = 100;
        List<string> divlist = new List<string>();
      
        string div = "";
        string lastid = "";
        LayoutList.Sort(new LayoutCompare());
        for (int i = 0; i < ColunmCount; i++)
        {
            foreach (BusinessComponentLayoutCall bcltc in LayoutList)
            {
                if (bcltc.columncount.Contains((i + 1).ToString()))
                {
                    if (div.IndexOf(string.Format("id='{0}Base", bcltc.componentid)) < 0)
                    {
                        divlist.Add(string.Format("<div id='{0}{1}' style='width:100%;'></div>", bcltc.componentid, bcltc.GetType()));
                        div += string.Format("<div id='{0}{1}' style='width:100%;'></div>", bcltc.componentid,bcltc.GetType());
                        lastid = bcltc.componentid;
                    }
                }
            }
        }

        
      //  return toLayout(divlist,lastid);
       return div;
    }


   
}

class LayoutCompare : IComparer<BusinessComponentLayoutCall>
{
    public int Compare(BusinessComponentLayoutCall x, BusinessComponentLayoutCall y)
    {
        int rs = 0;
        try
        {
            rs = Convert.ToInt32(x.rowcount) - Convert.ToInt32(y.rowcount);
            if (rs == 0)
            {
                rs = Convert.ToInt32(x.columncount) - Convert.ToInt32(y.columncount);

            }
        }
        catch (Exception e)
        {
        }

        return rs;
    }

}

