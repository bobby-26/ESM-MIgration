<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayRollSGForeignWorkerLevy.aspx.cs" Inherits="HR_PayRollSGForeignWorkerLevy" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="vessellist" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Month" Src="~/UserControls/UserControlMonth.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Year" Src="~/UserControls/UserControlYear.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title>Foreign Worker Levy</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvfwl.ClientID %>"));
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
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <telerik:RadAjaxPanel ID="radajaxpanel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
            <table width="100%">
                <tbody>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblvesselname" runat="server" Text="Vessel " />
                        </td>
                        <th>&nbsp
                        </th>
                        <td>
                            <eluc:vessellist ID="ddlvessellist" runat="server" Width="250px" CssClass="input" SyncActiveVesselsOnly="true" ManagementType="FUL"
                                Entitytype="VSL" AutoPostBack="false" ActiveVesselsOnly="true" VesselsOnly="true" />

                        </td>


                         <td>&nbsp</td>
                        <td></td>    
                        <td></td>
                        <td>&nbsp</td>
                        <td> 
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="RadLabel1" runat="server" Text="Month "  />
                        </td>
                        <td>&nbsp</td>
                        <td>
                            <eluc:Month runat="server" ID="ddlmonth"  CssClass="input_mandatory"/>
                        </td>
                        <td>&nbsp</td>
                        <td>
                            <telerik:RadLabel ID="RadLabel2" runat="server" Text="Year " />
                        </td>
                        <td>&nbsp</td>
                        <td>
                            <eluc:Year runat="server" ID="ddlyear" CssClass="input_mandatory"/>
                        </td>
                    </tr>
                </tbody>
            </table>
            <eluc:TabStrip ID="gvmenu" runat="server" OnTabStripCommand="gvmenu_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvfwl" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" ShowFooter="false" Style="margin-bottom: 0px" EnableViewState="true" EnableHeaderContextMenu="true"
                OnNeedDataSource="gvfwl_NeedDataSource"
                OnItemDataBound="gvfwl_ItemDataBound"
                OnItemCommand="gvfwl_ItemCommand">

                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDSGPRFWLAMOUNTID" TableLayout="Fixed">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    <ColumnGroups>
                        <telerik:GridColumnGroup Name="E" HeaderText="Employee" />
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText='Vessel' AllowSorting='true'>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblvessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText=' Code' AllowSorting='true' ColumnGroupName="E">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblcode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEECODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText='Name ' AllowSorting='true' ColumnGroupName="E">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblname" runat="server" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText='Type' AllowSorting='true' ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbltype" runat="server" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                      <telerik:GridTemplateColumn HeaderText='ID No.' AllowSorting='true' ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblidno" runat="server" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDINCOMETAXREFERENCENUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText='Work Permit No. ' AllowSorting='true' ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblworkpermitnumber" runat="server" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDWORKPERMITNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText='FWl Payable  ' AllowSorting='true' ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblfwl" runat="server" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDEMPLOYERFWLAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                       
                       <%-- <telerik:GridTemplateColumn HeaderText='Month ' AllowSorting='true' >
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblmonth" runat="server" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDMONTH") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder" EnablePostBackOnRowClick="true">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
