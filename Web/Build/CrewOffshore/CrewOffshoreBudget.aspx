<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreBudget.aspx.cs" Inherits="CrewOffshore_CrewOffshoreBudget" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Vessel Budget</title>

    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
      
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmRegistersBudget" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />



            <eluc:TabStrip ID="CrewQuery" runat="server" TabStrip="true" OnTabStripCommand="CrewQuery_TabStripCommand"></eluc:TabStrip>

            <div id="divFind" style="position: relative; z-index: +1;">
                <table id="tblConfigureBudget" width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                        </td>
                        <%--<td>
                                <eluc:Vessel ID="UcVessel" runat="server" CssClass="input" AppendDataBoundItems="true"
                                    AutoPostBack="true" OnTextChangedEvent="ucVessel_Changed" />
                            </td>--%>
                        <td>
                            <telerik:RadTextBox ID="txtVessel" runat="server" MaxLength="100" ReadOnly="true" CssClass="readonlytextbox"
                                Width="260px">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <b>
                <telerik:RadLabel ID="lblRev" runat="server" Text="Revision"></telerik:RadLabel>
            </b>

            <eluc:TabStrip ID="MenuRegistersBudget" runat="server" OnTabStripCommand="RegistersBudget_TabStripCommand"></eluc:TabStrip>


            <%-- <asp:GridView ID="gvBudgetRevision" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" ShowFooter="true" OnRowCommand="gvBudgetRevision_RowCommand"
                    OnRowEditing="gvBudgetRevision_RowEditing" OnRowCancelingEdit="gvBudgetRevision_RowCancelingEdit"
                    OnRowUpdating="gvBudgetRevision_RowUpdating" ShowHeader="true" EnableViewState="false"
                    DataKeyNames="FLDREVISIONID" OnRowDataBound="gvBudgetRevision_RowDataBound"
                    OnRowDeleting="gvBudgetRevision_OnRowDeleting">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvBudgetRevision" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvBudgetRevision_NeedDataSource"
                OnItemDataBound="gvBudgetRevision_ItemDataBound"
                OnItemCommand="gvBudgetRevision_ItemCommand"
                OnUpdateCommand="gvBudgetRevision_UpdateCommand"
                GroupingEnabled="false" EnableHeaderContextMenu="true"
                AutoGenerateColumns="false" ShowFooter="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView DataKeyNames="FLDREVISIONID" EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
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
                        <telerik:GridButtonColumn Text="DoubleClick" CommandName="Edit" Visible="false" />
                        <telerik:GridTemplateColumn HeaderText="Effective Date" HeaderStyle-Width="80px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEffectiveDate" CommandName="SELECTREVISION" CommandArgument="<%# Container.DataSetIndex %>" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDEFFECTIVEDATE")) %>'></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date Width="100%" ID="ucEffectiveDateEdit" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDEFFECTIVEDATE")) %>' CssClass="input_mandatory" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date Width="100%" ID="ucEffectiveDateAdd" runat="server" CssClass="input_mandatory" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Currency" HeaderStyle-Width="50px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Currency Width="100%" ID="ucCurrencyEdit" runat="server" CssClass="input" SelectedCurrency='<%# (DataBinder.Eval(Container,"DataItem.FLDCURRENCY")) %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Currency Width="100%" ID="ucCurrencyAdd" runat="server" CssClass="input" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Overlap Wage" HeaderStyle-Width="50px">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle HorizontalAlign="Right" />

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOverlap" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVERLAPWAGE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number Width="100%" ID="ucOverlapWageEdit" runat="server" DecimalPlace="2" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVERLAPWAGE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number Width="100%" ID="ucOverlapWageAdd" runat="server" CssClass="input" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Tank Clean Allowance" HeaderStyle-Width="50px">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle HorizontalAlign="Right" />

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTankCleanAllowance" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTANKCLEANALLOWANCE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number Width="100%" ID="ucTankCleanAllowanceEdit" runat="server" DecimalPlace="2" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTANKCLEANALLOWANCE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number Width="100%" ID="ucTankCleanAllowanceAdd" runat="server" CssClass="input" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="DP Allowance" HeaderStyle-Width="50px">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle HorizontalAlign="Right" />

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDPAllowance" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDPALLOWANCE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number Width="100%" ID="ucDPAllowanceEdit" runat="server" DecimalPlace="2" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDPALLOWANCE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number Width="100%" ID="ucDPAllowanceAdd" runat="server" CssClass="input" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Other Allowance" HeaderStyle-Width="50px">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle HorizontalAlign="Right" />

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOtherAllowance" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOTHERALLOWANCE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucOtherAllowanceEdit" runat="server" DecimalPlace="2" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOTHERALLOWANCE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="ucOtherAllowanceAdd" runat="server" CssClass="input" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Appointment Remarks" HeaderStyle-Width="200px">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" />
                            <HeaderStyle Width="350px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemrks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS").ToString().Length > 30 ? DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString().Substring(0, 30) + "..." : DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString() %>'></telerik:RadLabel>
                                <eluc:Tooltip ID="ucappremark" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>' TargetControlId="lblRemrks" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox Width="100%" ID="txtRemarksEdit" runat="server" TextMode="MultiLine" Rows="2"
                                    CssClass="gridinput" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'>
                                </asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox Width="100%" ID="txtRemarksAdd" runat="server" TextMode="MultiLine" Rows="2"
                                    CssClass="gridinput">
                                </asp:TextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Offer Letter Remarks" HeaderStyle-Width="200px">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" />
                            <HeaderStyle Width="350px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOfferRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFERLETTERREMARKS").ToString().Length > 30 ? DataBinder.Eval(Container, "DataItem.FLDOFFERLETTERREMARKS").ToString().Substring(0, 30) + "..." : DataBinder.Eval(Container, "DataItem.FLDOFFERLETTERREMARKS").ToString() %>'></telerik:RadLabel>
                                <eluc:Tooltip ID="ucofferremark" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFERLETTERREMARKS") %>' TargetControlId="lblOfferRemarks" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox Width="100%" ID="txtOfferRemarksEdit" runat="server" TextMode="MultiLine" Rows="2"
                                    CssClass="gridinput" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFERLETTERREMARKS") %>'>
                                </asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox Width="100%" ID="txtOfferRemarksAdd" runat="server" TextMode="MultiLine" Rows="2"
                                    CssClass="gridinput">
                                </asp:TextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Revision No" HeaderStyle-Width="50px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRevNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></HeaderStyle>

                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandName="EDITS" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                    ToolTip="Edit">
                                         <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                    ToolTip="Delete">
                                        <span class="icon"><i class="fa fa-trash"></i></span>
                                </asp:LinkButton>
                                <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:LinkButton runat="server" AlternateText="Copy Budget from previous revision"
                                    CommandName="COPY" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCopy"
                                    ToolTip="Copy Budget from previous revision">
                                          <span class="icon"><i class="fas fa-copy"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save"
                                    CommandName="UPDATE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                    ToolTip="Save">
                                        <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:LinkButton runat="server" AlternateText="Cancel"
                                    CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                    ToolTip="Cancel">
                                         <span class="icon"><i class="fa fa-trash"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save"
                                    CommandName="Add" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAdd"
                                    ToolTip="Add New">
                                        <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
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


            <br />
            <b>
                <telerik:RadLabel ID="lblBudget" runat="server" Text="Budget"></telerik:RadLabel>
            </b>

            <eluc:TabStrip ID="MenuRegistersVesslBudget" runat="server" OnTabStripCommand="RegistersVesselBudget_TabStripCommand"></eluc:TabStrip>


            <%--  <asp:GridView ID="gvBudget" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" ShowFooter="true" OnRowCommand="gvBudget_RowCommand"
                    ShowHeader="true" EnableViewState="false"
                    DataKeyNames="FLDBUDGETID" OnRowDataBound="gvBudget_RowDataBound">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvBudget" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" ShowFooter="true" OnNeedDataSource="gvBudget_NeedDataSource"
                OnItemCommand="gvBudget_ItemCommand"
                OnItemDataBound="gvBudget_ItemDataBound"
                GroupingEnabled="false" EnableHeaderContextMenu="true"
                AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView DataKeyNames="FLDBUDGETID" EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
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
                        <telerik:GridButtonColumn Text="DoubleClick" CommandName="Edit" Visible="false" />
                        <telerik:GridTemplateColumn HeaderText="Rank">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRank" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Rank ID="ucRankEdit" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" SelectedRank='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Rank ID="ucRankAdd" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Currency">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE")) %>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Budgeted Wage/day">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWage" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETEDWAGE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucWageEdit" runat="server" DecimalPlace="2" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETEDWAGE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="ucWageAdd" runat="server" CssClass="input_mandatory" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <%--   added by karthik B--%>
                        <telerik:GridTemplateColumn HeaderText="1st Year Scale">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblyear" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLD1STYEARSCALE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucyearEdit" runat="server" DecimalPlace="2" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLD1STYEARSCALE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="ucyearAdd" runat="server" CssClass="input_mandatory" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>



                        <telerik:GridTemplateColumn Visible="false" HeaderText="Overlap Wage/day">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOverlap" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVERLAPWAGE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucOverlapWageEdit" runat="server" DecimalPlace="2" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVERLAPWAGE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="ucOverlapWageAdd" runat="server" CssClass="input" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn Visible="false" HeaderText="Other allowance/day">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOtherAllowance" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOTHERALLOWANCE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucOtherAllowanceEdit" runat="server" DecimalPlace="2" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOTHERALLOWANCE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="ucOtherAllowanceAdd" runat="server" CssClass="input" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Preferred Nationality">
                            <HeaderStyle Width="300px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="400px"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNationality" Width="400px" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDPREFERREDNATIONALITYNAME").ToString().Length>50 ? DataBinder.Eval(Container, "DataItem.FLDPREFERREDNATIONALITYNAME").ToString().Substring(0, 50)+ "..." : DataBinder.Eval(Container, "DataItem.FLDPREFERREDNATIONALITYNAME").ToString()) %>'></telerik:RadLabel>
                                <eluc:Tooltip ID="ucNationalityToolTip" runat="server" Width="350px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPREFERREDNATIONALITYNAME") %>' TargetControlId="lblNationality" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <div style="height: 100px; overflow: auto; border: 0px; width: 400px" class="input">
                                    <telerik:RadCheckBoxList ID="cblNationalityEdit" Direction="Vertical" Width="400px" Columns="2" runat="server" CssClass="input"></telerik:RadCheckBoxList>
                                </div>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <div style="height: 100px; overflow: auto; border: 0px; width: 400px" class="input">
                                    <telerik:RadCheckBoxList ID="cblNationalityAdd" Direction="Vertical" Width="400px" Columns="2" runat="server" CssClass="input"></telerik:RadCheckBoxList>
                                </div>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></HeaderStyle>

                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                    ToolTip="Edit">
                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="DELETEBUDGET" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                    ToolTip="Delete">
                                            <span class="icon"><i class="fa fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save"
                                    CommandName="UPDATE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                    ToolTip="Save">
                                            <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:LinkButton runat="server" AlternateText="Cancel"
                                    CommandName="CANCEL" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                    ToolTip="Cancel">
                                            <span class="icon"><i class="fa fa-trash"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save"
                                    CommandName="ADDBUDGET" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAdd"
                                    ToolTip="Add New">
                                            <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
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



        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
