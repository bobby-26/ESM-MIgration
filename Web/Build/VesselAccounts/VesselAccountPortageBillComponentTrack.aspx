<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountPortageBillComponentTrack.aspx.cs"
    Inherits="VesselAccountPortageBillComponentTrack" %>

<!DOCTYPE html>
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselCrew" Src="~/UserControls/UserControlVesselEmployee.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew BOW</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
        EnableShadow="true">
    </telerik:RadWindowManager>
    <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="ReportNotRelievedOnTime"
        runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"
        Height="100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <table id="tblPortageBillTack" width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblEmpNo" runat="server" Text="File No.">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtEmpNo" runat="server" MaxLength="6" CssClass="input_mandatory">
                    </telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblcomponent" runat="server" Text="Component Group">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlcomponentgroup" runat="server" DataTextField="FLDNAME"
                        DataValueField="FLDCOMPONENTTRACKID" Filter="Contains" MarkFirstMatch="true"
                        EnableLoadOnDemand="True">
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblEmpName" runat="server" Text="Name">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtName" runat="server" MaxLength="100">
                    </telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblFrom" runat="server" Text="Closing Between">
                    </telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtFrom" runat="server" />
                    <eluc:Date ID="txtTo" runat="server" />
                </td>
            </tr>
        </table>
        <eluc:TabStrip ID="Menuportagebilltrack" runat="server" OnTabStripCommand="Menuportagebilltrack_TabStripCommand">
        </eluc:TabStrip>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server"
            DecorationZoneID="gvportagebilltrack" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadGrid RenderMode="Lightweight" ID="gvportagebilltrack" runat="server"
            Height="82%" AllowCustomPaging="false" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" OnNeedDataSource="gvportagebilltrack_NeedDataSource"
            OnItemCommand="gvportagebilltrack_ItemCommand" OnItemDataBound="gvportagebilltrack_ItemDataBound"
            OnSortCommand="gvportagebilltrack_SortCommand" EnablePartialRendering="false"
            EnableHeaderContextMenu="true" AutoGenerateColumns="false" OnUpdateCommand="gvportagebilltrack_UpdateCommand"
            OnCustomAggregate="gvportagebilltrack_OnCustomAggregate" OnDeleteCommand="gvportagebilltrack_DeleteCommand"
            OnRowCreated="RowCreated">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView ShowGroupFooter="true" DataKeyNames="FLDPORTAGEBILLCOMPONENTTRACK"
                EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true"
                ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" AutoGenerateColumns="false"
                TableLayout="Fixed">
                <GroupByExpressions>
                    <telerik:GridGroupByExpression>
                        <SelectFields>
                            <telerik:GridGroupByField FieldName="FLDGROUPBY" FieldAlias="Name" HeaderText=""
                                SortOrder="Ascending" />
                        </SelectFields>
                        <GroupByFields>
                            <telerik:GridGroupByField FieldName="FLDGROUPBY" SortOrder="Ascending" />
                        </GroupByFields>
                    </telerik:GridGroupByExpression>
                </GroupByExpressions>
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
                    <telerik:GridTemplateColumn Visible="false" UniqueName="FLDGROUPBY" DataField="FLDGROUPBY"
                        HeaderStyle-Width="10px" ShowSortIcon="true">
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblGroup" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUPBY") %>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Vessel" AllowSorting="false" HeaderStyle-Width="110px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblvesselname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Sign on Rank" AllowSorting="false" HeaderStyle-Width="68px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblrankcode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>'>
                            </telerik:RadLabel>
                            <telerik:RadLabel ID="lblid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPORTAGEBILLCOMPONENTTRACK")%>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Component" AllowSorting="false" HeaderStyle-Width="78px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblempcode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEECODE")%>'>
                            </telerik:RadLabel>
                            <telerik:RadLabel ID="lblempname" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPNAME")%>'>
                            </telerik:RadLabel>
                            <telerik:RadLabel ID="lblPortagetrackId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPORTAGEBILLCOMPONENTTRACK") %>'>
                            </telerik:RadLabel>
                            <telerik:RadLabel ID="lblcomponentname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'
                                CssClass="tooltip" ClientIDMode="AutoID">
                            </telerik:RadLabel>
                            <eluc:ToolTip ID="ucToolTipComponent" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'
                                TargetControlId="lblcomponentname" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="From" AllowSorting="false" HeaderStyle-Width="75px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDFROMDATE", "{0:dd/MMM/yyyy}"))%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="To" AllowSorting="false" HeaderStyle-Width="75px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDTODATE", "{0:dd/MMM/yyyy}"))%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Closing" AllowSorting="false" HeaderStyle-Width="75px"
                        ShowSortIcon="true" UniqueName="CLOSING" DataField="FLDCLOSINGDATE" Aggregate="Custom"
                        FooterAggregateFormatString="{0}">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDCLOSINGDATE", "{0:dd/MMM/yyyy}"))%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Retained" AllowSorting="false" HeaderStyle-Width="65px"
                        ShowSortIcon="true" UniqueName="RETAINED" DataField="FLDRETAINEDAMOUNT" Aggregate="Sum"
                        FooterAggregateFormatString="{0}">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRetainedAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRETAINEDAMOUNT") %>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Refund" AllowSorting="false" HeaderStyle-Width="65px"
                        ShowSortIcon="true" FooterText="Total:" Aggregate="Sum" DataField="FLDREFUNDAMOUNT"
                        FooterAggregateFormatString="{0}">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRefundamount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFUNDAMOUNT") %>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Currency" AllowSorting="false" HeaderStyle-Width="63px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblCurrencycode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Voucher No" AllowSorting="false" HeaderStyle-Width="242px"
                        ShowSortIcon="true" UniqueName="CURRENCY" Aggregate="Custom" FooterStyle-Font-Bold="true"
                        FooterAggregateFormatString="Balance Amount(Retain - Refund)  :{0}">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblVoucherNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERNO") %>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Remarks" AllowSorting="false" HeaderStyle-Width="134px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadTextBox ID="txtRemarksEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'
                                MaxLength="200">
                            </telerik:RadTextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="false" HeaderStyle-Width="80px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Edit" ID="cmdEdit" ToolTip="Edit" CommandName="EDIT"
                                Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Delete" ID="cmdDelete" CommandName="DELETE"
                                ToolTip="Delete" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                            </asp:LinkButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Save" ID="cmdSave" CommandName="Update"
                                ToolTip="Save" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Cancel" ID="cmdCancel" CommandName="Cancel"
                                ToolTip="Cancel" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                            </asp:LinkButton>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
