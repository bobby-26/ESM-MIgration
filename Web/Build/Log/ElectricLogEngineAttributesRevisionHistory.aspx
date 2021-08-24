<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogEngineAttributesRevisionHistory.aspx.cs" Inherits="ElectricLogEngineAttributesRevisionHistory" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Revision History</title>
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
            <eluc:Status runat="server" ID="ucStatus" />
            <table id="tblConfigure" width="100%">
                <tr>
                    <td width="35%" colspan="2">                       
                    </td>                   
                    <td width="10%">
                        <telerik:RadLabel ID="lblpublishedyn" runat="server" Text="Published YN"></telerik:RadLabel>
                    </td>
                    <td width="5%">
                        <telerik:RadCheckBox ID="ChkPublishedYN" runat="server" Enabled="false"></telerik:RadCheckBox>
                    </td>
                    <td width="10%">
                        <telerik:RadLabel ID="lblrevno" runat="server" Text="Rev No"></telerik:RadLabel>
                    </td>
                    <td width="20%">
                        <eluc:Number ID="txtrevno" Width="50px" runat="server" MaxLength="2" IsInteger="true" IsPositive="true" DecimalPlace="0" Enabled="false" />
                        <asp:LinkButton ID="cmdRevision" runat="server" AlternateText="Revision History"
                            ToolTip="Revision History">
                            <span class="icon"><i class="fas fa-history"></i></span>
                        </asp:LinkButton>
                    </td>
                    <td width="10%">
                        <telerik:RadLabel ID="lbldate" runat="server" Text="Published Date"></telerik:RadLabel>
                    </td>
                    <td width="10%">
                        <eluc:Date ID="ucdate" runat="server" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <div class="panel-heading">
                            <b>Machinery</b>
                         </div>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvOperationalHazard" runat="server" Height="600px" AllowCustomPaging="false"
                            CellSpacing="0" EnableViewState="true" GridLines="None" Font-Size="11px"
                            OnNeedDataSource="gvOperationalHazard_NeedDataSource" ShowFooter="False" ShowHeader="true" Width="100%">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" DataKeyNames="FLDENGINELOGPARAMETERID">
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
                                    <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="50%">
                                        <ItemStyle Wrap="true" HorizontalAlign="Left" Width="50%"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblParameterid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENGINELOGPARAMETERID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENGINELOGPARAMETER") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Quantity" HeaderStyle-Width="50%">
                                        <ItemStyle Wrap="true" HorizontalAlign="Left" Width="50%"></ItemStyle>
                                        <ItemTemplate>
                                            <eluc:Number ID="txtcount" Width="50px" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY") %>' MaxLength="2" IsInteger="true" IsPositive="true" DecimalPlace="0" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" EnablePostBackOnRowClick="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </td>
                    <td colspan="4">
                        <div class="panel-heading">
                            <b>Tank/Comp/Acc</b>
                         </div>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvtank" runat="server" Height="600px" AllowCustomPaging="false"
                            CellSpacing="0" EnableViewState="true" GridLines="None" Font-Size="11px"
                            OnNeedDataSource="gvtank_NeedDataSource" ShowFooter="False" ShowHeader="true" Width="100%">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" DataKeyNames="FLDENGINELOGPARAMETERID">
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
                                    <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="50%">
                                        <ItemStyle Wrap="true" HorizontalAlign="Left" Width="50%"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblParameterid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENGINELOGPARAMETERID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENGINELOGPARAMETER") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Quantity" HeaderStyle-Width="50%">
                                        <ItemStyle Wrap="true" HorizontalAlign="Left" Width="50%"></ItemStyle>
                                        <ItemTemplate>
                                            <eluc:Number ID="txtcount" Width="50px" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY") %>' MaxLength="2" IsInteger="true" IsPositive="true" DecimalPlace="0" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" EnablePostBackOnRowClick="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
