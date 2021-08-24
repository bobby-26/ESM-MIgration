<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaMentorPlannerActual.aspx.cs" Inherits="Presea_PreSeaMentorPlannerActual" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="System.Data" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PreSeaBatch" Src="~/UserControls/UserControlPreSeaBatch.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CldYear" Src="~/UserControls/UserControlCalenderYear.ascx" %>
<%@ Register TagPrefix="eluc" TagName="cldMonth" Src="~/UserControls/UserControlCalenderMonths.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Mentor Planner Actual</title>
    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
   </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="PnlMentorPlannerActual">
            <ContentTemplate>
                <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <eluc:Status runat="server" ID="ucStatus" />
                    <div class="subHeader" style="position: relative">
                        <eluc:Title runat="server" ID="MentorPlannerTitle" Text="Mentor Planner" ShowMenu="true"></eluc:Title>
                        <%--<asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />--%>
                    </div>
                    <div style="top: 0px; right: 0px; position: absolute">
                        <eluc:TabStrip ID="MenuMentorPlanner" runat="server" OnTabStripCommand="MenuMentorPlanner_TabStripCommand"
                            TabStrip="true"></eluc:TabStrip>
                    </div>
                    <table cellpadding="2" cellspacing="2" width="100%">
                        <tr>
                            <td>
                                <asp:Literal ID="lblMentor" runat="server" Text="Staff"></asp:Literal>
                            </td>
                            <td>
                                <span id="spnFacultyAdd">
                                    <asp:TextBox ID="txtFaculty" runat="server" CssClass="input" Enabled="false"
                                        MaxLength="50" Width="120px"></asp:TextBox>
                                    <asp:ImageButton runat="server" ID="imgFaculty" Style="cursor: pointer; vertical-align: top"
                                        ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" OnClick="imgFaculty_Click" />
                                    <asp:TextBox ID="txtFacultyDesignation" runat="server" CssClass="input" Width="0px"
                                        Enabled="false" MaxLength="50"></asp:TextBox>
                                    <asp:TextBox runat="server" ID="txtFacultyId" CssClass="input" Width="0px" MaxLength="20"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTAFFID") %>'></asp:TextBox>
                                    <asp:TextBox runat="server" ID="txtFacultyEmail" CssClass="input" Width="0px"
                                        MaxLength="20"></asp:TextBox>
                                </span>
                            </td>
                            <td>
                                <asp:Literal ID="lblYear" runat="server" Text="Year"></asp:Literal>
                            </td>
                            <td>
                                <eluc:CldYear AppendDataBoundItems="true" ID="ddlYear" runat="server" CssClass="input_mandatory" AutoPostBack="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblMonth" runat="server" Text="Month"></asp:Literal>
                            </td>
                            <td>
                                <eluc:cldMonth AppendDataBoundItems="true" ID="ddlMonth" runat="server" CssClass="input" AutoPostBack="true" />
                            </td>

                        </tr>

                    </table>
                    <br />
                    <div class="navSelect" style="position: relative; clear: both; width: 15px">
                        <eluc:TabStrip ID="MenuPreSea" runat="server" OnTabStripCommand="MenuPreSea_TabStripCommand"></eluc:TabStrip>
                    </div>
                    <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                        <asp:GridView ID="gvMentorPlan" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" OnRowDataBound="gvMentorPlan_RowDataBound" ShowHeader="true"
                            OnRowEditing="gvMentorPlan_RowEditing" OnRowCancelingEdit="gvMentorPlan_RowCancelingEdit"
                            AllowSorting="true" ShowFooter="false" EnableViewState="false" OnRowUpdating="gvMentorPlan_RowUpdating"
                            DataKeyNames="FLDMENTORPLANID">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <RowStyle Height="10px" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblStaff" runat="server" Text="Staff"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDMENTORNAME"]%>
                                        <asp:Label ID="lblMentorId" runat="server" Visible="false"
                                            Text='<%#((DataRowView)Container.DataItem)["FLDMENTORID"]%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblClassRoom" runat="server" Text="Class Room"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="txtClassroom" Visible="false"
                                            Text='<%# ((DataRowView)Container.DataItem)["FLDCLASSROOM"]%>'></asp:Label>
                                        <asp:Label runat="server" ID="lblClassroomName"
                                            Text='<%# ((DataRowView)Container.DataItem)["FLDROOMSHORTNAME"]%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblStrength" runat="server" Text="Strength"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="txtStrength" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDSTRENGTH"]%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblDate" runat="server" Text="Date"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="txtDate" runat="server"
                                            Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATE","{0:dd/MMM/yyyy}")) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblSession" runat="server" Text="Session"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="txtsessionName" runat="server"
                                            Text='<%# ((DataRowView)Container.DataItem)["FLDSESSIONNAME"]%>'></asp:Label>
                                        <asp:Label ID="txtsession" runat="server"
                                            Text='<%# ((DataRowView)Container.DataItem)["FLDSESSION"]%>'></asp:Label>
                                       <%-- <asp:DropDownList ID="ddlSession" runat="server" CssClass="readonlytextbox">
                                            <asp:ListItem Text="1st Session" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="2st Session" Value="2"></asp:ListItem>
                                        </asp:DropDownList>--%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblRemark" runat="server" Text="Remark"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="txtremarks" runat="server"
                                            Text='<%# ((DataRowView)Container.DataItem)["FLDREMARKS"]%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="IsCompletedYN">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblIsCompletedYN" runat="server" Text="IsCompletedYN">                                   
                                        </asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>                                        
                                        <asp:Label ID="lblIsCompleted" runat="server" 
                                            Text='<%# ((DataRowView)Container.DataItem)["FLDCOMPLETEDYN"]%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:CheckBox ID="chkIsCompletedYN" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDCOMPLETEDYN").ToString().Equals("Yes"))?true:false %>'></asp:CheckBox>
                                    </EditItemTemplate>
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
                                            CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                            ToolTip="Edit"></asp:ImageButton>
                                        <%--<img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" visible="false" />
                                        <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                            ToolTip="Delete"></asp:ImageButton>--%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                            CommandName="UPDATE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                            ToolTip="Save"></asp:ImageButton>
                                        <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="CANCEL" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                            ToolTip="Cancel"></asp:ImageButton>
                                    </EditItemTemplate>
                                    <FooterStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
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
        </asp:UpdatePanel>
    </form>
</body>
</html>
