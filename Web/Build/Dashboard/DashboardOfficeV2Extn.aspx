<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardOfficeV2Extn.aspx.cs" Inherits="Dashboard_DashboardOfficeV2Extn" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <telerik:RadCodeBlock ID="radCodeBlock1" runat="server">
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <link href="../Content/bootstrap.min.v3.css" rel="stylesheet" />
        <link href="../Content/fontawesome-all.min.css" rel="stylesheet" />
        <link href="../Content/dashboard.css" rel="stylesheet" />
        <script type="text/javascript">
            function resize() {

            }
            window.onload = window.onresize = resize;
            function pageLoad() {
                resize();
            }
        </script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <div class="gray-bg">

            <div class="row">

                <div class="col-lg-12">

                    <div class="panel-body">

                        <div class="col-lg-12">

                            <div class="col-lg-3">
                                <div class="panel panel-success">

                                    <div class="panel-heading">
                                        Drills
                                    </div>

                                    <div class="panel-body" style="height: 236px">

                                        <telerik:RadGrid ID="gvDrills" runat="server" AutoGenerateColumns="false" BorderStyle="None"
                                            AllowSorting="false" GroupingEnabled="false" ShowHeader="true" ShowFooter="false" Height="215px"
                                            EnableHeaderContextMenu="true" OnNeedDataSource="gvDrills_NeedDataSource"
                                            OnItemDataBound="gvDrills_ItemDataBound">
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
                                                    <telerik:GridTemplateColumn HeaderText="Mandatory Drills" HeaderStyle-Width="68%">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lbldrillname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDRILLNAME") %>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Overdue" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="32%">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="Drilloverdueanchor" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVERDUE") %>'></asp:LinkButton>

                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                </Columns>

                                            </MasterTableView>
                                            <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                                                <Resizing AllowColumnResize="true" />
                                            </ClientSettings>
                                        </telerik:RadGrid>

                                    </div>
                                </div>

                                <div class="panel panel-success">

                                    <div class="panel-heading">
                                        <a class="text-primary" style="color: white; text-decoration: underline" href="javascript: top.openNewWindow('dpms','Work & Rest Hours','Dashboard/DashboardOfficeV2TechnicalPMS.aspx?mod=WRHV2')">Work & Rest Hours</a>
                                    </div>

                                    <div class="panel-body" style="height: 270px">
                                        <telerik:RadGrid ID="gvWRH" runat="server" AutoGenerateColumns="false" OnNeedDataSource="gvWRH_NeedDataSource"
                                            AllowSorting="false" GroupingEnabled="false" BorderStyle="None" OnItemDataBound="gvWRH_ItemDataBound"
                                            EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000">
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
                                                    <telerik:GridTemplateColumn HeaderText="Stage Pending" HeaderStyle-Width="45%">
                                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblmeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURENAME") %>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Seafarer" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkSeafarerCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAFARERCOUNT") %>'></asp:LinkButton>
                                                            <telerik:RadLabel ID="lnkSeafarerUrl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAFARERURL") %>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="HOD" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkHODCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHODCOUNT") %>'></asp:LinkButton>
                                                            <telerik:RadLabel ID="lnkHODUrl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHODURL") %>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Master" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lblMasterCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMASTERCOUNT") %>'></asp:LinkButton>
                                                            <telerik:RadLabel ID="lblMasterUrl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMASTERURL") %>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                            </MasterTableView>
                                            <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                                                <Resizing AllowColumnResize="true" />
                                            </ClientSettings>
                                        </telerik:RadGrid>
                                    </div>
                                </div>

                                <div class="panel panel-success">

                                    <div class="panel-heading">
                                        <a class="text-primary" style="color: white; text-decoration: underline" href="javascript: top.openNewWindow('dpms','Open Reports','Dashboard/DashboardOfficeV2HSEQA.aspx?mod=OPENREPORT',false)">Open Reports</a>
                                    </div>

                                    <div class="panel-body" style="height: 155px">
                                        <telerik:RadGrid ID="gvOpenReports" runat="server" AutoGenerateColumns="false" BorderStyle="None"
                                            AllowSorting="false" GroupingEnabled="false" ShowHeader="false" ShowFooter="false"
                                            EnableHeaderContextMenu="true" OnNeedDataSource="gvOpenReports_NeedDataSource"
                                            OnItemDataBound="gvOpenReports_ItemDataBound">
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
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkcount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNT") %>'></asp:LinkButton>
                                                            <telerik:RadLabel ID="lblurl" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDURL") %>' Visible="false"></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </div>

                                </div>

                            </div>

                            <div class="col-lg-3">


                                <div class="panel panel-success" id="divTechMOC" runat="server">

                                    <div class="panel-heading">
                                        <a class="text-primary" style="color: white; text-decoration: underline" href="javascript: top.openNewWindow('dpms','MOCs','Dashboard/DashboardOfficeV2HSEQA.aspx?mod=MOC',false)">MOCs</a>
                                    </div>

                                    <div class="panel-body" style="height: 560px">
                                        <telerik:RadGrid ID="gvMOC" runat="server" AutoGenerateColumns="false"
                                            AllowSorting="false" GroupingEnabled="false" Height="480px" BorderStyle="None"
                                            EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000" OnNeedDataSource="gvMOC_NeedDataSource"
                                            OnItemDataBound="gvMOC_ItemDataBound">
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
                                                    <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="68%">
                                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblmeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Ship" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkShip" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHIPCOUNT") %>'></asp:LinkButton>
                                                            <telerik:RadLabel ID="lblShip" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHIPURL") %>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Office" HeaderStyle-Width="17%" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkOffice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFICECOUNT") %>'></asp:LinkButton>
                                                            <telerik:RadLabel ID="lblOffice" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFICEURL") %>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                            </MasterTableView>
                                            <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                                                <Resizing AllowColumnResize="true" />
                                            </ClientSettings>
                                        </telerik:RadGrid>
                                    </div>
                                </div>
                                <div class="panel panel-success" id="divTraining" runat="server">

                                    <div class="panel-heading">
                                        Trainings                                                                                                     
                                    </div>

                                    <div class="panel-body" style="height: 560px">
                                        <telerik:RadGrid ID="gvQMSTraining" runat="server" AutoGenerateColumns="false" BorderStyle="None"
                                            AllowSorting="false" GroupingEnabled="false" ShowHeader="true" ShowFooter="false" Height="480px"
                                            EnableHeaderContextMenu="true" OnNeedDataSource="gvQMSTraining_NeedDataSource"
                                            OnItemDataBound="gvQMSTraining_ItemDataBound">
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
                                                    <telerik:GridTemplateColumn HeaderText="Mandatory Trainings" HeaderStyle-Width="68%">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lbltrainingname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAININGNAME") %>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Overdue" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="32%">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="trainingoverdueanchor" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVERDUE") %>'></asp:LinkButton>

                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                </Columns>

                                            </MasterTableView>
                                            <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                                                <Resizing AllowColumnResize="true" />
                                            </ClientSettings>
                                        </telerik:RadGrid>
                                    </div>
                                </div>
                                <div class="panel panel-success">

                                    <div class="panel-heading">
                                        <a class="text-primary" style="color: white; text-decoration: underline" href="javascript: top.openNewWindow('dpms','Crew Complaints','Dashboard/DashboardOfficeV2HSEQA.aspx?mod=CREWCMPLTS',false)">Crew Complaints</a>
                                    </div>

                                    <div class="panel-body" style="height: 155px">
                                        <telerik:RadGrid ID="gvCrewComplaints" runat="server" AutoGenerateColumns="false" BorderStyle="None"
                                            AllowSorting="false" GroupingEnabled="false" ShowHeader="false" ShowFooter="false"
                                            EnableHeaderContextMenu="true" OnNeedDataSource="gvCrewComplaints_NeedDataSource"
                                            OnItemDataBound="gvCrewComplaints_ItemDataBound">
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
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkcount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNT") %>'></asp:LinkButton>
                                                            <telerik:RadLabel ID="lblurl" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDURL") %>' Visible="false"></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </div>

                                </div>

                            </div>

                            <div class="col-lg-3">

                                <div class="panel panel-success">

                                    <div class="panel-heading">
                                        <a class="text-primary" style="color: white; text-decoration: underline" href="javascript: top.openNewWindow('dpms','Accidents and Near Misses','Dashboard/DashboardOfficeV2HSEQA.aspx?mod=INCIDENT',false)">Accidents and Near Misses</a>
                                    </div>

                                    <div class="panel-body" style="height: 236px">
                                        <telerik:RadGrid ID="gvIncident" runat="server" AutoGenerateColumns="false" BorderStyle="None"
                                            AllowSorting="false" GroupingEnabled="false" ShowHeader="false" ShowFooter="false"
                                            EnableHeaderContextMenu="true" OnNeedDataSource="gvIncident_NeedDataSource"
                                            OnItemDataBound="gvIncident_ItemDataBound">
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
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkcount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNT") %>'></asp:LinkButton>
                                                            <telerik:RadLabel ID="lblurl" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDURL") %>' Visible="false"></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </div>
                                </div>


                                <div class="panel panel-success">

                                    <div class="panel-heading">
                                        <a class="text-primary" style="color: white; text-decoration: underline" href="javascript: top.openNewWindow('dpms','Machinery Damage / Failure','Dashboard/DashboardOfficeV2HSEQA.aspx?mod=MACDAMAGE',false)">Machinery Damage / Failure</a>
                                    </div>

                                    <div class="panel-body" style="height: 270px">
                                        <telerik:RadGrid ID="gvMachineryDamage" runat="server" AutoGenerateColumns="false" BorderStyle="None"
                                            AllowSorting="false" GroupingEnabled="false" ShowHeader="false" ShowFooter="false"
                                            EnableHeaderContextMenu="true" OnNeedDataSource="gvMachineryDamage_NeedDataSource"
                                            OnItemDataBound="gvMachineryDamage_ItemDataBound">
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
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkcount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNT") %>'></asp:LinkButton>
                                                            <telerik:RadLabel ID="lblurl" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDURL") %>' Visible="false"></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </div>
                                </div>

                                <div class="panel panel-success">
                                    <div class="panel-heading">
                                        <a class="text-primary" style="color: white; text-decoration: underline" href="javascript: top.openNewWindow('dpms','UC / UACTS','Dashboard/DashboardOfficeV2HSEQA.aspx?mod=UNSAFEACTS',false)">UC / UACTS</a>
                                    </div>
                                    <div class="panel-body" style="height: 155px">
                                        <telerik:RadGrid ID="gvUnSafeAct" runat="server" AutoGenerateColumns="false" BorderStyle="None"
                                            AllowSorting="false" GroupingEnabled="false" ShowHeader="false" ShowFooter="false"
                                            EnableHeaderContextMenu="true" OnNeedDataSource="gvUnSafeAct_NeedDataSource"
                                            OnItemDataBound="gvUnSafeAct_ItemDataBound">
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
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkcount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNT") %>'></asp:LinkButton>
                                                            <telerik:RadLabel ID="lblurl" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDURL") %>' Visible="false"></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-3">

                                <div class="panel panel-success" id="divauditdeficiency" runat="server">

                                    <div class="panel-heading">
                                        <a runat="server" class="text-primary" style="color: white; text-decoration: underline" href="javascript: top.openNewWindow('dpms','Deficiencies','Dashboard/DashboardOfficeV2HSEQA.aspx?mod=INSDEF',false)">Deficiencies</a>
                                    </div>

                                    <div class="panel-body" id="divDeficiency" runat="server" style="height: 232px">
                                        <telerik:RadGrid ID="gvDeficiencyStatus" runat="server" AutoGenerateColumns="false"
                                            AllowSorting="false" GroupingEnabled="false" Height="215px" BorderStyle="None"
                                            EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000" OnNeedDataSource="gvDeficiencyStatus_NeedDataSource"
                                            OnItemDataBound="gvDeficiencyStatus_ItemDataBound">
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
                                                    <telerik:GridTemplateColumn HeaderText="Deficiency" HeaderStyle-Width="58%">
                                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblmeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Ship" HeaderStyle-Width="21%" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkShip" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHIPCOUNT") %>'></asp:LinkButton>
                                                            <telerik:RadLabel ID="lblShip" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHIPURL") %>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Office" HeaderStyle-Width="21%" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkOffice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFICECOUNT") %>'></asp:LinkButton>
                                                            <telerik:RadLabel ID="lblOffice" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFICEURL") %>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                            </MasterTableView>
                                            <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                                                <Resizing AllowColumnResize="true" />
                                            </ClientSettings>
                                        </telerik:RadGrid>
                                    </div>
                                </div>
                                <div class="panel panel-success" id="divQMSMOC" runat="server">

                                    <div class="panel-heading">
                                        <a class="text-primary" style="color: white; text-decoration: underline" href="javascript: top.openNewWindow('dpms','MOCs','Dashboard/DashboardOfficeV2HSEQA.aspx?mod=MOC',false)">MOCs</a>
                                    </div>
                                    <div class="panel-body" style="height: 560PX">
                                        <telerik:RadGrid ID="gvQMSMOC" runat="server" AutoGenerateColumns="false"
                                            AllowSorting="false" GroupingEnabled="false" Height="480px" BorderStyle="None"
                                            EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000" OnNeedDataSource="gvQMSMOC_NeedDataSource"
                                            OnItemDataBound="gvQMSMOC_ItemDataBound">
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
                                                    <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="68%">
                                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblmeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Ship" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkShip" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHIPCOUNT") %>'></asp:LinkButton>
                                                            <telerik:RadLabel ID="lblShip" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHIPURL") %>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Office" HeaderStyle-Width="17%" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkOffice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFICECOUNT") %>'></asp:LinkButton>
                                                            <telerik:RadLabel ID="lblOffice" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFICEURL") %>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                            </MasterTableView>
                                            <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                                                <Resizing AllowColumnResize="true" />
                                            </ClientSettings>
                                        </telerik:RadGrid>
                                    </div>
                                </div>
                                <div class="panel panel-success" id="divProposal" runat="server">
                                    <div class="panel-heading">
                                        Proposal & Approval
                                    </div>
                                    <div class="panel-body" style="height: 270px">
                                        <telerik:RadGrid ID="gvProposal" runat="server" AutoGenerateColumns="false" BorderStyle="None"
                                            AllowSorting="false" GroupingEnabled="false" ShowHeader="false" ShowFooter="false"
                                            EnableHeaderContextMenu="true" OnNeedDataSource="gvProposal_NeedDataSource"
                                            OnItemDataBound="gvProposal_ItemDataBound">
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
                                                    <telerik:GridTemplateColumn HeaderStyle-Width="80%">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderStyle-Width="20%">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkcount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNT") %>'></asp:LinkButton>
                                                            <telerik:RadLabel ID="lblurl" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDURL") %>' Visible="false"></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </div>
                                </div>
                                <div class="panel panel-success">
                                    <div class="panel-heading">
                                        <a class="text-primary" style="color: white; text-decoration: underline" href="javascript: top.openNewWindow('dpms','Non Routine Risk Assessments','Dashboard/DashboardOfficeV2HSEQA.aspx?mod=NRRA',false)">Non Routine Risk Assessments</a>
                                    </div>
                                    <div class="panel-body" style="height: 155px">
                                        <telerik:RadGrid ID="gvNonRoutineRA" runat="server" AutoGenerateColumns="false" BorderStyle="None"
                                            AllowSorting="false" GroupingEnabled="false" ShowHeader="true" ShowFooter="false"
                                            EnableHeaderContextMenu="true" OnNeedDataSource="gvNonRoutineRA_NeedDataSource"
                                            OnItemDataBound="gvNonRoutineRA_ItemDataBound">
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
                                                    <telerik:GridTemplateColumn HeaderText="Old" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkOld" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOLDCOUNT") %>'></asp:LinkButton>
                                                            <telerik:RadLabel ID="lblOld" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOLDURL") %>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="New" HeaderStyle-Width="17%" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkNew" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEWCOUNT") %>'></asp:LinkButton>
                                                            <telerik:RadLabel ID="lblNew" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEWURL") %>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </div>
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
</body>
</html>
