<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportMISPromotedSeafarersAnalysis.aspx.cs"
    Inherits="CrewReportMISPromotedSeafarersAnalysis" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZoneList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationalityList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPoolList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BatchList" Src="~/UserControls/UserControlPreSeaBatchList.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Promoted Seafarers Analysis</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvCrew.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
        <style type="text/css">
            .mlabel {
                color: blue !important;                
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                TabStrip="false"></eluc:TabStrip>

            <table width="100%">
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblheader1" runat="server" CssClass="mlabel" Text="   Description: Promotions of all Seafarers Onboard and Onleave are reported"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblheader2" runat="server" CssClass="mlabel" Text="   Note: The Period filter is for the 'Promoted On' date"></telerik:RadLabel>
                    </td>
                </tr>

                <tr>
                    <td colspan="2">
                        <asp:Panel ID="pnlDate" runat="server" GroupingText="Period">
                            <table>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ForeColor="Black" ID="lblFrom" runat="server" Text="From"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Date ID="ucDate" runat="server" CssClass="input_mandatory" Width="120px" />
                                    </td>
                                    <td>
                                        <telerik:RadLabel ForeColor="Black" ID="lblTo" runat="server" Text="To"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Date ID="ucDate1" runat="server" CssClass="input_mandatory" Width="120px" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                    <td>
                        <telerik:RadLabel ForeColor="Black" ID="lblpromotedfromrank" runat="server" Text="Promoted From Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Rank ID="ucRankFrom" runat="server" AppendDataBoundItems="true" Width="210px" />
                    </td>
                    <td>
                        <telerik:RadLabel ForeColor="Black" ID="lblPromotedToRank" runat="server" Text="Promoted To Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Rank ID="ucRankTo" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" Width="250px" />
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ForeColor="Black" ID="lblZone" runat="server" Text="Zone"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Zone ID="ucZone" runat="server" AppendDataBoundItems="true" Width="210px" />
                    </td>
                    <td>
                        <telerik:RadLabel ForeColor="Black" ID="lblNationality" runat="server" Text="Nationality"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Nationality ID="ucNationality" runat="server" AppendDataBoundItems="true" Width="210px" />
                    </td>
                    <td>
                        <telerik:RadLabel ForeColor="Black" ID="lblBatch" runat="server" Text="Batch"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:BatchList ID="ucBatchList" AppendDataBoundItems="true" runat="server" Width="250px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ForeColor="Black" ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadListBox ID="lstStatus" runat="server" SelectionMode="Multiple" Height="70px"
                            DataTextField="FLDHARDNAME" DataValueField="FLDHARDCODE" Width="210px">
                        </telerik:RadListBox>
                    </td>
                    <td>
                        <telerik:RadLabel ForeColor="Black" ID="lblPool" runat="server" Text="Pool"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Pool ID="ucPool" runat="server" AppendDataBoundItems="true" Width="210px" />
                    </td>
                    <td colspan="2">
                        <telerik:RadCheckBox ID="chkCompanyExp" runat="server" Text="Include Seafarers not promoted through Phoenix" />
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid ID="gvCrew" runat="server" AutoGenerateColumns="False" OnSortCommand="gvCrew_Sorting"
                CellPadding="3" ShowHeader="true" EnableViewState="false" OnItemCommand="gvCrew_ItemCommand"
                OnItemDataBound="gvCrew_ItemDataBound" CellSpacing="0" GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnNeedDataSource="gvCrew_NeedDataSource" RenderMode="Lightweight" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Auto">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ForeColor="Black" ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>

                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="S.No">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSlNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="File No">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEmpFileNo" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Batch">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBatch" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCH") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="true" SortExpression="FLDEMPLOYEENAME">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEmpNo" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>' />
                                <asp:LinkButton ID="lnkName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Promoted On" HeaderStyle-Width="9%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblpromotedOn" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROMOTIONDATE","{0:dd/MMM/yyyy}") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Rank">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFrom" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFROMRANKNAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="To">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTo" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTORANKNAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Vessel On">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselOn" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENTVESSELNAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Sign On Date">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDCURRENTSIGNONDATE", "{0:dd/MMM/yyyy}")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Last Vessel">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLastVessel" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTVESSELNAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Sign Off Date">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDLASTSIGNOFFDATE", "{0:dd/MMM/yyyy}")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Promoted Onboard">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPromotedonbd" Visible="true" runat="server" Text='<%# Convert.ToString(Eval("ISPROMOTEDONBOARD").ToString() == "1" ? "Yes":"No") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>

                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <eluc:Status runat="server" ID="ucStatus" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
