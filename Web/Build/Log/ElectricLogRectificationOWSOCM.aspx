<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogRectificationOWSOCM.aspx.cs" Inherits="Log_ElectricLogRectificationOWSOCM" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Rectification of OWS, OCM or Stopping Device</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    <style>
        .style_one {
            border: 1px solid black;
            border-collapse: collapse;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <telerik:RadAjaxPanel runat="server">
            <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
            <eluc:Message ID="ucMessage" runat="server" Text="" Visible="false"></eluc:Message>
            <eluc:TabStrip ID="gvTabStrip" runat="server" OnTabStripCommand="gvTabStrip_TabStripCommand"></eluc:TabStrip>
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" Style="display: none;" />

            <div style="width: 100%; overflow: auto;">
                <div style="float: initial; margin: 0 50px">
                  
                    <table>
                        <tr>
                          <td>
                                <telerik:RadLabel ID="RadLabel9" runat="server" Text="Date of Operation"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadDatePicker RenderMode="Lightweight" runat="server" AutoPostBack="true" CssClass="input_mandatory" ID="txtOperationDate" MaxLength="12" OnSelectedDateChanged="txt_TextChanged" >
                                    <DateInput DateFormat="dd-MMM-yyyy" runat="server"></DateInput>
                                </telerik:RadDatePicker>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblFailureTime" Text="Failure Time" runat="server"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTimePicker ID="txtFailureTime" runat="server" CssClass="input_mandatory" AutoPostBack="true"  OnSelectedDateChanged="txt_TextChanged">
                                    <TimeView Interval="00:30:00" runat="server"></TimeView>
                                </telerik:RadTimePicker>
                            </td>
                            <td>
                                <telerik:RadLabel ID="Radlabel4" Text="HH:MM" runat="server"></telerik:RadLabel>
                            </td>

                            <td>
                                <telerik:RadLabel ID="RadLabel2" Text="UTC Failure Time" runat="server"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTimePicker ID="txtUTCFailureTime" runat="server" CssClass="input_mandatory" AutoPostBack="true"  OnSelectedDateChanged="txt_TextChanged">
                                    <TimeView Interval="00:30:00" runat="server"></TimeView>
                                </telerik:RadTimePicker>
                            </td>
                            <td>
                                <telerik:RadLabel ID="Radlabel3" Text="HH:MM" runat="server"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblEstimatedTime" Text="Time System is Functional" runat="server"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTimePicker ID="txtEstimatedTime" runat="server" CssClass="input_mandatory" AutoPostBack="true" OnSelectedDateChanged="txt_TextChanged">
                                    <TimeView Interval="00:30:00" runat="server"></TimeView>
                                </telerik:RadTimePicker>
                            </td>
                            <td>
                                <telerik:RadLabel ID="Radlabel6" Text="HH:MM" runat="server"></telerik:RadLabel>
                            </td>


                             <td>
                                <telerik:RadLabel ID="RadLabel5" Text="Time System is Functional" runat="server"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTimePicker ID="txtUTCEstimatedTime" runat="server" CssClass="input_mandatory" AutoPostBack="true" OnSelectedDateChanged="txt_TextChanged">
                                    <TimeView Interval="00:30:00" runat="server"></TimeView>
                                </telerik:RadTimePicker>
                            </td>
                            <td>
                                <telerik:RadLabel ID="Radlabel7" Text="HH:MM" runat="server"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblFailureReason" runat="server" Text="Reason For Failure"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtFailureReason"  OnTextChanged="txt_TextChanged" AutoPostBack="true" TextMode="MultiLine" CssClass="input_mandatory"  RenderMode="Lightweight" runat="server">
                                </telerik:RadTextBox>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblOverboardValue" runat="server" Text="Overboard Value no"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtOverboardValue"  runat="server" AutoPostBack="true" OnTextChanged="txt_TextChanged" CssClass="input_mandatory" RenderMode="Lightweight" MaxLength="10"></telerik:RadTextBox>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblSealNo" runat="server" Text="Seal No"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtSealNo" runat="server" AutoPostBack="true" CssClass="input_mandatory" OnTextChanged="txt_TextChanged" RenderMode="Lightweight" MaxLength="10"></telerik:RadTextBox>
                                
                            </td>
                            <td></td>
                        </tr>
                    </table>
                    <br />
                    <table cellpadding="5px" cellspacing="5px" class="style_one" style="font-size: small; padding: 0 50px; width: 100%">
                        <tr class="style_one">
                            <td class="style_one" style="width: 50px">
                                <b>
                                    <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                                </b>
                            </td>
                            <td class="style_one" style="width: 40px">
                                <b>
                                    <telerik:RadLabel ID="capCode" runat="server" Text="Code"></telerik:RadLabel>
                                </b>
                            </td>
                            <td class="style_one" style="width: 70px">
                                <b>
                                    <telerik:RadLabel ID="capItemNo" runat="server" Text="Item No."></telerik:RadLabel>
                                </b>
                            </td>
                            <td class="style_one">
                                <b>
                                    <telerik:RadLabel ID="lblcapRecord" runat="server" Text="Record of operation / Signature of officer in charge"></telerik:RadLabel>
                                </b>
                            </td>
                        </tr>
                        <tr class="style_one">
                            <td class="style_one">
                                <telerik:RadLabel ID="txtDate" runat="server" Width="100px"></telerik:RadLabel>
                            </td>
                            <td class="style_one" style="text-align: center">
                                <telerik:RadLabel ID="txtCode" runat="server"></telerik:RadLabel>
                            </td>
                            <td class="style_one" style="text-align: center">
                                <telerik:RadLabel ID="lblItemNo1" runat="server"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblRecord1" runat="server"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td class="style_one" style="text-align: center">
                                <telerik:RadLabel ID="lblItemNo2" runat="server"></telerik:RadLabel>
                            </td>
                            <td class="style_one">
                                <telerik:RadLabel ID="lblRecord2" runat="server"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td class="style_one" style="text-align: center">
                                <telerik:RadLabel ID="lblItemNo3" runat="server"></telerik:RadLabel>
                            </td>
                            <td class="style_one">
                                <telerik:RadLabel ID="lblRecord3" runat="server"></telerik:RadLabel>
                            </td>
                        </tr>
                      <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td class="style_one">
                                    <b>
                                        <telerik:RadLabel ID="lblIncharge" runat="server" Text="Officer Incharge :"></telerik:RadLabel>
                                    </b>
                                    <telerik:RadLabel ID="lblinchId" runat="server" Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblinchName" runat="server" Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblincRank" runat="server" Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblincSignDate" runat="server" Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblincsign" runat="server" Visible="false"></telerik:RadLabel>
                                    <asp:LinkButton runat="server" AlternateText="Incharge Sign"  
                                        CommandName="INCHARGESIGN" ID="btnInchargeSign"  OnClick="btnInchargeSign_Click"
                                        ToolTip="Incharge Sign" Width="20PX" Height="20PX">
                                         <span class="icon"> <i class="fas fa-file-signature"></i></span>
                                    </asp:LinkButton>
                                </td>
                            </tr>
                        <tr>
                            <td class="style_one" style="text-align: center">
                                <telerik:RadLabel ID="txtDate1" runat="server" Width="100px"></telerik:RadLabel>
                            </td>
                            <td class="style_one" style="text-align: center">
                                <telerik:RadLabel ID="txtCode1" runat="server"></telerik:RadLabel>
                            </td>
                            <td class="style_one" style="text-align: center"></td>
                            <td class="style_one">
                                <telerik:RadLabel ID="lblRecord4" runat="server"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td class="style_one" style="text-align: center">
                                <telerik:RadLabel ID="lblItemNo5" runat="server"></telerik:RadLabel>
                            </td>
                            <td class="style_one">
                                <telerik:RadLabel ID="lblRecord5" runat="server"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td class="style_one">
                                    <b>
                                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="Officer Incharge :"></telerik:RadLabel>
                                    </b>
                                    <telerik:RadLabel ID="lblSealinchId" runat="server" Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblSealinchName" runat="server" Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblSealincRank" runat="server" Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblSealincSignDate" runat="server" Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblSealInchargeRankId" runat="server" Visible="false"></telerik:RadLabel>
                                    <asp:LinkButton runat="server" AlternateText="Seal Incharge Sign"  OnClick="btnSealInchargeSign_Click"
                                        CommandName="INCHARGESIGN" ID="btnSealInchargeSign"  
                                        ToolTip="Incharge Sign" Width="20PX" Height="20PX">
                                         <span class="icon"> <i class="fas fa-file-signature"></i></span>
                                    </asp:LinkButton>
                                </td>
                            </tr>
                    </table>
                </div>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
