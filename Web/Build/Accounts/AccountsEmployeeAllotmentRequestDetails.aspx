<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsEmployeeAllotmentRequestDetails.aspx.cs" Inherits="AccountsEmployeeAllotmentRequestDetails" %>


<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Employee Allotment Details</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function CheckAll(chkAll) {
                var gv = document.getElementById("<%=gvAllotment.ClientID %>");
                for (i = 1; i < gv.rows.length; i++) {
                    gv.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = chkAll.checked;
                }
            }
        </script>
        <style type="text/css">
            .RadGrid .rgHeader, .RadGrid th.rgResizeCol, .RadGrid .rgRow td, .RadGrid .rgAltRow td {
                padding-left: 2px !important;
                padding-right: 2px !important;
                align-items: center !important;
            }
        </style>
    </telerik:RadCodeBlock>
</head>

<body>
    <form id="frmEmployeeAllotmentDetails" runat="server" autocomplete="off" submitdisabledcontrols="true">
        <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="90%">
            <eluc:Status ID="ucStatus" runat="server" Visible="false" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuOrderForm" runat="server" OnTabStripCommand="OrderForm_TabStripCommand"
                TabStrip="true" />
            <eluc:TabStrip ID="MenuPV" runat="server" OnTabStripCommand="MenuPV_TabStripCommand"></eluc:TabStrip>
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVesselName" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="180"></telerik:RadTextBox></td>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployeeName" runat="server" CssClass="readonlytextbox" Width="240px"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFileNo" runat="server" Text="File No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFileNo" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="80px">
                        </telerik:RadTextBox>
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRank" runat="server" CssClass="readonlytextbox" Width="80px"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMonthOf" runat="server" Text="Month Of"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtMonthAndYear" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="80px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="10">
                        <hr />
                        <b>
                            <telerik:RadLabel ID="lblPortageBill" runat="server" Text=" Portage Bill"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
            </table>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvPB" Height="28%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemDataBound="gvPB_ItemDataBound" OnNeedDataSource="gvPB_NeedDataSource"
                ShowFooter="True" ShowHeader="true" EnableViewState="true" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" HeaderStyle-HorizontalAlign="Center"
                    AutoGenerateColumns="false" CommandItemDisplay="Top" DataKeyNames="FLDPORTAGEBILLID,FLDSIGNONOFFID,FLDEMPLOYEEID">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Earnings" Name="Earnings" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Deductions" Name="Deductions" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="" Visible="false">
                            <HeaderStyle Width="0px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Image ID="imgFinalBal" runat="server" ImageUrl="<%$ PhoenixTheme:images/balance-mismatch.png %>" ToolTip="Final Balance Mismatch" Visible="false" />
                                <asp:Image ID="imgNegBal" runat="server" ImageUrl="<%$ PhoenixTheme:images/negative-balance.png %>" ToolTip="Negative Balance" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="File No" Visible="false">
                            <HeaderStyle Width="0px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDFILENO"].ToString() %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Employee Name" Visible="false">
                            <HeaderStyle Width="0px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEmployeename" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDEMPLOYEENAME"].ToString() %>'></asp:LinkButton>

                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank" Visible="false">
                            <HeaderStyle Width="0px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDRANKCODE"].ToString()%>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="From">
                            <HeaderStyle Width="80px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDFROMDATE"])%>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="To">
                            <HeaderStyle Width="80px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDTODATE"])%>
                            </ItemTemplate>
                            <FooterStyle Wrap="false" Font-Bold="true" />
                            <FooterTemplate>
                                <telerik:RadLabel ID="lblTotal" runat="server" Text="Total"></telerik:RadLabel>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Days">
                            <HeaderStyle Width="80px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDDAYS"].ToString()%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="2" EnableColumnClientFreeze="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <b>
                <telerik:RadLabel ID="lblAllotment" runat="server" Text=" Allotment"></telerik:RadLabel>
            </b>
            <eluc:TabStrip ID="MenuAllotment" runat="server" OnTabStripCommand="MenuAllotment_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvAllotment" Height="36%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvAllotment_ItemCommand" OnItemDataBound="gvAllotment_ItemDataBound" OnNeedDataSource="gvAllotment_NeedDataSource"
                ShowFooter="false" ShowHeader="true" EnableViewState="true" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" HeaderStyle-HorizontalAlign="Center"
                    AutoGenerateColumns="false" DataKeyNames="FLDALLOTMENTID">
                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Allotment Request" Name="Allotment Request" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Voucher No." Name="Voucher No." HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="">
                            <HeaderStyle Width="3%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Unlock" ImageUrl="<%$ PhoenixTheme:images/on-signer.png %>"
                                    ID="cmdtooltip" Visible="false"></asp:ImageButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="">
                            <HeaderStyle Width="22px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadCheckBox ID="chkItem" runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Date" ColumnGroupName="Allotment Request">
                            <HeaderStyle Width="8%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEmployeeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblAllotmentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALLOTMENTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRequestDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Number" ColumnGroupName="Allotment Request">
                            <HeaderStyle Width="12%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>

                                <telerik:RadLabel ID="lblRequestNo" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDREQUESTNUMBER") %>' ToolTip='<%#DataBinder.Eval(Container,"DataItem.FLDREQUESTNUMBER") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblMonth" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMONTH") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblYear" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDYEAR") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblUnlockYN" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNLOCKYN") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCancelYN" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCANCELYN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Allotment Type" HeaderStyle-Wrap="true">
                            <HeaderStyle Width="10%" />
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAllotmentTypeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALLOTMENTTYPE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblAllotmentType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALLOTMENTTYPENAME") %>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDALLOTMENTTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount (USD)" HeaderStyle-Wrap="true">
                            <HeaderStyle Width="8%" />
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCurrencyId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Status">
                            <HeaderStyle Width="120px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRequestStatus" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTSTATUS") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTSTATUSNAME") %>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTSTATUSNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="PMV Date" HeaderStyle-Wrap="true">
                            <HeaderStyle Width="8%" />
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPaymentDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="PMV" ColumnGroupName="Voucher No.">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPVNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTVOUCHERNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Charging" ColumnGroupName="Voucher No.">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblJVvoucherid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJVVOUCHERID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblJVVoucherno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJVVOUCHERNO") %>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Bank Payment" ColumnGroupName="Voucher No.">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblvoucherid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVoucherno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERNO") %>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Created By">
                            <HeaderStyle Width="190px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDCREATEDBY") %> <b>On </b><%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE", "{0:dd/MMM/yyyy}")) %>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="10%">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">
                                    Action
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Unlock" ImageUrl="<%$ PhoenixTheme:images/period-unlock.png %>"
                                    CommandName="UNLOCK" ID="cmdUnlock"
                                    Visible="false" ToolTip="Unlock Request"></asp:ImageButton>
                                <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    Visible="false" CommandName="CANCELREQUEST"
                                    ID="cmdCancelRequest" ToolTip="Cancel Request"></asp:ImageButton>
                                <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" ID="cmdEdit" Visible="false"
                                    ToolTip="Bank Details update"></asp:ImageButton>
                                <img id="Img5" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/annexure.png %>"
                                    CommandName="REIMBREC" ID="cmdReimRec"
                                    ToolTip="Reim/Rec Request" Visible="false"></asp:ImageButton>
                                <%--                                        <img id="Img3" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                                              <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/covering-letter.png %>"
                                                                  CommandName="SIDELETTER" ID="cmdSideLetter"
                                                                  ToolTip="SideLetter Request"></asp:ImageButton>--%>
                                <img id="Img4" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/covering-letter.png %>"
                                    CommandName="VERIFICATION" ID="cmdverification"
                                    ToolTip="Verification check"></asp:ImageButton>
                                <asp:ImageButton runat="server" AlternateText="history" ImageUrl="<%$ PhoenixTheme:images/te_pqtes.png %>"
                                    CommandName="HISTORY" ID="cmdHistory"
                                    Visible="true" ToolTip="History"></asp:ImageButton>
                                <img id="Img3" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="2" EnableColumnClientFreeze="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
