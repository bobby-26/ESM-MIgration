<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelQuotationAgentDetail.aspx.cs"
    Inherits="CrewTravelQuotationAgentDetail" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmMessage" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Agent Details </title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <script type="text/javascript">
            function resize() {
                var obj = document.getElementById("ifMoreInfo");
                obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight) - 250 + "px";

            }
        </script>
        <style>
            .tooltipHeight {
                height: 130px;
                padding: 0;
            }
        </style>

    </telerik:RadCodeBlock>
</head>
<body onload="resize()" onresize="resize()">
    <form id="frmCrewTravelQuotationAgentDetail" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="88%">
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:ConfirmMessage ID="ucConfirm" runat="server" Text="" Visible="false" />
            <eluc:TabStrip ID="MenuAgent" runat="server" TabStrip="true" OnTabStripCommand="MenuAgent_TabStripCommand"></eluc:TabStrip>
            <eluc:TabStrip ID="Menuapprove" runat="server" OnTabStripCommand="Menuapprove_TabStripCommand" TabStrip="true" Title=""></eluc:TabStrip>
            <telerik:RadLabel ID="lblTitle" runat="server"></telerik:RadLabel>
            <table style="color: Blue">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblToviewtheGuidelinesplacethemouseonthe" runat="server" Text="To view the Guidelines, place the mouse on the"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadToolTip RenderMode="Lightweight" ID="btnTooltipHelp" runat="server" CssClass="tooltipHeight" TargetControlID="btnHelp" HideDelay="10000">
                        </telerik:RadToolTip>
                        <asp:LinkButton runat="server" AlternateText="Guide" ID="btnHelp" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-info-circle"></i></span>
                        </asp:LinkButton>
                    </td>
                </tr>
            </table>
            <b>&nbsp;<telerik:RadLabel ID="lblTravelAgents" runat="server" Text="Travel Agents"></telerik:RadLabel>
            </b>

            <eluc:TabStrip ID="MenuAgentList" runat="server" OnTabStripCommand="MenuAgentList_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvAgent" runat="server" AllowCustomPaging="true" AllowSorting="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvAgent_ItemCommand" OnNeedDataSource="gvAgent_NeedDataSource" AllowPaging="true"
                OnItemDataBound="gvAgent_ItemDataBound" EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderStyle-Width="50px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblImageHeader" runat="server"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadCheckBox ID="chkSelect" Checked="true" runat="server" EnableViewState="true" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="250px" HeaderText="Agent">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTravelID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblTravelAgentId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELAGENTID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblAgentID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGENTID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <asp:LinkButton ID="lnkAgentName" runat="server" CommandName="SELECT"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGENTNAME") + " - " + DataBinder.Eval(Container,"DataItem.FLDAGENTCODE")%> '></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="70px" HeaderText="Sent Date">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSendDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDSENDDATE", "{0:dd/MMM/yyyy}")) +" "+ DataBinder.Eval(Container, "DataItem.FLDSENDDATE", "{0:t}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="70px" HeaderText="Received Date">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRecievedDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDRECEIVEDDATE", "{0:dd/MMM/yyyy}"))  +" "+ DataBinder.Eval(Container, "DataItem.FLDRECEIVEDDATE", "{0:t}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="50px" HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete"
                                    CommandArgument="<%# Container.DataSetIndex %>" ToolTip="Delete" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Communication" CommandName="Communication" ID="cmdCommunication"
                                    CommandArgument="<%# Container.DataSetIndex %>" ToolTip="Chat" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-comments"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="3" EnableColumnClientFreeze="true"
                        ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <hr />
            <iframe runat="server" id="ifMoreInfo" style="width: 100%" scrolling="auto" frameborder="0"></iframe>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
