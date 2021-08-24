<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionUndesirableEventWorstCase.aspx.cs" Inherits="InspectionUndesirableEventWorstCase" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hazard" Src="~/UserControls/UserControlRAHazardType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Severity" Src="~/UserControls/UserControlRASeverity.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Undesirable Event / Worst case</title>
    <telerik:RadCodeBlock ID="Radcodeblock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvRASubHazard.ClientID %>"));
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
    <form id="frmRASubHazard" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true"></telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="tblConfigure" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" >
            <eluc:TabStrip ID="MenuMapping" runat="server" OnTabStripCommand="MenuMapping_TabStripCommand" Title="Undesirable Event / Worst Case"></eluc:TabStrip>  
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="demo-container no-bg size-wide">
                <table id="tblConfigure" width="100%">
                    <tr>
                        <td width="20%">
                            <telerik:RadLabel ID="lblevent" runat="server" Text="Contact Type / Undesirable Event"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txteventname" runat="server" Width="270px" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <eluc:TabStrip ID="MenuRASubHazard" runat="server" OnTabStripCommand="RASubHazard_TabStripCommand"></eluc:TabStrip>
            <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvRASubHazard" DecoratedControls="All" EnableRoundedCorners="true" />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvRASubHazard" runat="server" AllowCustomPaging="true"
                Font-Size="11px" AllowPaging="true" AllowSorting="true" OnNeedDataSource="gvRASubHazard_NeedDataSource"
                Width="100%" CellPadding="3" OnItemCommand="gvRASubHazard_ItemCommand" OnSortCommand="gvSubHazard_SortCommand" OnItemDataBound="gvRASubHazard_ItemDataBound"
                ShowFooter="true" EnableHeaderContextMenu="true" GroupingEnabled="false"
                ShowHeader="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDWORSTCASEUNDESIRABLEEVENTID">
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
                        <telerik:GridTemplateColumn HeaderText="Worst Case" HeaderStyle-Width="19%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="19%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWorstCase" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORSTCASENAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lbluniqueid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORSTCASEUNDESIRABLEEVENTID") %>' Visible="false"></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <telerik:RadDropDownList ID="ddlWorstCase" runat="server" AppendDataBoundItems="true"
                                    CssClass="dropdown_mandatory" ExpandDirection="Up">
                                    <Items>
                                        <telerik:DropDownListItem Text="--Select--" Value="DUMMY" />
                                    </Items>
                                </telerik:RadDropDownList>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Severity" HeaderStyle-Width="10%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSeverity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEVERITY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Score" HeaderStyle-Width="5%">
                            <ItemStyle Wrap="true" HorizontalAlign="right" Width="5%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblScore" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCORE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Risk of Escalation" HeaderStyle-Width="14%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="14%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEngControl" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKOFESCALATION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Equipment" HeaderStyle-Width="14%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="14%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOperational" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEQUIPMENT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Procedures, Forms and Checklists" HeaderStyle-Width="14%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="14%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblProcedure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCEDURES") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="PPE" HeaderStyle-Width="14%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="14%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPPE" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPPELIST") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="10%">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Control Mapping" CommandName="CONTROLMAPPING" ID="cmdTypeMapping" ToolTip="Control Mapping">
                                        <span class="icon"><i class="fas fa-tasks"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                        <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="ADD" ID="cmdAdd" ToolTip="Add New">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
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

