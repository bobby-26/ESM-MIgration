<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkflowProcessTransitionAdd.aspx.cs" Inherits="WorkflowProcessTransitionAdd" %>


<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Process" Src="~/UserControls/UserControlWFProcess.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ProcessState" Src="~/UserControls/UserControlWFProcessState.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ProcessTarget" Src="~/UserControls/UserControlWFProcessTarget.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ProcessGroup" Src="~/UserControls/UserControlWFProcessGroup.ascx" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Workflow - Process Transition Add</title>

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
            <eluc:TabStrip ID="MenuWorkflowProcessTransitionAdd" runat="server" OnTabStripCommand="MenuWorkflowProcessTransitionAdd_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <table style="margin-left: 20px">

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
                        <telerik:RadLabel ID="RadLabel3" runat="server" Text="Row Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number runat="server" ID="txtrownumber" CssClass="input_mandatory" MaxLength="2" Width="120px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel4" runat="server" Text="Current State" Width="120px"></telerik:RadLabel>
                    </td>

                    <td>
                        <eluc:ProcessState ID="UcProcessCurrentState" runat="server" AutoPostBack="True" Width="150px"
                            CssClass="input_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel5" runat="server" Text="Next State" Width="120px"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:ProcessState ID="UcProcessNextState" runat="server" AutoPostBack="True" Width="150px"
                            CssClass="input_mandatory" />
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel7" runat="server" Text="Group" Width="120px"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:ProcessGroup ID="UcProcessGroupAdd" runat="server" AutoPostBack="True" Width="150px" CssClass="input_mandatory" />
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel6" runat="server" Text="Target" Width="120px"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:ProcessTarget ID="UcProcessTargetAdd" runat="server" AutoPostBack="True" Width="150px" CssClass="input_mandatory" />
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
