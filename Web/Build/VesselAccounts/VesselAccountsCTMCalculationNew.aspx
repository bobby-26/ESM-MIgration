<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsCTMCalculationNew.aspx.cs"
    Inherits="VesselAccounts_VesselAccountsCTMCalculationNew" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew BOW</title>
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
                <div class="subHeader">
                    <div style="position: absolute; right: 0px">
                        <eluc:TabStrip ID="MenuCTM" runat="server" OnTabStripCommand="MenuCTM_TabStripCommand">
                        </eluc:TabStrip>
                    </div>
                </div>
                <table width="50%" cellpadding="1" cellspacing="1">
                    <tr>
                        <td>
                            <asp:Literal ID="lblOpeningBalance" runat="server" Text="Opening Balance"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtOpening" runat="server" CssClass="readonlytextbox txtNumber"
                                ReadOnly="true" Width="90px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="gvCTM" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    GridLines="None" Width="50%" CellPadding="3" OnRowDataBound="gvCTM_RowDataBound"
                    ShowHeader="true" OnRowEditing="gvCTM_RowEditing" OnRowCancelingEdit="gvCTM_RowCancelingEdit"
                    OnRowUpdating="gvCTM_RowUpdating" ShowFooter="true" EnableViewState="false" DataKeyNames="FLDCALCULATIONID">
                    <FooterStyle CssClass="datagrid_footerstyle" Font-Bold="true" HorizontalAlign="Right">
                    </FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <Columns>
                        <asp:TemplateField HeaderText="Purpose">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDPURPOSE"] %>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Literal ID="lblTotal" runat="server" Text="Total"></asp:Literal>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Amount">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDAMOUNT"] %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblType" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDTYPE"] %>'></asp:Label>
                                <eluc:Number ID="txtAmount" runat="server" CssClass="input_mandatory" MaxLength="8"
                                    Text='<%#((DataRowView)Container.DataItem)["FLDAMOUNT"] %>' />
                            </EditItemTemplate>
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
                <table width="50%" cellpadding="1" cellspacing="1">
                    <tr>
                        <td>
                            <asp:Literal ID="lblCashOnBoard" runat="server" Text="Cash On Board"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCashonBoard" runat="server" CssClass="readonlytextbox txtNumber"
                                ReadOnly="true" Width="90px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblEstimate" runat="server" Text="Estimate Expenses"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEstimate" runat="server" CssClass="readonlytextbox txtNumber"
                                ReadOnly="true" Width="90px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="Shortfall" runat="server" Text="Shortfall"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBalance" runat="server" CssClass="readonlytextbox txtNumber"
                                ReadOnly="true" Width="90px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblRoundOffAmount" runat="server" Text="Round Off Amount"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRoundOffAmount" runat="server" CssClass="readonlytextbox txtNumber"
                                Width="90px" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblCTMtobearranged" runat="server" Text="CTM to be arranged"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Number ID="txtCTM" runat="server" CssClass="input_mandatory" IsInteger="true"
                                Width="90px" />
                        </td>
                    </tr>
                </table>
                <br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
