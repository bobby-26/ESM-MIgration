<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewPersonalDetails.aspx.cs"
    Inherits="CrewPersonalDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="../UserControls/UserControlNationality.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Sex" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZone.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaritalStatus" Src="../UserControls/UserControlMaritalStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="OwnerPool" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPool.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirmation" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PreSeaBatch" Src="~/UserControls/UserControlPreSeaBatch.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewMainPersonel" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader">
            <eluc:Title runat="server" ID="ttlCrewMainPersonel" Text="Passenger Details" ShowMenu="false">
            </eluc:Title>
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="CrewMainPersonal" runat="server" OnTabStripCommand="CrewMainPersonal_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <table cellpadding="2" cellspacing="2" width="100%">
            <tr>
                <td>
                            <asp:Literal ID="lblFirstName" runat="server" Text="First Name"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtFirstname" runat="server" CssClass="input" MaxLength="200" Enabled="false"></asp:TextBox>
                </td>
                <td>
                            <asp:Literal ID="lblMiddleName" runat="server" Text="Middle Name"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtMiddlename" runat="server" CssClass="input" MaxLength="200" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                            <asp:Literal ID="lblLastName" runat="server" Text="Last Name"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtLastname" runat="server" CssClass="input" MaxLength="200" Enabled="false"></asp:TextBox>
                </td>
                <td>
                             <asp:Literal ID="lblGender" runat="server" Text="Gender"></asp:Literal>
                </td>
                <td>
                    <eluc:Sex ID="ucSex" runat="server" AppendDataBoundItems="true" Enabled="false" CssClass="dropdown" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblDateofBirth" runat="server" Text="Date of Birth"></asp:Literal>
                </td>
                <td>
                    <eluc:Date ID="txtDateofBirth" runat="server" CssClass="input" Enabled="false" />
                </td>
                <td>
                    <asp:Literal ID="lblAge" runat="server" Text="Age"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtAge" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblNationality" runat="server" Text="Nationality"></asp:Literal>
                </td>
                <td>
                    <eluc:Nationality ID="ucNationality" runat="server" Readonly="true" AppendDataBoundItems="true"
                        CssClass="dropdown readonlytextbox" />
                </td>
                <td>
                    <asp:Literal ID="lblPassportNumber" runat="server" Text="Passport Number"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtPassport" runat="server" MaxLength="200" CssClass="input"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                     <asp:Literal ID="lblIssueDate" runat="server" Text="Issue Date"></asp:Literal>
                </td>
                <td>
                    <eluc:Date ID="ucDateOfIssue" runat="server" CssClass="input" Enabled="false" />
                </td>
                <td>
                    <asp:Literal ID="lblExpiryDate" runat="server" Text="Expiry Date"></asp:Literal>
                    
                </td>
                <td>
                    <eluc:Date ID="ucDateOfExpiry" runat="server" CssClass="input" Enabled="false" />
                    <td>
            </tr>
            <tr>
                <td>
                            <asp:Literal ID="lblRank" runat="server" Text="Rank"></asp:Literal>
                </td>
                <td>
                    <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                        Enabled="false" />
                </td>
               <td>
                            <asp:Literal ID="lblCDCNumber" runat="server" Text="CDC Number"></asp:Literal>
                   
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtCDCNumber" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
