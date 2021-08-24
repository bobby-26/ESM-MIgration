<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterMovement.aspx.cs"
    Inherits="RegisterMovement" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PopupMenu" Src="~/UserControls/UserControlPopupMenu.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="GWC" Src="~/UserControls/UserControlGlobalWageComponent.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head2" runat="server">
    <title>Movement</title>
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
    <form id="frmmovementairport" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
            EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadFormDecorator" runat="server" DecorationZoneID="tblConfiguremovement"
            DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"
            Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <telerik:RadNotification ID="ucStatus" RenderMode="Lightweight" runat="server" AutoCloseDelay="1000"
                ShowCloseButton="true" Title="Status" TitleIcon="none" ContentIcon="none" EnableRoundedCorners="true"
                Height="100px" Width="300px" Position="Center" Animation="Fade" ShowTitleMenu="false" Font-Bold="true">
            </telerik:RadNotification>
            <table id="tblConfiguremovement" width="100%">
                <tr>
                    <td width="4%">
                        <telerik:RadLabel ID="lblcode" runat="server" Text="Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCode" runat="server" MaxLength="6" CssClass="input"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtName" runat="server" MaxLength="100" CssClass="input"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblWage" runat="server" Text="Wage Component"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:GWC ID="ucGlobalWageComponentSearch" runat="server" AppendDataBoundItems="true" CssClass="input" Width="200px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblActiveYN" runat="server" Text="Active Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkActive" runat="server"></telerik:RadCheckBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPayableYN" runat="server" Text="Payable Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkpayable" runat="server"></telerik:RadCheckBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuRegistersMovement" runat="server" OnTabStripCommand="RegistersMovement_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvMovement" Height="88%" runat="server"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0"
                GridLines="None" OnItemCommand="gvMovement_ItemCommand" OnItemDataBound="gvMovement_ItemDataBound"
                OnNeedDataSource="gvMovement_NeedDataSource" ShowFooter="True" EnableViewState="true"
                OnSortCommand="gvMovement_SortCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                    AllowNaturalSort="false" AutoGenerateColumns="false" DataKeyNames="FLDMOVEMENTID">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true"
                        ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Code" AllowSorting="true" SortExpression="FLDCODE">
                            <HeaderStyle Width="75px" HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblcode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCODE") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtcodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCODE") %>'
                                    CssClass="gridinput" MaxLength="200" Width="100%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="6"
                                    Width="100%">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="true" SortExpression="FLDNAME">
                            <HeaderStyle Width="100px" HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblmovementid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOVEMENTID") %>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lnkName" runat="server" CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOVEMENTID") %>'>
                                </telerik:RadLabel>
                                <telerik:RadTextBox ID="txtNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'
                                    CssClass="gridinput_mandatory" MaxLength="100" Width="100%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtNameAdd" runat="server" CssClass="gridinput_mandatory"
                                    MaxLength="100" ToolTip="Enter" Width="100%">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Wage Component" AllowSorting="true" SortExpression="">
                            <HeaderStyle Width="130px" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblwagecomponentid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAGECOMPONENTID") %>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblcomponentid" runat="server" CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAGECOMPONENTNAME") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblcomponentId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAGECOMPONENTID") %>'
                                    Visible="false" />
                                <eluc:GWC ID="ucGlobalWageComponentEdit" runat="server" ComponentList='<%# PhoenixRegisterGlobalWageComponent.GloabalWageComponentList(1)%>'
                                    AppendDataBoundItems="true" CssClass="dropdown_mandatory" SelectedComponent='<%# DataBinder.Eval(Container,"DataItem.FLDWAGECOMPONENTID") %>'
                                    Width="100%" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:GWC ID="ucGlobalWageComponent" runat="server" CssClass="dropdown_mandatory"
                                    ComponentList='<%# PhoenixRegisterGlobalWageComponent.GloabalWageComponentList(1)%>'
                                    AppendDataBoundItems="true" Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Formula" AllowSorting="true" SortExpression="FLDFORMULA">
                            <HeaderStyle Width="60px" HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblformula" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMULA") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtformulaEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMULA") %>'
                                    CssClass="gridinput" MaxLength="200" Width="100%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtformulaAdd" runat="server" CssClass="input" MaxLength="200"
                                    Width="100%">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Percentage" AllowSorting="true" SortExpression="FLDPERCENTAGE">
                            <HeaderStyle Width="50px" HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblpercentage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERCENTAGE") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtpercentageEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERCENTAGE") %>'
                                    CssClass="gridinput" MaxLength="200" Width="100%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtpercentageAdd" runat="server" CssClass="input" MaxLength="200"
                                    Width="100%">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Active Y/N" AllowSorting="true" SortExpression="">
                            <HeaderStyle Width="50px" HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActiveYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDISACTIVE").ToString().Equals("1"))?"Yes":"No" %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkActiveYNEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDISACTIVE").ToString().Equals("1"))?true:false %>'>
                                </telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkActiveYNAdd" runat="server">
                                </telerik:RadCheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Payable Y/N" AllowSorting="true" SortExpression="">
                            <HeaderStyle Width="50px" HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPayableYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDISPAYABLE").ToString().Equals("1"))?"Yes":"No" %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkPayableYNEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDISPAYABLE").ToString().Equals("1"))?true:false %>'>
                                </telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkPayableYNAdd" runat="server">
                                </telerik:RadCheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="75px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit"
                                    ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete"
                                    ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Save" ID="cmdSave"
                                    ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel"
                                    ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Add" ID="cmdAdd"
                                    ToolTip="Add New">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox"
                        AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                    AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <asp:Button ID="ucConfirmDelete" runat="server" OnClick="ucConfirmDelete_Click" CssClass="hidden" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
