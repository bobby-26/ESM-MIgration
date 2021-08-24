<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersOwnerBudgetCodePBMapContractESM.aspx.cs" Inherits="RegistersOwnerBudgetCodePBMapContractESM" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Contract" Src="~/UserControls/UserControlContractCBA.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Budget" Src="~/UserControls/UserControlBudgetCode.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>City</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    
    <div runat="server" id="dvLink">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </div>
    
</telerik:RadCodeBlock></head>
<body>
    <form id="frmRegistersCity" runat="server">
    
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    
    <asp:UpdatePanel runat="server" ID="pnlCityEntry">
        <ContentTemplate>
        
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Standard Wage Components" ShowMenu="false" />
                    </div>
                </div>                
                 <table cellpadding="1" cellspacing="1" width="25%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblWageComponents" runat="server" Text="Wage Components"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Hard ID="ddlVessel" runat="server" HardTypeCode="156" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                AutoPostBack="true" OnTextChangedEvent="ddlVessel_Changed" />
                            <%--<eluc:Vessel ID="ddlVessel" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                AutoPostBack="true" OnTextChangedEvent="ddlVessel_Changed" VesselsOnly="true" />--%>
                        </td>
                        <td>
                                <asp:Literal ID="lblPrincipal" runat="server" Text="Principal"></asp:Literal>   
                        </td>
                        <td>
                            <eluc:Address runat="server" ID="ucOwner" CssClass="dropdown_mandatory" AddressType="128"
                                   AutoPostBack="true" />
                        </td>
                    </tr>
                </table>
                <br />                             
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuRegistersCity" runat="server" OnTabStripCommand="RegistersCity_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                
                <div id="divGrid" style="position: relative; z-index: 0">
                    <asp:GridView ID="gvCrew" runat="server" AutoGenerateColumns="false" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvCrew_RowCommand" OnRowDataBound="gvCrew_RowDataBound"
                        OnRowCancelingEdit="gvCrew_RowCancelingEdit" OnSelectedIndexChanging="gvCrew_SelectedIndexChanging"
                        OnRowDeleting="gvCrew_RowDeleting" OnRowUpdating="gvCrew_RowUpdating" OnRowEditing="gvCrew_RowEditing"
                        ShowFooter="true" ShowHeader="true" EnableViewState="false">
                        
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        
                        <Columns>
                             <asp:ButtonField Text="DoubleClick" CommandName="SELECT" Visible="false" />
                            <asp:TemplateField HeaderText="Short Code">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblComponentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>'></asp:Label>
                                    <asp:LinkButton ID="lnkShortCode" runat="server" CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></asp:Label>
                                    <asp:Label ID="lblComponentIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>'></asp:Label>
                                  <asp:TextBox ID="txtShortCodeEdit" runat="server" CssClass="input_mandatory" MaxLength="3" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                   <asp:TextBox ID="txtShortCodeAdd" runat="server" CssClass="input_mandatory" MaxLength="3" ToolTip="Enter Short Code"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Component Name">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTNAME")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                     <asp:TextBox ID="txtComponentNameEdit" runat="server" CssClass="input_mandatory" MaxLength="100" 
                                     Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTNAME")%>' ToolTip="Enter Component Name"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                     <asp:TextBox ID="txtComponentNameAdd" runat="server" CssClass="input_mandatory"
                                            MaxLength="100" ToolTip="Enter Component Name"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>                                                       
                            <asp:TemplateField HeaderText="Calculation Basis">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                     <%# DataBinder.Eval(Container, "DataItem.FLDCALCULATIONBASISNAME")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Hard ID="ddlCalBasisEdit" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" HardTypeCode="72"
                                         HardList="<%#PhoenixRegistersHard.ListHard(1,72) %>"
                                        SelectedHard='<%# DataBinder.Eval(Container, "DataItem.FLDCALCULATIONBASIS")%>'/>
                                </EditItemTemplate>
                                 <FooterTemplate>
                                    <eluc:Hard ID="ddlCalBasisAdd" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" HardTypeCode="72"
                                        HardList="<%#PhoenixRegistersHard.ListHard(1,72) %>"/>
                                </FooterTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Payable Basis">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                     <%# DataBinder.Eval(Container, "DataItem.FLDPAYABLEBASISNAME")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Hard ID="ddlPayBasisEdit" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" HardTypeCode="72" ShortNameFilter="BOC,EOC,MOC,SCC,BNC,CNC"
                                        HardList='<%#PhoenixRegistersHard.ListHard(1, 72, 0, "BOC,EOC,MOC,SCC,BNC,CNC")%>'
                                        SelectedHard='<%# DataBinder.Eval(Container, "DataItem.FLDPAYABLEBASIS")%>'/>
                                </EditItemTemplate>
                                 <FooterTemplate>
                                    <eluc:Hard ID="ddlPayBasisAdd" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" HardTypeCode="72" ShortNameFilter="BOC,EOC,MOC,SCC,BNC,CNC"
                                        HardList='<%#PhoenixRegistersHard.ListHard(1, 72, 0, "BOC,EOC,MOC,SCC,BNC,CNC")%>'/>
                                </FooterTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Budget Code">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDBUDGETCODE")%>
                                </ItemTemplate> 
                                <EditItemTemplate>
                                    <eluc:Budget ID="ddlBudgetEdit" runat="server" AppendDataBoundItems="true" CssClass="input" 
                                         BudgetCodeList="<%#PhoenixRegistersBudget.ListBudget() %>"
                                        SelectedBudgetCode='<%# DataBinder.Eval(Container, "DataItem.FLDBUDGETID")%>'/>
                                </EditItemTemplate>   
                                <FooterTemplate>
                                     <eluc:Budget ID="ddlBudgetAdd" runat="server" AppendDataBoundItems="true" CssClass="input" 
                                        BudgetCodeList="<%#PhoenixRegistersBudget.ListBudget() %>"/>
                                </FooterTemplate>
                            </asp:TemplateField> 
                            <asp:TemplateField HeaderText="Owner Budget Code">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <span id="spnPickListOwnerBudgetCodeEdit">                                        
                                        <asp:TextBox ID="txtOwnerBudgetCodeNameEdit" runat="server" Width="80%" Enabled="False" CssClass="input_mandatory"></asp:TextBox>
                                        <asp:ImageButton ID="btnShowOwnerBudgetEdit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                            ImageAlign="AbsMiddle" Text=".." CommandName="BUDGETCODE" CommandArgument="<%# Container.DataItemIndex %>" />
                                        <asp:TextBox ID="txtOwnerBudgetCodeIdEdit" runat="server" Width="0px" CssClass="hidden"></asp:TextBox>  
                                        <asp:TextBox ID="txtParentgroupIdEdit" runat="server" Width="0px" CssClass="hidden"></asp:TextBox>                                    
                                    </span>   
                                </EditItemTemplate> 
                                <FooterTemplate>
                                </FooterTemplate>                              
                            </asp:TemplateField>                      
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Update" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                        CommandName="Add" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdAdd"
                                        ToolTip="Add New"></asp:ImageButton>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <br />
                <b><asp:Literal ID="lblNoteSelectaComponenttoUpdateRankwisewages" runat="server" Text="Note: Select a Component to Update Rank wise wages."></asp:Literal></b>
                <asp:GridView ID="gvCR" runat="server" AutoGenerateColumns="false" Font-Size="11px"
                    Width="100%" CellPadding="3" OnRowCommand="gvCR_RowCommand" OnRowDataBound="gvCR_RowDataBound"
                    OnRowCancelingEdit="gvCR_RowCancelingEdit" OnRowDeleting="gvCR_RowDeleting" OnRowUpdating="gvCR_RowUpdating"
                    OnRowEditing="gvCR_RowEditing" ShowFooter="true" ShowHeader="true" EnableViewState="false">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <Columns>
                        <asp:TemplateField HeaderText="Rank">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblSubComponentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSUBCOMPONENTID")%>'></asp:Label>
                                 <%# DataBinder.Eval(Container, "DataItem.FLDRANKCODE")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblSubComponentIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSUBCOMPONENTID")%>'></asp:Label>
                                <asp:Label ID="lblRank" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDRANKID")%>'></asp:Label>
                                <%# DataBinder.Eval(Container, "DataItem.FLDRANKCODE")%>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Rank ID="ddlRankAdd" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                                    RankList="<%#PhoenixRegistersRank.ListRank() %>" AutoPostBack="true" OnTextChangedEvent="ddlRankAdd_TextChangedEvent" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Currency">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                 <%# DataBinder.Eval(Container, "DataItem.FLDCURRENCYNAME")%>
                            </ItemTemplate>
                             <EditItemTemplate>
                                    <eluc:Currency ID="ddlCurrencyEdit" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" 
                                         CurrencyList="<%#PhoenixRegistersCurrency.ListActiveCurrency(1, true) %>"
                                        SelectedCurrency='<%# DataBinder.Eval(Container, "DataItem.FLDCURRENCYID")%>'/>
                                </EditItemTemplate>   
                                <FooterTemplate>
                                     <eluc:Currency ID="ddlCurrencyAdd" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" 
                                        CurrencyList="<%#PhoenixRegistersCurrency.ListActiveCurrency(1, true) %>"/>
                                </FooterTemplate>         
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Amount">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                 <%# DataBinder.Eval(Container, "DataItem.FLDAMOUNT").ToString().Replace(".00", "") + " " + DataBinder.Eval(Container, "DataItem.FLDCALCULATIONNAME")
                                                                                                        + " " + DataBinder.Eval(Container, "DataItem.FLDMAINCOMPONENTNAME") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtAmountEdit" runat="server" CssClass="input_mandatory" Width="100px" Text='<%# DataBinder.Eval(Container, "DataItem.FLDAMOUNT")%>' />
                                <%--<eluc:Hard ID="ddlCalculationEdit" runat="server" AppendDataBoundItems="true" 
                                HardTypeCode="116" HardList="<%#PhoenixRegistersHard.ListHard(1,116) %>" CssClass="input" />
                                 <eluc:Contract ID="ddlContractEdit" runat="server" AppendDataBoundItems="true" CssClass="input" />--%>
                            </EditItemTemplate>
                             <FooterTemplate>                                 
                                <eluc:Number ID="txtAmountAdd" runat="server" CssClass="input_mandatory" Width="100px" />
                                <%--<eluc:Hard ID="ddlCalculationAdd" runat="server" AppendDataBoundItems="true" 
                                HardTypeCode="116" HardList="<%#PhoenixRegistersHard.ListHard(1,116) %>" CssClass="input" />
                                <eluc:Contract ID="ddlContractAdd" runat="server" AppendDataBoundItems="true" CssClass="input" ContractList='<%#ViewState["Rank"].ToString() != string.Empty ? PhoenixRegistersContract.ListCBAContract(General.GetNullableInteger(ViewState[IsOfficer(int.Parse(ViewState["Rank"].ToString())) ? "OFFICERS" : "RATINGS"].ToString()), null, null)
                                                                                                                                            : new System.Data.DataTable()%>' />--%>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="Update" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                    CommandName="Add" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdAdd"
                                    ToolTip="Add New"></asp:ImageButton>
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <br />
                &nbsp;<b><asp:Literal ID="lblMappedComponents" runat="server" Text="Mapped Components"></asp:Literal></b>
                 <asp:GridView ID="gvOwner" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowDataBound="gvOwner_RowDataBound" ShowHeader="true"
                        EnableViewState="false" AllowSorting="true" OnSorting="gvOwner_Sorting" OnSelectedIndexChanging="gvOwner_SelectedIndexChanging"
                        OnRowCommand="gvOwner_RowCommand">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblComponentName" runat="server" Text="Component Name"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTNAME")%>    
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblPrincipal" runat="server" Text="Principal"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDPRINCIAPL")%>   
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblOwnerBudgetCode" runat="server" Text="Owner Budget Code"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDOWNERBUDGETNAME")%>   
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                 </asp:GridView>
                 <div id="divPage" style="position: relative;">
                    <table width="100%" border="0" cellpadding="1" cellspacing="1" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap align="center">
                                <asp:Label ID="lblPagenumber" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblPages" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblRecords" runat="server">
                                </asp:Label>&nbsp;&nbsp;
                            </td>
                            <td nowrap align="left" width="50px">
                                <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap align="right" width="50px">
                                <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                            </td>
                            <td nowrap align="center">
                                <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                </asp:TextBox>
                                <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                    Width="40px"></asp:Button>
                            </td>
                        </tr>
                    </table>
                 </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
