<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersExamConfigurationAdd.aspx.cs"
    Inherits="Registers_RegistersExamConfigurationAdd" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlCourse.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Exam Configurartion</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div runat="server" id="DivHeader">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmRegistersExamConfig" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlExamConfig">
        <ContentTemplate>
            <div>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <div id="divHeading" style="vertical-align: top">
                        <asp:Literal ID="lblExamConfig" runat="server" Text="Exam Configurartion"></asp:Literal>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuRegistersExamConfigAdd" runat="server" OnTabStripCommand="MenuRegistersExamConfigAdd_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divControls">
                    <table width="100%" cellspacing="15">
                        <tr>
                            <td>
                                <asp:Literal ID="lblAnswer" runat="server" Text="Exam"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtExam" runat="server" Width="400px" CssClass="input_mandatory"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblCourse" runat="server" Text="Course"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Course ID="ddlCourse" runat="server" CourseList="<%#PhoenixRegistersDocumentCourse.ListNonCBTDocumentCourse()%>"
                                    ListCBTCourse="false" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                    AutoPostBack="true" />
                            </td>
                        </tr>
                       <%-- <tr>
                            <td>
                                <asp:Literal ID="Literal1" runat="server" Text="Level"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlLevel" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true">
                                </asp:DropDownList>
                            </td>
                        </tr>--%>
                        <tr>
                            <td>
                                <asp:Literal ID="lblScore" runat="server" Text="Max Questions"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Number ID="ucMaxQust" Width="100px" runat="server" CssClass="input_mandatory"
                                    IsInteger="true" MaxLength="3" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblPassMark" runat="server" Text="Pass Marks"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Number ID="ucPassMarks" Width="100px" runat="server" CssClass="input_mandatory"
                                    IsInteger="true" MaxLength="3" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
