<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsInvoiceIncomingInvoice.aspx.cs"
    Inherits="AccountsInvoiceIncomingInvoice" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationality.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Airport" Src="~/UserControls/UserControlAirport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="../UserControls/UserControlCommonAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlEarMarkCompany" Src="~/UserControls/UserControlEarMarkCompany.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Add Company</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="ds" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body style="overflow: hidden">
    <form id="form1" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts ="false" >
    </ajaxToolkit:ToolkitScriptManager>
    <div>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader" style="position: relative">
            <div id="divHeading" style="vertical-align: top">
                <asp:Literal ID="lblIncomingInvoice" runat="server" Text="Incoming Invoice"></asp:Literal>
            </div>
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuCompanyList" runat="server" OnTabStripCommand="CompanyList_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <div>
            <table>
                <tr>
                    <td width="10%">
                        <asp:Literal ID="lblEarMarkedCompany" runat="server" Text="Ear Marked Company"></asp:Literal>
                    </td>
                    <td width="20%">
                        <eluc:UserControlEarMarkCompany ID="ddlEarmarkedCompany" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                            Enabled="false" CssClass="input" runat="server" AppendDataBoundItems="true"  />
                        &nbsp;
                    </td>
                    <td width="10%">
                        <asp:Literal ID="lblDispatchDate" runat="server" Text="Dispatch Date"></asp:Literal>
                    </td>
                    <td width="20%">
                        <eluc:UserControlDate ID="txtDateofDispatch" runat="server" CssClass="readonlytextbox" Width ="120"
                            ReadOnly="true" />
                        &nbsp; &nbsp;
                    </td>
                </tr>
                <tr>
                    <td width="10%">
                        <asp:Literal ID="lblAirWayBillNumber" runat="server" Text="AirWay Bill Number"></asp:Literal>
                    </td>
                    <td width="20%">
                        <asp:TextBox ID="txtBillNumber" runat="server" CssClass="readonlytextbox" MaxLength="100" Width ="120"
                            ReadOnly="true"></asp:TextBox>
                    </td>
                    <td width="10%">
                        <asp:Literal ID="lblPersonprepared" runat="server" Text="Person prepared"></asp:Literal>
                    </td>
                    <td width="20%">
                        <asp:TextBox ID="txtPersonPrepared" runat="server" CssClass="readonlytextbox" MaxLength="100" Width ="120"
                            ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblInvoiceList" runat="server" Text="Invoice List"></asp:Literal>
                    </td>
                    <td>
                        <asp:CheckBoxList ID="CblInvoiceList" runat="server" BorderWidth="2" Width ="240">
                        </asp:CheckBoxList>
                    </td>
                    <td>
                        <asp:Literal ID="lblReceivedDate" runat="server" Text="Received Date"></asp:Literal></td>
                    <td>
                        <eluc:UserControlDate ID="txtRecieveDate" runat="server" Width ="120" />
                        </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
