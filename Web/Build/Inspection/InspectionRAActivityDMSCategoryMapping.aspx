<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRAActivityDMSCategoryMapping.aspx.cs" Inherits="InspectionRAActivityDMSCategoryMapping" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmMSCATMapping" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlContactType">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading" style="vertical-align: top">
                        <eluc:Title runat="server" ID="ucTitle" Text="DMS Category Mapping" ShowMenu="false"></eluc:Title>
                    </div>
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuMapping" runat="server" OnTabStripCommand="MenuMapping_TabStripCommand">
                    </eluc:TabStrip>
                </div>               
                <div id="divFind" style="position: relative; z-index: 2">
                    <table width="50%" id="tblMapping">
                        <tr>
                            <td>
                                <asp:Literal ID="lblDMSCategory" runat="server" Text="DMS Category"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlDmsCategory" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                                    Width="70%" OnTextChanged="rdoDMScategory_Changed">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rdoList" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" CellSpacing="2"
                                    RepeatColumns="2" OnSelectedIndexChanged="rdoDMScategory_Changed">
                                    <asp:ListItem Text="JHA" Value="0" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="RA" Value="1"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td colspan="2">
                                <asp:Label ID="lblSource" runat="server" Font-Bold="true" Text="Category"></asp:Label>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td colspan="2">
                                <asp:CheckBoxList ID="cblSource" runat="server" RepeatDirection="Horizontal" RepeatColumns="2"
                                    CellPadding="2">
                                </asp:CheckBoxList>
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
