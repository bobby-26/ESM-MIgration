<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogOperation.aspx.cs" Inherits="Log_ElectricLogOperation" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <title>Record of Operation</title>
    <telerik:radcodeblock id="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:radcodeblock>

    <style>
        .btnwidth{
            width:100px;
            height:75px;
            margin:  10px;
        }

        tr {
            text-align:center;
        }

        .fontcolor {
            color: royalblue !important;
        }

        /* simulating empty columns with button property*/
        .dummy-element {
            display:block;
            padding:5px 11px; 
            
        }
            /* Desktops and laptops ----------- */
            @media only screen and (min-width : 1224px) {
                .column-1 {
                    width: 250px;
                }

                .column-2 {
                    width: 150px;
                }

                .column-3 {
                    width: 150px;
                }

                .column-4 {
                    width: 150px;
                }

                .column-5 {
                    width: 150px;
                }

                .column-6 {
                    width: 150px;
                }

                .column-7 {
                    width: 250px;
                }

               #logmenu {
                    width: 100%;
                    margin: 0 auto;
                }
            }

            /* Large screens ----------- */
        @media only screen and (min-width : 1824px) {
            .column-1 {
                width: 450px;
            }

            .column-2 {
                width: 250px;
            }

            .column-3 {
                width: 250px;
            }

            .column-4 {
                width: 250px;
            }

            .column-5 {
                width: 250px;
            }

            .column-6 {
                width: 250px;
            }

            .column-7 {
                width: 450px;
            }

            .sub-table {
                width: 100%;
            }

            .sub-column-1 {
                text-align: right;
            }

            .sub-column-2 {
                text-align: left;
            }

            #logmenu {
                width: 80%;
                margin: 0 auto;
            }
        }
    </style>

