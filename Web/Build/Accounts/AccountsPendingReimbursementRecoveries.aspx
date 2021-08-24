<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsPendingReimbursementRecoveries.aspx.cs" Inherits="AccountsPendingReimbursementRecoveries" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Reimbursement/Recoveries</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <style type="text/css">
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
    <form id="frmAllotmentRemRec" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table>
                <tr>
                    <td><span id="Span1" class="icon" runat="server"><i class="fas fa-info-circle" style="align-content: center" />
                    </span></td>
                    <td>
                        <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="RadToolTip1" Width="300px" ShowEvent="onmouseover"
                            RelativeTo="Element" Animation="Fade" TargetControlID="Span1" IsClientID="true"
                            HideEvent="ManualClose" Position="TopCenter" EnableRoundedCorners="true" ContentScrolling="Auto">
                            <table cellpadding="1" cellspacing="1" width="100%" style="border-style: none; color: blue;"
                                runat="server" id="tblNote">
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblNote" runat="server" Text="Note"></telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblRecoveries" runat="server"
                                            Text="* Please enter the Payment Mode for All Pending Recoveries">
                                        </telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblPlea"
                                            runat="server" Text="* Please enter Remarks for Reason for Postponing Remittance of Reimbursement/ Recoveries">
                                        </telerik:RadLabel>
                                    </td>
                                </tr>
                            </table>
                        </telerik:RadToolTip>
                    </td>
                </tr>
            </table>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvRem" Height="95%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="false" CellSpacing="0" GridLines="None" EnableViewState="false"
                ShowFooter="false" ShowHeader="true" OnNeedDataSource="gvRem_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDDTKEY">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Reference No.">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENO") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Claim Date">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# string.Format("{0:dd/MMM/yyyy}", DataBinder.Eval(Container, "DataItem.FLDCLAIMDATE")) %>
                                <telerik:RadLabel ID="lblDTKey" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRemarksId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKSID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblAllotmentType" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALLOTMENTTYPE") %>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reimbursement/Recovery">
                            <HeaderStyle Width="12%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# GetName(DataBinder.Eval(Container,"DataItem.FLDEARNINGDEDUCTION").ToString())%>
                                <telerik:RadLabel ID="lblEarningDeduction" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEARNINGDEDUCTION") %>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblReimbursementId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREIMBURSEMENTID") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Purpose">
                            <HeaderStyle Width="12%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDPURPOSENAME") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Description">
                            <HeaderStyle Width="12%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Currency">
                            <HeaderStyle Width="8%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Unpaid Amount">
                            <HeaderStyle Width="8%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDUNPAIDAMOUNT") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Payment Mode">
                            <HeaderStyle Width="8%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTMODENAME") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Payment Status">
                            <HeaderStyle Width="8%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTSTATUSNAME") %>
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

