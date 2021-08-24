<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogORB2AccidentalDischarge.aspx.cs" Inherits="Log_ElectricLogORB2AccidentalDischarge" %>


<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Latitude" Src="~/UserControls/UserControlLatitude.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Longitude" Src="~/UserControls/UserControlLongitude.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <title>Accidental or Other Exceptional Discharge of Oil</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function NumberFieldKeyPress(obj, arg) {
                if (arg.get_keyCode() == 45) {
                    alert("This field only takes positive number.");
                    arg.set_cancel(true);
                }
            }
        </script>
    </telerik:RadCodeBlock>
    <style>
        .style_one {
            border: 1px solid black;
            border-collapse: collapse;
        }

        .notes {
            margin-top: 2%;
            margin-bottom: 2%;
        }

            .notes p {
                font-size: 13px;
                font-weight: bold;
                margin-top: 0px;
                margin-bottom: 0px;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">

            <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
            <eluc:Message ID="ucMessage" runat="server" Text="" Visible="false"></eluc:Message>
            <eluc:TabStrip ID="gvTabStrip" runat="server" OnTabStripCommand="MenuOperationApproval_TabStripCommand"></eluc:TabStrip>
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" Style="display: none;" />

            <div style="width: 100%; overflow: auto;">
                <div style="float: initial; margin: 0 50px">

                    <h3>Accidental or Other Exceptional Discharge of Oil</h3>

                    <table>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="RadLabel1" runat="server" Text="Date of Operation"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadDatePicker RenderMode="Lightweight" runat="server" CssClass="input_mandatory" AutoPostBack="true" ID="txtOperationDate" MaxLength="12" OnSelectedDateChanged="txtOperationDate_TextChangedEvent">
                                    <DateInput ID="DateInput1" DateFormat="dd-MMM-yyyy" runat="server"></DateInput>
                                </telerik:RadDatePicker>

                            </td>

                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblFailureTime" Text="Time of Accidental Discharge" runat="server"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTimePicker ID="txtaccidentTime" runat="server" CssClass="input_mandatory" AutoPostBack="true" OnSelectedDateChanged="txtFailureTime_SelectedDateChanged">
                                    <TimeView ID="TimeView1" Interval="00:30:00" runat="server"></TimeView>
                                </telerik:RadTimePicker>
                                <telerik:RadLabel ID="Radlabel6" runat="server" Text="HH:MM"></telerik:RadLabel>
                            </td>
                            <%-- <td>
                                <telerik:RadLabel ID="RadLabel5" Text="UTC" runat="server"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTimePicker ID="txtUTC" runat="server" CssClass="input_mandatory" AutoPostBack="true" OnSelectedDateChanged="txtFailureTime_SelectedDateChanged">
                                    <TimeView ID="TimeView2" Interval="00:30:00" runat="server"></TimeView>
                                </telerik:RadTimePicker>
                                <telerik:RadLabel ID="Radlabel8" runat="server" Text="HH:MM"></telerik:RadLabel>
                            </td>--%>
                            <td>
                                <telerik:RadLabel ID="lblStartPosistion" runat="server" Visible="true" Text="Position of Accidental Discharge"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Latitude ID="txtStartPosistionLat" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td>
                                <eluc:Longitude ID="txtStartPosistionLog" runat="server" CssClass="input_mandatory" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="RadLabel4" Text="UTC" runat="server"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTimePicker ID="txtUTC" runat="server" CssClass="input_mandatory" AutoPostBack="true" OnSelectedDateChanged="txtFailureTime_SelectedDateChanged">
                                    <TimeView ID="TimeView5" Interval="00:30:00" runat="server"></TimeView>
                                </telerik:RadTimePicker>
                                <telerik:RadLabel ID="Radlabel10" runat="server" Text="HH:MM"></telerik:RadLabel>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>

                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblBfrTrnsROBFrom" runat="server" Text="Approx Quantity in m3 " Style="vertical-align: text-bottom"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtquantity" AutoPostBack="true" CssClass="input_mandatory" MinValue="0" MaxValue="99999999" RenderMode="Lightweight" OnTextChanged="txtQuantity_TextChangedEvent" runat="server" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number">
                                    <NumberFormat DecimalSeparator="." DecimalDigits="2" />
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                                <telerik:RadLabel ID="lblfromUnit" runat="server" Text="m3"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblSealNo" runat="server" Text="Name of the Port/Dock/Anchorage if applicable"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtnameofport" runat="server" CssClass="input_mandatory" RenderMode="Lightweight" AutoPostBack="true" OnTextChanged="txtFailureReason_TextChanged">
                                </telerik:RadTextBox>
                                <%--<telerik:RadNumericTextBox ID="txtSealNo" RenderMode="Lightweight" CssClass="input_mandatory" runat="server" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number"  OnTextChanged="txtFailureReason_TextChanged">
                                </telerik:RadNumericTextBox>--%>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <telerik:RadLabel ID="RadLabel5" runat="server" Text="Type of Oil/ Cargo Discharged" Style="vertical-align: text-bottom"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txttype" runat="server" Width="200px" TextMode="MultiLine" CssClass="input_mandatory" RenderMode="Lightweight" AutoPostBack="true" OnTextChanged="txtFailureReason_TextChanged">
                                </telerik:RadTextBox>
                            </td>
                            <td style="width: 200px">
                                <telerik:RadLabel ID="RadLabel11" runat="server" Text="Circumstances of discharge or escape, the reasons thereof and general remarks." Style="vertical-align: text-bottom"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtremarks" runat="server" Width="200px" CssClass="input_mandatory" TextMode="MultiLine" RenderMode="Lightweight" AutoPostBack="true" OnTextChanged="txtFailureReason_TextChanged">
                                </telerik:RadTextBox>
                                <%--<telerik:RadNumericTextBox ID="txtSealNo" RenderMode="Lightweight" CssClass="input_mandatory" runat="server" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number"  OnTextChanged="txtFailureReason_TextChanged">
                                </telerik:RadNumericTextBox>--%>
                            </td>
                        </tr>




                    </table>
                    <div>
                        <br />
                        <table cellpadding="5px" cellspacing="5px" class="style_one" style="font-size: small; padding: 0 50px; width: 100%">
                            <tr class="style_one">
                                <td class="style_one" style="width: 50px">
                                    <b>
                                        <telerik:RadLabel ID="lblDate" runat="server" Visible="True" Text="Date"></telerik:RadLabel>
                                    </b>
                                </td>
                                <td class="style_one" style="width: 40px">
                                    <b>
                                        <telerik:RadLabel ID="capCode" runat="server" Visible="True" Text="Code"></telerik:RadLabel>
                                    </b>
                                </td>
                                <td class="style_one" style="width: 70px">
                                    <b>
                                        <telerik:RadLabel ID="capItemNo" runat="server" Visible="True" Text="Item No."></telerik:RadLabel>
                                    </b>
                                </td>
                                <td class="style_one">
                                    <b>
                                        <telerik:RadLabel ID="lblcapRecord" runat="server" Visible="True" Text="Record of operation / Signature of officer in charge"></telerik:RadLabel>
                                    </b>
                                </td>
                            </tr>
                            <tr class="style_one">
                                <td class="style_one">
                                    <telerik:RadLabel ID="txtDate" runat="server" Width="100px" Visible="True"></telerik:RadLabel>
                                </td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="txtCode" runat="server" Visible="True"></telerik:RadLabel>
                                </td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="txtItemNo" runat="server" Visible="True"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblRecord" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="txtItemNo1" runat="server"></telerik:RadLabel>
                                </td>
                                <td class="style_one">
                                    <telerik:RadLabel ID="lblrecord1" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="txtItemNo2" runat="server"></telerik:RadLabel>
                                </td>
                                <td class="style_one">
                                    <telerik:RadLabel ID="lblrecord2" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>


                            <tr>
                                <td></td>
                                <td></td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="txtItemNo3" runat="server"></telerik:RadLabel>
                                </td>
                                <td class="style_one">
                                    <telerik:RadLabel ID="lblrecord3" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>


                            <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td class="style_one">
                                    <b>
                                        <telerik:RadLabel ID="lblIncharge" runat="server" Text="Chief Officer :"></telerik:RadLabel>
                                    </b>
                                    <telerik:RadLabel ID="lblinchId" runat="server" Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblinchName" runat="server" Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblincRank" runat="server" Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblincSignDate" runat="server" Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblincsign" runat="server" Visible="false"></telerik:RadLabel>
                                    <asp:LinkButton runat="server" AlternateText="Incharge Sign"
                                        CommandName="INCHARGESIGN" ID="btnInchargeSign"  OnClick="btnInchargeSign_Click"
                                        ToolTip="Chief Officer Sign" Width="20PX" Height="20PX">
                                         <span class="icon"> <i class="fas fa-file-signature"></i></span>
                                    </asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="notes">
                        <p>Notes:</p>
                        <telerik:RadLabel runat="server" ID="lblnotes"></telerik:RadLabel>
                    </div>
                </div>
            </div>
            <br />
            <br />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
