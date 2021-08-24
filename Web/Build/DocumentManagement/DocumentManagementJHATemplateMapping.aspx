<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementJHATemplateMapping.aspx.cs" Inherits="DocumentManagement_DocumentManagementJHATemplateMapping" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Category" Src="~/UserControls/UserControlRACategory.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Import JHA</title>
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
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
         <eluc:TabStrip ID="MenuTemplateMapping" runat="server" OnTabStripCommand="MenuTemplateMapping_TabStripCommand" Title="Import JHA"></eluc:TabStrip>
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
                            <telerik:RadCheckBoxList ID="cblJHA" runat="server" CssClass="Checkbox" Direction="Vertical" AutoPostBack="true"
                                Columns="5">
                            </telerik:RadCheckBoxList>
                        </td>
                    </tr>
                </table>
          </telerik:RadAjaxPanel>
    </form>
</body>
</html>















<%--

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Category" Src="~/UserControls/UserControlRACategory.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Import JHA</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmTemplateMapping" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" Localization-OK="Yes" Localization-Cancel="No" Width="100%">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="tbljha" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:Status runat="server" ID="ucStatus" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <telerik:RadLabel ID="lblImportJHA" runat="server" Text="Import JHA"></telerik:RadLabel>
<       <eluc:TabStrip ID="MenuTemplateMapping" runat="server" OnTabStripCommand="MenuTemplateMapping_TabStripCommand"></eluc:TabStrip>
        <table id="tbljha" runat="server" cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td width="10%">
                    <b><telerik:RadLabel ID="lblCategory" runat="server" Text="Category"></telerik:RadLabel></b>
                </td>
                <td width="90%">
                    <telerik:RadComboBox ID="ddlCategory" runat="server" AppendDataBoundItems="true" Width="240px"
                        AutoPostBack="True" OnTextChanged="ddlCategory_Changed" DataTextField="FLDNAME"
                        DataValueField="FLDACTIVITYID" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                        <Items>
                            <telerik:RadComboBoxItem Text="--Select--" Value="DUMMY"></telerik:RadComboBoxItem>
                        </Items>
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:CheckBox ID="chkCheckAll" runat="server" Text="Check All" AutoPostBack="true"
                        OnCheckedChanged="SelectAll" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:CheckBoxList ID="cblJHA" runat="server" RepeatLayout="Table" RepeatDirection="Horizontal" AutoPostBack="true"
                RepeatColumns="6" CssClass="input">
            </asp:CheckBoxList>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>

--%>