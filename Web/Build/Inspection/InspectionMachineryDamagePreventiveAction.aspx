<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionMachineryDamagePreventiveAction.aspx.cs" Inherits="InspectionMachineryDamagePreventiveAction" %>

<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Department" Src="~/UserControls/UserControlDepartment.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SubDepartment" Src="~/UserControls/UserControlSubDepartment.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>CAR</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInspectionIncidentCriticalFactor" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text="" />
            <eluc:TabStrip ID="MenuCARGeneral" runat="server" OnTabStripCommand="MenuCARGeneral_TabStripCommand"></eluc:TabStrip>
            <table id="tblDetails" runat="server" width="100%">
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblPreventiveAction" runat="server" Text="Preventive Action"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtPreventiveAction" runat="server"
                            CssClass="input_mandatory" Height="50px" Rows="4" TextMode="MultiLine"
                            Width="97%" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblTargetDate" runat="server" Text="Target Date"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <eluc:Date ID="ucTargetDate" runat="server" CssClass="input_mandatory" COMMANDNAME="TARGETDATE"
                            DatePicker="true" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblTaskCategory" runat="server" Text="Task Category"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadComboBox ID="ddlCategory" runat="server" CssClass="input" AutoPostBack="true" Width="300px"
                            OnSelectedIndexChanged="ddlCategory_Changed" Filter="Contains">
                        </telerik:RadComboBox>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblTaskSubCategory" runat="server" Text="Task Subcategory"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%" id="tdddlSubcategory" runat="server">
                        <telerik:RadComboBox ID="ddlSubcategory" runat="server" CssClass="input" Width="300px"
                            Filter="Contains">
                        </telerik:RadComboBox>
                    </td>
                    <td width="35%" id="tdspnPickListDocument" runat="server">
                        <span id="spnPickListDocument">
                            <telerik:RadTextBox ID="txtDocumentName" runat="server" Width="300px" CssClass="input"></telerik:RadTextBox>
                            <asp:LinkButton ID="btnShowDocuments" runat="server" ImageAlign="AbsMiddle" Text="..">
                            <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtDocumentId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblDepartmentAssignedTo" runat="server"
                            Text="Department ( Assigned to )">
                        </telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <eluc:Department ID="ucDept" runat="server" AppendDataBoundItems="true"
                            AutoPostBack="true" CssClass="input"
                            OnTextChangedEvent="ucDept_TextChangedEvent" Width="300px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSubDepartment" runat="server" Text="Sub Department"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:SubDepartment ID="ucSubDept" runat="server" AppendDataBoundItems="true"
                            CssClass="input" Width="300px" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblCompletionDate" runat="server" Text="Completion Date"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <eluc:Date ID="ucCompletionDate" runat="server" CssClass="input" DatePicker="true" />
                    </td>
                    <td colspan="2"></td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
