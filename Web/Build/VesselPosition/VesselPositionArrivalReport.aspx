<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionArrivalReport.aspx.cs" Inherits="VesselPositionArrivalReport" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Voyage" Src="~/UserControls/UserControlVoyage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Arrival Report</title>
    <telerik:RadCodeBlock ID="Radcodeblock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
           function Resize() {

                   TelerikGridResize($find("<%= gvCourseList.ClientID %>"));
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewCourseList" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlCourseListEntry" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />

        <telerik:RadAjaxPanel runat="server" ID="pnlCourseListEntry">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text=""></eluc:Status>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

            <eluc:TabStrip ID="MenuCrewCourseList" runat="server" OnTabStripCommand="CrewCourseList_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid ID="gvCourseList" runat="server" AutoGenerateColumns="False" Font-Size="11px" GridLines="None"
                Width="100%" CellPadding="3" OnItemCommand="gvCourseList_RowCommand" OnItemDataBound="gvCourseList_ItemDataBound"
                AllowSorting="false" ShowFooter="false" ShowHeader="true" OnNeedDataSource="gvCourseList_NeedDataSource"
                EnableViewState="false" AllowCustomPaging="true" AllowPaging="true" EnableHeaderContextMenu="true" GroupingEnabled="false">

                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDVESSELARRIVALID">
                    <HeaderStyle Width="102px" />
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Left" HeaderText="Vessel Name">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="SentYN" CommandName="SENTYN" ID="ImgSentYN" Visible="false" ToolTip="Report Not Sent to Office">
                                <span class="icon" style="color:orangered;" ><i class="fas fa-exclamation"></i></span>
                                </asp:LinkButton>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Port" HeaderStyle-HorizontalAlign="Left">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPort" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItem %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Arrival Date" HeaderStyle-HorizontalAlign="Left">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblArrivalDate" runat="server" Width="120px" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATE")) + " " + DataBinder.Eval(Container,"DataItem.FLDDATE", "{0:HH:mm}") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselArrivalID" runat="server" Width="120px" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELARRIVALID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblvesselid" runat="server" Width="120px" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="ETB" HeaderStyle-HorizontalAlign="Left">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblETB" runat="server" Width="120px" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDETB")) + " " + DataBinder.Eval(Container,"DataItem.FLDETB", "{0:HH:mm}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="ETD" HeaderStyle-HorizontalAlign="Left">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="80px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblETD" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDETD")) + " " + DataBinder.Eval(Container,"DataItem.FLDETD", "{0:HH:mm}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Logged Distance" HeaderStyle-HorizontalAlign="Left" Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="80px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLoggedDistance" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOGGEDDISTANCE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Distance Observed" HeaderStyle-HorizontalAlign="Left">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="80px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDistanceObserved" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOBSERBEDDISTANCE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Distance Factor" HeaderStyle-HorizontalAlign="Left" Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="80px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDistanceFactor" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISTANCEFACTOR") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="120px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="REVIEW" ID="cmdReview" ToolTip="Review" Visible="false">
                                <span class="icon"><i class="fas fa-user-check-approved"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="PASSAGE" ID="cmdpassage" ToolTip="Calculate Passage Summary">
                                <span class="icon"><i class="fas fa-copy"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <%--        <Triggers>
            <asp:PostBackTrigger ControlID="gvCourseList" />
        </Triggers>--%>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
