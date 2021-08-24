<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewPlanTravel.aspx.cs" Inherits="Crew_CrewPlanTravel" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TravelReason" Src="~/UserControls/UserControlTravelReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MUCCity" Src="~/UserControls/UserControlMultiColumnCity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonToolTip" Src="~/UserControls/UserControlCommonToolTip.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Change Request</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewChangeRequest" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="85%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="CrewChangeRequestMenu" runat="server" Title="Travel Plan List" OnTabStripCommand="CrewChangeRequest_TabStripCommand"></eluc:TabStrip>
            <table width="100%">
                <tr>
                    <td colspan="4">
                        <table width="100%" cellpadding="2" cellspacing="2">
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtVessel" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="200px"></telerik:RadTextBox>

                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblVesselAccount" runat="server" Text="Vessel Account"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="ddlAccountDetails" runat="server" CssClass="dropdown_mandatory"
                                        OnDataBound="ddlAccountDetails_DataBound" DataTextField="FLDVESSELACCOUNTNAME"
                                        DataValueField="FLDACCOUNTID" Width="130px" EmptyMessage="Type to select"  EnableLoadOnDemand="True"
                                        Filter="Contains" MarkFirstMatch="true" >
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblOrigin" runat="server" Text="Departure"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:MUCCity ID="txtOrigin" runat="server" CssClass="input_mandatory" Width="200px" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="DepatureDate" runat="server" Text="Departure Date"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Date ID="txtorigindate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                                    <telerik:RadComboBox ID="ddlampmOriginDate" runat="server" CssClass="input_mandatory" Width="60px"
                                        EmptyMessage="Type to select"  EnableLoadOnDemand="True" Filter="Contains" MarkFirstMatch="true">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="--" Value="" />
                                            <telerik:RadComboBoxItem Text="AM" Value="1" />
                                            <telerik:RadComboBoxItem Text="PM" Value="2" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblDestination" runat="server" Text="Destination"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:MUCCity ID="txtDestination" runat="server" CssClass="input_mandatory" Width="200px" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblArrivalDate" runat="server" Text="Arrival Date"></telerik:RadLabel>

                                </td>
                                <td>
                                    <eluc:Date ID="txtDestinationdate" runat="server"  DatePicker="true" />
                                    <telerik:RadComboBox ID="ddlampmarrival" runat="server" Width="50px"
                                        EmptyMessage="Type to select"  EnableLoadOnDemand="True" Filter="Contains" MarkFirstMatch="true">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="--" Value="" />
                                            <telerik:RadComboBoxItem Text="AM" Value="1" />
                                            <telerik:RadComboBoxItem Text="PM" Value="2" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblPurpose" runat="server" Text="Purpose"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:TravelReason ID="ucpurpose" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                        ReasonFor="2" Width="200px" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="Literal1" runat="server" Text="Payment Mode"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Hard ID="Payment" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                        HardList="<%# PhoenixRegistersHard.ListHard(1,185) %>" HardTypeCode="185" Width="130px" SelectedHard="976" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>        
            <eluc:TabStrip ID="MenuCrewChangePlan" runat="server" OnTabStripCommand="ChangePlan_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvCCPlan" runat="server" AllowCustomPaging="true" AllowSorting="true" Height="65%"
                CellSpacing="0" GridLines="None" OnItemCommand="gvCCPlan_ItemCommand" OnNeedDataSource="gvCCPlan_NeedDataSource"
                OnItemDataBound="gvCCPlan_ItemDataBound" EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true" AllowPaging="true">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
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
                        <telerik:GridTemplateColumn HeaderText="Select">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblSelect" runat="server"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="false" HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemTemplate>
                                <telerik:RadCheckBox runat="server" ID="chkOffSigner" />
                                <eluc:CommonToolTip ID="ucCommonToolTip" runat="server" Screen='<%#"Crew/CrewTravelMoreInfoList.aspx?empId=" + DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERID").ToString() +"&familyId="%>' />
                                <asp:LinkButton runat="server" ID="cmdSetDestination" AlternateText="Destination" Width="20PX" Height="20PX"
                                    CommandName="SETDESTINATION" CommandArgument="<%# Container.DataSetIndex %>" ToolTip="Set Home Destination">                                
                                <span class="icon"><i class="far fa-check-circle-shd"></i></span>
                                </asp:LinkButton>
                                <telerik:RadLabel runat="server" ID="lblOffSignerCrewChangeNotReq" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERCREWCHANGENOTREQUEST") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="File No.">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="15%"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblFileNo" runat="server" Text="File No."></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDOFFSIGNERFILENO")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Off-Signer">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="25%"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblOffSigner" runat="server" Text="Off-Signer"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOffSignerId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lbloffsignerrankname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERRANK") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lbloffsignercon" runat="server" Text=" - "></telerik:RadLabel>
                                <asp:LinkButton ID="lnkOffSigner" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERNAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Tick">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblTick" runat="server"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="false" HorizontalAlign="Center" VerticalAlign="Middle" Width="10%" />
                            <ItemTemplate>
                                <telerik:RadCheckBox runat="server" ID="chkOnSigner" />
                                <eluc:CommonToolTip ID="ucCommonToolTiponsigner" runat="server" Screen='<%# "Crew/CrewTravelMoreInfoList.aspx?empId=" + DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID").ToString() +"&familyId="%>' />
                                <asp:LinkButton runat="server" ID="cmdSetOrigin" AlternateText="Departure" Width="20PX" Height="20PX"
                                    CommandName="SETORIGIN" CommandArgument="<%# Container.DataSetIndex %>" ToolTip="Set Departure">                                
                                <span class="icon"><i class="far fa-check-circle-sd"></i></span>
                                </asp:LinkButton>
                                <telerik:RadLabel runat="server" ID="lblOnSignerCrewChangeNotReq" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONSIGNERCREWCHANGENOTREQUEST") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="File No.">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="15%"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblRank" runat="server" Text="File No."></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDFILENO")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="On-Signer">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="25%"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblCrewHeader" runat="server" Text="On-Signer"></telerik:RadLabel>
                            </HeaderTemplate>
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
                                <telerik:RadLabel ID="lblonsignerrankname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblonsignercon" runat="server" Text=" - "></telerik:RadLabel>
                                <asp:LinkButton ID="lnkOnSigner" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONSIGNERNAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>           
        </telerik:RadAjaxPanel>
         <eluc:Status ID="ucStatus" runat="server" />
    </form>
</body>
</html>
