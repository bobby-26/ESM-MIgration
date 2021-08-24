<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectronicLogAnnexVIEngineParametersRecord.aspx.cs" Inherits="Log_ElectronicLogAnnexVIEngineParametersRecord" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
     <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
              <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:TabStrip ID="Tabstrip" runat="server" OnTabStripCommand="Tabstrip_TabStripCommand" />
            <table>
                <tr>
                    <td>
                        Adjustment Made
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" TextMode="MultiLine" Width="300px" Rows="2"  ID="tbadjustment" CssClass="input_mandatory"/>
                    </td>
                </tr>
                 <tr>
                    <td colspan="4">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                       Remarks
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" TextMode="MultiLine" Width="300px" Rows="2"  ID="tbremarks"/>
                    </td>
                </tr>
                 <tr>
                    <td colspan="4">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                       Date
                    </td>
                    <td>
                        <eluc:Date runat="server" id="tbdate" CssClass="input_mandatory" />
                        <telerik:RadLabel ID ="sno" runat="server" Visible="false" />
                        <telerik:RadLabel ID="status" runat="server" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <br />
                    </td>
                </tr>
                <tr  id="trreason" visible ="false" runat="server">
                    <td >
                       Reason 
                    </td>
                   
                    <td colspan="3">
                        <telerik:RadTextBox runat="server" ID="tbreason"  Width="200px"  CssClass="input_mandatory" />
                        
                    </td>
                </tr>
                 <tr>
                    <td colspan="4">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        Sign
                    </td>
                    <td colspan="3">
                         
                        <telerik:RadLabel ID="lblinchName" runat="server" Visible="false"></telerik:RadLabel>
                        <telerik:RadLabel ID="lblincRank" runat="server" Visible="false"></telerik:RadLabel>
                       
                        <telerik:RadLabel ID="lblincsign" runat="server" Visible="false"></telerik:RadLabel>
                        <asp:LinkButton runat="server" AlternateText="Incharge Sign" 
                            CommandName="INCHARGEAUTOSIGN" ID="btnInchargeSign" OnClick="btnInchargeSign_Click"
                            ToolTip="Incharge Sign" Width="20PX" Height="20PX">
                                         <span class="icon"> <i class="fas fa-file-signature"></i></span>
                        </asp:LinkButton>
                    </td>
                </tr>
            </table>
          </telerik:RadAjaxPanel>
    </form>
</body>
</html>
