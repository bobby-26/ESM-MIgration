<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterPICAdmin.aspx.cs" Inherits="Registers_RegisterPICAdmin" %>
<%@ Import Namespace="System.Data" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStripTelerik" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Direct PO</title>
    <telerik:RadCodeBlock id="DivHeader" runat="server">
    <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    <style type="text/css">
        .hidden
        {
            display: none;
        }
    </style>

    <script type="text/javascript">
        function resizeDiv() {
            var obj = document.getElementById("divScroll");
            var iframe = document.getElementById("ifMoreInfo");
            var rect = iframe.getBoundingClientRect();
            var x = rect.left;
            var y = rect.top;
            var w = rect.right - rect.left;
            var h = rect.bottom - rect.top;
            obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight) - (h + 70) + "px";
        }
    </script>

</head>
<body>
    <form id="frmInvoiceDirctPO" runat="server">
  <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
       <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" Height="100%">

         
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
             
                 <%--   <eluc:Title runat="server" ID="Title1" Text="Supplier Configuration"></eluc:Title>--%>
                    <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click"
                        CssClass="hidden" />
              
                 
                        <eluc:TabStrip ID="MenuDPO" runat="server" OnTabStripCommand="MenuDPO_TabStripCommand"
                            TabStrip="true"></eluc:TabStrip>
                  
              
            <%--    <div style="position: relative; overflow: hidden; clear: right;">--%>
                    <iframe runat="server" id="ifMoreInfo" scrolling="yes" style="min-height: 575px; width: 99.7%;">
                    </iframe>
          <%--      </div>--%>
              
              
          
      </telerik:RadAjaxPanel>
    </form>
</body>
</html>
