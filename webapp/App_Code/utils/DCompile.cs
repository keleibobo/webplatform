using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using UTCmpl;
using System.Reflection;
using System.Collections;
using System.IO;


/// <summary>
/// Summary description for DCompile
/// </summary>
public static class DCompile
{    
    public static Assembly amb = null;
    static DCompile()
    {
        Refresh();
    }
    public static void Refresh()
    {
        String dllname = DynamicComplie.ComplieCs(@"datadefine\\UTDataDefine.xml", @"Bin\\UTDataDefine.dll");
        if (dllname.Length > 0 && System.IO.File.Exists(dllname))
        {
            amb = DynamicComplie.LoadComplieResult(dllname);
        }
    }
}