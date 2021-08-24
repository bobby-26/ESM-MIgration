<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportCoursesDoneFilter.aspx.cs"
    Inherits="CrewReportCoursesDoneFilter" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Courses Done Report Filter</title>
 <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

 </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCoursesDoneReport" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div>
        <eluc:Status runat="server" ID="ucStatus" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader" style="position: relative">
            <div id="divHeading" style="vertical-align: top">
                <eluc:Title runat="server" ID="ucTitle" Text="Courses Done Report Filter"></eluc:Title>
            </div>
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuCoursesDoneReport" runat="server" OnTabStripCommand="MenuCoursesDoneReport_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td>
                    <asp:Literal ID="lblDocumentType" Text="Document Type" runat="server"></asp:Literal>
                </td>
                <td>
                    <eluc:Hard runat="server" ID="ucDocumentType" CssClass="input" AppendDataBoundItems="true"
                        HardTypeCode="103" AutoPostBack="true" OnTextChangedEvent="DocumentTypeSelection" />
                    <%--<eluc:Quick runat="server" ID="ucDocumentType" CssClass="dropdown_mandatory" AppendDataBoundItems="true" QuickTypeCode="2" QuickList='<%# PhoenixRegistersQuick.ListQuick(0, 2) %>' />--%>
                </td>
                <td>
                    <asp:Literal ID="lblFromDate" Text="From Date" runat="server"></asp:Literal>
                </td>
                <td>
                    <eluc:Date ID="txtFromDate" runat="server" Width="80px" CssClass="input_mandatory" />
                </td>
                <td>
                    <asp:Literal ID="lblToDate" runat="server" Text="To Date"></asp:Literal>
                </td>
                <td>
                    <eluc:Date ID="txtToDate" runat="server" Width="80px" CssClass="input_mandatory" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td></td>
                <td colspan="2">
                    <font color="blue"><b>Note: </b>The below list box(Vessel Type) is filter for the "Course"
                    </font>
                </td>
            </tr>
            <tr>
            <td>
                    <asp:Literal ID="lblCourses" runat="server" Text="Courses"></asp:Literal>
                </td>
                <td colspan="3">
                    <asp:Literal ID="lblCheckall" runat="server" Text="Check All"></asp:Literal>
                    <asp:CheckBox ID="chkChkAllCourse" runat="server" AutoPostBack="true" OnCheckedChanged="SelectAllCourse" />
                    <div runat="server" id="dvCourse" class="input_mandatory" style="overflow: auto;
                        width: 80%; height: 140px">
                        <asp:CheckBoxList runat="server" ID="cblCourse" Height="100%" RepeatColumns="1" RepeatDirection="Horizontal"
                            RepeatLayout="Flow">
                        </asp:CheckBoxList>
                    </div>
                </td>
                <td>
                   <asp:Literal ID="lblVesselType" Text="Vessel Type" runat="server"></asp:Literal>
                </td>
                <td>
                   <asp:Literal ID="lblCheckAll1" runat="server" Text="Check All"></asp:Literal>
                    <asp:CheckBox ID="chkChkAllVesselType" runat="server" AutoPostBack="true" OnCheckedChanged="SelectAllVesselType" />
                    <div runat="server" id="dvVesselType" class="input" style="overflow: auto; width: 100%;
                        height: 100px">
                        <asp:CheckBoxList runat="server" ID="cblVesselType" Height="100%" RepeatColumns="1"
                            RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="VesselTypeFilterSelection"
                            RepeatLayout="Flow">
                        </asp:CheckBoxList>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                   <font color="blue"><b>Note: </b>The below list box(Rank) is filter for the "Course"
                    </font>                
                </td>
                <td colspan="2">
                    <font color="blue"><b>Note: </b>The below list boxes are filters for the "Rank"
                    </font>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblRank" runat="server" Text="Rank"></asp:Literal>
                </td>
                <td>
                    <asp:Literal ID="lblCheckAll2" runat="server" Text="Check All"></asp:Literal>
                    <asp:CheckBox ID="chkChkAllRank" runat="server" AutoPostBack="true" OnCheckedChanged="SelectAllRank" />
                    <div runat="server" id="dvRank" class="input" style="overflow: auto; width: 80%;
                        height: 140px">
                        <asp:CheckBoxList runat="server" ID="cblRank" Height="100%" RepeatColumns="1" RepeatDirection="Horizontal"
                            RepeatLayout="Flow" AutoPostBack="true" OnSelectedIndexChanged="RankFilterSelection">
                        </asp:CheckBoxList>
                    </div>
                </td>
                <td colspan="4">
                    <asp:Panel ID="pnlRankFilter" runat="server" GroupingText="Filter" Width="100%" Height="40%">
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:Literal ID="lblDepartment" runat="server" Text="Department"></asp:Literal>
                                </td>
                                <td>
                                    <div runat="server" id="dvDepartment" class="input" style="overflow: auto; width: 60%;
                                        height: 140px">
                                        <asp:CheckBoxList runat="server" ID="cblDepartment" Height="100%" RepeatColumns="1"
                                            AutoPostBack="true" OnSelectedIndexChanged="DepartmentSelection" RepeatDirection="Horizontal"
                                            RepeatLayout="Flow">
                                        </asp:CheckBoxList>
                                    </div>
                                </td>
                                <td>
                                    <asp:Literal ID="lblLevel" runat="server" Text="Level"></asp:Literal>
                                </td>
                                <td>
                                    <div runat="server" id="dvLevel" class="input" style="overflow: auto; width: 80%;
                                        height: 80px">
                                        <asp:CheckBoxList runat="server" ID="cblLevel" Height="100%" RepeatColumns="1" RepeatDirection="Horizontal"
                                            AutoPostBack="true" OnSelectedIndexChanged="RankSelection" RepeatLayout="Flow">
                                            <asp:ListItem Value="1">Senior Officer</asp:ListItem>
                                            <asp:ListItem Value="2">Junior Officer</asp:ListItem>
                                            <asp:ListItem Value="193">Trainee</asp:ListItem>
                                            <asp:ListItem Value="191">Ratings</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
