<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceComponentJobAuditList.aspx.cs"
    Inherits="PlannedMaintenanceComponentJobAuditList" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Component Job Audit</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%--<link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />--%>
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <%--<script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>--%>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmComponentJob" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <telerik:RadGrid RenderMode="Lightweight" Width="100%" ID="RadGrid1" runat="server" OnItemDataBound="RadGrid1_ItemDataBound" OnNeedDataSource="RadGrid1_NeedDataSource" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" OnPreRender="RadGrid1_PreRender" AutoGenerateColumns="false" Height="98%" EnableHeaderContextMenu="true" GroupingEnabled="false">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="Top">
                <HeaderStyle Width="102px" />
                <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Job Code" ColumnGroupName="JobCode" HeaderStyle-Width="115px">
                        <HeaderStyle Width="77px" />
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDJOBCODE") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Job Title" ColumnGroupName="JobTitle">
                        <HeaderStyle Width="170px" />
                        <ItemStyle Wrap="false" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblJobTitle" runat="server" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDJOBTITLE") %>' ClientIDMode="AutoID"></telerik:RadLabel>
                            <eluc:ToolTip ID="ucToolTipJobTitle" runat="server" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDJOBTITLE") %>' TargetControlId="lblJobTitle" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Responsibility" ColumnGroupName="Responsibility">
                        <HeaderStyle Width="125px" />
                        <ItemStyle Wrap="false" />
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container,"DataItem.FLDDISCIPLINENAME") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Frequency" ColumnGroupName="Frequency">
                        <HeaderStyle Width="140px" />
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container,"DataItem.FLDFREQUENCYNAME") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Window (Days)" ColumnGroupName="Window">
                        <HeaderStyle Width="110px" />
                        <ItemStyle Wrap="false" HorizontalAlign="Left" />
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container,"DataItem.FLDWINDOW") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Next Due Date" ColumnGroupName="JobNextDueDate">
                        <HeaderStyle Width="123px" />
                        <ItemStyle Wrap="false" HorizontalAlign="Left" />
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container,"DataItem.FLDJOBNEXTDUEDATE") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Last Done (Hrs)" ColumnGroupName="LastDoneHrs">
                        <HeaderStyle Width="119px" />
                        <ItemStyle Wrap="false" HorizontalAlign="Left" />
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container,"DataItem.FLDLASTDONEHOURS") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Next Due (Hrs)" ColumnGroupName="NextDueHrs">
                        <HeaderStyle Width="115px" />
                        <ItemStyle Wrap="false" HorizontalAlign="Left" />
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container,"DataItem.FLDNEXTDUEHOURS") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="History Mandatory">
                        <HeaderStyle Width="133px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDMANDATORY").ToString().ToUpper()   ==  "Y" ? "Yes" : "No"%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Planning Method">
                        <HeaderStyle Width="122px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDPLANINGMETHOD")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Priority">
                        <HeaderStyle Width="68px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDPRIORITY") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Maintenance Type">
                        <HeaderStyle Width="126px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDMAINTNANCETYPE")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Maintenance Class">
                        <HeaderStyle Width="124px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDMAINTNANCECLASS")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Maintenance Claim">
                        <HeaderStyle Width="130px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDMAINTNANCECAUSE")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="History Template">
                        <HeaderStyle Width="119px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDFORMNAME")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Work Order No.">
                        <HeaderStyle Width="116px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDWORKORDERNUMBER")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Last Modified Date">
                        <HeaderStyle Width="128px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDMODIFIEDDATE")) %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Last Modified By">
                        <HeaderStyle Width="150px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDMODIFIEDBYNAME")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox" />
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
