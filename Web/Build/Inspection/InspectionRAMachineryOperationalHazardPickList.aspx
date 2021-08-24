<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRAMachineryOperationalHazardPickList.aspx.cs" Inherits="InspectionRAMachineryOperationalHazardPickList" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Category" Src="~/UserControls/UserControlRACategoryExtn.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
     <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
     <form id="frmMOC" runat="server" autocomplete="off">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuOperational" runat="server" OnTabStripCommand="Operational_TabStripCommand" Title="Aspect"></eluc:TabStrip>
            <table width="100%">
                <tr>
                    <td width="8%">
                        <telerik:RadLabel ID="lblNumber" runat="server" Text="Process"></telerik:RadLabel>
                    </td>
                    <td width="42%">
                        <eluc:Category ID="ucElement" runat="server" AppendDataBoundItems="true" AutoPostBack="true" OnTextChangedEvent="ucElement_TextChanged" Width="180px" />
                    </td>
                    <td width="8%">
                        <telerik:RadLabel ID="lblAspect" runat="server" Text="Keyword"></telerik:RadLabel>
                    </td>
                    <td width="42%">
                        <telerik:RadTextBox ID="txtOperationalHazard" runat="server" MaxLength="100" Width="270px" CssClass="input"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuRA" runat="server" OnTabStripCommand="MenuRA_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvOperationalHazard" runat="server" Height="80%" AllowCustomPaging="true" OnItemDataBound="gvOperationalHazard_ItemDataBound"
                CellSpacing="0" EnableViewState="false" GridLines="None" Font-Size="11px" OnItemCommand="gvOperationalHazard_ItemCommand"
                OnNeedDataSource="gvOperationalHazard_NeedDataSource" ShowFooter="False" ShowHeader="true" Width="100%">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDRISKASSESSEMENTOPERATIONALHAZARDID">
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
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn>
                            <HeaderStyle Width="4%" HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemTemplate>
                                <telerik:RadCheckBox ID="chkSelect" runat="server" OnCheckedChanged="chkSelect_CheckedChanged" AutoPostBack="true" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Process" HeaderStyle-Width="10%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblElement" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDELEMENT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Aspect" HeaderStyle-Width="15%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="15%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOperationalid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKASSESSEMENTOPERATIONALHAZARDID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkaspect" runat="server" CommandName="ASPECT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDASPECT") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Hazards / Risks" HeaderStyle-Width="17%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="17%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblHazards" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPERATIONALHAZARD") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Controls / Precautions" HeaderStyle-Width="44%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="44%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblControls" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTROLPRECAUTIONS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="10%">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="TEDIT" ID="cmdEdit" ToolTip="Edit">
                                        <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
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
