<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaNewApplicantPersonal.aspx.cs"
    Inherits="PreSeaNewApplicantPersonal" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
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
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirmation" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PreSeaBatch" Src="~/UserControls/UserControlPreSeaBatch.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PreSeaCourse" Src="~/UserControls/UserControlPreSeaCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PreSea New Applicant Personal</title>
   <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

    </telerik:RadCodeBlock>

    <script type="text/javascript">
        function showBMI() {
            var bmi = document.getElementById("divBMI");
            bmi.style.display = "block";
        }
    </script>

</head>
<body>
    <form id="frmPreSeaMainPersonel" runat="server" autocomplete="off">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="subHeader">
                <eluc:Title runat="server" ID="ttlPreSeaMainPersonel" Text="Applicant Personal Details"
                    ShowMenu="false"></eluc:Title>
            </div>
            <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                <eluc:TabStrip ID="PreSeaMainPersonal" runat="server" OnTabStripCommand="PreSeaMainPersonal_TabStripCommand"></eluc:TabStrip>
            </div>
            <table cellpadding="2" cellspacing="4" width="100%">
                <tr>
                    <td>First Name
                    </td>
                    <td>
                        <asp:TextBox ID="txtFirstname" runat="server" CssClass="input_mandatory" MaxLength="200"></asp:TextBox>
                    </td>
                    <td>Middle Name
                    </td>
                    <td>
                        <asp:TextBox ID="txtMiddlename" runat="server" CssClass="input" MaxLength="200"></asp:TextBox>
                    </td>
                    <td>Last Name
                    </td>
                    <td>
                        <asp:TextBox ID="txtLastname" runat="server" CssClass="input" MaxLength="200"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>File No
                    </td>
                    <td>
                        <asp:TextBox ID="txtFileNo" runat="server" CssClass="readonlytextbox" MaxLength="200"
                            Width="75px"></asp:TextBox>
                    </td>
                    <td>Entrance Roll No
                    </td>
                    <td>
                        <asp:TextBox ID="txtRollno" runat="server" CssClass="readonlytextbox" MaxLength="200"></asp:TextBox>
                    </td>
                    <td>Gender
                    </td>
                    <td>
                        <eluc:Sex ID="ucSex" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>Date of Birth
                    </td>
                    <td>
                        <eluc:Date ID="txtDateofBirth" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td>Place of Birth
                    </td>
                    <td>
                        <asp:TextBox ID="txtPlaceofBirth" runat="server" CssClass="input"></asp:TextBox>
                    </td>
                    <td>Civil Status
                    </td>
                    <td>
                        <eluc:MaritalStatus ID="ucMaritialStatus" runat="server" AppendDataBoundItems="true"
                            CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td>Age
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtAge" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td>Nationality
                    </td>
                    <td>
                        <eluc:Nationality ID="ucNationality" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                    </td>
                    <td>Seaman's Book Number
                    </td>
                    <td>
                        <asp:TextBox ID="txtSeamenBookNumber" runat="server" MaxLength="200" CssClass="readonlytextbox"
                            ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Passport Number
                    </td>
                    <td>
                        <asp:TextBox ID="txtPassport" runat="server" MaxLength="200" CssClass="readonlytextbox"
                            ReadOnly="true"></asp:TextBox>
                    </td>
                    <td>Height(Cms)
                    </td>
                    <td>
                        <eluc:Number ID="txtHeight" runat="server" CssClass="input_mandatory txtNumber" MaxLength="5"
                            IsInteger="true" AutoPostBack="true" />
                    </td>
                    <td>Weight(Kg)
                    </td>
                    <td>
                        <eluc:Number ID="txtWeight" runat="server" CssClass="input_mandatory txtNumber" MaxLength="6"
                            IsInteger="true" AutoPostBack="true" />
                    </td>
                </tr>
                <tr>
                    <td>Eye Sight
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlEyeSight" runat="server" CssClass="input_mandatory">
                            <asp:ListItem Text="Normal" Value="1"> </asp:ListItem>
                            <asp:ListItem Text="Short-Sighted" Value="2"> </asp:ListItem>
                            <asp:ListItem Text="Long-Sighted" Value="3"> </asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>Colour Blindness
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlColourBlindness" runat="server" CssClass="input_mandatory">
                            <asp:ListItem Text="Yes" Value="1"> </asp:ListItem>
                            <asp:ListItem Text="No" Value="0"> </asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>Distinguish Mark
                    </td>
                    <td>
                        <asp:TextBox ID="txtDistinguishMark" CssClass="input_mandatory" runat="server" MaxLength="200"
                            Height="50" Width="200" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Major illness/operations undergone before
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlIllnessYN" runat="server" CssClass="input_mandatory" AutoPostBack="true">
                            <asp:ListItem Text="Yes" Value="1"> </asp:ListItem>
                            <asp:ListItem Text="No" Value="0"> </asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>Major illness/operations Description
                    </td>
                    <td>
                        <asp:TextBox ID="txtIllnessDesc" CssClass="input" runat="server" MaxLength="200"
                            Height="50" Width="200"></asp:TextBox>
                    </td>
                    <td>
                        Blood Group
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox1" CssClass="input" runat="server" MaxLength="200"
                            Height="50" Width="200"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                     <td>Family Relation to Company
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlRelationToCompany" runat="server" CssClass="input_mandatory"
                            AutoPostBack="true">
                            <asp:ListItem Text="Yes" Value="1"> </asp:ListItem>
                            <asp:ListItem Text="No" Value="0"> </asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>Family Relation to Company Description
                    </td>
                    <td>
                        <asp:TextBox ID="txtRelationToCompany" CssClass="input" runat="server" MaxLength="200"
                            Height="50" Width="200"></asp:TextBox>
                    </td>
                    <td>Created By
                    </td>
                    <td>
                        <asp:TextBox ID="txtCreatedBy" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                     <td>Applied On
                    </td>
                    <td>
                        <asp:TextBox ID="txtAppliedOn" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td>Course
                    </td>
                    <td>
                        <eluc:PreSeaCourse ID="ucPreSeaCourse" runat="server" CssClass="input_mandatory"
                            AppendDataBoundItems="true" />
                    </td>
                    <td>Applied Batch
                    </td>
                    <td>
                        <eluc:PreSeaBatch ID="ucBatch" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" IsCalledFromPresea="1" />
                    </td>

                </tr>
                <tr>
                    <td>Territory Code
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rblTerritoryCode" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="0"> Urban </asp:ListItem>
                            <asp:ListItem Value="1">Rural</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td>Category
                    </td>
                    <td>
                        <eluc:Quick ID="ucCategory" runat="server" AppendDataBoundItems="true" CssClass="input"
                            QuickTypeCode="105" />
                    </td>
                    <td>
                        Indos Number
                    </td>
                    <td>
                        <asp:TextBox ID="txtIndosNo" runat="server" CssClass="readonlytextbox"  ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblPhotograph" runat="server" Text="Photograph"></asp:Literal><br />
                        <a id="aPreSeaImg" runat="server">
                            <asp:Image ID="imgPhoto" runat="server" Height="100px" ImageUrl="<%$ PhoenixTheme:images/Blank.png %>"
                                Width="120px" /></a>
                        <asp:ImageButton ID="imgIDCard" runat="server" ImageUrl="<%$ PhoenixTheme:images/id-card.png %>"
                            ToolTip="ID Card" />
                        <br />
                        <asp:FileUpload ID="txtFileUpload" runat="server" CssClass="input" />
                    </td>
                </tr>
            </table>
            <eluc:UserControlStatus ID="ucStatus" runat="server" />
        </div>
    </form>
</body>
</html>
