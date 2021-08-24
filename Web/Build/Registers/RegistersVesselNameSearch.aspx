<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersVesselNameSearch.aspx.cs"
    Inherits="RegistersVesselNameSearch" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%--<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Vessel Name Search</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <div id="Div1" runat="server">
            <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
            <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
            <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />

            <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

            <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

            <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        </div>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersVesselNameSearch" runat="server">
        <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
            runat="server">
        </ajaxToolkit:ToolkitScriptManager>

        <eluc:TabStrip ID="MenuRegisters" runat="server" OnTabStripCommand="MenuRegisters_TabStripCommand" TabStrip="true" />
        <table id="tblVesselNameSearch" width="50%">
            <tr>
                <td>
                    <asp:Literal ID="lblVesselName" runat="server" Text="Vessel Name"></asp:Literal>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtSearch" runat="server" MaxLength="100" CssClass="input"></telerik:RadTextBox>
                </td>
            </tr>
        </table>
        <eluc:TabStrip RenderMode="Lightweight" ID="MenuRegistersNameSearch" runat="server" OnTabStripCommand="RegistersNameSearch_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        <telerik:RadGrid ID="gvVesselNameSearch" runat="server" AutoGenerateColumns="False" OnNeedDataSource="gvVesselNameSearch_NeedDataSource"
            Font-Size="11px" Width="100%" CellPadding="3">
            <MasterTableView InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AutoGenerateColumns="false"
                DataKeyNames="FLDVESSELID" TableLayout="Fixed" EnableHeaderContextMenu="true">
        <%--        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />--%>
                <%--                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Vessel Id" Visible="false">
                        <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                        <headertemplate>
                                    <asp:Literal ID="lblHeadVesselId" runat="server" Text="Vessel Id"></asp:Literal>
                                </headertemplate>
                        <itemtemplate>
                                    <asp:Label ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></asp:Label>
                                </itemtemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Current Name">
                        <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                        <headertemplate>
                                    <asp:Literal ID="lblHeadCurrentVesselName" runat="server" Text="Current Name"></asp:Literal>
                                </headertemplate>
                        <itemtemplate>
                                    <asp:Label ID="lblCurrentVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENTNAME") %>'></asp:Label>
                                </itemtemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Vessel Name">
                        <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                        <headertemplate>
                                    <asp:Literal ID="lblHeadVesselName" runat="server" Text="Vessel Name"></asp:Literal>
                                </headertemplate>
                        <itemtemplate>
                                    <asp:Label ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                                </itemtemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Vessel Code">
                        <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                        <headertemplate>
                                    <asp:Literal ID="lblHeadVesselCode" runat="server" Text="Vessel Code"></asp:Literal>
                                </headertemplate>
                        <itemtemplate>
                                    <asp:Label ID="lblVesselCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELCODE") %>'></asp:Label>
                                </itemtemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="W.E.F">
                        <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                        <headertemplate>
                                    <asp:Label ID="lblHeadWEF" runat="server" Text="W.E.F"></asp:Label>
                                </headertemplate>
                        <itemtemplate>
                                    <asp:Label ID="lblWEF" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWEF") %>'></asp:Label>
                                </itemtemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Valid Till">
                        <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                        <headertemplate>
                                    <asp:Literal ID="lblHeadValidUntill" runat="server" Text="Valid Till"></asp:Literal>
                                </headertemplate>
                        <itemtemplate>
                                    <asp:Label ID="lblValidUntill" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALIDUNTIL") %>'></asp:Label>
                                </itemtemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Flag">
                        <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                        <headertemplate>
                                    <asp:Label ID="lblHeadFlag" runat="server" Text="Flag"></asp:Label>
                                </headertemplate>
                        <itemtemplate>
                                    <asp:Label ID="lblFlag" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLAGNAME") %>'></asp:Label>
                                </itemtemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Vessel Type">
                        <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                        <headertemplate>
                                    <asp:Literal ID="lblHeadVesselType" runat="server" Text="Vessel Type"></asp:Literal>
                                </headertemplate>
                        <itemtemplate>
                                    <asp:Label ID="lblVesselType" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE") %>'></asp:Label>
                                </itemtemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Owner">
                        <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                        <headertemplate>
                                    <asp:Literal ID="lblHeadOwner" runat="server" Text="Owner"></asp:Literal>
                                </headertemplate>
                        <itemtemplate>
                                    <asp:Label ID="lblOwner" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERNAME") %>'></asp:Label>
                                </itemtemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Modified Date">
                        <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                        <headertemplate>
                                    <asp:Label ID="lblHeadModifiedDate" runat="server" Text="Modified Date"></asp:Label>
                                </headertemplate>
                        <itemtemplate>
                                    <asp:Label ID="lblModifiedDate" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDMODIFIEDDATE") %>'></asp:Label>
                                </itemtemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
    </form>
</body>
</html>
