<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewCourseEnrollmentFilter.aspx.cs"
    Inherits="CrewCourseEnrollmentFilter" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlCourse.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Course Enrollment Query Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCourseEnrollmentFilter" runat="server">
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <div class="subHeader">
            <eluc:Title runat="server" ID="ttlTile" Text="" ShowMenu="false"></eluc:Title>
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="CourseEnrollmentFilterMain" runat="server" OnTabStripCommand="CourseEnrollmentFilterMain_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlCourseEnrollment">
            <ContentTemplate>
                <div id="divFind">
                    <table cellpadding="2" cellspacing="2">
                        <tr>
                            <td>
                                <asp:Literal ID="lblFileno" runat="server" Text="File no"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFileno" runat="server" CssClass="input" Width="220px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblRank" runat="server" Text="Rank"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Rank ID="ddlRank" runat="server" CssClass="input" AppendDataBoundItems="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblName" runat="server" Text="Name"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtName" runat="server" CssClass="input" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblAvlFromDate" runat="server" Text="Avl From Date"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Date ID="txtFromDate" runat="server" CssClass="input" />
                           
                                <asp:Literal ID="lblAvlToDate" runat="server" Text="Avl To Date"></asp:Literal>
                            
                                <eluc:Date ID="txtToDate" runat="server" CssClass="input" />
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
