<%@ Page Language="C#" StylesheetTheme="" AutoEventWireup="true" CodeFile="UTTime.aspx.cs" Inherits="UTLib_UTcalendar" %>

<html>
<head runat="server" >
<meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
<title>选择时间</title>
<link rel="stylesheet" href="../css/com.css" />
<style type="text/css"> 
BODY {
	FONT: 12px Arial,Verdana;
	background-color: #B1CCEF;
	 
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

var intDate	= dateNow.getDate();
var intHour	= dateNow.getHours();
var intMinute	= dateNow.getMinutes();
var intSecond	= dateNow.getSeconds();
var thisName = "";
var intYear = dateNow.getYear();
var intMonth = dateNow.getMonth() + 1;

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
 



 var day=intDate;var strTemp="",intTemp=0;
 //setCalendar(intYear,intMonth,intDate)

}
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
 



function isOk() {
    eval(thisName).value = hour.value + ":" + minute.value + ":" + second.value;
    eval(thisName).focus();
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
<body bgcolor="#D4CFC9" scroll="no" style="border:0;margin:5" onload="window.setTimeout('f_time()',1000)" >
<input runat="server" id="mode" type="hidden" />
<input runat="server" id="cdvalue" type="hidden" />
  <table border="0" cellspacing="0" cellpadding="0" style="padding-top:8" width="395" align="center">  
    <tr>
      <td   height=24 align=center background="../images/line06.gif" style="BORDER-LEFT: #4C74BC 1px solid; BORDER-TOP: #4C74BC 1px solid">时间</td>
      <td style="BORDER-BOTTOM: #4C74BC 1px solid; BORDER-LEFT: #4C74BC 1px solid" 
    >&nbsp;</td>
    </tr>
    <tr>
     

            <td align=middle  colspan=2 valign="top" bgcolor="#CDDFFA" style="MARGIN: 6px; BORDER-RIGHT: #4C74BC 1px solid; BORDER-LEFT: #4C74BC 1px solid"">
            <fieldset style="WIDTH: 170px; HEIGHT: 196px;border:1px solid #b1ccef">
                  <legend>&nbsp;&nbsp;&nbsp;时间(T)</legend>
                        <table width="180" height="180" border="0" cellpadding="0" cellspacing="0">
                          <tr>
                            <td align="center">
                                <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0" width="140" height="140">
                                <param name="movie" value="../images/clock.swf" />
                                <param name="quality" value="high" />
                                <param name="wmode" value="transparent" />
                                <param name="menu" value="false" />
                                <embed src="../images/clock.swf" quality="high" pluginspage="http://www.macromedia.com/go/getflashplayer" type="application/x-shockwave-flash" width="140" height="140"></embed>
                                </object>
                               </td>
                          </tr>
                        </table>
                <table border="0" cellSpacing=0 cellPadding=0 width="120" height="16"  class="boxIn1">   
                    <tr>
                    <td align="center">
                       <input name="hour" type="text" class="myinput" id="hour" onblur="if(this.value=='')this.value='0';"
                                            onfocus="this.select();obj=this;intMax=23" value="" maxlength="2" onkeyup="onTimeKeyUp(this,2,23,minute)">:
                                        <input name="minute" type="text" class="myinput" id="minute" onblur="if(this.value=='')this.value='0';"
                                            onfocus="this.select();obj=this;intMax=59" value="20" maxlength="2" onkeyup="onTimeKeyUp(this,5,59,second)">:
                                        <input name="second" type="text" class="myinput" id="second" onblur="if(this.value=='')this.value='0';"
                                            onfocus="this.select();obj=this;intMax=59" value="3" maxlength="2" onkeyup="onTimeKeyUp(this,59,59,null)">
                        </td>
               
                    </tr>
        
                </table>
      </fieldset>
     </td>
  </tr>
   <tr>
      <td 
    height=30 colspan=2 valign="top" bgcolor="#CDDFFA" 
    style="BORDER-BOTTOM: #4C74BC 1px solid; BORDER-LEFT: #4C74BC 1px solid;  BORDER-RIGHT: #4C74BC 1px solid">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;当前时区：中国标准时间<br></td>
    </tr>
    <tr>
      <td height=2 width=80></td>
      <td width=127 align=right></td>
      <td width=187 align=right></td>
    </tr>
    <tr>
      <td>&nbsp;</td>
      <td align=right onclick="isOk()"><a href="#" onMouseOut="MM_swapImgRestore()" onMouseOver="MM_swapImage('Image2','','../images/ok_on.gif',1)">
      <img src="../images/ok.gif" name="Image2" width="80" height="24" border="0"></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
      <a href="#" onMouseOut="MM_swapImgRestore()" onclick="window.close()" onMouseOver="MM_swapImage('Image1','','../images/cancel_on.gif',1)">
      <img src="../images/cancel.gif" name="Image1" width="80" height="24" border="0"></a>&nbsp;&nbsp;&nbsp;</td>
    </tr>
    <tr>
      <td height="3"></td>
      <td height="3" colspan=2 align=right></td>
    </tr>
  
</table>


</body>
</html>
<script type="text/javascript" >
var obj=document.getElementById("hour"),intMax=23;
document.all.hour.value=intHour
document.all.minute.value=intMinute
document.all.second.value=intSecond

selectTime(document.getElementById("cdvalue").value);




function f_cleartime()
{
  // window.clearTimeout(mytimeout);
}
var mytimeout
function f_time()
{
  window.clearTimeout(mytimeout);
  mytimeout=setTimeout("f_time()",1000)
  document.all.second.value=parseInt(document.all.second.value)+1
  if(document.all.second.value==60)
  {
    document.all.minute.value=parseInt(document.all.minute.value)+1
    document.all.second.value=0
  }
  if(document.all.minute.value==60)
  {
    document.all.hour.value=parseInt(document.all.hour.value)+1
    document.all.minute.value=0
  }
 
} 
</script>