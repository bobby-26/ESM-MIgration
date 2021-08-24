<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionPSCCopyMOURegisterData.aspx.cs" Inherits="Inspection_InspectionPSCCopyMOURegisterData" %>

<%@ Import Namespace="SouthNests.Phoenix.DocumentManagement" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TreeView" Src="~/UserControls/UserControlTreeView.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VerticalSplit" Src="~/UserControls/UserControlVerticalSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Flag" Src="~/UserControls/UserControlFlag.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Copy MOU</title>
    <telerik:RadCodeBlock runat="server" ID="DivHeader">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/Fonts/fontawesome/css/all.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmDocumentCategory" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <eluc:Status runat="server" ID="ucStatus" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuDocumentCategoryMain" Title="" OnTabStripCommand="MenuDocumentCategoryMain_TabStripCommand" runat="server"></eluc:TabStrip>
        <telerik:RadSplitter RenderMode="Lightweight" BorderColor="White" ID="RadSplitter1" runat="server" Height="100%" Width="100%">
            <telerik:RadPane ID="navigationPane" runat="server" Width="45%" Scrolling="None" Height="100%">
                <table width="75%" id="tblcontent" cellpadding="5" runat="server">
                    <tr>
                    <td>
                        <asp:Label ID="lblpscmoufrom" Width="200px"  runat="server" Text="PSC MOU From"></asp:Label>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlCompanyfrm" runat="server" Width="200px" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">                                
                            </telerik:RadComboBox>
                    </td>
                    </tr>
                    <tr>
                    <td>
                        <asp:Label ID="Label1" Width="200px"  runat="server" Text="PSC MOU To"></asp:Label>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlCompanyto" runat="server" Width="200px" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">                                
                            </telerik:RadComboBox>
                    </td>
                    </tr>
                </table>
            </telerik:RadPane>

        </telerik:RadSplitter>
    </form>
</body>
</html>
