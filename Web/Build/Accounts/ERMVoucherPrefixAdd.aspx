<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ERMVoucherPrefixAdd.aspx.cs"
    Inherits="ERMVoucherPrefixAdd" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
          <%: Scripts.Render("~/bundles/js") %>
          <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
     <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" Height="94%">
           
     
           
           
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <eluc:Status runat="server" ID="ucStatus" Visible="false" />
                 
                        
                       
               
                        <eluc:TabStrip ID="MenuBudget" runat="server" OnTabStripCommand="MenuMain_TabStripCommand"></eluc:TabStrip>
                
                        <br />
                        <table width="100%">
                            <tr>
                                <td>Company</td>
                                <td>
                                    <telerik:RadTextBox ID="txtcompany" runat="server" CssClass="input" Width="180px"></telerik:RadTextBox></td>
                                <td>Database</td>
                                <td>
                                    <telerik:RadTextBox ID="txtDatabase1" runat="server" CssClass="input" Width="180px"></telerik:RadTextBox></td>
                                <td>ERM</td>
                                <td>
                                    <telerik:RadTextBox ID="txtERM" runat="server" CssClass="input" Width="180px"></telerik:RadTextBox></td>
                            </tr>
                            <tr>

                                <td>Phoenix</td>
                                <td>
                                    <telerik:RadTextBox ID="txtPhoenix" runat="server" CssClass="input" Width="180px"></telerik:RadTextBox></td>
                                <td>Phoenix TRN</td>
                                <td>
                                    <telerik:RadTextBox ID="txtPhoenixTRN" runat="server" CssClass="input" Width="180px"></telerik:RadTextBox></td>
                                <td>X Access</td>
                                <td>
                                    <telerik:RadTextBox ID="txtXAccess" runat="server" CssClass="input" Width="180px"></telerik:RadTextBox></td>
                            </tr>
                            <tr>
                                <td>Z Tiime</td>
                                <td>
                                    <telerik:RadTextBox ID="txtXTime" runat="server" CssClass="input" Width="180px"></telerik:RadTextBox></td>
                                <td>ZU Time</td>
                                <td>
                                    <telerik:RadTextBox ID="txtZUTime" runat="server" CssClass="input" Width="180px"></telerik:RadTextBox></td>
                                <td>ZID</td>
                                <td>
                                    <telerik:RadTextBox ID="txtZID" runat="server" CssClass="input" Width="180px"></telerik:RadTextBox></td>
                            </tr>
                            <tr>

                                <td>X Type TRN</td>
                                <td>
                                    <telerik:RadTextBox ID="txtXtypeTRN" runat="server" CssClass="input" Width="180px"></telerik:RadTextBox></td>
                                <td>X TRN</td>
                                <td>
                                    <telerik:RadTextBox ID="txtXTRN" runat="server" CssClass="input" Width="180px"></telerik:RadTextBox></td>
                                <td>X Action</td>
                                <td>
                                    <telerik:RadTextBox ID="txtXAction" runat="server" CssClass="input" Width="180px"></telerik:RadTextBox></td>
                            </tr>
                            <tr>
                                <td>X Description</td>
                                <td>
                                    <telerik:RadTextBox ID="txtXDescription" runat="server" CssClass="input" Width="180px"></telerik:RadTextBox></td>
                                <td>X Number</td>
                                <td>
                                    <telerik:RadTextBox ID="txtXNumber" runat="server" CssClass="input" Width="180px"></telerik:RadTextBox></td>
                                <td>X Inc</td>
                                <td>
                                    <telerik:RadTextBox ID="txtXInc" runat="server" CssClass="input" Width="180px"></telerik:RadTextBox></td>
                            </tr>
                            <tr>

                                <td>Z Active</td>
                                <td>
                                    <telerik:RadTextBox ID="txtZActive" runat="server" CssClass="input" Width="180px"></telerik:RadTextBox></td>
                                <td>Column1</td>
                                <td>
                                    <telerik:RadTextBox ID="txtColumn1" runat="server" CssClass="input" Width="180px"></telerik:RadTextBox></td>
                                <td>Databse2</td>
                                <td>
                                    <telerik:RadTextBox ID="txtDatabase2" runat="server" CssClass="input" Width="180px"></telerik:RadTextBox></td>
                            </tr>
                        </table>
                 </telerik:RadAjaxPanel>
    </form>
</body>
</html>
