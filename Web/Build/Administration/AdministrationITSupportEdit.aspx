<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdministrationITSupportEdit.aspx.cs"
    Inherits="AdministrationITSupportEdit" ValidateRequest="false" %>

<!DOCTYPE html>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.DefectTracker" %>
<%@ Register TagPrefix="eluc" TagName="ModuleList" Src="~/UserControls/UserControlSepModuleList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Department" Src="~/UserControls/UserControlDepartment.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Category" Src="~/UserControls/UserControlITCategory.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ITStatus" Src="~/UserControls/UserControlITStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ITTeam" Src="~/UserControls/UserControlITTeam.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Edit</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
     <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
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
            <eluc:status id="ucStatus" runat="server"></eluc:status>
                <eluc:error id="ucError" runat="server" text="" visible="false"></eluc:error>
               
                        <eluc:TabStrip ID="MenuBugComment" runat="server" OnTabStripCommand="MenuBugComment_TabStripCommand" TabStrip="true">
                        </eluc:TabStrip>
                  
                        <eluc:tabstrip id="MenuITSupportEdit" runat="server" ontabstripcommand="MenuITSupportEdit_TabStripCommand">
                        </eluc:tabstrip>
                  
                    <table width="100%">
                        <tr>
                            <td colspan="6">
                                <Telerik:RadLabel ID="lblBugID" runat="server" Style="display:none" ></Telerik:RadLabel>
                                <Telerik:RadLabel ID="lblUniqueID" runat="server" Style="display:none" ></Telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <Telerik:RadLabel ID="ltrlCreatedOn" runat="server" Text="Created On" />
                            </td>
                            <td>
                                <Telerik:RadLabel ID="lblCreatedOn" runat="server"></Telerik:RadLabel>
                            </td>
                            <td>
                                <Telerik:RadLabel ID="ltrlClosedOn" runat="server" Text="Closed On" />
                            </td>
                            <td>
                                <Telerik:RadLabel ID="lblclosedon" runat="server"></Telerik:RadLabel>
                            </td>
                            <td>
                                <Telerik:RadLabel ID="ltrlClosedBy" runat="server" Text="Closed By" />
                            </td>
                            <td>
                             <Telerik:RadLabel ID="lblClosedByID" runat="server" Visible=false></Telerik:RadLabel>
                                <Telerik:RadLabel ID="lblClosedBy" runat="server"></Telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                               <Telerik:RadLabel ID="lblSystemName" runat="server" Text="System Name" /> 
                            </td>
                            <td>
                                <Telerik:RadTextBox ID="txtSystemName" runat="server" CssClass="input"></Telerik:RadTextBox>
                            </td>
                            <td>
                                <Telerik:RadLabel ID="lblDepartment" runat="server" Text="Department" /> 
                            </td>
                            <td>
                                <eluc:department id="ddlDepartmentList" runat="server" maxlength="100" appenddatabounditems="true"
                                    cssclass="input_mandatory" autopostback="true" />
                            </td>
                            <td valign="top">
                                <Telerik:RadLabel ID="lblStatus" runat="server" Text="Status" /> 
                                
                            </td>
                            <td>
                            <Telerik:RadLabel ID="lblStatusShortcode" runat="server" Visible="false"></Telerik:RadLabel>
                                <eluc:itstatus id="ddlITStatus" runat="server" appenddatabounditems="true" cssclass="input_mandatory"
                                    autopostback="true" OnTextChangedEvent='ddlITStatus_TextChanged' />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <Telerik:RadLabel ID="lblLoggedBy" runat="server" Text="Request User Name" /> 
                            </td>
                            <td valign="top">
                                <Telerik:RadTextBox ID="txtLoggedBy" runat="server" MaxLength="100" CssClass="input"></Telerik:RadTextBox>
                            </td>
                            <td>
                                <Telerik:RadLabel ID="lblCategory" runat="server" Text="Category" /> 
                            </td>
                            <td>
                                <eluc:category id="ddlCategoryType" runat="server" maxlength="100" appenddatabounditems="true"
                                    cssclass="input_mandatory" />
                            </td>
                            <td>
                                <Telerik:RadLabel ID="lblAttendBy" runat="server" Text="Attended By" /> 
                            </td>
                            <td>
                                <eluc:itteam id="ddlITTeam" runat="server" maxlength="100" appenddatabounditems="true"
                                    cssclass="input_mandatory" />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <Telerik:RadLabel ID="lblCallType" runat="server" Text="Call Type" />                                 
                            </td>
                            <td valign="top" colspan="3">
                                <Telerik:RadTextBox ID="txtCallType" runat="server" MaxLength="200" CssClass="input_mandatory" Width="90%"> 
                                </Telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <Telerik:RadLabel ID="lblActionTaken" runat="server" Text="Action Taken" />                                 
                            </td>
                            <td valign="top" colspan="3">
                                <Telerik:RadTextBox ID="txtActionTaken" runat="server" MaxLength="200" CssClass="input" Width="90%"></Telerik:RadTextBox>
                            </td>
                        </tr>
                      
                        <tr>
                            <td valign="top">
                                <Telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks" />
                            </td>
                            <td valign="top" colspan="3">
                                <Telerik:RadTextBox ID="txtRemarks" runat="server" MaxLength="500" TextMode="MultiLine" Width="90%"
                                    Rows="5" Columns="100" CssClass="input"></Telerik:RadTextBox>
                            </td>
                            <td rowspan="4" />
                        </tr>
                    </table>
 </telerik:RadAjaxPanel>
    </form>
</body>
</html>
