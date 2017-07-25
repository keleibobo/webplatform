using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using UTCmpl;

/// <summary>
/// UTDynCompile 的摘要说明
/// </summary>
namespace AppCode
{
    public static class DynCompile
    {
        private static Assembly _assembly = null;
        public static Assembly assembly
        {
            get
            {
                return _assembly;
            }
        }

        static DynCompile()
        {
            String dllname = DynamicComplie.ComplieCs(@"datadefine\UTDataDefine.xml", @"Bin\UTDataDefine.dll");
            if (dllname.Length > 0 && System.IO.File.Exists(dllname))
            {
                _assembly = DynamicComplie.LoadComplieResult(dllname);
            }
        }

        public static Object GetTypeInstance(String TypeName)
        {
            Object rt = null;
            if (_assembly != null)
            {
                rt = UTCmpl.DynamicComplie.GetInstance(_assembly, TypeName);
            }
            return rt;
        }
    }
}