<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersInstallationType.aspx.cs" Inherits="Registers_RegistersInstallationType" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Installation Type</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersInstallation" autocomplete="off" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadFormDecorator" runat="server" DecorationZoneID="tblConfiguremovement" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="98.5%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />

            <%-- <eluc:Title runat="server" ID="ucTitle" Text="Installation Type" />--%>

            <table id="tblInstallation" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCode" runat="server" Text="Installation Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSearch" runat="server" MaxLength="6" CssClass="input"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblInstallationName" runat="server" Text="Installation Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtInstallation" runat="server" MaxLength="200"
                            CssClass="input" Width="240px">
                        </telerik:RadTextBox>
                    </td>

                </tr>
            </table>

            <eluc:TabStrip ID="MenuRegistersInstallation" runat="server" OnTabStripCommand="RegistersInstallation_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvInstallation" Height="90%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvInstallation_ItemCommand" OnItemDataBound="gvInstallation_ItemDataBound" OnNeedDataSource="gvInstallation_NeedDataSource"
                ShowFooter="true" ShowHeader="true" EnableViewState="true" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Installation Code">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:ImageButton runat="server" ID="cmdHiddenSubmit" OnClick="cmdSearch_Click" CommandName="FLDTYPEOFINSTALLATIONCODE"
                                    ImageUrl="<%$ PhoenixTheme:images/spacer.png %>"
                                    CommandArgument="1" />
                                <asp:LinkButton ID="lblInstallationCodeHeader" runat="server" CommandName="Sort" CommandArgument="FLDTYPEOFINSTALLATIONCODE" ForeColor="White">Installation Code&nbsp;</asp:LinkButton>
                                <img id="FLDTYPEOFINSTALLATIONCODE" runat="server" visible="false" />
                                <%--<Telerik:RadLabel id="lblPropulsionCode" runat="server" Text="Propulsion Code"></Telerik:RadLabel>--%>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInstallationID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEOFINSTALLATIONID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkInstallationCode" runat="server" CommandName="EDIT"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEOFINSTALLATIONCODE") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblInstallationIDEdit" runat="server" Width="240px" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEOFINSTALLATIONID") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtInstallationCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEOFINSTALLATIONCODE") %>'
                                    CssClass="gridinput_mandatory" Width="240px" MaxLength="200">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtInstallationCodeAdd" runat="server" CssClass="gridinput_mandatory" Width="240px"
                                    MaxLength="4" ToolTip="Enter Installation Code">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Installation Name">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblInstallationNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDTYPEOFINSTALLATIONNAME" ForeColor="White">Installation Name&nbsp;</asp:LinkButton>
                                <img id="FLDTYPEOFINSTALLATIONNAME" runat="server" visible="false" />
                                <%--<Telerik:RadLabel id="lblPropulsionName" runat="server" Text="Propulsion Name"></Telerik:RadLabel>--%>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInstallation" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEOFINSTALLATIONNAME") %>'></telerik:RadLabel>

                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtInstallationEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEOFINSTALLATIONNAME") %>'
                                    CssClass="gridinput_mandatory" Width="240px" MaxLength="200">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtInstallationAdd" runat="server" Width="240px" CssClass="gridinput_mandatory"
                                    MaxLength="200">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="70px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">
                                    Action
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
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
            <%--        <Triggers>
            <asp:PostBackTrigger ControlID="MenuRegistersInstallation" />
        </Triggers>--%>
        </telerik:RadAjaxPanel>
    </form>
</body>

</html>
