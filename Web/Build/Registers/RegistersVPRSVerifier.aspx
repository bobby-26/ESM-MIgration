<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersVPRSVerifier.aspx.cs" Inherits="RegistersVPRSVerifier" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Verifier</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmVPRSVerifier" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="Status" />
            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblcode" Text="Short Code" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtcode" runat="server" CssClass="input" MaxLength="10"></telerik:RadTextBox>
                    </td>
                    <td width="30%"></td>
                    <td>
                        <telerik:RadLabel ID="lblverifr" Text="Verifier" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtverifier" runat="server" CssClass="input" MaxLength="500"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuVerifier" runat="server" OnTabStripCommand="Verifier_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvVerifier" Height="85%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvVerifier_ItemCommand" OnItemDataBound="gvVerifier_ItemDataBound" OnUpdateCommand="gvVerifier_UpdateCommand"
                ShowFooter="true" ShowHeader="true" EnableViewState="false" OnNeedDataSource="gvVerifier_NeedDataSource" OnSortCommand="gvVerifier_SortCommand"
                EnableHeaderContextMenu="true" GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDEUMRVVERIFIERID">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Short Code">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblShortName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtShortNameEdit" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCODE") %>' runat="server" CssClass="gridinput_mandatory" MaxLength="10"
                                    Width="100%" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtShortNameAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="10" Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Verifier">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVerifierId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEUMRVVERIFIERID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblverifier" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVERIFIERNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblVerifierIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEUMRVVERIFIERID") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtVerifierEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVERIFIERNAME") %>'
                                    CssClass="gridinput_mandatory" MaxLength="500" Width="100%"></telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtVerifierAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="500"
                                    Width="100%"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Address">
                            <HeaderStyle Width="20%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAddressItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDRESS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtAddressEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDRESS")%>' TextMode="MultiLine" Rows="2"
                                    CssClass="gridinput" Width="100%"></telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtAddressAdd" runat="server" CssClass="gridinput" TextMode="MultiLine" Rows="2" Width="100%"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Accreditation No">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAccNoItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCREDITATIONNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtAccNoEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCREDITATIONNO") %>'
                                    CssClass="gridinput" MaxLength="100" Width="100%"></telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtAccNoAdd" runat="server" CssClass="gridinput" MaxLength="100" Width="100%"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="To Mail">
                            <HeaderStyle Width="18%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbltomail" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOMAIL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtToMailEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOMAIL") %>'
                                    CssClass="gridinput_mandatory" MaxLength="200" Width="100%"></telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtToMailAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="200" Width="100%"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="CC Mail">
                            <HeaderStyle Width="17%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblccmail" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCCMAIL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtCCMailEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCCMAIL") %>'
                                    CssClass="gridinput" MaxLength="200" Width="100%"></telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtCCMailAdd" runat="server" CssClass="gridinput" MaxLength="200" Width="100%"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT"  ID="cmdEdit" ToolTip="Edit">
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
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Add" ID="cmdAdd" ToolTip="Add New">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
