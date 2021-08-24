<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersCargoType.aspx.cs"
    Inherits="RegistersCargoType" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CargoType" Src="~/UserControls/UserControlCargoType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cargo Type</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegisterCargoType" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:TabStrip ID="MenuRegistersCargoType" runat="server" OnTabStripCommand="RegistersCargoType_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCargoType" Height="93%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" EnableHeaderContextMenu="true" GroupingEnabled="false" OnItemCommand="gvCargoType_ItemCommand" OnItemDataBound="gvCargoType_ItemDataBound"
                ShowFooter="true" OnUpdateCommand="gvCargoType_UpdateCommand" OnNeedDataSource="gvCargoType_NeedDataSource"
                ShowHeader="true" EnableViewState="true" OnSortCommand="gvCargoType_SortCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDCARGOTYPECODE">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Cargo Type" AllowSorting="true" SortExpression="FLDCARGOTYPENAME">
                            <HeaderStyle Width="40%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCargoTypeCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARGOTYPECODE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCargoTypeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARGOTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblCargoTypeCodeEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARGOTYPECODE") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtCargoTypeNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARGOTYPENAME") %>'
                                    CssClass="gridinput_mandatory" MaxLength="200" Width="100%"></telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtCargoTypeNameAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="200"
                                    ToolTip="Enter CargoType Name" Width="100%"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Vessel Type">
                            <HeaderStyle Width="30%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselType" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <div style="height: 90px; overflow: auto; border: 1px; color: Black;">
                                    <telerik:RadCheckBoxList ID="ucVesselTypeListEdit" Direction="Vertical" runat="server">
                                    </telerik:RadCheckBoxList>
                                </div>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <div style="height: 90px; overflow: auto; border: 1px; color: Black;">
                                    <telerik:RadCheckBoxList ID="ucVesselTypeListAdd" Direction="Vertical" runat="server">
                                    </telerik:RadCheckBoxList>
                                </div>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Show Y/N">
                            <HeaderStyle Width="20%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="40%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblShowYN" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHOWYESNOINWORD") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox runat="server" ID="chkShowYN" /> <%--BackColor="Transparent" ForeColor="Transparent"--%>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox runat="server" ID="chkShowYN" /> <%--BackColor="Transparent" ForeColor="Transparent"--%>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
