<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnersMonthlyReportsRestHours.aspx.cs" Inherits="Owners_OwnersMonthlyReportsRestHours" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Work And Rest Hours Summary</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
       <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <link href="../Content/bootstrap.min.v3.css" rel="stylesheet" />
        <link href="../Content/OwnersReport.css" rel="stylesheet" />
        <%: Scripts.Render("~/bundles/js") %>

        <script type="text/javascript">
            function ConfirmVerify(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmWorkHours.UniqueID %>", "");
                }
            }
            function pageLoad() {
                PaneResized();
            }

            function PaneResized(sender, args) {
                var browserHeight = $telerik.$(window).height();
                var grid1 = $find("gvNC");
                grid1._gridDataDiv.style.height = (browserHeight - 380) + "px";

            }
        </script>
        <style>
            .rbToggleCheckbox {
                float: right !important;
            }
        </style>
        <style type="text/css">
        .rgExpand,
        .rgCollapse {
            display: none !important;
        }

        .panelfont {
            overflow: auto;
            font-size: 11px;
        }

        .RadGrid .rgHeader, .RadGrid th.rgResizeCol, .RadGrid .rgRow td, .RadGrid .rgAltRow td {
            padding-left: 2px !important;
            padding-right: 2px !important;
        }

        .higherZIndex {
            z-index: 2;
        }

        .RadGrid_Windows7 .rgHeader {
            color: black !important;
        }

        .rdTitleWrapper, .rdTitleBar {
            background-color: rgb(194, 220, 252) !important;
            background-image: linear-gradient(rgb(244, 248, 250), rgb(233, 242, 251) 50%, rgb(221, 231, 245) 50%, rgb(228, 237, 248)) !important;
            color: black !important;
        }

        .rgGroupCol {
            padding-left: 0 !important;
            padding-right: 0 !important;
            font-size: 1px !important;
        }

        .white {
            color: black !important;
            font-family: "Helvetica Neue",Helvetica,Arial,sans-serif !important;
            text-align-last: right !important;
        }
    </style>
    </telerik:RadCodeBlock>
</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="frmNonComplianceSummary" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <telerik:RadDockZone ID="RadDockZone1" runat="server" Orientation="Vertical" Docks="RadDock1,RadDock2" Height="100%" FitDocks="true">
            <telerik:RadDock ID="RadDock1" RenderMode="Lightweight" runat="server" Height="585px"
                EnableAnimation="true" EnableDrag="true" DockMode="Docked" DockHandle="TitleBar"
                EnableRoundedCorners="true" Resizable="false" CssClass="higherZIndex" Width="100%">
                <TitlebarTemplate>
                    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                        <ContentTemplate>
                            <table width="20%">
                                <tr>
                                    <td>
                                        <asp:LinkButton
                                            ID="lnkExpenses"
                                            runat="server"
                                            Style="text-decoration: underline; color: black;"
                                            Text="Work and Rest Hours">
                                        </asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lnkWorkandRestHoursComments" runat="server" ToolTip="Comments">
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
                    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
                    <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
                    </telerik:RadWindowManager>
