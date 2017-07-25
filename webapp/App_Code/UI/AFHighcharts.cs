using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using AppCode;

/// <summary>
/// Summary description for AFAFHighcharts
/// </summary>
namespace AppCode
{
    public class AFHighcharts
    {
        public AFHighcharts()
        {
            //
            // TODO: Add constructor logic here
            //


        }



        private static Dictionary<string, object> getSpline()
        {
            Dictionary<String, Object> charts = new Dictionary<string, object>() {
            {"chart",new Dictionary<string, object>() {{"type","spline"}}},
            {"xAxis",new Dictionary<string, object>() {{"type","datetime"}}}
            //,{"series",new List<object>{
            //    new Dictionary<string,object>(){{"name","d1"},{"data",new Object[]{1,2,3}}}}
            //    }
            };

            return charts;
        }





        /// <summary>
        /// 转化Hightcharts 中 Spline需要的json数据
        /// </summary>
        /// <param name="cval">cid对应的值</param>
        /// <returns></returns>

        public static string getSplineOptions(DataTable dtData, Dictionary<string, Object> options)
        {
            if (dtData == null) return "[]";
            string cid = "点";
            string cval = "值";
            string ctime = "时间";
            if (options != null)
            {
                cid = HttpUtility.UrlDecode(options["name"].ToString());
                cval = HttpUtility.UrlDecode(options["val"].ToString());
                ctime = HttpUtility.UrlDecode(options["time"].ToString());
                dtData.DefaultView.Sort = cid + "," + ctime;
            }
            List<Dictionary<string, object>> series = new List<Dictionary<string, object>>();
            string id = "";
            foreach (DataRow dr in dtData.Rows)
            {
                List<object> aData = new List<object>();
                DateTime time = Convert.ToDateTime(dr[ctime].ToString());
                aData.Add(MilliTimeStamp(time.ToUniversalTime()));
                aData.Add(Convert.ToDouble(dr[cval]));
                if (dr[cid].Equals(id))
                {
                    Dictionary<string, object> chartData = series[series.Count - 1];
                    List<List<object>> cData = (List<List<object>>)chartData["data"];
                    cData.Add(aData);
                }
                else
                {
                    id = dr[cid].ToString();
                    Dictionary<string, object> chartData = new Dictionary<string, object>();
                    chartData.Add("name", id);
                    List<List<object>> cData = new List<List<object>>();
                    cData.Add(aData);
                    ///color
                    String color = getColor(id, series.Count);
                    if (color != "")
                        chartData.Add("color", color);
                    chartData.Add("data", cData);
                    series.Add(chartData);
                }
            }
            string op = "";
            if (options != null && options.ContainsKey("options")) //未测
            {
                op = FormatUtil.toJSON(options["options"]);
                op = "," + op.Substring(1, op.Length - 2); //去 {} 加，
            }
            String rs = @"{ 
			chart: {
                type: 'spline'
            },
			xAxis: {
            type: 'datetime'
            },
			series:" + FormatUtil.toJSON(series) + "}";
            return rs;
        }




        public static string getColor(String key, int index)
        {
            string rs = "";//""默认
            //test red 
            return rs;
        }

