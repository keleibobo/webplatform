using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using UTDtBaseSvr;

/// <summary>
/// Summary description for Hightcharts
/// </summary>
/// 
namespace AppCode
{
    public class Highcharts
    {
        public Highcharts()
        {

        }

        public static string getColor(String key, int index)
        {
            string rs = "";//""默认
            //test red 
            return rs;
        }

        private static void updateSeries(String name, Dictionary<string, object> series, Dictionary<String, object> insert)//updateSeries("d2", d3, new Dictionary<string, object> { { "color", "red" } });
        {
            List<Dictionary<string, Object>> rs = (List<Dictionary<string, object>>)series["series"];
            foreach (Dictionary<string, object> d in rs)
            {
                if (d["name"].Equals(name))
                {
                    foreach (KeyValuePair<string, object> kvp in insert)
                    {
                        d.Add(kvp.Key, kvp.Value);
                    }
                    break;
                }
            }
        }

        private static long MilliTimeStamp(DateTime TheDate)
        {
            DateTime d1 = new DateTime(1970, 1, 1);
            return MilliTimeStamp(d1, TheDate);
        }

        private static long MilliTimeStamp(DateTime beginDate, DateTime endDate)
        {
            //  DateTime d1 = new DateTime(1970, 1, 1);
            TimeSpan ts = new TimeSpan(endDate.Ticks - beginDate.Ticks);
            //  long t = 8 * 3600 * 1000;
            return (long)ts.TotalMilliseconds;
        }


        /// <summary>
        /// 合并相同key，value为简单Dictionary或List<object> ，且无嵌套
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        private static void addTo(Dictionary<String, Object> from, Dictionary<String, Object> to)
        {
            foreach (KeyValuePair<String, Object> kvp in from)
            {
                if (to.ContainsKey(kvp.Key))
                {
                    Dictionary<String, Object> dx = new Dictionary<string, object>();

                    if (to[kvp.Key].GetType() == kvp.Value.GetType() && kvp.Value is Dictionary<string, object>)
                    {
                        addTo((Dictionary<String, Object>)from[kvp.Key], (Dictionary<String, Object>)to[kvp.Key]);
                    }
                    else if (to[kvp.Key] is List<object> && kvp.Value is List<object>)
                    {
                        to[kvp.Key] = ((List<object>)to[kvp.Key]).Union((List<object>)kvp.Value).ToList();
                    }
                }
                else
                {
                    to.Add(kvp.Key, kvp.Value);
                }
            }
        }


        public static string getNewData(DataTable dtData, Dictionary<string, Object> options, IChart port, ChartProperty CP, List<UTDtCnvrtTable.DataTableInfo> ltColumnInfo)
        {
            DataTable dt = dtData.DefaultView.ToTable();
            List<Dictionary<string, List<object>>> series = new List<Dictionary<string, List<object>>>();

            string rs = "";
            foreach (DataRow item in dt.Rows)
            {
                string itemName = ((System.Data.DataRow)(item)).ItemArray[1].ToString();
                string itemValues = ((System.Data.DataRow)(item)).ItemArray[2].ToString();

                Dictionary<string, object> date = new Dictionary<string, object>();
                date.Add(itemName, itemValues);


            }





            return rs;
        }
        
