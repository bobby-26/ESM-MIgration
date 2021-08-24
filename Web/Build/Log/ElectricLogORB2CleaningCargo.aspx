<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogORB2CleaningCargo.aspx.cs" Inherits="Log_ElectricLogORB2CleaningCargo" %>


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
    <title>Cleaning of Cargo Tanks</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function confirm(args) {
                if (args) {
                    __doPostBack("<%=confirm.UniqueID %>", "");
                }
            }
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

        .custom-width {
            width: 200px;
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
        <telerik:RadWindowManager runat="server" RenderMode="Lightweight" ID="RadWindowManager1"></telerik:RadWindowManager>


        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">

            <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
            <eluc:Message ID="ucMessage" runat="server" Text="" Visible="false"></eluc:Message>
            <eluc:TabStrip ID="gvTabStrip" runat="server" OnTabStripCommand="gvTabStrip_TabStripCommand"></eluc:TabStrip>
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" Style="display: none;" />

            <div style="width: 100%; overflow: auto;">
                <div style="float: initial; margin: 0 50px">

                    <h3>Cleaning of Cargo Tanks</h3>

                    <table>
                        <tr>
                            <td>
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
                                <telerik:RadLabel ID="lblCargoTankCleaned" runat="server" Text="Cargo Tank Cleaned"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox DropDownPosition="Static" CssClass="input_mandatory" ID="ddlCargoTankCleaned" runat="server" AutoPostBack="true" DataTextField="FLDNAME" OnSelectedIndexChanged="txtAfrTrnsROBFrom_TextChanged" DataValueField="FLDLOCATIONID" EnableLoadOnDemand="True"
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblCOWStartTime" runat="server" Text="Start Time"></telerik:RadLabel>
                            </td>
                            <td class="custom-width">
                                <telerik:RadTimePicker ID="txtCOWStartTime" runat="server" CssClass="input_mandatory" AutoPostBack="true" OnSelectedDateChanged="txtAfrTrnsROBFrom_TextChanged">
                                    <TimeView Interval="00:30:00"></TimeView>
                                </telerik:RadTimePicker>
                                <telerik:RadLabel ID="Radlabel2" runat="server" Text="HH:MM"></telerik:RadLabel>
                            </td>
                            <td>
                               <span class="right"> <telerik:RadLabel ID="lblStartPosistion" runat="server" Visible="true" Text="Start Position"></telerik:RadLabel></span>
                            </td>
                            <td>
                                <eluc:Latitude ID="txtStartPosistionLat" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td>
                                <eluc:Longitude ID="txtStartPosistionLog" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td>
                                <telerik:RadLabel runat="server" ID="lblStartPos" Text="( xx Deg xx Min N/S, xx Deg xx Min E/W)"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblCOWStopTime" runat="server" Text="Stop Time"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTimePicker ID="txtCOWStopTime" runat="server" AutoPostBack="true" CssClass="input_mandatory" OnSelectedDateChanged="txtAfrTrnsROBFrom_TextChanged">
                                    <TimeView Interval="00:30:00"></TimeView>
                                </telerik:RadTimePicker>
                                <telerik:RadLabel ID="Radlabel3" runat="server" Text="HH:MM"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblStopPosition" runat="server" Visible="true" Text="Stop Position"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Latitude ID="txtStopPosistionLat" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td>
                                <eluc:Longitude ID="txtStopPosistionLog" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td>
                                <telerik:RadLabel runat="server" ID="lblStopUnit" Text="( xx Deg xx Min N/S, xx Deg xx Min E/W)"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblTotalQuantity" runat="server" Text="Total Quantity of Wash Water Transferred" Style="vertical-align: text-bottom"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtWashWaterTransfered" CssClass="input_mandatory" MinValue="0" MaxValue="99999999" RenderMode="Lightweight" runat="server" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number" AutoPostBack="true" OnTextChanged="txtAfrTrnsROBFrom_TextChanged">
                                    <NumberFormat DecimalSeparator="." DecimalDigits="2" />
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>

                                <telerik:RadLabel ID="lblfromUnit" runat="server" Text="m3"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblFacility" runat="server" Text="Name of Reception Facility/Port" Visible="false"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtfacility" runat="server" CssClass="input_mandatory" RenderMode="Lightweight" AutoPostBack="true" OnTextChanged="txtAfrTrnsROBFrom_TextChanged" MaxLength="10" Visible="false">
                                </telerik:RadTextBox>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblWaterTransfered" runat="server" Text="Wash water Transferred to"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox DropDownPosition="Static" CssClass="input_mandatory" ID="ddlWaterTransferedFrom" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlWaterTransferedFrom_SelectedIndexChanged" EnableLoadOnDemand="True"
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblWashWaterTransferred" runat="server" Text="Wash water transferred to" Visible="false"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox DropDownPosition="Static" CssClass="input_mandatory" ID="ddlWashWaterTransferedTo" runat="server" AutoPostBack="true" DataTextField="FLDNAME" OnSelectedIndexChanged="txtAfrTrnsROBFrom_TextChanged" DataValueField="FLDLOCATIONID" EnableLoadOnDemand="True" Visible="false"
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblMethodCleaning" runat="server" Text="Method of tank Cleaning"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtMethodCleaning" runat="server" CssClass="input_mandatory" RenderMode="Lightweight" AutoPostBack="true" OnTextChanged="txtAfrTrnsROBFrom_TextChanged">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblTotalROB" runat="server" Text="Total ROB in Above tank" Visible="false"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtTotalROB" runat="server" CssClass="input_mandatory" MinValue="0" MaxValue="99999999" RenderMode="Lightweight" Width="100px" AutoPostBack="true" OnTextChanged="txtAfrTrnsROBFrom_TextChanged" Visible="false">
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                                <telerik:RadLabel ID="lblrobunit" runat="server" Text="m3"></telerik:RadLabel>
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

                    <div class="notes">
                        <p>Notes:</p>
                        <telerik:RadLabel runat="server" ID="lblnotes"></telerik:RadLabel>
                    </div>
                </div>
            </div>
         <asp:Button ID="confirm" runat="server" Text="confirm" OnClick="confirm_Click" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
