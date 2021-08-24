<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAllotmentRequestBankVerification.aspx.cs"
    Inherits="AccountsAllotmentRequestBankVerification" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Bank Verification</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="CrewBankAccountlink" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmAllotmentPB" runat="server">
    <ajaxtoolkit:toolkitscriptmanager id="ToolkitScriptManager1" runat="server" combinescripts="false">
    </ajaxtoolkit:toolkitscriptmanager>
    <asp:UpdatePanel runat="server" ID="pnlBankVerification">
        <ContentTemplate>
            <eluc:error id="ucError" runat="server" text="" visible="false"></eluc:error>
            <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%;
                position: absolute;">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:title runat="server" id="ucTitle" text="Crew Bank Account" showmenu="false" />
                    </div>
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                        <eluc:tabstrip id="MenuAllotment" runat="server" ontabstripcommand="MenuAllotment_TabStripCommand">
                        </eluc:tabstrip>
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
                                <asp:Literal ID="lblPleaseEnterRemarkswhythebankaccountcannotbeverified" runat="server"
                                    Text="* Please Enter Remarks why the bank account cannot be verified"></asp:Literal>
                                <br />
                            </td>
                        </tr>
                    </table>
                    <div id="divGrid" style="position: relative; z-index: +1">
                        <asp:GridView ID="gvCrewBankAccount" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" OnRowDataBound="gvCrewBankAccount_RowDataBound"
                            OnRowDeleting="gvCrewBankAccount_RowDeleting" ShowFooter="false" ShowHeader="true"
                            EnableViewState="false" AllowSorting="true" OnRowCommand="gvCrewBankAccount_RowCommand"
                            OnSorting="gvCrewBankAccount_Sorting" OnRowEditing="gvCrewBankAccount_RowEditing"
                            OnRowCancelingEdit="gvCrewBankAccount_RowCancellingEdit">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                            <RowStyle Height="10px" />
                            <RowStyle Height="10px" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkBankId" runat="server" Checked="false" AutoPostBack="true" OnCheckedChanged="chkBankId_OnCheckedChanged" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Account Type">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblAccountTypeHeader" runat="server">Account Type&nbsp;</asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblCrewBankAccountId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBANKACCOUNTID") %>'></asp:Label>
                                        <asp:Label ID="lblAccountType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTTYPENAME") %>'></asp:Label>
                                        <asp:Label ID="lblReferenceId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCEID") %>'></asp:Label>
                                        <asp:Label ID="lblDTKey" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></asp:Label>
                                        <asp:Label ID="lblRemarksId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKSID") %>'></asp:Label>
                                        <asp:Label ID="lblAllotmentType" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALLOTMENTTYPE") %>'></asp:Label>
                                        <%--<asp:LinkButton ID="lnkAccountType" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTTYPENAME") %>'></asp:LinkButton>--%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Account Name">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblBeneficiaryName" runat="server" Text="Beneficiary Name"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblAccountName" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>'
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTNAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Beneficiary Bank">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblSeafarerBankHeader" runat="server">Beneficiary Bank
                                        </asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lbSeafarerBank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBANKNAME" ) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Beneficiary AccountNo.">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblBeneficiaryAccountNo" runat="server" Text="Beneficiary AccountNo."></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblAccountNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTNUMBER" ) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Last Remitted Date">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblLastRemittedDate" runat="server" Text="Last Remitted Date"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblRemittedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTREMITTEDDATE" ) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Last Verified Date">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblLastVerifiedDate" runat="server" Text="Last Verified Date"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblVerifiedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTVERIFIEDDATE" ) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="InActiveYN">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblInActiveYN" runat="server" Text="Inactive YN"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblActiveYN" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVEYN") %>'></asp:Label>
                                        <asp:Label ID="lblInActiveYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINACTIVEYN") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remarks">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblRemarks" runat="server" Text="Remarks"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox runat="server" ID="txtBankVerificationRemarks" CssClass="input_mandatory"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>' TextMode="MultiLine"></asp:TextBox>
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
                    <div id="divPage" style="position: relative;" visible="false" runat="server">
                        <table width="100%" border="0" class="datagrid_pagestyle">
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
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
