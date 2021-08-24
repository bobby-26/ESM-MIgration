<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsSoaCheckingVoucherQuery.aspx.cs" Inherits="Accounts_AccountsSoaCheckingVoucherQuery" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Voucher Query</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSearchResults" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">

            <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">

                <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

                <div id="divGrid" style="position: relative; z-index: 0; overflow-x: scroll; overflow-y: hidden;">
                    <%-- <asp:GridView ID="gvOperations" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvOperations_RowCommand" OnRowDataBound="gvOperations_ItemDataBound"
                        OnRowCreated="gvOperations_RowCreated" ShowFooter="true" ShowHeader="true"
                        EnableViewState="false" AllowSorting="true" OnSelectedIndexChanging="gvOperations_SelectedIndexChanging"
                        OnSorting="gvOperations_Sorting">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvOperations" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvOperations_NeedDataSource"
                        OnItemCommand="gvOperations_ItemCommand"
                        OnItemCreated="gvOperations_ItemCreated"
                        OnItemDataBound="gvOperations_ItemDataBound1"
                        GroupingEnabled="false" EnableHeaderContextMenu="true"
                        AutoGenerateColumns="false" ShowFooter="true">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" TableLayout="Fixed">
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
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Question">

                                    <ItemTemplate>
                                        <asp:LinkButton ID="lblDescription" runat="server" CommandName="SELECT" CommandArgument="<%# Container.DataSetIndex %>" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox ID="txtDescriptionEdit" runat="server" TextMode="MultiLine" Width="250px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>' CssClass="input_mandatory"></telerik:RadTextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadTextBox ID="txtDescriptionAdd" runat="server" TextMode="MultiLine" Width="250px" CssClass="input_mandatory"></telerik:RadTextBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Raised By">

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblVoucherId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblQueryId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUERYID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblRaisedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRAISEDBY") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Raised Date">

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblRaisedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRAISEDDATE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Status">

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Actions">
                                
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                            CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                            ToolTip="Edit" Visible="false"></asp:ImageButton>
                                        <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Cancel Query" ImageUrl="<%$ PhoenixTheme:images/ticket_cancel.png %>"
                                            CommandName="CANCELTCKT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancelquery"
                                            ToolTip="Cancel Query" Visible="false"></asp:ImageButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                            CommandName="UPDATE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                            ToolTip="Save"></asp:ImageButton>
                                        <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="CANCEL" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                            ToolTip="Cancel"></asp:ImageButton>
                                    </EditItemTemplate>
                                    <FooterStyle HorizontalAlign="Center" />
                                    <FooterTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                            CommandName="OPERATIONSADD" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
                                            ToolTip="Add New"></asp:ImageButton>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </div>

                <br />
                <br />
                <div id="divCourse" style="position: relative; z-index: +1; overflow-x: scroll; overflow-y: hidden;">
                    <%-- <asp:GridView ID="gvDeck" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowDataBound="gvDeck_RowDataBound" OnRowCommand="gvDeck_RowCommand"
                        Style="margin-bottom: 0px" OnSelectedIndexChanging="gvDeck_SelectedIndexChanging"
                        EnableViewState="false" ShowFooter="true">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvDeck" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvDeck_NeedDataSource"
                        OnItemCommand="gvDeck_ItemCommand"             OnItemDataBound="gvDeck_ItemDataBound"
                        GroupingEnabled="false" EnableHeaderContextMenu="true"
                        AutoGenerateColumns="false" ShowFooter="true">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" TableLayout="Fixed">
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
                            <Columns>

                                <telerik:GridTemplateColumn HeaderText="Answer">
                                   
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblQueryAnswerId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUERYANSWERID") %>' Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                                </itemtemplate>
                                    <edititemtemplate>
                                    <telerik:RadLabel ID="lblQueryAnswerIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUERYANSWERID") %>' Visible="false"></telerik:RadLabel>
                                    <telerik:RadTextBox ID="txtDescriptionEdit" runat="server" TextMode="MultiLine" Width="250px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>' CssClass="input_mandatory"></telerik:RadTextBox>
                                </edititemtemplate>
                                    <footertemplate>
                                    <telerik:RadTextBox ID="txtDescriptionAdd" runat="server" TextMode="MultiLine" Width="250px" CssClass="input_mandatory"></telerik:RadTextBox>
                                </footertemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Answered By">
                                   
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblAnsweredBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDANSWEREDBYNAME") %>'></telerik:RadLabel>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Answered Date">
                               
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblAnsweredDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDANSWEREDDATE") %>'></telerik:RadLabel>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Responed By">
                                
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblResponedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRESPONDEDBY") %>'></telerik:RadLabel>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Re-open Remarks">
                                  
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblReOpenRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREOPENREMARKS") %>'></telerik:RadLabel>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Action">
                                
                                    <itemstyle wrap="False" horizontalalign="Center" width="40px"></itemstyle>
                                    <itemtemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Reopen" ImageUrl="<%$ PhoenixTheme:images/refresh.png %>"
                                        CommandName="REOPEN" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdReopen"
                                        ToolTip="Reopen"></asp:ImageButton>
                                    <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Close" ImageUrl="<%$ PhoenixTheme:images/completed.png %>"
                                        CommandName="CLOSE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdClose"
                                        ToolTip="Close"></asp:ImageButton>
                                </itemtemplate>
                                    <edititemtemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="UPDATE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="CANCEL" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </edititemtemplate>
                                    <footerstyle horizontalalign="Center" />
                                    <footertemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                        CommandName="DECKADD" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
                                        ToolTip="Add New"></asp:ImageButton>
                                </footertemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </div>

                <br />
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
