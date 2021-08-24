<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayRollSGEmployeeConfig.aspx.cs" Inherits="HR_PayRollSGEmployeeConfig" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Employee Payroll Configuration</title>
     <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

          <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvemployeeconfig.ClientID %>"));
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
        <%--<eluc:TabStrip ID="gvTabStrip" runat="server" OnTabStripCommand="gvTabStrip_TabStripCommand"></eluc:TabStrip>--%>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvemployeeconfig"  runat="server"   AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" EnableLinqExpressions="false" EnableViewState="true" AllowFilteringByColumn="true"
            CellSpacing="0" GridLines="None" ShowFooter="false" Style="margin-bottom: 0px"  
            OnNeedDataSource="gvemployeeconfig_NeedDataSource"
            OnItemDataBound="gvemployeeconfig_ItemDataBound"
                OnItemCommand="gvemployeeconfig_ItemCommand">
            
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDEMPLOYEEID" TableLayout="Fixed">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ColumnGroups>
                    <telerik:GridColumnGroup Name="FW" HeaderText="Foreign Worker"  />
                     <telerik:GridColumnGroup Name="EF" HeaderText="Date" ParentGroupName="FW"  />
                </ColumnGroups>
                <Columns>
                    <telerik:GridTemplateColumn HeaderText='Employee code' AllowSorting='true' DataField="FLDEMPLOYEE" UniqueName="FLDEMPLOYEE" FilterControlWidth="150px" FilterDelay="2000"  AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                       
                         <ItemTemplate>
                            <telerik:RadLabel ID="lblcode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEECODE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn HeaderText='Name ' AllowSorting='true' AllowFiltering
                        ="false">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblname" runat="server" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                       <telerik:GridTemplateColumn HeaderText='Joining date ' AllowSorting='true' ItemStyle-HorizontalAlign="Right" AllowFiltering="false">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lbljoiningdate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATEOFJOINING").ToString()) %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <%-- <telerik:GridTemplateColumn HeaderText='Race' AllowSorting='true'>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblrace" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRACE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>--%>
                    <telerik:GridTemplateColumn HeaderText='Country' AllowSorting='true' AllowFiltering="false">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblcountry" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                      <%-- <telerik:GridTemplateColumn HeaderText='Income-tax reference number' AllowSorting='true'>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lbltaxreferencenumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINCOMETAXREFERENCENUMBER") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>--%>
                      <telerik:GridTemplateColumn HeaderText='Type' AllowSorting='true' ItemStyle-Wrap="true" ColumnGroupName="FW" AllowFiltering="false">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lbltype" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFOREIGNWORKERTYPE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                      <telerik:GridTemplateColumn HeaderText='Number' AllowSorting='true' HeaderStyle-Width="100px" ColumnGroupName="FW" ItemStyle-HorizontalAlign="Left" AllowFiltering="false">
                        <ItemTemplate>
                                                      <%--  <telerik:RadLabel ID="RadLabel1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSPASSNUMBER") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="RadLabel2" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEPASSNUMBER") %>'></telerik:RadLabel>

                            <telerik:RadLabel ID="lblnumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFOREIGNWORKPERMITNUMBER") %>'></telerik:RadLabel>--%>
                                                   <telerik:RadLabel ID="RadLabel3" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKPERMITNUMBER") %>'></telerik:RadLabel>

                             </ItemTemplate>
                    </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn HeaderText='Skill' AllowSorting='true' HeaderStyle-Width="100px" ColumnGroupName="FW" AllowFiltering="false">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblskill" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSKILL") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn HeaderText='FWL Tier' AllowSorting='true' HeaderStyle-Width="100px" ColumnGroupName="FW" AllowFiltering="false">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lbltier" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSFPRFWLTEIR") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn HeaderText='Apply ' AllowSorting='true' HeaderStyle-Width="100px" ColumnGroupName="EF" ItemStyle-HorizontalAlign="Right" AllowFiltering="false">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblapplydate" runat="server" Text='<%# General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDAPPLYDATE").ToString()) %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                 <telerik:GridTemplateColumn HeaderText='Issue' AllowSorting='true' HeaderStyle-Width="100px" ColumnGroupName="EF" ItemStyle-HorizontalAlign="Right" AllowFiltering="false">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblfromdate" runat="server" Text='<%#  General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDWORKINGPASSEFFECTVEFROMDATE").ToString()) %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                      <telerik:GridTemplateColumn HeaderText='Expiry' AllowSorting='true' HeaderStyle-Width="100px" ColumnGroupName="EF" ItemStyle-HorizontalAlign="Right" AllowFiltering="false">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lbltodate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDWORKINGPASSEFFECTIVETODATE").ToString()) %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="50px" AllowSorting='true' HeaderTooltip="Action"  AllowFiltering="false" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate >
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
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>

             </telerik:RadAjaxPanel>
    </form>
</body>
</html>
