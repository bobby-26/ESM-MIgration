<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselpositionIMODCSRevisedConsumption.aspx.cs" Inherits="VesselpositionIMODCSRevisedConsumption" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="OilType" Src="~/UserControls/UserControlOilType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Oil Type</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegisterOilType" runat="server">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="panel1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />

        <telerik:RadAjaxPanel runat="server" ID="pnlOilType" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="MenuFuelTypeMain" Title="Record of revision of Fuel Oil Consumption Data Collection Plan" runat="server" OnTabStripCommand="MenuFuelTypeMain_TabStripCommand"></eluc:TabStrip>

            <eluc:TabStrip ID="MenuRegistersOilType" runat="server" OnTabStripCommand="RegistersOilType_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid ID="gvOilType" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" Height="86%" CellPadding="3" OnItemCommand="gvOilType_RowCommand" OnItemDataBound="gvOilType_ItemDataBound"
                AllowSorting="false" AllowCustomPaging="true" AllowPaging="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                OnUpdateCommand="gvOilType_RowUpdating" ShowFooter="true" OnNeedDataSource="gvOilType_NeedDataSource"
                ShowHeader="true" EnableViewState="false"  EnableHeaderContextMenu="true" GroupingEnabled="false">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDIMODCSREVISIONOFFOCONSUMPTIONID">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />

                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Date of revision" HeaderStyle-Width="20%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDateofrevision" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDREVISIONDATE"))%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="txtDateofrevisionEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONDATE") %>'
                                    CssClass="gridinput_mandatory" DatePicker="true"></eluc:Date>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date ID="txtDateofrevisionAdd" runat="server" CssClass="gridinput_mandatory" DatePicker="true"></eluc:Date>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Revised Provision" HeaderStyle-Width="70%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbloilconsumers" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONPROVISION") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lbloilconsumerid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIMODCSREVISIONOFFOCONSUMPTIONID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtoilconsumersEdit" runat="server" CssClass="gridinput_mandatory"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONPROVISION") %>' MaxLength="500" Width="98%" />
                                <telerik:RadLabel ID="lbloilconsumeridedit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIMODCSREVISIONOFFOCONSUMPTIONID") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtoilconsumersAdd" runat="server" CssClass="gridinput_mandatory" Width="98%"
                                    MaxLength="500" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="10%">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
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
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="UPDATE" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Add New" CommandName="Add" ID="cmdAdd" ToolTip="Add New">
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
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="80%" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

