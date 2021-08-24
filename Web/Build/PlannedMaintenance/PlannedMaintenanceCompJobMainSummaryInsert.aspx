<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceCompJobMainSummaryInsert.aspx.cs"
    Inherits="PlannedMaintenanceCompJobMainSummaryInsert" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiAddress" Src="~/UserControls/UserControlMultiColumnAddress.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Job Summary Add</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvGlobalComponent.ClientID %>"));
                }, 200);
            }

        </script>
    </telerik:RadCodeBlock>
</head>
<body onload="Resize();" onresize="Resize();">
    <form id="frmWorkOrder" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%" EnableAJAX="true">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text="" Visible="false"></eluc:Status>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:TabStrip ID="MenuWorkOrder" runat="server" OnTabStripCommand="MenuWorkOrder_TabStripCommand"></eluc:TabStrip>
            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTitle" runat="server" Text="Title"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtTitle" runat="server" Width="300px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblOwner" runat="server" Text="Principal"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MultiAddress runat="server" ID="ddlOwner" AddressType="<%# ((int)PhoenixAddressType.PRINCIPAL).ToString() %>"
                            Width="300px" AutoPostBack="true" OnTextChangedEvent="ddlOwner_TextChangedEvent" />
                    </td>
                    <td>
                        <asp:Literal ID="lblFleet" runat="server" Text="Fleet"></asp:Literal>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlFleet" runat="server" DataTextField="FLDFLEETDESCRIPTION" DataValueField="FLDFLEETID" AutoPostBack="true"
                            EmptyMessage="Type to select fleet" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                            OnItemChecked="ddlFleet_ItemChecked" Width="300px">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblType" runat="server" Text="Type" Visible="false"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rbBtnList" runat="server" Direction="Horizontal" Visible="false">
                            <Items>
                                <telerik:ButtonListItem Text="Only for User" Value="1" Selected="true" />
                                <telerik:ButtonListItem Text="Visible to Others" Value="2" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblVesselType" runat="server" Text="Vessel Type"></asp:Literal>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlVeselType" runat="server" DataTextField="FLDTYPEDESCRIPTION" DataValueField="FLDVESSELTYPEID" AutoPostBack="true"
                            EmptyMessage="Type to select vessel type" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                            OnItemChecked="ddlVeselType_ItemChecked" Width="300px">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlVessel" runat="server" DataTextField="FLDVESSELNAME" DataValueField="FLDVESSELID"
                            EmptyMessage="Type to select fleet" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="300px">
                        </telerik:RadComboBox>
                    </td>
                </tr>
            </table>
            <br />
            <eluc:TabStrip ID="menuGlobalComponent" runat="server" Title="Global Component" OnTabStripCommand="menuGlobalComponent_TabStripCommand"></eluc:TabStrip>
            <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvGlobalComponent" DecoratedControls="All" EnableRoundedCorners="true" />
            <telerik:RadGrid ID="gvGlobalComponent" runat="server" AllowPaging="true" AllowCustomPaging="true" OnNeedDataSource="gvGlobalComponent_NeedDataSource"
                OnItemCommand="gvGlobalComponent_ItemCommand" OnItemDataBound="gvGlobalComponent_ItemDataBound">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDGLOBALCOMPONENTSUMMARYID" ClientDataKeyNames="FLDGLOBALCOMPONENTSUMMARYID" AllowFilteringByColumn="true">
                    <HeaderStyle Width="102px" />
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Component" Name="COMPONENT" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Jobs" Name="JOBS" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>

                        <telerik:GridTemplateColumn HeaderText="Number" AllowFiltering="false" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDCOMPONENTNUMBER" ColumnGroupName="COMPONENT" UniqueName="COMPONENTNUMBER">
                            <HeaderStyle Width="90px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNumber" RenderMode="Lightweight" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="true" AllowFiltering="false" ShowSortIcon="true" SortExpression="FLDCOMPONENTNAME" ColumnGroupName="COMPONENT" UniqueName="COMPONENTNAME">
                            <HeaderStyle Width="180px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="180px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblName" RenderMode="Lightweight" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Code" AllowSorting="true" AllowFiltering="false" FilterDelay="5000" ShowFilterIcon="false" FilterControlWidth="100%" ShowSortIcon="false" SortExpression="FLDJOBCODE" ColumnGroupName="JOBS" UniqueName="CODE">
                            <HeaderStyle Width="70px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="70px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCode" RenderMode="Lightweight" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Title" AllowSorting="true" ShowSortIcon="true" AllowFiltering="false" FilterDelay="5000" ShowFilterIcon="false" FilterControlWidth="100%" SortExpression="FLDJOBTITLE" ColumnGroupName="JOBS" UniqueName="TITLE">
                            <HeaderStyle Width="300px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="300px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBTITLE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" UniqueName="ACTION" AllowFiltering="false">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="60px"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="60px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Job Description" Visible="false"
                                    CommandName="JOBDESCRIPTION" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdJobDesc"
                                    ToolTip="Job Description" Width="20PX" Height="20PX">
                                        <span class="icon"><i class="fas fa-list-alt"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" ID="cmdDelete" CommandName="DELETE" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                    ColumnsReorderMethod="Reorder" EnablePostBackOnRowClick="true">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <br />
        </telerik:RadAjaxPanel>
        <telerik:RadCodeBlock runat="server">
            <script type="text/javascript">
                Sys.Application.add_load(function () {
                    setTimeout(function () {
                        TelerikGridResize($find("<%= gvGlobalComponent.ClientID %>"));
                    }, 200);
                });

            </script>
        </telerik:RadCodeBlock>
    </form>
</body>
</html>
