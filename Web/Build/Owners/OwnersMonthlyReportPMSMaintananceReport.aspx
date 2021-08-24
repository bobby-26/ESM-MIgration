<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnersMonthlyReportPMSMaintananceReport.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="OwnersMonthlyReportPMSMaintananceReport" %>

<!DOCTYPE html>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <telerik:RadCodeBlock ID="radCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%--<%: Styles.Render("~/bundles/css") %>--%>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <link href="../Content/bootstrap.min.v3.css" rel="stylesheet" />
        <link href="../Content/OwnersReport.css" rel="stylesheet" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <script type="text/javascript">
            //Dummy function to ignore javascript page error
            function OnHeaderMenuShowing(sender, args) {
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

        .RadGrid .rgHeader, .RadGrid th.rgResizeCol, .RadGrid .rgRow td, .RadGrid .rgAltRow td {
            padding-left: 2px !important;
            padding-right: 2px !important;
        }

        .RadGrid_Windows7 .rgHeader {
            color: black !important;
        }

        .higherZIndex {
            z-index: 2;
        }

        .rdTitleWrapper, .rdTitleBar {
            background-color: rgb(194, 220, 252) !important;
            background-image: linear-gradient(rgb(244, 248, 250), rgb(233, 242, 251) 50%, rgb(221, 231, 245) 50%, rgb(228, 237, 248)) !important;
            color: black !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <table style="width: 100%" runat="server">
            <tr>
                <td style="width: 100%;">
                    <telerik:RadDockZone ID="rdMaintenance" runat="server" Orientation="Vertical" MinHeight="300px" Docks="RadDock1,RadDock2" Height="100%" FitDocks="true">
                        <telerik:RadDock ID="RadDock1" RenderMode="Lightweight" runat="server"
                            EnableAnimation="true" EnableDrag="true" DockMode="Docked" DockHandle="TitleBar"
                            EnableRoundedCorners="true" Resizable="false" CssClass="higherZIndex">
                            <TitlebarTemplate>
                                <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                    <ContentTemplate>
                                        <table width="20%">
                                            <tr>
                                                <td>
                                                    <asp:LinkButton
                                                        ID="lnkMA"
                                                        runat="server"
                                                        Style="text-decoration: underline; color: black;"
                                                        Text="Maintenance Form">
                                                    </asp:LinkButton>
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="lnkMAComments" runat="server" ToolTip="Comments">
                                                   <span class="icon"><i class="fa fa-comments-blue"></i></span>
                                                    </asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>

                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </TitlebarTemplate>
                            <Commands>
                                <telerik:DockExpandCollapseCommand />
                            </Commands>
                            <ContentTemplate>
                                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                                <telerik:RadGrid RenderMode="Lightweight" ID="gvMaintenance" runat="server" GridLines="None" OnNeedDataSource="gvMaintenance_NeedDataSource"
                                    Height="275px" AutoGenerateColumns="false" Width="100%" OnItemDataBound="gvMaintenance_ItemDataBound" OnItemCommand="gvMaintenance_ItemCommand"
                                    GroupingEnabled="false" EnableHeaderContextMenu="true" AllowPaging="false"
                                    ShowFooter="false" ShowHeader="true" EnableViewState="true" AllowCustomPaging="false">
                                    <MasterTableView TableLayout="Fixed">
                                        <NoRecordsTemplate>
                                            <table width="100%" border="0">
                                                <tr>
                                                    <td align="center">
                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </NoRecordsTemplate>
                                        <CommandItemSettings RefreshText="Search" ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderText="Form No." HeaderStyle-Width="5%">
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblFormNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Form Name" HeaderStyle-Width="25%">
                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblFormID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMID") %>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblworkorderid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERID") %>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblFormType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMTYPE") %>'></telerik:RadLabel>
                                                    <asp:LinkButton ID="lblFormName" runat="server" CommandName="DOWNLOAD" CommandArgument='<%# Container.DataItem %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNAME") %>'></asp:LinkButton>
                                                    <telerik:RadLabel ID="lblReportId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTID") %>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Jobcode & Title" HeaderStyle-Width="25%">
                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblWoname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNAME") %>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Component Number" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblcono" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNUMBER") %>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Component Name" HeaderStyle-Width="25%">
                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblconame" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lbljsonyn" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJSONREPORTYN") %>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Date" HeaderStyle-Width="10%" ItemStyle-Width="10%">
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblLastReportedDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDLASTREPORTEDDATE")) %>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Action" Visible="false" HeaderStyle-Width="20%" FooterStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit"
                                                        ToolTip="Edit"><span class="icon"><i class="fas fa-edit"></i></span></asp:LinkButton>

                                                    <asp:LinkButton runat="server" AlternateText="Excel Template" ID="cmdExcelTemplate"
                                                        CommandName="EXCEL" CommandArgument='<%# Container.DataItem %>' ToolTip="Excel Template">
                                    <span class="icon"><i class="far fa-file-excel"></i></span>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton runat="server" AlternateText="comp job" ID="cmdComjob"
                                                        CommandName="COMJOB" CommandArgument='<%# Container.DataItem %>' ToolTip="Component Job">
                                    <span class="icon"><i class="far fa-list-alt"></i></span>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton runat="server" AlternateText="reports" ID="cmdReports"
                                                        CommandName="REPORTS" CommandArgument='<%# Container.DataItem %>' ToolTip="Reports">
                                    <span class="icon"><i class="fas fa-file-alt"></i></span>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton runat="server" AlternateText="Delete" ID="cmdDelete"
                                                        CommandName="DELETE" CommandArgument='<%# Container.DataItem %>' ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:LinkButton runat="server" AlternateText="Save" ID="cmdSave" CommandName="Update" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton runat="server" AlternateText="Cancel" ID="cmdCancel" CommandName="Cancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                                    </asp:LinkButton>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:LinkButton runat="server" AlternateText="Add" ID="cmdAdd"
                                                        CommandName="ADD" CommandArgument='<%# Container.DataItem %>' ToolTip="Excel Template">
                                    <span class="icon"><i class="fas fa-plus-square"></i></span>
                                                    </asp:LinkButton>
                                                </FooterTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" EnableVirtualScrollPaging="false" />
                                        <Resizing AllowColumnResize="true" />
                                    </ClientSettings>
                                </telerik:RadGrid>
                            </ContentTemplate>
                        </telerik:RadDock>
                    </telerik:RadDockZone>
                </td>
            </tr>
            <tr>
                <td style="width: 100%;">
                    <telerik:RadDockZone ID="rdDefect" runat="server" Orientation="Vertical" MinHeight="300px" Docks="RadDock1,RadDock2" Height="100%" FitDocks="true">
                        <telerik:RadDock ID="RadDock2" RenderMode="Lightweight" runat="server"
                            EnableAnimation="true" EnableDrag="true" DockMode="Docked" DockHandle="TitleBar"
                            EnableRoundedCorners="true" Resizable="false" CssClass="higherZIndex">
                            <TitlebarTemplate>
                                <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                                    <ContentTemplate>
                                        <table width="20%">
                                            <tr>
                                                <td>
                                                    <asp:LinkButton
                                                        ID="lnkDefect"
                                                        runat="server"
                                                        Style="text-decoration: underline; color: black;"
                                                        Text="Defects / Non Routine Jobs">
                                                    </asp:LinkButton>
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="lnkDefectComments" runat="server" ToolTip="Comments">
                                                   <span class="icon"><i class="fa fa-comments-blue"></i></span>
                                                    </asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>

                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </TitlebarTemplate>
                            <Commands>
                                <telerik:DockExpandCollapseCommand />
                            </Commands>
                            <ContentTemplate>
                                <telerik:RadGrid RenderMode="Lightweight" ID="gvDefect" runat="server" GridLines="None" OnNeedDataSource="gvDefect_NeedDataSource"
                                    Height="300px" AutoGenerateColumns="false" OnItemDataBound="gvDefect_ItemDataBound" Width="100%"
                                    GroupingEnabled="false" EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000"
                                    ShowFooter="false" ShowHeader="true" EnableViewState="false">
                                    <MasterTableView TableLayout="Fixed">
                                        <NoRecordsTemplate>
                                            <table width="100%" border="0">
                                                <tr>
                                                    <td align="center">
                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </NoRecordsTemplate>
                                        <HeaderStyle Width="102px" />
                                        <CommandItemSettings RefreshText="Search" ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                                        <Columns>

                                            <telerik:GridTemplateColumn HeaderText="Defect No">
                                                <HeaderStyle HorizontalAlign="Left" Width="6%" />
                                                <ItemStyle Width="7%" />
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lbldefectno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEFECTNO")%>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lbldefectjobid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEFECTJOBID") %>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn HeaderText="Component">
                                                <HeaderStyle HorizontalAlign="Left" Width="16%" />
                                                <ItemStyle Width="16%" />
                                                <ItemTemplate>
                                                    <%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTNUMBER") + " - " + DataBinder.Eval(Container, "DataItem.FLDCOMPONENTNAME") %>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn HeaderText="Defect Details">
                                                <HeaderStyle HorizontalAlign="Left" Width="17%" />
                                                <ItemStyle Width="17%" />
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lbldetailsofdefect" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDETAILS") %>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Type" Visible="false">
                                                <HeaderStyle HorizontalAlign="Left" Width="120px" />
                                                <ItemStyle Width="120px" />
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPENAME") %>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn HeaderText="Job No" Visible="false">
                                                <HeaderStyle HorizontalAlign="Left" Width="120px" />
                                                <ItemStyle Width="120px" />
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblJobNo" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDJOBNO")) %>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Date of Issue" AllowSorting="false" Visible="false">
                                                <HeaderStyle HorizontalAlign="Left" Width="7%" />
                                                <ItemStyle Width="7%" />
                                                <ItemTemplate>
                                                    <%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE")) %>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Due" AllowSorting="true" SortExpression="FLDDUEDATE">
                                                <HeaderStyle HorizontalAlign="Left" Width="7%" />
                                                <ItemStyle Width="7%" />
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblDueDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPLANNINGDUEDATE")) %>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn HeaderText="Responsibility">
                                                <HeaderStyle HorizontalAlign="Left" Width="9%" />
                                                <ItemStyle Width="9%" />
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblResponsibility" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDRESPONSIBILITY")) %>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>


                                            <telerik:GridTemplateColumn HeaderText="WO No">
                                                <HeaderStyle HorizontalAlign="Left" Width="7%" />
                                                <ItemStyle Width="7%" />
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblWorkOrderNo" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDWORKGROUPNO")) %>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn HeaderText="Reqn No">
                                                <HeaderStyle HorizontalAlign="Left" Width="7%" />
                                                <ItemStyle Width="7%" />
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkReqNo" CommandName="REQUISITION" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDREQUISITIONNO")) %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn HeaderText="RA No">
                                                <HeaderStyle HorizontalAlign="Left" Width="7%" />
                                                <ItemStyle Width="7%" />
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblRANo" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDRANUMBER")) %>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn HeaderText="Source" Visible="false">
                                                <HeaderStyle HorizontalAlign="Left" Width="120px" />
                                                <ItemStyle Width="120px" />
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblIncident" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDSOURCENAME")) %>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn HeaderText="Significant Defect">
                                                <HeaderStyle HorizontalAlign="Left" Width="7%" />
                                                <ItemStyle Width="7%" />
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblSignificant" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDAFFECTNAVIGATION").ToString()=="1"?"Yes":"No") %>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn HeaderText="DD Job">
                                                <HeaderStyle HorizontalAlign="Left" Width="7%" />
                                                <ItemStyle Width="7%" />
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblDDJob" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDDDJOB").ToString()=="1"?"Yes":"No") %>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn HeaderText="Status">
                                                <HeaderStyle Width="9%" />
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblstatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblWorkOrderReq" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERREQUIRED") %>' Visible="false"></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" AllowFiltering="false" Visible="false">
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                                        CommandName="EDIT" ID="cmdEdit" ToolTip="Edit" Visible="false"></asp:ImageButton>
                                                    <asp:ImageButton runat="server" AlternateText="Postpone" ImageUrl="<%$ PhoenixTheme:images/31.png %>"
                                                        CommandName="POSTPONE" ID="cmdPostpone" ToolTip="Postpone"></asp:ImageButton>
                                                    <asp:ImageButton runat="server" AlternateText="Work order" ImageUrl="<%$ PhoenixTheme:images/Jobs.png %>"
                                                        CommandName="VERIFY" ID="cmdverify" ToolTip="Review" Visible="false"></asp:ImageButton>
                                                    <asp:ImageButton runat="server" AlternateText="Work order" ImageUrl="<%$ PhoenixTheme:images/Jobs.png %>"
                                                        CommandName="WORKORDER" ID="cmdWorkorder" ToolTip="Create Job" Visible="false"></asp:ImageButton>
                                                    <asp:ImageButton runat="server" AlternateText="Complete" ImageUrl="<%$ PhoenixTheme:images/approved.png %>"
                                                        CommandName="COMPLETE" ID="cmdComplete" ToolTip="Complete" Visible="false"></asp:ImageButton>
                                                    <asp:LinkButton runat="server" AlternateText="Communication"
                                                        CommandName="COMMUNICATION" ID="lnkCommunication" ToolTip="Comments">
                                <span class="icon"><i class="fas fa-postcomment"></i></span>
                                                    </asp:LinkButton>
                                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                                        CommandName="DELETE" ID="cmdDelete" ToolTip="Delete"></asp:ImageButton>
                                                    <asp:ImageButton runat="server" ID="cmdAtt" ToolTip="Attachment" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                        </Columns>
                                    </MasterTableView>
                                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                                        <Resizing AllowColumnResize="true" />
                                    </ClientSettings>
                                </telerik:RadGrid>
                            </ContentTemplate>
                        </telerik:RadDock>
                    </telerik:RadDockZone>
                </td>
            </tr>
            <tr>
                <td style="width: 98%;">
                    <telerik:RadDockZone ID="RadDockZone1" runat="server" Orientation="Vertical" MinHeight="300px" Docks="RadDock1,RadDock2,RadDock3" Height="100%">
                        <telerik:RadDock ID="RadDock3" RenderMode="Lightweight" runat="server"
                            EnableAnimation="true" EnableDrag="true" DockMode="Docked" DockHandle="TitleBar"
                            EnableRoundedCorners="true" Resizable="false" CssClass="higherZIndex">
                            <TitlebarTemplate>
                                <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                                    <ContentTemplate>
                                        <table width="20%">
                                            <tr>
                                                <td>
                                                    <asp:LinkButton
                                                        ID="lnkException"
                                                        runat="server"
                                                        Style="text-decoration: underline; color: black;"
                                                        Text="Exception Report">
                                                    </asp:LinkButton>
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="lnkExceptionComments" runat="server" ToolTip="Comments">
                                                   <span class="icon"><i class="fa fa-comments-blue"></i></span>
                                                    </asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>

                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </TitlebarTemplate>
                            <Commands>
                                <telerik:DockExpandCollapseCommand />
                            </Commands>
                            <ContentTemplate>
                                <telerik:RadGrid ID="RadGrid1" runat="server" AllowCustomPaging="false" AllowSorting="false" AllowPaging="false" Width="100%"
                                    CellSpacing="0" GridLines="None" OnNeedDataSource="RadGrid1_NeedDataSource" GroupingEnabled="false" EnableHeaderContextMenu="true"
                                    OnItemCommand="RadGrid1_ItemCommand" DataKeyNames="FLDWORKORDERID" OnItemDataBound="RadGrid1_ItemDataBound">
                                    <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                        AutoGenerateColumns="false" TableLayout="Fixed">
                                        <NoRecordsTemplate>
                                            <table width="100%" border="0">
                                                <tr>
                                                    <td align="center">
                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </NoRecordsTemplate>
                                        <CommandItemSettings ShowRefreshButton="false" RefreshText="Search" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                                        <HeaderStyle Width="102px" />
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderText="Job Number" UniqueName="FLDWORKORDERNUMBER" FilterDelay="2000"
                                                ShowFilterIcon="false" CurrentFilterFunction="Contains" FilterControlWidth="95%" HeaderStyle-Width="100px">
                                                <ItemTemplate>
                                                    <telerik:RadLabel runat="server" ID="lblworkorder" Text='<%#((DataRowView)Container.DataItem)["FLDWORKORDERNUMBER"]%>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="FLDWORKORDERNAME" HeaderText="Job Code & Title" AllowSorting="false" FilterDelay="2000"
                                                ShowFilterIcon="false" CurrentFilterFunction="Contains" FilterControlWidth="95%">
                                                <HeaderStyle Width="180px" />
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblTitle" runat="server"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNAME") %>' Visible="false"></telerik:RadLabel>
                                                    <asp:LinkButton ID="lnkTitle" runat="server" CommandName="Select" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNAME") %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Component No." UniqueName="FLDCOMPONENTNUMBER" FilterDelay="2000"
                                                ShowFilterIcon="false" CurrentFilterFunction="Contains" FilterControlWidth="95%">
                                                <HeaderStyle Width="90px" />
                                                <ItemStyle Width="90px" />
                                                <ItemTemplate>
                                                    <%#((DataRowView)Container.DataItem)["FLDCOMPONENTNUMBER"]%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Component Name" UniqueName="FLDCOMPONENTNAME" FilterDelay="2000"
                                                ShowFilterIcon="false" CurrentFilterFunction="Contains" FilterControlWidth="95%">
                                                <HeaderStyle Width="180px" />
                                                <ItemStyle Width="180px" />
                                                <ItemTemplate>
                                                    <%#((DataRowView)Container.DataItem)["FLDCOMPONENTNAME"]%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Category" UniqueName="FLDJOBCATEGORY" AllowFiltering="true" ShowSortIcon="false" FilterControlWidth="80px" FilterDelay="2000"
                                                AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="EqualTo" Visible="false">
                                                <HeaderStyle Width="140px" />
                                                <ItemTemplate>
                                                    <%#((DataRowView)Container.DataItem)["FLDJOBCATEGORY"]%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Frequency" UniqueName="FLDFREQUENCYTYPE" AllowSorting="false" ShowSortIcon="false" FilterControlWidth="80px" FilterDelay="2000"
                                                AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="EqualTo" Visible="false">
                                                <HeaderStyle Width="130px" />
                                                <ItemStyle Width="130px" />
                                                <ItemTemplate>
                                                    <%#((DataRowView)Container.DataItem)["FLDFREQUENCYNAME"]%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Priority" UniqueName="FLDPLANINGPRIORITY" FilterDelay="2000"
                                                ShowFilterIcon="false" CurrentFilterFunction="EqualTo" FilterControlWidth="95%" Visible="false">
                                                <HeaderStyle Width="70px" />
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <%#((DataRowView)Container.DataItem)["FLDPLANINGPRIORITY"]%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Is Defect Job" UniqueName="FLDJOBDONESTATUS" AllowFiltering="false" Visible="false">
                                                <HeaderStyle Width="70px" />
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <%#((DataRowView)Container.DataItem)["FLDJOBDONESTATUS"]%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Done Date" UniqueName="FLDWORKDONEDATE" DataField="FLDWORKDONEDATE">
                                                <HeaderStyle Width="100px" />
                                                <ItemTemplate>
                                                    <%#General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDWORKDONEDATE"])%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Done By" UniqueName="FLDREPORTBY" Visible="false">
                                                <HeaderStyle Width="180px" />
                                                <ItemStyle Width="180px" />
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <%#((DataRowView)Container.DataItem)["FLDREPORTBY"]%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Remarks" UniqueName="FLDREMARKS" AllowFiltering="false" Visible="false">
                                                <HeaderStyle Width="300px" />
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <%# General.SanitizeHtml(((DataRowView)Container.DataItem)["FLDREMARKS"].ToString())%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center" AllowFiltering="false" AllowSorting="false">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <%-- <asp:LinkButton runat="server" AlternateText="MaintenanceLog"
                                CommandName="MAINTENANCEFORM" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdRTemplates"
                                ToolTip="Reporting Templates" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-file"></i></span>
                            </asp:LinkButton>--%>
                                                    <asp:LinkButton runat="server" AlternateText="Parameters"
                                                        CommandName="PARAMETERS" ID="cmdParameters"
                                                        ToolTip="Parameters" Width="20px" Height="20px">
                               <span class="icon"><i class="fas fa-newspaper"></i></span>
                                                    </asp:LinkButton>
                                                    <%--<asp:ImageButton runat="server" ID="cmdAttachments" ToolTip="Attachment" 
                                CommandName="ATTACHMENTS" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" />

                            <asp:LinkButton runat="server" AlternateText="Parts"
                                CommandName="PARTS" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdParts"
                                ToolTip="Parts" Width="20px" Height="20px">
                                <span class="icon"><i class="fas fa-cogs"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="RA History"
                                CommandName="RA" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdRA"
                                ToolTip="RA" Width="20px" Height="20px">
                                <span class="icon"><i class="fas fa-eye"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="PTW History"
                                CommandName="PTW" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdPTW"
                                ToolTip="PTW" Width="20px" Height="20PX">
                                <span class="icon"><i class="fas fa-copy"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Report" ID="cmdReschedule" CommandName="RESCHEDULE" ToolTip="Postpone History">
                                 <span class="icon"><i class="far fa-calendar-alt"></i></span>
                            </asp:LinkButton>
                            <asp:ImageButton ID="lnkPtwWaive" runat="server" ImageUrl="<%$ PhoenixTheme:images/45.png %>" ToolTip="Waive" CommandName="WAIVE" />--%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
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
                                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                    </ClientSettings>
                                </telerik:RadGrid>
                            </ContentTemplate>
                        </telerik:RadDock>
                    </telerik:RadDockZone>
                </td>
            </tr>
        </table>
        <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
    </form>
</body>
</html>

