<%@ Page Language="C#" AutoEventWireup="True" CodeFile="InventorySpareItemControl.aspx.cs"
    Inherits="InventorySpareItemControl" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TreeView" Src="~/UserControls/UserControlTreeViewTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VerticalSplit" Src="~/UserControls/UserControlVerticalSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Location</title>
    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
        function PaneResized(sender, args) {
 
            var grid = $find("gvSpareItemControl");
            var maintbl = document.getElementById("tblmain");
            var subtbl = document.getElementById("details");
            grid._gridDataDiv.style.height = (maintbl.offsetHeight - subtbl.offsetHeight - 112) + "px";

        }
        function pageLoad() {
            PaneResized();
        }
    </script>
    </telerik:RadCodeBlock>
</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="frmRegistersLocation" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager" runat="server"></telerik:RadSkinManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gvSpareItemControl">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvSpareItemControl" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                        <telerik:AjaxUpdatedControl ControlID="ucStatus" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="repLocationList">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="details" />
                        <telerik:AjaxUpdatedControl ControlID="gvSpareItemControl" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="tvwLocation">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="details" />
                        <telerik:AjaxUpdatedControl ControlID="gvSpareItemControl" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="details">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="details" />
                        <telerik:AjaxUpdatedControl ControlID="gvSpareItemControl" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" CssClass="hidden" OnClick="cmdHiddenSubmit_Click" />
        
        <table id="tblmain" style="position: absolute; top: 0; bottom: 0; left: 0; right: 0; height:100%">
            <tr style="position: relative; vertical-align: top">
                <td style="height:100%;width:21%;">
                    <table cellpadding="0" cellspacing="0" style="float: left; width: 100%; height:100%">
                        <tr>
                            <td>
                                <telerik:RadDropDownList ID="ddlLocationAdd" runat="server" Width="100%" CssClass="input"
                                    OnSelectedIndexChanged="ddlLocationAdd_SelectedIndexChanged" AutoPostBack="true">
                                    <Items>
                                        <telerik:DropDownListItem Text="From tree" Value="1" Selected="true" />
                                        <telerik:DropDownListItem Text="From List" Value="2" />
                                    </Items>
                                </telerik:RadDropDownList>
                            </td>
                        </tr>
                        <tr style="position: relative;height:100%;">
                            <td>
                                <div id="divLocationTree" runat="server" style="height: 100%; width: auto; overflow:auto">
                                    <eluc:TreeView runat="server" ID="tvwLocation" OnNodeClickEvent="tvwLocation_NodeClickEvent"></eluc:TreeView>
                                </div>
                                <div id="divLocationList" runat="server" style="height: 100%; overflow: scroll">
                                    <asp:Repeater ID="repLocationList" runat="server">
                                        <HeaderTemplate>
                                            <table border="1" cellpadding="0" cellspacing="0" width="300px">
                                                <tr>
                                                    <tr>
                                                        <td>
                                                            <telerik:RadLabel ID="lblLocationName" runat="server" Text="Location Name"></telerik:RadLabel>
                                                        </td>
                                                        <td>
                                                            <telerik:RadLabel ID="lblLocationCode" runat="server" Text="Location Code"></telerik:RadLabel>
                                                        </td>
                                                    </tr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <telerik:RadLabel ID="lblLocationID" runat="server" Visible="false" Text='<%# Eval("FLDLOCATIONID")%>'></telerik:RadLabel>
                                                    <asp:LinkButton ID="lnkLocationName" runat="server" OnCommand="lnkLocationName" Font-Underline="false"
                                                        CommandArgument='<%# Eval("FLDLOCATIONID") + "," + Eval("FLDLOCATIONNAME") %>'
                                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATIONNAME")%>'></asp:LinkButton>
                                                </td>
                                                <td>
                                                    <telerik:RadLabel ID="Label1" runat="server" Visible="true" Text='<%# Eval("FLDLOCATIONCODE")%>'></telerik:RadLabel>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
                <td colspan="4" style=" height:100%; width:80%;">
                    <table width="100%" id="details" runat="server">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblSelectedLocation" runat="server" Text="Selected Location"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtLocationName" runat="server" ReadOnly="true" CssClass="Input readonlytextbox"
                                    Width="200px">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblMakerReference" runat="server" Text="Maker Reference"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtMekerRef" runat="server" CssClass="input" Width="90px"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblItemName" runat="server" Text="Item Name"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtName" runat="server" CssClass="input" Width="200px"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblItemNumber" runat="server" Text="Item Number"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtNumber" runat="server" CssClass="input" Width="90px"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblComponent" runat="server" Text="Component"></telerik:RadLabel>
                            </td>
                            <td>
                                <span id="spnPickComponent">
                                    <telerik:RadTextBox ID="txtComponent" runat="server" Width="90px" CssClass="Input readonlytextbox"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtComponentName" runat="server" Width="180px" CssClass="Input readonlytextbox"></telerik:RadTextBox>
                                    <asp:LinkButton ID="cmdShowComponent" runat="server" ImageAlign="AbsMiddle" Text="..">
                                        <span class="icon"><i class="fas fas fa-tasks"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" ID="cmdClear" ImageAlign="AbsMiddle" Text=".." OnClick="cmdClear_Click">
                                        <span class="icon"><i class="fas fa-paint-brush"></i></span>
                                    </asp:LinkButton>
                                    <telerik:RadTextBox ID="txtComponentID" CssClass="hidden" runat="server" Width="0px" />
                                </span>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblShowAvailableSpareItemsonly" runat="server" Text="Show Available Spare Items only"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadCheckBox ID="chkROB" runat="server" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblCritical" runat="server" Text="Critical"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadCheckBox ID="chkCritical" runat="server"></telerik:RadCheckBox>
                            </td>
                        </tr>
                    </table>
                    <eluc:TabStrip ID="MenuStockItemControl" runat="server" OnTabStripCommand="MenuStockItemControl_TabStripCommand"></eluc:TabStrip>
                    <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvSpareItemControl" DecoratedControls="All" EnableRoundedCorners="true" />
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvSpareItemControl" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" 
                        CellSpacing="0" GridLines="None" OnNeedDataSource="gvSpareItemControl_NeedDataSource" OnUpdateCommand="gvSpareItemControl_UpdateCommand"
                        OnItemDataBound="gvSpareItemControl_ItemDataBound" OnSortCommand="gvSpareItemControl_SortCommand" GroupingEnabled="false"
                        EnableHeaderContextMenu="true">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" DataKeyNames="FLDSPAREITEMLOCATIONID">
                            <HeaderStyle Width="102px" />
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Location" HeaderStyle-Width="15%">
                                    <ItemStyle Wrap="false" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblLocationName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATIONNAME") %>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATIONNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Number" HeaderStyle-Width="12%" AllowSorting="true" SortExpression="FLDNUMBER">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblSpareItemLocationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSPAREITEMLOCATIONID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblLocationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATIONID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblSpareItemId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSPAREITEMID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblSpareItemNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="20%" AllowSorting="true" SortExpression="FLDNAME">
                                    <ItemStyle Wrap="false" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblStockItemName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Component Name" HeaderStyle-Width="70px" AllowSorting="true" SortExpression="FLDCOMPONENTNAME">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblStockComponentId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>' Visible="false"></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblStockComponentName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Maker Reference" HeaderStyle-Width="20%" AllowSorting="true">
                                    <ItemStyle Wrap="false" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblStockItemMakerName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAKERREFERENCE") %>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDMAKERREFERENCE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Unit" HeaderStyle-Width="7%" AllowSorting="true">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblStockItemUnit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="In Stock" AllowSorting="true" HeaderStyle-Width="8%" SortExpression="FLDQUANTITY" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="false">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblInStockQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY","{0:n0}") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Actual" AllowSorting="true" HeaderStyle-Width="8%" SortExpression="FLDQUANTITY" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblActualQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY","{0:n0}") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Number ID="txtActualQuantityEdit" runat="server" MaxLength="7" Width="100%" IsInteger="true" CssClass="gridinput" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY","{0:n0}") %>' />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                    <HeaderStyle Width="10%" />
                                    <ItemStyle Width="10%" Wrap="false" />
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                        </asp:LinkButton>
                                        <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                        <asp:LinkButton runat="server" CommandName="MOVE" ID="cmdMove" ToolTip="Move Spare Item to another location">
                                            <span class="icon"><i class="fas fa-dolly"></i></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton runat="server" CommandName="Update" ID="cmdUpdate" ToolTip="Update">
                                            <span class="icon"><i class="fas fa-save"></i></span>
                                        </asp:LinkButton>
                                        <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                        <asp:LinkButton runat="server" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                            <span class="icon"><i class="fas fa-times-circle"></i></span>
                                        </asp:LinkButton>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <NoRecordsTemplate>
                                <table width="100%" border="0">
                                    <tr>
                                        <td align="center">
                                            <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </NoRecordsTemplate>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" CssClass="RadGrid_Default rgPagerTextBox" PageButtonCount="5" PagerTextFormat="{4}<strong>{5}</strong> Records Found" AlwaysVisible="true"
                                PageSizeLabelText="Records per page:" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="100%" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </td>
            </tr>
        </table>
            
    </form>
</body>
</html>
