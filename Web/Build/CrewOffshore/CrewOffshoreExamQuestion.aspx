<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreExamQuestion.aspx.cs" Inherits="CrewOffshoreExamQuestion" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Exam Question</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="CrewAppointmentLetterlink" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmExamQuestion" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlExamQuestion">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />
            <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%;
                position: absolute;">
                <div class="subHeader">
                    <eluc:Title runat="server" ID="ucTitle" Text="Test Question" ShowMenu="false" />
                    <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                </div>
                <%--<div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="CrewMenu" runat="server" OnTabStripCommand="CrewMenu_TabStripCommand"></eluc:TabStrip>
                </div>--%>
                <br /><br /><br />
                <table runat="server" id="tblExamQuestion" width="100%">
                    <tr>
                        <td>
                            <b><asp:Literal ID="lblNote" runat="server"></asp:Literal></b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br /><br /><br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b><asp:Literal ID="lblQuestion" runat="server"></asp:Literal></b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:RadioButtonList ID="rblAnswerList" runat="server" RepeatColumns="1" RepeatDirection="Vertical"></asp:RadioButtonList>
                            <asp:CheckBoxList ID="cblAnswerList" Visible="false" runat="server" RepeatColumns="1" RepeatDirection="Vertical"></asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br /><br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblError" runat="server" Font-Bold="true" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnSubmit" runat="server" Text="Next" OnClick="btnSubmit_Click" Visible="false"
                                  CssClass="cntxMenuSelect" />
                             <asp:Button ID="btnNext" runat="server" Text="Next" OnClick="btnNext_Click" CssClass = "cntxMenuSelect" />
                        </td>
                    </tr>
                    <%--<tr>
                        <td align = "center">
                            <asp:Button ID="btnNext" runat="server" Text="Next" OnClick="btnNext_Click" CssClass = "cntxMenuSelect" />
                        </td>
                    </tr>--%>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblThanks" runat="server" Font-Bold="true" Visible="false" ForeColor="Blue"></asp:Label>
                        </td>
                    </tr>
                </table> 
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
