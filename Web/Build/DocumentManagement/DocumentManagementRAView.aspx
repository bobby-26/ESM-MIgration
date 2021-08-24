<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementRAView.aspx.cs" Inherits="DocumentManagementRAView" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <telerik:RadCodeBlock ID="radCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvRiskAssessment.ClientID %>"));
                }, 200);
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
        <telerik:RadGrid RenderMode="Lightweight" ID="gvRiskAssessment" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false" OnNeedDataSource="gvRiskAssessment_NeedDataSource"
                OnItemDataBound="gvRiskAssessment_RowDataBound" ShowFooter="false" EnableViewState="false" DataKeyNames="FLDPROCESSID">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                    AutoGenerateColumns="false" TableLayout="Fixed" EnableHeaderContextMenu="true" CommandItemDisplay="None" DataKeyNames="FLDPROCESSID">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Type" Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="15%"></ItemStyle>
                            <HeaderTemplate>
                                Type
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTypeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Process">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="30%"></ItemStyle>
                            <HeaderTemplate>
                                Process
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblcategoryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Activity">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="50%"></ItemStyle>
                            <HeaderTemplate>
                                Activity
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActivityCondition" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVITYORCONDITION").ToString().Length > 45 ? (DataBinder.Eval(Container, "DataItem.FLDACTIVITYORCONDITION").ToString().Substring(0, 45) + "...") : DataBinder.Eval(Container, "DataItem.FLDACTIVITYORCONDITION").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucActivityCondition" TargetControlId="lblActivityCondition" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVITYORCONDITION") %>'
                                    Width="400px" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Process Name" Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                            <HeaderTemplate>
                                Process Name
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblProcessName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPROCESSNAME").ToString().Length > 45 ? (DataBinder.Eval(Container, "DataItem.FLDPROCESSNAME").ToString().Substring(0, 45) + "...") : DataBinder.Eval(Container, "DataItem.FLDPROCESSNAME").ToString()%>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucProcessName" TargetControlId="lblProcessName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCESSNAME") %>'
                                    Width="350px" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ref Number">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20%"></ItemStyle>
                            <HeaderTemplate>
                                Ref Number
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkRefNo" runat="server" CommandName="RASELECT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFNO") %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblProcessId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCESSID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
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
                <ClientSettings>
                    <Resizing AllowColumnResize="true" />
                    <Selecting AllowRowSelect="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                </ClientSettings>
            </telerik:RadGrid>
            </telerik:RadAjaxPanel>
    </form>
</body>
</html>