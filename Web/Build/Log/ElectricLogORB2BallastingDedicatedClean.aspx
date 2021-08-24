<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogORB2BallastingDedicatedClean.aspx.cs" Inherits="Log_ElectricLogORB2BallastingDedicatedClean" %>

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
<head runat="server">
    <title>Ballasting of Dedicated Clean Ballast Tank (CBT Tankers Only)</title>
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

            .units{
                width: 200px;
            }
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" RenderMode="Lightweight" ID="RadWindowManager1"></telerik:RadWindowManager>

        <telerik:RadAjaxPanel runat="server" ID="radAjaxPanel">

            <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
            <eluc:Message ID="ucMessage" runat="server" Text="" Visible="false"></eluc:Message>
            <eluc:TabStrip ID="gvTabStrip" runat="server" OnTabStripCommand="gvTabStrip_TabStripCommand"></eluc:TabStrip>
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" Style="display: none;" />

            <div style="width: 100%; overflow: auto;">
                <div style="float: initial; margin: 0 20px">
                    <h3>Ballasting of Dedicated Clean Ballast Tank (CBT Tankers Only)</h3>
                    <table>
                        <tr>
                            <td style="width:500px">
                                <telerik:RadLabel ID="lblDateOfOperation" runat="server" Text="Date of Operation"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadDatePicker RenderMode="Lightweight" runat="server" CssClass="input_mandatory" AutoPostBack="true" ID="txtOperationDate" MaxLength="12" OnSelectedDateChanged="txtOperationDate_SelectedDateChanged">
                                    <DateInput ID="DateInput1" DateFormat="dd-MMM-yyyy" runat="server"></DateInput>
                                </telerik:RadDatePicker>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblDedicatedTankCleaned" runat="server" Text="Dedicated Clean Ballast Tank"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox DropDownPosition="Static" CssClass="input_mandatory" ID="ddlTransferedFrom" runat="server" AutoPostBack="true" DataTextField="FLDNAME" OnSelectedIndexChanged="ddlTransferedFrom_TextChanged" DataValueField="FLDLOCATIONID" EnableLoadOnDemand="True"
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td >
                                <span class="right">
                                    <telerik:RadLabel ID="lblQuantityOily" runat="server" Visible="true" Text="Quantity of Oily water which, after line flushing, is transferred to slop tank(s) or Cargo Tank(s) in which slop is primarily stored (Identity of the tank)"></telerik:RadLabel>
                                </span>
                            </td>
                            <td>
                                <telerik:RadComboBox DropDownPosition="Static" CssClass="input_mandatory" ID="ddlTransferedTo" runat="server" AutoPostBack="true" DataTextField="FLDNAME" OnSelectedIndexChanged="ddlTransferedFrom_TextChanged" DataValueField="FLDLOCATIONID" EnableLoadOnDemand="True"
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </td>
                            <td></td>
                            <td style="width:500px">
                                <telerik:RadLabel ID="lblPosistionOnFlusing" runat="server" Text="Position of ship when water intended for flushing or Port Ballast was taken to Clean Ballast Tank"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Latitude ID="txtFlushingLatitude" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td>
                                <eluc:Longitude ID="txtFlushingLongitude" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td>
                                <telerik:RadLabel runat="server" ID="lblStartPos" Text="( xx Deg xx Min N/S, xx Deg xx Min E/W)" CssClass="units"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel runat="server" ID="lblTotalQuantity" Text="Total Quantity in above Tank in meter cubic"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtTotalQuantity" CssClass="input_mandatory" RenderMode="Lightweight" MinValue="0" MaxValue="99999999" runat="server" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number" AutoPostBack="true" OnTextChanged="txtrefresh_TextChanged">
                                    <NumberFormat DecimalSeparator="." DecimalDigits="2" />
                                     <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>

                            </td>
                            <td>
                                <telerik:RadLabel ID="lblfromUnit" runat="server" Text="m3"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblPosistionWhenFlushed" runat="server" Text="Position of Ship when pump(s) & line where flushed to Slop Tanks"></telerik:RadLabel>
                            </td>

                            <td>
                                <eluc:Latitude ID="txtFlushedLat" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td>
                                <eluc:Longitude ID="txtFlushedLong" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td>
                                <telerik:RadLabel runat="server" ID="RadLabel1" Text="( xx Deg xx Min N/S, xx Deg xx Min E/W)"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblQuantityCleaned" runat="server" Text="Quantity of Clean Ballast taken On board in meter cubic" Style="vertical-align: text-bottom"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtQuantityCleaned" CssClass="input_mandatory" RenderMode="Lightweight" MinValue="0" MaxValue="99999999" runat="server" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number" AutoPostBack="true" OnTextChanged="txtrefresh_TextChanged">
                                    <NumberFormat DecimalSeparator="." DecimalDigits="2" />
                                     <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>

                            </td>
                            <td>
                                <telerik:RadLabel ID="RadLabel2" runat="server" Text="m3"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblPosistionCleaned" runat="server" Text="Position of Ship when Ballast was taken in Dedicated Clean Ballast Tank"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Latitude ID="txtBallastLat" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td>
                                <eluc:Longitude ID="txtBallastLong" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td>
                                <telerik:RadLabel runat="server" ID="RadLabel3" Text="( xx Deg xx Min N/S, xx Deg xx Min E/W)"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblBallastTime" runat="server" Text="Time when valves separating the dedicated clean ballast tank from the cargo & stripping lines were closed"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTimePicker ID="txtballastTime" runat="server" CssClass="input_mandatory" AutoPostBack="true" OnSelectedDateChanged="txtrefresh_TextChanged">
                                    <TimeView Interval="00:30:00"></TimeView>
                                </telerik:RadTimePicker>
                            </td>
                            <td>
                                <telerik:RadLabel ID="Radlabel4" runat="server" Text="HH:MM"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblPosistionSeperatingBallst" runat="server" Text="Position when valves separating the dedicated clean ballast tank from the cargo & stripping lines were closed"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Latitude ID="txtSepratingLat" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td>
                                <eluc:Longitude ID="txtSepratingLong" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td>
                                <telerik:RadLabel runat="server" ID="RadLabel5" Text="( xx Deg xx Min N/S, xx Deg xx Min E/W)"></telerik:RadLabel>
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
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="txtItemNo4" runat="server"></telerik:RadLabel>
                                </td>
                                <td class="style_one">
                                    <telerik:RadLabel ID="lblrecord4" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="txtItemNo5" runat="server"></telerik:RadLabel>
                                </td>
                                <td class="style_one">
                                    <telerik:RadLabel ID="lblrecord5" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="txtItemNo6" runat="server"></telerik:RadLabel>
                                </td>
                                <td class="style_one">
                                    <telerik:RadLabel ID="lblrecord6" runat="server"></telerik:RadLabel>
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
                                        CommandName="INCHARGESIGN" ID="btnInchargeSign" OnClick="btnInchargeSign_Click"
                                        ToolTip="Chief Officer Sign" Width="20PX" Height="20PX">
                                         <span class="icon"> <i class="fas fa-file-signature"></i></span>
                                    </asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </div>

                    <%--<div class="notes">
                        <p>Notes:</p>
                        <telerik:RadLabel runat="server" ID="lblnotes"></telerik:RadLabel>
                    </div>--%>
                </div>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
