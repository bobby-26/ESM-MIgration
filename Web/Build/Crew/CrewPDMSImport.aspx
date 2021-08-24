<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewPDMSImport.aspx.cs" Inherits="Crew_CrewPDMSImport" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PDMS Import</title>

    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInvoiceAdminPage" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

        <eluc:Status runat="server" ID="ucStatus" />

        <eluc:TabStrip ID="MenuPDMSImort" runat="server" OnTabStripCommand="MenuPDMSImort_OnTabStripCommand"></eluc:TabStrip>

        <table width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblSelectType" runat="server" Text="Select Type"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadRadioButtonList ID="rblType" runat="server" Direction="Horizontal">
                        <Items>
                            <telerik:ButtonListItem Value="1" Text="Personal" Selected="true" />
                            <telerik:ButtonListItem Value="2" Text="Family" />
                        </Items>

                    </telerik:RadRadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblChooseafile" runat="server" Text="Choose a file"></telerik:RadLabel>
                </td>
                <td>
                      
                    <asp:FileUpload ID="FileUpload" runat="server" CssClass="input" />
                </td>
            </tr>
           
        </table>

    </form>
</body>
</html>
