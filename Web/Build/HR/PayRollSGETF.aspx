<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayRollSGETF.aspx.cs" Inherits="HR_PayRollSGETF" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title>Ethnic funds donation </title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

       <%--   <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvSGETF.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>--%>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        
        <telerik:RadAjaxPanel ID="radajaxpanel1" runat="server" Height="100%">
        <%-- For Popup Relaod --%>
            <div>
        <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" style="display:none;"/>
          <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
        
         <eluc:TabStrip ID="gvmenu" runat="server" OnTabStripCommand="gvmenu_TabStripCommand" TabStrip="true"></eluc:TabStrip>
             <table id="table1">
        <tbody>
           
            <tr>
                <th>
                    <telerik:RadLabel ID="radlblcongig" runat="server" Text="Payroll "/>
                 </th>
                 <th>
                 &nbsp
                 </th>
            <td>

            <telerik:RadComboBox ID="radcbconfig" runat="server"  CssClass="input"  EmptyMessage="Select Payroll Configuration"     Width="200px" />
            
            </td>
            </tr>
           
        </tbody>
    </table>
        <eluc:TabStrip ID="gvTabStrip" runat="server" OnTabStripCommand="gvTabStrip_TabStripCommand"></eluc:TabStrip>
       
             <telerik:RadGrid RenderMode="Lightweight" ID="gvSGETF"  runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" ShowFooter="false" Style="margin-bottom: 0px" EnableViewState="true"
            OnNeedDataSource="gvSGETF_NeedDataSource"
            OnItemDataBound="gvSGETF_ItemDataBound"
                 OnItemCommand="gvSGETF_ItemCommand">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDSGPRETHNICFUNDID">
                <HeaderStyle HorizontalAlign="Center" />
                
               
                <Columns>
                    <telerik:GridTemplateColumn HeaderText='Short code' AllowSorting='true' >
                       <ItemStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            
                            <telerik:RadLabel ID="lblshortcode"  runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDETHNICFUNDSHORTCODE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText='Name' AllowSorting='true' >
                       <ItemStyle HorizontalAlign="Left" />
                        <ItemTemplate>     
                        <telerik:RadLabel runat="server" ID="lblfundId" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDSGPRETHNICFUNDID") %>'></telerik:RadLabel>
                            <asp:LinkButton ID="lblname"  runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDETHNICFUNDNAME") %>'  CommandName="FUND"/>                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                       <telerik:GridTemplateColumn HeaderText='Description ' AllowSorting='true'  >
                           <ItemStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lbldescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDETHNICFUNDDESCRIPTION") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText='Race' AllowSorting='true' >
                        <ItemStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblrace" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                     
                 
                    <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="50px" AllowSorting='true' HeaderTooltip="Action" >
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Edit"
                                CommandName="EDIT"  ID="cmdEdit"
                                ToolTip="Edit" Width="20PX">
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
                <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                    PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder" EnablePostBackOnRowClick="true">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true"  />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
                </div>
            <div>
                 <eluc:TabStrip ID="tabETFRates" runat="server" Title="SHG fund monthly contribution" Visible="false" OnTabStripCommand="tabETFRates_TabStripCommand"></eluc:TabStrip>
                             <telerik:RadGrid RenderMode="Lightweight" ID="gvETFRate"  runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" ShowFooter="false" Style="margin-bottom: 0px" EnableViewState="true"
            OnNeedDataSource="gvETFRate_NeedDataSource"
            OnItemDataBound="gvETFRate_ItemDataBound"
                                 OnItemCommand="gvETFRate_ItemCommand"
           >
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDSGPRETHNICFUNDRATEID">
                <HeaderStyle HorizontalAlign="Center" />
                
                <ColumnGroups>
                   <telerik:GridColumnGroup HeaderText="Monthly Total Wage(SGD)" Name="MTW" />
                   
                </ColumnGroups>
                <Columns>
                    <telerik:GridTemplateColumn HeaderText='Minimum' AllowSorting='true' ColumnGroupName="MTW">
                       <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            
                            <telerik:RadLabel ID="lblminmum"  runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMINGROSSWAGE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText='Maximum' AllowSorting='true' ColumnGroupName="MTW">
                       <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblmaximum"  runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAXGROSSWAGE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                       <telerik:GridTemplateColumn HeaderText='Monthly Contribution (SGD) ' AllowSorting='true'  >
                           <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblmonthlycontribution" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDETNICFUNDMONTHLYCONTRIBUTION") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                 
                    <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="50px" AllowSorting='true' HeaderTooltip="Action">
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Edit"
                                CommandName="EDIT"  ID="cmdEdit1"
                                ToolTip="Edit" Width="20PX">
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
                <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                    PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder" >
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
            </div>
         </telerik:RadAjaxPanel>
    </form>
   
</body>
</html>
