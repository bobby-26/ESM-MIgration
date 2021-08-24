<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdministrationITSupportAdd.aspx.cs"
    Inherits="AdministrationITSupportAdd" %>

<!DOCTYPE html>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="ModuleList" Src="~/UserControls/UserControlSEPModuleList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BugTypes" Src="~/UserControls/UserControlSEPBugType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BugSeverity" Src="~/UserControls/UserControlSEPBugSeverity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BugPriority" Src="~/UserControls/UserControlSEPBugPriority.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ITStatus" Src="~/UserControls/UserControlITStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Department" Src="~/UserControls/UserControlDepartment.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Category" Src="~/UserControls/UserControlITCategory.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>IT Support</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
     <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    <script type="text/javascript" language="javascript">
        function fnAdminITSupportEdit(dtkey)
        {
            location.href = 'AdministrationITSupportList.aspx';
        }
    </script>

</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
            EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="ReportNotRelievedOnTime"
            runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"
            Height="100%">
                <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                
                    <eluc:TabStrip ID="MenuSupportAdd" runat="server" OnTabStripCommand="MenuSupportAdd_TabStripCommand">
                    </eluc:TabStrip>
                <table width="100%">
                    <tr>
                        <td>
                            System Name
                        </td>
                        <td>
                            <Telerik:RadTextBox ID="txtSystemName" runat="server" CssClass="input"></Telerik:RadTextBox>
                        </td>
                        <td>
                            Department
                        </td>
                        <td>
                            <eluc:Department ID="ddlDepartmentList" runat="server" MaxLength="100" AppendDataBoundItems="true"
                                CssClass="dropdown_mandatory" AutoPostBack="true" Width="120px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Category
                        </td>
                        <td>
                            <eluc:Category ID="ddlCategoryType" runat="server" MaxLength="100" Width="120px"
                                AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                        </td>
                         <td>
                           Request User Name
                        </td>
                        <td >
                            <Telerik:RadTextBox ID="txtLoggedBy" runat="server" MaxLength="100" CssClass="input"></Telerik:RadTextBox>
                        </td>
                       
                    </tr>
                  
                    <tr>
                        <td>
                            Call Type
                        </td>
                        <td colspan="3">
                            <Telerik:RadTextBox ID="txtCallType" runat="server" MaxLength="500" CssClass="input_mandatory"
                                Width="60%"></Telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Remarks
                        </td>
                        <td colspan="3">
                            <Telerik:RadTextBox ID="txtRemarks" runat="server"  TextMode="MultiLine"
                                Rows="4" Columns="100" CssClass="input" Width="90%"></Telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
           </telerik:RadAjaxPanel>
    </form>
</body>
</html>
