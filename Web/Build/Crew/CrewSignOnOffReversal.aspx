<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewSignOnOffReversal.aspx.cs"
    Inherits="CrewSignOnOffReversal" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Briefing De-Briefing List</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewSignOnOffReversal" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">


            <eluc:TabStrip ID="MenuCrewSignOnOffReversal" runat="server" OnTabStripCommand="CrewSignOnOffReversal_TabStripCommand"></eluc:TabStrip>

            <div id="divFind" style="position: relative; z-index: 2">
                <table id="tblConfigureCrewSignOnOffReversal" width="90%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblReversal" runat="server" Text="Reversal"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="drReversalType" runat="server" CssClass="dropdown_mandatory" Filter="Contains" >
                                <Items>
                                    <telerik:RadComboBoxItem Text="--Select--" Value="-1" />
                                    <telerik:RadComboBoxItem Text="to Onboard" Value="0" />
                                    <telerik:RadComboBoxItem Text="to Onleave" Value="1" />
                                </Items>

                            </telerik:RadComboBox>
                        </td>
                        <td colspan="4">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Vessel runat="server" ID="ucVessel" VesselsOnly="true" AppendDataBoundItems="true" Entitytype="VSL" ActiveVessels="true"
                                CssClass="dropdown_mandatory" AssignedVessels="true" />
                        </td>
                        <td colspan="4">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblSeafarerFileNumber" runat="server" Text="Seafarer File Number"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtFileNo" Width="60%" CssClass="input_mandatory"></telerik:RadTextBox>
                            <asp:LinkButton ID="ImgBtnValidFileno" runat="server" ImageAlign="AbsBottom" 
                                ToolTip="Verify Entered File Number" OnClick="ImgBtnValidFileno_Click1" >
                                <span class="icon"> <i class="fas fa-search"></i></span>
                            </asp:LinkButton>
                        </td>
                        <td colspan="4">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>

                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtMiddleName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtLastName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblEmployeeNumber" runat="server" Text="Employee Number"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <telerik:RadTextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <font color="blue">Note: Selected vessel should be a last sign on/off vessel
                                    <br />
                                Please verify the entered file number by clicking search icon next to the File number
                                    textbox</font>
                        </td>
                    </tr>
                </table>
                <eluc:Status ID="ucStatus" runat="server" />
            </div>
        </div>

    </form>
</body>
</html>
