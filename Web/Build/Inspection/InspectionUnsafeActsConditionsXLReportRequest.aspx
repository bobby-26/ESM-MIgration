<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionUnsafeActsConditionsXLReportRequest.aspx.cs" Inherits="InspectionUnsafeActsConditionsXLReportRequest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleet.ascx" %>
<%@ Register TagPrefix="eluc" TagName="addresstype" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselByCompany" Src="~/UserControls/UserControlVesselByOwner.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmOpenReportsFilter" runat="server">
    <div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">
            <eluc:Title runat="server" ID="Title1" Text="Unsafe Acts / Conditions" ShowMenu="true">
            </eluc:Title>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        </div>
    </div>
    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuOpenReportsFilter" runat="server" OnTabStripCommand="MenuOpenReportsFilter_TabStripCommand">
        </eluc:TabStrip>
    </div>
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlOpenReportsFilter">
        <ContentTemplate>
            <div id="divFind">
               <eluc:Status ID="ucStatus" runat="server" Text="" />            
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblFleet" runat="server" Text="Fleet"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Fleet runat="server" ID="ucTechFleet" Width="80%" CssClass="input" AppendDataBoundItems="true" />
                        </td>
                        <td>
                            <asp:Literal ID="lblOwner" runat="server" Text="Owner"></asp:Literal>
                        </td>
                        <td>
                            <eluc:addresstype runat="server" ID="ucAddrOwner" AddressType="128" Width="80%" AppendDataBoundItems="true"
                                AutoPostBack="true" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                        </td>
                        <td>
                            <eluc:VesselByCompany ID="ucVessel" runat="server" CssClass="input" AppendDataBoundItems="true" />
                        </td>
                        <td>
                            <asp:Literal ID="lblVesselType" runat="server" Text="Vessel Type"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Hard ID="ucVesselType" runat="server" AppendDataBoundItems="true" CssClass="input"
                                HardTypeCode="81" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblCategory" runat="server" Text="Category"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Hard ID="ucCategory" runat="server" AppendDataBoundItems="true" CssClass="input"
                                HardTypeCode="208" AutoPostBack="true" OnTextChangedEvent="ucCategory_TextChanged" />
                        </td>
                        <td>
                            <asp:Literal ID="lblSubCategory" runat="server" Text="Sub-category"></asp:Literal>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSubcategory" runat="server" CssClass="input" Width="250px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblReferenceNo" runat="server" Text="Reference Number"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtReferenceNumber" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblStatus" runat="server" Text="Status"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Hard ID="ucStatusofUA" runat="server" AppendDataBoundItems="true" HardTypeCode="146"
                                ShortNameFilter="OPN,CMP,CLD,CAD" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblIncidentFromDate" runat="server" Text="Incident From Date"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="ucIncidentNearMissFromDate" runat="server" CssClass="input" DatePicker="true" />
                        </td>
                        <td>
                            <asp:Literal ID="lblIncidentToDate" runat="server" Text="Incident To Date"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="ucIncidentNearMissToDate" runat="server" CssClass="input" DatePicker="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%--Reported From Date--%>
                        </td>
                        <td>
                            <eluc:Date ID="ucCreatedFromDate" runat="server" CssClass="input" Visible="false" />
                        </td>
                        <td>
                            <%--Reported To Date--%>
                        </td>
                        <td>
                            <eluc:Date ID="ucCreatedToDate" runat="server" CssClass="input" Visible="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblIncidentRaisedYN" runat="server" Text="Incident / Near Miss Raised YN "></asp:Literal>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIncidentNearMissRaisedYN" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
