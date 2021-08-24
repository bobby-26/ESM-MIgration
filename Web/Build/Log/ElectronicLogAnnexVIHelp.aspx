<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectronicLogAnnexVIHelp.aspx.cs" Inherits="Log_ElectronicLogAnnexVIHelp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Help</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <div>
                <table id="odstb" runat="server" visible="false">
                    <tr>
                        <th>
                            <b>Note:</b>
                        </th>
                        <th></th>
                    </tr>
                    <tr>
                        <td></td>
                        <td>1. Entries in the Ozone Depleting Substances Record Book are to be recorded in terms of mass (kg) of the substance and are to be completed without delay on each occasion, with respect of the following:
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>(a) Recharge, full or partial, of equipment containing ozone depleting substances.
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>(b) Repair or maintenance of equipment containing ozone depleting substances.
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>(c-1) deliberate, and (c-2) non-deliberate.
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>(d) Discharge of ozone depleting substances to land-based reception facilities. 
                        </td>
                    </tr>
                </table>

                <table id="ep" runat="server" visible="false">
                    <tr>
                        <th>
                            <b>Note:</b>
                        </th>
                        <th></th>
                    </tr>
                    <tr>
                        <td></td>
                        <td>1) Any data existing in the Nox Technical file regarding adjustment of Engine Parameters shall be transferred to this electronic record book. Scan a copy of the paper version (with all relevant signatures) and upload as attachment to the entry transferred.
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>2) The number of tables shown here reflects the number of diesel engines onboard, configured by the Administrator before first use of this record book. If these do not match those actually on board, please report this immediately.
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>3) A hardcopy of these records can also be found in the NOx Technical File (MARPOL Annex VI, and Nox Technical Code, EIAPP Certificate).
                        </td>
                    </tr>
                </table>
            </div>
        </telerik:RadAjaxPanel>
        <div>
        </div>
    </form>
</body>
</html>
