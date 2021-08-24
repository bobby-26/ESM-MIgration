<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogAnnexureConfig.aspx.cs" Inherits="Log_ElectricLogAnnexureConfig" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>NOX Operation</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    <style>

        :root{
            box-sizing: border-box;
        }

        body {
            box-sizing: inherit;
        }

        .container {
            margin: 0 15%;
        }

        .left {
            width:50%;
            float:left;
        }

        .right {
            width: 25%;
            float:right;
        }

        .ui-control {
            width: 135px !important;
        }

        .ui-label {
            width: 150px;
        }

        .ui-textarea {
            width: 250px !important;
        }

        .ui-attachment {
            margin-left: 30px;
        }

        .ui-header {
            text-align: center;
            text-decoration: underline;
        }

        .ui-table-header {
            text-align: center;
            font-weight: bold;
            background-color:#f1f5fb;
        }

        table {
            border-collapse: collapse;
            text-align:center;
            line-height: 25px;
        }

        td, th {
            border: 1px solid black;
        }

        .vesselParticular {
            width: 350px;
            height: 30px;
            text-align:left;
            margin-bottom: 20px;
        }

        .tank {
            width: 350px;
            height: 30px;
            margin-top: 20px;
        }

        .engine {
            width: 350px;
            height:30px;
            margin-top: 20px;
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:radscriptmanager runat="server" id="RadScriptManager1" />
        <telerik:radskinmanager id="RadSkinManager1" runat="server" />
            <eluc:TabStrip ID="gvTabStrip" runat="server" OnTabStripCommand="gvTabStrip_TabStripCommand"></eluc:TabStrip>
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" Style="display: none;" />
        
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" Visible="false" />

        <div>
            <h1 class="ui-header">This Page is Usually Accessed once before Putting the record books into use by Administrator</h1>

            <div class="container">

               
                <div class="left">
                    <h3>Vessel Particular's</h3>
                    <table class="vesselParticular">
                        <tr>
                            <td class="ui-table-header">Ship Name</td>
                            <td>
                                <telerik:RadLabel runat="server" ID="lblVesselName"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td class="ui-table-header">IMO No</td>
                            <td>
                                <telerik:RadLabel runat="server" ID="lblIMO"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td class="ui-table-header">Ship Type</td>
                            <td>
                                <telerik:RadLabel runat="server" ID="lblShipType"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td class="ui-table-header">Flag</td>
                            <td>
                                <telerik:RadLabel runat="server" ID="lblFlag"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td class="ui-table-header">Company</td>
                            <td>
                                <telerik:RadLabel runat="server" ID="lblCompany"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td class="ui-table-header">Classification</td>
                            <td>
                                <telerik:RadLabel runat="server" ID="lblClassfication"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>


                    <table class="tank" id="tblTankList" runat="server">
                        <thead>
                            <tr>
                                <th class="ui-table-header">FO Tank</th>
                                <th class="ui-table-header">Tank Name</th>
                                <th class="ui-table-header">Tank Capacity</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td class="ui-table-header">#1</td>
                                <td>
                                    <telerik:RadTextBox runat="server" ID="txtTankId1" Visible="false"></telerik:RadTextBox>
                                    <telerik:RadTextBox runat="server" ID="txtTankName1" CssClass="ui-control input_mandatory"></telerik:RadTextBox></td>
                                <td><telerik:RadNumericTextBox runat="server" RenderMode="Lightweight" ID="txtTankCapacity1" CssClass="ui-control input_mandatory"></telerik:RadNumericTextBox></td>
                            </tr>
                            <tr>
                                <td class="ui-table-header">#2</td>
                                
                                <td>
                                    <telerik:RadTextBox runat="server" ID="txtTankId2" Visible="false"></telerik:RadTextBox>
                                    <telerik:RadTextBox runat="server" ID="txtTankName2" CssClass="ui-control input_mandatory"></telerik:RadTextBox>
                                </td>
                                <td><telerik:RadNumericTextBox runat="server" RenderMode="Lightweight" ID="txtTankCapacity2" CssClass="ui-control input_mandatory"></telerik:RadNumericTextBox></td>
                            </tr>
                            <tr>
                                <td class="ui-table-header">#3</td>
                                
                                <td>
                                    <telerik:RadTextBox runat="server" ID="txtTankId3" Visible="false"></telerik:RadTextBox>
                                    <telerik:RadTextBox runat="server" ID="txtTankName3" CssClass="ui-control input_mandatory"></telerik:RadTextBox></td>
                                <td><telerik:RadNumericTextBox runat="server" RenderMode="Lightweight" ID="txtTankCapacity3" CssClass="ui-control input_mandatory"></telerik:RadNumericTextBox></td>
                            </tr>
                            <tr>
                                <td class="ui-table-header">#4</td>
                                
                                <td>
                                    <telerik:RadTextBox runat="server" ID="txtTankId4" Visible="false"></telerik:RadTextBox>
                                    <telerik:RadTextBox runat="server" ID="txtTankName4" CssClass="ui-control input_mandatory"></telerik:RadTextBox>
                                </td>
                                <td><telerik:RadNumericTextBox runat="server" RenderMode="Lightweight" ID="txtTankCapacity4" CssClass="ui-control input_mandatory"></telerik:RadNumericTextBox></td>
                            </tr>
                            <tr>
                                <td class="ui-table-header">#5</td>
                                
                                <td>
                                    <telerik:RadTextBox runat="server" ID="txtTankId5" Visible="false"></telerik:RadTextBox>
                                    <telerik:RadTextBox runat="server" ID="txtTankName5" CssClass="ui-control input_mandatory"></telerik:RadTextBox>
                                </td>
                                <td><telerik:RadNumericTextBox runat="server" RenderMode="Lightweight" ID="txtTankCapacity5" CssClass="ui-control input_mandatory"></telerik:RadNumericTextBox></td>
                            </tr>
                            <tr>
                                <td class="ui-table-header">#6</td>
                                
                                <td>
                                    <telerik:RadTextBox runat="server" ID="txtTankId6" Visible="false"></telerik:RadTextBox>
                                    <telerik:RadTextBox runat="server" ID="txtTankName6" CssClass="ui-control input_mandatory"></telerik:RadTextBox>
                                </td>
                                <td><telerik:RadNumericTextBox runat="server" RenderMode="Lightweight" ID="txtTankCapacity6" CssClass="ui-control input_mandatory"></telerik:RadNumericTextBox></td>
                            </tr>
                        </tbody>
                    </table>

                </div>
                       
                           
                <div class="right">
                           <table class="engine" id="tblEngine" runat="server">
                        <thead>
                            <tr>
                                <td class="ui-table-header">Diesel Engines</td>
                                <td class="ui-table-header">On Board</td>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>
                                    <telerik:RadLabel runat="server" ID="lblEngineName1" Text="Main Engine #1"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel runat="server" ID="lblMainEngineId1" Visible="false"></telerik:RadLabel>
                                    <telerik:RadComboBox runat="server" ID="ddlMainEngineStatus1"></telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel runat="server" ID="lblEngineName2" Text="Main Engine #2"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel runat="server" ID="lblMainEngineId2" Visible="false"></telerik:RadLabel>
                                    <telerik:RadComboBox runat="server" ID="ddlMainEngineStatus2"></telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel runat="server" ID="lblAuxEngineName1" Text="Aux. Engine #1" ></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel runat="server" ID="lblAuxEngineId1" Visible="false"></telerik:RadLabel>
                                    <telerik:RadComboBox runat="server" ID="ddlAuxEngineStatus1"></telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel runat="server" ID="lblAuxEngineName2" Text="Aux. Engine #2" ></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel runat="server" ID="lblAuxEngineId2" Visible="false"></telerik:RadLabel>
                                    <telerik:RadComboBox runat="server" ID="ddlAuxEngineStatus2"></telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel runat="server" ID="lblAuxEngineName3" Text="Aux. Engine #3"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel runat="server" ID="lblAuxEngineId3" Visible="false"></telerik:RadLabel>
                                    <telerik:RadComboBox runat="server" ID="ddlAuxEngineStatus3"></telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel runat="server" ID="lblAuxEngineName4" Text="Aux. Engine #4"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel runat="server" ID="lblAuxEngineId4" Visible="false"></telerik:RadLabel>
                                    <telerik:RadComboBox runat="server" ID="ddlAuxEngineStatus4"></telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel runat="server" ID="lblHarbourGen" Text="Harbour Generator"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel runat="server" ID="lblHarbourId" Visible="false"></telerik:RadLabel>
                                    <telerik:RadComboBox runat="server" ID="ddlHarbourGenerator"></telerik:RadComboBox>
                                </td>
                            </tr>
                        </tbody>
                    </table>

                            <p class="notes">
                                <telerik:RadLabel runat="server" ID="lblNotes"></telerik:RadLabel>
                            </p>
                </div>
            </div>

        </div>
    </form>
</body>
</html>
