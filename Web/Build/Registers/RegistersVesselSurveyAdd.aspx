<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersVesselSurveyAdd.aspx.cs"
    Inherits="Registers_RegistersVesselSurveyAdd" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Category" Src="~/UserControls/UserControlVesselCertificateCategoryList.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Survey</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div runat="server" id="dvscriptsk">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmRegistersSurvey" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlSurvey">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <asp:Literal runat="server" ID="lblheader" Text="Survey"></asp:Literal>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuRegistersSurveyAdd" runat="server" OnTabStripCommand="MenuRegistersSurveyAdd_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divControls">
                    <table width="100%" cellspacing="10">
                        <tr>
                            <td colspan="2">
                                <font color="blue"><b>Note: </b>
                                    <br />
                                    1. 'Last Survey Date' & 'Last Survey Type' will applicable for the Survey Type 'PERIODIC
                                    SURVEY' only<br />
                                    2. Modifications of Periodic Survey is not allowed after the Survey Scheduled</font>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVessel" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="365px" MaxLength="500"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblSurvey" runat="server" Text="Survey"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSurvey" runat="server" CssClass="input_mandatory" Width="365px"
                                    MaxLength="500"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblSurveyType" runat="server" Text="Survey Type"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSurveyType" runat="server" CssClass="input_mandatory" Width="150px"
                                    OnSelectedIndexChanged="ddlSurveyType_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblFrequency" runat="server" Text="Frequency"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Number ID="txtFrequency" runat="server" CssClass="readonlytextbox" IsPositive="true"
                                    IsInteger="true" Width="150px" MaxLength="3" ReadOnly="true" />
                                &nbsp;(Months)
                            </td>
                        </tr>
                        <tr runat="server" visible="false">
                            <td>
                                <asp:Literal ID="lblCertificateCategory" runat="server" Text="Certificate Category"></asp:Literal>
                            </td>
                            <td>
                                <asp:ListBox ID="lstCategory" runat="server" CssClass="input_mandatory" SelectionMode="Multiple"
                                    Width="50%"></asp:ListBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblWinPeriod" runat="server" Text="Window Period"></asp:Literal>
                            </td>
                            <td>
                                Before
                                <eluc:Number ID="txtWinPeriod" runat="server" CssClass="input" IsInteger="true" Width="120px" />
                                &nbsp;Months<br />
                                <br />
                                After&nbsp;&nbsp;
                                <eluc:Number ID="txtPlusMinusPeriod" runat="server" CssClass="input" IsInteger="true"
                                    Width="120px" />
                                &nbsp;Months
                            </td>
                        </tr>
                    </table>
                    <table width="100%">
                        <tr>
                            <td colspan="6">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <b>
                                    <asp:Literal ID="lblSurveyorHeader" runat="server" Text="Survey Configuration"></asp:Literal></b>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblCommencementDate" runat="server" Text="Commencement Date"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Date ID="ucCommenceDate" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td>
                                <asp:Literal ID="lblLastSurveyType" runat="server" Text="Last Survey Type"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlLastSurveyType" runat="server" CssClass="dropdown_mandatory">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Literal ID="lblLastSurveyDate" runat="server" Text="Last Survey Date"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Date ID="ucLastSurveyDate" runat="server" CssClass="input_mandatory" />
                            </td>
                        </tr>
                    </table>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
