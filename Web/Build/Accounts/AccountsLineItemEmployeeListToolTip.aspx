<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsLineItemEmployeeListToolTip.aspx.cs" Inherits="Accounts_AccountsLineItemEmployeeListToolTip" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonToolTip" Src="~/UserControls/UserControlCommonToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Allotment PV LineItem Details</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />
    <form id="frmLineItemDetails" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server"></telerik:RadSkinManager>
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="99%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <%-- <asp:Button ID="ucConfirmMessage" runat="server" OnConfirmMesage="OnAction_Click" CssClass="hidden" />--%>

            <eluc:Title runat="server" ID="Title1" Text="Allotment PV LineItem Details" Visible="false"></eluc:Title>
           <%-- <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" Visible="false" />--%>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvPVEmployeeDetail" runat="server" AllowCustomPaging="false" AllowSorting="false" AllowPaging="false"
                CellSpacing="0" GridLines="None" ShowFooter="false" ShowHeader="true" EnableViewState="true" OnNeedDataSource="gvPVEmployeeDetail_NeedDataSource">
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-CssClass="center">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Request No.">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRequestNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Allotment Type">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAllotmentType" runat="server" ForeColor="Maroon" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Month/Year">
                            <HeaderStyle Width="6.5%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMonth1" ForeColor="Maroon" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMONTH") %>'></telerik:RadLabel>

                                <telerik:RadLabel ID="lblYear1" ForeColor="Maroon" runat="server" Text='<%# " / "+ DataBinder.Eval(Container,"DataItem.FLDYEAR").ToString() %>'></telerik:RadLabel>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAmout" runat="server" ForeColor="Maroon" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" AllowExpandCollapse="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