        public static string unionspline(Dictionary<string, object> series)
        {
            string rs = "";
            Dictionary<string, object> charts = getSpline();//默认spline
            addTo(series, charts);
            updateSeries("yc1", charts, new Dictionary<string, object> { { "color", "red" } });// color
            rs = FormatUtil.toJSON(charts);
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




        /// <summary>
        /// 饼图
        /// </summary>
        /// <param name="dtData"></param>
        /// <param name="cid"></param>
        /// <param name="cval"></param>
        /// <returns></returns>
        public static string getPie(DataTable dtData, string cid, string cval)
        {
            string rs = "";

            List<object> piedata = new List<object>();
            foreach (DataRow dr in dtData.Rows)
            {
                List<object> data = new List<object>();
                piedata.Add(data);

                data.Add(dr[cid].ToString());
                data.Add(Convert.ToDouble(dr[cval]) / 1.0);


            }
            string options = @"  chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false
        },
        title: {
            text: 'Browser market shares at a specific website, 2010'
        },
        tooltip: {
    	    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    color: '#000000',
                    connectorColor: '#000000',
                    format: '<b>{point.name}</b>: {point.percentage:.1f} %'
                }
            }
        },
        series: [{
            type: 'pie',
            name: 'Browser share',
            data: [
                ['Firefox',   45.0],
                ['IE',       26.8],
                {
                    name: 'Chrome',
                    y: 12.8,
                    sliced: true,
                    selected: true
                },
                ['Safari',    8.5],
                ['Opera',     6.2],
                ['Others',   0.7]
            ]
        }]
    });";
            rs = options;
            return rs;
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



        ///重构
        ///
        public static string getOptions(DataTable dtData, Dictionary<string, Object> options, AFIChart port)
        {
            DataTable dt = dtData.DefaultView.ToTable();
            string cid = "描述";
            string cval = "值";
            string ctime = "时间";
            string ud = HttpUtility.UrlEncode("描述");
            if (dt.Rows.Count > 0)
            {
                cid = dt.Columns[0].ColumnName;
                if (!dt.Columns.Contains(cval))
                {
                    cval = "历史值";
                }
                if (dt.Columns.Contains(cid) && dt.Columns.Contains(ctime))
                    dt.DefaultView.Sort = cid + " asc," + ctime + " asc";
            }

            List<Dictionary<string, object>> series = new List<Dictionary<string, object>>();
            string id = "";
            DateTime now = DateTime.Now;

            foreach (DataRowView dr in dt.DefaultView)
            {
                List<object> aData = new List<object>();
                DateTime time = DateTime.Now;
                try
                {
                    if (dt.Columns.Contains("时间") && dr[ctime] != null && dr[ctime].ToString() != "")
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

                aData.Add(MilliTimeStamp(time.ToUniversalTime()));
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
                    port.addSeries(id, aData, series);
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
            exporting:{
                enabled:false
            },
            tooltip: {
        	    pointFormat: '{series.name}: <b>{point.percentage:.2f}%</b>',
            	percentageDecimals: 4
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
                if (dr["值"] != null && dr["值"].ToString() != "")
                {
                    total += Convert.ToSingle(dr["值"]);
                }
            }
            bool first = true;
            foreach (DataRow dr in dt.Rows)
            {
                float val = 0;
                if (dr["值"] != null && dr["值"].ToString() != "")
                {
                    val = Convert.ToSingle(dr["值"]);
                }
                if (first)
                {
                    rs += "['" + dr["点名"].ToString() + "'," + String.Format("{0:F4}", Convert.ToSingle(val) / total) + "]";
                    first = false;
                }
                else
                {
                    rs += ",['" + dr["点名"].ToString() + "'," + String.Format("{0:F4}", Convert.ToSingle(val) / total) + "]";
                }
            }
            rs += "]";
            rs += "}],";
            rs += "series123: [{" +
                "type: 'pie'," +
                "name: '实时数据'," +
                "data: [";

            first = true;
            foreach (DataRow dr in dt.Rows)
            {
                float val = 0;
                if (dr["值"] != null && dr["值"].ToString() != "")
                {
                    val = Convert.ToSingle(dr["值"]);
                }
                if (first)
                {
                    rs += "['" + dr["点名"].ToString() + "'," + String.Format("{0:F4}", Convert.ToSingle(val)) + "]";
                    first = false;
                }
                else
                {
                    rs += ",['" + dr["点名"].ToString() + "'," + String.Format("{0:F4}", Convert.ToSingle(val)) + "]";
                }
            }
            rs += "]";
            rs += "}]";
            rs += "}";
            return rs;
        }
    }







    //interface IChart
    public abstract class AFIChart
    {
        protected string type = "";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="aData"></param>
        /// <param name="series"></param>
        public abstract void addSeries(String id, List<object> aData, List<Dictionary<string, object>> series);

        public string getChart(List<Dictionary<string, object>> series)
        {
            return String.Format(type, AppCode.FormatUtil.toJSON(series));
        }

    }

    public class AFSplineSeries : AFIChart
    {
        public AFSplineSeries()
        {
            type = @"{{ 
			chart: {{
                type: 'spline',
                height:300
            }},
            exporting:{{
            enabled:false
            }},
			xAxis: {{type: 'datetime',
                labels: {{ 
                formatter: function() {{ 
                               return  Highcharts.dateFormat('%H:%M:%S', this.value); 
                }} 
                }} 
            }}, 
            tooltip: {{
              xDateFormat: '%Y-%m-%d %H:%M:%S'
          }},
            yAxis: {{
                title:{{
                    text: '值'
                }}
            }}, 
			series: {0} 
            }}";
        }
        public override void addSeries(String id, List<object> aData, List<Dictionary<string, object>> series)
        {
            Dictionary<string, object> chartData = new Dictionary<string, object>();
            chartData.Add("name", id);
            List<List<object>> cData = new List<List<object>>();
            cData.Add(aData);
            String color = getColor(id, series.Count);
            if (color != "")
            {
                chartData.Add("color", color);
            }
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

    public class AFColumnSeries : AFIChart
    {
        public AFColumnSeries()
        {
            type = @"{{ 
			chart: {{
                type: 'column',
                height:200
            }},
            tooltip: {
                enabled: false
            },
			series: {0} 
            }}";
        }
        public override void addSeries(String id, List<object> aData, List<Dictionary<string, object>> series)
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

        //public string getChart(List<Dictionary<string, object>> series)
        //{
        //    return String.Format(spline, AppCode.FormatUtil.toJSON(series));
        //}

        private string getColor(String key, int index)
        {
            string rs = "";//""默认
            //test red 
            return rs;
        }
    }
}