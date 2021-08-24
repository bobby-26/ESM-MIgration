<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceVesselSurveyCOCList.aspx.cs"
    Inherits="PlannedMaintenanceVesselSurveyCOCList" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Survey COC List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSurveyCOC" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvSurveyCOC,filterVessel" DecoratedControls="All" EnableRoundedCorners="true" />
            <table id="filterVessel">
                <tr>
                    <td colspan="2"></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblVessel" runat="server" Text="Vessel"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblVesselName" runat="server" Text="Vessel" CssClass="readonlytextbox"
                            Width="150px"></asp:Label>
                        <eluc:Vessel ID="ddlVessel" runat="server"  VesselsOnly="true" AutoPostBack="true" OnTextChangedEvent="cmdHiddenSubmit_Click"
                            ActiveVesselsOnly="true" SyncActiveVesselsOnly="true" Width="200px"  />
                    </td>
                </tr>
                <tr>
                    <td colspan="2"></td>
                </tr>
            </table>
            <div style="font-weight: 600; font-size: 12px;" runat="server">
                <eluc:TabStrip ID="MenuSurveyCOC" runat="server" OnTabStripCommand="MenuSurveyCOC_TabStripCommand"></eluc:TabStrip>
            </div>
            <telerik:RadGrid ID="gvSurveyCOC" runat="server" Height="85%" AutoGenerateColumns="False"
                Width="100%" CellPadding="3" AllowCustomPaging="true" AllowPaging="true" EnableHeaderContextMenu="true"
                OnRowCommand="gvSurveyCOC_RowCommand" OnItemDataBound="gvSurveyCOC_ItemDataBound"
                ShowHeader="true" EnableViewState="true" OnNeedDataSource="gvSurveyCOC_NeedDataSource">
                <SortingSettings SortedBackColor="#CCE5FF" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <ColumnGroups>
                        <telerik:GridColumnGroup Name="Completed" HeaderText="Completed" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Survey Type" UniqueName="Data" HeaderStyle-Width="12%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="12%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSurveyType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSURVEYTYPENAME") %>'></telerik:RadLabel>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Certificate" HeaderStyle-Width="20%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="20%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCertificates" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATENAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCertificateId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATEID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Due" UniqueName="DueDate" HeaderStyle-Width="10%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDueDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDUEDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="COC" HeaderStyle-Width="15%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="15%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCOC" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDITEM") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCertificateCOCId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATECOCID") %>'></telerik:RadLabel>

                                <telerik:RadLabel ID="lblDtkey" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsAtt" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDATTACHMENTYN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Description" HeaderStyle-Width="15%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="15%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                                <%--                                    <eluc:Tooltip ID="ucDescriptionToolTip" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDDESCRIPTIONTOOLTIP").ToString() %>' />--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="10%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSDESC") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblStatusCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date" ColumnGroupName="Completed" HeaderStyle-Width="8%">
                            <ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCompletedDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOMPLETEDDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks" ColumnGroupName="Completed" HeaderStyle-Width="13%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="15%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCompletedRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPLETEDREMARKS") %>'></telerik:RadLabel>
                                <%--                                    <eluc:Tooltip ID="ucCompletedRemarksToolTip" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCOMPLETEDREMARKSTOOLTIP").ToString() %>' />--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" UniqueName="Action" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" CommandName="COMPLETE"  ID="cmdComplete"
                                    ToolTip="Complete COC"><span class="icon"><i class="fas fa-tasks"></i></span></asp:LinkButton>
                                <asp:LinkButton runat="server" CommandName="SURVEYCOC" ID="cmdSvyAtt" ToolTip="Attachment">
                                    <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" FrozenColumnsCount="2" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>





    </form>
</body>
</html>
