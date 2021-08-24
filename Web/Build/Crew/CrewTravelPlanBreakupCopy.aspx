<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelPlanBreakupCopy.aspx.cs"
    Inherits="Crew_CrewTravelPlanBreakupCopy" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="../UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Copy Breakup</title>
   <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
   </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlBreakupCopy">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="Breakup" Text="Copy Breakup" ShowMenu="false"></eluc:Title>
                    <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuBreakupCopy" runat="server" OnTabStripCommand="MenuBreakupCopy_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <asp:GridView ID="gvPlan" runat="server" AutoGenerateColumns="False" CellPadding="3"
                    EnableViewState="false" Font-Size="11px" ShowHeader="true" Width="100%">
                    <FooterStyle CssClass="datagrid_footerstyle" />
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />
                    <Columns>
                        <asp:TemplateField HeaderText="Copy">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <HeaderTemplate>
                                <asp:Literal ID="lblCopy" runat="server" Text="Copy"></asp:Literal>
                            </HeaderTemplate>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <asp:CheckBox ID="chkCopy" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Name">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <asp:Literal ID="lblEmployee" runat="server" Text=" Employee"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblEmployeeId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'
                                    Visible="false"></asp:Label>
                                <asp:Label ID="lblRequestId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTID") %>'
                                    Visible="false"></asp:Label>
                                <asp:Label ID="lblCrewPlanID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWPLANID") %>'
                                    Visible="false"></asp:Label>
                                <%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Family Name">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <asp:Literal ID="lblFamily" runat="server" Text="Family"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblFamilyId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFAMILYID") %>'
                                    Visible="false"></asp:Label>
                                <%# DataBinder.Eval(Container,"DataItem.FLDFNAME") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Rank">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblRank" runat="server" Text="Rank"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDRANKNAME")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="On/Off-Signer">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblOnOffSigner" runat="server" Text="On/Off-Signer"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDONSIGNERYN")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Origin">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblOrig" runat="server" Text="Origin"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDORIGIN")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Destination">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblDest" runat="server" Text="Destination"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDDESTINATION")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <eluc:Status ID="ucStatus" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
