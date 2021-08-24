<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionNCReportByCategoryAndVesselTypeWiseAudit.aspx.cs" Inherits="InspectionNCReportByCategoryAndVesselTypeWiseAudit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Category And Vessel Type wise Break-up Report</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="divHead" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

    </div>
</telerik:RadCodeBlock></head>
<body>
   <form id="frmYearWiseNCTrend" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel ID="pnlBrkupNC" runat="server">
        <ContentTemplate>
            <eluc:Error ID="ucError" Visible="false" runat="server" Text="" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title Text="S8 - Analysis of Audit Reports" ID="ucTitle" runat="server"
                            ShowMenu="true" />
                        <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                            <eluc:TabStrip ID="MenuBreakUpNCGeneral" TabStrip="true" runat="server" OnTabStripCommand="MenuBreakUpNCGeneral_TabStripCommand" />
                        </div>
                    </div>
                </div>
                <table id="tblWI" width="60%" runat="server">
                    <tr>
                        <td>
                            <asp:Literal ID="lblFromMonth" runat="server" Text="From">
                            </asp:Literal>
                        </td>
                        <td>
                            <eluc:Hard ID="ucFromMonth" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                                HardTypeCode="55" SortByShortName="true" />
                            <eluc:Quick ID="ddlFromYear" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                                QuickTypeCode="55" />
                        </td>
                        <td>
                            <asp:Literal ID="lblToMonth" runat="server" Text="To">
                            </asp:Literal>
                        </td>
                        <td>
                            <eluc:Hard ID="ucToMonth" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                                HardTypeCode="55" SortByShortName="true" />
                            <eluc:Quick ID="ddlToYear" runat="server" QuickTypeCode="55" AppendDataBoundItems="true"
                                CssClass="input_mandatory" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblAuditName" runat="server" Text="Audit"></asp:Literal>
                        </td>
                        <td>
                            <asp:DropDownList AppendDataBoundItems="true" CssClass="input_mandatory" runat="server"
                                ID="ddlAuditName">
                                <asp:ListItem Text="--Select" Value="">--Select--</asp:ListItem>
                                <asp:ListItem Text="ISM" Value="ISM">ISM </asp:ListItem>
                                <asp:ListItem Text="ISPS" Value="ISPS">ISPS</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Literal ID="lblAuditCategory" runat="server" Text="Inspection Category"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Hard ID="ucInspectionCategory" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"
                                HardTypeCode="144" />
                        </td>
                    </tr>
                   <tr>
                        <td>
                            <asp:Literal ID="lblVesselType" runat="server" Text="Vessel Type"></asp:Literal>
                        </td>
                        <td>
                          <div id="divVesselType" runat="server" class="input_mandatory" style="overflow: auto; height: 100px;
                                width: 300px;">
                                 &nbsp;<asp:CheckBox ID="chkVesselTypeAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkVesselTypeAll_Changed"
                                    Text="---SELECT ALL---" />
                                <asp:CheckBoxList ID="chkVesselType" runat="server" RepeatColumns="2">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                         <td>
                            <asp:Literal ID="lblDeficiencyCategory" runat="server" Text="Category"></asp:Literal>
                        </td>
                        <td>
                          <div id="divDeficiencyCategory" runat="server" class="input" style="overflow: auto; height: 100px;
                                width: 400px;">
                                 &nbsp;<asp:CheckBox ID="chkDeficiencyCategoryAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkDeficiencyCategoryAll_Changed"
                                    Text="---SELECT ALL---" />
                                <asp:CheckBoxList ID="chkDeficiencyCategory" runat="server" RepeatColumns="2">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                    </tr>
                </table>
                <br />
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuBreakUpNC" runat="server" OnTabStripCommand="MenuBreakUpNC_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                  <table width="100%">
                    <td>
                        <asp:Literal runat="server" ID="ltGrid" Text=""></asp:Literal>
                    </td>
                </table>
                 </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
