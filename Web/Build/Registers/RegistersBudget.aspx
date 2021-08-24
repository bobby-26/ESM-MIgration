<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersBudget.aspx.cs" 
    Inherits="RegistersBudget" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Budget Code</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
     <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager  ID="ToolkitScriptManager1"
        runat="server">
    </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager" runat="server"></telerik:RadSkinManager>
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="tblBudget" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
    
       
            <eluc:TabStrip ID="MenuBudget" runat="server" OnTabStripCommand="Budget_TabStripCommand" TabStrip="true">
                    </eluc:TabStrip>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                

                <asp:Button ID="cmdHiddenSubmit" runat="server" CssClass="hidden" OnClick="cmdHiddenSubmit_Click" />
             
                    <table cellpadding="1" cellspacing="1" width="80%" >
                        
                        <tr>
                            <td width="30%">
                                <asp:Literal ID="lblSubAccountBudgetCode" runat="server" Text="Budget Code"></asp:Literal>
                            </td>
                            <td width="70%">
                                <telerik:RadTextBox ID="txtSubAccount" runat="server" MaxLength="10" Width="180px" CssClass="input_mandatory"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblDescription" runat="server" Text="Description"></asp:Literal >
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtDescription" runat="server"  MaxLength="200" Width="180px" CssClass="input_mandatory" TextMode="MultiLine" Rows="2" ></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblAccountType" runat="server" Text="Account Type"></asp:Literal>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblAccountType" runat="server" RepeatDirection="Horizontal"
                                    Width="180px" RepeatLayout="Table" CssClass="input_mandatory">
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblBudgetGroup" runat="server" Text="Budget Group"></asp:Literal>
                            </td>
                            <td>
<%--                                <eluc:Hard ID="ucHard" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"  AutoPostBack="true"
                                    OnTextChangedEvent="BudgetGroup_Changed" Width="180px"/>--%>
                                <eluc:Hard ID="ucHard" runat="server" CssClass="input_mandatory" OnTextChangedEvent="BudgetGroup_Changed" AutoPostBack="true"   />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblBudgetedExpense" runat="server" Text="Budgeted Expense" Width="60%"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadCheckBox ID="chkBudgetedExpense" runat="server"></telerik:RadCheckBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblActive" runat="server" Text="Active(Yes/No)"></asp:Literal>
                            </td>
                            <td>
                                <telerik:RadCheckBox ID="chkactive" runat="server" Checked="true"></telerik:RadCheckBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblOpenProject" runat="server" Text="Restrict to Open Projects Only"></asp:Literal>
                            </td>
                            <td>
                                <telerik:RadCheckBox ID="chkOpenProjecct" runat="server" Checked="true"></telerik:RadCheckBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblIncludefund" runat="server" Text="Included in Owner Fund Position"></asp:Literal>
                            </td>
                            <td>
                                <telerik:RadCheckBox ID="chkIncludefund" runat="server" Checked="true"></telerik:RadCheckBox>
                            </td>
                        </tr>
                          <tr>
                            <td>
                                <asp:Literal ID="lblOffsetHdr" runat="server" Text="Offsetting(Yes/No)"></asp:Literal>
                            </td>
                            <td>
                                <telerik:RadCheckBox ID="chkOffsetting" runat="server"></telerik:RadCheckBox>
                            </td>
                        </tr>
                    </table>
               
                <br />
                
                    <eluc:TabStrip ID="MenuRegistersBudget" runat="server" OnTabStripCommand="RegistersBudgetMenu_TabStripCommand" TabStrip="true">
                    </eluc:TabStrip>
               
                    <telerik:RadGrid RenderMode="Lightweight" ID="gdBudget" runat="server" AutoGenerateColumns="False" Font-Size="11px" GridLines="None"
                        AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" Height="50%"
                        Width="100%" CellPadding="3" OnItemCommand="gdBudget_ItemCommand" OnItemDataBound="gdBudget_ItemDataBound"
                         OnDeleteCommand="gdBudget_DeleteCommand" OnSortCommand="gdBudget_SortCommand" OnNeedDataSource="gdBudget_OnNeedDataSource"
                         ShowFooter="false" ShowHeader="true" EnableViewState="true">
                         <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                         <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" >
                   <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />

                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Sub Account" HeaderStyle-Width="75px">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblSubAccountHeader" runat="server" CommandName="Sort" CommandArgument="FLDSUBACCOUNT"
                                        >Code&nbsp;</asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblBudgetid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETID") %>'></telerik:RadLabel>
                                    <asp:LinkButton ID="lnkBudget" runat="server" CommandName="SELECT" CommandArgument='<%# Container.DataItem %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Description" HeaderStyle-Width="185px">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblDescriptionHeader" runat="server" CommandName="Sort" CommandArgument="FLDDESCRIPTION"
                                        >Description&nbsp;</asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Account Type" HeaderStyle-Width="105px">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblAccountType" runat="server" Text="Account Type"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblAccountType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTTYPE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Account Group" HeaderStyle-Width="150px">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblBudgetGroup" runat="server" Text="Budget Group"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblBudgetGroup" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETGROUP") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="BudgetedExpense" HeaderStyle-Width="124px">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblBudgetedExpenseYNHeader" runat="server">
                                        <asp:Literal ID="lblBudgetedExpense" runat="server" Text="Budgeted Expense"></asp:Literal>
                                    </telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblBudgetedExpense" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDBUDGETEDEXPENSE").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="IsActive" HeaderStyle-Width="105px">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblIsBudgetActive" runat="server">
                                        <asp:Literal ID="lbActive" runat="server" Text="Active(Yes/No)"></asp:Literal>
                                    </telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblIsActive" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDISACTIVE").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="IsRestrictToOpenProject" HeaderStyle-Width="182px">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblRestrictToOpenProjectHeader" runat="server" Text ="Restrict to Open Projects Only">                                    
                                    </telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRestrictToOpenProject" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDISRESTRICTTOOPENPROJECT").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                                </ItemTemplate>
                          </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Included in Owner Fund" HeaderStyle-Width="148px">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblIncludeOwnerfund" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDINCLUDEFUND").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                       <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="SELECT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        </Columns>
                                   <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="2" ScrollHeight="425px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
                    </telerik:RadGrid>
              
           
           
       
    
    </form>
</body>
</html>