<%--                    <telerik:RadAjaxManager ID="RadAjaxManager" runat="server">
                        <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="MenuReportNonCompliance">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="MenuReportNonCompliance" />
                                    <telerik:AjaxUpdatedControl ControlID="ucConfirmWorkHours" />
                                    <telerik:AjaxUpdatedControl ControlID="ucError" />
                                    <telerik:AjaxUpdatedControl ControlID="lblCO" />
                                    <telerik:AjaxUpdatedControl ControlID="lblCE" />
                                    <telerik:AjaxUpdatedControl ControlID="lblMASTERNAME" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="ddlMonth">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="ddlMonth" />
                                    <telerik:AjaxUpdatedControl ControlID="gvNC" />
                                    <telerik:AjaxUpdatedControl ControlID="MenuReportNonCompliance" />
                                    <telerik:AjaxUpdatedControl ControlID="lblCO" />
                                    <telerik:AjaxUpdatedControl ControlID="lblCE" />
                                    <telerik:AjaxUpdatedControl ControlID="lblMASTERNAME" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="ddlYear">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="ddlYear" />
                                    <telerik:AjaxUpdatedControl ControlID="gvNC" />
                                    <telerik:AjaxUpdatedControl ControlID="MenuReportNonCompliance" />
                                    <telerik:AjaxUpdatedControl ControlID="lblCO" />
                                    <telerik:AjaxUpdatedControl ControlID="lblCE" />
                                    <telerik:AjaxUpdatedControl ControlID="lblMASTERNAME" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="ucConfirmWorkHours">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="MenuReportNonCompliance" />
                                    <telerik:AjaxUpdatedControl ControlID="ucConfirmWorkHours" />
                                    <telerik:AjaxUpdatedControl ControlID="ucError" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                        </AjaxSettings>
                    </telerik:RadAjaxManager>--%>
                    <asp:Button ID="ucConfirmWorkHours" runat="server" Text="cmdConfirmWorkHours" OnClick="ucConfirmWorkHours_Click" CssClass="hidden" />
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <eluc:TabStrip ID="MenuReportNonCompliance" runat="server" OnTabStripCommand="MenuReportNonCompliance_TabStripCommand" Visible="false"></eluc:TabStrip>
                 <%--   <table width="60%">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblReportfortheMonthof" Visible="false" runat="server" Text="Report for the Month of :"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlMonth" runat="server" Visible="false" CssClass="input_mandatory" AutoPostBack="true" Filter="Contains" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="January" Value="1" />
                                        <telerik:RadComboBoxItem Text="February" Value="2" />
                                        <telerik:RadComboBoxItem Text="March" Value="3" />
                                        <telerik:RadComboBoxItem Text="April" Value="4" />
                                        <telerik:RadComboBoxItem Text="May" Value="5" />
                                        <telerik:RadComboBoxItem Text="June" Value="6" />
                                        <telerik:RadComboBoxItem Text="July" Value="7" />
                                        <telerik:RadComboBoxItem Text="August" Value="8" />
                                        <telerik:RadComboBoxItem Text="September" Value="9" />
                                        <telerik:RadComboBoxItem Text="October" Value="10" />
                                        <telerik:RadComboBoxItem Text="November" Value="11" />
                                        <telerik:RadComboBoxItem Text="December" Value="12" />
                                    </Items>
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblYear" Visible="false" runat="server" Text="Year :"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlYear" Visible="false" runat="server" CssClass="input_mandatory" AutoPostBack="true" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged"
                                    Filter="Contains" OnDataBound="ddlYear_DataBound" Sort="Descending">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                    </table>--%>

