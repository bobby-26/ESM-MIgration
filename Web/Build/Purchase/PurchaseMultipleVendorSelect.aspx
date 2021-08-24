<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseMultipleVendorSelect.aspx.cs"
    Inherits="PurchaseMultipleVendorSelect" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Owner" Src="~/UserControls/UserControlAddressType.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Address List</title>
    <telerik:RadCodeBlock runat="server" ID="RadCodeBlock">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%" Width="100%">
            <div style="font-weight: 600; font-size: 12px;" runat="server">
            <eluc:TabStrip ID="MenuAddress" runat="server" OnTabStripCommand="MenuAddress_TabStripCommand"></eluc:TabStrip>
                </div>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            
            <table cellpadding="1" cellspacing="1" width="100%">
                        <tr>
                            <td>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtNameSearch" runat="server" Text="" Width="200px"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblCode" runat="server" Text="Code"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtCode" Text="" Width="200px"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblPrincipal" runat="server" Text="Principal"></telerik:RadLabel>
                            </td>
                            <td>
                                <asp:HiddenField ID="hdnprincipalId" runat="server" />
                                <eluc:Owner runat="server" ID="ucPrincipal" Width="200px" AddressType="128,127"
                                            AppendDataBoundItems="true" AutoPostBack="true" />
                            </td>
                            <td>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblAddressType" runat="server" Text="Address Type"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadDropDownList runat="server" ID="ddlAddressType" AppendDataBoundItems="true" Width="200px" AutoPostBack="true">
                                </telerik:RadDropDownList>
                            </td>
                        </tr>
                    </table>

        <telerik:RadGrid RenderMode="Lightweight" ID="gvAddress" runat="server" Height="80%" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" Font-Size="11px" OnItemCommand="gvAddress_ItemCommand" OnItemDataBound="gvAddress_ItemDataBound"
                OnNeedDataSource="gvAddress_NeedDataSource" ShowFooter="False" ShowHeader="true" Width="100%">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDADDRESSCODE">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn>
                            <HeaderStyle Width="40px" />
                            <ItemStyle Width="40px" />
                            <ItemTemplate>
                                <telerik:RadCheckBox ID="chkVendorSelect" RenderMode="Lightweight" runat="server" OnCheckedChanged="SaveCheckedAddressValues" AutoPostBack="true"></telerik:RadCheckBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Code" AllowSorting="true" SortExpression="FLDCODE">
                            <HeaderStyle Width="100px" />
                            <ItemStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="true" SortExpression="FLDNAME">
                            <HeaderStyle Width="300px" />
                            <ItemStyle Width="300px" Wrap="false" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="lblAddressCode" Text='<%# Bind("FLDADDRESSCODE") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="lblEmail" Text='<%# Bind("FLDEMAIL1") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lnkAddressName" RenderMode="Lightweight" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                <br />
                                <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="lblNotApproved" ForeColor="Red" Text="" Visible="false"></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Owner" UniqueName="OWNER">
                            <HeaderStyle Width="120px" />
                            <ItemStyle Width="120px" />
                            <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="lblOwnerName" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOWNERLIST").ToString().Length>0 ? DataBinder.Eval(Container, "DataItem.FLDOWNERLIST").ToString().Replace("@n","<br/>"):DataBinder.Eval(Container,"DataItem.FLDOWNERLIST") %>' Width="240px"></telerik:RadLabel>
                             </ItemTemplate>
                            </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Phone 1" AllowSorting="true" SortExpression="FLDPHONE1">
                            <HeaderStyle Width="90px" />
                            <ItemStyle Width="90px" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblPhone1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPHONE1") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Email 1" AllowSorting="true" SortExpression="FLDEMAIL1">
                            <HeaderStyle Width="120px" />
                            <ItemStyle Width="120px" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblEmail1" runat="server"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDEMAIL1").ToString().Length>10 ? DataBinder.Eval(Container, "DataItem.FLDEMAIL1").ToString().Substring(0, 10) + "..." : DataBinder.Eval(Container, "DataItem.FLDEMAIL1").ToString() %>'>
                                </telerik:RadLabel>
                                <eluc:ToolTip RenderMode="Lightweight" ID="ucEmailTT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMAIL1") %>' />
                                <img id="imgEmail" runat="server" style="cursor: hand" src="<%$ PhoenixTheme:images/te_view.png %>" alt=""
                                    onmousedown="javascript:closeMoreInformation()" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <%--<asp:TemplateField>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblServicesHeader" runat="server">Services</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblServices" runat="server"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDPRODUCTTYPENAME").ToString().Length>10 ? DataBinder.Eval(Container, "DataItem.FLDPRODUCTTYPENAME").ToString().Substring(0, 10) + "..." : DataBinder.Eval(Container, "DataItem.FLDPRODUCTTYPENAME").ToString() %>'></telerik:RadLabel>                                    
                                <eluc:ToolTip ID="ucServicesTT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRODUCTTYPENAME") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>--%>

                        <telerik:GridTemplateColumn HeaderText="City" AllowSorting="true" SortExpression="FLDCITYNAME">
                            <HeaderStyle Width="90px" />
                            <ItemStyle Width="90px" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblCity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCITYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="State" AllowSorting="true" SortExpression="FLDSTATENAME">
                            <HeaderStyle Width="90px" />
                            <ItemStyle Width="90px" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblState" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Country" AllowSorting="true" SortExpression="FLDCOUNTRYNAME">
                            <HeaderStyle Width="90px" />
                            <ItemStyle Width="90px" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblCountry" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Status" AllowSorting="true" SortExpression="FLDHARDNAME">
                            <HeaderStyle Width="70px" />
                            <ItemStyle Width="70px" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle Width="60px" />
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="60px"></HeaderStyle>
                            <ItemStyle Wrap="false" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Relation" CommandName="RELATION" ID="cmdRelation" ToolTip="Related Address">
                                    <span class="icon"><i class="fas fa-address-card"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" />
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
