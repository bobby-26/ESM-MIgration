<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnersMonthlyReportCertifcates.aspx.cs" Inherits="OwnersMonthlyReportCertifcates" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <telerik:RadCodeBlock ID="radCodeBlock1" runat="server">
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <link href="../Content/bootstrap.min.v3.css" rel="stylesheet" />
        <link href="../Content/OwnersReport.css" rel="stylesheet" />
        <link href="../Content/fontawesome-all.min.css" rel="stylesheet" />
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

        .RadDock .rdTitleWrapper {
            padding: 5px 5px !important;
            line-height: 9px !important;
            height: 16px !important;
        }

        .fas, .fa {
            height: 11px !important;
            width: 11px !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="93%" EnableAJAX="false">
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <table style="width: 99.5%" runat="server">
                <tr>
                    <td style="width: 40%; vertical-align: top;" runat="server">
                        <telerik:RadDockZone ID="rdcertificate" runat="server" Orientation="Vertical" Docks="RadDock1,RadDock2" Height="100%" FitDocks="true">
                            <telerik:RadDock ID="RadDock1" RenderMode="Lightweight" runat="server" Height="250px" Title="Certificates and Surveys"
                                EnableAnimation="true" EnableDrag="true" DockMode="Docked" DockHandle="TitleBar"
                                EnableRoundedCorners="true" Resizable="false" CssClass="higherZIndex" Width="100%">
                                <TitlebarTemplate>
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <telerik:RadLabel runat="server" ID="lblPurchase" Text="Certificates and Surveys"></telerik:RadLabel>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="BtnCertificatesandSurveyInfo" runat="server" ToolTip="Info">
                                                   <span class="icon"><i class="fas fa-info-circle" style="color:black;"></i></span>
                                                        </asp:LinkButton>
                                                    </td>
                                                    <td>&nbsp;<asp:LinkButton ID="lnkCertificatesandSurveysComments" runat="server" ToolTip="Comments">
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
                                    <telerik:RadGrid ID="gvCertificateSchedule" runat="server" AutoGenerateColumns="false"
                                        AllowSorting="false" GroupingEnabled="false" BorderStyle="None"
                                        EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000" OnNeedDataSource="gvCertificateSchedule_NeedDataSource"
                                        OnItemDataBound="gvCertificateSchedule_ItemDataBound" Width="100%">
                                        <MasterTableView TableLayout="Fixed" EnableHeaderContextMenu="false">
                                            <NoRecordsTemplate>
                                                <table runat="server" width="100%" border="0">
                                                    <tr>
                                                        <td align="center">
                                                            <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </NoRecordsTemplate>
                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="Type" HeaderStyle-Width="55%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblmeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Overdue" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkOverdue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVERDUECOUNT") %>'></asp:LinkButton>
                                                        <telerik:RadLabel ID="lblOverdueurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVERDUEURL") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="30 Days" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnk30Days" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLD30COUNT") %>'></asp:LinkButton>
                                                        <telerik:RadLabel ID="lbl30Daysurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLD30URL") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="60 Days" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnk60Days" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLD60COUNT") %>'></asp:LinkButton>
                                                        <telerik:RadLabel ID="lbl60Daysurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLD60URL") %>'></telerik:RadLabel>
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
                    <td style="width: 60%; vertical-align: top;">
                        <telerik:RadDockZone ID="rdregulatory" runat="server" Orientation="Vertical" Docks="RadDock1,RadDock2" Height="100%" FitDocks="true">
                            <telerik:RadDock ID="RadDock2" RenderMode="Lightweight" runat="server" Height="250px" Title="Regulatory Matters"
                                EnableAnimation="true" EnableDrag="true" DockMode="Docked" DockHandle="TitleBar"
                                EnableRoundedCorners="true" Resizable="false" CssClass="higherZIndex" Width="100%">
                                <TitlebarTemplate>
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <telerik:RadLabel runat="server" ID="RadLabel1" Text="Regulatory Matters"></telerik:RadLabel>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="BtnRegulatory" runat="server" ToolTip="Info">
                                                   <span class="icon"><i class="fas fa-info-circle" style="color:black;"></i></span>
                                                        </asp:LinkButton>
                                                    </td>
                                                    <td>&nbsp;<asp:LinkButton ID="lnkRegulatoryMattersComments" runat="server" ToolTip="Comments">
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
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 50%; vertical-align: top;">
                                                <telerik:RadGrid ID="GVR" runat="server" AutoGenerateColumns="false"
                                                    AllowSorting="false" GroupingEnabled="false" BorderStyle="None" OnItemCommand="GVR_ItemCommand"
                                                    EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000" OnItemDataBound="GVR_ItemDataBound" OnNeedDataSource="GVR_NeedDataSource">
                                                    <MasterTableView TableLayout="Fixed" EnableHeaderContextMenu="false" Width="100%" EditMode="InPlace">
                                                        <NoRecordsTemplate>
                                                            <table runat="server" width="100%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="" HeaderStyle-Width="40%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblmeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURENAME") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Last" HeaderStyle-Width="40%" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></asp:LinkButton>
                                                                    <telerik:RadLabel ID="lblcount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDURL") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblShortCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>' Visible="false"></telerik:RadLabel>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <eluc:Date ID="ucDateEdit" runat="server" CssClass="gridinput_mandatory" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDMEASURE")) %>' Width="90%"
                                                                        DatePicker="true" />
                                                                    <telerik:RadLabel ID="lblShortCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>' Visible="false"></telerik:RadLabel>
                                                                </EditItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderStyle-Width="20%" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server" AlternateText="Edit" ToolTip="Edit" Width="20PX" Height="20PX"
                                                                        CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit">
                                                                        <span class="icon"><i class="fas fa-edit"></i></span>
                                                                    </asp:LinkButton>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:LinkButton runat="server" AlternateText="Save" ToolTip="Save" Width="20PX" Height="20PX"
                                                                        CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave">
                                                                        <span class="icon"><i class="fas fa-save"></i></span>
                                                                    </asp:LinkButton>
                                                                    <asp:LinkButton runat="server" AlternateText="Cancel" ToolTip="Cancel" Width="20PX" Height="20PX"
                                                                        CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel">
                                                                        <span class="icon"><i class="fas fa-times-circle"></i></span>
                                                                    </asp:LinkButton>
                                                                </EditItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                                                        <Resizing AllowColumnResize="true" />
                                                        <ClientEvents OnHeaderMenuShowing="OnHeaderMenuShowing" />
                                                    </ClientSettings>
                                                </telerik:RadGrid>
                                            </td>
                                            <td style="width: 50%; vertical-align: top;">
                                                <telerik:RadGrid ID="GVR2" runat="server" AutoGenerateColumns="false"
                                                    AllowSorting="false" GroupingEnabled="false" BorderStyle="None" OnItemCommand="GVR2_ItemCommand"
                                                    EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000" OnItemDataBound="GVR2_ItemDataBound" OnNeedDataSource="GVR2_NeedDataSource">
                                                    <MasterTableView TableLayout="Fixed" EnableHeaderContextMenu="false" Width="100%" EditMode="InPlace">
                                                        <NoRecordsTemplate>
                                                            <table runat="server" width="100%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="" HeaderStyle-Width="40%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblmeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURENAME") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Next" HeaderStyle-Width="40%" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></asp:LinkButton>
                                                                    <telerik:RadLabel ID="lblcount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDURL") %>'></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="lblShortCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>' Visible="false"></telerik:RadLabel>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <eluc:Date ID="ucDateEdit" runat="server" CssClass="gridinput_mandatory" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDMEASURE")) %>' Width="90%"
                                                                        DatePicker="true" />
                                                                    <telerik:RadLabel ID="lblShortCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>' Visible="false"></telerik:RadLabel>
                                                                </EditItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderStyle-Width="20%" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server" AlternateText="Edit" ToolTip="Edit" Width="20PX" Height="20PX"
                                                                        CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit">
                                                                        <span class="icon"><i class="fas fa-edit"></i></span>
                                                                    </asp:LinkButton>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:LinkButton runat="server" AlternateText="Save" ToolTip="Save" Width="20PX" Height="20PX"
                                                                        CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave">
                                                                        <span class="icon"><i class="fas fa-save"></i></span>
                                                                    </asp:LinkButton>
                                                                    <asp:LinkButton runat="server" AlternateText="Cancel" ToolTip="Cancel" Width="20PX" Height="20PX"
                                                                        CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel">
                                                                        <span class="icon"><i class="fas fa-times-circle"></i></span>
                                                                    </asp:LinkButton>
                                                                </EditItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                                                        <Resizing AllowColumnResize="true" />
                                                        <ClientEvents OnHeaderMenuShowing="OnHeaderMenuShowing" />
                                                    </ClientSettings>
                                                </telerik:RadGrid>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadDockZone ID="RadDockZone1" runat="server" Orientation="Vertical" Docks="RadDock3" Height="100%" FitDocks="true">
                            <telerik:RadDock ID="RadDock3" RenderMode="Lightweight" runat="server" Height="350px" Title="Schedule List"
                                EnableAnimation="true" EnableDrag="true" DockMode="Docked" DockHandle="TitleBar"
                                EnableRoundedCorners="true" Resizable="false" CssClass="higherZIndex" Width="100%">
                                <TitlebarTemplate>
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton ID="lnkCDISIRESchedule" runat="server" Style="text-decoration: underline; color: black;"
                                                            Text="CDI/ SIRE Schedule"></asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        &nbsp;<asp:LinkButton ID="BtnCDISIRESchedule" runat="server" ToolTip="Info">
                                                   <span class="icon"><i class="fas fa-info-circle" style="color:black;"></i></span>
                                                        </asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        &nbsp;<asp:LinkButton ID="lnkCDISIREScheduleComments" runat="server" ToolTip="Comments">
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
                                    <telerik:RadGrid RenderMode="Lightweight" ID="gvScheduleForCompany" runat="server" AutoGenerateColumns="False" EnableViewState="true"
                                        Font-Size="11px" Width="100%" CellPadding="3" OnNeedDataSource="gvScheduleForCompany_NeedDataSource"
                                        OnItemDataBound="gvScheduleForCompany_ItemDataBound" ShowFooter="false" ShowHeader="true" Height="300px"
                                        AllowSorting="true" GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="false">
                                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                        <MasterTableView EditMode="InPlace" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                            AutoGenerateColumns="false">
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
                                            <Columns>
                                                <telerik:GridTemplateColumn Visible="false">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgFlag" runat="server" Visible="false" />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn Visible="false">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="lnkInspectionNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDCOMPANYNAME">Company&nbsp;</asp:LinkButton>
                                                        <img id="FLDCOMPANYNAME" runat="server" visible="false" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblCompanyName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYNAME") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblCompanyId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYID") %>'
                                                            Visible="false">
                                                        </telerik:RadLabel>
                                                        <telerik:RadLabel ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'
                                                            Visible="false">
                                                        </telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblScheduleId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHEDULEID") %>' Visible="false">
                                                        </telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblScheduleByCompanyId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHEDULEBYCOMPANYID") %>' Visible="false">
                                                        </telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblInspectionId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONID") %>' Visible="false">
                                                        </telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblIsManual" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISMANUALINSPECTION") %>' Visible="false">
                                                        </telerik:RadLabel>
                                                        <asp:LinkButton ID="lnkVessel" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'
                                                            CommandName="SELECT" CommandArgument='<%# Container.DataItem%>'></asp:LinkButton>
                                                        <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>' Visible="false">
                                                        </telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>' Visible="false">
                                                        </telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Type" HeaderStyle-Width="9%">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="9%"></ItemStyle>
                                                    <HeaderTemplate>
                                                        Type
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblInspection" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONSHORTCODE") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="3rd Last" HeaderStyle-Width="17%">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="17%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblthirdInspection" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLD3RDINSPECTION") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="2nd Last" HeaderStyle-Width="17%">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="17%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblsecondInspection" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLD2NDINSPECTION") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Last" HeaderStyle-Width="17%">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="17%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblfirstInspection" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLD1STINSPECTION") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Due" HeaderStyle-Width="10%">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                                                    <HeaderTemplate>
                                                        Due
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblDueDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDUEDATE")) %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn Visible="false">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                                                    <HeaderTemplate>
                                                        <telerik:RadLabel ID="lblLastDoneHeader" runat="server">Last Done</telerik:RadLabel>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblLastDoneDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDLASTDONEDATE")) %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn Visible="false">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="120px"></ItemStyle>
                                                    <HeaderTemplate>
                                                        <telerik:RadLabel ID="lblBasisHeader" runat="server">Basis</telerik:RadLabel>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkBasisDetails" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBASISDETAILS") %>'
                                                            CommandName="SHOW" CommandArgument='<%# Container.DataItem%>'></asp:LinkButton>
                                                        <telerik:RadLabel ID="lblBasisDetails" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBASISDETAILS") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblBasisId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBASISID") %>'
                                                            Visible="false">
                                                        </telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Planned" HeaderStyle-Width="10%">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                                                    <HeaderTemplate>
                                                        Planned
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblPlannedDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPLANNEDDATE")) %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn Visible="false">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                                                    <HeaderTemplate>
                                                        <telerik:RadLabel ID="lblPlannedPortHeader" runat="server">Planned Port</telerik:RadLabel>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblPlannedPort" runat="server" Width="90px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME").ToString().Length>10 ? DataBinder.Eval(Container, "DataItem.FLDSEAPORTNAME").ToString().Substring(0, 10)+ "..." : DataBinder.Eval(Container, "DataItem.FLDSEAPORTNAME").ToString() %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn Visible="false">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                                                    <HeaderTemplate>
                                                        <telerik:RadLabel ID="lblInspectorHeader" runat="server">Inspector</telerik:RadLabel>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblInspector" runat="server" Width="90px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFINSPECTOR").ToString().Length>10 ? DataBinder.Eval(Container, "DataItem.FLDNAMEOFINSPECTOR").ToString().Substring(0, 10)+ "..." : DataBinder.Eval(Container, "DataItem.FLDNAMEOFINSPECTOR").ToString() %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Active" HeaderStyle-Width="8%">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="8%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblActive" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVEYN") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="12%">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="12%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblScheduleStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCHEDULESTATUS") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblStatusId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
