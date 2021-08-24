<%@ Page Language="C#" AutoEventWireup="True" CodeFile="RegistersWorkingGearItemLocation.aspx.cs"
    Inherits="RegistersWorkingGearItemLocation" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="WorkingGearType" Src="~/UserControls/UserControlWorkingGearType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZone.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Working Gear Items</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="WorkingGearItems" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmWorkingGearAdditionalItems" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlWorkingGearItem">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="Title1" Text="Working Gear Item Location" ShowMenu="false"></eluc:Title>
                </div>
                <div id="divFind" style="position: relative; z-index: 2">
                    <%--<table id="tblConfigureWorkingGearItem" width="100%">
                        <tr>
                            <td colspan="4">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblItemName" runat="server" Text="Item Name"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtItemName" runat="server" MaxLength="100" CssClass="readonlytextbox"
                                    Width="300px"></asp:TextBox>
                            </td>
                           <td>
                                <asp:Literal ID="lblPrice" runat="server" Text="Price"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Number ID="txtPrice" runat="server" CssClass="readonlytextbox" Width="125px"
                                    IsInteger="false" DecimalPlace="2" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblTotalStockinHand" runat="server" Text="Total Stock in Hand"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Number ID="txtStockinHand" runat="server" CssClass="readonlytextbox" Width="125px"
                                    IsInteger="false" DecimalPlace="2" />
                            </td>
                            <td>
                                <asp:Literal ID="lblTotalStockValueINR" runat="server" Text="Total Stock Value (INR)"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Number ID="txtStockValue" runat="server" CssClass="readonlytextbox" Width="125px"
                                    IsInteger="false" DecimalPlace="2" />
                            </td>
                        </tr>
                    </table>--%>
                    <table id="tblCondtions" width="100%">
                    <tr>
                    <td><asp:Label ID="lblZone" runat="server" Text="Zone"></asp:Label></td>
                    <td><eluc:zone ID="ucZone" runat="server" /></td>
                    </tr>
                    <tr>
                    <td><asp:Label ID="lblDate" runat="server" Text="Opening Date"></asp:Label></td>
                    <td><eluc:Date ID="ucDate" runat="server" /></td>
                    </tr>
                    
                    </table>
                </div>
                <div style="color: Blue;">
                    <asp:Literal ID="lblOnceOpeningbalanceisenterandsavedfortheZoneItcannotbechange" runat="server" Text="Once Opening balance is enter and saved for the Zone, It cannot be change"></asp:Literal>
                    <br />
                    <asp:Literal ID="lblPleaseenterthecorrectnumbers" runat="server" Text="Please enter the correct numbers"></asp:Literal>
                    <br />
                    <asp:Literal ID="lblHereafterStockcorrectioncanbedonethroughstockadjusmentscreenonly" runat="server" Text="Here after, Stock correction can be done through stock adjusment screen only"></asp:Literal>
                </div>
                <br style="clear: both;" />
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuRegistersWorkingGearItem" runat="server" OnTabStripCommand="RegistersWorkingGearItem_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative;">
                    <asp:GridView ID="gvRegistersworkgearitemlocation" runat="server" AutoGenerateColumns="False"
                        Font-Size="11px" Width="95%" CellPadding="3" OnRowCommand="gvRegistersworkgearitemlocation_RowCommand"
                        OnRowDataBound="gvRegistersworkgearitemlocation_ItemDataBound" OnRowCancelingEdit="gvRegistersworkgearitemlocation_RowCancelingEdit"
                        OnRowEditing="gvRegistersworkgearitemlocation_RowEditing" ShowFooter="false"
                        Style="margin-bottom: 0px" EnableViewState="false">
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle BackColor="#f9f9fa" />
                        <SelectedRowStyle BackColor="#bbddff" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <%--<asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderStyle Width="15%" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblZoneCode" runat="server" Text="Zone Code"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblWorkGearitemLocId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKGEARLOCATIONID") %>'></asp:Label>
                                    <asp:Label ID="lblZone" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDZONE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblWorkGearitemLocIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKGEARLOCATIONID") %>'></asp:Label>
                                    <asp:Label ID="lblWorkGearItemIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKINGGEARITEMID") %>'></asp:Label>
                                    <asp:Label ID="lblZoneIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDZONEID") %>'></asp:Label>
                                    <asp:Label ID="lblZoneEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDZONE") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>--%>
                            <%--<asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderStyle Width="30%" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblZoneName" runat="server" Text="Zone Name"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblZoneName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDZONENAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblZoneNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDZONENAME") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>--%>
                             <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderStyle Width="30%" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblSize" runat="server" Text="Size"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblWorkGearitemLocId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKGEARLOCATIONID") %>'></asp:Label>

                                    <asp:Label ID="lblSizeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIZENAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                  <asp:Label ID="lblWorkGearitemLocIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKGEARLOCATIONID") %>'></asp:Label>
                                <asp:Label ID="lblSizeCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIZEID") %>'></asp:Label>
                                    <asp:Label ID="lblSizeNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIZENAME") %>'></asp:Label>
                                    <asp:Label ID="lblWorkGearItemIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKINGGEARITEMID") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                 <HeaderStyle Width="15%" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblOpeningDate" runat="server" Text="Opening Date"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# string.Format("{0:dd/MMM/yyyy}", DataBinder.Eval(Container, "DataItem.FLDOPENINGENTRYDATE"))%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date ID="txtOpenDateEdit" runat="server" CssClass="input_mandatory" />
                                </EditItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderStyle Width="15%" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblStockinHandQty" runat="server" Text="Stock in Hand (Qty)"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblStockinHand" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOCKQUANTITY") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="txtStockinHandEdit" runat="server" CssClass="input" Width="90px"
                                        IsInteger="false" DefaultZero="true" DecimalPlace="2" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderStyle Width="15%" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblStockValueINR" runat="server" Text="Stock Value (INR)"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblStock" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOCKVALUE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblStockEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOCKVALUE") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderStyle Width="15%" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblPriceHeader" runat="server" Text="Price"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPrice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITPRICE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblPriceEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITPRICE") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblAction" runat="server" Text="Action"></asp:Literal>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Save" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="divPage" style="position: relative;">
                    <table width="95%" border="0" style="background-color: #88bbee">
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
                            <td width="20px">
                                &nbsp;
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
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="MenuRegistersWorkingGearItem" />
        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
