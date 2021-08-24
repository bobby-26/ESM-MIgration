<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogBunkeringLube.aspx.cs" Inherits="Log_ElectricLogBunkeringLube" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Bunkering Lube</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
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

        .bold {
            font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager runat="server" RenderMode="Lightweight" ID="RadWindowManager1"></telerik:RadWindowManager>
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
                                <telerik:RadLabel ID="lblOperationDate" runat="server" Text="Date of Operation"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadDatePicker RenderMode="Lightweight" runat="server" CssClass="input_mandatory" ID="txtOperationDate" MaxLength="12" OnSelectedDateChanged="txt_TextChanged" >
                                    <DateInput DateFormat="dd-MMM-yyyy" runat="server"></DateInput>
                                </telerik:RadDatePicker>

                                <telerik:RadLabel runat="server" ID="lblBunkeringLubeId" Visible="false"></telerik:RadLabel>
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblPort" Text="Port of Bunker" runat="server"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtPort" runat="server" CssClass="input_mandatory"  OnTextChanged="txt_TextChanged">
                                </telerik:RadTextBox>
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                               <tr>
                            <td>
                                <telerik:RadLabel ID="lblStartDate" Text="Start Date" runat="server"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadDatePicker ID="txtStartDate" RenderMode="Lightweight" runat="server" CssClass="input_mandatory"  MaxLength="12"  OnSelectedDateChanged="txt_TextChanged">
                                    <DateInput DateFormat="dd-MMM-yyyy" runat="server"></DateInput>
                                </telerik:RadDatePicker>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblStartTime" Text="Time" runat="server">
                                </telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTimePicker ID="txtStartTime" CssClass="input_mandatory" runat="server"  OnSelectedDateChanged="txt_TextChanged">
                                </telerik:RadTimePicker>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblStopDate" Text="Stop Date" runat="server">
                                </telerik:RadLabel>
                            </td>
                            <td>
                                <%--<eluc:Date ID="txtStopDate" runat="server" CssClass="input_mandatory"  OnTextChanged="txt_TextChanged"/>--%>
                                <telerik:RadDatePicker ID="txtStopDate" RenderMode="Lightweight" runat="server" CssClass="input_mandatory"  MaxLength="12"  OnSelectedDateChanged="txt_TextChanged">
                                    <DateInput DateFormat="dd-MMM-yyyy" runat="server"></DateInput>
                                </telerik:RadDatePicker>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblStopTime" Text="Time" runat="server"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTimePicker ID="txtStopTime" runat="server" CssClass="input_mandatory"  OnSelectedDateChanged="txt_TextChanged"></telerik:RadTimePicker>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblBunkerQty" Text="Total Bunker Quantity" runat="server"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtBunkerQty" RenderMode="Lightweight" runat="server" MinValue="0" MaxValue="99999999" OnTextChanged="txt_TextChanged" CssClass="input_mandatory"  MaxLength="22" Width="120px" Style="text-align: right;" Type="Number">
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblunit" Text="MT" CssClass="bold" runat="server"></telerik:RadLabel>

                            </td>
                            <td>
                                <telerik:RadLabel ID="lblBunkerType" Text="Type of Bunker" runat="server"></telerik:RadLabel>

                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtBunkerType" runat="server" CssClass="input_mandatory"  OnTextChanged="txt_TextChanged">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table>
                        <tr>
                            <td>Tank</td>
                            <td>Before Bunker ROB</td>
                            <td>Bunker Quantity</td>
                            <td>Final ROB</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadComboBox dropdownposition="Static" Style="width: 100%"  ID="ddlTransferFrom" runat="server" DataTextField="FLDNAME" OnSelectedIndexChanged="ddlTransferFrom_SelectedIndexChanged" DataValueField="FLDLOCATIONID" EnableLoadOnDemand="True"
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                 <telerik:RadNumericTextBox ID="txtBfrTrnsROBFrom" RenderMode="Lightweight" MinValue="0" MaxValue="99999999" CssClass="input_mandatory" runat="server" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number" >
                                     <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                            </td>
                            <td>
                            <%--    <telerik:RadTextBox ID="txtbfrBunkerQty" runat="server" CssClass="input_mandatory"  OnTextChanged="txtBunkerQty_TextChanged">
                                </telerik:RadTextBox>--%>
                                 <telerik:RadNumericTextBox ID="txtbfrBunkerQty" RenderMode="Lightweight" MinValue="0" MaxValue="99999999" CssClass="input_mandatory" runat="server" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number" OnTextChanged="txtBunkerQty_TextChanged"  AutoPostBack="true">
                                     <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtTotalQty" runat="server" Enabled="false">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
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
                            <td class="style_one"></td>
                            <td>
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
                    </table>
                </div>
            </div>
                <asp:Button ID="confirm" runat="server" Text="confirm" OnClick="confirm_Click" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
