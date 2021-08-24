<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlConfirmMessage.ascx.cs"
    Inherits="UserControlConfirmMessage" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
    <style type="text/css">
        #ModalBG
        {            
            background-color: #333333;
            position: absolute;
            top: 0;
            left: 0;            
            z-index: 99;
            opacity: .40;
            filter: alpha(opacity=40);
            -moz-opacity: .95;
            width:100%;
            height:100%;
        }
    </style>             
<div id="ModalBG"></div>
<telerik:RadAjaxPanel runat="server" ID="pnlErrorMessage" HorizontalAlign="Justify" BorderColor="Black" BorderStyle="Double" width="360px"
    OnLoad="pnlErrorMessage_Load">
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
                <span runat="server" id="spnHeaderMessage" title="Please Confirm" ></span><br />
                <span runat="server" id="spnErrorMessage"></span><br />
            </td>
        </tr>
        <tr>
            <td align="center">
                <telerik:RadButton runat="server" ID="cmdYes" CommandArgument="1" Text="Yes" OnClick="cmdYes_Click" Width="120px" CssClass="input"/>
            </td>
             <td align="center">
                <telerik:RadButton runat="server" ID="cmdNo" CommandArgument="0"  Text="No" OnClick="cmdNo_Click" Width="120px" CssClass="input"/>
            </td>
        </tr>
    </table>
</telerik:RadAjaxPanel>