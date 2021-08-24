<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersCrewReimbursementRecovery.aspx.cs" Inherits="Registers_RegistersCrewReimbursementRecovery" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ReimbursementRecovery" Src="~/UserControls/UserControlReimbursementRecovery.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Budget" Src="~/UserControls/UserControlBudgetCode.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ReimbursementRecovery</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>

<body>
    <form id="frmRegistersReimbursementRecovery" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="93%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <table id="tblConfigureReimbursementRecovery" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCode" runat="server" Text="Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRemReCode" runat="server" MaxLength="6" CssClass="input"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSearch" runat="server" MaxLength="100" CssClass="input"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox RenderMode="Lightweight" ID="ddlReimbursementRecoveryType" runat="server"
                            Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True" EmptyMessage="Type to select">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="--Select--"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="0" Text="REIMBURSEMENT"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="1" Text="RECOVERY"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblactive" runat="server" Text="InActive"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkReimbursementRecoveryActiveYN" runat="server"></telerik:RadCheckBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuRegistersReimbursementRecovery" runat="server" OnTabStripCommand="RegistersRegistersReimbursementRecovery_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvReimbursementRecovery" Height="90%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" ShowFooter="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvReimbursementRecovery_ItemCommand" OnItemDataBound="gvReimbursementRecovery_ItemDataBound"
                OnUpdateCommand="gvReimbursementRecovery_UpdateCommand" OnNeedDataSource="gvReimbursementRecovery_NeedDataSource" EnableHeaderContextMenu="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDHARDCODE">
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
                        <telerik:GridTemplateColumn HeaderText="Code" AllowSorting="true" SortExpression="FLDSHORTCODE">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReimbursementRecovery" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblHardCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtReimbursementRecoveryCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'
                                    CssClass="gridinput_mandatory" MaxLength="6" Width="100%">
                                </telerik:RadTextBox>
                                <telerik:RadLabel ID="lblHardCodeEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDCODE") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtReimbursementRecoveryNameAdd" runat="server" CssClass="gridinput_mandatory" Width="100%"
                                    MaxLength="6">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="true" SortExpression="FLDHARDNAME">
                            <HeaderStyle Width="23%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPurposeCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDCODE") %>'></telerik:RadLabel>
                                <%--                                    <asp:LinkButton ID="lnkPurposeName" runat="server" CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></asp:LinkButton>--%>
                                <telerik:RadLabel ID="lblPurposeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblPurposeCodeEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDCODE") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtPurposeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'
                                    CssClass="gridinput_mandatory" MaxLength="200" Width="100%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtPurposeAdd" runat="server" CssClass="gridinput_mandatory"
                                    MaxLength="100" ToolTip="Enter Purpose Name">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type" AllowSorting="true" SortExpression="FLDTYPE">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPurposeTypeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lnkPurposeTypeName" runat="server"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE").ToString() == "0" ? "Reimbursement" : "Recovery" %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadComboBox ID="ddlReimbursementRecoveryTypeEdit" CssClass="input_mandatory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'
                                    Width="100%" Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True" EmptyMessage="Type to select">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="" Text="--Select--"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Value="0" Text="Reimbursement"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Value="1" Text="RecoverY"></telerik:RadComboBoxItem>
                                    </Items>
                                </telerik:RadComboBox>
                                <telerik:RadLabel ID="lblReimbursementRecoveryTypeEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadComboBox ID="ddlReimbursementRecoveryAdd" CssClass="input_mandatory" runat="server" Width="100%"
                                    Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True" EmptyMessage="Type to select">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="" Text="--Select--"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Value="0" Text="Reimbursement"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Value="1" Text="RecoverY"></telerik:RadComboBoxItem>
                                    </Items>
                                </telerik:RadComboBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Active?" AllowSorting="true" SortExpression="FLDACTIVEYN">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDACTIVEYN").ToString() == "1" ? "Yes" : "No"%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkReimbursementRecoveryActiveYNEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDACTIVEYN").ToString().Equals("1"))?true:false %>'></asp:CheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:CheckBox ID="chkReimbursementRecoveryActiveYNAdd" runat="server"></asp:CheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Budget Code" AllowSorting="true" SortExpression="FLDSUBACCOUNT">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDSUBACCOUNT")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Budget ID="ddlBudgetCodeEdit" runat="server" AppendDataBoundItems="true"
                                    BudgetCodeList="<%#PhoenixRegistersBudget.ListBudget() %>" SelectedBudgetCode='<%# DataBinder.Eval(Container, "DataItem.FLDBUDGETID")%>'
                                    Width="100%" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Budget ID="ddlBudgetCodeAdd" runat="server" AppendDataBoundItems="true"
                                    BudgetCodeList="<%#PhoenixRegistersBudget.ListBudget() %>" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Charging Budget" AllowSorting="true" SortExpression="FLDCHARGINGBUDGETCODE">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDCHARGINGBUDGETCODE")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Budget ID="ddlChargingBudgetEdit" runat="server" AppendDataBoundItems="true"
                                    BudgetCodeList="<%#PhoenixRegistersBudget.ListBudget() %>" SelectedBudgetCode='<%# DataBinder.Eval(Container, "DataItem.FLDCHARGINGBUDGET")%>'
                                    Width="100%" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Budget ID="ddlChargingBudgetAdd" runat="server" AppendDataBoundItems="true"
                                    BudgetCodeList="<%#PhoenixRegistersBudget.ListBudget() %>" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Payment Mode">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Wrap="true" HorizontalAlign="Left" Width="25%" />
                            <FooterStyle Wrap="true" HorizontalAlign="Left" />

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPaymentMode" runat="server" EnableViewState="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTMODELISTCODE") %>'></telerik:RadLabel>

                            </ItemTemplate>
                            <EditItemTemplate>

                                <div style="height: 70px; overflow: auto;" class="input">

                                    <asp:CheckBoxList runat="server" ID="chkPaymentModeListEdit" AutoPostBack="false" CssClass="content">
                                        <Items>
                                            <asp:ListItem Text="Portage Bill" Value="697" />
                                            <asp:ListItem Text="Cash" Value="698" />
                                            <asp:ListItem Text="Allotment" Value="699" />

                                        </Items>
                                    </asp:CheckBoxList>
                                </div>
                            </EditItemTemplate>
                            <FooterTemplate>

                                <div style="height: 70px; overflow: auto;" class="input">

                                    <asp:CheckBoxList runat="server" ID="chkPaymentModeListAdd" AutoPostBack="false" CssClass="content">
                                        <Items>
                                            <asp:ListItem Text="Portage Bill" Value="697" />
                                            <asp:ListItem Text="Cash" Value="698" />
                                            <asp:ListItem Text="Allotment" Value="699" />

                                        </Items>
                                    </asp:CheckBoxList>
                                </div>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Add" ID="cmdAdd" ToolTip="Add New">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="2" EnableColumnClientFreeze="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>


