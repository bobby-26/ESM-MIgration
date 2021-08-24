<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreManningScale.aspx.cs" Inherits="CrewOffshore_CrewOffshoreManningScale" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationality.ascx" %>
<%@ Register TagName="AddrType" TagPrefix="eluc" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Certificate" Src="~/UserControls/UserControlCertificate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskText" Src="~/UserControls/UserControlMaskedTextBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Vessel ManningScale</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>

</head>
<body>

    <form id="frmRegistersManningScale" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%; position: absolute;">

                <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />


                <eluc:TabStrip ID="CrewQuery" runat="server" TabStrip="true" OnTabStripCommand="CrewQuery_TabStripCommand"></eluc:TabStrip>

                <div id="divFind" style="position: relative; z-index: +1;">
                    <table id="tblConfigureManningScale" width="100%">
                        <tr>
                            <td>Vessel
                            </td>
                            <%-- <td>
                                <eluc:Vessel ID="UcVessel" runat="server" CssClass="input" AppendDataBoundItems="true"
                                    AutoPostBack="true" OnTextChangedEvent="ucVessel_Changed" />
                            </td>--%>
                            <td>
                                <telerik:RadTextBox ID="txtVessel" runat="server" MaxLength="100" ReadOnly="true" CssClass="readonlytextbox"
                                    Width="260px">
                                </telerik:RadTextBox>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>Owner Scale Total
                            </td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtOwnerScaleTotal" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                            </td>
                            <td>Safe Scale Total
                            </td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtSafeScaleTotal" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <b>
                    <telerik:RadLabel ID="lblRev" runat="server" Text="Revision"></telerik:RadLabel>
                </b>

                <eluc:TabStrip ID="MenuRegistersRevision" runat="server" OnTabStripCommand="MenuRegistersRevision_TabStripCommand"></eluc:TabStrip>

                <div id="div1" style="position: relative; z-index: 2">
                    <%-- <asp:GridView ID="gvManningScaleRevision" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" ShowFooter="true" OnRowCommand="gvManningScaleRevision_RowCommand"
                    ShowHeader="true" EnableViewState="false"
                    DataKeyNames="FLDREVISIONID" OnRowDataBound="gvManningScaleRevision_RowDataBound">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvManningScaleRevision" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" ShowFooter="true" OnNeedDataSource="gvManningScaleRevision_NeedDataSource"
                        OnItemCommand="gvManningScaleRevision_ItemCommand"
                        OnItemDataBound="gvManningScaleRevision_ItemDataBound"
                        OnSortCommand="gvManningScaleRevision_SortCommand"
                        GroupingEnabled="false" EnableHeaderContextMenu="true"
                        AutoGenerateColumns="false" MasterTableView-ShowFooter="true">
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
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblEffectiveDateHeader" runat="server" Text="Effective Date"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEffectiveDate" CommandName="SELECTREVISION" CommandArgument="<%# Container.DataSetIndex %>" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDEFFECTIVEDATE")) %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Date ID="ucEffectiveDateEdit" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDEFFECTIVEDATE")) %>' CssClass="input_mandatory" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:Date ID="ucEffectiveDateAdd" runat="server" CssClass="input_mandatory" />
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblRevNoHeader" runat="server" Text="Revision No"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblRevNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONNO") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></HeaderStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblActionHeader" runat="server">Action</telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Edit"
                                            CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                            ToolTip="Edit">
                                     <span class="icon"><i class="fas fa-edit"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Delete"
                                            CommandName="DELETEREVISION" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                            ToolTip="Delete">
                                     <span class="icon"><i class="fa fa-trash"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Copy Manning Scale from previous revision"
                                            CommandName="COPY" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCopy"
                                            ToolTip="Copy Manning Scale from previous revision">
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
                                            CommandName="CANCEL" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                            ToolTip="Cancel">
                                     <span class="icon"><i class="fa fa-trash"></i></span>
                                        </asp:LinkButton>
                                    </EditItemTemplate>
                                    <FooterStyle HorizontalAlign="Center" />
                                    <FooterTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Save"
                                            CommandName="ADDREVISION" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAdd"
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
                </div>

                <br />
                <b>
                    <telerik:RadLabel ID="lblManningScale" runat="server" Text="Manning Scale"></telerik:RadLabel>
                </b>

                <eluc:TabStrip ID="MenuRegistersManningScale" runat="server" OnTabStripCommand="RegistersManningScale_TabStripCommand"></eluc:TabStrip>

                <div id="divGrid" style="position: relative; z-index: +1;">
                    <%-- <asp:GridView ID="gvManningScale" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" OnRowCommand="gvManningScale_RowCommand" OnRowDataBound="gvManningScale_ItemDataBound"
                    OnRowCancelingEdit="gvManningScale_RowCancelingEdit" OnRowDeleting="gvManningScale_RowDeleting"
                    OnRowEditing="gvManningScale_RowEditing" ShowFooter="true" ShowHeader="true"
                    EnableViewState="false" OnSorting="gvManningScale_Sorting" AllowSorting="true"
                    OnRowCreated="gvManningScale_RowCreated" OnSelectedIndexChanging="gvManningScale_SelectedIndexChanging"
                    OnRowUpdating="gvManningScale_RowUpdating">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                    <RowStyle Height="10px" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvManningScale" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" OnNeedDataSource="gvManningScale_NeedDataSource"
                        OnItemCommand="gvManningScale_ItemCommand"
                        OnSortCommand="gvManningScale_SortCommand"
                        OnItemDataBound="gvManningScale_ItemDataBound1"
                        GroupingEnabled="false" EnableHeaderContextMenu="true"
                        AutoGenerateColumns="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" TableLayout="Fixed" ShowFooter="true">
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
                                <telerik:GridTemplateColumn HeaderText="Rank">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" ID="cmdHiddenSubmit" OnClick="cmdSearch_Click" CommandName="FLDRANKNAME"
                                            ImageUrl="<%$ PhoenixTheme:images/spacer.png %>" CommandArgument="1" />
                                        <asp:LinkButton ID="lnkDoubleClick" runat="server" Visible="false" CommandName="Edit"
                                            CommandArgument='<%# Container.DataSetIndex %>'></asp:LinkButton>
                                        <telerik:RadLabel ID="lblManningScaleId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMANNINGSCALEID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                        <asp:LinkButton ID="lnkRankEdit" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>'
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="lnkDoubleClickEdit" runat="server" Visible="false" CommandName="Edit"
                                            CommandArgument='<%# Container.DataSetIndex %>'></asp:LinkButton>
                                        <telerik:RadLabel ID="lblManningScaleIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMANNINGSCALEID") %>'></telerik:RadLabel>
                                        <eluc:Rank ID="ucRank" RankList='<%# PhoenixRegistersRank.ListRank() %>' runat="server"
                                            CssClass="dropdown_mandatory" AppendDataBoundItems="true" SelectedRank='<%# DataBinder.Eval(Container, "DataItem.FLDRANK") %>' />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:Rank ID="ucRankAdd" RankList='<%# PhoenixRegistersRank.ListRank() %>' runat="server"
                                            CssClass="dropdown_mandatory" AppendDataBoundItems="true" />
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Equivalent Ranks">
                                    <ItemStyle Wrap="false" HorizontalAlign="Center" />

                                    <ItemTemplate>
                                        <asp:LinkButton ID="imgGroupList" runat="server">
                                            <span class="icon"><i class="fas fa-binoculars"></i></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Owner Scale">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>


                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblOwnerScale" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERSCALE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:MaskText ID="txtOwnerScaleEdit" runat="server" MaskText="##" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERSCALE") %>' />


                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:MaskText ID="txtOwnerScaleAdd" runat="server" CssClass="gridinput_mandatory"
                                            MaxLength="200" MaskText="##" Tooltip="Enter Owner Scale" Style="text-align: right;" />

                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Safe Scale">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblSafeScale" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSAFESCALE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:MaskText ID="txtSafeScaleEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSAFESCALE") %>'
                                            CssClass="gridinput_mandatory" MaxLength="200" MaskText="##" Style="text-align: right;"></eluc:MaskText>

                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:MaskText ID="txtSafeScaleAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="200"
                                            Tooltip="Enter Safe Scale" Style="text-align: right;" MaskText="##"></eluc:MaskText>

                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="CBA Scale">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblCBAScale" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCBASCALE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:MaskText ID="txtCBAScaleEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCBASCALE") %>'
                                            CssClass="gridinput_mandatory" MaxLength="200" Style="text-align: right;" MaskText="##"></eluc:MaskText>

                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:MaskText ID="txtCBAScaleAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="200"
                                            Tooltip="Enter CBA Scale" Style="text-align: right;" MaskText="##"></eluc:MaskText>

                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Contract Period(days)">
                                    <ItemStyle Wrap="false" HorizontalAlign="Right" />

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblContractPeriod" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTRACTPERIODDAYS") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:MaskText ID="txtContractPeriodEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTRACTPERIODDAYS") %>'
                                            CssClass="gridinput_mandatory" MaxLength="200" Style="text-align: right;" MaskText="###"></eluc:MaskText>

                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:MaskText ID="txtContractPeriodAdd" runat="server" CssClass="gridinput_mandatory"
                                            MaxLength="200" Tooltip="Enter Contract Period in Days" Style="text-align: right;" MaskText="###"></eluc:MaskText>

                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Remarks">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox ID="txtRemarksEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'
                                            CssClass="gridinput" MaxLength="100">
                                        </telerik:RadTextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadTextBox ID="txtRemarksAdd" runat="server" CssClass="gridinput" MaxLength="100"></telerik:RadTextBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Action">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Edit"
                                            CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                            ToolTip="Edit">
                                             <span class="icon"><i class="fas fa-edit"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Delete"
                                            CommandName="DELETE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                            ToolTip="Delete">
                                             <span class="icon"><i class="fa fa-trash"></i></span>
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" AlternateText="Ship Ack."
                                            CommandName="MapEquivalentRank" CommandArgument='<%# Container.DataSetIndex %>'
                                            ID="cmdEquivalentRank" ToolTip="Map Equivalent Rank">
                                            <span class="icon"><i class="fas fa-clipboard-list"></i></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Save"
                                            CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                            ToolTip="Save">
                                            <span class="icon"><i class="fas fa-save"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Cancel"
                                            CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                            ToolTip="Cancel">
                                             <span class="icon"><i class="fa fa-trash"></i></span>
                                        </asp:LinkButton>
                                    </EditItemTemplate>
                                    <FooterStyle HorizontalAlign="Center" />
                                    <FooterTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Save"
                                            CommandName="Add" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
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
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </div>

            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
