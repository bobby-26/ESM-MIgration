<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreTrainingNeedOverride.aspx.cs" Inherits="CrewOffshoreTrainingNeedOverride" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmCrew" Src="~/UserControls/UserControlConfirmMessage.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Training Summary</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
         <script type="text/javascript">
        function confirm(args) {
            if (args) {
                __doPostBack("<%=confirm.UniqueID %>", "");
            }
        }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewRecommendedCourses" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
             <asp:Button runat="server" ID="confirm" OnClick="btnCrewApprove_Click" />
            <div class="navigation" id="Div1" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
              
             
                    <eluc:TabStrip ID="MenuTrainingNeedOverride" runat="server" Title="Show Override Remarks" OnTabStripCommand="MenuTrainingNeedOverride_TabStripCommand"></eluc:TabStrip>
               
                <div>
                    <table width="75%">
                        <tr>
                         
                            <td>
                                <telerik:RadLabel ID="lblOverrideRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOverrideRemarks" runat="server" CssClass="input_mandatory" Height="50px" Rows="4"
                                    TextMode="MultiLine" Width="50%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblOverrideDate" runat="server" Text="Date"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="txtOverrideDate" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
                            </td>

                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblOverrideBy" runat="server" Text="Override By"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtOverrideBy" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                            </td>

                        </tr>
                    </table>
                </div>
             <%--   <eluc:ConfirmCrew ID="ucConfirmCrew" runat="server" OnConfirmMesage="btnCrewApprove_Click"
                    OKText="Yes" CancelText="No" Visible="false" />--%>
                <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
