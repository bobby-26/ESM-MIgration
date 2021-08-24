<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionMOCIntermediateVerificationAdd.aspx.cs"
    Inherits="InspectionMOCIntermediateVerificationAdd" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Department" Src="~/UserControls/UserControlDepartment.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script runat="server">
</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" RenderMode="Lightweight" ID="RadWindowManager1">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"
            Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text="" />
            <eluc:Title runat="server" ID="ucTitle" Text="MOC Intermediate Verification" Visible="false"></eluc:Title>
            <eluc:TabStrip ID="MenuMOCRequest" runat="server" Title="Section 1: Intermediate verification (By: Responsible person)" OnTabStripCommand="MOCRequest_TabStripCommand"></eluc:TabStrip>
            <table id="tblmoc" width="100%">
                <tr>
                    <td width="25%">
                        <telerik:RadLabel ID="lblIntVerificationDate" runat="server" Text="1.Date Intermediate Verification is due">
                        </telerik:RadLabel>
                    </td>
                    <td width="25%">
                        <eluc:Date ID="txtIntVerificationDate" runat="server" CssClass="input_mandatory"
                            DatePicker="true" Enabled="false" ReadOnly="true" />
                    </td>
                    <td width="25%">
                        <telerik:RadLabel ID="lblMOCPlanned" runat="server" Text="2.Is the MOC progressing as planned and are the identified actions adequate">
                        </telerik:RadLabel>
                    </td>
                    <td width="25%">
                        <asp:RadioButtonList ID="rblMOCPlanned" runat="server" OnSelectedIndexChanged="rblMOCPlanned_changed"
                            AutoPostBack="true" RepeatDirection="Horizontal" RepeatLayout="Table">
                            <asp:ListItem Text="Yes" Value="0" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="No" Value="1"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td >
                        <telerik:RadLabel ID="lblCompletionDate" runat="server" Text="3.Date Intermediate verification completed">
                        </telerik:RadLabel>
                    </td>
                    <td >
                        <eluc:Date ID="txtCompletionDate" runat="server" CssClass="input_mandatory" DatePicker="true"
                            AutoPostBack="true" OnTextChangedEvent="txtCompletionDate_TextChanged" />
                        <telerik:RadTextBox ID="txtmoctype" runat="server" Visible="false">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <b>
                            <telerik:RadLabel ID="lblActionPlan" runat="server" Text="a.Additional Actions to be taken, provide details by adding items below (this will be reflected in the action plan in Section B) (By responsible person)">
                            </telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <eluc:TabStrip ID="MenuCA" runat="server" OnTabStripCommand="MenuCA_TabStripCommand"
                            Visible="false"></eluc:TabStrip>
                        <telerik:RadGrid ID="gvMOCActionPlan" runat="server" AutoGenerateColumns="False"
                            Font-Size="11px" Width="100%" CellPadding="3" OnItemCommand="gvMOCActionPlan_RowCommand"
                            OnItemDataBound="gvMOCActionPlan_ItemDataBound" ShowHeader="true" EnableViewState="false"
                            OnUpdateCommand="gvMOCActionPlan_RowUpdating" OnNeedDataSource="gvMOCActionPlan_NeedDataSource"
                            ShowFooter="false">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                                AllowNaturalSort="false" AutoGenerateColumns="false" DataKeyNames="FLDMOCACTIONPLANID">
                                <NoRecordsTemplate>
                                    <table id="Table1" runat="server" width="100%" border="0">
                                        <tr>
                                            <td align="center">
                                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger"
                                                    Font-Bold="true">
                                                </telerik:RadLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </NoRecordsTemplate>
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Actions to be taken">
                                        <HeaderStyle Width="25%" HorizontalAlign="Left" />
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblMOCActionPlanid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCACTIONPLANID") %>'>
                                            </telerik:RadLabel>
                                            <telerik:RadLabel ID="lblActionToBeTaken" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDACTIONTOBETAKEN") %>'>
                                            </telerik:RadLabel>
                                            <telerik:RadLabel ID="lblMOC" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCID") %>'
                                                Visible="false">
                                            </telerik:RadLabel>
                                            <telerik:RadLabel ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'
                                                Visible="false">
                                            </telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Person In Charge">
                                        <HeaderStyle Width="20%" HorizontalAlign="Left" />
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblPIC" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPICNAME") %>'>
                                            </telerik:RadLabel>
                                            <telerik:RadLabel ID="lblPICRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPICRANK") %>'>
                                            </telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Target">
                                        <HeaderStyle Width="10%" HorizontalAlign="Left" />
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblTargetDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDTARGETDATE")) %>'
                                                Width="80px"></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Completion">
                                        <HeaderStyle Width="10%" HorizontalAlign="Left" />
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="Label2" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOMPLETIONDATE")) %>'
                                                Width="80px"></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Closed">
                                        <HeaderStyle Width="10%" HorizontalAlign="Left" />
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblClosedDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCLOSEDDATE")) %>'
                                                Width="80px"></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Status">
                                        <HeaderStyle Width="25%" HorizontalAlign="Left" />
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblstatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'>
                                            </telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Action">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDITROW" ID="cmdEdit"
                                                ToolTip="Edit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete"
                                                ToolTip="Delete">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                                AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
                        <table id="moetarget" width="100%" runat="server">
                            <tr>
                                <td width="25%">
                                    <telerik:RadLabel ID="lblMOCTargetDateExtended" runat="server" Text="b.Is the MOC Target date of implementation to be extended">
                                    </telerik:RadLabel>
                                </td>
                                <td width="25%">
                                    <asp:RadioButtonList ID="rblMOCTargetDateExtended" runat="server" OnSelectedIndexChanged="rblMOCTargetDateExtended_changed"
                                        AutoPostBack="true" RepeatDirection="Horizontal" RepeatLayout="Table">
                                        <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
