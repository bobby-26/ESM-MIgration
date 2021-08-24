<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreTrainingSubCategoryCourseMapping.aspx.cs" Inherits="CrewOffshoreTrainingSubCategoryCourseMapping" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Course Mapping</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="CrewCourseRequestlink" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmExamReq" runat="server" submitdisabledcontrols="true">
    <ajaxtoolkit:toolkitscriptmanager id="ToolkitScriptManager1" runat="server" combinescripts="false">
    </ajaxtoolkit:toolkitscriptmanager>
    <asp:UpdatePanel runat="server" ID="pnlExamReq">
        <ContentTemplate>
            <eluc:error id="ucError" runat="server" text="" visible="false"></eluc:error>
            <eluc:status id="ucStatus" runat="server" />
            <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%;
                position: absolute;">
                <div class="subHeader">
                    <eluc:title runat="server" id="ucTitle" text="Course/CBT Mapping" showmenu="false" />
                    <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:tabstrip id="CourseMapping" runat="server" ontabstripcommand="CourseMapping_TabStripCommand"></eluc:tabstrip>
                </div>
                <table runat="server" id="tblCourseMapping" width="100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblCategory" runat="server" Text="Category"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCategory" runat="server" Enabled="false" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblSubcategory" runat="server" Text="Subcategory"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSubcategory" runat="server" Enabled="false" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblTrainingCourse" runat="server" Text="Training Course"></asp:Literal>
                        </td>
                        <td>                                                        
                            <asp:DropDownList ID="ddlTrainingCourse" runat="server" AppendDataBoundItems="true" CssClass="input"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblCBT" runat="server" Text="CBT"></asp:Literal>
                        </td>
                        <td>                                                        
                            <asp:DropDownList ID="ddlCBT" runat="server" AppendDataBoundItems="true" CssClass="input"></asp:DropDownList>
                        </td>
                    </tr>
                </table>         
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
