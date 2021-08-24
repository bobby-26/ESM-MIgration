<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsSOAGenerationManualReportsDetails.aspx.cs"
    Inherits="Accounts_AccountsSOAGenerationManualReportsDetails" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselAccount" Src="~/UserControls/UserControlVesselAccount.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
 
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
 <html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
  <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
         <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
         <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
         <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
     </div>
    </telerik:RadCodeBlock></head>
   <body>
   <form id="frmInvoice" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts ="false" >
    </ajaxToolkit:ToolkitScriptManager>
     <asp:UpdatePanel runat="server" ID="pnlCommittedcostpost">
        <ContentTemplate>
         <eluc:Error ID="Error1" runat="server" Text="" Visible="false"></eluc:Error>
         <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
          <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
           <eluc:Status runat="server" ID="ucStatus" />
                <div class="subHeader" style="position: relative">
                <eluc:Title runat="server" ID="ttlAdvancePaymen" Text="Manual Report - File Attachment" ShowMenu="false">
                </eluc:Title>
                <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                <eluc:TabStrip ID="MenuCommittedcostpostTab" runat="server" TabStrip="false" OnTabStripCommand="CommittedcostpostTab_TabStripCommand">
                </eluc:TabStrip>
                </div>            
              <iframe runat="server" id="ifMoreInfo" scrolling="yes" style="min-height: 470px; width: 99.5%">
                </iframe>
                   </div>
                 
                          </ContentTemplate>
                           <Triggers>
                          </Triggers>
                          </asp:UpdatePanel>
      </form>                    
</body>
 </html>
        
