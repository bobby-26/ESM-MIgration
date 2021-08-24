<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogORB2DischargeBallastDedicatedClean.aspx.cs" Inherits="Log_ElectricLogORB2DischargeBallastDedicatedClean" %>

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
    <title>Discharge of Ballast from Dedicated Clean Ballast Tank (CBT Tankers Only)</title>
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
            margin-top:2%;
            margin-bottom:2%;
        }

        .notes p {
            font-size: 13px;
            font-weight:bold;
            margin-top:0px;
            margin-bottom:0px;
        }

        .units {
            width: 200px;
        }

        .col-width {
            width: 200px;
            display:inline-block;
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
                <div style="float: initial; margin: 0 50px">

                    <h3>Discharge of Ballast from Dedicated Clean Ballast Tank (CBT Tankers Only)</h3>

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
                                <telerik:RadLabel ID="lblIdentityTank" runat="server" Text="Identity of Tank (Discharged)"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox DropDownPosition="Static" CssClass="input_mandatory" ID="ddlTank" runat="server" AutoPostBack="true" DataTextField="FLDNAME" OnSelectedIndexChanged="txtAfrTrnsROBFrom_TextChanged" DataValueField="FLDLOCATIONID" EnableLoadOnDemand="True"
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblDischargeStartTime" runat="server" Text="Discharge Start Time"></telerik:RadLabel>
                            </td>
                            <td class="custom-width">
                                <telerik:RadTimePicker ID="txtDischargeStartTime" runat="server" CssClass="input_mandatory" AutoPostBack="true" OnSelectedDateChanged="txtAfrTrnsROBFrom_TextChanged">
                                    <TimeView Interval="00:30:00"></TimeView>
                                </telerik:RadTimePicker>
                            </td>
                            <td>
                                <telerik:RadLabel ID="Radlabel3" runat="server" Text="HH:MM"></telerik:RadLabel>
                            </td>
                            

                            <td>
                                <telerik:RadLabel ID="lblStartPosistion" runat="server" Text="Start Position"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Latitude ID="txtStartLat" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td>
                                <eluc:Longitude ID="txtStartLong" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td>
                                <telerik:RadLabel runat="server" ID="RadLabel2" Text="( xx Deg xx Min N/S, xx Deg xx Min E/W)" CssClass="units"></telerik:RadLabel>
                            </td>

                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblDischargeStopTime" runat="server" Text="Discharge Completion Time"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTimePicker ID="txtDischargeStopTime" runat="server" AutoPostBack="true" CssClass="input_mandatory" OnSelectedDateChanged="txtAfrTrnsROBFrom_TextChanged">
                                    <TimeView Interval="00:30:00"></TimeView>
                                </telerik:RadTimePicker>
                            </td>
                            <td>
                                <telerik:RadLabel ID="Radlabel8" runat="server" Text="HH:MM"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblCompletePosition" runat="server" Text="Completion Position"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Latitude ID="txtCompleteLat" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td>
                                <eluc:Longitude ID="txtCompleteLong" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td>
                                <telerik:RadLabel runat="server" ID="RadLabel10" Text="( xx Deg xx Min N/S, xx Deg xx Min E/W)"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblUTCStart" runat="server" Text="UTC Start Time"></telerik:RadLabel>
                            </td>
                            <td class="custom-width">
                                <telerik:RadTimePicker ID="txtUTCStartTime" runat="server" CssClass="input_mandatory" AutoPostBack="true" OnSelectedDateChanged="txtAfrTrnsROBFrom_TextChanged">
                                    <TimeView Interval="00:30:00"></TimeView>
                                </telerik:RadTimePicker>
                            </td>
                            <td>
                                <telerik:RadLabel ID="Radlabel9" runat="server" Text="HH:MM"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblPosistionSperating" runat="server" Text="Position when valves separating the dedicated clean ballast tank from the cargo & stripping lines were closed" Width="300px"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Latitude ID="txtSepratingLat" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td>
                                <eluc:Longitude ID="txtSepratingLong" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td>
                                <telerik:RadLabel runat="server" ID="RadLabel7" Text="( xx Deg xx Min N/S, xx Deg xx Min E/W)"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblUTCStop" runat="server" Text="UTC Stop Time"></telerik:RadLabel>
                            </td>
                            <td class="custom-width">
                                <telerik:RadTimePicker ID="txtUTCStopTime" runat="server" CssClass="input_mandatory" AutoPostBack="true" OnSelectedDateChanged="txtAfrTrnsROBFrom_TextChanged">
                                    <TimeView Interval="00:30:00"></TimeView>
                                </telerik:RadTimePicker>
                            </td>
                            <td>
                                <telerik:RadLabel ID="Radlabel5" runat="server" Text="HH:MM"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblTimeWhenSeperating" runat="server" Text="Time when valves separating the dedicated clean ballast tank from the cargo & stripping lines were closed"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTimePicker ID="txtTimeWhenSeperating" runat="server" CssClass="input_mandatory" AutoPostBack="true" OnSelectedDateChanged="txtAfrTrnsROBFrom_TextChanged">
                                    <TimeView Interval="00:30:00"></TimeView>
                                </telerik:RadTimePicker>

                            </td>
                            <td>
                                <telerik:RadLabel ID="Radlabel6" runat="server" Text="HH:MM"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <telerik:RadComboBox DropDownPosition="Static" CssClass="input_mandatory" ID="ddlContext" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlContext_SelectedIndexChanged" EnableLoadOnDemand="True"
                                    Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="col-width">
                                <telerik:RadLabel ID="lblQuantityDischarged" runat="server" Text="Quantity discharged in m3 to sea (66.1)" Visible="false"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtQuantityDischarged" CssClass="input_mandatory" MinValue="0" MaxValue="99999999" RenderMode="Lightweight" runat="server" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number" AutoPostBack="true" OnTextChanged="txtAfrTrnsROBFrom_TextChanged" Visible="false">
                                    <NumberFormat DecimalSeparator="." DecimalDigits="2" />
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblQuantityDischargedUnit" runat="server" Text="m3" Visible="false"></telerik:RadLabel>
                            </td>

                            
                        </tr>
                        <tr>
                            <td class="col-width">
                                <telerik:RadLabel ID="lblQunatityDischargedFacility" runat="server" Text="Quantity discharged in m3 to Reception Facility (66.2)" Visible="false"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtQunatityDischargedFacility" CssClass="input_mandatory" MinValue="0" MaxValue="99999999" RenderMode="Lightweight" runat="server" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number" AutoPostBack="true" OnTextChanged="txtAfrTrnsROBFrom_TextChanged" Visible="false">
                                    <NumberFormat DecimalSeparator="." DecimalDigits="2" />
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                            </td>
                            <td></td>
                            <td>
                                <telerik:RadLabel ID="lblFacility" runat="server" Text="Port of Discharge" Visible="false"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtfacility" runat="server" CssClass="input_mandatory" RenderMode="Lightweight" AutoPostBack="true" OnTextChanged="txtAfrTrnsROBFrom_TextChanged" Visible="false">
                                </telerik:RadTextBox>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblIndication" runat="server" Text="Was there any indication of oil contamination of ballast water during into Sea" CssClass="col-width"></telerik:RadLabel>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="txtIndication" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" Width="100px" AutoPostBack="true" OnSelectedIndexChanged="txtIndication_SelectedIndexChanged" >
                                    <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="No"></asp:ListItem>     
                                </asp:RadioButtonList>
                                <%--<telerik:RadTextBox runat="server" ID="txtIndication" CssClass="input_mandatory" AutoPostBack="true" OnTextChanged="txtAfrTrnsROBFrom_TextChanged" ></telerik:RadTextBox>--%>
                            </td>
                            <td>
                                <%--<telerik:RadLabel runat="server" ID="lblIndicationUnit" Text="Yes / No"></telerik:RadLabel>--%>
                            </td>
                            
                            <td>
                                <telerik:RadLabel ID="lblMontitored" runat="server" Text="Was the discharge monitored by Oil Content Meter"></telerik:RadLabel>
                            </td>
                            <td>
                                <%--<telerik:RadTextBox runat="server" ID="txtMonitored" CssClass="input_mandatory" AutoPostBack="true" OnTextChanged="txtAfrTrnsROBFrom_TextChanged"></telerik:RadTextBox>--%>
                                <asp:RadioButtonList ID="txtMonitored" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" Width="100px" AutoPostBack="true" OnSelectedIndexChanged="txtIndication_SelectedIndexChanged" >
                                    <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="No"></asp:ListItem>     
                                </asp:RadioButtonList>
                            </td>
                            <td>
                               <%-- <telerik:RadLabel runat="server" ID="RadLabel1" Text="Yes / No"></telerik:RadLabel>--%>
                            </td>
                        </tr>
                        <tr>
                            
                        </tr>
                        <tr>
                            
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
                                    <telerik:RadLabel ID="lblRecord1" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="txtItemNo2" runat="server"></telerik:RadLabel>
                                </td>
                                <td class="style_one">
                                    <telerik:RadLabel ID="lblRecord2" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="txtItemNo3" runat="server"></telerik:RadLabel>
                                </td>
                                <td class="style_one">
                                    <telerik:RadLabel ID="lblRecord3" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="txtItemNo4" runat="server"></telerik:RadLabel>
                                </td>
                                <td class="style_one">
                                    <telerik:RadLabel ID="lblRecord4" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="txtItemNo5" runat="server"></telerik:RadLabel>
                                </td>
                                <td class="style_one">
                                    <telerik:RadLabel ID="lblRecord5" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="txtItemNo6" runat="server"></telerik:RadLabel>
                                </td>
                                <td class="style_one">
                                    <telerik:RadLabel ID="lblRecord6" runat="server"></telerik:RadLabel>
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
                </div>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>