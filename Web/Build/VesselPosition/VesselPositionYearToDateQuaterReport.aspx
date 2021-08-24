<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionYearToDateQuaterReport.aspx.cs"
    Inherits="VesselPositionYearToDateQuaterReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Noon Report Range Config</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
           function Resize() {

                   TelerikGridResize($find("<%= gvMenuYeartodatequaterreport.ClientID %>"));
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
               Resize();
               fade('statusmessage');
            }
        </script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmNoonReportRangeConfig" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlYeartodatequaterreport" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel runat="server" ID="pnlYeartodatequaterreport">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuTab" TabStrip="true" runat="server" OnTabStripCommand="MenuTab_TabStripCommand"></eluc:TabStrip>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:Status runat="server" ID="ucStatus" />
            <table id="tblSearch" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselName" runat="server" Text="Vessel Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="UcVessel" runat="server" CssClass="dropdown_mandatory" AutoPostBack="true" SyncActiveVesselsOnly="True" AssignedVessels="true"
                            VesselsOnly="true" AppendDataBoundItems="true" OnTextChangedEvent="UcVessel_TextChangedEvent" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblYear" runat="server" Text="Year"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlYear" runat="server" CssClass="input" AutoPostBack="true" OnTextChanged="ddlYear_TextChanged">
                        </telerik:RadComboBox>
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblQuater" runat="server" Text="Quarter"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlPeriod" runat="server" CssClass="input" AutoPostBack="true">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value=""></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Jan-Mar" Value="1"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Jan-Jun" Value="2"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Jan-Sep" Value="3"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Jan-Dec" Value="4"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenuYeartodatequaterreport" runat="server" OnTabStripCommand="MenuNRRangeConfig_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid ID="gvMenuYeartodatequaterreport" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnItemCommand="gvMenuYeartodatequaterreport_RowCommand" OnItemDataBound="gvMenuYeartodatequaterreport_ItemDataBound"
                OnUpdateCommand="gvMenuYeartodatequaterreport_RowUpdating" OnNeedDataSource="gvMenuYeartodatequaterreport_NeedDataSource"
                ShowFooter="false" ShowHeader="true" EnableViewState="True" AllowSorting="false" EnableHeaderContextMenu="true" GroupingEnabled="false"
                AllowCustomPaging="true" AllowPaging="true">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDYTDEEOIACHIVEDID">
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
                        <telerik:GridTemplateColumn HeaderText="EEOI Measurement</br>Period" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="True" HorizontalAlign="Left" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPeriod" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERIOD") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblytdachivedid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDYTDEEOIACHIVEDID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblytdreviewid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDYTDEEOIACHIVEDREVIEWID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="txtPeriodEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERIOD") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblytdachivedidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDYTDEEOIACHIVEDID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblytdreviewidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDYTDEEOIACHIVEDREVIEWID") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="YTD - EEOI</br>Achieved" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="True" HorizontalAlign="right" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAchived" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTUALEEOI") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="txtAchivededit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTUALEEOI") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks / Reasons for</br>non-compliance (if applicable)" HeaderStyle-Width="30%" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="True" HorizontalAlign="left" Width="30%"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReason" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNONCOMPLIANCERESON") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtReasonEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNONCOMPLIANCERESON") %>' CssClass="gridinput" TextMode="MultiLine" Rows="6" Resize="Vertical" ToolTip="Enter Cause" Width="98%" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Emission Reduction </br>Measures/ Best practices </br>currently being followed by ships" HeaderStyle-Width="25%" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="True" HorizontalAlign="left" Width="25%"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMethodFollowed" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMISSIONREDUCTIONMEASUREFOLLOWED") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                    <asp:CheckBoxList ID="txtMethodFollowedEdit" RepeatDirection="Horizontal" RepeatLayout="Table" RepeatColumns="5" Height="120px" runat="server">
                                    </asp:CheckBoxList>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Additional Emission Reduction </br>Measures/ Best practices </br>proposed after reviewing" HeaderStyle-Width="25%" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="True" HorizontalAlign="left" Width="25%"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMethodPropossed" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMISSIONREDUCTIONMEASUREPROPOSSED") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBoxList ID="txtMethodPropossedEdit" RepeatDirection="Horizontal" RepeatLayout="Table" RepeatColumns="5" Height="120px" runat="server">
                                </asp:CheckBoxList>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="10%">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Breakup" CommandName="BREAKUP" ID="cmdBreakup" ToolTip="Breakup">
                                <span class="icon"><i class="fas fa-info-circle"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Re-Calculate" CommandName="RECALCULATE" ID="cmdRecalculate" ToolTip="Re-calculate">
                                <span class="icon"><i class="fa-calculator-alt"></i></span>
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
