<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsRHWorkCalenderRemarks.aspx.cs"
    Inherits="VesselAccountsRHWorkCalenderRemarks" %>

<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style>
            .RadRadioButtonList.rbHorizontalList .RadRadioButton {
                min-width: 100px;
                text-align: left;
            }
        </style>
        <style>
            .RadCheckBoxList span.rbText.rbToggleCheckbox {
                text-align: left;
            }
        </style>
        <%--<style type="text/css">
            .RadCheckBox {
                width: 99% !important;
            }

            .rbText {
                text-align: left;
                width: 99% !important;
            }

            .rbVerticalList {
                width: 99% !important;
            }
        </style>--%>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRestHourWorkCalenderRemarks" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Status ID="ucStatus" runat="server" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuRHGeneral" runat="server" OnTabStripCommand="RHGeneral_TabStripCommand" Title="Work Calendar Remarks"></eluc:TabStrip>
            <br />
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmpName" runat="server" CssClass="input" Enabled="false" Width="240px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true" CssClass="input"
                            Enabled="false" Width="240px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDate" runat="server" CssClass="input" Enabled="false" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblHour" runat="server" Text="Hour"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtHour" runat="server" CssClass="input txtNumber" MaxLength="2"
                            Enabled="false" Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReportingDay" runat="server" Text="Reporting Day"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtReportingday" runat="server" CssClass="input txtNumber" Enabled="false" Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadRadioButtonList ID="rbtnadvanceretard" runat="server" Enabled="false" Direction="Horizontal">
                            <Items>
                                <telerik:ButtonListItem Text="IDL W-E" Value="1" />
                                <telerik:ButtonListItem Text="IDL E-W" Value="2" />
                                <telerik:ButtonListItem Text="None" Value="0" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                    <td colspan="2">
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
                    <td colspan="2">
                        <telerik:RadRadioButtonList ID="rbnhourchange" runat="server" Enabled="false" Direction="Horizontal">
                            <Items>
                                <telerik:ButtonListItem Text="Advance" Value="1" />
                                <telerik:ButtonListItem Text="Retard" Value="2" />
                                <telerik:ButtonListItem Text="Reset" Value="0" Selected="True" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                    <td colspan="2">
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
            <table width="95%" cellpadding="1" cellspacing="1">
                <tr valign="top">
                    <th>
                        <telerik:RadLabel ID="lblReasonsforNC" runat="server" Text="Reasons for NC"></telerik:RadLabel>
                    </th>
                    <th>
                        <telerik:RadLabel ID="lblSystemCauses" runat="server" Text="System Causes"></telerik:RadLabel>
                    </th>
                    <th>
                        <telerik:RadLabel ID="lblCorrectiveActions" runat="server" Text="Corrective Actions"></telerik:RadLabel>
                    </th>
                </tr>
                <tr valign="top">
                    <td>
                        <telerik:RadCheckBoxList runat="server" ID="chkReason" Columns="1"></telerik:RadCheckBoxList>
                        <br />
                        <telerik:RadLabel ID="lblIfothersspecify" runat="server" Text="If others, specify,"></telerik:RadLabel>
                        <telerik:RadTextBox ID="txtremarks" runat="server" CssClass="input" TextMode="MultiLine" Resize="Both"
                            Height="70px" Width="90%">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadCheckBoxList runat="server" ID="chkSystemCause" Columns="1"></telerik:RadCheckBoxList>
                        <br />
                        <telerik:RadLabel ID="lblSysCauseIfothersspecify" runat="server" Text="If others, specify,"></telerik:RadLabel>
                        <telerik:RadTextBox ID="txtSysCause" runat="server" CssClass="input" TextMode="MultiLine" Resize="Both"
                            Height="70px" Width="90%">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadCheckBoxList runat="server" ID="chkCorrectiveAction" Columns="1">
                        </telerik:RadCheckBoxList>
                        <br />
                        <telerik:RadLabel ID="lblCorrectiveRemarkIfothersspecify" runat="server" Text="If others, specify,"></telerik:RadLabel>
                        <telerik:RadTextBox ID="txtCorrectiveRemark" runat="server" CssClass="input" TextMode="MultiLine" Resize="Both"
                            Height="70px" Width="90%">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <telerik:RadGrid ID="gvWorkHourRecord" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                            CellSpacing="0" GridLines="None" EnableHeaderContextMenu="true" GroupingEnabled="false" Style="margin-bottom: 0px" OnNeedDataSource="gvWorkHourRecord_NeedDataSource"
                            EnableViewState="false" OnItemDataBound="gvWorkHourRecord_ItemDataBound" OnSortCommand="gvWorkHourRecord_SortCommand">
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
                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblHour" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTINGHOUR") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Non Compliance">
                                        <HeaderStyle HorizontalAlign="Left" Width="70%" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
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
                        </telerik:RadGrid>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
