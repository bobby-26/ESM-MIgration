<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewEmployeeAvailableDate.aspx.cs" Inherits="CrewEmployeeAvailableDate" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register Src="~/UserControls/UserControlTabsTelerik.ascx" TagPrefix="eluc" TagName="TabStrip" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MUCCity" Src="~/UserControls/UserControlMultiColumnCity.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Employee Available Date</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script language="javascript">

            function cancelBack() {
                if ((event.keyCode == 8 ||
                   (event.keyCode == 37 && event.altKey) ||
                   (event.keyCode == 39 && event.altKey))
                    &&
                   (event.srcElement.form == null || event.srcElement.isTextEdit == false)
                  ) {
                    event.cancelBubble = true;
                    event.returnValue = false;
                    return false;
                }
            }
        </script>
    </telerik:RadCodeBlock>

</head>
<body onkeydown="return cancelBack()">
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuHeader" runat="server" OnTabStripCommand="MenuHeader_TabStripCommand"
            TabStrip="true" Visible="false"></eluc:TabStrip>
        <eluc:TabStrip ID="MenuAvailability" runat="server" OnTabStripCommand="MenuAvailability_TabStripCommand"
            TabStrip="true" Visible="false"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="93%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuTitle" runat="server" OnTabStripCommand="MenuTitle_TabStripCommand"
                TabStrip="true" Visible="false"></eluc:TabStrip>
            <asp:HiddenField ID="hdnqueryStringData" runat="server" />
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFileNo" runat="server" Text="File No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFileNo" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtName" runat="server" CssClass="readonlytextbox" Width="300px"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtEmployeeId" runat="server"></telerik:RadTextBox>
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRanksearch" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRank" runat="server" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAvailableStartDate" runat="server" Text="Available"></telerik:RadLabel>
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td align="center">From</td>
                                <td align="center"></td>
                                <td align="center">To</td>
                            </tr>
                            <tr>
                                <td>
                                    <eluc:Date ID="txtAvailableStartDate" runat="server" CssClass="input" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblDash" runat="server" Text="-"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Date ID="txtAvailableEndDate" runat="server" CssClass="input" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <br />

            <eluc:TabStrip ID="MenuCrewEmpAvailability" runat="server" OnTabStripCommand="CrewEmpAvailability_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvEmpAvailability" runat="server" AllowCustomPaging="true" AllowSorting="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvEmpAvailability_ItemCommand" OnNeedDataSource="gvEmpAvailability_NeedDataSource"
                OnItemDataBound="gvEmpAvailability_ItemDataBound" EnableViewState="false" Height="80%" GroupingEnabled="false"
                EnableHeaderContextMenu="true" AllowPaging="true" ShowFooter="true">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDEMPLOYEEAVAILABILITYDATESID">
                       <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
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
                        <telerik:GridTemplateColumn HeaderText="S.No">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRowNUmber" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDROWNUMBER") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Available From">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="RadLabel1" runat="server" Text="Available From"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStartDate" runat="server"
                                    Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDAVAILABLESTARTDATE","{0:dd/MMM/yyyy}")) %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="txtAvailableStartDateEdit" runat="server"
                                    CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container, "DataItem.FLDAVAILABLESTARTDATE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date ID="txtAvailableStartDateInsert" runat="server" CssClass="input_mandatory" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Available To">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblAvailableEndDate" runat="server" Text="Available To"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEndDate" runat="server"
                                    Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDAVAILABLEENDDATE","{0:dd/MMM/yyyy}")) %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="txtAvailableEndDateEdit" runat="server"
                                    CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container, "DataItem.FLDAVAILABLEENDDATE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date ID="txtAvailableEndDateInsert" runat="server" CssClass="input_mandatory" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Location">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblLocationHeader" runat="server" Text="Preferred Location"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLocation" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCITYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListCity">
                                    <telerik:RadTextBox ID="txtCityNameEdit" runat="server" Width="80%" Enabled="False"
                                        CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCITYNAME") %>'>
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowCityEdit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="Top" Text=".." CommandName="CITY" />
                                    <telerik:RadTextBox ID="txtPreferredLocationEdit" runat="server" Width="0px" CssClass="hidden"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDORIGINID") %>'>
                                    </telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <span id="spnPickListCity">
                                    <telerik:RadTextBox ID="txtCityName" runat="server" Width="80%" Enabled="False"
                                        CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCITYNAME") %>'>
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowCity" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="Top" Text=".." CommandName="CITY" />
                                    <telerik:RadTextBox ID="txtPreferredLocationInsert" runat="server" Width="0px" CssClass="hidden"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDORIGINID") %>'>
                                    </telerik:RadTextBox>
                                </span>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="110px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                    ToolTip="Edit" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                    ToolTip="Delete" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save"
                                    CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                    ToolTip="Save" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                    ToolTip="Cancel" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Add"
                                    CommandName="Add" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
                                    ToolTip="Add New" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                      <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="2" EnableColumnClientFreeze="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            </telerik:RadAjaxPanel>
    </form>
</body>
</html>
