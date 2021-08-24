<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlConfirmMessagePurchaseSendMail.ascx.cs"
    Inherits="UserControlConfirmMessagePurchaseSendMail" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<style type="text/css">
    .centered {
        position: absolute;
        left: 30%;
        top: 30%;
        z-index: 100;
        background: #FFFFFF;
        margin: 0 auto;
    }

    #ModalBG {
        background-color: #333333;
        position: absolute;
        top: 0;
        left: 0;
        z-index: 99;
        opacity: .40;
        filter: alpha(opacity=40);
        -moz-opacity: .95;
    }
</style>
<div id="ModalBG">
</div>
<telerik:RadAjaxPanel runat="server" ID="pnlErrorMessage" HorizontalAlign="Justify" EnableAJAX="false" BorderColor="Black" 
    BorderStyle="Double" Width="360px" CssClass="centered" OnLoad="pnlErrorMessage_Load">
    <table width="100%">
        <tr valign="top" align="right">
            <td>
                <asp:ImageButton runat="server" AlternateText="Close" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                    ID="ImgClose" ToolTip="Close" OnClick="ImgClose_Click"></asp:ImageButton>
            </td>
        </tr>
        <tr>
            <td valign="top" align="center">
                <font size="3"><b>Confirmation Message</b></font>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td colspan="2">
                <span runat="server" id="spnHeaderMessage" title="Do you want to send Email ?"></span>
                <span runat="server" id="spnErrorMessage"></span>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <telerik:RadLabel ID="lblchkboxmsg" ForeColor="#0000cc" runat="server" Text="If Don't send Email, please specify the reason."></telerik:RadLabel>                
                 <telerik:RadCheckBoxList runat="server" ID="chkOPtion" Direction="Vertical" OnSelectedIndexChanged="chkOPtion_Changed">
                     <DataBindings DataTextField="FLDHARDNAME"  DataValueField ="FLDHARDCODE"     />              
                </telerik:RadCheckBoxList>

            </td>
        </tr>
        <tr>
            <td>
                <span runat="server" id="spnHeaderMessage1"><font color="#0000CC">If the reason is Others,
                    Please specify the Remarks: </font></span>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <telerik:RadTextBox ID="txtRemarks" runat="server" CssClass="input_mandatory" TextMode="MultiLine"
                    Width="300px" Height="50px" Enabled="false"></telerik:RadTextBox>
            </td>
        </tr>
        <tr>
            <td align="center">
                <telerik:RadButton runat="server" ID="cmdYes" CommandArgument="1" Text="Send Email" OnClick="cmdYes_Click"
                    Width="120px" CssClass="input" />
            </td>
            <td align="center">
                <telerik:RadButton runat="server" ID="cmdNo" CommandArgument="0" Text="Don't Send Email"
                    OnClick="cmdNo_Click" Width="120px" CssClass="input" />
            </td>
        </tr>
    </table>
</telerik:RadAjaxPanel>
