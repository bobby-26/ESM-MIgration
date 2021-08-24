<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceComponentCounters.aspx.cs"
    Inherits="PlannedMaintenanceComponentCounters" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Component Counters</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvComponentCounter.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;
            function pageLoad() {
                Resize();
            }            
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmComponentCounter" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gvComponentCounter">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvComponentCounter"/>
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <eluc:TabStrip ID="MenuComponentCounter" runat="server" OnTabStripCommand="ComponentCounter_TabStripCommand"></eluc:TabStrip>
        <telerik:RadGrid ID="gvComponentCounter" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvComponentCounter_ItemCommand"
                OnNeedDataSource="gvComponentCounter_NeedDataSource" ShowFooter="true" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnItemDataBound="gvComponentCounter_ItemDataBound">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Counter Type" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDCOUNTERID">
                            <HeaderStyle Width="139px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCounterID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTERID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCounterType" runat="server" Visible="True" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblCounterIDEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTERID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCounterTypeEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTERTYPE") %>'></telerik:RadLabel>
                                <eluc:Hard ID="ddlCounterTypeEdit" runat="server" AppendDataBoundItems="true" Width="120px"
                                    HardTypeCode="111" HardList='<%#PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 111) %>'
                                    SelectedHard='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTERTYPE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Hard ID="ddlCounterTypeAdd" runat="server" CssClass="dropdown_mandatory" HardTypeCode="111"
                                    HardList='<%# PhoenixRegistersHard.ListHard(1, 111) %>' AppendDataBoundItems="true"
                                    Width="120px" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date Read">
                            <HeaderStyle Width="137px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReadingDate" runat="server" Visible="True" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDREADINGDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date runat="server" ID="txtReadingDateEdit" CssClass="input" Width="117px" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDREADINGDATE")) %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <span id="spnReadingDateAdd">
                                    <eluc:Date runat="server" ID="txtReadingDateAdd" CssClass="input" Width="117px" />
                                </span>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Current Value">
                            <HeaderStyle Width="100px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCurrentValue" runat="server" Visible="True" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENTVALUE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Decimal runat="server" ID="txtCurrentValueEdit" Width="80px" MaxLength="14" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENTVALUE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Decimal runat="server" ID="txtCurrentValueAdd" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENTVALUE") %>' CssClass="input" MaxLength="14" Width="80px" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Start Date">
                            <HeaderStyle Width="136px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStartDate" runat="server" Visible="True" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDSTARTDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date runat="server" ID="txtStartDateEdit" CssClass="input" Width="117px" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDSTARTDATE")) %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date runat="server" ID="txtStartDateAdd" CssClass="input" Width="117px" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Start Value">
                            <HeaderStyle Width="99px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right" />
                            <EditItemTemplate>
                                <eluc:Decimal runat="server" ID="txtStartValueEdit" CssClass="input"
                                    MaxLength="14" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTARTVALUE") %>'
                                    Width="80px" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStartValue" runat="server" Width="80px" Visible="True" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTARTVALUE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <eluc:Decimal runat="server" ID="txtStartValueAdd" CssClass="input" MaxLength="14" Width="80px" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Zeroed Date">
                            <HeaderStyle Width="136px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblZeroedDate" runat="server" Visible="True" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDZEROEDDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date runat="server" ID="txtZeroedDateEdit" CssClass="input" Width="117px" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDZEROEDDATE")) %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date runat="server" ID="txtZeroedDateAdd" CssClass="input" Width="117px" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Zeroed Value">
                            <HeaderStyle Width="98px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblZeroedValue" runat="server" Visible="True" Text='<%# DataBinder.Eval(Container,"DataItem.FLDZEROEDVALUE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Decimal runat="server" ID="txtZeroedValueEdit" CssClass="input txtNumber" AutoPostBack="true" OnTextChangedEvent="CalculateAverage"
                                    Width="80px" MaxLength="14" Text='<%# DataBinder.Eval(Container,"DataItem.FLDZEROEDVALUE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Decimal runat="server" ID="txtZeroedValueAdd" CssClass="input txtNumber" AutoPostBack="true" OnTextChangedEvent="CalculateAverage"
                                    Width="80px" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Average">
                            <HeaderStyle Width="98px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAverage" runat="server" Visible="True" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAVERAGE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnAverageEdit">
                                    <eluc:Decimal runat="server" ID="txtAverageEdit" CssClass="readonlytextbox txtNumber" ReadOnly="true" Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAVERAGE") %>' />
                                </span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <span id="spnAverageAdd">
                                    <eluc:Decimal runat="server" ID="txtAverageAdd" CssClass="readonlytextbox txtNumber" ReadOnly="true"
                                        Width="80px" />
                                </span>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Depends">
                            <HeaderStyle Width="135px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDepends" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPENDSONNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListShowComponentEdit">
                                    <telerik:RadTextBox ID="txtDependsOnEdit" runat="server" CssClass="input" MaxLength="100"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPENDSONNAME") %>' Width="80px">
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowComponentEdit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataItem %>" />
                                    <telerik:RadTextBox ID="txtDepdendsOnComponentNameEdit" runat="server" CssClass="input"
                                        MaxLength="100" Width="0">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtDependsOnComponentIDEdit" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPENDSON") %>'
                                        Width="0">
                                    </telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <span id="spnPickListShowComponentAdd">
                                    <telerik:RadTextBox ID="txtDependsOnAdd" runat="server" CssClass="input" MaxLength="100"
                                        Width="80px">
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowComponentAdd" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataItem %>" />
                                    <telerik:RadTextBox ID="txtDepdendsOnComponentName" runat="server" CssClass="input" MaxLength="100"
                                        Width="0">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtDependsOnComponentID" runat="server" CssClass="input" MaxLength="100"
                                        Width="0">
                                    </telerik:RadTextBox>
                                </span>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle Width="70px" />
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                    ToolTip="Edit" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                    ToolTip="Delete" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save"
                                    CommandName="UPDATE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                    ToolTip="Save" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                    ToolTip="Cancel" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save"
                                    CommandName="Add" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
                                    ToolTip="Add New" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
         
    </form>
</body>
</html>
