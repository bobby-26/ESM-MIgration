<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsOfficePortageBill.aspx.cs"
    Inherits="AccountsOfficePortageBill" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register Src="~/UserControls/UserControlVessel.ascx" TagName="Vessel" TagPrefix="eluc" %>
<%@ Register Src="~/UserControls/UserControlStatus.ascx" TagName="Status" TagPrefix="eluc" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Finalized Portage Bill</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript">
            $(document).ready(function () {
                var browserHeight = $telerik.$(window).height();
                $("#gvPB").height(browserHeight - 20);
            });
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />
    <form id="frmRegistersRank" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuPB" runat="server" OnTabStripCommand="MenuPB_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>

            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselName" runat="server" Text="Vessel Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ddlVessel" runat="server" AppendDataBoundItems="true" VesselsOnly="true" AutoPostBack="true"
                            CssClass="input" OnTextChangedEvent="FilterChanged" Width="240px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDropDownList ID="ddlStatus" runat="server" CssClass="input" AutoPostBack="true"
                            OnTextChanged="FilterChanged" Width="240px">
                            <Items>
                                <telerik:DropDownListItem Value="1" Text="Finalized by Vessel" />
                                <telerik:DropDownListItem Value="0" Text="Confirmed by Office" />
                                <telerik:DropDownListItem Value="2" Text="All" />
                            </Items>
                        </telerik:RadDropDownList>
                        <span id="Span1" class="icon" runat="server"><i class="fas fa-info-circle" style="align-content: center"></i></span>
                        <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="RadToolTip1" Width="300px" ShowEvent="onmouseover"
                            RelativeTo="Element" Animation="Fade" TargetControlID="Span1" IsClientID="true"
                            HideEvent="ManualClose" Position="BottomLeft" EnableRoundedCorners="true" ContentScrolling="Auto">
                            <table cellpadding="1" cellspacing="1">
                                <tr>
                                    <td style="color: Blue">
                                        <telerik:RadLabel ID="lblNote" runat="server" Text="Note : Click on the vessel name to see the mismatch between office final balance and vessel closing balance"></telerik:RadLabel>
                                    </td>
                                </tr>
                            </table>
                        </telerik:RadToolTip>
                    </td>
                </tr>
            </table>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvPB" Height="88%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" OnItemCommand="gvPB_ItemCommand" OnItemDataBound="gvPB_ItemDataBound" EnableViewState="false"
                ShowFooter="false" ShowHeader="true" OnNeedDataSource="gvPB_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDPORTAGEBILLID">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="">
                            <HeaderStyle Width="6%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Image ID="imgFinalBal" runat="server" ImageUrl="<%$ PhoenixTheme:images/currency_mismatch.png %>" Visible="false" />
                                <asp:Image ID="imgNegBal" runat="server" ImageUrl="<%$ PhoenixTheme:images/on-signer.png %>" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel">
                            <HeaderStyle Width="20%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkVessel" runat="server" CommandName="SELECT" Text='<%# ((DataRowView)Container.DataItem)["FLDVESSELNAME"].ToString() %>' ToolTip='<%# ((DataRowView)Container.DataItem)["FLDVESSELNAME"].ToString() %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Start">
                            <HeaderStyle Width="12%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDate" runat="server" Visible="false" Text='<%# string.Format("{0:dd/MMM/yyyy}", ((DataRowView)Container.DataItem)["FLDENDDATE"]) %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselid" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDVESSELID"] %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblPortagebillId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDPORTAGEBILLID"] %>'></telerik:RadLabel>
                                <%# string.Format("{0:dd/MMM/yyyy}", ((DataRowView)Container.DataItem)["FLDSTARTDATE"]) %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="End">
                            <HeaderStyle Width="12%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# string.Format("{0:dd/MMM/yyyy}", ((DataRowView)Container.DataItem)["FLDENDDATE"]) %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel Final Balance">
                            <HeaderStyle Width="20%" HorizontalAlign="Right" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDVESSELFINALBALANCE"].ToString() %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status">
                            <HeaderStyle Width="12%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDSTATUS"].ToString() == "1" ? "Confirmed by Office" : "Finalized by Vessel"%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="View PortageBill" CommandName="VIEW" ID="cmdView" ToolTip="View PortageBill">
                                    <span class="icon"><i class="fas fa-binoculars-tv"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Confirm PortageBill" CommandName="CONFIRM" ID="cmdConfirm" ToolTip="Confirm PortageBill"><span class="icon"><i class="fas fa-award"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Credit Leave" CommandName="CREDITLEAVE" ID="cmdCreditLeave" ToolTip="Credit Leave"><span class="icon"><i class="fas fa-coins"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <table cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <img id="Img2" src="<%$ PhoenixTheme:images/currency_mismatch.png%>" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFinalBalanceMismatchbetweenOfficeFinalBalanceandVesselClosingBalance" runat="server" Text="* Final Balance Mismatch between Office Final Balance and Vessel Closing Balance"></telerik:RadLabel>
                    </td>
                    <td>
                        <img id="Img1" src="<%$ PhoenixTheme:images/on-signer.png%>" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCrewwithNegativeBalances" runat="server" Text="* Crew with Negative Balances"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
            <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
