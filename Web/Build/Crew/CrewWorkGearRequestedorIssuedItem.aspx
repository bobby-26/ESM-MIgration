<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewWorkGearRequestedorIssuedItem.aspx.cs"
    Inherits="CrewWorkGearRequestedorIssuedItem" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
    <%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZone.ascx" %>
<%@ Register TagPrefix="eluc" TagName="WorkingGearItemType" Src="~/UserControls/UserControlWorkingGearItemType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Size" Src="~/UserControls/UserControlSize.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiAddr" Src="~/UserControls/UserControlMultiColumnAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<head id="Head1" runat="server">
    <title>Working Gear Needed Item</title>
      <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <div class="subHeader" style="position: relative">
                <div id="divHeading" style="vertical-align: top">
                    <eluc:Title runat="server" ID="ucTitle" Text="" ShowMenu="false" />
                    <eluc:Status runat="server" ID="ucStatus" Visible="false" />
                </div>
            </div>
            <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                <eluc:TabStrip ID="CrewWorkGearNeededItemRequest" runat="server" OnTabStripCommand="CrewWorkGearNeededItemRequest_TabStripCommand"></eluc:TabStrip>
            </div>
            <asp:Label ID="lblissued" runat="server" Text="Working Gear Issued List" Font-Bold="true"></asp:Label>
            <div id="divGrid" style="position: relative;">
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="Menuitems" runat="server" OnTabStripCommand="Menuitems_TabStripCommand"></eluc:TabStrip>
                </div>
                <asp:GridView ID="gvRegistersworkinggearitem" runat="server" AutoGenerateColumns="False"
                    Font-Size="11px" Width="100%" CellPadding="3"
                    OnRowDataBound="gvRegistersworkinggearitem_ItemDataBound"
                    AllowSorting="true" Style="margin-bottom: 0px" EnableViewState="false">
                    <FooterStyle ForeColor="#000066" BackColor="#dfdfdf"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" HorizontalAlign="Left" />
                    <AlternatingRowStyle BackColor="#f9f9fa" />
                    <SelectedRowStyle BackColor="#bbddff" Font-Bold="true" />
                    <RowStyle Height="10px" />
                    <Columns>
                        <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <HeaderTemplate>
                                <asp:Label ID="lblRemarksitemHeader" runat="server" Text="Working Gear Item">
                                    
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>

                                <asp:Label ID="lblGearitemiditem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKINGGEARITEMNAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblUnitPriceHeader" runat="server" Text=" Unit Price">
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblunitPrice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITPRICE") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblQuantityHeader" runat="server" Text="Requested Quantity">
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblGearitemQuantityitem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblOrderQuantity" runat="server" Text="Order Quantity">
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblOrderQuantityItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERQUANTITY") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblrQuantityHeader" runat="server" Text="Received Quantity">
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblrRGearitemQuantityitem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIVEDQUANTITY") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>
            </div>
            <div id="divPage" style="position: relative;">
                <table width="100%" border="0" style="background-color: #88bbee">
                    <tr>
                        <td nowrap="nowrap" align="center">
                            <asp:Label ID="lblPagenumber" runat="server">
                            </asp:Label>
                            <asp:Label ID="lblPages" runat="server">
                            </asp:Label>
                            <asp:Label ID="lblRecords" runat="server">
                            </asp:Label>&nbsp;&nbsp;
                        </td>
                        <td nowrap="nowrap" align="left" width="50px">
                            <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                        </td>
                        <td width="20px">&nbsp;
                        </td>
                        <td nowrap="nowrap" align="right" width="50px">
                            <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                        </td>
                        <td nowrap="nowrap" align="center">
                            <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                            </asp:TextBox>
                            <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                Width="40px"></asp:Button>
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <br />
            <asp:Label ID="lblRequest" runat="server" Text="Working Gear Requested List" Font-Bold="true"></asp:Label>
            <div style="position: relative;">
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="Menuitems1" runat="server" OnTabStripCommand="Menuitems_TabStripCommand1"></eluc:TabStrip>
                </div>
                <asp:GridView ID="gvRegistersworkinggearitemReq" runat="server" AutoGenerateColumns="False"
                    Font-Size="11px" Width="100%" CellPadding="3"
                    AllowSorting="true" Style="margin-bottom: 0px" EnableViewState="false">
                    <FooterStyle ForeColor="#000066" BackColor="#dfdfdf"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" HorizontalAlign="Left" />
                    <AlternatingRowStyle BackColor="#f9f9fa" />
                    <SelectedRowStyle BackColor="#bbddff" Font-Bold="true" />
                    <RowStyle Height="10px" />
                    <Columns>
                        <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblRemarksitemHeader" runat="server" Text="Working Gear Item">
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblGearitemiditem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKINGGEARITEMNAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblUnitPriceHeader" runat="server" Text=" Unit Price">
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblunitPrice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITPRICE") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblQuantityHeader" runat="server" Text="Requested Quantity">
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblGearitemQuantityitem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblOrderQuantity" runat="server" Text="Order Quantity">
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblOrderQuantityItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERQUANTITY") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblrQuantityHeader" runat="server" Text="Received Quantity">
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblrRGearitemQuantityitem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIVEDQUANTITY") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div id="divPage" style="position: relative;">
                <table width="100%" border="0" style="background-color: #88bbee">
                    <tr>
                        <td nowrap="nowrap" align="center">
                            <asp:Label ID="lblPagenumber1" runat="server">
                            </asp:Label>
                            <asp:Label ID="lblPages1" runat="server">
                            </asp:Label>
                            <asp:Label ID="lblRecords1" runat="server">
                            </asp:Label>&nbsp;&nbsp;
                        </td>
                        <td nowrap="nowrap" align="left" width="50px">
                            <asp:LinkButton ID="cmdPrevious1" runat="server" OnCommand="PagerButtonClick1" CommandName="prev1">Prev << </asp:LinkButton>
                        </td>
                        <td width="20px">&nbsp;
                        </td>
                        <td nowrap="nowrap" align="right" width="50px">
                            <asp:LinkButton ID="cmdNext1" OnCommand="PagerButtonClick1" runat="server" CommandName="next1">Next >></asp:LinkButton>
                        </td>
                        <td nowrap="nowrap" align="center">
                            <asp:TextBox ID="txtnopage1" MaxLength="3" Width="20px" runat="server" CssClass="input">
                            </asp:TextBox>
                            <asp:Button ID="btnGo1" runat="server" Text="Go" OnClick="cmdGo_Click1" CssClass="input"
                                Width="40px"></asp:Button>
                        </td>
                    </tr>
                </table>
            </div>
    </form>
</body>
</html>
