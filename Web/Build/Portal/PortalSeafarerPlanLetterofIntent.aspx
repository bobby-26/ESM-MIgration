<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PortalSeafarerPlanLetterofIntent.aspx.cs" Inherits="PortalSeafarerPlanLetterofIntent" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Plan</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmportalseafarer" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:TabStrip ID="HomeMenu" runat="server" OnTabStripCommand="HomeMenu_TabStripCommand" TabStrip="true"></eluc:TabStrip>

        <telerik:RadGrid RenderMode="Lightweight" ID="gvLetterOfInt" runat="server" Height="520px" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvLetterOfInt_NeedDataSource"
                        OnItemDataBound="gvLetterOfInt_ItemDataBound"
                        OnItemCommand="gvLetterOfInt_ItemCommand"
                        GroupingEnabled="false" EnableHeaderContextMenu="true"
                        AutoGenerateColumns="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
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
                            <HeaderStyle Width="102px" />
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Proposed Rank" Visible="false">
                                    <HeaderStyle Width="10%" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.FLDRANKNAME") %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Vessel Type">
                                    <HeaderStyle Width="37%" />
                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblloiletterid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDLETTERIDLOI")%>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblEmployeeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></telerik:RadLabel>
                                        <asp:LinkButton ID="lblFLDISSUEDDATELOI" Visible="false" CommandName="LOICANCEL" ToolTip="Click to cancel" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDLETTEROFINTENTSTATUS")%>'></asp:LinkButton>
                                        <telerik:RadLabel ID="lblstatusvalue" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDLETTEROFINTENT")%>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblVesselType" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDVESSELTYPESNAME")%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Contract Period">
                                    <HeaderStyle Width="10%" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblTotalScore" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCONTRACTPERIOD")%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Wage Agreed">
                                    <HeaderStyle Width="10%" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDailyRate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSALAGREEDUSD")%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Rejoining Bonus">
                                    <HeaderStyle Width="10%" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblRejoiningBonus" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREJOININGBONUS")%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Expected Join Date">
                                    <HeaderStyle Width="12%" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblInterviewId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPJOINDATE", "{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Created Date">
                                    <HeaderStyle Width="10%" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblCreatedDate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCREATEDDATE", "{0:dd/MMM/yyyy}")%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Issued Date">
                                    <HeaderStyle Width="10%" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblIssuedDate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDISSUEDDATE", "{0:dd/MMM/yyyy}")%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Accepted Date">
                                    <HeaderStyle Width="10%" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblAcceptedDate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDACCEPTEDDATE", "{0:dd/MMM/yyyy}")%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Modified Date" Visible="false">
                                    <HeaderStyle Width="10%" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblModifiedDate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMODIFIEDDATE", "{0:dd/MMM/yyyy}")%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Canceled Date">
                                    <HeaderStyle Width="10%" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblCancelDate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCANCELEDDATE", "{0:dd/MMM/yyyy}")%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Office Staff Name">
                                    <HeaderStyle Width="17%" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblOfficeStafeName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOFFICESTAFFNAME")%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Status">
                                    <HeaderStyle Width="10%" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblPDStatus" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDLETTEROFINTENTSTATUS")%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Action">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <%--<asp:LinkButton runat="server" AlternateText="Cancel"
                                            CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                            ToolTip="Cancel">
                                    <span class="icon"><i class="fa fa-trash"></i></span>
                                        </asp:LinkButton>--%>

                                        <asp:LinkButton runat="server" AlternateText="Letter of Intent" EnableViewState="true"
                                            ID="cmdOfferLetter" Visible="true" CommandName="LETTERINTENT" CommandArgument="<%# Container.DataSetIndex %>"
                                            ToolTip="Letter of Intent">
                                    <span class="icon"><i class="fas fa-appletter_red"></i></span>
                                        </asp:LinkButton>

                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="410px" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
