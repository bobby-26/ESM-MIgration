<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionWorkRequest.aspx.cs"
    Inherits="InspectionWorkRequest" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Discipline" Src="~/UserControls/UserControlDiscipline.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Work Requisition Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmWorkOrderRequisition" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuWorkOrderRequestion" runat="server" OnTabStripCommand="MenuWorkOrderRequestion_TabStripCommand"></eluc:TabStrip>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel Visible="false" runat="server" ID="lblWorkOrderRequisitionID"></telerik:RadLabel>
                        <telerik:RadLabel Visible="false" runat="server" ID="lblWorkOrderID"></telerik:RadLabel>
                        <telerik:RadTextBox ID="txtWorkOrderNumber" Width="120px" runat="server" Enabled="false"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblTitle" runat="server" Text="Title"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtTitle" runat="server" CssClass="input_mandatory" Width="300px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblComponent" runat="server" Text="Component"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListComponent">
                            <telerik:RadTextBox ID="txtComponentCode" runat="server" CssClass="input_mandatory" MaxLength="200"
                                Enabled="false" Width="80px"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtComponentName" runat="server" CssClass="input_mandatory" MaxLength="200"
                                Enabled="false" Width="260px"></telerik:RadTextBox>
                            <asp:LinkButton id="imgComponent" runat="server">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtComponentId" runat="server" Width="10px"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCreated" runat="server" Text="Created"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCreatedDate" runat="server" Width="90px" Enabled="false"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblJobDescription" runat="server" Text="Job Description"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListJob">
                            <telerik:RadTextBox ID="txtJobCode" runat="server" CssClass="input readonlytextbox" MaxLength="200"
                                ReadOnly="false" Width="80px"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtJobName" runat="server" CssClass="input readonlytextbox" MaxLength="200"
                                ReadOnly="false" Width="260px"></telerik:RadTextBox>
                            <asp:LinkButton id="imgJob" runat="server"> 
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtJobId" runat="server" Width="10px"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblResponsibility" runat="server" Text="Responsibility"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Discipline ID="ucDiscipline" runat="server" AppendDataBoundItems="true" Width="180px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEstimatedDuration" runat="server" Text="Estimated Duration(Hrs)"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Decimal runat="server" ID="txtDuration" Width="60px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblUnplannedWork" runat="server" Text="Unplanned Work"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkUnexpected" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPlannedStart" runat="server" Text="Planned Start"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtPlannedStartDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPriority" runat="server" Text="Priority"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Decimal runat="server" ID="txtPriority" Mask="9" Width="60px"
                            Text="1" />
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblWorkDetails" runat="server" Text="Work Details"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtJobDescription" runat="server" Width="342px" TextMode="MultiLine"
                            Height="50px" Resize="Both"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDefectList" runat="server" Text="Defect List"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox runat="server" ID="chkIsDefect" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPTWApproval" runat="server" Text="PTW Approval"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucWTOApproval" runat="server" HardTypeCode="117"
                            AppendDataBoundItems="true" DataBoundItemName="None" Width="180px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMaintenanceType" runat="server" Text="Maintenance Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucMainType" runat="server" AppendDataBoundItems="true" Width="180px" />
                    </td>
                </tr>
            </table>
            <hr />
            <telerik:RadGrid ID="gvWorkOrder" runat="server" AutoGenerateColumns="False" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" Width="100%" Height="48%" CellPadding="3" ShowHeader="true" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false"
                OnItemCommand="gvWorkOrder_ItemCommand" OnNeedDataSource="gvWorkOrder_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDWORKORDERID">
                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn>
                            <HeaderStyle Width="3%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:Image ID="imgFlag" runat="server" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Work Order Number">
                            <HeaderStyle Width="10%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWorkorderNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Work Order Title">
                            <HeaderStyle Width="15%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblComponentId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDCOMPONENTID"]%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblWorkOrderId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDWORKORDERID"] %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkWorkorderName" runat="server" CommandName="SELECT" Text=' <%#((DataRowView)Container.DataItem)["FLDWORKORDERNAME"]%>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Component Number">
                            <HeaderStyle Width="10%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDCOMPONENTNUMBER"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Component Name">
                            <HeaderStyle Width="20%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDCOMPONENTNAME"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Priority">
                            <HeaderStyle Width="10%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDPLANINGPRIORITY"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Resp Discipline">
                            <HeaderStyle Width="10%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDDISCIPLINENAME"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status">
                            <HeaderStyle Width="10%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDHARDNAME"] %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Completion Date">
                            <HeaderStyle Width="10%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#  General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDWORKORDERCOMPLETEDDATE"])%>
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
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
