<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogORB2FailureOfOilDischarge.aspx.cs" Inherits="Log_ElectricLogORB2FailureOfOilDischarge" %>


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
    <title>Failure of ODME</title>
    <telerik:radcodeblock id="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
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
            <eluc:TabStrip ID="gvTabStrip" runat="server" OnTabStripCommand="MenuOperationApproval_TabStripCommand"></eluc:TabStrip>
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" Style="display: none;" />
      
            <div style="width: 100%; overflow: auto;">
                <div style="float: initial; margin: 0 50px">

                    <h3>Failure of ODME</h3>

                    <table>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="RadLabel1" runat="server" Text="Date of Operation"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadDatePicker RenderMode="Lightweight" runat="server" CssClass="input_mandatory" AutoPostBack="true" ID="txtOperationDate" MaxLength="12" OnSelectedDateChanged="txtOperationDate_TextChangedEvent" >
                                    <DateInput ID="DateInput1" DateFormat="dd-MMM-yyyy" runat="server"></DateInput>
                                </telerik:RadDatePicker>
                            </td>

                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblFailureTime" Text="Failure Time" runat="server"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTimePicker ID="txtFailureTime" runat="server" CssClass="input_mandatory" AutoPostBack="true" OnSelectedDateChanged="txtFailureTime_SelectedDateChanged">
                                    <TimeView ID="TimeView1" Interval="00:30:00" runat="server"></TimeView>
                                </telerik:RadTimePicker>
                                <telerik:RadLabel ID="Radlabel6" runat="server" Text="HH:MM"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadLabel ID="RadLabel5" Text="UTC" runat="server"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTimePicker ID="txtUTC" runat="server" CssClass="input_mandatory" AutoPostBack="true" OnSelectedDateChanged="txtFailureTime_SelectedDateChanged">
                                    <TimeView ID="TimeView2" Interval="00:30:00" runat="server"></TimeView>
                                </telerik:RadTimePicker>
                                <telerik:RadLabel ID="Radlabel8" runat="server" Text="HH:MM"></telerik:RadLabel>
                            </td>
                        </tr>

                          <tr>
                            <td>
                                <telerik:RadLabel ID="lblFailureReason" runat="server" Text="Reason of Failure"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtFailureReason" RenderMode="Lightweight" TextMode="MultiLine" Rows="4" Width="200px" AutoPostBack="true" CssClass="input_mandatory" runat="server"  OnTextChanged="txtFailureReason_TextChanged">
                                </telerik:RadTextBox>
                            </td>
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
