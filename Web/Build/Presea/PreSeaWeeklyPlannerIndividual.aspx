<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaWeeklyPlannerIndividual.aspx.cs"
    Inherits="PreSeaWeeklyPlannerIndividual" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="System.Data" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Weekly Planner</title>
    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
</telerik:RadCodeBlock>
</head>
<body>
    <form id="frmIndividualWeekPlan" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlWeeklyPlanner">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="subHeader" style="position: relative">
                <eluc:Title runat="server" ID="ucTitle" Text="Weekly Planner - Individual" ShowMenu="false">
                </eluc:Title>
            </div>
           
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <asp:Literal ID="lblStaff" runat="server" Text="Name"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtName" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="150px"></asp:TextBox>
                    </td>
                </tr>
            </table>            
             <div class="navSelect" style="position: relative; clear: both; width: 15px">
                <eluc:TabStrip ID="MenuPreSea" runat="server" OnTabStripCommand="MenuPreSea_TabStripCommand">
                </eluc:TabStrip>
            </div>
            <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                <asp:GridView ID="gvWeeklyPlanner" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" ShowHeader="true" AllowSorting="true" ShowFooter="false"
                    EnableViewState="false">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblDate" runat="server" Text="Date"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDDATE"]%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblTime" runat="server" Text="Time"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDTIMESLOT"]%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblCourse" runat="server" Text="Course"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDPRESEACOURSENAME"]%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblBatch" runat="server" Text="Batch"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDBATCH"]%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblSemester" runat="server" Text="Semester"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDSEMESTERID"]%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblSection" runat="server" Text="Section"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDSECTIONNAME"]%>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblPractical" runat="server" Text="Practical"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDPRACTICALNAME"]%>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblSubject" runat="server" Text="Subject"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDSUBJECTNAME"]%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblClassRoom" runat="server" Text="Class Room"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDROOMNAME"]%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
