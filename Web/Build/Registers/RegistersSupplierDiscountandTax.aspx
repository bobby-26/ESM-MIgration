<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersSupplierDiscountandTax.aspx.cs"
    Inherits="Registers_RegisterSupplierDiscountandTax" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Category" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tax" Src="~/UserControls/UserControlTaxMaster.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TaxType" Src="~/UserControls/UserControlTaxType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>RegistersSuppliersDiscountandTax</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

    <style type="text/css">
        .style1
        {
            width: 419px;
        }
    </style>
</telerik:RadCodeBlock></head>
<body>
    <form id="suppileranddiscount" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlSupplier">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Supplier" />
                    </div>
                </div>
                <div id="divFind" style="position: relative; z-index: 2">
                    <table id="tblSupplier" width="100%">
                        <tr>
                            <td>
                                <asp:Literal ID="lblSupplier" runat="server" Text="Supplier"></asp:Literal>
                            </td>
                            <td>
                                <span id="spnPickListMaker">
                                    <asp:TextBox ID="txtSupplierNumber" runat="server" Width="60px" CssClass="input"
                                        Enabled="False"></asp:TextBox>
                                    <asp:TextBox ID="txtSupplierName" runat="server" Width="180px" CssClass="input" Enabled="False"></asp:TextBox>
                                    <asp:ImageButton runat="server" ID="cmdShowMaker" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" OnClientClick="return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=130,131', true);"
                                        Text=".." />
                                    <asp:TextBox ID="txtVendor" runat="server" Width="0px" CssClass="input"></asp:TextBox>
                                </span>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuRegistersSupplierType" runat="server" OnTabStripCommand="RegistersSupplierType_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0">
                    <asp:GridView ID="gvSupplierType" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvSupplierType_RowCommand" OnRowDataBound="gvSupplierType_ItemDataBound"
                        OnRowCreated="gvSupplierType_RowCreated" OnRowCancelingEdit="gvSupplierType_RowCancelingEdit"
                        OnRowDeleting="gvSupplierType_RowDeleting" OnRowUpdating="gvSupplierType_RowUpdating"
                        AllowSorting="True" OnSorting="gvSupplierType_Sorting" EnableViewState="False"
                        OnRowEditing="gvSupplierType_RowEditing" ShowFooter="True">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkSuppliercodeHeader" runat="server" CommandName="Sort" CommandArgument="FLDSUPPLIERCODE"
                                        ForeColor="White">Supplier&nbsp;</asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSupplierCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                   <asp:Label ID="lblSupplierCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                                    <asp:TextBox ID="txtSupplierCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPPLIERID") %>' Visible ="false" 
                                        CssClass="gridinput_mandatory" MaxLength="200"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <span id="spnPickListMaker1">
                                        <asp:TextBox ID="txtSupplierNumber" runat="server" Width="60px" CssClass="input"
                                            Enabled="False"></asp:TextBox>
                                        <asp:TextBox ID="txtSupplierName" runat="server" Width="180px" CssClass="input" Enabled="False"></asp:TextBox>
                                        <asp:ImageButton runat="server" ID="cmdShowMaker" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                            mageAlign="AbsMiddle" OnClientClick="return showPickList('spnPickListMaker1', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=130,131', true);"
                                            Text=".." />
                                        <asp:TextBox ID="txtVendor" runat="server" Width="0px" CssClass="input"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblTypeHeader" runat="server">Type </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTDCTYPENAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Category ID="ucTypeEdit" HardList='<%# PhoenixRegistersHard.ListHard(1,120)%>'
                                        SelectedHard='<%# DataBinder.Eval(Container,"DataItem.FLDTDCTYPE") %>' AppendDataBoundItems="true"
                                        HardTypeCode="120" runat="server" CssClass="dropdown_mandatory" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Category ID="ucTypeAdd" AppendDataBoundItems="true" HardList='<%# PhoenixRegistersHard.ListHard(1,120)%>'
                                        HardTypeCode="120" runat="server" CssClass="dropdown_mandatory" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblValuePerventageHeader" runat="server">Value/percentage </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblValuePercentage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTAXTYPENAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:TaxType ID="ucTaxTypeEdit" runat="server" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:TaxType ID="ucValueTypeAdd" runat="server" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblValueHeader" runat="server">Value </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblValue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTDCVALUE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtValueEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTDCVALUE") %>'
                                        CssClass="gridinput_mandatory" MaxLength="200"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtValueAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="200"
                                        ToolTip="Enter Value"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblDiscountDateHeader" runat="server">Discount Upto</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDiscountDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISCOUNTEFFECTIVEDATE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDiscountDateEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISCOUNTEFFECTIVEDATE") %>'
                                        CssClass="gridinput_mandatory" MaxLength="200"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Date ID="ucDiscountDateAdd" runat="server" CssClass="input_mandatory" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblDiscountAfterHeader" runat="server">Is Valid After Expiry Date</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDiscountAfterDate" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDISDELAYEDUTILIZATIONOFDISCOUNT").ToString().Equals("1"))?"Yes":"No" %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkDiscountAfterEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDISDELAYEDUTILIZATIONOFDISCOUNT").ToString().Equals("1"))?true:false %>'>
                                    </asp:CheckBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:CheckBox ID="chkDiscountAfterAdd" runat="server"></asp:CheckBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblremarksheader" runat="server">Remarks </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblremarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtSupplierRemarkEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'
                                        CssClass="gridinput_mandatory" MaxLength="200"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtRemarksAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="200"
                                        ToolTip="Enter Remarks"></asp:TextBox>
                                </FooterTemplate>
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
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="SAVE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="CANCEL" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                        CommandName="ADD" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdAdd"
                                        ToolTip="Add New"></asp:ImageButton>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="divPage" style="position: relative;">
                    <table width="100%" border="0" class="datagrid_pagestyle">
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
                        <eluc:Status runat="server" ID="ucStatus" />
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
