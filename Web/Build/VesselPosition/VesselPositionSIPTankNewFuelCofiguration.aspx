<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionSIPTankNewFuelCofiguration.aspx.cs" Inherits="VesselPositionSIPTankNewFuelCofiguration" MaintainScrollPositionOnPostback="true" %>

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
    <title>Tank capacity & fuel type</title>
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

            <eluc:TabStrip ID="TabTankInformation" Title="Tank capacity & fuel type" runat="server" OnTabStripCommand="TabTankInformation_TabStripCommand" TabStrip="true" />

            <table id="tblSearch" width="100%" style="display: none;">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselName" runat="server" Text="Vessel Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="UcVessel" runat="server" CssClass="dropdown_mandatory" AutoPostBack="true" SyncActiveVesselsOnly="True" AssignedVessels="true"
                            VesselsOnly="true" AppendDataBoundItems="true" />
                    </td>
                </tr>
            </table>
            
            <telerik:RadLabel ID="lblFuelTank" runat="server"><b>Fuel tanks</b></telerik:RadLabel>

            <telerik:RadGrid ID="gvSIPTanksConfuguration" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnItemCommand="gvSIPTanksConfuguration_RowCommand" OnItemDataBound="gvSIPTanksConfuguration_ItemDataBound"
                AllowSorting="false" OnUpdateCommand="gvSIPTanksConfuguration_RowUpdating" OnNeedDataSource="gvSIPTanksConfuguration_NeedDataSource"
                ShowFooter="false" ShowHeader="true" EnableHeaderContextMenu="true" GroupingEnabled="false"
                EnableViewState="false">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDSIPTANKID">
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
                        <telerik:GridTemplateColumn HeaderText="Tank Name" HeaderStyle-Width="25%" HeaderStyle-HorizontalAlign="Left">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="30%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTANKNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblTankID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIPTANKID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Capacity(m3)" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Left">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCapacilty" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELCAPACITY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblispTankIDEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIPTANKID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCapaciltyEdi" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELCAPACITY") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Current Fuel Type" HeaderStyle-Width="20%">
                            <ItemStyle Wrap="True" HorizontalAlign="Left" Width="20%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFeulType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="New Feul Type" HeaderStyle-Width="20%">
                            <ItemStyle Wrap="True" HorizontalAlign="Left" Width="20%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNewFeulType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEWFUELTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadComboBox ID="ddlNewFeulType" runat="server" Width="98%" CssClass="gridinput" DataValueField="FLDOILTYPECODE" DataTextField="FLDOILTYPENAME"></telerik:RadComboBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Tank Cleaning Req." HeaderStyle-Width="15%">
                            <ItemStyle Wrap="true" HorizontalAlign="Center"  />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTankCleanNot" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTANKCLEANNOTREQYN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkTankCleanNotReq" runat="server" Visible="false" Checked='<%# DataBinder.Eval(Container, "DataItem.FLDTANKCLEANNOTREQ").ToString().Equals("1") ? true : false %>' />
                                <telerik:RadRadioButtonList ID="rdTankCleanNotReq" runat="server" Direction="Horizontal">
                                    <Items>
                                        <telerik:ButtonListItem Value="0" Text="Yes"></telerik:ButtonListItem>
                                        <telerik:ButtonListItem Value="1" Text="No"></telerik:ButtonListItem>
                                        <telerik:ButtonListItem Value="2" Text="NA"></telerik:ButtonListItem>
                                    </Items>
                                </telerik:RadRadioButtonList>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
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
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <hr />
            <div id="divFuelTankSummary" runat="server" style="width: 100%;"></div>
            <hr />
            <telerik:RadLabel ID="lblSettlingServeice" runat="server"><b>Settling and service tanks</b></telerik:RadLabel>

            <telerik:RadGrid ID="gvSettlingServeice" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnItemCommand="gvSettlingServeice_RowCommand" OnItemDataBound="gvSettlingServeice_ItemDataBound"
                AllowSorting="false" OnUpdateCommand="gvSettlingServeice_RowUpdating" OnNeedDataSource="gvSettlingServeice_NeedDataSource"
                ShowFooter="false" ShowHeader="true" EnableHeaderContextMenu="true" GroupingEnabled="false"
                EnableViewState="false">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDSIPTANKID">
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
                        <telerik:GridTemplateColumn HeaderText="Tank Name" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="25%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" ></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTANKNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblTankID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIPTANKID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Capacity(m3)" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="10%">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCapacilty" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELCAPACITY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblispTankIDEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIPTANKID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCapaciltyEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELCAPACITY") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Current Fuel Type" HeaderStyle-Width="20%">
                            <ItemStyle Wrap="True" HorizontalAlign="Left" Width="20%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFeulType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="New Fuel Type" HeaderStyle-Width="20%">
                            <ItemStyle Wrap="True" HorizontalAlign="Left" Width="20%"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblNewFuelTypeHeader" runat="server">
                                    New Fuel Type&nbsp;
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNewFeulType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEWFUELTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadComboBox ID="ddlNewFeulType" runat="server" Width="98%" CssClass="gridinput" DataValueField="FLDOILTYPECODE" DataTextField="FLDOILTYPENAME"></telerik:RadComboBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Tank Cleaning Req." HeaderStyle-Width="15%">
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="10%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTankCleanNot" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTANKCLEANNOTREQYN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkTankCleanNotReq" runat="server" Visible="false" Checked='<%# DataBinder.Eval(Container, "DataItem.FLDTANKCLEANNOTREQ").ToString().Equals("1") ? true : false %>' />
                                <telerik:RadRadioButtonList ID="rdTankCleanNotReq" runat="server" Direction="Horizontal">
                                    <Items>
                                        <telerik:ButtonListItem Value="0" Text="Yes"></telerik:ButtonListItem>
                                        <telerik:ButtonListItem Value="1" Text="No"></telerik:ButtonListItem>
                                        <telerik:ButtonListItem Value="2" Text="NA"></telerik:ButtonListItem>
                                    </Items>
                                </telerik:RadRadioButtonList>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
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
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <hr />
            <div id="divServiceSettlingTankSummary" runat="server" style="width: 100%;"></div>
            <hr />
            <telerik:RadLabel ID="lblLubeOilTank" runat="server"><b>Lubricating oil tanks</b></telerik:RadLabel>

            <telerik:RadGrid ID="gvLubeOilTank" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnItemCommand="gvLubeOilTank_RowCommand" OnItemDataBound="gvLubeOilTank_ItemDataBound"
                AllowSorting="false" OnUpdateCommand="gvLubeOilTank_RowUpdating" OnNeedDataSource="gvLubeOilTank_NeedDataSource"
                ShowFooter="false" ShowHeader="true" EnableHeaderContextMenu="true" GroupingEnabled="false"
                EnableViewState="false">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDSIPTANKID">
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
                        <telerik:GridTemplateColumn HeaderText="Tank Name" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="25%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" ></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblHeaderTank" runat="server" Text="Tank Name"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTANKNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblTankID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIPTANKID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Capacity(m3)" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="10%">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCapacilty" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELCAPACITY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblispTankIDEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIPTANKID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCapaciltyEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELCAPACITY") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Current Lube Oil Type" HeaderStyle-Width="20%">
                            <ItemStyle Wrap="True" HorizontalAlign="Left" Width="20%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFeulType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="New Lube Oil Type" HeaderStyle-Width="20%">
                            <ItemStyle Wrap="True" HorizontalAlign="Left" Width="20%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNewFeulType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEWFUELTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadComboBox ID="ddlNewFeulType" runat="server" Width="98%" CssClass="gridinput" DataValueField="FLDOILTYPECODE" DataTextField="FLDOILTYPENAME"></telerik:RadComboBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Tank Cleaning Req." HeaderStyle-Width="15%">
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="10%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTankCleanNot" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTANKCLEANNOTREQYN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkTankCleanNotReq" runat="server" Visible="false" Checked='<%# DataBinder.Eval(Container, "DataItem.FLDTANKCLEANNOTREQ").ToString().Equals("1") ? true : false %>' />
                                <telerik:RadRadioButtonList ID="rdTankCleanNotReq" runat="server" Direction="Horizontal">
                                    <Items>
                                        <telerik:ButtonListItem Value="0" Text="Yes"></telerik:ButtonListItem>
                                        <telerik:ButtonListItem Value="1" Text="No"></telerik:ButtonListItem>
                                        <telerik:ButtonListItem Value="2" Text="NA"></telerik:ButtonListItem>
                                    </Items>
                                </telerik:RadRadioButtonList>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
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
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <hr />
            <div id="divLubeOilTank" runat="server" style="width: 100%;"></div>
            <hr />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
