<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersPniPOTypeMapping.aspx.cs" Inherits="Registers_RegistersPniPOTypeMapping" %>

<!DOCTYPE html>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PageNumber" Src="~/UserControls/UserControlPageNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Company</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersCompany" runat="server" >
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <%--<telerik:RadSkinManager ID="RadSkinManager1" runat="server" />--%>
     
        
      
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table id="tblConfigureCompany">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblselect" runat="server" MaxLength="100" Text="Select the Type">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="cmbtype" runat="server" MaxLength="100">
                            <Items>
                                <telerik:RadComboBoxItem Text="Off Signer" Value="0" />
                                <telerik:RadComboBoxItem Text="On Signer" Value="1" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>

                </tr>
            </table>
            <eluc:TabStrip ID="MenuRegistersPNI" runat="server" OnTabStripCommand="MenuRegistersPNI_TabStripCommand"></eluc:TabStrip>
        
                <telerik:RadGrid RenderMode="Lightweight" ID="gvpnimapping" runat="server" Height="500px" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" EnableViewState="false"
                    OnNeedDataSource="gvpnimapping_NeedDataSource"
                    OnItemCommand="gvpnimapping_ItemCommand"
                    OnItemDataBound="gvpnimapping_ItemDataBound"
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
                            <telerik:GridTemplateColumn HeaderText="Type" AllowSorting="true">
                                <HeaderStyle Width="45%" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lbldtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lbltype" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <eluc:Quick ID="cmdmappingtypeadd" runat="server" QuickTypeCode="176" />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Heading" ColumnGroupName="Name" AllowSorting="true" ShowSortIcon="true">
                                <HeaderStyle Width="25%" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblsubtypeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBTYPE") %>'></telerik:RadLabel>

                                    <telerik:RadLabel ID="lblsubtype" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadComboBox ID="cmbsubtypeadd" EmptyMessage="Type to select" Filter="Contains" DataTextField="FLDPARTNAME" DataValueField="FLDID" runat="server" Width="100%"></telerik:RadComboBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="On/Off Signer" AllowSorting="true">
                                <HeaderStyle Width="20%" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblsigner" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNERYN") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadComboBox ID="cmbsigneradd" runat="server" EmptyMessage="Type to select" Filter="Contains">
                                        <Items>
                                           
                                            <telerik:RadComboBoxItem Text="Off Signer" Value="0" />
                                            <telerik:RadComboBoxItem Text="On Signer" Value="1" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action" ColumnGroupName="Action" AllowSorting="true" SortExpression="">
                                <HeaderStyle Width="10%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>

                                    <asp:LinkButton runat="server" AlternateText="Edit" ID="cmdEdit" ToolTip="Edit" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" AlternateText="Delete" ID="cmdDelete" CommandName="DELETE" ToolTip="Delete" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Add" ID="cmdAdd" CommandName="ADD" ToolTip="Add" Width="20px" Height="20px">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                    </asp:LinkButton>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="415px" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
          
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
     
    </form>
</body>
</html>

