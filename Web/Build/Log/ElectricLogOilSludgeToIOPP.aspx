<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogOilSludgeToIOPP.aspx.cs" Inherits="Log_ElectricLogOilSludgeToIOPP" %>

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
    <title>Oil Sludge to IOPP Tank</title>
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
                        <telerik:RadDatePicker RenderMode="Lightweight" runat="server" CssClass="input_mandatory" ID="txtOperationDate" MaxLength="12"  OnSelectedDateChanged="txtOperationDate_SelectedDateChanged" AutoPostBack="true"  >
                            <DateInput DateFormat="dd-MMM-yyyy" runat="server"></DateInput>
                        </telerik:RadDatePicker>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFrom" Text="Sludge Transferred From" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <%--<telerik:RadComboBox DropDownPosition="Static" CssClass="input_mandatory"  ID="ddlTransferFrom" runat="server" DataTextField="FLDNAME" OnSelectedIndexChanged="ddlTransferFrom_SelectedIndexChanged" DataValueField="FLDLOCATIONID" EnableLoadOnDemand="True"
                            EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                        </telerik:RadComboBox>--%>
                         <telerik:RadTextBox runat="server" ID="ddlTransferFrom" CssClass="input_mandatory" OnTextChanged="ddlTransferFrom_SelectedIndexChanged" AutoPostBack="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblTo" runat="server"  Text="Sludge Transferred to tank" visible="true"  ></telerik:RadLabel>
                    </td>
                    <td>
                         <telerik:RadComboBox ID="ddlTransferTo" DropDownPosition="Static" CssClass="input_mandatory"  visible="true" runat="server" DataTextField="FLDNAME" DataValueField="FLDLOCATIONID" EnableLoadOnDemand="True"
                             EmptyMessage="Type to select" Filter="Contains" AutoPostBack="true" MarkFirstMatch="true" OnSelectedIndexChanged="ddlTransferTo_SelectedIndexChanged">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBfrTrnsROBFrom" runat="server" Text="Transferred Quantity"></telerik:RadLabel>
                    </td>
                    <td>
                         <telerik:RadNumericTextBox ID="txtTransferQuantity" CssClass="input_mandatory" MinValue="0" MaxValue="99999999" RenderMode="Lightweight" OnTextChanged="txtTransferQuantity_TextChangedEvent" AutoPostBack="true"  visible="true" runat="server"  MaxLength="22" Width="120px" style="text-align:right;" Type="Number">                            
                             <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                        </telerik:RadNumericTextBox>   
                        <telerik:RadLabel ID="lblfromUnit" runat="server" Text="m3"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBfrTrnsROBTo" runat="server" visible="true" Text="Before Transfer ROB"></telerik:RadLabel>
                    </td>
                    <td>
                         <telerik:RadNumericTextBox ID="txtBfrTrnsROBTo" CssClass="input_mandatory" MinValue="0" MaxValue="99999999" RenderMode="Lightweight" OnTextChanged="txtTransferQuantity_TextChangedEvent" AutoPostBack="true"  visible="true" runat="server"  MaxLength="22" Width="120px" style="text-align:right;" Type="Number">                            
                             <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                        </telerik:RadNumericTextBox>                         
                        <telerik:RadLabel ID="lbltounit" runat="server" visible="true" Text="m3"></telerik:RadLabel>
                    </td>
                </tr>
            <tr>
                <td></td>
                <td></td>
                <td>
                    <telerik:RadLabel ID="lblAftrROB" runat="server" visible="true" Text="After Transfer ROB"></telerik:RadLabel>
                    </td>
                    <td>
                         <telerik:RadNumericTextBox ID="txtAftrTrnsROBTo" CssClass="input_mandatory" MinValue="0" MaxValue="99999999" RenderMode="Lightweight" OnTextChanged="txtTransferQuantity_TextChangedEvent" AutoPostBack="true" visible="true" runat="server"  MaxLength="22" Width="120px" style="text-align:right;" Type="Number">
                            <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                        </telerik:RadNumericTextBox> 
                        <telerik:RadLabel ID="RadLabel3" runat="server" visible="true" Text="m3"></telerik:RadLabel>
                    </td>
            </tr>
            </table>
             <br />
           
                    <table cellpadding="5px" cellspacing="5px" class="style_one" style="font-size: small; padding: 0 50px; width: 100%">
                        <tr class="style_one">
                            <td class="style_one" style="width: 50px">
                                <b>
                                    <telerik:radlabel id="lblDate" runat="server" visible="True" text="Date"></telerik:radlabel>
                                </b>
                            </td>
                            <td class="style_one" style="width: 40px">
                                <b>
                                    <telerik:radlabel id="capCode" runat="server" visible="True" text="Code"></telerik:radlabel>
                                </b>
                            </td>
                            <td class="style_one" style="width: 70px">
                                <b>
                                    <telerik:radlabel id="capItemNo" runat="server" visible="True" text="Item No."></telerik:radlabel>
                                </b>
                            </td>
                            <td class="style_one">
                                <b>
                                    <telerik:radlabel id="lblcapRecord" runat="server" visible="True" text="Record of operation / Signature of officer in charge"></telerik:radlabel>
                                </b>
                            </td>
                        </tr>
                        <tr class="style_one">
                            <td class="style_one">
                                <telerik:radlabel id="txtDate" runat="server" width="100px" visible="True"></telerik:radlabel>
                            </td>
                            <td class="style_one" style="text-align:center">
                                <telerik:radlabel id="txtCode" runat="server" visible="True"></telerik:radlabel>
                            </td>
                            <td class="style_one" style="text-align:center">
                                <telerik:radlabel id="txtItemNo" runat="server" visible="True"></telerik:radlabel>
                            </td>
                            <td>
                                <telerik:radlabel id="lblRecord" runat="server"></telerik:radlabel>
                            </td>
                        </tr>
                         <tr>
                            <td></td>
                            <td></td>
                            <td class="style_one" style="text-align:center">
                                <telerik:radlabel id="txtcode1" runat="server"></telerik:radlabel>
                            </td>
                            <td class="style_one"> 
                                <telerik:radlabel id="lbltorecord" runat="server"></telerik:radlabel>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td class="style_one" style="text-align:center">
                                <telerik:radlabel id="txtcode2" runat="server"></telerik:radlabel>
                            </td>
                            <td class="style_one">
                                <telerik:radlabel id="lblAfterROB" runat="server"></telerik:radlabel>
                            </td>
                        </tr>
                         <tr>
                            <td></td>
                            <td></td>
                            <td class="style_one" style="text-align:center">
                                <telerik:radlabel id="txtcode3" runat="server"></telerik:radlabel>
                            </td>
                            <td class="style_one">
                                <telerik:radlabel id="lblFromRob" runat="server"></telerik:radlabel>
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
