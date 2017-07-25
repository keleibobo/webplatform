
using System;
using System.Collections.Generic;
using System.Data;
using System.Web; 
using System.Text;

/// <summary>
///UTCharts 的摘要说明
/// </summary>
public class UTCharts
{
    public UTCharts()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }
    /// <summary>
    /// 返回饼图XML数据
    /// </summary>
    /// <param name="dt">DataTable数据</param>
    /// <param name="sCaption">图表的标题</param>
    /// <param name="bExportType">false:客户端输出，true:服务器断输出（在服务器上temp目录产生一个文件）</param>
    /// <param name="bShowList">单击图表,是否显示明细</param>
    /// <param name="ChartAttribute">图表的其它属性</param>   
    /// <returns>返回饼图XML数据</returns>
    public static string PieChart(System.Data.DataTable dt, string sCaption, bool bExportType, bool bShowList, string ChartAttribute)
    {
        string ExportCode = " exportHandler='exportComponentSwf'   exportAtClient='1'";
        if (bExportType)
        {
            ExportCode = " exportAtClient='0'  exportHandler='../Charts/FCExporter.aspx'  exportAction='Save' ";
        }
        StringBuilder sb = new StringBuilder();
        sb.Append(string.Format("<chart {0} unescapeLinks='0' showAboutMenuItem='1'  exportFormats='JPEG=导出为JPG图片|PNG=导出为PNG图片|PDF=导出为PDF文件'  aboutMenuItemLabel='[导出图表文件]' aboutMenuItemLink=\"javascript:f_UTChartURL('{3}')\"  exportDialogMessage='正在导出,请稍候...' exportEnabled='1'   baseFontSize='12'  palette='2' animation='1' enableSmartLabels='1'   showValues='1' formatNumberScale='1'  showLabels='1' showLegend='1' showPercentValues='1' bgAngle='360' bgRatio='0,100' bgAlpha='40,100' bgColor='99CCFF,FFFFFF'  caption='{1}'  exportfilename='{1}' {2}>", ChartAttribute, sCaption, ExportCode, bExportType));
        string sLink = "";
        HttpContext context = HttpContext.Current;
        foreach (DataRow dr in dt.Rows)
        {

            if (bShowList)
            {

                sLink = string.Format("link=\"javascript:alert(1)\"");
            }
            sb.Append(string.Format("<set label='{0}' value='{1}' {2} isSliced='0' />", dr[0].ToString().Trim(), dr[1].ToString().Trim(), sLink));
        }
        sb.Append("</chart>");
        return sb.ToString();
    }

    /// <summary>
    /// 返回方柱图XML数据(自定义链接:f_UTChartURL())
    /// </summary>
    /// <param name="dt">DataTable数据</param>
    /// <param name="sCaption">图表的标题</param>
    /// <param name="bExportType">false:客户端输出，true:服务器断输出（在服务器上temp目录产生一个文件）</param>
    /// <param name="ChartAttribute">图表的其它属性</param>
    /// <returns>返回圆柱图XML数据</returns>
    public static string ColumnChart(System.Data.DataTable dt, string sCaption, bool bExportType, string ChartAttribute)
    {
        string ExportCode = " exportHandler='exportComponentSwf'   exportAtClient='1' ";
        if (bExportType)
        {
            ExportCode = " exportAtClient='0'  exportHandler='../Charts/FCExporter.aspx' exportFormats='JPEG=导出为JPG图片|PNG=导出为PNG图片|PDF=导出为PDF文件'   exportAction='Save' ";
        }
        StringBuilder sb = new StringBuilder();
        StringBuilder sb3 = new StringBuilder();
        StringBuilder sb4 = new StringBuilder();
        sb.Append(string.Format("<chart {0} palette='3' showAboutMenuItem='1' bgAlpha='40,100' bgColor='99CCFF,FFFFFF'  exportEnabled='1' exportDialogMessage='正在导出,请稍候...'    unescapeLinks='0'  formatNumberScale='0'  baseFontSize='14' caption='{1}'  exportfilename='{1}' {2}>", ChartAttribute, sCaption, ExportCode, bExportType));
        sb.Append("<categories> ");
        for (int iCell = 0; iCell < dt.Columns.Count; iCell++)
        {
            if (iCell > 0)
            {
                sb4.Append(string.Format("<dataset seriesname='{0}'   showValues='1'>", dt.Columns[iCell].ToString()));
            }
            for (int irow = 0; irow < dt.Rows.Count; irow++)
            {
                if (iCell == 0)
                {
                    sb3.Append(string.Format(" <category label='{0}' /> ", dt.Rows[irow][iCell].ToString()));
                }
                else
                {
                    sb4.Append(string.Format("<set value='{0}' link=\"javascript:alert('{0}')\" /> ", dt.Rows[irow][iCell].ToString()));
                }
            }
            if (iCell > 0)
                sb4.Append("</dataset>");
        }
        sb.Append(sb3);
        sb.Append("</categories>");
        sb.Append(sb4);
        sb.Append("</chart>");
        return sb.ToString();
    }


}