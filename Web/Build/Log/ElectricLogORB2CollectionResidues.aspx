<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogORB2CollectionResidues.aspx.cs" Inherits="Log_ElectricLogORB2CollectionResidues" %>


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
    <title>Collection, Transfer and Disposal of Residues and Oily Mixture not otherwise dealt with</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" src="<%=Session["sitepath"]%>/js/jquery-1.12.4.min.js"></script>
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
                    <h3>Collection, Transfer and Disposal of Residues and Oily Mixture not otherwise dealt with</h3>
                    <table runat="server" id="usercontrol">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="RadLabel1" runat="server" Text="Date of Operation"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadDatePicker RenderMode="Lightweight" runat="server" CssClass="input_mandatory" ID="txtOperationDate" MaxLength="12" OnSelectedDateChanged="txtOperationDate_SelectedDateChanged">
                                    <DateInput DateFormat="dd-MMM-yyyy" runat="server"></DateInput>
                                </telerik:RadDatePicker>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblFrom" runat="server" Text="Identity of Tank"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox DropDownPosition="Static" CssClass="input_mandatory" ID="ddlTransferFrom" runat="server" DataTextField="FLDNAME" OnSelectedIndexChanged="ddlWaterTransferedFrom_SelectedIndexChanged" AutoPostBack="true" DataValueField="FLDLOCATIONID" EnableLoadOnDemand="True"
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </td>

                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblQuantityTransfered" runat="server" Text="Quantity Transferred or Disposed from each Tank" Style="vertical-align: text-bottom"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtQuantityTransfered" AutoPostBack="true" CssClass="input_mandatory" MinValue="0" MaxValue="99999999" RenderMode="Lightweight" OnTextChanged="txtAfrTrnsROBFrom_TextChanged" runat="server" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number">
                                    <NumberFormat DecimalSeparator="." DecimalDigits="2" />
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                                <telerik:RadLabel ID="RadLabel18" runat="server" Text="m3"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblQantityRetained" runat="server" Text="Quantity retained in tank after disposal" Style="vertical-align: text-bottom"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtQuantityReatinedDisposal" AutoPostBack="true" CssClass="input_mandatory" MinValue="0" MaxValue="99999999" RenderMode="Lightweight" OnTextChanged="txtAfrTrnsROBFrom_TextChanged" runat="server" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number">
                                    <NumberFormat DecimalSeparator="." DecimalDigits="2" />
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                                <telerik:RadLabel ID="RadLabel3" runat="server" Text="m3"></telerik:RadLabel>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblMethodOfTransfer" runat="server" Text="Method of Transfer"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlMethodOfTransfer" runat="server"  DropDownPosition="Static" CssClass="input_mandatory" EnableLoadOnDemand="True" AutoPostBack="true" OnSelectedIndexChanged="ddlMethodOfTransfer_SelectedIndexChanged"
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblShoreDisposal" runat="server"  Visible="false" Text="57.1 (Shore Disposal)"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblBarageName" runat="server" Visible="false"  Text="Barge Name"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtBarageName" runat="server" Visible="false" CssClass="input_mandatory" RenderMode="Lightweight" AutoPostBack="true" OnTextChanged="txtAfrTrnsROBFrom_TextChanged" MaxLength="10">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr runat="server" id="row57One" visible="false">
                            <td></td>
                            <td></td>
                            <td></td>
                            <td>
                                <telerik:RadLabel ID="lblPort" runat="server"  Text="Port or Anchorage Name"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtPort" runat="server"  CssClass="input_mandatory" RenderMode="Lightweight" AutoPostBack="true" OnTextChanged="txtAfrTrnsROBFrom_TextChanged" MaxLength="10">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr runat="server" id="row57Two" visible="false">
                            <td></td>
                            <td></td>
                            <td></td>
                            <td>
                                <telerik:RadLabel ID="lblQuantityDischarge" runat="server"  Text="Quantity Discharged"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtQuantityDischarge" runat="server"  AutoPostBack="true" Enabled="false" CssClass="input_mandatory" RenderMode="Lightweight" OnTextChanged="txtAfrTrnsROBFrom_TextChanged" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number">
                                    <NumberFormat DecimalSeparator="." DecimalDigits="2" />
                                </telerik:RadNumericTextBox>
                                <telerik:RadLabel ID="RadLabel38" runat="server" Text="m3"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr runat="server" id="row572One" visible="false">
                            <td></td>
                            <td></td>
                            <td>
                                <telerik:RadLabel ID="lblMixedCargo" runat="server"  Text="57.2 (Mixed with Cargo)"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblQuantityMixed" runat="server"  Text="Quantity mixed with cargo"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtQuantityMixed" Enabled="false"></telerik:RadTextBox>
                                <telerik:RadLabel ID="RadLabel14" runat="server" Text="m3"></telerik:RadLabel>
                                <%--<telerik:RadComboBox runat="server"  DropDownPosition="Static" CssClass="input_mandatory"  ID="ddlQuantityMixed" DataTextField="FLDNAME" OnSelectedIndexChanged="txtAfrTrnsROBFrom_TextChanged" DataValueField="FLDLOCATIONID" EnableLoadOnDemand="True"
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>--%>
                            </td>
                        </tr>
                        <tr runat="server" id="row572Two" visible="false">
                            <td></td>
                            <td></td>
                            <td></td>
                            <td>
                                <telerik:RadLabel ID="lblMixedcargoTank" runat="server"  Text="Mixed with cargo in Tank"></telerik:RadLabel>
                            </td>
                            <td>
                                <%--<telerik:RadNumericTextBox ID="ddlMixedCargoTank" runat="server"  AutoPostBack="true" CssClass="input_mandatory" RenderMode="Lightweight" OnTextChanged="txtAfrTrnsROBFrom_TextChanged"  MaxLength="22" Width="120px" Style="text-align: right;" Type="Number">
                                    <NumberFormat DecimalSeparator="." DecimalDigits="2" />
                                </telerik:RadNumericTextBox>--%>
                                <telerik:RadComboBox ID="ddlMixedCargoTank" runat="server"  DropDownPosition="Static" CssClass="input_mandatory" DataTextField="FLDNAME" OnSelectedIndexChanged="ddlWaterTransferedFrom_SelectedIndexChanged" DataValueField="FLDLOCATIONID" EnableLoadOnDemand="True"
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                                
                            </td>
                        </tr>
                        <tr runat="server" id="row573One" visible="false">
                            <td></td>
                            <td></td>
                            <td>
                                <telerik:RadLabel ID="lblInternalTransfer" runat="server"  Text="57.3 (Internal Transfer)"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblTransferred" runat="server"  Text="Transferred to tank"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlTransferredTo" runat="server"  DropDownPosition="Static" CssClass="input_mandatory" DataTextField="FLDNAME" OnSelectedIndexChanged="ddlWaterTransferedFrom_SelectedIndexChanged" DataValueField="FLDLOCATIONID" EnableLoadOnDemand="True"
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr runat="server" id="row573Two" visible="false">
                            <td></td>
                            <td></td>
                            <td></td>
                            <td>
                                <telerik:RadLabel ID="lblTransferredQuantity" runat="server"  Text="Transferred Quantity"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtTransferQty" runat="server" MinValue="0" MaxValue="99999999" AutoPostBack="true" Enabled="false" CssClass="input_mandatory" RenderMode="Lightweight" OnTextChanged="txtAfrTrnsROBFrom_TextChanged" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number">
                                    <NumberFormat DecimalSeparator="." DecimalDigits="2" />
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                                <telerik:RadLabel ID="RadLabel16" runat="server" Text="m3"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr runat="server" id="row573Three" visible="false">
                            <td></td>
                            <td></td>
                            <td></td>
                            <td>
                                <telerik:RadLabel ID="lblQuantityRetained" runat="server"  Text="Quantity Retained in above tank"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtQuantityRetained" runat="server" MinValue="0" MaxValue="99999999" AutoPostBack="true" CssClass="input_mandatory" RenderMode="Lightweight" OnTextChanged="txtAfrTrnsROBFrom_TextChanged" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number">
                                    <NumberFormat DecimalSeparator="." DecimalDigits="2" />
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                                <telerik:RadLabel ID="RadLabel20" runat="server" Text="m3"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr runat="server" id="row574One" visible="false">
                            <td></td>
                            <td></td>
                            <td>
                                <telerik:RadLabel ID="lblOtherMethod" runat="server"  Text="57.4 (Other method of Disposal)"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblStateMethod" runat="server"  Text="State Method of Disposal"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtStateMethod" AutoPostBack="true" OnTextChanged="txtAfrTrnsROBFrom_TextChanged"></telerik:RadTextBox>

                            </td>
                        </tr>
                        <tr runat="server" id="row574Two" visible="false">
                            <td></td>
                            <td></td>
                            <td></td>
                            <%--<td></td>--%>
                            <td>
                                <telerik:RadLabel ID="lblQuantityDisposed" runat="server"  Text="Quantity Disposed"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtQuantityDisposed" runat="server" MinValue="0" MaxValue="99999999" AutoPostBack="true" Enabled="false" CssClass="input_mandatory" RenderMode="Lightweight" OnTextChanged="txtAfrTrnsROBFrom_TextChanged"  MaxLength="22" Width="120px" Style="text-align: right;" Type="Number">
                                    <NumberFormat DecimalSeparator="." DecimalDigits="2" />
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                                <telerik:RadLabel ID="RadLabel24" runat="server" Text="m3"></telerik:RadLabel>
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

                            <tr runat="server" id="inchargeRow">
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
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
