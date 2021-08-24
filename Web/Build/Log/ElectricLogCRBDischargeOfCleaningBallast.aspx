<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogCRBDischargeOfCleaningBallast.aspx.cs" Inherits="Log_ElectricLogCRBDischargeOfCleaningBallast" %>

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
    <title>Discharge of Ballast Water from Cargo Tanks</title>
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
                 <h3>Discharge of Ballast Water from Cargo Tanks</h3>
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
                                <telerik:RadLabel ID="lblFrom" runat="server" Text="Identity of Tanks, Discharged"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox DropDownPosition="Static" CssClass="input_mandatory"  ID="ddlTransferFrom" runat="server" DataTextField="FLDNAME" AutoPostBack="true" OnSelectedIndexChanged="ddlTransferFrom_SelectedIndexChanged" DataValueField="FLDLOCATIONID" EnableLoadOnDemand="True"
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lbldischargeballast" runat="server" Visible="true" Text="Discharge of Ballast from Cargo Tank into Sea" Width="200px"></telerik:RadLabel>
                            </td>
                            <td>
                                 <telerik:RadRadioButtonList runat="server" ID="ddldischargeballast"  AutoPostBack="true" Direction="Horizontal" OnSelectedIndexChanged="ddldischargeballast_SelectedIndexChanged">
                                    <Items>
                                        <telerik:ButtonListItem Value="No" Text="No" ToolTip="No"  Selected="true" />
                                        <telerik:ButtonListItem Value="Yes" Text="Yes" ToolTip="Yes"/>
                                    </Items>
                                </telerik:RadRadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lbldischargeport" runat="server" Visible="true" Text="Discharge of Ballast from Cargo Tank Port Reception" Width="200px"></telerik:RadLabel>
                            </td>
                            <td>
                                 <telerik:RadRadioButtonList runat="server" ID="ddldischargeport"  AutoPostBack="true" Direction="Horizontal" OnSelectedIndexChanged="ddldischargeballast_SelectedIndexChanged">
                                    <Items>
                                        <telerik:ButtonListItem Value="No" Text="No" ToolTip="No"  Selected="true" />
                                        <telerik:ButtonListItem Value="Yes" Text="Yes" ToolTip="Yes"/>
                                    </Items>
                                </telerik:RadRadioButtonList>
                            </td>
                           
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblDisStartTime" runat="server" Text="Discharge Start Time"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTimePicker ID="txtDisStartTime" runat="server" CssClass="input_mandatory" AutoPostBack="true" Width="100px" OnSelectedDateChanged="txtStartTime_SelectedDateChanged">
                                    <TimeView runat="server" Interval="00:30:00"></TimeView>
                                </telerik:RadTimePicker>
                                <telerik:RadLabel ID="Radlabel2" runat="server" Text="HH:MM"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblUTCStartTime" runat="server" Text="UTC"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTimePicker ID="txtUTCStartTime" runat="server" CssClass="input_mandatory" AutoPostBack="true" Width="100px" OnSelectedDateChanged="txtStartTime_SelectedDateChanged">
                                    <TimeView runat="server" Interval="00:30:00"></TimeView>
                                </telerik:RadTimePicker>
                                <telerik:RadLabel ID="Radlabel4" runat="server" Text="HH:MM"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblDisStopTime" runat="server" Text="Discharge Stop Time"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTimePicker ID="txtDisStopTime" runat="server" CssClass="input_mandatory" AutoPostBack="true" Width="100px" OnSelectedDateChanged="txtStartTime_SelectedDateChanged">
                                    <TimeView runat="server" Interval="00:30:00"></TimeView>
                                </telerik:RadTimePicker>
                                <telerik:RadLabel ID="Radlabel5" runat="server" Text="HH:MM"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblUTCStopTime" runat="server" Text="UTC"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTimePicker ID="txtUTCStopTime" runat="server" CssClass="input_mandatory" AutoPostBack="true" Width="100px" OnSelectedDateChanged="txtStartTime_SelectedDateChanged">
                                    <TimeView runat="server" Interval="00:30:00"></TimeView>
                                </telerik:RadTimePicker>
                                <telerik:RadLabel ID="Radlabel7" runat="server" Text="HH:MM"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                             <td>
                                <telerik:RadLabel ID="lblReceptionName" runat="server" Visible="true" Text="Name of Reception Facility, Terminal & Port"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtReceptionName" RenderMode="Lightweight" CssClass="input_mandatory" runat="server" TextMode="MultiLine" Width="120px" Height="75px" Resize="both" AutoPostBack="true" OnTextChanged="Refresh_TextChangedEvent" Rows="10" Columns="5">                                    
                                </telerik:RadTextBox>                                
                            </td>

                            <td>
                                <telerik:RadLabel ID="lblspeed" runat="server" Visible="true" Text="Ships Speed"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtspeed" RenderMode="Lightweight" MinValue="0" MaxValue="99999999" CssClass="input_mandatory" runat="server" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number" AutoPostBack="true" OnTextChanged="Refresh_TextChangedEvent">
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                                <telerik:RadLabel ID="RadLabel8" runat="server" Visible="true" Text="Knots"></telerik:RadLabel>
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
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="txtItemNo3" runat="server"></telerik:RadLabel>
                                </td>
                                <td class="style_one">
                                    <telerik:RadLabel ID="lblrecord3" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="txtItemNo4" runat="server"></telerik:RadLabel>
                                </td>
                                <td class="style_one">
                                    <telerik:RadLabel ID="lblrecord4" runat="server"></telerik:RadLabel>
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
                                        ToolTip="Incharge Sign" Width="20PX" Height="20PX">
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