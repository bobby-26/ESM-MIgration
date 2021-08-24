<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Accountsownerfundrequestregisterfilter.aspx.cs"
    Inherits="Accountsownerfundrequestregisterfilter" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStripTelerik" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
     <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
  
         
       
   
        <eluc:tabstrip id="MenuOfficeFilterMain" runat="server" ontabstripcommand="OfficeFilterMain_TabStripCommand">
        </eluc:tabstrip>
  
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
          <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" Height="94%">
   
                <table width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblAccount" runat="server" Text="Account "></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtAccountcode" runat="server" CssClass="input"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
          </telerik:RadAjaxPanel>
    </form>
</body>
</html>
