<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewNewApplicantRemarks.aspx.cs"
    Inherits="CrewNewApplicantRemarks" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MobileNumber" Src="../UserControls/UserControlMobileNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MCUser" Src="~/UserControls/UserControlMultiColumnUser.ascx" %>

<%@ Register TagPrefix="eluc" TagName="Quick" Src="../UserControls/UserControlQuick.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewNewApplicantRemarks" runat="server">
        <telerik:RadSkinManager runat="server" ID="RadSkinManager1"></telerik:RadSkinManager>
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="86%">
            <eluc:TabStrip ID="MenuDiscussion" runat="server" OnTabStripCommand="MenuDiscussion_TabStripCommand" Title="General Remarks"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployeeFirstName" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployeeMiddleName" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployeeLastName" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAppliedRank" runat="server" Text="Applied Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRank" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMobileNumber1" runat="server" Text="Mobile Number 1"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MobileNumber ID="txtMobileNumber1" IsMobileNumber="true" runat="server" ReadOnly="true" CssClass="readonlytextbox" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMobileNumber2" runat="server" Text="Mobile Number 2"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MobileNumber ID="txtMobileNumber2" IsMobileNumber="true" runat="server" ReadOnly="true" CssClass="readonlytextbox" />
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="1" cellspacing="0" style="padding: 1px; margin: 1px; border-style: solid; border-width: 1px;"
                width="99%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDOA" runat="server" Text="Date Of Availability"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDOA" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFollowupDate" runat="server" Text="FollowUp Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtFollowupDate" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSeafarerRequierements" runat="server" Text="Seafarer Requirement"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucSeafarerRequirement" runat="server" QuickTypeCode="122" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblActiveInActive" runat="server" Text="Active / In-Active"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rblInActive" runat="server" Direction="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="ActiveRemarks">
                            <Items>
                                <telerik:ButtonListItem Text="Active" Value="1" />
                                <telerik:ButtonListItem Text="In-Active" Value="0" />

                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReason" runat="server" Text="Reason"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ddlInactiveReason" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                            CssClass="dropdown_mandatory" OnTextChangedEvent="ddlReason_TextChanged" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtInActiveDate" runat="server" />
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="1" cellspacing="0" style="padding: 1px; margin: 1px; border-style: solid; border-width: 1px;"
                width="99%">
                <tr>
                    <td align="left" colspan="2">
                        <telerik:RadLabel ID="lblPostYourCommentsHere" runat="server" Text="Post Your Comments Here"></telerik:RadLabel>
                    </td>
                    <td align="left" style="vertical-align: top;" colspan="2">
                        <telerik:RadTextBox ID="txtNotesDescription" runat="server" CssClass="gridinput_mandatory"
                            Height="49px" TextMode="MultiLine" Width="692px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <table width="50%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSearchBy" runat="server" Text="Search By User"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MCUser ID="RadMcUserTD" runat="server" Width="80%" emailrequired="false" designationrequired="false" ItemsPerRequest="50" />
                        <asp:ImageButton ID="ImgSearch" runat="server" ImageAlign="AbsBottom" ImageUrl="<%$ PhoenixTheme:images/search.png %>"
                            ToolTip="Search" OnClick="ImgBtnSearch_Click" />
                    </td>
                </tr>
            </table>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvDiscussion" runat="server" EnableViewState="true" Height="60%"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvDiscussion_NeedDataSource" OnItemCommand="gvDiscussion_ItemCommand" EnableHeaderContextMenu="true" ShowFooter="false"
                AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
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
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Posted By" HeaderStyle-Width="15%" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.NAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Comments" HeaderStyle-Width="60%" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblComments" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.DESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Posted Date" HeaderStyle-Width="15%" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPostedDate" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.POSTEDDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
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
