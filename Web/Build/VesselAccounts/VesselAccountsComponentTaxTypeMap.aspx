<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsComponentTaxTypeMap.aspx.cs" Inherits="VesselAccounts_VesselAccountsComponentTaxTypeMap" %>

<!DOCTYPE html>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.VesselAccounts" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselCrew" Src="~/UserControls/UserControlVesselEmployee.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmMessage" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="EntryType" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonToolTip" Src="~/UserControls/UserControlCommonToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Component Tax Type Map</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">

        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        
        <eluc:TabStrip ID="MenuComponentTaxTypeMap" runat="server" TabStrip="true" OnTabStripCommand="MenuComponentTaxTypeMap_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="90%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />
             <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox RenderMode="Lightweight" ID="ddlType" runat="server" EmptyMessage="Type or select Type" MarkFirstMatch="true" AppendDataBoundItems="true"
                            Width="240px" CssClass="input_mandatory" AutoPostBack="True" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                            <Items>
                                <telerik:RadComboBoxItem Value="1" Text="CBA" Selected="true" />
                                <telerik:RadComboBoxItem Value="2" Text="Standard Wage Component" />
                                <telerik:RadComboBoxItem Value="3" Text="Component Agreed With Crew" />

                            </Items>
                        </telerik:RadComboBox>

                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCountry" runat="server" Text="Country"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Country ID="ddlCountry" CssClass="input_mandatory" runat="server" Width="240px" AppendDataBoundItems="true"></eluc:Country>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblWageComponents" runat="server" Text="Wage Components"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucWageComponents" runat="server" HardTypeCode="156" AppendDataBoundItems="true"
                            AutoPostBack="true" OnTextChangedEvent="ucWageComponents_Changed" Width="240px" />
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblUnion" runat="server" Text="Union"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Address ID="ddlUnion" runat="server" AppendDataBoundItems="true"
                            AutoPostBack="true" OnTextChangedEvent="ddlUnion_TextChangedEvent" AddressType="134" Width="240px" />

                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCBARevision" runat="server" Text="CBA Revision"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox DropDownPosition="Static" ID="ddlRevision" runat="server" EnableLoadOnDemand="True" AppendDataBoundItems="true"
                            EmptyMessage="Type to select Revision" Width="240px" DataTextField="FLDNAME" DataValueField="FLDREVISIONID" Filter="Contains" MarkFirstMatch="true">
                        </telerik:RadComboBox>
                    </td>
                </tr>

            </table>

            <eluc:TabStrip ID="MenuComponentTaxType" runat="server" OnTabStripCommand="MenuComponentTaxType_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvComponentTaxType" runat="server" AutoGenerateColumns="False" Width="100%" ShowHeader="true" EnableViewState="false" Height="88%"
                CellSpacing="0" GridLines="None" GroupingEnabled="false" AllowPaging="true" AllowSorting="true" EnableHeaderContextMenu="true" ShowFooter="false"
                OnNeedDataSource="gvComponentTaxType_NeedDataSource" OnItemDataBound="gvComponentTaxType_ItemDataBound" OnItemCommand="gvComponentTaxType_ItemCommand">
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" DataKeyNames="FLDID"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                    <%--<HeaderStyle Width="120px" />--%>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Component">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="center"></ItemStyle>
                            <ItemTemplate>
                                  <telerik:RadLabel ID="lblid" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDID"] %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblComponentid" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDCOMPONENTID"] %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblComponent" runat="server" ToolTip='<%#((DataRowView)Container.DataItem)["FLDCOMPONENTNAME"] %>' Text='<%#((DataRowView)Container.DataItem)["FLDCOMPONENTNAME"] %>'></telerik:RadLabel>
                            </ItemTemplate>
                 
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTypeid" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDTYPEID"] %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblType" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDNAME"] %>'></telerik:RadLabel>
                            </ItemTemplate>
                  
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Active Y/N" HeaderStyle-Width="100px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblactiveYNHeader" runat="server">
                                    Active Y/N
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblactiveYN" runat="server" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDISACTIVE").ToString() == "1" ? "Yes" : "No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                         
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                         
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" Position="Bottom" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="3" EnableColumnClientFreeze="true" ScrollHeight="350px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" ResizeGridOnColumnResize="false" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

