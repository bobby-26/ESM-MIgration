<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionQuestions.aspx.cs" Inherits="InspectionQuestions" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Confirm(args) {
                if (args) {
                    __doPostBack("<%=ucConfirm.UniqueID %>", "");
                }
            }
        </script>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvQuestions.ClientID %>"));
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
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <asp:Button ID="ucConfirm" runat="server" Text="confirm" OnClick="ucConfirm_Click" />
            <eluc:TabStrip ID="MenuQuestions" runat="server" OnTabStripCommand="MenuQuestions_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvQuestions" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvQuestions_ItemCommand" OnNeedDataSource="gvQuestions_NeedDataSource" ShowHeader="true"
                OnItemDataBound="gvQuestions_ItemDataBound" EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true" ShowFooter="false"
                OnSortCommand="gvQuestions_SortCommand">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="true">
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
                        <telerik:GridTemplateColumn AllowSorting="true" ShowSortIcon="true" SortExpression="FLDDMSQUESTIONID"
                            HeaderText="Questions">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Wrap="true" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblQuestionid" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDDMSQUESTIONID")) %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblQuestion" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDDMSQUESTION")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowSorting="true" ShowSortIcon="true" SortExpression="FLDSORTORDER"
                            HeaderText="Sort Order">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Wrap="true" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSortOrder" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDSORTORDER")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowSorting="true" ShowSortIcon="true" SortExpression="FLDACTIVEYN"
                            HeaderText="ActiveYN">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Wrap="true" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadCheckBox ID="cbActiveyn" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.FLDACTIVEYN").ToString().Equals("1") ? true:false %>' Enabled="false" ></telerik:RadCheckBox>                                
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" ToolTip="Edit" Width="20PX" Height="20PX"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" ToolTip="Delete" Width="20PX" Height="20PX"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Answer List" CommandName="ANSWER" ID="cmdAnswer" ToolTip="Options">
                                    <span class="icon"><i class="fa-Options" ></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="3" EnableColumnClientFreeze="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
