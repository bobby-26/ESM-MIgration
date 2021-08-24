<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewVesselPositionNoonReportList.aspx.cs"
    Inherits="CrewVesselPositionNoonReportList" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Latitude" Src="~/UserControls/UserControlLatitude.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Longitude" Src="~/UserControls/UserControlLongitude.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Vessel Position Noon</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmVesselPosition" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

        <eluc:TabStrip ID="MenuVesselPosition" runat="server" OnTabStripCommand="MenuVesselPosition_TabStripCommand"
            TabStrip="true"></eluc:TabStrip>

        <eluc:TabStrip ID="MenuCrewNoonReport" runat="server" OnTabStripCommand="CrewNoonReport_TabStripCommand"></eluc:TabStrip>


        <telerik:RadGrid RenderMode="Lightweight" ID="gvVesselPositionNoonReport" runat="server" Height="500px" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" OnNeedDataSource="gvVesselPositionNoonReport_NeedDataSource"
            OnItemDataBound="gvVesselPositionNoonReport_ItemDataBound"
            OnItemCommand="gvVesselPositionNoonReport_ItemCommand"
            GroupingEnabled="false" EnableHeaderContextMenu="true"
            AutoGenerateColumns="false">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" TableLayout="Fixed">
                <NoRecordsTemplate>
                    <table width="100%" border="0">
                        <tr>
                            <td align="center">
                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </NoRecordsTemplate>
                <HeaderStyle Width="102px" />
                <Columns>
                    <telerik:GridButtonColumn Text="SingleClick" CommandName="Edit" Visible="false" />
                    <telerik:GridTemplateColumn HeaderText="Date" HeaderStyle-Width="7%">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblNoonreportDate" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDNOONREPORTDATE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Message Type">
                        <ItemTemplate>
                            <table>
                                <tr>
                                    <th align="left">
                                        <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                                    </th>
                                    <td>
                                        <telerik:RadLabel ID="lblMessageType" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDMESSAGETYPENAME") %>'></telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left">
                                        <telerik:RadLabel ID="lblLatitude1" runat="server" Text="Latitude"></telerik:RadLabel>
                                    </th>
                                    <td>
                                        <telerik:RadLabel ID="lblLatitude" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDLATITUDE") %>'></telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left">
                                        <telerik:RadLabel ID="lblLongitude1" runat="server" Text="Longitude"></telerik:RadLabel>
                                    </th>
                                    <td>
                                        <telerik:RadLabel ID="lblLongitude" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDLONGITUDE") %>'></telerik:RadLabel>
                                    </td>
                                </tr>
                            </table>
                            <telerik:RadLabel ID="lblNoonReportId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDNOONREPORTID") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Wind">
                        <ItemTemplate>
                            <table width="100%" cellpadding="1" cellspacing="1">
                                <tr>
                                    <th align="left">
                                        <telerik:RadLabel ID="lblDirection" runat="server" Text="Direction"></telerik:RadLabel>
                                    </th>
                                    <td>
                                        <telerik:RadLabel ID="lblWindDirection" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDWINDDIRECTION") %>'></telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left">
                                        <telerik:RadLabel ID="lblForce" runat="server" Text="Force"></telerik:RadLabel>
                                    </th>
                                    <td>
                                        <telerik:RadLabel ID="lblWindForce" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDWINDFORCE","{0:n2}") %>'></telerik:RadLabel>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Sea">
                        <ItemTemplate>
                            <table width="100%" cellpadding="1" cellspacing="1">
                                <tr>
                                    <th align="left">
                                        <telerik:RadLabel ID="lblDirection1" runat="server" Text="Direction"></telerik:RadLabel>
                                    </th>
                                    <td>
                                        <telerik:RadLabel ID="lblSeaDirection" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDSEADIRECTION") %>'></telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left">
                                        <telerik:RadLabel ID="lblForce1" runat="server" Text="Force"></telerik:RadLabel>
                                    </th>
                                    <td>
                                        <telerik:RadLabel ID="lblSeaForce" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDSEAFORCE","{0:n2}") %>'></telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left">
                                        <telerik:RadLabel ID="lblTemperature1" runat="server" Text="Temperature"></telerik:RadLabel>
                                    </th>
                                    <td>
                                        <telerik:RadLabel ID="lblSeaTempreature" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDSEATEMPERATURE","{0:n2}") %>'></telerik:RadLabel>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Current">
                        <ItemTemplate>
                            <table width="100%" cellpadding="1" cellspacing="1">
                                <tr>
                                    <th align="left">
                                        <telerik:RadLabel ID="lblDirection2" runat="server" Text="Direction"></telerik:RadLabel>
                                    </th>
                                    <td>
                                        <telerik:RadLabel ID="lblCurrDirection" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCURRENTDIRECTION") %>'></telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left">
                                        <telerik:RadLabel ID="lblSpeed1" runat="server" Text="Speed"></telerik:RadLabel>
                                    </th>
                                    <td>
                                        <telerik:RadLabel ID="lblCurrSpeed" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCURRENTSPEED","{0:n2}") %>'></telerik:RadLabel>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Swell">
                        <ItemTemplate>
                            <table width="100%" cellpadding="1" cellspacing="1">
                                <tr>
                                    <th align="left">
                                        <telerik:RadLabel ID="lblDirection3" runat="server" Text="Direction"></telerik:RadLabel>
                                    </th>
                                    <td>
                                        <telerik:RadLabel ID="lblSwellDirection" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDSWELLDIRECTION") %>'></telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left">
                                        <telerik:RadLabel ID="lblHeight" runat="server" Text="Height"></telerik:RadLabel>
                                    </th>
                                    <td>
                                        <telerik:RadLabel ID="lblSwellHeight" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDSWELLHEIGHT","{0:n2}") %>'></telerik:RadLabel>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Vessel Course">
                        <ItemTemplate>
                            <table width="100%" cellpadding="1" cellspacing="1">
                                <tr>
                                    <th align="left">
                                        <telerik:RadLabel ID="lblCourse" runat="server" Text="Course"></telerik:RadLabel>
                                    </th>
                                    <td>
                                        <telerik:RadLabel ID="lblVesselCourse" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDVESSELCOURSE") %>'></telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left">
                                        <telerik:RadLabel ID="lblCargo" runat="server" Text="Cargo"></telerik:RadLabel>
                                    </th>
                                    <td>
                                        <telerik:RadLabel ID="lblCurrentCargo" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCURRENTCARGO") %>'></telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left">
                                        <telerik:RadLabel ID="lblEngineRPM1" runat="server" Text="Engine RPM"></telerik:RadLabel>
                                    </th>
                                    <td>
                                        <telerik:RadLabel ID="lblEngineRpm" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDENGINERPM","{0:n2}") %>'></telerik:RadLabel>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Charter">
                        <ItemTemplate>
                            <table width="100%" cellpadding="1" cellspacing="1">
                                <tr>
                                    <th align="left">
                                        <telerik:RadLabel ID="lblSpeed2" runat="server" Text="Speed"></telerik:RadLabel>
                                    </th>
                                    <td>
                                        <telerik:RadLabel ID="lblCpSpeed" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCPSPEED","{0:n2}") %>'></telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left">
                                        <telerik:RadLabel ID="lblFO" runat="server" Text="FO"></telerik:RadLabel>
                                    </th>
                                    <td>
                                        <telerik:RadLabel ID="lblCpFo" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCPFO","{0:n2}") %>'></telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left">
                                        <telerik:RadLabel ID="lblDO" runat="server" Text="DO"></telerik:RadLabel>
                                    </th>
                                    <td>
                                        <telerik:RadLabel ID="lblCpDo" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCPDO","{0:n2}") %>'></telerik:RadLabel>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Actual">
                        <ItemTemplate>
                            <table width="100%" cellpadding="1" cellspacing="1">
                                <tr>
                                    <th align="left">
                                        <telerik:RadLabel ID="lblSpeed3" runat="server" Text="Speed"></telerik:RadLabel>
                                    </th>
                                    <td>
                                        <telerik:RadLabel ID="lblAverageSpeed" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDAVERAGESPEED","{0:n2}") %>'></telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left">
                                        <telerik:RadLabel ID="lblFO1" runat="server" Text="FO"></telerik:RadLabel>
                                    </th>
                                    <td>
                                        <telerik:RadLabel ID="lblActualFo" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDACTUALFO","{0:n2}") %>'></telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left">
                                        <telerik:RadLabel ID="lblDO1" runat="server" Text="DO"></telerik:RadLabel>
                                    </th>
                                    <td>
                                        <telerik:RadLabel ID="lblActualDo" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDACTUALDO","{0:n2}") %>'></telerik:RadLabel>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Remarks" HeaderStyle-Width="7%">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Edit"
                                CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                ToolTip="Edit"><span class="icon"><i class="fas fa-edit"></i></span></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="415px" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>

    </form>
</body>
</html>
