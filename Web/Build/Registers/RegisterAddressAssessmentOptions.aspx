<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterAddressAssessmentOptions.aspx.cs" Inherits="Registers_RegisterAddressAssessmentOptions" %>

<!DOCTYPE html>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Question Option</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
            <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
            <eluc:TabStrip ID="MenuTitle" runat="server" OnTabStripCommand="MenuTitle_TabStripCommand" Title="Question Option"></eluc:TabStrip>
             <asp:Button runat="server" ID="cmdHiddenSubmit" CssClass="hidden" OnClick="cmdHiddenSubmit_Click" />

            <telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server" Height="95%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <table>
                    <tr>

                        <td>
                            <telerik:RadLabel ID="lblQuestionHeader" runat="server" Text="Question"></telerik:RadLabel>
                        </td>
                        <td style="width: 20px;"></td>
                        <td>
                            <telerik:RadTextBox ID="lblQuestionName" runat="server" Width="400px" CssClass="readonlytextbox"
                                ReadOnly="true" TextMode="MultiLine" Height="40px" Text="">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadTextBox ID="Questionid" Visible="false" runat="server" Width="20px" Text="Questionid"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="Categoryid" runat="server" Visible="false" Width="20px" Text="Categoryid"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
                <eluc:TabStrip ID="MenuRegistersQuestionOptions" runat="server" OnTabStripCommand="MenuRegistersQuestionOptions_TabStripCommand" />

                <telerik:RadGrid RenderMode="Lightweight" ID="gvOption" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" OnNeedDataSource="gvOption_NeedDataSource" Height=""
                    OnItemDataBound="gvOption_ItemDataBound"
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
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lnkQuestionHeader" runat="server" CommandName="Sort" CommandArgument="FLDQUESTIONID">
                                        &nbsp;
                                    </telerik:RadLabel>
                                    <%--<img id="FLDQUESTIONID" runat="server" visible="false" />--%>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblQuestionID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUESTIONID") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                    <telerik:RadLabel ID="lblOptionid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPTIONID") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderStyle-Width="300px" HeaderText="Option">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblOptionionNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDOPTIONNAME" Text="Option"></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblOptionionName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPTIONNAME")%>'></telerik:RadLabel>
                                </ItemTemplate>
                            
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Active Y/N" HeaderStyle-Width="75px">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblActive" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDACTIVEYN").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                                </ItemTemplate>
                             
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderStyle-Width="75px" HeaderText="Action">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblActionHeader" runat="server">
                                        Action
                                    </telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>

                                <ItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Edit" ToolTip="Edit" Width="20PX" Height="20PX"
                                        CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" AlternateText="Delete" ToolTip="Delete" Width="20PX" Height="20PX"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete">
                                <span class="icon"><i class="fas fa-trash"></i></span>
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
            </telerik:RadAjaxPanel>
        </div>
    </form>
</body>
</html>
