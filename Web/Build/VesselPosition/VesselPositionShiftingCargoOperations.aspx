<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionShiftingCargoOperations.aspx.cs" Inherits="VesselPositionShiftingCargoOperations" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Cargo" Src="~/UserControls/UserControlCargo.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Cargotype" Src="~/UserControls/UserControlCargoTypeMappedVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Cargo Operations</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function btnconfirm(args) {
                if (args) {
                    __doPostBack("<%=btnconfirm.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmCargoOperations" runat="server">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="panel1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <asp:Button ID="btnconfirm" runat="server" Text="confirm" OnClick="btnConfirm_Click" />
        
        <eluc:TabStrip ID="MenuCompanyList" runat="server" OnTabStripCommand="AgentPortList_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="panel1" runat="server" Height="100%">
            
            <table width="100%">
                <tr>
                    <td runat="server" id="divLoad" style="width: 100%">
                        <table>
                            <tr>
                                <td style="font-weight: bold; font-size: 16px"><u>
                                    <telerik:RadLabel ID="lblLoading" runat="server" Text="Loading"></telerik:RadLabel>
                                </u></td>
                            </tr>
                        </table>
                        <table cellpadding="1" cellspacing="1" width="100%">
                            <tr>
                                <td width="15%">
                                    <telerik:RadLabel ID="lblCargo" runat="server" Text="Cargo"></telerik:RadLabel>
                                </td>
                                <td>
                                    <%-- <eluc:Cargo ID="ucCargo" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" />--%>
                                    <eluc:Cargotype ID="ucCargo" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" />
                                    <telerik:RadLabel ID="lblOilMajorCargo" runat="server" Text="Oil Major Cargo" Visible="false"></telerik:RadLabel>
                                    <telerik:RadCheckBox ID="chkOilMajor" runat="server" Visible="false" />
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td style="width: 15%;">
                                    <telerik:RadLabel ID="lblOilmajor" runat="server" Text="Oil major"></telerik:RadLabel>
                                </td>
                                <td style="width: 25%;">
                                    <telerik:RadComboBox runat="server" ID="ddlOilMajor" CssClass="input"
                                        AppendDataBoundItems="true">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="--Select--" Value=""></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Text="BP" Value="BP"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Text="Exxon" Value="EXXON"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Text="Shell" Value="SHELL"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Text="Chevron" Value="CHEVRON"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Text="Total" Value="TOTAL"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Text="Others" Value="OTHERS"></telerik:RadComboBoxItem>
                                        </Items>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblQuantity" runat="server" Text="Quantity"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Number ID="ucQuantity" runat="server" CssClass="input_mandatory" DecimalPlace="2" IsPositive="true" />
                                    <telerik:RadComboBox ID="ddlUnit" runat="server" CssClass="input_mandatory" Visible="false">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="MT" Text="MT" Selected="True"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="m3" Text="m3"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="BBL" Text="BBL"></telerik:RadComboBoxItem>
                                        </Items>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblCommenced" runat="server" Text="Commenced"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Date runat="server" ID="txtCommenced" CssClass="input_mandatory" DatePicker="true" />
                                    <telerik:RadTimePicker ID="txtCommencedTime" runat="server" Width="80px" CssClass="input_mandatory"
                                        DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                                    </telerik:RadTimePicker>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblCompleted" runat="server" Text="Completed"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Date runat="server" ID="txtCompleted" CssClass="input_mandatory" DatePicker="true" />
                                    <telerik:RadTimePicker ID="txtCompletedTime" runat="server" Width="80px" CssClass="input_mandatory"
                                        DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                                    </telerik:RadTimePicker>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblsts" runat="server" Text="STS Transfer Y/N"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadCheckBox ID="chksts" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td runat="server" id="divDischarge" style="width: 100%">
                        <table>
                            <tr>
                                <td style="font-weight: bold; font-size: 16px"><u>
                                    <telerik:RadLabel ID="lblDischarging" runat="server" Text="Discharging"></telerik:RadLabel>
                                </u></td>
                            </tr>
                        </table>
                        <telerik:RadGrid ID="gvDischarge" runat="server" AutoGenerateColumns="false" Font-Size="11px" Width="100%" Height="100%"
                            OnItemCommand="gvDischarge_RowCommand" OnItemDataBound="gvDischarge_ItemDataBound" OnNeedDataSource="gvDischarge_NeedDataSource"
                            CellPadding="3" ShowFooter="false" ShowHeader="true" EnableViewState="false" AllowSorting="false" EnableHeaderContextMenu="true" GroupingEnabled="false">
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" DataKeyNames="FLDCARGOID" CommandItemDisplay="Top">
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
                                    <telerik:GridTemplateColumn HeaderText="Cargo">
                                        <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblCargoId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARGOID") %>' Visible="false"></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblCargoNameItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARGONAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Oil Major Cargo">
                                        <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblOilMajorCargo" runat="server" Text="Oil Major Cargo"></telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblOilMajorCargoItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILMAJORYN").ToString() == "1"?"Yes":"No" %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Oil Major">
                                        <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblOilMajorItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILMAJOR") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Qty on Board">
                                        <ItemStyle Wrap="false" HorizontalAlign="right" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblQtyOnBoardItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONBOARDQUANTITY") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Discharge Qty">
                                        <ItemStyle Wrap="false" HorizontalAlign="right" />
                                        <ItemTemplate>
                                            <eluc:Number ID="ucDischQty" runat="server" CssClass="gridinput" DecimalPlace="2" IsPositive="true" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Vapour Qty">
                                        <ItemStyle Wrap="false" HorizontalAlign="right" />
                                        <ItemTemplate>
                                            <eluc:Number ID="ucVapourQuantity" runat="server" CssClass="gridinput" DecimalPlace="1" IsPositive="true" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="STS Transfer Y/N">
                                        <ItemStyle Wrap="false" HorizontalAlign="center" />
                                        <ItemTemplate>
                                            <telerik:RadCheckBox ID="chksts" runat="server" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Commenced">
                                        <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <eluc:Date runat="server" ID="txtCommencedItem" CssClass="input_mandatory" DatePicker="true" />
                                            <telerik:RadTimePicker ID="txtCommencedTimeItem" runat="server" Width="80px" CssClass="input_mandatory"
                                                DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                                            </telerik:RadTimePicker>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Completed">
                                        <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <eluc:Date runat="server" ID="txtCompletedItem" CssClass="input_mandatory" DatePicker="true" />
                                            <telerik:RadTimePicker ID="txtCompletedTimeItem" runat="server" Width="80px" CssClass="input_mandatory"
                                                DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                                            </telerik:RadTimePicker>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Action">
                                        <ItemStyle Wrap="false" HorizontalAlign="Center" />
                                        <HeaderStyle Wrap="false" HorizontalAlign="Center" />
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblAction" runat="server" Text="Action"></telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Discharge" CommandName="DISCH" ID="cmdDischarge" ToolTip="Discharge">
                                                            <span class="icon"><i class="fas fa-save"></i></span>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </td>
                </tr>
            </table>
            <table cellpadding="1" cellspacing="1" width="75%" style="display: none;">
                <tr>
                    <td style="width: 40%">
                        <telerik:RadLabel ID="lblCargoOperation" runat="server" Text="Cargo Operation" Visible="false"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox runat="server" ID="ddlCargoOperation" AutoPostBack="true" CssClass="dropdown_mandatory" AppendDataBoundItems="true" Visible="false">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value=""></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Loading" Value="LOAD" Selected="True"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Discharging" Value="DISCH"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>

                    </td>

                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
