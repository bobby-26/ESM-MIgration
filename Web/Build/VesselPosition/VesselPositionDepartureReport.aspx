<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionDepartureReport.aspx.cs"
    Inherits="VesselPositionDepartureReport" %>

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
    <title>Departure List</title>
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
    <form id="frmDepartureeList" runat="server" submitdisabledcontrols="true">
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
            <eluc:TabStrip ID="MenuDepartureeList" runat="server" OnTabStripCommand="DepartureeList_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid ID="gvCourseList" runat="server" AutoGenerateColumns="False" Font-Size="11px" GridLines="None"
                Width="100%" CellPadding="3" OnItemCommand ="gvCourseList_RowCommand" OnItemDataBound ="gvCourseList_ItemDataBound"
                AllowSorting="false" OnNeedDataSource="gvCourseList_NeedDataSource" AllowPaging="true" AllowCustomPaging="true" EnableHeaderContextMenu="true" GroupingEnabled="false"
                 ShowFooter="false" ShowHeader="true" EnableViewState="false" >

                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDVESSELDEPARTUREID">
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
                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Vessel Name">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="SentYN" CommandName="SENTYN" ID="ImgSentYN" ToolTip="Report Not Sent to Office">
                                <span class="icon" style="color:orangered;" ><i class="fas fa-exclamation"></i></span>
                            </asp:LinkButton>
                            <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Port">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkPort" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItem %>'
                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Departure Date" HeaderStyle-Width="70px">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"  Width="70px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDepartureDate" runat="server" Width="120px" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDVESSELDEPARTUREDATE")) + " " + DataBinder.Eval(Container,"DataItem.FLDVESSELDEPARTUREDATE", "{0:HH:mm}") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblVesselDepartureID" runat="server" Width="120px" Visible="false"
                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELDEPARTUREID") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Next Port">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblNextPort" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEXTPORTNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="ETA" HeaderStyle-Width="70px">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="70px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblETA" runat="server"  Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDETA")) + " " + DataBinder.Eval(Container,"DataItem.FLDETA", "{0:HH:mm}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Cargo Operation" HeaderStyle-Width="80px">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="80px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblCargoOperation" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARGOOPERATION") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="STS YN" HeaderStyle-Width="50px">
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadCheckBox runat="server" ID="chksts" Enabled="false" Checked='<%# DataBinder.Eval(Container,"DataItem.FLDSTSOPERATION").ToString() == "1"? true : false %>'></telerik:RadCheckBox>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="80px">
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="80px"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Edit" CommandName="REVIEW" ID="cmdReview" ToolTip="Review" Visible="false">
                                <span class="icon"><i class="fas fa-user-check-approved"></i></span>
                                </asp:LinkButton>
                           <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="EU Voyage" CommandName="EUVOYAGE" ID="cmdEUVoyage" ToolTip="Generate EU Voyage">
                                <span class="icon"><i class="fas fa-copy"></i></span>
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
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
