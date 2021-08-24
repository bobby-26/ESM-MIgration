<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportsRetention.aspx.cs"
    Inherits="CrewReportsRetention" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPoolList.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                TabStrip="false"></eluc:TabStrip>
            <br />
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr style="color: black">
                    <td colspan="6" style="padding-left: 10px">  <font color="blue">Note :&nbsp;To view the Guidelines, put the mouse on the&nbsp;&nbsp;
                            <%--<img id="imgnotes" runat="server" src="<%$ PhoenixTheme:images/54.png %>" style="vertical-align: top;cursor: pointer" alt="NOTES" />--%>
                        <asp:LinkButton runat="server" ID="imgnotes">
                                <span class="icon"><i class="fas fa-list-alt-picklist"></i></span>
                        </asp:LinkButton>

                        &nbsp; button.</font>
                            <eluc:ToolTip ID="ucToolTipNW" runat="server" />
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td style="padding-left: 10px; padding-right: 10px">
                        <telerik:RadLabel ID="lblFromDate" runat="server" ForeColor="Black" Text="From"></telerik:RadLabel>
                    </td>
                    <td style="padding-right: 15px">
                        <eluc:Date ID="txtFromDate" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td style="padding-right: 10px">
                        <telerik:RadLabel ID="lblToDate" ForeColor="Black" runat="server" Text="To"></telerik:RadLabel>
                    </td>
                    <td style="padding-right: 15px">
                        <eluc:Date ID="txtToDate" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td style="padding-right: 10px">
                        <telerik:RadLabel ID="lblPool" ForeColor="Black" runat="server" Text="Pool"></telerik:RadLabel>
                    </td>
                    <td style="padding-right: 15px">
                        <eluc:Pool ID="lstPool" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" Width="240px" />
                    </td>
                    <td style="padding-right: 10px">
                        <telerik:RadLabel ID="lblShowRating" ForeColor="Black" runat="server" Text="Show Other Rank"></telerik:RadLabel>
                    </td>
                    <td style="padding-right: 15px">
                        <telerik:RadCheckBox ID="chkShowOther" runat="server" />
                    </td>
                    <td style="padding-right: 10px">
                        <telerik:RadLabel ID="lblGroup" ForeColor="Black" runat="server" Text="Group Rank"></telerik:RadLabel>
                    </td>
                    <td style="padding-right: 15px">
                        <telerik:RadCheckBox ID="chkGroup" runat="server" Checked="true" />
                    </td>

                </tr>
            </table>
            <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid ID="gvCrew" runat="server" AutoGenerateColumns="False" AllowPaging="true" AllowCustomPaging="true"
                CellPadding="3" RowStyle-Wrap="false" ShowHeader="true" EnableViewState="true" OnItemCommand="gvCrew_ItemCommand"
                OnItemDataBound="gvCrew_ItemDataBound" CellSpacing="0" GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnNeedDataSource="gvCrew_NeedDataSource" RenderMode="Lightweight" AllowSorting="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
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

                        <telerik:GridTemplateColumn HeaderText="S.No.">

                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblsno" Text="" />

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Rank">

                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblrank" Text="" />

                            </ItemTemplate>
                            <%-- <Itemtemplate>
                                   <%# DataBinder.Eval(Container,"DataItem.RANK") %>
                                </Itemtemplate>--%>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="BI">

                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.BI") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="EI">

                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.EI") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="AE">

                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.AE") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="S" Visible="false">

                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.S") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="UT" Visible="false">

                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.UT") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="BT" Visible="false">

                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.BT") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="RR">

                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.RR") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>

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
