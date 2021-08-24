<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementDocumentSectionReadUnreadList.aspx.cs" Inherits="DocumentManagementDocumentSectionReadUnreadList" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Import Namespace="SouthNests.Phoenix.DocumentManagement" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvUsers.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
                fade('statusmessage');
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table width="99.9%">
                <tr>
                    <td Width="30%">
                        <telerik:RadRadioButtonList ID="cbOfficeVessel" runat="server" Columns="3" Direction="Vertical" Font-Bold="true" OnSelectedIndexChanged="cbOfficeVessel_SelectedIndexChanged" AutoPostBack="true">
                            <Items>
                                <telerik:ButtonListItem Text="Vessel" Value="0" Selected="True" />
                                <telerik:ButtonListItem Text="Office" Value="1" />
                            </Items>
                            </telerik:RadRadioButtonList>
                    </td>
                    <td  Width="10%">
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                        <telerik:RadLabel ID="lblDepartment" runat="server" Text="Department"></telerik:RadLabel>
                    </td>
                    <td  Width="60%">
                        <telerik:RadComboBox ID="ucVessel" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                                    Width="180px" DataTextField="FLDVESSELNAME" DataValueField="FLDVESSELID" OnTextChangedEvent="ucVessel_TextChangedEvent"></telerik:RadComboBox>
                        <%--<eluc:Vessel ID="ucVessel" runat="server" ActiveVesselsOnly="true" VesselsOnly="true" SyncActiveVesselsOnly="true" Entitytype="VSL" AppendDataBoundItems="true" AutoPostBack="true"
                                CssClass="input" OnTextChangedEvent="ucVessel_TextChangedEvent" Width="180px" />--%>
                        <telerik:RadComboBox ID="ddlDepartment" runat="server" AutoPostBack="true" EmptyMessage="Type to  select Department" OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged"
                           DataTextField="FLDDEPARTMENTNAME" DataValueField="FLDDEPARTMENTID" Filter="Contains" MarkFirstMatch="true" Width="180px"></telerik:RadComboBox>
                    </td>

                </tr>
                <tr>
                    <td colspan="3">
                         <hr style="height: -12px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3" >
                        <telerik:RadRadioButtonList ID="cbReadUnread" runat="server" Columns="3" Direction="Vertical" Font-Bold="true" OnSelectedIndexChanged="cbReadUnread_SelectedIndexChanged" AutoPostBack="true">
                            <Items>
                                <telerik:ButtonListItem Text="Unread" Value="0" Selected="True" />
                                <telerik:ButtonListItem Text="Read" Value="1" />
                                <telerik:ButtonListItem Text="Read All" Value="2" />
                            </Items>
                            </telerik:RadRadioButtonList>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuReadUnread" runat="server" OnTabStripCommand="MenuReadUnread_TabStripCommand" TabStrip="true" />
            <telerik:RadGrid ID="gvUsers" runat="server" RenderMode="Lightweight" AllowPaging="true" AllowCustomPaging="true"
                OnNeedDataSource="gvUsers_NeedDataSource" OnItemCommand="gvUsers_ItemCommand" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDEMPLOYEEID">
                    <NoRecordsTemplate>
                        <table width="99.9%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Code" HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEECODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="25%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEmployeeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="Department" HeaderStyle-Width="25%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDepartment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Group Rank / Designation" HeaderStyle-Width="25%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblGroupRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUPRANK") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>  
                        <telerik:GridTemplateColumn HeaderText="Read Date" HeaderStyle-Width="15%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRead" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREADDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>                        
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
