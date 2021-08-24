<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshorePetronasMatrixReport.aspx.cs" Inherits="CrewOffshore_CrewOffshorePetronasMatrixReport" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CldYear" Src="~/UserControls/UserControlCalenderYear.ascx" %>
<%@ Register TagPrefix="eluc" TagName="cldMonth" Src="~/UserControls/UserControlCalenderMonths.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Shell Matrix</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlshellmatrix">
            <ContentTemplate>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">

                    <div class="subHeader" style="position: relative">
                        <div id="divHeading" style="vertical-align: top">
                            <eluc:Title runat="server" ID="ucTitle" Text="Course Planner" ShowMenu="true" />
                        </div>
                        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                    </div>
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                        <eluc:TabStrip ID="Menushellmatrix" runat="server" OnTabStripCommand="Menushellmatrix_TabStripCommand"
                            TabStrip="false">
                        </eluc:TabStrip>
                    </div>
                    <br />
                    <div class="navSelect" style="position: relative; width: 15px">
                        <eluc:TabStrip ID="MenuShellMatrixWeekly" runat="server" OnTabStripCommand="MenuShellMatrixWeekly_TabStripCommand">
                        </eluc:TabStrip>
                    </div>
                    <br />

                    <%-- <b>
                            <asp:Literal ID="lblTitle" runat="server" Text="Shell Weekly Matrix"></asp:Literal></b>--%>
                    <div id="divGrid" style="position: relative; z-index: 0" runat="server">
                        <asp:GridView ID="gvshellmatrixView" runat="server" AutoGenerateColumns="false" Font-Size="11px"
                            Width="100%" CellPadding="3" ShowFooter="false"
                            OnRowCreated="gvshellmatrixView_RowCreated"
                            EnableViewState="False">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <Columns>
                                <asp:TemplateField HeaderText="File No." HeaderStyle-Width="60px">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="60px"></ItemStyle>
                                    <ItemTemplate>
                                        <%--<asp:Label ID="lblFileNo" Text='<%# ((DataRowView)Container.DataItem)["FLDFILENO"].ToString() %>' Width="60px" runat="server"></asp:Label>
                                        --%><asp:Label ID="lblEmployeeId" runat="server" Visible="false"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                        <asp:GridView ID="gvShellNoRecordFound" runat="server" AutoGenerateColumns="false" Font-Size="11px"
                            Width="100%" CellPadding="3" ShowFooter="false" Visible="false"
                            EnableViewState="False">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <Columns>
                                <asp:TemplateField HeaderText="Course" HeaderStyle-Width="30%">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDEMPLOYEEID"].ToString() %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>

                    <eluc:Status ID="ucStatus" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>

