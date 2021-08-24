<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreExperienceDetailsFrame.aspx.cs" Inherits="CrewOffshore_CrewOffshoreExperienceDetailsFrame" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <link href="../Content/bootstrap.min.v3.css" rel="stylesheet" />
        <link href="../Content/fontawesome-all.min.css" rel="stylesheet" />
        <link href="../Content/dashboard.css" rel="stylesheet" />

    </telerik:RadCodeBlock>
</head>
<body style="background-color:white">
    <form id="form1" runat="server">
        <div>
            <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
            <telerik:RadSkinManager ID="RadSkinManager1" runat="server"></telerik:RadSkinManager>
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%" Font-Size="Smaller">
                <div>
                    <telerik:RadGrid ID="gvCrewExp" runat="server" AutoGenerateColumns="false"
                        AllowSorting="false" GroupingEnabled="true" BorderStyle="None"
                        EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000" OnNeedDataSource="gvCrewExp_NeedDataSource"
                        >
                        <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                            AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="true" GroupLoadMode="Client"
                            GroupHeaderItemStyle-CssClass="center">
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
                                <telerik:GridTemplateColumn HeaderText="Vessel" HeaderStyle-Width="120px" AllowSorting="false" ShowSortIcon="true">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Vessel Type" HeaderStyle-Width="100px" AllowSorting="false" ShowSortIcon="true">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblVesselType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELTYPENAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Rank" HeaderStyle-Width="50px" AllowSorting="false" ShowSortIcon="true">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lnkRank" CommandName="EDIT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblsignonoff" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNONOFFID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblCrewCompanyExperienceId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYEXPERIENCEID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="DWT" HeaderStyle-Width="50px" AllowSorting="false" ShowSortIcon="true">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDwtNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELDWT") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>                            
                                <telerik:GridTemplateColumn HeaderText="Engine Type / Model" HeaderStyle-Width="70px" AllowSorting="false" ShowSortIcon="true">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblEngine" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENGINETYPEMODEL") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="FromDate" HeaderText="From Date" HeaderStyle-Width="75px" AllowSorting="True" DataField="FLDFROMDATE" ShowSortIcon="true">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblFromDate" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDFROMDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="ToDate" HeaderText="To Date" HeaderStyle-Width="75px" AllowSorting="True" DataField="FLDTODATE" ShowSortIcon="true">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lbToDate" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDTODATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                 <telerik:GridTemplateColumn HeaderText="Wages" UniqueName="LASTSALARY" HeaderStyle-Width="50px" AllowSorting="false" ShowSortIcon="true">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblLastSalary" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDAILYRATEUSD") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Duration" HeaderStyle-Width="75px" AllowSorting="false" ShowSortIcon="true">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDuration" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDURATION") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>                              
                                <telerik:GridTemplateColumn HeaderText="Employer" HeaderStyle-Width="150px" AllowSorting="false" ShowSortIcon="true">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblManningCompanyName" runat="server" ClientIDMode="AutoID" CssClass="tooltip" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMANNINGCOMPANY").ToString().Length>15 ? DataBinder.Eval(Container, "DataItem.FLDMANNINGCOMPANY").ToString().Substring(0, 15) + "..." : DataBinder.Eval(Container, "DataItem.FLDMANNINGCOMPANY").ToString() %>'></telerik:RadLabel>
                                        <eluc:tooltip id="ucManningCompanyTT" runat="server" text='<%# DataBinder.Eval(Container,"DataItem.FLDMANNINGCOMPANY") %>' />
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
            </telerik:RadAjaxPanel>
        </div>
    </form>
</body>
</html>
