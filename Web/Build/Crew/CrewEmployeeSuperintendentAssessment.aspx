<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewEmployeeSuperintendentAssessment.aspx.cs"
    Inherits="CrewEmployeeSuperintendentAssessment" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>OC 28</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
         <script type="text/javascript">
function Resize() {
setTimeout(function () {
TelerikGridResize($find("<%= gvOC28.ClientID %>"));
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
    <form id="frmSupdtConcerns" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>                  
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />     
        <eluc:TabStrip ID="MainMenuSupdtConcerns" runat="server" TabStrip="true" OnTabStripCommand="MainMenuSupdtConcerns_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%" EnableAJAX="false">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" />
        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtMiddleName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtLastName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblEmployeeNumber" runat="server" Text="File No."></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                </td>
                <td colspan="3">
                    <telerik:RadTextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                </td>
            </tr>
        </table>

        <eluc:TabStrip ID="MenuSupdtConcerns" runat="server" OnTabStripCommand="MenuSupdtConcerns_TabStripCommand"></eluc:TabStrip>
       
        <telerik:RadGrid RenderMode="Lightweight" ID="gvOC28" runat="server" EnableViewState="false" 
            AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
            OnNeedDataSource="gvOC28_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvOC28_ItemDataBound"
            OnItemCommand="gvOC28_ItemCommand" ShowFooter="false" AutoGenerateColumns="false">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Wrap="false" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
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
                    <telerik:GridTemplateColumn HeaderText="Vessel" AllowSorting="false" ShowSortIcon="true">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblVessel" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblVesselId" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDSIGNONVESSELID") %>'
                                Visible="false">
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Vessel Type" AllowSorting="false" ShowSortIcon="true">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblVslType" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDTYPEDESCRIPTION") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblRank" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>'
                                Visible="false">
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Date Joined" AllowSorting="false" ShowSortIcon="true">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblJoinedDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATEOFJOINING")) %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Tentative S/off" AllowSorting="false" ShowSortIcon="true">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblSignoffDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDSIGNOFFDATE")) %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblSignonOffId" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDSIGNONOFFID") %>'
                                Visible="false">
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Assessment Date" AllowSorting="false" ShowSortIcon="true">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAssessmentDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDASSESSMENTDATE")) %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblAssessmentId" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDASSESSMENTID") %>'
                                Visible="false">
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Score" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="5%">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblScore" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALSCORE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Supt. Score" AllowSorting="false" ShowSortIcon="true">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblSuptScore" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALSUPTSCORE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Difference" AllowSorting="false" ShowSortIcon="true">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblScoreVarience" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCOREVARIENCE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Supt." AllowSorting="false" ShowSortIcon="true">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblSupt" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPTNAME") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblSuptId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPTID") %>'
                                Visible="false">
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Supt. Comments" AllowSorting="false" ShowSortIcon="true">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPTCOMMENTSTOOLTIP") %>'></telerik:RadLabel>
                            <eluc:ToolTip runat="server" ID="ucRemarks" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPTCOMMENTS") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="" HeaderStyle-Width="5%">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Export XL" ID="cmdCrewExport2XL" ToolTip="MST/CE Assessment" CommandName="CREWEXPORT2XL" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-file-excel"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Export XL" ID="cmdSuptExport2XL" CommandName="SUPTEXPORT2XL" ToolTip="Supt Assessment" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-file-excel"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Send Email" ID="cmdEmail" ToolTip="Send Email" CommandName="EMAIL" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-envelope"></i></span>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
          </telerik:RadAjaxPanel>
        <eluc:Status runat="server" ID="ucStatus" />
    </form>
</body>
</html>
