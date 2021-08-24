<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewPD.aspx.cs" Inherits="CrewPD" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonToolTip" Src="~/UserControls/UserControlCommonToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Query Activity</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvSearch.ClientID %>"));
                }, 10);
            }
            window.onresize = window.onload = Resize;
            function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadSkinManager runat="server" ID="RadSkinManager1"></telerik:RadSkinManager>
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" DestroyOnClose="true"></telerik:RadWindowManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="CrewQueryMenu" runat="server" OnTabStripCommand="PD_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvSearch" runat="server" EnableViewState="true"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvSearch_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvSearch_ItemDataBound"
                OnItemCommand="gvSearch_ItemCommand" OnUpdateCommand="gvSearch_UpdateCommand" ShowFooter="false" OnSortCommand="gvSearch_SortCommand"
                AutoGenerateColumns="false">
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="true" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="true" HeaderStyle-HorizontalAlign="Center">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <NestedViewSettings>
                        <ParentTableRelation>
                            <telerik:GridRelationFields MasterKeyField="FLDCREWPLANID" DetailKeyField="FLDCREWPLANID" />
                        </ParentTableRelation>
                    </NestedViewSettings>
                    <NestedViewTemplate>
                        <table>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="RadLabel1" runat="server" Font-Bold="true" Text="On Signer Last Vessel:"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblLastVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTVESSEL") %>'></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="RadLabel2" runat="server" Font-Bold="true" Text="Proposed By:"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblProposedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPOSEDBY") %>'></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NestedViewTemplate>
                    <HeaderStyle Width="120px" />
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="On Signer" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkemployee" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEmployeeId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblCrewPlanId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWPLANID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRank" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblAppType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVALTYPE") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSupt" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTECHDIRECTOR") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblTop4" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOP4") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblNewApp" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEWAPP") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblVessel" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblPdType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPDTYPE") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblRankCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Planned Relief" HeaderStyle-Wrap="true" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="16%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPlannedReliefDate" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDEXPECTEDJOINDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="PD Status" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPDStatus" runat="server" CssClass="tooltip" ClientIDMode="AutoID" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPDSTATUS")%>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipAddress" runat="server" TargetControlId="lblPDStatus" Text='<%#HttpContext.Current.Server.HtmlDecode(DataBinder.Eval(Container, "DataItem.FLDAPPROVALREMARKS").ToString()) %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--     <telerik:GridTemplateColumn HeaderText="Last vessel" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLastVessel" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDLASTVESSEL")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>
                        <telerik:GridTemplateColumn HeaderText="Off Signer" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOFFSIGNERNAME")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRelieverRank" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDRELIEVERRANK")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Relief Due" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Wrap="true" HeaderStyle-Width="16%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReliefDueDate" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDRELIEFDUEDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="20%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDVESSELNAME")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--<telerik:GridTemplateColumn HeaderText="Proposed By" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblProposedBy" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPROPOSEDBY")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>
                        <telerik:GridTemplateColumn HeaderText="Category" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="15%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSTATUS")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Superintendent Remarks" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Wrap="true" HeaderStyle-Width="25%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarks" runat="server" Style="word-wrap: break-word;"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDMUMBAIREMARKS")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblInterviewId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINTERVIEWID") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtMumbaiRemarks" runat="server" CssClass="gridinput_mandatory" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMUMBAIREMARKS")%>' Width="100%"></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="" HeaderStyle-Width="28%">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" ID="cmdEdit" ToolTip="Edit" CommandName="EDIT" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <eluc:CommonToolTip ID="ucCommonToolTip" runat="server" />
                                <asp:LinkButton runat="server" AlternateText="Approve" ID="cmdApprove" CommandName="Approve" ToolTip="Approve" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-award"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="PD Form"
                                    CommandName="PDFORM" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdPDForm"
                                    ToolTip="PD Form">
                                <span class="icon"><i class="fas fa-file-pr"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Attachment" ID="cmdAtt" ToolTip="Attachment" CommandName="Attachment" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Comment" ID="cmdComment" CommandName="Comment" ToolTip="Comments" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-comment-alt"></i></span>
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
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder" AllowExpandCollapse="true">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
