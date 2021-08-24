<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountAirfareNonVesselRegister.aspx.cs"
    Inherits="Accounts_AccountAirfareNonVesselRegister" %>

<%@ Import Namespace="SouthNests.Phoenix.Accounts" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStripTelerik" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Month" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Year" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCompany" Src="~/UserControls/UserControlCompany.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Airfare Account Code Register</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
      <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

</telerik:RadCodeBlock></head>
<body>
    <form id="frmOpeningsummary" runat="server" submitdisabledcontrols="true">
       <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" Height="100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
            
                        
                 
                         <eluc:TabStrip ID="MenuAirfareMain" runat="server" OnTabStripCommand="MenuAirfareMain_TabStripCommand"
                            TabStrip="true"></eluc:TabStrip>
                    <br />
                    <table cellpadding="2" cellspacing="2" width="50%">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblAccount" runat="server" Text="Account "></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtAccountcode" runat="server" CssClass="input"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblCompanyName" runat="server" Text="Company"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:UserControlCompany ID="ddlBillToCompany" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                                    CssClass="input_mandatory" runat="server" AppendDataBoundItems="true" />
                            </td>
                        </tr>
                    </table>
                    <br />
                   
                        <eluc:TabStrip ID="MenuOpeningSummaryGrid" runat="server" OnTabStripCommand="MenuOpeningSummaryGrid_TabStripCommand"></eluc:TabStrip>
                
                    

            
           <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator2" runat="server" DecorationZoneID="gvAirfare" DecoratedControls="All" EnableRoundedCorners="true" />
                <telerik:RadGrid RenderMode="Lightweight" ID="gvAirfare" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" Height="77%"
                    CellSpacing="0" GridLines="None" OnNeedDataSource="gvAirfare_NeedDataSource" AllowMultiRowSelection="true" FilterType="CheckList"
                    EnableViewState="false" EnableHeaderContextMenu="true" Width="100%" GroupingEnabled="false" OnSelectedIndexChanging="gvAirfare_SelectedIndexChanging"
                    OnItemDataBound="gvAirfare_ItemDataBound" OnItemCommand="gvAirfare_ItemCommand"
                    ShowFooter="true" ShowHeader="true" >
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDAIRFARENONVESSELREGISTERID">
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Account" HeaderStyle-Width="15%">
                                   
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblairfarenonvesselregisterId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAIRFARENONVESSELREGISTERID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblAccountCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadLabel ID="lblairfarenonvesselregisterIdEdit" runat="server" Visible="false"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDAIRFARENONVESSELREGISTERID") %>'></telerik:RadLabel>
                                        <span id="spnPickListAccountEdit">
                                            <telerik:RadTextBox ID="txtAccountCodeEdit" runat="server" CssClass="input_mandatory"
                                                MaxLength="20" Width="60px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE")  %>'></telerik:RadTextBox>
                                            <asp:ImageButton ID="imgShowAccountEdit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                                ImageAlign="AbsMiddle" Text=".." CommandName="picklist" CommandArgument="<%# Container.DataSetIndex %>" />
                                            <telerik:RadTextBox ID="txtAccountDescriptionEdit" runat="server" CssClass="hidden" Enabled="false"
                                                MaxLength="50" Width="0px"></telerik:RadTextBox>
                                            <telerik:RadTextBox ID="txtAccountIdEdit" runat="server" CssClass="hidden" MaxLength="20"
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTID")  %>' Width="0px"></telerik:RadTextBox>
                                            <telerik:RadTextBox ID="txtAccountSourceEdit" CssClass="hidden" runat="server" Width="0px"></telerik:RadTextBox>
                                            <telerik:RadTextBox ID="txtAccountUsageEdit" CssClass="hidden" runat="server" Width="0px"></telerik:RadTextBox>
                                            <telerik:RadTextBox ID="txtBudgetCodeEdit" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                            <telerik:RadTextBox ID="txtBudgetNameEdit" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                            <telerik:RadTextBox ID="txtBudgetIdEdit" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                            <telerik:RadTextBox ID="txtBudgetgroupIdEdit" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                        </span>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <span id="spnPickListAccount">
                                            <telerik:RadTextBox ID="txtAccountCode" runat="server" CssClass="input_mandatory" MaxLength="20"
                                        Width="65%">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtAccountDescription" runat="server" CssClass="hidden" MaxLength="50"
                                        Width="1%">
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="imgShowAccount" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." CommandName="BUDGETCODE" CommandArgument="<%# Container.DataSetIndex %>" />
                                    <telerik:RadTextBox ID="txtAccountId" runat="server" CssClass="hidden" MaxLength="20" Width="10px"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtAccountSource" CssClass="hidden" runat="server"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtAccountUsage" CssClass="hidden" runat="server"></telerik:RadTextBox>
                                    <br />
                                    <telerik:RadTextBox ID="txtBudgetCode" runat="server" Width="60px" CssClass="hidden"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetName" runat="server" Width="180px" CssClass="hidden"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetgroupId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Description" HeaderStyle-Width="15%">
                                    
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblAccountCodeDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Bill To Company" HeaderStyle-Width="25%">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                  
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblCompany" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:UserControlCompany ID="ddlCompanyEdit" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                                            CssClass="input_mandatory" runat="server" AppendDataBoundItems="true" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:UserControlCompany ID="ddlCompanyAdd" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                                            CssClass="input_mandatory" runat="server" AppendDataBoundItems="true" />
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Mark Up" HeaderStyle-Width="10%">
                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                    
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblActiveYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDMARKUP").ToString().Equals("Yes"))?"Yes":"No" %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:CheckBox ID="chkActiveYNEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDMARKUP").ToString().Equals("Yes"))?true:false %>' />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:CheckBox ID="chkActiveYN" runat="server" Checked="true" />
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="10%">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                  
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" ></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                            CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                            ToolTip="Edit"></asp:ImageButton>
                                        <%--<img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />--%>
                                        <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="Delete" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                            ToolTip="Delete"></asp:ImageButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                            CommandName="Save" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                            ToolTip="Save"></asp:ImageButton>
                                        <%--<img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />--%>
                                        <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                            ToolTip="Cancel"></asp:ImageButton>
                                    </EditItemTemplate>
                                    <FooterStyle HorizontalAlign="Center" />
                                    <FooterTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                            CommandName="Add" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAdd"
                                            ToolTip="Add New"></asp:ImageButton>
                                    </FooterTemplate>
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
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                        EnablePostBackOnRowClick="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="2" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
              </telerik:RadAjaxPanel>
                 
      </form>
</body>
</html>



