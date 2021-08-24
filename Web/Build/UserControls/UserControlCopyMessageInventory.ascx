<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlCopyMessageInventory.ascx.cs" Inherits="UserControlCopyMessageInventory" %>
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
        #divModalBG
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
    <div id="divModalBG"></div>    
<telerik:RadAjaxPanel runat="server" ID="pnlErrorMessage" HorizontalAlign="Justify" BorderColor="Black" BorderStyle="Double" Width="360px" CssClass="centered">
    <table width="100%">
        <tr>
            <td valign="top">
                <font size="2"><b>Confirmation Message</b></font>
            </td>
        </tr>
    </table>
    <table width="100%">        
        <tr>
            <td colspan="2"><font color="#0000CC">Please Specify :</font> 
                <telerik:RadCheckBoxList runat="server" ID="chkOPtion" RepeatDirection="Vertical">
                    <Items>
                    <telerik:ButtonListItem Value="1" Text="Copy Jobs" />
                    </Items>              
                </telerik:RadCheckBoxList>
            </td>
        </tr>
        <tr>
            <td align="center">
                <telerik:RadButton runat="server" ID="cmdYes" CommandArgument="1" Text="Save" OnClick="cmdYes_Click" Width="120px" CssClass="input"/>
            </td>
             <td align="center">
                <telerik:RadButton runat="server" ID="cmdNo" CommandArgument="0"  Text="Cancel" OnClick="cmdNo_Click" Width="120px" CssClass="input"/>
            </td>
        </tr>
    </table>
</telerik:RadAjaxPanel>