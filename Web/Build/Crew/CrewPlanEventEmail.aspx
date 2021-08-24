<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewPlanEventEmail.aspx.cs" Inherits="CrewPlanEventEmail" %>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Email</title>
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
                width: 697px !important;
                height: 170px !important;
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
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmOptionEmail" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
            <Windows>
                <telerik:RadWindow ID="RadWindow1" runat="server" Width="600px" Height="600px">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="EmailMenu" runat="server" OnTabStripCommand="EmailMenu_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%" Width="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table id="tblMain">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEmailTo" runat="server" Text="Email To:"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rblEmailTo" runat="server" Layout="Flow" Columns="2" Direction="Horizontal"
                            AutoPostBack="true" OnSelectedIndexChanged="rblEmailTo_SelectedIndexChanged">
                            <Items>
                                <telerik:ButtonListItem Value="1" Text="Agent" />
                                <telerik:ButtonListItem Value="2" Text="Master" Selected="true" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
                <tr>

                    <td>&nbsp;<telerik:RadLabel ID="lblPriority" runat="server" Text="Priority"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlPriority" Width="100px" runat="server" Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                            <Items>
                                <telerik:RadComboBoxItem Value="0" Text="Normal" Selected="true" />
                                <telerik:RadComboBoxItem Value="1" Text="Low" />
                                <telerik:RadComboBoxItem Value="2" Text="High" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;<telerik:RadLabel ID="lblTo" runat="server" Text="To"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtTO" Width="700px" runat="server"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;<telerik:RadLabel ID="lblCc" runat="server" Text="Cc"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCC" Width="700px" runat="server"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;<telerik:RadLabel ID="lblBcc" runat="server" Text="Bcc"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtBCC" Width="700px" runat="server"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;<telerik:RadLabel ID="lblSubject" runat="server" Text="Subject"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSubject" Width="700px" runat="server"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;<telerik:RadLabel ID="lblAttachments" runat="server" Text="Attachments"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadFileExplorer RenderMode="Lightweight" runat="server" ID="FileExplorer1" Width="698px" Height="200px" DisplayUpFolderItem="false"
                            EnableCopy="false" EnableCreateNewFolder="false" EnableOpenFile="true" OnClientFileOpen="onClientItemSelected">
                            <Configuration EnableAsyncUpload="true"></Configuration>
                        </telerik:RadFileExplorer>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <telerik:RadEditor ID="edtBody" runat="server" Width="99%" RenderMode="Lightweight" SkinID="DefaultSetOfTools">                           
                        </telerik:RadEditor>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
