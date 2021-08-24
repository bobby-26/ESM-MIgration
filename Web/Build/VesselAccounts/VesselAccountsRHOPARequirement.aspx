<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsRHOPARequirement.aspx.cs"
    Inherits="VesselAccountsRHOPARequirement" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rest Hour OPA</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRestHourCrewList" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Status ID="ucStatus" runat="server" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="MenuRHGeneral" runat="server" OnTabStripCommand="RHGeneral_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td colspan="3">
                        <telerik:RadLabel ID="lblnote" runat="server" EnableViewState="false" Text="Notes :" Font-Bold="true" ForeColor="Blue"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                    <td>
                        <telerik:RadLabel ID="lbl1" runat="server" Text="1." ForeColor="Blue"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblEntertheentryandexittimeintoUSWaters" runat="server" Text=" Enter the entry and exit time into US Waters." ForeColor="Blue"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                    <td>
                        <telerik:RadLabel ID="lbl2" runat="server" Text="2." ForeColor="Blue"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblUsethepicklisttoselectthedayandhourofentry" ForeColor="Blue" runat="server" Text="Use the pick list to select the day and hour of entry"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                    <td>
                        <telerik:RadLabel ID="lbl3" runat="server" Text="3." ForeColor="Blue"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRepeatthesametorecordthedayandhourofexit" ForeColor="Blue" runat="server" Text="Repeat the same to record the day and hour of exit."></telerik:RadLabel>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuOPAList" runat="server" OnTabStripCommand="CrewList_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvOPA" runat="server" AutoGenerateColumns="False" Width="100%" Height="80%"
                GridLines="None" OnItemDataBound="gvOPA_ItemDataBound" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" EnableHeaderContextMenu="true" GroupingEnabled="false" OnUpdateCommand="gvOPA_UpdateCommand"
                OnItemCommand="gvOPA_ItemCommand" CellPadding="3" ShowFooter="true" ShowHeader="true"
                EnableViewState="false" OnNeedDataSource="gvOPA_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
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
                        <telerik:GridTemplateColumn HeaderText="Sr.No">
                            <HeaderStyle HorizontalAlign="Left" Width="5%" />
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDROWNUMBER")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date of Entry">
                            <HeaderStyle HorizontalAlign="Left" Width="40%" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblopaid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPAREQUIREMENTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDateofEntry" runat="server" Text="Date" Font-Bold="true"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblstartdate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTARTDATE") %>'></telerik:RadLabel>
                                &nbsp;
                                    &nbsp;&nbsp; 
                                <telerik:RadLabel ID="lblHour" runat="server" Text="Hour" Font-Bold="true"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblStartReportingHour" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTARTREPORTINGHOUR") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblopaeditid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPAREQUIREMENTID") %>'></telerik:RadLabel>
                                <span id="spnPickListCalendarstart">
                                    <telerik:RadLabel ID="lblDateofEntryEdit" runat="server" Text="Date"></telerik:RadLabel>
                                    <telerik:RadTextBox
                                        ID="ucStartDate" runat="server" Width="130px" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSTARTDATE")%>'>
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtShipCalendarId" runat="server" Width="1px" CssClass="hidden"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHIPCALENDARIDSTART") %>'>
                                    </telerik:RadTextBox>
                                    <telerik:RadLabel ID="lblHour" runat="server" Text="Hour"></telerik:RadLabel>
                                    <telerik:RadTextBox ID="txtReportingHourEdit" runat="server" Width="50px" Enabled="false"
                                        CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTARTREPORTINGHOUR") %>'>
                                    </telerik:RadTextBox>
                                    <asp:LinkButton ID="cmdPickListCalendarstart" runat="server">
                                        <span class="icon"><i class="fas fas fa-tasks"></i></span>
                                    </asp:LinkButton>
                                </span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <span id="spnPickListCalendarstartadd">
                                    <telerik:RadLabel ID="lblDateFooter" runat="server" Text="Date"></telerik:RadLabel>
                                    <telerik:RadTextBox ID="ucStartDateadd" runat="server" Width="130px" Enabled="false" CssClass="input_mandatory"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtShipCalendaraddId" runat="server" Width="1px" CssClass="hidden"></telerik:RadTextBox>
                                    <telerik:RadLabel ID="lblHour" runat="server" Text="Hour"></telerik:RadLabel>
                                    <telerik:RadTextBox ID="txtReportingHourAdd" runat="server" Width="50px" Enabled="false"
                                        CssClass="input_mandatory">
                                    </telerik:RadTextBox>
                                    <asp:LinkButton runat="server" ID="cmdPickListCalendarstartadd">
                                            <span class="icon"><i class="fas fas fa-tasks"></i></span>
                                    </asp:LinkButton>
                                </span>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date of Exit">
                            <HeaderStyle HorizontalAlign="Left" Width="40%" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDateofExit" runat="server" Text="Date" Font-Bold="true"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblFinishdate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFINISHDATE") %>'></telerik:RadLabel>
                                &nbsp; &nbsp;&nbsp;
                                <telerik:RadLabel ID="lblReportingHour" runat="server" Text="Reporting Hour" Font-Bold="true"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblendReportingHour" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENDREPORTINGHOUR") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListCalendarend">
                                    <telerik:RadLabel ID="lblDateofExitEdit" runat="server" Text="Date"></telerik:RadLabel>
                                    <telerik:RadTextBox ID="ucendDate" runat="server" Width="130px" CssClass="input" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFINISHDATE")%>'></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtShipCalendarendId" runat="server" Width="1px" CssClass="hidden"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHIPCALENDARIDFINISH") %>'>
                                    </telerik:RadTextBox>
                                    <telerik:RadLabel ID="lblReportingHour" runat="server" Text="Reporting Hour"></telerik:RadLabel>
                                    <telerik:RadTextBox ID="txtendReportingHourEdit" runat="server" Width="50px" Enabled="false"
                                        CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENDREPORTINGHOUR") %>'>
                                    </telerik:RadTextBox>
                                    <asp:LinkButton runat="server" ID="cmdPickListCalendarend">
                                            <span class="icon"><i class="fas fas fa-tasks"></i></span>
                                    </asp:LinkButton>
                                </span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <span id="spnPickListCalendarendadd">
                                    <telerik:RadLabel ID="lblDateofExitAdd" runat="server" Text="Date"></telerik:RadLabel>
                                    <telerik:RadTextBox ID="ucendDateadd" runat="server" Width="130px" CssClass="input"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtShipCalendarendaddId" runat="server" Width="1px" CssClass="hidden"></telerik:RadTextBox>
                                    <telerik:RadLabel ID="lblReportingHour" runat="server" Text="Reporting Hour"></telerik:RadLabel>
                                    <telerik:RadTextBox ID="txtendReportingHourAdd" runat="server" Width="50px" Enabled="false"
                                        CssClass="input">
                                    </telerik:RadTextBox>
                                    <asp:LinkButton runat="server" ID="cmdPickListCalendarendadd">
                                            <span class="icon"><i class="fas fas fa-tasks"></i></span>
                                    </asp:LinkButton>
                                </span>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                            <ItemStyle HorizontalAlign="Center" Wrap="False" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="EDIT" CommandName="EDIT" ID="v" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Add" ID="cmdAdd" ToolTip="Add New">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
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
