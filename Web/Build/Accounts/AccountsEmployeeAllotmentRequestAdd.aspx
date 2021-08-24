<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsEmployeeAllotmentRequestAdd.aspx.cs" Inherits="AccountsEmployeeAllotmentRequestAdd" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.VesselAccounts" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BankAccount" Src="~/UserControls/UserControlEmployeeBankAccount.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmMessage" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
   
</telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <div class="subHeader" style="position: relative">
            <div id="divHeading" style="vertical-align: top">
                <eluc:Title runat="server" ID="ucTitle" Text="Vessel Supplier" ShowMenu="false" />
            </div>
        </div>
        <%--        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuStockItem" runat="server" OnTabStripCommand="MenuStockItem_TabStripCommand"></eluc:TabStrip>
        </div>--%>
        <br clear="all" />
        <asp:UpdatePanel runat="server" ID="pnlAddressEntry">
            <ContentTemplate>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <table width="100%" cellpadding="1" cellspacing="1">
                    <tr>
                        <td>
                            <asp:Literal ID="lblAllotmentType" runat="server" Text="Allotment Type"></asp:Literal>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlAllotmentType" runat="server" CssClass="input" DataTextField="FLDNAME"
                                DataValueField="FLDEARDEDTYPE" Width="300px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Literal ID="lblCashOnboard" runat="server" Text="Cash Onboard"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCashOnboard" runat="server" CssClass="readonlytextbox txtNumber"
                                ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblBOWCalculationDate" runat="server" Text="BOW Calculation Date"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="txtDate" runat="server" CssClass="input" />
                        </td>
                    </tr>
                </table>
                <br />
                <div id="divGrid" style="position: relative;">
                    <asp:GridView ID="gvCrewSearch" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowDataBound="gvCrewSearch_RowDataBound" ShowHeader="true"
                        EnableViewState="false" DataKeyNames="FLDEMPLOYEEID" OnRowCancelingEdit="gvCrewSearch_RowCancelingEdit"
                        OnRowEditing="gvCrewSearch_RowEditing" OnRowUpdating="gvCrewSearch_RowUpdating">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:TemplateField HeaderText="Employee Code">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblEmployeeId" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDEMPLOYEEID"] %>'></asp:Label>
                                    <%#((DataRowView)Container.DataItem)["FLDFILENO"] %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Employee Name">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%#((DataRowView)Container.DataItem)["FLDNAME"] %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rank">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%#((DataRowView)Container.DataItem)["FLDRANKCODE"] %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Currency">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Literal ID="lblUSD" runat="server" Text="USD"></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Amount">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <%#((DataRowView)Container.DataItem)["FLDAMOUNT"] %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblBalance" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDBALACEWAGE"] %>'></asp:Label>
                                    <eluc:Number ID="txtAmount" runat="server" CssClass="input" MaxLength="8" Width="90px"
                                        IsPositive="true" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date" Visible="false">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%#string.Format("{0:dd/MMM/yyyy}", ((DataRowView)Container.DataItem)["FLDDATE"]) %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblsignonoffid" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDSIGNONOFFID"] %>'></asp:Label>
                                    <asp:Label ID="lblsignoffDate" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDSIGNOFFDATE"] %>'></asp:Label>
                                    <eluc:Date ID="txtDate" runat="server" CssClass="input" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Bank Account">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%#string.Format("{0:dd/MMM/yyyy}", ((DataRowView)Container.DataItem)["FLDACCOUNTNUMBER"]) %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:BankAccount ID="ddlBankAccount" runat="server" CssClass="input" AppendDataBoundItems="true"
                                        EmployeeId='<%#((DataRowView)Container.DataItem)["FLDEMPLOYEEID"] %>' BankAccountList='<%#PhoenixVesselAccountsEmployeeBankAccount.ListEmployeeBankAccount(PhoenixSecurityContext.CurrentSecurityContext.VesselID, General.GetNullableInteger(((DataRowView)Container.DataItem)["FLDEMPLOYEEID"].ToString()))%>' />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Balance">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblBalanceWage" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDBALACEWAGE"] %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblAction" runat="server" Text="Action"></asp:Literal>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Update" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdUpdate"
                                        ToolTip="Update"></asp:ImageButton>
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <table width="100%" border="0" cellpadding="1" cellspacing="1" class="datagrid_pagestyle">
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
                                <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev"><< Prev </asp:LinkButton>
                            </td>
                            <td width="20px">&nbsp;
                            </td>
                            <td nowrap="nowrap" align="right" width="50px">
                                <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                            </td>
                            <td nowrap="nowrap" align="center">
                                <eluc:Number ID="txtnopage" runat="server" CssClass="input" Width="20px" MaxLength="3"
                                    IsInteger="true" />
                                <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                    Width="40px"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="gvCrewSearch" />
            </Triggers>
        </asp:UpdatePanel>
        <eluc:ConfirmMessage ID="ucConfirm" runat="server" Text="" Visible="false" OnConfirmMesage="CloseWindow" />
    </form>
</body>
</html>
