<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsStatementOfAccountsSummary.aspx.cs"
    Inherits="AccountsStatementOfAccountsSummary" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Statement of Accounts Summary</title>
     <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

      
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmOwnersAccounts" runat="server" autocomplete="off">
    <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>


 <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
          
         
                    <eluc:TabStrip ID="MenuReports" runat="server" OnTabStripCommand="MenuReports_TabStripCommand"></eluc:TabStrip>
            
            <div>
                <table cellpadding="2" cellspacing="1" style="width: 100%">
                    <tr>
                        <td style="width: 10%">
                            <telerik:RadLabel ID="lblVesselAccount" runat="server" Text="Vessel Account"></telerik:RadLabel>
                        </td>
                        <td style="width: 40%">
                            <span id="spnPickListVesselAccount">
                                <telerik:RadTextBox ID="txtAccountCode" runat="server" CssClass="input_mandatory" MaxLength="20"
                                    Width="10%"></telerik:RadTextBox>
                                <telerik:RadTextBox ID="txtAccountDescription" runat="server" CssClass="input_mandatory"
                                    MaxLength="50" Width="36%"></telerik:RadTextBox>
                                <asp:ImageButton runat="server" ID="imgShowAccount" Style="cursor: pointer; vertical-align: top"
                                    ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" OnClientClick="return showPickList('spnPickListVesselAccount', 'codehelp1', '', 'Common/CommonPickListVesselAndAccountCombined.aspx',true); " />
                                <telerik:RadTextBox ID="txtAccountId" runat="server" CssClass="input" MaxLength="20" Width="10px"></telerik:RadTextBox>
                                <telerik:RadTextBox ID="txtVesselId" runat="server" CssClass="input" MaxLength="20" Width="10px"></telerik:RadTextBox>
                            </span>
                        </td>
                        <td style="width: 20%"></td>
                        <td style="width: 20%"></td>
                    </tr>
                    <tr>
                        <td style="width: 20%">
                            <telerik:RadLabel ID="lblFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                        </td>
                        <td style="width: 20%">
                            <eluc:Date ID="ucfromDate" runat="server" CssClass="input_mandatory" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%">
                            <telerik:RadLabel ID="lblToDate" runat="server" Text="To Date"></telerik:RadLabel>
                        </td>
                        <td style="width: 20%">
                            <eluc:Date ID="uctoDate" runat="server" CssClass="input_mandatory" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblcurrency" runat="server" Text="Currency"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Currency runat="server" ID="ucCurrency" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                AutoPostBack="true" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
