using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Data;
using UTDtCnvrt;
using AppCode;
using UTDtBaseSvr;
using System.Web.Script.Serialization;

public partial class UI_WebUserControlChart : System.Web.UI.UserControl
{
    // w & h 
    /// <summary>
    /// 图形宽度 (最小显示宽度, 自动缩放) 
    /// </summary>
    protected int _chartWidth = 480;
    [Bindable(true), Category("Appearance"), DefaultValue("")]
    public int chartWidth
    {
        get { return _chartWidth; }
        set { _chartWidth = value; }
    }


    public String Id
    {
        set
       ;
        get;

    }

    public string Css;

    /// <summary>
    /// 图形高度 
    /// </summary>
    protected int _chartHeight = 360;
    [Bindable(true), Category("Appearance"), DefaultValue("")]
    public int chartHeight
    {
        get { return _chartHeight; }
        set { _chartHeight = value; }
    }

    // margin
    /// <summary>
    /// 图形与边界间隔 (默认为1) 
    /// </summary>
    protected int _chartMargin = 1;
    public int chartMargin
    {
        get { return _chartMargin; }
        set { _chartMargin = value; }
    }

    /// <summary>
    /// 图形类型
    /// </summary>
    public enum eChartType
    {
        /// <summary>
        /// 线性 (直连) 
        /// </summary>
        LineChart,
        /// <summary>
        /// 线性 (平滑过渡) 
        /// </summary>
        SplineChart,
        /// <summary>
        /// 柱状图 
        /// </summary>
        ColumnChart,
        /// <summary>
        /// 饼状图 
        /// </summary>
        PieChart,
        /// <summary>
        /// 横向柱状图
        /// </summary>
        BarChart,
    }

    // param
    public eChartType eCtType = eChartType.SplineChart;

    /// <summary>
    /// 图形的标题 
    /// </summary>
    public string _sTitle = "Chart title";
    public string sTitle
    {
        get { return _sTitle; }
        set { _sTitle = value; }
    }
    /// <summary>
    /// 图形的副标题 
    /// </summary>
    public string _ssTitle = "Subtitle";
    public string ssTitle
    {
        get { return _ssTitle; }
        set { _ssTitle = value; }
    }

    /// <summary>
    /// X轴标题 
    /// </summary>
    public string _sxTitle = "Time";
    public string sxTitle
    {
        get { return _sxTitle; }
        set { _sxTitle = value; }
    }
    /// <summary>
    /// Y轴标题 
    /// </summary>
    public string _syTitle = "Value";
    public string syTitle
    {
        get { return _syTitle; }
        set { _syTitle = value; }
    }

    /// <summary>
    /// 是否显示提示信息 
    /// </summary>
    public bool _bTips = true;
    public bool bTips
    {
        get { return _bTips; }
        set { _bTips = value; }
    }
    /// <summary>
    /// 是否显示图例 
    /// </summary>
    public bool _bLegend = true;
    public bool bLegend
    {
        get { return _bLegend; }
        set { _bLegend = value; }
    }
    /// <summary>
    /// 是否支持导出 
    /// </summary>
    public bool _bExport = false;
    public bool bExport
    {
        get { return _bExport; }
        set { _bExport = value; }
    }

    public Dictionary<string, List<object>> chartData = new Dictionary<string, List<object>>();
    Dictionary<string,Object> extendparam = new Dictionary<string, object>();
    string chartype = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        BusinessCall bcCall = (BusinessCall)HttpContext.Current.Session["bcCall"];
        ChartProperty CP = new ChartProperty();
        List<BusinessComponentCall> bcList = bcCall.bComponentList;
        foreach (BusinessComponentCall bc in bcList)
        {
            if (bc.type.Equals(HtmlComponetType.curvedlinechart.ToString()))
            {
                extendparam = LayoutUI.getParam(bc.extendparam, ';', '=');
                chartype = bc.type;

            }

        }
        if (extendparam.ContainsKey("chartproperty"))
        {
            extendparam.Remove("chartproperty");
        }
        if (extendparam.ContainsKey("interval"))
        {
            extendparam.Add(Id + "interval", extendparam["interval"]);
        }
        extendparam.Add("business", bcCall.BussinessName.ToLower());
        extendparam.Add("chartRefreshData", ReadConfig.TheReadConfig["chartRefreshData"]);

