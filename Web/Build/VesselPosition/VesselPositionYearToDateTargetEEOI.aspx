<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionYearToDateTargetEEOI.aspx.cs"
    Inherits="VesselPositionYearToDateTargetEEOI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Target EEOI</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmNoonReportRangeConfig" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
          <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlYeartodatequaterreport" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel runat="server" ID="pnlYeartodatequaterreport" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:Status runat="server" ID="ucStatus" />
            <table id="tblSearch" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselName" runat="server" Text="Vessel Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="UcVessel" runat="server" CssClass="dropdown_mandatory" AutoPostBack="true" SyncActiveVesselsOnly="True" AssignedVessels="true"
                            VesselsOnly="true" AppendDataBoundItems="true" OnTextChangedEvent="UcVessel_TextChangedEvent" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblYear" runat="server" Text="Year"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlYear" runat="server" CssClass="input" AutoPostBack="true" OnTextChanged="ddlYear_TextChanged">
                        </telerik:RadComboBox>
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenuYeartodatequaterreport" runat="server" OnTabStripCommand="MenuNRRangeConfig_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid ID="gvMenuYeartodatequaterreport" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" Height="84%" CellPadding="3" OnItemCommand="gvMenuYeartodatequaterreport_RowCommand" OnItemDataBound="gvMenuYeartodatequaterreport_ItemDataBound"
                OnUpdateCommand="gvMenuYeartodatequaterreport_RowUpdating" OnNeedDataSource="gvMenuYeartodatequaterreport_NeedDataSource" AllowPaging="true" AllowCustomPaging="true"
                ShowFooter="true" ShowHeader="true" EnableViewState="true" AllowSorting="False" EnableHeaderContextMenu="true" GroupingEnabled="false">

                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDTARGETEEOIID">
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
                    <telerik:GridTemplateColumn HeaderText="Vessel Name" HeaderStyle-Width="40%" HeaderStyle-HorizontalAlign="Left">
                        <ItemStyle Wrap="True" HorizontalAlign="Left" Width="40%"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Year" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="20%">
                        <ItemStyle Wrap="True" HorizontalAlign="center" Width="20%"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblYear" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEEOIYEAR") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblTargetEEOIId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTARGETEEOIID") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadComboBox ID="ddlYearEdit" runat="server" CssClass="input" AutoPostBack="true" Width="90px"></telerik:RadComboBox>
                            <telerik:RadLabel ID="lblTargetEEOIIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTARGETEEOIID") %>'></telerik:RadLabel>
                        </EditItemTemplate>
                        <FooterStyle HorizontalAlign="Center" />
                        <FooterTemplate>
                            <telerik:RadComboBox ID="ddlYear" runat="server" CssClass="input" AutoPostBack="true" Width="90px"></telerik:RadComboBox>
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Target EEOI" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="20%">
                        <ItemStyle Wrap="True" HorizontalAlign="right" Width="20%"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblTargetEEOI" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTARGETEEOI") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Number id="txtTargetEEOIEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTARGETEEOI") %>' CssClass="gridinput" ToolTip="Enter EEOI" Width="90px"  />
                        </EditItemTemplate>
                        <FooterStyle HorizontalAlign="Right" />
                        <FooterTemplate>
                            <eluc:Number id="txtTargetEEOI" runat="server"  CssClass="gridinput" ToolTip="Enter EEOI" Width="90px"  />
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="20%">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="20%"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Save" CommandName="UPDATE" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                        </EditItemTemplate>
                        <FooterStyle HorizontalAlign="Center" />
                        <FooterTemplate>
                            <asp:LinkButton runat="server" AlternateText="Add" CommandName="Add" ID="cmdAdd" ToolTip="Add New">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
