<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogORB2CrudeOilWashing.aspx.cs" Inherits="Log_ElectricLogORB2CrudeOilWashing" %>



<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crude Oil Washing (COW Tankers Only)</title>
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

        <telerik:RadAjaxPanel runat="server" ID="radAjaxPanel">

            <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
            <eluc:Message ID="ucMessage" runat="server" Text="" Visible="false"></eluc:Message>
            <eluc:TabStrip ID="gvTabStrip" runat="server" OnTabStripCommand="gvTabStrip_TabStripCommand"></eluc:TabStrip>
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" Style="display: none;" />
      
            <div style="width: 100%; overflow: auto;">
                <div style="float: initial; margin: 0 50px">
                 <h3>Crude Oil Washing (COW Tankers Only)</h3>
                    <table>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lbloperationdate" runat="server" Text="Date of Operation"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadDatePicker RenderMode="Lightweight" runat="server" CssClass="input_mandatory" ID="txtOperationDate" MaxLength="12" AutoPostBack="true" OnSelectedDateChanged="txtOperationDate_SelectedDateChanged" >
                                    <DateInput DateFormat="dd-MMM-yyyy" runat="server"></DateInput>
                                </telerik:RadDatePicker>
                            </td>
                        </tr>


                        <tr>
                        <td>
                                <telerik:RadLabel ID="lblterminalposition" runat="server" Text="Terminal or position" style="vertical-align:text-bottom"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtterminal" runat="server" CssClass="input_mandatory" RenderMode="Lightweight" AutoPostBack="true" OnTextChanged="txtrefresh_TextChanged" >
                                </telerik:RadTextBox>
                               
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblFrom" runat="server" Text="Crude Oil Washingn tank"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox DropDownPosition="Static" CssClass="input_mandatory"  ID="ddlTransferFrom" runat="server" DataTextField="FLDNAME" AutoPostBack="true" OnSelectedIndexChanged="ddlTransferFrom_SelectedIndexChanged" DataValueField="FLDLOCATIONID" EnableLoadOnDemand="True"
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblBfrTrnsROBFrom" runat="server" Text="Number of Tank Cleaning machines in use"></telerik:RadLabel>
                            </td>
                            <td>
                                <%--<eluc:Number ID="txtBfrTrnsROBFrom" runat="server" MaxLength="3" Width="120px"/>--%>

                                <telerik:RadNumericTextBox ID="txtnoofcleaningmachine" RenderMode="Lightweight" MinValue="0" MaxValue="99999999" CssClass="input_mandatory" runat="server" MaxLength="22" Width="120px" OnTextChanged="txtrefresh_TextChanged" AutoPostBack="true" Style="text-align: right;" Type="Number">
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                                
                            </td>
                        </tr>

                        <tr>
                         <td>
                                <telerik:RadLabel ID="lblcowstart" runat="server" Text="COW Start Time"></telerik:RadLabel>
                            </td>
                            <td>

                                <telerik:RadTimePicker ID="txtstarttime" runat="server" CssClass="input_mandatory" AutoPostBack="true"  OnSelectedDateChanged="txt_SelectedDateChanged">
                                    <TimeView Interval="00:30:00"></TimeView>
                                </telerik:RadTimePicker>
                                <telerik:RadLabel ID="Radlabel18" runat="server" Text="HH:MM"></telerik:RadLabel>
                            </td>
                        </tr>

                         <tr>
                         <td>
                                <telerik:RadLabel ID="RadLabel2" runat="server" Text="COW Stop Time"></telerik:RadLabel>
                            </td>
                            <td>

                                <telerik:RadTimePicker ID="txtstoptime" runat="server" CssClass="input_mandatory" AutoPostBack="true"  OnSelectedDateChanged="txt_SelectedDateChanged">
                                    <TimeView Interval="00:30:00"></TimeView>
                                </telerik:RadTimePicker>
                                <telerik:RadLabel ID="Radlabel3" runat="server" Text="HH:MM"></telerik:RadLabel>
                            </td>
                        </tr>

                         <tr>
                        <td>
                                <telerik:RadLabel ID="RadLabel4" runat="server" Text="Washing pattern employed" style="vertical-align:text-bottom"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtemployed" runat="server" CssClass="input_mandatory" RenderMode="Lightweight" OnTextChanged="txtrefresh_TextChanged" AutoPostBack="true">
                                </telerik:RadTextBox>
                               
                            </td>
                        </tr>

                          <tr>
                            <td>
                                <telerik:RadLabel ID="RadLabel5" runat="server" Text="Washing Line Pressure"></telerik:RadLabel>
                            </td>
                            <td>
                                <%--<eluc:Number ID="txtBfrTrnsROBFrom" runat="server" MaxLength="3" Width="120px"/>--%>

                                <telerik:RadNumericTextBox ID="txtpressure" RenderMode="Lightweight" CssClass="input_mandatory" runat="server" MinValue="0" MaxValue="99999999" Width="120px" OnTextChanged="txtrefresh_TextChanged" AutoPostBack="true" Style="text-align: right;" Type="Number">
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                                <telerik:RadLabel ID="RadLabel6" runat="server" Text="Kg/cm2"></telerik:RadLabel>
                            </td>
                        </tr>

                         <tr>
                        <td>
                                <telerik:RadLabel ID="RadLabel7" runat="server" Text="State method of establishing that the tanks was dry" style="vertical-align:text-bottom"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtstate" runat="server" CssClass="input_mandatory" RenderMode="Lightweight" OnTextChanged="txtrefresh_TextChanged" AutoPostBack="true" >
                                </telerik:RadTextBox>
                               
                            </td>
                        </tr>

                         <tr>
                        <td>
                                <telerik:RadLabel ID="RadLabel8" runat="server" Text="Remarks" style="vertical-align:text-bottom"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtremarks" runat="server" CssClass="input_mandatory" TextMode="MultiLine" Rows="5" Width="350px" RenderMode="Lightweight" OnTextChanged="txtrefresh_TextChanged" AutoPostBack="true" >
                                </telerik:RadTextBox>
                               
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
