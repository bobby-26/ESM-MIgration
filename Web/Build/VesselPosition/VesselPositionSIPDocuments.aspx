<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionSIPDocuments.aspx.cs" Inherits="VesselPositionSIPDocuments" MaintainScrollPositionOnPostback="true" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Documents</title>
   <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSIPDocuments" runat="server">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlSIPTanksConfuguration" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />

        <telerik:RadAjaxPanel runat="server" ID="pnlSIPTanksConfuguration">

                    <table width="100%">
                        <tr>
                            <td colspan="2">
                            <b><asp:LinkButton ID="lnkSIPGuideLines" runat="server" Text="SIP Manual and Instruction" OnClick="lnkSIPGuideLines_Click"></asp:LinkButton></b>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:30px;"></td>
                            <td>
                           <b>    <asp:LinkButton ID="lnkAppendix1" runat="server" Text="Appendix 1" OnClick="lnkAnnexer1_Click"></asp:LinkButton></b>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:30px;"></td>
                            <td>
                            <b>    <asp:LinkButton ID="lnkAppendix2" runat="server" Text="Appendix 2" OnClick="lnkAppendix2_Click"></asp:LinkButton></b>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:30px;"></td>
                            <td>
                             <b>   <asp:LinkButton ID="lnkAppendix3" runat="server" Text="Appendix 3" OnClick="lnkAppendix3_Click"></asp:LinkButton></b>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:30px;"></td>
                            <td>
                             <b>  <asp:LinkButton ID="lnkAppendix4" runat="server" Text="Appendix 4" OnClick="lnkAppendix4_Click"></asp:LinkButton></b>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:30px;"></td>
                            <td>
                              <b> <asp:LinkButton ID="lnkAppendix5" runat="server" Text="Appendix 5" OnClick="lnkAppendix5_Click"></asp:LinkButton></b>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                              <b>  <asp:LinkButton ID="lnkUserManual" runat="server" Text="SIP User Manual" OnClick="lnkUserManual_Click"></asp:LinkButton></b>
                            </td>
                        </tr>
                    </table>                   
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
