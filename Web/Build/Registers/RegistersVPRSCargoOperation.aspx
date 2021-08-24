<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersVPRSCargoOperation.aspx.cs" Inherits="RegistersVPRSCargoOperation" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CargoOperationType" Src="~/UserControls/UserControlVPRSCargoOperationType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVPRSVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Cargo Operations</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmVPRSCargoOperations" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="Status" />
            <eluc:TabStrip ID="MenuCargoOperations" runat="server" OnTabStripCommand="CargoOperations_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCargoOperations" Height="93%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvCargoOperations_ItemCommand" OnItemDataBound="gvCargoOperations_ItemDataBound"
                ShowFooter="true" ShowHeader="true" EnableViewState="true" OnNeedDataSource="gvCargoOperations_NeedDataSource" OnUpdateCommand="gvCargoOperations_UpdateCommand"
                OnSortCommand="gvCargoOperations_SortCommand" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDCARGOOPERATIONID">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Cargo Operation" AllowSorting="true" SortExpression="FLDCARGOOPERATION">
                            <HeaderStyle Width="20%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCargoOperationsId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARGOOPERATIONID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCargoOperation" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARGOOPERATION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblCargoOperationsIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARGOOPERATIONID") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtCargoOperationEdit" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARGOOPERATION") %>' runat="server" CssClass="gridinput_mandatory" MaxLength="200"
                                    Width="100%" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtCargoOperationAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="200" Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Short Code">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblShortCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtShortcodeEdit" runat="server" CssClass='input' Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'
                                    Width="100%"></telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtShortcodeAdd" runat="server" CssClass='input' Width="100%"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Cargo Operations Type">
                            <HeaderStyle Width="20%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCargoOperationsTypeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARGOOPERATIONTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:CargoOperationType runat="server" ID="ucCargoOperationTypeEdit" CargoOperationTypeList='<%# PhoenixRegistersCargoOperationType.ListCargoOperationType() %>'
                                    CssClass="dropdown_mandatory" AppendDataBoundItems="true" SelectedCargoOperationType='<%# DataBinder.Eval(Container,"DataItem.FLDCARGOOPERATIONTYPEID") %>' Width="100%" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:CargoOperationType runat="server" ID="ucCargoOperationTypeAdd" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                                    CargoOperationTypeList='<%# PhoenixRegistersCargoOperationType.ListCargoOperationType() %>' Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel Type">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselTypeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:VesselType runat="server" ID="ucVesselTypeEdit" VesselTypeList='<%# PhoenixRegistersVPRSVesselType.ListVesselType() %>'
                                    CssClass="dropdown_mandatory" AppendDataBoundItems="true" SelectedVesselType='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE") %>' Width="100%" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:VesselType runat="server" ID="ucVesselTypeAdd" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                                    VesselTypeList='<%# PhoenixRegistersVPRSVesselType.ListVesselType() %>' Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Value Type">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblValueTypeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALUETYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadDropDownList runat="server" ID="ddlValueTypeEdit" CssClass="dropdown_mandatory" AppendDataBoundItems="true" EnableDirectionDetection="true" Width="100%">
                                    <Items>
                                        <telerik:DropDownListItem Text="--Select--" Value="" />
                                        <telerik:DropDownListItem Text="Date & Time" Value="1" />
                                        <telerik:DropDownListItem Text="Decimal" Value="2" />
                                        <telerik:DropDownListItem Text="Free Text" Value="3" />
                                    </Items>
                                </telerik:RadDropDownList>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadDropDownList runat="server" ID="ddlValueTypeAdd" CssClass="dropdown_mandatory" AppendDataBoundItems="true" EnableDirectionDetection="true" Width="100%">
                                    <Items>
                                        <telerik:DropDownListItem Text="--Select--" Value="" />
                                        <telerik:DropDownListItem Text="Date & Time" Value="1" />
                                        <telerik:DropDownListItem Text="Decimal" Value="2" />
                                        <telerik:DropDownListItem Text="Free Text" Value="3" />
                                    </Items>
                                </telerik:RadDropDownList>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sort Order">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSortOrder" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSORTORDER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtEditSortOrder" runat="server" CssClass="input" MaxLength="5" IsInteger="false" IsPositive="true"
                                    Width="100%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSORTORDER") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtAddSortOrder" runat="server" CssClass="input" MaxLength="5" IsInteger="true" IsPositive="true"
                                    Width="100%" />
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
