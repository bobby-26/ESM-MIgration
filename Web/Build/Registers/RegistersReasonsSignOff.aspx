<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersReasonsSignOff.aspx.cs"
    Inherits="Registers_RegistersReasonsSignOff" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Signoff Reasons</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersReasonsSignoff" autocomplete="off" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuTitle" runat="server" Title="SignOff Reason"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server" Height="95%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <table id="tblConfigureReason" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReason" runat="server" Text="Reason"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSearchReason" runat="server" MaxLength="100" CssClass="input"
                            Width="360px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuRegistersReasonsSignoff" runat="server" OnTabStripCommand="RegistersReasonSignoff_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvReasonSignoff" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvReasonSignoff_ItemCommand" OnNeedDataSource="gvReasonSignoff_NeedDataSource" Height="85%"
                OnItemDataBound="gvReasonSignoff_ItemDataBound" EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true" ShowFooter="true">
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
                        <%--<asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />--%>
                        <telerik:GridTemplateColumn HeaderStyle-Width="200px" HeaderText="Code">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblAbbreviationHeader" runat="server" CommandName="Sort" CommandArgument="FLDABBREVIATION">Code&nbsp;</asp:LinkButton>
                                <%--<img id="FLDABBREVIATION" runat="server" visible="false" />--%>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAbbreviation" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDABBREVIATION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblAbbreviationEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDABBREVIATION") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtAbbreviationEdit" runat="server" Width="98%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDABBREVIATION") %>'
                                    CssClass="gridinput_mandatory" MaxLength="6">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtAbbreviationAdd" runat="server" CssClass="gridinput_mandatory" Width="98%"
                                    MaxLength="6" ToolTip="Enter Abbreviation">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="300px" HeaderText="Reason">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <%--<asp:ImageButton runat="server" ID="cmdHiddenSubmit" OnClick="cmdSearch_Click" CommandName="FLDREASON"
                                        ImageUrl="<%$ PhoenixTheme:images/spacer.png %>"
                                             />--%>
                                <asp:LinkButton ID="lblReasonHeader" runat="server" CommandName="Sort" CommandArgument="FLDREASON">Reason&nbsp;</asp:LinkButton>
                                <%--<img id="FLDREASON" runat="server" visible="false" />--%>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReasonID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREASONID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkReason" runat="server" CommandName="EDIT"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDREASON") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblReasonIDEDIT" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREASONID") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtReasonEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREASON") %>'
                                    CssClass="gridinput_mandatory" Width="98%" MaxLength="200">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtReasonAdd" runat="server" Width="98%" CssClass="gridinput_mandatory" MaxLength="200"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="200px" HeaderText="Group">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblGroupHeader" runat="server" CommandName="Sort" CommandArgument="FLDGROUP">Group&nbsp;</asp:LinkButton>
                                <%--    <img id="FLDGROUP" runat="server" visible="false" />--%>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblGroup" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUP") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblGroupEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUP") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtGroupEdit" runat="server" Width="98%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUP") %>'
                                    CssClass="gridinput_mandatory" MaxLength="50">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtGroupAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="50" Width="98%"
                                    ToolTip="Enter Group">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="70px" HeaderText="Action">
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
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ToolTip="Save" Width="20PX" Height="20PX"
                                    CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" ToolTip="Cancel" Width="20PX" Height="20PX"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel">
                                 <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Add" ToolTip="Add New" Width="20PX" Height="20PX"
                                    CommandName="Add" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd">
                                 <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
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
    </form>
</body>
</html>
