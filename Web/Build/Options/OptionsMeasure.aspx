<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OptionsMeasure.aspx.cs" Inherits="OptionsMeasure" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserGroup" Src="~/UserControls/UserControlUserGroup.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Measure Configuration</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

         <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvMeasures.ClientID %>"));
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
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:TabStrip ID="MenuTab" runat="server" OnTabStripCommand="MenuTab_TabStripCommand" TabStrip="true" />  
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
              <table width="80%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblUserGroup" runat="server" Text="User Group"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserGroup runat="server" ID="ucUserGroup" AutoPostBack="true" Width="420px" OnTextChangedEvent="ucUserGroup_TextChangedEvent" AppendDataBoundItems="true" />
                    </td>


                    <td>
                        <telerik:RadLabel ID="lblModule" runat="server" Text="Module "></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlModulelist" runat="server" CssClass="input" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlModulelist_SelectedIndexChanged"
                            AutoPostBack="True" DataTextField="FLDMODULENAME" DataValueField="FLDMODULEID" Width="270px" EmptyMessage="Type to select Module" Filter="Contains" MarkFirstMatch="true">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value="DUMMY"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
            </table>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvMeasures" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvMeasures_NeedDataSource" OnItemCommand="gvMeasures_ItemCommand" OnItemDataBound="gvMeasures_ItemDataBound"
                ShowFooter="FALSE" ShowHeader="true" EnableViewState="true" EnableHeaderContextMenu="true" GroupingEnabled="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                    AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center" DataKeyNames="FLDSHORTCODE">
                    <GroupByExpressions>
                        <telerik:GridGroupByExpression>
                            <SelectFields>
                               <telerik:GridGroupByField FieldName="FLDMODULENAME" FieldAlias="Module" SortOrder="Ascending" />
                            </SelectFields>
                            <GroupByFields>
                                <telerik:GridGroupByField FieldName="FLDMODULENAME" SortOrder="Ascending" />
                            </GroupByFields>
                        </telerik:GridGroupByExpression>
                    </GroupByExpressions>
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
                        <telerik:GridTemplateColumn HeaderText="Measure Name">
                            <HeaderStyle Width="80%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMeasureName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURENAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblMeasureCode" ToolTip="measurecode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'></telerik:RadLabel>                                                            
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Include YN">
                            <HeaderStyle Width="25%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                 <telerik:RadCheckBox runat="server" AutoPostBack="true" ID="chkMeasureRights" CommandName="UPDATE" />
                                <telerik:RadLabel runat="server" ID="lblRights" Visible="false"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDRIGHTS") %>'></telerik:RadLabel>

                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                    </Columns>
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

