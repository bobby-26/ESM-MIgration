<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionDashBoardOpenReportsList.aspx.cs" Inherits="Inspection_InspectionDashBoardOpenReportsList" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvDirectIncident.ClientID %>"));
                }, 200);
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
               Resize();
               fade('statusmessage');
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersInspection" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />

        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />


        <eluc:TabStrip ID="MenuRegistersInspectionGeneral" runat="server"
            OnTabStripCommand="MenuRegistersInspectionGeneral_TabStripCommand"></eluc:TabStrip>

        <%--<iframe runat="server" id="ifMoreInfo" scrolling="YES" style="min-height: 400px; width: 100%">
                </iframe>--%>

        <eluc:TabStrip ID="MenuRegistersInspection" runat="server" OnTabStripCommand="RegistersInspection_TabStripCommand"></eluc:TabStrip>

        <telerik:RadGrid RenderMode="Lightweight" ID="gvDirectIncident" runat="server" Width="100%" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" OnNeedDataSource="gvDirectIncident_NeedDataSource"
            OnSortCommand="gvDirectIncident_SortCommand"
            OnItemCommand="gvDirectIncident_ItemCommand"
            OnItemDataBound="gvDirectIncident_ItemDataBound1"
            GroupingEnabled="false" EnableHeaderContextMenu="true"
            AutoGenerateColumns="false">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" TableLayout="Fixed">
                <HeaderStyle Width="102px" />
                <NoRecordsTemplate>
                    <table width="100%" border="0">
                        <tr>
                            <td align="center">
                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </NoRecordsTemplate>

                <Columns>
                    <telerik:GridButtonColumn Text="DoubleClick" CommandName="Edit" Visible="false" />
                    <telerik:GridTemplateColumn HeaderText="Vessel / Office">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="80px"></ItemStyle>

                        <ItemTemplate>
                            <asp:LinkButton ID="lnkVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:LinkButton>
                            <telerik:RadLabel ID="lblVessel" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'
                                Visible="false">
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Reference Number">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="80px"></ItemStyle>

                        <ItemTemplate>
                            <asp:LinkButton ID="lnkReferenceno" runat="server" CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>"
                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPENREPORTREFNO") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Reported">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="50px"></ItemStyle>

                        <ItemTemplate>
                            <telerik:RadLabel ID="lblReportedDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDREPORTEDDATE")) %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblDirectIncidentId" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDDIRECTINCIDENTID")) %>'
                                Visible="false">
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Details">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="80px"></ItemStyle>

                        <ItemTemplate>
                            <telerik:RadLabel ID="lblSummaryFirstLine" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUMMARY").ToString().Length>35 ? DataBinder.Eval(Container, "DataItem.FLDSUMMARY").ToString().Substring(0, 35)+ "..." : DataBinder.Eval(Container, "DataItem.FLDSUMMARY").ToString() %>'></telerik:RadLabel>
                            <eluc:ToolTip ID="ucToolTipSummary" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUMMARY") %>' TargetControlId="lblSummaryFirstLine" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Crew Review Category">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="100px"></ItemStyle>

                        <ItemTemplate>
                            <telerik:RadLabel ID="lblReviewCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVIEWCATEGORYNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Remarks">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="50px"></ItemStyle>

                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString().Length>25 ? DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString().Substring(0, 25)+ "..." : DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString() %>'></telerik:RadLabel>
                            <eluc:ToolTip ID="ucToolTipRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>' TargetControlId="lblRemarks" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Evidence Required" HeaderStyle-Width="50px">
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>

                        <ItemTemplate>
                            <asp:CheckBox ID="chkEvidence" Enabled="false" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.FLDEVIDENCEREQUIRED").ToString().Equals("1") ? true:false %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Assigned to">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="100px"></ItemStyle>

                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAssigned" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENTNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Due">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="50px"></ItemStyle>

                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDueDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDUEDATE"))%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Completion" Visible="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="50px"></ItemStyle>

                        <ItemTemplate>
                            <telerik:RadLabel ID="lblCompletionDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOMPLETIONDATE")) %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Reference number" Visible="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="120px"></ItemStyle>

                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRefno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENUMBER") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Status">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="120px"></ItemStyle>

                        <ItemTemplate>
                            <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Cancel"
                                CommandName="CANCELACTION" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                ToolTip="Cancel">
                                             <span class="icon"><i class="fas fa-times-circle"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Attachment"
                                CommandName="ATTACHMENT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAtt"
                                ToolTip="Attachment">
                                            <span class="icon"><i class="fas fa-paperclip"></i></span>

                            </asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4"/>
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
            </telerik:RadAjaxPanel>
    </form>
</body>
</html>

