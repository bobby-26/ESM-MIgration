<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkflowActionAdd.aspx.cs" Inherits="WorkflowActionAdd" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ActionType" Src="~/UserControls/UserControlWFActionType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Action" Src="~/UserControls/UserControlWFAction.ascx" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Workflow - Action Add</title>

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
            <eluc:TabStrip ID="MenuWorkflowActionAdd" runat="server" OnTabStripCommand="MenuWorkflowActionAdd_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <table style="margin-left: 20px">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblstateType" runat="server" Text="Action Type"></telerik:RadLabel>

                    </td>
                    <td>
                        <eluc:ActionType ID="UcActionType" runat="server" AutoPostBack="true" AppendDataBoundItems="true" Width="120px" />
                    </td>
                </tr>

               <%-- <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel3" runat="server" Text="Action"></telerik:RadLabel>
                    </td>
                    <td>

                        <eluc:Action ID="UcAction" runat="server" AutoPostBack="True" AppendDataBoundItems="True" Width="120px" ActionTypeId="14E29BB1-4610-EA11-AA24-AB1100CA8520"
                            ActionTypeList='<%# PhoenixWorkflow.WORKFLOWACTIONLIST(General.GetNullableGuid(("14E29BB1-4610-EA11-AA24-AB1100CA8520").ToString())) %>' />
                    </td>
                </tr>--%>


                <tr>
                    <td>
                        <telerik:RadLabel ID="lblShortCode" runat="server" Text="Short Code"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtShortCode" runat="server" Text="" CssClass="input_mandatory" Width="120px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtName" runat="server" Text="" CssClass="input_mandatory" Width="160px"></telerik:RadTextBox>
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
