<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAllotmentRequestList.aspx.cs" Inherits="Accounts_AccountsAllotmentRequestList" %>

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
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonToolTip" Src="~/UserControls/UserControlCommonToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Month" Src="~/UserControls/UserControlMonth.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Year" Src="~/UserControls/UserControlYear.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Allotment List</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function DeleteRecord(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmDelete.UniqueID %>", "");
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
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <%--  <eluc:TabStrip ID="MenuAllotmentRequestList" runat="server" TabStrip="true" OnTabStripCommand="MenuAllotmentRequestList_TabStripCommand"></eluc:TabStrip>--%>
            <br />
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselName" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ddlVessel" runat="server" AppendDataBoundItems="true" VesselsOnly="true" CssClass="input_mandatory"
                            Width="180px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMonth" runat="server" Text="Month"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Month ID="ddlMonth" runat="server" Width="120px" CssClass="input_mandatory" AppendDataBoundItems="true"></eluc:Month>

                    </td>
                    <td>
                        <telerik:RadLabel ID="lblYear" runat="server" Text="Year"></telerik:RadLabel>
                    </td>

                    <td>
                        <eluc:Year ID="ddlyear" runat="server" Width="120px" OrderByAsc="false" CssClass="input_mandatory" AppendDataBoundItems="true"></eluc:Year>
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblBOWCalculationDate" Text="BOW Calculation Date" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDate" runat="server" CssClass="input" />
                        <asp:ImageButton runat="server" AlternateText="Calculate" ImageUrl="<%$ PhoenixTheme:images/Cal.png %>"
                            CommandName="SAVE" ID="cmdCalculate" ToolTip="Calculate BOW" Style="cursor: pointer; vertical-align: top"
                            OnClick="cmdCalculate_Click"></asp:ImageButton>
                    </td>
                </tr>

            </table>
            <br />
            <eluc:TabStrip ID="MenuAllotmentList" runat="server" OnTabStripCommand="MenuAllotmentList_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvAllotmentList" runat="server" AutoGenerateColumns="False" Width="100%" ShowHeader="true" EnableViewState="True" Height="84%"
                CellSpacing="0" GridLines="None" GroupingEnabled="false" AllowPaging="true" AllowSorting="true" EnableHeaderContextMenu="true" AllowCustomPaging="true"
                OnNeedDataSource="gvAllotmentList_NeedDataSource" OnItemDataBound="gvAllotmentList_ItemDataBound" OnItemCommand="gvAllotmentList_ItemCommand">
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataKeyNames="FLDALLOTMENTREQUESTID" HeaderStyle-HorizontalAlign="Center">

                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Rank">
                            <HeaderStyle Width="6%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAllotmentRequestId" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDALLOTMENTREQUESTID"] %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSignOnOffId" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDSIGNONOFFID"] %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblEmployeeId" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDEMPLOYEEID"] %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDVESSELID"] %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblLockedYN" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDLOCKEDYN"] %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRank" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDRANKCODE"] %>'></telerik:RadLabel>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name">
                            <HeaderStyle Width="14%" />
                            <ItemStyle Wrap="False" HorizontalAlign="center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblName" runat="server" ToolTip='<%#((DataRowView)Container.DataItem)["FLDEMPLOYEENAME"] %>' Text='<%#((DataRowView)Container.DataItem)["FLDEMPLOYEENAME"] %>'></telerik:RadLabel>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Bank Account">
                            <HeaderStyle Width="80px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBankAccount" runat="server" ToolTip='<%#((DataRowView)Container.DataItem)["FLDBANKACCOUNTNUMBER"] %>' Text='<%#((DataRowView)Container.DataItem)["FLDBANKACCOUNTNUMBER"] %>'></telerik:RadLabel>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Currency">
                            <HeaderStyle Width="9%" />
                            <ItemStyle Wrap="False" HorizontalAlign="center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDCURRENCYCODE"] %>'></telerik:RadLabel>

                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:UserControlCurrency ID="ddlCurrencyEdit" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>' Width="100%" CssClass="dropdown_mandatory"
                                    runat="server" AppendDataBoundItems="true" SelectedCurrency='<%# DataBinder.Eval(Container,"DataItem.FLDALLOTMENTCURRENCY") %>' />
                                <telerik:RadLabel ID="lblCurrencyEdit" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDALLOTMENTCURRENCY"]%>'></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount">
                            <HeaderStyle Width="7%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAmount" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDAMOUNT"] %>'></telerik:RadLabel>

                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtAmountEdit" runat="server" CssClass="input_mandatory" DecimalPlace="17" MaxLength="25"
                                    Width="100%" Text='<%#((DataRowView)Container.DataItem)["FLDAMOUNT"]%>' />
                                <%-- <telerik:RadLabel ID="lblAmount" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDAMOUNT"]%>'></telerik:RadLabel>--%>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblType" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDALLOTMENTTYPENAME"] %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblTypeValue" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDALLOTMENTTYPE"] %>'></telerik:RadLabel>

                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadComboBox RenderMode="Lightweight" ID="ddlType" runat="server" Width="100%" Filter="Contains" MarkFirstMatch="true" AppendDataBoundItems="true">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="" Text="--Select--" />
                                        <telerik:RadComboBoxItem Value="4" Text="Allotment" />
                                        <telerik:RadComboBoxItem Value="7" Text="Special Allotment" />
                                    </Items>
                                </telerik:RadComboBox>
                                <telerik:RadLabel ID="lblAllotmentType" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDALLOTMENTTYPE"]%>'></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Month/Year">
                            <HeaderStyle Width="7%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMonth1" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDMONTH"] %>'></telerik:RadLabel>

                                <telerik:RadLabel ID="lblYear1" runat="server" Text='<%# " / "+ DataBinder.Eval(Container,"DataItem.FLDYEAR").ToString() %>'></telerik:RadLabel>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Balance(USD)">
                            <HeaderStyle Width="8%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBalance" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDFINALBALANCE"] %>'></telerik:RadLabel>

                            </ItemTemplate>

                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="8%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" ID="cmdEdit" ToolTip="Edit" CommandName="Edit" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Generate Allotment" CommandName="GENERATEALLOTMENT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdGenerateAllotment" ToolTip="Generate Allotment">
                                    <span class="icon"><i class="fas fa-award"></i></span>
                                </asp:LinkButton>

                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ID="cmdSave" CommandName="Update" ToolTip="Save" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" ID="cmdCancel" CommandName="Cancel" ToolTip="Cancel" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
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
            <%--     <asp:Button ID="ucConfirmDeleteRecord" runat="server" OnClick="ucConfirmDeleteRecord_Click" CssClass="hidden" />--%>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
