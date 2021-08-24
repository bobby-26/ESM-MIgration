<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterCrewApprovalSubCategoryAdd.aspx.cs" Inherits="Registers_RegisterCrewApprovalSubCategoryAdd" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="CommonToolTip" Src="~/UserControls/UserControlCommonToolTip.ascx" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Category Level </title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <telerik:RadSkinManager runat="server" ID="RadSkinManager1"></telerik:RadSkinManager>
            <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuRemarks" runat="server" OnTabStripCommand="MenuRemarks_TabStripCommand"></eluc:TabStrip>
            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblapprovalcat" runat="server" Text="Approval Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="lblapprovalcatname" runat="server" Width="200PX" Text=""></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblcode" Text="Code" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtcode" Width="200PX" runat="server"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel1" Text="Category Level"  runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtcatlevelname" Width="200PX" runat="server"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblapplyto" Text="Applies to" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <div  class="input_mandatory">
                            <asp:CheckBoxList ID="chkRankApplicable" RepeatDirection="Vertical" RepeatColumns="3" Enabled="true"
                                runat="server">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