        ///重构
        ///
        public static string getOptions(DataTable dtData, Dictionary<string, Object> options, IChart port, ChartProperty CP, List<UTDtCnvrtTable.DataTableInfo> ltColumnInfo)
        {
            DataTable dt = dtData.DefaultView.ToTable();
            string cid = CP.seriesNameFiled;
            var qry = ltColumnInfo.Where(a => a.FieldName.Equals(cid, StringComparison.CurrentCultureIgnoreCase));
            if (qry != null)
            {
                cid = qry.First().Desc;
            }
            string cval = CP.yAxisFiled;
            qry = ltColumnInfo.Where(a => a.FieldName.Equals(cval, StringComparison.CurrentCultureIgnoreCase));
            if (qry != null)
            {
                cval = qry.First().Desc;
            }
            string ctime = CP.xAxisFiled;
            qry = ltColumnInfo.Where(a => a.FieldName.Equals(ctime, StringComparison.CurrentCultureIgnoreCase));
            if (qry != null)
            {
                ctime = qry.First().Desc;
            }

            string ud = HttpUtility.UrlEncode(cid);
            if (dt.Rows.Count > 0)
            {
                if (dt.Columns.Contains(cid) && dt.Columns.Contains(ctime))
                    dt.DefaultView.Sort = cid + " asc";
            }

            List<Dictionary<string, object>> series = new List<Dictionary<string, object>>();
            string id = "";
            DateTime now = DateTime.Now;

            bool bDateTime = false;
            if (CP.xAxisType.Equals("datetime", StringComparison.CurrentCultureIgnoreCase))
            {
                bDateTime = true;
            }
            long xvalue = 0;
            foreach (DataRowView dr in dt.DefaultView)
            {
                List<object> aData = new List<object>();
                xvalue = 0;
                if (bDateTime)
                {
                    DateTime time = DateTime.Now;
                    try
                    {
                        if (dt.Columns.Contains(ctime) && dr[ctime] != null && dr[ctime].ToString().Length > 0)
                        {
                            time = Convert.ToDateTime(dr[ctime].ToString());
                        }
                        else
                        {
                            time = now;
                        }
                    }
                    catch (Exception e)
                    {
                        time = now;
                    }
                    xvalue = MilliTimeStamp(time.ToUniversalTime());
                }
                else
                {
                    try
                    {
                        xvalue = Convert.ToInt64(dr[ctime].ToString());
                    }
                    catch (System.Exception ex)
                    {
                    }

                }

                aData.Add(xvalue);
                double val = 0;
                try
                {
                    val = Convert.ToDouble(dr[cval]);
                }
                catch (Exception e) { }
                aData.Add(val);
                if (dr[cid].Equals(id) && !dr[cid].Equals(""))
                {
                    Dictionary<string, object> chartData = series[series.Count - 1];
                    List<List<object>> cData = (List<List<object>>)chartData["data"];
                    cData.Add(aData);
                }
                else
                {
                    id = dr[cid].ToString();
                    port.addSeries(id, aData, series,CP);
                }
            }
            String rs = port.getChart(series);
            return rs;
        }
        public static string getColumnOptions(DataTable dtData)
        {
            DataTable dt = dtData.DefaultView.ToTable();
            string rs = @"{
            chart: {
                type: 'column',
                animation:false,
                height:300
            },
            title: {
                text: '实时数据'
            },
            plotOptions: {
            column: {
                pointPadding: 0.2,
                borderWidth: 0,
                pointWidth: 40
                    }
            },
            exporting:{
                enabled:false
            },
            xAxis: {
            categories: [";
            bool first = true;
            foreach (DataRow dr in dt.Rows)
            {
                if (first)
                {
                    rs += "'" + dr["描述"].ToString() + "'";
                    first = false;
                }
                else
                {
                    rs += ",'" + dr["描述"].ToString() + "'";
                }
            }
            rs += "]},";
            rs += @"yAxis: {
                title:{
                    text: '值'
                }
            },
            series: [{
                name:'实时数据',
            data:[";
            first = true;
            foreach (DataRow dr in dt.Rows)
            {
                if (first)
                {
                    if (dr["值"] != null && dr["值"].ToString() != "")
                    {
                        rs += dr["值"].ToString();
                    }
                    else
                    {
                        rs += "0";
                    }
                    first = false;
                }
                else
                {
                    if (dr["值"] != null && dr["值"].ToString() != "")
                    {
                        rs += "," + dr["值"].ToString();
                    }
                    else
                    {
                        rs += ",0";
                    }
                }
            }
            rs += "],";
            rs += @"dataLabels: {
                    enabled: true,
                    rotation: -90,
                    color: '#FFFFFF',
                    align: 'right',
                    x: 4,
                    y: 10,
                    style: {
                        fontSize: '13px',
                        fontFamily: 'Verdana, sans-serif'
                    }
                }";
            rs += "}]";
            rs += "}";
            return rs;
        }

        public static string getPieOptions(DataTable dtData)
        {
            DataTable dt = dtData.DefaultView.ToTable();
            string rs = @"{
            chart: {
                plotBackgroundColor: null,
                plotBorderWidth: null,
                plotShadow: false,
                height:300
            },
            title: {
                text: '实时数据'
            },
            tooltip: {
        	    pointFormat: '{series.name}: <b>{point.percentage}%</b>',
            	percentageDecimals: 2
            },
            exporting:{
            enabled:false
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: true,
                        color: '#000000',
                        connectorColor: '#000000',
                        formatter: function() {
                            return '<b>'+ this.point.name +'</b>: '+ this.percentage.toFixed(2) +' %';
                        }
                    }
                }
            },
            series: [{
                type: 'pie',
                name: '实时数据',
                data: [";
            float total = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["值"] != null || dr["值"].ToString() != "")
                {
                    total += Convert.ToSingle(dr["值"]);
                }
            }
            bool first = true;
            foreach (DataRow dr in dt.Rows)
            {
                float val = 0;
                if (dr["值"] != null || dr["值"].ToString() != "")
                {
                    val = Convert.ToSingle(dr["值"]);
                }
                if (first)
                {
                    rs += "['" + dr["描述"].ToString() + "'," + String.Format("{0:F4}", Convert.ToSingle(val) / total) + "]";
                    first = false;
                }
                else
                {
                    rs += ",['" + dr["描述"].ToString() + "'," + String.Format("{0:F4}", Convert.ToSingle(val) / total) + "]";
                }
            }
            rs += "]";
            rs += "}]";
            rs += "}";
            return rs;
        }
    }

    //interface IChart
    public abstract class IChart
    {
        protected string LineStr = "";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="aData"></param>
        /// <param name="series"></param>
        public abstract void addSeries(String id, List<object> aData, List<Dictionary<string, object>> series,ChartProperty CP);

        public string getChart(List<Dictionary<string, object>> series)
        {
            try
            {
                string s = AppCode.FormatUtil.toJSON(series);
                //s = "111";
                return LineStr+" "+ s+" }";
            }
            catch (Exception e)
            {
                return "";
            }
        }
    }


    public class SplineSeries : IChart
    {
        public SplineSeries(ChartProperty CP, DataRow dr, List<UTDtCnvrtTable.DataTableInfo> ltColumnInfo,string rendername)
        {
            string Height = CP.chartHight == "" ? "" :CP.chartHight;
            string Width = CP.chartWidth == "" ? "" : ",width:'" + CP.chartWidth + "'";
            string xAxisTitle = CP.xAxisTitle == "" ? "" : CP.xAxisTitle;
            string yAxisTitle = CP.yAxisTitle == "" ? "" : CP.yAxisTitle;
            string xAxisFormatter = CP.xAxisFormatter == "" ? "" : CP.xAxisFormatter;
            string tooltipFormetter = CP.tooltipFormetter == "" ? "%Y-%m-%d %H:%M:%S" : CP.tooltipFormetter;
            string Title = GetTitle(CP, dr, ltColumnInfo);
            LineStr = @"{{ 
            title:{{
                text:'{0}'
            }},
			chart: {{
                type: 'spline',
                reflow: true,
                height: '{1}',
                renderTo:'{7}'
                {2}
            }},
            exporting:{{
            enabled:false
            }},
			xAxis: {{type: 'datetime',
                title:{{
                    text: '{3}'
                }},
                labels: {{ 
                formatter: function() {{ 
                               return  Highcharts.dateFormat('{6}', this.value); 
                }} 
                }} 
            }}, 
            tooltip: {{
              xDateFormat: '{5}',
              style:{{color:'black',fontWeight: 'bold'}}
          }},
            yAxis: {{
                title:{{
                    text: '{4}'
                }}
            }}, 
			series: ";
            LineStr = String.Format(LineStr, Title, Height, Width, xAxisTitle, yAxisTitle, tooltipFormetter, xAxisFormatter,rendername);
        }

        private string GetTitle(ChartProperty CP, DataRow dr, List<UTDtCnvrtTable.DataTableInfo> ltColumnInfo)
        {
            string Title = "";
            if (CP.title != "")
            {
                Title = CP.title;
                foreach (UTDtCnvrtTable.DataTableInfo DTI in ltColumnInfo)
                {
                    if (CP.title.IndexOf(DTI.FieldName) > -1)
                    {
                        if (dr[DTI.Desc] != null)
                        {
                            if (dr[DTI.Desc].ToString().IndexOf(" ") > -1)
                            {
                                Title = Title.Replace(DTI.FieldName, dr[DTI.Desc].ToString().Split(' ')[0]);
                            }
                            else
                            {
                                Title = Title.Replace(DTI.FieldName, dr[DTI.Desc].ToString());
                            }
                        }
                    }
                }
            }
            return Title;
        }

        public override void addSeries(String id, List<object> aData, List<Dictionary<string, object>> series,ChartProperty CP)
        {
            Dictionary<string, object> chartData = new Dictionary<string, object>();
            chartData.Add("name", id);
            List<List<object>> cData = new List<List<object>>();
            cData.Add(aData);
            String color = getSeriesColor(CP, id);
            if (color != "")
            {
                chartData.Add("color", color);
            }
            chartData.Add("data", cData);
            series.Add(chartData);
        }

        private string getSeriesColor(ChartProperty CP, String Desc)
        {
            Dictionary<string, object> chartData = new Dictionary<string, object>();
            string rsColor = "";
            if (CP.seriesColor != "")
            {
                string[] cpstr1 = CP.seriesColor.Split(',');
                foreach (var str in cpstr1)
                {
                    string[] colorIndex = str.Split(':');
                    chartData.Add(colorIndex[0].ToString(), colorIndex[1].ToString());

                }
                foreach (var dataKey in chartData.Keys)
                {
                    if (Desc.Contains(dataKey))
                    {
                        rsColor = chartData[dataKey].ToString();
                        break;
                    }
                }
            }
            return rsColor;
        }

        private string getColor(String key, int index)
        {
            string rs = "";//""默认
            //test red 
            return rs;
        }
    }

    public class ColumnSeries : IChart
    {
        public ColumnSeries()
        {
            LineStr = @"{{ 
			chart: {{
                type: 'column',
                height:200
            }},
            tooltip: {
                enabled: false
            },
            exporting:{{
            enabled:false
            }},
			series: {0} 
            }}";
        }
        public override void addSeries(String id, List<object> aData, List<Dictionary<string, object>> series,ChartProperty CP)
        {

            Dictionary<string, object> chartData = new Dictionary<string, object>();
            chartData.Add("name", id);
            List<List<object>> cData = new List<List<object>>();

            cData.Add(aData);
            ///color
            String color = getColor(id, series.Count);
            if (color != "")
                chartData.Add("color", color);

            chartData.Add("dataLabels", new Dictionary<string, object> { { "enabled", true } });

            chartData.Add("data", cData);
            series.Add(chartData);
        }


        private string getColor(String key, int index)
        {
            string rs = "";//""默认
            //test red 
            return rs;
        }
    }
}
