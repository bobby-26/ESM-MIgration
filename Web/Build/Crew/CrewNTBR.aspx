<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewNTBR.aspx.cs" Inherits="CrewNTBR" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="UCNTBRReason" Src="~/UserControls/UserControlNTBRReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew NTBR Manager</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </telerik:RadCodeBlock>
    <style type="text/css">
        .scrolpan {
            overflow-y: auto;
            height: 80%;
        }

        .checkRtl {
            direction: rtl;
        }

        .fon {
            font-size: small !important;
        }
    </style>
</head>
<body>
    <form id="frmNTBRManager" runat="server">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <eluc:TabStrip ID="MenuNTBRManager" runat="server" OnTabStripCommand="CrewNTBRManager_TabStripCommand" Title="NTBR"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="90%" CssClass="scrolpan">
            <table cellpadding="1" cellspacing="1" width="99%">
                <tr>
                    <td style="width: 10%;">
                        <telerik:RadLabel ID="lblEmployeeCode" runat="server" Text="File No."></telerik:RadLabel>
                    </td>
                    <td style="width: 23%;">
                        <telerik:RadTextBox ID="txtEmployeeCode" runat="server" CssClass="readonlytextbox" ReadOnly="True" Enabled="false" Width="200px"></telerik:RadTextBox>
                    </td>
                    <td style="width: 10%;">
                        <telerik:RadLabel ID="lblEmployeename" runat="server" Text="Employee"></telerik:RadLabel>
                    </td>
                    <td style="width: 23%;">
                        <telerik:RadTextBox ID="txtEmployeeName" runat="server" MaxLength="50" CssClass="readonlytextbox"
                            ReadOnly="True" Enabled="false" Width="200px">
                        </telerik:RadTextBox>
                    </td>
                    <td style="width: 10%;">
                        <telerik:RadLabel ID="lblNationality" runat="server" Text="Nationality"></telerik:RadLabel>
                    </td>
                    <td style="width: 23%;">
                        <telerik:RadTextBox ID="txtNationality" runat="server" MaxLength="50" CssClass="readonlytextbox"
                            ReadOnly="True" Enabled="false" Width="200px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPresentRank" runat="server" Text="Present Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPresentRank" runat="server" MaxLength="20" CssClass="readonlytextbox"
                            ReadOnly="True" Enabled="false" Width="200px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLastVessel" runat="server" Text="Last Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtLastVessel" runat="server" MaxLength="20" CssClass="readonlytextbox"
                            ReadOnly="True" Enabled="false" Width="200px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSignedOff" runat="server" Text="Signed Off"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSignedOff" runat="server" MaxLength="20" CssClass="readonlytextbox"
                            ReadOnly="True" Enabled="false" Width="200px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <hr />
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSelect" runat="server" Text="Select"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rblPrincipalManager" runat="server" Direction="Horizontal"
                            AutoPostBack="true" OnSelectedIndexChanged="PrincipalManagerClick">
                            <Items>
                                <telerik:ButtonListItem Text="Manager" Value="1" Selected="true"></telerik:ButtonListItem>
                                <telerik:ButtonListItem Text="Principal" Value="2"></telerik:ButtonListItem>
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                     <td>
                        <telerik:RadLabel ID="lblNTBRDate" runat="server" Text="NTBR Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtNTBRDate" runat="server" CssClass="input_mandatory" />
                    </td>                   
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblManagerPrincipal" runat="server" Text="Principal/Manager"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:AddressType ID="ddlManager" runat="server" AddressType="126" AppendDataBoundItems="true"
                            CssClass="dropdown_mandatory" Width="200px" />
                        <div runat="server" visible="false" id="dvAddressType" class="input_mandatory" style="overflow: auto; width: 200px; height: 100px">
                            <telerik:RadListBox RenderMode="Lightweight" ID="cblAddressType" runat="server" CheckBoxes="true" ShowCheckAll="true" SelectionMode="Multiple">
                            </telerik:RadListBox>
                        </div>

                    </td>

                    <td>
                        <telerik:RadLabel ID="lblNTBRReason" runat="server" Text="NTBR Reason"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UCNTBRReason ID="ddlNTBRReason" runat="server" AppendDataBoundItems="true"
                            CssClass="dropdown_mandatory" Width="200px" NTBRReasonList="<%#PhoenixRegistersreasonsntbr.Listreasonsntbr()%>" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblNTBRRemarks" runat="server" Text="NTBR Remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtNTBRRemarks" runat="server" CssClass="input_mandatory" TextMode="MultiLine"
                            MaxLength="200" Width="200px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvNTBRManager" runat="server"
                AllowCustomPaging="false" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvNTBRManager_NeedDataSource" OnItemDataBound="gvNTBRManager_ItemDataBound"
                OnItemCommand="gvNTBRManager_ItemCommand" EnableHeaderContextMenu="true" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="NTBR" Name="NTBR" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="DE-NTBR" Name="DENTBR" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="S.No" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDNAME">
                            <HeaderStyle Width="5%" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNTBRid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNTBRID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSerialNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Principal/Manager" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDNAME">
                            <HeaderStyle Width="14%" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNTBRMgr" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNTBRID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkNTBRMgr" runat="server" CommandName="SELECT" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDNTBRID") %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDMANAGER") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reason" ColumnGroupName="NTBR">
                            <HeaderStyle Width="15%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNTBRReason" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREASON") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblNTBRReasonId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREASONID") %>'></telerik:RadLabel>
                                <eluc:UCNTBRReason ID="ddlNTBRReasonEdit" runat="server" NTBRReasonList="<%#PhoenixRegistersreasonsntbr.Listreasonsntbr()%>"
                                    SelectedNTBRMgrReason='<%# DataBinder.Eval(Container,"DataItem.FLDREASONID") %>'
                                    CssClass="dropdown_mandatory" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date" ColumnGroupName="NTBR">
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDNTBRDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="txtNTBRDateEdit" runat="server" MaxLength="10" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDNTBRDATE")) %>'></eluc:Date>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks" AllowSorting="false" ShowSortIcon="true" ColumnGroupName="NTBR">
                            <HeaderStyle Width="10%" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNTBRREMARKS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtNtbrRemarksEdit" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNTBRREMARKS") %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Created By" AllowSorting="false" ShowSortIcon="true" ColumnGroupName="NTBR">
                            <HeaderStyle Width="10%" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNTBRUser" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNTBRBYNAME") %>'></telerik:RadLabel>
                                <b>
                                    <telerik:RadLabel ID="RadLabel2" runat="server" Text="On : "></telerik:RadLabel>
                                </b>
                                <telerik:RadLabel ID="lblntbrDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText="Date" AllowSorting="false" ShowSortIcon="true" ColumnGroupName="DENTBR">
                            <HeaderStyle Width="10%" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDNTBRDateEdit" runat="server" Text=' <%#DataBinder.Eval(Container, "DataItem.FLDDENTBRDATE", "{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks" AllowSorting="false" ShowSortIcon="true" ColumnGroupName="DENTBR">
                            <HeaderStyle Width="10%" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDENtbrRemarksEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDENTBRREMARKS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Created By " AllowSorting="false" ShowSortIcon="true" ColumnGroupName="DENTBR">
                            <HeaderStyle Width="10%" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDENTBRUser" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDENTBRBYNAME") %>'></telerik:RadLabel>
                                <b>
                                    <telerik:RadLabel ID="RadLabel1" runat="server" Text="On : "></telerik:RadLabel>
                                </b>
                                <telerik:RadLabel ID="lblDENTBRDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMODIFIEDDATE", "{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action" UniqueName="Action">
                            <HeaderStyle Width="6%" HorizontalAlign="Center" />
                            <ItemStyle Wrap="false" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                    ToolTip="Edit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save"
                                    CommandName="UPDATE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                    ToolTip="Save">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel"
                                    CommandName="CANCEL" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                    ToolTip="Cancel">
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <%--<Virtualization EnableVirtualization="true" InitiallyCachedItemsCount="100" LoadingPanelID="RadAjaxLoadingPanel1" ItemsPerView="100" />--%>
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" />
                    <Scrolling AllowScroll="false" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
