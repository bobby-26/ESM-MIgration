<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionAuditChecklistVerification.aspx.cs"
    Inherits="InspectionAuditChecklistVerification" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Search Results</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            $(document).ready(function () {
                var browserHeight = $telerik.$(window).height();
                $("#gvChecklistVerification").height(browserHeight - 40);
            });

        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSearchResults" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="99.9%"></telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="RadAjaxPanel1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="99.9%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSectionmit" OnClick="cmdHiddenSectionmit_Click" />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvChecklistVerification" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowPaging="true" AllowCustomPaging="true"
                Width="100%" Height="100%" CellPadding="3" OnItemCommand="gvChecklistVerification_ItemCommand" OnItemDataBound="gvChecklistVerification_ItemDataBound"
                ShowFooter="true" ShowHeader="true" GroupingEnabled="false" EnableHeaderContextMenu="true" OnNeedDataSource="gvChecklistVerification_NeedDataSource"
                EnableViewState="false" AllowSorting="true" DataKeyNames="FLDVERIFICATIONID">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <ColumnGroups>
                        <telerik:GridColumnGroup Name="FromToPort" HeaderText="Port" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                    </ColumnGroups>
                    <NoRecordsTemplate>
                        <table width="99.9%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderStyle-Width="40px">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" VerticalAlign="Top" Width="10px"></ItemStyle>
                            <HeaderTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" ImageAlign="Middle" CommandName="CHECKLISTCLEAR" ID="cmdClear"
                                    ToolTip="Clear Selection">
                                         <span class="icon"><i class="fas fa-eraser"></i></span>
                                </asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadRadioButton ID="rdbUser" runat="server" OnCheckedChanged="rdbUser_CheckedChanged" ClientIDMode="AutoID"></telerik:RadRadioButton>
                                <telerik:RadLabel ID="lblVerificationId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVERIFICATIONID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblVerificationLineItemId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVERIFICATIONDONEBYID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Checklist" HeaderStyle-Width="235px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" VerticalAlign="Top" Width="150px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblChecklist" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCHECKLIST") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblChecklistId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCHECKLISTID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Verified Y/N" HeaderStyle-Width="94px">
                            <ItemStyle Wrap="true" HorizontalAlign="Center" VerticalAlign="Top" Width="20px"></ItemStyle>
                            <FooterStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:CheckBox ID="chkIsDone" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDISDONE").Equals(1))?true:false %>' Enabled="false" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkIsDoneEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDISDONE").Equals(1))?true:false %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:CheckBox ID="chkIsDoneAdd" runat="server" Enabled="false" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks" HeaderStyle-Width="235px">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" VerticalAlign="Top" Width="200px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtRemarksEdit" Width="100%" runat="server" Resize="Both" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>' CssClass="input_mandatory" TextMode="MultiLine" Rows="3"></telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtRemarksAdd" Width="100%" runat="server" Resize="Both" CssClass="input_mandatory" Rows="3" TextMode="MultiLine" Enabled="false"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reviewed By" HeaderStyle-Width="235px">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" VerticalAlign="Top" Width="150px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDoneBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDONEBY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="65px">
                            <ItemStyle Wrap="false" HorizontalAlign="Left" Width="80px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="40px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit"
                                    ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete"
                                    ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="UPDATE" ID="cmdSave" ToolTip="Save">
                                     <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="CANCEL" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="ADD" ID="cmdAdd" ToolTip="Add New">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
