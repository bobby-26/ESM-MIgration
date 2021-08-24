<%@ Page Language="C#" AutoEventWireup="True" CodeFile="RegistersPortageBillStandardComponentAdd.aspx.cs"
    Inherits="RegistersPortageBillStandardComponentAdd" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Portage Bill Standard Component</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersAirlines" autocomplete="off" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuContract" runat="server" OnTabStripCommand="Contract_TabStripCommand"></eluc:TabStrip>
            <table cellpadding="1" cellspacing="1" width="80%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblComponent" runat="server" Text="Component Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox DropDownPosition="Static" ID="ddlComponentTypeAdd" runat="server" EnableLoadOnDemand="True"
                            EmptyMessage="Type to select Component Type" Filter="Contains" MarkFirstMatch="true" AutoPostBack="true" Width="240px"
                            DataTextField="FLDNAME" DataValueField="FLDCOMPONENTID" OnSelectedIndexChanged="ddlComponentType_SelectedIndexChanged">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSubType" runat="server" Text="Sub Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ddlHardAdd" runat="server" CssClass="input" AppendDataBoundItems="true" Width="240px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCode" runat="server" Text="Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCodeAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="5"
                            Width="240px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDesc" runat="server" Text="Description"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDescAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="200"
                            Width="240px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBudget" runat="server" Text="Budget Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListBudgetAdd">
                            <telerik:RadTextBox ID="txtBudgetCodeAdd" runat="server" MaxLength="20" CssClass="input_mandatory"
                                Width="220px" Enabled="false">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtBudgetNameAdd" runat="server" Width="0px" CssClass="input hidden"
                                Enabled="False">
                            </telerik:RadTextBox>
                            <asp:LinkButton runat="server" ID="btnShowBudgetAdd" ToolTip="Show Course"  ForeColor="#4c93cc">
                                    <span class="icon"><i class="fas fas fa-list-alt"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtBudgetIdAdd" runat="server" Width="0px" CssClass="input hidden"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtBudgetgroupIdAdd" runat="server" Width="0px" CssClass="input hidden"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
