
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersDMRMeteorology.aspx.cs"
    Inherits="RegistersDMRMeteorology" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>DMR Meteorology</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmDMRMeteorology" runat="server">
    <telerik:RadScriptManager ID="radscript1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadAjaxPanel ID="panel1" runat="server">
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
            width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuDMRMeteorology" runat="server" OnTabStripCommand="MenuDMRMeteorology_TabStripCommand">
            </eluc:TabStrip>
            <%--< ShowFooter="true"
                    ShowHeader="true" EnableViewState="false">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvDMRMeteorology" runat="server" Height="500px"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0"
                GridLines="None" EnableViewState="false" OnNeedDataSource="gvDMRMeteorology_NeedDataSource"
                OnItemDataBound="gvDMRMeteorology_ItemDataBound" OnItemCommand="gvDMRMeteorology_ItemCommand"
                OnSortCommand="gvDMRMeteorology_SortCommand" GroupingEnabled="false" EnableHeaderContextMenu="true"
                AutoGenerateColumns="false" ShowFooter="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                    HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <NoRecordsTemplate>
                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger"
                            Font-Bold="true">
                        </telerik:RadLabel>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Short name" FooterText="New Short">

                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="15%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblShortName" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTNAME") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtShortNameEdit" runat="server" CssClass="gridinput_mandatory"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTNAME") %>' Width="50%" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtShortNameAdd" runat="server" CssClass="gridinput_mandatory"
                                    MaxLength="10" Width="50%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Meteorology name" FooterText="New Dmr">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMeteorologyId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMETEOROLOGYID") %>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblMeteorologyName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMETEOROLOGYNAME") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblMeteorologyIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMETEOROLOGYID") %>'>
                                </telerik:RadLabel>
                                <telerik:RadTextBox ID="txtMeteorologyNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMETEOROLOGYNAME") %>'
                                    CssClass="gridinput_mandatory" MaxLength="200" Width="90%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtMeteorologyNameAdd" runat="server" CssClass="gridinput_mandatory"
                                    MaxLength="200" Width="90%">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Min Value">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMinValue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMINVALUE") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtMinValueEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMINVALUE") %>'
                                    CssClass="input" MaxLength="5" Width="80px" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtMinValueAdd" runat="server" CssClass="input" MaxLength="5" Width="80px" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Max Value">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMaxValue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAXVALUE") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtMaxValueEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMAXVALUE") %>'
                                    CssClass="input" MaxLength="5" Width="80px" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtMaxValueAdd" runat="server" CssClass="input" MaxLength="5" Width="80px" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sort Order" FooterText="Sort Order">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSortOrder" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSORTORDER") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtSortOrderEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSORTORDER") %>'
                                    CssClass="input_mandatory" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtSortOrderAdd" runat="server" CssClass="input_mandatory" />
                                <%--<telerik:RadTextBox ID="txtSortOrderAdd2" runat="server" CssClass="gridinput_mandatory"  MaxLength="200"></telerik:RadTextBox>--%>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Value type name">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblValueTypeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALUETYPENAME") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadComboBox runat="server" ID="ddlValueTypeEdit" CssClass="dropdown_mandatory"
                                    AppendDataBoundItems="true">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="--Select--" Value=""></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="Decimal" Value="1"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="Condition" Value="2"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="Direction" Value="3"></telerik:RadComboBoxItem>
                                    </Items>
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadComboBox runat="server" ID="ddlValueTypeAdd" CssClass="dropdown_mandatory"
                                    AppendDataBoundItems="true">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="--Select--" Value=""></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="Decimal" Value="1"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="Condition" Value="2"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="Direction" Value="3"></telerik:RadComboBoxItem>
                                    </Items>
                                </telerik:RadComboBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit"
                                    ToolTip="Edit">
                                         <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete"
                                    ToolTip="Delete">
                                         <span class="icon"><i class="fa fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdSave"
                                    ToolTip="Save">
                                        <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel"
                                    ToolTip="Cancel">
                                        <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ToolTip="Add New" CommandName="Add"
                                    ID="cmdAdd">
                                        <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                    AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4"
                        ScrollHeight="415px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </div>
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
