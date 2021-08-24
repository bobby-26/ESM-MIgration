<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersAdminAssetLicenseAdd.aspx.cs" Inherits="RegistersAdminAssetLicenseAdd" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
      <title>License</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="ds" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmRegistersAdminAssetAdd" autocomplete="off" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlAdminAssetEntry">
        <ContentTemplate>
            <div style="z-index: 1;">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="License" ShowMenu="false" />
                    </div>
                </div>
                <div class="Header">
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                        <eluc:TabStrip ID="MenuAdminAssetAdd" runat="server" OnTabStripCommand="MenuAdminAssetAdd_TabStripCommand">
                        </eluc:TabStrip>
                    </div>
                </div>
                <br />
                <table cellpadding="1" cellspacing="1" width="100%">
                  <tr>
                        <td width="20%">
                            <asp:Literal ID="lblAssetType" runat="server" Text="Asset Type"></asp:Literal>
                        </td>
                        <td width="30%">
                            <asp:DropDownList ID="ddlAssetType" runat="server" CssClass="input_mandatory" Width="120px"
                                DataValueField="FLDASSETTYPEID" DataTextField="FLDNAME">
                            </asp:DropDownList>
                        </td>
                        <td width="20%">
                            <asp:Literal ID="lblPoReference" runat="server" Text="PO Reference"></asp:Literal>
                        </td>
                        <td width="30%">
                            <asp:TextBox runat="server" ID="TxtPoreference" MaxLength="100" CssClass="input" Width="120px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td width="20%">
                           <asp:Literal ID="lblAssetName" runat="server" Text="Name"></asp:Literal> 
                        </td>
                        <td width="30%">
                            <asp:TextBox ID="txtAssetName" runat="server" MaxLength="100" CssClass="input_mandatory" Width="120px"></asp:TextBox>
                        </td>
                        <td width="20%">
                            <asp:Literal ID="lblInvoiceDate" runat="server" Text="Invoice Date"></asp:Literal>
                        </td>
                        <td width="30%">
                            <eluc:UserControlDate ID="UcInvoiceDate" runat="server" DatePicker="true"
                                CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td width="20%">
                           <asp:Literal ID="lblSerialNumber" runat="server" Text="Media Part No"></asp:Literal>  
                        </td>
                        <td width="30%">
                            <asp:TextBox ID="Txtserialno" runat="server" MaxLength="100" CssClass="input_mandatory" Width="120px"></asp:TextBox>
                        </td>
                        <td width="20%">
                            <asp:Literal ID="lblInvoiceNo" runat="server" Text="Invoice No"></asp:Literal>
                        </td>
                        <td width="30%">
                            <asp:TextBox runat="server" ID="TxtInvoiceno" MaxLength="100" CssClass="input" Width="120px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td width="20%">
                            <asp:Literal ID="lblIdentificationNumber" runat="server" Text="License No"></asp:Literal> 
                        </td>
                        <td width="30%">
                            <asp:TextBox ID="TxtIdentityno" runat="server" MaxLength="100" CssClass="input" Width="120px"></asp:TextBox>
                        </td>
                        <td width="20%">
                            <asp:Literal ID="lblBudgetYear" runat="server" Text="Budget Year"></asp:Literal>
                        </td>
                        <td width="30%">
                            <asp:DropDownList ID="ddlYear" runat="server" CssClass="input" Width="120px">
                            </asp:DropDownList>
                        </td>
                    </tr>                
                    <tr>
                        <td width="20%">
                           <asp:Literal ID="lblVersion" runat="server" Text="Version"></asp:Literal>  
                        </td>
                        <td width="30%">
                            <eluc:Number ID="TxtVersion" runat="server" MaxLength="4" CssClass="input" Width="20px" />
                        </td>
                        <td width="20%">
                           <asp:Literal ID="lblExpiryDate" runat="server" Text="Expiry Date"></asp:Literal> 
                        </td>
                        <td width="30%">
                            <eluc:UserControlDate ID="UcExpirydate" DatePicker="true" runat="server"
                                CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td width="20%">
                           <asp:Literal ID="lblQuantity" runat="server" Text="No of Quantity"></asp:Literal> 
                        </td>
                        <td width="30%">
                        <eluc:Number runat="server" ID="ucQty" CssClass="input" MaxLength="3" Width="20px" />                                    
                        </td>
                        <td width="20%">
                            <asp:Literal ID="lblDisposalDate" runat="server" Text="Disposal Date"></asp:Literal>
                        </td>
                        <td width="30%">
                            <eluc:UserControlDate ID="ucDisposalDate" runat="server" DatePicker="true"
                                CssClass="input" />
                        </td>
                    </tr>           
                    <tr>
                        <td width="20%">
                            <asp:Literal ID="lblLocation" runat="server" Text="Location"></asp:Literal> 
                        </td>
                        <td width="30%">
                            <eluc:Company ID="ddlLocation" runat="server" Width="120px"
                                    AppendDataBoundItems="true" CssClass="readonlytextbox" Enabled="false" />
                        </td>
                        <td width="20%">
                            <asp:Literal ID="lblDisposalReason" runat="server" Text="Disposal Reason"></asp:Literal>
                        </td>
                        <td width="30%">
                            <asp:TextBox runat="server" ID="txtDisposalReason" CssClass="input" Width="250px" height="50px"
                                TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td width="20%">
                            <asp:Literal ID="lblAssetDescription" runat="server" Text="Description"></asp:Literal> 
                        </td>
                        <td width="30%">
                            <asp:TextBox ID="Txtdescriptionadd" runat="server" MaxLength="100" CssClass="input" Width="250px" height="50px" TextMode="MultiLine"></asp:TextBox>
                        </td>
                        <td width="20%">
                            <asp:Literal ID="lblRemarks" runat="server" Text="Remarks"></asp:Literal>
                        </td>
                        <td width="30%">
                            <asp:TextBox runat="server" ID="txtRemarks" CssClass="input" Width="250px" height="50px"
                                TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
