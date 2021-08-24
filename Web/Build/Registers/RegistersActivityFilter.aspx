<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersActivityFilter.aspx.cs"
    Inherits="RegistersActivityFilter" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Activity Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />     
        <eluc:TabStrip ID="MenuActivityFilterMain" runat="server" OnTabStripCommand="ActivityFilterMain_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%">                        
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCode" runat="server" Text="Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtActivityCode" MaxLength="6" ></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtActivityName" MaxLength="200" ></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblGroup" runat="server" Text="Group"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucGroup" runat="server" AppendDataBoundItems="true"  />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLeave" runat="server" Text="Leave"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucLeave" runat="server" AppendDataBoundItems="true"  />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPayRollHeader" runat="server" Text="Pay Roll Header"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtPayRollHeader" MaxLength="50" ></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLevel" runat="server" Text="Level"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadMaskedTextBox runat="server" ID="txtLevel" CssClass="input_mandatory" Mask="###"></telerik:RadMaskedTextBox>
                        <%-- <telerik:RadTextBox runat="server" ID="txtLevel"  MaxLength="3"></telerik:RadTextBox>
                            <ajaxToolkit:MaskedEditExtender ID="maskeditLevelAdd" runat="server" TargetControlID="txtLevel"
                                OnInvalidCssClass="MaskedEditError" Mask="999" MaskType="Number" InputDirection="LeftToRight"
                                AutoComplete="false" />--%>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
