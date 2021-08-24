<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsVesselVisitTravelAdvanceRequest.aspx.cs"
    Inherits="AccountsVesselVisitTravelAdvanceRequest" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Office/Travel advance </title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmVisitRegister" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <eluc:Title runat="server" ID="Title1" Text="Travel/office advance request/return form" Visible="false"></eluc:Title>
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click"
            CssClass="hidden" />
        <eluc:TabStrip ID="MenuTravelAdvanceMain" runat="server" OnTabStripCommand="MenuTravelAdvanceMain_TabStripCommand"
            TabStrip="true"></eluc:TabStrip>
                    <eluc:TabStrip ID="MenuTravelAdvanceSub" runat="server" OnTabStripCommand="MenuTravelAdvanceSub_TabStripCommand"></eluc:TabStrip>
        <table id="Table2" width="100%" style="color: Blue">
            <tr>
                <td>&nbsp;
                  <telerik:RadLabel ID="lbldee" runat="server" Font-Bold="true" Text="* Advance taken but not submitted the claim or not returned the balance advance
                            and the advance amount will automatically get adjusted against salary after 3 months.">
                  </telerik:RadLabel>
                </td>
            </tr>
        </table>
        <table cellpadding="2" cellspacing="1" style="width: 100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblemployee" runat="server" Text="Employee Id/Name"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtEmployee" Width="75%" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblvessel" runat="server" Text="Vessel"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtVessel" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblPurpose" runat="server" Text="Purpose"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtPurpose" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblFleet" runat="server" Text="Fleet"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtFleet" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                </td>
            </tr>
        </table>
        <eluc:TabStrip ID="MenuTravelAdvance" runat="server" OnTabStripCommand="MenuTravelAdvance_TabStripCommand"></eluc:TabStrip>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvTravelAdvance" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowPaging="true" AllowCustomPaging="true"
           Width="100%" CellPadding="3" OnItemCommand="gvTravelAdvance_ItemCommand" EnableHeaderContextMenu="true" GroupingEnabled="false"
           OnItemDataBound="gvTravelAdvance_ItemDataBound" OnNeedDataSource="gvTravelAdvance_NeedDataSource"
            ShowFooter="true" ShowHeader="true" EnableViewState="true" AllowSorting="true">
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
                    <telerik:GridTemplateColumn HeaderText="Travel Advance Id" HeaderStyle-Width="22%">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblTravelAdvanceId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELADVANCEID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblTravelAdvanceNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELADVANCENUMBER") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Requested</br> Date" HeaderStyle-Width="10%">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRequestedDate" runat="server" Text='<%# General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDREQUESTEDDATE") )%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Currency" HeaderStyle-Width="10%">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Currency runat="server" ID="ucCurrencyEdit" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>' Width="99%"
                                AppendDataBoundItems="true" AutoPostBack="false" CssClass="dropdown_mandatory" />
                            <telerik:RadLabel ID="lblCurrencyId" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYID") %>'
                                runat="server">
                            </telerik:RadLabel>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <eluc:Currency runat="server" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>' Width="99%"
                                ID="ucCurrencyAdd" AppendDataBoundItems="true" AutoPostBack="false" CssClass="dropdown_mandatory" ActiveCurrency="false" />
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Requested</br> Amount" HeaderStyle-Width="10%">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRequestedAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTAMOUNT","{0:###,###,###,##0.00}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Number ID="txtRequestedAmountEdit" runat="server" CssClass="input_mandatory txtNumber" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTAMOUNT") %>'
                                Width="99%"></eluc:Number>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <eluc:Number ID="txtRequestedAmountAdd" runat="server" CssClass="input_mandatory txtNumber"
                                Width="99%"></eluc:Number>
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Taken</br> Amount" HeaderStyle-Width="10%">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblTakenAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTAKENAMOUNT","{0:###,###,###,##0.00}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Return </br> Amount" HeaderStyle-Width="10%">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblReturnAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRETURNAMOUNT") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Balance" HeaderStyle-Width="8%">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblBalance" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBALANCE","{0:###,###,###,##0.00}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Advance Status" HeaderStyle-Width="10%">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAdvanceStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUICKNAME") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblAdvanceStatusCode" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADVANCESTATUS") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Claim </br>Submitted" HeaderStyle-Width="10%">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblclaim" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLAIMSUBMITTED") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Remarks" HeaderStyle-Width="15%">
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadTextBox ID="txtRemarksEdit" runat="server" CssClass="input" Width="99%" TextMode="MultiLine" Resize="Both" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadTextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <telerik:RadTextBox ID="txtRemarksAdd" runat="server" CssClass="input" Width="99%" TextMode="MultiLine" Resize="Both"></telerik:RadTextBox>
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Advance From</br>(Office/Vessel)" HeaderStyle-Width="11%">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAdvancefrom" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADVANCETYPE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <FooterTemplate>
                            <telerik:RadComboBox ID="ddlAdvance" runat="server" CssClass="input_mandatory" Width="99%">
                                <Items>
                                    <telerik:RadComboBoxItem Text="Select" Value="dummy"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Text="Office" Value="0"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Text="Vessel" Value="1"></telerik:RadComboBoxItem>
                                </Items>
                            </telerik:RadComboBox>
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="7%">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                CommandName="Edit" CommandArgument='<%# Container.DataItem %>' ID="cmdEdit"
                                ToolTip="Edit"></asp:ImageButton>
                            <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                width="3" />
                            <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                CommandName="Reject" CommandArgument='<%# Container.DataItem %>' ID="cmdCancel"
                                ToolTip="Cancel"></asp:ImageButton>
                            <img id="Img3" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                width="3" />
                            <%--<asp:ImageButton runat="server" AlternateText="TakeVesselCash" ImageUrl="<%$ PhoenixTheme:images/priority_invoice.png %>"
                                            CommandName="TakeVesselCash" CommandArgument='<%# Container.DataItem %>' ID="cmdTakeVesselCash" 
                                            ToolTip="Take Vessel Cash"></asp:ImageButton>--%>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                CommandName="Update" CommandArgument='<%# Container.DataItem %>' ID="cmdSave"
                                ToolTip="Save"></asp:ImageButton>
                            <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                width="3" />
                            <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                CommandName="Cancel" CommandArgument='<%# Container.DataItem %>' ID="cmdEditCancel"
                                ToolTip="Cancel"></asp:ImageButton>
                        </EditItemTemplate>
                        <FooterStyle HorizontalAlign="Center" />
                        <FooterTemplate>
                            <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                CommandName="Add" CommandArgument='<%# Container.DataItem %>' ID="cmdAdd"
                                ToolTip="Add New"></asp:ImageButton>
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
    </form>
</body>
</html>
