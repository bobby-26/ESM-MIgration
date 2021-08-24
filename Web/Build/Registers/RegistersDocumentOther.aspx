<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersDocumentOther.aspx.cs"
    Inherits="RegistersDocumentOther" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserGroup" Src="~/UserControls/UserControlUserGroup.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="DocumentCategory" Src="~/UserControls/UserControlDocumentCategory.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Other Documents</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersDocumentOther" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <%--<eluc:TabStrip ID="MenuTitle" runat="server" Title="Other Document"></eluc:TabStrip>--%>
        <telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server" Height="90%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <table id="tblConfigureDocumentOther">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDocumentName" runat="server" Text="Document Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSearchOtherDocuments" runat="server" MaxLength="100" CssClass="input"
                            Width="360px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkincludeinactive" Text="Include Inactive" runat="server"></telerik:RadCheckBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuRegistersDocumentOther" runat="server" OnTabStripCommand="RegistersDocumentOther_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvDocumentOther" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvDocumentOther_ItemCommand" OnNeedDataSource="gvDocumentOther_NeedDataSource" Height="95%"
                OnItemDataBound="gvDocumentOther_ItemDataBound" EnableViewState="false" GroupingEnabled="false" OnSortCommand="gvDocumentOther_SortCommand"
                EnableHeaderContextMenu="true" ShowFooter="false">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center"
                    CommandItemDisplay="None">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <%--<asp:GridView ID="gvDocumentOther" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvDocumentOther_RowCommand" OnRowDataBound="gvDocumentOther_ItemDataBound"
                        OnRowCancelingEdit="gvDocumentOther_RowCancelingEdit" OnRowDeleting="gvDocumentOther_RowDeleting"
                        OnRowEditing="gvDocumentOther_RowEditing" ShowFooter="true" ShowHeader="true"
                        EnableViewState="true" OnSorting="gvDocumentOther_Sorting" AllowSorting="true"
                        OnRowCreated="gvDocumentOther_RowCreated" OnSelectedIndexChanging="gvDocumentOther_SelectedIndexChanging"
                        OnRowUpdating="gvDocumentOther_RowUpdating">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />--%>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Document Code" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblDocumentCodeHeader" Text="Document Code" runat="server"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDocumentCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtDocumentCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTCODE") %>'
                                    CssClass="gridinput_mandatory" MaxLength="200">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtDocumentCodeAdd" runat="server" CssClass="gridinput_mandatory"
                                    MaxLength="200" ToolTip="Enter Document Code">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="120px" HeaderText="Document Category" HeaderStyle-HorizontalAlign="Left">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblCategoryHeader" runat="server" Text="Document Category"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCategoryName" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME"))%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:DocumentCategory ID="ucCategoryEdit" runat="server" RankList="<%# PhoenixRegistersDocumentCategory.ListDocumentCategory(1,null,null,null) %>" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:DocumentCategory ID="ucCategoryAdd" runat="server" RankList="<%# PhoenixRegistersDocumentCategory.ListDocumentCategory(1,null,null,null) %>" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Document Name" HeaderStyle-Width="210px" HeaderStyle-HorizontalAlign="center">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <%--    <asp:ImageButton runat="server" ID="cmdHiddenSubmit" OnClick="cmdSearch_Click" CommandName="FLDDOCUMENTNAME"
                                        ImageUrl="<%$ PhoenixTheme:images/spacer.png %>" CommandArgument="1" />--%>
                                <asp:LinkButton ID="lblDocumentNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDDOCUMENTNAME">
                                        Document Name&nbsp;</asp:LinkButton>
                                <%--     <img id="FLDDOCUMENTNAME" runat="server" visible="false" />--%>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDoubleClick" runat="server" Visible="false" CommandName="Edit"></asp:LinkButton>
                                <telerik:RadLabel ID="lblDocumentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkDocumentName" runat="server" CommandName="EDIT"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="lnkDoubleClickEdit" runat="server" Visible="false" CommandName="Edit"></asp:LinkButton>
                                <telerik:RadLabel ID="lblDocumentIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTID") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtDocumentNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>'
                                    CssClass="gridinput_mandatory" MaxLength="200">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtDocumentNameAdd" runat="server" CssClass="gridinput_mandatory"
                                    MaxLength="200" ToolTip="Enter Document Name">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderStyle-Width="50px" HeaderText="Rank">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblRankHeader" runat="server" Text="Rank"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lblRankNameList" AlternateText="Travel" Width="20PX" Height="20PX"
                                    CommandName="RANKLIST" CommandArgument="<%# Container.DataSetIndex %>" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDRANKNAME"))%>'>                          
                                <span class="icon"><i class="fas fa-bell-slash"></i></span></asp:LinkButton>
                                <%--<telerik:RadLabel ID="lblRankName" runat="server" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDRANKNAME"))%>' Text='<%# (DataBinder.Eval(Container,"DataItem.FLDRANKNAME"))%>'></telerik:RadLabel>--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="50px" HeaderText="Vessel Type" HeaderStyle-Wrap="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lblVesselType" AlternateText="Travel" Width="20PX" Height="20PX"
                                    CommandName="VESSELTYPELIST" CommandArgument="<%# Container.DataSetIndex %>" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE"))%>'>                          
                                <span class="icon"><i class="fas fa-bell-slash"></i></span></asp:LinkButton>
                                <%--<telerik:RadLabel ID="lblVesselType" runat="server" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE"))%>' Text='<%# (DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE"))%>'></telerik:RadLabel>--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="50px" HeaderText="Owner" HeaderStyle-Wrap="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblVesselTypeHeader" runat="server" Text="Owner"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lblowner" AlternateText="Travel" Width="20PX" Height="20PX"
                                    CommandName="VESSELTYPELIST" CommandArgument="<%# Container.DataSetIndex %>" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDOWNERLIST"))%>'>                          
                                <span class="icon"><i class="fas fa-bell-slash"></i></span></asp:LinkButton>
                                <%--<telerik:RadLabel ID="lblVesselType" runat="server" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE"))%>' Text='<%# (DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE"))%>'></telerik:RadLabel>--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="50px" HeaderText="Company" HeaderStyle-Wrap="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lblCompany" AlternateText="Travel" Width="20PX" Height="20PX"
                                    CommandName="VESSELTYPELIST" CommandArgument="<%# Container.DataSetIndex %>" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDCOMPANIES"))%>'>                          
                                <span class="icon"><i class="fas fa-bell-slash"></i></span></asp:LinkButton>
                                <%--<telerik:RadLabel ID="lblVesselType" runat="server" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE"))%>' Text='<%# (DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE"))%>'></telerik:RadLabel>--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="50px" HeaderText="Flag" HeaderStyle-Wrap="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lblFlag" AlternateText="Travel" Width="20PX" Height="20PX"
                                    CommandName="VESSELTYPELIST" CommandArgument="<%# Container.DataSetIndex %>" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDFLAG"))%>'>                          
                                <span class="icon"><i class="fas fa-bell-slash"></i></span></asp:LinkButton>
                                <%--<telerik:RadLabel ID="lblVesselType" runat="server" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE"))%>' Text='<%# (DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE"))%>'></telerik:RadLabel>--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Active Y/N" HeaderStyle-Width="65px" HeaderStyle-Wrap="true">
                            <ItemStyle Wrap="False" HorizontalAlign="center"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblLocalActiveHeader" runat="server">
                                    Active Y/N
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLocalActive" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDLOCALACTIVE").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkLocalActiveEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDLOCALACTIVE").ToString().Equals("1"))?true:false %>'></telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkLocalActiveAdd" runat="server"></telerik:RadCheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Group" HeaderStyle-Width="160px" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblGroupHeader" runat="server">
                                    Group&nbsp;
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblGroup" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUPNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Hard runat="server" ID="ucGroupEdit" ShortNameFilter="CDC,VSA,NON,WPT" HardTypeCode='<%# ((int)PhoenixHardTypeCode.CREWDOCUMENTTYPE).ToString() %>'
                                    HardList='<%# PhoenixRegistersHard.ListHard(0, ((int)PhoenixHardTypeCode.CREWDOCUMENTTYPE), byte.Parse("0"), "CDC,VSA,NON,WPT") %>'
                                    SelectedHard='<%# DataBinder.Eval(Container,"DataItem.FLDGROUP") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Hard runat="server" ID="ucGroupAdd" ShortNameFilter="CDC,VSA,NON,WPT" HardTypeCode='<%# ((int)PhoenixHardTypeCode.CREWDOCUMENTTYPE).ToString() %>'
                                    HardList='<%# PhoenixRegistersHard.ListHard(0, ((int)PhoenixHardTypeCode.CREWDOCUMENTTYPE), byte.Parse("0"), "CDC,VSA,NON,WPT") %>' />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Having Expiry" HeaderStyle-Width="70px" HeaderStyle-Wrap="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblHavingExpiryHeader" runat="server">
                                    Having Expiry
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblHavingExpiry" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDHAVINGEXPIRY").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkHavingExpiryEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDHAVINGEXPIRY").ToString().Equals("1"))?true:false %>'></telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkHavingExpiryAdd" runat="server"></telerik:RadCheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Short Expiry" HeaderStyle-Width="70px" HeaderStyle-Wrap="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblShortExpiryHeader" runat="server">
                                    Short Expiry
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblShortExpiry" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDSHORTEXPIRY").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkShortExpiryEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDSHORTEXPIRY").ToString().Equals("1"))?true:false %>'></telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkShortExpiryAdd" runat="server"></telerik:RadCheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Nok Y/N" HeaderStyle-Width="60px">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblNokYnHeader" runat="server">
                                    Nok Y/N
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNokYn" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDNOKYN").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkNokYnEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDNOKYN").ToString().Equals("1"))?true:false %>'></telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkNokYnAdd" runat="server"></telerik:RadCheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Offshore Stage" HeaderStyle-Width="160px">
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblStageHeader" runat="server" Text="Offshore Stage"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStageName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTAGE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadComboBox ID="ddlStageEdit" runat="server" AppendDataBoundItems="true" AllowCustomText="true" EmptyMessage="Type to Select">
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadComboBox ID="ddlStageAdd" runat="server" AppendDataBoundItems="true" AllowCustomText="true" EmptyMessage="Type to Select">
                                </telerik:RadComboBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Mandatory Y/N" HeaderStyle-Width="70px" HeaderStyle-Wrap="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblMandatoryHeader" runat="server" Text="Mandatory Y/N"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMandatoryYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDMANDATORYYN").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkMandatoryYNEdit" runat="server" AutoPostBack="true" OnCheckedChanged="chkMandatoryYNEdit_CheckedChanged"
                                    Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDMANDATORYYN").ToString().Equals("1"))?true:false %>'>
                                </telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkMandatoryYNAdd" runat="server" AutoPostBack="true" OnCheckedChanged="chkMandatoryYNAdd_CheckedChanged"></telerik:RadCheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Waiver Y/N" HeaderStyle-Width="70px" HeaderStyle-Wrap="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblWaiverHeader" runat="server" Text="Waiver Y/N"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWaiverYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDWAIVERYN").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkWaiverYNEdit" Enabled="false" runat="server" AutoPostBack="true"
                                    OnCheckedChanged="chkWaiverYNEdit_CheckedChanged" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDWAIVERYN").ToString().Equals("1"))?true:false %>'>
                                </telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkWaiverYNAdd" runat="server" Enabled="false" AutoPostBack="true"
                                    OnCheckedChanged="chkWaiverYNAdd_CheckedChanged">
                                </telerik:RadCheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="User Group to allow Waiver" HeaderStyle-Width="240px">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblUsergroupHeader" runat="server" Text="User Group to allow Waiver"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblUserGroup" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERGROUPNAME") %>' CssClass="tooltip" ClientIDMode="AutoID"></telerik:RadLabel>
                                <%-- <asp:LinkButton ID="ImgUserGroup" runat="server"><i class="fas fa-glasses"></i> </asp:LinkButton>
                                   <asp:ImageButton ID="ImgUserGroup" runat="server" ImageUrl="<%$ PhoenixTheme:images/te_view.png%>"
                                        CommandArgument='<%# Container.DataItemIndex %>'></asp:ImageButton>--%>
                                <eluc:Tooltip ID="ucUserGroup" Width="200px" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERGROUPNAME") %>' TargetControlId="lblUserGroup" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <%--<eluc:UserGroup runat="server" ID="ucUserGroupEdit" CssClass="input" Enabled="false" AppendDataBoundItems="true" />--%>
                                <div style="height: 100px; overflow: auto; width: 300px;">
                                    <telerik:RadCheckBoxList ID="chkUserGroupEdit" Direction="Vertical" Enabled="false"
                                        runat="server">
                                    </telerik:RadCheckBoxList>
                                </div>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <%--<eluc:UserGroup runat="server" ID="ucUserGroupAdd" Enabled="false" CssClass="input" AppendDataBoundItems="true" />--%>
                                <div style="height: 100px; overflow: auto; width: 300px;">
                                    <telerik:RadCheckBoxList ID="chkUserGroupAdd" RepeatDirection="Vertical" Enabled="false" runat="server"></telerik:RadCheckBoxList>
                                </div>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Addition Doc. Y/N" HeaderStyle-Width="120px" HeaderStyle-Wrap="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <%-- <HeaderTemplate>
                                <telerik:RadLabel ID="lblAdditionDocYnHeader" runat="server" Text='Show in "Additional Documents" on Crew Planner Y/N'>
                                </telerik:RadLabel>
                            </HeaderTemplate>--%>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAdditionDocYn" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDADDITIONALDOCYN").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkAdditionDocYnEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDADDITIONALDOCYN").ToString().Equals("1"))?true:false %>'></telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkAdditionDocYnAdd" runat="server"></telerik:RadCheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Requires Authentication Y/N" HeaderStyle-Width="120px" HeaderStyle-Wrap="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <%--  <HeaderTemplate>
                                <telerik:RadLabel ID="lblAuthReqYnHeader" runat="server" Text='Requires Authentication Y/N'>
                                </telerik:RadLabel>
                            </HeaderTemplate>--%>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAuthReqYnYn" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDAUTHENTICATIONREQYN").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkAuthReqYnEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDAUTHENTICATIONREQYN").ToString().Equals("1"))?true:false %>'></telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkAuthReqYnAdd" runat="server"></telerik:RadCheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CBA Other Document Y/N" HeaderStyle-Width="130px" HeaderStyle-Wrap="true">
                            <ItemStyle Wrap="False" HorizontalAlign="center"></ItemStyle>
                            <%-- <HeaderTemplate>
                                <telerik:RadLabel ID="lblCBAOtherDocumentHeader" runat="server">
                                    CBA Other Document Y/N
                                </telerik:RadLabel>
                            </HeaderTemplate>--%>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCBAOtherDocument" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDCBAOTHERMEMBERSHIPYN").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkCBAOtherDocumentEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDCBAOTHERMEMBERSHIPYN").ToString().Equals("1"))?true:false %>'
                                    AutoPostBack="false">
                                </telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkCBAOtherDocumentAdd" runat="server" AutoPostBack="false"></telerik:RadCheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Show in Master's checklist onboard Y/N" HeaderStyle-Width="150px" HeaderStyle-Wrap="true">
                            <ItemStyle Wrap="False" HorizontalAlign="center"></ItemStyle>
                            <%--  <HeaderTemplate>
                                <telerik:RadLabel ID="lblShowInMasterChecklistYnHeader" runat="server" Text="Show in Master's checklist onboard Y/N">
                                </telerik:RadLabel>
                            </HeaderTemplate>--%>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblShowInMasterChecklistYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDSHOWINMASTERCHECKLISTYN").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkShowInMasterChecklistYNEdit" runat="server" AutoPostBack="false" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDSHOWINMASTERCHECKLISTYN").ToString().Equals("1"))?true:false %>'></telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkShowInMasterChecklistYNAdd" runat="server" AutoPostBack="false"></telerik:RadCheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Photocopy Acceptable Y/N" HeaderStyle-Width="140px" HeaderStyle-Wrap="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <%--   <HeaderTemplate>
                                <telerik:RadLabel ID="lblPhotocopyAcceptableYnHeader" runat="server" Text="Photocopy Acceptable Y/N">
                                </telerik:RadLabel>
                            </HeaderTemplate>--%>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPhotocopyAcceptableYn" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDPHOTOCOPYACCEPTABLEYN").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkPhotocopyAcceptableYnEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDPHOTOCOPYACCEPTABLEYN").ToString().Equals("1"))?true:false %>'
                                    AutoPostBack="false">
                                </telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkPhotocopyAcceptableYnAdd" runat="server" AutoPostBack="false"></telerik:RadCheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="60px" HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">
                                    Action
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" ToolTip="Edit" Width="20PX" Height="20PX"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" ToolTip="Delete" Width="20PX" Height="20PX"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ToolTip="Save" Width="20PX" Height="20PX"
                                    CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" ToolTip="Cancel" Width="20PX" Height="20PX"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel">
                                 <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Add" ToolTip="Add New" Width="20PX" Height="20PX"
                                    CommandName="Add" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd">
                                 <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                            <%-- <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Update" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                        CommandName="Add" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAdd"
                                        ToolTip="Add New"></asp:ImageButton>
                                </FooterTemplate>--%>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="false" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="3" EnableColumnClientFreeze="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
