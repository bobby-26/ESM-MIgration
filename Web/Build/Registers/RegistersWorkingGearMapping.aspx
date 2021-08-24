<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersWorkingGearMapping.aspx.cs" Inherits="RegistersWorkingGearMapping" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="WorkingGearType" Src="~/UserControls/UserControlWorkingGearType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="WorkingGearItemType" Src="~/UserControls/UserControlWorkingGearItemType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register Src="~/UserControls/UserControlVessel.ascx" TagName="UserControlVessel"
    TagPrefix="eluc" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Vessel List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersVesselList" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <%--<eluc:TabStrip ID="MenuTitle" runat="server" Title="Working Gear Item Mapping"></eluc:TabStrip>--%>
        <telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server" Height="80%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" Visible="false" />
            <eluc:TabStrip ID="MenuCopy" runat="server" OnTabStripCommand="Copy_TabStripCommand" Title="Working Gear Item Mapping"></eluc:TabStrip>
            <table id="tblConfigureVesselList" width="100%">
                <tr>
                    <td>Vessel
                    </td>
                    <td>
                        <eluc:Vessel ID="ddlVessel" runat="server" CssClass="dropdown_mandatory" VesselsOnly="true" AppendDataBoundItems="true"
                            AutoPostBack="true" />
                        <%--OnTextChangedEvent="SetVessel" />--%>
                        <telerik:RadTextBox ID="TextBox1" runat="server" CssClass="input" ReadOnly="true" Visible="false"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Rank runat="server" ID="ucRank" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                            AutoPostBack="true" />
                    </td>
                </tr>
                <tr>
                </tr>
                <tr>
                    <td valign="middle">
                        <telerik:RadLabel ID="lblApplicableVessels" runat="server" Text="Copy To "></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkChkAllVessel" runat="server" AutoPostBack="true" OnCheckedChanged="SelectAllVessel" />
                        <telerik:RadLabel ID="lblCheckAll" runat="server" Text="Select all Vessel"></telerik:RadLabel>

                        <div id="divVesselList" class="input" style="overflow: auto; width: 60%; height: 100px">
                            <telerik:RadCheckBoxList runat="server" ID="cblVessel" Height="100%" Columns="1" AutoPostBack="true"
                                Direction="Horizontal" Layout="Flow" DataBindings-DataTextField="FLDVESSELNAME" 
                                DataBindings-DataValueField="FLDVESSELID">                                
                            </telerik:RadCheckBoxList>
                        </div>
                    </td>
                    <td valign="middle">
                        <telerik:RadLabel ID="lblrankcopy" runat="server" Text="Copy To"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkallrank" runat="server" AutoPostBack="true" OnCheckedChanged="SelectAllRank" />
                        <telerik:RadLabel ID="lblrankselectall" runat="server" Text="Select all Rank"></telerik:RadLabel>

                        <div id="divRankList" class="input" style="overflow: auto; width: 60%; height: 100px">
                            <telerik:RadCheckBoxList runat="server" ID="cblRanklst" Height="100%" Columns="1" AutoPostBack="true"
                                Direction="Horizontal" DataBindings-DataTextField="FLDRANKNAME"
                                 DataBindings-DataValueField="FLDRANKID" Layout="Flow">
                            </telerik:RadCheckBoxList>
                        </div>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuRegistersWorkingGearItem" Visible="false" runat="server" OnTabStripCommand="RegistersWorkingGearItem_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvRegistersworkinggearitemType" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvRegistersworkinggearitemType_ItemCommand" OnNeedDataSource="gvRegistersworkinggearitemType_NeedDataSource" Height="75%"
                OnItemDataBound="gvRegistersworkinggearitemType_ItemDataBound" EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true" ShowFooter="true">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
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
                        <%--<asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />--%>
                        <telerik:GridTemplateColumn HeaderStyle-Width="200px" HeaderText="Working Gear Item Type">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>                           
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblGearmappingid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKINGGEARITEMTYPEMAPPINGID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblGearitemTypeiditem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKINGGEARITEMTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <eluc:WorkingGearItemType ID="ucWorkingGearItemTypesAdd" AppendDataBoundItems="true" runat="server"
                                    TypeList='<%#PhoenixRegistersWorkingGearItemType.ListWorkingGearType(null)%>' CssClass="gridinput_mandatory" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="200px"  HeaderText="Quantity">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>                          
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblQuantityitem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtQuantity" runat="server" CssClass="input" Width="150px" IsInteger="true"
                                    IsPositive="true" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="100px"  HeaderText="Action">                           
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Delete" ToolTip="Delete" Width="20PX" Height="20PX"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Add" ToolTip="Add New" Width="20PX" Height="20PX"
                                    CommandName="Add" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd">
                                 <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
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
