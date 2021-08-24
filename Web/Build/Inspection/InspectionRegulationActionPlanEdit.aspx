<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRegulationActionPlanEdit.aspx.cs"
    Inherits="InspectionRegulationActionPlanEdit" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Department" Src="~/UserControls/UserControlDepartment.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Action Plan Edit</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script language="Javascript">
            function isNumberKey(evt) {
                var charCode = (evt.which) ? evt.which : event.keyCode;
                if (charCode != 46 && charCode > 31
            && (charCode < 48 || charCode > 57))
                    return false;

                return true;
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInspectionIncidentCriticalFactor" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"
        Height="100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status ID="ucStatus" runat="server" Text="" />
        <eluc:TabStrip ID="MenuGeneral" runat="server" OnTabStripCommand="MenuGeneral_TabStripCommand">
        </eluc:TabStrip>
        <table id="tblDetails" runat="server" width="100%">
            <tr>
                <td style="width: 3%">
                    <telerik:RadLabel ID="lblActionTaken" runat="server" Text="Actions to be taken">
                    </telerik:RadLabel>
                </td>
                <td style="width: 60%">
                    <telerik:RadTextBox ID="txtActionToBeTaken" runat="server" CssClass="input" Width="80%"
                        TextMode="MultiLine">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 3%">
                    <telerik:RadLabel ID="lbldepartment" runat="server" Text="Department">
                    </telerik:RadLabel>
                </td>
                <td style="width: 15%">
                    <eluc:Department ID="ucDepartmentedit" runat="server" CssClass="gridinput" DepartmentList='<%#PhoenixRegistersDepartment.Listdepartment(1,null)%>'
                        Width="30%" AutoPostBack="true" AppendDataBoundItems="true" />
                    <telerik:RadLabel ID="lbldepartmentid" runat="server" Visible="false">
                    </telerik:RadLabel>
                    <telerik:RadLabel ID="lbldept" runat="server" Visible="false">
                    </telerik:RadLabel>
                </td>
            </tr>           
            <tr>
                <td style="width: 3%">
                    <telerik:RadLabel ID="lblTargetDate" runat="server" Text="Target date">
                    </telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtTargetdateEdit" runat="server" CssClass="input_mandatory" DatePicker="true" />
                    <asp:Label ID="lblVesselId" runat="server" Visible="false"></asp:Label>
                </td>
            </tr>
        </table>
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
