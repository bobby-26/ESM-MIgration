<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkflowStateAdd.aspx.cs" Inherits="WorkflowStateAdd" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="StateType" Src="~/UserControls/UserControlWFStateType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="State" Src="~/UserControls/UserControlWFState.ascx" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Workflow - State Add</title>

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
            <eluc:TabStrip ID="MenuWorkFlowStateAdd" runat="server" OnTabStripCommand="MenuWorkFlowStateAdd_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <table style="margin-left: 20px">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblstateType" runat="server" Text="State Type"></telerik:RadLabel>
                        &nbsp;&nbsp;
                    </td>

                    <td>
                        <eluc:StateType ID="UcStateType" runat="server" AutoPostBack="true" Width="120px" />
                    </td>
                </tr>
          
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblShortCode" runat="server" Text="Short Code"></telerik:RadLabel>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtShortCode" runat="server" Text="" Width="120px" CssClass="input_mandatory"></telerik:RadTextBox>&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="Name"></telerik:RadLabel>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtName" runat="server" Text="" CssClass="input_mandatory" Width="160px"></telerik:RadTextBox>&nbsp;&nbsp;
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel2" runat="server" Text="Description"></telerik:RadLabel>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDescription" runat="server" Text="" TextMode="MultiLine" Width="240px" Rows="6" CssClass="input_mandatory"></telerik:RadTextBox>&nbsp;&nbsp;
                    </td>
                </tr>
            </table>



        </telerik:RadAjaxPanel>

        <div>
        </div>
    </form>
</body>
</html>
