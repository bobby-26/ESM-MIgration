<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersEUMRVMesurementInstrument.aspx.cs"
    Inherits="RegistersEUMRVMesurementInstrument" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>EUMRV Measurement Instrument</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmRegistersCity" runat="server">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlCityEntry" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />

        <telerik:RadAjaxPanel runat="server" ID="pnlCityEntry" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="MenuProcedureDetailList" Title="Measurement Instrument" runat="server" OnTabStripCommand="MenuProcedureDetailList_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>

            <table id="tblConfigureCity" width="100%">
                <tr>
                    <td><b><span>C.2.4	Description of the measurement instruments involved</span></b></td>
                </tr>
            </table>
            <table width="50%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblvessel" runat="server" Text="Vessel"></telerik:RadLabel>&nbsp;&nbsp;
                    </td>
                    <td>
                        <eluc:Vessel ID="ddlVessel" runat="server" CssClass="input" VesselsOnly="true" AppendDataBoundItems="true" AutoPostBack="true" />
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenuRegistersCity" runat="server" OnTabStripCommand="RegistersCity_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid ID="gvSWS" runat="server" AutoGenerateColumns="false" Font-Size="11px"
                GridLines="None" Width="100%" Height="77%" CellPadding="3" OnItemCommand="gvSWS_RowCommand"
                OnItemDataBound="gvSWS_RowDataBound" OnUpdateCommand="gvSWS_RowUpdating" OnNeedDataSource="gvSWS_NeedDataSource"
                ShowFooter="true" ShowHeader="true" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false" 
                AllowCustomPaging="true" AllowPaging="true"
                AllowSorting="false">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDDTKEY">
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
                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />

                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Measurement equipment (name)" HeaderStyle-Width="30%">
                        <ItemStyle Wrap="False"></ItemStyle>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDMEASUREMENTEQUIPMENT") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadTextBox runat="server" Width="98%" ID="txtMesurementEdit" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASUREMENTEQUIPMENT") %>' CssClass="gridinput_mandatory"></telerik:RadTextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <telerik:RadTextBox runat="server" Width="98%" ID="txtMesurementAdd" Text="" CssClass="gridinput_mandatory"></telerik:RadTextBox>
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Elements applied to (e.g. emission sources,tanks)" HeaderStyle-Width="30%">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lbldtKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEUMRVMESUREMENTINSTRUMENTID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblFueltype" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMISSIONSOURCENAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadLabel ID="lbldtKeyEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEUMRVMESUREMENTINSTRUMENTID") %>'></telerik:RadLabel>
                            <telerik:RadTextBox ID="txtEmissionEdit" Width="98%" runat="server" CssClass="gridinput_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMISSIONSOURCENAME") %>'></telerik:RadTextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <telerik:RadTextBox ID="txtEmissionAdd" Width="98%" runat="server" CssClass="gridinput_mandatory"></telerik:RadTextBox>
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Technical description (specification, age, maintenance intervals)" HeaderStyle-Width="30%">
                        <ItemStyle Wrap="False"></ItemStyle>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDTECHNICALDESC") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadTextBox runat="server" Width="98%" ID="txtTechDescEdit" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTECHNICALDESC") %>' CssClass="gridinput_mandatory"></telerik:RadTextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <telerik:RadTextBox runat="server" Width="98%" ID="txtTechDescAdd" Text="" CssClass="gridinput_mandatory"></telerik:RadTextBox>
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
                            <asp:LinkButton runat="server" AlternateText="Add" CommandName="Add" ID="cmdAdd" ToolTip="Add">
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
