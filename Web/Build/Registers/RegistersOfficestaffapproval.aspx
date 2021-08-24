<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersOfficestaffapproval.aspx.cs"
    Inherits="RegistersOfficestaffapproval" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersCity" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuOfficestaff" runat="server" OnTabStripCommand="MenuOfficestaff_TabStripCommand" TabStrip="True"></eluc:TabStrip>
        <eluc:TabStrip ID="MenuTitle" runat="server" Title="Approver List"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="85%">
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table width="100%">
                <tr>
                    <td>File No.</td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployeeNo" runat="server" ReadOnly="true" Text="" Width="98%" CssClass="readonlytextbox"></telerik:RadTextBox></td>
                    <td>User Name</td>
                    <td>
                        <telerik:RadTextBox ID="txtUsername" runat="server" ReadOnly="true" Text="" CssClass="readonlytextbox" Width="98%"></telerik:RadTextBox></td>
                    <td>Salutation</td>
                    <td>
                        <telerik:RadTextBox ID="txtSalutation" runat="server" ReadOnly="true" Text="" Width="98%" CssClass="readonlytextbox"></telerik:RadTextBox></td>

                </tr>
                <tr>
                    <td>First Name</td>
                    <td>
                        <telerik:RadTextBox ID="txtfirstname" runat="server" ReadOnly="true" Text="" CssClass="readonlytextbox" Width="98%"></telerik:RadTextBox></td>
                    <td>Middle Name</td>
                    <td>
                        <telerik:RadTextBox ID="txtmiddlename" runat="server" ReadOnly="true" Text="" CssClass="readonlytextbox" Width="98%"></telerik:RadTextBox></td>
                    <td>last Name</td>
                    <td>
                        <telerik:RadTextBox ID="txtLastname" runat="server" ReadOnly="true" Text="" CssClass="readonlytextbox" Width="98%"></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td>Type</td>
                    <td>
                        <telerik:RadComboBox ID="ddlAddType" runat="server" AutoPostBack="true" AppendDataBoundItems="true" Width="98%"
                             Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                        </telerik:RadComboBox>
                    </td>
                </tr>
            </table>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewComponentTrack" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="false"
                CellSpacing="0" GridLines="None" OnItemCommand="gvCrewComponentTrack_ItemCommand" OnNeedDataSource="gvCrewComponentTrack_NeedDataSource"
                OnItemDataBound="gvCrewComponentTrack_ItemDataBound" EnableViewState="false" ShowFooter="true" EnableHeaderContextMenu="true">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDCONFIGURATIONID">
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
                        <telerik:GridTemplateColumn HeaderText="Approver">
                            <ItemStyle Wrap="false" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblapproverstaffid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVERID") %>'></telerik:RadLabel>
                                <%# DataBinder.Eval(Container, "DataItem.FLDAPPROVERNAME")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListOfficeStaffedit">
                                    <telerik:RadTextBox ID="txtEditStaffName" runat="server" CssClass="input_mandatory" MaxLength="200"
                                        Width="80%" Text='<%# DataBinder.Eval(Container, "DataItem.FLDAPPROVERNAME")%>'>
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtEditStaffId" runat="server" CssClass="hidden" Enabled="false" MaxLength="50"
                                        Width="5px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVERID") %>'>
                                    </telerik:RadTextBox>
                                    <asp:ImageButton runat="server" ID="imgEdituser" Style="cursor: pointer; vertical-align: top"
                                        ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" OnClientClick="return showPickList('spnPickListOfficeStaffedit', 'codehelp1', '', '../Common/CommonPickListOfficeStaff.aspx', true); "
                                        ToolTip="Approver" />
                                </span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <span id="spnPickListOfficeStaff">
                                    <telerik:RadTextBox ID="txtAddStaffName" runat="server" CssClass="input_mandatory" MaxLength="200"
                                        Width="80%">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtAddStaffId" runat="server" CssClass="hidden" Enabled="false" MaxLength="50"
                                        Width="5px">
                                    </telerik:RadTextBox>
                                    <asp:ImageButton runat="server" ID="imgadduser" Style="cursor: pointer; vertical-align: top"
                                        ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" OnClientClick="return showPickList('spnPickListOfficeStaff', 'codehelp1', '', '../Common/CommonPickListOfficeStaff.aspx', true); "
                                        ToolTip="Approver" />
                                </span>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Level">
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container,"DataItem.FLDLEVEL") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadComboBox ID="ddleditlevel" runat="server" EmptyMessage="--select--"
                                    Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True" >
                                    <Items>
                                        <telerik:RadComboBoxItem Text="--Select--" Value="0"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="1" Value="1"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="2" Value="2"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="3" Value="3"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="4" Value="4"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="5" Value="5"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="6" Value="6"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="7" Value="7"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="8" Value="8"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="9" Value="9"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="10" Value="10"></telerik:RadComboBoxItem>
                                    </Items>
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadComboBox ID="ddlAddLevel" runat="server" EmptyMessage="--select--"
                                    Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True" >
                                    <Items>
                                        <telerik:RadComboBoxItem Text="--Select--" Value="0"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="1" Value="1"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="2" Value="2"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="3" Value="3"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="4" Value="4"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="5" Value="5"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="6" Value="6"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="7" Value="7"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="8" Value="8"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="9" Value="9"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="10" Value="10"></telerik:RadComboBoxItem>
                                    </Items>
                                </telerik:RadComboBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
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
