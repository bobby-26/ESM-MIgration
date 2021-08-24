<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersPersonalProfileRemarksMapping.aspx.cs" Inherits="RegistersPersonalProfileRemarksMapping" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title></title>
<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRemarksList" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <div class="subHeader" style="position: relative">
            <div id="divHeading" style="vertical-align: top">
                <eluc:Title runat="server" ID="ucTitle" Text="Appraisal Remarks" ShowMenu="false" />
            </div>
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuRemarks" runat="server" OnTabStripCommand="MenuRemarks_TabStripCommand"></eluc:TabStrip>
        </div>
        <asp:UpdatePanel runat="server" ID="pnlRemarks">
            <ContentTemplate>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div id="divGrid1" style="position: relative;">
                    <h4>
                        <asp:Literal ID="lblRemarksSelected" runat="server" Text="Selected Remarks"></asp:Literal></h4>
                    <asp:GridView ID="gvSelectedRemarks" runat="server" AutoGenerateColumns="False" CellPadding="3"
                        Font-Size="11px" ShowFooter="False" ShowHeader="true" Width="100%" EnableViewState="false" OnRowDataBound="gvSelectedRemarks_RowDataBound"
                        OnRowDeleting="gvSelectedRemarks_RowDeleting"> 
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblSelectedRemarksHeader" runat="server" Text="Remarks"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSelectedRemarksId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPRAISALREMARKSID") %>'></asp:Label>
                                    <asp:Label ID="lblSelectedRemarksName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPRAISALREMARKS") %>'></asp:Label>
                                    <asp:Label runat="server" ID="lblAppraisalRemarksMappingId" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPRAISALREMARKSMAPPINGID") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server"> 
                                    Action                                           
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl='<%$ PhoenixTheme:images/te_del.png%>'
                                        CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="divGrid" style="position: relative;">
                    <h4>
                        <asp:Literal ID="lblAppraisalRemarksRemaining" runat="server" Text="Remarks to be Selected"></asp:Literal></h4>
                    <asp:GridView ID="gvRemarks" runat="server" AutoGenerateColumns="False" CellPadding="3"
                        Font-Size="11px" OnRowCommand="gvRemarks_RowCommand" OnRowDataBound="gvRemarks_RowDataBound"
                        OnRowEditing="gvRemarks_RowEditing" OnSorting="gvRemarks_Sorting" AllowSorting="true"
                        ShowFooter="False" ShowHeader="true" Width="100%" EnableViewState="false">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblSelect" runat="server" Text="Select"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="chkSelect" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblAppraisalRemarksHeader" runat="server" Text="Remarks"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblAppraisalRemarksId" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPRAISALREMARKSID") %>' Visible="false"></asp:Label>
                                    <asp:Label runat="server" ID="lblAppraisalRemarksName" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPRAISALREMARKS") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="gvRemarks" />
            </Triggers>
        </asp:UpdatePanel>
    </form>
</body>
</html>
