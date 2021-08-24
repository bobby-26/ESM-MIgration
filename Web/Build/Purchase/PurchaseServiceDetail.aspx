<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseServiceDetail.aspx.cs"
    Inherits="PurchaseServiceDetail" %>


<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Custom" Src="~/UserControls/UserControlEditor.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Item Details</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
         <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmPlannedMaintenanceComponentDetail" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager" runat="server"></telerik:RadSkinManager>    
    
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip runat="server" ID="Tab" TabStrip="true" Title="Job Description" />
       
        <div>
            <table width ="100%">
                <tr>
                    <td>
                        <telerik:RadEditor ID="txtComponentDetail" runat="server" Width="100%" Height="400px" RenderMode="Lightweight" SkinID="DefaultSetOfTools">
                                <ImageManager ViewPaths="~/Attachments/Purchase/Editor"
                                    UploadPaths="~/Attachments/Purchase/Editor"
                                    EnableAsyncUpload="true"></ImageManager>
                            </telerik:RadEditor>
                    </td>
                </tr>
            </table>
        </div>
        <eluc:Status runat="server" ID="ucStatus" />
    
    </form>
</body>
</html>
