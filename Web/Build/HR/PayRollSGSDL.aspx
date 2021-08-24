<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayRollSGSDL.aspx.cs" Inherits="HR_PayRollSGSDL" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Skills Development Levy </title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

          <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvSGSDL.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
   <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    
        <telerik:RadAjaxPanel ID="radajaxpanel1" runat="server" >
        <%-- For Popup Relaod --%>
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

            <telerik:RadComboBox ID="radcbconfig" runat="server"  CssClass="input"  EmptyMessage=" Select Payroll Configuration"     Width="200px" />
            
            </td>
            </tr>
           
        </tbody>
    </table>
        <eluc:TabStrip ID="gvTabStrip" runat="server" OnTabStripCommand="gvTabStrip_TabStripCommand"></eluc:TabStrip>
       
             <telerik:RadGrid RenderMode="Lightweight" ID="gvSGSDL"  runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" ShowFooter="false" Style="margin-bottom: 0px" EnableViewState="true"
            OnNeedDataSource="gvSGSDL_NeedDataSource"
            OnItemDataBound="gvSGSDL_ItemDataBound"
                 OnItemCommand="gvSGSDL_ItemCommand">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDSGPRSDLID">
                <HeaderStyle HorizontalAlign="Center" />
                
                <ColumnGroups>
                    <telerik:GridColumnGroup HeaderText="SDL Payable (SGD)" Name="SDL" HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridColumnGroup>
                   
                </ColumnGroups>
                <Columns>
                    <telerik:GridTemplateColumn HeaderText=' SDL payable by Employer in Total Wage (%)' AllowSorting='true' >
                       <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            
                            <telerik:RadLabel ID="lblminage"  runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYERSDLCONTPERCENTAGE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                 

                       <telerik:GridTemplateColumn HeaderText='Minimum ' AllowSorting='true' ColumnGroupName="SDL" >
                           <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblmintw" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMINEMPLOYERSDLCONT") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText='Maximum' AllowSorting='true' ColumnGroupName="SDL">
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblmaxtw" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAXEMPLOYERSDLCONT") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                     
                 
                    <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="50px" AllowSorting='true' HeaderTooltip="Action">
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
