<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InventorySpareItemRequestGeneralAdd.aspx.cs" Inherits="Inventory_InventorySpareItemRequestGeneralAdd" %>

<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlUnit" Src="~/UserControls/UserControlUnit.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlAddress" Src="~/UserControls/UserControlMultiColumnAddress.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Spare Request</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
            <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
            <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
            </telerik:RadWindowManager>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" CssClass="hidden" OnClick="cmdHiddenSubmit_Click" />

            <eluc:TabStrip ID="MenuStockItemGeneral" runat="server" OnTabStripCommand="InventoryStockItemGeneral_TabStripCommand"></eluc:TabStrip>
            <telerik:RadLabel ID="lblNote" runat="server" Text="Note: Enter Existing Spare Number and click Fetch to get the Existing Spare Item."
                CssClass="guideline_text">
            </telerik:RadLabel>
           
            <telerik:RadAjaxPanel ID="pnlComponentGeneral" runat="server">
                <table>

                 
                    <tr>
                        <td>
                            <table>
                                <tr >
                                        <td colspan="2" align="center">
                                            <telerik:RadLabel ID="RadLabel2" runat="server" Text="Request Values"
                                                CssClass="guideline_text"></telerik:RadLabel>
                                        </td>
                                    </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblChangeRequestType" runat="server" Text="Request Type"></telerik:RadLabel>
                                    </td>
                                    <td >
                                        <telerik:RadDropDownList ID="ddlChangeReqType" CssClass="input_mandatory" runat="server"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlChangeReqType_SelectedIndexChanged">
                                            <Items>
                                                <telerik:DropDownListItem Text="--Select--" Value="" />
                                                <telerik:DropDownListItem Text="Insert New" Value="0" />
                                                <telerik:DropDownListItem Text="Update" Value="1" />
                                                <telerik:DropDownListItem Text="Delete" Value="2" />
                                            </Items>
                                        </telerik:RadDropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <span id="spnPickItem">
                                            <telerik:RadMaskedTextBox ID="txtNumber" runat="server" Mask="###.##.##.###"></telerik:RadMaskedTextBox>
                                            <asp:ImageButton runat="server" ID="cmdShowItem" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" Style="cursor: pointer; vertical-align: top;"
                                                ImageAlign="AbsMiddle" />
                                        </span>
                                        <asp:ImageButton ID="cmdNextNumber" runat="server" AlternateText="Next Number" ImageUrl="<%$ PhoenixTheme:images/next-number.png%>"
                                            Style="cursor: pointer; vertical-align: top;" ToolTip="NextNumber" OnClick="cmdNextNumber_OnClick" />
                                        &nbsp;&nbsp;&nbsp;<asp:Button ID="btnFetch" runat="server" Text="Fetch" CssClass="input"
                                            OnClick="btnFetch_OnClick" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox runat="server" ID="txtName" CssClass="input_mandatory" MaxLength="200"
                                            Width="240px">
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblMaker" runat="server" Text="Maker"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:UserControlAddress ID="txtMakerId" runat="server" AddressType="130,131" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblMakerReference" runat="server" Text="Maker's Reference"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtMakerReference" runat="server" CssClass="input" MaxLength="50"
                                            Width="240px">
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblPreferredVendor" runat="server" Text="Preferred Vendor"></telerik:RadLabel>
                                    </td>
                                    <td>

                                        <eluc:UserControlAddress ID="txtVendorId" runat="server" AddressType="130,131" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblStockMaximum" runat="server" Text="Stock Maximum"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadMaskedTextBox ID="txtStockMaximumQuantity" runat="server" Mask="#######"></telerik:RadMaskedTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblStockMinimum" runat="server" Text="Stock Minimum"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadMaskedTextBox ID="txtStockMinimumQuantity" Mask="#######" runat="server"></telerik:RadMaskedTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblReorderLevel" runat="server" Text="Reorder Level"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadMaskedTextBox ID="txtReOrderLevel" Mask="#######" runat="server"></telerik:RadMaskedTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblReorderQuantity" runat="server" Text="Reorder Quantity"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadMaskedTextBox ID="txtReOrderQuantity" Mask="#######" runat="server"></telerik:RadMaskedTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblWantedQuantity" runat="server" Text="Wanted Quantity"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadMaskedTextBox ID="txtWantedQuantity" Mask="#######" runat="server"></telerik:RadMaskedTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblUnit" runat="server" Text="Unit"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:UserControlUnit ID="ddlStockUnit" runat="server" AppendDataBoundItems="true"
                                            CssClass="input_mandatory" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblCritical" runat="server" Text="Critical"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadCheckBox ID="chkIsCritical" runat="server" Checked="false" />
                                    </td>
                                </tr>

                            </table>
                        </td>
                        <%-- Atual Values --%>

                        <td>
                            <div id="divOldValues" runat="server">
                                <table>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <telerik:RadLabel ID="RadLabel1" runat="server" Text="Atual Values"
                                                CssClass="guideline_text"></telerik:RadLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <br />
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblONumber" runat="server" Text="Number"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadTextBox runat="server" ID="txtONumber" CssClass="readonlytextbox" MaxLength="20"
                                                Width="80px" ReadOnly="true">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblOName" runat="server" Text="Name"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadTextBox runat="server" ID="txtOName" CssClass="readonlytextbox" MaxLength="200"
                                                Width="240px" ReadOnly="true">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblOMaker" runat="server" Text="Maker"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <span id="spnOPickListMaker">
                                                <telerik:RadTextBox ID="txtOMakerCode" runat="server" ReadOnly="true" CssClass="input readonlytextbox"
                                                    MaxLength="20" Width="60px">
                                                </telerik:RadTextBox>
                                                <telerik:RadTextBox ID="txtOMakerName" runat="server" ReadOnly="true" CssClass="input readonlytextbox"
                                                    MaxLength="200" Width="150px">
                                                </telerik:RadTextBox>
                                                <telerik:RadTextBox ID="txtOMakerId" runat="server" CssClass="input" ReadOnly="true" Width="0px"></telerik:RadTextBox>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblOMakerReference" runat="server" Text="Maker's Reference"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtOMakerReference" runat="server" MaxLength="50" Width="240px"
                                                CssClass="readonlytextbox" ReadOnly="true">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblOPreferredVendor" runat="server" Text="Preferred Vendor"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <span id="spnOPickListVendor">
                                                <telerik:RadTextBox ID="txtOPreferredVendorCode" runat="server" ReadOnly="true" CssClass="input readonlytextbox"
                                                    MaxLength="20" Width="60px">
                                                </telerik:RadTextBox>
                                                <telerik:RadTextBox ID="txtOPreferredVendorName" runat="server" ReadOnly="true" CssClass="input readonlytextbox"
                                                    MaxLength="200" Width="150px">
                                                </telerik:RadTextBox>
                                                <telerik:RadTextBox ID="txtOVendorId" runat="server" CssClass="input" Width="10px"></telerik:RadTextBox>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblOStockMaximum" runat="server" Text="Stock Maximum"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadTextBox runat="server" ID="txtOStockMaximumQuantity" CssClass="readonlytextbox txtNumber"
                                                MaxLength="9" Width="80px" ReadOnly="true">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblOStockMinimum" runat="server" Text="Stock Minimum"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtOStockMinimumQuantity" runat="server" CssClass="readonlytextbox txtNumber"
                                                MaxLength="9" Width="80px" ReadOnly="true">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblOReorderLevel" runat="server" Text="Reorder Level"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtOReOrderLevel" runat="server" CssClass="readonlytextbox txtNumber"
                                                MaxLength="2" Width="80px" ReadOnly="true">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblOReorderQuantity" runat="server" Text="Reorder Quantity"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadTextBox runat="server" ID="txtOReOrderQuantity" CssClass="readonlytextbox txtNumber"
                                                MaxLength="9" Style="text-align: right" Width="80px">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblOWantedQuantity" runat="server" Text="Wanted Quantity"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtOWantedQuantity" runat="server" CssClass="readonlytextbox txtNumber"
                                                MaxLength="9" Width="80px">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblOUnit" runat="server" Text="Unit"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <eluc:UserControlUnit ID="ddlOStockUnit" runat="server" AppendDataBoundItems="true"
                                                CssClass="readonlytextbox" Enabled="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblOCritical" runat="server" Text="Critical"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadCheckBox ID="chkOIsCritical" runat="server" Enabled="false" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>

                    </tr>
                    <tr>
                        <td colspan="2">
                            <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>


                            <telerik:RadTextBox ID="txtRemarksChange" runat="server" Rows="2" TextMode="MultiLine" Width="240px"
                                CssClass="input">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                </table>


            </telerik:RadAjaxPanel>
        </div>
    </form>
</body>
</html>
