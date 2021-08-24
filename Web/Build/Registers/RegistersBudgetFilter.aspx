<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersBudgetFilter.aspx.cs"
    Inherits="RegistersBudgetFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
         <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
         <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
 <%--   <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
           --%>
          <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
 
        <eluc:TabStrip ID="MenuMain" runat="server" OnTabStripCommand="MenuMain_TabStripCommand" TabStrip="true">
        </eluc:TabStrip>
   
    <telerik:RadAjaxPanel runat="server" ID="pnlBudgetEntry">
        
           <%-- <div id="divFind">--%>
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblBudgetCode" runat="server" Text="Budget Code"></asp:Literal>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtAccountCodeSearch" MaxLength="20" CssClass="input"
                                Width="150px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblDescription" runat="server" Text="Description"></asp:Literal>
                        </td>
                        <td>
                           <telerik:RadTextBox runat="server" ID="txtDescription" MaxLength="200" CssClass="input"
                                Width="240px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                    
                     <td>
                            <asp:Literal ID="lblGroup" runat="server" Text="Group"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Hard ID="ucGroupSearch" runat="server" CssClass="input" AppendDataBoundItems="true" />
                        </td>       
                        <td>
                            <asp:Literal ID="Isactive" runat="server" Text="Active(Yes/No)"></asp:Literal>
                        </td>
                        <td>
                            <telerik:RadCheckBox ID="chkactive" runat="server" Checked="true"></telerik:RadCheckBox>
                        </td>                  
                    </tr>
                </table>
            <%--</div>--%>
      
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
