<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogCRBInternalCargoTransfer.aspx.cs" Inherits="Log_ElectricLogCRBInternalCargoTransfer" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Internal Transfer of Annex II Cargo</title>
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
                 <h3>Internal Transfer of Annex II Cargo</h3>
                    <table>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="RadLabel1" runat="server" Text="Date of Operation"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadDatePicker RenderMode="Lightweight" runat="server" CssClass="input_mandatory" ID="txtOperationDate" MaxLength="12" AutoPostBack="true" OnSelectedDateChanged="txtOperationDate_SelectedDateChanged" >
                                    <DateInput DateFormat="dd-MMM-yyyy" runat="server"></DateInput>
                                </telerik:RadDatePicker>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblcargoname" runat="server" Text="Name of Cargo"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox RenderMode="Lightweight" runat="server" CssClass="input_mandatory" ID="txtcagoname" AutoPostBack="true" OnTextChanged = "Refresh_TextChanged" >                                    
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblcategory" runat="server" Text="Category"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox RenderMode="Lightweight" runat="server" CssClass="input_mandatory" ID="txtcategory" AutoPostBack="true" OnTextChanged = "Refresh_TextChanged" >                                    
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblFrom" runat="server" Text="Cargo Internal Transfer from"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox DropDownPosition="Static" CssClass="input_mandatory"  ID="ddlTransferFrom" runat="server" DataTextField="FLDNAME" AutoPostBack="true" OnSelectedIndexChanged="ddlTransferFrom_SelectedIndexChanged" DataValueField="FLDLOCATIONID" EnableLoadOnDemand="True"
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblTo" runat="server" Visible="true" Text="Cargo Transfer to tank"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox DropDownPosition="Static" CssClass="input_mandatory"  Visible="true" ID="ddlTransferTo" runat="server" DataTextField="FLDNAME" DataValueField="FLDLOCATIONID" EnableLoadOnDemand="True"
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true" AutoPostBack="true" OnSelectedIndexChanged="ddlTransferFrom_SelectedIndexChanged">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblBfrTrnsROBFrom" runat="server" Text="Before Transfer ROB"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtBfrTrnsROBFrom" RenderMode="Lightweight" CssClass="input_mandatory" MinValue="0" MaxValue="99999999" runat="server" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number" AutoPostBack="true" OnTextChanged = "Refresh_TextChanged">
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                                <telerik:RadLabel ID="lblfromUnit" runat="server" Text="m3"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblBfrTrnsROBTo" runat="server" Visible="true" Text="Before Transfer ROB"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtBfrTrnsROBTo" RenderMode="Lightweight" CssClass="input_mandatory" runat="server" MaxLength="22" MinValue="0" MaxValue="99999999" Width="120px" Style="text-align: right;" Type="Number" AutoPostBack="true" OnTextChanged = "Refresh_TextChanged">
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                                <telerik:RadLabel ID="lbltounit" runat="server" Visible="true" Text="m3"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblAfrTrnsROBFrom" runat="server" Text="After Transfer ROB"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtAfrTrnsROBFrom" RenderMode="Lightweight" CssClass="input_mandatory" MinValue="0" MaxValue="99999999" runat="server" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number"  AutoPostBack="true" OnTextChanged = "Refresh_TextChanged">
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                                <telerik:RadLabel ID="lblaftrfromUnit" runat="server" Text="m3"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblAfrTrnsROBTo" runat="server"  Visible="true" Text="After Transfer ROB"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtAfrTrnsROBTo" RenderMode="Lightweight" CssClass="input_mandatory" MinValue="0" MaxValue="99999999" runat="server" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number" AutoPostBack="true" OnTextChanged = "Refresh_TextChanged">
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                                <telerik:RadLabel ID="lblaftrtoUnit" runat="server" Visible="true" Text="m3"></telerik:RadLabel>
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
                            <td class="style_one" style="text-align:center">
                                <telerik:radlabel id="txtItemNo1" runat="server"></telerik:radlabel>
                            </td>
                            <td class="style_one"> 
                                <telerik:radlabel id="lblrecord1" runat="server"></telerik:radlabel>
                            </td>
                        </tr>

                         <tr>
                            <td></td>
                            <td></td>
                            <td class="style_one" style="text-align:center">
                                <telerik:radlabel id="txtItemNo2" runat="server"></telerik:radlabel>
                            </td>
                            <td class="style_one"> 
                                <telerik:radlabel id="lblrecord2" runat="server"></telerik:radlabel>
                            </td>
                        </tr>

                         <tr>
                            <td></td>
                            <td></td>
                            <td class="style_one" style="text-align:center">
                                <telerik:radlabel id="txtItemNo3" runat="server"></telerik:radlabel>
                            </td>
                            <td class="style_one"> 
                                <telerik:radlabel id="lblrecord3" runat="server"></telerik:radlabel>
                            </td>
                        </tr>

                         <tr>
                            <td></td>
                            <td></td>
                            <td class="style_one" style="text-align:center">
                                <telerik:radlabel id="txtItemNo4" runat="server"></telerik:radlabel>
                            </td>
                            <td class="style_one"> 
                                <telerik:radlabel id="lblrecord4" runat="server"></telerik:radlabel>
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
                                        CommandName="INCHARGESIGN" ID="btnInchargeSign" OnClick="btnInchargeSign_Click"  
                                        ToolTip="Incharge Sign" Width="20PX" Height="20PX">
                                         <span class="icon"> <i class="fas fa-file-signature"></i></span>
                                    </asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
         <asp:Button ID="confirm" runat="server" Text="confirm" OnClick="confirm_Click" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>