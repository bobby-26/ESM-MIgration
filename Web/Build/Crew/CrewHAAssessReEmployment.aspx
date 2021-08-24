<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewHAAssessReEmployment.aspx.cs"
    Inherits="Crew_CrewHAAssessReEmployment" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCNTBRReason" Src="~/UserControls/UserControlNTBRReason.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Re-Employment Suitability</title>
 <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
      <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewDateOfAvailability" runat="server">
    <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
  <telerik:RadAjaxPanel ID="panel1" runat="server">
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
      
          
         
       
            <eluc:TabStrip ID="MenuDO" runat="server" OnTabStripCommand="DOA_TabStripCommand">
            </eluc:TabStrip>
     
        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblEmployeeCode" runat="server" Text="Employee Code"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtEmployeeCode" runat="server" CssClass="readonlytextbox" ReadOnly="True"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblEmployeeName" runat="server" Text="Employee Name"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtEmployeeName" runat="server" MaxLength="50" CssClass="readonlytextbox"
                        ReadOnly="True" Width="150px"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblPresentRank" runat="server" Text="Present Rank"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtPayRank" runat="server" MaxLength="20" CssClass="readonlytextbox"
                        ReadOnly="True"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblLastVessel" runat="server" Text="Last Vessel"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtLastVessel" runat="server" MaxLength="20" CssClass="readonlytextbox"
                        ReadOnly="True"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblSignedOff" runat="server" Text="Signed Off"></telerik:RadLabel>
                </td>
                <td colspan="3">
                    <telerik:RadTextBox ID="txtSignedOff" runat="server" MaxLength="20" CssClass="readonlytextbox"
                        ReadOnly="True"></telerik:RadTextBox>
                </td>
                <%-- <td>
                        Vacation End
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVactionEnd" runat="server" CssClass="readonlytextbox" ReadOnly="True"></telerik:RadTextBox>
                    </td>--%>
            </tr>
        </table>
        <br />
        <hr />
        <table width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblstblremep" runat="server" Text="Is candidate Suitable for re-employment:"></telerik:RadLabel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:RadioButtonList ID="rblstblremep" runat="server" AutoPostBack="true" RepeatDirection="Horizontal"
                        OnSelectedIndexChanged="rblstblremep_SelectedIndexChanged">
                        <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                        <asp:ListItem Text="No" Value="2"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
        </table>
        <hr />
        <table cellpadding="1" cellspacing="1" width="100%" >
            <tr>
                <td>
                    <telerik:RadLabel ID="lblSelect" runat="server" Text="Select Principal/Manager"></telerik:RadLabel>
                </td>
                <td>
                    <asp:RadioButtonList ID="rblPrincipalManager" runat="server" RepeatDirection="Horizontal"
                        AutoPostBack="true" OnSelectedIndexChanged="PrincipalManagerClick" Enabled="false">
                        <asp:ListItem Value="1" Selected="true">Manager</asp:ListItem>
                        <asp:ListItem Value="2">Principal</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblPrincipalManager" runat="server" Text="Principal/Manager"></telerik:RadLabel>
                </td>
                <td colspan="3">
                    <eluc:AddressType ID="ddlManager" runat="server" AddressType="126" AppendDataBoundItems="true"
                        CssClass="dropdown_mandatory" Enabled="false" Width="150px"/>
                    <div runat="server" visible="false" id="dvAddressType" class="input_mandatory" style="overflow: auto;
                        width: 50%; height: 100px">
                        <asp:CheckBoxList runat="server" ID="cblAddressType" Height="100%" RepeatColumns="1"
                            RepeatDirection="Horizontal" RepeatLayout="Flow"  Enabled="false">
                        </asp:CheckBoxList>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblNTBRReason" runat="server" Text="NTBR Reason"></telerik:RadLabel>
                </td>
                <td colspan="3">
                    <eluc:UCNTBRReason ID="ddlNTBRReason" runat="server" AppendDataBoundItems="true"
                        CssClass="dropdown_mandatory"  Enabled="false"  Width="150px" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblNTBRDate" runat="server" Text="NTBR Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtNTBRDate" runat="server" CssClass="input_mandatory"  Enabled="false"/>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblNTBRRemarks" runat="server" Text="NTBR Remarks"></telerik:RadLabel>
                </td>
                <td>
                    <asp:TextBox ID="txtNTBRRemarks" runat="server" CssClass="input_mandatory" TextMode="MultiLine"
                        MaxLength="200" Width="300px"  Enabled="false"></asp:TextBox>
                </td>
            </tr>
        </table>
        <hr />
        <table width="100%">
            <tr>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblHaAssessmentRemarks" runat="server" Text="ReEmployment Remarks:"></telerik:RadLabel>
                </td>
                <td>
                    <asp:TextBox ID="txtHaAssessmentRemarks" runat="server" CssClass="input_mandatory"
                        TextMode="MultiLine" MaxLength="200" Width="300px" Enabled="false"></asp:TextBox>
                </td>
            </tr>
           
        </table>
        </div>
      </telerik:RadAjaxPanel>
    </form>
</body>
</html>
