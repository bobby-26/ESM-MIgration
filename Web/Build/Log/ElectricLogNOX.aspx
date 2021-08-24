<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogNOX.aspx.cs" Inherits="Log_ElectricLogNOX" %>

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
    <title>NOX Operation</title>
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
        .container {
            padding: 3%;
        }

        .ui-control {
            width: 100px !important;
        }

        .ui-label {
            width: 100px !important;
        }

        .ui-textarea {
            width: 250px !important;
        }

        .ui-attachment {
            margin-left: 30px;
        }

        .ui-header {
            text-align:center;
            text-decoration:underline;
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
                <table runat="server" id="tblEntry">
                    <tr>
                        <td>                            
                            <telerik:RadLabel runat="server" ID="lblstatusschange" Text="Status Change" ></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox runat="server" ID="ddlStatus" Width="150px" CssClass="input_mandatory"></telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>

                        <td class="ui-label">Date / Time</td>
                        <td>
                            <telerik:RadDateTimePicker RenderMode="Lightweight" ID="dtPicker" Width="100%" runat="server" CssClass="input_mandatory">
                                <TimeView TimeFormat="HH:mm:ss" runat="server">
                                </TimeView>
                                <DateInput DisplayDateFormat="dd-MM-yyyy HH:mm:ss" runat="server">
                                </DateInput>
                            </telerik:RadDateTimePicker>
                        </td>
                    </tr>
                    <tr>
                        <td class="ui-label">Latitude</td>
                        <td>
                            <eluc:Latitude runat="server" ID="txtLatitude" CssClass="input_mandatory" />
                        </td>
                    </tr>
                    <tr>
                        <td class="ui-label">Longitude</td>
                        <td>
                            <eluc:Longitude runat="server" ID="txtLongitude" CssClass="input_mandatory" />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                </table>

                <table runat="server" id="tblEngineDetails">
                    <thead>
                        <tr>
                            <td>
                                <telerik:RadLabel runat="server" ID="lblDieselEngine" Text="Diesel Engine" CssClass="ui-header"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadLabel runat="server" ID="RadLabel1" Text="Tier II & III" CssClass="ui-header"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadLabel runat="server" ID="RadLabel2" Text="Status" CssClass="ui-header"></telerik:RadLabel>
                            </td>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>
                                <telerik:RadLabel runat="server" ID="lblMainEngine1" Text="Main Engine 1" Visible="false" CssClass="ui-control"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox runat="server" ID="ddlTier1" Visible="false" CssClass="ui-control input_mandatory"></telerik:RadComboBox>
                            </td>
                            <td>
                                <telerik:RadComboBox runat="server" ID="ddlEngineStatus1" Visible="false" CssClass="ui-control input_mandatory"></telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel runat="server" ID="lblMainEngine2" Text="Main Engine 2" Visible="false" CssClass="ui-control"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox runat="server" ID="ddlTier2" Visible="false" CssClass="ui-control input_mandatory"></telerik:RadComboBox>
                            </td>
                            <td>
                                <telerik:RadComboBox runat="server" ID="ddlEngineStatus2" Visible="false" CssClass="ui-control input_mandatory"></telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel runat="server" ID="lblAuxEngine1" Text="Aux. Eng. 1" Visible="false" CssClass="ui-control"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox runat="server" ID="ddlAuxTier1" Visible="false" CssClass="ui-control input_mandatory"></telerik:RadComboBox>
                            </td>
                            <td>
                                <telerik:RadComboBox runat="server" ID="ddlAuxEngineStatus1" Visible="false" CssClass="ui-control input_mandatory"></telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel runat="server" ID="lblAuxEngine2" Text="Aux. Eng. 2" Visible="false" CssClass="ui-control"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox runat="server" ID="ddlAuxTier2" Visible="false" CssClass="ui-control input_mandatory"></telerik:RadComboBox>
                            </td>
                            <td>
                                <telerik:RadComboBox runat="server" ID="ddlAuxEngineStatus2" Visible="false" CssClass="ui-control input_mandatory"></telerik:RadComboBox>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <telerik:RadLabel runat="server" ID="lblAuxEngine3" Text="Aux. Eng. 3" Visible="false" CssClass="ui-control"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox runat="server" ID="ddlAuxTier3" Visible="false" CssClass="ui-control input_mandatory"></telerik:RadComboBox>
                            </td>
                            <td>
                                <telerik:RadComboBox runat="server" ID="ddlAuxEngineStatus3" Visible="false" CssClass="ui-control input_mandatory"></telerik:RadComboBox>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <telerik:RadLabel runat="server" ID="lblAuxEngine4" Text="Aux. Eng. 4" Visible="false" CssClass="ui-control"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox runat="server" ID="ddlAuxTier4" Visible="false" CssClass="ui-control input_mandatory"></telerik:RadComboBox>
                            </td>
                            <td>
                                <telerik:RadComboBox runat="server" ID="ddlAuxEngineStatus4" Visible="false" CssClass="ui-control input_mandatory"></telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel runat="server" ID="lblHarbour1" Text="Harbour Generator" Visible="false" CssClass="ui-control"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox runat="server" ID="ddlHarbourTier1" Visible="false" CssClass="ui-control input_mandatory"></telerik:RadComboBox>
                            </td>
                            <td>
                                <telerik:RadComboBox runat="server" ID="ddlHarbourStatus1" Visible="false" CssClass="ui-control input_mandatory"></telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                        <td>Officer Incharge</td>
                        <td><asp:LinkButton runat="server" AlternateText="Incharge Sign"
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
                    <telerik:RadLabel runat="server" ID="lblnotes" 
                        Text="NO2 TIER STATUS LOG <br/> 1) The tier and on/ off status of marine diesel engines installed on board a ship to which paragraph 5.1 of MARPOL Annex VI Regulation 13 applies which are certified to both Tier II and Tier III or which are certified to Tier II only shall be recorded at entry into and exit from an emission." ></telerik:RadLabel>
                </div>


            </div>


        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
