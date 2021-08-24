<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectronicLogAnnexVIODSAdd.aspx.cs" Inherits="Log_ElectronicLogAnnexVIODSAdd" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ozone Depleting Substance Record Add</title>
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
             <eluc:TabStrip ID="Tabstrip" runat="server" OnTabStripCommand="Tabstrip_TabStripCommand" />
             <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <br /><table>
                <tr>
                    <td>
                        Date 
                    </td>
                    <td></td>
                    <td>
                        <telerik:RadDatePicker ID="txtDate" runat="server" CssClass="input_mandatory" >
                        </telerik:RadDatePicker>                        
                    </td>    
                     <td >
                        <telerik:RadTimePicker ID="txtTime" runat="server"    DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm"  Width="63px" TimePopupButton-Visible="false" CssClass="input_mandatory">
                        </telerik:RadTimePicker>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                         Name of Equipment
                    </td>
                   <td>

                   </td>
                    <td colspan="2">
                        <telerik:RadTextBox runat="server" ID="tbequipmentname"  Width="200px" CssClass="input_mandatory"/>
                    </td>
                </tr>
                 <tr>
                    <td colspan="4">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        Maker
                    </td>
                    <td>

                   </td>
                    <td colspan="2">
                        <telerik:RadTextBox runat="server" ID="tbmaker"  Width="100px" />
                    </td>
                </tr>
                 <tr>
                    <td colspan="4">
                        <br />
                    </td>
                </tr>
                 <tr>
                    <td>
                        Model
                    </td>
                    <td>

                   </td>
                    <td colspan="2">
                        <telerik:RadTextBox runat="server" ID="tbmodel"  Width="100px" />
                    </td>
                </tr>
                 <tr>
                    <td colspan="4">
                        <br />
                    </td>
                </tr>
                 <tr>
                    <td>
                        Location
                    </td>
                    <td>

                   </td>
                    <td colspan="2">
                        <telerik:RadTextBox runat="server" ID="tblocation"  Width="100px" />
                    </td>
                </tr>
                 <tr>
                    <td colspan="4">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        Name of the ODS
                    </td>
                    <td>

                   </td>
                    <td colspan="2">
                        <telerik:RadTextBox runat="server" ID="tbods"  Width="200px"  CssClass="input_mandatory"/>
                    </td>
                </tr>
                 <tr>
                    <td colspan="4">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        Mass (Kg)
                    </td>
                    <td>

                   </td>
                    <td colspan="2">
                        <telerik:RadTextBox runat="server" ID="tbmass"  Width="100px"  CssClass="input_mandatory"/>
                        <telerik:RadTextBox runat="server" ID="tbstatus"  Width="100px"  Visible="false"/>
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
                    <td>

                   </td>
                    <td colspan="2">
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
                    <td colspan="2">
                         
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
