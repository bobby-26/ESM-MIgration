<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardHSEQAPlanReport.aspx.cs" Inherits="Dashboard_DashboardHSEQAPlanReport" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reporting a Plan</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
    <form id="form1" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
            EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1" />

        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="attachments">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="reportform" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>

        <div style="margin-left: 0px">
            <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Width="100%" Orientation="Horizontal" Height="600px">
                <telerik:RadPane ID="generalPane" runat="server" Scrolling="Y">
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
                    <eluc:TabStrip ID="Hseqaplanreporttab" runat="server" OnTabStripCommand="Hseqaplanreporttab_TabStripCommand"
                        TabStrip="true" />
                    <br />
                    <table style="margin-left: 20px; text-wrap: normal;" id="reportform">
                        <tr>
                            <td>
                                <telerik:RadLabel runat="server" Text="LI" />
                            </td>
                            <th>&nbsp &nbsp &nbsp</th>
                            <td>
                                <telerik:RadLabel runat="server" ID="radlblliname" />
                            </td>
                            <td>&nbsp &nbsp &nbsp
                            </td>
                            <td>
                                <telerik:RadLabel runat="server" Text="Interval" />
                            </td>
                            <th>&nbsp &nbsp &nbsp</th>
                            <td>
                                <telerik:RadLabel runat="server" ID="radlblinterval" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel runat="server" Text="Planned Date" />
                            </td>
                            <th>&nbsp &nbsp &nbsp
                            </th>
                            <td>
                                <telerik:RadLabel runat="server" ID="radlblPlanneddate" />
                            </td>
                            <td>&nbsp &nbsp &nbsp
                            </td>
                            <td>
                                <telerik:RadLabel runat="server" Text="Done Date" />
                            </td>
                            <th>&nbsp &nbsp &nbsp</th>
                            <td>
                                <eluc:Date ID="radlastdonedate" runat="server" CssClass="input_mandatory" />
                                <telerik:RadTextBox ID="lastdonecheck" runat="server" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <br />
                            </td>
                        </tr>
                        <tr>
                           
                           
                            <td>
                                <telerik:RadLabel runat="server" Text="Remarks" />
                            </td>
                            <th>&nbsp &nbsp &nbsp</th>
                            <td>
                                <telerik:RadTextBox ID="tbremarksentry" TextMode="MultiLine" Columns="2" runat="server" Width="250px" MaxLength="500"></telerik:RadTextBox>
                            </td>
                             <th>&nbsp &nbsp &nbsp</th>
                             <td>
                                <telerik:RadLabel runat="server" ID="Noattachmentreasontitle" Text="Reason" />
                            </td>
                            <th>
                                <telerik:RadLabel runat="server" ID="Noattachmentreasontitle1" Text=":&nbsp &nbsp &nbsp"></telerik:RadLabel>
                            </th>
                            <td>
                                <telerik:RadTextBox ID="reasonfornoattachments" TextMode="MultiLine" Columns="2" runat="server" EmptyMessage="Mention the Reason for no attachments if you didn't provide any." Width="250px" MaxLength="500"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <br />
                            </td>
                        </tr>
                    </table>
                    <eluc:Confirm ID="ucConfirm" runat="server" OKText="Ok"
                        CancelText="Cancel" Visible="false" />
                    <asp:Button ID="confirm" runat="server" Text="confirm" OnClick="confirm_Click" />
                </telerik:RadPane>
                <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Both"></telerik:RadSplitBar>
                <telerik:RadPane ID="RadPane1" runat="server" Scrolling="Y" MinHeight="80">
                    <iframe runat="server" id="attachments" scrolling="Y" style="min-height: 800px; width: 100%" frameborder="0"></iframe>
                </telerik:RadPane>
            </telerik:RadSplitter>
        </div>
    </form>
</body>
</html>
