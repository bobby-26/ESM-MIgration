<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportMedical.aspx.cs"
    Inherits="Crew_CrewReportMedical" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Junior Officers Report</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixCrew.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlCrewReportEntry">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Medical Report For Joiners"></eluc:Title>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                        TabStrip="false"></eluc:TabStrip>
                </div>
                <div id="divFind" style="position: relative; z-index: 2">
                    <div>
                        <table width="100%">
                            <tr>
                                <td>
                                   <asp:Literal ID="lblMonth" runat="server" Text="Month"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Hard ID="ddlMonthlist" runat="server" HardTypeCode="55" SortByShortName="true"
                                        AppendDataBoundItems="true" CssClass="input_mandatory" />
                                </td>
                                <td>
                                     <asp:Literal ID="lblYear" runat="server" Text="Year"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Quick ID="ddlYearlist" runat="server" QuickTypeCode="55" AppendDataBoundItems="true"
                                        CssClass="input_mandatory" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true" CssClass="input"
                                        VesselsOnly="true" />
                                </td>
                                <td>
                                    <asp:Literal ID="lblRank" runat="server" Text="Rank"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true" CssClass="input" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="navSelect" runat="server" id="divTab1" style="position: relative; width: 15px">
                        <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand">
                        </eluc:TabStrip>
                    </div>
                    <div id="divGrid" style="position: relative; overflow: auto; z-index: 0">
                        <asp:GridView ID="gvCrew" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            OnRowDataBound="gvCrew_RowDataBound" OnRowCommand="gvCrew_RowCommand" Width="100%"
                            CellPadding="3" RowStyle-Wrap="false" ShowHeader="true" EnableViewState="false"
                            AllowSorting="true" OnSorting="gvCrew_Sorting">
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                         <asp:Label ID="lblSrNoHeader" runat="server">Sr.No</asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblSrNo" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROW") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                         <asp:Label ID="lblMedicalSlipNoHeader" runat="server">Medical Slip No</asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblMedicalSlip" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEDICALSLIPNO") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                         <asp:Label ID="lblDateHeader" runat="server">Date</asp:Label>
                                        
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblDate" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPOINTMENTDATE") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                         <asp:Label ID="lblNameHeader" runat="server">Name</asp:Label>
                                        
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmpNo" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>' />
                                        <asp:LinkButton ID="lnkName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>' />                                        
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblRankHeader" runat="server">Rank</asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblRank" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblVesselHeader" runat="server">Vessel</asp:Label>
                                       
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblVessel" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblDoctorHeader" runat="server">Doctor</asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblDoctor" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCTOR") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblPlaceHeader" runat="server">Place</asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblPlace" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCITY") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblLOPANVANLIBHeader" runat="server"> LO/PAN/VAN/ LIB /P&I/LFT/MSO</asp:Label>
                                       
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblIloPan" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLAGMEDICAL") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblHIVHeader" runat="server">HIV</asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblHiv" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHIV") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblDAHeader" runat="server">D&A</asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblDA" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDATEST") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                       <asp:Label ID="lblFitUnfitHeader" runat="server">Fit/UnFit</asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblFitUnfit" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFITUNFIT") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                       <asp:Label ID="lblBillNoHeader" runat="server">Bill No</asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblBillNo" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBILLNO") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                       <asp:Label ID="lblBillDateHeader" runat="server">Bill Date</asp:Label>
                                       
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblBillDate" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBILLDATE") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                       <asp:Label ID="lblBillAmountHeader" runat="server">Bill Amount(in RS)</asp:Label>
                                       
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblBillAmountinRS" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBILLAMT") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                       <asp:Label ID="lblAmountPayableHeader" runat="server">Amount Payable</asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblAmountPayable" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMTPAYABLE") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                       <asp:Label ID="lblRemarksHeader" runat="server">Remarks</asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblRemarks" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div id="divPage" runat="server" style="position: relative;">
                        <table width="100%" border="0" class="datagrid_pagestyle">
                            <tr>
                                <td nowrap="nowrap" align="center">
                                    <asp:Label ID="lblPagenumber" runat="server">
                                    </asp:Label>
                                    <asp:Label ID="lblPages" runat="server">
                                    </asp:Label>
                                    <asp:Label ID="lblRecords" runat="server">
                                    </asp:Label>&nbsp;&nbsp;
                                </td>
                                <td nowrap="nowrap" align="left" width="50px">
                                    <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                                </td>
                                <td width="20px">
                                    &nbsp;
                                </td>
                                <td nowrap="nowrap" align="right" width="50px">
                                    <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                                </td>
                                <td nowrap="nowrap" align="center">
                                    <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                    </asp:TextBox>
                                    <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                        Width="40px"></asp:Button>
                                </td>
                            </tr>
                            <eluc:Status runat="server" ID="ucStatus" />
                        </table>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
