<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewChangeRequest.aspx.cs"
    Inherits="CrewChangeRequest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TravelReason" Src="~/UserControls/UserControlTravelReason.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Change Request</title>
    <style type="text/css">
        .style1 {
            height: 22px;
        }
    </style>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmCrewChangeRequest" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="CCPMenu" runat="server" Title="Crew Change Request" OnTabStripCommand="CCPMenu_TabStripCommand"></eluc:TabStrip>
            <eluc:TabStrip ID="CrewChangeRequestMenu" runat="server" TabStrip="false" OnTabStripCommand="CrewChangeRequest_TabStripCommand"></eluc:TabStrip>

            <table width="100%" cellpadding="1" cellspacing="1" style="color: Blue">
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblNotes" runat="server" Text="Notes :"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;<telerik:RadLabel ID="lbl1PleaseentertheVesselNameDateofCrewChangeandCrewChangePort"
                        runat="server" Text="1. Please enter the Vessel Name,Date of Crew Change and Crew Change Port.">
                        </telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;<telerik:RadLabel ID="lbl2PleaseentertheplaceofresidenceoftheseafarerandselecttheCrewChangeReason"
                        runat="server" Text="2. Please enter the place of residence of the seafarer and select the Crew Change Reason.">
                        </telerik:RadLabel>
                    </td>
                </tr>
            </table>
            <br />
            <table cellspacing="1" cellpadding="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselName" runat="server" Text="Vessel Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVessel" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="200px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselAccount" runat="server" Text="Vessel Account"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlAccountDetails" runat="server" CssClass="dropdown_mandatory"
                            OnDataBound="ddlAccountDetails_DataBound" DataTextField="FLDVESSELACCOUNTNAME"
                            DataValueField="FLDACCOUNTID" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type of select">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDateofCrewChange" runat="server" Text="Date of Crew Change"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDateOfCrewChange" runat="server" CssClass="input_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCrewChangePort" runat="server" Text="Crew Change Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Port ID="ucport" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCity" runat="server" Text="City"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListAirportdestinationedit">
                            <telerik:RadTextBox ID="txtcityname" runat="server" Width="25%" Enabled="False" CssClass="input_mandatory"></telerik:RadTextBox>
                            <asp:LinkButton ID="btnShowDestinationedit" runat="server" 
                                ImageAlign="Top" Text=".." OnClientClick="return showPickList('spnPickListAirportdestinationedit', 'codehelp1', '', 'Common/CommonPickListCity.aspx', true); " >
                                <span class="icon"><i class="fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtcityid" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCrewChangeReason" runat="server" Text="Crew Change Reason"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:TravelReason ID="ucCrewChangeReason" runat="server" AppendDataBoundItems="true"
                            CssClass="dropdown_mandatory" ReasonFor="1" />
                    </td>
                </tr>
            </table>
            <hr />
            <div id="divGrid" style="position: relative; z-index: 10; width: 100%;">
                <%--  <asp:GridView ID="gvCCPlan" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" ShowHeader="true" OnRowDataBound="gvCCPlan_RowDataBound"
                    OnRowCommand="gvCCPlan_OnRowCommand" EnableViewState="false" AllowSorting="true"
                    OnSorting="gvCCPlan_Sorting">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />--%>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvCCPlan" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvCCPlan_NeedDataSource"
                    OnItemCommand="gvCCPlan_ItemCommand"
                    OnItemDataBound="gvCCPlan_ItemDataBound"
                    OnSortCommand="gvCCPlan_SortCommand"
                    GroupingEnabled="false" EnableHeaderContextMenu="true"
                    AutoGenerateColumns="false">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed">
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

                            <telerik:GridTemplateColumn HeaderText="Tick">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                             
                                <ItemStyle Wrap="false" HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="chkOffSigner" />
                                    <telerik:RadLabel runat="server" ID="lblOffSignerCrewChange" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERCREWCHANGE") %>'></telerik:RadLabel>
                                    <telerik:RadLabel runat="server" ID="lblOffSignerCrewChangeNotReq" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERCREWCHANGENOTREQUEST") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Off-Signer">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30%"></HeaderStyle>
                    
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblOffSignerId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERID") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                    <asp:LinkButton ID="lnkOffSigner" runat="server" CommandArgument="SELECT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERNAME") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Tick">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                           
                                <ItemStyle Wrap="false" HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="chkOnSigner" />
                                    <telerik:RadLabel runat="server" ID="lblOnSignerCrewChange" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONSIGNERCREWCHANGE") %>'></telerik:RadLabel>
                                    <telerik:RadLabel runat="server" ID="lblOnSignerCrewChangeNotReq" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONSIGNERCREWCHANGENOTREQUEST") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="On-Signer">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30%"></HeaderStyle>
                            
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCrewPlanId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWPLANID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblEmployeeId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                    <telerik:RadLabel ID="lblRankId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                    <telerik:RadLabel ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                    <telerik:RadLabel ID="lblDocumentsReq" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTSREQUIRED") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                    <telerik:RadLabel ID="lblEmployee" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                    <asp:LinkButton ID="lnkEmployee" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'
                                        CommandName="SELECT" CommandArgument='<%# Container.DataSetIndex %>'></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Rank">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20%"></HeaderStyle>
                               
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDRANKNAME")%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
                
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