<%--                    <h2 style="text-align: center"><u>Rest Hours Non Compliance Analysis Report</u></h2>--%>
                  <%--  <table style="width: 100%">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblShipName" runat="server" Text="Ship's Name" Font-Bold="true"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblShipNameValue" runat="server" Text=""></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblIMO" runat="server" Text="IMO NO" Font-Bold="true"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblIMOValue" runat="server" Text=""></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblFlag" runat="server" Text="Flag" Font-Bold="true"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblFlagValue" runat="server" Text=""></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblMonth" runat="server" Text="Month & Year" Font-Bold="true"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblMonthValue" runat="server" Text=""></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>--%>
                    <telerik:RadGrid ID="gvNC" runat="server" AutoGenerateColumns="False" Font-Size="11px" GridLines="None" Width="100%"
                        AllowCustomPaging="false" AllowSorting="false" AllowPaging="false" CellSpacing="0" GroupingEnabled="false"
                        EnableViewState="true" OnItemDataBound="gvNC_ItemDataBound" OnNeedDataSource="gvNC_NeedDataSource">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false">
                            <NoRecordsTemplate>
                                <table runat="server" width="100%" border="0">
                                    <tr>
                                        <td align="center">
                                            <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </NoRecordsTemplate>
                            <ColumnGroups>

                                <telerik:GridColumnGroup HeaderText="STCW/ILO/MLC Requirements" Name="STCREQUIREMENTS">
                                    <HeaderStyle HorizontalAlign="Center" />
                                </telerik:GridColumnGroup>
                                <telerik:GridColumnGroup HeaderText="OPA 90 Requirements" Name="OPAREQUIREMENTS">
                                    <HeaderStyle HorizontalAlign="Center" />
                                </telerik:GridColumnGroup>
                            </ColumnGroups>
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="No">
                                    <HeaderStyle HorizontalAlign="Center" Width="25px" />
                                    <ItemStyle Wrap="true" HorizontalAlign="Center" Width="25px" />
                                    <ItemTemplate>
                                        <%# Container.ItemIndex+1 %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Name">
                                    <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="200px" />
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDEMPLOYEENAME"]%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Rank">
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="100px" />
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDRANKNAME"]%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="From">
                                    <HeaderStyle HorizontalAlign="Center" Width="60px" />
                                    <ItemStyle Wrap="true" HorizontalAlign="Center" Width="60px" />
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDFROMDATE"].ToString()%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="To">
                                    <HeaderStyle HorizontalAlign="Center" Width="60px" />
                                    <ItemStyle Wrap="true" HorizontalAlign="Center" Width="60px" />
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDTODATE"].ToString()%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="No. of Days with NCs" UniqueName="NCCOUNT">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle Wrap="true" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDNOOFDAYSWITHNC"]%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="No. Of days: < 10 hrs rest in any 24 hrs period" UniqueName="FLDS1" ColumnGroupName="STCREQUIREMENTS">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle Wrap="true" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDS1"].ToString() %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="No. of days: 10hrs rest split in more than 2 periods" UniqueName="FLDS2" ColumnGroupName="STCREQUIREMENTS">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle Wrap="true" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDS2"].ToString() %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="No. of days: Longests period of rest is < 6 hrs continuous rest" UniqueName="FLDS3" ColumnGroupName="STCREQUIREMENTS">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle Wrap="true" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDS3"].ToString() %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="No. of days: Interval between consecutive periods of rest is exceeding 14 hrs" UniqueName="FLDS4" ColumnGroupName="STCREQUIREMENTS">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle Wrap="true" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDS4"].ToString() %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="No. of days: Total rest in any 7 days period is < 77 hrs" UniqueName="FLDS5" ColumnGroupName="STCREQUIREMENTS">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle Wrap="true" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDS5"].ToString() %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="No. of days: Total hrs of work exceeding 15 hrs of work in last 24 hrs" UniqueName="FLDO1" ColumnGroupName="OPAREQUIREMENTS">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle Wrap="true" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDO1"].ToString() %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="No. of days: Total hrs of work exceeding 36 hrs of work in last 72 hrs" UniqueName="FLDO2" ColumnGroupName="OPAREQUIREMENTS">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle Wrap="true" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDO2"].ToString() %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Level of breach" UniqueName="FLDNCLEVEL">
                                    <HeaderStyle HorizontalAlign="Center" Width="50px" />
                                    <ItemStyle Wrap="true" HorizontalAlign="Center" Width="50px" />
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDNCLEVEL"]%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Remarks" UniqueName="FLDREMARKS">
                                    <HeaderStyle HorizontalAlign="Center" Width="250px" />
                                    <ItemStyle Wrap="true" HorizontalAlign="Center" Width="250px" />
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDREMARKS"]%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                    <br />
                    <telerik:RadLabel ID="lblNote" runat="server" Text="Note:" Font-Bold="true"></telerik:RadLabel>
                    <br />
                    <telerik:RadLabel ID="Note1" runat="server" Text="Level 1 Breach: Non Conformities can be prevented by the Seafarer or Head of Department."></telerik:RadLabel>
                    <br />
                    <telerik:RadLabel ID="RadLabel1" runat="server" Text="Level 2 Breach: Preventing Non Conformities requires the intervention / assistance of the Master."></telerik:RadLabel>
                    <br />
                    <telerik:RadLabel ID="RadLabel2" runat="server" Text="Level 3 Breach: Preventing Non Conformities requires the assistance of the Office."></telerik:RadLabel>
                    <br />
                    <br />
                    <table style="width: 100%">
                        <tr>
                            <td>
                                <telerik:RadLabel runat="server" ID="lblSigned" Text="Signed:"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadLabel runat="server" ID="lblChiefOffice" Text="Chief Officer:"></telerik:RadLabel>
                                <telerik:RadLabel runat="server" ID="lblCO" Text="" Font-Underline="true"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadLabel runat="server" ID="lblChiefEngineer" Text="Chief Engineer:"></telerik:RadLabel>
                                <telerik:RadLabel runat="server" ID="lblCE" Text="" Font-Underline="true"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadLabel runat="server" ID="lblMaster" Text="Master:"></telerik:RadLabel>
                                <telerik:RadLabel runat="server" ID="lblMasterName" Text="" Font-Underline="true"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </telerik:RadDock>
        </telerik:RadDockZone>
    </form>
</body>
</html>
