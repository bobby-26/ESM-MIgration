<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewPlanDetailTravel.aspx.cs" Inherits="CrewPlanDetailTravel" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="../UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MUCCity" Src="~/UserControls/UserControlMultiColumnCity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TravelReason" Src="~/UserControls/UserControlTravelReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonToolTip" Src="~/UserControls/UserControlCommonToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Event On-Signer Travel</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvCCPlan.ClientID %>"));
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
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager runat="server" ID="RadSkinManager1"></telerik:RadSkinManager>
        <telerik:RadWindowManager runat="server" ID="RadWindowManager2" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxManager runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gvCCPlan">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvCCPlan" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1"></telerik:RadAjaxLoadingPanel>

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />        
        <eluc:TabStrip ID="CrewTab" runat="server" OnTabStripCommand="CrewTab_TabStripCommand"></eluc:TabStrip>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvCCPlan" runat="server" AllowCustomPaging="true" AllowSorting="true" OnItemDataBound="gvCCPlan_ItemDataBound"
            CellSpacing="0" GridLines="None" OnItemCommand="gvCCPlan_ItemCommand" OnNeedDataSource="gvCCPlan_NeedDataSource" 
            EnableViewState="true" GroupingEnabled="false" EnableHeaderContextMenu="true" AllowPaging="true" AllowMultiRowEdit="true">
            <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" DataKeyNames="FLDCREWEVENTDETAILID" HeaderStyle-Font-Bold="true"
                ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" TableLayout="Fixed">
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
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="OnSigner" UniqueName="ONSIGNER">
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblcreweventdetailid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWEVENTDETAILID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblOnSignerId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONSIGNERID") %>'
                                Visible="false">
                            </telerik:RadLabel>
                            <telerik:RadLabel ID="lnkOffSigner" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONSIGNERNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadLabel ID="lblcreweventdetailidedit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWEVENTDETAILID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblOnSignerIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONSIGNERID") %>'
                                Visible="false">
                            </telerik:RadLabel>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Origin" UniqueName="FLDORIGINID" DataField="FLDSEAFARERAIRPORTCITYID">
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                        <ItemTemplate>
                            <eluc:MUCCity ID="ucOriginCity" runat="server" CssClass="dropdown_mandatory" Width="100%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAFARERAIRPORTCITYNAME") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Destination" UniqueName="FLDDESTINATIONCITYID" DataField="FLDDESTINATIONCITYID">
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                        <ItemTemplate>
                            <eluc:MUCCity ID="ucDestinationCity" runat="server" CssClass="dropdown_mandatory" Width="100%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONCITYNAME") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Departure Date" UniqueName="FLDFROMDATE" DataField="FLDFROMDATE">
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                        <ItemTemplate>
                            <eluc:Date runat="server" ID="txtDepartureDateEdit" CssClass="input_mandatory" Width="100%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFROMDATE") %>'></eluc:Date>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="" HeaderStyle-Width="8%">
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                        <ItemTemplate>
                            <telerik:RadComboBox ID="ddlampmdeparture" runat="server" CssClass="dropdown_mandatory" Width="100%"
                                Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True" EmptyMessage="Type to select">
                                <Items>
                                    <telerik:RadComboBoxItem Text="AM" Value="1" Selected="true" />
                                    <telerik:RadComboBoxItem Text="PM" Value="2" />
                                </Items>
                            </telerik:RadComboBox>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Arrival Date" UniqueName="FLDTODATE" DataField="FLDTODATE">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <ItemTemplate>
                            <eluc:Date runat="server" ID="txtArrivalDate" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTODATE") %>'
                                Width="100%"></eluc:Date>

                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="" HeaderStyle-Width="8%">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <ItemTemplate>
                            <telerik:RadComboBox ID="ddlampmarrival" runat="server" CssClass="input" Width="100%"
                                EnableLoadOnDemand="True" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                <Items>
                                    <telerik:RadComboBoxItem Text="AM" Value="1" />
                                    <telerik:RadComboBoxItem Text="PM" Value="2" />
                                </Items>
                            </telerik:RadComboBox>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Payment Mode" UniqueName="FLDPAYMENTMODE" DataField="FLDPAYMENTMODEID" HeaderStyle-Width="12%">
                        <ItemStyle Wrap="false" HorizontalAlign="Left"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <ItemTemplate>
                            <eluc:Hard ID="ucPaymentmode" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" SelectedHard='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTMODEID") %>'
                                HardList="<%# PhoenixRegistersHard.ListHard(1,185) %>" HardTypeCode="185" Width="100%" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Purpose" UniqueName="FLDPURPOSE" DataField="FLDPURPOSE">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <HeaderTemplate>
                            <telerik:RadLabel ID="lblPurposeHeader" runat="server">Purpose</telerik:RadLabel>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <eluc:TravelReason ID="ucPurpose" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" SelectedValue='<%# DataBinder.Eval(Container,"DataItem.FLDPURPOSE") %>'
                                ReasonList="<%# PhoenixCrewTravelRequest.ListTravelReason(null) %>" Width="100%" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
        <eluc:Status ID="ucStatus" runat="server" />

    </form>
</body>
</html>
