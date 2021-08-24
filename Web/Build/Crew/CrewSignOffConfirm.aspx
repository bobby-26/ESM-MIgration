<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewSignOffConfirm.aspx.cs"
    Inherits="CrewSignOffConfirm" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCTSignOffReason" Src="~/UserControls/UserControlSignOffReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCNationality" Src="~/UserControls/UserControlNationality.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCRank" Src="~/UserControls/UserControlRank.ascx" %>
<%--<%@ Register TagPrefix="eluc" TagName="UCVessel" Src="~/UserControls/UserControlVessel.ascx" %>--%>
<%@ Register TagPrefix="eluc" TagName="UCVessel" Src="~/UserControls/UserControlCommonVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCSignOn" Src="~/UserControls/UserControlSignOn.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCPort" Src="~/UserControls/UserControlSeaport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCCompany" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCPool" Src="~/UserControls/UserControlPool.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCLQuick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Sign-Off</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript">
            function checkTextAreaMaxLength(textBox, e, length) {

                var mLen = textBox["MaxLength"];
                if (null == mLen)
                    mLen = length;

                var maxLength = parseInt(mLen);
                if (!checkSpecialKeys(e)) {
                    if (textBox.value.length > maxLength - 1) {
                        if (window.event)//IE
                            e.returnValue = false;
                        else//Firefox
                            e.preventDefault();
                    }
                }
            }

            function checkSpecialKeys(e) {
                if (e.keyCode != 8 && e.keyCode != 46 && e.keyCode != 37 && e.keyCode != 38 && e.keyCode != 39 && e.keyCode != 40)
                    return false;
                else
                    return true;
            }
        </script>
        <script type="text/javascript">
            function confirm(args) {
                if (args) {
                    __doPostBack("<%=confirm.UniqueID %>", "");
            }
        }
        </script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSignOff" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
     
            <asp:Button ID="confirm" runat="server" Text="confirm" OnClick="btnSignOff_Click" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>


            <eluc:TabStrip ID="MenuSignOff" runat="server" OnTabStripCommand="CrewSignOff_TabStripCommand"></eluc:TabStrip>

            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeCode" runat="server" Text="File No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployeeCode" runat="server" CssClass="readonlytextbox" ReadOnly="True"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeName" runat="server" Text="Employee"></telerik:RadLabel>

                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtEmployeeName" runat="server" MaxLength="50" CssClass="readonlytextbox"
                            ReadOnly="True" Width="200px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRank" runat="server" MaxLength="20" CssClass="readonlytextbox"
                            ReadOnly="True">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVessel" runat="server" MaxLength="20" CssClass="readonlytextbox"
                            ReadOnly="True">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSignedOn" runat="server" Text="Signed On"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSignedOn" runat="server" MaxLength="20" CssClass="readonlytextbox"
                            ReadOnly="True">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBeginToD" Text="Begin ToD" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtBtoD" runat="server" MaxLength="20" CssClass="readonlytextbox"
                            ReadOnly="True">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReliefDue" runat="server" Text="Relief Due"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtReliefDue" runat="server" MaxLength="20" CssClass="readonlytextbox"
                            ReadOnly="True">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <hr />
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSignOffDate" runat="server" Text="SignOff Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtSignOffDate" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStandByDate" runat="server" Text="Standy By Date"></telerik:RadLabel>
                    </td>
                    <td colspan="1">
                        <eluc:Date ID="txtStandByDate" runat="server"  />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPort" runat="server" Text="Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UCPort ID="ddlPort" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLeaveStartDate" runat="server" Text="Leave Start Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtLeaveStartDate" runat="server" CssClass="readonlytextbox" Enabled="false" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLeaveCompletionDate" runat="server" Text="Leave Completion Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtLeaveCompletionDate" runat="server"  />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSignOffReason" runat="server" Text="Sign Off Reason"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UCTSignOffReason ID="ddlSignOffReason" runat="server" AppendDataBoundItems="true"
                            CssClass="dropdown_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReleiver" runat="server" Text="Releiver"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UCSignOn ID="ddlSignOn" runat="server" AppendDataBoundItems="true"  />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDateofAvailability" runat="server" Text="Date of Availabiltiy"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDOA" runat="server"  />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDOAGivenDate" runat="server" Text="DOA Given Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDOAGivenDate" runat="server"  />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEndToD" runat="server" Text="End ToD"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtEndToD" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                            OnTextChangedEvent="OnEtodClick" />
                    </td>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblAllowEndTravel" runat="server" Text="Allow end travel and s/off on same date"></telerik:RadLabel>
                        <asp:CheckBox ID="chkEndTravel" runat="server" AutoPostBack="true" OnCheckedChanged="OnEndTravelClick" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSignOffRemarks" runat="server"  TextMode="MultiLine"
                            onkeyDown="checkTextAreaMaxLength(this,event,'200');" Width="300px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <telerik:RadTextBox ID="txtSignOnOffId" runat="server" MaxLength="6"  Visible="false"></telerik:RadTextBox>
            <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
           
       

       <%-- <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />--%>
    </form>
</body>
</html>
