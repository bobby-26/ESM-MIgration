<%@ Page Language="C#" AutoEventWireup="True" CodeFile="AccountsOwnerChargingarrangements.aspx.cs"
    Inherits="AccountsOwnerChargingarrangements" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStripTelerik" Src="~/UserControls/UserControlTabsTelerik.ascx" %><%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCompany" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="../UserControls/UserControlDecimal.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <style type="text/css">
        .style1
        {
            width: 119px;
        }
    </style>
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>


   
</telerik:RadCodeBlock>
</head>
<body>
    <form id="frmAccountsFinancialYearSetup" runat="server">
     <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" Height="98%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
           
                    
              
                    <eluc:TabStrip ID="MenuOwner" runat="server" OnTabStripCommand="MenuOwner_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
              
                    <table id="tblFinancialYearSetup" width="100%">
                        <tr>
                            <td colspan="2">
                                <telerik:RadLabel ID="lblIfInvoiceCreditNoteDiscountReturnisNOVesselSupplierDiscountwillapply" runat="server" Text="If Invoice Credit Note Discount Return is NO, Vessel Supplier Discount will apply"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <telerik:RadLabel ID="lblOwnerCode" runat="server" Text="Owner Code"></telerik:RadLabel>
                            </td>
                            <td>
                                &nbsp;
                                <telerik:RadTextBox ID="txtCode" runat="server" CssClass="input" MaxLength="200"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtName" runat="server" MaxLength="200" CssClass="input"></telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
             
               
                 <%--   <eluc:TabStrip ID="MenuPeriodLock" runat="server" OnTabStripCommand="FinancialYearSetup_TabStripCommand">
                    </eluc:TabStrip>--%>
             
               
                    <eluc:TabStrip ID="MenuFinancialYearSetup" runat="server" OnTabStripCommand="FinancialYearSetup_TabStripCommand">
                    </eluc:TabStrip>

            <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator2" runat="server" DecorationZoneID="dgOwnerchargearrangements" DecoratedControls="All" EnableRoundedCorners="true" />
                <telerik:RadGrid RenderMode="Lightweight" ID="dgOwnerchargearrangements" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" OnNeedDataSource="dgOwnerchargearrangements_NeedDataSource" AllowMultiRowSelection="true" FilterType="CheckList"
                    EnableViewState="false" EnableHeaderContextMenu="true" Width="100%" GroupingEnabled="false" Height="80%" OnSelectedIndexChanging="dgOwnerchargearrangements_SelectedIndexChanging"
                    OnItemDataBound="dgOwnerchargearrangements_ItemDataBound" OnItemCommand="dgOwnerchargearrangements_ItemCommand"
                    ShowFooter="false" ShowHeader="true" OnSortCommand="dgOwnerchargearrangements_SortCommand">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDADDRESSCODE">

                          <Columns>
                           <%-- <asp:ButtonField CommandName="Edit" Text="DoubleClick" Visible="false" />--%>
                            <telerik:GridTemplateColumn HeaderText="Code" AllowSorting="true" SortExpression="FLDCODE">
                                
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblFinancialStartYear" runat="server" CommandArgument="<%# Container.DataSetIndex %>"
                                        CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCODE") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblPrincipalId" runat="server"  CommandName="Select"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDRESSCODE") %>'
                                        Visible="false"></telerik:RadLabel>
                                    
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="true" SortExpression="FLDADDRESSCODE">
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>' Visible="false"></telerik:RadLabel>
                                    <asp:LinkButton ID="lnkFinancialEndYear" runat="server" CommandArgument="<%# Container.DataSetIndex %>"
                                        CommandName="Select" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Airfare Markup %" AllowSorting="true" SortExpression="FLDFINANCIALYEAR">
                                
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lnkFinancialYear" runat="server" CommandArgument="<%# Container.DataSetIndex %>"
                                        CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMARKUPAIRFARE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                             
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="GST Chargeable">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblActiveYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDTAXBASIS").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkActiveYNEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDTAXBASIS").ToString().Equals("1"))?true:false %>' />
                                    <telerik:RadLabel ID="lblOwnerId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDRESSCODE") %>'
                                        Visible="false"></telerik:RadLabel>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Invoice Credit Note Discount Return">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCreditnotediscountreturnYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDISCREDITNOTEDISCOUNTRETURN").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkCreditnotediscountreturnYNEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDISCREDITNOTEDISCOUNTRETURN").ToString().Equals("1"))?true:false %>' />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                               
                                <ItemStyle HorizontalAlign="Center"  Wrap="False" />
                                <ItemTemplate>
                                    <img runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton ID="cmdEdit" runat="server" AlternateText="Edit" CommandArgument="<%# Container.DataSetIndex %>"
                                        CommandName="EDIT" ImageUrl="<%$ PhoenixTheme:images/te_edit.png%>" ToolTip="Edit" />
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton ID="cmdDelete" runat="server" AlternateText="Delete" CommandArgument="<%# Container.DataSetIndex %>"
                                        CommandName="DELETE" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>" ToolTip="Delete"
                                        Visible="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <img runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton ID="cmdSave" runat="server" AlternateText="Save" CommandArgument="<%# Container.DataSetIndex %>"
                                        CommandName="Save" ImageUrl="<%$ PhoenixTheme:images/save.png%>" ToolTip="Save" />
                                    <img runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                     <NoRecordsTemplate>
                            <table width="100%" border="0">
                                <tr>
                                    <td align="center">
                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                    </td>
                                </tr>
                            </table>
                        </NoRecordsTemplate>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                            PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                        EnablePostBackOnRowClick="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="2" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </telerik:RadAjaxPanel>
    </form>
</body>
</html>
                
       