<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersOwnerBudgetCodeMap.aspx.cs"
    Inherits="RegistersOwnerBudgetCodeMap" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>--%>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BudgetCode" Src="~/UserControls/UserControlBudgetCode.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="OwnerBudgetGroup" Src="~/UserControls/UserControlOwnerBudgetGroup.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Owner Budget Code Map</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
         <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmOwnerBudgetCode" runat="server" submitdisabledcontrols="true">
  <%--  <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>--%>
         <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadAjaxPanel runat="server" ID="pnlOwnreBudgetcodeEntry" Height="100%">
        
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
          <%--  <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%;
                position: absolute;">--%>
               
                <div id="divFind">
                    <table id="tblConfigureDocumentsRequired" width="100%">
                        <tr>
                            <td style="width: 10%">
                                <asp:Literal ID="lblOwner" runat="server" Text="Owner"></asp:Literal>
                            </td>
                            <td style="width: 80%">
                                <eluc:Address runat="server" ID="ucOwner" CssClass="dropdown_mandatory" AddressType="128"
                                    AutoPostBack="true" Width="150px" />
                            </td>
                        </tr>
                    </table>
                </div>
                
                    <eluc:TabStrip ID="ESMBudgetCode" runat="server" OnTabStripCommand="ESMBudgetCode_TabStripCommand" TabStrip="true">
                    </eluc:TabStrip>
                
<%--                <div id="divBUDGET" style="position: relative; z-index: +1;">--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvESMBudgetCode" runat="server" AutoGenerateColumns="False" Font-Size="11px" 
                        GridLines="None" Width="100%" CellPadding="3" OnItemCommand="gvESMBudgetCode_OnItemCommand" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" 
                        ShowFooter="true" ShowHeader="true" EnableViewState="false" OnSortCommand="gvESMBudgetCode_OnSortCommand" OnNeedDataSource="gvESMBudgetCode_OnNeedDataSource"  >
                             <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDBUDGETID">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                      <%--  <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />--%>
                       <%-- <RowStyle Height="10px" />--%>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Budget Code">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblBudgetCodeHeader" runat="server" CommandName="Sort" CommandArgument="FLDSUBACCOUNT">Budget Code&nbsp;</asp:LinkButton>
                                    <img id="FLDSUBACCOUNT" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'
                                        CommandName="Select" CommandArgument="<%# Container.DataItem %>" />
                                    <telerik:RadLabel ID="lblbudgetid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETID") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblBudgetCodeDescriptionHeader" runat="server">Description </telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblBudgetCodeDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblOwnerBudgetCodeMappedYNHeader" runat="server">Mapped (Y/N)</telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblOwnerBudgetCodeMappedYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETCODEEXISTS") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                       <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                    </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="240px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
                    </telerik:RadGrid>
            
            <%--    <br />
                <br />
                <br />--%>
                
                    <eluc:TabStrip ID="MenuOwnerBudgetCodeMap" runat="server" OnTabStripCommand="OwnerBudgetCodeMap_TabStripCommand" TabStrip="true">
                    </eluc:TabStrip>
                
