<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsEmployeeOC28Assessment.aspx.cs"
    Inherits="VesselAccounts_VesselAccountsEmployeeOC28Assessment" %>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSupdtConcerns" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
      
            <%--<asp:UpdatePanel runat="server" ID="pnlSupdtConcerns">
        <ContentTemplate>--%>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">


                <eluc:TabStrip ID="MainMenuSupdtConcerns" runat="server" TabStrip="true" OnTabStripCommand="MainMenuSupdtConcerns_TabStripCommand"></eluc:TabStrip>

                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtMiddleName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtLastName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblEmployeeNumber" runat="server" Text="File No."></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <telerik:RadTextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
                <hr />

                <eluc:TabStrip ID="MenuSupdtConcerns" runat="server" OnTabStripCommand="MenuSupdtConcerns_TabStripCommand"></eluc:TabStrip>

                <%--  <asp:GridView ID="gvOC28" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                GridLines="None" Width="100%" CellPadding="3" EnableViewState="false" OnRowDataBound="gvOC28_RowDataBound"
                OnRowCommand="gvOC28_RowCommand" OnRowCreated="gvOC28_RowCreated">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" />--%>

                <telerik:RadGrid RenderMode="Lightweight" ID="gvOC28" runat="server" Height="500px" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" OnNeedDataSource="gvOC28_NeedDataSource"
                    OnItemDataBound="gvOC28_ItemDataBound"
                    OnItemCommand="gvOC28_ItemCommand"
                    AutoGenerateColumns="false">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="Top">
                        <HeaderStyle Width="102px" />
                        <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>

                        <Columns>
                            <telerik:GridTemplateColumn>
                                <HeaderStyle Width="15%" />
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblVesselHeader" runat="server" Text="Vessel"></telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblVessel" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblVesselId" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDSIGNONVESSELID") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblVslTypeHeader" runat="server" Text="Vessel Type"></telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblVslType" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDTYPEDESCRIPTION") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblRank" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblJoinedDateHeader" runat="server" Text="Joining Date"></telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblJoinedDate" runat="server" Text='<%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDATEOFJOINING", "{0:dd/MMM/yyyy}"))%>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblSignoffDateHeader" runat="server" Text="Relief Due"></telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblSignoffDate" runat="server" Text='<%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDSIGNOFFDATE", "{0:dd/MMM/yyyy}"))%>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblSignonOffId" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDSIGNONOFFID") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblAssessmentDateHeader" runat="server" Text="Date"></telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblAssessmentDate" runat="server" Text='<%#  SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDASSESSMENTDATE", "{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblAssessmentId" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDASSESSMENTID") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblScoreHeader" runat="server" Text="Self"></telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblScore" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALSCORE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblSuptScoreHeader" runat="server" Text="Superintendent"></telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblSuptScore" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALSUPTSCORE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblScoreVarienceHeader" runat="server" Text="Difference"></telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblScoreVarience" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCOREVARIENCE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <HeaderStyle Wrap="true" />
                                <ItemStyle Width="60px" />
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblSuptHeader" runat="server" Text="By"></telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblSupt" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPTNAME") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblSuptId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPTID") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblRemarksHeader" runat="server" Text="Comments"></telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPTCOMMENTSTOOLTIP") %>'></telerik:RadLabel>
                                    <eluc:ToolTip runat="server" ID="ucRemarks" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPTCOMMENTS") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblActionHeader" runat="server" Text="Action"></telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Export XL" 
                                        CommandName="CREWEXPORT2XL" CommandArgument='<%# Container.DataSetIndex %>'
                                        ID="cmdCrewExport2XL" ToolTip="MST/CE Assessment">
                                        <span class="icon"><i class="fas fa-file-excel"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="415px" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>


                <eluc:Status runat="server" ID="ucStatus" />

            </div>
       

    </form>
</body>
</html>
