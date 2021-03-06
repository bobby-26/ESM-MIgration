<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CIDOSpecificReport.aspx.cs"
    Inherits="Accounts_CIDOSpecificReport" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>WCT Summary</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
    </div>
    <script type="text/javascript">
        function OffWindowH() {
            var OffWindowH = 0;

            window.scrollTo(0, 10000000);

            if (typeof self.pageYOffset != 'undefined')
                OffWindowH = self.pageYOffset;
            else if (document.compatMode && document.compatMode != 'BackCompat')
                OffWindowH = document.documentElement.scrollTop;
            else if (document.body && typeof (document.body.scrollTop) != 'undefined')
                OffWindowH = document.body.scrollTop;
            window.scrollTo(0, 0);

            return OffWindowH;
        }

        function WindowHeight() {
            var WindowHeight = 0;
            if (typeof (window.innerWidth) == 'number')
                WindowHeight = window.innerHeight;
            else if (document.documentElement && document.documentElement.clientHeight)
                WindowHeight = document.documentElement.clientHeight;
            else if (document.body && document.body.clientHeight)
                WindowHeight = document.body.clientHeight;

            return WindowHeight;
        }

        function pHeight() {
            var pHeight = OffWindowH() + WindowHeight();
            return pHeight;
        }

        function ModelDialog() {
            var Modal = document.getElementById("modal");
            if (Modal == null) {
                Modal = document.createElement("div");
                Modal.setAttribute("id", "modal");
                Modal.setAttribute("class", "modal");
                document.body.appendChild(Modal);
            }
            Modal.style.height = pHeight() + 'px';
            Modal.style.display = "block";
        }
        function Loading() {
            var Element = document.getElementById("loading");
            objh = parseFloat(Element.style.height) / 2;
            objw = parseFloat(Element.style.width) / 2;
            Element.style.top = Math.floor(Math.round((document.documentElement.offsetHeight / 2) + document.documentElement.scrollTop) - objh) + 'px';
            Element.style.left = Math.floor(Math.round((document.documentElement.offsetWidth / 2) + document.documentElement.scrollLeft) - objw) + 'px';
            Element.style.display = "block";
        }
        function ShowProgress() {
            ModelDialog();
            Loading();
        }
        function HideProgress() {
            var loading = document.getElementById("loading");
            loading.style.display = "none";
            var modal = document.getElementById("modal");
            if (modal != null)
                modal.style.display = "none";
        }
        var fileDownloadCheckTimer = null
        function Exportcido(vesselid, fnyear, Accountid, usercode, exportoption) {
            document.cookie = "fileDownloadToken=" + new Date() + ";path=/";
            document.getElementById("ifrGeneratedFile").src = "../Options/OptionsAccounts.ashx?methodname=CIDOEXPORT2XL&exportoption=" + exportoption + "&vessel=" + vesselid + "&fnyear=" + fnyear + "&usercode=" + usercode + "&Accountid=" + Accountid;
        }

        function getCookie(cname) {
            var name = cname + "=";
            var ca = document.cookie.split(';');
            for (var i = 0; i < ca.length; i++) {
                var c = ca[i];
                while (c.charAt(0) == ' ') c = c.substring(1);
                if (c.indexOf(name) != -1) return c.substring(name.length, c.length);
            }
            return "";
        }
        function ClearInterval() {
            ShowProgress();
            if (getCookie("fileDownloadToken") == "") {
                HideProgress();
                window.clearInterval(fileDownloadCheckTimer);
            }
        }
        
    </script>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmCIDOSpecificReport" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <eluc:Status ID="ucStatus" runat="server" />
    <div class="navigation" id="Div2" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <iframe runat="server" id="ifrGeneratedFile" src="about:blank" width="0" height="0"
            style="display: none" />
        <div class="subHeader" style="position: relative">
            <eluc:Title runat="server" ID="Title3" Text="CIDO Specific Report" ShowMenu="true">
            </eluc:Title>
            <asp:Button runat="server" ID="cmdHiddenSubmit" />
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="CIDOSpecificReport" runat="server" OnTabStripCommand="CIDOSpecificReport_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
        </div>
        <table id="tbldiv" runat="server" cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td>
                    <div class="subHeader" style="position: relative;">
                        <span class="navSelect" style="margin-top: 0px; float: right; width: auto;">
                            <eluc:TabStrip ID="CIDOSpecific" runat="server" OnTabStripCommand="CIDOSpecific_TabStripCommand">
                            </eluc:TabStrip>
                        </span>
                    </div>
                </td>
            </tr>
        </table>
        <div>
            <table cellpadding="2" cellspacing="1" style="width: 100%">
                <tr>
                    <td style="width: 10%">
                        <asp:Literal ID="lblOwner" runat="server" Text="Owner"></asp:Literal>
                    </td>
                    <td style="width: 80%">
                        <eluc:Address runat="server" ID="ucOwner" CssClass="dropdown_mandatory" AddressType="128"
                            OnTextChangedEvent="ucOwner_Onchange" AutoPostBack="true" AppendDataBoundItems="true" />
                    </td>
                </tr>
                
                <tr>
                    <td>
                         <asp:Literal ID="lblVesselname" runat="server" Text="Vessel Name"></asp:Literal>
                    </td>
                    <td>
                        <asp:DropDownList id="ddlvessel" runat="server" CssClass="dropdown_mandatory" AutoPostBack="true" ></asp:DropDownList>
                    </td> 

                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblYearMonth" runat="server" Text="FinancialYear"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Quick runat="server" ID="ucFinancialYear" QuickTypeCode="55" CssClass="dropdown_mandatory"
                            AppendDataBoundItems="true" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <iframe runat="server" id="ifMoreInfo" scrolling="auto" style="min-height: 600px;
                width: 100%;"></iframe>
        </div>
    </div>
    </form>
</body>
</html>
