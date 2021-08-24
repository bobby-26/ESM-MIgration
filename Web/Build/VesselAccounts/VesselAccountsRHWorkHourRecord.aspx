<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsRHWorkHourRecord.aspx.cs"
    Inherits="VesselAccountsRHWorkHourRecord" %>

<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="WorkSheet" Src="~/UserControls/UserControlRHTimeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style>
            .RadRadioButtonList.rbHorizontalList .RadRadioButton {
                /*min-width: 70px;*/
                text-align: left;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRestHourWOrkCalender" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuWorkHour" runat="server" OnTabStripCommand="MenuWorkHour_TabStripCommand" Title="Rest Work Hour"></eluc:TabStrip>
            <table runat="server" width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblNotes" Font-Bold="true" runat="server" Text="Notes:"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lbl1Clickon"
                            runat="server" ForeColor="Blue" Text="1.Click on the time strip hours strip to mark the attendance. Click
                                    once to record 1.0 hour, click again to change it to 0.5. The third click will reset
                                    the cell to 0.0.">
                        </telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lbl2Theapportioned" ForeColor="Blue"
                            runat="server" Text="2.The apportioned time will be displayed, if the clock is advanced
                                    or retarded during the hour">
                        </telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lbl3Thetotalworkhoursandresthoursareshownattheendofthetabstrip"
                            runat="server" Text="3.The total work hours and rest hours are shown at the end of the
                                    tab strip"
                            ForeColor="Blue">
                        </telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td width="10%">
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td width="40%">
                        <telerik:RadTextBox ID="txtEmpName" runat="server" Enabled="false" Width="210px"></telerik:RadTextBox>
                    </td>
                    <td width="10%">
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td width="40%">
                        <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true" Enabled="false" Width="210px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDate" runat="server" Enabled="false" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblHour" runat="server" Text="Hour"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtHour" runat="server" CssClass="input txtNumber" MaxLength="2"
                            Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReportingDay" runat="server" Text="Reporting Day"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtReportingday" runat="server" CssClass="input txtNumber" Enabled="false" />
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadRadioButtonList ID="rbtnadvanceretard" runat="server" Enabled="false" Direction="Horizontal">
                            <Items>
                                <telerik:ButtonListItem Text="IDL W-E" Value="1" />
                                <telerik:ButtonListItem Text="IDL E-W" Value="2" />
                                <telerik:ButtonListItem Text="None" Value="0" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rbtnworkplace" runat="server" Enabled="false" Direction="Horizontal">
                            <Items>
                                <telerik:ButtonListItem Text="Work at Port" Value="1" />
                                <telerik:ButtonListItem Text="Work at Sea" Value="2" />
                                <telerik:ButtonListItem Text="Work at Sea/Port" Value="3" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadRadioButtonList ID="rbnhourchange" runat="server" Enabled="false" Direction="Horizontal">
                            <Items>
                                <telerik:ButtonListItem Text="Advance" Value="1" />
                                <telerik:ButtonListItem Text="Retard" Value="2" />
                                <telerik:ButtonListItem Text="Reset" Value="0" Selected="True" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rbnhourvalue" runat="server" Enabled="false" Direction="Horizontal">
                            <Items>
                                <telerik:ButtonListItem Text="0.5 Hour" Value="1" />
                                <telerik:ButtonListItem Text="1.0 Hour" Value="2" />
                                <telerik:ButtonListItem Text="2.0 Hour" Value="3" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td width="10%">
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks" Font-Bold="true"></telerik:RadLabel>
                    </td>
                    <td width="90%">
                        <telerik:RadTextBox runat="server" ID="txtRemarks" CssClass="input" TextMode="MultiLine"
                            Height="80px" Width="79.5%" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblprevoius" Font-Bold="true" runat="server"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Panel ID="panel1" runat="server" Enabled="false">
                            <eluc:WorkSheet Id="PreSeaWSheet" runat="server" CssClass="input txtNumber" />
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblCurrent" Font-Bold="true" runat="server"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <eluc:WorkSheet Id="SeaWSheet" runat="server" OnTimeStripCommand="SeaWSheet_OnTimeStripCommand"
                            CssClass="input txtNumber" />
                    </td>
                </tr>
            </table>
            <telerik:RadGrid ID="gvWorkHourRecord" runat="server" Height="30%" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" EnableHeaderContextMenu="true" GroupingEnabled="false"
                Width="100%" CellPadding="3" EnableViewState="false" OnItemDataBound="gvWorkHourRecord_ItemDataBound">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Hour">
                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblHour" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTINGHOUR") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Non Compliance">
                            <HeaderStyle HorizontalAlign="Left" Width="70%" />
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblException" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNONCOMPLIANCE") %>'
                                    ForeColor="Red">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Legends">
                            <HeaderStyle HorizontalAlign="Left" Width="20%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Image ID="imgS1" runat="server" ImageUrl="<%$ PhoenixTheme:images/spacer.png %>"
                                    Width="16" Height="16" />
                                <eluc:ToolTip ID="ucToolTipnc1" runat="server" />
                                <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="16"
                                    height="16" />
                                <asp:Image ID="imgS2" runat="server" ImageUrl="<%$ PhoenixTheme:images/spacer.png %>"
                                    Width="16" Height="16" />
                                <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="16"
                                    height="16" />
                                <eluc:ToolTip ID="ucToolTipnc2" runat="server" />
                                <asp:Image ID="imgS3" runat="server" ImageUrl="<%$ PhoenixTheme:images/spacer.png %>"
                                    Width="16" Height="16" />
                                <img id="Img3" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="16"
                                    height="16" />
                                <eluc:ToolTip ID="ucToolTipnc3" runat="server" />
                                <asp:Image ID="imgS4" runat="server" ImageUrl="<%$ PhoenixTheme:images/spacer.png %>"
                                    Width="16" Height="16" />
                                <img id="Img4" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="16"
                                    height="16" />
                                <eluc:ToolTip ID="ucToolTipnc4" runat="server" />
                                <asp:Image ID="imgS5" runat="server" ImageUrl="<%$ PhoenixTheme:images/spacer.png %>"
                                    Width="16" Height="16" />
                                <img id="Img5" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="16"
                                    height="16" />
                                <eluc:ToolTip ID="ucToolTipnc5" runat="server" />
                                <asp:Image ID="imgO1" runat="server" ImageUrl="<%$ PhoenixTheme:images/spacer.png %>"
                                    Width="16" Height="16" />
                                <img id="Img6" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="16"
                                    height="16" />
                                <eluc:ToolTip ID="ucToolTipnc6" runat="server" />
                                <asp:Image ID="imgO2" runat="server" ImageUrl="<%$ PhoenixTheme:images/spacer.png %>"
                                    Width="16" Height="16" />
                                <eluc:ToolTip ID="ucToolTipnc7" runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <eluc:Status ID="ucStatus" runat="server" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
