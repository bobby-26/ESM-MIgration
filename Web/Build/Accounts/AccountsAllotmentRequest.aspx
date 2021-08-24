<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAllotmentRequest.aspx.cs"
    Inherits="AccountsAllotmentRequest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
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

<script type="text/javascript">
     function CheckAll(chkAll)
     {
        var gv = document.getElementById("<%=gvAllotment.ClientID %>");
        for(i = 1;i < gv.rows.length; i++)
        {
            gv.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = chkAll.checked;
        }
     }
</script>

<body>
    <form id="frmCostEvaluationRequest" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader" style="position: relative">
            <eluc:Title runat="server" ID="frmTitle" Text="Allotment Request"></eluc:Title>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                <eluc:TabStrip ID="MenuRequest" runat="server" OnTabStripCommand="MenuRequest_TabStripCommand"
                    TabStrip="true"></eluc:TabStrip>
            </div>
        </div>
        <div class="subHeader">
            <div class="divFloat" style="clear: right">
                <eluc:TabStrip ID="MenuPV" runat="server" OnTabStripCommand="MenuPV_TabStripCommand">
                </eluc:TabStrip>
            </div>
        </div>
        <table cellpadding="2" cellspacing="2" width="100%">
            <tr>
                <td rowspan="5">
                    <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                </td>
                <td rowspan="5">
                    <div runat="server" id="divCheckboxList" class="input" style="overflow: auto; height: 140px">
                        <asp:CheckBoxList ID="cblVesselList" runat="server" DataTextField="FLDVESSELNAME"
                            DataValueField="FLDVESSELID" RepeatDirection="Vertical" RepeatColumns="1">
                        </asp:CheckBoxList>
                    </div>
                </td>
                <td>
                    <asp:Literal ID="lblName" runat="server" Text="Name"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtName" runat="server" CssClass="input" Width="150px"></asp:TextBox>
                </td>
                <td>
                    <asp:Literal ID="lblRank" runat="server" Text="Rank"></asp:Literal>
                </td>
                <td>
                    <eluc:Rank ID="ucRank" runat="server" CssClass="input" AppendDataBoundItems="true"
                        Width="300px" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblFileNo" runat="server" Text="File No."></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtFileNo" runat="server" CssClass="input" Width="150px"></asp:TextBox>
                </td>
                <td>
                    <asp:Literal ID="lblAllotmentType" runat="server" Text="Allotment Type"></asp:Literal>
                </td>
                <td>
                    <asp:DropDownList ID="ddlAllotmentType" runat="server" CssClass="input" DataTextField="FLDALLOTMENTNAME"
                        DataValueField="FLDID" Width="300px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblMonth" runat="server" Text="Month"></asp:Literal>
                </td>
                <td>
                    <asp:DropDownList ID="ddlMonth" runat="server" CssClass="input" Width="120px">
                        <asp:ListItem Text="--Select--" Value="DUMMY"></asp:ListItem>
                        <asp:ListItem Text="January" Value="1"></asp:ListItem>
                        <asp:ListItem Text="February" Value="2"></asp:ListItem>
                        <asp:ListItem Text="March" Value="3"></asp:ListItem>
                        <asp:ListItem Text="April" Value="4"></asp:ListItem>
                        <asp:ListItem Text="May" Value="5"></asp:ListItem>
                        <asp:ListItem Text="June" Value="6"></asp:ListItem>
                        <asp:ListItem Text="July" Value="7"></asp:ListItem>
                        <asp:ListItem Text="August" Value="8"></asp:ListItem>
                        <asp:ListItem Text="September" Value="9"></asp:ListItem>
                        <asp:ListItem Text="October" Value="10"></asp:ListItem>
                        <asp:ListItem Text="November" Value="11"></asp:ListItem>
                        <asp:ListItem Text="December" Value="12"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Literal ID="lblStatus" runat="server" Text="Status"></asp:Literal>
                </td>
                <td>
                    <eluc:Hard ID="ucRequestStatus" runat="server" CssClass="input" AppendDataBoundItems="true"
                        HardTypeCode="238" Width="300px" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblYear" runat="server" Text="Year"></asp:Literal>
                </td>
                <td>
                    <asp:DropDownList ID="ddlYear" runat="server" CssClass="input" Width="120px">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Literal ID="lblRequestDateBW" runat="server" Text="Request Date B/W"></asp:Literal>
                </td>
                <td>
                    <eluc:Date ID="txtDateFrom" runat="server" CssClass="input" DatePicker="true" />
                    <eluc:Date ID="txtDateTo" runat="server" CssClass="input" DatePicker="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblOfficeType" runat="server" Text="Office Type"></asp:Literal>
                </td>
                <td>
                    <asp:CheckBoxList ID="cblOfficeTypeList" runat="server" RepeatDirection="Vertical"
                        class="input">
                        <asp:ListItem Value="0" Text="Reimbursement/Recoveries">
                        </asp:ListItem>
                        <asp:ListItem Value="-1" Text="Previous Arrears">
                        </asp:ListItem>
                    </asp:CheckBoxList>
                </td>
                <td colspan="2">
                    <asp:Literal ID="lblIsNotPaymentVoucherYN" runat="server" Text="Payment Voucher Not Yet Generated"></asp:Literal>
                    <asp:CheckBox ID="chkIsNotPaymentVoucherYN" runat="server" />
                </td>
            </tr>
        </table>
        <div class="navSelect" style="position: relative; width: 15px">
            <eluc:TabStrip ID="MenuAllotment" runat="server" OnTabStripCommand="MenuAllotment_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <div id="divGrid" style="position: relative; z-index: 1; width: 100%;">
            <asp:GridView ID="gvAllotment" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnRowCommand="gvAllotment_RowCommand" OnRowDataBound="gvAllotment_ItemDataBound"
                AllowSorting="true" ShowHeader="true" EnableViewState="true" OnSelectedIndexChanging="gvAllotment_SelectedIndexChanging"
                DataKeyNames="FLDALLOTMENTID">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" BorderColor="#FF0066" />
                <RowStyle Height="10px" />
                <Columns>
                    <asp:TemplateField>
                        <ItemStyle HorizontalAlign="Center" Wrap="False" Width="60px" />
                        <HeaderStyle HorizontalAlign="Left" Wrap="true" />
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkAll" runat="server" Text="All&nbsp;&nbsp;" TextAlign="Left"
                                onclick="CheckAll(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkItem" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Request Date">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:Literal ID="lblRequestDate" runat="server" Text="Request Date"></asp:Literal>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblAllotmentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALLOTMENTID") %>'></asp:Label>
                            <asp:Label ID="lblRequestDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTDATE") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Request No.">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:Literal ID="lblRequestNumber" runat="server" Text="Request Number"></asp:Literal>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkRequestNo" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDREQUESTNUMBER") %>'
                                CommandName="SELECT" CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
                            <asp:Label ID="lblVesselID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></asp:Label>
                            <asp:Label ID="lblMonth" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMONTH") %>'></asp:Label>
                            <asp:Label ID="lblYear" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDYEAR") %>'></asp:Label>
                            <asp:Label ID="lblUnlockYN" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNLOCKYN") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Rank">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:Literal ID="lblRank" runat="server" Text="Rank"></asp:Literal>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblVesselName" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                            <asp:Label ID="lblRankName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="File No.">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:Literal ID="lblFileNo" runat="server" Text="File No."></asp:Literal>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblEmployeeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></asp:Label>
                            <asp:Label ID="lblEmployeeFileNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Employee Name">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:Literal ID="lblEmployee" runat="server" Text="Employee"></asp:Literal>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblEmployeeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Allotment Type">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:Literal ID="lblAllotmentType" runat="server" Text="Allotment Type"></asp:Literal>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblAllotmentTypeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALLOTMENTTYPE") %>'></asp:Label>
                            <asp:Label ID="lblAllotmentType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALLOTMENTTYPENAME") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Amount(USD)">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <HeaderTemplate>
                            <asp:Literal ID="lblAmountUSD" runat="server" Text="Amount(USD)"></asp:Literal>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblCurrencyId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYID") %>'></asp:Label>
                            <asp:Label ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:Literal ID="lblStatus" runat="server" Text="Status"></asp:Literal>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblRequestStatus" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTSTATUS") %>'></asp:Label>
                            <asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTSTATUSNAME") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Payment Voucher Number">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:Literal ID="lblPVNo" runat="server" Text="Payment Voucher Number"></asp:Literal>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblPVNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTVOUCHERNUMBER") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Payment Date">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:Literal ID="lblPaymentDate" runat="server" Text="Payment Date"></asp:Literal>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblPaymentDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTDATE") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Payment Reference">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:Literal ID="lblPaymentReference" runat="server" Text="Payment Reference"></asp:Literal>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblPaymentRef" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTREFERENCE") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <HeaderTemplate>
                            <asp:Label ID="lblActionHeader" runat="server">
                                        Action
                            </asp:Label>
                        </HeaderTemplate>
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" AlternateText="Unlock" ImageUrl="<%$ PhoenixTheme:images/period-unlock.png %>"
                                CommandName="UNLOCK" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdUnlock"
                                Visible="false" ToolTip="Unlock Request"></asp:ImageButton>
                            <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                            <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                Visible="false" CommandName="CANCELREQUEST" CommandArgument='<%# Container.DataItemIndex %>'
                                ID="cmdCancelRequest" ToolTip="Cancel Request"></asp:ImageButton>
                            <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                            <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/annexure.png %>"
                                CommandName="REIMBREC" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdReimRec"
                                ToolTip="Reim/Rec Request"></asp:ImageButton>
                            <img id="Img3" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                            <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/covering-letter.png %>"
                                CommandName="SIDELETTER" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSideLetter"
                                ToolTip="SideLetter Request"></asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div id="divPage" style="position: relative; z-index: 0">
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
                        <asp:TextBox ID="txtnopage" MaxLength="5" Width="20px" runat="server" CssClass="input">
                        </asp:TextBox>
                        <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                            Width="40px"></asp:Button>
                    </td>
                </tr>
            </table>
        </div>
        <eluc:Status ID="ucStatus" runat="server" Visible="false" />
    </div>
    </form>
</body>
</html>
