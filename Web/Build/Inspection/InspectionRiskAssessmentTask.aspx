<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRiskAssessmentTask.aspx.cs" Inherits="InspectionRiskAssessmentTask" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MCUser" Src="~/UserControls/UserControlMultiColumnUser.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Crew" Src="~/UserControls/UserControlCrewList.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Risk Assessment Task</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
            <%: Scripts.Render("~/bundles/js") %>
            <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInspectionMapping" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:TabStrip ID="MenuMapping" runat="server" OnTabStripCommand="MenuMapping_TabStripCommand" Title="Risk Assessment Task"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
            <div id="divFind">
                <table width="100%">
                    <tr>
                        <td width="15%">
                            <telerik:RadLabel ID="lblDepartmentType" runat="server" Text="Type"></telerik:RadLabel>
                        </td>
                        <td width="35%">
                            <telerik:RadRadioButtonList ID="rblDepartmentType" runat="server" AutoPostBack="true"
                                Direction="Horizontal" OnSelectedIndexChanged="rblDepartmentType_SelectedIndexChanged">
                                <Items>
                                    <telerik:ButtonListItem Text="Ship" Value="1" Selected="True" />
                                    <telerik:ButtonListItem Text="Office" Value="2" />
                                </Items>
                            </telerik:RadRadioButtonList>
                        </td>
                        <td width="15%">
                            <telerik:RadLabel ID="lblstatus" runat="server" Text="Status"></telerik:RadLabel>
                        </td>
                        <td width="35%">
                            <telerik:RadTextBox ID="txtstatus" runat="server" Width="180px" ReadOnly="true" Enabled="false">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblTask" runat="server" Text="Task"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtTask" runat="server" Width="360px"
                                TextMode="MultiLine" Rows="2" Resize="Both">
                            </telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lbltargetdate" runat="server" Text="Target Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="uctargetdate" CssClass="input_mandatory" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblPIC" runat="server" Text="Responsiblity"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:MCUser ID="ucUser" runat="server" Width="270px" emailrequired="false" ItemsPerRequest="50" designationrequired="true" />
                            <eluc:Crew ID="ucCrew" runat="server" Width="270px" CssClass="input_mandatory" />                       
                        </td>
                        <td colspan="2"></td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblCompletionRemarks" runat="server" Text="Completion Remarks"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtCompletionRemarks" runat="server" Width="360px"
                                TextMode="MultiLine" Rows="2" Resize="Both">
                            </telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblcompletiondate" runat="server" Text="Completion Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="uccompletiondate" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
