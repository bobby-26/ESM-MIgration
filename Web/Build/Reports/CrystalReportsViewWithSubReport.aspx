<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrystalReportsViewWithSubReport.aspx.cs"
    Inherits="CrystalReportsViewWithSubReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="DisplayMessage" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmMessage" Src="../UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PurchaseConfirmMessage" Src="../UserControls/UserControlConfirmMessagePurchaseSendMail.ascx" %>
<%@ Register Src="../UserControls/UserControlCurrency.ascx" TagName="Currency" TagPrefix="eluc" %>
<%@ Register TagName="Status" TagPrefix="eluc" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register Src="../UserControls/UserControlHard.ascx" TagName="UserControlHard"
    TagPrefix="eluc" %>
<%@ Register Assembly="CrystalDecisions.Web" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PO Preview</title>
   <telerik:RadCodeBlock runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
   </telerik:RadCodeBlock>
  
</head>
<body>
    <form id="form1" runat="server">
     <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>    
    <asp:UpdatePanel runat="server" ID="pnlOrderForm">
        <ContentTemplate>
          <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%; border:none">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status ID="ucStatus" runat="server" />
                <eluc:DisplayMessage ID="ucConfirm" runat="server" Text="" Visible="false" />
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="Title1" Text="Reports"></eluc:Title>
                   </div>  
                 <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="OrderExportToPDF" runat="server" OnTabStripCommand="OrderExportToPDF_TabStripCommand">
                    </eluc:TabStrip>                    
                </div>
     
                <table cellpadding="1" cellspacing="1" width ="100%">
                <tr>
                    <td  align="left">
                        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True" EnableDrillDown="false"
                            HasCrystalLogo="False" HasPrintButton="True" >                           
                            </CR:CrystalReportViewer>                                                    
                    </td>
                </tr>
            </table>     
    </div>
     <input type="button" runat="server" id="isouterpage" name="isouterpage" style="visibility:hidden"/>
     <eluc:ConfirmMessage runat="server" ID="ucConfirmSent" Visible ="false"  Text="" OnConfirmMesage="ucConfirmSent_OnClick"></eluc:ConfirmMessage>
      <eluc:PurchaseConfirmMessage runat="server" ID="ucPurchaseConfirmSent" Visible ="false"  Text="" OnConfirmMesage="ucpurchaseConfirmSent_OnClick" />
    </ContentTemplate>
    </asp:UpdatePanel>       
    </form>
</body>
</html>
