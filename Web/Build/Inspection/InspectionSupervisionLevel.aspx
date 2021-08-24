<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionSupervisionLevel.aspx.cs" Inherits="InspectionSupervisionLevel" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Supervision Level</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvRALevel.ClientID %>"));
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
     <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true"></telerik:RadWindowManager>
     <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="tblConfigure" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
     <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" >
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>            
         <eluc:Status ID="ucStatus" runat="server"></eluc:Status>   
                <div id="divFind">
                    <table id="tblConfigure" width="100%">
                        <tr>
                            <td width="15%">
                               <asp:Literal ID="lblRiskLevel" runat="server" Text="Supervision Level"></asp:Literal>                            
                            </td>
                            <td width="80%">
                                <asp:TextBox ID="txtRiskLevel" runat="server" MaxLength="200" CssClass="input"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <eluc:TabStrip ID="MenuRACategory" runat="server" OnTabStripCommand="RACategory_TabStripCommand"></eluc:TabStrip>
                    <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadformDecortor1" runat="server" DecorationZoneID="gvRALevel" DecoratedControls="All" EnableRoundedCorners="true" />
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvRALevel" runat="server" AllowCustomPaging="true" 
                         Font-Size="11px" AllowPaging="true" AllowSorting="true" OnNeedDataSource="gvRALevel_NeedDataSource" 
                        Width="100%" CellPadding="3" OnItemCommand="gvRALevel_ItemCommand" OnSortCommand="gvRALevel_SortCommand" OnItemDataBound="gvRALevel_ItemDataBound"
                         ShowFooter="false" EnableHeaderContextMenu="true" GroupingEnabled="false"
                        ShowHeader="true">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDRISKLEVELID">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Supervision Level">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblRiskLevel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKLEVEL") %>'></asp:Label>
                                    <asp:Label ID="lblRiskLevelId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEVELID") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>                           
                            <telerik:GridTemplateColumn HeaderText="Score for Supervision">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <FooterStyle HorizontalAlign="Right"></FooterStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblSupervision" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCOREFORSUPERVISION") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="ucSupervisionEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCOREFORSUPERVISION") %>'
                                        CssClass="gridinput_mandatory" IsInteger="true" IsPositive="true" MaxLength="3" />
                                    <asp:Label ID="lblRiskLevelIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEVELID") %>'></asp:Label>
                                    <telerik:RadTextBox ID="txtRiskLevelEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKLEVEL") %>'></telerik:RadTextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Number ID="ucSupervisionAdd" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCOREFORSUPERVISION") %>'
                                        CssClass="gridinput_mandatory" IsInteger="true" IsPositive="true" MaxLength="3" />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Score for use in Aspect">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <FooterStyle HorizontalAlign="Right"></FooterStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblAspects" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDASPECTSSCORE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="ucAspectsEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDASPECTSSCORE") %>'
                                        CssClass="gridinput_mandatory" IsInteger="true" IsPositive="true" MaxLength="3" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Number ID="ucAspectsAdd" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDASPECTSSCORE") %>'
                                        CssClass="gridinput_mandatory" IsInteger="true" IsPositive="true" MaxLength="3" />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>                                                       
                            <telerik:GridTemplateColumn HeaderText="Action">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>                                
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>               
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
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

