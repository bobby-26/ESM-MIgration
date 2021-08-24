<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersFMSFileNoAdd.aspx.cs"
    Inherits="RegistersFMSFileNoAdd" MaintainScrollPositionOnPostback="true" %>

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
        <eluc:TabStrip ID="MenuFMSFileNo" runat="server" OnTabStripCommand="FMSFileNo_TabStripCommand"
            TabStrip="true"></eluc:TabStrip>
        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td width="11.23%">
                    <telerik:RadLabel ID="lblFileNo" runat="server" Text="File No">
                    </telerik:RadLabel>
                </td>
                <td width="63%">
                    <telerik:RadTextBox runat="server" ID="txtFileNo" MaxLength="6" Width="180px" CssClass="input_mandatory">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblFileDescription" runat="server" Text="Description">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtDescription" Rows="4" TextMode="MultiLine"
                        MaxLength="300" Width="360px" CssClass="input_mandatory">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblHint" runat="server" Text="Keyword">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtHint" runat="server" MaxLength="300" Rows="4" CssClass="input_mandatory"
                        TextMode="MultiLine" Width="360px">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblSource" runat="server" Text="Source Type">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlsource" runat="server" Width="200px" AppendDataBoundItems="true" AutoPostBack="true"
                        Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select source">
                    </telerik:RadComboBox>

                </td>
            </tr>
        </table>
    </form>
</body>
</html>
