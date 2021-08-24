<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OptionsMoreInfoGroupList.aspx.cs" Inherits="OptionsMoreInfoGroupList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Group List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadGrid RenderMode="Lightweight" ID="dgUserGroups" runat="server" CellSpacing="0" GridLines="None" EnableViewState="false" Font-Size="11px"
            GroupingEnabled="false" EnableHeaderContextMenu="true" OnNeedDataSource="dgUserGroups_NeedDataSource" ShowFooter="False" ShowHeader="true" Width="100%">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false">
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Group Name" AllowSorting="true" SortExpression="FLDGROUPNAME">
                        <ItemTemplate>
                            <asp:Label ID="lblGroupName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUPNAME") %>'></asp:Label>
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
