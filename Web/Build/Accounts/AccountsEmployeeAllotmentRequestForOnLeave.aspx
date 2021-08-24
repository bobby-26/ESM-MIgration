<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsEmployeeAllotmentRequestForOnLeave.aspx.cs" Inherits="AccountsEmployeeAllotmentRequestForOnLeave" %>


<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Month" Src="~/UserControls/UserControlMonth.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Year" Src="~/UserControls/UserControlYear.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>EmployeeAllotmentDetails</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function CheckAll(chkAll) {
                var gv = document.getElementById("<%=gvAllotmentOnLeave.ClientID %>");
                for (i = 1; i < gv.rows.length; i++) {
                    gv.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = chkAll.checked;
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmEmployeeAllotmentDetails" runat="server" autocomplete="off" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <eluc:TabStrip ID="MenuOrderForm" runat="server" OnTabStripCommand="OrderForm_TabStripCommand"
            TabStrip="true" />
        <eluc:TabStrip ID="MenuPV" runat="server" OnTabStripCommand="MenuPV_TabStripCommand"></eluc:TabStrip>
        <table width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblVesselName" runat="server" Text="Vessel"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Vessel ID="ddlVessel" runat="server" AppendDataBoundItems="true" VesselsOnly="true"
                        CssClass="input" Width="50%" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblMonth" runat="server" Text="Month"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Month ID="ddlMonth" runat="server" Width="140px"></eluc:Month>
                </td>
                <td>
                    <telerik:RadLabel ID="lblYear" runat="server" Text="Year"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Year ID="ddlYear" runat="server" Width="140px" OrderByAsc="false"></eluc:Year>
                </td>
            </tr>
        </table>
        <%-- <table width="100%">
            <tr>
                <td>--%>
        <eluc:TabStrip ID="MenuAllotment" runat="server" OnTabStripCommand="MenuAllotment_TabStripCommand"></eluc:TabStrip>
        <telerik:RadGrid ID="gvAllotmentOnLeave" runat="server" AutoGenerateColumns="False" Font-Size="11px"
            Width="100%" CellPadding="3" OnItemCommand="gvAllotmentOnLeave_ItemCommand" OnItemDataBound="gvAllotmentOnLeave_ItemDataBound"
            AllowSorting="true" ShowHeader="true" EnableViewState="false" OnNeedDataSource="gvAllotmentOnLeave_NeedDataSource">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDALLOTMENTID">
                <ColumnGroups>
                    <telerik:GridColumnGroup HeaderText="Allotment Request" Name="Allotment Request" HeaderStyle-HorizontalAlign="Center">
                    </telerik:GridColumnGroup>
                </ColumnGroups>
                <ColumnGroups>
                    <telerik:GridColumnGroup HeaderText="" Name="Group1" HeaderStyle-HorizontalAlign="Center">
                    </telerik:GridColumnGroup>
                </ColumnGroups>
                <ColumnGroups>
                    <telerik:GridColumnGroup HeaderText="Payment" Name="Payment" HeaderStyle-HorizontalAlign="Center">
                    </telerik:GridColumnGroup>
                </ColumnGroups>
                <ColumnGroups>
                    <telerik:GridColumnGroup HeaderText="" Name="Group2" HeaderStyle-HorizontalAlign="Center">
                    </telerik:GridColumnGroup>
                </ColumnGroups>
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
                    <telerik:GridTemplateColumn>
                        <%--<HeaderTemplate>
                            <telerik:RadCheckBox ID="chkAll" runat="server" Visible="false" Text="All&nbsp;&nbsp;" TextAlign="Left" 
                                onclick="CheckAll(this)" />
                        </HeaderTemplate>--%>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" AlternateText="Unlock" ImageUrl="<%$ PhoenixTheme:images/on-signer.png %>" ID="cmdtooltip" Visible="false"></asp:ImageButton>
                            <img id="Img6" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                            <telerik:RadCheckBox ID="chkItem" runat="server" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Date" ColumnGroupName="Allotment Request">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRank" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblEmpFileNo" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblEmployeeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblEmployeeName" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblAllotmentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALLOTMENTID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblRequestDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTDATE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Number" ColumnGroupName="Allotment Request">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRequestNo" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDREQUESTNUMBER") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblVesselID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblMonth" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMONTH") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblYear" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDYEAR") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblUnlockYN" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNLOCKYN") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Allotment Type" ColumnGroupName="Group1">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAllotmentTypeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALLOTMENTTYPE") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblAllotmentType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALLOTMENTTYPENAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Amount(USD)" ColumnGroupName="Group1">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblCurrencyId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Status" ColumnGroupName="Group1">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRequestStatus" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTSTATUS") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTSTATUSNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Voucher No." ColumnGroupName="Payment">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblPVNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTVOUCHERNUMBER") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Date" ColumnGroupName="Payment">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblPaymentDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTDATE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Voucher No." ColumnGroupName="Group2">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblvoucherid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblVoucherno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERNO") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Created By" ColumnGroupName="Group2">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDCREATEDBY") %> <b>On </b><%# DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action" ColumnGroupName="Group2">
                        <ItemTemplate>
                            <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                Visible="false" CommandName="CANCELREQUEST" ID="cmdCancelRequest" ToolTip="Cancel Request"></asp:ImageButton>
                            <img id="Img4" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                            <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/covering-letter.png %>"
                                CommandName="VERIFICATION" ID="cmdverification" ToolTip="Verification check"></asp:ImageButton>
                            <img id="Img3" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                            <asp:ImageButton runat="server" AlternateText="history" ImageUrl="<%$ PhoenixTheme:images/te_pqtes.png %>"
                                CommandName="HISTORY" ID="cmdHistory" Visible="true" ToolTip="History"></asp:ImageButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
        <eluc:Status ID="ucStatus" runat="server" Visible="false" />
    </form>
</body>
</html>
