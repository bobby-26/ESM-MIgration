<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRACargoListExtn.aspx.cs" Inherits="InspectionRACargoListExtn" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselByOwner.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleet.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cargo</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function ConfirmRevision(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmRevision.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRAType" runat="server" Text="Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlRAType" runat="server" Width="270px"  Filter="Contains" MarkFirstMatch="true">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRefNo" runat="server" Text="Ref Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRefNo" runat="server" Width="150px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblcategory" runat="server" Text="Activity"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlCategory" runat="server" AppendDataBoundItems="true"
                            AutoPostBack="True" DataTextField="FLDNAME" DataValueField="FLDACTIVITYID" Width="270px" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value="DUMMY" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblIntendedWork" runat="server" Text="Intended Work Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucDateIntendedWorkFrom" runat="server" />
                        -
                                    <eluc:Date ID="ucDateIntendedWorkTo" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFleet" runat="server" Text="Fleet"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Fleet runat="server" ID="ucTechFleet" Width="270px" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlStatus" runat="server" AppendDataBoundItems="true" Width="270px" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel runat="server" ID="ucVessel" AppendDataBoundItems="true" Width="270px" AssignedVessels="true" />
                    </td>
                    <td colspan="2"></td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuCargo" runat="server" OnTabStripCommand="MenuCargo_TabStripCommand"></eluc:TabStrip>
            <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvRiskAssessmentCargo" DecoratedControls="All" EnableRoundedCorners="true" />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvRiskAssessmentCargo" Height="74%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" EnableHeaderContextMenu="true" GroupingEnabled="false" ShowHeader="true" EnableViewState="false"
                OnItemDataBound="gvRiskAssessmentCargo_ItemDataBound" OnItemCommand="gvRiskAssessmentCargo_ItemCommand" OnNeedDataSource="gvRiskAssessmentCargo_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDRISKASSESSMENTCARGOID">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Ref. No" HeaderStyle-Width="13%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRefNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER")  %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblReferencid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCEID")  %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblInstallcode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSTALLCODE")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel" HeaderStyle-Width="10%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID")  %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsCreatedByOffice" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISCREATEDBYOFFICE")  %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Prepared" HeaderStyle-Width="8%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPrepared" runat="server" Text='<%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDDATE"]) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Intended <br> Work" HeaderStyle-Width="8%" AllowSorting="true" SortExpression="FLDINTENDEDWORKDATE" ShowSortIcon="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIntended" runat="server" Text='<%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDINTENDEDWORKDATE"])%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblTypeHeader" runat="server">Type</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Activity / Conditions" HeaderStyle-Width="30%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRiskAssessmentCargoID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKASSESSMENTCARGOID")  %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblRevisionno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONNO")  %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <asp:LinkButton ID="lnkJobActivity" runat="server" CommandName="EDITROW" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVITYCONDITIONS")  %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Target Date <br> for completion" HeaderStyle-Wrap="true" HeaderStyle-Width="11%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTarget" runat="server" Text='<%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDCOMPLETIONDATE"]) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Task <br> completed YN" HeaderStyle-Width="10%" HeaderStyle-Wrap="true" >
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMitigating" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTASKSTATUS")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rev No" HeaderStyle-Width="6%">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRevno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONNO")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="14%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS")  %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblStatusName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="14%">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="10%"></ItemStyle>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Approve" CommandName="APPROVE" ID="imgApprove" ToolTip="Approve">
                                    <span class="icon"><i class="fas fa-user-check"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Revision" CommandName="REVISION" ID="imgrevision" ToolTip="Revision">
                                    <span class="icon"><i class="fas fa-registered"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Report" CommandName="RACARGO" ID="cmdRACargo" ToolTip="Show PDF">
                                    <span class="icon"><i class="fas fa-chart-bar"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Revision" CommandName="VIEWREVISION" ID="cmdRevision" ToolTip="View Revisions">
                                    <span class="icon"><i class="fas fa-eye"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Copy" CommandName="COPYTEMPLATE" ID="imgCopyTemplate" ToolTip="Copy">
                                    <span class="icon"><i class="fas fa-copy"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <asp:Button ID="ucConfirmApprove" runat="server" Text="confirmApprove" OnClick="btnConfirmApprove_Click" />
            <asp:Button ID="ucConfirmRevision" runat="server" Text="confirmRevision" OnClick="btnConfirmRevision_Click" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

