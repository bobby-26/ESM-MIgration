<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionMOCTypeofChangeSubCategoryEdit.aspx.cs"
    Inherits="InspectionMOCTypeofChangeSubCategoryEdit" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>

<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>MOC SubCategory Edit</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmRegistersCountry" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server" Height="100%">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <telerik:RadFormDecorator ID="rfdinstruction" RenderMode="LightWeight" runat="server"
                DecoratedControls="All" EnableRoundedCorners="true" DecorationZoneID="divFind"></telerik:RadFormDecorator>

            <eluc:TabStrip ID="MenuMOCSubCategoryEdit" runat="server" OnTabStripCommand="MenuMOCSubCategoryEdit_TabStripCommand" TabStrip="true"></eluc:TabStrip>

            <br />
            <table id="tblMOCSubcategoryedit2">
                <tr>
                    <td>&nbsp&nbsp&nbsp&nbsp<telerik:RadLabel ID="lblCategory" runat="server" Text="Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCategory" runat="server" Width="270px"></telerik:RadTextBox>
                        <telerik:RadLabel ID="lblcategory2" runat="server" Visible="false"></telerik:RadLabel>
                    </td>
                </tr>

                <tr>
                    <td style="padding-right: 30px">&nbsp&nbsp&nbsp&nbsp<telerik:RadLabel ID="lblshortcode" runat="server" Text="Code"></telerik:RadLabel>
                    </td>

                    <td>
                        <telerik:RadTextBox ID="txtCodeEdit" runat="server" Width="270px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'></telerik:RadTextBox></td>

                </tr>
                <tr>
                    <td style="padding-right: 30px">&nbsp&nbsp&nbsp&nbsp<telerik:RadLabel ID="RadLabel1" runat="server" Text="SubCategory"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSubCategoryEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCSUBCATEGORYNAME") %>' Width="270px"
                            CssClass="gridinput_mandatory">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSubCategoryIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCSUBCATEGORYID") %>'
                            Visible="false">
                        </telerik:RadLabel>
                    </td>
                </tr>

                <tr>
                    <td>&nbsp&nbsp&nbsp&nbsp<telerik:RadLabel ID="RadLabel2" runat="server" Text="Proposer Level"></telerik:RadLabel>
                    </td>

                    <td>
                        <telerik:RadComboBox ID="ddlProposerRoleEdit" runat="server" DataTextField="FLDMOCAPPROVERROLE" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select Proposer Level"
                            DataValueField="FLDMOCAPPROVERROLEID" Width="270px">
                        </telerik:RadComboBox>

                        </asp:LinkButton>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblProposerRole" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPOSERROLEID") %>'
                            Visible="false">
                        </telerik:RadLabel>
                    </td>
                </tr>


                <td>
                    <telerik:RadLabel ID="lblTempApproverRole" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTEMPORARYAPPROVERROLEID") %>'
                        Visible="false">
                    </telerik:RadLabel>
                </td>     
                <tr>
                    <td>&nbsp&nbsp&nbsp&nbsp<telerik:RadLabel ID="RadLabel4" runat="server" Text="Approver Level 1"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlPermanantApproverRoleEdit" runat="server"
                            Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select Approval Level 1"
                            DataTextField="FLDMOCAPPROVERROLE" DataValueField="FLDMOCAPPROVERROLEID" Width="270px">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPermanantApproverRole" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERMANENTAPPROVERROLEID") %>'
                            Visible="false">
                        </telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp&nbsp&nbsp&nbsp<telerik:RadLabel ID="RadLabel3" runat="server" Text="Approver Level 2"></telerik:RadLabel>
                    </td>


                    <td>
                        <telerik:RadComboBox ID="ddlTempApproverRoleEdit" runat="server" DataTextField="FLDMOCAPPROVERROLE" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select Approval Level 2"
                            DataValueField="FLDMOCAPPROVERROLEID" Width="270px">
                        </telerik:RadComboBox>

                    </td>
                    </tr>
                    <tr>
                        <td>&nbsp&nbsp&nbsp&nbsp<telerik:RadLabel ID="RadLabel5" runat="server" Text="Responsible Person"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlresponsiblepersonedit" runat="server" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select Responsible Person"
                                DataTextField="FLDMOCAPPROVERROLE" DataValueField="FLDMOCAPPROVERROLEID" Width="270px">
                            </telerik:RadComboBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblresponsiblepersonrole" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRESPONSIBLEPERSONROLEID") %>' Visible="false" />
                        </td>
                    </tr>
            </table>

            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
