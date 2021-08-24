<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseFormFilter.aspx.cs"
    Inherits="PurchaseFormFilter" %>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MakerList" Src="~/UserControls/UserControlMaker.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="../UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="../UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="../UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
         <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>       

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmStockItemFilter" runat="server" autocomplete="off">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="MenuFormFilter">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuFormFilter" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>
        <eluc:TabStrip ID="MenuFormFilter" runat="server" OnTabStripCommand="MenuFormFilter_TabStripCommand">
                </eluc:TabStrip>
    <asp:UpdatePanel runat="server" ID="pnlDiscussion">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="margin-left: 0px; vertical-align: top;
                border: none; width: 100%">
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td>
                           <telerik:RadLabel RenderMode="Lightweight" ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                            <asp:Button ID="btnSubmit" runat="server" onclick="btnSubmit_Click"/>
                        </td>
                        <td>
                        
                            <eluc:Vessel runat="server" ID="ucVessel" AppendDataBoundItems="true" VesselsOnly="true" AssignedVessels="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblStockType" runat="server" Text="Stock Type"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadDropDownList runat="server" ID="ddlStockType" CssClass="input_mandatory" Width="240px">
                                <Items>
                                    <telerik:DropDownListItem Text="--Select--" Value="Dummy" Selected="true" />
                                    <telerik:DropDownListItem Text="Spares" Value="SPARE" />
                                    <telerik:DropDownListItem Text="Stores" Value="STORE" />
                                    <telerik:DropDownListItem Text="Service" Value="SERVICE" />
                                </Items>
                            </telerik:RadDropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight"  ID="txtFormNumber" runat="server" Width="90px" CssClass="input"></telerik:RadTextBox >
                        </td>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblFormState" runat="server" Text="Form State"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Hard ID="ucFormState" AppendDataBoundItems="true" CssClass="input" runat="server" Width="240px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblTitle" runat="server" Text="Title"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight"  ID="txtFromTitle" runat="server" Width="240px" CssClass="input" MaxLength="100"></telerik:RadTextBox >
                        </td>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblApproval" runat="server" Text="Approval"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Hard ID="ucApproval" AppendDataBoundItems="true" CssClass="input" runat="server" Width="240px"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="Vendor" runat="server" Text="Vendor"></telerik:RadLabel>
                        </td>
                        <td>
                            <span id="spnPickListMaker">
                                <telerik:RadTextBox RenderMode="Lightweight"  ID="txtVendorNumber" runat="server" Width="60px" CssClass="input" Enabled="False"></telerik:RadTextBox >
                                <telerik:RadTextBox RenderMode="Lightweight"  ID="txtVenderName" runat="server" Width="180px" CssClass="input" Enabled="False"></telerik:RadTextBox >
                                <%--<asp:ImageButton runat="server" ID="cmdShowMaker" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                    ImageAlign="AbsMiddle" OnClientClick="return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=130,131', true);"
                                    Text=".." />--%>
                                 <asp:LinkButton ID="cmdShowMaker" runat="server" ImageAlign="AbsMiddle" Text=".." OnClientClick="return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=130,131', true);">
                                    <span class="icon"><i class="fas fa-tasks"></i></span>
                                </asp:LinkButton>
                                <telerik:RadTextBox RenderMode="Lightweight"  ID="txtVendor" runat="server" Width="1px" CssClass="input"></telerik:RadTextBox >
                            </span>
                        </td>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblPriority" runat="server" Text="Priority"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Quick ID="UCPeority" AppendDataBoundItems="true" CssClass="input" runat="server" Width="240px"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblDeliveryLocation" runat="server" Text="Delivery Location"></telerik:RadLabel>
                        </td>
                        <td>
                            <span id="spnDLocation">
                                <telerik:RadTextBox RenderMode="Lightweight"  ID="txtDeliveryLocationCode" runat="server" Width="60px" CssClass="input"
                                    Enabled="False"></telerik:RadTextBox >
                                <telerik:RadTextBox RenderMode="Lightweight"  ID="txtDeliveryLocationName" runat="server" Width="180px" CssClass="input"
                                    Enabled="False"></telerik:RadTextBox >
                                <%--<asp:ImageButton ID="cmdShowLocation" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                    ImageAlign="AbsMiddle" OnClientClick="return showPickList('spnDLocation', 'codehelp1', '', '../Common/CommonPickListDeliveryLocation.aspx', true);"
                                    Text=".." />--%>
                                 <asp:LinkButton ID="cmdShowLocation" runat="server" ImageAlign="AbsMiddle" Text=".." OnClientClick="return showPickList('spnDLocation', 'codehelp1', '', '../Common/CommonPickListDeliveryLocation.aspx', true);">
                                    <span class="icon"><i class="fas fa-tasks"></i></span>
                                </asp:LinkButton>
                                <telerik:RadTextBox RenderMode="Lightweight"  ID="txtDeliveryLocationId" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox >
                            </span>
                        </td>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblReceiptCondition" runat="server" Text="Receipt Condition"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Hard ID="UCrecieptCondition" AppendDataBoundItems="true" CssClass="input" runat="server" Width="240px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblBudgetCode" runat="server" Text="Budget Code"></telerik:RadLabel>
                        </td>
                        <td>
                            <span id="spnPickListMainBudget">
                                <telerik:RadTextBox RenderMode="Lightweight"  ID="txtBudgetCode" runat="server" Width="60px" CssClass="input" Enabled="False"></telerik:RadTextBox >
                                <telerik:RadTextBox RenderMode="Lightweight"  ID="txtBudgetName" runat="server" Width="180px" CssClass="input" Enabled="False"></telerik:RadTextBox >
                                <%--<asp:ImageButton ID="btnShowBudget" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                    ImageAlign="AbsMiddle" OnClientClick="return showPickList('spnPickListMainBudget', 'codehelp1', '', '../Common/CommonPickListBudget.aspx?hardtypecode=30&isvalidate=1', true); "
                                    Text=".." />--%>
                                 <asp:LinkButton ID="btnShowBudget" runat="server" ImageAlign="AbsMiddle" Text=".." OnClientClick="return showPickList('spnPickListMainBudget', 'codehelp1', '', '../Common/CommonPickListBudget.aspx?hardtypecode=30&isvalidate=1', true); ">
                                    <span class="icon"><i class="fas fa-tasks"></i></span>
                                </asp:LinkButton>
                                <telerik:RadTextBox RenderMode="Lightweight"  ID="txtBudgetId" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox >
                                <telerik:RadTextBox RenderMode="Lightweight"  ID="txtBudgetgroupId" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox >
                            </span>
                        </td>
                        <td>
                            <%-- Financial Year--%>
                        </td>
                        <td>
                            <eluc:Quick ID="ucFinacialYear" AppendDataBoundItems="true" Visible ="false" CssClass="input" runat="server"  Width="240px"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblFormStatus" runat="server" Text="Form Status"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Hard ID="ucFormStatus" AppendDataBoundItems="true" CssClass="input" runat="server" Width="240px" />
                        </td>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblFormType" runat="server" Text="Form Type"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Hard ID="ucFormType" AppendDataBoundItems="true" CssClass="input" runat="server" Width="240px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblComponentClass" runat="server" Text="Component Class"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Quick ID="ddlComponentClass" runat="server" CssClass="input" AppendDataBoundItems="true"  Width="240px"/>
                        </td>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblMakerReferenceProductCode" runat="server" Text="Maker Reference / Product Code"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight"  runat="server" ID="txtMakerReference" CssClass="input" Width="240px"></telerik:RadTextBox >
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblOrderedDate" runat="server" Text="Ordered Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtOrderedDate" runat="server" Width="115px" CssClass="input" />
                            -
                            <eluc:Date ID="txtOrderedToDate" runat="server" Width="115px" CssClass="input" />
                        </td>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblCreatedDate" runat="server" Text="Created Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtCreatedDate" runat="server" Width="115px" CssClass="input" />
                            -
                            <eluc:Date ID="txtCreatedToDate" runat="server" Width="115px" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblApprovedDate" runat="server" Text="Approved Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtApprovedDate" runat="server" Width="115px" CssClass="input" />
                            -
                            <eluc:Date ID="txtApprovedToDate" runat="server" Width="115px" CssClass="input" />
                        </td>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblDept" runat="server"  Text="Department"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadDropDownList runat="server" ID="ddlDepartment" CssClass="input" Width="240px">
                            </telerik:RadDropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblReqStatus" runat="server" Text="Requisition Status" ></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlReqStatus" runat="server" RenderMode="Lightweight" EnableLoadOnDemand="true" Width="240px">
                                <Items>
                                    <telerik:RadComboBoxItem Text="--Select--" Value="Dummy" />
                                    <telerik:RadComboBoxItem Text="NEW REQUISITION" Value="NEW REQUISITION" />
                                    <telerik:RadComboBoxItem Text="VERIFIED" Value="VERIFIED" />
                                    <telerik:RadComboBoxItem Text="QUOTATION NOT RECEIVED" Value="QUOTATION NOT RECEIVED" />
                                    <telerik:RadComboBoxItem Text="AWAITING ALL QUOTE" Value="AWAITING ALL QUOTE" />
                                    <telerik:RadComboBoxItem Text="QUOTATION QUOTED" Value="QUOTATION QUOTED" />
                                    <telerik:RadComboBoxItem Text="SUBMITTED FOR SUPERINTENDENT TO REVIEW" Value="SUBMITTED FOR SUPERINTENDENT TO REVIEW" />
                                    <telerik:RadComboBoxItem Text="QUOTATION AWAITING APPROVAL" Value="QUOTATION AWAITING APPROVAL" />
                                    <telerik:RadComboBoxItem Text="QUOTATION PARTIALLY APPROVED" Value="QUOTATION PARTIALLY APPROVED" />
                                    <telerik:RadComboBoxItem Text="PO WAITING TO BE ISSUED" Value="PO WAITING TO BE ISSUED" />
                                    <telerik:RadComboBoxItem Text="PO ISSUED" Value="PO ISSUED" />
                                    <telerik:RadComboBoxItem Text="PO ISSUED SUPPLIED BY OWNER" Value="PO ISSUED SUPPLIED BY OWNER" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                        <td ><telerik:RadLabel ID="lblReason4Requisition" runat="server" Text="Reason for Requisition"></telerik:RadLabel></td>
                        <td>
                            <eluc:Quick ID="ucReason4Requisition" runat="server" CssClass="input"
                        AppendDataBoundItems="true" QuickTypeCode="147" Width="250px" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
