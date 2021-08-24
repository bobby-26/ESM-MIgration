<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewNewApplicantQueryActivity.aspx.cs"
    Inherits="CrewNewApplicantQueryActivity" %>

<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Query Activity</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadSkinManager runat="server" ID="RadSkinManager1"></telerik:RadSkinManager>
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" DestroyOnClose="true"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:UserControlStatus ID="ucStatus" runat="server" />

            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

            <eluc:TabStrip ID="CrewQueryMenu" runat="server" OnTabStripCommand="CrewQueryMenu_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewSearch" runat="server" AutoGenerateColumns="False" Height="99%" EnableViewState="true"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvCrewSearch_NeedDataSource" EnableHeaderContextMenu="true" OnItemCommand="gvCrewSearch_ItemCommand"
                OnItemDataBound="gvCrewSearch_ItemDataBound"  OnSortCommand="gvCrewSearch_SortCommand">
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" DataKeyNames="FLDEMPLOYEEID">
                    <NestedViewSettings>
                        <ParentTableRelation>
                            <telerik:GridRelationFields MasterKeyField="FLDEMPLOYEEID" DetailKeyField="FLDEMPLOYEEID" />
                        </ParentTableRelation>
                    </NestedViewSettings>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Name" ItemStyle-Width="15%" HeaderStyle-Width="15%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEmployeeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblActiveyn" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVEYN") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkEployeeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME").ToString()%>'
                                    CommandName="GETEMPLOYEE"></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Applied Rank" HeaderStyle-Width="100px" ShowSortIcon="true" AllowSorting="true" SortExpression="FLDRANKNAME">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAppliedRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="File No." HeaderStyle-Width="65px">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Zone">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDZONE")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CDC No.">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDSEAMANBOOKNO") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Batch">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDBATCH") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSTATUS") +" "+ DataBinder.Eval(Container, "DataItem.FLDSTATUSNAME")%>'
                                    ToolTip='<%#DataBinder.Eval(Container, "DataItem.FLDSTATUSDESCRIPTION")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="175px">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Reversal"
                                    CommandName="REVERSAL" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdReversal"
                                    ToolTip="Recover Applicant">
                                <span class="icon"><i class="fas fa-redo"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandName="GETEMPLOYEE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                    ToolTip="Edit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Activities"
                                    CommandName="ACTIVITY" CommandArgument="<%# Container.DataSetIndex %>" ID="imgActivity"
                                    ToolTip="Activities">
                                <span class="icon"><i class="fa fa-pencil-ruler"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="PD Form"
                                    CommandName="PDFORM" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdPDForm"
                                    ToolTip="PD Form">
                                <span class="icon"><i class="fas fa-file-pr"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Suitability Check"
                                    CommandName="SUITABILITYCHECK" CommandArgument="<%# Container.DataSetIndex %>" ID="imgSuitableCheck"
                                    ToolTip="Suitability Check">
                                <span class="icon"><i class="fas  fa-user-astronaut"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                    ToolTip="Delete">
                                <span class="icon"><i class="fas fa-user-minus"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Sync"
                                    CommandName="SYNC" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSync"
                                    ToolTip="Sync to Presea">
                                <span class="icon"><i class="fas fa-sync-alt"></i></span>
                                </asp:LinkButton>
                                 <asp:LinkButton runat="server" CommandName="PHOENIXSYNCLOGIN" 
                                    CommandArgument="<%# Container.DataSetIndex %>" ID="cmdServiceLogin"
                                    ToolTip="Phoenix Sync">
                                 <span class="icon"> <i class="fa fa-share-square-24"></i></span>
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
                    <NestedViewTemplate>
                        <table style="font-size: 11px; width: 60%">
                            <tr>
                                <td style="font-weight: 700">
                                    <telerik:RadLabel ID="RadLabel1" runat="server" Text="Applied On :"></telerik:RadLabel>

                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblAppliedOn" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPLIEDON","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                </td>
                                <td style="font-weight: 700">
                                    <telerik:RadLabel ID="RadLabel2" runat="server" Text="D.O.A :"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblDOA" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDOA", "{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                </td>
                                <td style="font-weight: 700">
                                    <telerik:RadLabel ID="RadLabel3" runat="server" Text="D.O.B :"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="RadLabel4" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDATEOFBIRTH", "{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td style="font-weight: 700">
                                    <telerik:RadLabel ID="RadLabel5" runat="server" Text="Last Remarks :"></telerik:RadLabel>
                                </td>
                                <td colspan="5">
                                    <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString() %>'></telerik:RadLabel>
                                    <eluc:ToolTip ID="ucToolTipAddress" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>' TargetControlId="lblRemarks" />
                                </td>
                            </tr>
                        </table>
                    </NestedViewTemplate>
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


