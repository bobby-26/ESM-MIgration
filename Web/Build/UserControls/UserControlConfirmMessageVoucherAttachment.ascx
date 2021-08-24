<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlConfirmMessageVoucherAttachment.ascx.cs"
    Inherits="UserControlConfirmMessageVoucherAttachment" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
    <style type="text/css">
        .centered
        {
            position: absolute;
            left: 30%;
            top: 30%;
            z-index: 100;
            background: #FFFFFF;
            margin: 0 auto;
        }
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
        }
    </style>             
    <div id="ModalBG"></div>    
<telerik:RadAjaxPanel runat="server" ID="pnlErrorMessage" HorizontalAlign="Justify" BorderColor="Black" BorderStyle="Double" Width="360px" CssClass="centered" OnLoad="pnlErrorMessage_Load">
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
                <span runat="server" id="spnHeaderMessage" title="Please Confirm" ></span>
                <span runat="server" id="spnErrorMessage"></span>
            </td>
        </tr>            
        <tr>
            <td colspan="2"><font color="#0000CC"></font> 
                <telerik:RadCheckBoxList ID="rdbList" runat="server" >
                    <Items>
                        <telerik:ButtonListItem Value="Line" Text="Remove only for this Voucher line item" Selected="True" />
                        <telerik:ButtonListItem Value="Voucher" Text="Remove from the entire voucher and all lineitems" Selected="True" />
                    </Items>
                </telerik:RadCheckBoxList>
            </td>
        </tr>
        <tr>
            <td align="center">
                <telerik:RadButton runat="server" ID="cmdYes" CommandArgument="1" Text="Delete" OnClick="cmdYes_Click" Width="120px" CssClass="input"/>
            </td>            
        </tr>
    </table>
</telerik:RadAjaxPanel>