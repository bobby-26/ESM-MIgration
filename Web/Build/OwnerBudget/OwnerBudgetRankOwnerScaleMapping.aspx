<%@ Page Language="C#" AutoEventWireup="True" CodeFile="OwnerBudgetRankOwnerScaleMapping.aspx.cs" Inherits="OwnerBudgetRankOwnerScaleMapping"
    MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>--%>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="QuickType" Src="~/UserControls/UserControlQuickType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationality.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Complement</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersQuick" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel runat="server" ID="pnlCountryEntry">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table id="tblConfigureQuick" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLevel" runat="server" Text="Level"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDropDownList ID="ddlLevel" runat="server" AutoPostBack="true">
                            <Items>
                                <telerik:DropDownListItem Text="Senior Officer" Value="351" Selected="True"></telerik:DropDownListItem>
                                <telerik:DropDownListItem Text="Junior Officer" Value="570"></telerik:DropDownListItem>
                                <telerik:DropDownListItem Text="Trainee" Value="573"></telerik:DropDownListItem>
                                <telerik:DropDownListItem Text="Ratings" Value="356"></telerik:DropDownListItem>
                            </Items>
                        </telerik:RadDropDownList>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuRegistersQuick" runat="server" OnTabStripCommand="RegistersQuick_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvQuick" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnItemCommand="gvQuick_ItemCommand" OnItemDataBound="gvQuick_ItemDataBound"
                OnRowCreated="gvQuick_RowCreated" OnDeleteCommand="gvQuick_DeleteCommand" AllowSorting="true" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnUpdateCommand="gvQuick_UpdateCommand" OnSortCommand="gvQuick_SortCommand" ShowFooter="false"
                ShowHeader="true" EnableViewState="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="" TableLayout="Fixed" >
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderStyle-Width="340px" HeaderText="Group Rank">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblManningScaleId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMANNINGSCALEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblGroupRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUPRANK") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--<asp:TemplateField FooterText="New Short">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" ></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblRank" runat=server" Text="Rank"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    
                                    <asp:Label ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField FooterText="New Short">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" ></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblRankCode" runat="server" Text="Rank Code"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblRankCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                        <telerik:GridTemplateColumn HeaderStyle-Width="340px" HeaderText="Owner Scale">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOwnerScale" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERSCALE") %>' Width="100px"></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucOwnerScale" runat="server" IsPositive="true" IsInteger="true" CssClass="input_mandatory"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDOWNERSCALE") %>' Width="100px" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn  HeaderText="Action" HeaderStyle-Width="140px" HeaderStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                         
                            <ItemStyle Wrap="false" HorizontalAlign="Center" />
                            <ItemTemplate >
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
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
