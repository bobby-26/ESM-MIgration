<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewSignOff.aspx.cs" Inherits="CrewSignOff" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCTSignOffReason" Src="~/UserControls/UserControlSignOffReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCNationality" Src="~/UserControls/UserControlNationality.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCRank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCVessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCSignOn" Src="~/UserControls/UserControlSignOn.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCPort" Src="~/UserControls/UserControlSeaport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCCompany" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCPool" Src="~/UserControls/UserControlPool.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCLQuick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Sign-Off</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript">
            function checkTextAreaMaxLength(textBox, e, length) {

                var mLen = textBox["MaxLength"];
                if (null == mLen)
                    mLen = length;

                var maxLength = parseInt(mLen);
                if (!checkSpecialKeys(e)) {
                    if (textBox.value.length > maxLength - 1) {
                        if (window.event)//IE
                            e.returnValue = false;
                        else//Firefox
                            e.preventDefault();
                    }
                }
            }
            function checkSpecialKeys(e) {
                if (e.keyCode != 8 && e.keyCode != 46 && e.keyCode != 37 && e.keyCode != 38 && e.keyCode != 39 && e.keyCode != 40)
                    return false;
                else
                    return true;
            } function confirm(args) {
                if (args) {
                    __doPostBack("<%=confirm.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>
    <style type="text/css">
        .scrolpan {
            overflow-y: auto;
            height: 80%;
        }

        .checkRtl {
            direction: rtl;
        }

        .fon {
            font-size: small !important;
        }
    </style>

</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmSignOff" DecoratedControls="All" EnableRoundedCorners="true" />
    <form id="frmSignOff" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <eluc:TabStrip ID="MenuSignOff" Title="Sign Off" runat="server" OnTabStripCommand="CrewSignOff_TabStripCommand" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="90%" CssClass="scrolpan">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table cellpadding="1" cellspacing="1" width="99%">
                <tr>
                    <td style="width: 13%;">
                        <asp:Button ID="confirm" runat="server" Text="confirm" OnClick="btnSignOff_Click" />
                        <telerik:RadLabel ID="lblEmployeeCode" runat="server" Text="File No."></telerik:RadLabel>
                    </td>
                    <td style="width: 20%;">
                        <telerik:RadTextBox ID="txtEmployeeCode" runat="server" Width="180px" CssClass="readonlytextbox" ReadOnly="true" Enabled="false"></telerik:RadTextBox>
                    </td>
                    <td style="width: 13%;">
                        <telerik:RadLabel ID="lblEmployeeName" runat="server" Text="Employee"></telerik:RadLabel>
                    </td>
                    <td style="width: 20%;">
                        <telerik:RadTextBox ID="txtEmployeeName" runat="server" Width="180px" MaxLength="50" CssClass="readonlytextbox"
                            ReadOnly="true" Enabled="false">
                        </telerik:RadTextBox>
                    </td>
                    <td style="width: 13%;">
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td style="width: 20%;">
                        <telerik:RadTextBox ID="txtRank" runat="server" Width="180px" MaxLength="20" CssClass="readonlytextbox"
                            ReadOnly="true" Enabled="false">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVessel" runat="server" Width="180px" MaxLength="20" CssClass="readonlytextbox"
                            ReadOnly="true" Enabled="false">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSignedOn" runat="server" Text="Signed On"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSignedOn" runat="server" MaxLength="20" CssClass="readonlytextbox"
                            ReadOnly="true" Enabled="false">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBeginToD" runat="server" Text="Begin ToD"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtBtoD" runat="server" MaxLength="20" CssClass="readonlytextbox"
                            ReadOnly="true" Enabled="false">
                        </telerik:RadTextBox>
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReliefDue" runat="server" Text="Relief Due"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtReliefDue" runat="server" MaxLength="20" CssClass="readonlytextbox"
                            ReadOnly="true" Enabled="false">
                        </telerik:RadTextBox>
                    </td>
                    <td><span id="Span1" class="icon" runat="server"><i class="fas fa-info-circle" style="align-content: center"></i></span>
                        <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="RadToolTip1" Width="500px" ShowEvent="onmouseover"
                            RelativeTo="Element" Animation="Fade" TargetControlID="Span1" IsClientID="true" CssClass="fon"
                            HideEvent="ManualClose" Position="TopCenter" EnableRoundedCorners="true" ContentScrolling="Auto"
                            Text="*Please Note, Remarks are mandatory in following conditions:<br/>a)Seafarers signinnig off on medical grounds.<br/> b)Seafarer signinnig off+/- 15days from the relief due date.<br/>* Please Note, Reason(Delayed/Early Relief) is mandatory if Seafarer signinnig off+/- 7 days from the relief due date.">
                        </telerik:RadToolTip>

                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSignOffDate" runat="server" Text="SignOff Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtSignOffDate" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStandbyByDate" runat="server" Text="Standy By Date"></telerik:RadLabel>
                    </td>
                    <td colspan="1">
                        <eluc:Date ID="txtStandByDate" runat="server" CssClass="input" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPort" runat="server" Text="Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UCPort ID="ddlPort" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLeaveStartDate" runat="server" Text="Leave Start Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtLeaveStartDate" runat="server" CssClass="readonlytextbox" Enabled="false" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLeaveCompletionDate" runat="Server" Text="Leave Completion Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtLeaveCompletionDate" runat="server" CssClass="input" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSignOffReason" runat="server" Text="Sign Off Reason"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UCTSignOffReason ID="ddlSignOffReason" runat="server" AppendDataBoundItems="true"
                            CssClass="dropdown_mandatory" OnTextChangedEvent="ddlSignOffReason_TextChanged"
                            AutoPostBack="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReleiver" runat="server" Text="Releiver"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UCSignOn ID="ddlSignOn" runat="server" AppendDataBoundItems="true" CssClass="input" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDateofAvailability" runat="server" Text="Date of Availabiltiy"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDOA" runat="server" CssClass="input" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDOAGivenDate" runat="server" Text="DOA Given Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDOAGivenDate" runat="server" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEndToD" runat="server" Text="End ToD"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtEndToD" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                            OnTextChangedEvent="OnEtodClick" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDispensationapplied" runat="server" Text="Dispensation Applied"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDropDownList ID="ddlDispensationapplied" runat="server" CssClass="input_mandatory">
                            <Items>
                                <telerik:DropDownListItem Value="" Text="--Select--" />
                                <telerik:DropDownListItem Text="YES" Value="1" />
                                <telerik:DropDownListItem Text="NO" Value="0" />
                                <telerik:DropDownListItem Text="NA" Value="2" />
                            </Items>
                        </telerik:RadDropDownList>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReliefReason" runat="server" Text="Reason(Delayed/Early Relief)"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UCLQuick ID="ucDelayEarlyReason" runat="server" CssClass="input" AppendDataBoundItems="true"
                            QuickTypeCode="143" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" valign="top">
                        <telerik:RadCheckBox ID="chkEndTravel" runat="server" CssClass="checkRtl" AutoPostBack="true" OnCheckedChanged="OnEndTravelClick" Text="Allow end travel and s/off on same date" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadTextBox ID="txtSignOffRemarks" runat="server" CssClass="input" TextMode="MultiLine"
                            onkeyDown="checkTextAreaMaxLength(this,event,'200');" Width="300px">
                        </telerik:RadTextBox>
                    </td>

                </tr>
            </table>
            <telerik:RadTextBox ID="txtSignOnOffId" runat="server" MaxLength="6" CssClass="input" Visible="false"></telerik:RadTextBox>
            <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvCrewSignOff" Width="100%" runat="server" AllowPaging="true"
                OnNeedDataSource="gvCrewSignOff_NeedDataSource" OnItemCommand="gvCrewSignOff_ItemCommand" ShowFooter="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                    AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Vessel">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSignOffId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNONOFFID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblPresentVessel" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="llnkVessel" runat="server" CommandName="EDIT"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME")  %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank">

                            <ItemStyle Wrap="False"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNOFFRANK") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reason">

                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInActiveReason" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNOFFREASON") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sign-Off On">

                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSignOffDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNOFFDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="btnSignOff_Click" OKText="Yes"
                CancelText="No" Visible="false" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
