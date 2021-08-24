<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaBatchList.aspx.cs"
    Inherits="PreSeaBatchList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="../UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Institution" Src="~/UserControls/UserControlInstitution.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Batch List</title>
   <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

   </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmBatchList" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <div>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader" style="position: relative">
            <div id="divHeading" style="vertical-align: top">
                Batch
            </div>
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuBatchList" runat="server" OnTabStripCommand="BatchList_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <table cellpadding="1" cellspacing="1" width="50%">
            <tr>
                <td>
                    Course
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddlCourse" DataTextField="FLDPRESEACOURSENAME" CssClass="dropdown_mandatory"
                        DataValueField="FLDPRESEACOURSEID" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                </td>
            </tr>
            <tr>
                <td>
                    Batch
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtBatch" runat="server" CssClass="input_mandatory" MaxLength="50"
                        Width="120px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                </td>
            </tr>
            <tr>
                <td>
                    From Date
                </td>
                <td colspan="3">
                    <eluc:Date runat="server" ID="txtFromDate" CssClass="input_mandatory" AutoPostBack="true"
                        Width="120px" OnTextChangedEvent="CalculateDuration" />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                </td>
            </tr>
            <tr>
                <td>
                    To Date
                </td>
                <td colspan="3">
                    <eluc:Date runat="server" ID="txtToDate" CssClass="input_mandatory" AutoPostBack="true"
                        Width="120px" OnTextChangedEvent="CalculateDuration" />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                </td>
            </tr>
            <tr>
                <td>
                    Duration(In Days)
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtDuration" runat="server" CssClass="readonlytextbox" MaxLength="50"
                        Width="60px" Style="text-align: right" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                </td>
            </tr>
            <tr>
                <td>
                    Age Limit
                </td>
                <td colspan="3">
                    <eluc:Number ID="txtAGELIMIT" runat="server" MaxLength="2" CssClass="input" />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                </td>
            </tr>
            <tr>
                <td>
                    Registration Fee
                </td>
                <td colspan="3">
                    <eluc:Number ID="txtRegFees" runat="server" CssClass="input txtNumber" MaxLength="8"
                        DecimalPlace="2" IsInteger="false" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
