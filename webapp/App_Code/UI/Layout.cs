using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.UI;
using System.Reflection;
using UTDtBaseSvr;

/// <summary>
/// 布局管理
/// </summary>
/// 

public class Layout
{
    int row, col;
    char split = '_';
  
    Dictionary<int, int> gridcell = new Dictionary<int, int>();
    string[,] cells;
    string moldFormat = "{1}<div ID='component_{0}' runat='server' ></div>";//style='position:relative;top:-10px;'

    public Dictionary<string, LayoutCell> layoutcells = new Dictionary<string, LayoutCell>();


    public Layout(int r, int c)
    {
        row = r;
        col = c;
        cells = new string[r, c];
    }

    public void setModule(string id,string title)
    {
        cells[0,0] = String.Format(moldFormat, id,title);
    }

    public void setModule(List<BusinessComponentCall> bccList, List<BusinessComponentLayoutCall> bclcList)
    {
        int id = 0;
      
        
        foreach (KeyValuePair<string, LayoutCell> kvp in layoutcells)
        { 
            LayoutCell layoutcell=kvp.Value;
            int r=layoutcell.row;
            int c= layoutcell.col;
            string title = layoutcell.title;
            if (title!=null&&!title.Equals(""))
            {
                title = "<p>" + title + "</p>";
            }
            cells[r,c] = String.Format(moldFormat,kvp.Key,title);
        }

      

    }

    public void setCSS()
    {
        setCSS(null);
    }

    public void setCSS(Dictionary<string, string> css)
    {


        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                string style = "" + i + split + j;
                string span = "span" + style;
                if (css != null && css.ContainsKey(style))
                {
                    style = " style='" + css[style] + "' ";

                }
                else
                {
                    style = "";
                }

                if (css != null && css.ContainsKey(span))
                {
                    span = css[span];
                }
                else
                {
                    span = "";
                }

                cells[i, j] = String.Format("<td {0} {2}>{1}</td>", style, cells[i, j], span);
            }
        }


    }




    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@" <table  style='width:100%;'   cellpadding='0' cellspacing='0'  >");

        for (int i = 0; i < row; i++)
        {
            sb.Append("<tr>");
            for (int j = 0; j < col; j++)
            {
               
                sb.Append(cells[i, j]);
            }
            sb.Append("</tr>");
        }

        sb.Append("</table>");
       
        return sb.ToString();
    }


    public void setCells(List<BusinessComponentCall> bccList, List<BusinessComponentLayoutCall> bclcList)
    {

        if (bccList.Count == 0) //无组件信息，默认首页svg组件
        {
            //BaseSrv.BusinessComponentCall bcc = new BaseSrv.BusinessComponentCall();
            //bcc.type = "svg";
            //bcc.id = "11";
            //bccList.Add(bcc);
            return;
        }

        layoutcells = new Dictionary<string, LayoutCell>();
        foreach (BusinessComponentCall bcc in bccList)
        {
            LayoutCell layoutcell = new LayoutCell();
            layoutcell.title = bcc.title;
            layoutcell.type = bcc.type;
            layoutcells.Add(bcc.id,layoutcell);
            
        }

        foreach (BusinessComponentLayoutCall bclc in bclcList)
        {
            if (layoutcells.ContainsKey(bclc.componentid))
            {
                LayoutCell layoutcell = layoutcells[bclc.componentid];
                int row = 0;
                int col = 0;
                try
                {
                    row = Convert.ToInt32(bclc.rowcount)-1;
                    col = Convert.ToInt32(bclc.columncount)-1;
                }
                catch
                {
                }
                layoutcell.row = row;
                layoutcell.col = col;
            }
        }
    }

    public String toLayoutCell()
    {

        StringBuilder sb = new StringBuilder();
        sb.Append("[");
        foreach (KeyValuePair<string, LayoutCell> kvp in layoutcells)
        {
            LayoutCell cell = kvp.Value;
            sb.Append("{");
            sb.Append(String.Format("'id':{0},'type':'{1}','row':{2},'col':{3}", kvp.Key, cell.type, cell.row, cell.col));
            sb.Append("},");
        }
        String rs = sb.ToString();
        if (rs.Length > 1)
        {
            rs = rs.Substring(0, rs.Length - 1);
          
        }
        rs = rs + "]";
        return rs;

    }




}

public class LayoutCell
{
    public int row
    {
        set;
        get;
    }
    public int col
    {
        set;
        get;
    }
    public String title
    {
        set;
        get;
    }
    public  String type
    {
        set;
        get;
    }

}

