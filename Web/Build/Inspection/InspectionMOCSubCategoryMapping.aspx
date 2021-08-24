<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionMOCSubCategoryMapping.aspx.cs"
    Inherits="InspectionMOCSubCategoryMapping" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Category Mapping</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <div id="InspectionMapping" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        </div>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInspectionMapping" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlInspectionMapping">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
            <eluc:TabStrip ID="MenuMapping" runat="server" OnTabStripCommand="MenuMapping_TabStripCommand">
            </eluc:TabStrip>
            <telerik:RadFormDecorator ID="rfdinstruction" RenderMode="LightWeight" runat="server"
                DecoratedControls="All" EnableRoundedCorners="true" DecorationZoneID="divFind">
            </telerik:RadFormDecorator>
            <div id="divFind" style="position: relative; z-index: 2">
                <table width="100%">
                    <tr>
                        <td>
                            &nbsp;&nbsp<asp:Label ID="lblType" runat="server" Font-Bold="true" Text="Description"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtmochardname" runat="server" ReadOnly="true" Width="300px" CssClass="readonlytextbox"></asp:TextBox>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="2">
                            &nbsp;&nbsp<asp:Label ID="lblSource" runat="server" Font-Bold="true" Text="Category"></asp:Label>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="2">
                            <div id="dvtype" runat="server" class="input" style="overflow: auto; width: 70%;
                                left: 1%; position: absolute; height: 350px;">
                                <asp:CheckBoxList ID="cbltypelist" runat="server" RepeatDirection="Vertical" RepeatColumns="1">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
