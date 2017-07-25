using System;
using System.Collections.Generic;
using System.Data;
using System.Xml.Serialization;

namespace BaseSrv
{
    //  [Serializable]
    public class Pair
    {
        public Pair() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sKey">key</param>
        /// <param name="sValue">Value</param>
        public Pair(string sKey, string sValue)
        {
            Key = sKey;
            Value = sValue;
        }
        /// <summary>
        /// key 
        /// </summary>
        //    [XmlAttribute("Key")]
        public string Key = "";

        /// <summary>
        /// Value
        /// </summary>
        //    [XmlAttribute("Value")]
        public string Value = "";
    }

    //  [Serializable]
    public class Single
    {
        public Single() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sValue">Value</param>
        public Single(string sValue)
        {
            Value = sValue;
        }
        /// <summary>
        /// Value
        /// </summary>
        //   [XmlAttribute("Value")]
        public string Value = "";
    }

    //  [Serializable]
    public class BusinessConditionCall
    {
        public BusinessConditionCall() { }

        /// <summary>
        /// 表现形式类型，单选还是多选
        /// </summary>
        public string PresentType = "";

        /// <summary>
        /// 所有值集合
        /// </summary>
        //   [XmlElement("AllItemName")]
        public List<Pair> AllItemName = new List<Pair>();

        /// <summary>
        /// 显示值集合，用于控制所有值那些显示出来用途
        /// </summary>
        //    [XmlElement("DisplayEnable")]
        public List<Single> DisplayEnable = new List<Single>();

        /// <summary>
        /// 显示值用户所选择值
        /// </summary>
        //   [XmlAttribute("DisplaySelect")]
        public List<Single> DisplaySelect = new List<Single>();

        /// <summary>
        /// 显示值所对应的标签 
        /// </summary>
        //   [XmlAttribute("DictionaryName")]
        public string DictionaryName = "";

        /// <summary>
        /// 显示值所对应的标签 
        /// </summary>
        //  [XmlAttribute("ComponetType")]
        public string ComponetType = "";

        /// <summary>
        /// 当用户选择自定义时，用户输入的值 
        /// </summary>
        //    [XmlAttribute("CustomValue")]
        public string CustomValue = "";

        /// <summary>
        /// 条件编号
        /// </summary>
        //    [XmlAttribute("id")]
        public string id = "";

        /// <summary>
        /// 条件名称
        /// </summary>
        //    [XmlAttribute("name")]
        public string name = "";
    }

  //  [XmlRootAttribute("BusinessCall", IsNullable = false)]
    public class BusinessCall
    {
        public BusinessCall(){ }
        /// <summary>
        /// 业务名
        /// </summary>
    //    [XmlAttribute("BussinessName")]
        public string BussinessName = "";

        /// <summary>
        /// 业务编号
        /// </summary>
     //   [XmlAttribute("BusinessID")]
        public string BusinessID = "";

        /// <summary>
        /// 角色名
        /// </summary>
      //  [XmlAttribute("RoleName")]
        public string RoleName = "";

        /// <summary>
        /// 条件列表
        /// </summary>
     //   [XmlElement("BusinessConditionCall")]
        public List<BusinessConditionCall> bccList = new List<BusinessConditionCall>();
        //public BusinessConditionCall bccList = new BusinessConditionCall();
    }
}
