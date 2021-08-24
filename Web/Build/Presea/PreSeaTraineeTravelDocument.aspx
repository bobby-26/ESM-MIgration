<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaTraineeTravelDocument.aspx.cs" Inherits="PreSeaTraineeTravelDocument" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Flag" Src="../UserControls/UserControlFlag.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ECNR" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="DocumentType" Src="~/UserControls/UserControlDocumentType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
     <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
</telerik:RadCodeBlock>
</head>
<body>
    <form id="frmTravelDocument" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlTravelDocument">
        <ContentTemplate>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader">
                    <eluc:Title runat="server" ID="TravelDocument" Text="Trainee Travel Document" ShowMenu="false">
                    </eluc:Title>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="CrewPassPort" runat="server" OnTabStripCommand="CrewPassPort_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <table width="100%" cellpadding="1" cellspacing="1">
                    <tr>
                        <td>
                            First Name
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            Middle Name
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtMiddleName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            Last Name
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtLastName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Employee Number
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            Rank
                        </td>
                        <td colspan="3">
                            <asp:TextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <td colspan="6">
                                <a id="cdcChecker" runat="server" target="_blank">"Indian CDC" Checker</a>
                            </td>
                        </td>
                    </tr>
                </table>
                <hr />
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td>
                            <b>Passport Detail</b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Passport Number
                        </td>
                        <td>
                            <asp:TextBox ID="txtPassportnumber" runat="server" CssClass="input_mandatory upperCase"></asp:TextBox>
                            <asp:Image ID="imgPPFlag" runat="server" Visible="false" />
                        </td>
                        <td>
                            Date of Issue
                        </td>
                        <td>
                            <eluc:Date ID="ucDateOfIssue" runat="server" CssClass="input_mandatory" />
                        </td>
                        <td>
                            Date of Expiry
                        </td>
                        <td>
                            <eluc:Date ID="ucDateOfExpiry" runat="server" CssClass="input_mandatory" />
                            <asp:Image ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" ID="imgPPClip" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Place of Issue
                        </td>
                        <td>
                            <asp:TextBox ID="txtPlaceOfIssue" runat="server" CssClass="input_mandatory"></asp:TextBox>
                        </td>
                        <td>
                            ECNR
                        </td>
                        <td>
                            <eluc:ECNR ID="ucECNR" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                        </td>
                        <td>
                            Minimum 3 Blank Pages
                        </td>
                        <td>
                            <eluc:ECNR ID="ucBlankPages" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                ShortNameFilter="S,N" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b>Seaman's Book Detail</b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Seaman's Book Number
                        </td>
                        <td>
                            <asp:TextBox ID="txtSeamanBookNumber" runat="server" CssClass="input_mandatory"></asp:TextBox>
                            <asp:Image ID="imgCCFlag" runat="server" Visible="false" />
                        </td>
                        <td>
                            Flag
                        </td>
                        <td>
                            <eluc:Flag ID="ucSeamanCountry" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" />
                        </td>
                        <td>
                            Date of Issue
                        </td>
                        <td>
                            <eluc:Date ID="ucSeamanDateOfIssue" runat="server" CssClass="input_mandatory" />
                            <asp:Image ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" ID="imgCCClip" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Date of Expiry
                        </td>
                        <td>
                            <eluc:Date ID="ucSeamanDateOfExpiry" runat="server" CssClass="input_mandatory" />
                        </td>
                        <td>
                            Place of Issue
                        </td>
                        <td>
                            <asp:TextBox ID="txtSeamanPlaceOfIssue" runat="server" CssClass="input_mandatory"></asp:TextBox>
                        </td>
                        <td>
                            Verified Y/N
                        </td>
                        <td>
                            <asp:CheckBox ID="chkVerifiedYN" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Verified By
                        </td>
                        <td>
                            <asp:TextBox ID="txtVerifiedBy" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            Verified On
                        </td>
                        <td>
                            <asp:TextBox ID="txtVerifiedOn" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                            <b>US Visa</b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Type
                        </td>
                        <td>
                            <asp:TextBox ID="txtUSVisaType" runat="server" MaxLength="50" CssClass="input"></asp:TextBox>
                            <asp:Image ID="imgUSVisa" runat="server" Visible="false" />
                        </td>
                        <td>
                            Number
                        </td>
                        <td>
                            <asp:TextBox ID="txtUSVisaNumber" runat="server" CssClass="input" MaxLength="50"></asp:TextBox>
                        </td>
                        <td>
                            Issued On
                        </td>
                        <td>
                            <eluc:Date ID="txtUSVisaIssuedOn" runat="server" CssClass="input" />
                            <asp:Image ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" ID="imgUSVisaClip"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Date of Expiry
                        </td>
                        <td>
                            <eluc:Date ID="txtUSDateofExpiry" runat="server" CssClass="input" />
                        </td>
                        <td>
                            Place of Issue
                        </td>
                        <td>
                            <asp:TextBox ID="txtUSPlaceOfIssue" runat="server" CssClass="input" MaxLength="200"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                            <b>MCV(Australia)</b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Tx/No
                        </td>
                        <td>
                            <asp:TextBox ID="txtMCVNumber" runat="server" CssClass="input" MaxLength="50"></asp:TextBox>
                            <asp:Image ID="imgMCV" runat="server" Visible="false" />
                        </td>
                        <td>
                            Issued On
                        </td>
                        <td>
                            <eluc:Date ID="txtMCVIssuedOn" runat="server" CssClass="input" />
                        </td>
                        <td>
                            Date of Expiry
                        </td>
                        <td>
                            <eluc:Date ID="txtMCVDateofExpiry" runat="server" CssClass="input" />
                            <asp:Image ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" ID="imgMCVClip" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Remarks
                        </td>
                        <td>
                            <asp:TextBox ID="txtMCVRemarks" runat="server" CssClass="input" MaxLength="200"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <hr />
                <table cellpadding="1" cellspacing="1">
                    <tr>
                        <td>
                            <img id="Img1" src="<%$ PhoenixTheme:images/red.png%>" runat="server" />
                        </td>
                        <td>
                            * Documents Expired
                        </td>
                        <td>
                            <img id="Img2" src="<%$ PhoenixTheme:images/yellow.png%>" runat="server" />
                        </td>
                        <td>
                            * Documents Expiring in 120 Days
                        </td>
                    </tr>
                </table>
                <eluc:Status runat="server" ID="ucStatus" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
