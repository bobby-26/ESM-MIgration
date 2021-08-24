<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionPNIMedicalCaseClose.aspx.cs" Inherits="InspectionPNIMedicalCaseClose" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Close</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
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
    <form id="form1" runat="server">
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
            <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
               
                <asp:Button ID="confirm" runat="server" Text="confirm" OnClick="btnApprove_Click" />
                    <eluc:TabStrip ID="MenuClose" runat="server" OnTabStripCommand="MenuClose_TabStripCommand"></eluc:TabStrip>
           
                <table width="100%">
                    <tr>
                        <td width="30%">
                            <telerik:RadLabel ID="lblReasonForClosing" runat="server" Text="Reason for closing "></telerik:RadLabel>
                        </td>
                        <td width="70%">
                            <eluc:Hard ID="ddlReason" runat="server" AppendDataBoundItems="true" AutoPostBack="true" HardTypeCode="241" CssClass="input_mandatory" />
                        </td>
                    </tr>
                    <tr>
                        <td width="30%">
                            <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                        </td>
                        <td width="70%">
                            <telerik:RadTextBox ID="txtClosingRemarks" CssClass="input_mandatory" runat="server" TextMode="MultiLine" Rows="3" Width="300px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td width="30%">
                            <telerik:RadLabel ID="lblReasonReopening" runat="Server" Text="Reason for reopening"></telerik:RadLabel>
                        </td>
                        <td width="70%">
                            <telerik:RadTextBox ID="txtReasonReopening" CssClass="input" runat="server" TextMode="MultiLine" Rows="3" Enabled="false" Width="300px"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
                <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Proceed" Localization-Cancel="Cancel" Width="100%"></telerik:RadWindowManager>

            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
