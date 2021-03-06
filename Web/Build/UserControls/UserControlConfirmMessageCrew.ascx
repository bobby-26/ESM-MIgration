<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlConfirmMessageCrew.ascx.cs"
    Inherits="UserControlConfirmMessageCrew" %>

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
<telerik:RadAjaxPanel runat="server" ID="pnlErrorMessage" HorizontalAlign="Justify" EnableAJAX="false" BorderColor="Black" BorderStyle="Double" Width="360px" CssClass="centered" OnLoad="pnlErrorMessage_Load">
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
                <span runat="server" id="spnHeaderMessage1"><font color="#0000CC">Remarks:</font></span>

            </td>
        </tr>
        <tr>

            <td colspan="3">
                <telerik:RadTextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Width="300px" Height="30px">
                </telerik:RadTextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2"><font color="#0000CC">
                <telerik:RadLabel  ID="lblchkboxmsg" runat="server" Text="Please Confirm if the seafarer could be processed for following :"></telerik:RadLabel></font>
                <telerik:RadCheckBoxList runat="server" ID="chkOPtion" Direction="Horizontal">
                    <Items>
                        <telerik:ButtonListItem Value="1" Text="Course" Selected="True" />
                        <telerik:ButtonListItem Value="2" Text="Licence" Selected="True" />
                        <telerik:ButtonListItem Value="3" Text="Medical" Selected="True" />
                        <telerik:ButtonListItem Value="4" Text="Travel to vessel" />
                    </Items>
                </telerik:RadCheckBoxList>
            </td>
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
