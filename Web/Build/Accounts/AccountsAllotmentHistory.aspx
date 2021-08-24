<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAllotmentHistory.aspx.cs" Inherits="Accounts_AccountsAllotmentHistory" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Allotment History</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>

    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />
    <form id="frmRegistersRank" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployeeName" runat="server" CssClass="readonlytextbox" Width="150px"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFileNo" runat="server" Text="File No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFileNo" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="150">
                        </telerik:RadTextBox>
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRank" runat="server" CssClass="readonlytextbox" Width="150px"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVesselName" runat="server" CssClass="readonlytextbox" Width="150" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMonthOf" runat="server" Text="Month Of"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtMonthAndYear" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="150">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <span style="float: left">
                            <telerik:RadLabel ID="lblallotmentType0" runat="server" Text="Allotment Type"></telerik:RadLabel>
                        </span></td>
                    <td>
                        <span style="float: left">
                            <telerik:RadTextBox ID="txtallotmentType" runat="server" CssClass="readonlytextbox"
                                ReadOnly="true" Width="150">
                            </telerik:RadTextBox>
                        </span></td>
                </tr>
                <tr>
                    <td>
                        <span style="float: left">
                            <telerik:RadLabel ID="lblamount" runat="server" Text="Allotment Amount"></telerik:RadLabel>
                        </span></td>
                    <td>
                        <span style="float: left">
                            <telerik:RadTextBox ID="txtAllotmentAmount" runat="server" CssClass="readonlytextbox"
                                ReadOnly="true" Width="150">
                            </telerik:RadTextBox>

                        </span></td>
                    <td>
                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="Request No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtReference" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="150">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>
            </table>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvAccountHistory" Height="80%" runat="server" AllowCustomPaging="true" AllowSorting="true"
                AllowPaging="false" CellSpacing="0" GridLines="None" EnableViewState="false"
                ShowFooter="false" ShowHeader="true" OnNeedDataSource="gvAccountHistory_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Status">
                            <HeaderStyle Width="30%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblAllotmentId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALLOTMENTID") %>'></asp:Label>
                                <asp:Label ID="lblstatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Created By">
                            <HeaderStyle Width="40%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblCreatedby" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDBY") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Created Date">
                            <HeaderStyle Width="12%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblCreateddate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE") %>'></asp:Label>
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

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
