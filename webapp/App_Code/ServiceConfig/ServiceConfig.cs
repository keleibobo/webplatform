using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Sockets;
using System.Management;
using UTDtCnvrt;
using UTUtil;

/// <summary>
/// Summary description for SvrConfig
/// </summary>
/// 
namespace AppCode
{
    public class ServiceInfo
    {
        public string setting = "";
        public string hostname = "";
        public string port = "";
        public string ip = "";
        public string url
        {
            get
            {
                return string.Format(@"{0}:{1}", ip, port);
            }
        }        
    }

    public static class ServiceConfig
    {        
        static Dictionary<String, ServiceInfo> dtService = new Dictionary<String, ServiceInfo>();
        
        static ServiceConfig()
        {
            InitialParam();
        }

        public static void InitialParam()
        {
            ServiceInfo si = null;
            if(dtService.ContainsKey(EnumServiceFlag.basesservice.ToString()))
            {
                si = dtService[EnumServiceFlag.basesservice.ToString()];                
            }
            if(si == null)
            {
                si = new ServiceInfo();
                dtService.Add(EnumServiceFlag.basesservice.ToString(), si);
            }
            LoadConfig(EnumServiceFlag.basesservice, si);

            si = null;
            if (dtService.ContainsKey(EnumServiceFlag.businessservice.ToString()))
            {
                si = dtService[EnumServiceFlag.businessservice.ToString()];
            }
            if (si == null)
            {
                si = new ServiceInfo();
                dtService.Add(EnumServiceFlag.businessservice.ToString(), si);
            }
            LoadConfig(EnumServiceFlag.businessservice, si);

            si = null;
            if (dtService.ContainsKey(EnumServiceFlag.userrightservice.ToString()))
            {
                si = dtService[EnumServiceFlag.userrightservice.ToString()];
            }
            if (si == null)
            {
                si = new ServiceInfo();
                dtService.Add(EnumServiceFlag.userrightservice.ToString(), si);
            }
            LoadConfig(EnumServiceFlag.userrightservice, si);

            si = null;
            if (dtService.ContainsKey(EnumServiceFlag.svgsvrservice.ToString()))
            {
                si = dtService[EnumServiceFlag.svgsvrservice.ToString()];
            }
            if (si == null)
            {
                si = new ServiceInfo();
                dtService.Add(EnumServiceFlag.svgsvrservice.ToString(), si);
            }
            LoadConfig(EnumServiceFlag.svgsvrservice, si);

            si = null;
            if (dtService.ContainsKey(EnumServiceFlag.utuploadservice.ToString()))
            {
                si = dtService[EnumServiceFlag.utuploadservice.ToString()];
            }
            if (si == null)
            {
                si = new ServiceInfo();
                dtService.Add(EnumServiceFlag.utuploadservice.ToString(), si);
            }
            LoadConfig(EnumServiceFlag.utuploadservice, si);            
        }

        //public static ServiceInfo GetInfo(String HostKey)
        //{
        //    ServiceInfo rt = null;
        //    if (dtService.ContainsKey(HostKey))
        //    {
        //        rt = dtService[HostKey];
        //    }
        //    return rt;
        //}

        public static ServiceInfo GetInfo(EnumServiceFlag serviceflag)
        {
            ServiceInfo rt = null;
            if (dtService.ContainsKey(serviceflag.ToString()))
            {
                rt = dtService[serviceflag.ToString()];
            }
            return rt;
        }

        //private static ServiceInfo LoadConfig(EnumServiceFlag serviceflag)
        //{
        //    ServiceInfo si = new ServiceInfo();
        //    if (ReadConfig.TheReadConfig[serviceflag.ToString()] != null)
        //    {
        //        si.setting = ReadConfig.TheReadConfig[serviceflag.ToString()];
        //    }
        //    if (si.url == null || si.url.Length == 0 || si.url.IndexOf(':') == -1)
        //    {
        //        si.setting = Environment.MachineName + ":6667";
        //    }
        //    si.hostname = si.setting.Split(':')[0];
        //    si.port = si.url.Split(':')[1];

        //    if (si.hostname.Equals("localhost", StringComparison.OrdinalIgnoreCase))
        //    {
        //        si.hostname = Environment.MachineName;
        //    }
        //    if (si.hostname.Split('.').Length == 4)
        //    {
        //        si.ip = si.hostname;
        //    }
        //    else
        //    {
        //        si.ip = GetLocalIP(si.hostname);
        //    }
        //    return si;
        //}

        private static void LoadConfig(EnumServiceFlag serviceflag,ServiceInfo si)
        {
            si.setting = "";
            if (ReadConfig.TheReadConfig[serviceflag.ToString()] != null)
            {
                si.setting = ReadConfig.TheReadConfig[serviceflag.ToString()];
            }
            if(si.setting.Length == 0 || si.setting.IndexOf(':') == -1)            
            {
                si.setting = Environment.MachineName + ":6667";
            }
            si.hostname = si.setting.Split(':')[0];
            si.port = si.setting.Split(':')[1];

            if (UTUtil.MyNetCard.IsIP(si.hostname))
            {
                si.ip = si.hostname;
            }
            else
            {
                bool blocal = false;
                //判断服务地址是否是本机
                if (si.hostname.Equals("localhost", StringComparison.OrdinalIgnoreCase) || si.hostname.Equals(Environment.MachineName, StringComparison.OrdinalIgnoreCase))
                {
                    si.hostname = Environment.MachineName;
                    blocal = true;
                }

                if (blocal)
                {
                    string netcard = "";
                    if (serviceflag != EnumServiceFlag.datasourceservice)
                    {
                        netcard = ReadConfig.TheReadConfig["netcardmac"].ToString();
                    }
                    else
                    {
                        netcard = ReadConfig.TheReadConfig["datasourcenetcard"].ToString();
                    }

                    if (netcard != null && netcard.Length > 0)
                    {
                        si.ip = UTUtil.MyNetCard.GetLocalIPByNetCard(netcard);
                    }
                    else
                    {
                        si.ip = UTUtil.MyNetCard.GetAddrByName(si.hostname);
                    }
                }
                else
                {
                    si.ip = UTUtil.MyNetCard.GetAddrByName(si.hostname);
                }                
            }            
        }

        public static string GetLocalIp()
        {
            return MyNetCard.GetIPByMachineName(Environment.MachineName);
        }
        public static string GetMacAddress()
        {
            try
            {
                //获取网卡硬件地址
                string mac = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        mac = mo["MacAddress"].ToString();
                        break;
                    }
                }
                moc = null;
                mc = null;
                return mac;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }
        }
    }
}