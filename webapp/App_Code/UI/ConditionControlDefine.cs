using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UTDtBaseSvr;

/// <summary>
/// Summary description for ConditionControlDefine
/// </summary>
public class ConditionControlDefine
{
    //public int id;
    public String menuid
    {
        set;

        get;

    }

    private List<BusinessConditionCall> _bccList;
    public List<BusinessConditionCall> bccList
    {
        set
        {
            _bccList = value;
        }
        get
        {
            if (_bccList == null)
            {
                _bccList = new List<BusinessConditionCall>();
            }
            return _bccList;
        }
    }

    public string stable = "";
    /// <summary>
    /// 业务类型属性
    /// </summary>
    public string sTable
    {
        get { return stable; }
        set { stable = value; }
    }
}