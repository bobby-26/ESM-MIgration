<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersCargo.aspx.cs" Inherits="RegistersCargo" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CargoType" Src="~/UserControls/UserControlCargoType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cargo </title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegisterCargo" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <table id="tblCargo" width="100%">
                <tr>
                    <td width="7%">
                        <telerik:RadLabel ID="RadlblCargoName" runat="server" Text="Cargo Name"></telerik:RadLabel>
                    </td>
                    <td width="28.3%">
                        <telerik:RadTextBox ID="txtCargoName" runat="server" CssClass="input" Width="45%"></telerik:RadTextBox>
                    </td>
                    <td width="6%">
                        <telerik:RadLabel ID="RadlblCargoType" runat="server" Text="Cargo Type"></telerik:RadLabel>
                    </td>
                    <td width="27.3%">
                        <eluc:CargoType ID="ucCargo" runat="server" AppendDataBoundItems="true" AutoPostBack="true" CssClass="input" Width="50%" />
                    </td>
                    <td width="6%">
                        <telerik:RadLabel ID="RadlblVesselType" runat="server" Text="Vessel Type"></telerik:RadLabel>
                    </td>
                    <td width="27.3%">
                        <eluc:Hard ID="ucVesselTypeSearch" AppendDataBoundItems="true" HardTypeCode="81" runat="server" CssClass="input" Width="50%" />
                        <%--<eluc:VesselType runat="server" ID="ucVesselTypeSearch" CssClass="input" AutoPostBack="true" AppendDataBoundItems="true" />--%>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuRegistersCargo" runat="server" OnTabStripCommand="RegistersCargo_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCargo" Height="88%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvCargo_ItemCommand" OnItemDataBound="gvCargo_ItemDataBound" OnUpdateCommand="gvCargo_UpdateCommand"
                ShowFooter="true" ShowHeader="true" EnableViewState="true" OnNeedDataSource="gvCargo_NeedDataSource" OnSortCommand="gvCargo_SortCommand" 
                EnableHeaderContextMenu="true" GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDCARGOCODE">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Short Name" AllowSorting="true" SortExpression="FLDSHORTNAME">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCargoCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARGOCODE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCargoShortName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblCargoCodeEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARGOCODE") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtCargoShortNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTNAME") %>'
                                    CssClass="gridinput_mandatory" MaxLength="200" Width="100%"></telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtCargoShortNameAdd" runat="server" CssClass="gridinput_mandatory"
                                    MaxLength="200" ToolTip="Enter Cargo Short Name" Width="100%"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="true" SortExpression="FLDCARGONAME">
                            <HeaderStyle Width="30%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCargoName" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARGONAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtCargoNameEdit" runat="server" CssClass="gridinput_mandatory"
                                    MaxLength="200" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARGONAME") %>' Width="100%"></telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtCargoNameAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="200"
                                    ToolTip="Enter Cargo Name" Width="100%"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Cargo Type">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCargoType" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARGOTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:CargoType ID="ucCargoTypeEdit" runat="server" CargoTypeList='<%#PhoenixRegistersCargo.ListCargoType(1)%>'
                                    CssClass="gridinput_mandatory" AppendDataBoundItems="true" Width="100%" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:CargoType ID="ucCargoType1" runat="server" CargoTypeList='<%#PhoenixRegistersCargo.ListCargoType(1)%>'
                                    CssClass="gridinput_mandatory" AppendDataBoundItems="true" Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Vessel Type">
                            <HeaderStyle Width="30%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselType" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <%--<eluc:Hard id="ucVesselTypeEdit" hardlist='<%# PhoenixRegistersHard.ListHard(1,81)%>'
                                        selectedhard='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE") %>' appenddatabounditems="true"
                                        hardtypecode="81" runat="server" cssclass="dropdown_mandatory" />--%>
                                <%--<eluc:VesselType runat="server" ID="ucVesselTypeEdit" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                         VesselTypeList='<%# PhoenixRegistersVesselType.ListVesselType(1) %>' />--%>
                                <div style="height: 90px; overflow: auto; border: 1px; color: Black;">
                                    <telerik:RadCheckBoxList ID="ucVesselTypeListEdit" RepeatDirection="Vertical" runat="server">
                                    </telerik:RadCheckBoxList>
                                </div>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <%--<eluc:Hard id="ucVesselTypeAdd" appenddatabounditems="true" hardlist='<%# PhoenixRegistersHard.ListHard(1,81)%>'
                                        hardtypecode="81" runat="server" cssclass="dropdown_mandatory" />--%>
                                <%--<eluc:VesselType runat="server" ID="" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                         VesselTypeList='<%# PhoenixRegistersVesselType.ListVesselType(1) %>' />--%>
                                <div style="height: 90px; overflow: auto; border: 1px; color: Black;">
                                    <telerik:RadCheckBoxList ID="ucVesselTypeListAdd" RepeatDirection="Vertical" runat="server">
                                    </telerik:RadCheckBoxList>
                                </div>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
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
