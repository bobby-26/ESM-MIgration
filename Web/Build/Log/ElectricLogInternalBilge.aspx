<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogInternalBilge.aspx.cs" Inherits="Log_ElectricLogInternalBilge" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Internal Bilge</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
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
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager runat="server" RenderMode="Lightweight" ID="RadWindowManager1"></telerik:RadWindowManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel">

            <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
            <eluc:Message ID="ucMessage" runat="server" Text="" Visible="false"></eluc:Message>
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" Style="display: none;" />
            <eluc:TabStrip ID="gvTabStrip" runat="server" OnTabStripCommand="gvTabStrip_TabStripCommand"></eluc:TabStrip>
            <div style="float: initial; margin: 0 50px">
                <table>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblOperationDate" runat="server" Text="Date of Operation"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadDatePicker RenderMode="Lightweight" runat="server" CssClass="input_mandatory" ID="txtOperationDate" AutoPostBack="true" MaxLength="12" OnSelectedDateChanged="txtOperationDate_SelectedDateChanged">
                                <DateInput DateFormat="dd-MMM-yyyy" runat="server"></DateInput>
                            </telerik:RadDatePicker>

                            <telerik:RadLabel runat="server" ID="lblTranscationDetailID" Visible="false"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblFrom" runat="server" Text="Bilge Transferred FROM"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlTransferFrom" dropdownposition="Static" CssClass="input_mandatory" runat="server" AutoPostBack="true" DataTextField="FLDNAME" OnSelectedIndexChanged="ddlTransferFrom_SelectedIndexChanged" DataValueField="FLDLOCATIONID" EnableLoadOnDemand="True"
                                EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                            </telerik:RadComboBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblTo" runat="server" Visible="true" Text="Bilge Transfer To"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox DropDownPosition="Static" CssClass="input_mandatory" AutoPostBack="true" Visible="true" ID="ddlTransferTo" runat="server" DataTextField="FLDNAME" DataValueField="FLDLOCATIONID" EnableLoadOnDemand="True"
                                EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true" OnSelectedIndexChanged="ddlTransferTo_SelectedIndexChanged">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblBfrTrnsROBFrom" runat="server" Text="Before Transfer ROB"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadNumericTextBox ID="txtBfrTrnsROBFrom" CssClass="input_mandatory" AutoPostBack="true" MinValue="0" MaxValue="99999999" OnTextChanged="bfr_selectedchanged" RenderMode="Lightweight" runat="server" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number">
                                <NumberFormat DecimalSeparator="." DecimalDigits="2" />
                                <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                            </telerik:RadNumericTextBox>
                            <telerik:RadLabel ID="lblfromUnit" runat="server" Text="m3"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadLabel ID="RadLabel3" runat="server" Visible="true" Text="Before Transfer ROB"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadNumericTextBox ID="txtBfrTrnsROBTo" CssClass="input_mandatory" AutoPostBack="true" MinValue="0" MaxValue="99999999" OnTextChanged="To_selectedchanged" RenderMode="Lightweight" Visible="true" runat="server" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number">
                                <NumberFormat DecimalSeparator="." DecimalDigits="2" />
                                <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                            </telerik:RadNumericTextBox>
                            <telerik:RadLabel ID="RadLabel4" runat="server" Visible="true" Text="m3"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblBfrTrnsROBTo" runat="server" Text="After Transfer ROB"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadNumericTextBox ID="txtAftrTrnsROBFrom" CssClass="input_mandatory" MinValue="0" MaxValue="99999999" AutoPostBack="true" OnTextChanged="txtAftrTrnsROBFrom_TextChanged" RenderMode="Lightweight" runat="server" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number">
                                <NumberFormat DecimalSeparator="." DecimalDigits="2" />
                                <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                            </telerik:RadNumericTextBox>
                            <telerik:RadLabel ID="lbltounit" runat="server" Text="m3"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblAfrTrnsROBTo" runat="server" Visible="true" Text="After Transfer ROB"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadNumericTextBox ID="txtAfrTrnsROBTo" Visible="true" AutoPostBack="true" MinValue="0" MaxValue="99999999" OnTextChanged="txtAftrTrnsROBFrom_TextChanged" CssClass="input_mandatory" RenderMode="Lightweight" runat="server" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number">
                                <NumberFormat DecimalSeparator="." DecimalDigits="2" />
                                <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                            </telerik:RadNumericTextBox>
                            <telerik:RadLabel ID="lblaftrtoUnit" runat="server" Visible="true" Text="m3"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblStartTime" runat="server" Text="Start Time"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTimePicker ID="txtStartTime" runat="server" CssClass="input_mandatory" AutoPostBack="true" OnSelectedDateChanged="txtAftrTrnsROBFrom_TextChanged">
                                <TimeView Interval="00:30:00" runat="server"></TimeView>
                            </telerik:RadTimePicker>
                            <telerik:RadLabel ID="Radlabel1" runat="server" Text="HH:MM"></telerik:RadLabel>

                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblStopTime" runat="server" Text="End Time"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTimePicker ID="txtStopTime" runat="server" CssClass="input_mandatory" AutoPostBack="true" OnSelectedDateChanged="txtAftrTrnsROBFrom_TextChanged">
                                <TimeView Interval="00:30:00" runat="server"></TimeView>
                            </telerik:RadTimePicker>
                            <telerik:RadLabel ID="Radlabel2" runat="server" Text="HH:MM"></telerik:RadLabel>
                        </td>
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
                            <telerik:RadLabel ID="txtItemNo" runat="server"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblRecord" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td class="style_one" style="text-align: center">
                            <telerik:RadLabel ID="txtcode2" runat="server"></telerik:RadLabel>
                        </td>
                        <td class="style_one">
                            <telerik:RadLabel ID="lblTimeRecord" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td class="style_one" style="text-align: center">
                            <telerik:RadLabel ID="txtcode3" runat="server"></telerik:RadLabel>
                        </td>
                        <td class="style_one">
                            <telerik:RadLabel ID="lbltorecord" runat="server"></telerik:RadLabel>
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
                </table>
            </div>
           <asp:Button ID="confirm" runat="server" Text="confirm" OnClick="confirm_Click" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
