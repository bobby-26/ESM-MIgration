<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogORB2DischargeOfWater.aspx.cs" Inherits="Log_ElectricLogORB2DischargeOfWater" %>


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
    <title>Discharge of Water from Slop tanks into the Sea</title>
    <telerik:radcodeblock id="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
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

        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">

            <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
            <eluc:Message ID="ucMessage" runat="server" Text="" Visible="false"></eluc:Message>
            <eluc:TabStrip ID="gvTabStrip" runat="server" OnTabStripCommand="gvTabStrip_TabStripCommand"></eluc:TabStrip>
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" Style="display: none;" />
      
            <div style="width: 100%; overflow:scroll;">
                <div style="float: initial; margin: 0 20px">

                    <h3>Discharge of Water from Slop tanks into the Sea</h3>

                    <table>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lbloperationdate" runat="server" Width="200px" Text="Date of Operation"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadDatePicker RenderMode="Lightweight" runat="server" CssClass="input_mandatory" AutoPostBack="true" Width="120px" ID="txtOperationDate" MaxLength="12" OnSelectedDateChanged="txtOperationDate_TextChangedEvent" >
                                    <DateInput ID="DateInput1" DateFormat="dd-MMM-yyyy" runat="server"></DateInput>
                                </telerik:RadDatePicker>
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="RadLabel4" runat="server" Text="Identity of Slop Tank"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox DropDownPosition="Static" CssClass="input_mandatory" width="150px" ID="ddlTransferFrom" runat="server" AutoPostBack="true"  DataTextField="FLDNAME" OnSelectedIndexChanged="ddlTransferFrom_SelectedIndexChanged" DataValueField="FLDLOCATIONID" EnableLoadOnDemand="True"
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                         <td>
                                <telerik:RadLabel ID="RadLabel15" runat="server" Text="Time of settling from last entry of residue to slop tank" style="vertical-align:text-bottom"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txttimeentry" AutoPostBack="true" CssClass="input_mandatory" MinValue="0" MaxValue="99999999" RenderMode="Lightweight" OnTextChanged="txtrefresh_TextChanged" runat="server" MaxLength="22" Width="100px" Style="text-align: right;" Type="Number">
                                    <NumberFormat DecimalSeparator="." DecimalDigits="2" />
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                                <telerik:RadLabel ID="RadLabel16" runat="server" Text="Hours"></telerik:RadLabel>
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>

                        <tr>
                         <td>
                                <telerik:RadLabel ID="RadLabel17" runat="server" Text="Time of settling from last discharge" style="vertical-align:text-bottom"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtdischarge" AutoPostBack="true" CssClass="input_mandatory" MinValue="0" MaxValue="99999999" RenderMode="Lightweight" OnTextChanged="txtrefresh_TextChanged" runat="server" MaxLength="22" Width="100px" Style="text-align: right;" Type="Number">
                                    <NumberFormat DecimalSeparator="." DecimalDigits="2" />
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                                <telerik:RadLabel ID="RadLabel18" runat="server" Text="Hours"></telerik:RadLabel>
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>


                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblPumpStartTime" runat="server" Text="Discharge Start Time"></telerik:RadLabel>
                            </td>
                            <td>

                                <telerik:RadTimePicker ID="txtStartTime" runat="server" CssClass="input_mandatory" AutoPostBack="true" Width="100px"  OnSelectedDateChanged="txt_SelectedDateChanged">
                                    <TimeView Interval="00:30:00"></TimeView>
                                </telerik:RadTimePicker>
                                <telerik:RadLabel ID="Radlabel2" runat="server" Text="HH:MM"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblutc" runat="server" Width="80px" Text="UTC" ></telerik:RadLabel>
                            </td>
                            <td>

                                <telerik:RadTimePicker ID="txtutc" runat="server" CssClass="input_mandatory" AutoPostBack="true" Width="100px" OnSelectedDateChanged="txt_SelectedDateChanged">
                                    <TimeView Interval="00:30:00"></TimeView>
                                </telerik:RadTimePicker>
                                <telerik:RadLabel ID="Radlabel20" runat="server" Text="HH:MM"></telerik:RadLabel>
                                <telerik:RadLabel ID="lbllength" runat="server" Width="150px" Text=""></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblStartPosistion" runat="server" Width="150px" Visible="true" Text="Start Position"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Latitude ID="txtStartPosistionLat" runat="server" CssClass="input_mandatory" />                                
                            </td>
                            <td>                                
                                <telerik:RadLabel runat="server" ID="lbllen" Width="200px" Text=""></telerik:RadLabel>                                
                                <eluc:Longitude ID="txtStartPosistionLog"  runat="server" CssClass="input_mandatory" />                                
                            </td>
                            <td>
                                <telerik:RadLabel runat="server" ID="lblStartPos" Width="200px" Text="( xx Deg xx Min N/S, xx Deg xx Min E/W)"></telerik:RadLabel>                                
                            </td>
                        </tr>


                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblPumpStopTime" runat="server" Text="Discharge Stop Time"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTimePicker ID="txtStopTime" runat="server" AutoPostBack="true" CssClass="input_mandatory" Width="100px" OnSelectedDateChanged="txt_SelectedDateChanged">
                                    <TimeView Interval="00:30:00"></TimeView>
                                </telerik:RadTimePicker>
                                <telerik:RadLabel ID="Radlabel3" runat="server" Text="HH:MM"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblutc2" runat="server" Text="UTC"></telerik:RadLabel>
                            </td>
                            <td>

                                <telerik:RadTimePicker ID="txtutc2" runat="server" CssClass="input_mandatory" AutoPostBack="true" Width="100px"  OnSelectedDateChanged="txt_SelectedDateChanged">
                                    <TimeView Interval="00:30:00"></TimeView>
                                </telerik:RadTimePicker>
                                <telerik:RadLabel ID="Radlabel22" runat="server" Text="HH:MM"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblStopPosition" runat="server"  Visible="true" Text="Stop Position"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Latitude ID="txtStopPosistionLat" runat="server" CssClass="input_mandatory"   OnSelectedDateChanged="txt_SelectedDateChanged" />                                
                            </td>
                            <td>
                                <eluc:Longitude ID="txtStopPosistionLog" runat="server" CssClass="input_mandatory"  OnSelectedDateChanged="txt_SelectedDateChanged" />                                
                            </td>
                            <td>
                                <telerik:RadLabel runat="server" ID="lblStopUnit" Width="200px" Text="( xx Deg xx Min N/S, xx Deg xx Min E/W)"></telerik:RadLabel>
                            </td>
                        </tr>                         
                        <tr>
                        <td>
                                <telerik:RadLabel ID="lblbulkqtydischarged" runat="server" Text="Bulk quantity discharged in m3" style="vertical-align:text-bottom"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtbulkquantity" AutoPostBack="true" CssClass="input_mandatory" MinValue="0" MaxValue="99999999" RenderMode="Lightweight" Width="100px" OnTextChanged="txtrefresh_TextChanged" runat="server" MaxLength="22" Style="text-align: right;" Type="Number">
                                    <NumberFormat DecimalSeparator="." DecimalDigits="2" />
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                                <telerik:RadLabel ID="RadLabel24" runat="server" Text="m3"></telerik:RadLabel>
                            </td>
                             <td>
                                <telerik:RadLabel ID="RadLabel25" runat="server" Text="Rate of discharge" style="vertical-align:text-bottom"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtrtdischarge" AutoPostBack="true" CssClass="input_mandatory" MinValue="0" MaxValue="99999999" RenderMode="Lightweight" OnTextChanged="txtrefresh_TextChanged" runat="server" MaxLength="22" Width="100px" Style="text-align: right;" Type="Number">
                                    <NumberFormat DecimalSeparator="." DecimalDigits="2" />
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                                <telerik:RadLabel ID="RadLabel26" runat="server" Text="m3/hr"></telerik:RadLabel>
                            </td>

                             <td>
                                <telerik:RadLabel ID="lblfinalqty" runat="server" Text="Final quantity discharged in m3" style="vertical-align:text-bottom"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtfinalqtydischarge" AutoPostBack="true" CssClass="input_mandatory" MinValue="0" MaxValue="99999999" RenderMode="Lightweight" OnTextChanged="txtrefresh_TextChanged" runat="server" MaxLength="22" Width="100px" Style="text-align: right;" Type="Number">
                                    <NumberFormat DecimalSeparator="." DecimalDigits="2" />
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                                <telerik:RadLabel ID="RadLabel28" runat="server" Text="m3"></telerik:RadLabel>
                            </td>

                            <td>
                                <telerik:RadLabel ID="RadLabel29" runat="server" Text="Rate of discharge" Width="110px" style="vertical-align:text-bottom; text-align:left"></telerik:RadLabel>
                                 <telerik:RadNumericTextBox ID="txtrtdischarge2" AutoPostBack="true" CssClass="input_mandatory" MinValue="0" MaxValue="99999999" RenderMode="Lightweight" OnTextChanged="txtrefresh_TextChanged" runat="server" MaxLength="22" Width="50px" Style="text-align: left;" Type="Number">
                                    <NumberFormat DecimalSeparator="." DecimalDigits="2" />
                                     <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                                <telerik:RadLabel ID="RadLabel30" runat="server" Text="m3/hr"></telerik:RadLabel>
                            </td>
                        </tr>

                        <tr>
                        <td>
                                <telerik:RadLabel ID="lblullagetotalcontent" runat="server" Text="Ullage of total contents at the start of discharge" style="vertical-align:text-bottom"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtullagedischarge" AutoPostBack="true" CssClass="input_mandatory" MinValue="0" MaxValue="99999999" RenderMode="Lightweight" OnTextChanged="txtrefresh_TextChanged" runat="server" MaxLength="22" Width="100px" Style="text-align: right;" Type="Number">
                                    <NumberFormat DecimalSeparator="." DecimalDigits="2" />
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                                <telerik:RadLabel ID="RadLabel32" runat="server" Text="mtr"></telerik:RadLabel>
                            </td>
                            <td></td>
                            <td></td>
                             <td>
                                <telerik:RadLabel ID="lblullageoilwater" runat="server" Text="Ullage of oil/water interface at start of discharge" style="vertical-align:text-bottom"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtullagestart" AutoPostBack="true" CssClass="input_mandatory" RenderMode="Lightweight" MinValue="0" MaxValue="99999999" OnTextChanged="txtrefresh_TextChanged" runat="server" MaxLength="22" Width="100px" Style="text-align: right;" Type="Number">
                                    <NumberFormat DecimalSeparator="." DecimalDigits="2" />
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                                <telerik:RadLabel ID="RadLabel34" runat="server" Text="mtr"></telerik:RadLabel>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>

                         <tr>
                        <td>
                                <telerik:RadLabel ID="lblshipspeed" runat="server" Text="Ships Speed" style="vertical-align:text-bottom"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtshipspeed" AutoPostBack="true" CssClass="input_mandatory" RenderMode="Lightweight" MinValue="0" MaxValue="99999999" OnTextChanged="txtrefresh_TextChanged" runat="server" MaxLength="22" Width="100px" Style="text-align: right;" Type="Number">
                                    <NumberFormat DecimalSeparator="." DecimalDigits="2" />
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                                <telerik:RadLabel ID="RadLabel36" runat="server" Text="knots"></telerik:RadLabel>
                            </td>
                            <td></td>
                            <td></td>
                             <td>
                                <telerik:RadLabel ID="lblullagecompletion" runat="server" Text="Ullage of oil/water interface on completion of discharge" style="vertical-align:text-bottom"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtullagecompletion" AutoPostBack="true" CssClass="input_mandatory" MinValue="0" MaxValue="99999999" RenderMode="Lightweight" OnTextChanged="txtrefresh_TextChanged" runat="server" MaxLength="22" Width="100px" Style="text-align: right;" Type="Number">
                                    <NumberFormat DecimalSeparator="." DecimalDigits="2" />
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                                <telerik:RadLabel ID="RadLabel38" runat="server" Text="mtr"></telerik:RadLabel>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                         <tr>
                            <td>
                                <telerik:RadLabel ID="lbldismonitoring" runat="server" Text="Was the discharge monitoring and control system in operation during discharge?" style="vertical-align:text-bottom"></telerik:RadLabel>
                            </td>
                            <td>                               
                                <asp:RadioButtonList ID="txtduringdischarge" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddlTransferFrom_SelectedIndexChanged" >
                                    <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="No"></asp:ListItem>     
                                </asp:RadioButtonList>
