<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionPEARSRARiskLevel.aspx.cs" Inherits="InspectionPEARSRARiskLevel" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Risk Level</title>
     <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
         <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvRARiskLevel.ClientID %>"));
                }, 200);
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
               Resize();
               fade('statusmessage');
            }
        </script>
    </telerik:RadCodeBlock>   
</head>
<body>
    <form id="frmRACategory" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="80%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <table>
                <tr>                    
                    <td Width="35%">
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtName" runat="server" MaxLength="100" Width="360px" CssClass="input"></telerik:RadTextBox>
                    </td>                    
                </tr>
            </table>
            <eluc:TabStrip ID="MenuRARiskLevel" runat="server" OnTabStripCommand="MenuRARiskLevel_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvRARiskLevel" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowPaging="true" AllowCustomPaging="true"
                Width="100%" CellPadding="3" OnItemCommand="gvRARiskLevel_ItemCommand" OnItemDataBound="gvRARiskLevel_ItemDataBound" EnableHeaderContextMenu="true" GroupingEnabled="false"
                OnNeedDataSource="gvRARiskLevel_NeedDataSource" ShowFooter="false" ShowHeader="true" EnableViewState="false">
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
                    <Columns>                        
                        <telerik:GridTemplateColumn HeaderText="Code" HeaderStyle-Width="10%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" ></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lnkCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Risk Level" AllowSorting="true" SortExpression="FLDNAME" ShowSortIcon="true" HeaderStyle-Width="20%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" ></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRiskLevelID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKLEVELID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKLEVEL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Min Range" HeaderStyle-Width="5%" >
                            <ItemStyle Wrap="False" HorizontalAlign="Right" ></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMinRange" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMINRANGE") %>' ></telerik:RadLabel>                                
                            </ItemTemplate>                            
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Max Range" HeaderStyle-Width="5%">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" ></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMaxRange" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAXRANGE") %>' ></telerik:RadLabel>                                
                            </ItemTemplate>                            
                        </telerik:GridTemplateColumn>
                       <%-- <telerik:GridTemplateColumn HeaderText="Color" HeaderStyle-Width="10%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <div id="divColor" runat="server" style='<%# "width:130px; height:17px; background-color:" + DataBinder.Eval(Container,"DataItem.FLDCOLOR") %>'></div>
                            </ItemTemplate>                       
                        </telerik:GridTemplateColumn>--%>
                        <telerik:GridTemplateColumn HeaderText="Remarks" HeaderStyle-Width="30%" >
                            <ItemStyle Wrap="False" HorizontalAlign="Left" ></ItemStyle>
                            <ItemTemplate>                               
                                <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Active Y/N" HeaderStyle-Width="10%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" ></ItemStyle>
                            <ItemTemplate>                               
                                <telerik:RadLabel ID="lblActiveyn" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVEYN") %>' ></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="10%">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" ></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                        <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                        <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>                            
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
        </telerik:RadAjaxPanel>
        <eluc:Status runat="server" ID="ucStatus" />
    </form>
</body>
</html>