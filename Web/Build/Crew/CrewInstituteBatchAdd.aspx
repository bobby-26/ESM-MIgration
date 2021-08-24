<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewInstituteBatchAdd.aspx.cs" Inherits="Crew_InstituteBatchAdd" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="../UserControls/UserControlAddressType.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Institute Batch Add</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuHeader" runat="server" OnTabStripCommand="MenuHeader_TabStripCommand" Title="Batch Add"></eluc:TabStrip>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="70%">
            <table cellpadding="1" cellspacing="2px" width="100%">
                <tr>

                    <td>
                        <telerik:RadLabel ID="lblInstitute" runat="server" Text="Institute"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListInstitute">
                            <telerik:RadTextBox ID="txtInstituteId" runat="server" Width="0"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtInstituteName" runat="server" Width="350px" CssClass="input_mandatory" Enabled="False"></telerik:RadTextBox>
                            <asp:LinkButton runat="server" Style="cursor: pointer; vertical-align: top" ID="btnShowInstitute" ToolTip="Select Institute">
                                <span class="icon"><i class="fas fa-tasks"></i></span>
                            </asp:LinkButton>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCourse" runat="server" Text="Course"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListCourse">
                            <telerik:RadTextBox ID="txtCourse" runat="server" Width="350px" 
                                CssClass="input_mandatory">
                            </telerik:RadTextBox>
                              <asp:LinkButton runat="server" Style="cursor: pointer; vertical-align: top" ID="btnShowCourse" ToolTip="Select Course"
                                  OnClick="btnShowCourse_Click">
                                <span class="icon"><i class="fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtCourseId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVenue" runat="server" Text="Venue"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <eluc:Address runat="server" ID="ucBatchVenue" CssClass="dropdown_mandatory" AddressType="138"
                            AddressList='<%# PhoenixRegistersAddress.ListAddress("138") %>' AppendDataBoundItems="true"
                            AutoPostBack="true" OnTextChangedEvent="BindVenueDetails" Width="350px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAddress" runat="server" Text="Address"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVenueAddress" runat="server" CssClass="readonlytextbox" TextMode="MultiLine"
                            Height="60px" Width="320px" ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPrimaryContact" runat="server" Text="Primary Contact"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVenuePrimaryContact" runat="server" CssClass="readonlytextbox"
                            ReadOnly="true" Width="220px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCity" runat="server" Text="City"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVenueCity" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="220px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPhoneNo" runat="server" Text="Phone No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVenuePhoneno" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="220px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblState" runat="server" Text="State"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVenueState" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="220px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblEMail" runat="server" Text="E-Mail"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVenueEmail" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="220px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCountry" runat="server" Text="Country"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVenueCountry" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="220px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPostalCode" runat="server" Text="Postal Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVenuePostalCode" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="220px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:Status ID="ucStatus" runat="server" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