</head>
<body>
    <form id="frmgvCounterUpdate" runat="server" submitdisabledcontrols="true">
        <telerik:radscriptmanager runat="server" id="RadScriptManager1" />
        <telerik:radskinmanager id="RadSkinManager1" runat="server" />

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        
        <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" />
        <div style="float: initial; margin: 0 50px">
            
            <table cellpadding="2px" cellspacing="0px" id="logmenu">
                <thead>
                    <tr>
                        <td colspan="7">
                            <br />
                            <b>ELECTRONIC OIL RECORD BOOK PART I (MACHINERY SPACE OPERATIONS)</b>
                            <br />
                            <br />
                        </td>
                    </tr>
                </thead>
                <tr>
                    <td></td>
                </tr>
                <tr>
                    <td></td>
                </tr>
                <tr>
                    <td class="column-1">
                         <b>
                            <telerik:radlabel id="lblCodeC" runat="server" visible="True" CssClass="fontcolor" text="CODE C"></telerik:radlabel>
                         </b>
                    </td>
                    <td class="column-2">
                        <b>
                            <telerik:radlabel id="lblCodeD" runat="server" visible="True" CssClass="fontcolor" text="CODE D"></telerik:radlabel>
                        </b>
                    </td>
                    <td class="column-2">
                        <b>
                            <telerik:radlabel id="lblCodeE" runat="server" visible="True" CssClass="fontcolor" text="CODE E"></telerik:radlabel>
                        </b>
                    </td>
                    <td class="column-3">
                        <b>
                            <telerik:radlabel id="lblCodeF" runat="server" visible="True" CssClass="fontcolor" text="CODE F"></telerik:radlabel>
                        </b>
                    </td>
                    <td class="column-4">
                        <b>
                            <telerik:radlabel id="lblCodeG" runat="server" visible="True" CssClass="fontcolor" text="CODE G"></telerik:radlabel>
                        </b>
                    </td>
                    <td class="column-5">
                        <b>
                            <telerik:radlabel id="lblCodeH" runat="server" visible="True" CssClass="fontcolor" text="CODE H"></telerik:radlabel>
                        </b>
                    </td>
                    <td class="column-6">
                        <b>
                            <telerik:radlabel id="lblCodeI" runat="server" visible="True" CssClass="fontcolor" text="CODE I"></telerik:radlabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table class="sub-table">
                            <tr>
                             <td class="sub-column-1">
                                  <telerik:radbutton id="btnWeeklyEntries" runat="server" visible="True" Cssclass="btnwidth" text="Weekly Entries"></telerik:radbutton>
                             </td>
                            <td class="sub-column-2">
                                  <telerik:radbutton id="btnSludgefromERtoDeckCargo" Cssclass="btnwidth" runat="server" visible="True" text="Sludge from ER to Deck/Cargo Slop tank"></telerik:radbutton>
                            </td>
                            </tr>
                        </table>
                   </td>
                    <td>
                        <telerik:radbutton id="btnBilgeWell" runat="server" visible="True" Cssclass="btnwidth" text="Bilge Well"></telerik:radbutton>
                    </td>
                    <td>
                        <telerik:radbutton id="btnAutomaticPumpingBilge" runat="server" visible="True" Cssclass="btnwidth" text="Automatic Pumping of bilge water overboard via 15 ppm equipment from tank listed"></telerik:radbutton>
                    </td>

                    <td>
                        <telerik:radbutton id="btnfailure" runat="server" visible="True" Cssclass="btnwidth" text="Failure of oil filtering equipment,Oil content meter or stopping device"></telerik:radbutton>
                    </td>
                    <td>
                        <telerik:radbutton id="btnAccidentalDischarge" runat="server" visible="True" Cssclass="btnwidth" text="Accidental Discharge or Other exceptional discharges of Oil"></telerik:radbutton>
                    </td>
                    <td>
                        <telerik:radbutton id="btnbunkering" runat="server" Cssclass="btnwidth" visible="True" text="Bunkering Fuel"></telerik:radbutton>
                    </td>
                    <td>
                        <table class="sub-table">
                            <tr>
                                <td class="sub-column-1">
                                    <telerik:radbutton id="btncargobilge" runat="server" Cssclass="btnwidth" visible="True" text="Cargo bilge holding tank to Tank 3.3(ER Bilge)"></telerik:radbutton>
                                </td>
                                <td class="sub-column-2">
                                    <telerik:radbutton id="btnQuarterlyTesting" runat="server" Cssclass="btnwidth" visible="True" text="Quartelry testing of OWS by actual overboard discharge"></telerik:radbutton>
                                </td>
                            </tr>
                        </table>
                        
                    </td>
                </tr>
                <tr>
                     <td>
                        <table class="sub-table">
                            <tr>
                            <td class="sub-column-1">
                                   <telerik:radbutton id="btnSludgeTransfer" Cssclass="btnwidth"  runat="server" visible="True" text="Sludge Transfer"></telerik:radbutton>
                            </td>
                            <td class="sub-column-2">
                                   <telerik:radbutton id="btnWaterDrained" Cssclass="btnwidth"  runat="server" visible="True" text="Water Drained from sludge tank"></telerik:radbutton>
                            </td>
                            </tr>
                        </table>
                   </td>
                    <td>
                        <telerik:radbutton id="btninternalbilge" Cssclass="btnwidth" runat="server"  visible="True" text="Internal Bilge"></telerik:radbutton>
                    </td>
                    <td>
                        <telerik:radbutton id="btnAutomaticTransferBilge" Cssclass="btnwidth" runat="server"  visible="True" text="Automatic Transfer of bilge water from engine-room bilge wells"></telerik:radbutton>
                    </td>
                    <td>
                        <telerik:radbutton id="btnRectification" Cssclass="btnwidth" runat="server" visible="True" text="Rectification of OWS, OCM of stopping device"></telerik:radbutton>
                    </td>
                    <td></td>
                    <td>
                        <telerik:radbutton id="btnbunkerniglube" Cssclass="btnwidth" runat="server" visible="True" text="Bunkering Lube"></telerik:radbutton>
                    </td>
                    <td>
                        <table class="sub-table">
                            <tr>
                                <td class="sub-column-1">
                                    <telerik:radbutton id="btnMissedentry" Cssclass="btnwidth" runat="server" visible="True" text="Missed Operational Entry"></telerik:radbutton>
                                </td>
                                <td class="sub-column-2">
                                    <telerik:radbutton id="btnBilgeMainenaneSepearator" Cssclass="btnwidth" runat="server" visible="True" text="Maintenance of 15ppm bilge separators"></telerik:radbutton>
                                </td>
                            </tr>
                        </table>
                        
                    </td>
                </tr>
                <tr>
                      <td>
                        <table class="sub-table">
                            <tr>
                                  <td class="sub-column-1">
                                      <telerik:radbutton id="btnEvpSludgeTank" Cssclass="btnwidth" runat="server" visible="True" text="Evaporation Sludge Tank"></telerik:radbutton>
                                  </td>
                                  <td class="sub-column-2">
                                      <telerik:radbutton id="btnSludgeBurning" Cssclass="btnwidth" runat="server" visible="True" text="Sludge burning in boiler"></telerik:radbutton>
                                  </td>
                            </tr>
                        </table>
                   </td>
                  
                    <td>
                        <telerik:radbutton id="btnOWSOperation" Cssclass="btnwidth" runat="server" visible="True" text="OWS Operation"></telerik:radbutton>
                    </td>
                    <td colspan="4"></td>
                    <td>
                        <table class="sub-table">
                            <tr>
                                <td class="sub-column-1">
                                    <telerik:RadButton ID="btndebunering" CssClass="btnwidth" runat="server" Visible="True" Text="De-bunkering of fuel oil"></telerik:RadButton>
                                </td>
                                <td class="sub-column-2">
                                    <telerik:RadButton ID="btnMainenanceIncinerator" CssClass="btnwidth" runat="server" Visible="True" Text="Maintenance of Incinerator"></telerik:RadButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table class="sub-table">
                            <tr>
                                <td class="sub-column-1">
                                    <telerik:RadButton ID="btnIncineration" CssClass="btnwidth" runat="server" Visible="True" Text="Incineration"></telerik:RadButton>
                                </td>
                                <td class="sub-column-2">
                                    <span class="btnwidth dummy-element"></span>
                                </td>
                            </tr>
                        </table>
                   </td>
                
                    <td>
                        <telerik:radbutton id="btnbilgetosludge" Cssclass="btnwidth" runat="server" visible="True" text="Bilge to Sludge"></telerik:radbutton>
                    </td>
                    <td colspan="4"></td>
                    <td>
                        <table class="sub-table">
                            <tr>
                                <td class="sub-column-1">
                                    <telerik:radbutton id="btnsealing" Cssclass="btnwidth" runat="server" visible="True" text="Sealing of overborad valve"></telerik:radbutton>
                                </td>
                                <td class="sub-column-2">
                                    <telerik:radbutton id="btnDisposingSludge" Cssclass="btnwidth" runat="server" visible="True" text="Disposing sludge from cleaning of oil tanks"></telerik:radbutton>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table class="sub-table">
                            <tr>
                                  <td class="sub-column-1">
                                     <telerik:radbutton id="btnMnlCollectionIOPPTank" Cssclass="btnwidth" runat="server" visible="True" text="Manual Collection of Oil Sludge to IOPP Tank"></telerik:radbutton>
                                  </td>
                                  <td class="sub-column-2">
                                      <span class="btnwidth dummy-element"></span>
                                  </td>
                            </tr>
                        </table>
                   </td>
                   
                    <td>
                        <telerik:radbutton id="btnBilgeShoreDisposal" Cssclass="btnwidth" runat="server" visible="True" text="Bilge Shore Disposal"></telerik:radbutton>
                    </td>
                    <td colspan="4"></td>
                    <td>
                        <table class="sub-table">
                            <tr>
                                <td class="sub-column-1">
                                    <telerik:radbutton id="btnunsealing" Cssclass="btnwidth" runat="server" visible="True" text="Un-sealing of overborad valve"></telerik:radbutton>
                                </td>
                                <td class="sub-column-2">
                                    <telerik:radbutton id="btnCleaningBilge" Cssclass="btnwidth" runat="server" visible="True" text="Cleaning Bilge Tank"></telerik:radbutton>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    
                    <td>
                        <table class="sub-table">
                            <tr>
                                   <td class="sub-column-1">
                                       <telerik:radbutton id="btnShoreSludgeDisposal" Cssclass="btnwidth" runat="server" visible="True" text="Shore Sludge Disposal"></telerik:radbutton>
                                   </td>
                                  <td class="sub-column-2">
                                      <span class="btnwidth dummy-element"></span>
                                  </td>
                            </tr>
                        </table>
                   </td>
                    <td>
                        <telerik:radbutton id="btnBilgeWaterfrmEr" Cssclass="btnwidth" runat="server" visible="True" text="Bilge water from ER to Deck/Cargo slop tank "></telerik:radbutton>
                    </td>
                    <td colspan="4"></td>
                    <td colspan="7">
                        <table class="sub-table">
                            <tr>
                                <td class="sub-column-1">
                                    <telerik:radbutton id="btnEvaporationFromBilge" Cssclass="btnwidth" runat="server" visible="True" text="Evaporation from Bilge Tank"></telerik:radbutton>
                                </td>
                                <td class="sub-column-2">
                                    <span class="btnwidth dummy-element"></span>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="6"></td>
                    <td>
                        <table class="sub-table">
                            <tr>
                                <td class="sub-column-1">
                                    <telerik:radbutton id="btnMiscellaneousEntry" Cssclass="btnwidth" runat="server" visible="True" text="Miscellaneous Entry"></telerik:radbutton>
                                </td>
                                <td class="sub-column-2">
                                    <span class="btnwidth dummy-element"></span>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
