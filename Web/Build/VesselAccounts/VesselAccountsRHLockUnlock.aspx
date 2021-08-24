<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsRHLockUnlock.aspx.cs"
    Inherits="VesselAccountsRHLockUnlock" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>RH Lock/Unlock</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRHLockUnlock" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuConfigTabStrip" TabStrip="true" runat="server" OnTabStripCommand="ConfigTap_TabStripCommand"></eluc:TabStrip>
            <table width="100%" cellpadding="2" cellspacing="2">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                        &nbsp;&nbsp;&nbsp
                                <telerik:RadTextBox ID="txtVesselName" runat="server" CssClass="input" Width="200px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLock" runat="server" Text="Locked (Y/N)"></telerik:RadLabel>
                        &nbsp;&nbsp;&nbsp
                                <telerik:RadComboBox ID="ddlLockYn" runat="server" CssClass="input_mandatory" Filter="Contains">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="--Select--" Value="Dummy" Selected="True" />
                                        <telerik:RadComboBoxItem Text="Yes" Value="1" />
                                        <telerik:RadComboBoxItem Text="No" Value="0" />
                                    </Items>
                                </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSignonAllow" runat="server" Text="Sign-On Enabled (Y/N)"></telerik:RadLabel>
                        &nbsp;&nbsp;&nbsp
                                <telerik:RadComboBox ID="ddlManualsignon" runat="server" CssClass="input_mandatory" Filter="Contains">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="--Select" Value="Dummy" Selected="True" />
                                        <telerik:RadComboBoxItem Text="Yes" Value="1" />
                                        <telerik:RadComboBoxItem Text="No" Value="0" />
                                    </Items>
                                </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <h1></h1>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuRHLockUnlock" runat="server" OnTabStripCommand="RHLockUnlock_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvRHLockUnlock" runat="server" AutoGenerateColumns="False" Font-Size="11px" GridLines="None" Width="100%" Height="78%"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" EnableHeaderContextMenu="true" GroupingEnabled="false" OnUpdateCommand="gvRHLockUnlock_UpdateCommand"
                OnItemCommand="gvRHLockUnlock_ItemCommand" OnItemDataBound="gvRHLockUnlock_ItemDataBound" OnNeedDataSource="gvRHLockUnlock_NeedDataSource"
                ShowHeader="true" EnableViewState="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
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
                        <telerik:GridTemplateColumn HeaderText="ID">
                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblLockUnlockId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCKUNLOCKID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel">
                            <HeaderStyle HorizontalAlign="Left" Width="20%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Locked (Y/N)">
                            <HeaderStyle HorizontalAlign="Left" Width="20%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLockYN" runat="server" Enabled="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCKEDYNNAME") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkLockYNEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDLOCKEDYN").ToString().Equals("1"))?true:false %>'></telerik:RadCheckBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Manual Sign-On Enabled(Y/N)">
                            <HeaderStyle HorizontalAlign="Left" Width="20%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAllowYN" runat="server" Enabled="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALLOW") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkAllowYNEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDALLOWYN").ToString().Equals("1"))?true:false %>'></telerik:RadCheckBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
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
