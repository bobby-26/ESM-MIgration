<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionIncidentRaiseMachineryDamage.aspx.cs" Inherits="InspectionIncidentRaiseMachineryDamage" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Category" Src="~/UserControls/UserControlIncidentNearMissCategory.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SubCategory" Src="~/UserControls/UserControlIncidentNearMissSubCategory.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Raise Machinery Damage / Failure</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuLocation" runat="server" OnTabStripCommand="MenuLocation_TabStripCommand"></eluc:TabStrip>
        <table id="tblFiles" width="75%">
            <tr>
                <td style="width: 30%">Category
                </td>
                <td>
                    <eluc:Category ID="ucCategory" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                        Width="240px" CssClass="input_mandatory" OnTextChangedEvent="ucCategory_Changed" />
                </td>
            </tr>
            <tr>
                <td>Sub Category
                </td>
                <td>
                    <eluc:SubCategory ID="ucSubcategory" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                        Width="240px" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <br />
                    <telerik:RadLabel ID="lblMessage" Width="85%" runat="server" ForeColor="Red" Font-Bold="true"
                        Font-Size="Small" Style="word-wrap: break-word;"></telerik:RadLabel>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>

