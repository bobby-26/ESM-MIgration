<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewCorrespondenceEmail.aspx.cs"
    Inherits="CrewCorrespondenceEmail" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Correspondence Email</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script lang="javascript" type="text/javascript">

            function OnGridRowDataBound(oGrid, args) {
                var dataItem = args.get_dataItem();
                if (dataItem.Length != null) {
                    dataItem.Length = formatSizeValue(dataItem.Length);
                }
            }

            function formatSizeValue(originalSizeValue) {
                if (originalSizeValue > 2000) {
                    var valueInKB = originalSizeValue / 1024;// Convert to MB
                    var newValue = Math.round(valueInKB * 100) / 100
                    return String.format("{0} KB", newValue);
                }

                return String.format("{0}", originalSizeValue);
            }
            function onClientItemSelected(fileExplorer, args) {
                if (args.get_item().get_type() == Telerik.Web.UI.FileExplorerItemType.File) {
                    // Cancel the default dialog;
                    args.set_cancel(true);
                    var filepath = args.get_item().get_path();
                    var names = (filepath).split('/');
                    var filename = names[2];
                    var ext = filename.split('.');

                    window.radopen("../common/FileDownload.aspx?filename=" + filename + "&filepath=" + filepath + "&mod=CREW", "Email Attachment");
                }
                else {
                    // if the item is a folder        
                    alert("The selected item is a directory");
                }
            }
        </script>
        <style>
            .RadFileExplorer .rtPlus {
                display: none !important;
            }

            .RadFileExplorer .rtMinus {
                display: none !important;
            }

            .rspFirstItem {
                display: none !important;
            }

            .rfeAddressBox {
                display: none !important;
            }

            .rfePaneGrid {
                width: 500px !important;
                height: 147px !important;
            }

            .rspResizeBar {
                display: none !important;
            }

            .leftPane {
                padding-right: 10px;
                margin-right: 20px;
            }

            .leftPane, .rightPane {
                float: left;
            }

            .previmage {
                width: 230px;
                height: 220px;
                vertical-align: middle;
            }

            #pvwImage {
                display: none;
                margin: 10px;
                width: 200px;
                height: 180px;
                vertical-align: middle;
            }

            .RadFileExplorer {
                width: 500px !important;
                height: 175px !important;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCorrespondenceEmail" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuCorrespondenceEmail" runat="server" OnTabStripCommand="CorrespondenceEmail_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <%--<asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" Text="cmdHiddenSubmit" />--%>
            <table width="70%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeNumber" runat="server" Text="File No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployeeNumber" runat="server" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtName" runat="server" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRank" runat="server" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <hr />
            <table cellpadding="1" cellspacing="1">
                <tr>
                    <td></td>
                    <td>
                        <telerik:RadLabel ID="lblAttachment" runat="server" Text="Attachments"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table>
                            <tr id="trFrom" runat="server">
                                <td>
                                    <telerik:RadLabel ID="lblFrom" runat="server" Text="From"></telerik:RadLabel>
                                </td>
                                <td colspan="3">
                                    <telerik:RadTextBox ID="txtFrom" Width="500px" runat="server"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr id="trTO" runat="server">
                                <td>
                                    <telerik:RadLabel ID="lblTo" runat="server" Text="To"></telerik:RadLabel>
                                </td>
                                <td colspan="3">
                                    <telerik:RadTextBox ID="txtTO" Width="500px" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr id="trCC" runat="server">
                                <td>
                                    <telerik:RadLabel ID="lblCc" runat="server" Text="Cc"></telerik:RadLabel>
                                </td>
                                <td colspan="3">
                                    <telerik:RadTextBox ID="txtCC" Width="500px" runat="server"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr id="trBCC" runat="server">
                                <td>
                                    <telerik:RadLabel ID="lblBcc" runat="server" Text="Bcc"></telerik:RadLabel>
                                </td>
                                <td colspan="3">
                                    <telerik:RadTextBox ID="txtBCC" Width="500px" runat="server"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblSubject" runat="server" Text="Subject"></telerik:RadLabel>
                                </td>
                                <td colspan="3">
                                    <telerik:RadTextBox ID="txtSubject" Width="500px" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblCorrespondenceType" runat="server" Text="Correspondence Type"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Quick ID="ddlCorrespondenceType" runat="server" QuickTypeCode="11" CssClass="input_mandatory"
                                        AppendDataBoundItems="true" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtDate" runat="server" ReadOnly="true"></telerik:RadTextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <telerik:RadFileExplorer RenderMode="Lightweight" runat="server" ID="FileExplorer1" DisplayUpFolderItem="false"
                                        EnableCopy="false" EnableCreateNewFolder="false" EnableOpenFile="true" OnClientFileOpen="onClientItemSelected">
                                        <Configuration EnableAsyncUpload="true"></Configuration>
                                    </telerik:RadFileExplorer>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <telerik:RadEditor ID="txtBody" runat="server" Width="99%" RenderMode="Lightweight" SkinID="DefaultSetOfTools">
            </telerik:RadEditor>
            <eluc:Status ID="ucStatus" runat="server" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
