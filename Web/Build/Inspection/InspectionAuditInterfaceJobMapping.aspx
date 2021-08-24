<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionAuditInterfaceJobMapping.aspx.cs" Inherits="InspectionAuditInterfaceJobMapping" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Job</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvInspectionJob" runat="server" CellSpacing="0" GridLines="None"
                AllowSorting="true" EnableHeaderContextMenu="true" EnableViewState="false" Height="90%"
                GroupingEnabled="false" OnNeedDataSource="gvInspectionJob_NeedDataSource" OnItemDataBound="gvInspectionJob_ItemDataBound">

                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">

                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>

                    <Columns>

                         <telerik:GridTemplateColumn HeaderText="JOb Code" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="True" HorizontalAlign="Left" Width="100px"></ItemStyle>
                            <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblJobCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="JOb Mapping" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="True" HorizontalAlign="Left" Width="320px"></ItemStyle>
                            <HeaderStyle Width="320px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblJobTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBTITLE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                           <telerik:GridTemplateColumn HeaderText="JOb Details" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="True" HorizontalAlign="Left" Width="320px"></ItemStyle>
                            <HeaderStyle Width="320px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblJobDetails" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBDETAILS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText="Last Done Date" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="70px"></ItemStyle>
                            <HeaderStyle Width="70px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDate" runat="server" Text='<% # General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDLASTDATE").ToString())  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                         <telerik:GridTemplateColumn HeaderText="Due Date" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="70px"></ItemStyle>
                            <HeaderStyle Width="70px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDueDate" runat="server" Text='<% # General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDUEDATE").ToString())  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>

                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                    ColumnsReorderMethod="Reorder">

                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>

            </telerik:RadGrid>

        </telerik:RadAjaxPanel>

    </form>
</body>
</html>
