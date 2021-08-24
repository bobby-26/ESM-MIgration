<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersContractCrew.aspx.cs"
    Inherits="RegistersContractCrew" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Wage Components</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmCrewList" DecoratedControls="All" EnableRoundedCorners="true" />
    <form id="frmCrewList" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuRegistersCity" runat="server" OnTabStripCommand="RegistersCity_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCrew" Height="97%" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" OnItemCommand="gvCrew_ItemCommand" OnItemDataBound="gvCrew_ItemDataBound" EnableHeaderContextMenu="true"
                ShowFooter="false" ShowHeader="true" EnableViewState="false" OnNeedDataSource="gvCrew_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true" AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Code">
                            <HeaderStyle Width="6%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblComponentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblShortCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Component">
                            <HeaderStyle Width="17%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblComponentname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'></telerik:RadLabel>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Global Wage Component">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWageComponentName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDWAGECOMPONENTNAME")%>' ToolTip='<%# DataBinder.Eval(Container, "DataItem.FLDWAGECOMPONENTNAME")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Calculation Basis">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblcalbasis" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCALCULATIONBASISNAME") %>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDCALCULATIONBASISNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Payable Basis">
                            <HeaderStyle Width="16%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblpayabelbasis" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYABLEBASISNAME") %>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDPAYABLEBASISNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Included Onboard">
                            <HeaderStyle Width="8%" Wrap="true" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDINCLUDEDONBOARDYNNAME")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Earning / Deduction">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDEARNINGDEDUCTIONNAME")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Active">
                            <HeaderStyle Width="5%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDACTIVEYNNAME")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Contract Setting">
                            <HeaderStyle Width="9%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDSHOWTOOWNERDESC")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Show to owner at sign off">
                            <HeaderStyle Width="11%" Wrap="true" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDSHOWTOOWNERSIGNOFFDESC")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Is Check Offer Letter">
                            <HeaderStyle Width="7%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDISCHECKOFFERLETTERDESC")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
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
