<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsPhoneCardRequisition.aspx.cs"
    Inherits="AccountsPhoneCardRequisition" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BudgetCode" Src="~/UserControls/UserControlBudgetCode.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Phone Card Requisition</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div runat="server" id="DivHeader">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmPhoneCardRequisition" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts ="false" >
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlPhoneCardEntry">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="div1">
                        <eluc:Title runat="server" ID="ucTitle" Text="Phone Card Requisition" />
                        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                    </div>
                </div>
                <div style="position: relative; overflow: hidden; clear: right;">
                    <iframe runat="server" id="ifMoreInfo" style="min-height: 275px; width: 100%; overflow-x: hidden">
                    </iframe>
                </div>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuOffice" runat="server" OnTabStripCommand="MenuOffice_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative;">
                    <asp:GridView ID="gvPhoneCards" runat="server" AutoGenerateColumns="False" CellPadding="3" OnRowUpdating="gvPhoneCards_RowUpdating"
                        Font-Size="11px" OnRowDataBound="gvPhoneCards_RowDataBound" OnRowCommand="gvPhoneCards_RowCommand"
                        OnRowEditing="gvPhoneCards_RowEditing" OnRowCancelingEdit="gvPhoneCards_RowCancelingEdit" 
                        AllowSorting="true" OnSorting="gvPhoneCards_Sorting" EnableViewState="false"
                        ShowFooter="false" ShowHeader="true" Width="100%">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:ButtonField CommandName="Edit" Text="DoubleClick" Visible="false" />
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblRequisitionNumber" runat="server" Text="Requisition Number"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lblRequisitionNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENO")%>' CommandName="SELECT" CommandArgument="<%# Container.DataItemIndex %>"></asp:LinkButton>
                                    <asp:Label ID="lblRequestid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTID")%>'></asp:Label>                                    
                                    <asp:Label ID="lblOrderid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblOrderDate" runat="server" Text="Order Date"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# string.Format("{0:dd/MMM/yyyy}", DataBinder.Eval(Container, "DataItem.FLDREQUESTDATE"))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblSentDate" runat="server" Text="Sent Date"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSentDate" runat="server" Text='<%# string.Format("{0:dd/MMM/yyyy}", DataBinder.Eval(Container, "DataItem.FLDSENTDATE")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblSupplier" runat="server" Text="Supplier"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSupplierName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPPLIER")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <span id="spnPickListVendor">
                                        <asp:TextBox ID="txtVendorCode" runat="server" ReadOnly="false" CssClass="input_mandatory" 
                                            MaxLength="15" Width="90px"></asp:TextBox>
                                        <asp:TextBox ID="txtVendorName" runat="server" ReadOnly="false" CssClass="input_mandatory" 
                                            Width="200px"></asp:TextBox>
                                        <img runat="server" id="ImgShowMakerVendor" style="cursor: pointer; vertical-align: top"
                                            src="<%$ PhoenixTheme:images/picklist.png %>" onclick="return showPickList('spnPickListVendor', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=131&ignoreiframe=true', true); " />
                                        <asp:TextBox ID="txtVendorId" runat="server" CssClass="input" Width="10px"></asp:TextBox>
                                    </span>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblBudgetCode" runat="server" Text="Budget Code"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblBudgetcode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETCODE")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:BudgetCode runat="server" ID="ucBudgetCodeEdit" AppendDataBoundItems="true"
                                        CssClass="input_mandatory" BudgetCodeList='<%# PhoenixRegistersBudget.ListBudget() %>'
                                        SelectedBudgetSubAccount='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETCODE") %>' />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblBilltoCompany" runat="server" Text="Bill to Company"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblBilltoCompany" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBILLTOCOMPANYNAME")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblCompanyId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDBILLTOCOMPANY") %>'></asp:Label>
                                    <eluc:Company ID="ddlCompany" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                                        CssClass="dropdown_mandatory" runat="server" SelectedCompany='<%# DataBinder.Eval(Container, "DataItem.FLDBILLTOCOMPANY") %>'
                                        AppendDataBoundItems="true" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblAction" runat="server" Text="Action"></asp:Literal>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl='<%$ PhoenixTheme:images/te_edit.png%>'
                                        CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Update" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
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
                                <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev"><< Prev </asp:LinkButton>
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
    </asp:UpdatePanel>
    </form>
</body>
</html>
