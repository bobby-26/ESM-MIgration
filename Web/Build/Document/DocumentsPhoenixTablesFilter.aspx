<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentsPhoenixTablesFilter.aspx.cs"
    Inherits="DocumentsPhoenixTablesFilter" %>
<%@ Import Namespace="System.Data" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tables" Src="~/UserControls/UserControlVesselTables.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>        
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader" style="position: relative">
            <div id="divHeading" style="vertical-align: top">
                <asp:Label runat="server" ID="lblCaption" Font-Bold="true" Text="Phoenix Tables Filter"></asp:Label>
            </div>
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuOfficeFilterMain" runat="server" OnTabStripCommand="OfficeFilterMain_TabStripCommand">
            </eluc:TabStrip>
            
        </div>
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        
        <%--<eluc:Tables ID="ddlTables" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" Visible="false"
                                AppendOwnerCharterer="true" />--%>
        <%--<asp:UpdatePanel runat="server" ID="pnlAddressEntry">
            <ContentTemplate>            
                <div>
                    <asp:PlaceHolder ID="DynamicControlsHolder" runat="server"></asp:PlaceHolder>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />--%>
        <table cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        Vessel
                    </td>
                    <td>
                        <eluc:Vessel ID="ddlVessel" runat="server" CssClass="input" VesselsOnly="true" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        From Date
                    </td>
                    <td>
                        <eluc:Date ID="txtFromDate" runat="server" CssClass="input" />
                    </td>
                    <td>
                        To Date
                    </td>
                    <td>
                        <eluc:Date ID="txtToDate" runat="server" CssClass="input" />
                    </td>
                </tr>
        </table>
        <br />
        <asp:GridView ID="gvTableColumnVarcharList" runat="server" AutoGenerateColumns=false Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowDataBound="gvTableColumnVarcharList_RowDataBound"
                        OnRowCancelingEdit="gvTableColumnVarcharList_RowCancelingEdit"
                        ShowHeader="true" EnableViewState="false">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField HeaderText="Field Name">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblFieldName" runat="server" Text='<%#((DataRowView)Container.DataItem)["COLUMN_NAME"]%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Operator">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:RadioButtonList ID="rOpertor" runat="server">
                                        <asp:ListItem Value="LIKE" Text="Like" Selected></asp:ListItem>
                                        <asp:ListItem Value="=" Text="Equal To"></asp:ListItem>
                                        <asp:ListItem Value="!=" Text="Not Equal To"></asp:ListItem>
                                    </asp:RadioButtonList>  
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Value">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtValue" runat="server" CssClass="input"></asp:TextBox>    
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
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </ItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
        </asp:GridView>
    </form>
</body>
</html>
