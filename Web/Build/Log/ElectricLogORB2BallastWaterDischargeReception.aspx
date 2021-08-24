<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogORB2BallastWaterDischargeReception.aspx.cs" Inherits="Log_ElectricLogORB2BallastWaterDischargeReception" %>

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
    <title>Ballast Water Discharge to Reception Facility</title>
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

                    <h3>Ballast Water Discharge to Reception Facility</h3>

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
                                <telerik:RadLabel ID="lblCargoTankCleaned" runat="server" Text="Port Name where Ballast water was discharged"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtPortName" Width="" CssClass="input_mandatory"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblResignation" runat="server" Text="Name & Designation of Reception facility"></telerik:RadLabel>
                            </td>
                            <td class="custom-width">
                                <telerik:RadTextBox runat="server" ID="txtFacility" Width="" CssClass="input_mandatory"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblTotalQuantity" runat="server" Text="Total quantity of Ballast Water discharged in Cubic meters"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtTotalQuantity" CssClass="input_mandatory" RenderMode="Lightweight" MinValue="0" MaxValue="99999999" runat="server" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number" AutoPostBack="true" OnTextChanged="txtAfrTrnsROBFrom_TextChanged">
                                    <NumberFormat DecimalSeparator="." DecimalDigits="2" />
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>

                                <telerik:RadLabel ID="lblfromUnit" runat="server" Text="m3"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblDateSignature" runat="server" Text="Date, Signature & Stamp of Port authority Official" Style="vertical-align: text-bottom"></telerik:RadLabel>
                            </td>
                            <td>

                                <asp:LinkButton runat="server" AlternateText="Attachment"
                                    CommandName="ATTACHMENT" ID="cmdAttachment"
                                    OnClick="cmdAttachment_Click" ToolTip="Attachment" Width="20PX" Height="20PX">
                                        <span class="icon"><i runat="server" id="attachmentIcon" class="fas fa-paperclip-na"></i></span>
                                </asp:LinkButton>
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
                                    <telerik:RadDropDownList runat="server" ID="ddlAttachment" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="ddlAttachment_SelectedIndexChanged" Width="200px"></telerik:RadDropDownList>
                                    <asp:HyperLink ID="lnkfilename" Target="_blank" Text="View" runat="server" Width="14px" Visible="false"
                                        Height="14px" ToolTip="Download File">
                                    </asp:HyperLink>
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
                </div>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>