<%--                                <telerik:RadTextBox ID="txtduringdischarge" RenderMode="Lightweight" AutoPostBack="true" OnTextChanged="txtrefresh_TextChanged" CssClass="input_mandatory" runat="server" MaxLength="22" Width="100px" Style="text-align: right;" Type="Number">
                                </telerik:RadTextBox>
                                <telerik:RadLabel ID="lblyesno" runat="server" Text="Yes/No"></telerik:RadLabel>--%>
                            </td>
                            <td></td>
                            <td></td>
                            <td>
                                <telerik:RadLabel ID="lblregularcheck" runat="server" Visible="true" Text="Was a regular check carried out on the effluent and the surface of the water in the locality of ghe discharge?" style="vertical-align:text-bottom"></telerik:RadLabel>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="txtregularcheckdischsrge" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddlTransferFrom_SelectedIndexChanged" >
                                    <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="No"></asp:ListItem>     
                                </asp:RadioButtonList>
<%--                                <telerik:RadTextBox ID="txtregularcheckdischsrge" RenderMode="Lightweight" CssClass="input_mandatory" runat="server" OnTextChanged="txtrefresh_TextChanged" AutoPostBack="true" MaxLength="22" Width="100px" Style="text-align: right;" Type="Number">
                                </telerik:RadTextBox>
                                <telerik:RadLabel ID="lblregularcheckdischsrge" runat="server" Visible="true" Text="Yes/No"></telerik:RadLabel>--%>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                         <td>
                                <telerik:RadLabel ID="lblconfirm" runat="server" Visible="true" Text="Confirm that all applicable valves in the ships pipping system have been closed on completion of discharge from the slop tanks" style="vertical-align:text-bottom"></telerik:RadLabel>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="txtconfirmvalves" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddlTransferFrom_SelectedIndexChanged" >
                                    <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="No"></asp:ListItem>     
                                </asp:RadioButtonList>
