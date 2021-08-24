<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogDeckBook.aspx.cs" Inherits="Log_ElectricLogDeckBook" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Deck log</title>
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
            function pageLoad(sender, eventArgs) {
          <%--       if (!eventArgs.get_isPartialLoad()) {
                     setTimeout(function () { $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("Operation"); }, 50);
                     setTimeout(function () { $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("OrdersInfomation"); }, 300);
                     setTimeout(function () { $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("AccidentsNearMisses"); }, 600);
                     setTimeout(function () { $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("Purchase"); }, 900);
                 }--%>
             }
        </script>
        <style>
            .underline {
                text-decoration: underline;
                display:inline-block;
            }
            .text-rotate {
                 transform: rotate(-90deg);

                 /* Legacy vendor prefixes that you probably don't need... */

                 /* Safari */
                 -webkit-transform: rotate(-90deg);

                 /* Firefox */
                 -moz-transform: rotate(-90deg);

                 /* IE */
                 -ms-transform: rotate(-90deg);

                 /* Opera */
                 -o-transform: rotate(-90deg);

                 /* Internet Explorer */
                 filter: progid:DXImageTransform.Microsoft.BasicImage(rotation=3);
            }

            .bordered-table td {
                width: 30px;
                height: 30px;
                border:1px solid black;
                border-collapse: collapse;
            }

            .bordered-table th {
                border:1px solid black;
                border-collapse: collapse;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
     <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1"></telerik:RadAjaxLoadingPanel>
        <div class="gray-bg">
            <div class="row page-heading bg-success">

                <div class="col-lg-12">
                    <h2 class="font-bold">
                        <asp:Literal ID="lblVessel" runat="server" Text=""></asp:Literal></h2>
                    <h4>
                        <asp:Literal ID="ltrlVoyageInfo" runat="server" Text=""></asp:Literal>

                        <button class="btn btn-primary " type="button" onclick="javascript: top.openNewWindow('dsd','Instructions','Log/ElectricLogDeckLogInstructions.aspx')">
                            <i class="fas fa-tachometer-alt"></i>&nbsp;Instructions
                        </button>
                        <button class="btn btn-primary " type="button" onclick="javascript: top.openNewWindow('dsd','Record of Drills and Training Check LSA/ FFA','Log/ElectricLogDeckLogDrills.aspx')">
                            <i class="fas fa-cogs"></i>&nbsp;Record of Drills and Training Check LSA/ FFA</button>
                        <button class="btn btn-primary " type="button" onclick="javascript: top.openNewWindow('dsd','Configuration','Log/ElecticLogLoadLineDraughtWater.aspx')">
                            <i class="fas fa-cogs"></i>&nbsp;Load Line and Draught Water</button>
                        <button class="btn btn-warning " type="button" onclick="javascript: top.openNewWindow('dsd','Configuration','Log/ElectricLogDeckLogConfiguration.aspx')">
                            <i class="fa fa-flag"></i>&nbsp;Configuration
                        </button>
                    </h4>

                </div>

            </div>

            <div class="row">

                <div class="col-lg-8">
                    <br />

                    <div class="col-lg-12">

                        <div class="panel panel-success">
                            <div class="panel-heading">
                                Wind Log | <span class="label label-primary" style="cursor: pointer" onclick="return false;">Add</span>
                            </div>
                            <div class="panel-body">
                                <telerik:RadGrid ID="gvShipTime" runat="server" AutoGenerateColumns="false"
                                    AllowSorting="false" GroupingEnabled="false"
                                    EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1000" OnNeedDataSource="gvShipTime_NeedDataSource">
                                    <MasterTableView TableLayout="Fixed">
                                        <ColumnGroups>
                                            <telerik:GridColumnGroup HeaderText="2. Winds" Name="wind"></telerik:GridColumnGroup>
                                            <telerik:GridColumnGroup HeaderText="3. Swell" Name="swell"></telerik:GridColumnGroup>
                                            <telerik:GridColumnGroup HeaderText="7. Temp" Name="temp"></telerik:GridColumnGroup>
                                        </ColumnGroups>
                                        <Columns>
                                            <telerik:GridBoundColumn HeaderText="1. Ship's Time"></telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn HeaderText="Direction" ColumnGroupName="wind"></telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn HeaderText="Force" ColumnGroupName="wind"></telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn HeaderText="Force" ColumnGroupName="swell"></telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn HeaderText="Details" ColumnGroupName="swell"></telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn HeaderText="4. Sea State"></telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn HeaderText="5. Weather / Visiblity"></telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn HeaderText="6. Barometer Pressure"></telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn HeaderText="Wet" ColumnGroupName="temp"></telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn HeaderText="Dry" ColumnGroupName="temp"></telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn HeaderText="8. Watch Keeping Level"></telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn HeaderText="9. Name of Helsman and Lookout"></telerik:GridBoundColumn>

                                        </Columns>

                                        <NoRecordsTemplate>
                                            <table width="100%" border="0">
                                                <tr>
                                                    <td align="center">
                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </NoRecordsTemplate>
                                    </MasterTableView>
                                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                        <Virtualization EnableVirtualization="true" InitiallyCachedItemsCount="2000"
                                            LoadingPanelID="RadAjaxLoadingPanel1" ItemsPerView="100" />
                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="217px" />
                                        <Resizing AllowColumnResize="true" />
                                    </ClientSettings>
                                    <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                </telerik:RadGrid>
                            </div>
                        </div>

                        <div class="panel panel-success">
                            <div class="panel-heading">
                                19. Sounding of Tank , Hold bilges Void Spaces tanken and recorded in Deck Sounding Log - BD 1 : Yes / No* | <span class="label label-primary" style="cursor: pointer" onclick="return false;">Add</span>
                            </div>
                            <div class="panel-body">
                                <p>
                                    <telerik:RadLabel runat="server" ID="lblSoundingReason" Text="If Sounding are taken, reason for same:" CssClass="underline"></telerik:RadLabel>
                                </p>
                                <p>
                                    <telerik:RadLabel runat="server" ID="txtSoundingReason" Text="" CssClass="underline"></telerik:RadLabel>
                                </p>
                                 <p>
                                    <telerik:RadLabel runat="server" ID="lblAppropriate" Text="* Deleted as appropriate" CssClass="underline"></telerik:RadLabel>
                                </p>
                            </div>
                        </div>

                        <div class="panel panel-success">
                            <div class="panel-heading">
                                20. Master's Inspection Results : | <span class="label label-primary" style="cursor: pointer" onclick="return false;">Add</span>
                            </div>
                            <div class="panel-body">
                                <p>
                                    <telerik:RadLabel runat="server" ID="RadLabel28" CssClass="underline"></telerik:RadLabel>

                                </p>
                                <p>
                                    <telerik:RadLabel runat="server" ID="RadLabel29" CssClass="underline"></telerik:RadLabel>
                                </p>
                                <p>
                                    <telerik:RadLabel runat="server" ID="RadLabel30" CssClass="underline"></telerik:RadLabel>
                                </p>
                            </div>
                        </div>

                        <div class="panel panel-success">

                            <div class="panel-heading">
                                21. Ch. Officer's Inspection Results : | <span class="label label-primary" style="cursor: pointer" onclick="return false;">Add</span>
                            </div>

                            <div class="panel-body">
                                <p>
                                    <telerik:RadLabel runat="server" ID="lblChInspectionResult1" CssClass="underline"></telerik:RadLabel>
                                </p>
                                <p>
                                    <telerik:RadLabel runat="server" ID="lblChInspectionResult2" CssClass="underline"></telerik:RadLabel>
                                </p>
                                <p>
                                    <telerik:RadLabel runat="server" ID="lblChInspectionResult3" CssClass="underline"></telerik:RadLabel>
                                </p>
                            </div>
                        </div>

                        <div class="panel panel-success">

                            <div class="panel-heading">
                                22. IG Press : | <span class="label label-primary" style="cursor: pointer" onclick="return false;">Add</span>
                            </div>

                            <div class="panel-body">
                             <table class="bordered-table" style="display:block;height:150px;width:100%;">
                                 <tr>
                                        <th rowspan="4"> <telerik:RadLabel runat="server" ID="lbl0" Text="IG Press"  CssClass="border bold text-rotate"></telerik:RadLabel> </th>
                                        <td> <telerik:RadLabel runat="server" ID="lbl1"  CssClass="border bold" Text="00"></telerik:RadLabel> </td>
                                        <td> <telerik:RadLabel runat="server" ID="lbl2"  CssClass="border bold" ></telerik:RadLabel> </td>
                                        <td> <telerik:RadLabel runat="server" ID="lbl3"  CssClass="border bold" Text="04"></telerik:RadLabel> </td>
                                        <td> <telerik:RadLabel runat="server" ID="lbl4"  CssClass="border bold"></telerik:RadLabel> </td>
                                        <td> <telerik:RadLabel runat="server" ID="lbl5"  CssClass="border bold" Text="08"></telerik:RadLabel> </td>
                                        <td> <telerik:RadLabel runat="server" ID="lbl6"  CssClass="border bold"></telerik:RadLabel> </td>
                                        <td> <telerik:RadLabel runat="server" ID="lbl7"  CssClass="border bold" Text="12"></telerik:RadLabel> </td>
                                        <td> <telerik:RadLabel runat="server" ID="lbl8"  CssClass="border bold"></telerik:RadLabel> </td>
                                        <td> <telerik:RadLabel runat="server" ID="lbl9"  CssClass="border bold" Text="16"></telerik:RadLabel> </td>
                                        <td> <telerik:RadLabel runat="server" ID="lbl10"  CssClass="border bold"></telerik:RadLabel> </td>
                                        <td> <telerik:RadLabel runat="server" ID="lbl11"  CssClass="border bold" Text="20"></telerik:RadLabel> </td>
                                        <td> <telerik:RadLabel runat="server" ID="lbl12"  CssClass="border bold"></telerik:RadLabel> </td>
                                 </tr>
                                 <tr>
                                         <td> <telerik:RadLabel runat="server" ID="RadLabel2"  CssClass="border bold" Text="01"></telerik:RadLabel> </td>
                                        <td> <telerik:RadLabel runat="server" ID="RadLabel3"  CssClass="border bold" ></telerik:RadLabel> </td>
                                        <td> <telerik:RadLabel runat="server" ID="RadLabel4"  CssClass="border bold" Text="05"></telerik:RadLabel> </td>
                                        <td> <telerik:RadLabel runat="server" ID="RadLabel5"  CssClass="border bold"></telerik:RadLabel> </td>
                                        <td> <telerik:RadLabel runat="server" ID="RadLabel6"  CssClass="border bold" Text="09"></telerik:RadLabel> </td>
                                        <td> <telerik:RadLabel runat="server" ID="RadLabel7"  CssClass="border bold"></telerik:RadLabel> </td>
                                        <td> <telerik:RadLabel runat="server" ID="RadLabel8"  CssClass="border bold" Text="13"></telerik:RadLabel> </td>
                                        <td> <telerik:RadLabel runat="server" ID="RadLabel9"  CssClass="border bold"></telerik:RadLabel> </td>
                                        <td> <telerik:RadLabel runat="server" ID="RadLabel10"  CssClass="border bold" Text="17"></telerik:RadLabel> </td>
                                        <td> <telerik:RadLabel runat="server" ID="RadLabel11"  CssClass="border bold"></telerik:RadLabel> </td>
                                        <td> <telerik:RadLabel runat="server" ID="RadLabel31"  CssClass="border bold" Text="21"></telerik:RadLabel> </td>
                                        <td> <telerik:RadLabel runat="server" ID="RadLabel32"  CssClass="border bold"></telerik:RadLabel> </td>
                                 </tr>
                                   <tr>
                                        <td> <telerik:RadLabel runat="server" ID="RadLabel33"  CssClass="border bold" Text="02"></telerik:RadLabel> </td>
                                        <td> <telerik:RadLabel runat="server" ID="RadLabel34"  CssClass="border bold" ></telerik:RadLabel> </td>
                                        <td> <telerik:RadLabel runat="server" ID="RadLabel35"  CssClass="border bold" Text="06"></telerik:RadLabel> </td>
                                        <td> <telerik:RadLabel runat="server" ID="RadLabel36"  CssClass="border bold"></telerik:RadLabel> </td>
                                        <td> <telerik:RadLabel runat="server" ID="RadLabel37"  CssClass="border bold" Text="10"></telerik:RadLabel> </td>
                                        <td> <telerik:RadLabel runat="server" ID="RadLabel38"  CssClass="border bold"></telerik:RadLabel> </td>
                                        <td> <telerik:RadLabel runat="server" ID="RadLabel39"  CssClass="border bold" Text="14"></telerik:RadLabel> </td>
                                        <td> <telerik:RadLabel runat="server" ID="RadLabel40"  CssClass="border bold"></telerik:RadLabel> </td>
                                        <td> <telerik:RadLabel runat="server" ID="RadLabel41"  CssClass="border bold" Text="18"></telerik:RadLabel> </td>
                                        <td> <telerik:RadLabel runat="server" ID="RadLabel42"  CssClass="border bold"></telerik:RadLabel> </td>
                                        <td> <telerik:RadLabel runat="server" ID="RadLabel43"  CssClass="border bold" Text="22"></telerik:RadLabel> </td>
                                        <td> <telerik:RadLabel runat="server" ID="RadLabel44"  CssClass="border bold"></telerik:RadLabel> </td>
                                 </tr>
                                       <tr>
                                        <td> <telerik:RadLabel runat="server" ID="RadLabel45"  CssClass="border bold" Text="03"></telerik:RadLabel> </td>
                                        <td> <telerik:RadLabel runat="server" ID="RadLabel46"  CssClass="border bold" ></telerik:RadLabel> </td>
                                        <td> <telerik:RadLabel runat="server" ID="RadLabel47"  CssClass="border bold" Text="07"></telerik:RadLabel> </td>
                                        <td> <telerik:RadLabel runat="server" ID="RadLabel48"  CssClass="border bold"></telerik:RadLabel> </td>
                                        <td> <telerik:RadLabel runat="server" ID="RadLabel49"  CssClass="border bold" Text="11"></telerik:RadLabel> </td>
                                        <td> <telerik:RadLabel runat="server" ID="RadLabel50"  CssClass="border bold"></telerik:RadLabel> </td>
                                        <td> <telerik:RadLabel runat="server" ID="RadLabel51"  CssClass="border bold" Text="15"></telerik:RadLabel> </td>
                                        <td> <telerik:RadLabel runat="server" ID="RadLabel52"  CssClass="border bold"></telerik:RadLabel> </td>
                                        <td> <telerik:RadLabel runat="server" ID="RadLabel53"  CssClass="border bold" Text="19"></telerik:RadLabel> </td>
                                        <td> <telerik:RadLabel runat="server" ID="RadLabel54"  CssClass="border bold"></telerik:RadLabel> </td>
                                        <td> <telerik:RadLabel runat="server" ID="RadLabel55"  CssClass="border bold" Text="23"></telerik:RadLabel> </td>
                                        <td> <telerik:RadLabel runat="server" ID="RadLabel56"  CssClass="border bold"></telerik:RadLabel> </td>
                                 </tr>
                             </table>
                            </div>
                        </div>

                        <div class="panel panel-success">

                            <div class="panel-heading">
                                23. AM & PM Drafts : | <span class="label label-primary" style="cursor: pointer" onclick="return false;">Add</span>
                            </div>

                            <div class="panel-body">
                                <p>
                                    <telerik:RadLabel runat="server" ID="lblAMDraft" CssClass="underline" Text="AM Drafts"></telerik:RadLabel>
                                
                                    <telerik:RadLabel runat="server" ID="lblAMF" CssClass="underline" Text="F:"></telerik:RadLabel>
                                
                                    <telerik:RadLabel runat="server" ID="lblAMA" CssClass="underline" Text="A:"></telerik:RadLabel>
                               
                                    <telerik:RadLabel runat="server" ID="lblAMM" CssClass="underline" Text="M:"></telerik:RadLabel>
                               
                                    <telerik:RadLabel runat="server" ID="lblPMDraft" CssClass="underline" Text="PM Drafts"></telerik:RadLabel>
                               
                                    <telerik:RadLabel runat="server" ID="lblPMF" CssClass="underline" Text="F:"></telerik:RadLabel>
                                
                                    <telerik:RadLabel runat="server" ID="lblPMA" CssClass="underline" Text="A:"></telerik:RadLabel>
                                
                                    <telerik:RadLabel runat="server" ID="lblPMM" CssClass="underline" Text="M:"></telerik:RadLabel>
                                </p>
                            </div>
                        </div>

                        <div class="panel panel-success">

                            <div class="panel-heading">
                                10. Record of Daily Events / Operations : | <span class="label label-primary" style="cursor: pointer" onclick="return false;">Add</span>
                            </div>

                            <div class="panel-body">
                                <p>
                                    <telerik:RadLabel runat="server" ID="RadLabel1" CssClass="underline"></telerik:RadLabel>
                                </p>
                                <p>
                                    <telerik:RadLabel runat="server" ID="RadLabel16" CssClass="underline"></telerik:RadLabel>
                                </p>
                                <p>
                                    <telerik:RadLabel runat="server" ID="RadLabel17" CssClass="underline"></telerik:RadLabel>
                                </p>
                            </div>
                        </div>

                        <div class="panel panel-success">

                            <div class="panel-heading">
                                29. RADAR LOG : | <span class="label label-primary" style="cursor: pointer" onclick="return false;">Add</span>
                            </div>

                            <div class="panel-body">
                                <telerik:RadGrid ID="gvRadarLog" runat="server" AutoGenerateColumns="false"
                                    AllowSorting="false" GroupingEnabled="false"
                                    EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1000" OnNeedDataSource="gvRadarLog_NeedDataSource">
                                    <MasterTableView TableLayout="Fixed">
                                        <ColumnGroups>
                                            <telerik:GridColumnGroup HeaderText="No1. RADAR" Name="radar1"></telerik:GridColumnGroup>
                                            <telerik:GridColumnGroup HeaderText="No2. RADAR" Name="radar2"></telerik:GridColumnGroup>
                                        </ColumnGroups>
                                        <Columns>
                                            <telerik:GridBoundColumn HeaderText="WATCH"></telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn HeaderText="PM READING" ColumnGroupName="radar1"></telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn HeaderText="HOURS OF USE" ColumnGroupName="radar1"></telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn HeaderText="PM READING" ColumnGroupName="radar2"></telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn HeaderText="HOURS OF USE" ColumnGroupName="radar2"></telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn HeaderText="REMARKS (Compare with inital PM reading , replacement of magnetron etc.)" ColumnGroupName="radar2"></telerik:GridBoundColumn>


                                        </Columns>

                                        <NoRecordsTemplate>
                                            <table width="100%" border="0">
                                                <tr>
                                                    <td align="center">
                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </NoRecordsTemplate>
                                    </MasterTableView>
                                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                        <Virtualization EnableVirtualization="true" InitiallyCachedItemsCount="2000"
                                            LoadingPanelID="RadAjaxLoadingPanel1" ItemsPerView="100" />
                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="217px" />
                                        <Resizing AllowColumnResize="true" />
                                    </ClientSettings>
                                    <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                </telerik:RadGrid>
                            </div>
                        </div>

                        <div class="panel panel-success">

                            <div class="panel-heading">
                            </div>

                            <div class="panel-body">

                                <div class="col-lg-6">

                                    <div class="panel panel-info">

                                        <div class="panel-heading">
                                            26. Lookouts Dispended in accorrdance with Bridge Management Manual  | <span class="label label-primary" style="cursor: pointer" onclick="return false;">Add</span>
                                        </div>

                                        <div class="panel-body text-center" runat="server" id="Div13">
                                            <telerik:RadGrid ID="gvLookOut" runat="server" AutoGenerateColumns="false"
                                                AllowSorting="false" GroupingEnabled="false"
                                                EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1000" OnNeedDataSource="gvLookOut_NeedDataSource">
                                                <MasterTableView TableLayout="Fixed">
                                                    <Columns>
                                                        <telerik:GridBoundColumn HeaderText="From"></telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn HeaderText="To"></telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn HeaderText="Master's Sign"></telerik:GridBoundColumn>
                                                    </Columns>
                                                    <NoRecordsTemplate>
                                                        <table width="100%" border="0">
                                                            <tr>
                                                                <td align="center">
                                                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </NoRecordsTemplate>
                                                </MasterTableView>
                                                <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                    <Virtualization EnableVirtualization="true" InitiallyCachedItemsCount="2000"
                                                        LoadingPanelID="RadAjaxLoadingPanel1" ItemsPerView="100" />
                                                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="217px" />
                                                    <Resizing AllowColumnResize="true" />
                                                </ClientSettings>
                                                <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                            </telerik:RadGrid>

                                        </div>
                                        <br />


                                    </div>

                                </div>

                                <div class="col-lg-6">

                                    <div class="panel panel-info">

                                        <div class="panel-heading">
                                            28. Miscellaneous/ Adjustments of Ship's Clock | <span class="label label-primary" style="cursor: pointer" onclick="return false;">Add</span>
                                        </div>

                                        <div class="panel-body text-center" runat="server" id="Div14">
                                            <p style="height: 230px;">
                                                <telerik:RadLabel runat="server" ID="RadLabel18" CssClass="underline"></telerik:RadLabel>
                                            </p>
                                        </div>

                                        <br />


                                    </div>

                                </div>

                            </div>
                        </div>

                        <div class="panel panel-success">

                            <div class="panel-heading">
                            </div>

                            <div class="panel-body">

                                <div class="col-lg-6">

                                    <div class="panel panel-info">

                                        <div class="panel-heading">
                                            25. Fire/ Safety Patrols  | <span class="label label-primary" style="cursor: pointer" onclick="return false;">Add</span>
                                        </div>

                                        <div class="panel-body text-center" runat="server" id="Div15">
                                            <telerik:RadGrid ID="gvFireSafety" runat="server" AutoGenerateColumns="false"
                                                AllowSorting="false" GroupingEnabled="false"
                                                EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1000" OnNeedDataSource="gvFireSafety_NeedDataSource">
                                                <MasterTableView TableLayout="Fixed">
                                                    <Columns>
                                                        <telerik:GridBoundColumn HeaderText="Time"></telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn HeaderText="Rank"></telerik:GridBoundColumn>
                                                    </Columns>
                                                    <NoRecordsTemplate>
                                                        <table width="100%" border="0">
                                                            <tr>
                                                                <td align="center">
                                                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </NoRecordsTemplate>
                                                </MasterTableView>
                                                <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                    <Virtualization EnableVirtualization="true" InitiallyCachedItemsCount="2000"
                                                        LoadingPanelID="RadAjaxLoadingPanel1" ItemsPerView="100" />
                                                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="217px" />
                                                    <Resizing AllowColumnResize="true" />
                                                </ClientSettings>
                                                <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                            </telerik:RadGrid>
                                        </div>
                                        <br />
                                    </div>
                                </div>

                                <div class="col-lg-6">
                                    <div class="panel panel-info">
                                        <div class="panel-heading">
                                            27. Details/ Checks of Loadline Items, Anchor Lashings Etc.  | <span class="label label-primary" style="cursor: pointer" onclick="return false;">Add</span>
                                        </div>

                                        <div class="panel-body text-center" runat="server" id="Div16">
                                            <p style="height: 230px;">
                                                <telerik:RadLabel runat="server" ID="RadLabel19" CssClass="underline"></telerik:RadLabel>
                                            </p>
                                        </div>

                                        <br />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="panel panel-success">

                        <div class="panel-heading">
                            <%--<a class="text-primary" style="color: white" href="#">26. Lookouts Dispended in accorrdance with Bridge Management Manual</a>--%>
                        </div>

                        <div class="panel-body">

                            <div class="col-lg-6">

                                <div class="panel panel-info">

                                    <div class="panel-heading">
                                         24. ISPS Requirements :  | <span class="label label-primary" style="cursor: pointer" onclick="return false;">Add</span>
                                    </div>

                                    <div class="panel-body text-center" runat="server" id="Div17">
                                            <p>
                                                 <h4>Security Level of Vessel: 
                                                     <telerik:RadLabel ID="RadLabel20" runat="server" CssClass="underline"></telerik:RadLabel>
                                                 </h4>
                                             </p>
                                             <hr />
                                             <p>
                                                 <h4>Security Level of Port:
                                                  <telerik:RadLabel ID="RadLabel21" runat="server" CssClass="underline"></telerik:RadLabel>
                                                 </h4>
                                             </p>
                                             <hr />
                                             <p>
                                                 <h4>Security Patrol carried outal (Port):
                                                  <telerik:RadLabel ID="RadLabel22" runat="server" CssClass="underline"></telerik:RadLabel>
                                                 </h4>
                                             </p>
                                             <hr />
                                             <p>
                                                 <h4>Frequency:
                                                  <telerik:RadLabel ID="RadLabel23" runat="server" CssClass="underline"></telerik:RadLabel>
                                                 </h4>
                                             </p>
                                    </div>
                                    <br />
                                </div>
                            </div>

                            <div class="col-lg-6">

                                <div class="panel panel-info">

                                    <div class="panel-heading">
                                         Stoway / Drug/ Contraband Search Carried Out: Yes / No* | <span class="label label-primary" style="cursor: pointer" onclick="return false;">Add</span>
                                    </div>

                                    <div class="panel-body text-center" runat="server" id="Div18">
                                        <p>
                                            <h4>Time: From 
                                                   <telerik:RadLabel ID="RadLabel24" runat="server" CssClass="underline"></telerik:RadLabel>
                                                <span>To:</span>
                                                <telerik:RadLabel ID="RadLabel25" runat="server" CssClass="underline"></telerik:RadLabel>
                                            </h4>
                                        </p>
                                        <hr />
                                        <p>
                                            <h4>Result: 
                                               <telerik:RadLabel ID="RadLabel26" runat="server" CssClass="underline"></telerik:RadLabel>
                                            </h4>
                                        </p>
                                        <hr />
                                        <p>
                                            <h4>Any Additional Security Information
                                                <telerik:RadLabel ID="RadLabel27" runat="server" CssClass="underline"></telerik:RadLabel>
                                            </h4>
                                        </p>
                                    </div>

                                    <br />
                                </div>
                            </div>
                        </div>
                    </div>
                     </div>
                </div>


                <div class="col-lg-4">
                    <br />

                

                    <div class="panel panel-success">

                        <div class="panel-heading">
                            Noon Reports
                        </div>

                        <div class="panel-body">
                            <div class="col-lg-12">

                                <div class="panel panel-info">

                                    <div class="panel-heading">
                                       Noon Posistion | <span class="label label-primary" style="cursor: pointer" onclick="return false;">Add</span>
                                    </div>

                                    <div class="panel-body text-center" runat="server" id="Div6">
                                            <telerik:RadGrid ID="gvNoonPos" runat="server" AutoGenerateColumns="false"
                                    AllowSorting="false" GroupingEnabled="false"
                                    EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1000" OnNeedDataSource="gvNoonPos_NeedDataSource">
                                    <MasterTableView TableLayout="Fixed">
                                        <ColumnGroups>
                                            <telerik:GridColumnGroup HeaderText="Noon Posistion (Obs/GPS/Celestial)" Name="noonpos"></telerik:GridColumnGroup>

                                            <telerik:GridColumnGroup HeaderText="11. Latitude" Name="lat" ParentGroupName="noonpos"></telerik:GridColumnGroup>
                                            <telerik:GridColumnGroup HeaderText="12. Longitude" Name="lon" ParentGroupName="noonpos"></telerik:GridColumnGroup>
                                         
                                        </ColumnGroups>
                                        <Columns>
                                            <telerik:GridBoundColumn HeaderText="Deg" ColumnGroupName="lat"></telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn HeaderText="Min" ColumnGroupName="lat"></telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn HeaderText="Deg" ColumnGroupName="lon"></telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn HeaderText="Min" ColumnGroupName="lon"></telerik:GridBoundColumn>
                                        </Columns>
                                        <NoRecordsTemplate>
                                            <table width="100%" border="0">
                                                <tr>
                                                    <td align="center">
                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </NoRecordsTemplate>
                                    </MasterTableView>
                                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                        <Virtualization EnableVirtualization="true" InitiallyCachedItemsCount="2000"
                                            LoadingPanelID="RadAjaxLoadingPanel1" ItemsPerView="100" />
                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="217px" />
                                        <Resizing AllowColumnResize="true" />
                                    </ClientSettings>
                                    <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                </telerik:RadGrid>
                                    </div>
                                    <br />
                                </div>

                                <div class="panel panel-info">

                                    <div class="panel-heading">
                                        13. Distance (nautical miles)  | <span class="label label-primary" style="cursor: pointer" onclick="return false;">Add</span>
                                    </div>

                                    <div class="panel-body text-center" runat="server" id="Div7">
                                         <p>
                                              <h4>Distance Travelled: 
                                                  <telerik:RadLabel ID="lblDistanceTavelled" runat="server" CssClass="underline"></telerik:RadLabel>
                                              </h4>
                                          </p>
                                          <hr />
                                          <p>
                                              <h4>Total Voyage distance travelled:
                                               <telerik:RadLabel ID="RadLabel12" runat="server" CssClass="underline"></telerik:RadLabel>
                                              </h4>
                                          </p>
                                          <hr />
                                          <p>
                                              <h4>Distance to go:
                                               <telerik:RadLabel ID="RadLabel13" runat="server" CssClass="underline"></telerik:RadLabel>
                                              </h4>
                                          </p>
                                          <hr />
                                    </div>
                                    <br />
                                </div>

                                <div class="panel panel-info">
                                    <div class="panel-heading">
                                        14. Steaming Time  | <span class="label label-primary" style="cursor: pointer" onclick="return false;">Add</span>
                                    </div>

                                    <div class="panel-body text-center" runat="server" id="Div8">
                                        <p>
                                            <h4 class="text-center">(HH/MM) 
                                            </h4>
                                        </p>
                                        <hr />
                                        <p>
                                            <h4>Total Voyage distance travelled:
                                               <telerik:RadLabel ID="RadLabel14" runat="server" CssClass="underline"></telerik:RadLabel>
                                            </h4>
                                        </p>
                                        <hr />
                                        <p>
                                            <h4>Distance to go:
                                               <telerik:RadLabel ID="RadLabel15" runat="server" CssClass="underline"></telerik:RadLabel>
                                            </h4>
                                        </p>
                                        <hr />
                                    </div>
                                    <br />
                                </div>

                                <div class="panel panel-info">
                                    <div class="panel-heading">
                                        15. Speed (knots)  | <span class="label label-primary" style="cursor: pointer" onclick="return false;">Add</span>
                                    </div>

                                    <div class="panel-body text-center" runat="server" id="Div9">
                                        <p>
                                             <h4>Average Speed of the day - :
                                               <telerik:RadLabel ID="lblAvgSpeed" runat="server" CssClass="underline"></telerik:RadLabel>
                                            </h4>
                                        </p>
                                        <hr />
                                        <p>
                                            <h4>General Average -
                                               <telerik:RadLabel ID="lblGeneralAverage" runat="server" CssClass="underline"></telerik:RadLabel>
                                            </h4>
                                        </p>
                                        <hr />
                                        <p>
                                            <h4>Average RPM:
                                               <telerik:RadLabel ID="lblAvergaeRPM" runat="server" CssClass="underline"></telerik:RadLabel>
                                            </h4>
                                        </p>
                                        <hr />
                                    </div>
                                    <br />
                                </div>

                                <div class="panel panel-info">
                                    <div class="panel-heading">
                                        16. FW ROB  | <span class="label label-primary" style="cursor: pointer" onclick="return false;">Add</span>
                                    </div>

                                    <div class="panel-body text-center" runat="server" id="Div10">
                                        <p>
                                             <h4>Fresh Water - :
                                               <telerik:RadLabel ID="lblFreshWater" runat="server" CssClass="underline"></telerik:RadLabel>
                                            </h4>
                                        </p>
                                        <hr />
                                        <p>
                                            <h4>Drinking Water -
                                               <telerik:RadLabel ID="lblDrinkingWater" runat="server" CssClass="underline"></telerik:RadLabel>
                                            </h4>
                                        </p>
                                        <hr />
                                        <p>
                                            <h4>TK Clng Water:
                                               <telerik:RadLabel ID="lblClngWater" runat="server" CssClass="underline"></telerik:RadLabel>
                                            </h4>
                                        </p>
                                        <hr />
                                    </div>
                                    <br />
                                </div>

                                <div class="panel panel-info">
                                    <div class="panel-heading">
                                        17. Fuel ROB (MENTION EACH GRADE SEPARATELY)  | <span class="label label-primary" style="cursor: pointer" onclick="return false;">Add</span>
                                    </div>

                                    <div class="panel-body text-center" runat="server" id="Div11">
                                              <telerik:RadGrid ID="gvFuelROB" runat="server" AutoGenerateColumns="false"
                                    AllowSorting="false" GroupingEnabled="false"
                                    EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1000" OnNeedDataSource="gvFuelROB_NeedDataSource">
                                    <MasterTableView TableLayout="Fixed">
                                        <ColumnGroups>
                                            <telerik:GridColumnGroup HeaderText="Fuel ROB" Name="fuelrob"></telerik:GridColumnGroup>
                                        </ColumnGroups>
                                        <Columns>
                                            <telerik:GridBoundColumn  ColumnGroupName="fuelrob"></telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn  ColumnGroupName="fuelrob"></telerik:GridBoundColumn>
                                        </Columns>
                                        <NoRecordsTemplate>
                                            <table width="100%" border="0">
                                                <tr>
                                                    <td align="center">
                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </NoRecordsTemplate>
                                    </MasterTableView>
                                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                        <Virtualization EnableVirtualization="true" InitiallyCachedItemsCount="2000"
                                            LoadingPanelID="RadAjaxLoadingPanel1" ItemsPerView="100" />
                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="217px" />
                                        <Resizing AllowColumnResize="true" />
                                    </ClientSettings>
                                    <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                </telerik:RadGrid>
                                    </div>  
                                    <br />
                                </div>

                                <div class="panel panel-info">
                                    <div class="panel-heading">
                                        18. RUNNING HOURS OF RADAR  | <span class="label label-primary" style="cursor: pointer" onclick="return false;">Add</span>
                                    </div>

                                    <div class="panel-body text-center" runat="server" id="Div12">
                                        <p>
                                             <h4>No.1 RADAR :
                                               <telerik:RadLabel ID="lblRadar1" runat="server" CssClass="underline"></telerik:RadLabel>
                                            </h4>
                                        </p>
                                        <hr />
                                        <p>
                                            <h4>No.2 RADAR -
                                               <telerik:RadLabel ID="lblRadar2" runat="server" CssClass="underline"></telerik:RadLabel>
                                            </h4>
                                        </p>
                                        <hr />
                                    </div>
                                    <br />
                                </div>

                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script type="text/javascript" src="../Scripts/jquery-3.4.1.min.js"></script>
    <script type="text/javascript" src="../Scripts/bootstrap.min.v3.js"></script>
    <script type="text/javascript" src="../Scripts/dashboard.js"></script>
</body>
</html>

