<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreSummary.aspx.cs" Inherits="CrewOffshoreSummary" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Summary</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmCrewSummary" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%; position: absolute;">
            <eluc:TabStrip ID="tabstrip1" runat="server" Title="Crew Summary" />

            <table width="100%" cellpadding="1" cellspacing="1">
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
                    <td runat="server" id="tdempno">
                        <telerik:RadLabel ID="lblEmployeeNo" runat="server" Text="Employee Number"></telerik:RadLabel>
                    </td>
                    <td runat="server" id="tdempnum">
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
            <div id="divFind">
                <table id="tblConfigureDocumentsRequired">
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Panel runat="server" ID="pnlExperience" GroupingText="Experience">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton runat="server" ID="rbCompany" Checked="true" AutoPostBack="true"
                                                            GroupName="rbExperience" />
                                                    </td>
                                                    <td>
                                                        <telerik:RadLabel ID="lblCompany" runat="server" Text="Company"></telerik:RadLabel>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton runat="server" ID="rbFull" AutoPostBack="true" GroupName="rbExperience" />
                                                    </td>
                                                    <td>
                                                        <telerik:RadLabel ID="lblfull" runat="Server" Text="Full"></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                    <td>
                                        <asp:Panel ID="pnlRankType" runat="server" GroupingText="Rank Exp Type">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton runat="server" ID="rblMonths" Checked="true" AutoPostBack="true"
                                                            GroupName="rblRank" />
                                                    </td>
                                                    <td>
                                                        <telerik:RadLabel ID="lblExpinMonth" runat="server" Text="Exp in Months"></telerik:RadLabel>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton runat="server" ID="rblDays" AutoPostBack="true" GroupName="rblRank" />
                                                    </td>
                                                    <td>
                                                        <telerik:RadLabel ID="lblExpinDecimal" runat="server" Text="Exp in Decimal"></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Panel runat="server" ID="pnlXX">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton runat="server" ID="rbVesselTypeRank" AutoPostBack="true" Checked="true"
                                                            GroupName="rbExperienceType" />
                                                    </td>
                                                    <td>
                                                        <telerik:RadLabel ID="lblVesselTypeRank" runat="server" Text="Vessel Type - Rank"></telerik:RadLabel>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton runat="server" ID="rbCompanyRank" AutoPostBack="true" GroupName="rbExperienceType" />
                                                    </td>
                                                    <td>
                                                        <telerik:RadLabel ID="lblCompanyRank" runat="server" Text="Company - Rank"></telerik:RadLabel>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton runat="server" ID="rbEngineTypeRank" AutoPostBack="true" GroupName="rbExperienceType" />
                                                    </td>
                                                    <td>
                                                        <telerik:RadLabel ID="lblEnginetype" runat="server" Text="Engine Type - Rank"></telerik:RadLabel>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton runat="server" ID="rbPrincipalRank" AutoPostBack="true" GroupName="rbExperienceType" />
                                                    </td>
                                                    <td>
                                                        <telerik:RadLabel ID="lblPrincipalRank" runat="server" Text="Principal - Rank"></telerik:RadLabel>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton runat="server" ID="rbOwnerRank" AutoPostBack="true" GroupName="rbExperienceType" />
                                                    </td>
                                                    <td>
                                                        <telerik:RadLabel ID="lblOwnerRank" runat="server" Text="Owner - Rank"></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <br />



            <table border="0" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" ID="lblExpInMonths" Font-Bold="true" Font-Size="Small"
                            Text="*Experience In Months and Days">
                        </telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Label runat="server" ID="ltGrid" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
            <br />

            <eluc:TabStrip ID="MenuOprExpExcel" runat="server" OnTabStripCommand="OprExpExcel_TabStripCommand"></eluc:TabStrip>

            <table width="100%">
                <tr>
                    <td style="vertical-align: bottom;">
                        <u><b>
                            <telerik:RadLabel ID="lblOprExp" runat="server" Text="*Operational Experience"></telerik:RadLabel>
                        </b></u>
                        <br />
                        <%--  <asp:GridView ID="gvOprExp" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                Width="100%" CellPadding="2" AllowSorting="true" ShowHeader="true" ShowFooter="false"
                                OnRowDataBound="gvOprExp_RowDataBound" OnRowCreated="gvOprExp_RowCreated"  EnableViewState="false">
                                <FooterStyle CssClass="datagrid_footerstyle" />
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvOprExp" runat="server" Height="" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                            CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvOprExp_NeedDataSource"
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
                                <ColumnGroups>
                                    <telerik:GridColumnGroup HeaderText="Previous Ranks" Name="Previous" HeaderStyle-HorizontalAlign="Center">
                                    </telerik:GridColumnGroup>
                                </ColumnGroups>
                                <ColumnGroups>
                                    <telerik:GridColumnGroup HeaderText="Current Ranks" Name="Current" HeaderStyle-HorizontalAlign="Center">
                                    </telerik:GridColumnGroup>
                                </ColumnGroups>
                                <HeaderStyle Width="102px" />
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Experience">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle Width="40%" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOTHEREXPERIANCESUMMARYNAME")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Other Experience" ColumnGroupName="Previous">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle Width="15%" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblPROCVALUE" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCVALUE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn ColumnGroupName="Previous" HeaderText="Company Experience">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle Width="15%" />

                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblpreExpCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRVALUE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Other Experience" ColumnGroupName="Current">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle Width="15%" />

                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblCexpOCVALUE" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOCVALUE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Company Experience" ColumnGroupName="Current">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle Width="15%" />

                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblExpCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALUE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </td>
                </tr>
            </table>
            <br />

            <eluc:TabStrip ID="MenuSplCrgorxp" runat="server" OnTabStripCommand="SplCrgorxpExcel_TabStripCommand"></eluc:TabStrip>

            <table>
                <tr>
                    <td style="vertical-align: bottom;">
                        <u><b>
                            <telerik:RadLabel ID="lblSplCrgorxp" runat="server" Text="*Special Cargo Handling Experience"></telerik:RadLabel>
                        </b></u>
                        <br />
                        <%--  <asp:GridView ID="gvSplCrgorexp" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="2" AllowSorting="true" ShowHeader="true" ShowFooter="false"
                            OnRowDataBound="SplCrgorexp_RowDataBound" OnRowCreated="gvSplCrgorexp_RowCreated" EnableViewState="false">
                            <FooterStyle CssClass="datagrid_footerstyle" />
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvSplCrgorexp" runat="server" Height="" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                            CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvSplCrgorexp_NeedDataSource"
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
                                <ColumnGroups>
                                    <telerik:GridColumnGroup HeaderText="Previous Ranks" Name="Previous" HeaderStyle-HorizontalAlign="Center">
                                    </telerik:GridColumnGroup>
                                </ColumnGroups>
                                <ColumnGroups>
                                    <telerik:GridColumnGroup HeaderText="Current Ranks" Name="Current" HeaderStyle-HorizontalAlign="Center">
                                    </telerik:GridColumnGroup>
                                </ColumnGroups>
                                <HeaderStyle Width="102px" />
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Experience">
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle Width="40%" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblName1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOTHEREXPERIANCESUMMARYNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Other Experience" ColumnGroupName="Previous">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle Width="15%" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblpreCexpCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCVALUE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Company Experience" ColumnGroupName="Previous">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle Width="15%" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblPRVALUE" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRVALUE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Other Experience" ColumnGroupName="Current">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle Width="15%" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblsplOCVALUE" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOCVALUE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Company Experience" ColumnGroupName="Current">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle Width="15%" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblCexpCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALUE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </td>
                </tr>
            </table>
            <br />

            <eluc:TabStrip ID="MenuAnchorHandlingexcel" runat="server" OnTabStripCommand="MenuAnchorHandlExcel_TabStripCommand"></eluc:TabStrip>

            <table>
                <tr>
                    <td style="vertical-align: bottom;">
                        <u><b>
                            <telerik:RadLabel ID="lblAncrHndlng" runat="server" Text="*Anchor Handling Experience"></telerik:RadLabel>
                        </b></u>
                        <br />
                        <%-- <asp:GridView ID="GvAncrHndlng" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="2" AllowSorting="true" ShowHeader="true" ShowFooter="false"
                            OnRowDataBound="GvAncrHndlng_RowDataBound" OnRowCreated="GvAncrHndlng_RowCreated" EnableViewState="false">
                            <FooterStyle CssClass="datagrid_footerstyle" />
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
                        <telerik:RadGrid RenderMode="Lightweight" ID="GvAncrHndlng" runat="server" Height="" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                            CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="GvAncrHndlng_NeedDataSource"
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
                                <ColumnGroups>
                                    <telerik:GridColumnGroup HeaderText="Previous Ranks" Name="Previous" HeaderStyle-HorizontalAlign="Center">
                                    </telerik:GridColumnGroup>
                                </ColumnGroups>
                                <ColumnGroups>
                                    <telerik:GridColumnGroup HeaderText="Current Ranks" Name="Current" HeaderStyle-HorizontalAlign="Center">
                                    </telerik:GridColumnGroup>
                                </ColumnGroups>
                                <HeaderStyle Width="102px" />
                                <Columns>
                                    <telerik:GridTemplateColumn>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="70%"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblGvAncrHndlngHeader" Text="Experience" runat="server"></telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblGvAncrHndlngname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOTHEREXPERIANCESUMMARYNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="30%"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblpreCexpHeader" runat="server" Text="Other Experience"></telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblANPROCVALUE" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCVALUE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="50%"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblpreGvAncrHndHeader" runat="server" Text="Company Experience"></telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblpreGvAncrHndCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRVALUE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="30%"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblCexpHeader" runat="server" Text="Other Experience"></telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblOCVALUE" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOCVALUE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="50%"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblGvAncrHndHeader" runat="server" Text="Company Experience"></telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblGvAncrHndCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALUE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </td>
                </tr>
            </table>
            <%--</div>--%>
        </div>
    </form>
</body>
</html>
