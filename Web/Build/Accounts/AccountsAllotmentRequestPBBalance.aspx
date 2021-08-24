<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAllotmentRequestPBBalance.aspx.cs"
    Inherits="AccountsAllotmentRequestPBBalance" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PB Balance</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlPBBalances">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="PB Balance" ShowMenu="false" />
                    </div>
                    <br />
                    <table cellpadding="1" cellspacing="1" width="100%" style="border-style: none; color: blue;"
                        runat="server" id="tblNote">
                        <tr>
                            <td>
                                <asp:Literal ID="lblNote" runat="server" Text="Note"></asp:Literal>
                                <br /> 
                                <asp:Literal ID="lblPleaseenterRemarkswhyweareacceptingnegativefinalbalance" runat="server"
                                    Text="* Please enter Remarks why we are accepting negative final balance."></asp:Literal>
                                <br />
                                <asp:Literal ID="lblForBOWwithnegativebalancepleasecreateRecoveryentry" runat="server"
                                    Text="* For BOW with negative balance, please create Recovery entry"></asp:Literal>
                            </td>
                        </tr>
                    </table>                    
                    <div id="divGrid" style="position: relative; z-index: +1">
                        <asp:GridView ID="gvPBBalance" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" OnRowDataBound="gvPBBalance_RowDataBound" ShowFooter="false"
                            ShowHeader="true" EnableViewState="true" AllowSorting="true" OnRowCommand="gvPBBalance_RowCommand"
                            OnRowEditing="gvPBBalance_RowEditing" OnRowCancelingEdit="gvPBBalance_RowCancellingEdit">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                            <RowStyle Height="10px" />
                            <RowStyle Height="10px" />
                            <Columns>
                                <asp:TemplateField HeaderText="Final Balance Vessel">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblFinalBalVessel" runat="server" Text="Final Balance in Vessel - USD"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblRemarksId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKSID") %>'></asp:Label>
                                        <asp:Label ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></asp:Label>
                                        <asp:Label ID="lblAllotmentType" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALLOTMENTTYPE") %>'></asp:Label>
                                        <asp:Label ID="lblVesselFinalBal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELAMOUNT") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Final Balance in Office">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblFinalBalOff" runat="server" Text="Final Balance Office - USD"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblOfficeFinalBal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFICEAMOUNT") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remarks">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblRemarks" runat="server" Text="Remarks"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox runat="server" ID="txtBankVerificationRemarks" CssClass="input_mandatory"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>' TextMode="MultiLine"
                                            Width="250px"></asp:TextBox>
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
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
