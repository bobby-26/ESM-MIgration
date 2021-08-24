<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StandardFormFBformView.aspx.cs"
    Inherits="StandardFormFBformView" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/content/bootstrap.min.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/content/formio.full.min.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/content/phoenixformio.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/scripts/jquery-3.4.1.min.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/scripts/formio.full.min.js"></script>        
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <%--        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>--%>


    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" EnableScriptCombine="false">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server">
        </telerik:RadSkinManager>
        <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" CssClass="hidden" />
        <div>
            <div class="row mt-4">
                <div class="col-sm-8 offset-sm-2">
                    <div id="formio" class="card card-body bg-light">
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="col-sm-4" style="display: none">
                <div class="card card-body bg-light jsonviewer">
                    <pre id="json" runat="server"></pre>
                    <asp:HiddenField ID="hdnFormId" runat="server" />
                    <asp:HiddenField ID="hdnReportId" runat="server" />
                    <asp:HiddenField ID="hdnstatusname" runat="server" />
                    <asp:HiddenField ID="hdnType" runat="server" />

                </div>
            </div>
        </div>
    </form>
    <telerik:RadCodeBlock runat="server" ID="RadCodeBlock2">
        <script type="text/javascript">

            var jsonElement = document.getElementById('json');
            var formElement = document.getElementById('formio');
            var subJSON = document.getElementById('subjson');
            var SitePath = "<%=Session["sitepath"]%>" + "/PhoenixWebFunctions.aspx";
            var revid = '<%=ViewState["REVID"].ToString()%>';
            window.onload = function () {
           // console.log("am in this page");
                var strReportJson = JSON.stringify(<%=ViewState["DATAJSON"].ToString()%>);
                var formId = document.getElementById('<%= hdnFormId.ClientID %>').value;

                var bool = (<%=",3,7,".Contains(","+ViewState["Status"].ToString()+",") ? "1" : "0" %> == '1' ? true : false);

                var ReportId = document.getElementById('<%= hdnReportId.ClientID %>').value;

                var formiojson = <%=ViewState["jsonstring"].ToString()%>;
                var dropdownusername = FormioUtils.searchComponents(formiojson.components, { 'key': 'username' });
                for (var i = 0; i < dropdownusername.length; i++) {
                    var ranklist = dropdownusername[i].properties.rank
                    $.ajax({
                        type: 'POST',
                        url: SitePath,
                        data: "functionname=CREWLISTJSON|param=" + (ranklist == null ? "" : ranklist) + "|reportid=" + ReportId,
                        success: function (response) {
                            dropdownusername[i].data.values = response;
                        },
                        async: false
                    });
                }
                var crewList = FormioUtils.searchComponents(formiojson.components, { 'phoenixtype': 'crewlist' });
                for (var i = 0; i < crewList.length; i++) {
                    var ranklist = crewList[i].properties != null ? crewList[i].properties.rank : null;
                    $.ajax({
                        type: 'POST',
                        url: SitePath,
                        data: "functionname=CREWLISTJSON|param=" + (ranklist == null ? "" : ranklist) + "|reportid=" + ReportId,
                        success: function (response) {
                            if(crewList[i].data != null) {
                                crewList[i].data.values = response;
                            }
                            var defvalue = '<%=ViewState["REFID"].ToString()%>';
                            if (defvalue != '') {
                                crewList[i].defaultValue = defvalue;
                            }
                        },
                        async: false
                    });
                }

                var signature = FormioUtils.searchComponents(formiojson.components, { 'phoenixtype': 'signature' });
                var sign = '';
                var ranklist = ',';
                for (var i = 0; i < signature.length; i++) {
                    var username = FormioUtils.searchComponents(signature[i].columns, { 'phoenixtype': 'crewlist' });
                    if (username.length > 0) {
                        sign += '#' + username[0].key + '~,' + (username[0].properties != null && username[0].properties.rank != null ? username[0].properties.rank : 'All') + ',#`';
                    }
                }
                
                Formio.createForm(formElement, formiojson, { readOnly: bool });
                Formio.createForm(document.getElementById('formio'), formiojson, { readOnly: bool }).then(function (form) {
                    form.nosubmit = true;
                    var param = "";
                    var strJsondata = strReportJson.toString() == "{}" ? JSON.stringify(form._submission.data) : strReportJson;
                    var userFields = FormioUtils.searchComponents(formiojson.components, { 'type': 'textfield' });

                    for (var i = 0; i < userFields.length; i++) {
                        var component = userFields[i];
                        if (component["key"].indexOf("user") >= 0) {
                            param = param + component["key"] + "~" + component["properties"]["rank"] + "`";
                        }
                        if (component["key"].indexOf("revisionNo") >= 0) {
                            param = param + component["key"] + "~" + document.getElementById('<%= hdnFormId.ClientID %>').value + "`";
                        }
                    }
                    if (param.length > 0)
                        param = param.substring(0, param.length - 1);

                    $.ajax({
                        type: 'POST',
                        url: SitePath,
                        data: "functionname=FBVALUEFETCH|data=" + strJsondata + "|param=" + param + "|reportid=" + ReportId,
                        success: function (response) {
                            strJsondata = response
                        },
                        async: false
                    });
                    form.on('submit', function (submission) {
                        submitData(submission, sign);
                    });

                    form.on('Print', function (click) {
                        if (ReportId != null && ReportId != "") {
                            $.ajax({
                                type: 'POST',
                                url: SitePath,
                                data: "functionname=FBPRINT|status=4|reportid=" + ReportId,
                                async: false
                            });
                        }
                        window.print();
                    });

                     form.on('PendingApproval-dutyofficer', function (click) {
                    // var status =  '<%=ViewState["Status"].ToString()%>';
                       if (ReportId != null && ReportId != "") {
                            $.ajax({
                                type: 'POST',
                                url: SitePath,
                                //data: "functionname=APPROVALSTATUS|STATUS="+ status+"|reportid=" + ReportId,
                                data: "functionname=APPROVALSTATUS|status=10|reportid=" + ReportId,
                                async: false
                                
                            });
                        }
                        alert('Sent to DutyOfficer for Approval')
                    });

                      form.on('PendingApproval-dutyengineer', function (click) {
                       if (ReportId != null && ReportId != "") {
                            $.ajax({
                                type: 'POST',
                                url: SitePath,
                                //data: "functionname=APPROVALSTATUS|STATUS="+ status+"|reportid=" + ReportId,
                                data: "functionname=APPROVALSTATUS|status=12|reportid=" + ReportId,
                                async: false
                                
                            });
                        }
                        alert('Sent to DutyEngineer for Approval')
                    });

                     form.on('PendingApproval-ChiefOfficer', function (click) {
                        if (ReportId != null && ReportId != "") {
                            $.ajax({
                                type: 'POST',
                                url: SitePath,
                                //data: "functionname=APPROVALSTATUS|STATUS="+ status+"|reportid=" + ReportId,
                                data: "functionname=APPROVALSTATUS|status=13|reportid=" + ReportId,
                                async: false
                                
                            });
                        }
                        alert('Sent to ChiefOfficer for Approval')
                    });

                      form.on('PendingApproval-Chiefengineer', function (click) {
                    // var status =  '<%=ViewState["Status"].ToString()%>';
                        if (ReportId != null && ReportId != "") {
                            $.ajax({
                                type: 'POST',
                                url: SitePath,
                                //data: "functionname=APPROVALSTATUS|STATUS="+ status+"|reportid=" + ReportId,
                                data: "functionname=APPROVALSTATUS|status=11|reportid=" + ReportId,
                                async: false
                                
                            });
                        }
                        alert('Sent to ChiefEngineer for Approval')
                    });

                        form.on('PendingApproval-Master', function (click) {
                    // var status =  '<%=ViewState["Status"].ToString()%>';
                        if (ReportId != null && ReportId != "") {
                            $.ajax({
                                type: 'POST',
                                url: SitePath,
                                //data: "functionname=APPROVALSTATUS|STATUS="+ status+"|reportid=" + ReportId,
                                data: "functionname=APPROVALSTATUS|status=8|reportid=" + ReportId,
                                async: false
                                
                            });
                        }
                        alert('Sent to Master for Approval')
                    });




                    form.on('Verify', function (click) {
                        var data = JSON.stringify(click);
                        $.ajax({
                            type: 'POST',
                            url: SitePath,
                            data: "functionname=ValidateUser|data=" + data + "|signature="+sign,
                            success: function (response) {
                                form.submission = {
                                    data: response
                                };
                                setTimeout(function () { submitData(form.submission, sign); }, 600);
                            },
                            async: false
                        });
                    });

                    form.submission = {
                        data: strJsondata
                    };
                });
                setTimeout(replaceBase64, 100);
            }
            function replaceBase64() {
                var img = document.querySelectorAll("img");
                img.forEach(function (data) {
                    if (data.src.indexOf("data:image/png;base64,") > -1) {
                        data.src = data.src.replace(/&plus;/g, "+");
                    }
                });
            }
            function submitData(formData, sign) {
                var formId = document.getElementById('<%= hdnFormId.ClientID %>').value;
                var ReportId = document.getElementById('<%= hdnReportId.ClientID %>').value;
                var Type = document.getElementById('<%= hdnType.ClientID %>').value;
                var workorderId = '<%= ViewState["workorderId"].ToString() %>'
                if (formData.metadata != null)
                    formData.metadata.referrer = "";
                $.ajax({
                    type: 'POST',
                    url: SitePath,
                    data: "functionname=REPORTINSERT|FormId=" + formId + "|strjson=" + JSON.stringify(formData) + "|Reportid=" + ReportId + "|Type=" + Type + "|signature=" + sign + "|revid=" + revid,
                    success: function (res) {
                        if (res != null) {
                            res = JSON.parse(res)
                            if (res.reportId != null) {
                                document.getElementById('hdnReportId').value = res.reportId;
                                if (workorderId != null && workorderId != "") {
                                    document.getElementById("<%=btnSubmit.UniqueID %>").click();
                                }
                            }
                            alert('Saved Successfully');
                        }                        
                    },
                    async: false
                });
            }             
        </script>

    </telerik:RadCodeBlock>
</body>
</html>

