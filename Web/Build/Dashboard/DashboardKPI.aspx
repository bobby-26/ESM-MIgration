<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardKPI.aspx.cs" Inherits="DashboardKPI" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>KPI</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
function Resize() {
setTimeout(function () {
TelerikGridResize($find("<%= gvKpi.ClientID %>"));
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
    <form id="frmVPRSServiceType" runat="server">
   <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
            EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="tblConfigureCity"
            runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"
            Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            
            <telerik:RadGrid RenderMode="Lightweight" ID="gvKpi" runat="server"
                 EnableViewState="false" AllowCustomPaging="true" AllowSorting="true"
                AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false" OnNeedDataSource="gvKpi_NeedDataSource"
                EnableHeaderContextMenu="true" OnItemDataBound="gvKpi_ItemDataBound" OnSortCommand="gvKpi_SortCommand"
                OnItemCommand="gvKpi_ItemCommand" ShowFooter="true" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                    HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle Width="102px" />
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger"
                                        Font-Bold="true">
                                    </telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    
                        <Columns>
                            
                            <telerik:GridTemplateColumn HeaderText="Measure Name" AllowSorting="false" HeaderStyle-Width="150px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblkpi" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDKPIID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblMeasureName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURENAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadLabel ID="lblkpiEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDKPIID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblMeasureNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURENAME") %>'></telerik:RadLabel>
                                </EditItemTemplate>
                                <FooterTemplate>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Vessel Name" AllowSorting="false" HeaderStyle-Width="150px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                 <EditItemTemplate>
                                <telerik:RadComboBox ID="ddlvessellist" runat="server" DataTextField="FLDVESSELNAME" DataValueField="FLDVESSELID" ></telerik:RadComboBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadComboBox ID="ddlvessellistAdd" runat="server" DataTextField="FLDVESSELNAME" DataValueField="FLDVESSELID" ></telerik:RadComboBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Range From" AllowSorting="false" HeaderStyle-Width="150px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                           
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblFromC" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFROMCOUNT") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="txtFromCEdit" runat="server" Width="80px"  IsInteger="true"
                                        IsPositive="true" MaxLength="9" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFROMCOUNT") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Number ID="txtFromCAdd" runat="server" Width="80px"  IsInteger="true"
                                        IsPositive="true" MaxLength="9" />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Range To" AllowSorting="false" HeaderStyle-Width="150px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblToC" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOCOUNT") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="txtToCEdit" runat="server" Width="80px"  IsInteger="true"
                                        IsPositive="true" MaxLength="9" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOCOUNT") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Number ID="txtToCAdd" runat="server" Width="80px"  IsInteger="true"
                                        IsPositive="true" MaxLength="9" />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Color" AllowSorting="false" HeaderStyle-Width="150px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            
                                <ItemTemplate>
                                    <div id="divColor" runat="server" style='<%# "width:80px; height:10px; background-color:" + DataBinder.Eval(Container,"DataItem.FLDCOLOR") %>'></div>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    
                                    <telerik:RadComboBox ID="ddlColorEdit" runat="server" DataTextField="FLDNAME" DataValueField="FLDCOLORCODE" ></telerik:RadComboBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                   
                                    <telerik:RadComboBox ID="ddlColorAdd" runat="server" DataTextField="FLDNAME" DataValueField="FLDCOLORCODE" ></telerik:RadComboBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Save" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Add" ID="cmdAdd" ToolTip="Add New">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                    AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4"
                        EnableNextPrevFrozenColumns="true" ScrollHeight="415px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
