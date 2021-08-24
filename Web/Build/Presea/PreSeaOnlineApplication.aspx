<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaOnlineApplication.aspx.cs"
    EnableViewState="true" Inherits="PreSeaOnlineApplication" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlQuick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZone.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PhoneNumber" Src="../UserControls/UserControlPhoneNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonAddress" Src="../UserControls/UserControlCommonAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Institution" Src="~/UserControls/UserControlPreSeaMultiColAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PreSeaQualificaiton" Src="~/UserControls/UserControlPreSeaQualification.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PreSeaCourse" Src="~/UserControls/UserControlPreSeaCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PreSeaBatch" Src="~/UserControls/UserControlPreSeaBatch.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaritalStatus" Src="../UserControls/UserControlMaritalStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="../UserControls/UserControlNationality.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Sex" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="City" Src="~/UserControls/UserControlCity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="State" Src="~/UserControls/UserControlState.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Relation" Src="../UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PreSea Online Application</title>
   <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

  </telerik:RadCodeBlock>

    <script type="text/javascript">
        function resize() {
            var obj = document.getElementById("ifMoreSSLC");
            if(obj != null)
            obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight)-140 + "px";
            
            obj = document.getElementById("ifMoreHSC"); 
            if(obj != null)
            obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight)-140 + "px";
            
            obj = document.getElementById("idMoreGraduation");
            if(obj != null)
            obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight)-140 + "px";

        }       
    </script>

