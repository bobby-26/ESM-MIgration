<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionCDISIREMatrixContentAdd.aspx.cs" Inherits="Inspection_InspectionCDISIREMatrixContentAdd" %>

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

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CDI/SIRE Matrix Content</title>
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
        <eluc:TabStrip ID="MenuDocumentCategoryMain" Title="CDI/SIRE Matrix Content" runat="server" OnTabStripCommand="MenuDocumentCategoryMain_TabStripCommand"></eluc:TabStrip>
        <telerik:RadSplitter RenderMode="Lightweight" BorderColor="White" ID="RadSplitter1" runat="server" Height="100%" Width="100%">
        <telerik:RadPane ID="navigationPane" runat="server" Width="45%" Height="100%">
        <table width="100%" cellpadding="5">
            <tr>
                <td>
                    <telerik:RadAjaxPanel ID="Pnlcolumnlabel" runat="server">
                    </telerik:RadAjaxPanel>
                </td>
                <td>
                    <telerik:RadAjaxPanel ID="pnlcolumns" runat="server">
                    </telerik:RadAjaxPanel>
                </td>
            </tr>
            </table>
            </telerik:RadPane>
            <telerik:RadPane ID="RadPane1" runat="server" Width="50%" Height="100%">
            <table>
            <%--                <tr>
                    <td>
                        Procedure Name
                    </td>
                    <td>
                        <span id="spnPickListDocument">                            
                            <asp:ImageButton ID="btnShowDocuments" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" Text=".." />
                            <asp:TextBox ID="txtDocumentId" runat="server" Width="0px" CssClass="hidden"></asp:TextBox>
                            <asp:TextBox ID="txtDocumentName" runat="server" Width="150px" CssClass="input"></asp:TextBox>
                        </span>                 
                    </td>
                </tr>--%>
            <tr id="procedurespicklist" runat="server" valign="top">
                <td colspan="1" runat="server" width="150px">Procedures   
                </td>
                <td colspan="2">
                    <asp:RadioButtonList ID="rblType" runat="server" Visible="false" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblType_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="0">Forms,Posters,Checklist</asp:ListItem>
<%--                        <asp:ListItem Value="1">Procedures</asp:ListItem>
                        <asp:ListItem Value="2">Contingency/Emergency</asp:ListItem>--%>
                    </asp:RadioButtonList>
                    <span id="spnPickListDocument">
                        <telerik:RadTextBox ID="txtDocumentName" runat="server" Width="250px" Enabled="False" Style="font-weight: bold"
                            CssClass="input"></telerik:RadTextBox>
                        <asp:LinkButton ID="btnShowDocuments" runat="server" Text="..">
                        <span class="icon"><i class="fas fa-tasks"></i></span>
                            </asp:LinkButton>
                        <telerik:RadTextBox ID="txtDocumentId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                    </span>
                    <asp:LinkButton ID="lnkFormAdd" runat="server" OnClick="lnkFormAdd_Click" Text="Add"></asp:LinkButton>
                    <br />
                    <%--<asp:ListBox ID="lstFormPoster" runat="server" CssClass="input" Width="360px" AutoPostBack="true" OnSelectedIndexChanged="lstFormPoster_Changed"></asp:ListBox>--%>
                    <%--<asp:RadioButtonList ID="rblFormPoster" runat="server" CssClass="input" Width="360px" AutoPostBack="true" OnSelectedIndexChanged="rblFormPoster_Changed"></asp:RadioButtonList>--%>
                    <div id="divForms" runat="server" style="height: 100px; width:300px ;overflow:auto; border-width: 1px; border-style: solid; border: 1px solid #9f9fff">
                        <table id="tblForms" runat="server">
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td runat="server" width="150px" valign="top">Responsibility
                </td>
                <td>
                    <telerik:RadTextBox ID="txtDeptlist" Enabled="false" runat="server" CssClass="input" Rows="4" TextMode="MultiLine"
                        Width="300px"></telerik:RadTextBox>
                </td>
                <td>
                    <div id="dvClass" runat="server" class="input" style="overflow: auto; width: 200px;
                        height: 95px">
                        <asp:CheckBoxList ID="chkDeptList" runat="server" CssClass="input" DataTextField="FLDGROUPRANK"
                            DataValueField="FLDGROUPRANKID" Height="100%" RepeatDirection="Vertical" Width="100%">
                        </asp:CheckBoxList>
                    </div>
                </td>
            </tr>
            <tr>
                <td runat="server" width="150px" valign="top">Objective Evidence
                </td>
                <td>
                    <telerik:RadTextBox ID="txtObj" runat="server" CssClass="input" Rows="4" TextMode="MultiLine"
                       Width="300px"></telerik:RadTextBox>
                    <span id="spnPickListFMS">
                        <telerik:RadTextBox ID="txtReportName" runat="server" Width="250px" Enabled="False" Style="font-weight: bold"
                            CssClass="input"></telerik:RadTextBox>
                        <asp:LinkButton ID="btnShowFMS" runat="server" Text=".." >
                        <span class="icon"><i class="fas fa-tasks"></i></span>
                            </asp:LinkButton>
                        <telerik:RadTextBox ID="txtReportId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtFormId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                    </span>
                    <asp:LinkButton ID="lnkReportAdd" runat="server" OnClick="lnkReportAdd_Click" Text="Add"></asp:LinkButton>
                    <br />
                    <%--<asp:ListBox ID="lstFormPoster" runat="server" CssClass="input" Width="360px" AutoPostBack="true" OnSelectedIndexChanged="lstFormPoster_Changed"></asp:ListBox>--%>
                    <%--<asp:RadioButtonList ID="rblFormPoster" runat="server" CssClass="input" Width="360px" AutoPostBack="true" OnSelectedIndexChanged="rblFormPoster_Changed"></asp:RadioButtonList>--%>
                    <div id="divReports" runat="server" style="height: 100px; width: 300px;overflow:auto; border-width: 1px; border-style: solid; border: 1px solid #9f9fff">
                        <table id="tblReports" runat="server">
                        </table>
                    </div>
                </td>
            </tr>
        </table>
                </telerik:RadPane>
            </telerik:RadSplitter>
    </form>
</body>
</html>
