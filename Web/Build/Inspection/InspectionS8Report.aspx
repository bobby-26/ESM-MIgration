<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionS8Report.aspx.cs" Inherits="InspectionS8Report" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>S8 Report</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmAuditSummary" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
            <eluc:Error ID="ucError" Visible="false" runat="server" Text="" />
            <eluc:Title Text="S8 - Analysis of Audit Report Data" ID="ucTitle" runat="server" Visible="false"
                ShowMenu="true" />
            <table id="tblWI" width="88%" runat="server">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFrom" runat="server" Text="From"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlFrommonth" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                            EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="--Select--"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="1" Text="Jan"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="2" Text="Feb"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="3" Text="Mar"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="4" Text="Apr"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="5" Text="May"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="6" Text="Jun"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="7" Text="Jul"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="8" Text="Aug"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="9" Text="Sep"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="10" Text="Oct"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="11" Text="Nov"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="12" Text="Dec"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                        <telerik:RadComboBox ID="ddlFromYear" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                            EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblTo" runat="server" Text="To"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlTomonth" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                            EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="--Select--"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="1" Text="Jan"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="2" Text="Feb"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="3" Text="Mar"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="4" Text="Apr"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="5" Text="May"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="6" Text="Jun"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="7" Text="Jul"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="8" Text="Aug"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="9" Text="Sep"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="10" Text="Oct"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="11" Text="Nov"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="12" Text="Dec"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                        <telerik:RadComboBox ID="ddlToYear" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                            EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselType" runat="server" Text="Vessel Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkVesselTypeAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkVesselTypeAll_Changed"
                            Text="---SELECT ALL---" />
                        <telerik:RadCheckBoxList ID="chkVesselType" runat="server" Columns="2" CssClass="input" Direction="Vertical">
                        </telerik:RadCheckBoxList>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDeficiencyCategory" runat="server" Text="Category"></telerik:RadLabel>
                    </td>
                    <td>&nbsp;<telerik:RadCheckBox ID="chkDeficiencyCategoryAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkDeficiencyCategoryAll_Changed"
                        Text="---SELECT ALL---" />
                        <telerik:RadCheckBoxList ID="chkDeficiencyCategory" runat="server" Columns="2" CssClass="input" Direction="Vertical">
                        </telerik:RadCheckBoxList>
                    </td>
                </tr>
            </table>
            <br />
            <eluc:TabStrip ID="MenuAuditSummaryNC" runat="server" OnTabStripCommand="MenuAuditSummaryNC_TabStripCommand"></eluc:TabStrip>
            <table width="100%" >
                <tr >
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblSummary" runat="server" Text="1) Analysis of all non-conformities (NC)"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr >
                    <td>
                        <telerik:RadLabel ID="lblGridISM" runat="server" Text="" Width="95%"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblGridISPS" runat="server" Text="" Width="95%"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblExternalISM" runat="server" Text="2) Category and Vessel type wise break-up of the External ISM audit NCs are:"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblGridExternalISM" runat="server" Text="" width="90%"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblExternalISPS" runat="server" Text="3) Category and Vessel type wise break-up of the External ISPS audit NCs are:"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblGridExternalISPS" runat="server" Text="" width="90%"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblInternalISM" runat="server" Text="4) Category and Vessel type wise break-up of the Internal ISM audit NCs are:"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblGridInternalISM" runat="server" Text="" width="90%" ></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblInternalISPS" runat="server" Text="5) Category and Vessel type wise break-up of the Internal ISPS audit NCs are:"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblGridInternalISPS" runat="server" Text="" width="90%"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
