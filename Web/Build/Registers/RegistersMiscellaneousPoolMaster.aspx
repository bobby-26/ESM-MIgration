<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersMiscellaneousPoolMaster.aspx.cs"
    Inherits="RegistersMiscellaneousPoolMaster" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPool.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Pool</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersPoolMaster" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="RadGrid1" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="98%">           
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <table id="tblGuidance" runat="server">
                <tr>
                    <td colspan="4">
                        <telerik:RadLabel ID="lblNote" runat="server" Text="Note: please map the 'contract company' (for each 'pool') for which the contract would be generated. The same would reflect in the header of the crew contract generated."
                            ForeColor="Blue" Font-Bold="true">
                        </telerik:RadLabel>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuRegistersPoolMaster" runat="server" OnTabStripCommand="RegistersRegistersPoolMaster_TabStripCommand"></eluc:TabStrip>    
            <telerik:RadGrid RenderMode="Lightweight" ID="gvPoolMaster" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnDeleteCommand="gvPoolMaster_DeleteCommand" OnSortCommand="gvPoolMaster_SortCommand" ShowFooter="true"
                OnNeedDataSource="gvPoolMaster_NeedDataSource" Height="88%" EnableHeaderContextMenu="true" OnUpdateCommand="gvPoolMaster_UpdateCommand"
                OnItemDataBound="gvPoolMaster_ItemDataBound" OnItemCommand="gvPoolMaster_ItemCommand" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDPOOLID" TableLayout="Fixed">
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
                        <telerik:GridTemplateColumn HeaderText="Pool" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDPOOLNAME">
                            <HeaderStyle Width="108px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPoolId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOOLID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblPoolName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOOLNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblPoolIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOOLID") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtPoolNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOOLNAME") %>'
                                    CssClass="gridinput_mandatory" MaxLength="20">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtPoolNameAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="50"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Description">
                            <HeaderStyle Width="200px" Wrap="false" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtDescriptionEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'
                                    CssClass="gridinput" MaxLength="50">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterStyle Width="200PX" />
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtDescriptionAdd" runat="server" Width="300PX" CssClass="gridinput" MaxLength="50"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Parent Pool" ShowSortIcon="true">
                            <HeaderStyle Width="87px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblParentPool" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARENTPOOLNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblParentpool" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPARENTPOOL") %>'></telerik:RadLabel>
                                <eluc:Pool ID="ddlParentPoolEdit" runat="server" AppendDataBoundItems="true" Width="100%" PoolList="<%#PhoenixRegistersMiscellaneousPoolMaster.ListMiscellaneousPoolMaster() %>" SelectedPool='<%# DataBinder.Eval(Container,"DataItem.FLDPARENTPOOL") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Pool ID="ddlParentPoolAdd" runat="server" AppendDataBoundItems="true" Width="100%"  PoolList="<%#PhoenixRegistersMiscellaneousPoolMaster.ListMiscellaneousPoolMaster() %>" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Contract Company" ShowSortIcon="true">
                            <HeaderStyle Width="95px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblContractCompany" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTRACTCOMPANYSHORTCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblCompanyId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCONTRACTCOMPANY") %>'></telerik:RadLabel>
                                <eluc:Company ID="ddlCompanyEdit" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>' Width="100%" 
                                    runat="server" SelectedCompany='<%# DataBinder.Eval(Container, "DataItem.FLDCONTRACTCOMPANY") %>'
                                    AppendDataBoundItems="true" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Company ID="ddlCompanyAdd" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>' Width="100%" 
                                    runat="server" SelectedCompany='<%# DataBinder.Eval(Container, "DataItem.FLDCONTRACTCOMPANY") %>'
                                    AppendDataBoundItems="true" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Direct Sign-On" ShowSortIcon="true">
                            <HeaderStyle Width="95px" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDDIRECTSIGNON").ToString() =="1" ? "Yes" : "No"%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkDirectSignonEdit" runat="server" AutoPostBack="false" Checked='<%# DataBinder.Eval(Container, "DataItem.FLDDIRECTSIGNON").ToString() =="1" ? true : false%>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkDirectSignonAdd" runat="server" AutoPostBack="false" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Service Sync" ShowSortIcon="true">
                            <HeaderStyle Width="95px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblServiceSync" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSERVICESYNCYN").ToString() =="1" ? "Yes" : "No"%>'></telerik:RadLabel>                                
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkServiceSyncEdit" runat="server" AutoPostBack="false" Checked='<%# DataBinder.Eval(Container, "DataItem.FLDSERVICESYNCYN").ToString() =="1" ? true : false%>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkServiceSyncAdd" runat="server" AutoPostBack="false" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" HeaderStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                            <HeaderStyle Width="60px" />
                            <ItemStyle Width="60px" Wrap="false" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Map Other Document" CommandName="MAP" ID="cmdMap" ToolTip="Map Other Document">
                                     <span class="icon"><i class="fas fa-list-alt"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ID="cmdAdd" CommandName="Add" ToolTip="Add New" Width="20px" Height="20px">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="2" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
