<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefectTrackerAuthendicateSeafarer.aspx.cs"
    Inherits="DefectTracker_DefectTrackerAuthendicateSeafarer" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title>Authendicate Seafarer</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function GetSeafarerDetails() {
                var vesselid = document.getElementById('<% =ucVessel.FindControl("ddlVessel").ClientID %>').value;
                if (vesselid == "Dummy")
                    return;
                var args = "function=GetSeafarerDetails|vesselid=" + vesselid;
                var arrValue = AjxGet('http://119.81.76.123/phoenix/Options/OptionsAuthenticateSeafarer.aspx?function=GetSeafarerDetails&vesselid=' + vesselid, 'Seafarer', false);

                var valueList = document.getElementById('Seafarer').outerText;
                document.getElementById('Seafarer').style.display = 'none';
                var valueList = valueList.split('|');
                if (arrValue != "") {
                    document.getElementById('<%=lblMasterName.ClientID%>').value = valueList[0];
                 document.getElementById('<%=lblCheifEngineername.ClientID%>').value = valueList[1];
             }
         }
        </script>

        <script type="text/javascript">
            function ValidateSeafarer() {
                var fileno = document.getElementById('<%=txtFileNo.ClientID%>').value;
                var dob = document.getElementById('<%=ucDateofBirth.ClientID%>').value;
                var vesselid = document.getElementById('<% =ucVessel.FindControl("ddlVessel").ClientID %>').value;
                var args = "function=ValidateSeafarer|fileno=" + fileno + "|dob=" + dob + "|vesselid=" + vesselid;
                var arrValue = AjxPost(args, 'http://119.81.76.123/phoenix/Options/OptionsAuthenticateUser.aspx', null, false);

                if (arrValue != "") {
                    var valueList = arrValue.split('|');
                    var seafarer = valueList[0];
                    var filenumber = valueList[1];

                    if (seafarer == 'Error')
                        alert(filenumber)
                    else
                        window.location = SitePath + "DefectTracker/DefectTrackerExportFileDownload.aspx?vesselid=" + vesselid + "&fileno=" + filenumber + "&seafarer=" + seafarer;
                }
            }
        </script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="form1" DecoratedControls="All" />
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <eluc:Error ID="ucError" ErrorMessage="" runat="server" Text="" Visible="false"></eluc:Error>

        <eluc:TabStrip ID="MenuSubmit" runat="server" OnTabStripCommand="MenuAuthendicate_TabStripCommand"></eluc:TabStrip>


        <telerik:RadLabel ID="lblSeafarer" runat="server" />

        <table width="80%">
            <tr>
                <td>
                    <b>Notes :</b>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <font color="blue">
                        <li>Use Employee Number/File Number for username.</li>
                        <li>Use Date of birth for password. The date format should be in dd/mm/yy or dd-mm-yyyy.</li>
                        <li>For eg. 30-09-2013 or 30/09/2013.</li>
                    </font>
                    <br />
                </td>
            </tr>
            <tr>
                <td>Vessel
                </td>
                <td>
                    <eluc:Vessel runat="server" ID="ucVessel" VesselsOnly="true" AppendDataBoundItems="true"
                        CssClass="input" OnTextChangedEvent="ucVessel_TextChanged" AutoPostBack="true" Width="240px" />
                </td>
            </tr>
            <tr>
                <td>Master
                </td>
                <td>
                    <telerik:RadTextBox ID="lblMasterName" Width="240px" CssClass="readonlytextbox" AutoPostBack="true"
                        runat="server" ReadOnly="true">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>Chief Engineer
                </td>
                <td>
                    <telerik:RadTextBox ID="lblCheifEngineername" Width="240px" CssClass="readonlytextbox" AutoPostBack="true"
                        runat="server" ReadOnly="true">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>Username
                </td>
                <td>
                    <telerik:RadTextBox ID="txtFileNo" runat="server" CssClass="input_mandatory" Width="240px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>Password
                </td>
                <td>
                    <telerik:RadTextBox ID="ucDateofBirth" runat="server" TextMode="Password" CssClass="input_mandatory" Width="240px" />
                </td>
            </tr>
        </table>

    </form>
</body>
</html>
