﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementDocumentSectionNewContentView.aspx.cs" Inherits="DocumentManagement_DocumentManagementDocumentSectionNewContentView" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CustomEditor" Src="~/UserControls/UserControlEditor.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Preview Mode</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style type="text/css">
            div.ajax__htmleditor_attachedpopup_default {
                border-color: #B0B0B0;
                border-style: solid;
                border-width: 1px;
                font-family: Arial;
                font-size: 11px;
                padding: 5px;
                background-color: #DFDFDF;
                height: 75px;
                width: 258px;
            }

            .ajax__htmleditor_attachedpopup_default label, button, div, input, select, td, fieldset {
                font-family: Tahoma;
                font-size: 11px;
            }

            .ajax__htmleditor_attachedpopup_default div.ajax__htmleditor_popup_confirmbutton {
                float: right;
                margin-left: 5px;
                margin-top: 5px;
            }

            .ajax__htmleditor_attachedpopup_default div.ajax__htmleditor_popup_boxbutton {
                background-color: #C2C2C2;
                border-width: 0;
                margin: 0;
                padding: 1px;
            }

            .ajax__htmleditor_attachedpopup_default .ajax__htmleditor_popup_bgibutton {
                background: url(<%=HttpContext.Current.Session["images"] + "/popupbtn_bg.png"%>) repeat-x scroll 0 0 transparent;
                border-width: 0;
                font-weight: bold;
                height: 19px;
                overflow: hidden;
                text-align: center;
                vertical-align: middle;
                border-color: #B0B0B0;
                border-style: solid;
                border-width: 1px;
                width: 60px;
            }
        </style>
        <script language="javascript" type="text/javascript">

          <%--  function show(e) {
                document.getElementById("<%=dv.ClientID%>").style.display = "block";            
             showFloatDiv(e);
         }

         function hide() {
             document.getElementById("<%=dv.ClientID%>").style.display = "none";          
         }--%>

         <%--function showFloatDiv(e) {
             if (!e) {
                 e = window.event || arguments.callee.caller.arguments[0];
             }
             var scrolledV = scrollV();
             var scrolledH = (navigator.appName == 'Netscape') ? document.body.scrollLeft : document.body.scrollLeft;
             tempX = (navigator.appName == 'Netscape') ? e.clientX : event.clientX;
             tempY = (navigator.appName == 'Netscape') ? e.clientY : event.clientY;
             var obj = document.getElementById("<%=dv.ClientID%>");
             var x = ((obj.offsetWidth + tempX) - browserWidth());
             obj.style.left = (tempX + scrolledH) - (x > 0 ? x : 0) + 'px';
             obj.style.top = (tempY + scrolledV) + 'px';
             obj.style.display = "block";
         }--%>
         function browserWidth() {
             var wth;
             if (window.innerWidth) {
                 wth = window.innerWidth;
             }
             else if (document.documentElement && document.documentElement.clientWidth) {
                 wth = document.documentElement.clientWidth;
             }
             else if (document.body) {
                 wth = document.body.clientWidth;
             }
             return wth;
         }
         function scrollV() {
             var scrolledV;
             if (window.pageYOffset) {
                 scrolledV = window.pageYOffset;
             }
             else if (document.documentElement && document.documentElement.scrollTop) {
                 scrolledV = document.documentElement.scrollTop;
             }
             else if (document.body) {
                 scrolledV = document.body.scrollTop;
             }
             return scrolledV;
         }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" ShowChooser="false" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
         <eluc:TabStrip ID="MenuSave" runat="server" OnTabStripCommand="MenuSave_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        <script type="text/javascript" src="../js/PhoenixDMS.js"></script>

        <telerik:RadEditor ID="ucEditor" RenderMode="Lightweight" runat="server" EnableTrackChanges="true" Width="100%" Height="93%" EmptyMessage="" ToolsFile="~/Content/DMS-Editor-Preview.xml"
            OnClientLoad="TelerikDemo.toggleTrackChanges" EnableComments="true" ContentFilters="DefaultFilters, PdfExportFilter, ConvertInlineStylesToAttributes"  OnExportContent="RadEditor1_ExportContent"
            SkinID="WordLikeExperience" ToolbarMode="RibbonBar" EditModes="Design" ImageManager-EnableImageEditor="true" Enabled="false">
            <ExportSettings OpenInNewWindow="true"></ExportSettings>
            <TrackChangesSettings CanAcceptTrackChanges="true" UserCssId="reU0" />
        </telerik:RadEditor>  
        <telerik:RadWindow RenderMode="Lightweight" ID="RadWindow1" runat="server" Title="Upload File" Behaviors="Close, Move" VisibleStatusbar="false" AutoSize="true">
            <ContentTemplate>
                <div style="width: 350px">
                    Upload a DOCX, DOC or RTF file to load its content for editing. You can also use plain text (.txt) or HTML (.htm, .html) files and content.<br />
                    <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" ID="RadAsyncUpload1" AllowedFileExtensions="doc,docx,rtf,txt,htm,html,md" MaxFileInputsCount="1"
                        OnClientValidationFailed="TelerikDemo.OnClientValidationFailed" OnClientFileUploaded="TelerikDemo.uploadFile"
                        OnFileUploaded="RadAsyncUpload1_FileUploaded" Width="100%">
                    </telerik:RadAsyncUpload>
                </div>
            </ContentTemplate>
        </telerik:RadWindow>
    </form>
</body>
</html>

