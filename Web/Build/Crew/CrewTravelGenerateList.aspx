<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelGenerateList.aspx.cs"
    Inherits="Crew_CrewTravelGenerateList" %>

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
<head id="Head1" runat="server">
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
    <asp:UpdatePanel runat="server" ID="pnlTravel">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="Breakup" Text="Travel Existing Request" ShowMenu="false"></eluc:Title>
                    <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuTravel" runat="server" OnTabStripCommand="MenuTravel_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <table cellpadding="1" cellspacing="1" >
                    <tr>
                        <td>
                            <asp:Literal ID="lblSelect" runat="server" Text="Raise Travel"></asp:Literal>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rbGenerateType" runat="server" RepeatDirection="Horizontal"
                                AutoPostBack="true" >
                                <asp:ListItem Value="1" >New</asp:ListItem>
                                <asp:ListItem Value="2" Selected="true">Existing</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="gvTravel" runat="server" AutoGenerateColumns="False" CellPadding="3"
                    EnableViewState="false" Font-Size="11px" ShowHeader="true" Width="100%">
                    <FooterStyle CssClass="datagrid_footerstyle" />
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <asp:Literal ID="lblRequestNo" runat="server" Text=" Request No"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkRequestNO" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDREQUISITIONNO") %>'
                                    CommandName="SELECT" CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
                                  <asp:Label ID="lblTravelId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELID") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderTemplate>
                                <asp:Literal ID="lblDate" runat="server" Text="Crew Change"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDDATEOFCREWCHANGE", "{0:dd/MM/yyyy}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderTemplate>
                                <asp:Literal ID="lblPort" runat="server" Text="Port"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCrewChangePort" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderTemplate>
                                <asp:Literal ID="lblReason" runat="server" Text="Reason"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCrewChangeReason" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREASON") %>'></asp:Label>
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
