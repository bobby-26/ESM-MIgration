<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DryDockSendQuery.aspx.cs" Inherits="DryDockSendQuery" ValidateRequest="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>MailManager</title>
    <telerik:RadCodeBlock runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript"  language="javascript">
        //<!--
        function HandleKeyDown(obj, e)
        {
            var KeyCode = (e.which) ? e.which : e.keyCode 
            var target = (e.srcElement) ? e.srcElement : e.target;
            var tabKeyCode = 9;

            if (KeyCode == tabKeyCode && target == obj)
            {
                if(e.which)
                {
                    var l = obj.value.substring(obj.selectionStart, 1) + '      '
                    obj.value = obj.value.substring(obj.selectionStart, 1) + '      ' + obj.value.substring(obj.selectionEnd);
                    o.setSelectionRange(l.length, l.length);
                    obj.focus();
                    e.preventDefault();
                }
                else
                {
                    obj.selection = document.selection.createRange();
                    obj.selection.text = String.fromCharCode(tabKeyCode);
                    event.returnValue = false;
                }
                return false;
            }
            return true;
        }
        //-->
        </script>



   </telerik:RadCodeBlock>
</head>

<body>
    <form id="form1" runat="server">
   <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>   
        <div class="subHeader">
         
            <div style="font-weight: 600; font-size: 12px;" runat="server">
                <eluc:TabStrip ID="MenuMailReply" runat="server" OnTabStripCommand="MenuMailReply_TabStripCommand" >
                </eluc:TabStrip>              
            </div>            
        </div>        
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>    
        <table width="100%" border="1">
            <tr>
                <td width="40%">
                    <table width="100%">
                        <tr>
                            <td width="10%">
                               <telerik:RadLabel ID="lblFrom" runat="server" Text="From"></telerik:RadLabel>
                            </td>
                            <td width="90%">
                                <telerik:RadTextBox ID="txtFrom" runat="server" ReadOnly="true" CssClass="input" Width="90%"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                               <telerik:RadLabel ID="lblTo" runat="server" Text="To"></telerik:RadLabel>
                                
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtTo" runat="server" CssClass="input" Width="90%"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                               <telerik:RadLabel ID="lblCc" runat="server" Text="Cc"></telerik:RadLabel>
                                
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtCc" runat="server" CssClass="input" Width="90%"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                               <telerik:RadLabel ID="lblSubject" runat="server" Text="Subject"></telerik:RadLabel>
                                
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtSubject" runat="server" CssClass="input" Width="90%"></telerik:RadTextBox>
                            </td>
                        </tr>
                    </table> 
                </td>
            </tr>
        </table>
    <table width="100%">
        <tr>
            <td colspan="2">
                    <asp:HyperLink ID="lnkfilename" Target="_blank" Text="Click here to view Zip file contents" runat="server" Width="200 px"
                        Height="14px" ToolTip="Download File">
                    </asp:HyperLink>
            </td>
        </tr>
    </table>                       
        <table width="100%">
            <tr>
                <td colspan="2" width="100%">
                   <telerik:RadTextBox ID="txtMessage" runat="server" TextMode="MultiLine" Rows="18" Width="100%"  CssClass="input" Font-Size="Small" onkeydown="HandleKeyDown(this, event)"></telerik:RadTextBox>
                </td>
            </tr>                
        </table>
    </form>
</body>
</html>
