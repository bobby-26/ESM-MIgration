<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRACargoperationalHazardPickList.aspx.cs" Inherits="InspectionRACargoperationalHazardPickList" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>MOC Question List</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
     <form id="frmMOC" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlMOC">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Operational Hazard" ShowMenu = "false"></eluc:Title>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                    <eluc:TabStrip ID="MenuMOC" runat="server" OnTabStripCommand="Operational_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                 <div id="divMOCQuestion" style="position: relative; z-index: +1">
                    <asp:GridView ID="gvOperationalHazard" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" 
                         ShowFooter="false" onrowediting="gvMOCQuestion_RowEditing">
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="5px" />
                        <Columns>
                            <asp:TemplateField HeaderText="">
                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="30%"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblElementHeader" runat="server">Element</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblElement" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDELEMENT") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Name">
                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="30%"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblOperationalHeader" runat="server">Operational Hazard</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblOperationalid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKASSESSEMENTOPERATIONALHAZARDID") %>'></asp:Label>
                                    <asp:Label ID="lblOperational" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPERATIONALHAZARD") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="">
                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="30%"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblControlHeader" runat="server">Conrol Precautions</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblControl" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTROLPRECAUTIONS") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblOptionHeader" runat="server">Select</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                 <asp:CheckBox ID="chkRequiredYNEdit" runat="server" OnCheckedChanged = "chkOptions_CheckedChanged" AutoPostBack ="true" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
              </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="gvOperationalHazard" />
        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>

