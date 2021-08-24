<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAllotmentList.aspx.cs" Inherits="Accounts_AccountsAllotmentList" %>

<!DOCTYPE html>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.VesselAccounts" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselCrew" Src="~/UserControls/UserControlVesselEmployee.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmMessage" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="EntryType" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonToolTip" Src="~/UserControls/UserControlCommonToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Month" Src="~/UserControls/UserControlMonth.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Year" Src="~/UserControls/UserControlYear.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZone.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleet.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlPool" Src="~/UserControls/UserControlPool.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationality.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AccountsAllotmentList</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function DeleteRecord(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmDelete.UniqueID %>", "");
                }
            }
            function DeleteRecords(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmDeleteRecord.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server"></telerik:RadSkinManager>
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="99%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="MenuAllotmentList" runat="server" TabStrip="true" OnTabStripCommand="MenuAllotmentList_TabStripCommand"></eluc:TabStrip>
            <table runat="server" id="table1" style="display:none">
                <tr>

                    <td>
                        <telerik:RadTextBox ID="txtTotalSelectedAmount" runat="server" Visible="false" CssClass="readonlytextbox" Width="150px" Enabled="false"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtNoofTransaction" runat="server" Visible="false" CssClass="readonlytextbox" Width="150px" Enabled="false"></telerik:RadTextBox>
                    </td>

                </tr>
            </table>
            <eluc:TabStrip ID="MenuAllotment" runat="server" OnTabStripCommand="MenuAllotment_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvAllotment" runat="server" AutoGenerateColumns="False" Width="100%" ShowHeader="true" EnableViewState="true" Height="89.5%"
                CellSpacing="0" GridLines="None" GroupingEnabled="false" AllowPaging="true" AllowSorting="true" EnableHeaderContextMenu="true" AllowCustomPaging="true"
                OnNeedDataSource="gvAllotment_NeedDataSource" OnItemDataBound="gvAllotment_ItemDataBound" OnItemCommand="gvAllotment_ItemCommand">
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataKeyNames="FLDALLOTMENTID" HeaderStyle-HorizontalAlign="Center">

                    <Columns>

                        <telerik:GridTemplateColumn UniqueName="Listcheckbox">
                            <HeaderStyle Width="30px" HorizontalAlign="Center" />
                            <ItemStyle Width="30px" HorizontalAlign="Center" />
                            <HeaderTemplate>
                                <telerik:RadCheckBox ID="chkAllAllotment" runat="server" AutoPostBack="true"
                                    OnPreRender="CheckAll" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadCheckBox ID="chkSelect" runat="server" EnableViewState="true" AutoPostBack="true" OnCheckedChanged="SaveCheckedValues" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAllotmentId" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDALLOTMENTID"] %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblEmployeeId" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDEMPLOYEEID"] %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDVESSELID"] %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblMonth" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDMONTH"] %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblYear" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDYEAR"] %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselName" runat="server" ToolTip='<%#((DataRowView)Container.DataItem)["FLDVESSELNAME"] %>' Text='<%#((DataRowView)Container.DataItem)["FLDVESSELNAME"] %>'></telerik:RadLabel>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="File No./ Name">
                            <HeaderStyle Width="13%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFileNo" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDFILENO"] %>'></telerik:RadLabel>
                                <br />
                                <telerik:RadLabel ID="lblName" runat="server" Text='<%# " / "+ DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME").ToString() %>'></telerik:RadLabel>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText="Rank">
                            <HeaderStyle Width="5.5%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRank" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDRANKNAME"] %>'></telerik:RadLabel>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Bank Account">
                            <HeaderStyle Width="235px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBankAccount" runat="server" ToolTip='<%#((DataRowView)Container.DataItem)["FLDBANK"] %>' Text='<%#((DataRowView)Container.DataItem)["FLDBANK"] %>'></telerik:RadLabel>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Currency">
                            <HeaderStyle Width="6%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDCURRENCYCODE"] %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCurrencyId" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDCURRENCYID"] %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount">
                            <HeaderStyle Width="6%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Month/Year">
                            <HeaderStyle Width="6.5%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMonth1" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDMONTH"] %>'></telerik:RadLabel>

                                <telerik:RadLabel ID="lblYear1" runat="server" Text='<%# " / "+ DataBinder.Eval(Container,"DataItem.FLDYEAR").ToString() %>'></telerik:RadLabel>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDREQUESTSTATUSNAME"] %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRequestStatus" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDREQUESTSTATUS"] %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="PMV No." HeaderStyle-Wrap="false">
                            <HeaderStyle Width="8%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPMVNo" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDPVVOUCHERNO"] %>'></telerik:RadLabel>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="8%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>


                                <asp:LinkButton runat="server" AlternateText="Approve" CommandName="APPROVE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdApprove" ToolTip="Approve">
                                    <span class="icon"><i class="fas fa-award"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="verification" CommandName="VERIFICATION" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdverification" ToolTip="verification">
                                    <span class="icon"><i class="fa-envelope-open-text"></i></span>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" Position="Bottom" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="3" EnableColumnClientFreeze="true" ScrollHeight="350px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" ResizeGridOnColumnResize="false" />
                </ClientSettings>

            </telerik:RadGrid>
            <asp:Button ID="ucConfirmDelete" runat="server" OnClick="ucConfirmDelete_Click" CssClass="hidden" />
            <asp:Button ID="ucConfirmDeleteRecord" runat="server" OnClick="ucConfirmDeleteRecord_Click" CssClass="hidden" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
