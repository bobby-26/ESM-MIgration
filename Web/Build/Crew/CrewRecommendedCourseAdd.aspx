<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewRecommendedCourseAdd.aspx.cs"
    Inherits="CrewRecommendedCourseAdd" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="User" Src="~/UserControls/UserControlMultiColumnUser.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Recommended Course</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewRecommendedCourseAdd" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
         <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuCrewRecommendedCourse" runat="server" OnTabStripCommand="CrewRecommendedCourse_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%" Width="80%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <table cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCourse" runat="server" Text="Course" ></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadListBox ID="lstCourse" runat="server" AppendDataBoundItems="true" SelectionMode="Multiple"  
                            Width="50%" Height="150px">
                        </telerik:RadListBox>
                        <telerik:RadComboBox ID="ddlCourse" runat="server" CssClass="dropdown_mandatory" OnDataBound="ddlCourse_DataBound" ></telerik:RadComboBox>
                    </td>
                </tr>
                   <tr>
                    <td>
                        <telerik:RadLabel ID="Recommendedby" runat="server" Text="Recommended by" ></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:User ID="ddlCreatedBy" runat="server" CssClass="dropdown_mandatory" ActiveYN="172" Width="30%" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRecommendedDate" runat="server" Text="Recommended Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date runat="server" ID="txtRecommendedDate" CssClass="input_mandatory" />
                    </td>
                </tr>
             
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRemarks" runat="server" MaxLength="200" TextMode="MultiLine" Width ="30%"
                           >
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
