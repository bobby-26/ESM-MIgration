<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionDeficiencyReport.aspx.cs"
    Inherits="InspectionDeficiencyReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Custom" Src="~/UserControls/UserControlEditor.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>View Mode</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </div>
    <style type="text/css">
        p
        {
            margin: 0;
            padding: 0;
            text-align: left;
            max-width: 950px;
            max-width: 900px\9;
            display: block;
        }
        table
        {
            table-layout: fixed; /*border: 1px solid #f00;*/
            word-wrap: break-word;
            max-width: 950px;
            max-width: 900px\9;
            margin: 0;
            padding: 0;
        }
        img
        {
            table-layout: fixed;
            max-width: 950px;
            max-width: 900px\9;
            margin: 0;
            padding: 0;
            display: block;
        }
        span
        {
            margin: 0;
            padding: 0;
            word-wrap: break-word;
            max-width: 950px;
            max-width: 900px\9; *zoom:1;*display:inline;}
        span span
        {
            margin: 0;
            padding: 0;
            word-wrap: break-word;
            max-width: 950px;
            max-width: 900px\9; *zoom:1;*display:inline;}</style>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager2" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="navigation" id="Div2" style="top: 0px; margin-left: 0px; vertical-align: text-top;
        position: static; height: auto; width: 100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <div class="subHeader">
            <div class="subHeader">
                <eluc:Title runat="server" ID="ttlContent" Text="Deficiency" ShowMenu="false"></eluc:Title>
                <%--<asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />--%>
            </div>
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
            <eluc:TabStrip ID="MenuClose" runat="server" OnTabStripCommand="MenuClose_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <%--<img id="imgSearch" runat="server" src="<%$ PhoenixTheme:images/icon_print.png %>" onclick="printPreviewDiv(document.getElementById('divForm'));return false;"
                    style="cursor: pointer; vertical-align: middle; padding-bottom: 3px; width:25px; height:25px;" />--%>
    </div>
    <br />
    <br />
    <br />
    <div id="divForm" runat="server" style="padding-left: 2%; display: inline-block;
        width: 100%; table-layout: fixed;">
        <%--<span id="span1" runat="server" title="View" style="display: inline-block;"></span>--%>
        <table id="tblDeficiency" runat="server">
            <tr style="text-align: left; margin: 0px; font-size: 10pt; font-weight: bold;">
                <td id="td1" runat="server" colspan="4" style="text-align: left;">
                    <asp:Literal ID="lblCompanyName" runat="server"></asp:Literal>
                </td>
                <td id="td2" runat="server" colspan="4" style="text-align: right;">
                    <asp:Literal ID="lblCode" runat="server" Text="AI 2 – FORM B – Rev 0 – 10/15"></asp:Literal>
                </td>
            </tr>
            <tr style="text-align: center; margin: 0px; font-size: 12pt; font-weight: bold; text-decoration: underline;">
                <td id="tdReportHeader" runat="server" colspan="8">
                    <asp:Literal ID="lblDeficiencyReport" runat="server" Text="Non Conformity / Observation Report"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td colspan="8">
                    <br />
                    <br />
                </td>
            </tr>
            <tr>
                <td id="tdRefnumberHeader" runat="server" colspan="2" style="text-align: left; margin: 0px;
                    font-size: 9pt; font-weight: bold;">
                    <asp:Literal ID="lblReferenceNo" runat="server" Text="Reference Number"></asp:Literal>
                </td>
                <td id="tdRefNumber" runat="server" colspan="2" style="text-align: left; margin: 0px;
                    font-size: 9pt;">
                </td>
                <td id="tdIssuedDateHeader" runat="server" colspan="2" style="text-align: left; margin: 0px;
                    font-size: 9pt; font-weight: bold;">
                    <asp:Literal ID="lblIssuedDate" runat="server" Text="Issued Date"></asp:Literal>
                </td>
                <td id="tdIssuedDate" runat="server" colspan="2" style="text-align: left; margin: 0px;
                    font-size: 9pt;">
                </td>
            </tr>
            <tr>
                <td id="tdVesselHeader" runat="server" colspan="2" style="text-align: left; margin: 0px;
                    font-size: 9pt; font-weight: bold;">
                    <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                </td>
                <td id="tdVessel" runat="server" colspan="2" style="text-align: left; margin: 0px;
                    font-size: 9pt;">
                </td>
                <td id="tdStatusHeader" runat="server" colspan="2" style="text-align: left; margin: 0px;
                    font-size: 9pt; font-weight: bold;">
                    <asp:Literal ID="lblStatus" runat="server" Text="Status"></asp:Literal>
                </td>
                <td id="tdStatus" runat="server" colspan="2" style="text-align: left; margin: 0px;
                    font-size: 9pt;">
                </td>
            </tr>
            <tr>
                <td colspan="8">
                    <br />
                </td>
            </tr>
            <tr style="text-align: left; margin: 0px; font-size: 10pt; font-weight: bold; text-decoration: underline;">
                <td id="tdDetailsHeader" runat="server" colspan="8">
                    <asp:Literal ID="lblDetails" runat="server" Text="Details"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td colspan="8">
                    <br />
                </td>
            </tr>
            <tr>
                <td id="tdDeficiencyTypeHeader" runat="server" colspan="2" style="text-align: left;
                    margin: 0px; font-size: 9pt; font-weight: bold;">
                    <asp:Literal ID="lblDeficiencyType" runat="server" Text="Deficiency Type"></asp:Literal>
                </td>
                <td id="tdDeficiencyType" runat="server" colspan="6" style="text-align: left; margin: 0px;
                    font-size: 9pt;">
                </td>
            </tr>
            <tr>
                <td id="tdSourceHeader" runat="server" colspan="2" style="text-align: left; margin: 0px;
                    font-size: 9pt; font-weight: bold;">
                    <asp:Literal ID="lblSource" runat="Server" Text="Source"></asp:Literal>
                </td>
                <td id="tdSource" runat="server" colspan="6" style="text-align: left; margin: 0px;
                    font-size: 9pt;">
                </td>
            </tr>
            <tr>
                <td id="tdDeficiencyCategoryHeader" runat="server" colspan="2" style="text-align: left;
                    margin: 0px; font-size: 9pt; font-weight: bold;">
                    <asp:Literal ID="lblDeficiencyCategory" runat="server" Text="Deficiency Category"></asp:Literal>
                </td>
                <td id="tdDeficiencyCategory" runat="server" colspan="6" style="text-align: left;
                    margin: 0px; font-size: 9pt;">
                </td>
            </tr>
            <tr>
                <td id="tdItemHeader" runat="server" colspan="2" style="text-align: left; margin: 0px;
                    font-size: 9pt; font-weight: bold;">
                    <asp:Literal ID="lblItem" runat="server" Text="Item"></asp:Literal>
                </td>
                <td id="tdItem" runat="server" colspan="6" style="text-align: left; margin: 0px;
                    font-size: 9pt;">
                </td>
            </tr>
            <tr>
                <td id="tdChapterNoHeader" runat="server" colspan="2" style="text-align: left; margin: 0px;
                    font-size: 9pt; font-weight: bold;">
                    <asp:Literal ID="lblChapterNo" runat="server" Text="Chapter No"></asp:Literal>
                </td>
                <td id="tdChapterNo" runat="server" colspan="6" style="text-align: left; margin: 0px;
                    font-size: 9pt;">
                </td>
            </tr>
            <tr>
                <td id="tdChecklistReferenceHeader" runat="server" colspan="2" style="text-align: left;
                    margin: 0px; font-size: 9pt; font-weight: bold;">
                    <asp:Literal ID="lblCheckListReference" runat="server" Text="Checklist Reference"></asp:Literal>
                </td>
                <td id="tdChecklistReference" runat="server" colspan="6" style="text-align: left;
                    margin: 0px; font-size: 9pt;">
                </td>
            </tr>
            <tr runat="server" visible="false">
                <td id="tdKeyHeader" runat="server" colspan="2" style="text-align: left; margin: 0px;
                    font-size: 9pt; font-weight: bold;">
                    <asp:Literal ID="lblKey" runat="server" Text="Key"></asp:Literal>
                </td>
                <td id="tdKey" runat="server" colspan="6" style="text-align: left; margin: 0px; font-size: 9pt;">
                </td>
            </tr>
            <tr>
                <td id="tdDeficiencyDescHeader" runat="server" colspan="2" style="text-align: left;
                    margin: 0px; font-size: 9pt; font-weight: bold;">
                    <asp:Literal ID="lblDeficiencyDescription" runat="server" Text="Description"></asp:Literal>
                </td>
                <td id="tdDeficiencyDesc" runat="server" colspan="6" style="text-align: left; margin: 0px;
                    font-size: 9pt;">
                </td>
            </tr>
            <tr>
                <td colspan="8">
                    <br />
                </td>
            </tr>
            <tr style="text-align: left; margin: 0px; font-size: 10pt; font-weight: bold; text-decoration: underline;">
                <td id="tdRCAHeader" runat="server" colspan="8">
                    <asp:Literal ID="lblRootCauseAnalysis" runat="server" Text="Root Cause Analysis"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td colspan="8">
                    <br />
                </td>
            </tr>
            <tr>
                <td id="tdRCAReqHeader" runat="server" style="text-align: left; margin: 0px; font-size: 9pt;
                    font-weight: bold;">
                    <asp:Literal ID="lblRCARequired" runat="server" Text="RCA Required"></asp:Literal>
                </td>
                <td id="tdRCAReq" runat="server" style="text-align: left; margin: 0px; font-size: 9pt;">
                </td>
                <td id="tdRCATargetDateHeader" runat="server" style="text-align: left; margin: 0px;
                    font-size: 9pt; font-weight: bold;">
                    <asp:Literal ID="lblRCATargetDate" runat="server" Text="RCA Target Date"></asp:Literal>
                </td>
                <td id="tdRCATargetDate" runat="server" style="text-align: left; margin: 0px; font-size: 9pt;">
                </td>
                <td id="tdRCACompletedHeader" runat="server" style="text-align: left; margin: 0px;
                    font-size: 9pt; font-weight: bold;">
                    <asp:Literal ID="lblRCACompleted" runat="server" Text="RCA Completed"></asp:Literal>
                </td>
                <td id="tdRCACompleted" runat="server" style="text-align: left; margin: 0px; font-size: 9pt;">
                </td>
                <td id="tdDateHeader" runat="server" style="text-align: left; margin: 0px; font-size: 9pt;
                    font-weight: bold;">
                    <asp:Literal ID="lblDate" runat="server" Text="Date"></asp:Literal>
                </td>
                <td id="tdDate" runat="server" style="text-align: left; margin: 0px; font-size: 9pt;">
                </td>
            </tr>
            <tr>
                <td colspan="8">
                    <br />
                </td>
            </tr>
            <tr id="trRCA" runat="server">
                <td id="tdRCA" runat="server" colspan="8">
                </td>
            </tr>
            <tr id="trRCANIL" runat="server" visible="false">
                <td colspan="8">
                    <b>
                        <asp:Literal ID="lblNIL" runat="server" Text="NIL"></asp:Literal></b>
                </td>
            </tr>
            <tr>
                <td colspan="8">
                    <br />
                </td>
            </tr>
            <tr style="text-align: left; margin: 0px; font-size: 10pt; font-weight: bold; text-decoration: underline;">
                <td id="tdCAHeader" runat="server" colspan="8">
                    <asp:Literal ID="lblCorrectiveAction" runat="server" Text="Corrective Action"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td colspan="8">
                    <br />
                </td>
            </tr>
            <tr id="trCA" runat="server">
                <td id="tdCA" runat="server" colspan="8">
                </td>
            </tr>
            <tr id="trCANIL" runat="server" visible="false">
                <td colspan="8">
                    <b>
                        <asp:Literal ID="lblNIL2" runat="server" Text="NIL"></asp:Literal></b>
                </td>
            </tr>
            <tr>
                <td colspan="8">
                    <br />
                </td>
            </tr>
            <tr style="text-align: left; margin: 0px; font-size: 10pt; font-weight: bold; text-decoration: underline;">
                <td id="tdPAHeader" runat="server" colspan="8">
                    <asp:Literal ID="lblPreventiveAction" runat="server" Text="Preventive Action"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td colspan="8">
                    <br />
                </td>
            </tr>
            <tr id="trPA" runat="server">
                <td id="tdPA" runat="server" colspan="8">
                </td>
            </tr>
            <tr id="trPANIL" runat="server" visible="false">
                <td colspan="8">
                    <b>
                        <asp:Literal ID="lblNIL3" runat="server" Text="NIL"></asp:Literal></b>
                </td>
            </tr>
            <tr>
                <td colspan="8">
                    <br />
                </td>
            </tr>
            <tr style="text-align: left; margin: 0px; font-size: 10pt; font-weight: bold; text-decoration: underline;">
                <td id="tdCloseOutHeader" runat="server" colspan="8">
                    <asp:Literal ID="lblCloseOut" runat="Server" Text="Close Out"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td colspan="8">
                    <br />
                </td>
            </tr>
            <tr>
                <td id="tdCloseOutDateHeader" runat="server" colspan="2" style="text-align: left;
                    margin: 0px; font-size: 9pt; font-weight: bold;">
                    <asp:Literal ID="lblCloseOutDate" runat="server" Text="Close Out Date"></asp:Literal>
                </td>
                <td id="tdCloseOutDate" runat="server" colspan="2" style="text-align: left; margin: 0px;
                    font-size: 9pt;">
                </td>
                <td id="tdCloseOutByHeader" runat="server" colspan="2" style="text-align: left; margin: 0px;
                    font-size: 9pt; font-weight: bold;">
                    <asp:Literal ID="lblCloseOutBy" runat="server" Text="Close Out by"></asp:Literal>
                </td>
                <td id="tdCloseOutBy" runat="server" colspan="2" style="text-align: left; margin: 0px;
                    font-size: 9pt;">
                </td>
            </tr>
            <tr>
                <td id="tdCloseOutRemarksHeader" runat="server" colspan="2" style="text-align: left;
                    margin: 0px; font-size: 9pt; font-weight: bold;">
                    <asp:Literal ID="lblCloseOutRemarks" runat="Server" Text="Close Out Remarks"></asp:Literal>
                </td>
                <td id="tdCloseOutRemarks" runat="server" colspan="6" style="text-align: left; margin: 0px;
                    font-size: 9pt;">
                </td>
            </tr>
        </table>
    </div>
    <br />
    <br />
    </form>
</body>
</html>
