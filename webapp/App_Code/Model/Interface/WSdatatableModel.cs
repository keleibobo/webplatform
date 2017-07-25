using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UTDtBaseSvr;
using System.Data;


/// <summary>
/// Summary description for WSdatatableModel
/// </summary>
/// 
namespace AppCode
{
    public class WSdatatableModel
    {
        public static Dictionary<string, DataTable> table = new Dictionary<string, DataTable>();
        public static DataTable getDataTableFromWs(string tablename)
        {
            DataTable dt = null;
            if (table.ContainsKey(tablename))
            {
                dt = table[tablename];
            }
            else
            {
                string action = "gettable";
                object[] args = new object[] { "tablename=" + tablename };
                object rt = WSUtil.getFromWS(action, args);
                if (rt != null)
                {
                    dt = UTUtil.DataTableSerializer.DESerialize(rt.ToString());
                }
                table.Add(tablename, dt);
            }
            return dt;
        }

        public static DataView getDataViewFromWS(string tablename)
        {
            return getDataTableFromWs(tablename).DefaultView;
        }
    }
}