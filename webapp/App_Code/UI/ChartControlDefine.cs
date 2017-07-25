using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ChartControlDefine
/// </summary>
public class ChartControlDefine
{
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
}