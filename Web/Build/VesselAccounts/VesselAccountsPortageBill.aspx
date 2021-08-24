<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsPortageBill.aspx.cs"
    Inherits="VesselAccountsPortageBill" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselCrew" Src="~/UserControls/UserControlVesselEmployee.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
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
                        <eluc:Title runat="server" ID="Title1" Text="Portage Bill generation" ShowMenu="true"></eluc:Title>
                    </div>
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                        <eluc:TabStrip ID="MenuPB" runat="server" OnTabStripCommand="MenuPB_TabStripCommand"></eluc:TabStrip>
                    </div>
                    <table width="100%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                                <asp:Literal ID="lblFromDate" runat="server" Text="From Date"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Date ID="txtFromDate" runat="server" CssClass="readonlytextbox" Enabled="false" />
                            </td>
                            <td>
                                <asp:Literal ID="lblClosingDate" runat="server" Text="Closing Date"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Date ID="txtClosginDate" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td>
                                <asp:Literal ID="lblPortageBillHistory" runat="server" Text="Portage Bill History"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlHistory" runat="server" CssClass="input" DataTextField="FLDPBCLOSGINDATE"
                                    DataValueField="FLDPBCLOSGINDATE" AutoPostBack="true" OnSelectedIndexChanged="ddlHistory_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div class="navSelect" style="position: relative; clear: both; width: 15px">
                        <eluc:TabStrip ID="MenuPBExcel" runat="server" OnTabStripCommand="MenuPBExcel_TabStripCommand"></eluc:TabStrip>
                    </div>
                    <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                        <asp:GridView ID="gvPB" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            GridLines="None" Width="100%" CellPadding="3" ShowFooter="True" OnRowDataBound="gvPB_RowDataBound"
                            EnableViewState="False">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <Columns>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="FinalizePortageBill_Confirm"
                        Visible="false" />
                    <eluc:Confirm ID="ucUnlock" runat="server" OnConfirmMesage="UnLockPortageBill_Confirm"
                        Visible="false" />
                    <eluc:Status ID="ucStatus" runat="server" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
