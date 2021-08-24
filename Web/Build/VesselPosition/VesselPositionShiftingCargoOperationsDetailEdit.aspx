<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionShiftingCargoOperationsDetailEdit.aspx.cs" Inherits="VesselPositionShiftingCargoOperationsDetailEdit" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Cargo" Src="~/UserControls/UserControlCargo.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Operations</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmCargoOperations" runat="server">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true" />
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="panel1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="panel1" runat="server">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <telerik:RadGrid ID="gvCargoOperation" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnItemCommand="gvCargoOperation_RowCommand" OnItemDataBound="gvCargoOperation_ItemDataBound"
                EnableHeaderContextMenu="true" GroupingEnabled="false"
                ShowFooter="false" ShowHeader="true" AllowSorting="false" OnNeedDataSource="gvCargoOperation_NeedDataSource"
                EnableViewState="false">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDOPERATIONID" CommandItemDisplay="Top">
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
                        <telerik:GridTemplateColumn HeaderText="Description">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARGOOPERATION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Time / Value">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblValue" runat="server"></telerik:RadLabel>
                                <eluc:Tooltip ID="ucValueTT" runat="server" />
                                <telerik:RadLabel ID="lblValueTypeView" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALUETYPE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDepCargoOprId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPCARGOOPRID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtValueDecimalEdit" runat="server" CssClass="input txtNumber" DecimalPlace="2" IsPositive="true" MaxLength="9" />

                                <telerik:RadTextBox runat="server" ID="txtValueFreeTextEdit" CssClass="input" Height="30px" Width="140px"
                                    TextMode="MultiLine">
                                </telerik:RadTextBox>

                                <eluc:Date runat="server" ID="txtValueDateEdit" CssClass="input" DatePicker="true" />
                                <telerik:RadTimePicker ID="txtValueDateEditTime" runat="server" Width="80px" CssClass="input"
                                    DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                                </telerik:RadTimePicker>
                                <telerik:RadLabel ID="lblValueType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALUETYPE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCargoOprId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARGOOPERATIONID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblOperationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPERATIONID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDepCargoOprIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPCARGOOPRID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselDepartureID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELDEPARTUREID") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Remarks">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString().Length>15 ? DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString().Substring(0, 15) + "..." : DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString() %>'></telerik:RadLabel>
                                <eluc:Tooltip ID="ucRemarksTT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox runat="server" ID="txtRemarksEdit" CssClass="input" Height="30px" Width="140px"
                                    TextMode="MultiLine" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'>
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="120px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                                            <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="DISCHARGE" ID="cmdSave" ToolTip="Save">
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
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" />
                </ClientSettings>
            </telerik:RadGrid>

            <table cellpadding="1" cellspacing="1" width="75%" style="visibility: hidden; display: none;">
                <tr>
                    <td style="width: 40%">
                        <asp:Literal ID="lblCargoOperation" runat="server" Text="Cargo Operation" Visible="false"></asp:Literal>
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
