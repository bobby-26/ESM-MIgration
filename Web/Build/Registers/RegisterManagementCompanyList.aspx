<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterManagementCompanyList.aspx.cs" Inherits="RegisterManagementCompanyList" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Management Company List</title>
     <telerik:RadCodeBlock ID="Radcodeblock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1"></telerik:RadAjaxLoadingPanel>
        <telerik:RadAjaxPanel ID="radajaxpanel1" runat="server" height="60%">
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
                    <eluc:TabStrip ID="MenuRankList" runat="server" OnTabStripCommand="MenuRankList_TabStripCommand">
              </eluc:TabStrip>
             <table width="100%">
                        <tr>
                            <td colspan="2">
                                <div id="dvRanklist" runat="server" class="input" style="overflow: auto; width: 99%;
                                    left:1%; position: absolute; height: 90%;">
                                    <telerik:RadCheckBoxList ID="ddlcompany" runat="server"  Columns="1">
                                    </telerik:RadCheckBoxList>
                                </div>
                            </td>
                       </tr>
                 </table>
            </telerik:RadAjaxPanel>

    </form>
</body>
</html>
