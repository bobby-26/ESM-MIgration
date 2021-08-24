<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelEmail.aspx.cs"
    Inherits="CrewTravelEmail" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Mail Compose</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmOptionEmail" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />        
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="88%">
            <telerik:RadLabel ID="lblTitle" runat="server" Visible="false"></telerik:RadLabel>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="EmailMenu" runat="server" OnTabStripCommand="EmailMenu_TabStripCommand"></eluc:TabStrip>
            <br />
            <telerik:RadLabel ID="lblheadertext" runat="server" Text="A separate E-mail will be send to all the below Agents." ForeColor="Blue" Font-Bold="True"></telerik:RadLabel>
            <telerik:RadGrid RenderMode="Lightweight" ID="RepAgents" runat="server" AllowCustomPaging="false" AllowSorting="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="RepAgents_NeedDataSource" MasterTableView-GridLines="None"
                EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true" AllowPaging="false" OnItemDataBound="RepAgents_ItemDataBound">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderStyle-Width="50px">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" Width="70px" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblSNoHeader" runat="server">S.No.</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                         <telerik:RadLabel ID="lblSNo" runat="server"></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="270px">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblAgentNameHeader" runat="server">Agent Name</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbltravelagentid" runat="server" Visible="false" Text='<%# Eval("FLDTRAVELAGENTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblreqno" runat="server" Visible="false" Text='<%# Eval("FLDREQUISITIONNO") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtname" runat="server" Text='<%# Eval("FLDNAME") %>' CssClass="readonlytextbox" ReadOnly="true"
                                    Width="250px">
                                </telerik:RadTextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                     
                        <telerik:GridTemplateColumn HeaderStyle-Width="270px">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblToHeader" runat="server">To</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadTextBox ID="txtEmailto" runat="server" Text='<%# Eval("FLDEMAIL1") %>' CssClass="readonlytextbox" ReadOnly="true"
                                    Width="250px">
                                </telerik:RadTextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                      
                        <telerik:GridTemplateColumn HeaderStyle-Width="370px">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblCcHeader" runat="server">Cc</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadTextBox ID="txtEmailcc" runat="server" Text='<%# Eval("FLDEMAIL2") %>' CssClass="readonlytextbox" ReadOnly="true"
                                    Width="350px">
                                </telerik:RadTextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="3" EnableColumnClientFreeze="true"
                        ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <br />
             <table width="100%" style="font-family: Tahoma; font-size: 11px">
                <tr>
                    <td style="width: 40px">
                        <b>Subject</b>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSubject" CssClass="readonlytextbox" Width="70%" runat="server" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
             <telerik:RadEditor ID="edtBody" runat="server" Width="99%" EmptyMessage="" RenderMode="Lightweight" SkinID="DefaultSetOfTools">
            </telerik:RadEditor>          
        </telerik:RadAjaxPanel>
        <%--   <cc1:Editor ID="edtBody" runat="server" Width="100%" Height="275px" CssClass="readonlytextbox" />--%>
    </form>
</body>
</html>
