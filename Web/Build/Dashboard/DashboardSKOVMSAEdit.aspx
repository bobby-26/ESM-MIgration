<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardSKOVMSAEdit.aspx.cs" Inherits="Dashboard_DashboardSKOVMSAEdit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                  <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
     <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
       <div style="margin-left: 0px">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="Tabstripspiaddmenu" runat="server" OnTabStripCommand="Tabstripspiaddmenu_TabStripCommand"
                TabStrip="true" />
            <br />
            <table style="margin-left: 20px">
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" Text="ID" />
                    </td>
                    <td>
                        &nbsp
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" Width="150px" ID="radtbidentry" CssClass="input_mandatory"/>
                    </td>
                    <td>
                        &nbsp
                    </td>
                    <td>
                        <telerik:RadLabel runat="server" Text="Short Code" />
                    </td>
                    <td>
                        &nbsp
                    </td>
                    <td>
                         <telerik:RadTextBox runat="server" Width="150px" ID="radtbshortcodeentry" CssClass="input_mandatory"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="7">
                        <br />
                    </td>
                </tr>
                <tr>
                   <td>
                       <telerik:RadLabel runat="server" Text="Description" />
                   </td> 
                    <td>
                        &nbsp
                    </td>
                    <td colspan="5">
                        <telerik:RadTextBox runat="server" Width="400px" ID="radtbdescriptionentry" CssClass="input_mandatory" TextMode="MultiLine" Rows="2"/>
                    </td>
                </tr>

             </table>
           
           </div>
    </form>
</body>
</html>
