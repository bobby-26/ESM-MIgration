<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionJHATemplateMapping.aspx.cs"
    Inherits="InspectionJHATemplateMapping" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Category" Src="~/UserControls/UserControlRACategory.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Import JHA</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmTemplateMapping" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" Localization-OK="Yes" Localization-Cancel="No" Width="100%">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="tbljha" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:Status runat="server" ID="ucStatus" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
<%--        <telerik:RadLabel ID="lblImportJHA" runat="server" Text="Import JHA"></telerik:RadLabel>--%>
        <eluc:TabStrip ID="MenuTemplateMapping" runat="server" OnTabStripCommand="MenuTemplateMapping_TabStripCommand"></eluc:TabStrip>
        <table id="tbljha" runat="server" cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td width="10%">
                    <b><telerik:RadLabel ID="lblCategory" runat="server" Text="Category"></telerik:RadLabel></b>
                </td>
                <td width="90%">
                    <telerik:RadComboBox ID="ddlCategory" runat="server" AppendDataBoundItems="true" Width="240px"
                        AutoPostBack="True" OnTextChanged="ddlCategory_Changed" DataTextField="FLDNAME"
                        DataValueField="FLDACTIVITYID" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                        <Items>
                            <telerik:RadComboBoxItem Text="--Select--" Value="DUMMY"></telerik:RadComboBoxItem>
                        </Items>
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:CheckBox ID="chkCheckAll" runat="server" Text="Check All" AutoPostBack="true"
                        OnCheckedChanged="SelectAll" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:CheckBoxList ID="cblJHA" runat="server" RepeatLayout="Table" RepeatDirection="Horizontal" AutoPostBack="true"
                RepeatColumns="6" CssClass="input">
            </asp:CheckBoxList>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
