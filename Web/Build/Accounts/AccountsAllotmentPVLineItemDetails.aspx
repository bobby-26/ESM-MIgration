<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAllotmentPVLineItemDetails.aspx.cs" Inherits="Accounts_AccountsAllotmentPVLineItemDetails" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonToolTip" Src="~/UserControls/UserControlCommonToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Allotment PV LineItem Details</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style type="text/css">
            .scrolpan {
                overflow-y: auto;
                height: 80%;
            }

            .checkRtl {
                direction: rtl;
            }

            .fon {
                font-size: small !important;
            }
        </style>

    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />
    <form id="frmLineItemDetails" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server"></telerik:RadSkinManager>
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="85%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <%-- <asp:Button ID="ucConfirmMessage" runat="server" OnConfirmMesage="OnAction_Click" CssClass="hidden" />--%>

            <eluc:Title runat="server" ID="Title1" Text="Allotment PV LineItem Details" Visible="false"></eluc:Title>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" Visible="false" />
            <eluc:TabStrip ID="MenuVoucher" runat="server" OnTabStripCommand="Voucher_TabStripCommand"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuItemDetail" runat="server" OnTabStripCommand="MenuItemDetail_TabStripCommand"></eluc:TabStrip>
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>

                    <td style="width: 50%; vertical-align: top">

                        <telerik:RadGrid ID="gvAllotmentVesselList" runat="server" AutoGenerateColumns="False" Width="100%" ShowHeader="true" EnableViewState="true"
                            CellSpacing="0" GridLines="None" GroupingEnabled="false" AllowPaging="true" AllowSorting="true" EnableHeaderContextMenu="true" AllowCustomPaging="true"
                            OnNeedDataSource="gvAllotmentVesselList_NeedDataSource" OnItemDataBound="gvAllotmentVesselList_ItemDataBound" OnItemCommand="gvAllotmentVesselList_ItemCommand">
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false">
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Vessel Code">
                                        <HeaderStyle Width="6%" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDVESSELID")%>'></telerik:RadLabel>

                                            <telerik:RadLabel ID="lblVesselCode" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDVESSELCODE")%>'></telerik:RadLabel>

                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Vessel Name">
                                        <HeaderStyle Width="10%" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblVesselName" runat="server" ToolTip='<%# DataBinder.Eval(Container, "DataItem.FLDVESSELNAME")%>' Text='<%# DataBinder.Eval(Container, "DataItem.FLDVESSELNAME")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="No.of Transaction">
                                        <HeaderStyle Width="8%" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblTransaction" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOUNT")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Amount">
                                        <HeaderStyle Width="10%" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lblAmount" runat="server" CommandName="AMOUNT" Text='<%# DataBinder.Eval(Container, "DataItem.FLDALLTOMENTTOTAL")%>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                </Columns>
                                <NoRecordsTemplate>
                                    <table runat="server" width="100%" border="0">
                                        <tr>
                                            <td align="center">
                                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </NoRecordsTemplate>
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                <%--<Scrolling AllowScroll="false" UseStaticHeaders="true" SaveScrollPosition="false" />--%>
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>

                    </td>
                    <td style="width: 50%">
                        <table width="100%">
                            <tr>
                                <%--<td></td>--%>
                                <td colspan="2">
                                    <telerik:RadTextBox ID="txtName" runat="server" CssClass="readonlytextbox" ReadOnly="true" Enabled="false" Width="250px">
                                    </telerik:RadTextBox>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblPreparedBy" runat="server" Text="Prepared By:"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtPreparedBy" runat="server" CssClass="readonlytextbox" ReadOnly="true" Enabled="false" Width="150px">
                                    </telerik:RadTextBox>
                                </td>


                            </tr>
                            <tr>
                                <%--  <td></td>--%>
                                <td colspan="2">
                                    <telerik:RadTextBox ID="txtDetail" runat="server" CssClass="readonlytextbox" ReadOnly="true" Enabled="false" Width="250px">
                                    </telerik:RadTextBox>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblDate" runat="server" Text="Date:"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Date ID="txtDate" runat="server" CssClass="readonlytextbox" ReadOnly="true" Enabled="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblCurrency" runat="server" Text="Currency:"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtCurrency" runat="server" CssClass="readonlytextbox" ReadOnly="true" Enabled="false" Width="150px">
                                    </telerik:RadTextBox>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblApprovedBy" runat="server" Text="Approved By:"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtApprovedBy" runat="server" CssClass="readonlytextbox" ReadOnly="true" Enabled="false" Width="150px">
                                    </telerik:RadTextBox>
                                </td>

                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblRemittingAgent" runat="server" Text="Remitting Agent:"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtRemittingAgent" runat="server" CssClass="readonlytextbox" ReadOnly="true" Enabled="false" Width="150px">
                                    </telerik:RadTextBox>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblBank" runat="server" Text="Bank:"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtBank" runat="server" CssClass="readonlytextbox" ReadOnly="true" Enabled="false" Width="150px">
                                    </telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblBeneficiary" runat="server" Text="Beneficiary:"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtBeneficiary" runat="server" CssClass="readonlytextbox" ReadOnly="true" Enabled="false" Width="150px">
                                    </telerik:RadTextBox>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblSwift" runat="server" Text="Swift Code:"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtSwiftCode" runat="server" CssClass="readonlytextbox" ReadOnly="true" Enabled="false" Width="150px">
                                    </telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblAccount" runat="server" Text="Account No.:"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtAccountNo" runat="server" CssClass="readonlytextbox" ReadOnly="true" Enabled="false" Width="150px">
                                    </telerik:RadTextBox>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblIntermediarySwiftCode" runat="server" Text="Intermediary Swift Code:"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtIntermediarySwiftCode" runat="server" CssClass="readonlytextbox" ReadOnly="true" Enabled="false" Width="150px">
                                    </telerik:RadTextBox>
                                </td>
                            </tr>

                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">


                        <eluc:TabStrip ID="MenuEmployeeList" runat="server" OnTabStripCommand="MenuEmployeeList_TabStripCommand"></eluc:TabStrip>
                        <telerik:RadGrid ID="gvAllotmentEmployeeList" runat="server" AutoGenerateColumns="False" Width="100%" ShowHeader="true" EnableViewState="true" Height="100%"
                            CellSpacing="0" GridLines="None" GroupingEnabled="false" AllowPaging="true" AllowSorting="true" EnableHeaderContextMenu="true" AllowCustomPaging="true"
                            OnNeedDataSource="gvAllotmentEmployeeList_NeedDataSource" OnItemDataBound="gvAllotmentEmployeeList_ItemDataBound" OnItemCommand="gvAllotmentEmployeeList_ItemCommand">
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false">
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="">
                                        <HeaderStyle Width="5%" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <eluc:CommonToolTip ID="ucCommonToolTip" runat="server" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Vessel Code">
                                        <HeaderStyle Width="6.3%" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDVESSELID")%>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblEmployeeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDEMPLOYEEID")%>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblBankAccountId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDBANKACCOUNTID")%>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblVesselCode" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDVESSELCODE")%>'></telerik:RadLabel>

                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Vessel Name">
                                        <HeaderStyle Width="11.2%" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblVesselName" runat="server" ToolTip='<%# DataBinder.Eval(Container, "DataItem.FLDVESSELNAME")%>' Text='<%# DataBinder.Eval(Container, "DataItem.FLDVESSELNAME")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="File No./ Name">
                                        <HeaderStyle Width="21%" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblFileNo" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFILENO")%>'></telerik:RadLabel>
                                            <br />
                                            <telerik:RadLabel ID="lblName" runat="server" Text='<%# " / "+ DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME").ToString() %>'></telerik:RadLabel>

                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Rank">
                                        <HeaderStyle Width="6%" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDRANKCODE")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Amount">
                                        <HeaderStyle Width="8.5%" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAllotmentAmount" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDALLTOMENTTOTAL")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Beneficiary Name">
                                        <HeaderStyle Width="16%" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAccountName" runat="server" ToolTip='<%# DataBinder.Eval(Container, "DataItem.FLDACCOUNTNAME")%>' Text='<%# DataBinder.Eval(Container, "DataItem.FLDACCOUNTNAME")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Bank Name">
                                        <HeaderStyle Width="17%" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblBankName" runat="server" ToolTip='<%# DataBinder.Eval(Container, "DataItem.FLDBANKNAME")%>' Text='<%# DataBinder.Eval(Container, "DataItem.FLDBANKNAME")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="SWIFT Code">
                                        <HeaderStyle Width="10%" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblSwiftCode" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDBANKSWIFTCODE")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Account No.">
                                        <HeaderStyle Width="9%" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAccountNo" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDACCOUNTNUMBER")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Intermediary Swift">
                                        <HeaderStyle Width="10%" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblIntermediarySwift" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDINTERMEDIATEBANKSWIFTCODE")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                                <NoRecordsTemplate>
                                    <table runat="server" width="100%" border="0">
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
                                <%--<Scrolling AllowScroll="false" UseStaticHeaders="true" SaveScrollPosition="false" />--%>
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>

                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
