<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewLicenceRequestExtraInfo.aspx.cs"
    Inherits="CrewLicenceRequestExtraInfo" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCompany" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="../UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="User" Src="../UserControls/UserControlMultiColumnUser.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>More Info.</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="LicenceRequest" runat="server" OnTabStripCommand="LicenceRequest_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table cellpadding="4" cellspacing="2" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="ltReqNo" runat="server" Text="Request No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtReqNo" runat="server" ReadOnly="true" Enabled="false" Width="80%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFileNo" runat="server" Text="File No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFileno" runat="server" Enabled="false" Width="80%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtName" runat="server" Enabled="false" Width="80%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFlag" runat="server" Text="Flag"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFlag" runat="server" ReadOnly="true" Enabled="false" Width="80%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVessel" runat="server" ReadOnly="true"  Enabled="false" Width="80%"></telerik:RadTextBox>
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblJoinedVessel" runat="server" Text="Joined Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtJoinedVessel" runat="server" ReadOnly="true" Width="80%"></telerik:RadTextBox>
                        <asp:LinkButton runat="server" AlternateText="Search" ID="imgJoinedVessel" CommandName="JOINEDVESSEL"
                            OnClick="OnClickJoinedVessel" ToolTip="Refresh">                         
                                    <span class="icon"><i class="fas fa-redo"></i></span>
                        </asp:LinkButton>

                    </td>

                    <td>
                        <telerik:RadLabel ID="lblConsulate" runat="server" Text="Consulate"></telerik:RadLabel>
                    </td>
                    <td>                  
                        <telerik:RadComboBox ID="ddlFlagAddress" runat="server" CssClass="dropdown_mandatory" Width="80%" OnDataBound="ddlFlagAddress_DataBound"
                            AutoPostBack="true" Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">        
                        </telerik:RadComboBox>

                    </td>

                    <td>
                        <telerik:RadLabel ID="lblCompany" runat="server" Text="Company"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlCompany ID="ddlCompanyNameInReport" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                            runat="server" AppendDataBoundItems="true" Width="80%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAuthorizedRepresentative" runat="server" Text=" Authorized Representative"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAuthorizedRep" runat="server" Width="80%"></telerik:RadTextBox>
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblDesignation" runat="server" Text="Designation"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDesignation" runat="server" Width="80%"></telerik:RadTextBox>
                    </td>
                    <td colspan="4"></td>
                </tr>
                <tr>
                    <td colspan="8">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <telerik:RadLabel ID="lblPostYourCommentsHere" runat="server" Text="Post Your Comments Here"></telerik:RadLabel>
                    </td>
                    <td align="left" style="vertical-align: top;" colspan="6">
                        <telerik:RadTextBox ID="txtNotesDescription" runat="server" CssClass="gridinput_mandatory"
                            Height="49px" TextMode="MultiLine" Width="692px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSearchBy" runat="server" Text="Search By User"></telerik:RadLabel>
                    </td>
                    <td colspan="7">
                        <eluc:User ID="ucUser" runat="server" AppendDataBoundItems="true"
                            ActiveYN="172" width="25%"/>
                        <asp:LinkButton runat="server" AlternateText="Search" ID="ImgSearch"
                            OnClick="ImgBtnSearch_Click" ToolTip="Search">                         
                                <span class="icon"><i class="fas fa-search"></i></span>
                        </asp:LinkButton>

                    </td>
                </tr>
            </table>
            <br />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvDiscussion" runat="server" EnableViewState="false" Height="50%"
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
                                <telerik:RadLabel ID="lblDescription" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.DESCRIPTION")%>'></telerik:RadLabel>
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
            <eluc:Status ID="ucStatus" runat="server" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
