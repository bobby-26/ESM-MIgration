<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementDocumentGeneral.aspx.cs" Inherits="DocumentManagementDocumentGeneral" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Custom" Src="~/UserControls/UserControlEditor.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Document View</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/Fonts/fontawesome/css/all.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript">
            function OnSectionClick(event) {
                //var ifMoreInfo = parent.document.getElementById("ifMoreInfo");
                //var divDocumentCategory = parent.document.getElementById("divDocumentCategory");
                //window.parent.resizeFrameFromIframe(ifMoreInfo);
                //window.parent.resizeFrameFromIframe(divDocumentCategory);
                //jQuery(window.parent).scrollTop();
                //window.parent.document.body.scrollTop(0);
                //window.parent.location.href = "#topdiv";
            }
            function printPreviewDiv(elementId) {
                var printContent = elementId;
                var windowUrl = 'about:blank';
                var uniqueName = new Date();
                var windowName = 'Print' + uniqueName.getTime();
                var printWindow = window.open(windowUrl, windowName, 'left=50000,top=50000,width=0,height=0');
                var printPreviewObject = '<object id="printPreviewElement" width="0" height="0" classid="CLSID:8856F961-340A-11D0-A96B-00C04FD705A2"></object>';

                printWindow.document.write(printContent.innerHTML);
                printWindow.document.write(printPreviewObject);
                printWindow.document.write('<script language=JavaScript>');
                printWindow.document.write('printPreviewElement.ExecWB(7, 2);');
                printWindow.document.write('printPreviewElement.outerHTML = "";');
                printWindow.document.write('<' + '/script>');
                printWindow.document.close();
                printWindow.focus();
                printWindow.close();
            }
        </script>
        <style type="text/css">
            body {
                line-height: 20px !important;
            }
            p {
                margin: 0;
                padding: 0;
                text-align: left;
                max-width: 950px;
                max-width: 900px\9;
                display: block;
            }

            table {
                table-layout: fixed; /*border: 1px solid #f00;*/
                word-wrap: break-word;
                max-width: 950px;
                max-width: 900px\9;
                margin: 0;
                padding: 0;
            }

            img {
                table-layout: fixed;
                max-width: 950px;
                max-width: 900px\9;
                margin: 0;
                padding: 0;
                display: block;
            }

            span {
                margin: 0;
                padding: 0;
                word-wrap: break-word;
                max-width: 950px;
                max-width: 900px\9;
                *zoom: 1;
                *display: inline;
            }

                span span {
                    margin: 0;
                    padding: 0;
                    word-wrap: break-word;
                    max-width: 950px;
                    max-width: 900px\9;
                    *zoom: 1;
                    *display: inline;
                }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form runat="server" id="form1">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager2" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <%--    <asp:UpdatePanel runat="server" ID="pnlInvoice">
        <ContentTemplate>--%>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        <eluc:TabStrip ID="MenuClose" runat="server" OnTabStripCommand="MenuClose_TabStripCommand" TabStrip="false" Title="View"></eluc:TabStrip>

        <%--<img id="imgSearch" runat="server" src="<%$ PhoenixTheme:images/icon_print.png %>" onclick="printPreviewDiv(document.getElementById('divForm'));return false;"
                    style="cursor: pointer; vertical-align: middle; padding-bottom: 3px; width:25px; height:25px;" />--%>

        <%--        </ContentTemplate>
    </asp:UpdatePanel>--%>
        <br />
        <br />
        <br />
        <div id="divForm" runat="server" style="padding-left: 2%; display: inline-block; width: 100%; table-layout: fixed;">
            <%--<span id="span1" runat="server" title="View" style="display: inline-block;"></span>--%>
        </div>
        <br />
        <br />
    </form>
</body>
</html>
