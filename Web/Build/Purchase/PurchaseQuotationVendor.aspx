<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseQuotationVendor.aspx.cs"
    Inherits="PurchaseQuotationVendor" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="../UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlUserName" Src="../UserControls/UserControlUserName.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tax" Src="~/UserControls/UserControlTaxMaster.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TaxType" Src="~/UserControls/UserControlTaxType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="../UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Quotation Edit - Office side</title>
    <telerik:RadCodeBlock ID="RadCodeBlock" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript">
        function openCreditNoteDiscount() {
            var quotationid = '<%= ViewState["quotationid"] == null ? "" : ViewState["quotationid"].ToString() %>';
            //var txtDiscount = '<%=txtDiscount.ClientID %>';
            var txtDiscount = $find('txtDiscount');
            alert(quotationid);
            alert(txtDiscount);
            alert(txtDiscount.value);
            return showPickList('spnDiscount', 'codehelp1', '', '../Purchase/PurchaseQuotationEsmDiscount.aspx?quotationid=' + quotationid + '&discount=' + document.getElementById(txtDiscount).value, true);
        }


        function GridCreated(sender, args) {
            var scrollArea = sender.GridDataDiv;
            var dataHeight = sender.get_masterTableView().get_element().clientHeight;
            if (dataHeight < 200) {
                scrollArea.style.height = dataHeight + "px";
            }
        }


    </script>

        <style type="text/css">
            .table {
                border-collapse: collapse;
                
            }

            .table td,th {
                border: 1px solid black;
            }
        </style>
    </telerik:RadCodeBlock>

    
