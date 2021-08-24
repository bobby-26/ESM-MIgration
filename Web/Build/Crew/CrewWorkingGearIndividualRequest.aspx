<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewWorkingGearIndividualRequest.aspx.cs"
    Inherits="CrewWorkingGearIndividualRequest" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZone.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiAddr" Src="~/UserControls/UserControlMultiColumnAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Size" Src="~/UserControls/UserControlWorkingGearSize.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew In-Active</title>
      <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
         <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewList" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlWorkGearRequest">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <div id="divHeading" style="vertical-align: top">
                        <eluc:Title runat="server" ID="ucTitle" Text="Request from Vendor" ShowMenu="false" />
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                    <eluc:TabStrip ID="CrewWorkGearIndividualRequest" runat="server" OnTabStripCommand="CrewWorkGearIndividualRequest_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div style="color: Blue;">
                    1. First save the request date, supplier name and payable by.
                    <br />
                    2. By Default item list will show, based on the configured set of items against
                    the proposed vessel and the seafarer's rank
                    <br />
                    3. You can add extra items by clicking "Add Items" tab
                </div>
                <br style="clear: both;" />
                <div id="divDetails" style="position: relative; z-index: 2; width: 100%;">
                    <table width="100%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                                <asp:Literal ID="lblName" runat="server"  Text="Name"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtName" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="150px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblEmployeeNumber" runat="server" Text="Employee Number"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblRank" runat="server" Text="Rank"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtVessel" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td colspan="4">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblReferenceNo" runat="server" Text="Reference No"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtRefNo" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblRequestDate" runat="server" Text="Request date"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Date ID="txtRequestDate" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td>
                                <asp:Literal ID="lblSupplier" runat="server" Text="Supplier"></asp:Literal>
                            </td>
                            <td>
                                <eluc:MultiAddr ID="ucMultiAddr" AddressType="130,131,132" runat="server" CssClass="input_mandatory"
                                    Width="300px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblPayableBy" runat="server" Text="Payable by"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlPayby" runat="server" CssClass="input_mandatory">
                                    <asp:ListItem Text="-Select-" Value=""></asp:ListItem>
                                    <asp:ListItem Text="Office" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Seafarer" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td colspan="2">
                                &nbsp;
                            </td>
                            <td colspan="2" style="color: Blue;">
                                To find supplier, “Please type some letter of the vendor name and click “enter”
                                to search”
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <h4>
                    <asp:Literal ID="lblRequestedItems" runat="server" Text="Requested Items"></asp:Literal></h4>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="WorkGearRequestItems" runat="server" OnTabStripCommand="WorkGearRequestItems_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                    <asp:GridView ID="gvReq" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowHeader="true" DataKeyNames="FLDORDERLINEID"
                        OnRowEditing="gvReq_RowEditing" OnRowCancelingEdit="gvReq_RowCancelingEdit" OnRowDataBound="gvReq_RowDataBound"
                        EnableViewState="false" OnRowUpdating="gvReq_RowUpdating" AllowSorting="true"
                        OnRowDeleting="gvReq_RowDeleting">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblItemHeader" runat="server">Item</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblOrderlineid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERLINEID") %>'></asp:Label>
                                    <asp:Label ID="lblItemId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKINGGEARITEMID") %>'></asp:Label>
                                    <asp:LinkButton ID="lblItem" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex%>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKINGGEARITEMNAME") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblOrderlineidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERLINEID") %>'></asp:Label>
                                    <asp:LinkButton ID="lblItemEdit" runat="server" CommandArgument='<%# Container.DataItemIndex%>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKINGGEARITEMNAME") %>'></asp:LinkButton>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblSizeHeader" runat="server" >Size</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSize" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIZENAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Size ID="ucSizeEdit" runat="server" AppendDataBoundItems="true" SelectedSize='<%# DataBinder.Eval(Container,"DataItem.FLDSIZEID") %>' 
                                    SizeList=' <%#PhoenixWorkingGearSize.ListSize(General.GetNullableGuid(DataBinder.Eval(Container,"DataItem.FLDWORKINGGEARITEMID").ToString())) %>'
                                        CssClass="gridinput_mandatory" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblQuatityHeader" runat="server" >Quantity</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="txtQuantityEdit" runat="server" CssClass="input_mandatory" Width="90px"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY") %>' />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server" >Action</asp:Label>
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
                                        CommandName="UPDATE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="CANCEL" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <table width="100%" border="0" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap align="center">
                                <asp:Label ID="lblPagenumber" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblPages" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblRecords" runat="server">
                                </asp:Label>&nbsp;&nbsp;
                            </td>
                            <td nowrap align="left" width="50px">
                                <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap align="right" width="50px">
                                <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                            </td>
                            <td nowrap align="center">
                                <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                </asp:TextBox>
                                <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                    Width="40px"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
                <eluc:Status ID="ucStatus" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
