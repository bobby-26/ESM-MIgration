<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsCaptainCash.aspx.cs" Inherits="AccountsCaptainCash" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStripTelerik" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselCrew" Src="~/UserControls/UserControlVesselEmployee.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register Src="~/UserControls/UserControlVessel.ascx" TagName="Vessel" TagPrefix="eluc" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Office Captain Cash</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
         <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>

        <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" Height="94%">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text="" Visible="false"></eluc:Status>

            <eluc:TabStrip ID="MenuCaptainCashMain" runat="server" OnTabStripCommand="MenuCaptainCashMain_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>

            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselName" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ddlVessel" runat="server" AppendDataBoundItems="true" VesselsOnly="true" AutoPostBack="true"
                            CssClass="input" OnTextChangedEvent="FilterChanged" Width="180px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>

                        <telerik:RadComboBox ID="ddlStatus" runat="server" CssClass="input" AutoPostBack="true" Width="180px"
                            OnTextChanged="FilterChanged">
                            <Items>
                                <telerik:RadComboBoxItem Value="1" Text="Finalized by Vessel"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="0" Text="Confirmed by Office"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="2" Text="All"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVoucherNumber" runat="server" Text="Voucher Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtVoucherNo" MaxLength="50" CssClass="input"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblToDate" runat="server" Text="To Date Between"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtFromDate" runat="server" CssClass="input" />
                        <eluc:Date ID="txtToDate" runat="server" CssClass="input" />

                    </td>
                </tr>
            </table>


            <eluc:TabStrip ID="MenuCaptainCash" runat="server" OnTabStripCommand="MenuCaptainCash_TabStripCommand"></eluc:TabStrip>

            <telerik:RadCheckBox ID="chkDefaultFilter" runat="server" AutoPostBack="true" Visible="false" />

            <telerik:RadGrid RenderMode="Lightweight" ID="gvCaptainCash" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvCaptainCash_NeedDataSource" AllowMultiRowSelection="true" FilterType="CheckList"
                EnableViewState="false" EnableHeaderContextMenu="true" Width="100%" Height="81%"
                OnItemDataBound="gvCaptainCash_ItemDataBound" OnItemCommand="gvCaptainCash_ItemCommand"
                ShowFooter="false" ShowHeader="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDBALANCEID" TableLayout="Fixed">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />

                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Vessel Name">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBalanceId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDBALANCEID"]%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblAccountId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDACCOUNTID"]%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDVESSELID"]%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblConfirmFlag" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDCONFIRMFLAG"]%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDate" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDTODATE"]%>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkVesselName" runat="server" CommandName="SELECT" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME")  %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel Account Code">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="25%" />
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDVESSELACCOUNTCODE"]%>
                            </ItemTemplate>
                            <EditItemTemplate>

                                <telerik:RadComboBox ID="ddlAccountDetails" runat="server" CssClass="dropdown_mandatory" AutoPostBack="true" Width="180px"
                                    DataTextField="FLDVESSELACCOUNTNAME" DataValueField="FLDACCOUNTID">
                                </telerik:RadComboBox>

                                <telerik:RadLabel ID="lblEditVesselId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDVESSELID"]%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblEditAccountId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDACCOUNTID"]%>'></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="From">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblfrom" runat="server" Text='<%# string.Format("{0:dd/MMM/yyyy}", ((DataRowView)Container.DataItem)["FLDFROMDATE"])%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="To">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTo" runat="server" Text='<%# string.Format("{0:dd/MMM/yyyy}", ((DataRowView)Container.DataItem)["FLDTODATE"])%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Voucher Number">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="15%" />
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDVOUCHERNUMBER"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Posted On">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <%# string.Format("{0:dd/MMM/yyyy}", ((DataRowView)Container.DataItem)["FLDPOSEDDATE"])%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">
                                    Action
                                </telerik:RadLabel>
                            </HeaderTemplate><HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Confirm Captain Cash" CommandName="CONFIRM" ID="cmdConfirm" ToolTip="Confirm">
                                    <span class="icon"><i class="fas fa-clipboard-check"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Posting Draft View" CommandName="DRAFT" ID="cmdDraft" ToolTip="Posting Draft View">
                                    <span class="icon"><i class="fas fa-file-alt"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-times"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>

                            </EditItemTemplate>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                    EnablePostBackOnRowClick="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="2" EnableNextPrevFrozenColumns="true" ScrollHeight="415px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>



