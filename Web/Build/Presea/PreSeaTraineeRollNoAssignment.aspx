<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaTraineeRollNoAssignment.aspx.cs"
    Inherits="PreSeaTraineeRollNoAssignment" %>

<%@ Import Namespace="System.Data" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Exam</title>
    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmBondReq" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlPreSeaEntranceExam">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="AssignRollNo" Text="Assign Roll No" ShowMenu="false">
                    </eluc:Title>
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                        <eluc:TabStrip ID="MenuPreSea" runat="server" OnTabStripCommand="MenuPreSea_TabStripCommand">
                        </eluc:TabStrip>
                    </div>
                </div>
                <table cellpadding="2" cellspacing="2" width="50%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblName" runat="server" Text="Name"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtName" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="120px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblCourse" runat="server" Text="Course"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCourse" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="120px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblBatch" runat="server" Text="Batch"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBatchApplied" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="120px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblRollNo" runat="server" Text="Roll No"></asp:Literal>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlRollNo" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                                Width="120px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblSection" runat="server" Text="Section"></asp:Literal>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlSection" AppendDataBoundItems="true" CssClass="input"
                                Width="120px">
                                <asp:ListItem Text="--Select--" Value="">--Select--</asp:ListItem>
                                <asp:ListItem Text="A" Value="1">A</asp:ListItem>
                                <asp:ListItem Text="B" Value="2">B</asp:ListItem>
                                <asp:ListItem Text="C" Value="3">C</asp:ListItem>
                                <asp:ListItem Text="D" Value="4">D</asp:ListItem>
                                <asp:ListItem Text="E" Value="5">E</asp:ListItem>
                                <asp:ListItem Text="F" Value="6">F</asp:ListItem>
                                <asp:ListItem Text="G" Value="7">G</asp:ListItem>
                                <asp:ListItem Text="H" Value="8">H</asp:ListItem>
                                <asp:ListItem Text="I" Value="9">I</asp:ListItem>
                                <asp:ListItem Text="J" Value="10">J</asp:ListItem>
                                <asp:ListItem Text="K" Value="11">K</asp:ListItem>
                                <asp:ListItem Text="L" Value="12">L</asp:ListItem>
                                <asp:ListItem Text="M" Value="13">M</asp:ListItem>
                                <asp:ListItem Text="N" Value="14">N</asp:ListItem>
                                <asp:ListItem Text="O" Value="15">O</asp:ListItem>
                                <asp:ListItem Text="P" Value="16">P</asp:ListItem>
                                <asp:ListItem Text="Q" Value="17">Q</asp:ListItem>
                                <asp:ListItem Text="R" Value="18">R</asp:ListItem>
                                <asp:ListItem Text="S" Value="19">S</asp:ListItem>
                                <asp:ListItem Text="T" Value="20">T</asp:ListItem>
                                <asp:ListItem Text="U" Value="21">U</asp:ListItem>
                                <asp:ListItem Text="V" Value="22">V</asp:ListItem>
                                <asp:ListItem Text="W" Value="23">W</asp:ListItem>
                                <asp:ListItem Text="X" Value="24">X</asp:ListItem>
                                <asp:ListItem Text="Y" Value="25">Y</asp:ListItem>
                                <asp:ListItem Text="Z" Value="26">Z</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <td>
                        <asp:Literal ID="lblPractical" runat="server" Text="Practical"></asp:Literal>
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlPractical" AppendDataBoundItems="true" CssClass="input"
                            Width="120px">
                            <asp:ListItem Text="--Select--" Value="">--Select--</asp:ListItem>
                            <asp:ListItem Text="A" Value="1">A</asp:ListItem>
                            <asp:ListItem Text="B" Value="2">B</asp:ListItem>
                            <asp:ListItem Text="C" Value="3">C</asp:ListItem>
                            <asp:ListItem Text="D" Value="4">D</asp:ListItem>
                            <asp:ListItem Text="E" Value="5">E</asp:ListItem>
                            <asp:ListItem Text="F" Value="6">F</asp:ListItem>
                            <asp:ListItem Text="G" Value="7">G</asp:ListItem>
                            <asp:ListItem Text="H" Value="8">H</asp:ListItem>
                            <asp:ListItem Text="I" Value="9">I</asp:ListItem>
                            <asp:ListItem Text="J" Value="10">J</asp:ListItem>
                            <asp:ListItem Text="K" Value="11">K</asp:ListItem>
                            <asp:ListItem Text="L" Value="12">L</asp:ListItem>
                            <asp:ListItem Text="M" Value="13">M</asp:ListItem>
                            <asp:ListItem Text="N" Value="14">N</asp:ListItem>
                            <asp:ListItem Text="O" Value="15">O</asp:ListItem>
                            <asp:ListItem Text="P" Value="16">P</asp:ListItem>
                            <asp:ListItem Text="Q" Value="17">Q</asp:ListItem>
                            <asp:ListItem Text="R" Value="18">R</asp:ListItem>
                            <asp:ListItem Text="S" Value="19">S</asp:ListItem>
                            <asp:ListItem Text="T" Value="20">T</asp:ListItem>
                            <asp:ListItem Text="U" Value="21">U</asp:ListItem>
                            <asp:ListItem Text="V" Value="22">V</asp:ListItem>
                            <asp:ListItem Text="W" Value="23">W</asp:ListItem>
                            <asp:ListItem Text="X" Value="24">X</asp:ListItem>
                            <asp:ListItem Text="Y" Value="25">Y</asp:ListItem>
                            <asp:ListItem Text="Z" Value="26">Z</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
