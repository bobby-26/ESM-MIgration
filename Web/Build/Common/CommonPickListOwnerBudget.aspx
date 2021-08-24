<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonPickListOwnerBudget.aspx.cs"
    Inherits="CommonPickListOwnerBudget" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="OwnerBudgetGroup" Src="~/UserControls/UserControlOwnerBudgetGroup.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Owner Budget</title>
    <telerik:RadCodeBlock ID="RadCodeBlock" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="MenuBudget">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuBudget" />
                        <telerik:AjaxUpdatedControl ControlID="rgvBudget" UpdatePanelHeight="85%" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                        <telerik:AjaxUpdatedControl ControlID="ucConfirm" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rgvBudget">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rgvBudget" UpdatePanelHeight="85%" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ucOwnerBudgetGroup">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rgvBudget" UpdatePanelHeight="85%" />
                        <telerik:AjaxUpdatedControl ControlID="ucOwnerBudgetGroup" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel" runat="server" RenderMode="Lightweight"></telerik:RadAjaxLoadingPanel>

    <div style="font-weight:600;font-size:12px" runat="server">
    <eluc:TabStrip ID="MenuBudget" runat="server" OnTabStripCommand="MenuBudget_TabStripCommand">
    </eluc:TabStrip>
        </div>
    <eluc:Confirm ID="ucConfirm" runat="server" Visible="false" OnConfirmMesage="CloseWindow" />
    
    <br clear="all" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table id="tblsearch" width="100%" cellpadding="1px" cellspacing="1px">
                <tr>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblOwnerBudgetCode" runat="server" Text="Budget Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtOwenrBudgetCode" runat="server"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblOwnerBudgetGroup" runat="server" Text="Budget Group"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:OwnerBudgetGroup runat="server" ID="ucOwnerBudgetGroup" AppendDataBoundItems="true" 
                            AutoPostBack="true" />
                    </td>
                    <td>
                        <telerik:RadCheckBox RenderMode="Lightweight" ID="chkShowAll" runat="server" Text="Show All" AutoPostBack="true"
                            ToolTip="Show All the Budget Code for the Owner" OnCheckedChanged="chkShowAll_CheckedChanged"
                            Visible="False" />
                    </td>
                </tr>
            </table>
        <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="rgvBudget" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" Height="100%"
                    CellSpacing="0" GridLines="None" OnNeedDataSource="rgvBudget_NeedDataSource" OnItemCommand="rgvBudget_ItemCommand">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDOWNERBUDGETCODEMAPID" >
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Code" UniqueName="CODE">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="lblBudgetId" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETCODEMAPID") %>'
                                        Visible="false"></telerik:RadLabel>
                                    <asp:LinkButton ID="lnkBudget" runat="server" CommandArgument='<%# Container.DataItem %>' CommandName="SELECT"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETCODE") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="DESCRIPTION" HeaderText="Description">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="lblDescription" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDOWNERCODEDESCRIPTION") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Group" UniqueName="GROUP">
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="lblBudgetGroupId" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETGROUPID") %>'
                                        Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="lblBudgetGroup" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETGROUP") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        <%--<ClientEvents OnGridCreated="GridCreated" />--%>
                    </ClientSettings>
                </telerik:RadGrid>
    </form>
</body>
</html>
