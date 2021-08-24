<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreTrainingCourseRequest.aspx.cs"
    Inherits="CrewOffshoreTrainingCourseRequest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Course Request</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="CrewCourseRequestlink" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmCourseReq" runat="server" submitdisabledcontrols="true">
    <ajaxtoolkit:toolkitscriptmanager id="ToolkitScriptManager1" runat="server" combinescripts="false">
    </ajaxtoolkit:toolkitscriptmanager>
    <asp:UpdatePanel runat="server" ID="pnlCourseReq">
        <ContentTemplate>
            <eluc:error id="ucError" runat="server" text="" visible="false"></eluc:error>
            <eluc:status id="ucStatus" runat="server" />
            <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%;
                position: absolute;">
                <div class="subHeader">
                    <eluc:title runat="server" id="ucTitle" text="Requested Course" showmenu="false" />
                    <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:tabstrip id="CrewMenu" runat="server" ontabstripcommand="CrewMenu_TabStripCommand"></eluc:tabstrip>
                </div>
                <table runat="server" id="tblPersonalMaster" width="100%">
                    <tr>
                        <td style="width:25px">
                            <asp:Literal ID="lblCourse" runat="server" Text="Courses to be done" Visible="false"></asp:Literal>
                        </td>
                        <td style="width:75px">                            
                            <b><asp:Label ID="lblNil" runat="server" Visible="false" Text="NIL"></asp:Label></b>
                            <asp:CheckBoxList ID="cblCourseList" runat="server" CssClass="input" Visible="false">
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                </table>
                <br />
                <table runat="server" id="tblCourseReq" width="100%">
                    <%--<tr>
                        <td>
                            <b><asp:Literal ID="lblReqCourses" runat="server" Text="Requested Course"></asp:Literal></b>
                        </td>
                    </tr>--%>
                    <tr>
                        <td>
                            <div id="divGrid" style="position: relative; z-index: 10; width: 100%;">
                                <asp:GridView ID="gvCourseReq" runat="server" AutoGenerateColumns="False" Font-Size="11px"                                    
                                    Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false">
                                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                    <RowStyle Height="10px" />
                                    <Columns>
                                        <asp:ButtonField Text="DoubleClick" CommandName="Select" Visible="false" />
                                        <asp:TemplateField HeaderText="Course">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>                                                
                                                <asp:Label ID="lblCourse" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>         
                                        <asp:TemplateField HeaderText="Completed Y/N">
                                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkCompletedYN" runat="server" Enabled="false" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDCOMPLETEDYN").ToString().Equals("1"))?true:false %>'></asp:CheckBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>      
                                        <asp:TemplateField HeaderText="Completed Date">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>                                                
                                                <asp:Label ID="lblCompletedDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDCOMPLETIONDATE")) %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>                                     
                                    </Columns>
                                </asp:GridView>
                            </div>
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
                                    <td width="20px">
                                        &nbsp;
                                    </td>
                                    <td nowrap align="right" width="50px">
                                        <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                                    </td>
                                    <td nowrap align="center">
                                        <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                        </asp:TextBox>
                                        <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                            Width="40px"></asp:Button>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
