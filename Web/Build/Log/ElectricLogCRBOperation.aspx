<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogCRBOperation.aspx.cs" Inherits="Log_ElectricLogCRBOperation" %>

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
            width:110px;
            height:110px;
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
            visibility: hidden;
            
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

                 #logmenu {
                    width: 100%;
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

            #logmenu {
                width: 80%;
                margin:0 auto;
            }
        }
    </style>

</head>
<body>
    <form id="frmgvCounterUpdate" runat="server" submitdisabledcontrols="true">
        <telerik:radscriptmanager runat="server" id="RadScriptManager1" />
        <telerik:radskinmanager id="RadSkinManager1" runat="server" />

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        
        <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" Style="display: none;"/>
        <div style="float: initial; margin: 0 50px">
            
            <table cellpadding="2px" cellspacing="0px" id="logmenu">
                <thead>
                    <tr>
                        <td colspan="7">
                            <br />
                            <b>MARPOL ANNEX II CARGO RECORD BOOK CODES</b>
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
                            <telerik:radlabel id="lblCodeA" runat="server" visible="True" CssClass="fontcolor" text="CODE A"></telerik:radlabel>
                         </b>
                    </td>
                    <td class="column-2">
                        <b>
                            <telerik:radlabel id="lblCodeB" runat="server" visible="True" CssClass="fontcolor" text="CODE B"></telerik:radlabel>
                        </b>
                    </td>
                    <td class="column-3">
                        <b>
                            <telerik:radlabel id="lblCodeC" runat="server" visible="True" CssClass="fontcolor" text="CODE C"></telerik:radlabel>
                        </b>
                    </td>
                    <td class="column-4">
                        <b>
                            <telerik:radlabel id="lblCodeD" runat="server" visible="True" CssClass="fontcolor" text="CODE D"></telerik:radlabel>
                            
                        </b>
                    </td>
                    <td class="column-5">
                        <b>
                            <telerik:radlabel id="lblCodeE" runat="server" visible="True" CssClass="fontcolor" text="CODE E"></telerik:radlabel>
                        </b>
                    </td>
                    <td class="column-6">
                        <b>
                            <telerik:radlabel id="lblCodeF" runat="server" visible="True" CssClass="fontcolor" text="CODE F"></telerik:radlabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td><telerik:radbutton id="btnLoadingCargo" Cssclass="btnwidth" runat="server" visible="True" text="Loading of Cargo"></telerik:radbutton></td>
                    <td><telerik:radbutton id="btnInternalTransferOfOilCargo" Cssclass="btnwidth" runat="server" visible="True" text="Internal Transfer of Annex II cargo"></telerik:radbutton></td>
                    <td><telerik:radbutton id="btnUnloadingOilCargo" Cssclass="btnwidth" runat="server" visible="True" text="Unloading of Oil Cargo"></telerik:radbutton></td>
                    <td><telerik:radbutton id="btnMandatoryPreWash" Cssclass="btnwidth" runat="server" visible="True" text="Mandatory Prewash in Accordance wiht the Ships Procedures and Arrangements Manual"></telerik:radbutton></td>
                    <td><telerik:radbutton id="btnCleaningCargoTank" Cssclass="btnwidth" runat="server" visible="True" text="Cleaning of Cargo Tanks except mandatory prewash (other prewash operations, final wash, ventilation, etc.)"></telerik:radbutton></td>
                    <td><telerik:radbutton id="btnDischargeSea" Cssclass="btnwidth" runat="server" visible="True" text="Discharge into the Sea of tank Washings"></telerik:radbutton></td>
                </tr>
                <tr>
                    <td class="column-1">
                        <b>
                            <telerik:RadLabel ID="lblCodeM" runat="server" Visible="True" CssClass="fontcolor" Text="CODE G"></telerik:RadLabel>
                        </b>
                    </td>
                    <td class="column-2">
                        <b>
                            <telerik:RadLabel ID="lblCodeN" runat="server" Visible="True" CssClass="fontcolor" Text="CODE H"></telerik:RadLabel>
                        </b>
                    </td>
                    <td class="column-3">
                        <b>
                            <telerik:RadLabel ID="lblCodeO" runat="server" Visible="True" CssClass="fontcolor" Text="CODE I"></telerik:RadLabel>
                        </b>
                    </td>
                    <td class="column-4">
                        <b>
                            <telerik:radlabel id="Radlabel1" runat="server" visible="True" CssClass="fontcolor" text="CODE J"></telerik:radlabel>
                            
                        </b>
                    </td>
                    <td class="column-5">
                        <b>
                            <telerik:radlabel id="Radlabel2" runat="server" visible="True" CssClass="fontcolor" text="CODE K"></telerik:radlabel>
                        </b>
                    </td>
                    <td class="column-6">
                        <b>
                            <telerik:radlabel id="Radlabel3" runat="server" visible="false" CssClass="fontcolor" text="Dummy Element"></telerik:radlabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td>  <telerik:radbutton id="btnBallastingCargoTank" Cssclass="btnwidth" runat="server" visible="True" text="Ballasting of Cargo Tanks"></telerik:radbutton></td>
                    <td>  <telerik:radbutton id="btnDischargeBallastWater" Cssclass="btnwidth" runat="server" visible="True" text="Discharge of ballast water from cargo tanks"></telerik:radbutton></td>
                    <td>  <telerik:radbutton id="btnAccidentalDischarge" Cssclass="btnwidth" runat="server" visible="True" text="Accidental or other exceptional discharge"></telerik:radbutton></td>
                    <td>  <telerik:radbutton id="btnControlAuthorized" Cssclass="btnwidth" runat="server" visible="True" text="Control by Authorized Surveyors"></telerik:radbutton></td>
                    <td>  <telerik:radbutton id="btnAdditionalOperational" Cssclass="btnwidth" runat="server" visible="True" text="Additional operational procedures and remarks"></telerik:radbutton></td>
                    <td>  
                        <telerik:radbutton id="Radbutton2" Cssclass="btnwidth dummy-element" runat="server" visible="True" text="Dummt Element"></telerik:radbutton>
                    </td>
                </tr>
            </table>

            <br />
            <br />

            <table cellpadding="2px" cellspacing="0px" style="width: 100%;">
                
            </table>
        </div>
    </form>
</body>
</html>
