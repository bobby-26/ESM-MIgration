<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportSupernumararyDetails.aspx.cs"
    Inherits="CrewReportSupernumararyDetails" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlVesselList" Src="../UserControls/UserControlVesselCommonCheckBoxList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlZoneList" Src="../UserControls/UserControlZoneList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PoolList" Src="~/UserControls/UserControlPoolList.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Supernumarary Details</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
            EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="tblConfigureCity"
            runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"
            Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="MenuReport" runat="server" OnTabStripCommand="MenuReport_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>

            <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                TabStrip="false"></eluc:TabStrip>

            <table border="1" style="border-collapse: collapse;">
                <tr>
                    <td width="25%">
                        <table valign="top">
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblFromDate" runat="server" Text="Details Between"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Date ID="ucfromdate" runat="server" CssClass="input_mandatory" />
                                    <eluc:Date ID="uctodate" runat="server" CssClass="input_mandatory" />
                                </td>
                            </tr>
                            <tr>

                                <td>

                                    <asp:Panel ID="pnlFormat" runat="server" GroupingText="Formats" Visible="false">
                                    <telerik:RadRadioButtonList ID="rblFormats" OnSelectedIndexChanged="rblFormats_SelectedIndexChanged"
                                        RepeatDirection="Horizontal" runat="server" AutoPostBack="true">
                                        <Items>
                                            <telerik:ButtonListItem Text="Format1"></telerik:ButtonListItem>
                                            <telerik:ButtonListItem Text="Format2"></telerik:ButtonListItem>
                                        </Items>
                                    </telerik:RadRadioButtonList>
                                     </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblPrincipal" runat="server" Text="Principal"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Principal ID="ucPrincipal" runat="server" AddressType="128" AppendDataBoundItems="true" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td width="90%">
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblVesselType" runat="server" Text="Vessel Type"></telerik:RadLabel>
                                    </BR>
                                                <eluc:VesselType ID="ucVesselType" runat="server" AppendDataBoundItems="true" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblPool" runat="server" Text="Pool"></telerik:RadLabel>
                                    </BR>
                                                <eluc:PoolList ID="ucPool" runat="server" AppendDataBoundItems="true" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                                    </BR>
                                                <eluc:UserControlVesselList ID="ucVessel" runat="server" VesselsOnly="true" ActiveVesselsOnly="true"
                                                    AppendDataBoundItems="true" Width="200px" Entitytype="VSL" AssignedVessel="true" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblZone" runat="server" Text="Zone"></telerik:RadLabel>
                                    </BR>
                                                <eluc:UserControlZoneList ID="ucZone" runat="server" AppendDataBoundItems="true" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvSupernumarary" runat="server"
                Height="65%" EnableViewState="false" AllowCustomPaging="true" AllowSorting="true"
                AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false" OnNeedDataSource="gvSupernumarary_NeedDataSource"
                EnableHeaderContextMenu="true" OnItemDataBound="gvSupernumarary_ItemDataBound"
                OnItemCommand="gvSupernumarary_ItemCommand" ShowFooter="False" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                    HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle Width="102px" />
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
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
                        <telerik:GridTemplateColumn HeaderText="S.No." AllowSorting="false" HeaderStyle-Width="25px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>
                                <center>
                                    <telerik:RadLabel ID="lblSno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>' />
                                </center>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Family's Name" AllowSorting="false" HeaderStyle-Width="50px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFamilyId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFAMILYID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblEmpNo" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>' />
                                <asp:LinkButton ID="lnkName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFAMILYNAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Family's DOB" AllowSorting="false" HeaderStyle-Width="38px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFamilydob" Visible="true" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDFAMILYDOB","{0:dd/MMM/yyyy}")) %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Relation" AllowSorting="false" HeaderStyle-Width="30px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRelation" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRELATION") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="File No" AllowSorting="false" HeaderStyle-Width="30px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFileno" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Seafarer's Name" AllowSorting="false" HeaderStyle-Width="50px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEmployeename" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEYNAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank" AllowSorting="false" HeaderStyle-Width="40px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRank" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Seafarer's DOB" AllowSorting="false" HeaderStyle-Width="35px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEmpdob" Visible="true" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDEMPDOB","{0:dd/MMM/yyyy}")) %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel" AllowSorting="false" HeaderStyle-Width="50px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVessel" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Signon Date" AllowSorting="false" HeaderStyle-Width="38px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEmpsignondate" Visible="true" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDFAMILYSIGNONDATE","{0:dd/MMM/yyyy}")) %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sign-on Port" AllowSorting="false" HeaderStyle-Width="50px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDFAMILYSIGNONPORT")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Signoff Date" AllowSorting="false" HeaderStyle-Width="30px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEmpsignoffdate" Visible="true" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDFAMILYSIGNOFFDATE","{0:dd/MMM/yyyy}")) %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sign-off Port" AllowSorting="false" HeaderStyle-Width="30px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDFAMILYSIGNOFFPORT")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sail Duration(Months)" AllowSorting="false" HeaderStyle-Width="30px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDSAILDURATION")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                    AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4"
                        EnableNextPrevFrozenColumns="true" ScrollHeight="415px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
