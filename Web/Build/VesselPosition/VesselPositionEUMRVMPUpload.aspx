<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionEUMRVMPUpload.aspx.cs"
    Inherits="VesselPositionEUMRVMPUpload" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Splitter" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Attachment</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

    <script type="text/javascript">
        var Template = new Array();
        Template[0] = '<a href="#" id="lnkRemoveFile{counter}" onclick="return removeFile(this);">Remove</a>';
        Template[1] = '<input id="txtFileUpload{counter}" name="txtFileUpload{counter}" type="file" class="input" />';
        Template[2] = 'Choose a file';
        var counter = 1;

        function addFile(description) {           
            counter++;
            var tbl = document.getElementById("tblFiles");
            var rowCount = tbl.rows.length;
            var row = tbl.insertRow(rowCount - 1);
            var cell;

            for (var i = 0; i < Template.length; i++) {
                cell = row.insertCell(0);
                cell.innerHTML = Template[i].replace(/\{counter\}/g, counter).replace(/\{value\}/g, (description == null) ? '' : description);
            }
        }
        function removeFile(ctrl) {
            var tbl = document.getElementById("tblFiles");
            if (tbl.rows.length > 2)
                tbl.deleteRow(ctrl.parentNode.parentNode.rowIndex);
        }
        
    </script>

</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
  <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
       <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="txtFileUpload1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>

    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="AttachmentList" runat="server" OnTabStripCommand="AttachmentList_TabStripCommand">
            </eluc:TabStrip>
        <table width="50%">
            <tr>
                <td>
                   <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel> 
                </td>
                <td>
                    <eluc:Vessel ID="ucVessel" runat="server" CssClass="input_mandatory" SyncActiveVesselsOnly="True" VesselsOnly="true" />
                </td>
            </tr>
            
        </table>
        <br />
        <table id="tblFiles">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblChooseafile" runat="server" Text="Choose a file"></telerik:RadLabel>
                </td>
                <td colspan="2">
                    <%--<asp:FileUpload ID="txtFileUpload1" runat="server" CssClass="input" />--%>
                    <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" ID="txtFileUpload1" MultipleFileSelection="Automatic"
                        DropZones=".DropZone1"  />
                </td>
            </tr>
            <%--<tr>
                <td colspan="3" align="right">
                    <a href="#" onclick="return addFile();">Add File</a>
                </td>
            </tr>--%>
        </table>
        <hr />

    </form>
</body>
</html>
