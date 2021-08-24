<%@ Page Language="C#" AutoEventWireup="True" CodeFile="AccountsCompanyFinancialYearSetup.aspx.cs"
    Inherits="AccountsCompanyFinancialYearSetup" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register Src="../UserControls/UserControlMoney.ascx" TagName="UserControlMoney"
    TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Company Financial Year Setup</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

</telerik:RadCodeBlock></head>
<body>
    <form id="frmAccountsCompanyFinancialYearSetup" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts ="false" >
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlCompanyFinancialYearSetup">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="Title1" Text="Company Voucher Number Setup" />
                    </div>
                </div>
                <div id="divFind" style="position: relative; z-index: 2">
                    <table id="tblExchangeRate">
                        <tr>
                            <td>
                                <asp:Literal ID="lblCompanyName" runat="server" Text="Company Name :"></asp:Literal>
                            </td>
                            <td width="480px">
                                <%=strCompanyName %>
                            </td>
                            <td>
                                <asp:Literal ID="lblFinYear" runat="server" Text="Fin.Year :"></asp:Literal>
                            </td>
                            <td>
                                <%=strFinancialYear%>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuFinancialYearSetup" runat="server" OnTabStripCommand="FinancialYearSetup_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuCompanyFinancialYearSetup" runat="server" OnTabStripCommand="CompanyFinancialYearSetup_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative;">
                    <asp:GridView ID="dgFinancialYearSetup" runat="server" AutoGenerateColumns="False"
                        CellPadding="3" Font-Size="11px" OnRowCancelingEdit="dgFinancialYearSetup_RowCancelingEdit"
                        OnRowCommand="dgFinancialYearSetup_RowCommand" OnRowDataBound="dgFinancialYearSetup_ItemDataBound"
                        OnRowDeleting="dgFinancialYearSetup_RowDeleting" OnRowEditing="dgFinancialYearSetup_RowEditing"
                        ShowFooter="False" Style="margin-bottom: 0px" Width="100%">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:ButtonField CommandName="Edit" Text="DoubleClick" Visible="false" />
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblVoucherTypeHeader" runat="server">Voucher Type                            
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <itemtemplate>
                                    <asp:Label  ID="lblVoucherType" runat="server"><%# DataBinder.Eval(Container, "DataItem.FLDVOUCHERTYPE")%></asp:Label>
                                </itemtemplate>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblSubVoucherTypeHeader" runat="server">Sub Voucher Type                            
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkSubVoucherType" runat="server" CommandArgument="<%# Container.DataItemIndex %>"
                                        CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBVOUCHERTYPE") %>'></asp:LinkButton>
                                    <asp:TextBox ID="hidtxtMapCode" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMAPCODE")%>'
                                        Visible="false"> </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblStartingNumberHeader" runat="server">Starting Number</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblStartingNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTARTINGNUMBER") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtStartingNumberEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTARTINGNUMBER") %>'
                                        CssClass="gridinput_mandatory txtNumber" MaxLength="20"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditStartingNumberEdit" runat="server"
                                        AutoComplete="false" InputDirection="RightToLeft" Mask="999999999" MaskType="Number"
                                        OnInvalidCssClass="MaskedEditError" TargetControlID="txtStartingNumberEdit" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblRunningNumberHeader" runat="server">Running Number</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblRunningNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRUNNINGNUMBER") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtRunningNumberEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRUNNINGNUMBER") %>'
                                        CssClass="gridinput_mandatory txtNumber" MaxLength="20"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditRunningNumberEdit" runat="server" AutoComplete="false"
                                        InputDirection="RightToLeft" Mask="999999999" MaskType="Number" OnInvalidCssClass="MaskedEditError"
                                        TargetControlID="txtRunningNumberEdit" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblResetYNHeader" runat="server"> Reset Number </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblResetYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDRESETNUMBER").ToString().Equals("1"))?"Yes":"No" %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkResetYNEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDRESETNUMBER").ToString().Equals("1"))?true:false %>' />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server"> Action </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="100px" Wrap="False" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="cmdEdit" runat="server" AlternateText="Edit" CommandArgument="<%# Container.DataItemIndex %>"
                                        CommandName="EDIT" ImageUrl="<%$ PhoenixTheme:images/te_edit.png%>" ToolTip="Edit" />
                                    <img runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton ID="cmdSave" runat="server" AlternateText="Save" CommandArgument="<%# Container.DataItemIndex %>"
                                        CommandName="Save" ImageUrl="<%$ PhoenixTheme:images/save.png%>" ToolTip="Save" />
                                    <img runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton ID="cmdCancel" runat="server" AlternateText="Cancel" CommandArgument="<%# Container.DataItemIndex %>"
                                        CommandName="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png%>" ToolTip="Cancel" />
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
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