<%--                                <telerik:RadTextBox ID="txtconfirmvalves" RenderMode="Lightweight" CssClass="input_mandatory" runat="server"  MaxLength="22" OnTextChanged="txtrefresh_TextChanged" AutoPostBack="true" Width="100px" Style="text-align: right;" Type="Number">
                                </telerik:RadTextBox>
                                <telerik:RadLabel ID="lblconfirmvalves" runat="server" Visible="true" Text="Yes/No"></telerik:RadLabel>--%>
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
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
                            <td class="style_one" style="text-align:center">
                                <telerik:radlabel id="txtItemNo9" runat="server"></telerik:radlabel>
                            </td>
                            <td class="style_one"> 
                                <telerik:radlabel id="lblrecord9" runat="server"></telerik:radlabel>
                            </td>
                        </tr>

                         <tr>
                            <td></td>
                            <td></td>
                            <td class="style_one" style="text-align:center">
                                <telerik:radlabel id="txtItemNo10" runat="server"></telerik:radlabel>
                            </td>
                            <td class="style_one"> 
                                <telerik:radlabel id="lblrecord10" runat="server"></telerik:radlabel>
                            </td>
                        </tr>

                         <tr>
                            <td></td>
                            <td></td>
                            <td class="style_one" style="text-align:center">
                                <telerik:radlabel id="txtItemNo11" runat="server"></telerik:radlabel>
                            </td>
                            <td class="style_one"> 
                                <telerik:radlabel id="lblrecord11" runat="server"></telerik:radlabel>
                            </td>
                        </tr>

                         <tr>
                            <td></td>
                            <td></td>
                            <td class="style_one" style="text-align:center">
                                <telerik:radlabel id="txtItemNo12" runat="server"></telerik:radlabel>
                            </td>
                            <td class="style_one"> 
                                <telerik:radlabel id="lblrecord12" runat="server"></telerik:radlabel>
                            </td>
                        </tr>

                         <tr>
                            <td></td>
                            <td></td>
                            <td class="style_one" style="text-align:center">
                                <telerik:radlabel id="txtItemNo13" runat="server"></telerik:radlabel>
                            </td>
                            <td class="style_one"> 
                                <telerik:radlabel id="lblrecord13" runat="server"></telerik:radlabel>
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
                    <div class="notes">
                        <p>Notes:</p>
                        <telerik:RadLabel runat="server" ID="lblnotes"></telerik:RadLabel>
                    </div>

                </div>
            </div>
            <br />
            <br />
                    

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>