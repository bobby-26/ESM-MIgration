<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionOrganization.aspx.cs" Inherits="InspectionOrganization" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Organization</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            $(document).ready(function () {
                var browserHeight = $telerik.$(window).height();
                $("#gvOrganization").height(browserHeight - 40);
            });

        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersOrganization" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="99.9%"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Title runat="server" ID="ucTitle" Text="Organization" Visible="false"></eluc:Title>
            <table width="100%" cellpadding="2" cellspacing="2">
                <tr>
                    <td style="width: 10%">
                        <telerik:RadLabel ID="lblCategory" runat="server" Text="Category"></telerik:RadLabel>
                    </td>
                    <td style="width: 20%">
                        <eluc:Hard ID="ucInspectionCategory" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                            CssClass="input_mandatory" HardList="<%# PhoenixRegistersHard.ListHard(1,144) %>" HardTypeCode="144" OnTextChangedEvent="ucInspectionCategory_Changed" />
                    </td>
                    <td width="10%">
                        <telerik:RadLabel ID="lblShortCode" runat="server" Text="Code"></telerik:RadLabel>
                    </td>
                    <td width="25%">
                        <telerik:RadTextBox ID="txtShortCode" MaxLength="20" runat="server" CssClass="input" Width="200px"></telerik:RadTextBox>
                    </td>
                    <td width="10%">
                        <telerik:RadLabel ID="lblOrganizationName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td width="25%">
                        <telerik:RadTextBox ID="txtOrganizationName" runat="server" CssClass="input" MaxLength="200" Width="200px"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuRegistersOrganization" runat="server" OnTabStripCommand="RegistersOrganization_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvOrganization" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowPaging="true" AllowCustomPaging="true"
                OnRowCreated="gvOrganization_RowCreated" Width="100%" Height="87.5%" CellPadding="3" OnItemCommand="gvOrganization_ItemCommand" GroupingEnabled="false" EnableHeaderContextMenu="true"
               OnItemDataBound="gvOrganization_ItemDataBound" OnNeedDataSource="gvOrganization_NeedDataSource"
                ShowFooter="true" ShowHeader="true" EnableViewState="false" AllowSorting="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <NoRecordsTemplate>
                        <table width="99.9%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <%--<asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />--%>
                        <telerik:GridTemplateColumn HeaderText="Code">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblShortCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORGANIZATIONCODE") %>'
                                    Width="50px">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtShortCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORGANIZATIONCODE") %>'
                                    CssClass="gridinput_mandatory" Width="98%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtShortCodeAdd" runat="server" CssClass="input_mandatory" Width="98%"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="60%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOrganization" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORGANIZATIONNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblOrganizationid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORGANIZATIONID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtOrganizationEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORGANIZATIONNAME") %>'
                                    CssClass="gridinput_mandatory" Width="98%">
                                </telerik:RadTextBox>
                                <telerik:RadLabel ID="lblOrganizationidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORGANIZATIONID") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtOrganizationAdd" runat="server" CssClass="gridinput_mandatory" Width="98%"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                        <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                        <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="UPDATE" ID="cmdSave" ToolTip="Save">
                                        <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="CANCEL" ID="cmdCancel" ToolTip="Cancel">
                                        <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="ADD" ID="cmdAdd" ToolTip="Add New">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <eluc:Status runat="server" ID="ucStatus" />
            <%--            <div id="divPage" style="position: relative;">
                <table width="100%" border="0" class="datagrid_pagestyle">
                    <tr>
                        <td nowrap="nowrap" align="center">
                            <telerik:RadLabel ID="lblPagenumber" runat="server">
                            </telerik:RadLabel>
                            <telerik:RadLabel ID="lblPages" runat="server">
                            </telerik:RadLabel>
                            <telerik:RadLabel ID="lblRecords" runat="server">
                            </telerik:RadLabel>&nbsp;&nbsp;
                        </td>
                        <td nowrap="nowrap" align="left" width="50px">
                            <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                        </td>
                        <td width="20px">&nbsp;
                        </td>
                        <td nowrap="nowrap" align="right" width="50px">
                            <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                        </td>
                        <td nowrap="nowrap" align="center">
                            <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                            </asp:TextBox>
                            <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                Width="40px"></asp:Button>
                        </td>
                    </tr>
                    <eluc:Status runat="server" ID="ucStatus" />
                </table>
            </div>--%>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>



