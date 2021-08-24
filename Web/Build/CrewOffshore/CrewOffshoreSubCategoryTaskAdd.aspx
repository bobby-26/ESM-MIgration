<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreSubCategoryTaskAdd.aspx.cs" Inherits="CrewOffshore_CrewOffshoreSubCategoryTaskAdd" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToBeDoneBy" Src="~/UserControls/UserControlOffshoreToBeDoneBy.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <telerik:RadScriptManager ID="radscript1" runat="server">
            </telerik:RadScriptManager>
            <telerik:RadAjaxPanel ID="panel1" runat="server">
                <eluc:Status runat="server" ID="ucStatus" />
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:TabStrip ID="MenuDoumentCourseList" runat="server" OnTabStripCommand="MenuDoumentCourseList_TabStripCommand"></eluc:TabStrip>
                <table>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblCourse" runat="server" Text="Category"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlCompetenceCategory" runat="server" CssClass="input" Width="200px"
                                AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlCompetenceCategory_SelectedIndexChanged"
                                EmptyMessage="Type to select" MarkFirstMatch="true">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblsubcategory" runat="server" Text="Sub Category"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddsubcategory" runat="server" CssClass="input" Width="200px"
                                EmptyMessage="Type to select" MarkFirstMatch="true"
                                AppendDataBoundItems="true" AutoPostBack="true">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lbltask" runat="server" Text="Task Title"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txttask" runat="server" Text="" Width="300px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="RadLabel3" runat="server" Text="Task Description"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtdescription" runat="server" Text="" TextMode="MultiLine" Width="350px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblapplies" runat="server" Text="Applicable"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="chkRankList" runat="server"
                                EmptyMessage="Type to select rank" Filter="Contains" MarkFirstMatch="true" Width="200px" CheckBoxes="true"
                                EnableCheckAllItemsCheckBox="true">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lbllevel" runat="server" Text="Level"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddllevel" runat="server" Width="200px"
                                EmptyMessage="Type to select level" Filter="Contains" MarkFirstMatch="true">
                                <Items>
                                    <telerik:RadComboBoxItem Text="1" Value="1" />
                                    <telerik:RadComboBoxItem Text="2" Value="2" />
                                    <telerik:RadComboBoxItem Text="3" Value="3" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="RadLabel1" runat="server" Text="To be done by"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:ToBeDoneBy ID="ucToBeDoneby" Width="200px" runat="server" CssClass="input_mandatory"
                                AppendDataBoundItems="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="RadLabel2" runat="server" Text="Assessor"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="chkassessor" runat="server" Width="200px"
                                EmptyMessage="Type to select rank" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true"
                                EnableCheckAllItemsCheckBox="true">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                </table>

            </telerik:RadAjaxPanel>
        </div>
    </form>
</body>
</html>
