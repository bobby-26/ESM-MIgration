<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionDashBoardAuditOfficeRecordList.aspx.cs" Inherits="Inspection_InspectionDashBoardAuditOfficeRecordList" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Inspection" Src="~/UserControls/UserControlInspection.ascx" %>
<%@ Register TagPrefix="eluc" TagName="IChapter" Src="~/UserControls/UserControlInspectionChapter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ITopic" Src="~/UserControls/UserControlInspectionTopic.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Checklist" Src="~/UserControls/UserControlInspectionChecklist.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddrType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleet.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Confirm(args) {
                if (args) {
                    __doPostBack("<%=ucConfirm.UniqueID %>", "");
                }
            } function ConfirmDelete(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmDelete.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInspectionScheduleSearch" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status ID="ucStatus" runat="server" />
        <eluc:Title runat="server" ID="ucTitle" Text="Office Audit / Inspection Log" Visible="false"></eluc:Title>
        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <asp:Button ID="ucConfirm" runat="server" OKText="Yes" CancelText="No" OnClick="ucConfirm_Click" />
        <asp:Button ID="ucConfirmDelete" runat="server" Text="ConfirmDelete" OnClick="btnConfirmDelete_Click" />
<%--        <eluc:TabStrip ID="MenuGeneral" runat="server" TabStrip="true" OnTabStripCommand="MenuGeneral_TabStripCommand"></eluc:TabStrip>--%>
        <eluc:TabStrip ID="MenuInspectionScheduleSearch" runat="server" OnTabStripCommand="InspectionScheduleSearch_TabStripCommand"></eluc:TabStrip>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvAuditRecordList" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowPaging="true" AllowCustomPaging="true"
            Width="100%" Height="88%" CellPadding="3" OnItemCommand="gvAuditRecordList_ItemCommand" OnItemDataBound="gvAuditRecordList_ItemDataBound"
            OnSortCommand="gvAuditRecordList_SortCommand" OnNeedDataSource="gvAuditRecordList_NeedDataSource" AllowSorting="true" GroupingEnabled="false"
            EnableHeaderContextMenu="true" ShowFooter="false" ShowHeader="true" EnableViewState="false" GridLines="None">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDREVIEWSCHEDULEID" TableLayout="Fixed">
                <ColumnGroups>
                    <telerik:GridColumnGroup Name="DeficiencyCount" HeaderText="Def Count" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                    <telerik:GridColumnGroup Name="AuditInsp" HeaderText="Audit/Inspection" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                </ColumnGroups>
                <NoRecordsTemplate>
                    <table width="100%" border="0">
                        <tr>
                            <td align="center">
                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </NoRecordsTemplate>
                <HeaderStyle Width="102px" />
                <CommandItemSettings RefreshText="Search" ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                <Columns>
                    <telerik:GridTemplateColumn HeaderStyle-Width="85px" HeaderText="Company">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblCompanyCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderStyle-Width="41px" HeaderText="M/C">
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="20px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblManual" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISMANUAL") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Reference Number" HeaderStyle-Width="220px">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="80px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblInspectionRefNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENUMBER").ToString() %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblCompanyId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblDTkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                            <eluc:ToolTip ID="ucToolTipInspectionRefNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENUMBER") %>' TargetControlId="lblInspectionRefNo" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn>
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkInspectionNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDREVIEWNAME">Audit / Inspection</asp:LinkButton>
                            <img id="FLDREVIEWNAME" runat="server" visible="false" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblInspectionScheduleId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVIEWSCHEDULEID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblPlannerId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVIEWSCHEDULEPLANNERID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblInspectionShortcode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblInspectionDtKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                            <asp:LinkButton ID="lnkInspection" runat="server" CommandName="NAVIGATE" CommandArgument="<%# Container.DataItem %>"
                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE").ToString().Length>35 ? DataBinder.Eval(Container, "DataItem.FLDSHORTCODE").ToString().Substring(0, 35)+ "..." : DataBinder.Eval(Container, "DataItem.FLDSHORTCODE").ToString() %>'></asp:LinkButton>
                            <%--<eluc:ToolTip ID="ucToolTipName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>' />--%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderStyle-Width="78px" HeaderText="Last Done">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="70px"></ItemStyle>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkCompletionDateHeaderr" runat="server" CommandName="Sort" CommandArgument="FLDCOMPLETIONDATE">Last Done</asp:LinkButton>
                            <img id="FLDCOMPLETIONDATE" runat="server" visible="false" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblInspectionCompletionDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOMPLETIONDATE")) %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Name" ColumnGroupName="AuditInsp">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="120px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblNameOfInspector" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFINSPECTOR").ToString() %>'></telerik:RadLabel>
                            <eluc:ToolTip ID="ucToolTipInspector" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFINSPECTOR") %>' TargetControlId="lblNameOfInspector" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderStyle-Width="85px" HeaderText="Organization" ColumnGroupName="AuditInsp">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="80px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblOrganisation" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORGANISATION").ToString().Length>8 ? DataBinder.Eval(Container, "DataItem.FLDORGANISATION").ToString().Substring(0, 8)+ "..." : DataBinder.Eval(Container, "DataItem.FLDORGANISATION").ToString()  %>'></telerik:RadLabel>
                            <eluc:ToolTip ID="ucToolTipOrganisation" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORGANISATION") %>' TargetControlId="lblOrganisation" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderStyle-Width="75px" HeaderText="Status">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="60px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblStatus" runat="server" Width="50px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME").ToString() %>'></telerik:RadLabel>
                            <eluc:ToolTip ID="ucToolTipStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>' TargetControlId="lblStatusHeader" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderStyle-Width="50px" HeaderText="Total" ColumnGroupName="DeficiencyCount">
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="60px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDeficiencyCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEFICIENCYCOUNT") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderStyle-Width="45px" HeaderText="MNC" ColumnGroupName="DeficiencyCount">
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="60px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblMajorNCCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAJORNCCOUNT") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderStyle-Width="35px" HeaderText="NC" ColumnGroupName="DeficiencyCount">
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="60px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblNCCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNCCOUNT") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderStyle-Width="40px" HeaderText="OBS" ColumnGroupName="DeficiencyCount">
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="60px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblOBSCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOBSCOUNT") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderStyle-Width="110px" HeaderText="Action">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Attachment" CommandName="ATTACHMENT" ID="cmdAtt" ToolTip="Attachment">
                                     <span class="icon"> <i class="fa fa-paperclip"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Summary" CommandName="DEFICIENCYSUMMARY"
                                ID="cmdDeficiencySummary" ToolTip="View Deficiency Summary">
                                     <span class="icon"><i class="fas fa-eye"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Export XL" CommandName="EXPORT2XL" ID="cmdExport2XL" ToolTip="Export XL">
                                      <span class="icon"><i class="fas fa-file-excel"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" Visible="true" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
        <table cellpadding="1" cellspacing="1">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblManual" runat="server" Text="* M - Manual "></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadLabel ID="lblConfiguration" runat="server" Text="* C - Configuration"></telerik:RadLabel>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
