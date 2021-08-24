<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionSIPTankCofiguration.aspx.cs" Inherits="VesselPositionSIPTankCofiguration" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="OilType" Src="~/UserControls/UserControlOilType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Splitter" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Bunker Receipt</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSIPTanksConfuguration" runat="server">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlSIPTanksConfuguration" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />

        <telerik:RadAjaxPanel runat="server" ID="pnlSIPTanksConfuguration">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table id="tblSearch" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselName" runat="server" Text="Vessel Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="UcVessel" runat="server" CssClass="dropdown_mandatory" AutoPostBack="true" SyncActiveVesselsOnly="True" AssignedVessels="true"
                            VesselsOnly="true" AppendDataBoundItems="true" OnTextChangedEvent="UcVessel_TextChangedEvent" />
                    </td>
                </tr>
            </table>

            <telerik:RadLabel ID="lblFuelTank" runat="server"><b>Fuel tanks</b></telerik:RadLabel>

            <telerik:RadGrid ID="gvSIPTanksConfuguration" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnItemCommand="gvSIPTanksConfuguration_RowCommand" OnItemDataBound="gvSIPTanksConfuguration_ItemDataBound"
                AllowSorting="false" OnUpdateCommand="gvSIPTanksConfuguration_RowUpdating" OnNeedDataSource="gvSIPTanksConfuguration_NeedDataSource"
                ShowFooter="true" ShowHeader="true" EnableHeaderContextMenu="true" GroupingEnabled="false"
                EnableViewState="false">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />

                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Tank Name" HeaderStyle-HorizontalAlign="Left">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="40%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTANKNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblTankID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIPTANKID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtTanknameEdit" runat="server" CssClass="gridinput_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTANKNAME") %>' Width="98%"></telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtTanknameAdd" runat="server" CssClass="gridinput_mandatory" Width="98%"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Capacity(m3)" HeaderStyle-HorizontalAlign="Left">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="20%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCapacilty" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELCAPACITY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblispTankIDEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIPTANKID") %>'></telerik:RadLabel>
                                <eluc:Number ID="txtCapacityEdit" runat="server" CssClass="gridinput" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELCAPACITY") %>' MaxLength="14" DecimalPlace="2" Width="98%" />
                                <telerik:RadLabel ID="lblTankIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTANKID") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtCapacityAdd" runat="server" CssClass="gridinput" MaxLength="14" DecimalPlace="2" Width="98%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Current Fuel Type">
                            <ItemStyle Wrap="True" HorizontalAlign="Left" Width="30%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFeulType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblFeulTypeidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELTYPENAME") %>'></telerik:RadLabel>
                                <telerik:RadComboBox ID="ddlFeulType" runat="server" Width="98%" CssClass="gridinput" DataValueField="FLDOILTYPECODE" DataTextField="FLDOILTYPENAME"></telerik:RadComboBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadComboBox ID="ddlFeulTypeAdd" runat="server" Width="98%" CssClass="gridinput" DataValueField="FLDOILTYPECODE" DataTextField="FLDOILTYPENAME"></telerik:RadComboBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="10%"></ItemStyle>
                            <FooterStyle HorizontalAlign="Center" />
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
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Add" CommandName="Add" ID="cmdAdd" ToolTip="Add">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

            <br />
            <telerik:RadLabel ID="lblSettlingServeice" runat="server"><b>Settling and service tanks</b></telerik:RadLabel>

            <telerik:RadGrid ID="gvSettlingServeice" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnItemCommand="gvSettlingServeice_RowCommand" OnItemDataBound="gvSettlingServeice_ItemDataBound"
                AllowSorting="false" OnUpdateCommand="gvSettlingServeice_RowUpdating" OnNeedDataSource="gvSettlingServeice_NeedDataSource"
                ShowFooter="true" ShowHeader="true" EnableHeaderContextMenu="true" GroupingEnabled="false"
                EnableViewState="false">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Tank Name" HeaderStyle-HorizontalAlign="Left">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="40%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTANKNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblTankID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIPTANKID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtTanknameEdit" runat="server" CssClass="gridinput_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTANKNAME") %>' Width="98%"></telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtTanknameAdd" runat="server" CssClass="gridinput_mandatory" Width="98%"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Capacity(m3)" HeaderStyle-HorizontalAlign="Left">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="20%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCapacilty" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELCAPACITY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblispTankIDEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIPTANKID") %>'></telerik:RadLabel>
                                <eluc:Number ID="txtCapacityEdit" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELCAPACITY") %>' MaxLength="14" DecimalPlace="2" Width="98%" />
                                <telerik:RadLabel ID="lblTankIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTANKID") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtCapacityAdd" runat="server" CssClass="gridinput" MaxLength="14" DecimalPlace="2" Width="98%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Current Fuel Type">
                            <ItemStyle Wrap="True" HorizontalAlign="Left" Width="30%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFeulType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblFeulTypeidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELTYPENAME") %>'></telerik:RadLabel>
                                <telerik:RadComboBox ID="ddlFeulType" runat="server" Width="98%" CssClass="gridinput" DataValueField="FLDOILTYPECODE" DataTextField="FLDOILTYPENAME"></telerik:RadComboBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadComboBox ID="ddlFeulTypeAdd" runat="server" Width="98%" CssClass="gridinput" DataValueField="FLDOILTYPECODE" DataTextField="FLDOILTYPENAME"></telerik:RadComboBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="10%"></ItemStyle>
                            <FooterStyle HorizontalAlign="Center" />
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
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Add" CommandName="Add" ID="cmdAdd" ToolTip="Add">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

            <br />
            <telerik:RadLabel ID="lblLubeOilTank" runat="server"><b>Lubricating oil tanks</b></telerik:RadLabel>

            <telerik:RadGrid ID="gvLubeOilTank" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnItemCommand="gvLubeOilTank_RowCommand" OnItemDataBound="gvLubeOilTank_ItemDataBound"
                AllowSorting="false" OnUpdateCommand="gvLubeOilTank_RowUpdating" OnNeedDataSource="gvLubeOilTank_NeedDataSource"
                ShowFooter="true" ShowHeader="true" EnableHeaderContextMenu="true" GroupingEnabled="false"
                EnableViewState="false">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />

                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Tank Name" HeaderStyle-HorizontalAlign="Left">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="40%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTANKNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblTankID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIPTANKID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtTanknameEdit" runat="server" CssClass="gridinput_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTANKNAME") %>' Width="98%"></telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtTanknameAdd" runat="server" CssClass="gridinput_mandatory" Width="98%"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Capacity(m3)" HeaderStyle-HorizontalAlign="Left">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="20%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCapacilty" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELCAPACITY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblispTankIDEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIPTANKID") %>'></telerik:RadLabel>
                                <eluc:Number ID="txtCapacityEdit" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELCAPACITY") %>' MaxLength="14" DecimalPlace="2" Width="98%" />
                                <telerik:RadLabel ID="lblTankIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTANKID") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtCapacityAdd" runat="server" CssClass="gridinput" MaxLength="14" DecimalPlace="2" Width="98%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Current Lube Oil Type">
                            <ItemStyle Wrap="True" HorizontalAlign="Left" Width="30%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFeulType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblFeulTypeidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELTYPENAME") %>'></telerik:RadLabel>
                                <telerik:RadComboBox ID="ddlFeulType" runat="server" Width="98%" CssClass="gridinput" DataValueField="FLDOILTYPECODE" DataTextField="FLDOILTYPENAME"></telerik:RadComboBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadComboBox ID="ddlFeulTypeAdd" runat="server" Width="98%" CssClass="gridinput" DataValueField="FLDOILTYPECODE" DataTextField="FLDOILTYPENAME"></telerik:RadComboBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="10%"></ItemStyle>
                            <FooterStyle HorizontalAlign="Center" />
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
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Add" CommandName="Add" ID="cmdAdd" ToolTip="Add">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
