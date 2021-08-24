<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewWorkingGearEmail.aspx.cs"
    Inherits="CrewWorkingGearEmail" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmMessage" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Mail Compose</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmOptionEmail" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:ConfirmMessage ID="ucConfirm" runat="server" Text="" Visible="false" />
            <eluc:TabStrip ID="EmailMenu" runat="server" OnTabStripCommand="EmailMenu_TabStripCommand"></eluc:TabStrip>
            <br />
            <table width="100%" style="font-family: Tahoma; font-size: 11px">              
                <tr>
                    <td colspan="2">

                        <telerik:RadGrid RenderMode="Lightweight" ID="RepAgents" runat="server" EnableViewState="false"
                            AllowCustomPaging="true" AllowSorting="true" AllowPaging="false" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                            OnNeedDataSource="RepAgents_NeedDataSource" EnableHeaderContextMenu="true"
                            ShowFooter="false"
                            AutoGenerateColumns="false">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" TableLayout="Fixed">
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
                                    <telerik:GridTemplateColumn HeaderText="S.No." AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="5%">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblQuantityitem" runat="server" Text='<%# Container.DataSetIndex + 1 %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Agent Name" AllowSorting="false" ShowSortIcon="true">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lbltravelagentid" runat="server" Visible="false" Text='<%# Eval("FLDCREWWGAGENTID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblagentid" runat="server" Visible="false" Text='<%# Eval("FLDAGENTID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblNeededid" runat="server" Visible="false" Text='<%# Eval("FLDNEEDEDID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblreqno" runat="server" Visible="false" Text='<%# Eval("FLDREFNUMBER") %>'></telerik:RadLabel>
                                            <telerik:RadTextBox ID="txtname" runat="server" Text='<%# Eval("FLDNAME") %>' CssClass="readonlytextbox" ReadOnly="true"
                                                Width="100%">
                                            </telerik:RadTextBox>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="To" AllowSorting="false" ShowSortIcon="true">
                                        <ItemTemplate>
                                            <telerik:RadTextBox ID="txtEmailto" runat="server" Text='<%# Eval("FLDEMAIL1") %>' CssClass="readonlytextbox" ReadOnly="true"
                                                Width="250px">
                                            </telerik:RadTextBox>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Cc" AllowSorting="false" ShowSortIcon="true">
                                        <ItemTemplate>
                                            <telerik:RadTextBox ID="txtEmailcc" runat="server" Text='<%# Eval("FLDEMAIL2") %>' CssClass="readonlytextbox" ReadOnly="true"
                                                Width="350px">
                                            </telerik:RadTextBox>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                                <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>

                        <br />
                    </td>
                </tr>
            </table>
            <table width="100%" style="font-family: Tahoma; font-size: 11px">
                <tr>
                    <td style="width: 40px">                        
                        <telerik:RadLabel ID="lblsub" runat="server" Text="Subject" Font-Bold="true"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSubject" CssClass="readonlytextbox" Width="80%" runat="server" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <span id="Span1" class="icon" runat="server"><i class="fas fa-info-circle" style="align-content: center"></i></span>
                        <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="RadToolTip2" Width="300px" ShowEvent="onmouseover"
                            RelativeTo="Element" Animation="Fade" TargetControlID="Span1" IsClientID="true"
                            HideEvent="ManualClose" Position="MiddleRight" EnableRoundedCorners="true"
                            Text="Note: A separate E-mail will be sent to all the listed Agents.">
                        </telerik:RadToolTip>
                    </td>
                </tr>
            </table>
            <br />
            <telerik:RadEditor ID="edtBody" runat="server" Width="99%" RenderMode="Lightweight" EditModes="Design" SkinID="DefaultSetOfTools">
            </telerik:RadEditor>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
