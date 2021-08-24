<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseFormType.aspx.cs"
    Inherits="PurchaseFormType" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>New Requisition</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
         <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

</telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPurchaseFormType" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        
            <eluc:TabStrip ID="MenuStockItemGeneral" runat="server" OnTabStripCommand="InventoryStockItemGeneral_TabStripCommand"  Title="New Requisition" >
            </eluc:TabStrip>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <asp:Button runat="server" ID="cmdHiddenPick" OnClick="cmdHiddenPick_Click" CssClass="hidden" />
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td style ="width:20%">
                       <telerik:RadLabel RenderMode="Lightweight" ID="lblVessel" runat="server"  Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td >
                        <eluc:Vessel runat="server" ID="ucVessel" AppendDataBoundItems="true" CssClass="input_mandatory" 
                            AssignedVessels="true" VesselsOnly="true" OnTextChangedEvent="ddlStockType_TextChanged" Width="180px"
                            AutoPostBack="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                       <telerik:RadLabel RenderMode="Lightweight" ID="lblStockType" runat="server"  Text="Stock Type"></telerik:RadLabel>
                        
                    </td>
                    <td>
                        <telerik:RadDropDownList RenderMode="Lightweight" runat="server" ID="ddlStockType" CssClass="input_mandatory" OnSelectedIndexChanged="ddlStockType_TextChanged"
                            AutoPostBack="true" Width="180px">
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
                       <telerik:RadLabel RenderMode="Lightweight" ID="lblDept" runat="server"  Text="Department"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDropDownList runat="server" ID="ddlDepartment" CssClass="input_mandatory" Width="180px" >
                        </telerik:RadDropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                       <telerik:RadLabel RenderMode="Lightweight" ID="lblCreationMethods" runat="server"  Text="Creation Methods"></telerik:RadLabel>
                        
                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rblCreation" runat="server" Direction="Horizontal" Enabled="false"
                            Width="180px">
                            <Items>
                                <telerik:ButtonListItem Text="Manual" Value="Manual" Selected="true" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td>
                    <telerik:RadLabel ID="lblClass" runat ="server" Text="Component Class">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListClass">
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtStockClass" runat="server" Width="60px" CssClass="readOnly" ReadOnly="true" ></telerik:RadTextBox>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtStockClassName" runat="server" Width="120px" CssClass="readOnly" ReadOnly="true"></telerik:RadTextBox>
                            <asp:LinkButton ID="btnStockClassPickList" runat="server" ImageAlign="AbsMiddle" Text=".." OnClientClick="return showPickList('spnPickListClass', 'codehelp1', '', 'Common/CommonPickListComponentClass.aspx', true);">
                                    <span class="icon"><i class="fas fa-tasks"></i></span>
                                </asp:LinkButton>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtStockClassId" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblComponent" runat="server" Text="Component"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListComponent">
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtComponentNo" runat="server" Width="60px" CssClass="input"></telerik:RadTextBox>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtComponentName" runat="server" Width="120px"></telerik:RadTextBox>
                            <asp:LinkButton ID="IbtnPickListComponent" runat="server" ImageAlign="AbsMiddle" Text=".." >
                                    <span class="icon"><i class="fas fa-tasks"></i></span>
                                </asp:LinkButton>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtComponentID" runat="server" Width="0px"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                </table>
    </form>
</body>
</html>
