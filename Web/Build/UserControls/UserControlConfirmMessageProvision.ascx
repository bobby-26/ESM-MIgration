<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlConfirmMessageProvision.ascx.cs"
    Inherits="UserControlConfirmMessageProvision" %>
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
<div id="ModalBG"></div>
<telerik:RadAjaxPanel runat="server" ID="pnlErrorMessage"  HorizontalAlign="Justify" BorderColor="Black" EnableAJAX="false" BorderStyle="Double" Width="460px" CssClass="centered" OnLoad="pnlErrorMessage_Load">
    <table width="100%">
        <tr>
            <td valign="top">
                <font size="2"><b>Confirmation Message</b></font>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td colspan="2">
                <span runat="server" id="spnHeaderMessage" title="Please Confirm"></span>
                <span runat="server" id="spnErrorMessage"></span>
            </td>
        </tr>
        <tr>
            <td>
                <span runat="server" id="Span1">Budgeted Victualling Rate</span>
            </td>
            <td>
                <telerik:RadTextBox ID="txtBudgeted" runat="server" CssClass="input" ReadOnly="true"
                    Width="300px"></telerik:RadTextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span runat="server" id="Span2">Actual Victualling Rate</span>
            </td>
            <td>
                <telerik:RadTextBox ID="txtActual" runat="server" CssClass="input" ReadOnly="true"
                    Width="300px"></telerik:RadTextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span runat="server" id="spnHeaderMessage1">Reason for High Victualling</span>
                <br />
                <span runat="server" id="spnHeaderMessage2"></span>
            </td>
            <td>
                <telerik:RadTextBox ID="txtRemarks" runat="server" CssClass="input_mandatory"
                    TextMode="MultiLine" Width="300px" Height="30px"></telerik:RadTextBox>
            </td>
        </tr>
        <tr>
        </tr>
      
        <tr>
            <td align="center">
                <telerik:RadButton runat="server" ID="cmdYes" CommandArgument="1" Text="Yes" OnClick="cmdYes_Click" Width="120px" CssClass="input" />
            </td>
            <td align="center">
                <telerik:RadButton runat="server" ID="cmdNo" CommandArgument="0" Text="No" OnClick="cmdNo_Click" Width="120px" CssClass="input" />
            </td>
        </tr>
    </table>
</telerik:RadAjaxPanel>
