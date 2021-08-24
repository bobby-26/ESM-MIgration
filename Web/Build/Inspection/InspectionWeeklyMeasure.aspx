<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionWeeklyMeasure.aspx.cs" Inherits="InspectionWeeklyMeasure" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Weekly Measure</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmAuditSummary" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel ID="pnlSummaryNC" runat="server">
        <ContentTemplate>
            <eluc:Error ID="ucError" Visible="false" runat="server" Text="" />
                    <eluc:TabStrip ID="MenuAuditSummaryNC" runat="server" OnTabStripCommand="MenuAuditSummaryNC_TabStripCommand">
                    </eluc:TabStrip>
                  <table width="100%">
                      <tr>
                          <td>
                              <asp:Literal ID="lblGridISM" runat="server" Text=""></asp:Literal>
                          </td>
                      </tr>                      
                </table>
<%--                <br />
                <table cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <asp:Label ID="lblManual" runat="server" Text="* Count Combination - Current week / Last week" Font-Bold="true" Font-Italic="true"></asp:Label>
                    </td>                    
                </tr>--%>
        </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
