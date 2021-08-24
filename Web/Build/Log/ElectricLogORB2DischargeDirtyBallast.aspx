<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogORB2DischargeDirtyBallast.aspx.cs" Inherits="Log_ElectricLogORB2DischargeDirtyBallast" %>


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
    <title>Discharge of Dirty Ballast</title>
    <telerik:radcodeblock id="Radcodeblock1" runat="server">
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
    </telerik:radcodeblock>
  <style>
        .style_one {
            border: 1px solid black;
            border-collapse: collapse;
        }

        .right {
            display:inline-block;
            float:right;
        }

        .left {
            display: inline-block;
            float: left;
            width: 150px;
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

        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">

            <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
            <eluc:Message ID="ucMessage" runat="server" Text="" Visible="false"></eluc:Message>
            <eluc:TabStrip ID="gvTabStrip" runat="server" OnTabStripCommand="gvTabStrip_TabStripCommand"></eluc:TabStrip>
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" Style="display: none;" />
      
            <div style="width: 100%; overflow: auto;">
                <div style="float: initial; margin: 0 20px">
                    <h3>Discharge of Dirty Ballast</h3>

                    <table>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="RadLabel1" runat="server" Text="Date of Operation"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadDatePicker RenderMode="Lightweight" runat="server" CssClass="input_mandatory" Width="150px" AutoPostBack="true" ID="txtOperationDate" MaxLength="12" OnSelectedDateChanged="txtOperationDate_TextChangedEvent" >
                                    <DateInput ID="DateInput1" DateFormat="dd-MMM-yyyy" runat="server"></DateInput>
                                </telerik:RadDatePicker>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="RadLabel4" runat="server" Text="Dirty Ballast Discharged from"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox DropDownPosition="Static" CssClass="input_mandatory"  ID="ddlTransferFrom" runat="server" Width="120px" AutoPostBack="true" DataTextField="FLDNAME" OnSelectedIndexChanged="ddlTransferFrom_SelectedIndexChanged" DataValueField="FLDLOCATIONID" EnableLoadOnDemand="True"
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblPumpStartTime" runat="server" Text="Discharge Start Time (LT)"></telerik:RadLabel>
                            </td>
                            <td>

                                <telerik:RadTimePicker ID="txtStartTime" runat="server" Width="100px" CssClass="input_mandatory" AutoPostBack="true"  OnSelectedDateChanged="txtrefresh_TextChanged">
                                    <TimeView Interval="00:30:00"></TimeView>
                                </telerik:RadTimePicker>
                                <telerik:RadLabel ID="Radlabel2" runat="server" Text="HH:MM"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblStartPosistion" runat="server" Visible="true" Text="Start Position"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Latitude ID="txtStartPosistionLat" runat="server" CssClass="float-left input_mandatory" />
                            </td>
                            <td>
                                <eluc:Longitude ID="txtStartPosistionLog"  runat="server" CssClass="float-left input_mandatory" />
                            </td>
                            <td>
                                <telerik:RadLabel runat="server" ID="lblStartPos" Text="( xx Deg xx Min N/S, xx Deg xx Min E/W)"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblPumpStopTime" runat="server" Text="Discharge Stop Time (LT)"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTimePicker ID="txtStopTime" runat="server" Width="100px" AutoPostBack="true" CssClass="input_mandatory"  OnSelectedDateChanged="txtrefresh_TextChanged">
                                    <TimeView Interval="00:30:00"></TimeView>
                                </telerik:RadTimePicker>
                                <telerik:RadLabel ID="Radlabel3" runat="server" Text="HH:MM"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblStopPosition" runat="server"  Visible="true" Text="Stop Position"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Latitude ID="txtStopPosistionLat" runat="server" CssClass="float-left input_mandatory" />
                            </td>
                            <td>
                                <eluc:Longitude ID="txtStopPosistionLog" runat="server" CssClass="float-left input_mandatory" />
                            </td>
                            <td>
                                <telerik:RadLabel runat="server" ID="lblStopUnit" Text="( xx Deg xx Min N/S, xx Deg xx Min E/W)"></telerik:RadLabel>
                            </td>
                        </tr>
                         <tr>
                            <td>
                                <telerik:RadLabel ID="lbltotalquantity" runat="server" Text="Total Quantity of Dirty Ballast Discharged" style="vertical-align:text-bottom"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txttotquantity" AutoPostBack="true" CssClass="input_mandatory" Width="100px" MinValue="0" MaxValue="99999999" RenderMode="Lightweight" OnTextChanged="txtrefresh_TextChanged" runat="server" MaxLength="22" Style="text-align: right;" Type="Number">
                                    <NumberFormat DecimalSeparator="." DecimalDigits="2" />
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                                <telerik:RadLabel ID="lblfromUnit" runat="server" Text="m3"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblreception" runat="server" Text="Reception Port"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtreceptionport" runat="server" CssClass="input_mandatory" RenderMode="Lightweight" Width="150px" AutoPostBack="true" OnTextChanged="txtrefresh_TextChanged">
                                </telerik:RadTextBox>
                                <%--<telerik:RadNumericTextBox ID="txtSealNo" RenderMode="Lightweight" CssClass="input_mandatory" runat="server" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number"  OnTextChanged="txtFailureReason_TextChanged">
                                </telerik:RadNumericTextBox>--%>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblquantitydischarge" runat="server" Text="Quantiy discharged to shore reception facility"></telerik:RadLabel>
                            </td>
                            <td>
<%--                                <telerik:RadTextBox ID="txtquantitydischarge" runat="server" CssClass="input_mandatory" RenderMode="Lightweight" Width="150px" AutoPostBack="true" OnTextChanged="txtrefresh_TextChanged">
                                </telerik:RadTextBox>--%>
                                <telerik:RadNumericTextBox ID="txtquantitydischarge" RenderMode="Lightweight" CssClass="input_mandatory" MinValue="0" MaxValue="99999999" runat="server" MaxLength="22" Width="120px" Style="text-align: right;" AutoPostBack="true" Type="Number"  OnTextChanged="txtrefresh_TextChanged">
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                                <telerik:RadLabel ID="lblqtyshoreunit" runat="server" Text="m3"></telerik:RadLabel>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <telerik:RadLabel ID="RadLabel5" runat="server" Text="Wash Water Transferred To"></telerik:RadLabel>
                            </td>
                            <td>                                

                                <telerik:RadComboBox DropDownPosition="Static" CssClass="input_mandatory" ID="ddlWaterTransferedFrom" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlWaterTransferedFrom_SelectedIndexChanged" EnableLoadOnDemand="True"
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>                               
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblBfrTrnsROBTo" runat="server" Visible="true" Text="Wash Water Transferred To"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox DropDownPosition="Static" CssClass="input_mandatory"  ID="ddlwashwaterto1" runat="server" Width="120px" AutoPostBack="true" DataTextField="FLDNAME" OnSelectedIndexChanged="ddlTransferFrom_SelectedIndexChanged" DataValueField="FLDLOCATIONID" EnableLoadOnDemand="True"
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>                                
                            </td>
                        </tr>

                         <tr>
                            <td>
                                <telerik:RadLabel ID="RadLabel8" runat="server" Text="Ships Speed" style="vertical-align:text-bottom"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtspeed" AutoPostBack="true" CssClass="input_mandatory" RenderMode="Lightweight" MinValue="0" MaxValue="99999999" Width="100px" OnTextChanged="txtrefresh_TextChanged" runat="server" Style="text-align: right;" Type="Number">
                                    <NumberFormat DecimalSeparator="." DecimalDigits="2" />
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                                <telerik:RadLabel ID="RadLabel9" runat="server" Text="Knots"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lbltotalrob" runat="server" Text="Total ROB in Above Tank or Shore"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txttotalrob" AutoPostBack="true" CssClass="input_mandatory" MinValue="0" MaxValue="99999999" RenderMode="Lightweight" Width="100px" OnTextChanged="txtrefresh_TextChanged" runat="server" MaxLength="22" Style="text-align: right;" Type="Number">
                                    <NumberFormat DecimalSeparator="." DecimalDigits="2" />
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                                <telerik:RadLabel ID="lbltotunit" runat="server" Text="m3"></telerik:RadLabel>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <telerik:RadLabel ID="RadLabel11" runat="server" Text="Quantity of oily-water transferred to slop tank" Style="vertical-align: text-bottom"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtqtytransfer" AutoPostBack="true" CssClass="input_mandatory" MinValue="0" MaxValue="99999999" RenderMode="Lightweight" OnTextChanged="txtrefresh_TextChanged" runat="server" MaxLength="22" Width="100px" Style="text-align: right;" Type="Number">
                                    <NumberFormat DecimalSeparator="." DecimalDigits="2" />
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                                <telerik:RadLabel ID="lblqtyoilyunit" runat="server" Text="m3"></telerik:RadLabel>
                            </td>
                        </tr>

                        <tr>
                            
                            <td>
                                <telerik:RadLabel ID="RadLabel7" runat="server" Text="Was the discharge monitoring and control system<br/> in operation during discharge?" style="vertical-align:text-bottom"></telerik:RadLabel>
                            </td>
                            <td>
<%--                                <telerik:RadComboBox DropDownPosition="Static" CssClass="input_mandatory" ID="ddldischargemonitoring" runat="server" AutoPostBack="true" AppendDataBoundItems="false"  OnSelectedIndexChanged="ddlTransferFrom_SelectedIndexChanged" 
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                    <Items>
                                    <telerik:RadComboBoxItem runat="server" Text="Yes" />   
                                    <telerik:RadComboBoxItem runat="server" Text="No" />   
                                        </Items>
                                </telerik:RadComboBox>  --%>
                                <asp:RadioButtonList ID="ddldischargemonitoring" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddlTransferFrom_SelectedIndexChanged" >
                                    <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="No"></asp:ListItem>     
                                </asp:RadioButtonList>
<%--                                <telerik:RadTextBox ID="txtdischargemonitoring" runat="server" CssClass="input_mandatory" RenderMode="Lightweight" Width="100px" AutoPostBack="true" OnTextChanged="txtrefresh_TextChanged" >
                                </telerik:RadTextBox>
                                <telerik:RadLabel ID="RadLabel12" runat="server" Text="Yes/No"></telerik:RadLabel>--%>
                            </td>

                            <td>
                                <telerik:RadLabel ID="RadLabel13" runat="server" Text="Was a regular check carried out on the effluent<br/> and the surface of the water in <br/>the locality of ghe discharge?" style="vertical-align:text-bottom"></telerik:RadLabel>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="ddlregularcheck" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddlTransferFrom_SelectedIndexChanged" >
                                    <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="No"></asp:ListItem>     
                                </asp:RadioButtonList>
<%--                                <telerik:RadComboBox DropDownPosition="Static" CssClass="input_mandatory" ID="ddlregularcheck" runat="server" AutoPostBack="true" AppendDataBoundItems="false"  OnSelectedIndexChanged="ddlTransferFrom_SelectedIndexChanged" 
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                    <Items>
                                    <telerik:RadComboBoxItem runat="server" Text="Yes" />   
                                    <telerik:RadComboBoxItem runat="server" Text="No" />   
                                        </Items>
                                </telerik:RadComboBox>  --%>


<%--                                <telerik:RadTextBox ID="txtlocalitydischarge" runat="server" CssClass="input_mandatory" RenderMode="Lightweight" Width="100px" AutoPostBack="true" OnTextChanged="txtrefresh_TextChanged" MaxLength="10">
                                </telerik:RadTextBox>
                                <telerik:RadLabel ID="RadLabel14" runat="server" Text="Yes/No"></telerik:RadLabel>--%>
                            </td>
                        </tr>
                    </table>

                    <div>
                        <br />
                        <table class="style_one" style="font-size: small; padding: 0 50px; width: 100%">
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
                         <tr>
                            <td></td>
                            <td></td>
                            <td class="style_one" style="text-align:center">
                                <telerik:radlabel id="txtItemNo5" runat="server"></telerik:radlabel>
                            </td>
                            <td class="style_one"> 
                                <telerik:radlabel id="lblrecord5" runat="server"></telerik:radlabel>
                            </td>
                        </tr>

                         <tr>
                            <td></td>
                            <td></td>
                            <td class="style_one" style="text-align:center">
                                <telerik:radlabel id="txtItemNo6" runat="server"></telerik:radlabel>
                            </td>
                            <td class="style_one"> 
                                <telerik:radlabel id="lblrecord6" runat="server"></telerik:radlabel>
                            </td>
                        </tr>

                         <tr>
                            <td></td>
                            <td></td>
                            <td class="style_one" style="text-align:center">
                                <telerik:radlabel id="txtItemNo7" runat="server"></telerik:radlabel>
                            </td>
                            <td class="style_one"> 
                                <telerik:radlabel id="lblrecord7" runat="server"></telerik:radlabel>
                            </td>
                        </tr>

                         <tr>
                            <td></td>
                            <td></td>
                            <td class="style_one" style="text-align:center">
                                <telerik:radlabel id="txtItemNo8" runat="server"></telerik:radlabel>
                            </td>
                            <td class="style_one"> 
                                <telerik:radlabel id="lblrecord8" runat="server"></telerik:radlabel>
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
                                        ToolTip="Incharge Sign" Width="20PX" Height="20PX">
                                         <span class="icon"> <i class="fas fa-file-signature"></i></span>
                                    </asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                        </div>
                    </div>
                </div>
            <br />
            <br />
                    <div class="notes">
                        <p>Note:</p>
                        <telerik:RadLabel runat="server" ID="lblnotes"></telerik:RadLabel>
                    </div>
         <asp:Button ID="confirm" runat="server" Text="confirm" OnClick="confirm_Click" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
