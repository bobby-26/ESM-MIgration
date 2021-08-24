<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewCourseAttendance.aspx.cs"
    Inherits="CrewCourseAttendance" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Course Attendance</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <style type="text/css">
        .LeftAlign
        {
            text-align: center;
        }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCourseAttendance" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlCourseAttendance">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <asp:Literal ID="lblAttendance" runat="server" Text="Attendance"></asp:Literal>
                         <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" />
                    </div>
                </div>
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblCourse" runat="server" Text="Course"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Course ID="ucCourse" runat="server" AppendDataBoundItems="true" CssClass="readonlytextbox"
                                Enabled="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblCourseType" runat="server" Text="Course Type"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Hard runat="server" ID="ucCourseType" CssClass="readonlytextbox" AppendDataBoundItems="true"
                                Enabled="false" HardTypeCode="103" />
                        </td>
                        <td>
                            <asp:Literal ID="lblBatchNo" runat="server" Text="Batch No"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBatchNo" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="360px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblAttendanceDate" runat="server" Text="Attendance Date"></asp:Literal>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlAttendanceDate" runat="server" CssClass="input" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlAttendanceDate_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <div class="navSelect" style="position: relative; clear: both; width: 15px">
                    <eluc:TabStrip ID="MenuAttendance" runat="server" OnTabStripCommand="MenuAttendance_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="div1" style="position: relative; z-index: 0">
                    <asp:GridView ID="gvAttendanceList" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="50%" CellPadding="3" OnRowCommand="gvAttendanceList_RowCommand" OnRowDataBound="gvAttendanceList_RowDataBound"
                        AllowSorting="true" OnRowEditing="gvAttendanceList_RowEditing" OnRowCancelingEdit="gvAttendanceList_RowCancelingEdit"
                        ShowFooter="false" ShowHeader="true" EnableViewState="false">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblEmployeeName" runat="server" Text="Employee Name"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>  
                                   <asp:Label ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></asp:Label>
                                    <asp:Label ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></asp:Label>      
                                    <asp:Label ID="lblCourseId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSEID") %>'></asp:Label>
                                    <asp:Label ID="lblBatchId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCHID") %>'></asp:Label>
                                     <asp:Label ID="lblEmployeeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></asp:Label>
                                    <asp:LinkButton ID="lnkName" runat="server" CommandName="EDIT" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.Name") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField>
                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="180px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblRankCode" runat="server" Text="Rank Code"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblRankCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.Rank Code") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-CssClass="LeftAlign" ItemStyle-CssClass="LeftAlign">
                                <ItemStyle Wrap="true" HorizontalAlign="Left" ></ItemStyle>
                                <HeaderTemplate>
                                     <asp:Label ID="lblAMHeader"  runat="server" Text="AM" ></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAM" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAM") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkAMEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDAM").ToString().Equals("P"))?true:false %>'>
                                    </asp:CheckBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:CheckBox ID="chkAMAdd" runat="server"></asp:CheckBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-CssClass="LeftAlign" ItemStyle-CssClass="LeftAlign">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblPM" runat="server" Text="PM"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPM" runat="server"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDPM") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkPMEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDPM").ToString().Equals("P"))?true:false %>'>
                                    </asp:CheckBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:CheckBox ID="chkPMAdd" runat="server"></asp:CheckBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField FooterStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                     <img id="Img3" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                     <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                        CommandName="ATTACHMENT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAtt"
                                        ToolTip="Attachment"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="SAVE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuCourseAttendance" runat="server" OnTabStripCommand="MenuCourseAttendance_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <br />
                <b><asp:Literal ID="lblAttendanceList" runat="server" Text="Attendance List"></asp:Literal></b>
                <div class="navSelect" style="position: relative; clear: both; width: 15px">
                    <eluc:TabStrip ID="MenuGridCourseAttendance" runat="server" OnTabStripCommand="MenuGridCourseAttendance_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                    <asp:GridView ID="gvAttendance" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowFooter="false" OnRowDataBound="gvAttendance_RowDataBound"
                        EnableViewState="False">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                        </Columns>
                    </asp:GridView>
                </div>
                <div>
                    <asp:GridView ID="gvAttendance1" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowFooter="false" OnRowDataBound="gvAttendance1_RowDataBound"
                        EnableViewState="False">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                        </Columns>
                    </asp:GridView>
                </div>
                <%-- <eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="FinalizePortageBill_Confirm" Visible="false" />--%>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <eluc:Status ID="ucStatus" runat="server" />
    </form>
</body>
</html>
