<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersAddressAssessmentQuestion.aspx.cs" Inherits="Registers_RegistersAddressAssessmentQuestion" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Address Assessment Question</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
              <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvQuestion.ClientID %>"));
                }, 200);
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
            <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
             <asp:Button runat="server" ID="cmdHiddenSubmit" CssClass="hidden" OnClick="cmdHiddenSubmit_Click" />
            <telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server" Height="95%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <table>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblquestion" Text="Question" runat="server"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtquestion" Text="" Width="200px" runat="server"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblactive" Text="Is active" runat="server"></telerik:RadLabel>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkisactive" Text="" runat="server" />
                        </td>
                    </tr>
                </table>
                <eluc:TabStrip ID="MenuRegistersQuestion" runat="server" OnTabStripCommand="MenuRegistersQuestion_TabStripCommand"></eluc:TabStrip>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvQuestion" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" OnNeedDataSource="gvQuestion_NeedDataSource" Height=""
                    OnItemDataBound="gvQuestion_ItemDataBound"  OnItemCommand="gvQuestion_ItemCommand"              
                    EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true" ShowFooter="false">
                    <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
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
                            <telerik:GridTemplateColumn Visible="false">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblQuestionId" Visible="false" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDQUESTIONID")) %>'></telerik:RadLabel>
                                </ItemTemplate>

                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderStyle-Width="250px" HeaderText="Question">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemStyle HorizontalAlign="Left" Width="300px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblQuestionName" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDQUESTION")) %>'></telerik:RadLabel>
                                </ItemTemplate>

                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Require Remark Y/N" HeaderStyle-Width="110px">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRequireRemark" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDREQUIREREMARK").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                                </ItemTemplate>

                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Active Y/N" HeaderStyle-Width="70px">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblActive" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDISACTIVE").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                                </ItemTemplate>

                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="70px">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Edit" ToolTip="Edit" Width="20PX" Height="20PX"
                                        CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                    </asp:LinkButton>                                 
                                    <asp:LinkButton runat="server" AlternateText="Question Option" ToolTip="Question Option" Width="20PX" Height="20PX"
                                        CommandName="SELECT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdselect">
                                <span class="icon"><i class="fas fa-list-alt"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>

                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </telerik:RadAjaxPanel>
        </div>
    </form>
</body>
</html>
