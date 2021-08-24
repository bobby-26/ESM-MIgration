<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentMangementFormDesignView.aspx.cs" Inherits="DocumentManagement_DocumentMangementFormDesignView" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title id="lbltitle" runat="server"></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/bootstrap/css/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/bootstrap/css/formio.full.min.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/bootstrap/js/formio.full.min.js"></script>

    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/jquery-1.12.4.min.js"></script>
    <style>
        .display-none
        {
            display: none;
        }
    </style>
</telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <div>
            <div class="row mt-4">
                <div class="col-sm-8 offset-sm-2">
                    <div id="formio" class="card card-body bg-light"></div>
                </div>
                <div class="clearfix"></div>
            </div>
            <div class="col-sm-4">
                <div class="card card-body bg-light jsonviewer">
                    <pre id="json" runat="server"></pre>
                    <pre id="dataJson" runat="server"></pre>
                    <asp:HiddenField ID="hdnFormId" runat="server" />
                    <asp:HiddenField ID="hdnReportId" runat="server" />
                    <asp:HiddenField ID="renderJson" runat="server" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
<telerik:RadCodeBlock runat="server" ID="RadCodeBlock2">
<script type="text/javascript">
    var jsonElement = document.getElementById('json');
    var formElement = document.getElementById('formio');
    var subJSON = document.getElementById('subjson');

    window.onload = function () {

        var strjson = json.innerHTML;
        var strdataJson = dataJson.innerHTML;
        var formId = document.getElementById('<%= hdnFormId.ClientID %>').value;

        var SitePath = "<%=Session["sitepath"]%>" + "/PhoenixWebFunctions.aspx";

        var bool = (<%=ViewState["Status"].ToString() %> == '1' ? false : true);

        Formio.createForm(document.getElementById('formio'), JSON.parse(strjson), {readOnly: bool}).then(function (form) {
            form.nosubmit = true;            
            var strJsondata = strReportJson != "" ? strReportJson : JSON.stringify(form._submission.data);
            $.ajax({
                type: 'POST',
                url: SitePath,
                data: "functionname=FBVALUEFETCH|data=" + strJsondata,
                success: function (response) {
                    strJsondata = response
                },
                async: false
            });
            
            form.on('submit', function (submission) {
            
                var formId = document.getElementById('<%= hdnFormId.ClientID %>').value;
                var ReportId = document.getElementById('<%= hdnReportId.ClientID %>').value;
                AjxPost("functionname=REPORTINSERT|FormId=" + formId + "|strjson=" + JSON.stringify(submission) + "|Reportid=" + ReportId, SitePath, null, false);                    
            });
            form.submission = {
                data: strJsondata
            };            
        });
    }

</script>
</telerik:RadCodeBlock>