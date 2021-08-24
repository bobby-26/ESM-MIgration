<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsOwnerFundPositionDataPopulate.aspx.cs" Inherits="Accounts_AccountsOwnerFundPositionDataPopulate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmMessage" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Finanacial Year Statement</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div2" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
    </div>
    <%--<style type="text/css">
        body
        {
            font-family: Arial;
            font-size: 10pt;
        }
        .Grid, .ChildGrid
        {
            border: 1px solid #ccc;
        }
        .Grid td, .Grid th
        {
            border: 1px solid #ccc;
        }
        .Grid th
        {
            background-color: #B8DBFD;
            color: #333;
            font-weight: bold;
        }
        .ChildGrid td, .ChildGrid th
        {
            border: 1px solid #ccc;
        }
        .ChildGrid th
        {
            background-color: #ccc;
            color: #333;
            font-weight: bold;
        }
    </style>--%>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status ID="ucStatus" runat="server" />
        <eluc:Confirm ID="ucConfirmMessage" runat="server" OKText="Yes" CancelText="No" />
        <%--   <div class="navigation" id="Div1" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <div class="subHeader">
            <div class="divFloatLeft">
                <eluc:Title runat="server" ID="Title3" Text="Owner Fund Position" ShowMenu="true">
                </eluc:Title>
            </div>
            <div class="subHeader">
                <div class="divFloat">
                    <div class="divFloat" style="clear: right">
                        <eluc:TabStrip ID="MenuMainFilter" runat="server" OnTabStripCommand="MenuMainFilter_TabStripCommand"
                            TabStrip="true"></eluc:TabStrip>
                    </div>
                </div>
            </div>
        </div>
        <div class="subHeader">
            <div class="divFloat" style="clear: right">
                <eluc:TabStrip ID="MenuFinancialYearStatement" runat="server" OnTabStripCommand="MenuFinancialYearStatement_TabStripCommand">
                </eluc:TabStrip>
            </div>
        </div>
    </div>--%>
        <div class="subHeader" style="position: relative">
            <div class="divFloatLeft">
                <eluc:Title runat="server" ID="ucTitle" Text="Owner Fund Position" ShowMenu="false"></eluc:Title>
            </div>
            <div class="subHeader">
                <div class="divFloat" style="clear: right">
                    <eluc:TabStrip ID="MenuFinancialYearStatement" runat="server" OnTabStripCommand="MenuFinancialYearStatement_TabStripCommand"></eluc:TabStrip>
                </div>
            </div>
        </div>

        <br />
        <br />
        <table cellpadding="1" cellspacing="1" width="30%">
            <tr>
                <td style="width: 10%">
                    <asp:Literal ID="lblOwner" runat="server" Text="Owner"></asp:Literal>
                </td>
                <td style="width: 80%">
                    <eluc:Address runat="server" ID="ucOwner" CssClass="dropdown_mandatory" AddressType="128" Width="200"
                        AppendDataBoundItems="false" />
                </td>
            </tr>
            <tr>
                <td style="width: 20%">
                    <asp:Literal ID="lblFromDate" runat="server" Text="As On Date"></asp:Literal>
                </td>
                <td style="width: 30%">
                    <eluc:Date ID="txtFromDate" runat="server" Width="60px" CssClass="input_mandatory" DatePicker="true" />
                </td>
            </tr>
        </table>
        <br />
        <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
        <script type="text/javascript">
            $("body").on("click", "[src*=collapsed]", function () {
                $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
                $(this).attr("src", "../css/Theme2/images/expanded.gif");
            });
            $("body").on("click", "[src*=expanded]", function () {
                $(this).attr("src", "../css/Theme2/images/collapsed.gif");
                $(this).closest("tr").next().remove();
            });
        </script>
    </form>
</body>
</html>
