<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsBankDownloadMappedTemplates.aspx.cs" Inherits="Accounts_AccountsBankDownloadMappedTemplates" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Accounts</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmRegistersBank" runat="server" submitdisabledcontrols="true">
      <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel runat="server" ID="pnlBankEntry">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
                            <eluc:Title runat="server" ID="ucTitle" ShowMenu="false" Text="Bank Download Mapped Templates" />
                        <table>
                            <tr>
                                <td>Type</td>
                                <td>
                                    <telerik:RadComboBox ID="ddltype" runat="server" CssClass="dropdown_mandatory" OnSelectedIndexChanged="ddltype_SelectedIndexChanged" AutoPostBack="true">
                                        <Items>
                                        <telerik:RadComboBoxItem Text="--select--" Value="0"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="Remittance" Value="1"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="Allotment Remittance" Value="2"></telerik:RadComboBoxItem>
                                            </Items>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                        </table>
                        <eluc:TabStrip ID="MenuRegistersBank" runat="server" OnTabStripCommand="RegistersBank_TabStripCommand"></eluc:TabStrip>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvTemplate" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" OnItemCommand="gvTemplate_RowCommand" OnItemCreated="gvTemplate_ItemDataBound"
                             OnDeleteCommand="gvTemplate_RowDeleting" OnNeedDataSource="gvTemplate_NeedDataSource" AllowPaging="true" AllowCustomPaging="true"
                            OnRowCreated="gvTemplate_RowCreated" GroupingEnabled="false" EnableHeaderContextMenu="true"
                            ShowFooter="true" ShowHeader="true" EnableViewState="false" AllowSorting="true"
                            OnSelectedIndexChanging="gvTemplate_SelectedIndexChanging" OnUpdateCommand="gvTemplate_RowUpdating"
                            OnSortCommand="gvTemplate_Sorting">
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
                                <telerik:GridTemplateColumn HeaderText="Payment Mode">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lbltemplateMapId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTEMPLATEMAPPINGID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblpaymentmode" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDHARDNAME")) %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadLabel ID="lbltemplateMapIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTEMPLATEMAPPINGID") %>'></telerik:RadLabel>
                                        <eluc:Hard ID="ucpaymentmodeEdit" runat="server" AppendDataBoundItems="true"
                                            CssClass="dropdown_mandatory"
                                            HardTypeCode="132" SelectedHard='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTMODE") %>' />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:Hard ID="ucpaymentmodeAdd" runat="server" AppendDataBoundItems="true"
                                            CssClass="dropdown_mandatory" HardTypeCode="132" />
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Template Name">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                  <ItemTemplate>
                                        <telerik:RadLabel ID="lbltemplate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTEMPLATECODE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadComboBox ID="ddltemplateedit" runat="server" CssClass="input">
                                        </telerik:RadComboBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadComboBox ID="ddltemplateadd" runat="server" CssClass="input">
                                        </telerik:RadComboBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Action">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                   <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                            CommandName="EDIT" CommandArgument='<%# Container.DataItem %>' ID="cmdEdit"
                                            ToolTip="Edit"></asp:ImageButton>
                                        <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="DELETE" CommandArgument='<%# Container.DataItem %>' ID="cmdDelete"
                                            ToolTip="Delete"></asp:ImageButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                            CommandName="UPDATE" CommandArgument='<%# Container.DataItem %>' ID="cmdSave"
                                            ToolTip="Save"></asp:ImageButton>
                                        <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="Cancel" CommandArgument='<%# Container.DataItem %>' ID="cmdCancel"
                                            ToolTip="Cancel"></asp:ImageButton>
                                    </EditItemTemplate>
                                    <FooterStyle HorizontalAlign="Center" />
                                    <FooterTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                            CommandName="ADD" CommandArgument='<%# Container.DataItem %>' ID="cmdAdd"
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
      
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
