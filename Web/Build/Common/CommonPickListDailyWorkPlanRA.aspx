<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonPickListDailyWorkPlanRA.aspx.cs" Inherits="CommonPickListDailyWorkPlanRA" %>

<!DOCTYPE html>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Risk Assessment</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersPortAgent" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:TabStrip ID="MenuPortAgent" runat="server" OnTabStripCommand="MenuPortAgent_TabStripCommand"></eluc:TabStrip>
            <table id="tblConfigurePortAgent" width="100%">
                <tr>
                    <td width="5%">
                        <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                    </td>
                    <td width="35%">
                        <telerik:RadRadioButtonList ID="rblType" runat="server" CssClass="input" Direction="Horizontal"
                            AutoPostBack="true" OnSelectedIndexChanged="rblType_SelectedIndexChanged" Width="94%">
                            <Items>
                                <telerik:ButtonListItem Text="Process" Value="1" />
                                <telerik:ButtonListItem Text="Generic" Value="2" />
                                <telerik:ButtonListItem Text="Machinery" Value="3" />
                                <telerik:ButtonListItem Text="Navigation" Value="4" />
                                <telerik:ButtonListItem Text="Cargo" Value="5" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                    <td width="5%">
                        <telerik:RadLabel ID="lblKeyword" runat="server" Text="Keyword"></telerik:RadLabel>
                    </td>
                    <td width="20%">
                        <telerik:RadTextBox ID="txtActivity" runat="server" CssClass="input" Width="80%"></telerik:RadTextBox>
                    </td>
                    <td width="5%">
                        <telerik:RadLabel ID="lblCategory" runat="server" Text="Category"></telerik:RadLabel>
                    </td>
                    <td width="30%">
                        <telerik:RadComboBox ID="ddlCategory" runat="server" CssClass="input" AutoPostBack="true" Width="70%"
                            Filter="Contains" >
                        </telerik:RadComboBox>
                    </td>
                </tr>
            </table>
            <telerik:RadGrid ID="gvPortAgent" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowPaging="true" AllowCustomPaging="true" 
                Height="85%" CellPadding="3" OnItemCommand="gvPortAgent_ItemCommand" OnItemDataBound="gvPortAgent_ItemDataBound"
                ShowHeader="true" AllowSorting="true" OnNeedDataSource="gvPortAgent_NeedDataSource"> 
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDRISKASSESSMENTID">
                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Category">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORY")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Activity">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActivity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVITY").ToString().Length>10 ? DataBinder.Eval(Container, "DataItem.FLDACTIVITY").ToString().Substring(0, 10) + "..." : DataBinder.Eval(Container, "DataItem.FLDACTIVITY").ToString() %> '></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipActivity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVITY") %>' TargetControlId="lblActivity" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Process Name" Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblProcessName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCESSNAME")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reference No">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRAId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKASSESSMENTID")  %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE")  %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRefNo" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFNO")  %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkRefNo" runat="server" CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFNO") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn  HeaderText="Risks/Aspects" Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblRisksAspects" runat="server" Text="Risks/Aspects"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRA" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKASSESSMENT")  %>'></telerik:RadLabel>
                            </ItemTemplate>
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
