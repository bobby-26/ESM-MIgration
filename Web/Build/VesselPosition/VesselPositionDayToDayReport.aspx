<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionDayToDayReport.aspx.cs"
    Inherits="VesselPositionDayToDayReport" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Reports</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
           function Resize() {
               setTimeout(function () {

                   TelerikGridResize($find("<%= gvDayToDayReport.ClientID %>"));
               }, 200);

           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
               Resize();               
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewCourseList" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlCourseListEntry" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />

        <telerik:RadAjaxPanel runat="server" ID="pnlDayToDayReport">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <table width="60%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <%--<telerik:RadComboBox ID="ddlVessel" runat="server" CssClass="input" AutoPostBack="true" OnTextChangedEvent="ddlVessel_TextChangedEvent">
                        </telerik:RadComboBox>--%>
                        <eluc:Vessel ID="ddlVessel" runat="server" CssClass="input" AppendDataBoundItems="true" AutoPostBack="true" VesselsOnly="true" SyncActiveVesselsOnly="True" AssignedVessels="true"
                            OnTextChangedEvent="ddlVessel_TextChangedEvent"/>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblyear" runat="server" Text="Year"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlYear" runat="server" CssClass="input" AutoPostBack="true" OnTextChangedEvent="ddlYear_TextChangedEvent">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblmonth" runat="server" Text="Month"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlMonth" runat="server" CssClass="input" AutoPostBack="true" OnTextChangedEvent="ddlMonth_TextChangedEvent">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="--Select--"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="1" Text="Jan"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="2" Text="Feb"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="3" Text="Mar"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="4" Text="Apr"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="5" Text="May"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="6" Text="Jun"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="7" Text="Jul"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="8" Text="Aug"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="9" Text="Sep"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="10" Text="Oct"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="11" Text="Nov"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="12" Text="Dec"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuDayToDayReportTab" runat="server" OnTabStripCommand="DayToDayReportTab_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid ID="gvDayToDayReport" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnItemCommand="gvDayToDayReport_RowCommand"
                AllowSorting="false" ShowFooter="false" ShowHeader="true" OnUpdateCommand="gvDayToDayReport_RowUpdating"
                EnableViewState="false" OnNeedDataSource="gvDayToDayReport_NeedDataSource"
                AllowCustomPaging="true" AllowPaging="true" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDPRSMPADAYTODAYREPORTID">
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
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />

                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Vessel" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="false" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblReportID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRSMPADAYTODAYREPORTID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Start Date" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStartDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFROMDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="End Date" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEndDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTODATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="IMO number" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIMONumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIMONUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ship type" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblShipType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Gross <br/>tonnage (GT)" HeaderStyle-Wrap="true" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblGrossTonnage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREGISTEREDGT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Net <br/>tonnage (NT)" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNetTonnage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREGISTEREDNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Deadweight <br/> tonnage (DWT)" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDeadweightTonnage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDWTSUMMER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="EEDI(gCO2/t.nm)" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEEDI" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEEDI") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ice class" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIceclass" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDICECLASS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Main propulsion <br/> power" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMainpropulsionpower" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEPOWEROUTPUT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Auxiliary <br/> engine(s)" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAuxiliaryenginer" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAEPOWEROUTPUT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Distance <br/> travelled (nm)" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDistancetravelled" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISTANCETRAVELLED") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Hours underway (h)" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblHoursunderway" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHOURSUNDERWAY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Diesel/Gas Oil" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDieselGasOil" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMDOMGOCONS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="LFO" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLFO" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLFOCONS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="HFO" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblHFO" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHFOCONS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="LPG (Propane)" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLPG" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLPGPROPANE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="LPG (Butane)" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLPGB" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLPGBUTANE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="LNG" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLNG" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLNG") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Methanol" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMethanol" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMETHANOL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ethanol" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEthanol" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDETHANOL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Other" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOther" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOTHERS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Method of <br/> measure cons" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMethodofmeasurecons" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELCONSMEASUREMETHOD") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150px">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtRemarks" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="center" />
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
