<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsPettyCashGeneralNew.aspx.cs"
    Inherits="VesselAccountsPettyCashGeneralNew" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeaPort" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew BOW</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlNTBRManager">
            <ContentTemplate>
                <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <div class="subHeader" style="position: relative">
                        <eluc:Title runat="server" ID="Title1" Text="Log"></eluc:Title>
                    </div>
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                        <eluc:TabStrip ID="MenuPettyCash" runat="server" OnTabStripCommand="MenuPettyCash_TabStripCommand"
                            TabStrip="true"></eluc:TabStrip>
                    </div>
                    <div class="subHeader">
                        <div style="position: absolute; right: 0px">
                            <eluc:TabStrip ID="MenuPettyCash1" runat="server" OnTabStripCommand="MenuPettyCash1_TabStripCommand"></eluc:TabStrip>
                        </div>
                    </div>
                    <br />
                    <table width="50%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                                <asp:Literal ID="lblFromDate" runat="server" Text="From"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Date ID="txtFromDate" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
                            </td>
                            <td>
                                <asp:Literal ID="lblToDate" runat="server" Text="To"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Date ID="txtToDate" runat="server" CssClass="input_mandatory" />
                            </td>
                        </tr>
                        <%--    <td>
                            <asp:Label ID="lblClosingDate" runat="server" Text="Closing On"></asp:Label>
                        </td>
                        <td>
                            <eluc:Date ID="txtClosingDate" runat="server" CssClass="input_mandatory" />
                        </td> <tr >
                        <td>
                            <asp:Literal ID="lblCashOnboard" runat="server" Text="Cash Onboard"></asp:Literal>
                        </td>
                        <td colspan="5">
                            <asp:TextBox ID="txtCashOnboard" runat="server" CssClass="readonlytextbox txtNumber"
                                ReadOnly="true" Width="90px"></asp:TextBox>
                        </td>
                    </tr>--%>
                        <tr>
                            <td colspan="4">
                                <asp:GridView ID="gvCTM" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                    Width="100%" CellPadding="3" OnRowDataBound="gvCTM_RowDataBound" ShowHeader="true"
                                    ShowFooter="true" EnableViewState="false" GridLines="None">
                                    <FooterStyle Font-Bold="true" HorizontalAlign="Right"></FooterStyle>
                                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <%#((DataRowView)Container.DataItem)["FLDPURPOSE"] %>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <b>Total</b>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Receipts" HeaderStyle-HorizontalAlign="Right">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <ItemTemplate>
                                                <%#",1,".Contains("," + ((DataRowView)Container.DataItem)["FLDCREDITDEBIT"].ToString() + ",") ? ((DataRowView)Container.DataItem)["FLDAMOUNT"] : string.Empty%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Payments" HeaderStyle-HorizontalAlign="Right">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <ItemTemplate>
                                                <%#!",1,".Contains("," + ((DataRowView)Container.DataItem)["FLDCREDITDEBIT"].ToString() + ",") ? ((DataRowView)Container.DataItem)["FLDAMOUNT"] : string.Empty%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div id="div1" runat="server">
                        <table width="50%" cellpadding="1" cellspacing="1">
                            <tr>
                                <td>Receipts
                                </td>
                                <td>(+)
                                </td>
                                <td>
                                    <asp:TextBox ID="txtReceipts" runat="server" CssClass="readonlytextbox txtNumber"
                                        ReadOnly="true" Width="90px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Payments
                                </td>
                                <td>(-)
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPayments" runat="server" CssClass="readonlytextbox txtNumber"
                                        ReadOnly="true" Width="90px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Closing Balance
                                </td>
                                <td>(=)
                                </td>
                                <td>
                                    <asp:TextBox ID="txtclosingbalance" runat="server" CssClass="readonlytextbox txtNumber"
                                        ReadOnly="true" Width="90px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                    <eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="LockCTM_Confirm" Visible="false" />
                    <eluc:Confirm ID="ucUnLockConfirm" runat="server" OnConfirmMesage="UnLockCTM_Confirm"
                        Visible="false" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
    </form>
</body>
</html>
