using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AppCode;
using UTDtBaseSvr;

/// <summary>
/// Summary description for TreeViewControlDefine
/// </summary>
public class TreeViewControlDefine
{
    public TreeViewControlDefine()
    {
        MuiltSelect = false;
    }

    /// <summary>
    /// 导航类型
    /// </summary>
    List<ClassNaviagteCall> bcNavigateList = null;

    /// <summary>
    /// 是否允许多选择
    /// </summary>
    private bool _muiltselect = false;
    public bool MuiltSelect
    {
        set{
            _muiltselect = value;
        }
        get{
            return _muiltselect;
        }
    }

  
    /// <summary>
    /// 导航树组件编号
    /// </summary>
    //private string _divid;
    //public string ID
    //{
    //    set
    //    {
    //        _divid = value;
    //    }
    //    get
    //    {
    //        return _divid;
    //    }
    //}

  

    string rootname = LoginUserModel.Title;
    public string RootName
    {
        set
        {
            rootname = value;
        }
        get
        {
            return rootname;
        }
    }

  

}