<%--                <div id="divGrid" style="position: relative; z-index: +1;">--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvOwnerBudgetCodeMap" runat="server" AutoGenerateColumns="False"
                        AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" Height="25%"
                        Font-Size="11px" GridLines="None" Width="100%" CellPadding="3" OnItemCommand="gvOwnerBudgetCodeMap_OnItemCommand"
                        OnItemDataBound="gvOwnerBudgetCodeMap_ItemDataBound" OnNeedDataSource="gvOwnerBudgetCodeMap_OnNeedDataSource"
                        OnDeleteCommand="gvOwnerBudgetCodeMap_OnDeleteCommand"
                        ShowFooter="true" ShowHeader="true" EnableViewState="true" OnSortCommand="gvOwnerBudgetCodeMap_OnSortCommand" >
                          <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDOWNERBUDGETCODEMAPID">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <%--    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />--%>
                      <%--  <RowStyle Height="10px" />--%>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Budget Code" HeaderStyle-Width="92px">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblBudgetHeader" runat="server">Budget Code&nbsp;</telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblBudget" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'>
                                    </telerik:RadLabel>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadTextBox ID="txtEsmBudgetadd" runat="server" CssClass="input_mandatory" Enabled="false"></telerik:RadTextBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderStyle-Width="120px">
                                <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblOwnerTopGroup" runat="server" Text="Owner Top Group"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblOwnerTopGroup" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARENTNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Owner Budget Group" HeaderStyle-Width="147px">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblBudgetGroupHeader" runat="server">Owner Budget Group&nbsp;</telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel runat="server" ID="lblOwnerBudgetCodeId" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETCODEMAPID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblBudgetGroup" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETGROUP") %>'>
                                    </telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Owner Budget Code" HeaderStyle-Width="142px">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblBudgetCodeHeader" runat="server">Owner Budget Code&nbsp;</telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblOwnerBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETCODE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <span id="spnPickListOwnerBudgetCodeEdit">
                                        <telerik:RadTextBox ID="txtOwnerBudgetCodeNameEdit" runat="server" Width="80%" Enabled="False"
                                            CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETCODE")%>'></telerik:RadTextBox>
                                        <asp:LinkButton ID="btnShowOwnerBudgetEdit" runat="server" 
                                            ImageAlign="AbsMiddle" Text=".." CommandName="BUDGETCODE" CommandArgument="<%# Container.DataItem %>" >
                                   <span class="icon"><i class="fas fa-tasks"></i></span></asp:LinkButton>

                                        <telerik:RadTextBox ID="txtOwnerBudgetCodeIdEdit" runat="server" Width="0px" CssClass="hidden"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETCODEID")%>'></telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtParentgroupIdEdit" runat="server" Width="0px" CssClass="hidden"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETGROUPID") %>'></telerik:RadTextBox>
                                    </span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <span id="spnPickListOwnerBudgetCodeAdd">
                                        <telerik:RadTextBox ID="txtOwnerBudgetCodeName" runat="server" Width="80%" Enabled="False"
                                            CssClass="input_mandatory"></telerik:RadTextBox>
                                        <asp:LinkButton ID="btnShowOwnerBudgetAdd" runat="server" 
                                            ImageAlign="AbsMiddle" Text=".." CommandName="BUDGETCODE" CommandArgument="<%# Container.DataItem %>" >
                                            <span class="icon"><i class="fas fa-tasks"></i></span></asp:LinkButton>
                                     <%--   <span class="icon"><i class="fas fa-tasks"></i></span>--%>
                                        <telerik:RadTextBox ID="txtOwnerBudgetCodeId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtParentgroupId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                    </span>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderStyle-Width="154px">
                                <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblOwnerCodeDescription" runat="server" Text="Owner Code Description"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblOwnerCodeDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERCODEDESCRIPTION") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderStyle-Width="154px">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" Width="15%" />
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblNotIncludeYNHeader" runat="server">Included in Owner SOA</telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblIncludeYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINCULDEINOWNERREPORTYN") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadCheckBox ID="chkIncludeYNEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDNOTINCULDEINOWNERREPORT").ToString().Equals("1"))?false:true %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadCheckBox ID="chkIncludeYNAdd" runat="server" Checked="true" />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" Width="10%" />
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblActiveYNHeader" runat="server">ActiveYN</telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblActiveYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETCODEMAPACTIVEYN") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadCheckBox ID="chkActiveYNEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDACTIVEYN").ToString().Equals("1"))?true:false %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadCheckBox ID="chkActiveYNAdd" runat="server" Checked="true" />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                              <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                            <HeaderStyle Width="88px" />
                            <ItemStyle Width="20px" Wrap="false" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
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
                                     <FooterTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Add"  CommandName="Add"  ID="cmdAdd"
                                        ToolTip="Add New"> <span class="icon"><i class="fas fa-plus-circle"></i> </span>  </asp:LinkButton>
                                </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        </Columns>
                                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                              </MasterTableView>
                          <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling  SaveScrollPosition="true"  />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
                    </telerik:RadGrid>
              
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
