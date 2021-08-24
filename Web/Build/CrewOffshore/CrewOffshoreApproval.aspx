<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreApproval.aspx.cs" Inherits="PhoenixCrewOffshoreApproval" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SignOnReason" Src="~/UserControls/UserControlSignOnReason.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Proposal</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <telerik:RadCodeBlock ID="Radcodeblock2" runat="server">
            <%: Scripts.Render("~/bundles/js") %>
            <%: Styles.Render("~/bundles/css") %>
            <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        </telerik:RadCodeBlock>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmAppraisalQuestion" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">

            <div class="navigation" id="Div1" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status ID="ucStatus" runat="server" />
               
               
                    <eluc:TabStrip ID="CrewMenuGeneral" runat="server" Title="Approval" OnTabStripCommand="CrewMenuGeneral_TabStripCommand"></eluc:TabStrip>
                    <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
              
                <br />
                <div id="divInput" runat="server">
                    <table id="tblProposal" runat="server" width="100%">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Vessel runat="server" ID="ucVessel" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                                    VesselsOnly="true" AssignedVessels="true" AutoPostBack="true" OnTextChangedEvent="SetVesselType" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblVesselType" runat="server" Text="Vessel Type"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:VesselType ID="ucVesselType" runat="server" AppendDataBoundItems="true" CssClass="readonlytextbox"
                                    Enabled="false" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblRank1" runat="server" Text=" Rank"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select rank" ID="ddlRank" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                                    DataTextField="FLDRANKNAME" DataValueField="FLDRANKID" AutoPostBack="true" OnTextChanged="ddlRank_Changed">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="Dummy" Text="--Select--" />
                                    </Items>
                                   
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblExpectedJoiningDate" runat="server" Text=" Expected Joining Date"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="ucDate" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblMatrix" runat="server" Text="Training Matrix"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select training matrix" ID="ddlTrainingMatrix" runat="server" Width="255px" CssClass="input_mandatory"
                                    AppendDataBoundItems="true">
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblOffsigner" runat="server" Text="Off-signer"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select offsigner" ID="ddlOffSigner" runat="server" Width="242px" CssClass="input"
                                    AppendDataBoundItems="true" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblRemarks" runat="server" Text="Approval Remarks"></telerik:RadLabel>
                            </td>
                            <td colspan="5">
                                <asp:TextBox ID="txtRemaks" runat="server" TextMode="MultiLine" Width="400px" Height="30px"
                                    CssClass="input_mandatory"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblSignOnReason" runat="server" Text="SignOn Reason"></telerik:RadLabel>
                            </td>
                            <td colspan="5">
                                <eluc:SignOnReason runat="server" ID="ucSignOnReason" CssClass="input" AppendDataBoundItems="true" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
