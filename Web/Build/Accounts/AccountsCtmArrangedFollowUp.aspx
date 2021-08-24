<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsCtmArrangedFollowUp.aspx.cs" Inherits="AccountsCtmArrangedFollowUp" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStripTelerik" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselCrew" Src="~/UserControls/UserControlVesselEmployee.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Arranged CTM Follow Up</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />
    <form id="frmRegistersRank" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>

        <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text="" Visible="false"></eluc:Status>
            <table width="80%" cellpadding="1" cellspacing="1">
                <tr>

                    <td>
                        <telerik:RadLabel ID="lblFromDate" runat="server" Text="ETA From"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtFromDate" runat="server" CssClass="input" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblToDate" runat="server" Text="ETA To"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtToDate" runat="server" CssClass="input" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblArrangedVia" runat="server" Text="Arranged Via"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ddlArrangedVia" runat="server" AppendDataBoundItems="true" HardTypeCode="213" AutoPostBack="true"
                            ShortNameFilter="HCY,RME" CssClass="input" OnSelectedIndexChanged="Arraivedvia_Changed" />
                    </td>
                    <td>Exclude Fully Received</td>
                    <td>
                        <telerik:RadCheckBox ID="chkStatus" runat="server" AutoPostBack="true" Text="" />
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenuCTM" runat="server" OnTabStripCommand="MenuCTM_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvCTM" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvCTM_NeedDataSource" AllowMultiRowSelection="true" FilterType="CheckList"
                EnableViewState="false" EnableHeaderContextMenu="true" Width="100%" Height="93%"
                OnItemDataBound="gvCTM_ItemDataBound" OnItemCommand="gvCTM_ItemCommand"
                ShowFooter="false" ShowHeader="true" OnSortCommand="gvCTM_SortCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDCAPTAINCASHID" TableLayout="Fixed" CommandItemDisplay="Top">

                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Arranged" Name="Arranged" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Received" Name="Received" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="102px"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="Label1" runat="server">
                                    Action
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>


                                <asp:ImageButton runat="server" AlternateText="Email" ImageUrl="<%$ PhoenixTheme:images/Email.png %>"
                                    CommandName="SENDMAIL" ID="cmdEmail"
                                    Visible="false" ToolTip="Send mail to vessel"></asp:ImageButton>
                                <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="PMV No.">
                            <HeaderStyle Width="120px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTVOUCHERNUMBER")%>

                                <telerik:RadLabel ID="lblVesselid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselMail" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELMAILID")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblUserMail" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERMAILID")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDate" runat="server" Visible="false" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDDATE")%>'></telerik:RadLabel>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel">
                            <HeaderStyle Width="115px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="ETA" AllowSorting="true" SortExpression="FLDETA">
                            <HeaderStyle Width="102px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <%# string.Format("{0:dd/MMM/yyyy}", DataBinder.Eval(Container,"DataItem.FLDETA"))%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Currency">
                            <HeaderStyle Width="102px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Via" ColumnGroupName="Arranged">
                            <HeaderStyle Width="102px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDARRANGEDVIA")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount" ColumnGroupName="Arranged">
                            <HeaderStyle Width="102px" HorizontalAlign="Right" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDAMOUNTARRANGED")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount" ColumnGroupName="Received">
                            <HeaderStyle Width="102px" HorizontalAlign="Right" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReceivedamount" runat="server" Text=' <%# DataBinder.Eval(Container,"DataItem.FLDRECEIVEDAMOUNT")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date" ColumnGroupName="Received">
                            <HeaderStyle Width="102px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# string.Format("{0:dd/MMM/yyyy}", DataBinder.Eval(Container,"DataItem.FLDRECEIVEDDATE"))%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CTM Request Status">
                            <HeaderStyle Width="180px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS")%>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS")%>'></telerik:RadLabel>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Port" AllowSorting="true" SortExpression="FLDSEAPORTNAME">
                            <HeaderStyle Width="140px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSeaPort" runat="server" ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME")%>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME")%>'></telerik:RadLabel>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Delivered By" AllowSorting="true" SortExpression="FLDSUPPLIERNAME">
                            <HeaderStyle Width="150px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lbldeliverby" runat="server" ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDDELIVEREDBY")%>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDDELIVEREDBY")%>'></telerik:RadLabel>

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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                    EnablePostBackOnRowClick="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="2" EnableNextPrevFrozenColumns="true" ScrollHeight="415px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <%--   <asp:Button ID="ucConfirmDelete" runat="server" OnClick="ucConfirmDelete_Click" CssClass="hidden" />--%>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
