<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PortalTravelQuote.aspx.cs"
    Inherits="PortalTravelQuote" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ucVessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Travel Quote</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmVesselSynchronizationStatus" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="85%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRequestNo" runat="server" Text="Request No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtReqNo" runat="server"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:ucVessel ID="ddlVessel" runat="server" AppendDataBoundItems="true" AssignedVessels="true" AutoPostBack="true" Entitytype="VSL" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCrewChangeBetween" runat="server" Text="Crew Change Between"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtStartDate" Width="120px" runat="server" DatePicker="true" />
                        &nbsp;and&nbsp;
                            <eluc:Date ID="txtEndDate" Width="120px" runat="server" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblquoteSentBetween" runat="server" Text="Quote Sent Between"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtQuoteSentFromDate" Width="120px" runat="server"
                            DatePicker="true" />
                        &nbsp;and&nbsp;
                            <eluc:Date ID="txtQuoteSentToDate" Width="120px" runat="server" DatePicker="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlStatus" runat="server" DataValueField="FLDHARDCODE"
                            DataTextField="FLDHARDNAME" Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                        </telerik:RadComboBox>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenutravelList" runat="server" OnTabStripCommand="MenutravelList_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvTravelQuote" runat="server" AutoGenerateColumns="False" Height="89%" EnableViewState="false"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvTravelQuote_NeedDataSource" EnableHeaderContextMenu="true" OnItemCommand="gvTravelQuote_ItemCommand" OnEditCommand="gvTravelQuote_EditCommand"
                OnUpdateCommand="gvTravelQuote_UpdateCommand" OnItemDataBound="gvTravelQuote_ItemDataBound">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
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

                        <telerik:GridTemplateColumn HeaderText="Request No." AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTravelAgentID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELAGENTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblTravelID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblAgentID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGENTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblFinalizedYN" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFINALIZEDYN") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkReferenceNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUISITIONNO") %>'
                                    CommandName="SELECT" CommandArgument="<%# Container.DataSetIndex %>"></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel Name" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>                                
                                <telerik:RadLabel ID="lblDateOfCrewChange" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDDATEOFCREWCHANGE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Port" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPortOfCrewChange" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPORTOFCREWCHANGE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSeaPortName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTravelStatus" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELSTATUS") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sent On" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>                                
                                <telerik:RadLabel ID="lbl" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDSENDDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="User Name" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblhandledby" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHANDLESBY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txthandledby" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHANDLESBY") %>'></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" HeaderStyle-Width="10%" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="cmdEdit" ToolTip="Edit" CommandName="EDIT" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" ID="cmdQuote" CommandName="QUOTE" ToolTip="Quote" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-file-alt"></i></span> 
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" ToolTip="Issue Ticket" Width="20PX" Height="20PX" CommandName="ISSUE"
                                    CommandArgument='<%# Container.DataSetIndex %>' ID="cmdIssue">
                                <span class="icon"><i class="fas fa-tag"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ID="cmdSave" CommandName="Update" ToolTip="Save" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" ID="cmdCancel" CommandName="Cancel" ToolTip="Cancel" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
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
