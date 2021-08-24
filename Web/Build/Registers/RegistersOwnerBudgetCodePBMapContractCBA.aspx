<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersOwnerBudgetCodePBMapContractCBA.aspx.cs" Inherits="RegistersOwnerBudgetCodePBMapContractCBA" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="YesNo" Src="~/UserControls/UserControlYesNo.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Contract" Src="~/UserControls/UserControlContractCBA.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Budget" Src="~/UserControls/UserControlBudgetCode.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>City</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    
    <div runat="server" id="dvLink">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
                
    </div>
    
</telerik:RadCodeBlock></head>
<body>
    <form id="frmRegistersCity" runat="server" submitdisabledcontrols="true">
    
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    
    <asp:UpdatePanel runat="server" ID="pnlContractEntry">
        <ContentTemplate>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="CBA Contract" ShowMenu="false" />
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuRevision" runat="server" OnTabStripCommand="Revision_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                 <table cellpadding="1" cellspacing="1" width="100%" style="border-style: none;
                    color: blue;">
                    <tr>
                        <td colspan="3">
                            <b>
                                <asp:Label ID="Label1" runat="server" EnableViewState="false" Text="Notes :" CssClass="input"
                                    BorderStyle="None" ForeColor="Blue"></asp:Label></b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Literal ID="lbl1" runat="server" Text="1."></asp:Literal>
                        </td>
                        <td>                          
                            <asp:Literal ID="lblPleasefillinthenewEffectiveDateandExpiryDateandclicknewrevision" runat="server" Text="Please fill in the new &quot;Effective Date&quot; and &quot;Expiry Date&quot; and click new revision"></asp:Literal>
                        </td>
                    </tr>                                       
                </table>
                <br />
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblPrincipal" runat="server" Text="Principal"></asp:Literal>   
                        </td>
                        <td>
                            <eluc:Address runat="server" ID="ucOwner" CssClass="dropdown_mandatory" AddressType="128"
                                    AutoPostBack="true" />
                        </td>
                        <td>
                            <asp:Literal ID="lblUnion" runat="server" Text="Union"></asp:Literal> 
                        </td>
                        <td>
                            <eluc:Address ID="ddlUnion" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                AutoPostBack="true" OnTextChangedEvent="ddlUnion_Changed" AddressType="134" />
                                <asp:Image ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" ID="imgClip" runat="server" />
                        </td>
                        <td>
                            <asp:Literal ID="lblRevisionNo" runat="server" Text="Revision No"></asp:Literal> 
                        </td>
                        <td>
                            <asp:TextBox ID="txtRevisionNo" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                           <asp:Literal ID="lblEffectiveDate" runat="server" Text="Effective Date"></asp:Literal> 
                        </td>
                        <td>
                            <eluc:Date ID="txtEffectiveDate" runat="server" CssClass="input_mandatory" />
                        </td>
                        <td>
                            <asp:Literal ID="lblExpiryDate" runat="server" Text="Expiry Date"></asp:Literal> 
                        </td>
                        <td>
                            <eluc:Date ID="txtExpiryDate" runat="server" CssClass="input_mandatory" />
                        </td>
                        <td>
                            <asp:Literal ID="lblHistory" runat="server" Text="History"></asp:Literal> 
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlHistory" runat="server" CssClass="input" DataTextField="FLDREVISIONNO" DataValueField="FLDATDTKEY" AutoPostBack="true" OnTextChanged="ddlUnion_Changed">
                            </asp:DropDownList>
                        </td>
                    </tr>                    
                </table>
                <br />
                &nbsp;<b><asp:Literal ID="lblMainComponent" runat="server" Text="Main Component"></asp:Literal> </b>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuContract" runat="server" OnTabStripCommand="Contract_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divBUDGET" style="position: relative; z-index: +1;">
                    <asp:GridView ID="gvCBA" runat="server" AutoGenerateColumns="false" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowDataBound="gvCBA_RowDataBound" OnRowDeleting="gvCBA_RowDeleting" OnRowUpdating="gvCBA_RowUpdating"
                        OnRowEditing="gvCBA_RowEditing" OnSelectedIndexChanging="gvCBA_SelectedIndexChanging" OnRowCancelingEdit="gvCBA_RowCancelingEdit"
                        ShowHeader="true" EnableViewState="false">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="SELECT" Visible="false" />
                            <asp:TemplateField HeaderText="Short Code">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle> 
                                <ItemTemplate>
                                     <asp:Label ID="lblComponentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTID")%>'></asp:Label>
                                     <asp:LinkButton ID="lnkShortName" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>'
                                        Text='<%# DataBinder.Eval(Container, "DataItem.FLDSHORTCODE")%>'></asp:LinkButton>                                    
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDTKEY")%>'></asp:Label>
                                    <asp:Label ID="lblCBAComponentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTID")%>'></asp:Label>                                   
                                    <asp:TextBox ID="txtShortCodeEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'
                                        CssClass="gridinput_mandatory" MaxLength="5"></asp:TextBox>
                                     <asp:LinkButton ID="lnkShortNameEdit" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>'
                                        Text='<%# DataBinder.Eval(Container, "DataItem.FLDSHORTCODE")%>'></asp:LinkButton>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Component Name">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>                                
                                <ItemTemplate>
                                   <%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTNAME")%>
                                </ItemTemplate>  
                                <EditItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTNAME")%>
                                </EditItemTemplate>                             
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                 <HeaderTemplate>
                                    <asp:Literal ID="lblPayabletoExternal" runat="server" Text="Payable to External"></asp:Literal>  <br /> <asp:Literal ID="lblOrganizations" runat="server" Text="Organizations"></asp:Literal> 
                                </HeaderTemplate>                                
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDPAYABLEEXTORG").ToString().Equals("1") ? "Yes" : "No"%>
                                </ItemTemplate> 
                                <EditItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDPAYABLEEXTORG").ToString().Equals("1") ? "Yes" : "No"%>
                                </EditItemTemplate>                              
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                     <asp:Literal ID="lblIncludedinContractual" runat="server" Text="Included in Contractual"></asp:Literal> <br /> <asp:Literal ID="lblDeductions" runat="server" Text="Deductions"></asp:Literal> 
                                </HeaderTemplate>     
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDINCLUDECONTDED").ToString().Equals("1") ? "Yes" : "No"%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDINCLUDECONTDED").ToString().Equals("1") ? "Yes" : "No"%>
                                </EditItemTemplate>                                
                            </asp:TemplateField>                           
                            <asp:TemplateField HeaderText="Included in Contractual Earnings">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDINCLUDECONTEAR").ToString().Equals("1") ? "Yes" : "No"%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDINCLUDECONTEAR").ToString().Equals("1") ? "Yes" : "No"%>
                                </EditItemTemplate>                               
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Supplier Payable">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                     <%# DataBinder.Eval(Container, "DataItem.FLDSUPPLIERNAME")%>
                                </ItemTemplate> 
                                <EditItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDSUPPLIERNAME")%>
                                </EditItemTemplate>                              
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblSupplierPayable" runat="server" Text="Supplier Payable"></asp:Literal> <br /> <asp:Literal ID="lblBasis" runat="server" Text="Basis"></asp:Literal> 
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDSUPPLIERPAYBASISNAME")%>
                                </ItemTemplate>  
                                <EditItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDSUPPLIERPAYBASISNAME")%>
                                </EditItemTemplate>                             
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblCalculationUnit" runat="server" Text="Calculation Unit"></asp:Literal> <br /> <asp:Literal ID="lblBasis" runat="server" Text="Basis"></asp:Literal> 
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDCALUNITBASISNAME")%>
                                </ItemTemplate>  
                                <EditItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDCALUNITBASISNAME")%>
                                </EditItemTemplate>                              
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblCalculationTime" runat="server" Text="Calculation Time"></asp:Literal>  <br /> <asp:Literal ID="lblBasis" runat="server" Text="Basis"></asp:Literal> 
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDCALTIMEBASISNAME")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDCALTIMEBASISNAME")%>
                                </EditItemTemplate>                                
                            </asp:TemplateField>
                             <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblOnboardPayableDeduction" runat="server" Text="Onboard Payable/ Deduction"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDONBPAYDEDNAME")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDONBPAYDEDNAME")%>
                                </EditItemTemplate>                              
                            </asp:TemplateField>
                            <%--<asp:TemplateField HeaderText="Applicable Vessel Types">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDVESSELTYPENAME")%>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="Currency">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDCURRENCYNAME")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDCURRENCYNAME")%>
                                </EditItemTemplate>                               
                            </asp:TemplateField>                            
                            <asp:TemplateField HeaderText="Budget Code">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDBUDGETCODE")%>
                                </ItemTemplate> 
                                <EditItemTemplate>
                                    <asp:Label ID="lblBudgetId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDBUDGETCODE")%>'></asp:Label>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDBUDGETCODE")%>
                                </EditItemTemplate>                              
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
                </div>
                <br />
                &nbsp;<b><asp:Literal ID="lblSubComponentNotePleaseselectaMainComponenttoUpdateSubComponent" runat="server" Text="Sub Component(Note : Please select a Main Component to Update Sub Component)"></asp:Literal></b>
                <asp:GridView ID="gvSubCBA" runat="server" AutoGenerateColumns="false" Font-Size="11px"
                    Width="100%" CellPadding="3" OnRowDataBound="gvSubCBA_RowDataBound" OnRowCommand="gvSubCBA_RowCommand"  OnRowDeleting="gvSubCBA_RowDeleting"
                    OnRowEditing="gvSubCBA_RowEditing" OnRowCancelingEdit="gvSubCBA_RowCancelingEdit" OnRowUpdating="gvSubCBA_RowUpdating"
                    ShowHeader="true" ShowFooter="true" EnableViewState="false">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                             <asp:TemplateField HeaderText="Short Code">
                                <ItemTemplate>
                                    <asp:Label ID="lblComponentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTID")%>'></asp:Label>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDSHORTCODE")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                     <asp:Label ID="lblComponentEditId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTID")%>'></asp:Label>
                                     <asp:TextBox ID="txtShortCodeEdit" runat="server" CssClass="input_mandatory" MaxLength="5" 
                                     Text='<%# DataBinder.Eval(Container, "DataItem.FLDSHORTCODE")%>' ToolTip="Enter Short Code"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>                                    
                                    <asp:TextBox ID="txtShortCodeAdd" runat="server" CssClass="input_mandatory" MaxLength="5"
                                            ToolTip="Enter Shprt Code"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sub Component Name">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTNAME")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                     <asp:TextBox ID="txtComponentNameEdit" runat="server" CssClass="input_mandatory" MaxLength="100" 
                                     Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTNAME")%>' ToolTip="Enter Sub Component Name"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                     <asp:TextBox ID="txtComponentNameAdd" runat="server" CssClass="input_mandatory"
                                            MaxLength="100" ToolTip="Enter Component Name"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Company Accruing">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDCOMPANYNAME")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                     <eluc:Company ID="ddlCompanyEdit" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" 
                                     CompanyList="<%#PhoenixRegistersCompany.ListCompany() %>" SelectedCompany='<%# DataBinder.Eval(Container, "DataItem.FLDACCRUALCOMPANY")%>'/>
                                </EditItemTemplate>
                                 <FooterTemplate>
                                     <eluc:Company ID="ddlCompanyAdd" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" 
                                     CompanyList="<%#PhoenixRegistersCompany.ListCompany() %>"/>
                                </FooterTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Supplier Payable">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDSUPPLIERNAME")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Address ID="ddlSupplierEdit" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" AddressType="531" 
                                        AddressList='<%#PhoenixRegistersAddress.ListAddress("531") %>' SelectedAddress='<%# DataBinder.Eval(Container, "DataItem.FLDSUPPLIERPAY")%>'/>
                                </EditItemTemplate>
                                 <FooterTemplate>
                                    <eluc:Address ID="ddlSupplierAdd" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" AddressType="531" 
                                    AddressList='<%#PhoenixRegistersAddress.ListAddress("531") %>'/>
                                </FooterTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Supplier Payable Basis">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDSUPPLIERPAYBASISNAME")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Hard ID="ddlSupplierBasisEdit" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" HardTypeCode="112" 
                                         HardList="<%#PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,112) %>"
                                        SelectedHard='<%# DataBinder.Eval(Container, "DataItem.FLDSUPPLIERPAYBASIS")%>'/>
                                </EditItemTemplate>
                                 <FooterTemplate>
                                    <eluc:Hard ID="ddlSupplierBasisAdd" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" HardTypeCode="112" 
                                        HardList="<%#PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,112) %>"/>
                                </FooterTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Calculation Unit Basis">
                                <ItemTemplate>
                                     <%# DataBinder.Eval(Container, "DataItem.FLDCALUNITBASISNAME")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Hard ID="ddlCalUnitBasisEdit" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" HardTypeCode="113"
                                         HardList="<%#PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,113) %>"
                                        SelectedHard='<%# DataBinder.Eval(Container, "DataItem.FLDCALUNITBASIS")%>'/>
                                </EditItemTemplate>
                                 <FooterTemplate>
                                    <eluc:Hard ID="ddlCalUnitBasisAdd" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" HardTypeCode="113"
                                        HardList="<%#PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,113) %>"/>
                                </FooterTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Calculation Time Basis">
                                <ItemTemplate>
                                     <%# DataBinder.Eval(Container, "DataItem.FLDCALTIMEBASISNAME")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Hard ID="ddlCalTimeBasisEdit" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" HardTypeCode="114"
                                        HardList="<%#PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,114) %>"
                                        SelectedHard='<%# DataBinder.Eval(Container, "DataItem.FLDCALTIMEBASIS")%>'/>
                                </EditItemTemplate>
                                 <FooterTemplate>
                                    <eluc:Hard ID="ddlCalTimeBasisAdd" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" HardTypeCode="114"
                                        HardList="<%#PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,114) %>"/>
                                </FooterTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Currency">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDCURRENCYNAME")%>
                                </ItemTemplate> 
                                <EditItemTemplate>
                                    <eluc:Currency ID="ddlCurrencyEdit" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" 
                                         CurrencyList="<%#PhoenixRegistersCurrency.ListActiveCurrency(PhoenixSecurityContext.CurrentSecurityContext.UserCode, true) %>"
                                        SelectedCurrency='<%# DataBinder.Eval(Container, "DataItem.FLDCURRENCYID")%>'/>
                                </EditItemTemplate>   
                                <FooterTemplate>
                                     <eluc:Currency ID="ddlCurrencyAdd" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" 
                                        CurrencyList="<%#PhoenixRegistersCurrency.ListActiveCurrency(PhoenixSecurityContext.CurrentSecurityContext.UserCode, true) %>"/>
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
                                    <asp:Literal ID="lblUnion" runat="server" Text="Union"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDUNIONNAME")%>    
                                </ItemTemplate>
                            </asp:TemplateField>
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
                <eluc:Status ID="ucStatus" runat="server" />             
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
