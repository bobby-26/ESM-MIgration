<%@ Page Language="C#" AutoEventWireup="True" CodeFile="RegistersInsuranceType.aspx.cs"
    Inherits="RegistersInsuranceType" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>

<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Insurance Type</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersInsuranceType" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
            <eluc:TabStrip ID="MenuTitle" runat="server" Title="Insurance Type"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <table id="tblConfigureInsuranceType" width="50%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblInsuranceType" runat="server" Text="Insurance Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSearch" runat="server" MaxLength="100" CssClass="input"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuRegistersInsuranceType" runat="server" OnTabStripCommand="RegistersInsuranceType_TabStripCommand"></eluc:TabStrip>
            <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="RadGrid1" DecoratedControls="All" EnableRoundedCorners="true" />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvInsuranceType" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" ShowFooter="true"
                CellSpacing="0" GridLines="None" OnDeleteCommand="gvInsuranceType_DeleteCommand" OnSortCommand="gvInsuranceType_SortCommand" Height="90%"
                OnNeedDataSource="gvInsuranceType_NeedDataSource" EnableHeaderContextMenu="true"
                OnItemDataBound="gvInsuranceType_ItemDataBound" OnItemCommand="gvInsuranceType_ItemCommand" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDINSURANCETYPEID" TableLayout="Fixed">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Insurance Type" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDINSURANCETYPENAME">
                            <HeaderStyle Width="115px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInsuranceTypeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSURANCETYPEID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkInsuranceTypeName" runat="server" CommandName="EDIT"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSURANCETYPENAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblInsuranceTypeIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSURANCETYPEID") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtInsuranceTypeNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSURANCETYPENAME") %>'
                                    CssClass="gridinput_mandatory" MaxLength="200">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtInsuranceTypeNameAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="200"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Premium" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDINSURANCETYPENAME">
                            <HeaderStyle Width="115px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPremium" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPREMIUM","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Decimal runat="server" ID="txtPremiumEdit" CssClass="gridinput_mandatory txtNumber" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPREMIUM","{0:n2}") %>'
                                    MaxLength="6" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Decimal runat="server" ID="txtPremiumAdd" CssClass="gridinput_mandatory txtNumber" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPREMIUM","{0:n2}") %>'
                                    MaxLength="6" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Principal Share" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDINSURANCETYPENAME">
                            <HeaderStyle Width="115px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPrincipalShare" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRINCIPALSHARE","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Decimal runat="server" ID="txtPrincipalShareEdit" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRINCIPALSHARE","{0:n2}") %>'
                                    OnTextChanged="txtPrincipalShareEdit_TextChanged" CssClass="gridinput_mandatory txtNumber" MaxLength="200" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Decimal runat="server" ID="txtPrincipalShareAdd" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRINCIPALSHARE","{0:n2}") %>'
                                    OnTextChanged="txtPrincipalShareEdit_TextChanged" CssClass="gridinput_mandatory txtNumber" MaxLength="200" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Crew Share" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDCREWSHARE">
                            <HeaderStyle Width="115px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCrewShare" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWSHARE","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtCrewShareEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWSHARE","{0:n2}") %>'
                                    Enabled="false" CssClass="gridinput txtNumber readonlytextbox" MaxLength="200">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtCrewShareEdit" runat="server" CssClass="gridinput txtNumber readonlytextbox" MaxLength="200"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="End Of Leave">
                            <HeaderStyle Width="115px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPrincipalShareTillEndOfLeave" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDPRINCIPALSHARETILLENDOFLEAVE").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkPrincipalShareTillEndOfLeaveEdit" runat="server" AutoPostBack="false" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDPRINCIPALSHARETILLENDOFLEAVE").ToString().Equals("1"))?true:false %>'></asp:CheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:CheckBox ID="chkPrincipalShareTillEndOfLeaveAdd" runat="server" AutoPostBack="false" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Leave Per Day">
                            <HeaderStyle Width="115px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLeavePerDay" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEAVEPERDAY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <%--<telerik:RadTextBox ID="txtLeavePerDayEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEAVEPERDAY") %>'
                                    CssClass="gridinput_mandatory txtNumber" MaxLength="200">
                                </telerik:RadTextBox>--%>
                                <%--<ajaxToolkit:MaskedEditExtender ID="maskeditLeavePerDayEdit" runat="server" TargetControlID="txtLeavePerDayEdit"
                                        Mask="99" MaskType="Number" InputDirection="RightToLeft" AutoComplete="false">
                                    </ajaxToolkit:MaskedEditExtender>--%>
                                <eluc:Decimal runat="server" ID="txtLeavePerDayEdit" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEAVEPERDAY") %>'
                                    CssClass="gridinput_mandatory txtNumber" MaxLength="200" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Decimal runat="server" ID="txtLeavePerDayAdd" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEAVEPERDAY") %>'
                                    CssClass="gridinput_mandatory txtNumber" MaxLength="200" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                            <HeaderStyle Width="40px" />
                            <ItemStyle Width="40px" Wrap="false" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
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
