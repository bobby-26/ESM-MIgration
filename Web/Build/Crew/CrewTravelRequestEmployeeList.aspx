<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelRequestEmployeeList.aspx.cs" Inherits="Crew_CrewTravelRequestEmployeeList" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Designation" Src="../UserControls/UserControlDesignation.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="88%">            
            <eluc:TabStrip ID="CrewList" runat="server" Title="Crew List" OnTabStripCommand="CrewList_TabStripCommand"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCrewFrom" runat="server" Text="Crew From"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rblCrewFrom" runat="server" AutoPostBack="true" Direction="Horizontal">
                            <Items>
                                <telerik:ButtonListItem Text="Personal Master" Value="0" Selected="True" />
                                <telerik:ButtonListItem Text="New Applicant" Value="0"  />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                     <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtName" runat="server" CssClass="input"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>                   
                    <td>
                        <telerik:RadLabel ID="lblFileNo" runat="server" Text="File No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFileNo" runat="server" CssClass="input"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPresentRank" runat="server" Text="Designation/Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Rank ID="ddlRank" runat="server" CssClass="input" Visible="false" AppendDataBoundItems="true" AutoPostBack="true" />
                        <eluc:Designation ID="ucDesignation" runat="server" Visible="false" AppendDataBoundItems="true" AutoPostBack="true" CssClass="input" />
                    </td>
                </tr>
            </table>
            <br />

            <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewList" runat="server" AllowCustomPaging="true" AllowSorting="true" Height="95%"
                CellSpacing="0" GridLines="None" OnItemCommand="gvCrewList_ItemCommand" OnNeedDataSource="gvCrewList_NeedDataSource"
                OnItemDataBound="gvCrewList_ItemDataBound" EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true" AllowPaging="true">
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
                        <telerik:GridTemplateColumn HeaderText="Employee Code">
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblFileNo" runat="server" Text="File No."></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFileNo" runat="server"></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lnkEployeeName" runat="server"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIRSTNAME") + " " + DataBinder.Eval(Container,"DataItem.FLDMIDDLENAME") + " " + DataBinder.Eval(Container,"DataItem.FLDLASTNAME") %>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblEmpId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblRankHeader" runat="server" CommandName="Sort" CommandArgument="FLDRANKNAME"
                                    Text="Designation/Rank"></asp:LinkButton>
                                <img id="FLDRANKNAME" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAppliedRank" runat="server"></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Passport No.">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDPASSPORTNO") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date of Birth">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDDATEOFBIRTH", "{0:dd/MMM/yyyy}") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">Action</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                 <asp:LinkButton runat="server" AlternateText="Add Travel" ToolTip="Add Travel" Width="20PX" Height="20PX"
                                    CommandName="ADDTRAVELREQUEST" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdGenerateRequest">
                                <span class="icon"><i class="fas fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="3" EnableColumnClientFreeze="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
