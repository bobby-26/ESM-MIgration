<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionShiftingReportOperations.aspx.cs" Inherits="VesselPositionShiftingReportOperations" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Voyage" Src="~/UserControls/UserControlVoyage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Cargo" Src="~/UserControls/UserControlCargo.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PortActivity" Src="~/UserControls/UserControlPortActivity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Latitude" Src="~/UserControls/UserControlLatitude.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Longitude" Src="~/UserControls/UserControlLongitude.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Unit" Src="~/UserControls/UserControlUnit.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="../UserControls/UserControlCurrency.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

    <title>Operations</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmDepartureReport" runat="server">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="panel1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenPick" runat="server" OnClick="cmdHiddenPick_Click" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenPick_Click" />
            <eluc:TabStrip ID="MenuDRSubTab" runat="server" TabStrip="true" OnTabStripCommand="MenuDRSubTab_TabStripCommand"></eluc:TabStrip>
            <eluc:TabStrip ID="ProjectBilling" runat="server" OnTabStripCommand="ProjectBilling_TabStripCommand"></eluc:TabStrip>
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td colspan="4">
                        <telerik:RadLabel ID="lblAlertSenttoOFC" runat="server" Text="Report Sent to Office" Visible="false"
                            Font-Bold="False" Font-Size="Large" Font-Underline="True" ForeColor="Red" BorderColor="Red">
                        </telerik:RadLabel>
                        <br />
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblCargoOperationHeader" runat="server" Text="No Cargo Operations in this port"></telerik:RadLabel>
                        </b>

                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chbCargoOperationExists" runat="server" AutoPostBack="true" OnCheckedChanged="chbCargoOperationExists_OnCheckedChanged"></telerik:RadCheckBox>
                    </td>
                </tr>
            </table>

            
            <telerik:RadGrid ID="gvCargoOperation" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnItemCommand="gvCargoOperation_RowCommand" OnItemDataBound="gvCargoOperation_ItemDataBound"
                OnNeedDataSource="gvCargoOperation_NeedDataSource" AllowSorting="false" ShowHeader="true" EnableViewState="false"
                EnableHeaderContextMenu="true" GroupingEnabled="false">

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
                        <telerik:GridTemplateColumn HeaderText="Cargo Operation" HeaderStyle-Wrap="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOperationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOPERATIONID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVoyageid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDVOYAGEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblOperationName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOPERATIONNAME")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCargoId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCARGOID")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Cargo">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCargo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARGONAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Qty (MT)" HeaderStyle-Wrap="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY") %>' CssClass="input_mandatory" DecimalPlace="2" IsPositive="true" MaxLength="9" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Vapour Qty (MT)" HeaderStyle-Wrap="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <EditItemTemplate>
                                <eluc:Number ID="txtVapourQty" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVAPOURQUANTITY") %>' CssClass="input_mandatory" DecimalPlace="1" IsPositive="true" MaxLength="4" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn Visible="false" HeaderText="Unit">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblUnit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNIT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Oil Major Cargo" HeaderStyle-Wrap="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOilMajoryn" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILMAJORCARGOYN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkOilMajor" runat="server" AutoPostBack="true" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Oil Major" HeaderStyle-Wrap="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOilMajor" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILMAJOR") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
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
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="STS Transfer Y/N" HeaderStyle-Wrap="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblsts" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTSOPERATION") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblstshidden" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTSOPERATIONYN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Commenced">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCommenced" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOMMENCED")) + " " + DataBinder.Eval(Container,"DataItem.FLDCOMMENCED", "{0:HH:mm}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Completed">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCompleted" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOMPLETED")) + " " + DataBinder.Eval(Container,"DataItem.FLDCOMPLETED", "{0:HH:mm}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                                            <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="SELECT" ID="cmdOperation"
                                    ToolTip="Operations">
                                        <span class="icon"><i class="fas fa-info-circle"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Attachment" CommandName="ATTACHMENT" ID="cmdAtt" ToolTip="Attachment">
                                                            <span class="icon"><i class="fas fa-paperclip"></i></span>
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
            <br />
            <b>
                <telerik:RadLabel ID="lblBallastDeballasting" runat="server" Text="Ballast / Deballasting"></telerik:RadLabel>
            </b>
            <table cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCommencedBallasting" runat="server" Text="Commenced Ballasting"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date runat="server" ID="txtBallastCommenced" CssClass="input" DatePicker="true" />
                        <telerik:RadTimePicker ID="txtBallastCommencedTime" runat="server" Width="80px" CssClass="input"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCompletedBallasting" runat="server" Text="Completed Ballasting"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date runat="server" ID="txtBallastCompleted" CssClass="input" DatePicker="true" />
                        <telerik:RadTimePicker ID="txtBallastCompletedTime" runat="server" Width="80px" CssClass="input"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblQuantityBallasted" runat="server" Text="Quantity Ballasted"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number runat="server" ID="txtVolumeBallasted" CssClass="input" DecimalPlace="2" Width="80px"
                            MaxLength="8" />
                        &nbsp;
                            <telerik:RadLabel ID="lblQuantityBallastedMT" runat="server" Text="MT"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCommencedDeballasting" runat="server" Text="Commenced Deballasting"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date runat="server" ID="txtDeballastCommenced" CssClass="input" DatePicker="true" />
                       <telerik:RadTimePicker ID="txtDeballastCommencedTime" runat="server" Width="80px" CssClass="input"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCompletedDeballasting" runat="server" Text="Completed Deballasting"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date runat="server" ID="txtDeballastCompleted" CssClass="input" DatePicker="true" />
                        <telerik:RadTimePicker ID="txtDeballastCompletedTime" runat="server" Width="80px" CssClass="input"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblQuantityDeballasted" runat="server" Text="Quantity Deballasted"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number runat="server" ID="txtVolumeDeballasted" CssClass="input" DecimalPlace="2" Width="80px"
                            MaxLength="8" />
                        &nbsp;
                            <telerik:RadLabel ID="lblQuantityDeballastedMT" runat="server" Text="MT"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
            <br />
            <b>
                <telerik:RadLabel ID="lblOtherOperations" runat="server" Text="Other Operations"></telerik:RadLabel>
            </b>
            <telerik:RadGrid ID="gvOtherOpr" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnItemCommand="gvOtherOpr_RowCommand" OnItemDataBound="gvOtherOpr_ItemDataBound"
                OnUpdateCommand="gvOtherOpr_RowUpdating" ShowFooter="true" ShowHeader="true" AllowSorting="false"
                EnableViewState="false" OnNeedDataSource="gvOtherOpr_NeedDataSource" EnableHeaderContextMenu="true" GroupingEnabled="false" >
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
                            <telerik:RadLabel ID="lblOperationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPERATIONID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDESCRIPTION").ToString().Length>15 ? DataBinder.Eval(Container, "DataItem.FLDDESCRIPTION").ToString().Substring(0, 15) + "..." : DataBinder.Eval(Container, "DataItem.FLDDESCRIPTION").ToString() %>'></telerik:RadLabel>
                            <eluc:Tooltip ID="ucDescriptionTT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadLabel ID="lblOperationIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPERATIONID") %>'></telerik:RadLabel>
                            <telerik:RadTextBox runat="server" ID="txtDescriptionEdit" CssClass="input" Height="30px"
                                Width="450px" TextMode="MultiLine" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'>
                            </telerik:RadTextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <telerik:RadTextBox runat="server" ID="txtDescriptionAdd" CssClass="input" Height="30px"
                                Width="450px" TextMode="MultiLine">
                            </telerik:RadTextBox>
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Commenced">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <FooterStyle Wrap="false" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="txtCommenced" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOMMENCEDDATE")) + " " + DataBinder.Eval(Container,"DataItem.FLDCOMMENCEDDATE", "{0:HH:mm}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Date runat="server" ID="txtCommencedEdit" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMMENCEDDATE") %>' CssClass="input" DatePicker="true" />
                            <telerik:RadTimePicker ID="txtCommencedEditTime" runat="server" Width="80px" CssClass="input" DbSelectedDate='<%# DataBinder.Eval(Container,"DataItem.FLDCOMMENCEDDATE", "{0:HH:mm}") %>'
                                    DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                                </telerik:RadTimePicker>                        </EditItemTemplate>
                        <FooterTemplate>
                            <eluc:Date ID="txtCommencedAdd" runat="server" CssClass="input" DatePicker="true" />
                            <telerik:RadTimePicker ID="txtCommencedAddTime" runat="server" Width="80px" CssClass="input"
                                    DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                                </telerik:RadTimePicker>                        </FooterTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Completed">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <FooterStyle Wrap="false" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="txtCompleted" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOMPLETEDDATE")) + " " + DataBinder.Eval(Container,"DataItem.FLDCOMPLETEDDATE", "{0:HH:mm}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Date runat="server" ID="txtCompletedEdit" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPLETEDDATE") %>' CssClass="input" DatePicker="true" />
                            <telerik:RadTimePicker ID="txtCompletedEditTime" runat="server" Width="80px" CssClass="input" DbSelectedDate='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPLETEDDATE", "{0:HH:mm}") %>'
                                    DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                                </telerik:RadTimePicker>                        </EditItemTemplate>
                        <FooterTemplate>
                            <eluc:Date ID="txtCompletedAdd" runat="server" CssClass="input" DatePicker="true" />
                            <telerik:RadTimePicker ID="txtCompletedAddTime" runat="server" Width="80px" CssClass="input"
                                    DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                                </telerik:RadTimePicker>
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn Visible="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <telerik:RadLabel ID="lblDateandTimeHeader" runat="server" Text="Date and Time"></telerik:RadLabel>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <telerik:RadLabel ID="txtValue" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDVALUE")) + " " + DataBinder.Eval(Container,"DataItem.FLDVALUE", "{0:HH:mm}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Date runat="server" ID="txtValueDateEdit" CssClass="input" DatePicker="true" />
                            <telerik:RadTextBox ID="txtValueDateEditTime" runat="server" CssClass="input" Width="50px" />
                            <%--<ajaxToolkit:MaskedEditExtender ID="mskValueDate" runat="server" AcceptAMPM="false" ClearMaskOnLostFocus="false" 
                                            ClearTextOnInvalid="true" Mask="99:99" MaskType="Time" TargetControlID="txtValueDateEditTime" UserTimeFormat="TwentyFourHour" />--%>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <eluc:Date ID="txtValueDateAdd" runat="server" CssClass="input" DatePicker="true" />
                            <telerik:RadTextBox ID="txtValueDateAddTime" runat="server" CssClass="input" Width="30px" />
                            <%--<ajaxToolkit:MaskedEditExtender ID="mskValueDateAdd" runat="server" AcceptAMPM="false" ClearMaskOnLostFocus="false" 
                                            ClearTextOnInvalid="true" Mask="99:99" MaskType="Time" TargetControlID="txtValueDateAddTime" UserTimeFormat="TwentyFourHour" />--%>
                        </FooterTemplate>
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
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="UPDATE" ID="cmdSave" ToolTip="Save">
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
                    </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <eluc:Status runat="server" ID="ucStatus" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
