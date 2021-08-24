<%@ Page Language="C#" AutoEventWireup="True" CodeFile="AccountsProjectBillingChargingPostVoucher.aspx.cs"
    Inherits="AccountsProjectBillingChargingPostVoucher" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Country</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmRegistersCountry" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlCountryEntry">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader">
                    <div class="divFloatLeft">
                        <eluc:Title runat="server" ID="Title1" Text="General" />
                        <asp:Button ID="cmdHiddenPick" runat="server" OnClick="cmdHiddenSubmit_Click" />
                    </div>
                    <div style="position: absolute; right: 0px">
                        <eluc:TabStrip ID="MenuProjectBillingMain" runat="server" OnTabStripCommand="MenuProjectBillingMain_TabStripCommand"
                            TabStrip="true"></eluc:TabStrip>
                    </div>
                </div>
                <div class="subHeader">
                    <div class="divFloatLeft">
                        <eluc:Title runat="server" ID="Title2" Text="" ShowMenu="false" />
                    </div>
                    <div class="navSelect" style="position: absolute; right: 0px">
                        <eluc:TabStrip ID="MenuProjectBilling" runat="server" OnTabStripCommand="MenuProjectBilling_TabStripCommand"
                            TabStrip="false"></eluc:TabStrip>
                    </div>
            </div>
            <div style="position: relative;">
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblBillingCompany" runat="server" Text="Billing Company"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBillingCompany" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblPostDate" runat="server" Text="Post Date"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="txtPostDate" runat="server" CssClass="input" DatePicker="true" AutoPostBack="true"
                                OnTextChangedEvent="txtPostDate_TextChangedEvent" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="navSelect" style="position: relative; width: 15px">
                <eluc:TabStrip ID="MenuRegistersCountry" runat="server" OnTabStripCommand="RegistersCountry_TabStripCommand">
                </eluc:TabStrip>
            </div>
            <div id="divGrid" style="width: 100%;overflow-x:scroll;">
                <asp:GridView ID="gvCountry" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    OnRowCreated="gvCountry_RowCreated" Width="100%" CellPadding="3" OnRowCommand="gvCountry_RowCommand"
                    OnRowDataBound="gvCountry_ItemDataBound" OnRowCancelingEdit="gvCountry_RowCancelingEdit"
                    OnRowDeleting="gvCountry_RowDeleting" OnRowUpdating="gvCountry_RowUpdating" OnRowEditing="gvCountry_RowEditing"
                    ShowFooter="true" ShowHeader="true" EnableViewState="false" AllowSorting="true"
                    OnSorting="gvCountry_Sorting">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <Columns>
                        <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                        <asp:TemplateField HeaderText="Country Name">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblProjectBillingName" runat="server" Text="Project Billing Name"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblProjectBillingName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROJECTBILLINGNAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nationality">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblProjectBillingGroup" runat="server" Text="Project Billing Group"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblProjectBillingGroup" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROJECTBILLINGGROUP") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblVesselIssued" runat="server" Text="Vessel Issued"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblVesselIssued" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="80px"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblDateIssued" runat="server" Text="Date Issued"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblDateIssued" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUEDATE") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblIssuedQty" runat="server" Text="Issued Qty"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblIssuedQty" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUEDQTY") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblBillingQty" runat="server" Text="Billing Qty"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblBilledQty" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBILLEDQTY") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtBilledQty" runat="server" CssClass="input txtNumber" DecimalPlace="4"
                                    IsPositive="true" MaxLength="9" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBILLEDQTY") %>' />
                                <asp:Label ID="lblProjectBillingIssueIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROJECTBILLINGISSUEID") %>'></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblBudgetCode" runat="server" Text="Budget Code"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETCODE") %>'></asp:Label>
                                <asp:Label ID="lblBudgetCodeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETID") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListTaxBudgetEdit">
                                    <asp:TextBox ID="txtBudgetCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETCODE") %>'
                                        MaxLength="20" CssClass="input" Width="60px"></asp:TextBox>
                                    <asp:TextBox ID="txtBudgetNameEdit" runat="server" Width="0px" CssClass="input" Enabled="False"></asp:TextBox>
                                    <asp:ImageButton ID="btnShowBudgetEdit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataItemIndex %>" />
                                    <asp:TextBox ID="txtBudgetIdEdit" runat="server" Width="0px" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETID") %>'></asp:TextBox>
                                    <asp:TextBox ID="txtBudgetgroupIdEdit" runat="server" Width="0px" CssClass="input"></asp:TextBox>
                                </span>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblOwnerBudgetCode" runat="server" Text="Owner Budget Code"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblOwnerBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETGROUP") %>'></asp:Label>
                                <asp:Label ID="lblOwnerBudgetCodeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETGROUPID") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListOwnerBudgetEdit">
                                    <asp:TextBox ID="txtOwnerBudgetCodeEdit" runat="server" MaxLength="20" CssClass="input"
                                        Width="60px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETGROUP") %>'></asp:TextBox>
                                    <asp:TextBox ID="txtOwnerBudgetNameEdit" runat="server" Width="0px" CssClass="input"
                                        Enabled="False"></asp:TextBox>
                                    <asp:ImageButton ID="btnShowOwnerBudgetEdit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataItemIndex %>" />
                                    <asp:TextBox ID="txtOwnerBudgetIdEdit" runat="server" Width="0px" CssClass="input"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETGROUPID") %>'></asp:TextBox>
                                    <asp:TextBox ID="txtOwnerBudgetgroupIdEdit" runat="server" Width="0px" CssClass="input"></asp:TextBox>
                                </span>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblRate" runat="server" Text="Rate"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblRate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSELLINGAMOUNT") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right" />
                            <HeaderTemplate>
                                <asp:Literal ID="lblCurrency" runat="server" Text="Currency"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblProjectBillingIssueId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROJECTBILLINGISSUEID") %>'></asp:Label>
                                <asp:Label ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></asp:Label>
                                <asp:Label ID="lblBillingCompanyId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYID") %>'></asp:Label>
                                <asp:Label ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right" />
                            <HeaderTemplate>
                                <asp:Literal ID="lblAmount" runat="server" Text="Amount"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblAmount" runat="server" CssClass="txtNumber" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <b><asp:Literal ID="lblTotalAmountUSD" runat="server" Text="Total Amount(USD)"></asp:Literal>&nbsp;:</b>
                                <asp:Label ID="lblTotalAmount" runat="server"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right" />
                            <HeaderTemplate>
                                <asp:Literal ID="lblAmountUSD" runat="server" Text="Amount(USD)"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblAmountUSD" runat="server" CssClass="txtNumber" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNTUSD") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblTotalAmountUSD" runat="server"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument="<%# (Container.DataItemIndex)%>" ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                <%--<asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument='<%# (Container.DataItemIndex)%>' ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>--%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="Save" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div id="divPage" style="position: relative;">
                <table width="100%" border="0" class="datagrid_pagestyle">
                    <tr>
                        <td nowrap="nowrap" align="center">
                            <asp:Label ID="lblPagenumber" runat="server">
                            </asp:Label>
                            <asp:Label ID="lblPages" runat="server">
                            </asp:Label>
                            <asp:Label ID="lblRecords" runat="server">
                            </asp:Label>&nbsp;&nbsp;
                        </td>
                        <td nowrap="nowrap" align="left" width="50px">
                            <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                        </td>
                        <td width="20px">
                            &nbsp;
                        </td>
                        <td nowrap="nowrap" align="right" width="50px">
                            <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                        </td>
                        <td nowrap="nowrap" align="center">
                            <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                            </asp:TextBox>
                            <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                Width="40px"></asp:Button>
                        </td>
                    </tr>
                    <eluc:Status runat="server" ID="ucStatus" />
                </table>
            </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
