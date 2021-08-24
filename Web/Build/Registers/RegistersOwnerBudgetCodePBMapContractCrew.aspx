<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersOwnerBudgetCodePBMapContractCrew.aspx.cs" Inherits="RegistersOwnerBudgetCodePBMapContractCrew" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="System.Data" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
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
                        <eluc:Title runat="server" ID="ucTitle" Text="Components Agreed with Crew" ShowMenu="false" />
                    </div>
                </div>
                
                <div>
                    <table cellpadding="1" cellspacing="1" width="100%">
                        <tr>
                            <td>
                                <asp:Literal ID="lblPrincipal" runat="server" Text="Principal"></asp:Literal>   
                            </td>
                            <td>
                                <eluc:Address runat="server" ID="ucOwner" CssClass="dropdown_mandatory" AddressType="128"
                                        AutoPostBack="true" />
                            </td>
                         </tr>
                     </table>   
                </div>                
                                             
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuRegistersCity" runat="server" OnTabStripCommand="RegistersCity_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                
                <div id="divGrid" style="position: relative; z-index: 0">
                    <asp:GridView ID="gvCrew" runat="server" AutoGenerateColumns="false" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvCrew_RowCommand" OnRowDataBound="gvCrew_RowDataBound"
                        OnRowCancelingEdit="gvCrew_RowCancelingEdit"
                        OnRowDeleting="gvCrew_RowDeleting" OnRowUpdating="gvCrew_RowUpdating" OnRowEditing="gvCrew_RowEditing"
                        ShowFooter="true" ShowHeader="true" EnableViewState="false">
                        
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField HeaderText="Short Code">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>                               
                                <ItemTemplate>
                                    <asp:Label ID="lblComponentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>'></asp:Label>
                                    <asp:LinkButton ID="lnkShortCode" runat="server" CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblComponentIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>'></asp:Label>
                                    <asp:Label ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></asp:Label>
                                  <asp:TextBox ID="txtShortCodeEdit" runat="server" CssClass="input_mandatory" MaxLength="3" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                   <asp:TextBox ID="txtShortCodeAdd" runat="server" CssClass="input_mandatory" MaxLength="3" ToolTip="Enter Short Code"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Component Name">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTNAME")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                     <asp:TextBox ID="txtComponentNameEdit" runat="server" CssClass="gridinput_mandatory" MaxLength="100" 
                                     Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTNAME")%>' ToolTip="Enter Component Name"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                     <asp:TextBox ID="txtComponentNameAdd" runat="server" CssClass="input_mandatory"
                                            MaxLength="100" ToolTip="Enter Component Name"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Vessel Chargeable">
                                <ItemTemplate>
                                     <%# DataBinder.Eval(Container, "DataItem.FLDVESSELCHARGEABLENAME")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                     <eluc:Hard ID="ddlVesselChargeEdit" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" HardTypeCode="115"
                                         HardList="<%#PhoenixRegistersHard.ListHard(1,115) %>"
                                        SelectedHard='<%# DataBinder.Eval(Container, "DataItem.FLDVESSELCHARGEABLE")%>'/>
                                </EditItemTemplate>
                                <FooterTemplate>
                                     <eluc:Hard ID="ddlVesselChargeAdd" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" HardTypeCode="115"
                                         HardList="<%#PhoenixRegistersHard.ListHard(1,115) %>"/>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Budget Codes">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                     <span id="spnPickListTaxBudgetEdit">
                                        <asp:TextBox ID="txtBudgetCodeEdit" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'
                                            MaxLength="20" CssClass="input_mandatory" Width="80%"></asp:TextBox>
                                        <asp:TextBox ID="txtBudgetNameEdit" runat="server" Width="0px" CssClass="input" Enabled="False"></asp:TextBox>
                                        <asp:ImageButton ID="btnShowBudgetEdit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                            ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataItemIndex %>" />
                                        <asp:TextBox ID="txtBudgetIdEdit" runat="server" Width="0px" CssClass="input" Text='<%#DataBinder.Eval(Container,"DataItem.FLDBUDGETID") %>'></asp:TextBox>
                                        <asp:TextBox ID="txtBudgetgroupIdEdit" runat="server" Width="0px" CssClass="input"></asp:TextBox>
                                    </span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <span id="spnPickListTaxBudgetAdd">
                                        <asp:TextBox ID="txtBudgetCodeAdd" runat="server" MaxLength="20" CssClass="input_mandatory" Width="80%"></asp:TextBox>
                                        <asp:TextBox ID="txtBudgetNameAdd" runat="server" Width="0px" CssClass="input" Enabled="False"></asp:TextBox>
                                        <asp:ImageButton ID="btnShowBudgetAdd" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                            ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataItemIndex %>" />
                                        <asp:TextBox ID="txtBudgetIdAdd" runat="server" Width="0px" CssClass="input"></asp:TextBox>
                                        <asp:TextBox ID="txtBudgetgroupIdAdd" runat="server" Width="0px" CssClass="input"></asp:TextBox>
                                    </span>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Calculation Basis">
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
                                <ItemTemplate>
                                     <%# DataBinder.Eval(Container, "DataItem.FLDPAYABLEBASISNAME")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Hard ID="ddlPayBasisEdit" runat="server" AppendDataBoundItems="true" ShortNameFilter="BOC,EOC,MOC,SCC,BNC,CNC" CssClass="input_mandatory" HardTypeCode="72"
                                        HardList='<%#PhoenixRegistersHard.ListHard(1, 72, 0, "BOC,EOC,MOC,SCC,BNC,CNC")%>'
                                        SelectedHard='<%# DataBinder.Eval(Container, "DataItem.FLDPAYABLEBASIS")%>'/>
                                </EditItemTemplate>
                                 <FooterTemplate>
                                    <eluc:Hard ID="ddlPayBasisAdd" runat="server" AppendDataBoundItems="true" ShortNameFilter="BOC,EOC,MOC,SCC,BNC,CNC" CssClass="input_mandatory" HardTypeCode="72"
                                        HardList='<%#PhoenixRegistersHard.ListHard(1, 72, 0, "BOC,EOC,MOC,SCC,BNC,CNC")%>'/>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Included Onboard">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDINCLUDEDONBOARDYNNAME")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:RadioButtonList ID="rblYesNoEdit" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:RadioButtonList ID="rblYesNoAdd" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Description">
                                <ItemTemplate>
                                     <%# DataBinder.Eval(Container, "DataItem.FLDDESCRIPTION")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEditDescription" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDESCRIPTION")%>' CssClass="input"></asp:TextBox>    
                                </EditItemTemplate>
                                 <FooterTemplate>
                                    <asp:TextBox ID="txtAddDescription" runat="server" CssClass="input"></asp:TextBox>
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
                             <asp:TemplateField HeaderText="Earning Deduction">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDEARNINGDEDUCTIONNAME")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:RadioButtonList ID="rblEarningDeductionEdit" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="Earning" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Deduction" Value="-1"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:RadioButtonList ID="rblEarningDeductionAdd" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="Earning" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Deduction" Value="-1"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </FooterTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Active YN">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDACTIVEYNNAME")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:RadioButtonList ID="rblActiveYesNoEdit" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:RadioButtonList ID="rblActiveYesNoAdd" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="Yes" Value="1" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                    </asp:RadioButtonList>
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