<%--    <asp:GridView ID="gvCaptainCash" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" OnRowDataBound="gvCaptainCash_RowDataBound" OnRowDeleting="gvCaptainCash_RowDeleting" ShowHeader="true"
                            EnableViewState="false" AllowSorting="true" OnSorting="gvCaptainCash_Sorting" OnSelectedIndexChanging="gvCaptainCash_SelectedIndexChanging"
                            OnRowEditing="gvCaptainCash_RowEditing" OnRowCommand="gvCaptainCash_RowCommand"
                            OnRowCancelingEdit="gvCaptainCash_RowCancelingEdit" OnRowUpdating="gvCaptainCash_RowUpdating"
                            DataKeyNames="FLDBALANCEID">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <RowStyle Height="10px" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblVesselName" runat="server" Text="Vessel Name"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblBalanceId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDBALANCEID"]%>'></asp:Label>
                                        <asp:Label ID="lblAccountId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDACCOUNTID"]%>'></asp:Label>
                                        <asp:Label ID="lblVesselId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDVESSELID"]%>'></asp:Label>
                                        <asp:Label ID="lblConfirmFlag" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDCONFIRMFLAG"]%>'></asp:Label>
                                        <asp:Label ID="lblDate" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDTODATE"]%>'></asp:Label>
                                        <asp:LinkButton ID="lnkVesselName" runat="server" CommandName="SELECT" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME")  %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblVesselAccountCode" runat="server" Text="Vessel Account Code"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDVESSELACCOUNTCODE"]%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlAccountDetails" runat="server" CssClass="dropdown_mandatory"
                                            DataTextField="FLDVESSELACCOUNTNAME" DataValueField="FLDACCOUNTID">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblEditVesselId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDVESSELID"]%>'></asp:Label>
                                        <asp:Label ID="lblEditAccountId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDACCOUNTID"]%>'></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblFromDate" runat="server" Text="From Date"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                          <asp:Label ID="lblfrom" runat="server"  Text='<%# string.Format("{0:dd/MMM/yyyy}", ((DataRowView)Container.DataItem)["FLDFROMDATE"])%>' ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblToDate" runat="server" Text="To Date"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                          <asp:Label ID="lblTo" runat="server"  Text='<%# string.Format("{0:dd/MMM/yyyy}", ((DataRowView)Container.DataItem)["FLDTODATE"])%>' ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblVoucherNumber" runat="server" Text="Voucher Number"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDVOUCHERNUMBER"]%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblPostedDate" runat="server" Text="Posted Date"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# string.Format("{0:dd/MMM/yyyy}", ((DataRowView)Container.DataItem)["FLDPOSEDDATE"])%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField >
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                                        </asp:Label>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                            CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                            ToolTip="Edit"></asp:ImageButton>
                                        <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                       <%-- <asp:ImageButton runat="server" AlternateText="View Captain Cash" ImageUrl="<%$ PhoenixTheme:images/view-task.png %>"
                                            CommandName="VIEW" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdView"
                                            ToolTip="View Captain Cash"></asp:ImageButton>
                                        <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />--%>
<%--  <asp:ImageButton runat="server" AlternateText="Confirm Captain Cash" ImageUrl="<%$ PhoenixTheme:images/approve.png %>"
                                            CommandName="CONFIRM" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdConfirm"
                                            ToolTip="Confirm"></asp:ImageButton>
                                        <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Posting Draft View" ImageUrl="<%$ PhoenixTheme:images/document_view.png %>"
                                            CommandName="DRAFT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDraft"
                                            ToolTip="Posting Draft View"></asp:ImageButton>
                                        <img id="Img4" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                            ToolTip="Delete"></asp:ImageButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                            CommandName="Update" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                            ToolTip="Save"></asp:ImageButton>
                                        <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                            ToolTip="Cancel"></asp:ImageButton>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div id="divPage" style="position: relative;">
                        <table width="100%" border="0" cellpadding="1" cellspacing="1" class="datagrid_pagestyle">
                            <tr>
                                <td nowrap align="center">
                                    <asp:Label ID="lblPagenumber" runat="server">
                                    </asp:Label>
                                    <asp:Label ID="lblPages" runat="server">
                                    </asp:Label>
                                    <asp:Label ID="lblRecords" runat="server">
                                    </asp:Label>&nbsp;&nbsp;
                                </td>
                                <td nowrap align="left" width="50px">
                                    <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                                </td>
                                <td width="20px">&nbsp;
                                </td>
                                <td nowrap align="right" width="50px">
                                    <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                                </td>
                                <td nowrap align="center">
                                    <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                    </asp:TextBox>
                                    <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                        Width="40px"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>--%>
