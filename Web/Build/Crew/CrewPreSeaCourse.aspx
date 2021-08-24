<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewPreSeaCourse.aspx.cs"
    Inherits="CrewPreSeaCourse" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PreSea Courses</title>
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
    <form id="frmCrewPreSeaCourse" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager" runat="server"></telerik:RadSkinManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <eluc:TabStrip ID="MenuCrewPreSeaCourse" runat="server" OnTabStripCommand="CrewPreSeaCourse_TabStripCommand"></eluc:TabStrip>
        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblqualification" runat="server" Text="Qualification"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Course ID="ucCourse" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                        DataBoundItemName="Direct Entry" AutoPostBack="true" OnTextChangedEvent="DisablePreSea"
                        DocumentType="450" Width="300px" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblPlaceofInstitution" Text="Place of Institution" runat="server"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtPlaceOfInstittution" runat="server" CssClass="input_mandatory"
                        MaxLength="200">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblInstitution" Text="Institution" runat="server"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Address ID="ucInstitution" runat="server" CssClass="dropdown_mandatory" AddressType="138"
                        AppendDataBoundItems="true" Width="300px" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblFromDate" Text="From Date" runat="server"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtFromDate" runat="server" Width="130px" CssClass="input_mandatory" />
                </td>
            </tr>
            <tr>

                <td>
                    <telerik:RadLabel ID="lblToDate" Text="To Date" runat="server"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtToDate" runat="server" Width="130px" CssClass="input_mandatory" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblPassDate" Text="Pass Date" runat="server"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtPassDate" runat="server" Width="130px" CssClass="input_mandatory" />
                </td>
            </tr>
            <tr>

                <td>
                    <telerik:RadLabel ID="lblPercentAge" Text="Percentage" runat="server"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Decimal ID="txtPercentage" runat="server" CssClass="input txtNumber" Width="60px" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblGrade" Text="Grade" runat="server"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtGrade" runat="server" CssClass="input" MaxLength="1"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>

                <td>
                    <telerik:RadLabel ID="lblOallRank" runat="server" Text="O'all Rank"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Number ID="ucOallRank" runat="server" CssClass="input" MaxLength="3" Width="60px" MaskText="###" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtRemarks" runat="server" CssClass="input" MaxLength="200" TextMode="MultiLine"
                        Height="50px" Width="220px">
                    </telerik:RadTextBox>
                </td>

            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblFileSelect" runat="server" Text="Choose a File"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadUpload ID="txtFileUpload" runat="server" MaxFileInputsCount="1" OverwriteExistingFiles="false"
                        ControlObjectsVisibility="None" Skin="Silk">
                    </telerik:RadUpload>
                    <%--<asp:FileUpload ID="txtFileUpload" runat="server" CssClass="input_mandatory" Width="180" />--%>
                </td>

                <%-- <td>
                        <telerik:RadLable ID="lblAttachment" runat="server" Text = "Attachment"></telerik:RadLable>
                    </td>--%>
            </tr>
        </table>
    </form>
</body>
</html>
