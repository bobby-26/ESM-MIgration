<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsContractComponent.aspx.cs"
    Inherits="VesselAccountsContractComponent" %>

<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Component</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmBondReq" runat="server" autocomplete="off">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlComponent">
            <ContentTemplate>
                <div style="top: 100px; margin-left: auto; margin-right: auto; width: 100%;">
                    <div style="top: 100px; margin-left: auto; margin-right: auto; vertical-align: middle;">
                        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                        <div class="subHeader" style="position: relative; right: 0px">
                            <eluc:Title runat="server" ID="Title1" Text="Contract Component" ShowMenu="false"></eluc:Title>
                        </div>
                    </div>
                    <asp:GridView ID="gvCC" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        GridLines="None" OnRowDataBound="gvCC_RowDataBound" Width="100%" CellPadding="3"
                        ShowHeader="true" OnRowUpdating="gvCC_RowUpdating" EnableViewState="false" OnRowEditing="gvCC_RowEditing"
                        OnRowCancelingEdit="gvCC_RowCancelingEdit" DataKeyNames="FLDCONTRACTCOMPID">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField HeaderText="Component Name">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%# ((DataRowView)Container.DataItem)["FLDCOMPONENTNAME"] %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtComponentName" runat="server" CssClass="gridinput_mandatory"
                                        MaxLength="200" Text='<%# ((DataRowView)Container.DataItem)["FLDCOMPONENTNAME"] %>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Amount(USD)">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <%# ((DataRowView)Container.DataItem)["FLDAMOUNT"] %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="txtAmount" runat="server" CssClass="input_mandatory" Text='<%# ((DataRowView)Container.DataItem)["FLDAMOUNT"] %>' />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Component Type">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%# ((DataRowView)Container.DataItem)["FLDEARDEDTYPE"].ToString() == "1" ? "Earning" : "Deduction" %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">
                                            Action
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Update" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
