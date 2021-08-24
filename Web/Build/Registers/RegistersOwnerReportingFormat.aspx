<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersOwnerReportingFormat.aspx.cs"
    Inherits="RegistersOwnerReportingFormat" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Accounts" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TreeView" Src="~/UserControls/UserControlTreeView.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VerticalSplit" Src="~/UserControls/UserControlVerticalSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SubReport" Src="~/UserControls/UserControlOwnerSubReportType.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>OwnerBudgetCode</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmOwnerBudgetCode" runat="server">
        <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlCreditNote">
            <ContentTemplate>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div style="top: 100px; margin-left: auto; margin-right: auto; vertical-align: middle;">
                 
                       
                        
                            <eluc:TabStrip ID="MenuBudgetTab" runat="server" TabStrip="true" OnTabStripCommand="BudgetTab_TabStripCommand"></eluc:TabStrip>
                           
                  
                            <eluc:TabStrip ID="MenuOwnerBudgetCodeMain" runat="server" OnTabStripCommand="MenuOwnerBudgetCodeMain_TabStripCommand" TabStrip="true"></eluc:TabStrip>
                  

                    <div id="divFind">
                        <table id="tblConfigure" width="80%">
                            <tr>
                                <td style="width: 20%">
                                    <asp:Literal ID="lblOwner" runat="server" Text="Owner"></asp:Literal>
                                </td>
                                <td  style="width: 20%">
                                    <eluc:Address runat="server" ID="ucOwner" CssClass="dropdown_mandatory" AddressType="128"
                                        OnTextChangedEvent="ucOwner_Onchange" AutoPostBack="true" AppendDataBoundItems="true" Width="120px" />
                                </td> 
                                <td style="width:10%"></td>  
                                <td style="width:30%">
                                    <asp:Literal ID="lblReceivedDate" runat="server" Text="Committed Cost Based on Goods Received"></asp:Literal>
                                </td> 
                                <td style="width:20%">
                                     <asp:CheckBox ID="chkReceivedDateYN" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%">
                                    <asp:Literal ID="lblCommittedCostsShownYN" runat="server" Text="Committed Costs Shown in Statement of Account"></asp:Literal>
                                </td>
                                <td style="width: 10%">
                                    <asp:CheckBox ID="chkCommittedCostsShownYN" runat="server" />
                                </td>
                                <td style="width: 20%"></td>
                                <td style="width: 20%">
                                    <asp:Literal ID="Literal2" runat="server" Text="Show Zero Amount Voucher"></asp:Literal>
                                </td>
                                <td style="width: 20%">
                                    <asp:CheckBox ID="chkShowZeroAmountVoucherYN" runat="server" />
                                </td>                  
                            </tr>
                            <tr>
                                <td style="width: 30%">
                                    <asp:Literal ID="Literal1" runat="server" Text="PO description shown in Committed Cost voucher"></asp:Literal>
                                </td>
                                <td style="width: 10%">
                                    <asp:CheckBox ID="chkPOdescriptionShownYN" runat="server" />
                                </td>
                                <td style="width: 20%" ></td>
                                  <td style="width: 20%">
                                    <asp:Literal ID="lblStatememt" Text="Mapped Statement of Reference" runat="server"></asp:Literal>
                                </td>
                                <td style="width: 20%" >                                    
                                   <%-- <asp:TextBox ID="txtStatement" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="300px" TextMode="MultiLine" Rows="2" ></asp:TextBox>--%>
                                    <asp:Label ID="txtStatement" runat="server" Width="400px" ForeColor="Blue"></asp:Label>
                                </td>                                    
                            </tr>
                        </table>
                    </div>
                    <br />
                    <div style="overflow: scroll; width: 20%; float: left; height: 500px;" id="divOwnerBGroup">
                        <%--<div class="navSelect" style="position: relative; width: 15px">
                <eluc:TabStrip ID="MenuOwnerBGroupExport" runat="server" OnTabStripCommand="OwnerBGroupExport_TabStripCommand">
                </eluc:TabStrip>
            </div>--%>
                        <table style="float: left; width: 100%;">
                            <tr style="position: absolute">
                                <eluc:TreeView runat="server" ID="tvwOwnerBudgetCode" OnSelectNodeEvent="ucTree_SelectNodeEvent"></eluc:TreeView>
                                <asp:Label runat="server" ID="lblSelectedNode"></asp:Label>
                            </tr>
                        </table>
                    </div>
                    <eluc:VerticalSplit runat="server" ID="ucVerticalSplit" TargetControlID="divOwnerBGroup" />
                    <div style="position: relative; float: left; margin: 5px; width: 25%">
                        <table width="100%" cellpadding="5">
                            <tr>
                                <td>
                                    <asp:Literal ID="lblOwnerBudgetCode" runat="server" Text="Owner Budget Code"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtOwnerBudgetcode" CssClass="readonlytextbox" Width="200" runat="server" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="lblOwnerBudgetCodeDescription" runat="server" Text="Owner Budget Code Description"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtbudgetdesc" CssClass="readonlytextbox" Width="200" runat="server" ReadOnly="true"
                                        Height="60" TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="lblBudgeted" runat="server" Text="Budgeted"></asp:Literal>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkBudgeted" runat="server" Enabled="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="lblStatementReference" runat="server" Text="Statement Reference Type"></asp:Literal>
                                </td>
                                <td>
                                    <telerik:RadDropDownList ID="ddlType" runat="server" CssClass="input_mandatory">
                                        <Items>
                                        <telerik:DropDownListItem Value="Dummy" Text="--Select--"></telerik:DropDownListItem>
                                        <telerik:DropDownListItem Value="Monthly Report" Text="Monthly Report"></telerik:DropDownListItem>
                                        <telerik:DropDownListItem Value="Non-Budgeted Report" Text="Non-Budgeted Report"></telerik:DropDownListItem>
                                        <telerik:DropDownListItem Value="Predelivery Report" Text="Predelivery Report"></telerik:DropDownListItem>
                                        <telerik:DropDownListItem Value="Dry Docking Report" Text="Dry Docking Report"></telerik:DropDownListItem>
                                            </Items>
                                    </telerik:RadDropDownList>
                                </td>
                            </tr>

                        </table>                    
                    </div>
                    <eluc:VerticalSplit runat="server" ID="VerticalSplit1" TargetControlID="ucVerticalSplit" />                       
                    <div style="position: relative; float: left; margin: 5px; width: 40%">
                        <asp:Panel ID="pnl1" runat="server" Height="250px" BorderWidth="1px" ScrollBars="Auto" Width="540px" GroupingText="Consolidated PDF">                   
                                <%--<asp:Label ID="lblhdr1" runat="server" Font-Bold="true" ForeColor="Blue" Visible="false">Consolidated PDF</asp:Label>--%>
                            <table>
                                <tr>
                                    <td>
                                        Reference Type :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlStatementReferenceType" runat="server" CssClass="input_mandatory" AutoPostBack="true"  OnSelectedIndexChanged="ddlStatementReferenceType_OnSelectedIndexChanged" >
                                            
                                        <asp:ListItem Value="Dummy">--Select--</asp:ListItem>
                                        <asp:ListItem Value="Monthly Report">Monthly Report</asp:ListItem>
                                        <asp:ListItem Value="Non-Budgeted Report">Non-Budgeted Report</asp:ListItem>
                                        <asp:ListItem Value="Predelivery Report">Predelivery Report</asp:ListItem>
                                        <asp:ListItem Value="Dry Docking Report">Dry Docking Report</asp:ListItem>
                                    </asp:DropDownList>               
                                    </td>
                                </tr>
                             
                                </table>
                                <table width="100%" cellpadding="5">                                                                        
                                    <asp:GridView ID="dgFinancialYearSetup" runat="server" AutoGenerateColumns="False"
                                        CellPadding="3" Font-Size="11px" OnRowCommand="dgFinancialYearSetup_RowCommand"
                                        OnRowDataBound="dgFinancialYearSetup_ItemDataBound" OnRowDeleting="dgFinancialYearSetup_RowDeleting"
                                        OnSorting="dgFinancialYearSetup_Sorting" OnRowEditing="dgFinancialYearSetup_RowEditing"
                                        OnRowCancelingEdit="dgFinancialYearSetup_RowCancelingEdit" OnRowCreated="dgFinancialYearSetup_RowCreated"
                                        AllowSorting="true" ShowFooter="True" Style="margin-bottom: 0px" Width="100%"
                                        EnableViewState="false" GridLines="None">
                                        <FooterStyle CssClass="datagrid_footerstyle" />
                                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                        <RowStyle Height="10px" />
                                        <Columns>
                                            <asp:ButtonField CommandName="Edit" Text="DoubleClick" Visible="false" />
                                            <asp:TemplateField>
                                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblReportTypeHeader" runat="server">Report Type&nbsp;</asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReportTypeID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTCODE") %>'></asp:Label>
                                                    <asp:Label ID="lblReportType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTTYPE") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:DropDownList ID="ddlOwnerReportType" runat="server" CssClass="input" AutoPostBack="true" OnSelectedIndexChanged="ddlOwnerReportType_OnSelectedIndexChanged"></asp:DropDownList>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblSubReportTypeHeader" runat="server">Sub Report Type&nbsp;</asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSubReportTypeID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBREPORTCODE") %>'></asp:Label>
                                                    <asp:Label ID="lblSubReportType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBREPORTTYPE") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:DropDownList ID="ddlOwnerSubReportType" runat="server" CssClass="input"></asp:DropDownList>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Sort Order">
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblSortOrderHeader" runat="server">Order</asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSortOrder" runat="server" CommandArgument="<%# Container.DataItemIndex %>"
                                                        CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSORTORDER") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtEditSortOrder" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSORTORDER") %>'
                                                        CssClass="input_mandatory" Width="30px" />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtSortOrder" runat="server" CssClass="input_mandatory" Width="30px"></asp:TextBox>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblActionHeader" runat="server"> Action </asp:Label>
                                                </HeaderTemplate>
                                                <FooterTemplate>
                                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                                    <asp:ImageButton ID="cmdAdd" runat="server" AlternateText="Save" CommandArgument="<%# Container.DataItemIndex %>"
                                                        CommandName="Add" ImageUrl="<%$ PhoenixTheme:images/te_check.png%>" ToolTip="Add New" />
                                                </FooterTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="100px" Wrap="False" />
                                                <ItemTemplate>
                                                    <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                                    <asp:ImageButton ID="cmdEdit" runat="server" AlternateText="Edit" CommandArgument="<%# Container.DataItemIndex %>"
                                                        CommandName="EDIT" ImageUrl="<%$ PhoenixTheme:images/te_edit.png%>" ToolTip="Edit" />
                                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                                    <asp:ImageButton ID="cmdDelete" runat="server" AlternateText="Delete" CommandArgument="<%# Container.DataItemIndex %>"
                                                        CommandName="DELETE" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>" ToolTip="Delete" />
                                                    <img id="Img5" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <img id="Img4" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                                    <asp:ImageButton ID="cmdSave" runat="server" AlternateText="Save" CommandArgument="<%# Container.DataItemIndex %>"
                                                        CommandName="Save" ImageUrl="<%$ PhoenixTheme:images/save.png%>" ToolTip="Save" />
                                                    <img id="Img5" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                                    <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                                        CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                                        ToolTip="Cancel"></asp:ImageButton>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </table> 
                                </asp:Panel>  
                            <asp:Panel ID="Pnl3" runat="server" Height="195px" BorderWidth="1px" ScrollBars="Auto"  Width="540px" GroupingText="Owner Access rights"   >                                         
                             <table>                                   
                                    <tr>      
                                        <td colspan="4">                                           
                                        </td>                                                                         
                                    </tr>
                                    <tr>                                        
                                        <td>Vessel Trial Balance</td> 
                                        <td>:</td>                                       
                                        <td>
                                            <asp:DropDownList ID="ddlVesselTrialBalance" runat="server" CssClass="input" Width="300px" AppendDataBoundItems="true"></asp:DropDownList></td>
                                         <td>
                                            <asp:CheckBox ID="chkVTB" runat="server" AutoPostBack="true" OnCheckedChanged="chkVTB_OnCheckedChanged" /></td>
                                    </tr>                                    
                                    <tr>                                        
                                        <td>Summary Expenses</td>
                                        <td>:</td>                                       
                                        <td>
                                            <asp:DropDownList ID="ddlVesselSummaryExpenses" runat="server" CssClass="input" Width="300px" AppendDataBoundItems="true"></asp:DropDownList></td>
                                        <td>
                                            <asp:CheckBox ID="chkSME" runat="server" AutoPostBack="true" OnCheckedChanged="chkSME_OnCheckedChanged" /></td>                                        
                                    </tr>                                    
                                    <tr>                                        
                                        <td>Monthly Variance Report</td>   
                                        <td>:</td>                                                                             
                                        <td>
                                            <asp:DropDownList ID="ddlMonthlyVariance" runat="server" CssClass="input" Width="300px" AppendDataBoundItems="true"></asp:DropDownList></td>
                                         <td>
                                            <asp:CheckBox ID="chkMVR" runat="server" AutoPostBack="true" OnCheckedChanged="chkMVR_OnCheckedChanged" /></td>                                   
                                    </tr>
                                    <tr>                                        
                                        <td>Yearly Variance Report</td>
                                        <td>:</td>                                       
                                         <td>
                                            <asp:DropDownList ID="ddlYearlyVariance" runat="server" CssClass="input" Width="300px" AppendDataBoundItems="true"></asp:DropDownList></td> 
                                        <td>
                                            <asp:CheckBox ID="chkYVR" runat="server" AutoPostBack="true" OnCheckedChanged="chkYVR_OnCheckedChanged" /></td>                                                                                                                      
                                    </tr>                                    
                                    <tr>                                        
                                        <td>Accumulated Variance Report</td>
                                        <td>:</td>                                       
                                        <td>
                                            <asp:DropDownList ID="ddlAccumulatedVariance" runat="server" CssClass="input" Width="300px" AppendDataBoundItems="true"></asp:DropDownList></td>
                                        <td>
                                            <asp:CheckBox ID="chkAVR" runat="server" AutoPostBack="true" OnCheckedChanged="chkAVR_OnCheckedChanged" /></td>                                        
                                    </tr>                                  
                                    <tr>                                        
                                        <td>Funds Position</td>
                                        <td>:</td>                                       
                                        <td>
                                            <asp:DropDownList ID="ddlFundsPosition" runat="server" CssClass="input" Width="300px" AppendDataBoundItems="true"></asp:DropDownList></td>
                                        <td>
                                            <asp:CheckBox ID="chkFPS" runat="server" AutoPostBack="true" OnCheckedChanged="chkFPS_OnCheckedChanged" /></td>                                        
                                    </tr>   
                                 <tr>                                        
                                        <td>YTD Details</td>
                                        <td>:</td>                                       
                                        <td>
                                            <asp:DropDownList ID="ddlYTDdetails" runat="server" CssClass="input" Width="300px" AppendDataBoundItems="true"></asp:DropDownList></td>
                                        <td>
                                            <asp:CheckBox ID="chkYTD" runat="server" AutoPostBack="true" OnCheckedChanged="chkYTD_OnCheckedChanged" /></td>                                        
                                    </tr>                                    
                                </table>                                      
                         </asp:Panel>              
                         <asp:Panel ID="pnl2" runat="server" Height="100px" BorderWidth="1px" ScrollBars="Auto"  Width="540px" GroupingText="Additional Attachment">                        
                                <%--<asp:Label ID="lblhdr2" runat="server" Font-Bold="true" ForeColor="Blue" Visible="false">Additional Attachment</asp:Label>--%>
                                <table width="100%" cellpadding="5">
                                    <asp:GridView ID="gvAdditionalAttachment" runat="server" AutoGenerateColumns="False"
                                        CellPadding="3" Font-Size="11px" OnRowCommand="dgvAdditionalAttachment_RowCommand"
                                        OnRowDataBound="gvAdditionalAttachment_ItemDataBound" OnRowDeleting="gvAdditionalAttachment_RowDeleting"
                                        OnSorting="gvAdditionalAttachment_Sorting" OnRowEditing="gvAdditionalAttachment_RowEditing"
                                        OnRowCancelingEdit="gvAdditionalAttachment_RowCancelingEdit" OnRowCreated="dgvAdditionalAttachment_RowCreated"
                                        AllowSorting="true" ShowFooter="True" Style="margin-bottom: 0px" Width="100%"
                                        EnableViewState="false" GridLines="None">
                                        <FooterStyle CssClass="datagrid_footerstyle" />
                                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                        <RowStyle Height="10px" />
                                        <Columns>
                                            <asp:ButtonField CommandName="Edit" Text="DoubleClick" Visible="false" />
                                            <asp:TemplateField>
                                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblReportTypeHeader" runat="server">Report Type</asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReportTypeID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTCODE") %>'></asp:Label>
                                                    <asp:Label ID="lblReportType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTTYPE") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:DropDownList ID="ddlOwnerReportType" runat="server" CssClass="input" AutoPostBack="true" OnSelectedIndexChanged="ddlOwnerReportTypeAdd_OnSelectedIndexChanged"></asp:DropDownList>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblSubReportTypeHeader" runat="server">Sub Report Type</asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSubReportTypeID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBREPORTCODE") %>'></asp:Label>
                                                    <asp:Label ID="lblSubReportType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBREPORTTYPE") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:DropDownList ID="ddlOwnerSubReportType" runat="server" CssClass="input"></asp:DropDownList>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Sort Order">
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblSortOrderHeader" runat="server">Order</asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSortOrder" runat="server" CommandArgument="<%# Container.DataItemIndex %>"
                                                        CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSORTORDER") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtEditSortOrder" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSORTORDER") %>'
                                                        CssClass="input_mandatory" Width="30px" />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtSortOrderAddl" runat="server" CssClass="input_mandatory" Width="30px"></asp:TextBox>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblActionHeader" runat="server"> Action </asp:Label>
                                                </HeaderTemplate>
                                                <FooterTemplate>
                                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                                    <asp:ImageButton ID="cmdAdd" runat="server" AlternateText="Save" CommandArgument="<%# Container.DataItemIndex %>"
                                                        CommandName="Add" ImageUrl="<%$ PhoenixTheme:images/te_check.png%>" ToolTip="Add New" />
                                                </FooterTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="100px" Wrap="False" />
                                                <ItemTemplate>
                                                    <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                                    <asp:ImageButton ID="cmdEdit" runat="server" AlternateText="Edit" CommandArgument="<%# Container.DataItemIndex %>"
                                                        CommandName="EDIT" ImageUrl="<%$ PhoenixTheme:images/te_edit.png%>" ToolTip="Edit" />
                                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                                    <asp:ImageButton ID="cmdDelete" runat="server" AlternateText="Delete" CommandArgument="<%# Container.DataItemIndex %>"
                                                        CommandName="DELETE" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>" ToolTip="Delete" />
                                                    <img id="Img5" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <img id="Img4" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                                    <asp:ImageButton ID="cmdSave" runat="server" AlternateText="Save" CommandArgument="<%# Container.DataItemIndex %>"
                                                        CommandName="Save" ImageUrl="<%$ PhoenixTheme:images/save.png%>" ToolTip="Save" />
                                                    <img id="Img5" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                                    <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                                        CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                                        ToolTip="Cancel"></asp:ImageButton>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </table>
                               </asp:Panel>                                            
                    </div>                  
                </div>                
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
