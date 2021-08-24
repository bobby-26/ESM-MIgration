<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentsPhoenixTables.aspx.cs" Inherits="DocumentsPhoenixTables" %>

<!DOCTYPE html>
<%@ Import Namespace="System.Data" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tables" Src="~/UserControls/UserControlVesselTables.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Table Records</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function pageLoad() {
                PaneResized();
            }
            function PaneResized(sender, args) {
                var browserHeight = $telerik.$(window).height();
                var grid = $find("gvTableList");
                grid._gridDataDiv.style.height = (browserHeight - 300) + "px";
            }
        </script>
        <style type="text/css">
            .hidden {
                display: none;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server" submitdisabledcontrols="true">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status ID="ucStatus" runat="server" Text=""></eluc:Status>
        <eluc:TabStrip ID="MenuTables" runat="server" OnTabStripCommand="MenuTables_TabStripCommand"></eluc:TabStrip>
        <table width="100%">
            <tr>
                <td>
                    <table cellpadding="1" cellspacing="1" width="100%">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblTables" runat="server" Text="Tables"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Tables ID="ddlTables" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" AutoPostBack="true" OnTextChangedEvent="ddlTables_TextChangedEvent"
                                    AppendOwnerCharterer="true" Width="80%" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <asp:Panel ID="pnlSynchronizer" runat="server" GroupingText="Synchronizer">
                        <table cellpadding="1" cellspacing="1" width="100%">
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Vessel ID="ucVessel" Width="70%" runat="server" CssClass="input_mandatory" SyncActiveVesselsOnly="true" AppendDataBoundItems="true" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblAuditType" runat="server" Text="Audit Type"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadDropDownList ID="ddlAuditType" runat="server" CssClass="input_mandatory">
                                        <Items>
                                            <telerik:DropDownListItem Text="Insert" Value="0" Selected="True" />
                                            <telerik:DropDownListItem Text="Update" Value="1" />
                                        </Items>
                                    </telerik:RadDropDownList>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblTableType" runat="server" Text="Type"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadDropDownList ID="ddlTableType" runat="server" CssClass="input_mandatory">
                                        <Items>
                                            <telerik:DropDownListItem Text="Crew Audit" Value="0" Selected="True" />
                                            <telerik:DropDownListItem Text="Audit" Value="1" />
                                        </Items>
                                    </telerik:RadDropDownList>
                                </td>
                                <td>
                                    <asp:ImageButton runat="server" ID="imgbtnSend" CommandName="Send" ImageUrl="~/css/Theme1/images/24.png" OnClick="imgbtnSend_Click" ToolTip="Send to Vessel" AlternateText="Send to Vessel" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <br />
        <div>
            <table>
                <tr>
                    <td colspan="3">
                        <telerik:RadGrid ID="gridtodisplaytempData" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" ShowFooter="true"
                            CellSpacing="0" GridLines="None" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false" Width="70%"
                            OnItemCommand="gridtodisplaytempData_ItemCommand" OnItemDataBound="gridtodisplaytempData_ItemDataBound" OnEditCommand="gridtodisplaytempData_EditCommand">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" DataKeyNames="">
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
                                    <telerik:GridTemplateColumn HeaderText="Operator">
                                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                        <ItemTemplate>
                                            <telerik:RadLabel runat="server" ID="lblFLD1" Text='<%#Eval("FLD1") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadDropDownList ID="ddlEditOperater1" Width="100%" runat="server" CssClass="input_mandatory">
                                                <Items>
                                                    <telerik:DropDownListItem Text="--Select--" />
                                                    <telerik:DropDownListItem Text="AND" />
                                                    <telerik:DropDownListItem Text="OR" />
                                                    <telerik:DropDownListItem Text="AND(" />
                                                    <telerik:DropDownListItem Text="OR(" />
                                                </Items>
                                            </telerik:RadDropDownList>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadDropDownList ID="ddlAddOperater1" Width="100%" runat="server" CssClass="input_mandatory">
                                                <Items>
                                                    <telerik:DropDownListItem Text="--Select--" />
                                                    <telerik:DropDownListItem Text="AND" />
                                                    <telerik:DropDownListItem Text="OR" />
                                                    <telerik:DropDownListItem Text="AND(" />
                                                    <telerik:DropDownListItem Text="OR(" />
                                                </Items>
                                            </telerik:RadDropDownList>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Field Name">
                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                        <ItemTemplate>
                                            <telerik:RadLabel runat="server" ID="lblFLD2" Text='<%#Eval("FLD2") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadComboBox ID="ddlEditField" Width="100%" runat="server" CssClass="gridinput_mandatory" DataTextField="COLUMN_NAME" DataValueField="COLUMN_NAME" Filter="Contains">
                                            </telerik:RadComboBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadComboBox ID="ddlAddField" Width="100%" runat="server" CssClass="gridinput_mandatory" DataTextField="COLUMN_NAME" DataValueField="COLUMN_NAME" Filter="Contains">
                                            </telerik:RadComboBox>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Condition">
                                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                        <ItemTemplate>
                                            <telerik:RadLabel runat="server" ID="lblFLD3" Text='<%#Eval("FLD3") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadDropDownList ID="ddlEditOperater2" Width="100%" CssClass="input_mandatory" runat="server">
                                                <Items>
                                                    <telerik:DropDownListItem Text="--Select--" />
                                                    <telerik:DropDownListItem Text="LIKE" />
                                                    <telerik:DropDownListItem Text="IN" />
                                                    <telerik:DropDownListItem Text="=" />
                                                    <telerik:DropDownListItem Text="!=" />
                                                    <telerik:DropDownListItem Text=">" />
                                                    <telerik:DropDownListItem Text="<" />
                                                </Items>
                                            </telerik:RadDropDownList>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadDropDownList ID="ddlAddOperater2" Width="100%" CssClass="input_mandatory" runat="server">
                                                <Items>
                                                    <telerik:DropDownListItem Text="--Select--" />
                                                    <telerik:DropDownListItem Text="LIKE" />
                                                    <telerik:DropDownListItem Text="IN" />
                                                    <telerik:DropDownListItem Text="=" />
                                                    <telerik:DropDownListItem Text="!=" />
                                                    <telerik:DropDownListItem Text=">" />
                                                    <telerik:DropDownListItem Text="<" />
                                                </Items>
                                            </telerik:RadDropDownList>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Value">
                                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                        <ItemTemplate>
                                            <telerik:RadLabel runat="server" ID="lblFLD4" Text='<%#Eval("FLD4") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadTextBox ID="txtEditValue" Width="100%" runat="server"></telerik:RadTextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadTextBox ID="txtAddValue" Width="100%" runat="server"></telerik:RadTextBox>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn>
                                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                        <ItemTemplate>
                                            <telerik:RadLabel runat="server" ID="lblFLD5" Text='<%#Eval("FLD5") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadDropDownList ID="ddlEditOperater3" Width="100%" runat="server">
                                                <Items>
                                                    <telerik:DropDownListItem Text="--Select--" />
                                                    <telerik:DropDownListItem Text=")" />
                                                </Items>
                                            </telerik:RadDropDownList>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadDropDownList ID="ddlAddOperater3" Width="100%" runat="server">
                                                <Items>
                                                    <telerik:DropDownListItem Text="--Select--" />
                                                    <telerik:DropDownListItem Text=")" />
                                                </Items>
                                            </telerik:RadDropDownList>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Action">
                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <FooterStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                                CommandName="EDIT" ID="cmdEdit" ToolTip="Edit"></asp:ImageButton>
                                            <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                                width="3" />
                                            <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                                CommandName="DELETE" ID="cmdDelete" ToolTip="Delete"></asp:ImageButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                                CommandName="Save" ID="cmdSave" ToolTip="Save"></asp:ImageButton>
                                            <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                                width="3" />
                                            <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                                CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel"></asp:ImageButton>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                                CommandName="Add" ID="cmdAdd" ToolTip="Add New"></asp:ImageButton>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                            <ClientSettings>
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <telerik:RadLabel ID="lblSql" runat="server"></telerik:RadLabel>
        </div>
        <br />
        <telerik:RadGrid ID="gvTableList" runat="server" AutoGenerateColumns="true" Width="100%" Font-Size="11px"
            CellPadding="3" ShowHeader="true" EnableViewState="false">
            <MasterTableView Width="100%">
                <NoRecordsTemplate>
                    <table runat="server" width="100%" border="0">
                        <tr>
                            <td align="center">
                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </NoRecordsTemplate>
            </MasterTableView>
            <%--<ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true">
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
            </ClientSettings>--%>
        </telerik:RadGrid>
    </form>
</body>
</html>
