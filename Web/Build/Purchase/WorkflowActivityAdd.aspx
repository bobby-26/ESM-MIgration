<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkflowActivityAdd.aspx.cs" Inherits="WorkflowActivityAdd" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework"   %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ActivityType" Src="~/UserControls/UserControlWFActivityType.ascx"  %>
<%@ Register TagPrefix="eluc" TagName="Activity" Src="~/UserControls/UserControlWFActivity.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Workflow - Activity Add</title>

    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">

        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuWorkflowActivityAdd" runat="server" OnTabStripCommand="MenuWorkflowActivityAdd_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <table style="margin-left: 20px">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblActivityType" runat="server" Text="Activity Type"></telerik:RadLabel>

                    </td>
                    <td>
                        <eluc:ActivityType ID="UcActivityType" runat="server" AutoPostBack="true" AppendDataBoundItems="true"   Width="150px" CssClass="input_mandatory"/>
                    </td>
                </tr>

                <%--<tr>
                    <td>
                        <telerik:RadLabel ID="Label3" runat="server" Text="Activity"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Activity ID="UcActivity" runat="server" AutoPostBack="True" Width="150px"  AppendDataBoundItems="True" ActivityTypeId="FBF7C649-FF10-EA11-AA24-AB1200A9F089" 
                            ActionTypeList='<%# PhoenixWorkflow.WORKFLOWACTIVITYLIST(General.GetNullableGuid(("FBF7C649-FF10-EA11-AA24-AB1200A9F089").ToString())) %>' />

                    </td>
                </tr>--%>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblShortCode" runat="server" Text="Short Code"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtShortCode" runat="server" Text="" CssClass="input_mandatory" Width="160px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="Name"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtName" runat="server" Text="" Width="160px" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel2" runat="server" Text="Description"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDescription" runat="server" Text="" Width="240px" TextMode="MultiLine" Rows="6" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>


        <div>
        </div>
    </form>
</body>
</html>
