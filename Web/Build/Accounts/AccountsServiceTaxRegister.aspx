<%@ Page Language="C#" AutoEventWireup="True" CodeFile="AccountsServiceTaxRegister.aspx.cs" Inherits="AccountsServiceTaxRegister"
    MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlDecimal.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>State</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
          <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvSTaxRegister.ClientID %>"));
                }, 200);
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersState" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel runat="server" ID="pnlStateEntry">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
<%--            <eluc:Title runat="server" ID="ucTitle" Text="Service Tax Register" />--%>
            <eluc:TabStrip ID="MenuRegister" runat="server" OnTabStripCommand="MenuRegister_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvSTaxRegister" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnItemCommand="gvSTaxRegister_RowCommand" OnItemDataBound="gvSTaxRegister_ItemDataBound" OnNeedDataSource="gvSTaxRegister_NeedDataSource"
                OnRowCreated="gvSTaxRegister_RowCreated" AllowPaging="true" AllowCustomPaging="true" EnableHeaderContextMenu="true" GroupingEnabled="false"
                OnDeleteCommand="gvSTaxRegister_RowDeleting" OnUpdateCommand="gvSTaxRegister_RowUpdating" 
                ShowFooter="true" ShowHeader="true" OnSortCommand="gvSTaxRegister_Sorting" AllowSorting="true"
                EnableViewState="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" >
                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                <Columns>
                    <telerik:GridTemplateColumn FooterText="Type of Payment" HeaderText="Type of Payment">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                         <ItemTemplate>
                            <telerik:RadLabel ID="lblPaymentTypeItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERVICETAXPAYMENTTYPE") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblPaymentId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERVICETAXID") %>' Visible="false"></telerik:RadLabel>
                        </ItemTemplate>
                        <FooterTemplate>
                            <telerik:RadTextBox ID="txtPaymentType" runat="server" CssClass="gridinput_mandatory"></telerik:RadTextBox>
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Section Code">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                         <ItemTemplate>
                            <telerik:RadLabel ID="lblSectionCodeItem" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDSECTIONCODE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <FooterTemplate>
                            <telerik:RadTextBox ID="txtSectionCode" runat="server" CssClass="gridinput"></telerik:RadTextBox>
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Basic Rate">
                         <HeaderStyle  HorizontalAlign="Right"/>

                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <FooterStyle HorizontalAlign="Right" />
                       <ItemTemplate>
                            <telerik:RadLabel ID="lblBasicItem" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDBASICRATE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Number ID="ucBasicEdit" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDBASICRATE") %>' CssClass="gridinput" Mask="99.999" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <eluc:Number ID="ucBasicAdd" runat="server" CssClass="gridinput" Mask="99.999" />
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Primary EC">
                         <HeaderStyle  HorizontalAlign="Right"/>

                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <FooterStyle HorizontalAlign="Right" />
                          <ItemTemplate>
                            <telerik:RadLabel ID="lblPrimaryECItem" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDPRIMARYEC") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Number ID="ucPrimaryECEdit" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDPRIMARYEC") %>' CssClass="gridinput" Mask="99.999" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <eluc:Number ID="ucPrimaryECAdd" runat="server" CssClass="gridinput" Mask="99.999" />
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Secondary EC">
                         <HeaderStyle  HorizontalAlign="Right"/>

                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <FooterStyle HorizontalAlign="Right" />
                         <ItemTemplate>
                            <telerik:RadLabel ID="lblSecondaryECItem" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDSECONDARYEC") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Number ID="ucSecondaryECEdit" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDSECONDARYEC") %>' CssClass="gridinput" Mask="99.999" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <eluc:Number ID="ucSecondaryECAdd" runat="server" CssClass="gridinput" Mask="99.999" />
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Service Tax Rate">
                         <HeaderStyle  HorizontalAlign="Right"/>

                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <FooterStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblServiceTax" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDSERVICETAX") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                       <ItemStyle Wrap="False" HorizontalAlign="Center" ></ItemStyle>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                CommandName="EDIT" CommandArgument="<%# Container.DataItem %>" ID="cmdEdit"
                                ToolTip="Edit"></asp:ImageButton>
                            <img runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                            <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                CommandName="DELETE" CommandArgument="<%# Container.DataItem %>" ID="cmdDelete"
                                ToolTip="Delete"></asp:ImageButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                CommandName="Save" CommandArgument="<%# Container.DataItem %>" ID="cmdSave"
                                ToolTip="Save"></asp:ImageButton>
                            <img runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                            <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                CommandName="Cancel" CommandArgument="<%# Container.DataItem %>" ID="cmdCancel"
                                ToolTip="Cancel"></asp:ImageButton>
                        </EditItemTemplate>
                        <FooterStyle HorizontalAlign="Center" />
                        <FooterTemplate>
                            <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                CommandName="Add" CommandArgument="<%# Container.DataItem %>" ID="cmdAdd"
                                ToolTip="Add New"></asp:ImageButton>
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                    </MasterTableView>
                  <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
                   <%--  <triggers>
                <asp:PostBackTrigger ControlID="MenuRegister" />
            </triggers>--%>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
