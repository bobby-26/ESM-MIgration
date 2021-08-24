<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsInvoiceAnalysis.aspx.cs"
    Inherits="Accounts_AccountsInvoiceAnalysis" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="../UserControls/UserControlHard.ascx" %>

<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Invoice Analysis</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInvoiceAnalysis" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table style="width: 55%">
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" ID="lblPurchaseSupdt" Width="100px" Text="Purchase Supdt"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDropDownList ID="ddlSuptList" runat="server" CssClass="dropdown_mandatory" Width="134px"
                            DataTextField="FLDUSERNAME" DataValueField="FLDUSERCODE">
                        </telerik:RadDropDownList>
                    </td>
                    <td style="width: 70px"></td>

                    <td>
                        <telerik:RadLabel runat="server" ID="lblInvoiceType" Text="Invoice Type" Width="80px"></telerik:RadLabel>
                    </td>
                    <td>
                        <div id="dvClass" runat="server" class="input" style="overflow: auto; height: 95px; width: 150px">
                            <telerik:RadCheckBoxList ID="chkInvoiceTypeList" runat="server" Direction="Vertical"
                                DataBindings-DataTextField="FLDHARDNAME" DataBindings-DataValueField="FLDHARDCODE">
                            </telerik:RadCheckBoxList>
                        </div>
                    </td>
                </tr>
                <br />
                <br />
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" Width="112px" ID="lblReceivedFromDate" Text="Received From Date"></telerik:RadLabel>
                    </td>

                    <td>
                        <eluc:Date ID="ucFromDate" Width="134px" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td style="width: 70px"></td>
                    <td>
                        <telerik:RadLabel runat="server" ID="lblReceivedToDate" Text="Received To Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucToDate" Width="134px" runat="server" CssClass="input_mandatory" />
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenuOrderForm" runat="server" OnTabStripCommand="MenuOrderForm_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvFormDetails" Height="60%" runat="server" AllowSorting="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvFormDetails_ItemCommand" OnNeedDataSource="gvFormDetails_NeedDataSource"
                ShowFooter="false" ShowHeader="true" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
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
                        <telerik:GridTemplateColumn HeaderText="User Name">
                            <ItemTemplate>
                                <asp:LinkButton ID="lblUserName" runat="server" CommandName="SELECT"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERNAME") %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblPurchaserId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURCHASERID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <b>
                                    <telerik:RadLabel ID="lblltTotal" runat="server" Text="Total"></telerik:RadLabel>
                                </b>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Initial Pending">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPendingCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPENDINGCOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <b>
                                    <%=iPendingcount%>
                                </b>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Received">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReceived" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIVED") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <b>
                                    <%=iReceivedcount%>
                                </b>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Cleared">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCleared" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLEARED") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <b>
                                    <%=iClearedcount%>
                                </b>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Re Opened">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReOpened" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREOPENED") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <b>
                                    <%=iReOpenedcount%>
                                </b>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Re Cleared">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReCleared" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECLEARED") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <b>
                                    <%=iReClearedcount%>
                                </b>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Cancelled">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCancelled" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCANCELLED") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <b>
                                    <%=iTotalCancelled%>
                                </b>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Total Pending">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTotal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFINALTOTAL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <b>
                                    <%=iTotalcount%>
                                </b>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Actual  Cleared">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActualCleared" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTUALCLEARED") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <b>
                                    <%=iActualClearedcount%>
                                </b>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
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
