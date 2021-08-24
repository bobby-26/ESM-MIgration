<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionSealUsage.aspx.cs"
    Inherits="InspectionSealUsage" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Seal Usage List</title>
    <telerik:RadCodeBlock runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <%-- <script language="javascript" type="text/javascript">
            function OpenDropDownPopupEdit(ddlRemarks) {
                var ddlId = ddlRemarks.id;
                var lblId = ddlId.replace("ddlRemarksEdit", "lblIdEdit");
                var lblSealUsageId = document.getElementById(lblId);
                var sealusageid;
                if (lblSealUsageId) {
                    sealusageid = lblSealUsageId.innerHTML;
                }
                var remarksid = ddlRemarks.value;
                if (remarksid == '4' || remarksid == '5' || remarksid == '6') {
                    javascript: parent.Openpopup('codehelp1', '', '../Inspection/InspectionSealUsageRemarks.aspx?type=1&sealusageid=' + sealusageid + '&remarksid=' + remarksid, 'xlarge'); return false;
                }
            }
            function OpenDropDownPopupAdd(ddlRemarks) {
                var remarksid = ddlRemarks.value;
                if (remarksid == '4' || remarksid == '5' || remarksid == '6') {
                    javascript: parent.Openpopup('codehelp1', '', '../Inspection/InspectionSealUsageRemarks.aspx?type=1&remarksid=' + remarksid, 'xlarge'); return false;
                }
            }
        </script>--%>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server"></telerik:RadSkinManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div>
                <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

                <table width="100%">
                    <tr>
                        <td>
                            <font color="blue" size="0"><b>
                                <telerik:RadLabel ID="lblNote" runat="server" Text="Note: In Location dropdown, # indicates that they are seal points."></telerik:RadLabel>
                            </b>

                            </font>
                        </td>
                    </tr>
                </table>
                <div style="font-weight: 600; font-size: 12px;" runat="server">
                    <eluc:TabStrip ID="MenuSeal" runat="server" OnTabStripCommand="MenuSeal_TabStripCommand"></eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative;">
                    <%-- <asp:GridView ID="gvSealUsage" runat="server" AutoGenerateColumns="False" CellPadding="3"
                        Font-Size="11px" OnRowCommand="gvSealUsage_RowCommand" OnRowDataBound="gvSealUsage_RowDataBound"
                        AllowSorting="true" OnSorting="gvSealUsage_Sorting" OnRowCancelingEdit="gvSealUsage_RowCancelingEdit"
                        OnRowEditing="gvSealUsage_RowEditing" OnRowUpdating="gvSealUsage_RowUpdating" OnRowCreated="gvSealUsage_RowCreated"
                        EnableViewState="false" ShowFooter="true" ShowHeader="true" Width="100%">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>--%>

                    <telerik:RadGrid RenderMode="Lightweight" ID="gvSealUsage" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None"
                        GroupingEnabled="false" EnableHeaderContextMenu="true"
                        OnNeedDataSource="gvSealUsage_NeedDataSource"
                        OnItemDataBound="gvSealUsage_ItemDataBound"
                        OnItemCommand="gvSealUsage_ItemCommand"
                        OnSortCommand="gvSealUsage_SortCommand">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                        <MasterTableView ShowFooter="true" EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" TableLayout="Fixed" Height="10px">
                            <HeaderStyle Width="102px" />
                            <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                            <NoRecordsTemplate>
                                <table width="100%" border="0">
                                    <tr>
                                        <td align="center">
                                            <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </NoRecordsTemplate>
                            <Columns>
                                <telerik:GridButtonColumn CommandName="Edit" Text="DoubleClick" Visible="false" />
                                <telerik:GridTemplateColumn>
                                    <HeaderStyle Width="30px" />

                                    <ItemTemplate>
                                        <asp:Image ID="imgFlag" runat="server" Visible="false" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Location">
                                    <HeaderStyle Width="150px" />

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblSealUsageId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEALUSAGEID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblLocationName" runat="server" Width="90%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATIONNAME") %>'></telerik:RadLabel>

                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadLabel ID="lblSealUsageIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEALUSAGEID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblIdEdit" runat="server" Visible="false" CssClass="hidden" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEALUSAGEID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblLocationidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATIONID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblLocationNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATIONNAME") %>'></telerik:RadLabel>

                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadComboBox ID="ddlLocationAdd" runat="server" CssClass="input_mandatory" DataTextField="FLDLOCATIONNAME"
                                            Width="100%" DataValueField="FLDLOCATIONID" Filter="Contains" EmptyMessage="--Select--" MarkFirstMatch="true" />
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Seal Point">
                                    <HeaderStyle Width="100px" />

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblSealPoint" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEALPOINTNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Seal Number">
                                    <HeaderStyle Width="100px" />
                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblSealNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEALNO") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <span id="spnPickListSealNumberedit">
                                            <telerik:RadTextBox ID="txtSealNumberEdit" runat="server" Width="65%" Enabled="False" CssClass="input_mandatory"></telerik:RadTextBox>
                                            <asp:LinkButton ID="btnShowSealNumberEdit" runat="server"
                                                ImageAlign="Top" Text=".." CommandArgument="<%# Container.DataSetIndex %>">
                                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                                            </asp:LinkButton>
                                            <telerik:RadTextBox ID="txtSealidEdit" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                        </span>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <span id="spnPickListSealNumberAdd">
                                            <telerik:RadTextBox ID="txtSealNumberAdd" runat="server" Width="65%" Enabled="False" CssClass="input_mandatory"></telerik:RadTextBox>
                                            <asp:LinkButton ID="btnShowSealNumberAdd" runat="server"
                                                ImageAlign="Top" Text=".." CommandArgument="<%# Container.DataSetIndex %>"><span class="icon"><i class="fas fas fa-tasks"></i></span></asp:LinkButton>
                                            <telerik:RadTextBox ID="txtSealidAdd" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                        </span>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Seal Type">
                                    <HeaderStyle Width="100px" />
                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblSealType" runat="server" Width="95%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEALTYPENAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Seal Affixed by">
                                    <HeaderStyle Width="150px" />
                                    <ItemStyle HorizontalAlign="Left" Width="12%" />

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblSealAffixedby" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERSONAFFIXINGSEAL") %>'></telerik:RadLabel>
                                        -
                                    <telerik:RadLabel ID="lblRankname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <span id="spnSealAffixedByEdit">
                                            <telerik:RadTextBox ID="txtSealAffixedbyNameEdit" runat="server" CssClass="input_mandatory"
                                                Enabled="false" MaxLength="50" Width="40%">
                                            </telerik:RadTextBox>
                                            <telerik:RadTextBox ID="txtSealAffixedbyRankEdit" runat="server" CssClass="input_mandatory"
                                                Enabled="false" MaxLength="50" Width="40%">
                                            </telerik:RadTextBox>
                                            <asp:LinkButton runat="server" ID="btnSealAffixedbyEdit"
                                                ImageAlign="Top" Text=".." CommandArgument="<%# Container.DataSetIndex %>">
                                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                                            </asp:LinkButton>
                                            <telerik:RadTextBox ID="txtSealAffixedbyIdEdit" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                        </span>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <span id="spnSealAffixedByAdd">
                                            <telerik:RadTextBox ID="txtSealAffixedbyNameAdd" runat="server" CssClass="input_mandatory"
                                                Enabled="false" MaxLength="50" Width="40%">
                                            </telerik:RadTextBox>
                                            <telerik:RadTextBox ID="txtSealAffixedbyRankAdd" runat="server" CssClass="input_mandatory"
                                                Enabled="false" MaxLength="50" Width="40%">
                                            </telerik:RadTextBox>
                                            <asp:LinkButton runat="server" ID="btnSealAffixedbyAdd"
                                                ImageAlign="Top" Text=".." CommandArgument="<%# Container.DataSetIndex %>">
                                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                                            </asp:LinkButton>
                                            <telerik:RadTextBox ID="txtSealAffixedbyIdAdd" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                        </span>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Date Affixed">
                                    <HeaderStyle Width="150px" />
                                    <ItemStyle HorizontalAlign="Left" Wrap="False" Width="5%" />

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDateAffixed" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDATEAFFIXED")) %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Date ID="txtAffixedDateEdit" runat="server" CssClass="input_mandatory" DatePicker="true"
                                            Width="100%" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDATEAFFIXED")) %>' />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:Date ID="txtAffixedDateAdd" runat="server" CssClass="input_mandatory" DatePicker="true"
                                            Width="100%" />
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Reason">
                                    <HeaderStyle Width="100px" />
                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblReason" runat="server" Width="100%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREASONNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Quick ID="ucReasonEdit" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"
                                            QuickTypeCode="88" SelectedQuick='<%# DataBinder.Eval(Container,"DataItem.FLDREASON") %>' SelectedText=""
                                            AutoPostBack="true" OnTextChangedEvent="ucReasonEdit_Changed" Width="100%" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:Quick ID="ucReasonAdd" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"
                                            Width="100%" AutoPostBack="true" QuickTypeCode="88" OnTextChangedEvent="ucReasonAdd_Changed" />
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Replacement Reason">
                                    <HeaderStyle Width="100px" />
                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblRemarks" runat="server" Width="100%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSAGEREMARKSNAME") %>'></telerik:RadLabel>
                                        <eluc:ToolTip ID="ucToolTipUsageRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKSNAME") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadComboBox ID="ddlRemarksEdit" runat="server" AutoPostBack="true"
                                            Width="100%" OnTextChanged="ddlRemarksAdd_TextChanged" Filter="Contains" EmptyMessage="--Select--" MarkFirstMatch="true">
                                        </telerik:RadComboBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadComboBox ID="ddlRemarksAdd" runat="server" AutoPostBack="true"
                                            Width="100%" OnTextChanged="ddlRemarksAdd_TextChanged" Filter="Contains" EmptyMessage="--Select--" MarkFirstMatch="true">
                                        </telerik:RadComboBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn FooterStyle-HorizontalAlign="Center" HeaderText="Action">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                                    <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Edit"
                                            CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                            ToolTip="Edit">
                                         <span class="icon"><i class="fas fa-edit"></i></span>
                                        </asp:LinkButton>
                                        <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                        <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="History"
                                            CommandName="HISTORY" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdHistory"
                                            ToolTip="Show History">
                                        <span class="icon"><i class="fas fa-clipboard-list"></i></span>
                                        </asp:LinkButton>
                                        <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                        <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="ShowChange Remarks"
                                            CommandName="VIEWCHANGEREMARKS" CommandArgument="<%# Container.DataSetIndex %>"
                                            ID="cmdViewChangeRemarks" ToolTip="View/Change Replacement Remarks">
                                        <span class="icon"><i class="fas fa-comment-alt"></i></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Save"
                                            CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                            ToolTip="Save">
                                        <span class="icon"><i class="fas fa-save"></i></span>
                                        </asp:LinkButton>
                                        <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                        <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Cancel"
                                            CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                            ToolTip="Cancel">
                                         <span class="icon"><i class="fas fa-times-circle"></i></span>
                                        </asp:LinkButton>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Save"
                                            CommandName="Add" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
                                            ToolTip="Add New">
                                         <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                        </asp:LinkButton>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle AlwaysVisible="true" Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="380px" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>

                </div>

                <table cellpadding="1" cellspacing="1">
                    <tr>
                        <td>
                            <img id="Img3" src="<%$ PhoenixTheme:images/red-symbol.png%>" runat="server" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblOverDue" runat="server" Text=" * Replacement Overdue"></telerik:RadLabel>
                        </td>
                        <td>
                            <img id="Img2" src="<%$ PhoenixTheme:images/yellow-symbol.png%>" runat="server" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblDueWithin30days" runat="server" Text=" * Replacement Due within 30 days"></telerik:RadLabel>
                        </td>
                        <td>
                            <img id="Img5" src="<%$ PhoenixTheme:images/green-symbol.png%>" runat="server" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblDueWithin60Days" runat="server" Text=" * Replacement Due within 60 days"></telerik:RadLabel>
                        </td>
                    </tr>

                </table>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
