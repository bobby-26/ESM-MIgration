<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StandardFormSMCMeeting.aspx.cs"
    Inherits="StandardFormSMCMeeting" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>DE 37 SMC Meeting </title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
     <script language="javascript" type="text/javascript">
        function cmdPrint_Click()
        {            
            document.getElementById('cmdPrint').style.visibility = "hidden";
            window.print();
        }
    </script>    
</telerik:RadCodeBlock></head>
<body>
    <form id="frmSMCMeeting" runat="server" autocomplete="off">
     <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader" style="position: relative">
            <eluc:Title runat="server" ID="Title1" Text="SMC Meeting" ShowMenu="false"></eluc:Title>
             <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="StandardForm_TabStripCommand" />
            <asp:Button ID="cmdHiddenConfirm" runat="server" Text="cmdHiddenConfirm" OnClick="StandardFormConfirm_TabStripCommand" />
        </div>
       
        <asp:UpdatePanel runat="server" ID="pnlStandardForm">
            <ContentTemplate>
            <input type="button" id="cmdPrint" value="Print" onclick="cmdPrint_Click();" runat="server" visible="false" />
                <table width="95%">
                    <tr>
                        <td colspan="3" align="left">
                            <b><asp:Literal ID="lblCompanyName" runat="server" Text="EXECUTIVE SHIP MANAGEMENT"></asp:Literal> </b>
                        </td>
                        <td align="left">
                            <asp:Literal ID="lblfileRef" runat="server" Text="File Ref – 153.16"></asp:Literal>
                        </td>
                        <td align="right">
                            <asp:Literal ID="lblDE37" runat="server" Text="DE 37"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="left">
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:Literal id="lblM22" runat="server" Text="M 22"></asp:Literal>
                        </td>
                        <td align="right">
                            <asp:Literal ID="lblPage1of3" runat="server" Text="Page 1 of 3"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" align="right">
                            <asp:Literal ID="lbl309REV0" runat="server" Text="(3/09 REV 0)"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" align="center">
                            <h2 style="margin-bottom: 0px">
                                <asp:Literal ID="lblshipMaintanceCommiteeMeeting" runat="server" Text="SHIP’S MAINTENANCE COMMITTEE MEETING<br />"></asp:Literal>
                            </h2>
                            <asp:Literal ID="lblTObeReportedtotheOfficebymaileverymonth" runat="server" Text="(To be reported to the office by email every month)"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <b><asp:Literal ID="lblSMCMeetingNo" runat="server" Text="SMC MEETING NO."></asp:Literal></b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMeetingNo" runat="server" CssClass="input"></asp:TextBox><asp:Literal ID="lblMonthYear" runat="server" Text="&nbsp;(Month
                            /Year)"></asp:Literal>
                        </td>
                        <td>
                            <b><asp:Literal ID="lblDate" runat="server" Text="Date"></asp:Literal></b>
                        </td>
                        <td>
                            <eluc:Date ID="txtDate" runat="server" CssClass="input" DatePicker="true" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <asp:Literal ID="lblTobeCOnductedAroundthe15thEveryMonth" runat="server" Text="(To be conducted around the 15th of every month)"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <b><asp:Literal ID="lblAttendees" runat="server" Text="Attendees:"></asp:Literal> </b>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <asp:TextBox ID="txtAddress" runat="server" CssClass="input" TextMode="MultiLine"
                                Height="51px" Width="1122px"></asp:TextBox><br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <b><asp:Literal ID="lblPendingJobsFromThosePlannedInThePreviousMonth" runat="server" Text="Pending Jobs from those planned in the previous month:-"></asp:Literal></b>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <b><asp:Literal ID="lblEngine" runat="server" Text="ENGINE:"></asp:Literal></b>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtEngine" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <b><asp:Literal ID="lblDeck" runat="server" Text="DECK:"></asp:Literal></b>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtDeck" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                           <b> <asp:Literal ID="lblMajorJobs" runat="server" Text="Major Jobs and cosmetic up gradation planned for next month:-"></asp:Literal></b><br />
                            <asp:Literal ID="lblRoutineJobs" runat="server" Text="(Routine jobs are not be included, cosmetic up gradation to reference areas by frame
                            number /location etc)<br />"></asp:Literal>
                            <b><asp:Literal ID="lblJobDeck" runat="server" Text="Deck:"></asp:Literal></b>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <asp:TextBox ID="txtDeckJobs" runat="server" CssClass="input" TextMode="MultiLine"
                                Height="51px" Width="1122px"></asp:TextBox><br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <b><asp:Literal ID="lblEngineJobs" runat="server" Text="Engine:"></asp:Literal></b>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <asp:TextBox ID="txtEngineJobs" runat="server" CssClass="input" TextMode="MultiLine"
                                Height="51px" Width="1122px"></asp:TextBox><br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <b><asp:Literal ID="lblFollowUpCompletionStatusofLastSupdt" runat="server" Text="Follow up / Completion status of last SUPDT. VIR"></asp:Literal></b><br />
                            <br />
                            <asp:Literal ID="lblTheCompletedItemsoftheVIR" runat="server" Text="(The completed items of the VIR to be indicated by their code numbers with relevant
                            <br />comments/information and any jobs which cannot be completed within the stipulated target date may only be<br />extended with the approval of the office)"></asp:Literal>
                            
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <b><asp:Literal ID="lblPendingDeficienciesRelatedtoPSC" runat="server" Text="Pending deficiencies related to PSC/ Oil Major/External & Owners inspections:"></asp:Literal>
                            </b>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <asp:TextBox ID="txtPendingInspections" runat="server" CssClass="input" TextMode="MultiLine"
                                Height="51px" Width="1122px"></asp:TextBox><br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <b><asp:Literal ID="lblDeckPendingRequisition" runat="server" Text="DECK:"></asp:Literal></b><br />
                            <br />
                            <b><asp:Literal ID="lblPendingRequisition" runat="server" Text="PENDING REQUISITION"></asp:Literal></b>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <asp:TextBox ID="txtDeckPendingEnquires" runat="server" CssClass="input" TextMode="MultiLine"
                                Height="51px" Width="1122px"></asp:TextBox><br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <b><asp:Literal ID="lblEnginePendingRequisition" runat="server" Text="ENGINE:"></asp:Literal></b>
                            <br />
                            <br />
                            <b><asp:Literal ID="lblPendingRequisitionEnquies" runat="server" Text="PENDING REQUISITION"></asp:Literal></b>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <asp:TextBox ID="txtEnginePendingEnquires" runat="server" CssClass="input" TextMode="MultiLine"
                                Height="51px" Width="1122px"></asp:TextBox><br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <b><asp:literal ID="lblRequisitionReceivedDuringThisMonth" runat="server" Text="REQUISITION RECEIVED DURING THIS MONTH:"></asp:literal></b><br />
                            <br />
                            <b><asp:Literal ID="lblEnginRequisitione" runat="server" Text="ENGINE:"></asp:Literal></b>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <asp:TextBox ID="txtEngineRequisition" runat="server" CssClass="input" TextMode="MultiLine"
                                Height="51px" Width="1122px"></asp:TextBox><br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <b><asp:Literal ID="lblDeckRequisition" runat="server" Text="DECK:"></asp:Literal></b>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <asp:TextBox ID="txtDeckRequisition" runat="server" CssClass="input" TextMode="MultiLine"
                                Height="51px" Width="1122px"></asp:TextBox><br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <b><asp:Literal ID="lblRemarksCritiacalSpares" runat="server" Text="Remarks (Critical Spares etc):"></asp:Literal></b>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <b><asp:Literal ID="lblEngineRemarks" runat="server" Text="ENGINE:"></asp:Literal> </b>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtEngineRemarks" runat="server" CssClass="input" TextMode="MultiLine"
                                Height="51px" Width="945px"></asp:TextBox><br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <b><asp:Literal ID="lblSIgnatures" runat="server" Text="Signatures:"></asp:Literal></b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b><asp:Literal ID="lblMaster" runat="server" Text="Master:"></asp:Literal></b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMasterSign" runat="server" CssClass="input"></asp:TextBox><br />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <b><asp:Literal ID="lblChiefEngineer" runat="server" Text="Chief Engineer:"></asp:Literal></b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtChiefEngineerSign" runat="server" CssClass="input"></asp:TextBox><br />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
