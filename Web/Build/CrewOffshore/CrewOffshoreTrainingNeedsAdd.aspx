<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreTrainingNeedsAdd.aspx.cs" Inherits="CrewOffshoreTrainingNeedsAdd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmCrew" Src="~/UserControls/UserControlConfirmMessage.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Training Needs</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

         <script type="text/javascript">
        function confirm(args) {
            if (args) {
                __doPostBack("<%=confirm.UniqueID %>", "");
            }
        }
        </script>s
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <asp:Button ID="confirm" runat="server" OnClick="btnCrewApprove_Click" />

                <eluc:TabStrip ID="CrewQuery" runat="server" Title="Add Training Needs" OnTabStripCommand="CrewQuery_TabStripCommand"></eluc:TabStrip>

                <table width="100%" cellpadding="1" cellspacing="1">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" AutoPostBack="true"
                                OnTextChangedEvent="ucVessel_TextChanged" Width="150" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblEmployee" runat="server" Text="Employee"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlEmployee" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                                AppendDataBoundItems="true" OnSelectedIndexChanged="ddlEmployee_SelectedIndexChanged" Width="150px" 
                                Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select employee">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblCategory" runat="server" Text="Category"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlCategoryAdd" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                                AppendDataBoundItems="true" OnSelectedIndexChanged="ddlCategoryAdd_SelectedIndexChanged" Width="150px"
                                Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select category">
                            </telerik:RadComboBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblSubCategory" runat="server" Text="Sub Category"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlSubCategoryAdd" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" AutoPostBack="true" Width="150px"
                                Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select sub category">
                            </telerik:RadComboBox>
                        </td>

                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblTrainingNeed" runat="server" Text="Identified Training Need"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtTrainignNeed" runat="server" CssClass="input_mandatory" Width="150"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblImprovementHeader" runat="server" Text="Level of Improvement"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Quick ID="ucImprovementAdd" runat="server" CssClass="input_mandatory"
                                AppendDataBoundItems="true" QuickTypeCode="128" Width="150" />
                        </td>

                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblTypeofTrainingHeader" runat="server" Text="Type of Training"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Quick ID="ucTypeofTrainingAdd" runat="server" CssClass="input_mandatory"
                                AppendDataBoundItems="true" QuickTypeCode="129" Width="150" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRemarks" runat="server" CssClass="input" TextMode="MultiLine" Rows="3" Width="250px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
           <%-- <eluc:ConfirmCrew ID="ucConfirmCrew" runat="server" OnConfirmMesage="btnCrewApprove_Click"
                OKText="Yes" CancelText="No" Visible="false" />--%>
            <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
