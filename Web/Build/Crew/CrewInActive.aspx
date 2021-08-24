<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewInActive.aspx.cs" Inherits="CrewInActive" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlPool" Src="~/UserControls/UserControlPool.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew In-Active</title>
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
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmInActive" DecoratedControls="All" EnableRoundedCorners="true" />
    <form id="frmInActive" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <eluc:TabStrip ID="MenuInActive" runat="server" OnTabStripCommand="CrewInActive_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="90%" CssClass="scrolpan">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td style="width: 13%;">
                        <telerik:RadLabel ID="lblfileno" runat="server" Text="File No."></telerik:RadLabel>
                    </td>
                    <td style="width: 20%;">
                        <telerik:RadTextBox ID="txtEmployeeCode" runat="server" CssClass="readonlytextbox" ReadOnly="True" Enabled="false" Width="180px"></telerik:RadTextBox>
                    </td>
                    <td style="width: 13%;">
                        <telerik:RadLabel ID="lblempname" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td style="width: 20%;">
                        <telerik:RadTextBox ID="txtEmployeeName" runat="server" MaxLength="50" CssClass="readonlytextbox"
                            ReadOnly="True" Enabled="false" Width="180px">
                        </telerik:RadTextBox>
                    </td>
                    <td style="width: 13%;">
                        <telerik:RadLabel ID="lblpresentrank" runat="server" Text="Present Rank"></telerik:RadLabel>
                    </td>
                    <td style="width: 20%;">
                        <telerik:RadTextBox ID="txtPresentRank" runat="server" MaxLength="20" CssClass="readonlytextbox"
                            ReadOnly="True" Enabled="false" Width="180px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLVessel" runat="server" Text="Last Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtLastVessel" runat="server" MaxLength="20" CssClass="readonlytextbox"
                            ReadOnly="True" Enabled="false" Width="180px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSignedOff" runat="server" Text="Signed Off"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSignedOff" runat="server" MaxLength="20" CssClass="readonlytextbox"
                            ReadOnly="True" Enabled="false" Width="180px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDOA" runat="server" Text="DOA"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtDOA" CssClass="readonlytextbox" ReadOnly="True" Enabled="false" Width="180px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="InActiveReason" runat="server" Text="In-Active Reason"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ddlInactiveReason" runat="server" AppendDataBoundItems="true" HardTypeCode="53"
                            ShortNameFilter="LFT,DTH,EXM,TSP,TTO" CssClass="dropdown_mandatory" Width="180px" />
                    </td>
                    <td>
                        <asp:Literal ID="ltInactiveCategory" runat="server" Text="In-Active Category"></asp:Literal>
                    </td>
                    <td>
                        <telerik:RadComboBox DropDownPosition="Static" CssClass="input" ID="ddlInactiveCategory" runat="server" EnableLoadOnDemand="True"
                            EmptyMessage="Type to select Category" Filter="Contains" AppendDataBoundItems="true" Width="180px"
                            AutoPostBack="true" MarkFirstMatch="true" OnSelectedIndexChanged="ddlInactiveCategory_SelectedIndexChanged">
                        </telerik:RadComboBox>

                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPool" runat="server" Text="Pool"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlPool ID="ucPool" runat="server" CssClass="input" Enabled="false" AppendDataBoundItems="true" Width="180px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblInAcivedate" runat="server" Text="In-Active Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtInActiveDate" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAcivedate" runat="server" Text="Active Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtActiveDate" runat="server" CssClass="input_mandatory" AutoPostBack="True"
                            ontextchanged="txtActiveDate_TextChanged" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblIActiveRemarks" runat="server" Text="In-Active Remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtInActiveRemarks" runat="server" CssClass="input_mandatory" TextMode="MultiLine"
                            MaxLength="800" Width="180px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblActiveRemarks" runat="server" Text="Active Remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtActiveRemarks" runat="server" CssClass="input_mandatory" TextMode="MultiLine"
                            MaxLength="800" Width="180px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewInActive" runat="server"
                AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvCrewInActive_NeedDataSource" OnItemCommand="gvCrewInActive_ItemCommand" EnableHeaderContextMenu="true" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="In-Active" Name="InActive" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Active" Name="Active" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="S.No" AllowSorting="true" ShowSortIcon="true">
                            <HeaderStyle Width="5%" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActiveId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSerialNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reason" AllowSorting="true" ShowSortIcon="true">
                            <HeaderStyle Width="14%" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkReason" runat="server" CommandName="SELECT" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVEID") %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDREASONNAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Category" ColumnGroupName="InActive">
                            <HeaderStyle Width="15%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lnkInActiveCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINACTIVECATEGORYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date" ColumnGroupName="InActive">
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInActiveDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDINACTIVEDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks" AllowSorting="false" ShowSortIcon="true" ColumnGroupName="InActive">
                            <HeaderStyle Width="10%" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInActiveRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINACTIVEREMARKS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="User" AllowSorting="false" ShowSortIcon="true" ColumnGroupName="InActive">
                            <HeaderStyle Width="10%" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInActiveUser" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINACTIVEBYNAME") %>'></telerik:RadLabel>
                                <b>
                                    <telerik:RadLabel ID="RadLabel2" runat="server" Text="On : "></telerik:RadLabel>
                                </b>
                                <telerik:RadLabel ID="lbInActiveOn" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date" AllowSorting="false" ShowSortIcon="true" ColumnGroupName="Active">
                            <HeaderStyle Width="10%" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActiveDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDACTIVEDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks" AllowSorting="false" ShowSortIcon="true" ColumnGroupName="Active">
                            <HeaderStyle Width="10%" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActiveRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVEREMARKS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="User  " AllowSorting="false" ShowSortIcon="true" ColumnGroupName="Active">
                            <HeaderStyle Width="10%" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActiveUser" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVEBYNAME") %>'></telerik:RadLabel>
                                <b>
                                    <telerik:RadLabel ID="RadLabel1" runat="server" Text="On : "></telerik:RadLabel>
                                </b>
                                <telerik:RadLabel ID="lblModifiedDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDMODIFIEDDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" UniqueName="Action">
                            <HeaderStyle Width="6%" HorizontalAlign="Center" />
                            <ItemStyle Wrap="false" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
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
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" />
                    <Scrolling AllowScroll="false" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
