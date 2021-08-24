<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InventoryComponentGeneral.aspx.cs"
    Inherits="InventoryComponentGeneral" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlUnit" Src="~/UserControls/UserControlUnit.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlQuick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlHard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskedTextBox" Src="~/UserControls/UserControlMaskedTextBox.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>General</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPlannedMaintenanceComponentTypeGeneral" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />

        <telerik:RadAjaxPanel ID="pnlComponentGeneral" runat="server">

            <div style="font-weight: 600; font-size: 12px;" runat="server">
                <eluc:TabStrip ID="MenuComponentGeneral" runat="server" OnTabStripCommand="PlannedMaintenanceComponent_TabStripCommand"></eluc:TabStrip>
            </div>
            <br clear="all" />

            <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                <AjaxSettings>
                    <telerik:AjaxSetting AjaxControlID="ucConfirm">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="pnlComponentGeneral" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MaskedTextBox MaskText="###.##.##" runat="server" ID="txtComponentNumber" Width="100px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSerialNumber" runat="server" Text="Serial Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSerialNumber" runat="server" CssClass="input" MaxLength="50"
                            Width="80px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtComponentName" runat="server" CssClass="input_mandatory" MaxLength="200" Width="240px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBudgetCode" runat="server" Text="Budget Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListMainBudget">
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtBudgetCode" runat="server" Width="60px" CssClass="input"></telerik:RadTextBox>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtBudgetName" runat="server" Width="175px" CssClass="input"></telerik:RadTextBox>
                            <asp:LinkButton ID="btnShowBudget" runat="server" ImageAlign="AbsMiddle" Text="..">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" ID="imgClearBudget" ImageAlign="AbsMiddle" Text=".." OnClick="imgClearBudget_Click">
                                <span class="icon"><i class="fas fa-paint-brush"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtBudgetId" runat="server" CssClass="hidden"></telerik:RadTextBox>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtBudgetgroupId" runat="server" CssClass="hidden"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMaker" runat="server" Text="Maker"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListMaker">
                            <telerik:RadTextBox ID="txtMakerCode" runat="server" MaxLength="20" Width="85px" ReadOnly="true" CssClass="input readonlytextbox">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtMakerName" runat="server" MaxLength="200" Width="150px" ReadOnly="true" CssClass="input readonlytextbox">
                            </telerik:RadTextBox>
                            <asp:LinkButton ID="imgShowMaker" runat="server" ImageAlign="AbsMiddle" Text="..">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" ID="cmdMakerClear" ImageAlign="AbsMiddle" Text=".." OnClick="cmdMakerClear_Click">
                                <span class="icon"><i class="fas fa-paint-brush"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtMakerId" runat="server" CssClass="hidden"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVendor" runat="server" Text="Vendor"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListVendor">
                            <telerik:RadTextBox ID="txtVendorCode" runat="server" ReadOnly="true" CssClass="input readonlytextbox"
                                MaxLength="20" Width="85px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtVendorName" runat="server" ReadOnly="true" CssClass="input readonlytextbox"
                                MaxLength="200" Width="150px">
                            </telerik:RadTextBox>
                            <asp:LinkButton ID="imgShowVendor" runat="server" ImageAlign="AbsMiddle" Text="..">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" ID="cmdVendorClear" ImageAlign="AbsMiddle" Text=".." OnClick="cmdVendorClear_Click">
                                <span class="icon"><i class="fas fa-paint-brush"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtVendorId" runat="server" CssClass="hidden"></telerik:RadTextBox>
                        </span>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLocation" runat="server" Text="Location"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtLocation" runat="server" CssClass="input" MaxLength="200" Width="240px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblParentComponent" runat="server" Text="Parent Component"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListParentComponent">
                            <telerik:RadTextBox ID="txtParentComponentNumber" runat="server" ReadOnly="true" CssClass="input readonlytextbox"
                                MaxLength="20" Width="85px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtParentComponentName" runat="server" ReadOnly="true" CssClass="input readonlytextbox"
                                MaxLength="200" Width="150px">
                            </telerik:RadTextBox>
                            <asp:LinkButton ID="imgShowParentComponent" runat="server" ImageAlign="AbsMiddle" Text="..">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" ID="cmdClearParentComponent" ImageAlign="AbsMiddle" Text=".." OnClick="cmdClearParentComponent_Click">
                                <span class="icon"><i class="fas fa-paint-brush"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtParentComponentID" runat="server" CssClass="hidden"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblClassCode" runat="server" Text="Class Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtClassCode" runat="server" CssClass="input" MaxLength="200" Width="240px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtType" runat="server" CssClass="input" MaxLength="50" Width="240px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblintalltion" runat="server" Text="Installation Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtinstallation" runat="server" DatePicker="true" MaxLength="200" Width="240px"></eluc:Date>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlHard runat="server" ID="ucStatus" HardTypeCode="13" AppendDataBoundItems="true" />
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblclassification" Text="Critical Category" runat="server"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadRadioButtonList ID="rbCompClasification" runat="server" Direction="Horizontal">
                            <Items>
                                <telerik:ButtonListItem Text="NA" Value="0" />
                                <telerik:ButtonListItem Text="Safety" Value="1" />
                                <telerik:ButtonListItem Text="Operational" Value="2" />
                                <telerik:ButtonListItem Text="Environmental" Value="3" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
                <tr style="visibility: hidden">
                    <td>
                        <telerik:RadLabel ID="lblOperational" runat="server" Text="Operational & Safety Critical"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkOperationalCritical" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblEnvironmental" runat="server" Text="Environmental Critical"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkEnvironmentalCritical" runat="server" />
                    </td>
                </tr>
            </table>
            <eluc:Confirm ID="ucConfirm" runat="server" Visible="false" OnConfirmMesage="ucConfirm_ConfirmMesage" OKText="Yes" CancelText="No" />
        </telerik:RadAjaxPanel>
        <%--  </ContentTemplate>
            <Triggers>
            </Triggers>
        </asp:UpdatePanel>--%>
        <%--<eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />--%>
    </form>
</body>
</html>
