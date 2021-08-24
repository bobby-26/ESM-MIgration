<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAllotmentRequesPendingReimbRecoveries.aspx.cs"
    Inherits="AccountsAllotmentRequesPendingReimbRecoveries" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Office ReimRec</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="CrewBankAccountlink" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmAllotmentRemRec" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlRemRec">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%;
                position: absolute;">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Reimbursement/Recoveries" ShowMenu="false" />
                    </div>
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                        <eluc:TabStrip ID="MenuAllotment" runat="server" OnTabStripCommand="MenuAllotment_TabStripCommand" TabStrip="true" >
                        </eluc:TabStrip>
                    </div>
                    <table cellpadding="1" cellspacing="1" width="100%" style="border-style: none; color: blue;"
                        runat="server" id="tblNote">
                        <tr>
                            <td>
                                <br />
                                <asp:Literal ID="lblNote" runat="server" Text="Note"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblPleaseenterthePaymentModeforAllPendingRecoveries" runat="server"
                                    Text="* Please enter the Payment Mode for All Pending Recoveries"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblPleaseenterRemarksforReasonforPostponingRemittanceofReimbursementRecoveries"
                                    runat="server" Text="* Please enter Remarks for Reason for Postponing Remittance of Reimbursement/ Recoveries"></asp:Literal>
                            </td>
                        </tr>
                    </table>
                    <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                        <asp:GridView ID="gvRem" runat="server" AutoGenerateColumns="False" Width="100%"
                            CellPadding="3" OnRowDataBound="gvRem_RowDataBound" ShowFooter="false" ShowHeader="true"
                            EnableViewState="false" AllowSorting="true" OnRowCommand="gvRem_RowCommand" OnRowEditing="gvRem_RowEditing"
                            OnRowCancelingEdit="gvRem_RowCancellingEdit" OnSelectedIndexChanging="gvRem_SelectedIndexChanging" DataKeyNames="FLDDTKEY">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                            <RowStyle Height="10px" />
                            <Columns>
                                <asp:TemplateField HeaderText="Allotment Req.No.">
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblAllotmentReq" runat="server" Text="Allotment</br>Req.No."></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDREQUESTNUMBER"]%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Reference No.">
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblReferenceNo" runat="server" Text="Reference No."></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDREFERENCENO"]%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="File No">
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblClaimDate" runat="server" Text="Claim Date"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDCLAIMDATE"]%>
                                        <asp:Label ID="lblReferenceId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCEID") %>'></asp:Label>
                                        <asp:Label ID="lblDTKey" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></asp:Label>
                                        <asp:Label ID="lblRemarksId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKSID") %>'></asp:Label>
                                        <asp:Label ID="lblAllotmentType" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALLOTMENTTYPE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Reimbursement/Recovery">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblReimbursementRecovery" runat="server" Text="Reimbursement/Recovery"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# GetName(((DataRowView)Container.DataItem)["FLDEARNINGDEDUCTION"].ToString())%>
                                        <asp:Label ID="lblEarningDeduction" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEARNINGDEDUCTION") %>'>
                                        </asp:Label>    
                                        <asp:Label ID="lblReimbursementId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREIMBURSEMENTID") %>' >
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Purpose">
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblPurpose" runat="server" Text="Purpose"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDPURPOSENAME"]%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description">
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblDescription" runat="server" Text="Description"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDDESCRIPTION"]%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Currency">
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblCurrency" runat="server" Text="Currency"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDCURRENCYCODE"]%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="UnpaidAmount">
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblUnpaidAmount" runat="server" Text="Unpaid Amount"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDUNPAIDAMOUNT"]%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Payment Mode">
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblPaymentMode" runat="server" Text="Payment Mode"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDPAYMENTMODENAME"]%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Payment Status">
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblPaymentStatus" runat="server" Text="Payment Status"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDPAYMENTSTATUSNAME"]%>
                                    </ItemTemplate>
                                </asp:TemplateField>                                
                                <asp:TemplateField HeaderText="Remarks">
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblRemarks" runat="server" Text="Remarks"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDREMARKS"]%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="input_mandatory" TextMode="MultiLine"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Updated By">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblUpdatedbyheader" runat="server" Text="Updated By"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblUpdatedby" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMODIFIEDBY") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Updated Date">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblUpdatedDateheader" runat="server" Text="Updated Date"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblUpdatedDate" runat="server" 
                                        Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDMODIFIEDDATE")) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblAction" runat="server" Text="Action"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl='<%$ PhoenixTheme:images/te_edit.png%>'
                                            CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                            ToolTip="Edit"></asp:ImageButton>
                                        <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                        <asp:ImageButton runat="server" Visible="false" AlternateText="Payment" ImageUrl='<%$ PhoenixTheme:images/text-detail.png%>'
                                            CommandName="PAYMENT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdPayment"
                                            ToolTip="Payment/Recovery Schedule"></asp:ImageButton>
                                        <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl='<%$ PhoenixTheme:images/save.png%>'
                                            CommandName="Save" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                            ToolTip="Save"></asp:ImageButton>
                                        <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl='<%$ PhoenixTheme:images/te_del.png%>'
                                            CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                            ToolTip="Cancel"></asp:ImageButton>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
