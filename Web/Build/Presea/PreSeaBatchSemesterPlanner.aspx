<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaBatchSemesterPlanner.aspx.cs"
    Inherits="PreSeaBatchSemesterPlanner" %>

<%@ Import Namespace="System.Data" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PreSeaBatch" Src="~/UserControls/UserControlPreSeaBatch.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlPreSeaCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Semester" Src="~/UserControls/UserControlPreSeaCourseSemester.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Semester Planner</title>
    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPreSeaExam" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlPreSeaEntranceExam">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="EntranceExamInterview" Text="Semester Planner" ShowMenu="true">
                    </eluc:Title>
                    <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                </div>
                <div style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuBatchPlanner" runat="server" OnTabStripCommand="BatchPlanner_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                </div>
                <%--<div class="subHeader" style="position: relative">
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                        <eluc:TabStrip ID="MenuPreSeaSemester" runat="server" OnTabStripCommand="MenuPreSeaSemester_TabStripCommand">
                        </eluc:TabStrip>
                    </div>
                </div>--%>
                <%--<table style="width: 100%">
                    <tr>
                        <td colspan="6" style="color: Blue;">
                            Note : Initiate Plan before enter date for subjects. </br>
                        </td>
                    </tr>
                </table>--%>
                <table cellpadding="2" cellspacing="2" width="80%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblCourse" runat="server" Text="Course"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Course ID="ddlCourse" runat="server" AppendDataBoundItems="true" CssClass="readonlytextbox" />
                        </td>
                        <td>
                            <asp:Literal ID="lblYear" runat="server" Text="Year">
                            </asp:Literal>
                        </td>
                        <td>
                            <eluc:Quick ID="ddlYear" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                                QuickTypeCode="55" AutoPostBack="true" />
                        </td>
                        <td>
                            <asp:Literal ID="lblPlan" runat="server" Text="Semester"></asp:Literal>
                        </td>
                        <td>
                             <eluc:Semester ID="ddlSemester" runat="server" Width="120px"  
                                 CssClass="dropdown_mandatory" AppendDataBoundItems="true" AutoPostBack="true"/>     
                          <%--  <asp:DropDownList ID="ddlPlan" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                                CssClass="input_mandatory" Width="120px">
                                <asp:ListItem Value="" Text="--Select--">--Select--</asp:ListItem>
                                <asp:ListItem Value="1" Text="1">1</asp:ListItem>
                                <asp:ListItem Value="2" Text="2">2</asp:ListItem>
                            </asp:DropDownList>--%>
                        </td>
                    </tr>
                </table>
                <br />
                <div class="navSelect" style="position: relative; clear: both; width: 15px">
                    <eluc:TabStrip ID="MenuPreSea" runat="server" OnTabStripCommand="MenuPreSea_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                    <asp:GridView ID="gvPreSea" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowDataBound="gvPreSea_RowDataBound" ShowHeader="true"
                        OnRowEditing="gvPreSea_RowEditing" OnRowCancelingEdit="gvPreSea_RowCancelingEdit"
                        OnRowCommand="gvPreSea_RowCommand" AllowSorting="true" OnSorting="gvPreSea_Sorting"
                        OnRowUpdating="gvPreSea_RowUpdating" OnSelectedIndexChanging="gvPreSea_SelectedIndexChanging"
                        ShowFooter="false" EnableViewState="false">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblSemester" runat="server" Text="Semester"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSemesterId" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDSEMESTERID"]%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblSemesterIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEMESTERID") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblSubjectId" runat="server" Text="Subject"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSemesterPlanId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEMESTERPLANID") %>'></asp:Label>                                    
                                    <asp:Label ID="lblPlanId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLANID") %>'></asp:Label>                                    
                                    <asp:Label ID="lblSubjectId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTID") %>'></asp:Label>
                                    <asp:LinkButton ID="lnkSubjectName" runat="server" CommandName="SELECT" CommandArgument='<%# Container.DataItemIndex %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTNAME") %>'>
                                    </asp:LinkButton>
                                    <%--<asp:Label ID="lblSubjectName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTNAME") %>'></asp:Label>--%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblSemesterPlanIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEMESTERPLANID") %>'></asp:Label>
                                    <asp:Label ID="lblPlanIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLANID") %>'></asp:Label>
                                    <asp:Label ID="lblSubjectIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTID") %>'></asp:Label>
                                    <%--<asp:Label ID="lblSubjectNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTNAME") %>'></asp:Label>--%>
                                    <asp:LinkButton ID="lblSubjectNameEdit" runat="server" CommandName="SELECT" CommandArgument='<%# Container.DataItemIndex %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTNAME") %>'>
                                    </asp:LinkButton>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblDate" runat="server" Text="Date"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDate" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDDATE"]%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date ID="txtDate" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATE") %>' />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Start Time
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblStartTime" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFROMTIME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="txtStartTimeEdit" runat="server" CssClass="input" Mask="99.99" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFROMTIME") %>' />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="End Time">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    End Time
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblEndTime" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTIME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="txtEndTimeEdit" runat="server" CssClass="input" Mask="99.99" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTIME") %>' />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblInvListHeader" runat="server">Invigilators&nbsp;
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblInvList" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVIGILATORS") %>'
                                        Visible="false"></asp:Label>
                                    <img id="imgInvList" runat="server" visible="false" src="<%$ PhoenixTheme:images/te_view.png %>"
                                        onmousedown="javascript:closeMoreInformation()" />
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
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="PLANEDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                        ToolTip="Edit" Visible="false"></asp:ImageButton>
                                    <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" visible="false" />
                                    <asp:ImageButton runat="server" AlternateText="Details" ImageUrl="<%$ PhoenixTheme:images/annexure.png %>"
                                        CommandName="INVIGILATOR" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdInvigilator"
                                        ToolTip="Invigilator Details"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="PLANUPDATE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                        ToolTip="Save" Visible="false"></asp:ImageButton>
                                    <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="PLANCANCEL" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel" Visible="false"></asp:ImageButton>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
