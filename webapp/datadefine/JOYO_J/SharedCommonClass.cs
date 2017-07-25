﻿/***************************************************************************************************
* 版权所有：珠海优特电力科技股份有限公司
* 版 本 号：1.00
* 文 件 名：SharedCommonClass.cs
* 生成日期：2013.6.6
* 更新日期：
* 作    者：刘拥民
* 功能说明：和WEB平台共享的类文件，WEB平台会编译此文件，类必须能够序列化
***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WFWCFServer
{
    /// <summary>
    /// 厂站短名称及全名称字典
    /// </summary>
    public class StationValuePairs
    {
        /// <summary>
        /// 厂站短名称
        /// </summary>
        public string ShortName;
        /// <summary>
        /// 厂站描述（长名称）
        /// </summary>
        public string FullName;
    }

    /// <summary>
    /// 设备类型ID及名称字典
    /// </summary>
    public class DeviceTypeValuePairs
    {
        /// <summary>
        /// 设备类型编号
        /// </summary>
        public int TypeID;
        /// <summary>
        /// 设备类型名称
        /// </summary>
        public string TypeName;
    }

    /// <summary>
    /// 人员ID及姓名字典
    /// </summary>
    public class StaffValuePairs
    {
        /// <summary>
        /// 人员ID
        /// </summary>
        public int StaffID;
        /// <summary>
        /// 人员姓名
        /// </summary>
        public string StaffName;
    }

    /// <summary>
    /// 未闭锁设备记录类
    /// </summary>
    public class UnlockedEstDevice
    {
        /// <summary>
        /// 厂站名称
        /// </summary>
        public string StationName;
        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName;
        /// <summary>
        /// 电压等级
        /// </summary>
        public string Voltage;
        /// <summary>
        /// 设备类型
        /// </summary>
        public string DevType;
        /// <summary>
        /// 设置DataTable查询结果显示的描述，其结构定义为第1行为特殊数据，表名此类的信息，从第2行开始为字段定义，格式：列索引，是否显示(0:隐藏,1:显示)，列描述，是否关键字段（0：非关键，1:关键）
        /// </summary>
        public List<string> SetDataTableInfo()
        {
            List<string> rt = new List<string>();
            rt.Add("UnlockedEstDevice,1,1,闭锁设备记录,0");
            rt.Add("DeviceName,1,1,设备描述,0");
            rt.Add("StationName,2,1,厂站名称,0");
            rt.Add("Voltage,3,1,电压等级,0");
            rt.Add("DevType,4,1,设备类型,0");
            return rt;
        }
    }

    /// <summary>
    /// E匙通操作记录类
    /// </summary>
    public class EstOperateItem
    {
        /// <summary>
        /// 操作人
        /// </summary>
        public string OperatorName;
        /// <summary>
        /// 设备名称
        /// </summary>
        public string DevName;
        /// <summary>
        /// 设备厂站
        /// </summary>
        public string StaionName;
        /// <summary>
        /// 操作时间
        /// </summary>
        public string OperateTime;
        /// <summary>
        /// 操作结果
        /// </summary>
        public string OperateResult;
        /// <summary>
        /// 设置DataTable查询结果显示的描述，其结构定义为第1行为特殊数据，表名此类的信息，从第2行开始为字段定义，格式：列索引，是否显示(0:隐藏,1:显示)，列描述，是否关键字段（0：非关键，1:关键）
        /// </summary>
        public List<string> SetDataTableInfo()
        {
            List<string> rt = new List<string>();
            rt.Add("EstOperateItem,1,1,E匙通操作记录,0");
            rt.Add("OperatorName,1,1,操作人员,0");
            rt.Add("DevName,2,1,设备名称,0");
            rt.Add("StaionName,3,1,厂站名称,0");
            rt.Add("OperateTime,4,1,操作时间,0");
            rt.Add("OperateResult,5,1,操作结果,0");
            return rt;
        }
    }

    /// <summary>
    /// 操作票索引表记录类
    /// </summary>
    public class HistoryTaskLogItem
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string ID;
        /// <summary>
        /// 任务号
        /// </summary>
        public string TaskID;
        /// <summary>
        /// 站名
        /// </summary>
        public string Station;
        /// <summary>
        /// 开始开票时间
        /// </summary>
        public string StartTime;
        /// <summary>
        /// 历史票编号
        /// </summary>
        public string HISTORYID;
        /// <summary>
        /// 任务名
        /// </summary>
        public string TaskName;
        /// <summary>
        /// 开票人
        /// </summary>
        public string WRITER;
        /// <summary>
        /// 操作人
        /// </summary>
        public string OPERATOR;
        /// <summary>
        /// 值班负责人
        /// </summary>
        public string MANAGER;
        /// <summary>
        /// 监护人
        /// </summary>
        public string USER4;
        /// <summary>
        /// 打印票号
        /// </summary>
        public string PrintNumber;
        /// <summary>
        /// 打印次数
        /// </summary>
        public string PrintCount;
        /// <summary>
        /// 是否已传票
        /// </summary>
        public string ToKey;
        /// <summary>
        /// 操作描述
        /// </summary>
        public string FinishDesc;
        /// <summary>
        /// 是否是当前正在操作的任务（实时任务），1为实时任务，0为历史任务
        /// </summary>
        public string IsCurrentTask;
        /// <summary>
        /// 设置DataTable查询结果显示的描述，其结构定义为第1行为特殊数据，表名此类的信息，从第2行开始为字段定义，格式：列索引，是否显示(0:隐藏,1:显示)，列描述，是否关键字段（0：非关键，1:关键）
        /// </summary>
        public List<string> SetDataTableInfo()
        {
            List<string> rt = new List<string>();
            rt.Add("HistoryTaskLogItem,1,1,操作票索引表记录,0");
            rt.Add("ID,1,0,编号,0");
            rt.Add("TaskID,2,0,任务号,0");
            rt.Add("Station,3,1,站名,0");
            rt.Add("StartTime,4,1,开始开票时间,0");
            rt.Add("HISTORYID,5,0,历史票编号,1");
            rt.Add("TaskName,6,1,任务名,0");
            rt.Add("WRITER,7,1,开票人,0");
            rt.Add("OPERATOR,8,1,操作人,0");
            rt.Add("MANAGER,9,0,值班负责人,0");
            rt.Add("USER4,10,1,监护人,0");
            rt.Add("PrintNumber,11,1,打印票号,0");
            rt.Add("PrintCount,12,0,打印次数,0");
            rt.Add("ToKey,13,1,是否已传票,0");
            rt.Add("FinishDesc,14,1,操作描述,0");
            rt.Add("IsCurrentTask,15,0,是否当前任务,0");
            return rt;
        }
    }

    /// <summary>
    /// 操作票内容
    /// </summary>
    public class OpSheetItem
    {
        /// <summary>
        /// 序号
        /// </summary>
        public string Sequence;
        /// <summary>
        /// 操作项描述
        /// </summary>
        public string SBBH;
        /// <summary>
        /// 是否完成
        /// </summary>
        public string Finished;
        /// <summary>
        /// 设置DataTable查询结果显示的描述，其结构定义为第1行为特殊数据，表名此类的信息，从第2行开始为字段定义，格式：列索引，是否显示(0:隐藏,1:显示)，列描述，是否关键字段（0：非关键，1:关键）
        /// </summary>
        public List<string> SetDataTableInfo()
        {
            List<string> rt = new List<string>();
            rt.Add("OpSheetItem,1,1,E操作票内容,0");
            rt.Add("Sequence,1,1,序号,0");
            rt.Add("SBBH,2,1,操作项描述,0");
            rt.Add("Finished,3,1,是否完成,0");
            return rt;
        }
    }

    /// <summary>
    /// 设备状态类，此类的列表不用来显示，而是更新图形状态用的
    /// </summary>
    public class SvgData
    {
        /// <summary>
        /// 设备厂站
        /// </summary>
        public String StationName = "";
        /// <summary>
        /// 设备编号
        /// </summary>
        public String PointName = "";
        /// <summary>
        /// 设备状态值
        /// </summary>
        public String Value = "";
        /// <summary>
        /// 最后一次更新时间
        /// </summary>
        public String LastreFreshTime = "";
        /// <summary>
        /// 按照其他类增加此函数，但没有内容，设备状态值不显示，只用来更新接线图中设备状态
        /// </summary>
        public List<string> SetDataTableInfo()
        {
            List<string> rt = new List<string>();
            return rt;
        }
    }
}
