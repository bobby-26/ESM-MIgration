<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsReimbursementFilter.aspx.cs"
    Inherits="VesselAccountsReimbursementFilter" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagName="Rank" TagPrefix="eluc" Src="../UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reimbursement / Recovery Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <eluc:TabStrip ID="MenuPD" runat="server" OnTabStripCommand="PD_TabStripCommand"></eluc:TabStrip>
        </div>
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel runat="server" ID="pnlFilter">
            <div id="divFind">
                <table cellpadding="2" cellspacing="2">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblFileno" Text="File No" runat="server"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtFileNo" runat="server" ></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblName" Text="Name" runat="server"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtName" runat="server" ></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblRank" Text="Rank" runat="server"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Rank ID="ddlRank" runat="server" AppendDataBoundItems="true"  />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblReimbursementRecovery" Text="Reimbursement/ Recovery" runat="server"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadDropDownList ID="ddlEarDed" runat="server"  AutoPostBack="true" OnSelectedIndexChanged="ddlEarDed_SelectedIndexChanged">
                                <Items>
                                    <telerik:DropDownListItem Text="--Select--" Value=""></telerik:DropDownListItem>
                                    <telerik:DropDownListItem Text="Reimbursement(B.O.C)" Value="1"></telerik:DropDownListItem>
                                    <telerik:DropDownListItem Text="Reimbursement(Monthly)" Value="2"></telerik:DropDownListItem>
                                    <telerik:DropDownListItem Text="Reimbursement(E.O.C)" Value="3"></telerik:DropDownListItem>
                                    <telerik:DropDownListItem Text="Recovery(B.O.C)" Value="-1"></telerik:DropDownListItem>
                                    <telerik:DropDownListItem Text="Recovery(Monthly)" Value="-2"></telerik:DropDownListItem>
                                    <telerik:DropDownListItem Text="Recovery(E.O.C)" Value="-3"></telerik:DropDownListItem>
                                </Items>
                            </telerik:RadDropDownList>
                            <%--                            <asp:DropDownList ID="ddlEarDed" runat="server" CssClass="input" AutoPostBack="true" OnSelectedIndexChanged="ddlEarDed_SelectedIndexChanged">
                                <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                <asp:ListItem Text="Reimbursement(B.O.C)" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Reimbursement(Monthly)" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Reimbursement(E.O.C)" Value="3"></asp:ListItem>
                                <asp:ListItem Text="Recovery(B.O.C)" Value="-1"></asp:ListItem>
                                <asp:ListItem Text="Recovery(Monthly)" Value="-2"></asp:ListItem>
                                <asp:ListItem Text="Recovery(E.O.C)" Value="-3"></asp:ListItem>
                            </asp:DropDownList>--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblPurpose" Text="Purpose" runat="server"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Hard ID="ddlPurpose" runat="server"  AppendDataBoundItems="true"
                                HardTypeCode="128" ShortNameFilter="TRV,USV,AFR,EBG,CFE,LFE,MEF" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblApproved" Text="Approved/Not Approved" runat="server"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadDropDownList ID="ddlApproved" runat="server" >
                                <Items>
                                    <telerik:DropDownListItem Text="--Select--" Value=""/>
                                    <telerik:DropDownListItem Value="1" Text="Approved" /> 
                                    <telerik:DropDownListItem Value="0" Text="Not Approved" />
                                </Items>
                            </telerik:RadDropDownList>
                            <%--                            <asp:DropDownList ID="ddlApproved" runat="server" >
                                <asp:ListItem Value="">--Select--</asp:ListItem>
                                <asp:ListItem Value="1">Approved</asp:ListItem>
                                <asp:ListItem Value="0">Not Approved</asp:ListItem>
                            </asp:DropDownList>--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblActive" Text="Status" runat="server"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadDropDownList ID="ddlStatus" runat="server" >
                                <Items>
                                    <telerik:DropDownListItem Value="" Text="--Select--"/>
                                    <telerik:DropDownListItem Value="1" Text="Active" />
                                    <telerik:DropDownListItem Value="0" Text="In-Active"/>
                                </Items>
                            </telerik:RadDropDownList>
                        </td>
                    </tr>
                </table>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
