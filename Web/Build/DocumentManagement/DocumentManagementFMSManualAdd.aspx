<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementFMSManualAdd.aspx.cs" Inherits="DocumentManagement_DocumentManagementFMSManualAdd" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>FMS File No</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <eluc:Status ID="ucStatus" runat="server" Text="" />
    <eluc:TabStrip ID="MenuFMSFileNo" runat="server" OnTabStripCommand="MenuFMSFileNo_TabStripCommand"
        TabStrip="true"></eluc:TabStrip>
    <table cellpadding="1" cellspacing="1" width="100%">
        <tr>
            <td style="padding-right: 30px">
                <telerik:RadLabel ID="lblManualCategory" runat="server" Text="Category">
                </telerik:RadLabel>
            </td>
            <td style="padding-right: 30px">
                <telerik:RadComboBox ID="ddlCategory" runat="server" AutoPostBack="true" AppendDataBoundItems="true"
                    OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" Width="270px" Filter="Contains" CssClass="input_mandatory"
                    MarkFirstMatch="true" EmptyMessage="Type to select Category">
                </telerik:RadComboBox>
            </td>
        </tr>
        <tr>
            <td style="padding-right: 30px">
                <telerik:RadLabel ID="lblManualSubcategory" runat="server" Text="Sub Category">
                </telerik:RadLabel>
            </td>
            <td style="padding-right: 30px">
                <telerik:RadComboBox ID="ddlSubCategory" Width="270px" runat="server" Filter="Contains"
                    EmptyMessage="Type to Select" CssClass="input_mandatory">
                </telerik:RadComboBox>
            </td>
        </tr>
        <tr>
            <td width="11.23%">
                <telerik:RadLabel ID="lblManualNo" runat="server" Text="Manual No">
                </telerik:RadLabel>
            </td>
            <td width="63%">
                <telerik:RadTextBox runat="server" ID="txtManualno" Width="360px"
                    CssClass="input_mandatory">
                </telerik:RadTextBox>
            </td>
        </tr>
        <tr>
            <td>
                <telerik:RadLabel ID="lblManualName" runat="server" Text="Manual Name">
                </telerik:RadLabel>
            </td>
            <td>
                <telerik:RadTextBox runat="server" ID="txtManualName" Rows="2" TextMode="MultiLine"
                    MaxLength="300" Width="360px" CssClass="input_mandatory">
                </telerik:RadTextBox>
            </td>
        </tr>        
    </table>
    </form>
</body>
</html>