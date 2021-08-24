<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardOfficeV2Accounts.aspx.cs" Inherits="Dashboard_DashboardOfficeV2Accounts" %>

<!DOCTYPE html>

<%@ Register TagPrefix="eluc" TagName="Month" Src="~/UserControls/UserControlMonth.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Year" Src="~/UserControls/UserControlYear.ascx" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <telerik:RadCodeBlock ID="radCodeBlock1" runat="server">
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <link href="../Content/bootstrap.min.v3.css" rel="stylesheet" />
        <link href="../Content/fontawesome-all.min.css" rel="stylesheet" />
        <link href="../Content/dashboard.css" rel="stylesheet" />
        <script type="text/javascript">
            function Onclicktab(id) {
                //Get the Button reference and trigger the click event          
                if (id == 2) {
                    setTimeout(function () { $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("SOAOTHERS"); }, 100);
                }
                if (id == 3) {
                    setTimeout(function () { $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("AP"); }, 100);
                }
                if (id == 4) {
                    setTimeout(function () { $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("REMITTANCE"); }, 100);
                }
                if (id == 5) {
                    setTimeout(function () { $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("FUNDSFLOW"); }, 100);
                }
                if (id == 6) {
                    setTimeout(function () { $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("CTMCASH"); }, 100);
                }
            }
        </script>
    </telerik:RadCodeBlock>
    <style type="text/css">
        .panelHeight {
            height: 440px;
        }

        .panelfont {
            overflow: auto;
            font-size: 11px;
        }

        .icon {
            padding-right: 2px;
            font-size: 16px;
        }

        .radMapWrapper {
            padding: 0 21px 21px 21px;
            background-color: #EBEDEE;
            display: inline-block;
            *display: inline;
            zoom: 1;
        }

        .leftCol {
            float: left;
        }

        .rightCol {
            padding-left: 55px;
            font-size: 14px;
            text-align: left;
            line-height: 19px;
        }

        .leftCol .vessel {
            font-weight: bold;
        }

        .leftCol .location {
            border-top: 1px solid #c9c9c9;
            margin-top: 10px;
            padding-top: 10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1"></telerik:RadAjaxLoadingPanel>
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
                <AjaxSettings>
                    <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                        <UpdatedControls>
                            <%--SOA Other--%>
                            <telerik:AjaxUpdatedControl ControlID="gvFundReceived" LoadingPanelID="RadAjaxLoadingPanel1" />
                            <telerik:AjaxUpdatedControl ControlID="gvOutstandingFund" LoadingPanelID="RadAjaxLoadingPanel1" />
                            <telerik:AjaxUpdatedControl ControlID="gvPhoneCard" LoadingPanelID="RadAjaxLoadingPanel1" />
                            <telerik:AjaxUpdatedControl ControlID="gvOutstandingTotal" LoadingPanelID="RadAjaxLoadingPanel1" />
                            <telerik:AjaxUpdatedControl ControlID="gvUnallFundRcpt" LoadingPanelID="RadAjaxLoadingPanel1" />
                            <telerik:AjaxUpdatedControl ControlID="gvAllotNYP" LoadingPanelID="RadAjaxLoadingPanel1" />
                            <%--  AP--%>
                            <telerik:AjaxUpdatedControl ControlID="gvInvoiceParApp" LoadingPanelID="RadAjaxLoadingPanel1" />
                            <telerik:AjaxUpdatedControl ControlID="gvGeneratePMV" LoadingPanelID="RadAjaxLoadingPanel1" />
                            <telerik:AjaxUpdatedControl ControlID="gvAdvanceAccChecking" LoadingPanelID="RadAjaxLoadingPanel1" />
                            <telerik:AjaxUpdatedControl ControlID="gvInvoiceAccChecking" LoadingPanelID="RadAjaxLoadingPanel1" />
                            <telerik:AjaxUpdatedControl ControlID="gvInvoiceAdvPMV" LoadingPanelID="RadAjaxLoadingPanel1" />
                            <telerik:AjaxUpdatedControl ControlID="gvPOAdvFollowUp" LoadingPanelID="RadAjaxLoadingPanel1" />
                            <telerik:AjaxUpdatedControl ControlID="gvPOADPMVNotGen" LoadingPanelID="RadAjaxLoadingPanel1" />
                            <telerik:AjaxUpdatedControl ControlID="gvRebatesFlw" LoadingPanelID="RadAjaxLoadingPanel1" />
                            <telerik:AjaxUpdatedControl ControlID="gvSluggishInvoice" LoadingPanelID="RadAjaxLoadingPanel1" />
                            <%--  Remittance--%>
                            <telerik:AjaxUpdatedControl ControlID="gvRMTNotGen" LoadingPanelID="RadAjaxLoadingPanel1" />
                            <telerik:AjaxUpdatedControl ControlID="gvApprovedPMVRMTnotGen" LoadingPanelID="RadAjaxLoadingPanel1" />
                            <telerik:AjaxUpdatedControl ControlID="gvApprovedAllotM" LoadingPanelID="RadAjaxLoadingPanel1" />
                            <telerik:AjaxUpdatedControl ControlID="gvRemittanceCount" LoadingPanelID="RadAjaxLoadingPanel1" />
                            <%-- Fund Flow--%>
                            <telerik:AjaxUpdatedControl ControlID="gvPendingFR" LoadingPanelID="RadAjaxLoadingPanel1" />
                            <telerik:AjaxUpdatedControl ControlID="gvBankBalance" LoadingPanelID="RadAjaxLoadingPanel1" />
                            <telerik:AjaxUpdatedControl ControlID="gvEstimatedFU" LoadingPanelID="RadAjaxLoadingPanel1" />
                            <telerik:AjaxUpdatedControl ControlID="gvApprovedSupplPMVs" LoadingPanelID="RadAjaxLoadingPanel1" />
                            <%-- CTM Cash--%>
                            <telerik:AjaxUpdatedControl ControlID="gvLocalCP" LoadingPanelID="RadAjaxLoadingPanel1" />
                            <telerik:AjaxUpdatedControl ControlID="gvCashUSD" LoadingPanelID="RadAjaxLoadingPanel1" />
                            <telerik:AjaxUpdatedControl ControlID="gvCashSGD" LoadingPanelID="RadAjaxLoadingPanel1" />
                            <telerik:AjaxUpdatedControl ControlID="gvCashMYR" LoadingPanelID="RadAjaxLoadingPanel1" />
                            <telerik:AjaxUpdatedControl ControlID="gvCTMArrng" LoadingPanelID="RadAjaxLoadingPanel1" />
                            <telerik:AjaxUpdatedControl ControlID="gvTravelAdv" LoadingPanelID="RadAjaxLoadingPanel1" />
                            <telerik:AjaxUpdatedControl ControlID="gvArranCTMNOTRev" LoadingPanelID="RadAjaxLoadingPanel1" />
                            <telerik:AjaxUpdatedControl ControlID="gvOldpendingPOS" LoadingPanelID="RadAjaxLoadingPanel1" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>
            <div class="gray-bg">
                <div class="row page-heading bg-success">
                    <div class="col-lg-3">
                        <h2 class="font-bold">
                            <a runat="server" id="lnkDashboard" href="#" style="color: white" onclick="javascript: top.openNewWindow('filter','Filter','Dashboard/DashboardOfficeV2Filter.aspx?d=3', false, 600, 300,null,null,{icon: '<i class=\'fas fa-filter\'></i>'})">Accounts<i class="fas fa-filter"></i>
                            </a>
                        </h2>
                    </div>
                    <div class="col-lg-9">
                        <br />
                        <a id="lnkVetting" runat="server" class="btn btn-warning" href="javascript: top.openNewWindow('dpopup','Vetting','Dashboard/DashboardOfficeV2TechnicalPMS.aspx?mod=vetting')">Vetting</a>
                        <a id="lnkPhoenixAnalytics" class="btn btn-warning" target="_blank" runat="server">Phoenix Analytics</a>
                        <a id="lnkWRHAnalytics" class="btn btn-warning" target="_blank" runat="server">WRH Analytics</a>
                        <a id="lnkAnalytics" class="btn btn-warning" runat="server" href="javascript: top.openNewWindow('dpopup','Analytics','Dashboard/QualityPBI.html')">Analytics</a>

                        <asp:LinkButton ID="btnHSQEA" runat="server" OnClick="btnHSQEA_Click" Text="HSEQA" CssClass="btn btn-primary"></asp:LinkButton>
                        <asp:LinkButton ID="BtnTech" runat="server" OnClick="BtnTech_Click" Text="Tech" CssClass="btn btn-primary"></asp:LinkButton>
                        <asp:LinkButton ID="BtnCrew" runat="server" OnClick="BtnCrew_Click" Text="Crew" CssClass="btn btn-primary"></asp:LinkButton>
                    </div>
                </div>
                <div class="row">
                    <div class="tabs-container">
                        <ul class="nav nav-tabs">
                            <li class="" id="idsoareport" runat="server"><a data-toggle="tab" href="#tab1">SOA Reporting</a></li>
                            <%--          <li class="" id="idsoaother" runat="server">
                                <a data-toggle="tab" onclick="return Onclicktab(2);" href="#tab2">SOA Others</a>
                            </li>
                            <li class="" id="idap">
                                <a data-toggle="tab" onclick="return Onclicktab(3);" href="#tab3">AP</a>
                            </li>
                            <li class="" id="idremittance">
                                <a data-toggle="tab" onclick="return Onclicktab(4);" href="#tab4">Remittance</a>
                            </li>
                            <li class="" id="idfunsflow">
                                <a data-toggle="tab" onclick="return Onclicktab(5);" href="#tab5">Funds Flow</a>
                            </li>--%>
                            <li class="" id="idctmcash">
                                <a data-toggle="tab" onclick="return Onclicktab(6);" href="#tab6">CTM + Cash</a>
                            </li>
                        </ul>
                        <div class="tab-content">
                            <div class="tab-pane" id="tab1" runat="server">
                                <div class="panel-body">
                                    <%--         <div class="col-lg-4">
                                        <div class="panel panel-success">
                                            <div class="panel-heading">
                                                Open Drydocking POs
                                            </div>
                                            <div class="panel-body">
                                                <telerik:RadGrid ID="gvOpenDryPOs" runat="server" AutoGenerateColumns="false"
                                                    AllowSorting="false" GroupingEnabled="false" Height="250px" OnItemDataBound="gvOpenDryPOs_ItemDataBound"
                                                    EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1000" OnNeedDataSource="gvOpenDryPOs_NeedDataSource">
                                                    <MasterTableView TableLayout="Fixed">
                                                        <NoRecordsTemplate>
                                                            <table runat="server" width="100%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Vessel" HeaderStyle-Width="40%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblVesselO" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="First PO" HeaderStyle-Width="40%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkFirstPO" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblFirstPOurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="185px" />
                                                        <Resizing AllowColumnResize="true" />
                                                    </ClientSettings>
                                                    <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                                </telerik:RadGrid>
                                            </div>
                                        </div>
                                    </div>--%>
                                    <div class="col-lg-8">
                                        <%-- <div class="panel panel-success">
                                            <div class="panel-heading">
                                                Open Technical POs (USD)
                                            </div>
                                            <div class="panel-body">
                                                <div class="row">
                                                    <div class="tabs-container">
                                                        <ul class="nav nav-tabs">
                                                            <li class="" id="Li1" runat="server">
                                                                <a data-toggle="tab" href="#Techtab1">Technical POs</a>
                                                            </li>
                                                            <li class="" id="Li2" runat="server">
                                                                <a data-toggle="tab" onclick="return Onclicktab(2);" href="#Directtab2">Direct POs</a>
                                                            </li>
                                                        </ul>
                                                        <div class="tab-content">
                                                            <div class="tab-pane" id="Techtab1" runat="server">
                                                                <telerik:RadGrid ID="gvOpenTechPOs" runat="server" AutoGenerateColumns="false" Width="100%"
                                                                    AllowSorting="false" GroupingEnabled="false" Height="227px" OnItemDataBound="gvOpenTechPOs_ItemDataBound"
                                                                    EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1000" OnNeedDataSource="gvOpenTechPOs_NeedDataSource">
                                                                    <MasterTableView TableLayout="Fixed">
                                                                        <NoRecordsTemplate>
                                                                            <table runat="server" width="100%" border="0">
                                                                                <tr>
                                                                                    <td align="center">
                                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </NoRecordsTemplate>
                                                                        <Columns>
                                                                            <telerik:GridTemplateColumn HeaderText="Vessel" HeaderStyle-Width="40%">
                                                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                                                <ItemTemplate>
                                                                                    <telerik:RadLabel ID="lblmeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></telerik:RadLabel>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn HeaderText="0-60 days" HeaderStyle-Width="20%">
                                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                                <ItemTemplate>
                                                                                    <telerik:RadLabel ID="lnk06count" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLD60COUNT") %>'></telerik:RadLabel>
                                                                                    <telerik:RadLabel ID="lbl06url" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLD60URL") %>'></telerik:RadLabel>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn HeaderText="60-120 days" HeaderStyle-Width="20%">
                                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                                <ItemTemplate>
                                                                                    <telerik:RadLabel ID="lnk60count" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLD60TO120COUNT") %>'></telerik:RadLabel>
                                                                                    <telerik:RadLabel ID="lbl60url" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLD60TO120URL") %>'></telerik:RadLabel>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn HeaderText="120-180 days " HeaderStyle-Width="20%">
                                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                                <ItemTemplate>
                                                                                    <telerik:RadLabel ID="lnk120count" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLD120TO180COUNT") %>'></telerik:RadLabel>
                                                                                    <telerik:RadLabel ID="lbl120url" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLD120TO180URL") %>'></telerik:RadLabel>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn HeaderText=">180 Days" HeaderStyle-Width="20%">
                                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                                <ItemTemplate>
                                                                                    <telerik:RadLabel ID="lnkGT180count" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGT180COUNT") %>'></telerik:RadLabel>
                                                                                    <telerik:RadLabel ID="lblGT180url" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGT180URL") %>'></telerik:RadLabel>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>
                                                                        </Columns>
                                                                    </MasterTableView>
                                                                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="185px" />
                                                                        <Resizing AllowColumnResize="true" />
                                                                    </ClientSettings>
                                                                    <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                                                </telerik:RadGrid>
                                                            </div>
                                                            <div class="tab-pane" id="Directtab2" runat="server">

                                                                <telerik:RadGrid ID="gvDirectPOs" runat="server" AutoGenerateColumns="false"
                                                                    AllowSorting="false" GroupingEnabled="false" Height="227px" OnItemDataBound="gvDirectPOs_ItemDataBound"
                                                                    EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1000" OnNeedDataSource="gvDirectPOs_NeedDataSource">
                                                                    <MasterTableView TableLayout="Fixed">
                                                                        <NoRecordsTemplate>
                                                                            <table runat="server" width="100%" border="0">
                                                                                <tr>
                                                                                    <td align="center">
                                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </NoRecordsTemplate>
                                                                        <Columns>
                                                                            <telerik:GridTemplateColumn HeaderText="Vessel" HeaderStyle-Width="40%">
                                                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                                                <ItemTemplate>
                                                                                    <telerik:RadLabel ID="lblmeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></telerik:RadLabel>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn HeaderText="0-60 days" HeaderStyle-Width="20%">
                                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                                <ItemTemplate>
                                                                                    <telerik:RadLabel ID="lnk06count" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLD60COUNT") %>'></telerik:RadLabel>
                                                                                    <telerik:RadLabel ID="lbl06url" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLD60URL") %>'></telerik:RadLabel>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn HeaderText="60-120 days" HeaderStyle-Width="20%">
                                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                                <ItemTemplate>
                                                                                    <telerik:RadLabel ID="lnk60count" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLD60TO120COUNT") %>'></telerik:RadLabel>
                                                                                    <telerik:RadLabel ID="lbl60url" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLD60TO120URL") %>'></telerik:RadLabel>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn HeaderText="120-180 days " HeaderStyle-Width="20%">
                                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                                <ItemTemplate>
                                                                                    <telerik:RadLabel ID="lnk120count" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLD120TO180COUNT") %>'></telerik:RadLabel>
                                                                                    <telerik:RadLabel ID="lbl120url" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLD120TO180URL") %>'></telerik:RadLabel>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn HeaderText=">180 Days" HeaderStyle-Width="20%">
                                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                                <ItemTemplate>
                                                                                    <telerik:RadLabel ID="lnkGT180count" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGT180COUNT") %>'></telerik:RadLabel>
                                                                                    <telerik:RadLabel ID="lblGT180url" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGT180URL") %>'></telerik:RadLabel>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>
                                                                        </Columns>
                                                                    </MasterTableView>
                                                                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="185px" />
                                                                        <Resizing AllowColumnResize="true" />
                                                                    </ClientSettings>
                                                                    <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                                                </telerik:RadGrid>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>--%>
                                    </div>
                                    <div class="col-lg-12">
                                        <div class="panel panel-success">                                   

                                            <div class="panel-heading">
                                                Portage Bill Not Yet Finalized
                                            </div>

                                            <div class="panel-body">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <telerik:RadLabel ID="lblReportfortheMonthof" runat="server" Text="Month"></telerik:RadLabel>
                                                        </td>
                                                        <td>
                                                            <eluc:Month ID="ddlMonth" runat="server" Width="80%" OnTextChangedEvent="ddlMonth_TextChangedEvent"
                                                                AutoPostBack="true"></eluc:Month>
                                                        </td>
                                                        <td>
                                                            <telerik:RadLabel ID="lblYear" runat="server" Text="Year"></telerik:RadLabel>
                                                        </td>
                                                        <td>
                                                            <eluc:Year ID="ddlYear" runat="server" Width="60%" OrderByAsc="false" OnTextChangedEvent="ddlYear_TextChangedEvent"
                                                                AutoPostBack="true"></eluc:Year>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <telerik:RadGrid ID="gvPortage" runat="server" AutoGenerateColumns="false"
                                                    AllowSorting="false" GroupingEnabled="false" OnItemCommand="gvPortage_ItemCommand" OnItemDataBound="gvPortage_ItemDataBound"
                                                    EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1000" OnNeedDataSource="gvPortage_NeedDataSource">
                                                    <MasterTableView TableLayout="Fixed">
                                                        <NoRecordsTemplate>
                                                            <table runat="server" width="100%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Vessel" HeaderStyle-Width="30%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblvesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblvesselid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Portage Bill" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnk2ndcount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPBYN") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>

                                                            <telerik:GridTemplateColumn HeaderText="Portage Bill </br> Closing date" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblPBClosingdate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPBCLOSINGDATE", "{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>

                                                            <telerik:GridTemplateColumn HeaderText="Captain  Cash" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkCCYN1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCCYN") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>

                                                            <telerik:GridTemplateColumn HeaderText="Captain  Cash</br> Closing date" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblCCNClosingdate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCCNCLOSINGDATE", "{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>


                                                            <telerik:GridTemplateColumn HeaderText="Provision" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkpubcount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROVISIONYN") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>

                                                            <telerik:GridTemplateColumn HeaderText="Provision</br> Closing date" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblPVNClosingdate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPVNCLOSINGDATE", "{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>

                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>

                                                            <telerik:GridTemplateColumn HeaderText="Action">
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="102px"></HeaderStyle>
                                                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton runat="server" AlternateText="Email" ImageUrl="<%$ PhoenixTheme:images/Email.png %>"
                                                                        CommandName="SENDMAIL" ID="cmdEmail" ToolTip="Send mail to vessel"></asp:ImageButton>
                                                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                                                    <asp:LinkButton runat="server" AlternateText="History" ToolTip="View Send mail History" Width="20PX" Height="20PX"
                                                                        CommandName="HISTORY" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdHistory">
                                                                      <span class="icon"><i class="fa fa-history"></i></span>
                                                                    </asp:LinkButton>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="185px" />
                                                        <Resizing AllowColumnResize="true" />
                                                    </ClientSettings>
                                                    <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                                </telerik:RadGrid>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-12">

                                        <div class="panel panel-success">

                                            <div class="panel-heading">
                                                Monthly JV Not Yet Posted
                                            </div>

                                            <div class="panel-body">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <telerik:RadLabel ID="RadLabel1" runat="server" Text="Month"></telerik:RadLabel>
                                                        </td>
                                                        <td>
                                                            <eluc:Month ID="ddlMonthJV" runat="server" Width="80%" OnTextChangedEvent="ddlMonthJV_TextChangedEvent"
                                                                AutoPostBack="true"></eluc:Month>
                                                        </td>
                                                        <td>
                                                            <telerik:RadLabel ID="RadLabel2" runat="server" Text="Year"></telerik:RadLabel>
                                                        </td>
                                                        <td>
                                                            <eluc:Year ID="ddlYearJV" runat="server" Width="60%" OrderByAsc="false" OnTextChangedEvent="ddlYearJV_TextChangedEvent"
                                                                AutoPostBack="true"></eluc:Year>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <telerik:RadGrid ID="gvMonthlyJV" runat="server" AutoGenerateColumns="false"
                                                    AllowSorting="false" GroupingEnabled="false" OnItemCommand="gvMonthlyJV_ItemCommand" OnItemDataBound="gvMonthlyJV_ItemDataBound"
                                                    EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1000" OnNeedDataSource="gvMonthlyJV_NeedDataSource">
                                                    <MasterTableView TableLayout="Fixed">
                                                        <NoRecordsTemplate>
                                                            <table runat="server" width="100%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Vessel" HeaderStyle-Width="25%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblvesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblvesselid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblPortageBillid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPORTAGEBILLID") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblOCCID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFICECAPTAINCASHID") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblIMBID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINTERNALMONTHLYBILLINGID") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblCCVMAPID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMMITTEDCOSTVOUCHERMAPID") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Portage Bill" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkPBYN" CommandName="PORTAGEBILL" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPBYN") %>'><</asp:LinkButton>
                                                                    <telerik:RadLabel ID="lblPBYN" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPBYN") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Captain Cash" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkCCYN" runat="server" CommandName="CAPTAINCASH" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCCYN") %>'></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Standard Bill" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkStandardBill" runat="server" CommandName="STANDARDBILL" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTANDARDBILLINGYN") %>'></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Committed Cost" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkCommitedYN" runat="server" CommandName="COMMITTEDCOST" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMMITEDCOSTYN") %>'></asp:LinkButton>
                                                                    <telerik:RadLabel ID="lblvslAccid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELACCOUNTID") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblenddate" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENDDATE") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblstartdate" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTARTDATE") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="185px" />
                                                        <Resizing AllowColumnResize="true" />
                                                    </ClientSettings>
                                                    <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                                </telerik:RadGrid>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-lg-6">
                                        <div class="panel panel-success">

                                            <div class="panel-heading">
                                                SOAs Pending Publish
                                            </div>

                                            <div class="panel-body">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <telerik:RadLabel ID="lblsoamonth" runat="server" Text="Month"></telerik:RadLabel>
                                                        </td>
                                                        <td>
                                                            <eluc:Month ID="ddlsoamonth" runat="server" Width="80%" OnTextChangedEvent="ddlMonthSOA_TextChangedEvent"
                                                                AutoPostBack="true"></eluc:Month>
                                                        </td>
                                                        <td>
                                                            <telerik:RadLabel ID="lblsoayear" runat="server" Text="Year"></telerik:RadLabel>
                                                        </td>
                                                        <td>
                                                            <eluc:Year ID="ddlsoayear" runat="server" Width="60%" OrderByAsc="false" OnTextChangedEvent="ddlYearSOA_TextChangedEvent"
                                                                AutoPostBack="true"></eluc:Year>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <telerik:RadGrid ID="gvSOAPendingList" runat="server" AutoGenerateColumns="false"
                                                    AllowSorting="false" GroupingEnabled="false" Height="170px" OnItemDataBound="gvSOAPendingList_ItemDataBound"
                                                    EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1500" OnNeedDataSource="gvSOAPendingList_NeedDataSource">
                                                    <MasterTableView TableLayout="Fixed">
                                                        <NoRecordsTemplate>
                                                            <table runat="server" width="100%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="SOA Type" HeaderStyle-Width="40%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblmeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Pending Published " HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnk2ndcount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECLEVELCOUNT") %>'></asp:LinkButton>
                                                                    <telerik:RadLabel ID="lbl2ndurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECLEVELEURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Pending 2nd Level" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnk1stcount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIRSTLEVELCOUNT") %>'></asp:LinkButton>
                                                                    <telerik:RadLabel ID="lbl1sturl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIRSTLEVELEURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Pending 1st Level" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkpubcount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPUBCOUNT") %>'></asp:LinkButton>
                                                                    <telerik:RadLabel ID="lblpuburl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPUBURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>

                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="185px" />
                                                        <Resizing AllowColumnResize="true" />
                                                    </ClientSettings>
                                                    <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                                </telerik:RadGrid>
                                                <br />
                                            </div>
                                        </div>
                                    </div>

                                    <%--   <div class="col-lg-6">
                                        <div class="panel panel-success">
                                            <div class="panel-heading">
                                                Unbilled Entries
                                            </div>

                                            <div class="panel-body">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <telerik:RadLabel ID="RadLabel4" runat="server" Text="Month"></telerik:RadLabel>
                                                        </td>
                                                        <td>
                                                            <eluc:Month ID="ddlMonthUnbill" runat="server" Width="80%" OnTextChangedEvent="ddlMonthUnbill_TextChangedEvent"
                                                                AutoPostBack="true"></eluc:Month>
                                                        </td>
                                                        <td>
                                                            <telerik:RadLabel ID="RadLabel5" runat="server" Text="Year"></telerik:RadLabel>
                                                        </td>
                                                        <td>
                                                            <eluc:Year ID="ddlYearUnbill" runat="server" Width="60%" OrderByAsc="false" OnTextChangedEvent="ddlYearUnbill_TextChangedEvent"
                                                                AutoPostBack="true"></eluc:Year>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <telerik:RadGrid ID="gvUnbilledEntr" runat="server" AutoGenerateColumns="false"
                                                    AllowSorting="false" GroupingEnabled="false" Height="170px" OnItemDataBound="gvUnbilledEntr_ItemDataBound"
                                                    EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1500" OnNeedDataSource="gvUnbilledEntr_NeedDataSource">
                                                    <MasterTableView TableLayout="Fixed">
                                                        <NoRecordsTemplate>
                                                            <table runat="server" width="100%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Vessel" HeaderStyle-Width="40%">
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="RadLabel6" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Month" HeaderStyle-Width="10%">
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblMonth" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMONTH") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Count" HeaderStyle-Width="10%">
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkCountt" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblCounturl" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="USD" HeaderStyle-Width="20%">
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblUSD" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUM") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="185px" />
                                                        <Resizing AllowColumnResize="true" />
                                                    </ClientSettings>
                                                    <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                                </telerik:RadGrid>
                                            </div>
                                        </div>
                                    </div>--%>
                                </div>
                            </div>

                            <div id="tab2" class="tab-pane" runat="server">

                                <div class="panel-body">

                                    <%--<div class="col-lg-6">
                                        <div class="panel panel-success">

                                            <div class="panel-heading">
                                                Funds Received in Last 7 Days
                                            </div>
                                            <div class="panel-body">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <telerik:RadLabel ID="RadLabel3" runat="server" Text="Sub-Type"></telerik:RadLabel>
                                                        </td>
                                                        <td>
                                                            <telerik:RadComboBox ID="ddlFund7days" runat="server" DataSource='<%# PhoenixRegistersQuick.ListQuick(1, 154)%>' OnTextChanged="ddlFund7days_TextChanged" AutoPostBack="true"
                                                                DataTextField="FLDQUICKNAME" DataValueField="FLDQUICKCODE" OnDataBound="ddlFund7days_DataBound" Width="200px" Filter="Contains" EmptyMessage="Type to select">
                                                            </telerik:RadComboBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <telerik:RadGrid ID="gvFundReceived" runat="server" AutoGenerateColumns="false"
                                                    AllowSorting="false" GroupingEnabled="false" OnItemCommand="gvFundReceived_ItemCommand"
                                                    EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1500">
                                                    <MasterTableView TableLayout="Fixed">
                                                        <NoRecordsTemplate>
                                                            <table runat="server" width="100%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Vessel">
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Sub-Type">
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblSubType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBTYPE") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Reference No">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkRefNO" CommandName="FUNDRECV" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENUMBER") %>'></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Recevied Date">
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblRevDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIVEDDATE") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Currency">
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblCurrencyCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Recevied Amount">
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblRecAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIVEDAMOUNT") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="185px" />
                                                        <Resizing AllowColumnResize="true" />
                                                    </ClientSettings>
                                                    <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                                </telerik:RadGrid>
                                            </div>
                                        </div>
                                    </div>--%>
                                    <%-- <div class="col-lg-6">
                                        <div class="panel panel-success">

                                            <div class="panel-heading">
                                                Outstanding Vessel Funds Requests
                                            </div>

                                            <div class="panel-body">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <telerik:RadLabel ID="lblsubtype" runat="server" Text="Sub-Type"></telerik:RadLabel>
                                                        </td>
                                                        <td>
                                                            <telerik:RadComboBox ID="ddlSubtype" runat="server" DataSource='<%# PhoenixRegistersQuick.ListQuick(1, 154)%>' OnTextChanged="ddlSubtype_TextChanged" AutoPostBack="true"
                                                                DataTextField="FLDQUICKNAME" DataValueField="FLDQUICKCODE" OnDataBound="ddlSubtype_DataBound" Width="200px" Filter="Contains" EmptyMessage="Type to select">
                                                            </telerik:RadComboBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <telerik:RadGrid ID="gvOutstandingFund" runat="server" AutoGenerateColumns="false"
                                                    AllowSorting="false" GroupingEnabled="false" Height="227px" OnItemDataBound="gvOutstandingFund_ItemDataBound"
                                                    EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1000">
                                                    <MasterTableView TableLayout="Fixed">
                                                        <NoRecordsTemplate>
                                                            <table runat="server" width="100%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Principal" HeaderStyle-Width="40%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblmeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblprincipalid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRINCIPALID") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="30-60 days" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnk33dayscount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTHREESIXCOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lbl30daysurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTHREESIXURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="60-90 days" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnk90dayscount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIXNINECOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lbl90daysurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIXNINEURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText=">90 days " HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkgrt90count" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGTRNINECOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblgrt90url" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGTRNINEURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Total" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnktotalcount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALCOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lbltotalurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGTRNINEURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="185px" />
                                                        <Resizing AllowColumnResize="true" />
                                                    </ClientSettings>
                                                    <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                                </telerik:RadGrid>
                                            </div>
                                        </div>
                                    </div>--%>
                                    <%--                                <div class="col-lg-6">
                                        <div class="panel panel-success">
                                            <div class="panel-heading">
                                                Unallocated Funds Receipt
                                            </div>

                                            <div class="panel-body">
                                                <telerik:RadGrid ID="gvUnallFundRcpt" runat="server" AutoGenerateColumns="false" Height="200px"
                                                    AllowSorting="false" GroupingEnabled="false"
                                                    EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1500">
                                                    <MasterTableView TableLayout="Fixed">
                                                        <NoRecordsTemplate>
                                                            <table runat="server" width="100%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Recevied Date" HeaderStyle-Width="10%">
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIVEDDATE", "{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>--%>
                                    <%--<telerik:RadLabel ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIVEDDATE") %>'></telerik:RadLabel>--%>
                                    <%--                               </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Currency" HeaderStyle-Width="8%">
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblSubType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Amount" HeaderStyle-Width="10%">
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT","{0:0.00}") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Company" HeaderStyle-Width="10%">
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblRevDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Bank Account" HeaderStyle-Width="25%">
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblCurrencyCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBANKACCOUNT") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Bank Narrative" HeaderStyle-Width="10%">
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblRecAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBANKNARRATIVE") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="185px" />
                                                        <Resizing AllowColumnResize="true" />
                                                    </ClientSettings>
                                                    <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                                </telerik:RadGrid>
                                            </div>
                                        </div>
                                    </div>--%>

                                    <%--  <div class="col-lg-6">
                                        <div class="panel panel-success">

                                            <div class="panel-heading">
                                                Phonecards Not Yet Arranged
                                            </div>
                                            <div class="panel-body">
                                                <telerik:RadGrid ID="gvPhoneCard" runat="server" AutoGenerateColumns="false"
                                                    AllowSorting="false" GroupingEnabled="false" Height="200px" OnItemDataBound="gvPhoneCard_ItemDataBound"
                                                    EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1000">
                                                    <MasterTableView TableLayout="Fixed">
                                                        <NoRecordsTemplate>
                                                            <table runat="server" width="100%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Vessel" HeaderStyle-Width="40%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblmeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblvesselid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="0-3 days" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnk3dayscount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLESSTHREECOUNT") %>'></asp:LinkButton>
                                                                    <telerik:RadLabel ID="lbl3daysurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLESSTHREEURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="3-7 days" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnk7dayscount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTHREESEVCOUNT") %>'></asp:LinkButton>
                                                                    <telerik:RadLabel ID="lbl7daysurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTHREESEVURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText=">7 days " HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkgrtsevcount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGTRSEVCOUNT") %>'></asp:LinkButton>
                                                                    <telerik:RadLabel ID="lblgrtsevappurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGTRSEVURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Total" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnktotalcount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALCOUNT") %>'></asp:LinkButton>
                                                                    <telerik:RadLabel ID="lbltotalurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGTRSEVURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="185px" />
                                                        <Resizing AllowColumnResize="true" />
                                                    </ClientSettings>
                                                    <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                                </telerik:RadGrid>
                                            </div>
                                        </div>
                                    </div>--%>
                                    <%--    <div class="col-lg-6">
                                        <div class="panel panel-success">

                                            <div class="panel-heading">
                                                Outstanding Funds Requests
                                            </div>

                                            <div class="panel-body">
                                                <telerik:RadGrid ID="gvOutstandingTotal" runat="server" AutoGenerateColumns="false" BorderStyle="None" Height="200px"
                                                    AllowSorting="false" GroupingEnabled="false" ShowHeader="false" ShowFooter="false" OnItemDataBound="gvOutstandingTotal_ItemDataBound"
                                                    EnableHeaderContextMenu="true">
                                                    <MasterTableView TableLayout="Fixed">
                                                        <NoRecordsTemplate>
                                                            <table runat="server" width="100%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkcount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblurl" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDURL") %>' Visible="false"></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </div>
                                        </div>
                                    </div>--%>

                                    <%--  <div class="col-lg-6">
                                        <div class="panel panel-success">
                                            <div class="panel-heading">
                                                Allotment Not Yet Processed
                                            </div>

                                            <div class="panel-body">
                                                <telerik:RadGrid ID="gvAllotNYP" runat="server" AutoGenerateColumns="false"
                                                    AllowSorting="false" GroupingEnabled="false" Height="200px"
                                                    EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1000">
                                                    <MasterTableView TableLayout="Fixed">
                                                        <NoRecordsTemplate>
                                                            <table runat="server" width="100%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Vessel" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblmeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblvesselid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Allotment Type" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblbaa" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblvv" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="0-3 days" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnk3dayscount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLESSTHREECOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lbl3daysurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLESSTHREEURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="3-7 days " HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkgrtsevcount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTHREESEVCOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblgrtsevappurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTHREESEVURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="7-14 days " HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkgrt714count" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEVFORTEENCOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblgrt714url" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEVFORTEENSEVURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText=">14 Days" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkGTR14Count" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGTRFORTEENCOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblGTR14url" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGTRFORTEENURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Total" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnktotalcount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALCOUNT") %>'></telerik:RadLabel>
                                                                    <%--                                                                    <telerik:RadLabel ID="lbltotalurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGTRSEVURL") %>'></telerik:RadLabel>--%>
                                    <%--</ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="185px" />
                                                        <Resizing AllowColumnResize="true" />
                                                    </ClientSettings>
                                                    <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                                </telerik:RadGrid>
                                            </div>
                                        </div>
                                    </div>--%>
                                </div>
                            </div>

                            <div id="tab3" class="tab-pane">
                                <div class="panel-body">
                                    <%--    <div class="col-lg-6">
                                        <div class="panel panel-success">
                                            <div class="panel-heading">
                                                Invoice PMV: Partially Approved
                                            </div>
                                            <div class="panel-body">
                                                <telerik:RadGrid ID="gvInvoiceParApp" runat="server" AutoGenerateColumns="false"
                                                    AllowSorting="false" GroupingEnabled="false" Height="200px" OnItemDataBound="gvInvoiceParApp_ItemDataBound"
                                                    EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1000">
                                                    <MasterTableView TableLayout="Fixed">
                                                        <NoRecordsTemplate>
                                                            <table runat="server" width="100%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Company" HeaderStyle-Width="40%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblmeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="0-3 days" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnk3dayscount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLESSTHREECOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lbl3daysurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLESSTHREEURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="3-7 days" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnk7dayscount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTHREESEVCOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lbl7daysurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTHREESEVURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText=">7 days " HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkgrtsevcount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGTRSEVCOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblgrtsevappurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGTRSEVURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Total" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnktotalcount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALCOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lbltotalurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGTRSEVURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="185px" />
                                                        <Resizing AllowColumnResize="true" />
                                                    </ClientSettings>
                                                    <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                                </telerik:RadGrid>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-6">
                                        <div class="panel panel-success">
                                            <div class="panel-heading">
                                                Invoice + Advance PMV for Follow-Up
                                            </div>

                                            <div class="panel-body">
                                                <telerik:RadGrid ID="gvInvoiceAdvPMV" runat="server" AutoGenerateColumns="false"
                                                    AllowSorting="false" GroupingEnabled="false" Height="200px" OnItemDataBound="gvInvoiceAdvPMV_ItemDataBound"
                                                    EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1000">
                                                    <MasterTableView TableLayout="Fixed">
                                                        <NoRecordsTemplate>
                                                            <table runat="server" width="100%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Aging" HeaderStyle-Width="40%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblmeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Pending Approval" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkpendappcount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPENDINGCOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblpendappurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPENDINGEURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Partially Approved" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkparappcount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARTIALCOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblparappurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARTIALURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Fully Approved" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkfullappcount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFULLCOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblfullappurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFULLURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>

                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="185px" />
                                                        <Resizing AllowColumnResize="true" />
                                                    </ClientSettings>
                                                    <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                                </telerik:RadGrid>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-3">
                                        <div class="panel panel-success">

                                            <div class="panel-heading">
                                                Invoices: Accounts Checking
                                            </div>

                                            <div class="panel-body panelfont">
                                                <telerik:RadGrid ID="gvInvoiceAccChecking" runat="server" AutoGenerateColumns="false"
                                                    AllowSorting="false" GroupingEnabled="false" Height="130px" OnItemDataBound="gvInvoiceAccChecking_ItemDataBound"
                                                    EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1000">
                                                    <MasterTableView TableLayout="Fixed">
                                                        <NoRecordsTemplate>
                                                            <table runat="server" width="100%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Invoice" HeaderStyle-Width="15%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblmeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Account Checking" HeaderStyle-Width="25%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkacccheckingcount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICECOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblacccheckingurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICEURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="185px" />
                                                        <Resizing AllowColumnResize="true" />
                                                    </ClientSettings>
                                                    <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                                </telerik:RadGrid>
                                            </div>

                                        </div>
                                    </div>

                                    <div class="col-lg-3">
                                        <div class="panel panel-success">
                                            <div class="panel-heading">
                                                Advance Payment: Accounts Checking
                                            </div>
                                            <div class="panel-body panelfont">
                                                <telerik:RadGrid ID="gvAdvanceAccChecking" runat="server" AutoGenerateColumns="false"
                                                    AllowSorting="false" GroupingEnabled="false" Height="130px" OnItemDataBound="gvAdvanceAccChecking_ItemDataBound"
                                                    EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1000">
                                                    <MasterTableView TableLayout="Fixed">
                                                        <NoRecordsTemplate>
                                                            <table runat="server" width="100%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Adv. Payment" HeaderStyle-Width="35%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblmeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="PO Adv." HeaderStyle-Width="25%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkpoadvancecount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADVCOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblpoadvanceurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADVURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Deposit" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkdepositcount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPOSITCOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lbldepositurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPOSITURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>

                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="185px" />
                                                        <Resizing AllowColumnResize="true" />
                                                    </ClientSettings>
                                                    <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                                </telerik:RadGrid>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-lg-3">
                                        <div class="panel panel-success">
                                            <div class="panel-heading">
                                                Generate PMV
                                            </div>
                                            <div class="panel-body">
                                                <telerik:RadGrid ID="gvGeneratePMV" runat="server" AutoGenerateColumns="false"
                                                    AllowSorting="false" GroupingEnabled="false" Height="130px" OnItemDataBound="gvGeneratePMV_ItemDataBound"
                                                    EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1000">
                                                    <MasterTableView TableLayout="Fixed">
                                                        <NoRecordsTemplate>
                                                            <table runat="server" width="100%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="PMV" HeaderStyle-Width="30%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblmeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Invoice" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkinvoicecount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICECOUNT") %>'></asp:LinkButton>
                                                                    <telerik:RadLabel ID="lblinvoiceurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICEURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="PO Advance" HeaderStyle-Width="23%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkpoadvancecount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOCOUNT") %>'></asp:LinkButton>
                                                                    <telerik:RadLabel ID="lblpoadvanceurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Deposit" HeaderStyle-Width="23%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkdepositcount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPOSITCOUNT") %>'></asp:LinkButton>
                                                                    <telerik:RadLabel ID="lbldepositurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPOSITURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>

                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="185px" />
                                                        <Resizing AllowColumnResize="true" />
                                                    </ClientSettings>
                                                    <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                                </telerik:RadGrid>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-3">
                                        <div class="panel panel-success">

                                            <div class="panel-heading">
                                                PO Advances Follow-up (USD)
                                            </div>

                                            <div class="panel-body">
                                                <telerik:RadGrid ID="gvPOAdvFollowUp" runat="server" AutoGenerateColumns="false"
                                                    AllowSorting="false" GroupingEnabled="false" Height="130px"
                                                    EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1000">
                                                    <MasterTableView TableLayout="Fixed">
                                                        <NoRecordsTemplate>
                                                            <table runat="server" width="100%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Months" HeaderStyle-Width="15%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblMeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></telerik:RadLabel>
                                                                    <%--<telerik:RadLabel ID="lbl36url" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICEURL") %>'></telerik:RadLabel>--%>
                                    <%--                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Amount" HeaderStyle-Width="25%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADVANCEAMOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblAmount" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADVANCEURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="185px" />
                                                        <Resizing AllowColumnResize="true" />
                                                    </ClientSettings>
                                                    <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                                </telerik:RadGrid>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-lg-4">
                                        <div class="panel panel-success">

                                            <div class="panel-heading">
                                                PO Advance/ Deposit PMV: RMT/CAS Not Generated
                                            </div>

                                            <div class="panel-body">
                                                <telerik:RadGrid ID="gvPOADPMVNotGen" runat="server" AutoGenerateColumns="false"
                                                    AllowSorting="false" GroupingEnabled="false" Height="180px" OnItemDataBound="gvInvoiceParApp_ItemDataBound"
                                                    EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1000">
                                                    <MasterTableView TableLayout="Fixed">
                                                        <NoRecordsTemplate>
                                                            <table runat="server" width="100%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Company" HeaderStyle-Width="40%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblmeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="0-3 days" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnk3dayscount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTHREECOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lbl3daysurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTHREEURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="3-7 days" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnk7dayscount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTHREESEVENCOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lbl7daysurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTHREESEVENURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText=">7 days " HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkgrtsevcount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGTRSEVENCOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblgrtsevappurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGTRSEVENURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Total" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnktotalcount1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALCOUNT") %>'></telerik:RadLabel>--%>
                                    <%--<telerik:RadLabel ID="lbltotalurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGTRSEVURL") %>'></telerik:RadLabel>--%>
                                    <%--                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="185px" />
                                                        <Resizing AllowColumnResize="true" />
                                                    </ClientSettings>
                                                    <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                                </telerik:RadGrid>
                                            </div>
                                        </div>
                                    </div>


                                    <div class="col-lg-4">
                                        <div class="panel panel-success">

                                            <div class="panel-heading">
                                                Rebates Follow-up (USD)
                                            </div>

                                            <div class="panel-body">
                                                <telerik:RadGrid ID="gvRebatesFlw" runat="server" AutoGenerateColumns="false"
                                                    AllowSorting="false" GroupingEnabled="false" Height="180px"
                                                    EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1000">
                                                    <MasterTableView TableLayout="Fixed">
                                                        <NoRecordsTemplate>
                                                            <table runat="server" width="100%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Months" HeaderStyle-Width="15%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblMeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></telerik:RadLabel>--%>
                                    <%--<telerik:RadLabel ID="lbl36url" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICEURL") %>'></telerik:RadLabel>--%>
                                    <%--                     </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Amount" HeaderStyle-Width="25%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREBATEAMOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblAmount" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREBATEURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="185px" />
                                                        <Resizing AllowColumnResize="true" />
                                                    </ClientSettings>
                                                    <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                                </telerik:RadGrid>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-lg-4">
                                        <div class="panel panel-success">
                                            <div class="panel-heading">
                                                Sluggish Invoices
                                            </div>

                                            <div class="panel-body">
                                                <telerik:RadGrid ID="gvSluggishInvoice" runat="server" AutoGenerateColumns="false"
                                                    AllowSorting="false" GroupingEnabled="false" Height="180px"
                                                    EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1000">
                                                    <MasterTableView TableLayout="Fixed">
                                                        <NoRecordsTemplate>
                                                            <table runat="server" width="100%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="PIC" HeaderStyle-Width="15%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkPIC" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERNAME") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblMeasure" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="0-3 Days" HeaderStyle-Width="25%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnk03Count" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTHREECOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lbl03url" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTHREEURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="3-7 Days" HeaderStyle-Width="25%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkGT37Count" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTHREESEVENCOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblGT37url" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTHREESEVENURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText=">7 Days" HeaderStyle-Width="25%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkGT7Count" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGTRSEVENCOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblGT7url" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGTRSEVENURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Total" HeaderStyle-Width="25%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>--%>
                                    <%--<asp:LinkButton ID="lnkGT90Count" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICECOUNT") %>'></asp:LinkButton>--%>
                                    <%--                            <telerik:RadLabel ID="lblTotal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALCOUNT") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="185px" />
                                                        <Resizing AllowColumnResize="true" />
                                                    </ClientSettings>
                                                    <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                                </telerik:RadGrid>
                                            </div>
                                        </div>
                                    </div>--%>
                                </div>
                            </div>

                            <div id="tab4" class="tab-pane">

                                <div class="panel-body">

                                    <%--  <div class="col-lg-6">
                                        <div class="panel panel-success">
                                            <div class="panel-heading">
                                                Priority PMV: RMT Not Generated
                                            </div>

                                            <div class="panel-body">
                                                <telerik:RadGrid ID="gvRMTNotGen" runat="server" AutoGenerateColumns="false"
                                                    AllowSorting="false" GroupingEnabled="false" Height="200px" OnItemDataBound="gvRMTNotGen_ItemDataBound"
                                                    EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1000">
                                                    <MasterTableView TableLayout="Fixed">
                                                        <NoRecordsTemplate>
                                                            <table runat="server" width="100%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Company" HeaderStyle-Width="40%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblmeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="CTM" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkADVPMVCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADVPMVCOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblADVPMVurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADVPMVURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="PO Advance" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkCTMPMVCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCTMPMVCOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblCTMPMVurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCTMPMVURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Deposit" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkdepositPMVcount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPOSITPMVCOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lbldepositPMVurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPOSITPMVURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Total" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkTotalcount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALCOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblTotalurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="185px" />
                                                        <Resizing AllowColumnResize="true" />
                                                    </ClientSettings>
                                                    <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                                </telerik:RadGrid>
                                            </div>
                                        </div>
                                    </div>--%>

                                    <%--   <div class="col-lg-6">
                                        <div class="panel panel-success">
                                            <div class="panel-heading">
                                                Approved PMV: RMT Not Generated
                                            </div>

                                            <div class="panel-body">
                                                <telerik:RadGrid ID="gvApprovedPMVRMTnotGen" runat="server" AutoGenerateColumns="false"
                                                    AllowSorting="false" GroupingEnabled="false" Height="200px" OnItemDataBound="gvApprovedPMVRMTnotGen_ItemDataBound"
                                                    EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1000">
                                                    <MasterTableView TableLayout="Fixed">
                                                        <NoRecordsTemplate>
                                                            <table runat="server" width="100%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Company" HeaderStyle-Width="40%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblCname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblmeasure" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="0-3 days" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkThreeCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTHREECOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblThreeurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTHREEURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="3-7 days" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkThreeSevenCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTHREESEVENCOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblThreeSevenurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTHREESEVENURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText=">7 days" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkdGTRSevenCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGTRSEVENCOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblGTRSevenurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGTRSEVENURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Total" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblTotalC" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALCOUNT") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="185px" />
                                                        <Resizing AllowColumnResize="true" />
                                                    </ClientSettings>
                                                    <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                                </telerik:RadGrid>
                                            </div>
                                        </div>
                                    </div>--%>

                                    <%--     <div class="col-lg-6">
                                        <div class="panel panel-success">
                                            <div class="panel-heading">
                                                Approved Allotment PMV: RMT Not Generated
                                            </div>

                                            <div class="panel-body">
                                                <telerik:RadGrid ID="gvApprovedAllotM" runat="server" AutoGenerateColumns="false"
                                                    AllowSorting="false" GroupingEnabled="false" Height="200px" OnItemDataBound="gvApprovedAllotM_ItemDataBound"
                                                    EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1000">
                                                    <MasterTableView TableLayout="Fixed">
                                                        <NoRecordsTemplate>
                                                            <table runat="server" width="100%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Company" HeaderStyle-Width="40%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblmeasure" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblCName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="0-3 Days" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnk03Count" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTHREECOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lbl03url" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTHREEURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="3-7 Days" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnk37Count" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTHREESEVENCOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lbl37url" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTHREESEVENURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText=">7 Days" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkGTR7Count" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGTRSEVENCOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblGTR7url" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGTRSEVENURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Total" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblTotalC" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALCOUNT") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="185px" />
                                                        <Resizing AllowColumnResize="true" />
                                                    </ClientSettings>
                                                    <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                                </telerik:RadGrid>
                                            </div>
                                        </div>
                                    </div>--%>

                                    <%--   <div class="col-lg-6">
                                        <div class="panel panel-success">
                                            <div class="panel-heading">
                                                Remittances Count
                                            </div>

                                            <div class="panel-body">
                                                <telerik:RadGrid ID="gvRemittanceCount" runat="server" AutoGenerateColumns="false"
                                                    AllowSorting="false" GroupingEnabled="false" Height="200px" OnItemDataBound="gvRemittanceCount_ItemDataBound"
                                                    EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1000">
                                                    <MasterTableView TableLayout="Fixed">
                                                        <NoRecordsTemplate>
                                                            <table runat="server" width="100%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Company" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblmeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Remittance Checking" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkRemittanceCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMITTANCECHECKINGCOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblRemittanceurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMITTANCECHECKINGURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Verified" HeaderStyle-Width="17%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkVerifiedCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVERIFIEDCOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblVerifiedurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVERIFIEDURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Payment Authorization" HeaderStyle-Width="22%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkPACount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYAUTHCOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblPAurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYAUTHURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="FX Contract Assignment" HeaderStyle-Width="22%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkFXACount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFXCONTCOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblFXurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFXCONTURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Preparing Instructions to Bank" HeaderStyle-Width="22%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkPIBcount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPREINSBANKCOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblPIBurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPREINSBANKURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="File NotGenerated" HeaderStyle-Width="17%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkFileNotCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENOTGENCOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblFileNoturl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENOTGENURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="File Generated" HeaderStyle-Width="17%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkFileCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILEGENCOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblFileurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILEGENURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="185px" />
                                                        <Resizing AllowColumnResize="true" />
                                                    </ClientSettings>
                                                    <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                                </telerik:RadGrid>
                                            </div>
                                        </div>
                                    </div>--%>
                                </div>
                            </div>

                            <div id="tab5" class="tab-pane">

                                <div class="panel-body">

                                    <%--  <div class="col-lg-6">
                                        <div class="panel panel-success">
                                            <div class="panel-heading">
                                                Pending Funds Requests (USD est.)
                                            </div>

                                            <div class="panel-body">
                                                <telerik:RadGrid ID="gvPendingFR" runat="server" AutoGenerateColumns="false"
                                                    AllowSorting="false" GroupingEnabled="false" Height="200px" OnItemDataBound="gvPendingFR_ItemDataBound"
                                                    EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1000">
                                                    <MasterTableView TableLayout="Fixed">
                                                        <NoRecordsTemplate>
                                                            <table runat="server" width="100%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Company" HeaderStyle-Width="25%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblmeasure" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblCName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Bank Account (CUR)" HeaderStyle-Width="35%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblBankInfo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBANKACINFO") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Overdue 2wk" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkOD2WKAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOD2WKAMOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblOD2WKurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOD2WKURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Overdue 1 wk" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkOD1WKAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOD1WKAMOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblOD1WKurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOD1WKURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="This Week" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkThisWKAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTHISWKAMOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel Visible="false" ID="lblThisWKurl" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTHISWKURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Next Week" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkNEXTWK" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEXTWKAMOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblNEXTWKurl" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEXTWKURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Following Weeks" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblFollowingWK" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFOLLWINGWKAMOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lnkFollowingWKurl" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFOLLWINGWKURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="185px" />
                                                        <Resizing AllowColumnResize="true" />
                                                    </ClientSettings>
                                                    <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                                </telerik:RadGrid>
                                            </div>
                                        </div>
                                    </div>--%>

                                    <%--    <div class="col-lg-6">
                                        <div class="panel panel-success">
                                            <div class="panel-heading">
                                                Bank Balances
                                            </div>

                                            <div class="panel-body">
                                                <telerik:RadGrid ID="gvBankBalance" runat="server" AutoGenerateColumns="false"
                                                    AllowSorting="false" GroupingEnabled="false" Height="200px"
                                                    EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1000">
                                                    <MasterTableView TableLayout="Fixed">
                                                        <NoRecordsTemplate>
                                                            <table runat="server" width="100%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Company" HeaderStyle-Width="25%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblCName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANY") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Bank Account (Cur)" HeaderStyle-Width="40%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblBankAcut" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="AC Currency" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblACCurr" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBANKAMOUNT") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="USD(est.)" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblUSD" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSDAMOUNT") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="185px" />
                                                        <Resizing AllowColumnResize="true" />
                                                    </ClientSettings>
                                                    <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                                </telerik:RadGrid>
                                            </div>
                                        </div>
                                    </div>--%>

                                    <%-- <div class="col-lg-6">
                                        <div class="panel panel-success">
                                            <div class="panel-heading">
                                                Estimated Firm Outflows
                                            </div>

                                            <div class="panel-body">
                                                <telerik:RadGrid ID="gvEstimatedFU" runat="server" AutoGenerateColumns="false"
                                                    AllowSorting="false" GroupingEnabled="false" Height="200px" OnItemDataBound="gvEstimatedFU_ItemDataBound"
                                                    EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1000">
                                                    <MasterTableView TableLayout="Fixed">
                                                        <NoRecordsTemplate>
                                                            <table runat="server" width="100%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Company" HeaderStyle-Width="25%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblCName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="CTM" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkCTMSum" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCTMPMVSUM") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblCTMurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCTMPMVURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="PO Advance" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkPOAdv" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADVPMVSUM") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblPOAdvurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADVPMVURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Deposite" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkDepositePMV" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPOSITPMVSUM") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblDepositeurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPOSITPMVURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Under Processing" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkUnderProc" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMSUM") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblUnderProc" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="USD(est.)" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkTotalSum" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALSUM") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblTotalurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="185px" />
                                                        <Resizing AllowColumnResize="true" />
                                                    </ClientSettings>
                                                    <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                                </telerik:RadGrid>
                                            </div>
                                        </div>
                                    </div>--%>

                                    <%--    <div class="col-lg-6">
                                        <div class="panel panel-success">
                                            <div class="panel-heading">
                                                Approved Supplier PMVs
                                            </div>

                                            <div class="panel-body">
                                                <telerik:RadGrid ID="gvApprovedSupplPMVs" runat="server" AutoGenerateColumns="false"
                                                    AllowSorting="false" GroupingEnabled="false" Height="200px" OnItemDataBound="gvApprovedSupplPMVs_ItemDataBound"
                                                    EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1000">
                                                    <MasterTableView TableLayout="Fixed">
                                                        <NoRecordsTemplate>
                                                            <table runat="server" width="100%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Company" HeaderStyle-Width="25%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblCName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblmeasure" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="0-1 week" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkLT1WK" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLT1WKSUM") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblLT1WKurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLT1WKURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="1-2 week" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkLT2WK" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLT2WKSUM") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblLT2WKurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLT2WKURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="2-4 week" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkLT4WK" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLT4WKSUM") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblLT4WKurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLT4WKURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText=">4 week" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkGT4WK" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGT4WKSUM") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblGT4WKurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGT4WKURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="USD(est.)" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkTotalSum" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALSUM") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblTotalurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="185px" />
                                                        <Resizing AllowColumnResize="true" />
                                                    </ClientSettings>
                                                    <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                                </telerik:RadGrid>
                                            </div>
                                        </div>
                                    </div>--%>
                                </div>
                            </div>

                            <div id="tab6" class="tab-pane">
                                <div class="panel-body">

                                    <%--  <div class="col-lg-4">
                                        <div class="panel panel-success">
                                            <div class="panel-heading">
                                                Cash Balances(USD)
                                            </div>

                                            <div class="panel-body">
                                                <telerik:RadGrid ID="gvCashUSD" runat="server" AutoGenerateColumns="false"
                                                    AllowSorting="false" GroupingEnabled="false" Height="200px"
                                                    EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1000">
                                                    <MasterTableView TableLayout="Fixed">
                                                        <NoRecordsTemplate>
                                                            <table runat="server" width="100%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Company" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblCName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANY") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Currency" HeaderStyle-Width="30%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblAccountCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Amount" HeaderStyle-Width="15%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblCashAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCASHAMOUNT") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="185px" />
                                                        <Resizing AllowColumnResize="true" />
                                                    </ClientSettings>
                                                    <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                                </telerik:RadGrid>
                                            </div>
                                        </div>
                                    </div>--%>
                                    <%--    <div class="col-lg-4">
                                        <div class="panel panel-success">
                                            <div class="panel-heading">
                                                Cash Balances(SGD)
                                            </div>

                                            <div class="panel-body">
                                                <telerik:RadGrid ID="gvCashSGD" runat="server" AutoGenerateColumns="false"
                                                    AllowSorting="false" GroupingEnabled="false" Height="200px"
                                                    EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1000">
                                                    <MasterTableView TableLayout="Fixed">
                                                        <NoRecordsTemplate>
                                                            <table runat="server" width="100%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Company" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblCName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANY") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Currency" HeaderStyle-Width="30%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblAccountCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Amount" HeaderStyle-Width="15%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblCashAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCASHAMOUNT") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="185px" />
                                                        <Resizing AllowColumnResize="true" />
                                                    </ClientSettings>
                                                    <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                                </telerik:RadGrid>
                                            </div>
                                        </div>
                                    </div>--%>
                                    <%--  <div class="col-lg-4">
                                        <div class="panel panel-success">
                                            <div class="panel-heading">
                                                Cash Balances(MYR)
                                            </div>

                                            <div class="panel-body">
                                                <telerik:RadGrid ID="gvCashMYR" runat="server" AutoGenerateColumns="false"
                                                    AllowSorting="false" GroupingEnabled="false" Height="200px"
                                                    EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1000">
                                                    <MasterTableView TableLayout="Fixed">
                                                        <NoRecordsTemplate>
                                                            <table runat="server" width="100%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Company" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblCName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANY") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Currency" HeaderStyle-Width="30%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblAccountCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Amount" HeaderStyle-Width="15%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblCashAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCASHAMOUNT") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="185px" />
                                                        <Resizing AllowColumnResize="true" />
                                                    </ClientSettings>
                                                    <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                                </telerik:RadGrid>
                                            </div>
                                        </div>
                                    </div>--%>
                                    <%--    <div class="col-lg-2">
                                        <div class="panel panel-success">

                                            <div class="panel-heading">
                                                Local Claims Posting
                                            </div>

                                            <div class="panel-body">
                                                <telerik:RadGrid ID="gvLocalCP" runat="server" AutoGenerateColumns="false"
                                                    AllowSorting="false" GroupingEnabled="false" Height="200px" OnItemDataBound="gvLocalCP_ItemDataBound"
                                                    EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1000">
                                                    <MasterTableView TableLayout="Fixed">
                                                        <NoRecordsTemplate>
                                                            <table runat="server" width="100%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText=">7 Days" HeaderStyle-Width="40%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkGT7Days" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLTSEVENCOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblGT7Days" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLTSEVENURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="<7 Days" HeaderStyle-Width="40%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkLS7Days" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGTSEVENCOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblLSDays" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGTSEVENURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="185px" />
                                                        <Resizing AllowColumnResize="true" />
                                                    </ClientSettings>
                                                    <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                                </telerik:RadGrid>
                                            </div>
                                        </div>
                                    </div>--%>

                                    <div class="col-lg-12">
                                        <div class="panel panel-success">
                                            <div class="panel-heading">
                                                CTM to be Arranged
                                            </div>

                                            <div class="panel-body">
                                                <telerik:RadGrid ID="gvCTMArrng" runat="server" AutoGenerateColumns="false"
                                                    AllowSorting="false" GroupingEnabled="false" Height="200px" OnItemCommand="gvCTMArrng_ItemCommand"
                                                    EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1000">
                                                    <MasterTableView TableLayout="Fixed">
                                                        <NoRecordsTemplate>
                                                            <table runat="server" width="100%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Vessel" HeaderStyle-Width="25%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:LinkButton>
                                                                    <telerik:RadLabel ID="lblVesselID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblCaptainCashID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCAPTAINCASHID") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="ETA" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblETA" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDETA", "{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="ETD" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblETD" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDETD", "{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Country " HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblCountry" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYNAME") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Currency " HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Amount " HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Days Lapsed " HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblDaysLapsed" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDELAPSED") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="185px" />
                                                        <Resizing AllowColumnResize="true" />
                                                    </ClientSettings>
                                                    <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                                </telerik:RadGrid>
                                            </div>
                                        </div>
                                    </div>

                                    <%--   <div class="col-lg-5">
                                        <div class="panel panel-success">
                                            <div class="panel-heading">
                                                Travel Advances to be Paid
                                            </div>

                                            <div class="panel-body">
                                                <telerik:RadGrid ID="gvTravelAdv" runat="server" AutoGenerateColumns="false"
                                                    AllowSorting="false" GroupingEnabled="false" Height="200px" OnItemDataBound="gvTravelAdv_ItemDataBound"
                                                    EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1000">
                                                    <MasterTableView TableLayout="Fixed">
                                                        <NoRecordsTemplate>
                                                            <table runat="server" width="100%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Company" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblmeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="PMV Approval" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkPMVAppcount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPMVAPPROVALCOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblPMVAppsurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPMVAPPROVALURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Cash Request Generation" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkCRGcount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCASHREQGENCOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblCRGsurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCASHREQGENURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Pending Posting" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lnkgPPcount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPENDINGPOSTCOUNT") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblPPurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPENDINGPOSTURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="185px" />
                                                        <Resizing AllowColumnResize="true" />
                                                    </ClientSettings>
                                                    <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                                </telerik:RadGrid>
                                            </div>
                                        </div>
                                    </div>--%>

                                    <div class="col-lg-12">
                                        <div class="panel panel-success">
                                            <div class="panel-heading">
                                                Arranged CTM Not Yet Received
                                            </div>

                                            <div class="panel-body">
                                                <telerik:RadGrid ID="gvArranCTMNOTRev" runat="server" AutoGenerateColumns="false"
                                                    AllowSorting="false" GroupingEnabled="false" Height="200px"
                                                    EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1000">
                                                    <MasterTableView TableLayout="Fixed">
                                                        <NoRecordsTemplate>
                                                            <table runat="server" width="100%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Vessel" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblmeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="ETA" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblETA" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDETA", "{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="ETD" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblETD" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDETD", "{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Country" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblCountry" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYNAME") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Currency" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Amount" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Arranged Via" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <%--<telerik:RadLabel ID="lblArrangedVia" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGTRONETWECOUNT") %>'></telerik:RadLabel>--%>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Company" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblCompany" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPPLIERNAME") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="PMV" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblPMV" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTVOUCHERNUMBER") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="PMV Status" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblPMVStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTVOUCHERSTATUS") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="RMT/CAS" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblRMTCAS" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMITTANCENUMBER") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Received Amount" HeaderStyle-Width="20%">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblReceivedAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIVEDAMOUNT") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="185px" />
                                                        <Resizing AllowColumnResize="true" />
                                                    </ClientSettings>
                                                    <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                                </telerik:RadGrid>
                                            </div>
                                        </div>
                                    </div>

                                    <%--                                <div class="col-lg-6">
                                    <div class="panel panel-success">

                                        <div class="panel-heading">
                                            Old Pending POs (USD)
                                        </div>

                                        <div class="panel-body" style="height: 235px">

                                            <telerik:RadGrid ID="gvOldpendingPOS" runat="server" AutoGenerateColumns="false"
                                                AllowSorting="false" GroupingEnabled="false" Height="200px"
                                                EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1000" OnNeedDataSource="gvOldpendingPOS_NeedDataSource">
                                                <MasterTableView TableLayout="Fixed">
                                                    <NoRecordsTemplate>
                                                        <table runat="server" width="100%" border="0">
                                                            <tr>
                                                                <td align="center">
                                                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </NoRecordsTemplate>
                                                    <Columns>
                                                        <telerik:GridTemplateColumn HeaderText="Vessel" HeaderStyle-Width="40%">
                                                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                            <ItemTemplate>
                                                                <telerik:RadLabel ID="lblmeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></telerik:RadLabel>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="60-90 days" HeaderStyle-Width="20%">
                                                            <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                            <ItemTemplate>
                                                                <telerik:RadLabel ID="lnksixtydayscount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIXTYCOUNT") %>'></telerik:RadLabel>
                                                                <telerik:RadLabel ID="lblsixtydaysurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIXTYURL") %>'></telerik:RadLabel>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="90-120 days" HeaderStyle-Width="20%">
                                                            <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                            <ItemTemplate>
                                                                <telerik:RadLabel ID="lnkninetydayscount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNINETYCOUNT") %>'></telerik:RadLabel>
                                                                <telerik:RadLabel ID="lblninetydaysurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNINETYURL") %>'></telerik:RadLabel>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText=">120 days " HeaderStyle-Width="20%">
                                                            <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                                            <ItemTemplate>
                                                                <telerik:RadLabel ID="lnkgrtonetwecount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGTRONETWECOUNT") %>'></telerik:RadLabel>
                                                                <telerik:RadLabel ID="lblgrtonetweurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGTRONETWEURL") %>'></telerik:RadLabel>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                    </Columns>
                                                </MasterTableView>
                                                <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="185px" />
                                                    <Resizing AllowColumnResize="true" />
                                                </ClientSettings>
                                                <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                            </telerik:RadGrid>

                                        </div>
                                    </div>
                                </div>--%>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
        </telerik:RadAjaxPanel>
    </form>
    <script type="text/javascript" src="../Scripts/jquery-3.4.1.min.js"></script>
    <script type="text/javascript" src="../Scripts/bootstrap.min.v3.js"></script>
</body>
</html>
