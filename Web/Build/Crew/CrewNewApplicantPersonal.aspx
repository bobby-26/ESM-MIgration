<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewNewApplicantPersonal.aspx.cs"
    Inherits="CrewNewApplicantPersonal" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPool.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlNationality" Src="~/UserControls/UserControlNationality.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlRank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlHard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlQuick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlZone" Src="~/UserControls/UserControlZone.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlMaritalStatus" Src="~/UserControls/UserControlMaritalStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Batch" Src="~/UserControls/UserControlPreSeaBatch.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew</title>

    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixCrew.js"></script>
        <script type="text/javascript">
            function showBMI() {
                var bmi = document.getElementById("divBMI");
                bmi.style.display = "block";
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewPersonal" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="CrewPersonaltab" runat="server" OnTabStripCommand="CrewPersonaltab_TabStripCommand" Title="Personal"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <table cellpadding="10" cellspacing="10" id="table1" runat="server">
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblPhotograph" runat="server" Text="Photograph" Visible="false"></telerik:RadLabel>
                        <br />
                        <a id="aCrewImg" runat="server">
                            <asp:Image ID="imgPhoto" runat="server" Height="100px" ImageUrl="<%$ PhoenixTheme:images/Blank.png %>"
                                Width="120px" /></a>
                        <br />
                        <%--<asp:FileUpload ID="txtFileUpload" runat="server"  />--%>
                        <telerik:RadUpload ID="txtFileUpload" runat="server" MaxFileInputsCount="1" OverwriteExistingFiles="false"
                            ControlObjectsVisibility="None" Skin="Silk">
                        </telerik:RadUpload>
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtFirstname" runat="server" CssClass="input_mandatory upperCase" MaxLength="200" Width="150px"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtMiddleName" runat="server" CssClass="input upperCase" MaxLength="200" Width="150px"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtLastName" runat="server" CssClass="input upperCase" MaxLength="200" Width="150px"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblCreatedBy" runat="Server" Text="Created By"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtCreatedBy" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="150px"></telerik:RadTextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table id="table2" runat="server" cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeFileNo" runat="server" Text="Employee/File No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployeeCode" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="150px"></telerik:RadTextBox>
                        <asp:LinkButton runat="server" ID="imgPDForm" AlternateText="PD Form" ToolTip="PD Form" Visible="false">
                            <span class="icon"><i class="fas fa-file"></i></span>
                        </asp:LinkButton>
                        <asp:LinkButton runat="server" AlternateText="Email" Visible="false"
                            ID="cmdEmail" ToolTip="Pdform Email" Width="20PX" Height="20PX">                            
                            <span class="icon"><i class="fas fa-envelope"></i></span>
                        </asp:LinkButton>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="Server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtEmployeeStatus" CssClass="readonlytextbox" ReadOnly="true" Width="150px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCivilStatus" runat="server" Text="Civil Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlMaritalStatus ID="ddlMaritialStatus" runat="server" AppendDataBoundItems="true"
                            CssClass="dropdown_mandatory" Width="150px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFileNo" runat="server" Text="File No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRepFileNo" runat="server" CssClass="input_mandatory upperCase" Width="150px"></telerik:RadTextBox>
                        <span id="Span1" class="icon" runat="server"><i class="fas fa-info-circle" style="align-content: center"></i></span>
                        <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="RadToolTip1" Width="300px" ShowEvent="onmouseover"
                            RelativeTo="Element" Animation="Fade" TargetControlID="Span1" IsClientID="true"
                            HideEvent="ManualClose" Position="TopCenter" EnableRoundedCorners="true"
                            Text="If present employee, please enter File No. with prefix 'E' . If new employee please enter 'NA'">
                        </telerik:RadToolTip>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRankApplied" runat="server" Text="Rank Applied"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlRank ID="ddlRank" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="150px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAppliedOn" runat="server" Text="Applied On"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAppliedOn" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="150px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPlaceofBirth" runat="server" Text="Place of Birth"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPlaceofBirth" runat="server" CssClass="input upperCase" Width="150px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDateofBirth" runat="server" Text="D.O.B."></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="txtDateofBirth" runat="server" CssClass="input_mandatory"
                            Width="150px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAge" runat="server" Text=" Age"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtAge" CssClass="readonlytextbox" ReadOnly="true" Width="150px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblNationality" runat="server" Text="Nationality"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlNationality ID="ddlNationality" runat="server" AppendDataBoundItems="true"
                            CssClass="dropdown_mandatory" Width="150px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPassportNo" runat="server" Text="Passport"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPassport" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            MaxLength="200" Width="150px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSeamansBookNumber" runat="server" Text="CDC No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSeamenBookNumber" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            MaxLength="200" Width="150px">
                        </telerik:RadTextBox>
                        <span id="Span2" class="icon" runat="server"><i class="fas fa-info-circle" style="align-content: center"></i></span>
                        <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="RadToolTip2" Width="300px" ShowEvent="onmouseover"
                            RelativeTo="Element" Animation="Fade" TargetControlID="Span2" IsClientID="true"
                            HideEvent="ManualClose" Position="TopCenter" EnableRoundedCorners="true"
                            Text="Please enter only the correct CDC No without space( For eg. Mum123456) If not available please enter 'NA'">
                        </telerik:RadToolTip>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPanNo" runat="server" Text="PAN No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPanNo" runat="server" CssClass="input upperCase" Width="150px"></telerik:RadTextBox>
                        <asp:Image ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" ID="imgPPClipPanNo"
                            runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblUIDNO" runat="server" Text="UID No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtUidNo" runat="server" CssClass="input upperCase" Width="150px"></telerik:RadTextBox>
                        <asp:Image ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" ID="imgPPClipUidNo"
                            runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblINDOSNo" runat="Server" Text="INDOS No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtINDOsNumber" runat="server" CssClass="readonlytextbox" ReadOnly="true" MaxLength="50" Width="150px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSIMSPreseaBatch" runat="server" Text="PreSea Batch"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Batch ID="ucBatch" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" Width="150px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPool" runat="Server" Text="Pool"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Pool ID="ucPool" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Enabled="false" Width="150px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblZone" runat="Server" Text="Manning Office"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlZone ID="ddlZone" runat="server" AppendDataBoundItems="true"
                            Width="150px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblGender" runat="server" Text="Gender"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlHard ID="ddlSex" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="150px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblHeight" runat="Server" Text="Height(Cms)"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtHeight" runat="server" CssClass="input_mandatory txtNumber" MaxLength="5"
                            IsInteger="true" OnTextChangedEvent="CalculateBMI" AutoPostBack="true" Width="150px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblWeight" runat="server" Text="Weight(Kg)"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtWeight" runat="server" MaxLength="6"
                            OnTextChangedEvent="CalculateBMI" AutoPostBack="true" Width="150px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblHairColor" runat="server" Text="Hair Colour"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtHairColor" CssClass="input_mandatory upperCase" runat="server" MaxLength="50" Width="150px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblEyeColour" runat="server" Text="Eye Colour"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEyeColor" CssClass="input_mandatory upperCase" runat="server" MaxLength="50" Width="150px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDistinguishMark" runat="server" Text="Distinguish Mark"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDistinguishMark" CssClass="input_mandatory upperCase" runat="server" MaxLength="200" Width="150px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblShoeSize" runat="server" Text="Shoe Size(Inch)"></telerik:RadLabel>
                    </td>
                    <td>
                        <%--<eluc:Number ID="txtSheos" runat="server" CssClass="input txtNumber" MaxLength="5" Width="150px" IsPositive="true" />--%>
                        <telerik:RadComboBox DropDownPosition="Static" ID="ddlShoesSize" runat="server" EnableLoadOnDemand="True" Width="150px"
                            EmptyMessage="Type to select Shoe Size" Filter="Contains" MarkFirstMatch="true">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="--Select--" Selected="true" />
                                <telerik:RadComboBoxItem Value="6.00" Text="6" />
                                <telerik:RadComboBoxItem Value="7.00" Text="7" />
                                <telerik:RadComboBoxItem Value="8.00" Text="8" />
                                <telerik:RadComboBoxItem Value="9.00" Text="9" />
                                <telerik:RadComboBoxItem Value="10.00" Text="10" />
                                <telerik:RadComboBoxItem Value="11.00" Text="11" />
                                <telerik:RadComboBoxItem Value="12.00" Text="12" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBMI" runat="server" Text="BMI"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtBMI" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                            MaxLength="4" Width="150px">
                        </telerik:RadTextBox>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMentor" runat="server" Text="Mentor"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListFleetManager">
                            <telerik:RadTextBox ID="txtMentorName" runat="server" MaxLength="100" Width="150px"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtuserDesignation" runat="server" CssClass="hidden" Enabled="false"
                                MaxLength="30" Width="5px" ReadOnly="true">
                            </telerik:RadTextBox>
                            <asp:LinkButton runat="server" Style="cursor: pointer; vertical-align: top"
                                ID="imguser" OnClientClick="return showPickList('spnPickListFleetManager', 'codehelp1', '', 'Common/CommonPickListUser.aspx?framename=ifMoreInfo', true); "
                                ToolTip="Select Mentor">
                                <span class="icon"><i class="fas fa-list-alt-picklist"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox runat="server" ID="txtuserid" CssClass="hidden"></telerik:RadTextBox>
                            <telerik:RadTextBox runat="server" ID="txtuserEmailHidden" CssClass="hidden" Width="5px"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblUploadResume" runat="Server" Text="Upload Resume"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:FileUpload ID="UploadResume" runat="server" />
                        <asp:Image ID="imgResume" runat="server" ImageUrl="<%$ PhoenixTheme:images/download_1.png %>" ToolTip="Resume" />
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr id="Tr1" visible="false" runat="server">
                    <td>
                        <telerik:RadLabel ID="lblPassportName" runat="server" Text="Name as per Passport"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPassportName" runat="server" MaxLength="200"
                            Width="150px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBoilerSuitSize" runat="server" Text="Boiler Suit Size"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlBoilerSuitSize" runat="server" EnableLoadOnDemand="True" Width="150px"
                            EmptyMessage="Type to select Shirt Size" Filter="Contains" MarkFirstMatch="true">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="--Select--" Selected="true"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="S" Text="S"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="M" Text="M"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="L" Text="L"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="XL" Text="XL"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="XXL" Text="XXL"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="XXXL" Text="XXXL"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="XXXXL" Text="XXXXL"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
            </table>
            <div id="divBMI" runat="server" style="position: absolute; display: none; top: 0px; left: 0px; z-index: 10; background-color: White"
                onmouseout="javascript:this.style.display='none';">
            </div>
            <eluc:UserControlStatus ID="ucStatus" runat="server" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
