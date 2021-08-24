<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreSignOffList.aspx.cs"
    Inherits="CrewOffshoreSignOffList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeaPort" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SignOnReason" Src="~/UserControls/UserControlSignOnReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SignOffReason" Src="~/UserControls/UserControlSignOffReason.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Sign Off List</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

                <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click"
                    CssClass="hidden" />

                <b>
                    <telerik:RadLabel ID="lblSignOnList" runat="server" Text="Sign-On List"></telerik:RadLabel>
                </b>

                <eluc:TabStrip ID="CrewSignOn" runat="server" OnTabStripCommand="CrewSignOn_TabStripCommand"></eluc:TabStrip>

                <div id="divGrid" style="position: relative; z-index: 1; width: 100%;">
                    <%--  <asp:GridView ID="gvCrewSearch" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowDataBound="gvCrewSearch_RowDataBound" OnRowEditing="gvCrewSearch_RowEditing"
                        OnRowCancelingEdit="gvCrewSearch_RowCancelingEdit" OnRowUpdating="gvCrewSearch_RowUpdating"
                        OnRowDeleting="gvCrewSearch_RowDeleting" OnRowCommand="gvCrewSearch_RowCommand" AllowSorting="true" OnSorting="gvCrewSearch_Sorting"
                        ShowHeader="true" EnableViewState="false" DataKeyNames="FLDSIGNONOFFID">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewSearch" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvCrewSearch_NeedDataSource"
                        OnItemDataBound="gvCrewSearch_ItemDataBound"
                        OnItemCommand="gvCrewSearch_ItemCommand"
                        OnSortCommand="gvCrewSearch_SortCommand"
                        GroupingEnabled="false" EnableHeaderContextMenu="true"
                        AutoGenerateColumns="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView DataKeyNames="FLDSIGNONOFFID" EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="Top">
                            <CommandItemSettings ShowRefreshButton="false" ShowAddNewRecordButton="false" />
                            <NoRecordsTemplate>
                                <table width="100%" border="0">
                                    <tr>
                                        <td align="center">
                                            <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </NoRecordsTemplate>
                            <HeaderStyle Width="102px" />
                            <Columns>

                                <telerik:GridTemplateColumn HeaderText="Vessel">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDVESSELNAME"]%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="File No">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="85px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblFileNo" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDFILENO"]%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Name">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="200px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblSignOnOffId" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDSIGNONOFFID"] %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblEmployeeid" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDEMPLOYEEID"]%>'></telerik:RadLabel>
                                        <asp:LinkButton ID="lnkEmployeeName" runat="server" CommandArgument="<%#Container.DataSetIndex%>"
                                            Text='<%# ((DataRowView)Container.DataItem)["FLDNAME"]%>' CommandName="GETEMPLOYEE"></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Rank">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="100px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblRank" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDSIGNONRANKNAME"]%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Nationality">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="100px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblNationality" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDNATIONALITYNAME"]%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Passport No">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="100px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblPassportNo" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDPASSPORTNO"]%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Currency" UniqueName="Currency">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderStyle Width="75px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblItemCurrency" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDCURRENCYCODE"]%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadLabel ID="lbleditCurrency" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDCURRENCYCODE"]%>'></telerik:RadLabel>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Daily Rate" UniqueName="DailyRate">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderStyle Width="75px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDailyRate" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDDAILYRATE"]%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadLabel ID="lblDailyRateEdit" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDDAILYRATE"]%>'></telerik:RadLabel>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Daily DP Allowance">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderStyle Width="75px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDPRate" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDDPALLOWANCE"]%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Sign On Date">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblSignOnDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDSIGNONDATE")) %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblSeaPortId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDSIGNONSEAPORTID"] %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblSignOnOffIdAdd" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDSIGNONOFFID"] %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblCrewPlanId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDCREWPLANID"] %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblSignonDateCheckId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDSIGNONDATECHECKID"] %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadLabel ID="lblSignOnOffIdEdit" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDSIGNONOFFID"] %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblSignonDateCheckIdEdit" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDSIGNONDATECHECKID"] %>'></telerik:RadLabel>
                                        <eluc:Date Width="100%" ID="txtSignOnDate" runat="server" CssClass="input_mandatory" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDSIGNONDATE"))%>'
                                            AutoPostBack="true" OnTextChangedEvent='txtSignOnDate_TextChanged' />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Sign-On Reason">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblSignonReason" runat="server" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDSIGNONREASON") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblSignonReasonId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDSIGNONREASONID"] %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadLabel ID="lblSignOnIdEdit" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDSIGNONREASONID"] %>'></telerik:RadLabel>
                                        <eluc:SignOnReason Width="100%" runat="server" ID="ucSignOnReasonEdit" CssClass="input_mandatory" AppendDataBoundItems="true"
                                            SignOnReasonList='<%#SouthNests.Phoenix.Registers.PhoenixRegistersreasonssignon.Listreasonssignon() %>' />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Sign-On Port">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblSignOnPort" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDSIGNONSEAPORTNAME"]%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:SeaPort Width="100%" runat="server" ID="ddlSeaPort" CssClass="input_mandatory" AppendDataBoundItems="true"
                                            SeaportList='<%#SouthNests.Phoenix.Registers.PhoenixRegistersSeaport.ListSeaport() %>'
                                            SelectedSeaport='<%# ((DataRowView)Container.DataItem)["FLDSIGNONSEAPORTID"]%>' />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="End of Contract">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="100px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblReliefDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDRELIEFDUEDATE"))%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadLabel ID="lblReliefDateEdit" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDRELIEFDUEDATE"))%>'></telerik:RadLabel>
                                        <eluc:Date Width="100%" ID="txtReliefDateEdit" runat="server" Visible="false" CssClass="input" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDRELIEFDUEDATE"))%>' />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Max Tour of Duty">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="80px"></ItemStyle>
                                    <HeaderStyle Width="75px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lbl90ReliefDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLD90RELIEFDATE"))%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadLabel ID="lbl90ReliefDateEdit" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLD90RELIEFDATE"))%>'></telerik:RadLabel>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Action">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="200px"></HeaderStyle>

                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="EDIT"
                                            CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                            ToolTip="Edit">                                            
                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Document Checklist"
                                            CommandName="DOCUMENTCHECKLIST" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDocChecklist"
                                            ToolTip="Documents Checklist">
                                            
                                            <span class="icon"><i class="fas fa-list-ul"></i></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Approve"
                                            CommandName="APPROVE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdApprove"
                                            ToolTip="Sign-on">
                                            <span class="icon"><i class="fas fa-award"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Cancel"
                                            CommandName="CANCELAPPOINTMENTLETTER" CommandArgument='<%# Container.DataSetIndex %>'
                                            ID="cmdCancelAppointment" ToolTip="Cancel Appointment">
                                            <span class="icon"><i class="fas fa-times-circle-cancel"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" CommandArgument="<%# Container.DataSetIndex %>" CommandName="APPOINTMENTLETTERPDF"
                                            ID="cmdAppointmentLetter" ImageAlign="AbsMiddle"
                                            ToolTip="Show Contract">
                                               <span class="icon"><i class="fas fa-clipboard-list-jd"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton ID="cmdTrainingCourse" runat="server"
                                            CommandArgument="<%#Container.DataSetIndex%>" ToolTip="Training Matrix Requirements"
                                            CommandName="TRAININGMATRIX">
                                              <span class="icon"><i class="fas fa-pencil-ruler"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Delete"
                                            CommandName="DELETE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                            ToolTip="Delete">
                                            <span class="icon"><i class="fa fa-trash"></i></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Save"
                                            CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                            ToolTip="Save">
                                            <span class="icon"><i class="fas fa-save"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Cancel"
                                            CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                            ToolTip="Cancel">
                                            <span class="icon"><i class="fas fa-times-circle"></i></span>
                                        </asp:LinkButton>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="false" UseStaticHeaders="true" SaveScrollPosition="true"  ScrollHeight="" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>

                </div>
                <br />
                <b>
                    <telerik:RadLabel ID="lblSignOffList" runat="server" Text="Sign-Off List"></telerik:RadLabel>
                </b>

                <eluc:TabStrip ID="CrewSignOff" runat="server" OnTabStripCommand="CrewSignOff_TabStripCommand"></eluc:TabStrip>

                <div id="divGridSO" style="position: relative; z-index: 0; width: 100%;">
                    <%--  <asp:GridView ID="gvSignOff" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowDataBound="gvSignOff_RowDataBound" OnRowCommand="gvSignOff_RowCommand"
                        OnRowEditing="gvSignOff_RowEditing" OnRowCancelingEdit="gvSignOff_RowCancelingEdit" AllowSorting="true" OnSorting="gvSignOff_Sorting"
                        OnRowUpdating="gvSignOff_RowUpdating" ShowHeader="true" EnableViewState="false"
                        DataKeyNames="FLDSIGNONOFFID">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvSignOff" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvSignOff_NeedDataSource"
                        OnItemCommand="gvSignOff_ItemCommand"
                        OnSortCommand="gvSignOff_SortCommand"
                        OnItemDataBound ="gvSignOff_ItemDataBound"
                        GroupingEnabled="false" EnableHeaderContextMenu="true"
                        AutoGenerateColumns="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView DataKeyNames="FLDSIGNONOFFID" EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="Top">
                            <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                            <NoRecordsTemplate>
                                <table width="100%" border="0">
                                    <tr>
                                        <td align="center">
                                            <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </NoRecordsTemplate>
                            <HeaderStyle Width="102px" />
                            <Columns>

                                <telerik:GridTemplateColumn HeaderText="Vessel">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblVessel" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDVESSELNAME"]%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="File No">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                  <HeaderStyle Width="100px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblFileNo" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDFILENO"]%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Name">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                              <HeaderStyle Width="200px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblSignOnOffid" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDSIGNONOFFID"]%>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblEmployeeid" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDEMPLOYEEID"]%>'></telerik:RadLabel>
                                        <asp:LinkButton ID="lnkEmployeeName" runat="server" CommandArgument="<%#Container.DataSetIndex%>"
                                            Text='<%# ((DataRowView)Container.DataItem)["FLDNAME"]%>' CommandName="GETEMPLOYEE"></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Rank">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                 <HeaderStyle Width="100px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblRank" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDSIGNONRANKNAME"]%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Nationality">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                 <HeaderStyle Width="100px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblNationality" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDNATIONALITYNAME"]%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Passport No">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                   <HeaderStyle Width="100px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblPassportNo" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDPASSPORTNO"]%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Currency" UniqueName="Currency">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                 <HeaderStyle Width="75px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblItemCurrency" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDCURRENCYCODE"]%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadLabel ID="lbleditCurrency" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDCURRENCYCODE"]%>'></telerik:RadLabel>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Daily Rate" UniqueName="DailyRate">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                               <HeaderStyle Width="75px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDailyRate" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDDAILYRATE"]%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadLabel ID="lblDailyRate" runat="server" Text=' <%# ((DataRowView)Container.DataItem)["FLDDAILYRATE"]%>'></telerik:RadLabel>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Daily DP Allowance">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                 <HeaderStyle Width="75px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDPRate" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDDPALLOWANCE"]%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Sign On Date">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                  <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDateJoined" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDSIGNONDATE")) %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Date Width="100%" ID="txtSignOnDate" runat="server" CssClass="input_mandatory" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDSIGNONDATE"))%>'
                                            AutoPostBack="true" OnTextChangedEvent='txtSignOffDate_TextChanged' />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Sign-Off Date">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                 <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblSignOffDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDSIGNOFFDATE")) %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblSeaPortId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDSIGNOFFSEAPORTID"] %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblSignOnOffIdAdd" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDSIGNONOFFID"] %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadLabel ID="lblSignOnOffIdEdit" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDSIGNONOFFID"] %>'></telerik:RadLabel>
                                        <eluc:Date Width="100%" ID="txtSignOffDate" runat="server" CssClass="input_mandatory" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDSIGNOFFDATE"))%>'
                                            AutoPostBack="true" OnTextChangedEvent='txtSignOffDate_TextChanged' />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Sign-Off Reason">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                   <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblSignoffReason" runat="server" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDSIGNOFFREASON") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblSignoffReasonId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDSIGNOFFREASONID"] %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadLabel ID="lblSignOffIdEdit" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDSIGNOFFREASONID"] %>'></telerik:RadLabel>
                                        <eluc:SignOffReason Width="100%" runat="server" ID="ucSignOffReasonEdit" CssClass="input_mandatory" AppendDataBoundItems="true"
                                            SignOffReasonList='<%#SouthNests.Phoenix.Registers.PhoenixRegistersreasonssignoff.Listreasonssignoff() %>' />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Sign-Off Port">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                   <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblSignOffPort" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDSIGNOFFSEAPORTNAME"]%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:SeaPort runat="server" ID="ddlSeaPort" CssClass="input_mandatory" AppendDataBoundItems="true"
                                            SeaportList='<%#SouthNests.Phoenix.Registers.PhoenixRegistersSeaport.ListSeaport() %>'
                                            SelectedSeaport='<%# ((DataRowView)Container.DataItem)["FLDSIGNOFFSEAPORTID"]%>' />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn Visible="false">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblEstimatedTravelDateHeader" runat="server" Text="Estimated Travel End Date"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblEstimatedTravelDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDETOD")) %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Date ID="txtEstimatedTravelEndDate" runat="server" CssClass="input_mandatory" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDETOD"))%>' />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="End of Contract">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                  <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblReliefDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDRELIEFDUEDATE"))%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadLabel ID="lblReliefDateEdit" runat="server" Visible="true" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDRELIEFDUEDATE"))%>'></telerik:RadLabel>
                                        <eluc:Date Width="100%" ID="txtReliefDateEdit" runat="server" Visible="false" CssClass="input_mandatory" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDRELIEFDUEDATE"))%>' />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Max Tour of Duty">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                 <HeaderStyle Width="75px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblCompletionDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLD90RELIEFDATE"))%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadLabel ID="lbl90ReliefDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLD90RELIEFDATE")) %>' />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Action">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                   <HeaderStyle Width="150px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="EDIT"
                                            CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                            ToolTip="Edit">
                                          <span class="icon"><i class="fas fa-edit"></i></span>
                                        </asp:LinkButton>
                                        
                                        <asp:LinkButton runat="server" AlternateText="Document Checklist"
                                            CommandName="DOCUMENTCHECKLIST" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDocChecklist"
                                            ToolTip="Documents Checklist">
                                             <span class="icon"><i class="fas fa-list-ul"></i></i></span>
                                        </asp:LinkButton>
                                        
                                        <asp:LinkButton runat="server" AlternateText="Approve" 
                                            CommandName="APPROVE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdApprove"
                                            ToolTip="Confirm">
                                             <span class="icon"><i class="fas fa-award"></i></span>
                                        </asp:LinkButton>
                                     
                                        <asp:LinkButton runat="server" AlternateText="Cancel" 
                                            CommandName="CANCELAPPOINTMENTLETTER" CommandArgument='<%# Container.DataSetIndex %>'
                                            ID="cmdCancelAppointment" ToolTip="Cancel Appointment">
                                             <span class="icon"><i class="fas fa-times-circle-cancel"></i></span>
                                        </asp:LinkButton>
                                      
                                        <asp:LinkButton ID="cmdTrainingCourse" runat="server" 
                                            CommandArgument="<%#Container.DataSetIndex%>" ToolTip="Training Matrix Requirements"
                                            CommandName="TRAININGMATRIX" >
                                             <span class="icon"><i class="fas fa-pencil-ruler"></i></span>
                                        </asp:LinkButton>
                                       
                                        <asp:LinkButton runat="server" AlternateText="Raise Relief Request" CommandName="RAISERELIEFREQUEST"
                                            CommandArgument="<%# Container.DataSetIndex %>" 
                                            ID="cmdRaiseReliefRequest" ToolTip="Raise Relief Request">
                                            <span class="icon"><i class="fas fa-sign-out-alt-mc"></i></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Save" 
                                            CommandName="Update" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                            ToolTip="Save">
                                            <span class="icon"><i class="fas fa-save"></i></span>
                                        </asp:LinkButton>
                                    
                                         
                                        <asp:LinkButton runat="server" AlternateText="Cancel" 
                                            CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                            ToolTip="Cancel">
                                            <span class="icon"><i class="fas fa-times-circle"></i></span>
                                        </asp:LinkButton>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="false" UseStaticHeaders="true" SaveScrollPosition="true" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>

                </div>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
