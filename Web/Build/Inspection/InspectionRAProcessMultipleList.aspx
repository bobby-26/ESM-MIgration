<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRAProcessMultipleList.aspx.cs" Inherits="InspectionRAProcessMultipleList" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Category" Src="~/UserControls/UserControlRACategory.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Risk Assessment Process</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <eluc:TabStrip ID="MenuGeneral" runat="server" OnTabStripCommand="MenuGeneral_TabStripCommand"></eluc:TabStrip>
        <iframe runat="server" id="ifMoreInfo" scrolling="yes" style="min-height: 450px; width: 99.5%"></iframe>
        <eluc:TabStrip ID="MenuProcess" runat="server" OnTabStripCommand="MenuProcess_TabStripCommand"></eluc:TabStrip>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvRiskAssessmentProcess" runat="server" AutoGenerateColumns="False" AllowPaging="true" AllowCustomPaging="true"
            Font-Size="11px" Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" OnItemCommand="gvRiskAssessmentProcess_ItemCommand" GroupingEnabled="false" EnableHeaderContextMenu="true"
            DataKeyNames="FLDRISKASSESSMENTPROCESSMULTIPLEID" OnItemDataBound="gvRiskAssessmentProcess_ItemDataBound" OnNeedDataSource="gvRiskAssessmentProcess_NeedDataSource"
            OnSelectedIndexChanging="gvRiskAssessmentProcess_SelectedIndexChanging">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false">
                <ColumnGroups>
                    <telerik:GridColumnGroup Name="FromToPort" HeaderText="Port" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                </ColumnGroups>
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
                    <%--<asp:ButtonField Text="DoubleClick" CommandName="EDIT" Visible="false" />--%>
                    <telerik:GridTemplateColumn HeaderText="Activity/Condition">
                        <ItemStyle Wrap="true" HorizontalAlign="Left" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblProcessMultipleId" Visible="false" runat="server" Width="30px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKASSESSMENTPROCESSMULTIPLEID")  %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblActivityCondition" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVITYCONDITION")  %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Category">
                        <ItemStyle Wrap="true" HorizontalAlign="Left" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblProcessName" Width="100px" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME")  %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Date">
                        <ItemStyle Wrap="true" HorizontalAlign="Left" Width="80px"></ItemStyle>
                        <ItemTemplate>
                            <%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDCREATEDDATE"])%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                        <ItemStyle Wrap="true" HorizontalAlign="Center" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit"
                                ToolTip="Edit">
                                 <span class="icon"><i class="fas fa-edit"></i></span>
                            </asp:LinkButton>
                            <img id="Img3" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                            <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete"
                                ToolTip="Delete">
                                 <span class="icon"><i class="fas fa-trash"></i></span>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
    </form>
</body>
</html>
