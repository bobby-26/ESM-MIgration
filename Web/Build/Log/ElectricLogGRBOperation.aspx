<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogGRBOperation.aspx.cs" Inherits="Log_ElectricLogGRBOperation" %>


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
                width:100%;
            }

            .sub-column-1
            {
                text-align:right;
            }

            .sub-column-2
            {
                text-align:left;
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
            
            <telerik:RadLabel runat="server" ID="lblUrl"></telerik:RadLabel>

            <table cellpadding="2px" cellspacing="0px" style="width: 100%;" id="logmenu">
                <thead>
                    <tr>
                        <td colspan="7">
                            <br />
                            <b>RECORD OF GARBAGE DISCHARGES PART I</b>
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
                    <td>
                        <telerik:radbutton id="btnPlannedDisposal" runat="server" visible="True" Cssclass="btnwidth" text="Planned Disposal"></telerik:radbutton>
                    </td>
                    <td>
                        <telerik:radbutton id="btnExceptionalDischarge" runat="server" Cssclass="btnwidth" visible="True" text="Exceptional Discharge"></telerik:radbutton>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>