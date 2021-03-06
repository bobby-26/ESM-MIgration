<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersSize.aspx.cs" Inherits="Registers_RegistersSize" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Size</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function DeleteRecord(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmDelete.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersSize" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <table id="tblConfiSize" width="100%">
                <tr>
                    <td width="5%">
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td width="95%">
                        <telerik:RadTextBox ID="txtSearch" runat="server" MaxLength="100" Width="20%"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuRegistersSize" runat="server" OnTabStripCommand="MenuRegistersSize_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" runat="server" Height="88%" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnDeleteCommand="RadGrid1_DeleteCommand" OnSortCommand="RadGrid1_SortCommand" ShowFooter="true"
                OnNeedDataSource="RadGrid1_NeedDataSource" OnPreRender="RadGrid1_PreRender" EnableHeaderContextMenu="true" GroupingEnabled="false"
                OnItemDataBound="RadGrid1_ItemDataBound" OnItemCommand="RadGrid1_ItemCommand" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDSIZEID" TableLayout="Fixed">
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
                        <telerik:GridTemplateColumn HeaderText="Size Name" HeaderStyle-Width="30%" AllowSorting="true" UniqueName="SizeName" SortExpression="FLDSIZENAME">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSizeCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIZEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lnkSizeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIZENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblSizeCodeEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIZEID") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtSizeNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIZENAME") %>'
                                    MaxLength="100">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtSizeNameAdd" runat="server" Width="100%" MaxLength="100" CssClass="gridinput_mandatory"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Description" HeaderStyle-Width="30%" AllowSorting="true" SortExpression="FLDDESCRIPTION">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtSizeDescriptionEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'
                                    CssClass="gridinput" MaxLength="100">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtSizeDescriptionAdd" runat="server" Width="100%" CssClass="gridinput" MaxLength="100"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Active Y/N" HeaderStyle-Width="25%" AllowSorting="true" SortExpression="FLDDESCRIPTION">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActiveYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDACTIVEYN").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkActiveYNEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDACTIVEYN").ToString().Equals("1"))?true:false %>'></asp:CheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:CheckBox ID="chkActiveYNAdd" runat="server"  />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="false" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Add" ID="cmdAdd" ToolTip="Add New">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="425px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <asp:Button ID="ucConfirmDelete" runat="server" OnClick="ucConfirmDelete_Click" CssClass="hidden" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
