<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceWOPostponementQuestionFeedback.aspx.cs" 
    Inherits="PlannedMaintenance_PlannedMaintenanceWOPostponementQuestionFeedback" %>

<!DOCTYPE html>

<%@ Import Namespace="Southnests.Phoenix.PlannedMaintenance" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvPostponementFeedback.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = resize;
            function pageLoad() {
                resize();
                fade('statusmessage');
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text="" Visible="false" />
            <eluc:TabStrip ID="MenuFeedback" runat="server" OnTabStripCommand="MenuFeedback_TabStripCommand" TabStrip="false"></eluc:TabStrip>
            <telerik:RadGrid ID="gvPostponementFeedback" runat="server" AllowSorting="false" AllowPaging="false"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvPostponementFeedback_NeedDataSource" OnItemDataBound="gvPostponementFeedback_ItemDataBound"
                GroupingEnabled="false" EnableHeaderContextMenu="true" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
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
                        <telerik:GridTemplateColumn HeaderText="S.No">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemTemplate>
                                <%#Container.DataSetIndex+1 %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Postponement Feedback Questions">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemTemplate>
                                <table cellspacing="10">
                                    <tr>
                                        <td style="font-weight: bold;">
                                            <telerik:RadLabel ID="lblQuestionId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDQUESTIONID")%>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblFeedBackId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDFEEDBACKID")%>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblCommentsyn" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDISCOMMENT")%>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblQuestionName" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDQUESTIONNAME")%>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblOrder" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDORDERNO")%>'>
                                            </telerik:RadLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RadioButtonList ID="rblOptions" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblOptions_SelectedIndexChanged"></asp:RadioButtonList>
                                            <telerik:RadLabel ID="lblCommentsEnable" runat="server" Visible="false"></telerik:RadLabel>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trOptioncomments" visible="false">
                                        <td>Option Comments (If Any)<br />
                                            <telerik:RadTextBox ID="txtOptionComments" Visible="false" runat="server" CssClass="input" TextMode="MultiLine" Resize="Both"
                                                onkeyDown="checkTextAreaMaxLength(this,event,'500');" Width="200px" Height="40px">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trcomments" visible="false">
                                        <td>Question Comments (If Any)<br />
                                            <telerik:RadTextBox ID="txtQuestionComments" Visible="false" runat="server" CssClass="input" TextMode="MultiLine" Resize="Both"
                                                onkeyDown="checkTextAreaMaxLength(this,event,'500');" Width="400px" Height="60px">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="false" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="200px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
