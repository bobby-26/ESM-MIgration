<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogQuarterlyTestingOWS.aspx.cs" Inherits="Log_ElectricLogQuarterlyTestingOWS" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Latitude" Src="~/UserControls/UserControlLatitude.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Longitude" Src="~/UserControls/UserControlLongitude.ascx" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Quarterly testing of OWS by actual overboard discharge</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
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

        .table-fullwidth {
            width: 100%;
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

            <div style="margin: 0 50px">
                <table style="padding-left: 50px;">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblOperationDate" runat="server" Text="Date of Operation"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadDatePicker RenderMode="Lightweight" runat="server" CssClass="input_mandatory" AutoPostBack="true" ID="txtOperationDate" MaxLength="12" OnSelectedDateChanged="txtOperationDate_SelectedDateChanged" >
                                <DateInput DateFormat="dd-MMM-yyyy" runat="server"></DateInput>
                            </telerik:RadDatePicker>
                            <telerik:RadLabel runat="server" ID="lblBilgeTransferId" Visible="false"></telerik:RadLabel>
                            <telerik:RadLabel runat="server" ID="lblTranscationDetailId" Visible="false"></telerik:RadLabel>
                        </td>
                        <td colspan="3"></td>
                    </tr>
                    <tr>
                        <td>
                            <b>
                                <telerik:RadLabel ID="lblFrom" runat="server" Text="Quarterly OWS Operation"></telerik:RadLabel>
                            </b>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlTransferFrom" AutoPostBack="true" OnSelectedIndexChanged="ddlTransferFrom_SelectedIndexChanged" CssClass="input_mandatory" dropdownposition="Static" runat="server" DataTextField="FLDNAME" DataValueField="FLDLOCATIONID" EnableLoadOnDemand="True"
                                EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                            </telerik:RadComboBox>
                        </td>
                        <td colspan="3"></td>

                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblBfrTrnsROBFrom" runat="server" Text="Before OWS Ops ROB"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadNumericTextBox ID="txtBfrTrnsROBFrom" AutoPostBack="true" CssClass="input_mandatory" MinValue="0" MaxValue="99999999" RenderMode="Lightweight" runat="server" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number">
                                <NumberFormat DecimalSeparator="." DecimalDigits="2" />
                                <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                            </telerik:RadNumericTextBox>
                            <telerik:RadLabel ID="lblfromUnit" runat="server" Text="m3"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadLabel ID="RadLabel4" runat="server" Text="Sounding of Bilge tank before test"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadNumericTextBox ID="txtSoundingBefore" AutoPostBack="true" CssClass="input_mandatory" MinValue="0" MaxValue="99999999" OnTextChanged="txt_SelectedDateChanged" RenderMode="Lightweight" runat="server" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number">
                                <NumberFormat DecimalSeparator="." DecimalDigits="2" />
                                <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                            </telerik:RadNumericTextBox>                            
                            <telerik:RadLabel ID="RadLabel23" runat="server" Text="mtr"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblBfrTrnsROBTo" runat="server" Text="After OWS Ops ROB"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadNumericTextBox ID="txtAftrTrnsROBFrom" CssClass="input_mandatory" AutoPostBack="true" MinValue="0" MaxValue="99999999" RenderMode="Lightweight" runat="server" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number"  OnTextChanged="txt_SelectedDateChanged">
                                <NumberFormat DecimalSeparator="." DecimalDigits="2" />
                                <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                            </telerik:RadNumericTextBox>
                            <telerik:RadLabel ID="lbltounit" runat="server" Text="m3"></telerik:RadLabel>
                        </td>

                        <td>
                            <telerik:RadLabel ID="RadLabel6" runat="server" Text="Sounding of Bilge tank after test"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadNumericTextBox ID="txtSoundingAfter" AutoPostBack="true" CssClass="input_mandatory" MinValue="0" MaxValue="99999999" OnTextChanged="txt_SelectedDateChanged" RenderMode="Lightweight" runat="server" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number">
                                <NumberFormat DecimalSeparator="." DecimalDigits="2" />
                                <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                            </telerik:RadNumericTextBox>  
                            <telerik:RadLabel ID="RadLabel24" runat="server" Text="mtr"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblStartTime" runat="server" Text="Start Time"></telerik:RadLabel>
                        </td>
                        <td>

                            <telerik:RadTimePicker ID="txtStartTime" runat="server" AutoPostBack="true" CssClass="input_mandatory"  OnSelectedDateChanged="txt_SelectedDateChanged">
                                <TimeView Interval="00:30:00"></TimeView>
                            </telerik:RadTimePicker>
                            <telerik:RadLabel ID="Radlabel1" runat="server" Text="HH:MM"></telerik:RadLabel>
                        </td>

                        <td style="text-align: right">
                            <telerik:RadLabel ID="lblStartPosistion" runat="server" Text="Start Position"></telerik:RadLabel>
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
                            <telerik:RadLabel ID="lblStopTime" runat="server" Text="End Time"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTimePicker ID="txtStopTime" runat="server" CssClass="input_mandatory" AutoPostBack="true" OnSelectedDateChanged="txt_SelectedDateChanged">
                                <TimeView Interval="00:30:00"></TimeView>
                            </telerik:RadTimePicker>
                            <telerik:RadLabel ID="Radlabel2" runat="server" Text="HH:MM"></telerik:RadLabel>
                        </td>
                        <td style="text-align: right">
                            <telerik:RadLabel ID="lblStopPosistion" runat="server" Text="Stop Position"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Latitude ID="txtStopPosistionLat" runat="server" CssClass="input_mandatory" AutoPostBack="true"  OnSelectedDateChanged="txt_SelectedDateChanged" />
                        </td>
                        <td>
                            <eluc:Longitude ID="txtStopPosistionLog" runat="server" CssClass="input_mandatory" AutoPostBack="true"  OnSelectedDateChanged="txt_SelectedDateChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblOperatingTime" runat="server" Text="Operating Time"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadNumericTextBox ID="txtOperatingTime" CssClass="input_mandatory" AutoPostBack="true" MinValue="0" MaxValue="99999999" RenderMode="Lightweight" runat="server" MaxLength="22" Width="120px"  OnTextChanged="txt_SelectedDateChanged" Style="text-align: right;" Type="Number">
                                <NumberFormat DecimalSeparator="." DecimalDigits="2" />
                                <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                            </telerik:RadNumericTextBox>
                            <telerik:RadLabel ID="Radlabel3" runat="server" Text="Hours"></telerik:RadLabel>
                        </td>
                        <td colspan="3"></td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblOWSCapacityRunTime" runat="server" Text="OWS Capacity Runtime"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadNumericTextBox ID="txtOWSCapacityRunTime" CssClass="input_mandatory" RenderMode="Lightweight" MinValue="0" MaxValue="99999999" AutoPostBack="true" runat="server" MaxLength="22" Width="120px"  OnTextChanged="txt_SelectedDateChanged" Style="text-align: right;" Type="Number" >
                                <NumberFormat DecimalSeparator="." DecimalDigits="2" />
                                <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                            </telerik:RadNumericTextBox>
                            <telerik:RadLabel ID="Radlabel5" runat="server" Text="CBM"></telerik:RadLabel>
                        </td>
                        <td colspan="3"></td>
                    </tr>
                </table>
                <br />
                <table style="padding-left: 50px;" cellpadding="5px" cellspacing="5px" class="style_one table-fullwidth">
                    <tr>
                        <td style="width: 50px">
                            <b>
                                <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                            </b>
                        </td>
                        <td style="width: 40px">
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
                            <telerik:RadLabel ID="txtItemNo" runat="server"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblRecord" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td class="style_one" style="text-align: center"></td>
                        <td class="style_one">
                            <telerik:RadLabel ID="lblRecord1" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td class="style_one" style="text-align: center"></td>
                        <td class="style_one">
                            <telerik:RadLabel ID="lblRecord2" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>

                    <tr>
                        <td></td>
                        <td></td>
                        <td class="style_one" style="text-align: center"></td>
                        <td class="style_one">
                            <telerik:RadLabel ID="lblRecord3" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>

                    <tr>
                        <td></td>
                        <td></td>
                        <td class="style_one" style="text-align: center"></td>
                        <td class="style_one">
                            <telerik:RadLabel ID="lblRecord4" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>

                    <tr>
                        <td></td>
                        <td></td>
                        <td class="style_one" style="text-align: center"></td>
                        <td class="style_one">
                            <telerik:RadLabel ID="lblRecord5" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>

                    <tr>
                        <td></td>
                        <td></td>
                        <td class="style_one" style="text-align: center"></td>
                        <td class="style_one">
                            <telerik:RadLabel ID="lblRecord6" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>

                    <tr>
                        <td></td>
                        <td></td>
                        <td class="style_one" style="text-align: center"></td>
                        <td class="style_one">
                            <telerik:RadLabel ID="lblRecord7" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>

                    <tr>
                        <td></td>
                        <td></td>
                        <td class="style_one" style="text-align: center"></td>
                        <td class="style_one">
                            <telerik:RadLabel ID="lblRecord8" runat="server"></telerik:RadLabel>
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
                            <telerik:RadLabel runat="server" ID="lblDate1"></telerik:RadLabel>
                        </td>
                        <td class="style_one" style="text-align: center">
                            <telerik:RadLabel runat="server" ID="lblCode1"></telerik:RadLabel>
                        </td>
                        <td class="style_one" style="text-align: center">
                            <telerik:RadLabel runat="server" ID="lblItemNo1"></telerik:RadLabel>
                        </td>
                        <td class="style_one">
                            <telerik:RadLabel ID="lblRecord9" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td class="style_one" style="text-align: center">
                        </td>
                        <td class="style_one">
                            <telerik:RadLabel ID="lblRecord10" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td class="style_one" style="text-align: center">
                            <telerik:RadLabel runat="server" ID="lblItemNo2"></telerik:RadLabel>
                        </td>
                        <td class="style_one">
                            <telerik:RadLabel ID="lblRecord11" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td class="style_one" style="text-align: center">
                            <telerik:RadLabel runat="server" ID="lblItemNo3"></telerik:RadLabel>
                        </td>
                        <td class="style_one">
                            <telerik:RadLabel ID="lblRecord12" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td class="style_one" style="text-align: center"></td>
                        <td class="style_one">
                            <telerik:RadLabel ID="lblRecord13" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td class="style_one" style="text-align: center"></td>
                        <td class="style_one">
                            <telerik:RadLabel ID="lblRecord14" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td class="style_one">
                                    <b>
                                        <telerik:RadLabel ID="RadLabel8" runat="server" Text="Officer Incharge :"></telerik:RadLabel>
                                    </b>
                                    <telerik:RadLabel ID="lblSealinchId" runat="server" Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblSealinchName" runat="server" Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblSealincRank" runat="server" Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblSealincSignDate" runat="server" Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblSealInchargeRankId" runat="server" Visible="false"></telerik:RadLabel>
                                    <asp:LinkButton runat="server" AlternateText="Seal Incharge Sign"  
                                        CommandName="INCHARGESIGN" ID="btnSealInchargeSign" OnClick="btnSealInchargeSign_Click" 
                                        ToolTip="Incharge Sign" Width="20PX" Height="20PX">
                                         <span class="icon"> <i class="fas fa-file-signature"></i></span>
                                    </asp:LinkButton>
                                </td>
                            </tr>
                </table>
            </div>
            <br />
            <br />
            <br />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
