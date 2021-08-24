<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionSealNumberRecording.aspx.cs"
    Inherits="InspectionSealNumberRecording" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript">
            function confirm(args) {
                if (args) {
                    __doPostBack("<%=confirm.UniqueID %>", "");
            }
        }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSealReq" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status ID="ucStatus" runat="server" />
                <asp:Button ID="confirm" runat="server" OnClick="btnConfirm_Click" />

                <eluc:TabStrip ID="MenuSealNumber" runat="server" OnTabStripCommand="MenuSealNumber_TabStripCommand"></eluc:TabStrip>

                <table width="100%" cellpadding="1" cellspacing="1">
                    <tr>
                        <td colspan="4">
                            <b><font color="blue" size="0">
                                <telerik:RadLabel ID="lblNote" runat="server" Text="Note: To issue the seals, you can either select
                                seals from the list or specify a range of seal numbers.">
                                </telerik:RadLabel>
                            </font></b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblFromSealNo" runat="server" Text="From Seal Number"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtFromSealNo" runat="server" CssClass="input" AutoPostBack="true" OnTextChanged="txtFromSealNo_Changed"></telerik:RadTextBox>
                            <%--<asp:DropDownList ID="ddlFromSealNo" runat="server" CssClass="input" AutoPostBack="true" OnTextChanged="ddlFromSealNo_Changed"
                                DataTextField="FLDSEALNO" DataValueField="FLDSEALID"></asp:DropDownList>--%>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblToSealNo" runat="server" Text="To Seal Number"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtToSealNo" runat="server" CssClass="input" AutoPostBack="true" OnTextChanged="txtToSealNo_Changed"></telerik:RadTextBox>
                            <%--<asp:DropDownList ID="ddlToSealNo" runat="server" CssClass="input" AutoPostBack="true" OnTextChanged="ddlToSealNo_Changed"
                                DataTextField="FLDSEALNO" DataValueField="FLDSEALID"></asp:DropDownList>--%>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblNoOfSealsSelected" runat="server" Text="No. of seals selected"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtSelectedSeals" runat="server" CssClass="input" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblRequestedQTY" runat="server" Text="Requested Qty"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtReqQty" runat="server" CssClass="input" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblIssuedQty" runat="server" Text="Issued Qty"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtIssuedQty" runat="server" CssClass="input" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblOfficeROB" runat="server" Text="Office ROB"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtROB" runat="server" CssClass="input" Width="50px" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <b>
                                <telerik:RadLabel ID="lblFilter" runat="server" Text="Filter"></telerik:RadLabel>
                            </b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblOfficeLocation" runat="server" Text="Office Location"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlOfficeLocation" AutoPostBack="true" runat="server" Width="200px"
                                CssClass="input" DataTextField="FLDLOCATIONNAME" DataValueField="FLDLOCATIONID" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select office location" />
                        </td>
                    </tr>
                </table>
                <br />

                <eluc:TabStrip ID="MenuSealExport" runat="server" OnTabStripCommand="MenuSealExport_TabStripCommand"></eluc:TabStrip>

                <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                    <%-- <asp:GridView ID="gvSealNumber" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" OnRowDataBound="gvSealNumber_RowDataBound" ShowHeader="true"
                    EnableViewState="false" AllowSorting="true" OnSorting="gvSealNumber_Sorting"
                    DataKeyNames="FLDSEALID">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvSealNumber" runat="server" Height="500px" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvSealNumber_NeedDataSource"
                        OnItemCommand="gvSealNumber_ItemCommand"
                        OnItemDataBound="gvSealNumber_ItemDataBound"
                        OnSortCommand="gvSealNumber_SortCommand"
                        GroupingEnabled="false" EnableHeaderContextMenu="true"
                        AutoGenerateColumns="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDSEALID">
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
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="false" HorizontalAlign="Center" Width="80px" />
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkAllSeal" runat="server" Text="Select All" AutoPostBack="true"
                                            OnPreRender="CheckAll" />
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="false" HorizontalAlign="Center" Width="80px" />
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelect" runat="server" EnableViewState="true" OnCheckedChanged="SaveCheckedValues" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lbLSealNoHeader" runat="server">Seal Number</asp:Label>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDSEALNO"] %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblStatusHeader" runat="server">Status</asp:Label>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDRECEIPTSTATUSNAME"]%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblOfficeLocation" runat="server">Office Location</asp:Label>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDLOCATIONNAME"]%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="415px" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>

                </div>
            </div>
            <%-- <eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="btnConfirm_Click" OKText="Yes"
            CancelText="No" />--%>
            <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
