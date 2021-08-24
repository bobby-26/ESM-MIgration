<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsInterCompanyTransferEntriesCreateVoucher.aspx.cs" Inherits="AccountsInterCompanyTransferEntriesCreateVoucher" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Inter Company Transfer Entries Create Voucher</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="ds" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="formInterCompanyCreateVoucher" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts ="false" >
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlCreateVoucher">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false">
                </eluc:Error>
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="ttlVoucher" Text="Create Voucher" ShowMenu="true">
                    </eluc:Title>
                    <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                    <eluc:TabStrip ID="MenuCreateVoucherGeneral" runat="server" OnTabStripCommand="MenuCreateVoucherGeneral_TabStripCommand"
                        TabStrip="true">
                    </eluc:TabStrip>
                </div>
                <iframe runat="server" id="ifMoreInfo" style="min-height: 330px; width: 100%; border: 0"
                    scrolling="yes"></iframe>                
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuCreateVoucher" runat="server" OnTabStripCommand="MenuCreateVoucher_TabStripCommand">
                    </eluc:TabStrip>
                </div>               
                <div id="divGrid" style="position: relative; z-index: +1">
                    <asp:GridView ID="gvCreateVoucher" runat="server" AllowSorting="true" AutoGenerateColumns="False"
                        CellPadding="3" EnableViewState="false" Font-Size="11px" Width="100%" OnSelectedIndexChanging="gvCreateVoucher_SelectedIndexChanging"
                        OnRowCancelingEdit="gvCreateVoucher_RowCancelingEdit" OnRowDataBound="gvCreateVoucher_RowDataBound"
                        OnRowEditing="gvCreateVoucher_RowEditing" OnRowUpdating="gvCreateVoucher_RowUpdating">
                        <FooterStyle CssClass="datagrid_footerstyle" />
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField HeaderText="Row Number">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>
                                    <asp:Label ID="lblVoucherLineNo" runat="server"
                                         Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERLINEITEMNO") %>'></asp:Label>
                                    <asp:Label ID="lblVoucherLineId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERLINEITEMID") %>'
                                        Visible="false"></asp:Label>
                                   <%-- <asp:Label ID="lblVouchertypeId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERTYPE") %>'
                                        Visible="false"></asp:Label>--%>
                                         <asp:Label ID="lblRefernceNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCEDOCUMENTNO") %>'
                                        Visible="false"></asp:Label>
                                        <asp:Label ID="lblvoucherid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERID") %>'
                                        Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Account">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>
                                    <asp:Label ID="lblAccount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Account Description">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>
                                    <asp:Label ID="lblAccountDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTDESCRIPTION") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sub Account">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>
                                    <asp:Label ID="lblSubAccount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETCODE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>                           
                            <asp:TemplateField HeaderText="Currency">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>
                                    <asp:Label ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYNAME") %>'></asp:Label>
                                    <asp:Label ID="lblCurrencyCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Prime Amount">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>
                                    <asp:Label ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number runat="server" ID="txtAmountEdit" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT","{0:n2}") %>' />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Base Rate">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>
                                    <asp:Label ID="lblBaseRate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBASEEXCHANGERATE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Report Rate">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>
                                    <asp:Label ID="lblReportRate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTEXCHANGERATE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Long Description">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>
                                 <asp:Label ID="lblLongDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLONGDESCRIPTION").ToString().Length>15 ? (DataBinder.Eval(Container,"DataItem.FLDLONGDESCRIPTION") as string).Substring(0,15):DataBinder.Eval(Container,"DataItem.FLDLONGDESCRIPTION")%> '>  </asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtLongDescriptionEdit" runat="server" CssClass="input_mandatory" TextMode="MultiLine"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDLONGDESCRIPTION") %>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="UPDATE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>   
                <div id="divPage" style="position: relative;" visible="false">
                    <table width="100%" border="0" cellpadding="1" cellspacing="1" class="datagrid_pagestyle" visible="false">
                        <tr>
                            <td nowrap align="center">
                                <asp:Label ID="lblPagenumber" runat="server" visible="false">
                                </asp:Label>
                                <asp:Label ID="lblPages" runat="server" visible="false">
                                </asp:Label>
                                <asp:Label ID="lblRecords" runat="server" visible="false">
                                </asp:Label>&nbsp;&nbsp;
                            </td>
                            <td nowrap align="left" width="50px">
                                <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev" visible="false">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap align="right" width="50px">
                                <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next" visible="false">Next >></asp:LinkButton>
                            </td>
                            <td nowrap align="center">
                                <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input" visible="false">
                                </asp:TextBox>
                                <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                    Width="40px" visible="false"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>             
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
