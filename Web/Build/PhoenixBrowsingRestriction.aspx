<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PhoenixBrowsingRestriction.aspx.cs" Inherits="PhoenixBrowsingRestriction" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title><%=Application["softwarename"].ToString() %> - Browsing Restrictions</title>    
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table cellspacing="5" cellpadding="5">
            <tr>
                <td id="Td1" width="100%" valign="top">
                    <h1 id="H1" style="font: 14pt/16pt verdana; color: #0000ff">Phoenix - Browsing Restrictions</h1>
                </td>
            </tr>
            <tr>
                <td id="tableProps" width="100%" valign="top">
                    <font style="font: 9pt/12pt verdana; color: black">Following functions are NOT SUPPORTED in Phoenix. </font>                        
                </td>
            </tr>
            <tr><td>&nbsp;&nbsp;&nbsp;&nbsp;<font style="font: 9pt/12pt verdana; color: black">Open in Multiple Windows</font></td></tr>
            <tr><td>&nbsp;&nbsp;&nbsp;&nbsp;<font style="font: 9pt/12pt verdana; color: black">Open in Multiple tabs</font></td></tr>
            <tr><td>&nbsp;&nbsp;&nbsp;&nbsp;<font style="font: 9pt/12pt verdana; color: black">Press F5 to refresh</font></td></tr>
        </table>
        <hr size="1" style="color:blue" />  
        <a href="javascript:closeWindow();">Close Window</a>                                      
    </div>
    </form>    
</body>
</html>

