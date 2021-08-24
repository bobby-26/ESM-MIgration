<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReApprovalforSeafarers.aspx.cs"
    Inherits="CrewReApprovalforSeafarers" ValidateRequest="false" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Occassion" Src="~/UserControls/UserControlOccassionForReport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Re Approval Seafarer</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmReApprovalforSeafarers" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="90%">
            <eluc:TabStrip ID="CrewReApprovalSeafarer" runat="server" OnTabStripCommand="CrewReApprovalSeafarer_TabStripCommand"></eluc:TabStrip>            
            <asp:Button ID="cmdHiddenPick" runat="server" OnClick="cmdHiddenSubmit_Click" />
            <table width="85%" cellspacing="3">
                <tr>
                    <td>
                        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                        <eluc:Status ID="ucStatus" runat="server" Text="" Visible="false"></eluc:Status>
                        <telerik:RadLabel ID="lblEmployeeFileNo" runat="server" Text="File No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployeeFileNo" runat="server" ReadOnly="true" Width="52%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployeeName" runat="server" ReadOnly="true" Width="50%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCategory" runat="server" Text="Category"></telerik:RadLabel>
                    </td>
                    <td>

                        <telerik:RadComboBox ID="ddlCategory" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" Width="52%"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                        </telerik:RadComboBox>

                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSubCategory" runat="server" Text="Sub Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlSubCategory" runat="server" CssClass="dropdown_mandatory" Width="50%"
                            AutoPostBack="true" Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                        </telerik:RadComboBox>

                    </td>
                </tr>
                <tr>
                    <td rowspan="2">
                        <telerik:RadLabel ID="lblActionTaken" runat="server" Text="Action to be Taken"></telerik:RadLabel>
                    </td>
                    <td rowspan="2">
                        <telerik:RadRadioButtonList ID="rblActionType" runat="server" Layout="Flow" Columns="1" Direction="Horizontal"
                            AutoPostBack="true" OnSelectedIndexChanged="rblActionType_SelectedIndexChanged">
                            <Items>
                                <telerik:ButtonListItem Value="1" Text="To be Trained" />
                                <telerik:ButtonListItem Value="2" Text="To be Counselled" />
                                <telerik:ButtonListItem Value="3" Text="NTBR" />
                                <telerik:ButtonListItem Value="4" Text="Check Fitness" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lbl" runat="server" Text="Courses"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnCourse">
                            <telerik:RadTextBox ID="txtCourseName" runat="server"
                                Width="50%" Wrap="true">
                            </telerik:RadTextBox>
                            <img runat="server" id="imgShowCourse" style="cursor: pointer; vertical-align: top"
                                src="<%$ PhoenixTheme:images/picklist.png  %>" />
                            <telerik:RadTextBox ID="txtCourseId" runat="server" ReadOnly="true"
                                Width="10px">
                            </telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCouncelledRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCouncelledRemarks" runat="server"
                            Width="50%" TextMode="MultiLine">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rblStatus" runat="server" Layout="Flow" Columns="1" Direction="Horizontal"
                            AutoPostBack="true" OnSelectedIndexChanged="rblStatus_SelectedIndexChanged">
                            <Items>
                                <telerik:ButtonListItem Value="1" Text="NTBR" />
                                <telerik:ButtonListItem Value="0" Text="Re-Employ" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblNTBRType" runat="server" Text="NTBR Type"></telerik:RadLabel>

                        <br />
                        <br />
                        <telerik:RadLabel ID="lblManager" runat="Server" Text="Manager"></telerik:RadLabel>
                        <br />
                        <br />
                        <telerik:RadLabel ID="lblPrincipal" runat="server" Text="Principal"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rblManagerPrincipal" runat="server" Layout="Flow" Columns="2" Direction="Horizontal"
                            AutoPostBack="true" OnSelectedIndexChanged="rblManagerPrincipal_SelectedIndexChanged">
                            <Items>
                                <telerik:ButtonListItem Value="1" Text="Manager" />
                                <telerik:ButtonListItem Value="2" Text="Principal" />
                            </Items>
                        </telerik:RadRadioButtonList>

                        <br />
                        <eluc:Address ID="ddlManager" AddressType="126" runat="server" AppendDataBoundItems="true"
                            Enabled="false" Width="50%" />
                        <br />
                        <br />
                        <eluc:Principal ID="ucPrinicipal" runat="server" AddressType="128" AppendDataBoundItems="true"
                            Enabled="false" Width="50%" />
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
