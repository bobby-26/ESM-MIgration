<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersFacultyList.aspx.cs"
    Inherits="RegistersFacultyList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Faculty List</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="ds" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmFacultyList" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <div>
        <eluc:Status runat="server" ID="ucStatus" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader" style="position: relative">
            <div id="divHeading" style="vertical-align: top">
                <asp:Literal ID="lblFacultyList" runat="server" Text="Faculty List"></asp:Literal>
            </div>
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuFacultyList" runat="server" OnTabStripCommand="FacultyList_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td>
                    <asp:Literal ID="lblFacultyCode" runat="server" Text="Faculty Code"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtCode" MaxLength="10" Width="120px" CssClass="input_mandatory"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblFacultyName" runat="server" Text="Faculty Name"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtFacultyName" MaxLength="100" Width="240px" CssClass="input_mandatory"></asp:TextBox>
                </td>
            </tr>
          
            <tr>
                <td>
                    <asp:Literal ID="lblCourses" runat="server" Text="Courses"></asp:Literal>
                </td>
                <td>
                    <div runat="server" id="dvCourse" class="input_mandatory" style="overflow: auto;
                        width: 60%; height: 230px">
                        <asp:CheckBoxList runat="server" ID="cblCourse" Height="100%" RepeatColumns="1" RepeatDirection="Horizontal"
                            RepeatLayout="Flow">
                        </asp:CheckBoxList>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblActiveYN" runat="server" Text="Active Y/N"></asp:Literal>
                </td>
                <td>
                    <asp:CheckBox runat="server" ID="chkActiveYesOrNo" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
