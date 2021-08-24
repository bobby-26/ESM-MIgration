<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionMOCRevisionHistory.aspx.cs" Inherits="InspectionMOCRevisionHistory" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MOC Revision History</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
         <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
         <telerik:RadGrid ID="gvMocRevision" runat="server" AutoGenerateColumns="False" OnNeedDataSource="gvMocRevision_NeedDataSource" Font-Size="11px"
                            CellPadding="3"  AllowPaging="false" AllowCustomPaging="false"
                            ShowFooter="false" EnableHeaderContextMenu="true" GroupingEnabled="false" ShowHeader="true" EnableViewState="true" AllowSorting="true">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                                AllowNaturalSort="false" AutoGenerateColumns="false">
                                <NoRecordsTemplate>
                                    <table runat="server" width="100%" border="0">
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
                        <telerik:GridTemplateColumn HeaderText="Office/Ship">
                            <ItemStyle HorizontalAlign="Left"/>
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblofficeship" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'>
                                </telerik:RadLabel>                               
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="RA.No">
                            <ItemStyle HorizontalAlign="Left" />

                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblranumber" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status">
                            <ItemStyle HorizontalAlign="Left"  />
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblstatus" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Requested By">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblrequestedby" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDREQUESTEDBYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Requested Date">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblrequestedby" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDREQUESTEDDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rev.No">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblrevisionno" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDREVISIONNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
               <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                   AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                   <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                   <Scrolling  UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                   <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
               </ClientSettings>
            </telerik:RadGrid>
    </form>
</body>
</html>
