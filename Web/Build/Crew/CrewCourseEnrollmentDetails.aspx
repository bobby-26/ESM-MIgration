<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewCourseEnrollmentDetails.aspx.cs"
    Inherits="CrewCourseEnrollmentDetails" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Batch" Src="~/UserControls/UserControlBatch.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressType.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew EnrollmentDetials</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmEnrollmentDetials" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlSignOn">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <asp:Literal ID="lblCourseEnrollmentDetails" runat="server" Text="Course Enrollment Details"></asp:Literal>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                    <eluc:TabStrip ID="MenuCourseEnrollmentDetails" runat="server" OnTabStripCommand="CourseEnrollmentDetails_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblEmployeeCode" runat="server" Text="Employee Code"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmployeeCode" runat="server" CssClass="readonlytextbox" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblEmployeeName" runat="server" Text="Employee Name"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmployeeName" runat="server" MaxLength="50" CssClass="readonlytextbox"
                                ReadOnly="True" Width="180px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblEmail" runat="server" Text="Email"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmail" runat="server" MaxLength="20" CssClass="readonlytextbox"
                                ReadOnly="True" Width="180px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblRank" runat="server" Text="Rank"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRank" runat="server" MaxLength="20" CssClass="readonlytextbox"
                                ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblPriority" runat="server" Text="Priority"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Number ID="txtPriority" runat="server" CssClass="input_mandatory txtNumber"
                                IsInteger="true" MaxLength="1" />
                            <font color="blue"><asp:Literal ID="lbl1Urgent2Low" runat="server" Text="(1 - Urgent, 2 - Low)"></asp:Literal></font>
                        </td>
                        <td>
                            <asp:Literal ID="lblCost" runat="server" Text="Cost"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Number ID="txtCost" runat="server" CssClass="input txtNumber" MaxLength="10"
                                IsInteger="false" Width="80px" />
                            <eluc:Currency runat="server" ID="ucCurrency" AppendDataBoundItems="true" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblSelectCostCenter" runat="server" Text="Select Cost Center"></asp:Literal>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblCostCenter" runat="server" RepeatDirection="Horizontal"
                                AutoPostBack="true" OnSelectedIndexChanged="PrincipalManagerClick">
                                <asp:ListItem Value="1" Selected="true">Seafarer</asp:ListItem>
                                <asp:ListItem Value="2">Principal</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td>
                            <asp:Literal ID="lblRemarks" runat="server" Text="Remarks"></asp:Literal>
                        </td>
                        <td colspan="5">
                            <asp:TextBox ID="txtRemarks" runat="server" CssClass="input" TextMode="MultiLine"
                                MaxLength="200" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblCostCenter" runat="server" Text="Cost Center"></asp:Literal>
                        </td>
                        <td>
                            <eluc:AddressType runat="server" ID="ucPrincipal" AddressType="128" CssClass="input"
                                AppendDataBoundItems="true" Width="80%" />
                        </td>
                        <td>
                            <asp:Literal ID="lblBatchNo" runat="server" Text="Batch No"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Batch ID="ucBatch" runat="server" CssClass="input" AppendDataBoundItems="true" IsOutside="true" />
                        </td>
                    </tr>
                </table>
                <eluc:Status ID="ucStatus" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
