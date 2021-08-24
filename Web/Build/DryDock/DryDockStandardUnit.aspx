<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DryDockStandardUnit.aspx.cs" Inherits="DryDockStandardUnit" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Unit" Src="~/UserControls/UserControlUnit.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Responsibilty" Src="~/UserControls/UserControlDiscipline.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
         <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
         <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvStandardUnitsLineItem.ClientID %>"));
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
    <form id="frmStandardUnit" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuHeader" runat="server" TabStrip="true" OnTabStripCommand="MenuHeader_TabStripCommand"></eluc:TabStrip>
        <eluc:TabStrip ID="MenuStandardUnitSpecification" runat="server" OnTabStripCommand="StandardUnitSpecification_TabStripCommand"></eluc:TabStrip>
       <telerik:RadAjaxPanel runat="server" ID="radajaxpanel1">
         <table width="70%" cellpadding="1" cellspacing="3">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtNumber" CssClass="input_mandatory" MaxLength="10" Width="60%"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblTitle" runat="server" Text="Title"></telerik:RadLabel>

                </td>
                <td>
                    <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtTitle" CssClass="input_mandatory" MaxLength="100"
                        Width="60%">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr valign="top">
                <td>
                    <telerik:RadLabel ID="lblDescription" runat="server" Text="Description"></telerik:RadLabel>

                </td>
                <td>
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtJobDescription" runat="server" CssClass="input_mandatory" Width="60%"
                        TextMode="MultiLine" Rows="6">
                    </telerik:RadTextBox>
                </td>
            </tr>
        </table>
        <hr />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvStandardUnitsLineItem" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadGrid RenderMode="Lightweight" ID="gvStandardUnitsLineItem" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" ShowFooter="true"
            OnNeedDataSource="gvStandardUnitsLineItem_NeedDataSource"
            OnUpdateCommand="gvStandardUnitsLineItem_UpdateCommand"
            OnItemDataBound="gvStandardUnitsLineItem_ItemDataBound"
            OnInsertCommand="gvStandardUnitsLineItem_InsertCommand"
            OnPreRender="gvStandardUnitsLineItem_PreRender"
            OnItemCommand="gvStandardUnitsLineItem_ItemCommand"
            AutoGenerateColumns="false" EnableHeaderContextMenu="true">
            <%--OnInsertCommand="gvStandardUnitsLineItem_InsertCommand"--%>
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDJOBDETAILID" TableLayout="Fixed">
                <NoRecordsTemplate>
                    <table width="100%" border="0">
                        <tr>
                            <td align="center">
                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </NoRecordsTemplate>
                <HeaderStyle Width="102px" />
                <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                <Columns>
                    <telerik:GridEditCommandColumn>
                        <HeaderStyle Width="10%" />
                    </telerik:GridEditCommandColumn>

                    <telerik:GridTemplateColumn HeaderText="Description">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderStyle Width="40%" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lbljobdetailid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBDETAILID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblDetail" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadLabel ID="lbljobdetailidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBDETAILID") %>'></telerik:RadLabel>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtDetailEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'
                                CssClass="gridinput_mandatory" ToolTip="Job Detail" Width="100%">
                            </telerik:RadTextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <telerik:RadTextBox ID="txtDetailAdd" runat="server" CssClass="gridinput_mandatory" Width="100%" ToolTip="Job Detail"></telerik:RadTextBox>
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Unit">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderStyle Width="30%" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblUnit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Unit ID="ucUnitEdit" Width="100%" runat="server" AppendDataBoundItems="true" UnitList='<%# PhoenixRegistersUnit.ListUnit()%>'
                                SelectedUnit='<%# DataBinder.Eval(Container,"DataItem.FLDUNIT") %>' />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <eluc:Unit ID="ucUnitAdd" runat="server" AppendDataBoundItems="true" UnitList='<%# PhoenixRegistersUnit.ListUnit()%>' Width="100%" />
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn>
                        <HeaderStyle Width="5%" />
                        <FooterStyle HorizontalAlign="Center" />
                        <FooterTemplate>
                            <asp:LinkButton ID="cmdadd" AlternateText="Add" CommandName="ADD" ToolTip="Add" runat="server"><span class="icon"><i class="fas fa-plus-circle"></i></span></asp:LinkButton>
                        </FooterTemplate>

                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" ScrollHeight="" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="2" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        <eluc:Status ID="ucStatus" runat="server" />
           </telerik:RadAjaxPanel>
    </form>
</body>
</html>
