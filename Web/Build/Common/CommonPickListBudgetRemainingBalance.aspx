<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonPickListBudgetRemainingBalance.aspx.cs"
    Inherits="CommonPickListBudgetRemainingBalance" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Budget</title>
    <telerik:RadCodeBlock runat="server" ID="RadCodeBlock">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    <script type="text/javascript">
    function confirmCallbackFn(arg)
    {
        if (arg) //the user clicked OK
        {
            __doPostBack("<%=hdnCallback.UniqueID %>", "");
        }
    }
</script>
</telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No"></telerik:RadWindowManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="MenuBudget">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuBudget" />
                        <telerik:AjaxUpdatedControl ControlID="rgvBudget" UpdatePanelHeight="75%" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                        <telerik:AjaxUpdatedControl ControlID="ucConfirm" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rgvBudget">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rgvBudget" UpdatePanelHeight="75%" />
                        <telerik:AjaxUpdatedControl ControlID="ucConfirm" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ucBudgetGroup">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rgvBudget" UpdatePanelHeight="75%" />
                        <telerik:AjaxUpdatedControl ControlID="ucBudgetGroup" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel" runat="server" RenderMode="Lightweight"></telerik:RadAjaxLoadingPanel>
    
        
        <eluc:TabStrip ID="MenuBudget" runat="server" OnTabStripCommand="MenuBudget_TabStripCommand">
        </eluc:TabStrip>
    <br clear="all" />
        <asp:Button ID="hdnCallback" runat="server" CssClass="hidden" OnClick="CloseWindow" /> 
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Confirm ID="ucConfirm" runat="server" Visible="false"  OnConfirmMesage="CloseWindow"  />
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblBudgetCode" runat="server" Text="Budget Code"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtBudgetSearch" runat="server" Text=""></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblDescription" runat="server" Text="Description"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtDescriptionNameSearch" runat="server" CssClass="input" Text=""></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblBudgetGroup" runat="server" Text="Budget Group"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Hard ID="ucBudgetGroup" AppendDataBoundItems="true" runat="server" AutoPostBack="true" OnTextChangedEvent="ucBudgetGroup_TextChangedEvent"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblBudgetAmount" runat="server" Text="Budget Amount"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtBudgetAmount" runat="server" ReadOnly="true"
                                Text=""></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblCommittedAmount" runat="server" Text="Committed Amount"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtCommittedAmount" runat="server" ReadOnly="true"
                                Text=""></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblPaidAmount" runat="server" Text="Charged Amount"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtPaidAmount" runat="server" ReadOnly="true" Text=""></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblVariance" runat="server" Text="Variance"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtVariance" ReadOnly="true" Text=""></telerik:RadTextBox>
                        </td>
                        <td>
                           <telerik:RadLabel RenderMode="Lightweight" ID="lblApprovedNotOrdered" runat="server" Text="Approved Not Ordered"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtApprovedNotOrdered" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                </table>
                <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="rgvBudget" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" Height="100%"
                    CellSpacing="0" GridLines="None" OnNeedDataSource="rgvBudget_NeedDataSource" OnItemCommand="rgvBudget_ItemCommand">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDBUDGETID" >
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Code" UniqueName="CODE">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="lblBudgetId" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDBUDGETID") %>'
                                        Visible="false"></telerik:RadLabel>
                                    <asp:LinkButton ID="lnkBudget" runat="server" CommandArgument='<%# Container.DataItem %>' CommandName="SELECT"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="DESCRIPTION" HeaderText="Description">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="lblDescription" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Group" UniqueName="GROUP">
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="lblBudgetGroupId" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDBUDGETGROUPID") %>'
                                        Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="lblBudgetGroup" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDBUDGETGROUP") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox"/>
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
