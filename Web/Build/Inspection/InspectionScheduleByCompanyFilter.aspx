<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionScheduleByCompanyFilter.aspx.cs"
    Inherits="InspectionScheduleByCompanyFilter" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Inspection" Src="~/UserControls/UserControlInspection.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleet.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Owner" Src="~/UserControls/UserControlAddressOwner.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselByOwner" Src="~/UserControls/UserControlVesselByOwner.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiPort" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Inspection Schedule Filter</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
   <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmInspectionScheduleFilter" runat="server">
    <div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">
            <eluc:title runat="server" id="Title1" text="CDI / SIRE Schedule Filter" showmenu="true">
            </eluc:title>
        </div>
    </div>
    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:tabstrip id="MenuScheduleFilter" runat="server" ontabstripcommand="MenuScheduleFilter_TabStripCommand">
        </eluc:tabstrip>
    </div>
    <ajaxtoolkit:toolkitscriptmanager id="ToolkitScriptManager1" runat="server" combinescripts="false">
    </ajaxtoolkit:toolkitscriptmanager>
    <asp:UpdatePanel runat="server" ID="pnlScheduleFilter">
        <ContentTemplate>
            <div id="divFind">
                <table cellpadding="2" cellspacing="2" width="100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblFleet" runat="server" Text="Fleet"></asp:Literal>
                        </td>
                        <td>
                            <eluc:fleet runat="server" id="ucTechFleet" width="270px" cssclass="input" appenddatabounditems="true" />
                        </td>
                        <td>
                            <asp:Literal ID="lblOwner" runat="server" Text="Owner"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Owner runat="server" id="ucAddrOwner" addresstype="128" width="270px" appenddatabounditems="true"
                                autopostback="true" cssclass="input" OnTextChangedEvent="ucAddrOwner_Changed" />
                            <eluc:addresstype runat="server" id="ucCharterer" addresstype="123" width="270px" appenddatabounditems="true"
                                autopostback="true" cssclass="input" Visible="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                        </td>
                        <td>
                            <eluc:VesselByOwner runat="server" ID="ucVessel" AppendDataBoundItems="true" CssClass="input" 
                                VesselsOnly="true" AutoPostBack="true" OnTextChangedEvent="ucVessel_Changed" Width = "270px" />
                        </td>
                        <td>
                           <asp:Literal ID="lblVesselType" runat="server" Text="Vessel Type"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Hard ID="ucVesselType" runat="server" AppendDataBoundItems="true" CssClass="input" HardTypeCode="81" Width = "270px"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <asp:Literal ID="lblVetting" runat="server" Text="Vetting"></asp:Literal>
                        </td>
                        <td>
                            <%--<eluc:inspection runat="server" id="ucVetting" appenddatabounditems="true" cssclass="input"
                                width="155px" AutoPostBack="true" OnTextChangedEvent="ucVetting_Changed" />--%>
                            <asp:DropDownList ID="ucVetting" runat="server" CssClass="input" width="270px" AutoPostBack="true" 
                                OnTextChangedEvent="ucVetting_Changed"></asp:DropDownList>
                            <eluc:hard id="ucAuditType" runat="server" visible="false" shortnamefilter="INS"
                                autopostback="true" cssclass="input" hardtypecode="148" ontextchangedevent="Bind_UserControls" Width = "270px" />
                        </td>
                        <td>
                            <asp:Literal ID="lblBasis" runat="server" Text="Basis"></asp:Literal>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlBasis" runat="server" AutoPostBack="true" CssClass="input" Width = "270px"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblPort" runat="server" Text="Port"></asp:Literal>
                        </td>
                        <td>
                             <eluc:MultiPort id="ucPort" runat="server"  CssClass="input"
                               width="270px"  />
                        </td>
                        <td>
                           <asp:Literal ID="lblstatus" runat="server" Text="Status"></asp:Literal>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPlanned" runat="server" CssClass="input" width="270px">
                                <asp:ListItem Text="All" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Planned" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Not Planned" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:literal ID="lblInspector" runat="server" Text="Inspector"></asp:literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtInspector" runat="server" CssClass="input" Width = "270px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblCompany" runat="server" Text="Company"></asp:Literal>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCompany" runat="server" AutoPostBack="true" CssClass="input"
                               width="270px">
                            </asp:DropDownList>
                        </td>
                    </tr>      
                    <tr>
                        <td>
                            <asp:Literal ID="lblDueFrom" runat="server" Text="Due From"></asp:Literal>
                        </td>
                        <td>
                            <eluc:date id="txtFrom" cssclass="input" runat="server" datepicker="true" />
                        </td>
                        <td>
                            <asp:Literal ID="lblDueTo" runat="server" Text="Due To"></asp:Literal>
                        </td>
                        <td>
                            <eluc:date id="txtTo" cssclass="input" runat="server" datepicker="true" />
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                           <asp:Literal ID="lblPlannedFrom" runat="server" Text="Planned From"></asp:Literal>
                        </td>
                        <td>
                            <eluc:date id="ucDoneFrom" cssclass="input" runat="server" datepicker="true" Visible="false" />
                            <eluc:date id="ucPlannedFrom" cssclass="input" runat="server" datepicker="true" />
                        </td>
                        <td>
                            <asp:Literal ID="lblPlannedTo" runat="server" Text="Planned To"></asp:Literal>
                        </td>
                        <td>
                            <eluc:date id="ucDoneTo" cssclass="input" runat="server" datepicker="true" Visible="false" />
                            <eluc:date id="ucPlannedTo" cssclass="input" runat="server" datepicker="true" />
                        </td>
                    </tr>       
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
