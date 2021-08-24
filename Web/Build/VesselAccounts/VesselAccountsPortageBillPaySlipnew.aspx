<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsPortageBillPaySlipnew.aspx.cs"
    Inherits="VesselAccountsPortageBillPaySlipnew" %>

<!DOCTYPE html>
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselCrew" Src="~/UserControls/UserControlVesselEmployee.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Payslip</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script lang="javascript" type="text/javascript">
            function cmdPrint_Click() {
                document.getElementById('cmdPrint');
                window.print();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel runat="server" ID="pnlNTBRManager" Height="50%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFileNo" runat="server" Text="File No"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtFileNo" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="150px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFirstName" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="150px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtLastName" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="150px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRankNationality" runat="server" Text="Rank / Nationality"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRank" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="120px">
                        </telerik:RadTextBox>
                        /
                            <telerik:RadTextBox ID="txtNationality" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="120px">
                            </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="SeamanBookNo" runat="server" Text="Seaman Book No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSeamanBook" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="150px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPortofEngagement" runat="server" Text="Port of Engagement"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPort" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="150px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblContractPayCommencementDate" runat="server" Text="Contract/ Pay Commencement Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDate" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="150px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblContractPeriod" runat="server" Text="Contract Period"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtContractPeriod" runat="server" CssClass="readonlytextbox" Width="90px" />
                        +/-
                            <telerik:RadTextBox ID="txtPlusMinusPeriod" runat="server" CssClass="readonlytextbox" Width="90px" />
                        (Months)
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVesselJoining" runat="server" Text="Vessel Joining"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVessel" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="150px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblStartDate" runat="server" Text="Start Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtStartDate" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="150px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblEndDate" runat="server" Text="End Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtEndDate" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="150px" />
                    </td>
                </tr>
            </table>
            <br />
            <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
                <center>
                        <b>
                            <h3>
                                <td>
                                  <telerik:RadLabel ID="lblPayslipfortheMonthof" runat="server" Text="Payslip for the Month of"></telerik:RadLabel>
                                  <telerik:RadLabel ID="lbldate" runat="server"><%= DateTime.Parse(Request.QueryString["pbd"]).ToString("MMM yyyy") %></telerik:RadLabel> 
                            </h3>
                        </b>
                    </center>
            </telerik:RadCodeBlock>
            <eluc:TabStrip ID="MenuPBExcel" runat="server" OnTabStripCommand="MenuPBExcel_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvPB" runat="server" AutoGenerateColumns="False" Width="100%" Height="95%" ShowHeader="true" EnableViewState="false"
                CellSpacing="0" GridLines="None" GroupingEnabled="false" AllowPaging="true" AllowSorting="true" EnableHeaderContextMenu="true" ShowFooter="true"
                OnNeedDataSource="gvPB_NeedDataSource" OnItemDataBound="gvPB_ItemDataBound" OnItemCommand="gvPB_ItemCommand" OnCustomAggregate="gvPB_CustomAggregate">
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center"
                    GroupHeaderItemStyle-Wrap="false" Width="100%">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Earnings">
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDECOMPONENTNAME"]%>
                            </ItemTemplate>
                            <FooterTemplate>
                                <telerik:RadLabel ID="lblTotalEarnings" runat="server" Text="Total Earnings"></telerik:RadLabel>
                            </FooterTemplate>
                            <FooterStyle Wrap="false" HorizontalAlign="Left" Font-Bold="true" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount" UniqueName="EAMOUNT" DataField="FLDEAMOUNT" FooterStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" Aggregate="Sum" FooterAggregateFormatString="{0}">
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDEAMOUNT"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Deductions" UniqueName="DEDUCTIONS">
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDDCOMPONENTNAME"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount" UniqueName="DAMOUNT" DataField="FLDDAMOUNT" FooterStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" Aggregate="Sum" FooterAggregateFormatString="{0}">
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDDAMOUNT"]%>
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
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder" Scrolling-EnableNextPrevFrozenColumns="true">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" ResizeGridOnColumnResize="false" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
        <eluc:Status ID="ucStatus" runat="server" />
    </form>
</body>
</html>
