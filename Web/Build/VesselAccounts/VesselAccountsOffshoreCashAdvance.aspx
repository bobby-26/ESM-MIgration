<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsOffshoreCashAdvance.aspx.cs"
    Inherits="VesselAccountsOffshoreCashAdvance" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.VesselAccounts" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselCrew" Src="~/UserControls/UserControlVesselEmployee.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BankAccount" Src="~/UserControls/UserControlEmployeeBankAccount.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmMessage" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Vessel Sign-On</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
    </telerik:RadCodeBlock>
    <style type="text/css">
        .hidden {
            display: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlNTBRManager">
            <ContentTemplate>
                <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <div class="subHeader" style="position: relative">
                        <eluc:Title runat="server" ID="Title1" Text="Cash Advance" ShowMenu="true"></eluc:Title>
                        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                            <eluc:TabStrip ID="MenuEarDedGeneral" runat="server" TabStrip="true" OnTabStripCommand="MenuEarDedGeneral_TabStripCommand"></eluc:TabStrip>
                        </div>
                        <%--<div class="subHeader" style="position: relative;">
                        <span class="navSelect" style="margin-top: 0px; float: right; width: auto;">
                            <eluc:TabStrip ID="MenuEarDed" runat="server" OnTabStripCommand="MenuEarDed_TabStripCommand"
                                TabStrip="false"></eluc:TabStrip>
                        </span>
                    </div>--%>
                        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click"
                            CssClass="hidden" />
                    </div>
                    <table width="70%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                                <asp:Literal ID="lblType" runat="server" Text="Type"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlType" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                                    OnTextChanged="ddlEmployee_TextChangedEvent">
                                    <asp:ListItem Value="3" Text="Cash Advance"></asp:ListItem>
                                    <%--<asp:ListItem Value="1" Text="Onboard Earnings/Deduction"></asp:ListItem>
                                <asp:ListItem Value="4" Text="Allotment"></asp:ListItem>
                                <asp:ListItem Value="5" Text="Radio Log"></asp:ListItem>
                                <asp:ListItem Value="7" Text="Special Allotment"></asp:ListItem>--%>
                                </asp:DropDownList>
                                <asp:ImageButton ID="imgClip" runat="server" OnClick="imgClip_onClick" ToolTip="Upload Attachment" />
                            </td>
                            <td>
                                <asp:Literal ID="lblEmployeeName" runat="server" Text="Employee Name"></asp:Literal>
                            </td>
                            <td>
                                <%-- <eluc:VesselCrew ID="ddlEmployee" runat="server" CssClass="input" AppendDataBoundItems="true"
                                AutoPostBack="true" OnTextChangedEvent="ddlEmployee_TextChangedEvent" />--%>
                                <asp:DropDownList ID="ddlEmployee" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                                    OnTextChanged="ddlEmployee_TextChangedEvent">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Literal ID="lblBalanceOfwage" runat="server" Text="Balance Of wage"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBal" runat="server" Width="80px" CssClass="input" Enabled="false"
                                    Style="text-align: right;" Text="0.00"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblMonth" runat="server" Text="Month"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlMonth" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                                    OnTextChanged="ddlEmployee_TextChangedEvent">
                                    <asp:ListItem Value="" Text="--Select--"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Jan"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Feb"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="Mar"></asp:ListItem>
                                    <asp:ListItem Value="4" Text="Apr"></asp:ListItem>
                                    <asp:ListItem Value="5" Text="May"></asp:ListItem>
                                    <asp:ListItem Value="6" Text="Jun"></asp:ListItem>
                                    <asp:ListItem Value="7" Text="Jul"></asp:ListItem>
                                    <asp:ListItem Value="8" Text="Aug"></asp:ListItem>
                                    <asp:ListItem Value="9" Text="Sep"></asp:ListItem>
                                    <asp:ListItem Value="10" Text="Oct"></asp:ListItem>
                                    <asp:ListItem Value="11" Text="Nov"></asp:ListItem>
                                    <asp:ListItem Value="12" Text="Dec"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Literal ID="lblYear" runat="server" Text="Year"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlYear" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                                    OnTextChanged="ddlEmployee_TextChangedEvent">
                                </asp:DropDownList>
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr runat="server" id="trErD">
                            <td>
                                <asp:Literal ID="lblEarningDeduction" runat="server" Text="Earning/Deduction"></asp:Literal>
                            </td>
                            <td colspan="3">
                                <asp:RadioButtonList ID="rblEarningDeduction" runat="server" RepeatDirection="Horizontal"
                                    AutoPostBack="true" OnTextChanged="ddlEmployee_TextChangedEvent">
                                    <asp:ListItem Selected="True" Value="1" Text="Earning"></asp:ListItem>
                                    <asp:ListItem Value="-1" Text="Deduction"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div class="navSelect" style="position: relative; clear: both; width: 15px">
                        <eluc:TabStrip ID="MenuBondIssue" runat="server" OnTabStripCommand="MenuBondIssue_TabStripCommand"></eluc:TabStrip>
                    </div>
                    <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                        <asp:GridView ID="gvCrewSearch" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            GridLines="None" Width="100%" CellPadding="3" OnRowDataBound="gvCrewSearch_RowDataBound"
                            OnRowEditing="gvCrewSearch_RowEditing" OnRowCancelingEdit="gvCrewSearch_RowCancelingEdit"
                            OnRowUpdating="gvCrewSearch_RowUpdating" OnRowDeleting="gvCrewSearch_RowDeleting"
                            ShowHeader="true" EnableViewState="false" OnRowCommand="gvCrewSearch_RowCommand"
                            DataKeyNames="FLDEARNINGDEDUCTIONID">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <Columns>
                                <asp:TemplateField HeaderText="Employee Code">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="lblDTKey" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDDTKEY"] %>'></asp:Label>
                                        <asp:Label ID="lblFileNo" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDFILENO"] %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Employee Name">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <%#((DataRowView)Container.DataItem)["FLDEMPLOYEENAME"] %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Rank">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <%#((DataRowView)Container.DataItem)["FLDRANKCODE"] %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Entry Type">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <%#((DataRowView)Container.DataItem)["FLDHARDNAME"] %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Purpose">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="lblPurposeText" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURPOSE").ToString().Length>30 ? DataBinder.Eval(Container, "DataItem.FLDPURPOSE").ToString().Substring(0, 30)+ "..." : DataBinder.Eval(Container, "DataItem.FLDPURPOSE").ToString() %>'></asp:Label>
                                        <eluc:ToolTip ID="ucToolTip" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURPOSE")%>'
                                            Width="450px" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblWageHeadId" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDWAGEHEADID"] %>'></asp:Label>
                                        <asp:Label ID="lblEmployeeId" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDEMPLOYEEID"] %>'></asp:Label>
                                        <asp:Label ID="lblSignOnOffId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDSIGNONOFFID"]%>'></asp:Label>
                                        <asp:Label ID="lblPurpose" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDPURPOSE"]%>'></asp:Label>
                                        <asp:Label ID="lblMALAlredyConfirmedYN" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDMALALREDYCONFIRMEDYN"]%>'></asp:Label>
                                        <asp:TextBox ID="txtPurposeEdit" runat="server" CssClass="gridinput_mandatory" Text='<%#((DataRowView)Container.DataItem)["FLDPURPOSE"]%>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Currency">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:Literal ID="lblUSD" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDCURRENCYNAME"] %>'></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <ItemTemplate>
                                        <%#((DataRowView)Container.DataItem)["FLDAMOUNT"]%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblBalance" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDBALACEWAGE"] %>'></asp:Label>
                                        <eluc:Number ID="lblAmount" runat="server" Visible="false" CssClass="input_mandatory"
                                            MaxLength="8" IsPositive='<%#((DataRowView)Container.DataItem)["FLDSHORTNAME"].ToString() == "BRF" ? false : true %>'
                                            Width="90px" Text='<%#((DataRowView)Container.DataItem)["FLDAMOUNT"]%>' />
                                        <eluc:Number ID="txtAmountEdit" runat="server" CssClass="input_mandatory" MaxLength="8"
                                            IsPositive='<%#((DataRowView)Container.DataItem)["FLDSHORTNAME"].ToString() == "BRF" ? false : true %>'
                                            Width="90px" Text='<%#((DataRowView)Container.DataItem)["FLDAMOUNT"]%>' />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Bank Account/Beneficiary Name">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <%#((DataRowView)Container.DataItem)["FLDACCOUNTNUMBER"] %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:BankAccount ID="ddlBankAccount" runat="server" CssClass="dropdown_mandatory"
                                            AppendDataBoundItems="true" EmployeeId='<%#((DataRowView)Container.DataItem)["FLDEMPLOYEEID"] %>'
                                            BankAccountList='<%#PhoenixVesselAccountsEmployeeBankAccount.ListEmployeeBankAccount(PhoenixSecurityContext.CurrentSecurityContext.VesselID, General.GetNullableInteger(((DataRowView)Container.DataItem)["FLDEMPLOYEEID"].ToString()))%>' />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <%#string.Format("{0:dd/MM/yyyy}", ((DataRowView)Container.DataItem)["FLDMONTHYEAR"]) %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <%#string.Format("{0:dd/MM/yyyy}", ((DataRowView)Container.DataItem)["FLDDATE"]) %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Date ID="txtDate" runat="server" CssClass="input_mandatory" Text='<%# ((DataRowView)Container.DataItem)["FLDDATE"] %>' />
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
                                        <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                        <asp:ImageButton runat="server" Visible="false" AlternateText="Confirm" ImageUrl="<%$ PhoenixTheme:images/approve.png %>"
                                            CommandName="CONFIRM" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdConfirm"
                                            ToolTip="Confirm"></asp:ImageButton>
                                        <img id="Img4" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Approve" ImageUrl="<%$ PhoenixTheme:images/approved.png %>"
                                            CommandName="APPROVE" Visible="false" CommandArgument="<%# Container.DataItemIndex %>"
                                            ID="imgApprove" ToolTip="Approve"></asp:ImageButton>
                                        <img id="Img6" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Approve" ImageUrl="<%$ PhoenixTheme:images/te_view.png %>"
                                            CommandName="PURPOSE" CommandArgument="<%# Container.DataItemIndex %>" ID="imgPurpose"
                                            ToolTip="Update Purpose"></asp:ImageButton>
                                        <img id="Img5" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                            CommandName="ATTACHMENT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAtt"
                                            ToolTip="Upload Attachment"></asp:ImageButton>
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
                                    <FooterTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                            CommandName="Add" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdAdd"
                                            ToolTip="Add New"></asp:ImageButton>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <table width="100%" border="0" cellpadding="1" cellspacing="1" class="datagrid_pagestyle">
                            <tr>
                                <td nowrap="nowrap" align="center">
                                    <asp:Label ID="lblPagenumber" runat="server">
                                    </asp:Label>
                                    <asp:Label ID="lblPages" runat="server">
                                    </asp:Label>
                                    <asp:Label ID="lblRecords" runat="server">
                                    </asp:Label>&nbsp;&nbsp;
                                </td>
                                <td nowrap="nowrap" align="left" width="50px">
                                    <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev"><< Prev </asp:LinkButton>
                                </td>
                                <td width="20px">&nbsp;
                                </td>
                                <td nowrap="nowrap" align="right" width="50px">
                                    <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                                </td>
                                <td nowrap="nowrap" align="center">
                                    <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                    </asp:TextBox>
                                    <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                        Width="40px"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <eluc:ConfirmMessage ID="ucConfirm" runat="server" Text="" OnConfirmMesage="ucConfirm_OnClick"
                    Visible="false" />
                <eluc:ConfirmMessage ID="ucAllotmentConfirm" runat="server" Text="" OnConfirmMesage="ucConfirmAllotment_OnClick"
                    OKText="Yes" CancelText="No" Visible="false" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
