<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersBatchList.aspx.cs"
    Inherits="RegistersBatchList" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="../UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Institution" Src="~/UserControls/UserControlInstitution.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Batch List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmBatchList" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />        
        <telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuBatchList" runat="server" OnTabStripCommand="BatchList_TabStripCommand"></eluc:TabStrip>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>Institution
                    </td>
                    <td>
                        <eluc:Address runat="server" ID="ucInstitution" CssClass="dropdown_mandatory" AddressType="138" Width="22%"
                            AddressList='<%# PhoenixRegistersAddress.ListAddress("138") %>' AppendDataBoundItems="true" />
                        <eluc:Institution runat="server" ID="ucCourseInstitution" AppendDataBoundItems="true"
                            CssClass="input_mandatory" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td>Course
                    </td>
                    <td>
                        <eluc:Course ID="ucCourse" runat="server" CourseList="<%#PhoenixRegistersDocumentCourse.ListDocumentCourse(null)%>" Width="22%"
                            ListCBTCourse="true" AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>Batch No
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtBatch" runat="server" CssClass="input_mandatory" MaxLength="50"
                            Width="22%">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>From Date
                    </td>
                    <td>
                        <eluc:Date runat="server" ID="txtFromDate" CssClass="input_mandatory" AutoPostBack="false" Width="22%"
                            OnTextChangedEvent="CalculateDuration" />
                    </td>
                </tr>
                <tr>
                    <td>To Date
                    </td>
                    <td>
                        <eluc:Date runat="server" ID="txtToDate" CssClass="input_mandatory" AutoPostBack="true" Width="22%"
                            OnTextChangedEvent="CalculateDuration" />
                    </td>
                </tr>
                <tr>
                    <td>Duration(In Days)
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDuration" runat="server" CssClass="readonlytextbox" MaxLength="50" Width="22%"
                            Enabled="false">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>Status
                    </td>
                    <td>
                        <eluc:Hard ID="ucStatus" runat="server" AutoPostBack="true" AppendDataBoundItems="true" Width="22%"
                            HardTypeCode="152" />
                    </td>
                </tr>
                <tr>
                    <td>Closed Y/N
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkClosedYN" runat="server"></telerik:RadCheckBox>
                    </td>
                </tr>
                <tr>
                    <td>Exclude Sunday
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkExcludeSunday" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Include Holidays
                    </td>
                    <td>
                        <asp:CheckBox ID="chkIncludeHoliday" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Published Y/N
                    </td>
                    <td>
                        <asp:CheckBox ID="chkPublishYN" runat="server" Enabled="false"></asp:CheckBox>
                    </td>
                </tr>
                <tr>
                    <td>Notes
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtNotes" TextMode="MultiLine" Rows="4" runat="server" Width="50%"
                            CssClass="input">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr runat="server" visible="false">
                    <td>Is Pre-Sea batch Y/N
                    </td>
                    <td>
                        <telerik:RadDropDownList ID="ddlPreseayn" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true">
                            <Items>
                                <telerik:DropDownListItem Value="Dummy" Text="--Select--"></telerik:DropDownListItem>
                                <telerik:DropDownListItem Value="1" Text="Yes"></telerik:DropDownListItem>
                                <telerik:DropDownListItem Value="0" Text="No"></telerik:DropDownListItem>
                            </Items>
                        </telerik:RadDropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblNoofSeats" runat="server" Text="No of Seats"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtNoOfSeats" runat="server" CssClass="input txtNumber" MaxLength="5"
                            IsInteger="true" />
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
