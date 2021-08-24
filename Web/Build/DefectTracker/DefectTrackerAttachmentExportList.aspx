<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefectTrackerAttachmentExportList.aspx.cs"
    Inherits="DefectTrackerAttachmentExportList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>DMS Export</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlPatchEntry">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="subHeader">
                <div id="divHeading" class="divFloatLeft">
                    <eluc:Title runat="server" ID="Title1" Text="EDMS Export" ShowMenu="true" />
                </div>
                <div style="position: absolute; top: 0px; right: 0px">
                    <eluc:TabStrip ID="MenuEDMSExport" runat="server" OnTabStripCommand="MenuEDMSExport_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                    <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                </div>
            </div>
            <div id="divGrid" style="position: relative; z-index: 1">
                <asp:GridView ID="gvDataExport" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />
                    <Columns>
                        <asp:TemplateField HeaderText="Patch Name">
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Vessel Name
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lblFileName" CommandName="SELECT" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDVESSELNAME").ToString() %>'> </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Patch Name">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                Last Export
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblSerial" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDENTITYSERIAL").ToString() %>'> </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Patch Name">
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Export Date
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblExportDate" runat="server" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDEXPORTDATE", "{0:dd/MM/yyyy}")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
    </div>
    </form>
</body>
</html>
