<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionPSCMOUSRPPriorityDurationAdd.aspx.cs" Inherits="Inspection_InspectionPSCMOUSRPPriorityDurationAdd" %>

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
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SRP Duration</title>
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
                <table width="75%" id="tblcontent" cellpadding="5" runat="server">
                    <tr>
                    <td>
                        <asp:Label ID="lblpscmou" Width="200px"  runat="server" Text="PSC MOU"></asp:Label>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlCompany" runat="server" Width="150px" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">                                
                            </telerik:RadComboBox>
                    </td>
                    </tr>
                    <tr>
                    <td>
                        <asp:Label ID="lblrisk" Width="200px"  runat="server" Text="Risk"></asp:Label>
                        </td>
                        <td>
                             <telerik:RadComboBox ID="ddlRiskAdd" runat="server" Width="150px" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true"></telerik:RadComboBox>
                    </td>
                    </tr>
                    <tr>
                    <td>
                        <asp:Label ID="lblDurationFrom" Width="200px"  runat="server" Text="Duration From"></asp:Label>
                        </td>
                        <td>
                            <eluc:Number ID="txtdurationfrom" runat="server" CssClass="gridinput_mandatory" IsInteger="true"
                                MaxLength="3" />
                    </td>
                    </tr>
                    <tr>
                    <td>
                        <asp:Label ID="lblDurationTo" Width="200px"  runat="server" Text="Duration To"></asp:Label>
                        </td>
                        <td>
                            <eluc:Number ID="txtdurationto" runat="server" CssClass="gridinput_mandatory" IsInteger="true"
                                MaxLength="3" />

                    </td>
                    </tr>
                    <tr>
                    <td>
                        <asp:Label ID="lblpriority" Width="200px"  runat="server" Text="Priority"></asp:Label>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlprioritylvlAdd" runat="server" Width="150px" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">          
                            </telerik:RadComboBox>
                    </td>
                    </tr>
                   <tr>
                    <td>
                        <asp:Label ID="lblremarks" Width="200px"  runat="server" Text="Remarks"></asp:Label>
                        </td>
                        <td>
                             <telerik:RadTextBox ID="txtremarks" runat="server" Text=""></telerik:RadTextBox>
                    </td>
                    </tr>
                </table>
    </form>
</body>
</html>

