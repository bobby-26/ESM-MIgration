<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementFMSMaintenanceFormComponentJobMapList.aspx.cs"
    Inherits="DocumentManagementFMSMaintenanceFormComponentJobMapList" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Component Job</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= RadGrid1.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmComponentJob" runat="server" autocomplete="off">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <telerik:RadCodeBlock ID="Radcodeblock2" runat="server">
        <eluc:TabStrip ID="MenuDivComponentJob" runat="server" OnTabStripCommand="MenuDivComponentJob_TabStripCommand">
        </eluc:TabStrip>
    </telerik:RadCodeBlock>
    <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel1">
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server"
            DecorationZoneID="RadGrid1" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" runat="server" AllowCustomPaging="true"
            AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" OnNeedDataSource="RadGrid1_NeedDataSource"
            OnPreRender="RadGrid1_PreRender" EnableHeaderContextMenu="true" GroupingEnabled="false"
            OnItemDataBound="RadGrid1_ItemDataBound" OnItemCommand="RadGrid1_ItemCommand">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                AllowNaturalSort="false" AutoGenerateColumns="false">
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
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Component No.">
                        <HeaderStyle Width="119px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNUMBER") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Component Name">
                        <HeaderStyle Width="325px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Job Code">
                        <HeaderStyle Width="110px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDJOBCODE") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Job Title">
                        <HeaderStyle Width="340px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDJOBTITLE") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Frequency">
                        <HeaderStyle Width="155px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDFREQUENCYNAME")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Remaining Frequency">
                        <HeaderStyle Width="161px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDREMAININGFREQUENCY")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Last Done">
                        <HeaderStyle Width="90px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDJOBLASTDONEDATE"))%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Last Done Hour">
                        <HeaderStyle Width="112px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDLASTDONEHOURS"))%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Responsibility">
                        <HeaderStyle Width="120px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDDISCIPLINENAME")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Next Due">
                        <HeaderStyle Width="110px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDJOBNEXTDUEDATE"))%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Priority">
                        <HeaderStyle Width="70px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDPRIORITY")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" ScrollHeight="" UseStaticHeaders="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
