<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersAdminAssetAdd.aspx.cs"
    Inherits="RegistersAdminAssetAdd" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZone.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="head1" runat="server">
    <title>Hardware</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="ds" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
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
                        <eluc:Title runat="server" ID="ucTitle" Text="Item" ShowMenu="false" />
                    </div>
                </div>
                <div class="Header">
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                        <eluc:TabStrip ID="MenuAdminAssetAdd" runat="server" OnTabStripCommand="MenuAdminAssetAdd_TabStripCommand">
                        </eluc:TabStrip>
                    </div>
                </div>
                <br />
                <table width="100%">
                    <tr>
                        <td width="110px">
                            <asp:Literal ID="lblAssetType" runat="server" Text="Asset"></asp:Literal>
                        </td>
                        <td width= "264px">
                            <asp:DropDownList ID="ddlAssetType" runat="server" CssClass="input_mandatory" Width="120px"
                                DataValueField="FLDASSETTYPEID" DataTextField="FLDNAME" OnSelectedIndexChanged="ddlAssetType_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td width = "135px">
                            <asp:Literal ID="lblServiceTag" runat="server" Text="Serial Number"></asp:Literal>  
                        </td>
                        <td width="339px">
                            <asp:TextBox ID="txtSerialNo" runat="server" MaxLength="50" CssClass="input_mandatory"></asp:TextBox>
                        </td>
                        <td width="174px">
                            <asp:Literal ID="lblBudgetYear" runat="server" Text="Budget Year"></asp:Literal>
                        </td>
                        <td width="339px">
                            <asp:DropDownList ID="ddlYear" runat="server" CssClass="input" Width="70px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td width="110px">
                            <asp:Literal ID="lblAssetName" runat="server" Text="Name"></asp:Literal> 
                        </td>
                        <td width= "264px">
                            <asp:TextBox ID="txtAssetName" runat="server" MaxLength="100" CssClass="input_mandatory"></asp:TextBox>
                        </td>
                        <td width = "135px">
                            <asp:Literal ID="lblLocation" runat="server" Text="Zone"></asp:Literal> 
                        </td>
                        <td width="339px">
                             <eluc:Zone ID="ddlLocation" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" Width="120px" />
                        </td>
                        <td width="174px">
                            <asp:Literal ID="lblExpiryDate" runat="server" Text="Expiry Date"></asp:Literal> 
                        </td>
                        <td width="339px">
                            <eluc:UserControlDate ID="UcExpirydate" DatePicker="true" runat="server"
                                CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td width="110px">
                            <asp:Literal ID="lblTagNumber" runat="server" Text="Tag Number"></asp:Literal>  
                        </td>
                        <td width= "264px">
                            <asp:TextBox ID="txtTagNumber" runat="server" MaxLength="50" CssClass="input"></asp:TextBox>
                        </td>
                        <td width = "135px">
                            <asp:Literal ID="lblPoReference" runat="server" Text="PO Reference"></asp:Literal>
                        </td>
                        <td width="339px">
                            <asp:TextBox runat="server" ID="TxtPoreference" MaxLength="100" CssClass="input" Width="115px"></asp:TextBox>
                        </td>
                        <td width="174px">
                            <asp:Literal ID="lblDisposalDate" runat="server" Text="Disposal Date"></asp:Literal>
                        </td>
                        <td width="339px">
                            <eluc:UserControlDate ID="ucDisposalDate" runat="server" DatePicker="true"
                                CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td width="110px">
                            <asp:Literal ID="lblMaker" runat="server" Text="Maker"></asp:Literal>  
                        </td>
                        <td width= "264px">
                            <asp:TextBox ID="txtMaker" runat="server" MaxLength="100" CssClass="input_mandatory"></asp:TextBox>
                        </td>
                        <td width = "135px">
                            <asp:Literal ID="lblInvoiceNo" runat="server" Text="Invoice No"></asp:Literal>
                        </td>
                        <td width="339px">
                            <asp:TextBox runat="server" ID="TxtInvoiceno" MaxLength="100" Width="115px" CssClass="input"></asp:TextBox>
                        </td>
                        <td width="174px">
                            <asp:Literal ID="lblDisposalReason" runat="server" Text="Disposal Reason"></asp:Literal>
                        </td>
                        <td width="339px">
                            <asp:TextBox runat="server" ID="txtDisposalReason" CssClass="input" Width="250px" height="50px"
                                TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td width="110px">
                            <asp:Literal ID="lblModel" runat="server" Text="Model"></asp:Literal> 
                        </td>
                        <td width= "264px">
                            <asp:TextBox ID="txtModel" runat="server" MaxLength="100" CssClass="input"></asp:TextBox>
                        </td>
                        <td width = "135px">
                            <asp:Literal ID="lblInvoiceDate" runat="server" Text="Invoice Date"></asp:Literal>
                        </td>
                        <td width="339px">
                            <eluc:UserControlDate ID="UcInvoiceDate" runat="server" DatePicker="true"
                                CssClass="input" />
                        </td>
                        <td width="174px">
                            <asp:Literal ID="lblRemarks" runat="server" Text="Remarks"></asp:Literal>
                        </td>
                        <td width="339px">
                            <asp:TextBox runat="server" ID="txtRemarks" CssClass="input" Width="250px" height="50px"
                                TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td width="110px">
                            <asp:Literal ID="lblAssetDescription" runat="server" Text="Description"></asp:Literal> 
                        </td>
                        <td width= "264px">
                            <asp:TextBox ID="Txtdescriptionadd" runat="server" CssClass="input" Width="250px" height="50px" TextMode="MultiLine"></asp:TextBox>
                        </td>
                        <td width="174px">

                        </td>
                        <td width="339px">

                        </td>
                    </tr>
                </table>
         </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
