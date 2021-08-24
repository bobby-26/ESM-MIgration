<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewBatchListReport.aspx.cs" Inherits="Crew_CrewBatchListReport" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonToolTip" Src="~/UserControls/UserControlCommonToolTip.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Batch List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlBatch">
            <ContentTemplate>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                    <div class="subHeader" style="position: relative">
                        <div id="divHeading">
                            <eluc:Title runat="server" ID="ucTitle" Text="Batch List" ShowMenu="true" />
                        </div>
                    </div>

                    <div id="divFind" style="position: relative; z-index: 2">
                        <table id="tblBatchListSearch" cellpadding="1" cellspacing="2" width="100%">
                            <%-- <tr runat="server" visible="false">
                                <td>
                                    <asp:Literal ID="Literal1" runat="server" Text="Course Code"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtcourseCode" runat="server" CssClass="input"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Literal ID="Literal2" runat="server" Text="Course"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtcourseName" runat="server" CssClass="input" Width="300px"></asp:TextBox>
                                    <asp:TextBox ID="txtCourseId" runat="server" Width="100px"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <tr>
                                <td>
                                    <asp:Literal ID="lblInstitute" runat="server" Text="Institute"></asp:Literal>
                                </td>
                                <td>
                                    <span id="spnPickListInstitute">
                                        <asp:TextBox ID="txtInstituteId" runat="server" Width="100px"></asp:TextBox>
                                        <asp:TextBox ID="txtInstituteName" runat="server" Width="350px" CssClass="input" Enabled="false"></asp:TextBox>
                                        <img runat="server" id="btnShowInstitute" style="cursor: pointer; vertical-align: top" alt=""
                                            src="<%$ PhoenixTheme:images/picklist.png %>" onclick="return showPickList('spnPickListInstitute', 'codehelp1', '', 'Common/CommonPickListInistituteList.aspx', true); " />
                                    </span>

                                </td>
                                <td>
                                    <asp:Literal ID="lblCourse" runat="server" Text="Course"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Course ID="ddlCourse" runat="server" CourseList="<%#PhoenixRegistersDocumentCourse.ListNonCBTDocumentCourse()%>"
                                        ListCBTCourse="false" AppendDataBoundItems="true" CssClass="input"
                                        AutoPostBack="true" />
                                </td>

                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="lblBatchNo" runat="server" Text="Batch No"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtBatchNoSearch" CssClass="input"></asp:TextBox>
                                </td>

                                <td>
                                    <asp:Literal ID="lblStatus" runat="server" Text="Status"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Hard ID="ucBatchStatus" runat="server" AppendDataBoundItems="true"
                                        CssClass="input" HardTypeCode="152" ShortNameFilter="OPN,CNL" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="lblDuration" runat="server" Text="Duration"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Date ID="txtStartDate" runat="server" CssClass="input_mandatory" />
                                    <asp:Literal ID="lblDash" runat="server" Text="-"></asp:Literal>
                                    <eluc:Date ID="txtEndDate" runat="server" CssClass="input_mandatory" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                    <%--<asp:Label ID="lbld" runat="server" ForeColor="Blue" Text="Note : By default batches will show only the open status."></asp:Label>--%>
                    <br />
                    <br />
                    <b>
                        <asp:Literal ID="lblTitle" runat="server" Text="Batch List"></asp:Literal>
                    </b>
                    <div class="navSelect" style="position: relative; width: 15px">
                        <eluc:TabStrip ID="MenuCrewBatchList" runat="server" OnTabStripCommand="MenuCrewBatchList_TabStripCommand"></eluc:TabStrip>
                    </div>

                    <div id="divGrid" style="position: relative; z-index: 0">
                        <asp:GridView ID="gvBatchList" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" OnRowDataBound="gvBatchList_RowDataBound"
                            AllowSorting="true" ShowFooter="false" ShowHeader="true" EnableViewState="false"
                            DataKeyNames="FLDCREWINSTITUTEBATCHID">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                            <RowStyle Height="10px" />
                            <Columns>
                                <asp:TemplateField HeaderText="S.No">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="lblRowNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblBatchNoHeader" runat="server" Text="Batch No"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%--<asp:Label ID="lblCourseId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSEID") %>'></asp:Label>
                                        <asp:Label ID="lblInstituteId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSTITUTEID") %>'></asp:Label>--%>
                                        <asp:Label ID="lblBatchNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCHNO") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblFromDate" runat="server" Text="Start"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblFromDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDFROMDATE","{0:dd/MMM/yyyy}")) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>

                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblInstituteNameHeader" runat="server" Text="Institute"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblInstitute" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblStatusHeader" runat="server" Text="Status"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatusName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblActionHeader" runat="server">
                                        Action
                                        </asp:Label>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <eluc:CommonToolTip ID="ucCommonToolTip" runat="server"
                                            Screen='<%# "Crew/CrewBatchMoreInfoList.aspx?batchId=" + DataBinder.Eval(Container,"DataItem.FLDCREWINSTITUTEBATCHID").ToString()%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%--<asp:TemplateField FooterStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                                        </asp:Label>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Batch Plan" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                            ID="imgAddPlan" ToolTip="Batch Plan" ImageAlign="AbsMiddle" OnClick="imgAddPlan_Click"
                                            CommandArgument="<%# Container.DataItemIndex %>" />
                                        <asp:ImageButton runat="server" CommandArgument="<%# Container.DataItemIndex %>"
                                            ID="cmdEnrollment" ImageUrl="<%$ PhoenixTheme:images/billingparties.png %>" ImageAlign="AbsMiddle"
                                            CommandName="ENROLL" Text=".." ToolTip="Enrollment" Visible="false" />
                                        <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                        <asp:ImageButton runat="server" CommandArgument="<%# Container.DataItemIndex %>"
                                            ID="cmdCancelBatch" ImageUrl="<%$ PhoenixTheme:images/red-symbol.png %>" ImageAlign="AbsMiddle"
                                            CommandName="CANCELBATCH" Text=".." ToolTip="Cancel" />

                                        <asp:ImageButton runat="server" CommandArgument="<%# Container.DataItemIndex %>"
                                            ID="cmdAttendance" ImageUrl="<%$ PhoenixTheme:images/time-schedule.png %>" ImageAlign="AbsMiddle"
                                            CommandName="ATTEND" Text=".." ToolTip="Attendance" Visible="false" />
                                        <%--<img id="Img3" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />--%>
                                <%-- <asp:ImageButton runat="server" CommandArgument="<%# Container.DataItemIndex %>"
                                            ID="cmdEnrollmentCompleted" ImageUrl="<%$ PhoenixTheme:images/edit-confirm.png %>" ImageAlign="AbsMiddle"
                                            CommandName="ENROLLMENT" Text=".." ToolTip="Enrollment Complete" Visible="false" />
                                        <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                        <asp:ImageButton runat="server" CommandArgument="<%# Container.DataItemIndex %>"
                                            ID="cmdDelete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>" ImageAlign="AbsMiddle"
                                            CommandName="DELETE" Text=".." ToolTip="Delete" />

                                    </ItemTemplate>
                                    <FooterStyle HorizontalAlign="Center" />
                                </asp:TemplateField>--%>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div id="divPage" style="position: relative;">
                        <table width="100%" border="0" class="datagrid_pagestyle">
                            <tr>
                                <td nowrap align="center">
                                    <asp:Label ID="lblPagenumber" runat="server">
                                    </asp:Label>
                                    <asp:Label ID="lblPages" runat="server">
                                    </asp:Label>
                                    <asp:Label ID="lblRecords" runat="server">
                                    </asp:Label>&nbsp;&nbsp;
                                </td>
                                <td nowrap align="left" width="50px">
                                    <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                                </td>
                                <td width="20px">&nbsp;
                                </td>
                                <td nowrap align="right" width="50px">
                                    <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                                </td>
                                <td nowrap align="center">
                                    <eluc:Number ID="txtnopage" runat="server" Width="20px" IsInteger="true" CssClass="input" />
                                    <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                        Width="40px"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="gvBatchList" />
            </Triggers>
        </asp:UpdatePanel>
    </form>
</body>
</html>