        foreach (BusinessComponentLayoutCall bclc in bcCall.bcLayoutList)
        {
            if (bclc.componentid == Id && bclc.componentlayout != null)
            {
                Dictionary<string, object> obj = (Dictionary<string, object>)FormatUtil.fromJSON(bclc.componentlayout);
                if (obj != null && obj.ContainsKey("css"))
                {
                    Css = FormatUtil.toJSON(obj["css"]);
                    break;
                }

            }
        }
    }

    protected override void Render(HtmlTextWriter output)
    {
        string sHtml = string.Empty;
        string sParam = string.Empty;
        string cid = "component_" + Id + "_" + eCtType.ToString().ToLower();
        sParam = AsChartParam() + AsChartDatas();

        // 1 - js
        string sHtml1 = @"<script type='text/javascript'>";
        // 2 - head
        string sHtml2 = @"$(function () {$(document).ready(function (){";
        // 3 - set
        string sHtml3 = @"Highcharts.setOptions({global:{useUTC:false}});";//@"Highcharts.setOptions(Highcharts.theme);";
        // 4 - chart
        string sHtml4 = String.Format(@" chart= $('#{0}').highcharts({{ {1} }});",cid,sParam);
        // 5 - end 
        string sHtml5 = @" }); });";
        // 6 - js
        string sHtml6 = @"</script>";
        // 7 - div
        string sHtml7 =string.Format( @"       
        <div id='{0}' style='min-width: {1}px; height: {2}px; margin: {3} auto;bottom:100px;'></div>", cid, chartWidth, chartHeight, chartMargin);
        //<!--<script src='js/themes/grid.js'></script>-->
        sHtml = sHtml1 + sHtml2 + sHtml3 + sHtml4 + sHtml5 + sHtml6 + sHtml7;
        //test
        sHtml = sHtml7;
      //  sHtml += String.Format("<script type='text/javascript' >f_loadchart('{0}','{1}');setLayout('{0}','{2}');</script>", cid, FormatUtil.toJSON(extendparam), Css);
        sHtml += String.Format("<script type='text/javascript' >setLayout('{0}','{2}');SetExtendparam('{1}');</script>", cid, FormatUtil.toJSON(extendparam), Css);
        output.Write(sHtml);
    }

    public string AsChartParam()
    {
        string s = string.Empty;
        string sTmp = string.Empty;

        sTmp = @"chart: { type: '" + GetTypeString(eCtType) + @"', marginRight: 8,  events: { load: function () { } } },";
        s += sTmp;

        sTmp = @"title: { text: '" + sTitle + "' },";
        s += sTmp;

        sTmp = @"subtitle: { text: '" + ssTitle + "' },";
        s += sTmp;

        sTmp = @"xAxis: { type: 'datetime',title: { text: '" + sxTitle + "' }, plotLines: [{ value: 0, width: 2, color: '#808080' }]},";
        //, type: 'datetime'
        // , //tickPixelInterval: 150 
        s += sTmp;

        sTmp = @"yAxis: { title: { text: '" + syTitle + "' }, plotLines: [{ value: 0, width: 2, color: '#808080' }] },";
        s += sTmp;

        if (bTips)
        {
            sTmp = @"tooltip: { formatter: " + GetTipsString() + " },";
            s += sTmp;
        }

        sTmp = @"legend: { enabled: " + (bLegend ? "true" : "false") + " },";
        s += sTmp;

        sTmp = @"exporting: { enabled: " + (bExport ? "true" : "false") + " },";
        s += sTmp;

        return s;
    }

    public string AsChartDatas()
    {
        string s = string.Empty;
        string sTmp = string.Empty;

        if (chartData != null)
        {
            List<SeriesData> chartDatalist = new List<SeriesData>();
            foreach(KeyValuePair<String, List<Object>> kv in chartData){
                SeriesData series = new SeriesData();
                series.name = kv.Key;
                series.data = kv.Value;
                chartDatalist.Add(series);
            }
            if (chartDatalist.Count == 0)
            {
                chartDatalist.Add(new SeriesData());
            }
            s+= string.Format("series:{0}", FormatUtil.toJSON(chartDatalist));
        }
       
        return s;
    }

    private string GetTypeString(eChartType e)
    {
        string s = string.Empty;

        switch (e)
        {
            case eChartType.SplineChart:
                s = "spline";
                break;
            case eChartType.ColumnChart:
                s = "column";
                break;
            case eChartType.PieChart:
                s = "pie";
                break;
            case eChartType.BarChart:
                s = "bar";
                break;
            case eChartType.LineChart: // DO NOT BREAK ! 
            default:
                s = "line";
                break;
        }

        return s;
    }

    private string GetTipsString()
    {
        string s = string.Empty;

        s = @"function () { return '<b>' + this.series.name + '</b><br/>' + Highcharts.dateFormat('%Y-%m-%d %H:%M:%S', this.x) + '<br/>' + Highcharts.numberFormat(this.y, 2); }";

        return s;
    }


    class SeriesData
    {
        public string name = "";
        public List<object> data = new List<object>();
    }
    
}