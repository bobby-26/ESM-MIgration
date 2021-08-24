<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionStandardNonRoutineRA.aspx.cs" Inherits="InspectionStandardNonRoutineRA" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselByOwner.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleet.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title>Non Routine RA</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function ConfirmApprove(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmApprove.UniqueID %>", "");
                }
            } function ConfirmRevision(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmRevision.UniqueID %>", "");
                }
            }
        </script>
        <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvRiskAssessment.ClientID %>"));
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
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:TabStrip ID="MenuGeneric" runat="server" OnTabStripCommand="MenuGeneric_TabStripCommand"></eluc:TabStrip>
            <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvRiskAssessment" DecoratedControls="All" EnableRoundedCorners="true" />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvRiskAssessment" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" EnableHeaderContextMenu="true" GroupingEnabled="false" ShowHeader="true" EnableViewState="false" AllowMultiRowSelection="true" AllowFilteringByColumn="true"
                OnItemDataBound="gvRiskAssessment_ItemDataBound" OnItemCommand="gvRiskAssessment_ItemCommand" OnNeedDataSource="gvRiskAssessment_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDRISKASSESSMENTID" AllowFilteringByColumn="True" ClientDataKeyNames="FLDRISKASSESSMENTID">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Ref. No" HeaderStyle-Width="10%" DataField="FLDNUMBER" UniqueName="FLDNUMBER" FilterControlWidth="99%"
                            FilterDelay="2000" AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRefNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER")  %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblInstallcode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSTALLCODE")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel" Visible="false">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID")  %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lbltypeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEID")  %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsCreatedByOffice" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISCREATEDBYOFFICE")  %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Prepared" HeaderStyle-Width="10%" AllowFiltering="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblprepared" runat="server" Text='<%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDDATE"])%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Intended <br> Work" HeaderStyle-Width="8%" AllowSorting="true" SortExpression="FLDINTENDEDWORKDATE" ShowSortIcon="true" Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIntended" runat="server" Text='<%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDINTENDEDWORKDATE"])%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type" HeaderStyle-Width="10%" ShowFilterIcon="false" UniqueName="FLDTYPE" FilterControlWidth="99%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <FilterTemplate>
                                <telerik:RadComboBox ID="ddlRAType" runat="server" AutoPostBack="true" Width="99%" AppendDataBoundItems="true" OnDataBinding="ddlRAType_DataBinding"
                                    OnSelectedIndexChanged="ddlRAType_SelectedIndexChanged" SelectedValue='<%# ViewState["ddlRAType"] %>'>
                                </telerik:RadComboBox>
                            </FilterTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Activity" HeaderStyle-Width="25%" FilterControlWidth="99%" ShowFilterIcon="false" Visible="false"
                            UniqueName="FLDACTIVITYNAME" FilterDelay="2000" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <FilterTemplate>
                                <telerik:RadComboBox ID="ddlCategory" runat="server" Width="99%" AppendDataBoundItems="true" 
                                    OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" OnDataBinding="ddlCategory_DataBinding"
                                    AutoPostBack="True" SelectedValue='<%# ViewState["ddlCategory"] %>'>
                                </telerik:RadComboBox>
                            </FilterTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActivity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVITYNAME")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Activity / Conditions" HeaderStyle-Width="35%" ShowFilterIcon="false" FilterControlWidth="99%"
                            UniqueName="FLDACTIVITYCONDITIONS" DataField="FLDACTIVITYCONDITIONS" FilterDelay="2000" CurrentFilterFunction="Contains" AutoPostBackOnFilter="false">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRiskAssessmentGenericID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKASSESSMENTID")  %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblRevisionno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONNO")  %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <asp:LinkButton ID="lnkJobActivity" runat="server" CommandName="EDITROW" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVITYCONDITIONS")  %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Target Date <br> for completion" HeaderStyle-Wrap="true" Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTarget" runat="server" Text='<%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDCOMPLETIONDATE"])%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Task <br> completed YN" HeaderStyle-Width="10%" HeaderStyle-Wrap="true" Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMitigating" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTASKSTATUS")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rev No" HeaderStyle-Width="10%" AllowFiltering="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRevno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONNO")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="15%" Visible="false">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblStatusHeader" runat="server">Status</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS")  %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblStatusName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="15%" AllowFiltering="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="15%"></ItemStyle>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Revision" CommandName="REVISION" ID="imgrevision" ToolTip="Create Revision">
                                    <span class="icon"><i class="fas fa-registered"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Report" CommandName="RAGENERIC" ID="cmdRAGeneric" ToolTip="Show PDF">
                                    <span class="icon"><i class="fas fa-file-pdf"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Revision" CommandName="VIEWREVISION" ID="cmdRevision" ToolTip="View Revisions">
                                    <span class="icon"><i class="fas fa-eye"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Copy" CommandName="COPYTEMPLATE" ID="imgCopyTemplate" ToolTip="Copy as Non Routine RA">
                                    <span class="icon"><i class="fas fa-copy"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                   <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="2" EnableColumnClientFreeze="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <asp:Button ID="ucConfirmApprove" runat="server" Text="confirmApprove" OnClick="btnConfirmApprove_Click" />
            <asp:Button ID="ucConfirmRevision" runat="server" Text="confirmRevision" OnClick="btnConfirmRevision_Click" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
