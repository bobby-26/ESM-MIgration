<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsVesselVisitRidingSuperintendentVisit.aspx.cs" Inherits="AccountsVesselVisitRidingSuperintendentVisit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Multiport" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>IT Visit</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </div>
    <style type="text/css">
        .hidden
        {
            display: none;
        }
    </style>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmSupVisit" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts ="false" >
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlSupVisit">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="Title1" Text="Riding Superintendent Visit - India" ShowMenu="false"></eluc:Title>
                    <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click"
                        CssClass="hidden" />
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuSupVisit" runat="server" OnTabStripCommand="MenuSupVisit_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <table id="tbldiv" runat="server" cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td>
                            <div class="subHeader" style="position: relative;">
                                <span class="navSelect" style="margin-top: 0px; float: right; width: auto;">
                                    <eluc:TabStrip ID="MenuITVisitSub" runat="server" OnTabStripCommand="MenuITVisitSub_TabStripCommand">
                                    </eluc:TabStrip>
                                </span>
                            </div>
                        </td>
                    </tr>
                </table>               
                <table id="Table2" width="100%" style="color: Blue">
                    <tr>
                        <td>
                            &nbsp;* From/ To Date include Travelling Days
                        </td>
                    </tr>
                </table>
                <table cellpadding="2" cellspacing="1" style="width: 100%">
                    <tr>
                        <td width="12%">
                            <asp:Literal ID="lblFormNo" runat="server" Text="Form No"></asp:Literal>
                        </td>
                        <td width="38%">
                            <asp:TextBox ID="txtFormNo" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="50%"></asp:TextBox>
                        </td>
                        <td width="10%" />
                        <td width="40%"/>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr style="height: -15px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                            
                        </td>
                        <td>
                            <eluc:Vessel ID="ddlVessel" runat="server" CssClass="input_mandatory" VesselsOnly="true"
                                AppendDataBoundItems="true" AutoPostBack="true" OnTextChangedEvent="ddlVessel_Changed"  />
                        </td>
                        <td>
                            <asp:Literal ID="lblVesselAccount" runat="server" Text="Vessel Account"></asp:Literal>
                            
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlAccountDetails" runat="server" CssClass="dropdown_mandatory" OnDataBound="ddlAccountDetails_DataBound"
                                    DataTextField="FLDVESSELACCOUNTNAME" DataValueField="FLDACCOUNTID" Width="250px" ></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr style="height: -15px" />
                        </td>
                    </tr>
                    <tr>

                     <tr>
                        <td>
                           <asp:Literal ID="Literal1" runat="server" Text="Employee Name"></asp:Literal>
                        </td>
                      
                        <td>
                            <span id="spnPickListFleetManager">
                                <asp:TextBox ID="txtMentorName" runat="server" CssClass="input_mandatory" MaxLength="100"
                                    Width="80%"></asp:TextBox>
                                <asp:TextBox ID="txtuserDesignation" runat="server" CssClass="hidden" Enabled="false"
                                    MaxLength="30" Width="5px" ReadOnly="true"></asp:TextBox>
                                <asp:ImageButton runat="server" ID="imguser" Style="cursor: pointer; vertical-align: top"
                                    ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" OnClientClick="return showPickList('spnPickListFleetManager', 'codehelp1', '', '../Common/CommonPickListEmployeeAccount.aspx', true); "
                                    ToolTip="Select Employee" />
                                <asp:TextBox runat="server" ID="txtuserid" CssClass="hidden"></asp:TextBox>
                                <asp:TextBox runat="server" ID="txtuserEmailHidden" CssClass="hidden" Width="5px"></asp:TextBox>
                            </span>
                        </td>
                        <td>
                            <asp:Literal ID ="lblBudgetedVisit" runat="server" Text="Budgeted Visit"></asp:Literal>
                        </td>
                        <td>
                            <asp:DropDownList ID ="ddlBudgetedVisit" runat="server" CssClass="input_mandatory">
                                <asp:ListItem Text="--Select--" Value="Dummy"></asp:ListItem>
                                <asp:ListItem Text="Budgeted" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Non - Budgeted" Value="2"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr style="height: -15px" />
                        </td>
                    </tr>
                  
                    <tr>
                        <td>
                            <asp:Literal ID="lblPort" runat="server" Text="Port"></asp:Literal>
                        </td>
                        <td style="z-index:2;">
                            <eluc:Multiport ID="ucPortMulti" runat="server" CssClass="input_mandatory" Width="400px"/>
                        </td>
                        <td >
                            <asp:Literal ID ="lblCountry" runat="server" Text="Country"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Country ID="ddlcountrylist" runat="server" AppendDataBoundItems="true" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblFromDate" runat="server" Text="From Date"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="ucFromDate" runat="server" CssClass="input_mandatory" DatePicker="true"/>
                        </td>
                        <td>
                            <asp:Literal ID="lblFromTime" runat="server" Text="Time"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFromTime" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="50px"/>
                            <ajaxToolkit:MaskedEditExtender ID="txtFromTimeMask" runat="server" AcceptAMPM="false"
                                    ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99" MaskType="Time"
                                    TargetControlID="txtFromTime" UserTimeFormat="TwentyFourHour" /> hrs
                             
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblToDate" runat="server" Text="To Date"></asp:Literal>
                            
                        </td>
                        <td>
                            <eluc:Date ID="ucToDate" runat="server" CssClass="input_mandatory" DatePicker="true"/>
                        </td>
                        <td>
                            <asp:Literal ID="lblToTime" runat="server" Text="Time"></asp:Literal>
                            
                        </td>
                        <td>
                            <asp:TextBox ID="txtToTime" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="50px" />
                            <ajaxToolkit:MaskedEditExtender ID="txtToTimeMask" runat="server" AcceptAMPM="false"
                                    ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99" MaskType="Time"
                                    TargetControlID="txtToTime" UserTimeFormat="TwentyFourHour" /> hrs
                             
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblTravelHours" runat="server" Text="Travel Hours"></asp:Literal>
                            
                        </td>
                        <td>
                            <eluc:Number ID="txtTravelHours" runat="server" CssClass="readonlytextbox" ReadOnly="true" DecimalPlace="2" IsPositive="true" />
                        </td>
                        <td />
                        <td />
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr style="height: -15px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblCreatedBy" runat="server" Text="Created By"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCreatedBy" runat="server" CssClass="readonlytextbox" ReadOnly="true"/>
                        </td>
                        <td>
                            <asp:Literal ID="lblCreatedDate" runat="server" Text="Date"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="ucCreatedDate" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblPurpose" runat="server" Text="Purpose"></asp:Literal>
                            
                        </td>
                        <td>
                            <asp:TextBox ID="txtPurpose" runat="server" CssClass="input_mandatory" Width="360px"
                                Height="60px" TextMode="MultiLine"></asp:TextBox>
                        </td>
                        <td />
                        <td />
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblCompletedBy" runat="server" Text="Completed By"></asp:Literal>
                            
                        </td>
                        <td>
                            <asp:TextBox ID="txtCompletedBy" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
                        </td>
                        <td>
                            <asp:Literal ID="lblCompletedDate" runat="server" Text="Date"></asp:Literal>
                            
                        </td>
                        <td>
                            <eluc:Date ID="ucCompletedDate" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblAccountVoucherNo" runat="server" Text="Account Voucher Number"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAccountVoucherNo" runat="server" CssClass="readonlytextbox" ReadOnly="true"/>
                        </td>
                        <td />
                        <td />
                    </tr>
                </table>
                <eluc:Status runat="server" ID="ucStatus" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
    </form>
</body>
</html>
