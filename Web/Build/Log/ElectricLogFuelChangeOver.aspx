<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogFuelChangeOver.aspx.cs" Inherits="Log_ElectricLogFuelChangeOver" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Latitude" Src="~/UserControls/UserControlLatitude.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Longitude" Src="~/UserControls/UserControlLongitude.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Fuel Change Over</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    <style>
        .container {
            padding: 3%;
        }

        .ui-control {
            width: 125px !important;
        }

        .ui-label {
            width: 150px;
        }

        .ui-header {
            text-align: center;
            text-decoration: underline;
        }

        table {
            line-height: 1em;
        }

        th {
            text-align: center;
        }

        .ui-degree {
            width: 125px !important;
        }

        .beforeAfter, .entryExit, .startComplted, .signature, .machinery {
            margin-top: 1.5em;
        }

        .left {
            width: 65%;
            float: left;
        }

        .right {
            width: 30%;
            float: right;
        }

        .clearfloat {
            clear: both;
        }
    </style>


</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager runat="server" RenderMode="Lightweight" ID="RadWindowManager1"></telerik:RadWindowManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <telerik:RadAjaxPanel runat="server" ID="radAjaxPanel">

            <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
            <eluc:Message ID="ucMessage" runat="server" Text="" Visible="false"></eluc:Message>
            <eluc:TabStrip ID="gvTabStrip" runat="server" OnTabStripCommand="gvTabStrip_TabStripCommand"></eluc:TabStrip>
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" Style="display: none;" />

            <div class="container">

                <div class="left">

                    <table runat="server" id="tblEntry" class="entryExit">
                        <tr>
                            <td class="ui-label">Entry / Exit</td>
                            <td>
                                <telerik:RadComboBox runat="server" ID="ddlEntry" CssClass="ui-control input_mandatory"></telerik:RadComboBox>
                            </td>
                        </tr>
                    </table>

                    <table runat="server" id="tblChangeOver" class="beforeAfter">
                        <thead>
                            <tr>
                                <th></th>
                                <th>Before</th>
                                <th>After</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td class="ui-label">Type</td>
                                <td>
                                    <telerik:RadTextBox runat="server" ID="txtTypeBefore" CssClass="ui-control input_mandatory"></telerik:RadTextBox></td>
                                <td>
                                    <telerik:RadTextBox runat="server" ID="txtTypeAfter" CssClass="ui-control input_mandatory"></telerik:RadTextBox></td>
                            </tr>
                            <tr>
                                <td class="ui-label">Sulphur</td>
                                <td>
                                    <telerik:RadNumericTextBox runat="server" ID="txtSulphurBefore" CssClass="ui-control input_mandatory"></telerik:RadNumericTextBox></td>
                                <td>
                                    <telerik:RadNumericTextBox runat="server" ID="txtSulphurAfter" CssClass="ui-control input_mandatory"></telerik:RadNumericTextBox></td>
                            </tr>
                        </tbody>
                        <tr>
                            <td class="ui-label">BDN</td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtBDNBefore" CssClass="ui-control input_mandatory"></telerik:RadTextBox></td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtBDNAfter" CssClass="ui-control input_mandatory"></telerik:RadTextBox></td>
                        </tr>
                    </table>

                    <table class="startComplted">
                        <thead>
                            <tr>
                                <th></th>
                                <th>Start Change Over</th>
                                <th>Completed Change Over</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td class="ui-label">Date</td>
                                <td>
                                    <telerik:RadDatePicker runat="server" ID="dtDateTimeStart" CssClass="ui-control input_mandatory"></telerik:RadDatePicker>
                                </td>
                                <td>
                                    <telerik:RadDatePicker runat="server" ID="dtDateTimeCompleted" CssClass="ui-control input_mandatory"></telerik:RadDatePicker>
                                </td>
                              
                            </tr>
                             <tr>
                                <td class="ui-label">Time</td>
                                <td>
                                    <telerik:RadTimePicker ID="txtStartTime" CssClass="ui-control input_mandatory" runat="server">
                                     </telerik:RadTimePicker>
                                </td>
                                <td>
                                    <telerik:RadTimePicker ID="txtCompletedTime" CssClass="ui-control input_mandatory" runat="server">
                                     </telerik:RadTimePicker>
                                </td>
                            </tr>
                            <tr>
                                <td class="ui-label">Position</td>
                                <td>
                                    <telerik:RadTextBox runat="server" ID="txtStartPosistion" CssClass="input_mandatory ui-degree" />
                                </td>
                                <td>
                                    <telerik:RadTextBox runat="server" ID="txtCompletedPosistion" CssClass="input_mandatory ui-degree" />
                                </td>
                                <td>
                                    
                                </td>
                            </tr>
                        </tbody>
                    </table>

                    <table class="entryExit">
                        <thead>
                            <tr>
                                <th></th>
                                <th>Entry Intro / Exit From ECA</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td class="ui-label">Date </td>
                                <td>
                                    <telerik:RadDatePicker runat="server" ID="dtEntryExitDate" CssClass="ui-control input_mandatory"></telerik:RadDatePicker>
                                </td>
                              
                            </tr>
                            <tr>
                                <td class="ui-label">Time</td>
                                <td>
                                    <telerik:RadTimePicker ID="txtEntryExitTime" CssClass="ui-control input_mandatory" runat="server">
                                     </telerik:RadTimePicker>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Position
                                </td>
                                  <td>
                                    <telerik:RadTextBox runat="server" ID="txtEntryPosistion" CssClass="input_mandatory ui-degree" />
                                </td>
                            </tr>
                        </tbody>
                    </table>

                    <table class="machinery">
                        <tbody>
                            <tr>
                                <td class="ui-label">Machinery Which Had FO Change Over</td>
                                <td>
                                    <telerik:RadTextBox runat="server" ID="txtMachinery" CssClass="ui-control input_mandatory"></telerik:RadTextBox></td>
                            </tr>
                        </tbody>
                    </table>

                </div>

                <div class="right">
                    <h2>Tank Details</h2>

                    <asp:Repeater ID="rptrTank" runat="server">
                        <HeaderTemplate>
                            <table class="tblTankDetails">
                                <thead>
                                    <tr>
                                        <th>Tank Name</th>
                                        <th>Capacity</th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="tblrowcolor">
                                <td>
                                    <telerik:RadLabel runat="server" ID="lblROBId" Visible="false" Text='<%#Eval("FLDROBID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel runat="server" ID="lblTankId" Visible="false" Text='<%#Eval("FLDTANKID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel runat="server" ID="lblTankName" Text='<%#Eval("FLDTANKNAME") %>'></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadNumericTextBox runat="server" ID="txtTankROB" Text='<%#Eval("FLDROB") %>'></telerik:RadNumericTextBox>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </tbody>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>

                <div class="clearfloat">

                    <table class="signature">
                        <tbody>

                            <tr>
                                <td class="ui-label">Officer Incharge</td>
                                <td>
                                    <asp:LinkButton runat="server" AlternateText="Incharge Sign"
                                        CommandName="INCHARGESIGN" ID="btnInchargeSign" OnClick="btnInchargeSign_Click"
                                        ToolTip="Incharge Sign" Width="20PX" Height="20PX">
                                         <span class="icon"> <i class="fas fa-file-signature"></i></span>
                                    </asp:LinkButton>
                                    <telerik:RadLabel ID="lblinchId" runat="server" Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblinchName" runat="server" Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblincRank" runat="server" Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblincSignDate" runat="server" Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblincsign" runat="server" Visible="false"></telerik:RadLabel>
                                </td>
                            </tr>
                        </tbody>
                    </table>

                    <div class="notes">
                        <p><b>Notes:</b></p>
                        <telerik:RadLabel runat="server" ID="lblnotes"></telerik:RadLabel>
                    </div>

                </div>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

