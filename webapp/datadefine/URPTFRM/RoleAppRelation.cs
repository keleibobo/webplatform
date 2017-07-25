using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Text;

namespace UserRightObj
{
    public class RoleAppRelation
    {
        public string AppName = "";
        public string RoleId = "";
        public string RoleName = "";
        public string OutAppId = "";
        public string OutAppName = "";


        public List<string> SetDataTableInfo()
        {
            List<string> rt = new List<string>();
            rt.Add("RoleAppRelation,1,1,项目映射列表,1");
            rt.Add("AppName,1,1,项目名称,1");
            rt.Add("RoleId,2,1,角色ID,1");
            rt.Add("RoleName,3,1,角色名称,1");
            rt.Add("OutAppId,4,1,第三方项目ID,1");
            rt.Add("OutAppName,5,1,第三方项目名称,1");
            return rt;
        }

    }
}
