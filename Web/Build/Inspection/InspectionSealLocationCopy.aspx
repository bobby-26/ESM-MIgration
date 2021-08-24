<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionSealLocationCopy.aspx.cs" Inherits="InspectionSealLocationCopy" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Seal Location Copy</title>
    <telerik:RadCodeBlock runat="server" >
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

   </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSealLocationCopy" runat="server" autocomplete="off">
   <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                   <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
              
                <div style="font-weight: 600; font-size: 12px;" runat="server">
                    <eluc:TabStrip ID="MenuCopy" runat="server" OnTabStripCommand="MenuCopy_TabStripCommand">
                    </eluc:TabStrip>
                </div>               
                <div id="divFind" style="position: relative; z-index: 2">
                    <table width="100%" id="tblInspection">
                        <tr>
                            <td>
                               <telerik:RadLabel ID="lblSourceHeader" runat="server" Text="Source"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtSource" runat="server" CssClass="input" Enabled="false"></telerik:RadTextBox>
                            </td>                            
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblDestination" runat="server" Text="Destination"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" />
                            </td>
                        </tr>
                    </table>
                </div>                
            </div>
            <eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="btnConfirm_Click" OKText="Yes"
                CancelText="No" /> 
        
    </form>
</body>
</html>
