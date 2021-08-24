<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewLicenceRequestsApprovedFilter.aspx.cs" Inherits="Crew_CrewLicenceRequestsApprovedFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Licence Requests Payment Filter</title>
       <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPurchaseForm" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlOrderForm">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader">
                    <div class="divFloatLeft">
                        <eluc:Title runat="server" ID="frmTitle" Text="Filter" ShowMenu="false"></eluc:Title>
                    </div>
                    <div class="divFloat">
                        <eluc:TabStrip ID="MenuOrderFormMain" runat="server" OnTabStripCommand="MenuOrderFormMain_TabStripCommand"
                            ></eluc:TabStrip>
                    </div>
                </div>
                <div id="find">
                    <table>
                        <tr>
                            <td>
                                <asp:Literal ID="lblVoucherNumber" runat="server" Text="Voucher Number"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVoucherNumeber" runat="server" CssClass="input"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblLicenceRequestNumber" runat="server" Text="Licence Request Number"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLicenceRequestNumber" runat="server" CssClass="input"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblCurrency" runat="server" Text="Currency"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Currency ID="ucCurrency" runat="server" CssClass="input" AppendDataBoundItems="true" />
                            </td>
                            <td>
                                <asp:Literal ID="lblConsulate" runat="server" Text="Consulate"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Address ID="ucAddress" runat="server" CssClass="input" AppendDataBoundItems="true" AddressType="334" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblBilltoCompany" runat="server" Text="Bill to Company"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Company ID="ucComapny" runat="server" CssClass="input" AppendDataBoundItems="true" />
                            </td>
                            <td>
                                <asp:Literal ID="lblChargedVessel" runat="server" Text="Charged Vessel"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Vessel ID="ucVessel" runat="server" CssClass="input" AppendDataBoundItems="true" EntityType="VSL" AssignedVessels="true"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblCrewName" runat="server" Text="Crew Name"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCrewName" runat="server" CssClass="input" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblCrewRank" runat="server" Text="Crew Rank"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Rank ID="ucRank" runat="server" CssClass="input" AppendDataBoundItems="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblApprovedFromDate" runat="server" Text="Approved From Date"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Date ID="ucFromDate" runat="server" CssClass="input" />
                            </td>
                            <td>
                                <asp:Literal ID="lblApprovedToDate" runat="server" Text="Approved To Date"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Date ID="ucToDate" runat="server" CssClass="input" /> 
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
