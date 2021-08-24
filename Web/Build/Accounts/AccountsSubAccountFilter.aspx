<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsSubAccountFilter.aspx.cs"
    Inherits="AccountsSubAccountFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title> <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
       
    </telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
          <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
                       
   
        <eluc:TabStrip ID="MenuMain" runat="server" OnTabStripCommand="MenuMain_TabStripCommand">
        </eluc:TabStrip>

                <table width="100%">
                    <tr>
                        <td>
                            <Telerik:RadLabel ID="lblBudgetCode" runat="server" Text="Budget Code"></Telerik:RadLabel>
                        </td>
                        <td>
                            <Telerik:RadTextBox runat="server" ID="txtAccountCodeSearch" MaxLength="20" CssClass="input"
                                Width="150px"></Telerik:RadTextBox>
                        </td>
                        <td>
                            <Telerik:RadLabel ID="lblDescription" runat="server" Text="Description"></Telerik:RadLabel>
                        </td>
                        <td>
                           <Telerik:RadTextBox runat="server" ID="txtDescription" MaxLength="200" CssClass="input"
                                Width="240px"></Telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
           
    </form>
</body>
</html>
