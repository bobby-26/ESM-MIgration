<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnersPMSWorkorderList.aspx.cs"
    Inherits="OwnersPMSWorkorderList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlOwnersVessel.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Component Maintenance Record</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmWorkOrder" DecoratedControls="All" />
    <form id="frmWorkOrder" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div id="divFind" style="position: relative; z-index: 2">
                <table width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Vessel ID="ucVessel" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                                OnTextChangedEvent="ucVessel_OnTextChangedEvent" AutoPostBack="true" Width="180px"></eluc:Vessel>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblComponentNumber" runat="server" Text="Component Number"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtComponentNumber" runat="server" MaxLength="1000" Width="180px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblWorkOrderTitle" runat="server" Text="Work Order Title"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtWorkorderName" runat="server" MaxLength="50" Width="180px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblClassName" runat="server" Visible="false" Text="Component Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadDropDownList ID="ddlComponentClass" runat="server" Visible="true" Width="180px"
                                DataTextField="FLDCOMPONENTNAME" AppendDataBoundItems="true" DataValueField="FLDCOMPONENTID">
                                <Items>
                                    <telerik:DropDownListItem Text="--Select--" Value="" />
                                </Items>
                            </telerik:RadDropDownList>
                            <eluc:Hard ID="ddlStockClassType" runat="server" AppendDataBoundItems="true" Width="180px"
                                Visible="false" />
                        </td>
                    </tr>
                </table>
            </div>
            <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="MenuShowExcel_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvComponenetJob" Height="87%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" OnItemCommand="gvComponenetJob_ItemCommand" OnItemDataBound="gvComponenetJob_ItemDataBound" EnableViewState="false"
                ShowFooter="false" ShowHeader="true" OnNeedDataSource="gvComponenetJob_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" AutoGenerateColumns="false" DataKeyNames="FLDWORKORDERID" TableLayout="Fixed" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" HeaderStyle-Width="100%" GroupHeaderItemStyle-Wrap="false" Width="100%" >
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Last Done" Name="LastDone" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Component" Name="Component" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Number" ColumnGroupName="Component">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNUMBER") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" ColumnGroupName="Component">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Work Order Title">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNAME") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Frequency">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="8%" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDFREQUENCYNAME")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date" ColumnGroupName="LastDone">
                            <HeaderStyle Width="8%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDJOBLASTDONEDATE") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="hours" ColumnGroupName="LastDone">
                            <HeaderStyle Width="8%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDLASTDONEHOURS") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Hours Since">
                            <HeaderStyle Width="8%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDRUNHOURSINCE")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Last Counter Update On">
                            <HeaderStyle Width="8%"  Wrap="true"/>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDLASTCOUNTERUPDATE")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Current Value">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDCURRENTVALUE")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" ResizeGridOnColumnResize="false" />
                </ClientSettings>
            </telerik:RadGrid>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
