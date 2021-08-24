<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersDocumentRequiredLicenceCopyToFilter.aspx.cs"
    Inherits="RegistersDocumentRequiredLicenceCopyToFilter" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Copy Licence</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <style type="text/css">
            .fon {
                font-size: small !important;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>

    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersDocumentsRequiredLicenceCopy" DecoratedControls="All" />
    <form id="frmRegistersDocumentsRequiredLicenceCopy" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" Visible="false" />


            <eluc:TabStrip ID="MenuCopy" runat="server" OnTabStripCommand="Copy_TabStripCommand"></eluc:TabStrip>

            <table width="100%">

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselType" runat="server" Text="Vessel Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:VesselType runat="server" ID="ucVesselType" CssClass="gridinput_mandatory" AppendDataBoundItems="true"
                            AutoPostBack="true" OnTextChangedEvent="BindVessel" Width="240px" />

                        <span id="Span1" class="icon" runat="server"><i class="fas fa-info-circle" style="align-content: center"></i></span>
                        <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="RadToolTip1" Width="300px" ShowEvent="onmouseover"
                            RelativeTo="Element" Animation="Fade" TargetControlID="Span1" IsClientID="true" CssClass="fon"
                            HideEvent="ManualClose" Position="TopCenter" EnableRoundedCorners="true" ContentScrolling="Auto"
                            Text="Notes :<br/>1. Licence can be copied only to vessels of the same flag. <br/> 2. Data won't be copied to already copied vessels even if selected">
                        </telerik:RadToolTip>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPrincipal" runat="server" Text="Principal"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:AddressType runat="server" ID="ucPrincipal" AddressType="128" CssClass="input"
                            AppendDataBoundItems="true" AutoPostBack="true" OnTextChangedEvent="BindVessel" Width="240px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCopyTo" runat="server" Text="Copy To"></telerik:RadLabel>
                    </td>
                    <td colspan="1">
                        <table width="100%">
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblCheckAll" runat="server" Text="Check All"></telerik:RadLabel>
                                    <asp:CheckBox ID="chkChkAllVessel" runat="server" AutoPostBack="true" OnCheckedChanged="SelectAllVessel" />
                                    <div runat="server" id="dvVessel" class="input_mandatory" style="overflow: auto; width: 90%; height: 100px">
                                        <asp:CheckBoxList runat="server" ID="cblVessel" Height="100%" RepeatColumns="1" RepeatDirection="Horizontal"
                                            RepeatLayout="Flow">
                                        </asp:CheckBoxList>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
