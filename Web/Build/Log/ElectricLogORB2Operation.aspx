<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogORB2Operation.aspx.cs" Inherits="Log_ElectricLogORB2Operation" %>

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
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>

    <style>
        .btnwidth {
            width: 100px;
            height: 75px;
            margin:  10px;
        }

        tr {
            text-align: center;
        }

        .fontcolor {
            color: royalblue !important;
        }

        /* simulating empty columns with button property*/
        .dummy-element {
            display: block;
            padding: 5px 11px;
        }
        /* Desktops and laptops ----------- */
        @media only screen and (min-width : 1224px) {
            .column-1 {
                width: 250px;
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
                width: 250px;
            }
        }

        /* Large screens ----------- */
        @media only screen and (min-width : 1824px) {
            .column-1 {
                width: 250px;
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
                width: 250px;
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
        }
    </style>

</head>
<body>
    <form id="frmgvCounterUpdate" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

        <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" Style="display: none;" />
        <div style="float: inherit; margin: 0 50px">
            <table cellpadding="2px" cellspacing="0px" style="width: 90%; position: static;" id="logmenu">
                <thead>
                    <tr>
                        <td colspan="7">
                            <br />
                            <b>MARPOL ANNEX I OIL RECORD BOOK PART II CODES</b>
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
                            <telerik:RadLabel ID="lblCodeA" runat="server" Visible="True" CssClass="fontcolor" Text="CODE A"></telerik:RadLabel>
                        </b>
                    </td>
                    <td class="column-5">
                        <b>
                            <telerik:RadLabel ID="lblCodeE" runat="server" Visible="True" CssClass="fontcolor" Text="CODE E"></telerik:RadLabel>
                        </b>
                    </td>
                    <td class="column-2">
                        <b>
                            <telerik:RadLabel ID="lblCodeI" runat="server" Visible="True" CssClass="fontcolor" Text="CODE I"></telerik:RadLabel>
                        </b>
                    </td>

                    <td class="column-1">
                        <b>
                            <telerik:RadLabel ID="lblCodeM" runat="server" Visible="True" CssClass="fontcolor" Text="CODE M"></telerik:RadLabel>
                        </b>
                    </td>
                    <td class="column-1">
                        <b>
                            <telerik:RadLabel ID="lblCodeOM" runat="server" Visible="True" CssClass="fontcolor" Text="CODE O"></telerik:RadLabel>
                        </b>
                    </td>




                </tr>
                <tr>
                    <td>
                        <telerik:RadButton ID="btnLoadingCargo" CssClass="btnwidth" runat="server" Visible="True" Text="Loading of Cargo"></telerik:RadButton>
                    </td>
                    <td>
                        <telerik:RadButton ID="btnBallastingCargoTanks" CssClass="btnwidth" runat="server" Visible="True" Text="Ballasting of Cargo Tanks"></telerik:RadButton>
                    </td>
                    <td>
                        <telerik:RadButton ID="btnDischargeOfWater" CssClass="btnwidth" runat="server" Visible="True" Text="Discharge of water from Slop Tanks into sea"></telerik:RadButton>
                    </td>


                    <td>
                        <telerik:RadButton ID="btnFailureOfOilDischarge" CssClass="btnwidth" runat="server" Visible="True" Text="Failure of Oil Discharge Monitoring & Control System"></telerik:RadButton>
                    </td>

                    <td>
                        <telerik:RadButton ID="btnMissedOperation" CssClass="btnwidth" runat="server" Visible="True" Text="Missed Operational Entry"></telerik:RadButton>
                    </td>


                </tr>
                <tr>
                    <td class="column-2">
                        <b>
                            <telerik:RadLabel ID="lblCodeB" runat="server" Visible="True" CssClass="fontcolor" Text="CODE B"></telerik:RadLabel>
                        </b>
                    </td>
                    <td class="column-6">
                        <b>
                            <telerik:RadLabel ID="lblCodeF" runat="server" Visible="True" CssClass="fontcolor" Text="CODE F"></telerik:RadLabel>
                        </b>
                    </td>
                    <td class="column-3">
                        <b>
                            <telerik:RadLabel ID="RadlabelJ" runat="server" Visible="True" CssClass="fontcolor" Text="CODE J"></telerik:RadLabel>
                        </b>
                    </td>

                    <td class="column-2">
                        <b>

                        </b>
                    </td>
                    <td class="column-4">
                        <b>
                            <telerik:RadLabel ID="lblCodeP" runat="server" Visible="True" CssClass="fontcolor" Text="CODE P"></telerik:RadLabel>
                        </b>
                    </td>



                </tr>
                <tr>
                  <td>
                        <telerik:RadButton ID="btnInternalTransferOfOilCargo" CssClass="btnwidth" runat="server" Visible="True" Text="Internal Transfer of Oil Cargo During Voyage"></telerik:RadButton>
                    </td>
                    <td>
                        <telerik:RadButton ID="btnBallastDedicatedClean" CssClass="btnwidth" runat="server" Visible="True" Text="Ballasting of Dedicated Clean Ballast Tanks (CBT Tankers Only)"></telerik:RadButton>
                    </td>
                    <td>
                        <telerik:RadButton ID="btnCollectionResidues" CssClass="btnwidth" runat="server" Visible="True" Text="Collection, Transfer and Disposal of Residues and Oily Misture not otherwise dealt with"></telerik:RadButton>
                    </td>

                    <td>
                           <telerik:RadButton ID="btnAfterRepairOilDischarge" CssClass="btnwidth" runat="server" Visible="True" Text="After Repair of Oil Discharge Monitoring & Control System"></telerik:RadButton>                                                        
                    </td>
                    <td>
                        <telerik:RadButton ID="btnLoadingofBallast" CssClass="btnwidth" runat="server" Visible="True" Text="Loading of Ballast Water"></telerik:RadButton>
                    </td>


                </tr>
                <tr>
                    <td class="column-3">
                        <b>
                            <telerik:RadLabel ID="lblCodeC" runat="server" Visible="True" CssClass="fontcolor" Text="CODE C"></telerik:RadLabel>
                        </b>
                    </td>
                    <td class="column-7">
                        <b>
                            <telerik:RadLabel ID="lblCodeG" runat="server" Visible="True" CssClass="fontcolor" Text="CODE G"></telerik:RadLabel>
                        </b>
                    </td>
                    <td class="column-4">
                        <b>
                            <telerik:RadLabel ID="RadlabelK" runat="server" Visible="True" CssClass="fontcolor" Text="CODE K"></telerik:RadLabel>
                        </b>
                    </td>
                    <td class="column-2">
                        <b>
                            <telerik:RadLabel ID="lblCodeN" runat="server" Visible="True" CssClass="fontcolor" Text="CODE N"></telerik:RadLabel>
                        </b>
                    </td>
                    <td class="column-5">
                        <b>
                            <telerik:RadLabel ID="lblCodeQ" runat="server" Visible="True" CssClass="fontcolor" Text="CODE Q"></telerik:RadLabel>
                        </b>
                    </td>


                </tr>
                <tr>
                   <td>
                        <telerik:RadButton ID="btnUnloadingOilCargo" CssClass="btnwidth" runat="server" Visible="True" Text="Unloading of Oil Cargo"></telerik:RadButton>
                    </td>
                    <td>
                        <telerik:RadButton ID="btnCleaningCargo" CssClass="btnwidth" runat="server" Visible="True" Text="Cleaning of Cargo Tanks"></telerik:RadButton>
                    </td>
                    <td>
                        <telerik:RadButton ID="btnDischargeOfCleanBallast" CssClass="btnwidth" runat="server" Visible="True" Text="Discharge of Clean Ballast Contained in Cargo Tanks"></telerik:RadButton>
                    </td>
                    <td>
                        <telerik:RadButton ID="btnAccidentalDischarge" CssClass="btnwidth" runat="server" Visible="True" Text="Accidental or Other Exceptional Discharge of Oil"></telerik:RadButton>
                    </td>

                    <td>
                        <telerik:RadButton ID="btnReallocationBallast" CssClass="btnwidth" runat="server" Visible="True" Text="Re-allocation of Ballast Water within the Ship"></telerik:RadButton>
                    </td>


                </tr>
                <tr>
                  <td class="column-4">
                        <b>
                            <telerik:RadLabel ID="lblCodeD" runat="server" Visible="True" CssClass="fontcolor" Text="CODE D"></telerik:RadLabel>

                        </b>
                    </td>
                    <td class="column-1">
                        <b>
                            <telerik:RadLabel ID="lblCodeH" runat="server" Visible="True" CssClass="fontcolor" Text="CODE H"></telerik:RadLabel>

                        </b>
                    </td>
                    <td class="column-5">
                        <b>
                            <telerik:RadLabel ID="lblCodeL" runat="server" Visible="True" CssClass="fontcolor" Text="CODE L"></telerik:RadLabel>
                        </b>
                    </td>
                    <td class="column-3">
                        <b>
                            <telerik:RadLabel ID="lblCodeO" runat="server" Visible="True" CssClass="fontcolor" Text="CODE O"></telerik:RadLabel>
                        </b>
                    </td>
                    
                    <td class="column-6">
                        <b>
                            <telerik:RadLabel ID="lblCodeR" runat="server" Visible="True" CssClass="fontcolor" Text="CODE R"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadButton ID="btnCrudeOilWashing" CssClass="btnwidth" runat="server" Visible="True" Text="Crude Oil Washing (COW Tankers only)"></telerik:RadButton>
                    </td>
                    <td>
                        <telerik:RadButton ID="btnDischargeOfDirtyBallast" CssClass="btnwidth" runat="server" Visible="True" Text="Discharge of Dirty Ballast"></telerik:RadButton>
                    </td>
                    <td>
                        <telerik:RadButton ID="btnDischargeOfBallastDedicated" CssClass="btnwidth" runat="server" Visible="True" Text="Discharge of Ballast from Dedicated Clean Ballast Tanks (CBT Tankers only)"></telerik:RadButton>
                    </td>
                    <td>
                        <telerik:RadButton ID="btnAdditionalOperationProcedure" CssClass="btnwidth" runat="server" Visible="True" Text="Additional Operational Procedures and General"></telerik:RadButton>
                    </td>
                    <td>
                        <telerik:RadButton ID="btnBallastReceptionFacility" CssClass="btnwidth" runat="server" Visible="True" Text="Ballast Water Discharge to Reception Facility"></telerik:RadButton>
                    </td>
                </tr>
                <tr>


                </tr>
                <tr>


                </tr>
                <tr>

                </tr>
                <tr>
                </tr>
                <tr>
                </tr>
                <tr>

                </tr>

            </table>
            <br />
            <table cellpadding="2px" cellspacing="0px" style="width: 71%; position: static;" id="logmenu1">
                <tr>
                    <td class="column-6">
                        <span class="dummy-element"></span>
                    </td>
                    <td class="column-7">
                        <span class="dummy-element"></span>
                    </td>
                </tr>
                <tr>

                    <td></td>
                    <td></td>
                </tr>
            </table>
            <br />
            <table cellpadding="2px" cellspacing="0px" style="width: 82%; position: static;" id="logmenu2">
                <tr>
                    <td class="column-7">
                        <span class="dummy-element"></span>
                    </td>
                </tr>
                <tr>

                </tr>
                <tr>
                    <td>

                    </td>
                    <td></td>

                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
