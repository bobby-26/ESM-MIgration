<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OptionModuleVesselConfiguration.aspx.cs" Inherits="OptionModuleVesselConfiguration" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleet.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiAddress" Src="~/UserControls/UserControlMultiColumnAddress.ascx" %>


<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Module Vessel Configuration</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <style type="text/css">
            .divFloatLeft {
                height: 53px;
            }
        </style>

        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvVesselList.ClientID %>"));
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
        <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />

        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />

            <table id="tblVesselNameSearch" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblApplication" runat="server" Text="Application"></telerik:RadLabel>
                    </td>
                    <td>

                        <telerik:RadComboBox runat="server" ID="ddlAdministratorMenuList" Filter="Contains" AutoPostBack="true" Width="180px" OnTextChanged="ddlAdministratorMenuList_TextChanged" AppendDataBoundItems="true" DataTextField="FLDAPPLICATIONNAME" DataValueField="FLDAPPLICATIONCODE">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value="Dummy" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblFleet" runat="server" Text="Fleet"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Fleet runat="server" ID="ucTechFleet" Width="180px" CssClass="input" AppendDataBoundItems="true" OnTextChangedEvent="ucFleet_TextChanged" AutoPostBack="true" />
                    </td>
                    <td>
                    <telerik:RadLabel ID="lblManagementType" runat="server" Text="Management Type"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Hard runat="server" ID="ucManagementType" AppendDataBoundItems="true" AutoPostBack="true"
                        HardTypeCode="31"  Width="180px" OnTextChangedEvent="ucManagementType_TextChanged" />
                </td>
                
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselName" runat="server" Text="Vessel Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtvesselName" runat="server" Width="180px" MaxLength="200" CssClass="input" OnTextChanged="txtVesselName_TextChanged" AutoPostBack="true"></telerik:RadTextBox>
                    </td>
                    <td>
                    <telerik:RadLabel ID="lblowner" runat="server" Text="Owner"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:MultiAddress runat="server" ID="ucowner" AddressType='<%# ((int)PhoenixAddressType.PRINCIPAL).ToString() %>'
                        Width="180px" CssClass="input" OnTextChangedEvent="ucowner_TextChanged" AutoPostBack="true"/>
                </td>
                   <td>
                    <telerik:RadLabel ID="lbloutof" runat="server" Text="Out of Management"></telerik:RadLabel>
                </td>
                <td>
                   <telerik:RadCheckBox ID="chkoutofmanagement" runat="server" AutoPostBack="true" OnCheckedChanged="chk_changed"></telerik:RadCheckBox>
                 </td>

                </tr>
            </table>
            <eluc:TabStrip ID="MenuDataSynchronizer" runat="server" OnTabStripCommand="MenuDataSynchronizer_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvVesselList" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemDataBound="gvVesselList_ItemDataBound" OnItemCommand="gvVesselList_ItemCommand" OnNeedDataSource="gvVesselList_NeedDataSource"
                ShowFooter="false" ShowHeader="true" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false">
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

                        <telerik:GridTemplateColumn HeaderText="Vessel Code">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblvesselcode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Vessel Name">
                            <HeaderStyle Width="45%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkVesselName" runat="server" CommandName="SELECT"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:LinkButton>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Active Y/N">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblactive" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVE") %>'></telerik:RadLabel>

                                <telerik:RadCheckBox runat="server" ID="chkActiveYN" CommandName="UPDATEACTIVEYN" Checked='<%# DataBinder.Eval(Container, "DataItem.FLDACTIVEYN").ToString().Equals("1") ? true : false %>'
                                    AutoPostBack="true"  />
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="2" EnableColumnClientFreeze="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
