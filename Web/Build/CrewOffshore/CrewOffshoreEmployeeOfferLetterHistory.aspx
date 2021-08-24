<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreEmployeeOfferLetterHistory.aspx.cs" Inherits="CrewOffshoreEmployeeOfferLetterHistory" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Offer Letter History</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmCrewAppointmentLetter" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%; position: absolute;">
                <eluc:TabStrip ID="CrewQuery" Title="Crew Offer Letter History" runat="server" OnTabStripCommand="CrewQuery_TabStripCommand"></eluc:TabStrip>

                <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />


                <div id="divCrewCompanyExperience" style="position: relative; z-index: +1">
                    <table id="tblCrewCompanyExperience" width="100%">
                        <tr>
                            <td style="width: 10%">
                                <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                            </td>
                            <td style="width: 10%">
                                <telerik:RadTextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                            </td>
                            <td style="width: 10%">
                                <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                            </td>
                            <td style="width: 10%">
                                <telerik:RadTextBox runat="server" ID="txtMiddleName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                            </td>
                            <td style="width: 10%">
                                <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                            </td>
                            <td style="width: 10%">
                                <telerik:RadTextBox runat="server" ID="txtLastName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 10%">
                                <telerik:RadLabel ID="lblEmployeeNumber" runat="server" Text="Employee Number"></telerik:RadLabel>
                            </td>
                            <td style="width: 10%">
                                <telerik:RadTextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                            </td>
                            <td style="width: 10%">
                                <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                            </td>
                            <td style="width: 10%">
                                <telerik:RadTextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <hr />

                <eluc:TabStrip ID="MenuCrewAppointmentHistory" runat="server" OnTabStripCommand="CrewAppointmentHistory_TabStripCommand"></eluc:TabStrip>

                <div id="divGrid" style="position: relative; z-index: +1">
                    <%--   <asp:GridView ID="gvCrewAppointmentLetter" runat="server" AutoGenerateColumns="False"
                    Font-Size="11px" Width="100%" CellPadding="3" OnRowCommand="gvCrewAppointmentLetter_RowCommand"
                    OnRowDataBound="gvCrewAppointmentLetter_RowDataBound"
                    OnRowEditing="gvCrewAppointmentLetter_RowEditing" ShowFooter="false" ShowHeader="true"
                    EnableViewState="false" AllowSorting="true" OnSorting="gvCrewAppointmentLetter_Sorting">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                    <RowStyle Height="10px" />
                    <RowStyle Height="10px" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewAppointmentLetter" runat="server" Height="400px" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvCrewAppointmentLetter_NeedDataSource"
                        OnItemCommand="gvCrewAppointmentLetter_ItemCommand"
                        OnItemDataBound="gvCrewAppointmentLetter_ItemDataBound"
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
                                <telerik:GridTemplateColumn HeaderText="Vessel">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblVessel" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>'
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'>
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Rank">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblLetterId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLETTERID") %>'></telerik:RadLabel>
                                        <%--<telerik:RadLabel ID="lblCrewAppointmentLetterId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPOINTMENTLETTERID") %>'></telerik:RadLabel>--%>
                                        <telerik:RadLabel ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>'></telerik:RadLabel>
                                        <asp:LinkButton ID="lnkRank" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' Visible="false"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Wage Agreed (USD)">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblWageAgreedItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSALAGREEDUSD") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Contract Period (Days)">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblContractPeriodItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLUSORMINUSRANGE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Action">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" CommandArgument="<%# Container.DataSetIndex %>" CommandName="OFFERLETTERPDF"
                                            ID="cmdOfferletter" ImageAlign="AbsMiddle"
                                            Text=".." ToolTip="Show Offer Letter">
                                     <span class="icon"><i class="fas fa-file-contract"></i></span>
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
                </div>

            </div>
        </telerik:RadAjaxPanel>
    </form>

</body>
</html>
