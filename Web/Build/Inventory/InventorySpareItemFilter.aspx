<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InventorySpareItemFilter.aspx.cs"
    Inherits="InventorySpareItemFilter" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MakerList" Src="~/UserControls/UserControlMaker.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Spare item Filter</title>
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
                <td width="25%">
                    <telerik:RadLabel ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtNumber" runat="server" CssClass="input" MaxLength="13" Width="223px"></telerik:RadTextBox>
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
                    <telerik:RadLabel ID="lblMaker" runat="server" Text="Maker"></telerik:RadLabel>
                </td>
                <td>
                    <span id="spnPickListMaker">
                        <telerik:RadTextBox ID="txtMakerCode" runat="server" ReadOnly="false" CssClass="input readonlytextbox"
                            MaxLength="20" Width="100px">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtMakerName" runat="server" ReadOnly="false" CssClass="input readonlytextbox"
                            MaxLength="200" Width="120px">
                        </telerik:RadTextBox>
                        <asp:LinkButton ID="ImgShowMaker" runat="server" ImageAlign="AbsMiddle" Text="..">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                        </asp:LinkButton>
                        <asp:LinkButton runat="server" ID="cmdClear" ImageAlign="AbsMiddle" Text=".." OnClick="cmdClear_Click">
                            <span class="icon"><i class="fas fa-paint-brush"></i></span>
                        </asp:LinkButton>
                        <telerik:RadTextBox ID="txtMakerId" runat="server" CssClass="hidden"></telerik:RadTextBox>
                    </span>&nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 20">
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
                        <asp:LinkButton runat="server" ID="cmdVendorClear" ImageAlign="AbsMiddle" Text=".." OnClick="cmdVendorClear_Click">
                            <span class="icon"><i class="fas fa-paint-brush"></i></span>
                        </asp:LinkButton>
                        <telerik:RadTextBox ID="txtVendorId" runat="server" CssClass="hidden"></telerik:RadTextBox>
                    </span>&nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblCritical" runat="server" Text="Critical"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadCheckBox ID="chkCritical" runat="server" AutoPostBack="false"></telerik:RadCheckBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblMakerReference" runat="server" Text="Maker Reference"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtMakerReference" runat="server" CssClass="input" MaxLength="200"
                        Width="223px">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblShowAvailableSpareItemsonly" runat="server" Text="Show Available Spare Items only"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadCheckBox ID="chkROB" runat="server" AutoPostBack="false" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblComponentNumber" runat="server" Text="Component Number"></telerik:RadLabel>
                </td>
                <td>
                    <span id="spnPickListComponent">
                        <telerik:RadTextBox ID="txtComponentNumber" runat="server" CssClass="input" Width="223px" MaxLength="20"></telerik:RadTextBox>
                        <asp:LinkButton ID="ImgShowComponent" runat="server" ImageAlign="AbsMiddle" Text="..">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="cmdClearComponent" runat="server" ImageAlign="AbsMiddle" Text=".." OnClick="cmdClearComponent_Click">
                                <span class="icon"><i class="fas fa-paint-brush"></i></span>
                        </asp:LinkButton>
                        <telerik:RadTextBox ID="txtTmpComponentName" runat="server" CssClass="hidden"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtComponentId" runat="server" CssClass="hidden"></telerik:RadTextBox>
                    </span>&nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblComponentName" runat="server" Text="Component Name"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtComponentName" CssClass="input" Width="223px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>       
                <td >
                        <telerik:RadLabel ID="lblmaterialnumber" runat="server" Text="Material Number"></telerik:RadLabel>
                    </td>
                    <td >
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtmaterialnumber" runat="server"  CssClass="input"
                            Width="223px">
                        </telerik:RadTextBox>
                    </td>
                   </tr>
            <tr>
            
            <tr>
                <td>
                    <%-- Drawing Number--%>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtDrawing" runat="server" Visible="false" CssClass="input" MaxLength="200"
                        Width="203px">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <%-- Global Search--%>
                </td>
                <td>
                    <telerik:RadCheckBox ID="chkGlobalSearch" Checked="false" runat="server" Visible="false"
                        CssClass="input">
                    </telerik:RadCheckBox>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
