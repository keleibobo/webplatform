<%@ Page Language="C#" StylesheetTheme="" AutoEventWireup="true" CodeFile="UTDate.aspx.cs" Inherits="UTLib_UTcalendar" %>

<html>
<head runat="server" >
<meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
<title>选择日期和时间</title>
<link rel="stylesheet" href="../css/com.css" />
<style type="text/css"> 
BODY {
	FONT: 12px Arial,Verdana;
	background-color: #A9DFE7;
	 
}
SELECT {
	FONT: 12px Arial,Verdana
}
INPUT {
	FONT: 12px Arial,Verdana
}
TABLE {
	FONT-SIZE: 12px
}
.f8 {
	FONT-SIZE: 7px
}
   .myinput
        {
            text-align: center;
            width: 20px;
            border: 0px;
            height: 16px;
            vertical-align: baseline;
        }
.boxIn {
	 BORDER-BOTTOM: #7F9DB9 1px solid; BORDER-LEFT: #7F9DB9 1px solid; BACKGROUND: #ffffff; BORDER-TOP: #7F9DB9 1px solid; BORDER-RIGHT: #7F9DB9 1px solid
}
.myButton {
	WIDTH: 25px; HEIGHT: 10px; FONT-SIZE: 8px
}
 
.boxIn1 {HEIGHT: 20px; BORDER-BOTTOM: #7F9DB9 1px solid; BORDER-LEFT: #7F9DB9 1px solid; BACKGROUND: #ffffff; BORDER-TOP: #7F9DB9 1px solid; BORDER-RIGHT: #7F9DB9 1px solid}
.myButton1 {WIDTH: 25px; HEIGHT: 10px; FONT-SIZE: 5px}
 
</style>

<script type="text/javascript" >
var dateNow	= new Date();
var intYear	= dateNow.getYear();
var intMonth	= dateNow.getMonth();
var intDate	= dateNow.getDate();

var thisName="";
var arrMonth=new Array("一月","二月","三月","四月","五月","六月","七月","八月","九月","十月","十一月","十二月");

function selectTime(fieldname) {

    if (navigator.appName == "Microsoft Internet Explorer")
     thisName = "window.dialogArguments.document.all." + fieldname;
    else
        thisName = "opener.document.getElementById('" + fieldname + "')";
 
    try
    {
	    var strTime=eval(thisName).value;	 
	    strTime=strTime.replace(/[-:]/g," ");
	    var arrTime=strTime.split(" ");
    }
	catch (e)
     {
         
	    return false;
    }
 
var i=0;
if(!isNaN(arrTime[0])){
	i=parseInt(arrTime[0]);
	if(i>1000&&i<9000)intYear=i;
}
if(!isNaN(arrTime[1])){
	i=parseInt(arrTime[1]);
	if(i>0&&i<13)intMonth=i-1;
}
var dateFirst=new Date(intYear,intMonth,1);
var intDay=dateFirst.getDay();
var arrDays=new Array(31,28,31,30,31,30,31,31,30,31,30,31);
var intDays=arrDays[intMonth];
if(intMonth==1)intDays+=!(intYear%4);
if(!isNaN(arrTime[2])){
	i=parseInt(arrTime[2]);
	if(i>0&&i<=intDays)intDate=i;
}



  day=intDate;var strTemp="",intTemp=0;
 setCalendar(intYear,intMonth,intDate)

}

var day = 0;
function onTimeKeyUp(obj,num,maxnum,next) {
    var strTemp = obj.value;
    if (isNaN(strTemp))
obj.value=strTemp.substring(0,strTemp.length-1);
    else {
        var intTemp = parseInt(strTemp); 
        if (intTemp > maxnum) obj.value = strTemp.substring(0, strTemp.length - 1);
         else if (intTemp > num) {
            next.focus();
}}}
 
function setCalendar(intYear,intMonth,intDate)
{
 
var dateFirst=new Date(intYear,intMonth,1);
var intDay=dateFirst.getDay();
var arrDays=new Array(31,28,31,30,31,30,31,31,30,31,30,31);
if(intYear%4==0)arrDays=new Array(31,29,31,30,31,30,31,31,30,31,30,31);
var intDays=arrDays[intMonth],str="";
if(month==1)intDays+=!(intYear%4);
if(intDate>intDays)intDate=intDays;intDay--;
for(var i=0;i<42;i++)
{if(i<=intDay)date[i].innerHTML="";
else if(i<=intDays+intDay){str=i-intDay;
if(str<10)str="&nbsp;"+str;
if(i-intDay==intDate)
{ strTemp = str; str = "<font color='#FFFFFF' style='background:#E87400'>" + str + "</font>"; intTemp = i; day = intDate; } date[i].innerHTML = str;
} else date[i].innerHTML = "";
} 
}
function setNewDate(num)
{ var str = date[num].innerHTML; var i = str.length; if (i > 1 && i < 8) { date[intTemp].innerHTML = strTemp; intTemp = num; strTemp = str; date[num].innerHTML = "<font color='#FFFFFF' style='background:#E87400'>" + strTemp + "</font>"; if (i > 2) str = str.substr(i - 1); day = parseInt(str); } }

function isOk() {
  eval(thisName).value=year.value+"-"+(parseInt(month.value)+1)+"-"+day;
   
  eval(thisName).focus()
  window.close()
}
  
function timeAdd(obj0)
{

 try
  {
    var i=parseInt(obj.value);if(i<intMax)obj.value=i+1;obj.select();obj0.focus();
  }
  catch(e){}
}
function timeRid(obj0)
{
  try
  {
    var i=parseInt(obj.value);if(i>0)obj.value=i-1;obj.select();obj0.focus();
  }
  catch(e){}
}

function MM_swapImgRestore() { //v3.0
    var i, x, a = document.MM_sr; for (i = 0; a && i < a.length && (x = a[i]) && x.oSrc; i++) x.src = x.oSrc;
}

function MM_preloadImages() { //v3.0
    var d = document; if (d.images) {
        if (!d.MM_p) d.MM_p = new Array();
        var i, j = d.MM_p.length, a = MM_preloadImages.arguments; for (i = 0; i < a.length; i++)
            if (a[i].indexOf("#") != 0) { d.MM_p[j] = new Image; d.MM_p[j++].src = a[i]; } 
    }
}

function MM_findObj(n, d) { //v4.01
    var p, i, x; if (!d) d = document; if ((p = n.indexOf("?")) > 0 && parent.frames.length) {
        d = parent.frames[n.substring(p + 1)].document; n = n.substring(0, p);
    }
    if (!(x = d[n]) && d.all) x = d.all[n]; for (i = 0; !x && i < d.forms.length; i++) x = d.forms[i][n];
    for (i = 0; !x && d.layers && i < d.layers.length; i++) x = MM_findObj(n, d.layers[i].document);
    if (!x && d.getElementById) x = d.getElementById(n); return x;
}

function MM_swapImage() { //v3.0
    var i, j = 0, x, a = MM_swapImage.arguments; document.MM_sr = new Array; for (i = 0; i < (a.length - 2); i += 3)
        if ((x = MM_findObj(a[i])) != null) { document.MM_sr[j++] = x; if (!x.oSrc) x.oSrc = x.src; x.src = a[i + 2]; }
}

// onbeforeunload="f_cleartime()" 
</script>
</head>
<body bgcolor="#D4CFC9" scroll="no" style="border:0;margin:5" >
<input runat="server" id="mode" type="hidden" />
<input runat="server" id="cdvalue" type="hidden" />
  <table border="0" cellspacing="0" cellpadding="0" style="padding-top:8" width="195" align="center">  
    <tr>
      <td   height=24 align=center background="../images/line06.gif" style="BORDER-LEFT: #4C74BC 1px solid; BORDER-TOP: #4C74BC 1px solid">日期</td>
      <td style="BORDER-BOTTOM: #4C74BC 1px solid; BORDER-LEFT: #4C74BC 1px solid" 
    >&nbsp;</td>
    </tr>
    <tr>
      <td height="200" colspan=2    align="center" bgcolor="#C4F1F7" style="BORDER-RIGHT: #4C74BC 1px solid;BORDER-LEFT: #4C74BC 1px solid; MARGIN: 10px">
         <fieldset     style="WIDTH: 190px; HEIGHT: 196px; border:1px solid #b1ccef ">
        <legend>&nbsp;&nbsp;&nbsp;日期(D)</legend>
        <table width="100%" height="6" border="0" cellpadding="0" cellspacing="0">
          <tr>
            <td></td>
          </tr>
        </table>
        <select   class="search1"   style="WIDTH: 82px" id="month"    onChange="setCalendar(year.value,month.value,day)" name="month" >
         </select>
        <select style="WIDTH: 82px" id="year"  class="search1"    onChange="setCalendar(year.value,month.value,day)" name="year">
        </select>
                <table width="100%" height="10" border="0" cellpadding="0" cellspacing="0">
                  <tr>
                    <td></td>
                  </tr>
                </table>
               <table width="168" height="133"   border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"  class="TextBorder008">
                 <tr style=" background-color :#A2FFFF">
                    <td width=24 height="19" align="center" >日</td>
                    <td width=24 align="center" >一</td>
                    <td width=24 align="center" >二</td>
                    <td width=24 align="center" >三</td>
                    <td width=24 align="center" >四</td>
                    <td width=24 align="center" >五</td>
                    <td width=24 align="center" >六</td>
                    <%for (int i = 0; i < 42; i++)
                      { %>
                    <%if (i % 7 == 0)
                      {%>
                    </tr>
                    <tr align="center">
                    <%} %>
                    <td id="date" ondblclick="isOk()" height=19 onclick="&#115&#101&#116&#78&#101&#119&#68&#97&#116&#101(<%=i %>)">&nbsp;</td>
                    <%} %>
                    </tr>
                    
                </table>
                </fieldset>
            </td>

           
  </tr>
   <tr>
      <td 
    height=30 colspan=2 valign="top" bgcolor="#C4F1F7"
    style="BORDER-BOTTOM: #4C74BC 1px solid; BORDER-LEFT: #4C74BC 1px solid; MARGIN: 8px; BORDER-RIGHT: #4C74BC 1px solid">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;当前时区：中国标准时间<br></td>
    </tr>
    <tr>
      <td height=2 width=80></td>
      <td width=127 align=right></td>
      <td width=187 align=right></td>
    </tr>
    <tr>
   
      <td colspan=2 align=right onclick="isOk()"><a href="#" onMouseOut="MM_swapImgRestore()" onMouseOver="MM_swapImage('Image2','','../images/ok_on.gif',1)">
      <img src="../images/ok.gif" name="Image2" width="80" height="24" border="0"></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
      <a href="#" onMouseOut="MM_swapImgRestore()" onclick="window.close()" onMouseOver="MM_swapImage('Image1','','../images/cancel_on.gif',1)">
      <img src="../images/cancel.gif" name="Image1" width="80" height="24" border="0"></a>&nbsp;&nbsp;&nbsp;</td>
    </tr>
    <tr>
      <td height="3"></td>
      <td height="3"  align=right></td>
    </tr>
  
</table>


</body>
</html>
<script type="text/javascript" >

selectTime(document.getElementById("cdvalue").value);
for(iyear=2010;iyear<=2025;iyear++)
{
  obj=document.createElement("OPTION")
  obj.text  = iyear+"年"
  obj.value =iyear 
  document.all.year.add(obj)
  if(iyear==intYear)obj.selected=true
  
}

for(imonth=0;imonth<=11;imonth++)
{
  obj=document.createElement("OPTION")
  obj.text  =arrMonth[imonth]
  obj.value =imonth   
  document.all.month.add(obj)
  if(imonth==intMonth)obj.selected=true
  
}

function f_cleartime()
{
  // window.clearTimeout(mytimeout);
}

</script>