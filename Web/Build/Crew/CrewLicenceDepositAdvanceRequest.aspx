<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewLicenceDepositAdvanceRequest.aspx.cs" Inherits="Crew_CrewLicenceDepositAdvanceRequest" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Deposit Utilization</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmDepositUtilization" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlDepositUtilizationEntry">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%; position: absolute;">
                <div class="subHeader">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Advance Requests" ></eluc:Title>
                        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="DepositUtilization" runat="server" OnTabStripCommand="DepositUtilization_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divFind">
                    <table>
                        <tr>
                            <td>
                                <asp:Literal ID="lblRequestNumber" runat="server" Text="Request Number"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRequestNumber" runat="server" CssClass="readonlytextbox" ReadOnly="true" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblApplicationAmount" runat="server" Text="Application Amount"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtApplicationAmount" runat="server" CssClass="readonlytextbox" ReadOnly="true" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblAmountCommitted" runat="server" Text="Amount Committed"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAmountCommitted" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblBalance" runat="server" Text="Balance"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBalance" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <br />
                <div style="position: relative; width: 15px;">
                    <eluc:TabStrip ID="DepositUtilizationSub" runat="server" OnTabStripCommand="DepositUtilizationSub_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divAdvancesMade" style="position: relative; z-index: +1;">
                    <asp:GridView ID="gvAdvancesMade" runat="server" AutoGenerateColumns="False"
                        Font-Size="11px" Width="100%" CellPadding="3" OnRowCommand="gvAdvancesMade_RowCommand"
                        ShowHeader="true" EnableViewState="false" OnSorting="gvAdvancesMade_Sorting"
                        AllowSorting="true" OnRowEditing="gvAdvancesMade_RowEditing" OnRowCancelingEdit="gvAdvancesMade_RowCancelingEdit"
                         OnRowUpdating="gvAdvancesMade_RowUpdating" OnRowDataBound="gvAdvancesMade_RowDataBound">
                        
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblAdvanceNumber" runat="server" Text="Advance Number"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPaymentDone" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTDONE") %>'></asp:Label>
                                    <asp:Label ID="lblAdvanceId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADVANCEPAYMENTID") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblAdvanceNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADVANCEPAYMENTNUMBER") %>'></asp:Label>
                                    <asp:Label ID="lblRequestBalance" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDBALANCEREMAINING") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblConsulate" runat="server" Text="Consulate"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblConsulate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblCurrency" runat="server" Text="Currency"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblDepositAmount" runat="server" Text="Deposit Amount"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDepositAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPOSITAMOUNT") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblUtilizedAmount" runat="server" Text="Utilized Amount"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblUtilizedAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUTILIZEDAMOUNT") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblBalance" runat="server" Text="Balance"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblBalance" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMAININGAMOUNT") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblAmounttobeUtilized" runat="server" Text="Amount to be Utilized"></asp:Literal>
                                </HeaderTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="ucAmount" runat="server" CssClass="input_mandatory" DecimalPlace="2" Text='' />
                                </EditItemTemplate> 
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
                                        CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="View All Committed Amount" ImageUrl="<%$ PhoenixTheme:images/te_pqtes.png %>"
                                        CommandName="HISTORY" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdHistory"
                                        ToolTip="View All Committed Amount"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Update" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="divPage1" style="position: relative;">
                    <table width="100%" border="0" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap align="center">
                                <asp:Label ID="lblPagenumber1" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblPages1" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblRecords1" runat="server">
                                </asp:Label>&nbsp;&nbsp;
                            </td>
                            <td nowrap align="left" width="50px">
                                <asp:LinkButton ID="cmdPrevious1" runat="server" OnCommand="PagerButtonClick1" CommandName="prev">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap align="right" width="50px">
                                <asp:LinkButton ID="cmdNext1" OnCommand="PagerButtonClick1" runat="server" CommandName="next">Next >></asp:LinkButton>
                            </td>
                            <td nowrap align="center">
                                <asp:TextBox ID="txtnopage1" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                </asp:TextBox>
                                <asp:Button ID="btnGo1" runat="server" Text="Go" OnClick="cmdGo_Click1" CssClass="input"
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
