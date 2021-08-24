<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InventoryStoreItemFilter.aspx.cs"
    Inherits="InventoryStoreItemFilter" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MakerList" Src="~/UserControls/UserControlMaker.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlHard" Src="~/UserControls/UserControlHard.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Store item Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmStockItemFilter" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div style="font-weight: 600; font-size: 12px;" runat="server">
            <eluc:TabStrip ID="MenuStockItemFilter" runat="server" OnTabStripCommand="MenuStockItemFilter_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        </div>
        <br clear="all" />

        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtNumber" runat="server" CssClass="input" MaxLength="8" Width="223px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtName" runat="server" CssClass="input" Width="223px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 15%">
                    <telerik:RadLabel ID="lblPreferredVendor" runat="server" Text="Preferred Vendor"></telerik:RadLabel>
                </td>
                <td>
                    <span id="spnPickListVendor">
                        <telerik:RadTextBox ID="txtPreferredVendorCode" runat="server" ReadOnly="false" CssClass="input readonlytextbox"
                            MaxLength="20" Width="100px">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtPreferredVendorName" runat="server" ReadOnly="false" CssClass="input readonlytextbox"
                            MaxLength="200" Width="120px">
                        </telerik:RadTextBox>
                        <asp:LinkButton ID="ImgShowMakerVendor" runat="server" ImageAlign="AbsMiddle" Text="..">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                        </asp:LinkButton>
                        <asp:LinkButton runat="server" ID="cmdClear" ImageAlign="AbsMiddle" Text=".." OnClick="cmdClear_Click">
                            <span class="icon"><i class="fas fa-paint-brush"></i></span>
                        </asp:LinkButton>
                        <telerik:RadTextBox ID="txtVendorId" runat="server" CssClass="input" Width="10px"></telerik:RadTextBox>
                    </span>&nbsp;
                        
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblStoreType" runat="server" Text="Store Type"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:UserControlHard ID="ddlStockClass" runat="server" Visible="true" OnTextChangedEvent="ddlStockClass_OnTextChangedEvent" AutoPostBack="true" CssClass="input" Width="223px"
                        AppendDataBoundItems="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblsubclass" runat="server" Text="Subclass Type"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ddlsubclasstype"  AutoPostBack="true" EnableLoadOnDemand="true" Width="223px"  DataTextField="FLDSUBCLASS" DataValueField="FLDSUBCLASSID">
                    </telerik:RadComboBox>

                     </td>
            </tr>
            
            <tr>
                <td>
                    <telerik:RadLabel ID="lblShowAvailableStoreItemsonly" runat="server" Text="Show Available Store Items only"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadCheckBox ID="chkROB" runat="server" AutoPostBack="false" />
                </td>
            </tr>
            <tr>
                <td style="width: 15%">
                    <telerik:RadLabel ID="lblProductCode" runat="server" Text="Product Code"></telerik:RadLabel>
                </td>
                <td style="width: 30%">
                    <telerik:RadTextBox ID="txtVendorReference" runat="server" ReadOnly="false" CssClass="input"
                        MaxLength="10" Width="223px">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>       
                <td style="width: 15%">
                        <telerik:RadLabel ID="lblmaterialnumber" runat="server" Text="Material Number"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtmaterialnumber" runat="server"  CssClass="input"
                            Width="223px">
                        </telerik:RadTextBox>
                    </td>
                   </tr>
            <tr>
                <td>
                    <%-- Global Search--%>
                </td>
                <td>
                    <telerik:RadCheckBox ID="chkGlobalSearch" Checked="false" Visible="false" runat="server"
                        CssClass="input">
                    </telerik:RadCheckBox>
                </td>
            </tr>
        </table>

    </form>
</body>
</html>
