<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionDashboardMainFleetRAProcessListExtn.aspx.cs" Inherits="InspectionDashboardMainFleetRAProcessListExtn" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Category" Src="~/UserControls/UserControlRACategoryExtn.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Process RA</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function confirm(args) {
                if (args) {
                    __doPostBack("<%=ucConfirm.UniqueID %>", "");
                }
            } function ConfirmIssue(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmIssue.UniqueID %>", "");
                }
            } function confirmRevision(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmRevision.UniqueID %>", "");
                }
            }
        </script>
        <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvRiskAssessmentProcess.ClientID %>"));
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
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:TabStrip ID="MenuARSubTab" runat="server" TabStrip="true" OnTabStripCommand="MenuARSubTab_TabStripCommand"></eluc:TabStrip>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:TabStrip ID="MenuProcess" runat="server" OnTabStripCommand="MenuProcess_TabStripCommand"></eluc:TabStrip>
            <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvRiskAssessmentProcess" DecoratedControls="All" EnableRoundedCorners="true" />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvRiskAssessmentProcess" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" EnableHeaderContextMenu="true" GroupingEnabled="false" ShowHeader="true" EnableViewState="false"
                OnItemDataBound="gvRiskAssessmentProcess_ItemDataBound" AllowMultiRowSelection="true" AllowFilteringByColumn="true" FilterType="CheckList"
                OnItemCommand="gvRiskAssessmentProcess_ItemCommand" OnNeedDataSource="gvRiskAssessmentProcess_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDRISKASSESSMENTPROCESSID" ClientDataKeyNames="FLDRISKASSESSMENTPROCESSID" AllowFilteringByColumn="true">
                    <ColumnGroups>
                        <telerik:GridColumnGroup Name="Level of Residual Risk" HeaderText="Level of Residual Risk" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
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
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Ref. No" DataField="FLDNUMBER" UniqueName="FLDNUMBER"
                            FilterDelay="2000" HeaderStyle-Width="12%" FilterControlWidth="99%"
                            AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="8%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRefNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER")  %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblReferencid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCEID")  %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblInstallcode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSTALLCODE")  %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRiskAssessmentProcessID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKASSESSMENTPROCESSID")  %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Prepared" HeaderStyle-Width="8%" AllowFiltering="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="8%"></ItemStyle>
                            <ItemTemplate>
                                <%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDDATE"]) %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Process" HeaderStyle-Width="13%" ShowFilterIcon="false" UniqueName="FLDTYPE">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="13%"></ItemStyle>
                            <FilterTemplate>
                                <telerik:RadComboBox ID="ddlRAType" runat="server" AutoPostBack="true" Width="99%" AppendDataBoundItems="true"   OnDataBinding="ddlRAType_DataBinding" OnSelectedIndexChanged="ddlRAType_SelectedIndexChanged" SelectedValue='<%# ViewState["ddlRAType"] %>'>
                                </telerik:RadComboBox>
                            </FilterTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Activity" HeaderStyle-Width="17%" ShowFilterIcon="false" UniqueName="FLDPROCESSNAME" FilterControlWidth="99%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="17%"></ItemStyle>
                            <FilterTemplate>
                                <telerik:RadComboBox ID="ddlCategory" runat="server" Width="99%" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" OnDataBinding="ddlCategory_DataBinding"
                                    AutoPostBack="True"  SelectedValue='<%# ViewState["ddlCategory"] %>' >
                                </telerik:RadComboBox>
                            </FilterTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkJobActivity" runat="server" CommandName="EDITROW" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCESSNAME")  %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="13%" ShowFilterIcon="false" UniqueName="FLDSTATUS" FilterControlWidth="99%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="13%"></ItemStyle>
                            <FilterTemplate>
                                <telerik:RadComboBox ID="ddlStatus" runat="server" AppendDataBoundItems="true" AutoPostBack="true"  Width="99%" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged" SelectedValue='<%# ViewState["ddlStatus"] %>'  >
                                    <Items>
                                        <telerik:RadComboBoxItem Value="Dummy" Text="All"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Value="1" Text="Draft"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Value="3" Text="Approved for Use"></telerik:RadComboBoxItem>
                                    </Items>
                                </telerik:RadComboBox>
                            </FilterTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatusID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS")  %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblActiveyn" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVEYN")  %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rev No" HeaderStyle-Width="6%" HeaderStyle-HorizontalAlign="Center" AllowFiltering="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="6%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRevNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONNO")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Approved" HeaderStyle-Width="8%" AllowFiltering="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="8%"></ItemStyle>
                            <ItemTemplate>
                                <%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDISSUEDDATE"])%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="H&S" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Right" ColumnGroupName="Level of Residual Risk" HeaderStyle-Font-Size="XX-Small" AllowFiltering="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="5%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblHealth" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHSSCORE")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="ENV" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Right" ColumnGroupName="Level of Residual Risk" HeaderStyle-Font-Size="XX-Small" AllowFiltering="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="5%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEnvironmental" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENVSCORE")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="ECO" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Right" ColumnGroupName="Level of Residual Risk" HeaderStyle-Font-Size="XX-Small" AllowFiltering="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="5%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEcononmic" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDECOSCORE")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="WCS" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Right" ColumnGroupName="Level of Residual Risk" HeaderStyle-Font-Size="XX-Small" AllowFiltering="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="5%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWorstCase" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWSSCORE")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Hazards" AllowFiltering="false" HeaderStyle-Width="4%">
                            <HeaderStyle HorizontalAlign="Center" Width="4%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Health and Safety" CommandName="HEALTH" ID="lnkHealth" ToolTip="Health and Safety Hazard" Width="16px">
                                    <span class="icon"><i class="fa-Health"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Enviromental" CommandName="ENV" ID="lnkenv" ToolTip="Enviromental Hazard" Width="16px">
                                    <span class="icon"><i class="fa-Environmental"></i></span>
                                </asp:LinkButton>                                                          
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Controls" AllowFiltering="false" HeaderTooltip="Controls" HeaderStyle-Width="10%">
                            <HeaderStyle HorizontalAlign="Center" Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Equipment" CommandName="COMPONENTS" ID="lnkcomponent" ToolTip="Equipment" Width="16px">
                                    <span class="icon"><i class="fa-PMS"></i></span>
                                </asp:LinkButton>
                                 <asp:LinkButton runat="server" AlternateText="Procedure" CommandName="PROCDURE" ID="lnkprocedure" ToolTip="Procedures" Width="16px">
                                    <span class="icon"><i class="fa-process"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Forms and Checklist" CommandName="FORMS" ID="lnkforms" ToolTip="Forms and Checklist" Width="16px">
                                    <span class="icon"><i class="fa-file-contract-af"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Work Permits" CommandName="WORKPERMIT" ID="lnkWorkPermits" ToolTip="Work Permits" Width="16px">
                                    <span class="icon"><i class="fa-administration"></i></span>
                                </asp:LinkButton> 
                                 <asp:LinkButton runat="server" AlternateText="PPE" CommandName="PPE" ID="lnkPPE" ToolTip="PPE" Width="16px">
                                    <span class="icon"><i class="fa-PPE"></i></span>
                                </asp:LinkButton>                                                               
                                <asp:LinkButton runat="server" AlternateText="EPSS" CommandName="EPSS" ID="lnkEPSS" ToolTip="EPSS" Width="16px">
                                    <span class="icon"><i class="fa-Elog"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Font-Size="XX-Small" AllowFiltering="false">
                            <HeaderStyle HorizontalAlign="Center" Width="13%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Approve" CommandName="APPROVE" ID="imgApprove" ToolTip="Approve">
                                    <span class="icon"><i class="fas fa-user-check"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Revision" CommandName="REVISION" ID="imgrevision" ToolTip="Create Revision">
                                    <span class="icon"><i class="fas fa-registered"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Process PDF" CommandName="REPORT" ID="cmdRAProcess" ToolTip="Show PDF">
                                    <span class="icon"><i class="fas fa-file-pdf"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Revision" CommandName="VIEWREVISION" ID="cmdRevisions" ToolTip="View Revisions">
                                    <span class="icon"><i class="fas fa-eye"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Copy RA to Company" CommandName="OFFICECOPY" ID="imgOfficeCopy" ToolTip="Copy RA to Company">
                                    <span class="icon"><i class="fas fa-file-import"></i></span>
                                </asp:LinkButton>
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
            <asp:Button ID="ucConfirm" runat="server" Text="confirm" OnClick="btnConfirm_Click" />
            <asp:Button ID="ucConfirmIssue" runat="server" Text="confirmIssue" OnClick="btnConfirmIssue_Click" />
            <asp:Button ID="ucConfirmRevision" runat="server" Text="confirmRevision" OnClick="btnConfirmRevision_Click" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
