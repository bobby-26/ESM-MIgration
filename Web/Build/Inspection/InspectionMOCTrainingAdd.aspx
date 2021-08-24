<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionMOCTrainingAdd.aspx.cs"
    Inherits="InspectionMOCTrainingAdd" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Department" Src="~/UserControls/UserControlDepartment.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Training Required</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInspectionMapping" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
        EnableShadow="true">
    </telerik:RadWindowManager>
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"
        Height="100%">
        <eluc:TabStrip ID="MenuMapping" runat="server" OnTabStripCommand="MenuMapping_TabStripCommand"
            Title="MOC Training Task"></eluc:TabStrip>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click"
            CssClass="hidden" />
        <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
        <div id="divFind">
            <table width="100%" CellPadding="3" CellSpacing="3">
                <tr>
                    <td width="15%">
                        <telerik:RadLabel ID="lblDepartmentType" runat="server" Text="Type">
                        </telerik:RadLabel>
                    </td>
                    <td width="35%">
                        <telerik:RadRadioButtonList ID="rblDepartmentType" runat="server" AutoPostBack="true"
                            Direction="Horizontal" OnSelectedIndexChanged="rblDepartmentType_SelectedIndexChanged">
                            <Items>
                                <telerik:ButtonListItem Text="Ship Staff" Value="1" Selected="True" />
                                <telerik:ButtonListItem Text="Shore Staff" Value="2" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                    <%--<td width="15%">
                            <telerik:RadLabel ID="lblstatus" runat="server" Text="Status"></telerik:RadLabel>
                        </td>
                        <td width="35%">
                            <telerik:RadTextBox ID="txtstatus" runat="server" Width="180px" ReadOnly="true" Enabled="false">
                            </telerik:RadTextBox>
                        </td>--%>
                    <td>
                        <telerik:RadLabel ID="lbltargetdate" runat="server" Text="Target Date">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="uctargetdate" CssClass="input_mandatory" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTrainingRequired" runat="server" Text="Training Required">
                        </telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtTrainingRequired" runat="server" Width="360px" TextMode="MultiLine"
                            Rows="2" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDepartmentPersonTrained" runat="server" Text="Dept./Persons to be trained">
                        </telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtTrained" runat="server" Width="360px" TextMode="MultiLine"
                            Rows="2" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDepartment" runat="server" Text="Department"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Department ID="ucDept" runat="server" AppendDataBoundItems="true" 
                            Width="270px" />
                    </td>
                    <td colspan="2">

                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPIC" runat="server" Text="Responsiblity">
                        </telerik:RadLabel>
                    </td>
                    <td>                        
                        <span id="spnReportedByShip3">
                            <telerik:RadTextBox ID="txtPICNameAdd" runat="server" CssClass="input" Enabled="false"
                                MaxLength="100" Width="45%">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtPICRankAdd" runat="server" CssClass="input" Enabled="false"
                                MaxLength="100" Width="45%">
                            </telerik:RadTextBox>
                            <asp:LinkButton runat="server" ID="imgReportedByShip" Style="cursor: pointer; vertical-align: top">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtPICIdAdd" runat="server" CssClass="input" MaxLength="20"
                                Width="0px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox runat="server" ID="txtShoreTeamAppointedByEmail" CssClass="input"
                                Width="0px" MaxLength="20">
                            </telerik:RadTextBox>
                        </span>
                    </td>
                    <td colspan="2">
                    </td>
                </tr>
            </table>
        </div>
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
