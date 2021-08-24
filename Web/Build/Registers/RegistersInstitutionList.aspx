<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersInstitutionList.aspx.cs"
    Inherits="RegistersInstitutionList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlCommonAddress.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Institution List</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

</telerik:RadCodeBlock></head>
<body>
    <form id="frmInstitution" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">
            <asp:Label runat="server" ID="lblCaption" Font-Bold="true" Text="Institution"></asp:Label>
        </div>
    </div>
    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuInstitutionList" runat="server" OnTabStripCommand="MenuInstitutionList_TabStripCommand">
        </eluc:TabStrip>
    </div>
    <div id="divFind">
        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td>
                    &nbsp;<asp:Literal ID="lblInstitution" runat="server" Text="Institution"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtName" MaxLength="100" CssClass="input_mandatory"
                        Width="360px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <eluc:Address ID="ucAddress" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                   &nbsp;<asp:Literal ID="lblPhone1" runat="server" Text="Phone 1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtPhone1" runat="server" CssClass="input_mandatory txtNumber" MaxLength="50"></asp:TextBox>
                    <ajaxToolkit:MaskedEditExtender ID="maskeditPhone1" runat="server" TargetControlID="txtPhone1"
                        OnInvalidCssClass="MaskedEditError" Mask="9999999999" MaskType="Number" InputDirection="RightToLeft"
                        AutoComplete="false" />
                </td>
            </tr>
            <tr>
                <td>
                   &nbsp;<asp:Literal ID="lblPhone2" runat="server" Text="Phone 2"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtPhone2" runat="server" CssClass="input txtNumber" MaxLength="50"></asp:TextBox>
                    <ajaxToolkit:MaskedEditExtender ID="maskeditPhone2" runat="server" TargetControlID="txtPhone2"
                        OnInvalidCssClass="MaskedEditError" Mask="9999999999" MaskType="Number" InputDirection="RightToLeft"
                        AutoComplete="false" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;<asp:Literal ID="lblFaxNo" runat="server" Text="Fax No"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtFaxno" runat="server" CssClass="input txtNumber" MaxLength="50"></asp:TextBox>
                    <ajaxToolkit:MaskedEditExtender ID="maskeditfaxno" runat="server" TargetControlID="txtFaxno"
                        OnInvalidCssClass="MaskedEditError" Mask="9999999999" MaskType="Number" InputDirection="RightToLeft"
                        AutoComplete="false" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;<asp:Literal ID="lblEmail" runat="server" Text="Email"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="input_mandatory" MaxLength="100"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;<asp:Literal ID="lblContact" runat="server" Text="Contact"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtContact" runat="server" CssClass="input" MaxLength="100"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