</head>
<body>
    <form id="frmQuotationVendor" runat="server" autocomplete="off">
     <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="MenuVender">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuVender" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>

    <eluc:TabStrip ID="MenuVender" runat="server" OnTabStripCommand="MenuVender_TabStripCommand"></eluc:TabStrip>
    <asp:UpdatePanel runat="server" ID="pnlFormGeneral">
        <ContentTemplate>
            <asp:Button ID="cmdHiddenPick" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenPick_Click" CssClass="hidden" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <div class="navigation" id="Div2" style="margin-top:0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblVendor" runat="server" Text="Vendor"></telerik:RadLabel>
                        </td>
                        <td>
                            <span id="spnPickListMaker">
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtVenderCode" runat="server" Width="85px" CssClass="input_mandatory"></telerik:RadTextBox>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtVenderName" runat="server" Width="180px" CssClass="input_mandatory"></telerik:RadTextBox>
                                <%--<asp:ImageButton ID="btnPickVender" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" CommandName="PICKVENDOR"
                                    ImageAlign="AbsMiddle" OnClientClick="return showPickList('spnPickListMaker', 'codehelp1', '', 'Common/CommonPickListAddressOwner.aspx?addresstype=132,130,131,128&framename=ifMoreInfo&txtsupcode='+ document.getElementById('txtVenderCode').value +'&txtsupname='+ document.getElementById('txtVenderName').value+'&principal='+document.getElementById('hdnprincipalId').value, true);"
                                    Text=".." />--%>
                                 <asp:LinkButton ID="btnPickVender" runat="server" ImageAlign="AbsMiddle" Text=".." OnClientClick="return showPickList('spnPickListMaker', 'codehelp1', '', 'Common/CommonPickListAddressOwner.aspx?addresstype=132,130,131,128&windowname=detail&framename=ifMoreInfo&POPUP=Y&txtsupcode='+ document.getElementById('txtVenderCode').value +'&txtsupname='+ document.getElementById('txtVenderName').value+'&principal='+document.getElementById('hdnprincipalId').value, true);">
                                    <span class="icon"><i class="fas fa-tasks"></i></span>
                                </asp:LinkButton>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtVenderID" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                            </span>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtVenderReference" runat="server" Width="0px" Visible="false" CssClass="input"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblQuotationRef" runat="server" Text="Reference"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtQtnRefenceno" runat="server" Width="120px" CssClass="input"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblReceivedDate" runat="server" Text="Received Date"></telerik:RadLabel>
                        </td>
                        <td class="style1">
                            <eluc:Date ID="txtRecivedDate" runat="server" Width="120px" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Repeater ID="rptPreferredby" runat="server" Visible="false">
                                <HeaderTemplate>
                                    <table style="width: 100%;" class="table">
                                        <tr class="DataGrid-HeaderStyle">
                                             <th>Preferred by</th>
                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                        <tr class="DataGrid-alternatingStyle">
                                            <td>
                                                <telerik:RadLabel ID="lblPrincipalName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRINCIPAL") %>'></telerik:RadLabel>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </td>
                        <td colspan="4">

                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblapprovalby" runat="server" Text="Approval By"></telerik:RadLabel>
                        </td>
                        <td>
                           <span id="spnApprovalBy">
                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtusername" runat="server" CssClass="input" Enabled="false" Width="270px"></telerik:RadTextBox>
                                    <%--<asp:ImageButton runat="server" ID="imgShowPO" Style="cursor: pointer; vertical-align: middle"
                                        ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" OnClientClick="return showPickList('spnApprovalBy', 'codehelp1', '', 'Common/CommonPickListPurchaseApprovalUser.aspx', true); " />--%>
                                    
                                     <asp:LinkButton ID="imgShowPO" runat="server" ImageAlign="AbsMiddle" Text=".." OnClientClick="return showPickList('spnApprovalBy', 'codehelp1', '', 'Common/CommonPickListPurchaseApprovalUser.aspx', true); ">
                                        <span class="icon"><i class="fas fa-tasks"></i></span>
                                    </asp:LinkButton>

                                    <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtusercode" CssClass="input" Width="0px"></telerik:RadTextBox>
                                    <asp:ImageButton ID="cmdMap" runat="server" ImageUrl="<%$ PhoenixTheme:images/Modify.png %>"
                                            ImageAlign="AbsMiddle" Text=".." OnClick="cmdMapUser_Click" ToolTip="Map Approval BY" />
                                </span>
                        </td>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblType" runat="server" Text="Items Type"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Hard ID="ddlType" runat="server" AppendDataBoundItems="true" CssClass="input" HardTypeCode="244" Width="120px" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblPartPaid" runat="server" Text="Part Paid"></telerik:RadLabel>
                        </td>
                        <td>
                            <span id="spnPicPartPaid">
                                <eluc:Decimal ID="txtPartPaid" runat="server" Width="120px" CssClass="input" ReadOnly="true" />
                                <%--<asp:ImageButton ID="cmdPicPartPaid" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                    ImageAlign="AbsMiddle" Text=".." />--%>
                                 <asp:LinkButton ID="cmdPicPartPaid" runat="server" ImageAlign="AbsMiddle" Text="..">
                                        <span class="icon"><i class="fas fa-tasks"></i></span>
                                 </asp:LinkButton>
                            </span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblExpirydate" runat="server" Text="Expiry date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtExpirationDate" runat="server" Width="120px" CssClass="input" />
                        </td>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblSentDate" runat="server" Text="Sent Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtSentDate" runat="server" Width="120px" CssClass="input" Enabled="false" />
                        </td>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblRejectedDate" runat="server" Text="Rejected Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtRejectedDate" runat="server" Width="120px" CssClass="input" Enabled="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblQuotationValidUntil" runat="server" Text="Valid Until"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtOrderDate" runat="server" Width="120px" CssClass="input" />
                        </td>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblSentBy" runat="server" Text="Sent By"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtNameSentBy" runat="server" CssClass="readonlytextbox" Width="120px"
                                Enabled="false"></telerik:RadTextBox>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtSentById" runat="server" CssClass="input" Width="0px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblRejectedBy" runat="server" Text="Rejected By"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtNameRejectedBy" runat="server" CssClass="readonlytextbox" Width="120px"
                                Enabled="false"></telerik:RadTextBox>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtRejectedById" runat="server" CssClass="input" Width="0px"></telerik:RadTextBox>
                            <eluc:Decimal ID="ucTotalAmount" runat="server" CssClass="input" ReadOnly="true"
                                Visible="false" Width="0px" />
                            <eluc:Decimal ID="txtTotalInUSD" runat="server" CssClass="input" ReadOnly="true"
                                Visible="false" Width="0px" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblQuotedCurrency" runat="server" Text="Quoted Currency"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:UserControlCurrency ID="ucCurrency" AppendDataBoundItems="false" CssClass="input "
                                runat="server" Width="60px" />
                            /
                            <telerik:RadTextBox RenderMode="Lightweight" ID="lblExchangeRate" runat="server" Text="" Width="90px" ReadOnly="true"
                                CssClass="input txtNumber"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblRateTotalPrice" runat="server" Text="Total Price"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Decimal ID="txtRate" runat="server" Width="120px" ReadOnly="true" CssClass="input" />
                        </td>
                        <td>
                            <%--Total Discount--%>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblTotalDisc" runat="server" Text="Total Discount"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Decimal ID="ucTotalDiscount" runat="server" Width="120px" ReadOnly="true" CssClass="input" DecimalDigits="2" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblCurrency" runat="server" Text="Currency"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblUSD" runat="server" Text="USD / 1.000000"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblUsdTotalPrice" runat="server" Text="Total Price"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Decimal ID="txtUsdPrice" runat="server" Width="120px" ReadOnly="true" CssClass="input" />
                        </td>
                        <td>
                            <%--Total Discount--%>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblUSDTotalDisc" runat="server" Text="Total Discount"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Decimal ID="txtTotalDiscount" runat="server" Width="120px" ReadOnly="true" CssClass="input" DecimalDigits="2" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblDiscount" runat="server" Text="Discount %"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Decimal RenderMode="Lightweight" ID="txtSupplierDiscount" runat="server" Width="120px" CssClass="input txtNumber" DecimalDigits="2"></eluc:Decimal>
                        </td>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblDelTimeDays" runat="server" Text="Del. Time(Days)"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Decimal ID="txtDeliveryTime" runat="server" Width="120px" CssClass="input" Mask="999"/>
                        </td>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblCreditNoteDisc" runat="server" Text="Credit Note Discount %"></telerik:RadLabel>
                        </td>
                        <td class="style1">
                            <span id="spnDiscount">
                                <eluc:Decimal runat="server" ID="txtDiscount" Width="120px" CssClass="input" Mask="99.99" />
                                <%--<asp:ImageButton ID="cmdDiscount" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                    ImageAlign="AbsMiddle" Text=".." />--%>
                                 <asp:LinkButton ID="cmdDiscount" runat="server" ImageAlign="AbsMiddle" Text="..">
                            <span class="icon"><i class="fas fa-tasks"></i></span>
                        </asp:LinkButton>
                            </span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblDeliveryTerms" runat="server" Text="Delivery Terms"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Quick ID="UCDeliveryTerms" AppendDataBoundItems="true" CssClass="input" runat="server" Width="120px" />
                        </td>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblPaymentTerms" runat="server" Text="Payment Terms"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Quick ID="UCPaymentTerms" AppendDataBoundItems="true" CssClass="input" runat="server" Width="120px" />
                        </td>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblModeofTransport" runat="server" Text="Mode of Transport"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Quick runat="server" ID="ucModeOfTransport" CssClass="input" QuickTypeCode="77" Width="120px"
                                AppendDataBoundItems="true" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblEmailIDsCaption" runat="server" Text="Email IDs"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <telerik:RadTextBox RenderMode="Lightweight" ID="lblEmailIds" runat="server" CssClass="input" Width="850px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblVesselAccount" runat="server" Text="Vessel Account " Visible="false"></telerik:RadLabel>
                        </td>
                        <td colspan="5">
                            <asp:DropDownList ID="ddlAccountDetails" runat="server" CssClass="dropdown_mandatory"
                                OnDataBound="ddlAccountDetails_DataBound" DataTextField="FLDVESSELACCOUNTDESCRIPTION"
                                DataValueField="FLDVESSELACCOUNTID" OnSelectedIndexChanged="ddlAccountDetails_SelectedIndexChanged"
                                AutoPostBack="true" Visible="false">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:HiddenField ID="hdnprincipalId" runat="server" />
                        </td>
                    </tr>
                </table>
                <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvTax" DecoratedControls="All" EnableRoundedCorners="true" />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvTax" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                CellSpacing="0" GridLines="None" OnDeleteCommand="gvTax_DeleteCommand" ShowFooter="true" Width="100%"
                OnNeedDataSource="gvTax_NeedDataSource" OnEditCommand="gvTax_EditCommand"
                OnItemDataBound="gvTax_ItemDataBound1" OnItemCommand="gvTax_ItemCommand" OnUpdateCommand="gvTax_UpdateCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDQUOTATIONTAXMAPCODE" TableLayout="Fixed">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Delivery/Tax/Other Charges Description">
                            <ItemStyle Wrap="false" Width="40%" />
                            <HeaderStyle Width="40%" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblDescriptionEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtDescriptionEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'
                                    CssClass="gridinput_mandatory" MaxLength="50" Width="100%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtDescriptionAdd" Text='' runat="server" CssClass="gridinput_mandatory"
                                    MaxLength="50" Width="100%">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Type">
                            <ItemStyle Wrap="False" Width="20%"></ItemStyle>
                            <HeaderStyle Width="20%" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTAXTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:TaxType ID="ucTaxTypeEdit" runat="server" TaxType='<%# DataBinder.Eval(Container,"DataItem.FLDTAXTYPE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:TaxType ID="ucTaxTypeAdd" runat="server" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Value">
                            <ItemStyle Wrap="False" Width="10%" HorizontalAlign="Right"></ItemStyle>
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblValue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALUE" ,"{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Decimal ID="txtValueEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALUE","{0:n2}") %>'
                                    CssClass="gridinput_mandatory txtNumber" DecimalDigits="2" Mask="99999.99" Width="100%" ></eluc:Decimal>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Decimal ID="txtValueAdd" runat="server" CssClass="gridinput_mandatory txtNumber"
                                    Width="100%" DecimalDigits="2" Mask="999999.99"></eluc:Decimal>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                         <telerik:GridTemplateColumn HeaderText="Budget">
                            <ItemStyle Wrap="False" Width="20%"></ItemStyle>
                            <HeaderStyle Width="20%" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblbudgetname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListMainBudgetEdit">
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtBudgetCodeEdit" runat="server" Width="120px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>' CssClass="input"></telerik:RadTextBox>
                                    <asp:LinkButton ID="btnShowBudgetEdit" runat="server" ImageAlign="AbsMiddle" Text="..">
                                    <span class="icon"><i class="fas fa-tasks"></i></span>
                                </asp:LinkButton>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtBudgetNameEdit" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtBudgetIdEdit" runat="server" Width="0px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETID") %>' CssClass="hidden"></telerik:RadTextBox>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtBudgetgroupIdEdit" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                            </span>

                            </EditItemTemplate>
                            <FooterTemplate>
                                <span id="spnPickListMainBudget">
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtBudgetCode" runat="server" Width="120px" CssClass="input"></telerik:RadTextBox>
                                    <asp:LinkButton ID="btnShowBudget" runat="server" ImageAlign="AbsMiddle" Text="..">
                                    <span class="icon"><i class="fas fa-tasks"></i></span>
                                </asp:LinkButton>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtBudgetName" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtBudgetId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtBudgetgroupId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                            </span>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                         <telerik:GridTemplateColumn HeaderText="Owner Budget">
                            <ItemStyle Wrap="False" Width="20%" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="20%" Wrap="false" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblownerbudgetname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERACCOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListOwnerBudgetEdit">
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtOwnerBudgetCodeEdit" runat="server" MaxLength="20" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERACCOUNT") %>' CssClass="input"
                                    Width="120px"></telerik:RadTextBox>
                                <asp:LinkButton ID="btnShowOwnerBudgetEdit" runat="server" ImageAlign="AbsMiddle" Text="..">
                                    <span class="icon"><i class="fas fa-tasks"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" ID="imgClearOwnerBudgetEdit" ImageAlign="AbsMiddle" Text=".." Visible="false" >
                                    <span class="icon"><i class="fas fa-paint-brush"></i></span>
                                </asp:LinkButton>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtOwnerBudgetNameEdit" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtOwnerBudgetIdEdit" runat="server" Width="0px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETID") %>' CssClass="hidden"></telerik:RadTextBox>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtOwnerBudgetgroupIdEdit" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                            </span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <span id="spnPickListOwnerBudget">
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtOwnerBudgetCode" runat="server" Text="" MaxLength="20" CssClass="input"
                                    Width="120px"></telerik:RadTextBox>
                                <asp:LinkButton ID="btnShowOwnerBudget" runat="server" ImageAlign="AbsMiddle" Text="..">
                                    <span class="icon"><i class="fas fa-tasks"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" ID="imgClearOwnerBudget" ImageAlign="AbsMiddle" Text=".." Visible="false" >
                                    <span class="icon"><i class="fas fa-paint-brush"></i></span>
                                </asp:LinkButton>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtOwnerBudgetName" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtOwnerBudgetId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtOwnerBudgetgroupId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                            </span>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Amount">
                            <ItemStyle Wrap="False" Width="10%" HorizontalAlign="Right"></ItemStyle>
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblTaxAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTAXAMOUNT","{0:n2}") %>'
                                    CssClass="txtNumber">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                            <HeaderStyle Width="120px" HorizontalAlign="Center" />
                            <ItemStyle Width="120px" HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandName="EDIT" ID="cmdEdit"
                                    ToolTip="Edit">
                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="DELETE" ID="cmdDelete"
                                    ToolTip="Delete">
                                            <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save"
                                    CommandName="Update" ID="cmdUpdate" ToolTip="Update">
                                            <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Cancel"
                                    CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                            <span class="icon"><i class="fas fa-times"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Add"
                                    CommandName="ADD" ID="cmdAdd" ToolTip="Add">
                                            <span class="icon"><i class="fas fa-plus"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                    </Columns>
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>

                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Items matching your search criteria"
                        PageSizeLabelText="Items per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <ClientEvents OnGridCreated="GridCreated" />
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="4" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    <%--<ClientEvents OnGridCreated="GridCreated" />--%>
                </ClientSettings>
            </telerik:RadGrid>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
