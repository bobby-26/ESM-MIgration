<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceGlobalComponentRAMapping.aspx.cs" Inherits="PlannedMaintenanceGlobalComponentRAMapping" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Category" Src="~/UserControls/UserControlRACategory.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Map Risk Assessment</title>
     <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

           <style type="text/css">        .RadCheckBox.Checkbox {            width: 99% !important;        }        .rbText.Checkbox {            text-align: left;            width: 89% !important;        }        .rbVerticalList.Checkbox {            width: 19% !important;        }    </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmTemplateMapping" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager ID="RadWindowManager" runat="server" RenderMode="Lightweight"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
         <eluc:TabStrip ID="MenuTemplateMapping" runat="server" OnTabStripCommand="MenuTemplateMapping_TabStripCommand" Title="Map Risk Assessment"></eluc:TabStrip>
                <eluc:Status runat="server" ID="ucStatus" />
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>               
                <table width="100%">
                    <tr>
                        <td width="10%">
                            <telerik:RadLabel ID="lblCategory" runat="server" Text="Activity"></telerik:RadLabel>
                        </td>
                        <td width="90%">
                            <telerik:RadComboBox ID="ddlCategory" runat="server" CssClass="input" AppendDataBoundItems="true"
                                AutoPostBack="True" OnTextChanged="ddlCategory_Changed" DataTextField="FLDNAME"
                                DataValueField="FLDACTIVITYID" Width="270px" EmptyMessage="Type to select Activity" Filter="Contains" MarkFirstMatch="true">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <telerik:RadCheckBox ID="chkCheckAll" Font-Bold="true" runat="server" Text="Check All" AutoPostBack="true"
                                OnCheckedChanged="SelectAll" />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <telerik:RadCheckBoxList ID="cblRA" runat="server" CssClass="Checkbox" Direction="Vertical" AutoPostBack="true"
                                Columns="5">
                            </telerik:RadCheckBoxList>
                        </td>
                    </tr>
                </table>
          </telerik:RadAjaxPanel>
    </form>
</body>
</html>
