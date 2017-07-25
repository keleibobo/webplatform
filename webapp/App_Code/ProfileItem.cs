using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

/// <summary>
/// Summary description for ProfileItem
/// </summary>
public class ProfileItem
{
    [XmlElement]
    public List<item> item = new List<item>();
}

public class item
{
    [XmlAttribute("name")]
    public string name
    { set; get; }
}


public class webcomm_Event
{
    /// <summary>
    /// 时间日期 yyyy-MM-dd HH:mm 
    /// </summary>
    public string dt = "";
    /// <summary>
    /// 报警类型 
    /// </summary>
    public string alarmtype = "";
    /// <summary>
    /// 事件类型 
    /// </summary>
    public string eventtype = "";
    /// <summary>
    /// 报警级别 
    /// </summary>
    public string alarmlevel = "";
    /// <summary>
    /// 事件描述
    /// </summary>
    public string desc = "";
    /// <summary>
    /// 扩展参数
    /// </summary>
    public string extendparams = "";
}