</head>
<body onload="resize()" onresize="resize()">
    <div style="margin: 0 auto; width: 1024px; text-align: left;">
        <form id="form1" runat="server" autocomplete="off">
        <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
            runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        <table width="100%" style="top: 0px; right: 0px; margin: 0; position: relative;"
            cellpadding="0" cellspacing="0">
            <tr>
                <td align="left" style="width: 20%">
                    <img id="img1" runat="server" alt="" src="<%$ PhoenixTheme:images/sims.png %>" />
                </td>
                <td style="font-size: medium; font-weight: bold; width: 60%;" align="center">
                    SAMUNDRA INSTITUTE OF MARITIME STUDIES
                    <br />
                    <br />
                    ONLINE APPLICATION FORM
                </td>
                <td style="width: 20%" valign="top" align="right">
                    <span style="vertical-align: bottom;">
                        <asp:Label ID="lblWelcome" runat="server" Text="Welcome"></asp:Label>
                        <br />
                        <asp:LinkButton ID="lnkSignOut" runat="server" OnClick="lnkSignOut_Click">Log Out</asp:LinkButton>
                    </span>
                </td>
            </tr>
        </table>
        <hr />
        <asp:UpdatePanel ID="pnlOnlineApplication" runat="server">
            <ContentTemplate>
                <div style="margin: 0 auto; width: 100%">
                    <div class="navAppSelect" style="position: relative; float: left; width: 18%; height: auto;"
                        runat="server">
                        <a id="lnkBasic" runat="server" onserverclick="lnk_serverclick" style="width: auto;
                            font-size: 12px;">Basic Details</a> <a id="lnkSSLC" runat="server" onserverclick="lnk_serverclick"
                                style="width: auto; font-size: 12px;">SSLC / (10th) Details</a> <a id="lnkHSC" runat="server"
                                    onserverclick="lnk_serverclick" style="width: auto; font-size: 12px;">HSC (12th)
                                    / Diploma Details</a> <a id="lnkGraduation" runat="server" onserverclick="lnk_serverclick"
                                        style="width: auto; font-size: 12px;">Graduation Details</a> <a id="lnkFamily" runat="server"
                                            onserverclick="lnk_serverclick" style="width: auto; font-size: 12px;">Family Details</a>
                        <a id="lnkOther" runat="server" onserverclick="lnk_serverclick" style="width: auto;
                            font-size: 12px;">Other Details</a><a id="lnkTerms" runat="server" onserverclick="lnk_serverclick"
                                style="width: auto; font-size: 12px;"> Terms of Application</a><a id="lnkPhoto" runat="server"
                                    onserverclick="lnk_serverclick" style="width: auto; font-size: 12px;">Upload Photo</a>
                        <a id="lnkPayment" runat="server" onserverclick="lnk_serverclick" style="width: auto;
                            font-size: 12px;">Payment</a> <a id="lnkDeclaration" runat="server" onserverclick="lnk_serverclick"
                                style="width: auto; font-size: 12px;">Declaration by the Applicant</a> <a id="lnkVenue"
                                    runat="server" onserverclick="lnk_serverclick" style="width: auto; font-size: 12px;">
                                    SIMS/ESM Field Office</a>
                        <div id="divAdditionalInfo" runat="server">
                            <table>
                                <tr>
                                    <td>
                                        <h3>
                                            Additional Information</h3>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <a id="lnkPersonal" runat="Server" onserverclick="lnk_serverclick" style="width: auto;
                            font-size: 12px;">Personal Details</a><a id="lnkExtra" runat="server" onserverclick="lnk_serverclick"
                                style="width: auto; font-size: 12px;">Extra Curicular Activities</a> <a id="lnkFamily1"
                                    runat="server" onserverclick="lnk_serverclick" style="width: auto; font-size: 12px;">
                                    Family details</a>
                        <div id="divCompletion" runat="server">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblPercentageCompletion" runat="server"></asp:Label>
                                        <b>% completed.</b>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div style="width: 82%; float: left; position: relative;">
                        <div id="divBasic" runat="server" style="width: 100%; position: absolute;">
                            <table cellpadding="2" cellspacing="3" width="100%">
                                <tr>
                                    <td>
                                        Course
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlCourse" DataTextField="FLDPRESEACOURSENAME" CssClass="dropdown_mandatory"
                                            AutoPostBack="true" DataValueField="FLDPRESEACOURSEID" runat="server" OnSelectedIndexChanged="Course_Changed">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        The Batch you wish to Apply
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlBatch" runat="server" DataTextField="FLDBATCH" DataValueField="FLDBATCHID"
                                            CssClass="dropdown_mandatory">
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="2">
                                        <asp:LinkButton ID="lnkAlready" runat="server" OnClick="lnkAlready_Click">Click here, if already registered.</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        First Name
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFirstname" runat="server" CssClass="input_mandatory" MaxLength="200"></asp:TextBox>
                                    </td>
                                    <td>
                                        Middle Name
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMiddlename" runat="server" CssClass="input" MaxLength="200"></asp:TextBox>
                                    </td>
                                    <td>
                                        Last Name
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLastname" runat="server" CssClass="input" MaxLength="200"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Date of Birth
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtDateofBirth" runat="server" CssClass="input_mandatory" DatePicker="true" />
                                    </td>
                                    <td>
                                        Gender
                                    </td>
                                    <td>
                                        <eluc:Sex ID="ucGender" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                                            Enabled="false" HardList='<%# PhoenixRegistersHard.ListHard(1,(int)PhoenixHardTypeCode.SEX)%>' />
                                    </td>
                                    <td>
                                        Nationality
                                    </td>
                                    <td>
                                        <eluc:Nationality ID="ucNationality" runat="server" AppendDataBoundItems="true" CssClass="input"
                                            Readonly="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Choose your highest Qualification
                                    </td>
                                    <td>
                                        <eluc:PreSeaQualificaiton ID="ucHighestQualificaiton" runat="server" CssClass="input_mandatory"
                                            AppendDataBoundItems="true" />
                                    </td>
                                    <td>
                                        EMail
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEmail" CssClass="input_mandatory" runat="server" MaxLength="200"
                                            Width="185px"></asp:TextBox>
                                    </td>
                                    <td colspan="1" align="right">
                                        <font color="blue">(Use "," to add more E-Mail Eg : (xx@xx.com,yy@yy.com)</font>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Contact No
                                    </td>
                                    <td colspan="2">
                                        <eluc:PhoneNumber ID="txtContact" runat="server" CssClass="input_mandatory" />
                                    </td>
                                </tr>
                            </table>
                            <hr />
                            <table cellpadding="1" cellspacing="1" width="100%">
                                <tr>
                                    <td>
                                        Colour Blindness
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="ddlColourBlindness" runat="server" RepeatDirection="Horizontal"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlColourBlindness_Changed">
                                            <asp:ListItem Text="Yes" Value="1"> </asp:ListItem>
                                            <asp:ListItem Text="No" Value="0" Selected="True"> </asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td>
                                        Eye Sight
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlEyeSight" runat="server" CssClass="input_mandatory">
                                            <asp:ListItem Text="Normal" Value="1"> </asp:ListItem>
                                            <asp:ListItem Text="Short-Sighted" Value="2"> </asp:ListItem>
                                            <asp:ListItem Text="Long-Sighted" Value="3"> </asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Major illness/</br> operations undergone before
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="ddlIllnessYN" runat="server" RepeatDirection="Horizontal"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlIllnessYN_Changed">
                                            <asp:ListItem Text="Yes" Value="1"> </asp:ListItem>
                                            <asp:ListItem Text="No" Value="0"> </asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td>
                                        Major illness/</br>operations Description, if any
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIllnessDesc" CssClass="input" runat="server" Height="50" Width="200"
                                            TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr visible="false" runat="server">
                                </tr>
                                <tr>
                                    <td colspan="6" align="right">
                                        <asp:Button ID="btnBasicSave" runat="server" CommandName="Basic" Text="Save" CssClass="DataGrid-HeaderStyle"
                                            OnClick="Application_Save" Font-Bold="true" BorderWidth="0" Height="20px" Width="75px" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPersonal" runat="server" style="width: 100%; position: absolute;">
                            <table cellpadding="1" cellspacing="1" width="100%">
                                <tr>
                                    <td colspan="3">
                                        <b>Postal Address </b>&nbsp;&nbsp;&nbsp;
                                        <asp:CheckBox ID="chkPostalCopyAddress" runat="server" Text="Copy Permanent Address"
                                            AutoPostBack="True" OnCheckedChanged="chkCopyAddress_CheckedChanged" />
                                    </td>
                                    <td colspan="3">
                                        <b>Permanent Address </b>&nbsp;&nbsp;&nbsp;
                                        <asp:CheckBox ID="chkCopyAddress" runat="server" Text="Copy Postal Address" AutoPostBack="True"
                                            OnCheckedChanged="chkCopyAddress_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <eluc:CommonAddress ID="PostalAddress" runat="server" />
                                    </td>
                                    <td colspan="3">
                                        <eluc:CommonAddress ID="PermanentAddress" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <h4>
                                            Physical standards
                                        </h4>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        Height(Cms) &nbsp;&nbsp;&nbsp;
                                        <eluc:Number ID="txtHeight" runat="server" CssClass="input_mandatory txtNumber" MaxLength="3"
                                            IsInteger="true" AutoPostBack="true" />
                                    </td>
                                    <td colspan="3">
                                        Weight(Kg) &nbsp;&nbsp;&nbsp;
                                        <eluc:Number ID="txtWeight" runat="server" CssClass="input_mandatory txtNumber" MaxLength="3"
                                            IsInteger="true" AutoPostBack="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        Distinguish Mark
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDistinguishMark" CssClass="input_mandatory" runat="server" MaxLength="200"
                                            Height="50" Width="200" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6" align="right">
                                        <asp:Button ID="btnPersonalSave" runat="server" CommandName="Personal" Text="Save"
                                            CssClass="DataGrid-HeaderStyle" OnClick="Application_Save" Font-Bold="true" BorderWidth="0"
                                            Height="20px" Width="75px" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divSSLC" runat="server" style="width: 100%; position: absolute;">
                            <table style="width: 100%;">
                                <tr>
                                    <td>
                                        <iframe runat="server" id="ifMoreSSLC" src="../Presea/PreSeaNewApplicantSSLC.aspx"
                                            scrolling="no" style="min-height: 745px; width: 95%; border: 0;"></iframe>
                                    </td>
                                </tr>
                            </table>
                            <br />
                        </div>
                        <div id="divHSC" runat="server" style="width: 100%; position: absolute;">
                            <table style="width: 100%;">
                                <tr>
                                    <td>
                                        <iframe runat="server" id="ifMoreHSC" scrolling="no" src="../Presea/PreSeaNewApplicantHSC.aspx"
                                            style="min-height: 745px; width: 95%; border: 0;"></iframe>
                                    </td>
                                </tr>
                            </table>
                            <br />
                        </div>
                        <div id="divGraduation" runat="server" style="width: 100%; position: absolute;">
                            <table style="width: 100%;">
                                <tr>
                                    <td>
                                        <iframe runat="server" id="idMoreGraduation" scrolling="no" src="../Presea/PreSeaNewApplicantGraduation.aspx"
                                            style="min-height: 745px; width: 95%; border: 0;"></iframe>
                                    </td>
                                </tr>
                            </table>
                            <br />
                        </div>
                        <div id="divApplication" runat="server" style="width: 100%; position: absolute;"
                            visible="false">
                            <table style="width: 100%;">
                                <tr>
                                    <td>
                                        <iframe runat="server" id="ifMoreApplication" scrolling="no" style="min-height: 610px;
                                            width: 90%; border: 0;"></iframe>
                                    </td>
                                </tr>
                            </table>
                            <br />
                        </div>
                        <div id="divFamily" runat="server" style="width: 100%; position: absolute;">
                            <b>Guardian/Family member correspondence Details </b></br> Note : Please add Family
                            name in the grid and cilk the 'Family name' then enter correspondence Details.
                            <br />
                            <br />
                            <asp:GridView ID="gvFamily" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                Width="100%" CellPadding="3" OnRowDataBound="gvFamily_RowDataBound" OnRowDeleting="gvFamily_RowDeleting"
                                OnRowEditing="gvFamily_RowEditing" OnRowCancelingEdit="gvFamily_RowCancelingEdit"
                                OnRowCommand="gvFamily_RowCommand" ShowFooter="true" OnRowUpdating="gvFamily_RowUpdating"
                                Style="margin-bottom: 0px" EnableViewState="true">
                                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                <RowStyle Height="10px" />
                                <Columns>
                                    <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                                    <asp:TemplateField HeaderText="From Date">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderTemplate>
                                            S.No
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblSlNo" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Name
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblFamilyId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFAMILYID") %>'></asp:Label>
                                            <asp:LinkButton ID="lblFamilyName" runat="server" CommandArgument='<%#Container.DataItemIndex%>'
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIRSTNAME") %>' CommandName="SELECT"></asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblFamilyIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFAMILYID") %>'></asp:Label>
                                            <asp:TextBox ID="txtFamilyNameEdit" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIRSTNAME") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtFamilyNameAdd" runat="server" CssClass="input_mandatory"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            Relation
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container, "DataItem.FLDRELATION")%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <eluc:Relation ID="ucRelationEdit" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                                                QuickList='<%# PhoenixRegistersQuick.ListQuick(1, (int)PhoenixQuickTypeCode.MISCELLANEOUSRELATION) %>' />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <eluc:Relation ID="ucRelationAdd" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                                                QuickList='<%# PhoenixRegistersQuick.ListQuick(1, (int)PhoenixQuickTypeCode.MISCELLANEOUSRELATION) %>' />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            Gender
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container, "DataItem.FLDSEXNAME")%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <eluc:Sex ID="ucGenderEdit" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                                                HardList='<%# PhoenixRegistersHard.ListHard(1,(int)PhoenixHardTypeCode.SEX)%>' />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <eluc:Sex ID="ucGenderAdd" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                                                HardList='<%# PhoenixRegistersHard.ListHard(1,(int)PhoenixHardTypeCode.SEX)%>' />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            Occupation
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container, "DataItem.FLDOCCUPATION")%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlOccupationEdit" runat="server" CssClass="input" AppendDataBoundItems="true">
                                                <asp:ListItem Value="Dummy" Text="--Select--"></asp:ListItem>
                                                <asp:ListItem Value="Medical" Text="Medical"></asp:ListItem>
                                                <asp:ListItem Value="Engineering" Text="Engineering"></asp:ListItem>
                                                <asp:ListItem Value="Law" Text="Law"></asp:ListItem>
                                                <asp:ListItem Value="Paramilitary force" Text="Paramilitary force"></asp:ListItem>
                                                <asp:ListItem Value="Police force" Text="Police force"></asp:ListItem>
                                                <asp:ListItem Value="Student" Text="Student"></asp:ListItem>
                                                <asp:ListItem Value="Housewife" Text="Housewife"></asp:ListItem>
                                                <asp:ListItem Value="Businessmen" Text="Businessmen"></asp:ListItem>
                                                <asp:ListItem Value="Self-employed" Text="Self-employed"></asp:ListItem>
                                                <asp:ListItem Value="Politics" Text="Politics"></asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlOccupationAdd" runat="server" CssClass="input" AppendDataBoundItems="true">
                                                <asp:ListItem Value="Dummy" Text="--Select--"></asp:ListItem>
                                                <asp:ListItem Value="Medical" Text="Medical"></asp:ListItem>
                                                <asp:ListItem Value="Engineering" Text="Engineering"></asp:ListItem>
                                                <asp:ListItem Value="Law" Text="Law"></asp:ListItem>
                                                <asp:ListItem Value="Paramilitary force" Text="Paramilitary force"></asp:ListItem>
                                                <asp:ListItem Value="Police force" Text="Police force"></asp:ListItem>
                                                <asp:ListItem Value="Student" Text="Student"></asp:ListItem>
                                                <asp:ListItem Value="Housewife" Text="Housewife"></asp:ListItem>
                                                <asp:ListItem Value="Businessmen" Text="Businessmen"></asp:ListItem>
                                                <asp:ListItem Value="Self-employed" Text="Self-employed"></asp:ListItem>
                                                <asp:ListItem Value="Politics" Text="Politics"></asp:ListItem>
                                            </asp:DropDownList>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            Contact No
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container, "DataItem.FLDMOBILENUMBER")%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <eluc:PhoneNumber ID="txtFamilyContactNoEdit" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMOBILENUMBER")%>' />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <eluc:PhoneNumber ID="txtFamilyContactNoAdd" runat="server" CssClass="input" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblActionHeader" runat="server" Text="Action">
                                            </asp:Label>
                                        </HeaderTemplate>
                                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                                CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdXEdit"
                                                ToolTip="Edit"></asp:ImageButton>
                                            <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                            <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl='<%$ PhoenixTheme:images/te_del.png%>'
                                                CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdXDelete"
                                                ToolTip="Delete"></asp:ImageButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                                CommandName="UPDATE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                                ToolTip="Save"></asp:ImageButton>
                                            <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                            <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                                CommandName="CANCEL" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                                ToolTip="Cancel"></asp:ImageButton>
                                        </EditItemTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <FooterTemplate>
                                            <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                                CommandName="ADD" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAdd"
                                                ToolTip="Add New"></asp:ImageButton>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <table style="width: 100%;">
                                <tr>
                                    <td>
                                        Office Tel/Mobile No
                                    </td>
                                    <td>
                                        <eluc:PhoneNumber ID="txtFamilyOfficeNo" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        Email Address
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFamilyEmail" runat="server" CssClass="input" MaxLength="500"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <table style="width: 100%;">
                                <tr runat="server" id="trFamilyAddress">
                                    <td colspan="2" rowspan="3">
                                        <eluc:CommonAddress ID="ucFamilyAddress" runat="server" />
                                    </td>
                                </tr>
                                <tr runat="server" id="trIncome">
                                    <td>
                                        Annual income of parents
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlAnualIncome" runat="server" CssClass="input">
                                            <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Less than 120000" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="1 Lac 20 Thousand - 2 Lacs 40 Thousand" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="2 Lacs 40 Thousand - 3 Lacs 60 Thousand" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="3 Lacs 60 Thousand - 6 Lacs " Value="4"></asp:ListItem>
                                            <asp:ListItem Text="Above 6 Lacs" Value="5"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <hr />
                            <table style="width: 100%;">
                                <tr>
                                    <td colspan="3">
                                        Any family members/relatives previously and/or currently employed by Executive Ship
                                        Management Pte Ltd or Samundra Institute of Maritime Studies? &nbsp;
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rdoESMFamilyRelation" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" valign="top">
                                        If Yes, please provide his/her/their Names&nbsp; &amp;&nbsp;Desiganations&nbsp;
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtESMFamilyNames" runat="server" TextMode="MultiLine" CssClass="input"
                                            Width="300px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="right">
                                        <asp:Button ID="btnFamilySave" runat="server" CommandName="Family" Text="Save" CssClass="DataGrid-HeaderStyle"
                                            OnClick="Application_Save" Font-Bold="true" BorderWidth="0" Height="20px" Width="75px" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divExtra" runat="server" style="width: 100%; position: absolute;">
                            <asp:GridView ID="gvAwardAndCertificate" runat="server" AutoGenerateColumns="False"
                                Font-Size="11px" Width="100%" CellPadding="3" OnRowDataBound="gvAwardAndCertificate_RowDataBound"
                                OnRowDeleting="gvAwardAndCertificate_RowDeleting" OnRowEditing="gvAwardAndCertificate_RowEditing"
                                OnRowCancelingEdit="gvAwardAndCertificate_RowCancelingEdit" OnRowCommand="gvAwardAndCertificate_RowCommand"
                                ShowFooter="true" OnRowUpdating="gvAwardAndCertificate_RowUpdating" Style="margin-bottom: 0px"
                                EnableViewState="true">
                                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                <RowStyle Height="10px" />
                                <Columns>
                                    <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                                    <asp:TemplateField HeaderText="From Date">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderTemplate>
                                            S.No
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblSlNo" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Award/Certificate
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblAwardId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAWARDID") %>'></asp:Label>
                                            <asp:Label ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></asp:Label>
                                            <asp:Label ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></asp:Label>
                                            <asp:LinkButton ID="lblCertificate" runat="server" CommandArgument='<%#Container.DataItemIndex%>'
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATENAME") %>' CommandName="EDIT"></asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlCertificateEdit" runat="server" CssClass="input" AppendDataBoundItems="true">
                                                <asp:ListItem Value="Dummy" Text="--Select--"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="Long Service Award"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="Cultural"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="Sports"></asp:ListItem>
                                                <asp:ListItem Value="4" Text="Technical"></asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlCertificateAdd" runat="server" CssClass="input" AppendDataBoundItems="true">
                                                <asp:ListItem Value="Dummy" Text="--Select--"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="Long Service Award"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="Cultural"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="Sports"></asp:ListItem>
                                                <asp:ListItem Value="4" Text="Technical"></asp:ListItem>
                                            </asp:DropDownList>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            Issue Date
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container, "DataItem.FLDISSUEDATE", "{0:dd/MMM/yyyy}")%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblAwardIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAWARDID") %>'></asp:Label>
                                            <eluc:Date runat="server" ID="txtIssueDateEdit" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUEDATE") %>' />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <eluc:Date runat="server" ID="txtIssueDateAdd" CssClass="input" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            Remarks
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container, "DataItem.FLDREMARKS")%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtRemarksEdit" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'
                                                MaxLength="200"></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtRemarksAdd" runat="server" CssClass="input_mandatory" MaxLength="200"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblActionHeader" runat="server" Text="Action">
                                            </asp:Label>
                                        </HeaderTemplate>
                                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                                CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdXEdit"
                                                ToolTip="Edit"></asp:ImageButton>
                                            <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                            <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl='<%$ PhoenixTheme:images/te_del.png%>'
                                                CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdXDelete"
                                                ToolTip="Delete"></asp:ImageButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                                CommandName="UPDATE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                                ToolTip="Save"></asp:ImageButton>
                                            <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                            <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                                CommandName="CANCEL" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                                ToolTip="Cancel"></asp:ImageButton>
                                        </EditItemTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <FooterTemplate>
                                            <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                                CommandName="ADD" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAdd"
                                                ToolTip="Add New"></asp:ImageButton>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div id="divOther" runat="server" style="width: 100%; position: absolute;">
                            <table cellpadding="1" cellspacing="1" width="100%">
                                <tr>
                                    <td colspan="2">
                                        Exam Venue
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Panel ID="Panel1" runat="server" GroupingText="First Choice" Width="100%" CssClass="input_mandatory">
                                            <asp:RadioButtonList ID="rblExamVenueFirst" runat="server" RepeatDirection="Horizontal"
                                                DataTextField="FLDEXAMVENUENAME" DataValueField="FLDEXAMVENUEID" RepeatColumns="5"
                                                RepeatLayout="Table">
                                            </asp:RadioButtonList>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Panel ID="Panel2" runat="server" GroupingText="Second Choice" Width="100%">
                                            <asp:RadioButtonList ID="rblExamVenueSecond" runat="server" RepeatDirection="Horizontal"
                                                DataTextField="FLDEXAMVENUENAME" DataValueField="FLDEXAMVENUEID" RepeatColumns="5"
                                                RepeatLayout="Table">
                                            </asp:RadioButtonList>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 64%">
                                        <asp:Panel ID="Panel3" runat="server" GroupingText="How did you know about Samundra Institute of Maritime Studies"
                                            Width="100%">
                                            <table width="100%" cellpadding="2" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="chkNewspaperMagazine" runat="server" Text="Newspaper/Magazine" />
                                                    </td>
                                                    <td>
                                                        <eluc:Quick ID="ucNewspaperMagazine" runat="server" CssClass="input" AppendDataBoundItems="true" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="chkFamilyRelativeFriends" runat="server" Text="Family/Relatives/Friends" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkFlyers" runat="server" Text="Flyers" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="chkSchoolCollege" runat="server" Text="School/College" />
                                                    </td>
                                                    <td>
                                                        <eluc:Quick ID="ucSchoolCollage" runat="server" CssClass="input" AppendDataBoundItems="true" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="chkEducationJoFfair" runat="server" Text="Education/Job Fair" />
                                                    </td>
                                                    <td>
                                                        <table width="100%" cellpadding="1" cellspacing="1">
                                                            <tr>
                                                                <td>
                                                                    <eluc:Country runat="server" ID="ucCountry" AutoPostBack="true" AppendDataBoundItems="true"
                                                                        CssClass="input" OnTextChangedEvent="ucCountry_TextChanged" />
                                                                    &nbsp;State:&nbsp;
                                                                </td>
                                                                <td>
                                                                    <eluc:State ID="ucState" CssClass="input" runat="server" AppendDataBoundItems="true"
                                                                        AutoPostBack="true" OnTextChangedEvent="ddlState_TextChanged" />
                                                                </td>
                                                                <td>
                                                                    Place:&nbsp;
                                                                </td>
                                                                <td>
                                                                    <eluc:City ID="ddlPlace" runat="server" AppendDataBoundItems="true" CssClass="input" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="chkEmailBySims" runat="server" Text="Emails sent by SIMS" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkShiksha" runat="server" Text="www.Shiksha.com" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="chkInternet" runat="server" Text="Internet" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtInternet" runat="server" CssClass="input" Width="300"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="chkDirectContact" runat="server" Text="Directly Contacted" />
                                                    </td>
                                                    <td>
                                                        <eluc:Quick ID="ucDirectContact" runat="server" CssClass="input" AppendDataBoundItems="true" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="chkOthers" runat="server" Text="Others,please provide details" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtOthers" runat="server" CssClass="input" Width="300"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                    <td style="width: 36%" valign="top">
                                        <asp:Panel ID="Panel4" runat="server" GroupingText="Any other information you want to tell us about yourself?"
                                            Width="100%">
                                            <asp:TextBox ID="txtAboutYourselfRemarks" runat="server" TextMode="MultiLine" CssClass="input"
                                                Width="98%" Height="200px"></asp:TextBox>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right">
                                        <asp:Button ID="btnOtherSave" runat="server" CommandName="Other" Text="Save" CssClass="DataGrid-HeaderStyle"
                                            OnClick="Application_Save" Font-Bold="true" BorderWidth="0" Height="20px" Width="75px" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divVenue" runat="server" style="width: 100%; position: absolute;">
                            <table style="font-size: 13px;">
                                <tr>
                                    <td colspan="3">
                                        Examination venues will be announced by respective Exam Centers.
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 341px;">
                                        <b>Delhi ESM Field Office </b>
                                        <br />
                                        Executive Ship Management Pte Ltd<br />
                                        502A, Rectangle -I , D-4,
                                        <br />
                                        District Centre, Saket,
                                        <br />
                                        New Delhi –110017<br />
                                        Tel: + 91-11-49882700<br />
                                        Fax: + 91-11-2956-2777
                                        <br />
                                        <b>Mob: 09999390027</b><br />
                                        Email: esmdelhi@executiveship.com<br />
                                    </td>
                                    <td style="width: 342px;">
                                        <b>Chandigarh ESM Field Office</b><br />
                                        Executive Ship Management Pte Ltd<br />
                                        S.C.O. 427/ 428 (1st Floor)<br />
                                        Sector 35-C, Chandigarh - 160022<br />
                                        Tel: + 91-172-2600417/2620417/4023700<br />
                                        Fax:+ 91-172-264-5417<br />
                                        <b>Mob: 09888599076</b><br />
                                        Email: esmchandigarh@executiveship.com<br />
                                    </td>
                                    <td style="width: 341px;">
                                        <b>Chennai ESM Field Office</b><br />
                                        Executive Ship Management Pte Ltd<br />
                                        Batra Centre 1st Floor
                                        <br />
                                        No. 28, Sardar Patel Road
                                        <br />
                                        Guindy, Chennai 600032<br />
                                        Tel: + 91-44-30453300<br />
                                        Fax:+ 91-44-30453341<br />
                                        <b>Mob: 09940555574</b><br />
                                        Email: esmchennai@executiveship.com<br />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 341px;">
                                        <b>Kolkata ESM Field Office</b><br />
                                        Executive Ship Management Pte Ltd<br />
                                        Unit 604, 6th floor, Jasmine Tower,<br />
                                        No.31,Shakespeare Sarani,<br />
                                        Kolkata - 700017.<br />
                                        Tel : + 91-33-40167900<br />
                                        Fax: + 91-33-40167910<br />
                                        <b>Mob : 08051808332</b>
                                        <br />
                                        Email : esmkalkata@executiveship.com<br />
                                    </td>
                                    <td style="width: 342px;">
                                        <b>Patna ESM Field Office</b><br />
                                        Executive Ship Management Pte Ltd
                                        <br />
                                        211, 2nd Floor Sisodia Palace, Gorakhnath Compound,<br />
                                        East Boring Canal Road<br />
                                        Patna - 800 001<br />
                                        Tel : +91-612-2532124/6530634<br />
                                        <b>Mob: 08051808332 </b>
                                        <br />
                                        Email : esmpatna@executiveship.com<br />
                                    </td>
                                    <td style="width: 341px;">
                                        <b>Dehradun ESM Field Office</b><br />
                                        Executive Ship Management Pte Ltd
                                        <br />
                                        Opposite Premier Complex, 1st Floor,
                                        <br />
                                        Adjoining State Bank of India Kaulagarh Road,
                                        <br />
                                        Rajender Nagar, Dehradun India 248001
                                        <br />
                                        Tel : +91 135 2751827/47,
                                        <br />
                                        Fax : +91 135 2751827
                                        <br />
                                        <b>Mob : +91 9997997445 </b>
                                        <br />
                                        Email : esmddun@executiveship.com<br />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <b>Guwahati SIMS Field Office<br />
                                            Samundra Institute of Maritime Studies<br />
                                            Mob: 09881554132 </b>
                                        <br />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divTerms" runat="server" style="width: 100%; position: absolute;">
                            <table width="100%" cellpadding="1" cellspacing="1" style="font-size: 12px;">
                                <tr>
                                    <td>
                                        All related costs incurred as a result of the short listed applicant attending the
                                        interview, including cost of medical tests and psychometric test, shall be fully
                                        borne by the applicant himself.
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        The list of successful candidates who are accepted for SIMS pre sea training, would
                                        be announced in our website. These candidates are to submit a non-refundable joining
                                        fee (except for caution fee which will be refunded after adjusting any dues) as
                                        per the schedule date stated in our website. Those who pay the joining fee within
                                        the time frame, will only be accepted for admission.
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Any dispute concerning the interpretation of any of the terms and conditions of
                                        admission or prospectus shall be resolved amicably. If the student(s) / candidate(s)
                                        / parent(s) and the institute failed to resolve the dispute amicably, then the matter
                                        should be referred to the sole arbitrator to be nominated by the institute. The
                                        award passed by the sole arbitrator shall be binding on both the parties. The place
                                        of arbitration shall be the institute's premises only and the provisions of arbitration
                                        and conciliation act, 1996 shall apply to the arbitration proceedings.
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Jurisdiction clause: -
                                        <br />
                                        All legal disputes are subject to Mumbai / Pune court only.
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divDeclaration" runat="server" style="width: 100%; position: absolute;">
                            <table style="font-size: 12px;">
                                <tr>
                                    <td>
                                        I declare that all information provided in this form, is true and complete in all
                                        respect. I fully understand that any misrepresentation or omission of information
                                        may be considered sufficient for withdrawal of the offer and dismissal from the
                                        course at any time. I also agree to the terms set out in above section
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        I confirm that the information as set out in this application were provided by me
                                        and that the said information is true and correct.
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        I understand that I may be subject to prosecution if I have provided any information,
                                        which is false in any material particular or is misleading by reason of the omission
                                        of any material particular.
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b>Further I, declare:</b>
                                        <br />
                                        1. I shall not engage in or participate in any business or be self-employed during
                                        my training at SIMS.
                                        <br />
                                        2. I shall not misuse controlled drugs or take part in any political or other illegal
                                        activities during my stay in SIMS premise.
                                        <br />
                                        3. I have not submitted any false statement or submitted any document which I know
                                        to be false in order to obtain the admission at SIMS
                                        <br />
                                        4. I confirm that my candidature is purely on my merit and does not involve any
                                        recommendations.
                                        <br />
                                        5. I have not bribed any one in ESM, SIMS, Agent, or outside agencies to get my
                                        admission at SIMS.
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        I understand that if I breach any conditions above, during training at SIMS or even
                                        after training at SIMS, I will be subjected to:
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        1. Prosecution in court
                                        <br />
                                        2. Dismissal and pay all costs including Training fees if it is during training
                                        at SIMS prior employment onboard.
                                        <br />
                                        3. Immediate dismissal from the ship and I agree to pay SIMS:-
                                        <br />
                                        <b>&nbsp;&nbsp;a. all the cost incurred for my repatriation
                                            <br />
                                            &nbsp;&nbsp;b. all the costs incurred for my relievers joining ship
                                            <br />
                                            &nbsp;&nbsp;c. all the costs incurred, including training fee for my pre-sea training
                                            at SIMS
                                            <br />
                                            &nbsp;&nbsp;d. all the costs incurred for my post-sea training at SIMS</b>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkDeclaration" runat="server" Font-Bold="true" ForeColor="BlueViolet"
                                            AutoPostBack="true" OnCheckedChanged="chkDeclaration_CheckedChanged" Text="I have read the
                                                Terms & conditions, which associated with this application form. I agree and submit
                                                this application  " />
                                    </td>
                                </tr>
                            </table>
                            </Content>
                        </div>
                        <div id="divPhoto" runat="server" style="width: 100%; position: absolute;">
                            <table id="Table2" width="100%" style="color: Blue">
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblNotes" runat="server" Text="<b>Note :</b>  &nbsp; Kindly confirm before upload photo, photo once uploaded cannot be changed."></asp:Literal>
                                    </td>
                                </tr>
                            </table>
                            <table cellpadding="1" cellspacing="1" width="100%">
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <a id="aPreSeaImg" runat="server">
                                            <asp:Image ID="imgPhoto" runat="server" Height="100px" ImageUrl="<%$ PhoenixTheme:images/Blank.png %>"
                                                Width="120px" /></a>
                                        <br />
                                        <asp:FileUpload ID="txtFileUpload" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="btnPhotoSave" runat="server" CommandName="Photo" Text="Save" CssClass="DataGrid-HeaderStyle"
                                            OnClick="Application_Save" Font-Bold="true" BorderWidth="0" Height="20px" Width="75px" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPayment" runat="server" style="width: 100%; position: absolute;">
                            <table cellpadding="1" cellspacing="1" width="100%">
                                <tr>
                                    <td style="width: 15%;">
                                        Pay mode
                                    </td>
                                    <td colspan="3" style="width: 65%;">
                                        <asp:RadioButtonList ID="rdoPayMode" RepeatDirection="Horizontal" RepeatColumns="3"
                                            runat="server" AutoPostBack="true" OnSelectedIndexChanged="PayMode_Changed">
                                            <asp:ListItem Text="Net Banking (NEFT, RTGS)" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Demand Draft" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Others" Value="3"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr id="DDInfo" runat="server">
                                    <td style="width: 10%;">
                                        DD No
                                    </td>
                                    <td style="width: 35%;">
                                        <eluc:Number runat="server" ID="txtDDNumber" IsInteger="true" Width="125px" CssClass="input_mandatory" />
                                    </td>
                                    <td style="width: 15%;">
                                        DD Details
                                    </td>
                                    <td style="width: 35%;">
                                        <asp:TextBox ID="txtDDDeatils" runat="server" CssClass="input_mandatory" TextMode="MultiLine"
                                            Width="90%" Height="75px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="NetPayInfo" runat="server">
                                    <td style="width: 15%;">
                                        Transfer From (A/C No)
                                    </td>
                                    <td style="width: 35%;">
                                        <eluc:Number runat="server" ID="txtNETrfrACno" IsInteger="true" Width="125px" CssClass="input_mandatory" />
                                    </td>
                                    <td style="width: 15%;">
                                        Transaction Details
                                    </td>
                                    <td style="width: 35%;">
                                        <asp:TextBox ID="txtNETTrfrDetails" runat="server" CssClass="input_mandatory" TextMode="MultiLine"
                                            Width="95%" Height="75px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="OtherPayInfo" runat="server">
                                    <td style="width: 15%;">
                                        Payment Details
                                    </td>
                                    <td colspan="3" style="width: 65%;">
                                        <asp:TextBox ID="txtOtherPayDeatail" runat="server" CssClass="input_mandatory" TextMode="MultiLine"
                                            Width="60%" Height="75px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="right">
                                        <asp:Button ID="btnPaymentSave" runat="server" CssClass="DataGrid-HeaderStyle" CommandName="Payment"
                                            Text="Save" OnClick="Application_Save" Font-Bold="true" BorderWidth="0" Height="20px"
                                            Width="75px" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <eluc:Status runat="server" ID="ucStatus" />
                        <eluc:Error ID="ucError" runat="server" Text=""></eluc:Error>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnPhotoSave" />
            </Triggers>
        </asp:UpdatePanel>
        <input type="button" runat="server" id="isouterpage" name="isouterpage" style="visibility: hidden" />
        </form>
    </div>
</body>
</html>
