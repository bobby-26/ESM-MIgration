<%@ Page Language="C#" AutoEventWireup="True" CodeFile="OptionsMobilePermissionsLog.aspx.cs"
    Inherits="OptionsMobilePermissionsLog" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskedTextBox.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head2" runat="server">
    <title>Device Access</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style type="text/css">
            .linkB {
                text-decoration: none;
            }

            .bgOR {
                background-color: orangered !important;
            }
        </style>
        <script type="text/javascript">
            function DeleteRecord(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmDelete.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmMobilePermissionsLog" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <table id="tblConfigureDevice">
                <tr>
                    <td width="8%" style="text-align: right">
                        <telerik:RadLabel ID="lblApplication" runat="server" Text="Application"></telerik:RadLabel>
                    </td>
                    <td width="17%">
                        <telerik:RadTextBox ID="txtApplication" runat="server" MaxLength="20"></telerik:RadTextBox>
                    </td>
                    <td width="8%" style="text-align: right">
                        <telerik:RadLabel ID="lblPath" runat="server" Text="Path"></telerik:RadLabel>
                    </td>
                    <td width="17%">
                        <telerik:RadTextBox ID="txtPath" runat="server" MaxLength="20"></telerik:RadTextBox>
                    </td>
                    <td width="8%" style="text-align: right">
                        <telerik:RadLabel ID="lblType" runat="server" Text="Request Type"></telerik:RadLabel>
                    </td>
                    <td width="17%">
                        <telerik:RadTextBox ID="txtType" runat="server" MaxLength="20"></telerik:RadTextBox>
                    </td>
                    <td width="8%" style="text-align: right">
                        <telerik:RadLabel ID="lblContentType" runat="server" Text="Content Type"></telerik:RadLabel>
                    </td>
                    <td width="17%">
                        <telerik:RadTextBox ID="txtContentType" runat="server" MaxLength="20"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td width="8%" style="text-align: right">
                        <telerik:RadLabel ID="lblRequestedOn" runat="server" Text="Requested from"></telerik:RadLabel>
                    </td>
                    <td width="17%">
                        <eluc:Date ID="txtRequestedOn" runat="server" DatePicker="true" />
                    </td>
                    <td width="8%" style="text-align: right">
                        <telerik:RadLabel ID="lblRequestedTill" runat="server" Text=" - To - "></telerik:RadLabel>
                    </td>
                    <td width="17%">
                        <eluc:Date ID="txtRequestedTill" runat="server" DatePicker="true" />
                    </td>

                    <%--                 <td width="8%">
                        <telerik:RadLabel ID="lblDesc" runat="server" Text="Description"></telerik:RadLabel>
                    </td>
                    <td width="12%">
                        <telerik:RadTextBox ID="txtDesc" runat="server" MaxLength="20"></telerik:RadTextBox>
                    </td>--%>
                    <td width="8%" style="text-align: right">
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td width="17%">
                        <telerik:RadTextBox ID="txtStatus" runat="server" MaxLength="20"></telerik:RadTextBox>
                    </td>
                    <td width="8%" style="text-align: right">
                        <telerik:RadLabel ID="lblException" runat="server" Text="Exception"></telerik:RadLabel>
                    </td>
                    <td width="17%">
                        <telerik:RadTextBox ID="txtException" runat="server" MaxLength="20"></telerik:RadTextBox>
                    </td>

                </tr>
            </table>
            <eluc:TabStrip ID="MenuOptionsMobilePermissionsLog" runat="server" OnTabStripCommand="RegistersMobilePermissionsLog_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvMobilePermissionsLog" Height="88%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvMobilePermissionsLog_ItemCommand" OnItemDataBound="gvMobilePermissionsLog_ItemDataBound"
                ShowFooter="true" ShowHeader="true" EnableViewState="true" OnSortCommand="gvMobilePermissionsLog_SortCommand"
                OnNeedDataSource="gvMobilePermissionsLog_NeedDataSource" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDAPPLICATION">
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
                        <telerik:GridTemplateColumn HeaderText="Application" AllowSorting="true" SortExpression="FLDAPPLICATION">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLogId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOGID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblApplication" runat="server" ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDAPPLICATION") %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPLICATION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Path" AllowSorting="true" SortExpression="FLDPATH">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPath" runat="server" ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDPATH") %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDPATH") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Request Type" AllowSorting="true" SortExpression="FLDTYPE">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblType" runat="server" ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Content Type" AllowSorting="true" SortExpression="FLDCONTENTTYPE">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblContentType" runat="server" ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDCONTENTTYPE") %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTENTTYPE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Parameters" AllowSorting="true" SortExpression="FLDPARAMETERS">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblParameters" runat="server" ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDPARAMETERS") %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARAMETERS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Device" AllowSorting="true" SortExpression="FLDRESPONSE">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblResponse" runat="server" ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDRESPONSE").ToString() == "NA" ? "Not Registered" : "Registered" %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDRESPONSE").ToString() == "NA" ? "Not Registered" : "Registered" %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" AllowSorting="true" SortExpression="FLDSTATUS" UniqueName="gvStatus">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                       <%--   <telerik:GridTemplateColumn HeaderText="Status Code" AllowSorting="true" Visible="false" SortExpression="FLDSTATUSCODE">
                            <HeaderStyle Width="8%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatusCode" runat="server" ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSCODE") %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="Description" AllowSorting="true" SortExpression="FLDDESCRIPTION">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDescription" runat="server" ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>
                        <telerik:GridTemplateColumn HeaderText="Exception" AllowSorting="true" SortExpression="FLDEXCEPTION">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblException" runat="server" ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDEXCEPTION") %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXCEPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Requested On" AllowSorting="true" ShowSortIcon="true">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRequestedOn" runat="server" ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE","{0:dd/MMM/yyyy HH:mm}") %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE","{0:dd/MMM/yyyy HH:mm}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" EnableNextPrevFrozenColumns="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <asp:Button ID="ucConfirmDelete" runat="server" OnClick="ucConfirmDelete_Click" CssClass="hidden" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
