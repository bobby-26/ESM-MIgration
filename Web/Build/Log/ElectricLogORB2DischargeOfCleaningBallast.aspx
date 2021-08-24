<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogORB2DischargeOfCleaningBallast.aspx.cs" Inherits="Log_ElectricLogORB2DischargeOfCleaningBallast" %>


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
    <title>Discharge of Clean Ballast contained in Cargo Tanks</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    <style>
        .style_one {
            border: 1px solid black;
            border-collapse: collapse;
        }

        .right {
            display:inline-block;
            text-align:right;
            width: 100%;
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
                <div style="float: initial; margin: 0 50px">

                    <h3>Discharge of Clean Ballast contained in Cargo Tanks</h3>

                    <table>
                        <tr>
                            <td style="width:250px">
                                <telerik:RadLabel ID="RadLabel1" runat="server" Text="Date of Operation"></telerik:RadLabel>
                            </td>
                            <td style="width:300px">
                                <telerik:RadDatePicker RenderMode="Lightweight" runat="server" CssClass="input_mandatory" AutoPostBack="true" ID="txtOperationDate" MaxLength="12" OnSelectedDateChanged="txtOperationDate_SelectedDateChanged">
                                    <DateInput ID="DateInput1" DateFormat="dd-MMM-yyyy" runat="server"></DateInput>
                                </telerik:RadDatePicker>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="RadLabel4" runat="server" Text="Identity of  Tank Discharged"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox DropDownPosition="Static" CssClass="input_mandatory" ID="ddlTransferFrom" runat="server" AutoPostBack="true" DataTextField="FLDNAME" OnSelectedIndexChanged="ddlWaterTransferedFrom_SelectedIndexChanged" DataValueField="FLDLOCATIONID" EnableLoadOnDemand="True"
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblDischargeStartTime" runat="server" Text="Discharge Start Time"></telerik:RadLabel>
                            </td>
                            <td>

                                <telerik:RadTimePicker ID="txtDischargeStartTime" runat="server" CssClass="input_mandatory" AutoPostBack="true" OnSelectedDateChanged="txtOperationDate_SelectedDateChanged">
                                    <TimeView Interval="00:30:00"></TimeView>
                                </telerik:RadTimePicker>
                                <telerik:RadLabel ID="Radlabel2" runat="server" Text="HH:MM"></telerik:RadLabel>
                            </td>

                            <td>
                               <span class="right"> <telerik:RadLabel ID="lblStartPosistion" runat="server" Visible="true" Text="Start Position"></telerik:RadLabel></span>
                            </td>
                            <td>
                                <eluc:Latitude ID="txtStartPosistionLat" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td>
                                <eluc:Longitude ID="txtStartPosistionLog" runat="server" CssClass="input_mandatory" />
                            </td>
                            <%-- <td>
                                <telerik:RadLabel runat="server" ID="lblStartPos" Text="( xx Deg xx Min N/S, xx Deg xx Min E/W)"></telerik:RadLabel>
                            </td>--%>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblDischargeStopTime" runat="server" Text="Discharge Stop Time"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTimePicker ID="txtDischargeStopTime" runat="server" AutoPostBack="true" CssClass="input_mandatory" OnSelectedDateChanged="txtOperationDate_SelectedDateChanged">
                                    <TimeView Interval="00:30:00"></TimeView>
                                </telerik:RadTimePicker>
                                <telerik:RadLabel ID="Radlabel3" runat="server" Text="HH:MM"></telerik:RadLabel>
                            </td>


                            
                        </tr>

                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblUTCStartTime" runat="server" Text="UTC Start Time"></telerik:RadLabel>
                            </td>
                            <td>

                                <telerik:RadTimePicker ID="txtUTCStartTime" runat="server" CssClass="input_mandatory" AutoPostBack="true" OnSelectedDateChanged="txtOperationDate_SelectedDateChanged">
                                    <TimeView Interval="00:30:00"></TimeView>
                                </telerik:RadTimePicker>
                                <telerik:RadLabel ID="Radlabel16" runat="server" Text="HH:MM"></telerik:RadLabel>
                            </td>
                            <td>
                                <span class="right"><telerik:RadLabel ID="lblStopPosition" runat="server" Visible="true" Text="Stop Position"></telerik:RadLabel></span>
                            </td>
                            <td>
                                <eluc:Latitude ID="txtStopPosistionLat" runat="server" CssClass="input_mandatory" OnSelectedDateChanged="txtOperationDate_SelectedDateChanged" />
                            </td>
                            <td>
                                <eluc:Longitude ID="txtStopPosistionLog" runat="server" CssClass="input_mandatory" OnSelectedDateChanged="txtOperationDate_SelectedDateChanged" />
                            </td>
                            <%--<td>
                                <telerik:RadLabel runat="server" ID="lblStopUnit" Text="( xx Deg xx Min N/S, xx Deg xx Min E/W)"></telerik:RadLabel>
                            </td>--%>
                        </tr>

                        <tr>
                            <td>
                                <telerik:RadLabel ID="RadLabel17" runat="server" Text="UTC Stop Time"></telerik:RadLabel>
                            </td>
                            <td>

                                <telerik:RadTimePicker ID="txtUTCStopTime" runat="server" CssClass="input_mandatory" AutoPostBack="true" OnSelectedDateChanged="txtOperationDate_SelectedDateChanged">
                                    <TimeView Interval="00:30:00"></TimeView>
                                </telerik:RadTimePicker>
                                <telerik:RadLabel ID="Radlabel18" runat="server" Text="HH:MM"></telerik:RadLabel>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>

                        <tr>
                            <td>
                                <telerik:RadLabel ID="RadLabel7" runat="server" Text="Was the tank empty on completion" Style="vertical-align: text-bottom"></telerik:RadLabel>
                            </td>
                            <td>
<%--                                <telerik:RadTextBox ID="txtTankEmpty" runat="server" CssClass="input_mandatory" RenderMode="Lightweight" AutoPostBack="true" OnTextChanged="txtOperationDate_SelectedDateChanged">
                                </telerik:RadTextBox>
                                <telerik:RadLabel ID="RadLabel12" runat="server" Text="Yes/No"></telerik:RadLabel>--%>
                                <asp:RadioButtonList ID="txtTankEmpty" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" Width="100px" AutoPostBack="true" OnSelectedIndexChanged="txtTankEmpty_SelectedIndexChanged" >
                                    <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="No"></asp:ListItem>     
                                </asp:RadioButtonList>
                            </td>

                            <td style="width: 200px;">
                                <telerik:RadLabel ID="RadLabel13" runat="server" Text="Was a regular check carried out on the effluent and the surface of the water in the locality of the discharge?" Style="vertical-align: text-bottom"></telerik:RadLabel>
                            </td>
                            <td>
<%--                                <telerik:RadTextBox ID="txtRegularCheck" runat="server" Width="100px" CssClass="input_mandatory" RenderMode="Lightweight" AutoPostBack="true" OnTextChanged="txtOperationDate_SelectedDateChanged">
                                </telerik:RadTextBox>
                                <telerik:RadLabel ID="RadLabel14" runat="server" Text="Yes/No"></telerik:RadLabel>--%>
                                <asp:RadioButtonList ID="txtRegularCheck" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" Width="100px" AutoPostBack="true" OnSelectedIndexChanged="txtTankEmpty_SelectedIndexChanged" >
                                    <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="No"></asp:ListItem>     
                                </asp:RadioButtonList>
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
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="txtItemNo1" runat="server"></telerik:RadLabel>
                                </td>
                                <td class="style_one">
                                    <telerik:RadLabel ID="lblrecord1" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="txtItemNo2" runat="server"></telerik:RadLabel>
                                </td>
                                <td class="style_one">
                                    <telerik:RadLabel ID="lblrecord2" runat="server"></telerik:RadLabel>
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
                                        ToolTip="Chief Officer Sign" Width="20PX" Height="20PX">
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

