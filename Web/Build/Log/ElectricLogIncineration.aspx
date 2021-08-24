<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogIncineration.aspx.cs" Inherits="Log_ElectricLogIncineration" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Incineration</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
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
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <telerik:RadAjaxPanel runat="server">
            <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
            <eluc:Message ID="ucMessage" runat="server" Text="" Visible="false"></eluc:Message>
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" Style="display: none;" />

            <eluc:TabStrip ID="gvTabStrip" runat="server" OnTabStripCommand="gvTabStrip_TabStripCommand"></eluc:TabStrip>
            <div style="width: 100%; overflow: auto;">
                <div style="float: initial; margin: 0 50px">
                    <table>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="RadLabel1" runat="server" Text="Date of Operation"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadDatePicker RenderMode="Lightweight" runat="server" CssClass="input_mandatory" ID="txtOperationDate" MaxLength="12" AutoPostBack="true" OnSelectedDateChanged="txtOperationDate_TextChangedEvent" >
                                    <DateInput DateFormat="dd-MMM-yyyy" runat="server"></DateInput>
                                </telerik:RadDatePicker>
                                <telerik:RadLabel ID="lblIncinerationDetailID" runat="server" Visible="false"></telerik:RadLabel>

                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblFrom" runat="server" Text="Incineration"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox DropDownPosition="Static" CssClass="input_mandatory"  ID="ddlTransferFrom" runat="server" DataTextField="FLDNAME" OnSelectedIndexChanged="ddlTransferFrom_SelectedIndexChanged" AutoPostBack="true" DataValueField="FLDLOCATIONID" EnableLoadOnDemand="True"
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblBfrTrnsROBFrom" runat="server" Text="Before Incineration ROB"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtBfrTrnsROBFrom" CssClass="input_mandatory" MinValue="0" MaxValue="99999999" RenderMode="Lightweight" runat="server" OnTextChanged="bfr_selectedindexchange" AutoPostBack="true" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number">
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                                <telerik:RadLabel ID="lblfromUnit" runat="server" Text="m3"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblAfrTrnsROBFrom" runat="server" Text="After Incineration ROB"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtAfrTrnsROBFrom" CssClass="input_mandatory" RenderMode="Lightweight" MinValue="0" MaxValue="99999999" OnTextChanged="txtAfrTrnsROBFrom_TextChangedEvent" AutoPostBack="true" runat="server" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number">
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                                <%--<eluc:Number ID="txtAfrTrnsROBFrom" runat="server" CssClass="input_mandatory txtNumber" MaxLength="3" Width="120px" OnTextChangedEvent="txtAfrTrnsROBFrom_TextChangedEvent"  />--%>
                                <telerik:RadLabel ID="lblaftrfromUnit" runat="server" Text="m3"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblTimeinHrs" runat="server" Text="Incineration Time in Hours"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtTimeInHrs" CssClass="input_mandatory" RenderMode="Lightweight" MinValue="0" MaxValue="99999999" NumberFormat-DecimalDigits="0" AllowRounding="true" OnTextChanged="txt_TextChangedEvent" AutoPostBack="true" runat="server" MaxLength="10" Width="120px" Style="text-align: right;" Type="Number">
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblLtrsHrs" runat="server" Text="Incineration Ltrs/Hour"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtLtrsHrs" CssClass="input_mandatory" RenderMode="Lightweight" MinValue="0" MaxValue="99999999" NumberFormat-DecimalDigits="0" OnTextChanged="txt_TextChangedEvent" AutoPostBack="true" runat="server" MaxLength="10" Width="120px" Style="text-align: right;" Type="Number">
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>                                
                            </td>
                        </tr>
                    </table>
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
                        <tr id="rowTwo" runat="server">
                            <td></td>
                            <td></td>
                            <td></td>
                            <td class="style_one">
                                <telerik:RadLabel ID="lbltorecord" runat="server"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr runat="server" id="inchargeRow">
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
                                        ID="btnInchargeSign"  OnClick="btnInchargeSign_Click"
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
