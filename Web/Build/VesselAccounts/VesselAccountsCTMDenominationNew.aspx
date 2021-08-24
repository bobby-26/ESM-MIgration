<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsCTMDenominationNew.aspx.cs"
    Inherits="VesselAccountsCTMDenominationNew" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>CTM Denomination</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
    <asp:UpdatePanel runat="server" ID="pnlNTBRManager">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="Title1" Text="Cash Request"></eluc:Title>
                </div>
                <div class="navSelect" style="width: auto; float: right; margin-top: -26px">
                    <eluc:TabStrip ID="MenuCTMMain" runat="server" OnTabStripCommand="MenuCTMMain_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                </div>
                <table width="50%" cellpadding="1" cellspacing="1">
                    <tr>
                        <td>
                            <asp:Literal ID="lblCTMtobearranged" runat="server" Text="CTM to be arranged"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Number ID="txtAmount" runat="server" CssClass="input readonlytextbox" ReadOnly="true"
                                Width="90px" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:GridView ID="gvDenomination" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    GridLines="None" Width="50%" CellPadding="3" OnRowDataBound="gvDenomination_RowDataBound"
                    OnRowEditing="gvDenomination_RowEditing" OnRowCancelingEdit="gvDenomination_RowCancelingEdit"
                    OnRowUpdating="gvDenomination_RowUpdating" OnRowDeleting="gvDenomination_RowDeleting"
                    ShowHeader="true" ShowFooter="true" EnableViewState="false" DataKeyNames="FLDDENOMINATIONID">
                    <FooterStyle CssClass="datagrid_footerstyle" Font-Bold="true" HorizontalAlign="Right">
                    </FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" HorizontalAlign="Center" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />
                    <Columns>
                        <asp:TemplateField HeaderText="Denomination">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDDENOMINATION"] %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Notes">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDNOTES"] %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblDenomination" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDDENOMINATION"] %>'></asp:Label>
                                <eluc:Number ID="txtNotes" runat="server" Width="90px" CssClass="input_mandatory"
                                    IsInteger="true" Text='<%#((DataRowView)Container.DataItem)["FLDNOTES"] %>' MaxLength="3" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:Literal ID="lblTotal" runat="server" Text="Total"></asp:Literal>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDTOTAL"]%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblAction" runat="server" Text="Action"></asp:Literal>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="Update" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
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
