<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreReliefPlan.aspx.cs"
    Inherits="CrewOffshoreReliefPlan" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeaPort" Src="~/UserControls/UserControlSeaport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonToolTip" Src="~/UserControls/UserControlCommonToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Query Activity</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <%--  <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= Gvplanningalert.ClientID %>"));
                }, 200);
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>--%>
         <style type="text/css">
        .bluefont {
            color: blue  !important;  
        }

       
    </style>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

            <eluc:TabStrip ID="CrewRelieverTabs" runat="server" OnTabStripCommand="CrewRelieverTabs_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>

            <div style="fon">
                <table cellpadding="2" cellspacing="2" width="100%">
                    <tr>
                        <td style="width: 60px">
                            <asp:Label ID="lblVessel" runat="server" Text="Vessel"></asp:Label>
                        </td>
                        <td style="width: 90px">
                            <eluc:Vessel ID="ddlVessel" runat="server" VesselsOnly="true"
                                AssignedVessels="true" AutoPostBack="true" OnTextChangedEvent="ddlVessel_TextChanged" Width="150px" />
                            <%-- --%>
                            <asp:TextBox ID="txtVesselName" runat="server" ReadOnly="true" Visible="false"></asp:TextBox>
                        </td>
                        <td style="width: 60px">
                            <asp:Label ID="lblRank" runat="server" Text="Rank"></asp:Label>
                        </td>
                        <td style="width: 90px">
                            <eluc:Rank ID="ucRank" runat="server"
                                AutoPostBack="true" OnTextChangedEvent="ucRank_TextChanged" Width="150px" />
                            <%-- --%>
                            <asp:TextBox ID="txtRank" runat="server" ReadOnly="true" Visible="false"></asp:TextBox>
                        </td>
                        <td style="width: 60px">
                            <asp:Label ID="lblDays" runat="server" Text="Relief Due (days)"></asp:Label>
                        </td>
                        <td style="width: 90px">
                            <eluc:Number ID="txtReliefDue" runat="server" CssClass="input txtNumber" MaxLength="3"
                                IsInteger="true" />
                            <asp:LinkButton ID="imgSearch" runat="server"
                                OnClick="imgSearch_Click" ToolTip="Search">
                                    <span class="icon"><i class="fas fa-search"></i></span>
                            </asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>

            <asp:Label ID="lblgvtitle" runat="server" Text="Relief Planner Alert" Font-Size="8" Font-Bold="true"></asp:Label>
            <%--<asp:GridView ID="Gvplanningalert" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowHeader="true"
                        EnableViewState="false"
                        AllowSorting="true" OnRowDataBound="Gvplanningalert_RowDataBound">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />--%>
            <telerik:RadGrid RenderMode="Lightweight" ID="Gvplanningalert" runat="server" Height="" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="Gvplanningalert_NeedDataSource"
                OnItemCommand="Gvplanningalert_ItemCommand"
                GroupingEnabled="false" EnableHeaderContextMenu="true"
                AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />

                    <Columns>

                        <telerik:GridTemplateColumn HeaderText="Vessel">
                            <ItemTemplate>
                                <asp:Label ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDVESSELNAME") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="MST and CE planned">
                            <ItemTemplate>
                                <asp:Label ID="lblMSTandCEplanned" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMSTANDCEYN") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CE and 2E planned">
                            <ItemTemplate>
                                <asp:Label ID="lblCEand2Eplanned" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCEAND2EYN") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Planned Deck Officers(%)">
                            <ItemTemplate>
                                <asp:Label ID="lblPlannedDeckofficers" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDECKOFFICERPLANNEDPRS")  %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Planned Engine Officers(%)">
                            <ItemTemplate>
                                <asp:Label ID="lblPlannedEngineofficers" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDENGINEOFFICEPLANNEDPRS")  %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Total Crew Planned(%)">
                            <ItemTemplate>
                                <asp:Label ID="lblplannedcrew" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTOTALPLANEDCREWPRS") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="false" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

            <br />

            <eluc:TabStrip ID="QueryMenu" runat="server" OnTabStripCommand="QueryMenu_TabStripCommand"></eluc:TabStrip>


            <%-- <asp:GridView ID="gvSearch" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        OnRowCreated="gvSearch_RowCreated" Width="100%" CellPadding="3" ShowHeader="true"
                        OnRowDataBound="gvSearch_RowDataBound" OnRowEditing="gvSearch_RowEditing" EnableViewState="false"
                        AllowSorting="true" OnSorting="gvSearch_Sorting" OnRowDeleting="gvSearch_RowDeleting"
                        OnRowUpdating="gvSearch_RowUpdating" OnRowCancelingEdit="gvSearch_RowCancelingEdit"
                        OnSelectedIndexChanging="gvSearch_SelectedIndexChanging" OnRowCommand="gvSearch_RowCommand">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />--%>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvSearch" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvSearch_NeedDataSource"
                OnItemCommand="gvSearch_ItemCommand"
                OnItemDataBound="gvSearch_ItemDataBound"
                OnSortCommand="gvSearch_SortCommand"
                GroupingEnabled="false" EnableHeaderContextMenu="true"
                AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="Top">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Current Crew On Board" Name="onboard" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Planned Crew" Name="planned" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <%--<NestedViewSettings>
                        <ParentTableRelation>
                            <telerik:GridRelationFields MasterKeyField="FLDCREWPLANID" DetailKeyField="FLDCREWPLANID" />
                        </ParentTableRelation>
                    </NestedViewSettings>--%>
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="false" ShowAddNewRecordButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="" HeaderStyle-Width="35px">
                            <ItemStyle Wrap="false" HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemTemplate>
                                <telerik:RadCheckBox runat="server" ID="chkPlan" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Event Ref. No.">
                            <HeaderStyle Width="100px" />
                            <ItemTemplate>
                               <%-- <asp:Label ID="lblEvent" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDEVENTDATE")) %>'></asp:Label>--%>
                                 <asp:Label ID="lblEventRef" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREFERENCENO") %>'></asp:Label>                         
                                <asp:Label ID="lblEventId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWEVENTID") %>'></asp:Label>
                                <asp:Label ID="lblSignonoffid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNONOFFID") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel">
                            <HeaderStyle Width="150px" />
                            <ItemTemplate>

                                <asp:Label ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDVESSELNAME") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="OffSigner " ColumnGroupName="onboard">
                            <HeaderStyle Width="150px" />
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkRelievee" CommandName="SELECTROW" CommandArgument='<%# Container.DataSetIndex %>'
                                    runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:LinkButton>
                                <asp:Label ID="lblCrewPlanId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWPLANID") %>'></asp:Label>
                                <asp:Label ID="lblName" runat="server" Visible="false" Text=' <%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank" ColumnGroupName="onboard">
                            <HeaderStyle Width="50px" />
                            <ItemTemplate>
                                <asp:Label ID="lblEmployeeId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'
                                    Visible="false">
                                </asp:Label>
                                <asp:Label ID="lblRankId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>'
                                    Visible="false">
                                </asp:Label>
                                <asp:Label ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'
                                    Visible="false">
                                </asp:Label>
                                <asp:Label ID="lblVesseltype" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE") %>'
                                    Visible="false">
                                </asp:Label>
                                <asp:Label ID="lblTrainingmatrixid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAININGMATRIXID") %>'></asp:Label>
                                <asp:Label ID="lblJoinDate" runat="server" Text=' <%# DataBinder.Eval(Container, "DataItem.FLDEXPECTEDJOINDATE", "{0:dd/MMM/yyyy}") %>'
                                    Visible="false">
                                </asp:Label>
                                <asp:Label ID="lnkRank" runat="server" CommandArgument="<%#Container.DataSetIndex%>"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Nationality" ColumnGroupName="onboard">
                            <HeaderStyle Width="75px" />
                            <ItemTemplate>
                                <asp:Label ID="lblOffsignerNationality" runat="server" Text=' <%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERNATIONALITY") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Addl. Cert." ColumnGroupName="onboard">
                            <HeaderStyle Width="50px" />
                            <ItemTemplate>
                                <asp:Label Visible="false" ID="lblOffSignerCertificate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOFFSIGNERCERTIFICATE").ToString().Length>20 ? HttpContext.Current.Server.HtmlDecode(DataBinder.Eval(Container, "DataItem.FLDOFFSIGNERCERTIFICATE").ToString()).ToString().Substring(0, 20)+ "..." : HttpContext.Current.Server.HtmlDecode(DataBinder.Eval(Container, "DataItem.FLDOFFSIGNERCERTIFICATE").ToString()) %>'></asp:Label>
                                <eluc:ToolTip Visible="false" ID="ucToolTipOffSignerCertificate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOFFSIGNERCERTIFICATE") %>' />
                                <img id="imgAddCertOffSigner" runat="server" style="cursor: hand" src="<%$ PhoenixTheme:images/annexure.png %>"
                                    alt="AddCert" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Daily Rate" ColumnGroupName="onboard">
                            <HeaderStyle Width="60px" />
                            <ItemTemplate>
                                <asp:Label ID="lblcurrencyonboard" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCURRENCYCODEONBOARD")%>'></asp:Label>
                                <asp:Label ID="lblDailyRate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOFFSIGNERDAILYRATE")%>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Daily DP Allowance" ColumnGroupName="onboard">
                            <HeaderStyle Width="50px" />
                            <ItemTemplate>
                                <asp:Label ID="lblDPRate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOFFSIGNERDPALLOWANCE")%>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sign on" ColumnGroupName="onboard">
                            <HeaderStyle Width="75px" />
                            <ItemTemplate>
                                <asp:Label ID="lblDateJoined" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDOFFSIGNERJOINDATE")) %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Days Onboard" ColumnGroupName="onboard">
                            <HeaderStyle Width="50px" />
                            <ItemTemplate>
                                <asp:Label ID="lblOnboardDays" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDAYSONBOARD") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="End of contract" ColumnGroupName="onboard">
                            <HeaderStyle Width="75px" />
                            <ItemTemplate>
                                <asp:Label ID="lblReliefDue" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDRELIEFDUEDATE")) %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Max Tour of Duty" ColumnGroupName="onboard">
                            <HeaderStyle Width="50px" />
                            <ItemTemplate>
                                <asp:Label ID="lbl90day" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDOFFSIGNER90DAYSRELIEF")) %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>



                        <telerik:GridTemplateColumn HeaderText="Reliever Name" ColumnGroupName="planned">
                            <HeaderStyle Width="100px" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblRelieverId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRELIEVERID") %>'
                                    Visible="false">
                                </asp:Label>
                                <asp:LinkButton ID="lnkReliever" CommandName="SELECTROW" CommandArgument='<%# Container.DataSetIndex %>'
                                    runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRELIEVERNAME") %>'></asp:LinkButton>
                                <asp:Label ID="lblRelieverName" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRELIEVERNAME") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Nationality" ColumnGroupName="planned">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblRelieverNationality" runat="server" Text=' <%# DataBinder.Eval(Container,"DataItem.FLDRELIEVERNATIONALITY") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="WD" ColumnGroupName="planned">
                            <HeaderStyle Width="30px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <eluc:CommonToolTip ID="ucCommonToolTip" runat="server" Screen='<%# "CrewOffshore/CrewOffshoreToolTipWaivedDocuments.aspx?crewplanid=" + DataBinder.Eval(Container,"DataItem.FLDCREWPLANID").ToString() %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Proposed Rank" ColumnGroupName="planned">
                            <HeaderStyle Width="50px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblRelieverRankId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRELIEVERRANKID") %>'
                                    Visible="false">
                                </asp:Label>
                                <asp:Label ID="RadLabel14" runat="server" Text=' <%# DataBinder.Eval(Container,"DataItem.FLDRELIEVERRANK") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="P" ColumnGroupName="planned">
                            <HeaderStyle Width="20px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Image ID="ImgFlagP" runat="server" alt="" ImageUrl="<%$ PhoenixTheme:images/Red.png %>"
                                    Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="A" ColumnGroupName="planned">
                            <HeaderStyle Width="20px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Image ID="ImgFlagA" runat="server" alt="" ImageUrl="<%$ PhoenixTheme:images/Red.png %>"
                                    Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="T" ColumnGroupName="planned">
                            <HeaderStyle Width="20px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Image ID="ImgFlagT" runat="server" alt="" ImageUrl="<%$ PhoenixTheme:images/Red.png %>"
                                    Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="S" ColumnGroupName="planned">
                            <HeaderStyle Width="20px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Image ID="ImgFlagS" runat="server" alt="" ImageUrl="<%$ PhoenixTheme:images/Red.png %>"
                                    Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Addl. Cert." ColumnGroupName="planned">
                            <HeaderStyle Width="30px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblOnSignerCertificate" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDONSIGNERCERTIFICATE").ToString().Length>20 ? HttpContext.Current.Server.HtmlDecode(DataBinder.Eval(Container, "DataItem.FLDONSIGNERCERTIFICATE").ToString()).ToString().Substring(0, 20)+ "..." : HttpContext.Current.Server.HtmlDecode(DataBinder.Eval(Container, "DataItem.FLDONSIGNERCERTIFICATE").ToString()) %>'></asp:Label>
                                <eluc:ToolTip ID="ucToolTipOnSignerCertificate" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDONSIGNERCERTIFICATE")%>' />
                                <img id="imgAddCertOnSigner" runat="server" style="cursor: hand" src="<%$ PhoenixTheme:images/annexure.png %>"
                                    alt="AddCert" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Daily Rate" ColumnGroupName="planned">
                            <HeaderStyle Width="60px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblcurrency" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCURRENCYCODE")%>'></asp:Label>
                                <asp:Label ID="lblDailyRateReliever" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDRELIEVERDAILYRATE")%>'></asp:Label>
                                <eluc:ToolTip ID="ucToolTipNW" runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Expected Join Date" ColumnGroupName="planned" AllowSorting="true" SortExpression="FLDEXPECTEDJOINDATE">
                            <HeaderStyle Width="150px" />
                            <ItemTemplate>
                                <asp:Label ID="lblReliefDue1" Visible="false" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDRELIEFDUEDATE")) %>'></asp:Label>
                                <asp:Label ID="lblJoiningDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDEXPECTEDJOINDATE")) %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date Width="100%" ID="txtJoiningDate" runat="server" CssClass="input_mandatory" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDEXPECTEDJOINDATE")) %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Joining Port" ColumnGroupName="planned">
                            <HeaderStyle Width="150px" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDSEAPORTNAME")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:SeaPort ID="ddlPlannedPort" Width="100%" runat="server" AppendDataBoundItems="true"
                                    SeaportList="<%#SouthNests.Phoenix.Registers.PhoenixRegistersSeaport.ListSeaport() %>"
                                    SelectedSeaport='<%# DataBinder.Eval(Container, "DataItem.FLDSEAPORTID") %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks" ColumnGroupName="planned">
                            <HeaderStyle Width="60px" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:Label ID="lblPDStatusID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPDSTATUSID")%>'></asp:Label>
                                <asp:Label ID="lblRemarks" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPROPOSALREMARKS").ToString().Length>20 ? HttpContext.Current.Server.HtmlDecode(DataBinder.Eval(Container, "DataItem.FLDPROPOSALREMARKS").ToString()).ToString().Substring(0, 20)+ "..." : HttpContext.Current.Server.HtmlDecode(DataBinder.Eval(Container, "DataItem.FLDPROPOSALREMARKS").ToString()) %>'></asp:Label>
                                <asp:LinkButton ID="imgRemarks" runat="server"
                                    CommandArgument='<%# Container.DataSetIndex %>'>
                                    <span class="icon"><i class="fas fa-glasses"></i></span>
                                </asp:LinkButton>
                                <eluc:ToolTip ID="ucToolTipAddress" Width="200px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPROPOSALREMARKS") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Offshore Stage / Employee Status" ColumnGroupName="planned">
                            <HeaderStyle Width="100px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblPDStatus" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPDSTATUS")%>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Offer Letter Signed" ColumnGroupName="planned">
                            <HeaderStyle Width="50px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblOfferLetterSigned" runat="server"
                                    Text='<%# (DataBinder.Eval(Container,"DataItem.FLDOFFERLETTERSIGNEDYN"))%>'
                                    Visible="false">
                                </asp:Label>
                                <asp:Label ID="lblOfferLetter" runat="server"
                                    Text='<%# (DataBinder.Eval(Container,"DataItem.FLDOFFERLETTERSIGNEDYN").ToString().Equals("1"))?"Yes":"No" %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkOfferLetterSigned" runat="server"
                                    Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDOFFERLETTERSIGNEDYN").ToString().Equals("1"))?true:false %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <HeaderStyle Width="300px"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                    ToolTip="Edit">
                                     <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="Delete" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                    ToolTip="De-Plan">
                                    <span class="icon"><i class="fas fa-calendar-times-deplan"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Show"
                                    CommandName="SHOW" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdShow"
                                    ToolTip="Show Reliever">
                                    <span class="icon"><i class="fas fa-check-square-showlist"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="PD Form"
                                    ID="cmdPDForm" CommandName="PDFORM" ToolTip="PD Form">
                                     <span class="icon"><i class="fas fa-file"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Offer Letter"
                                    ID="cmdOfferLetter" Visible="false" CommandName="OFFERLETTER" CommandArgument="<%# Container.DataSetIndex %>"
                                    ToolTip="Offer Letter">
                                    <span class="icon"><i class="fas fa-file-signature-ol"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Document Checklist"
                                    CommandName="DOCUMENTCHECKLIST" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDocChecklist"
                                    ToolTip="Documents Checklist" Visible="false">
                                     <span class="icon"><i class="fas fa-list-ul"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Document Checklist"
                                    CommandName="DOCUMENTCHECKLISTMAIL" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDocumentCheckLIstmail"
                                    ToolTip="Documents Checklist E-Mail" Visible="false">
                                    <span class="icon"><i class="fas fa-envelope"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Approve Travel"
                                    ID="cmdApproveTravel" Visible="false" CommandName="APPROVETRAVEL" CommandArgument="<%# Container.DataSetIndex %>"
                                    ToolTip="Approve Travel">
                                    <span class="icon"><i class="fas fa-plane-departure"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Generate Appointment Letter"
                                    ID="cmdAppLetter" Visible="false" CommandName="APPOINTMENTLETTER" CommandArgument="<%# Container.DataSetIndex %>"
                                    ToolTip="Appointment Letter">
                                    <span class="icon"><i class="fas fa-file-signature-ol"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Cancel"
                                    CommandName="CANCELAPPOINTMENTLETTER" CommandArgument='<%# Container.DataSetIndex %>'
                                    ID="cmdCancelAppointment" Visible="false" ToolTip="Cancel Appointment">
                                    <span class="icon"><i class="fas fa-times-circle-cancel"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Approve Sign/On"
                                    ID="cmdApproveSignOn" Visible="false" CommandName="APPROVESIGNON" CommandArgument="<%# Container.DataSetIndex %>"
                                    ToolTip="Approve Sign/On">
                                    <span class="icon"><i class="fas fa-award"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Crew Change Request"
                                    ID="cmdIniTravel" Visible="false" CommandName="CREWCHANGEREQUEST" CommandArgument="<%# Container.DataSetIndex %>"
                                    ToolTip="Travel Request">
                                    <span class="icon"> <i class="fas fa-plane"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Medical Request"
                                    CommandName="MEDICALREQUEST" CommandArgument='<%# Container.DataSetIndex %>'
                                    ID="cmdMedical" ToolTip="Initiate Medical Request" Visible="false">
                                    <span class="icon"><i class="fas fa-briefcase-medical"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Course Request"
                                    CommandName="COURSEREQUEST" CommandArgument='<%# Container.DataSetIndex %>'
                                    ID="cmdCourse" ToolTip="Initiate Course Request" Visible="false">
                                    <span class="icon"><i class="fas fa-book"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" Visible="false" CommandArgument="<%# Container.DataSetIndex %>"
                                    CommandName="APPOINTMENTLETTERPDF" ID="cmdAppointmentLetter"
                                    ImageAlign="AbsMiddle" Text=".." ToolTip="Show Pdf">
                                    <span class="icon"><i class="fas fa-file-pdf"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton ID="cmdRemark" CommandName="REMARK" runat="server"
                                    ToolTip="Remarks" AlternateText="Remarks" CommandArgument='<%# Container.DataSetIndex %>'>
                                    <span class="icon"><i class="fas fa-clipboard-list"></i></span>
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
                            <FooterStyle HorizontalAlign="Center" />
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <%--<NestedViewTemplate>
                        <table style="font-size: 11px; width: 60%">
                            <tr>
                                <td rowspan="3">
                                    <asp:Label ID="RadLabel1" runat="server" Text="On Signer" Style="font-weight: bold; text-decoration: underline; font-style: italic"></asp:Label>
                                </td>

                                <td>
                                    <asp:Label ID="RadLabel3" runat="server" Text='Nationality' Style="font-weight: bold;"></asp:Label>
                                </td>
                                <td style="width: 1%"><b>:</b></td>
                                <td>
                                    <asp:Label ID="lblOffsignerNationality" runat="server" Text=' <%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERNATIONALITY") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="RadLabel4" runat="server" Text='Daily Rate' Style="font-weight: bold;"></asp:Label>
                                </td>
                                <td style="width: 1%"><b>:</b></td>
                                <td>
                                    <asp:Label ID="lblcurrencyonboard" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCURRENCYCODEONBOARD")%>'></asp:Label>
                                    <asp:Label ID="RadLabel12" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOFFSIGNERJOINDATE")%>'></asp:Label>
                                    <asp:Label ID="RadLabel15" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDVESSELID")%>'></asp:Label>

                                    <asp:Label ID="lblDailyRate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOFFSIGNERDAILYRATE")%>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="RadLabel5" runat="server" Text='DPAllowance' Style="font-weight: bold;"></asp:Label>
                                </td>
                                <td style="width: 1%"><b>:</b></td>
                                <td>
                                    <asp:Label ID="lblDPRate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOFFSIGNERDPALLOWANCE")+ " (Daily)"%>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="RadLabel7" runat="server" Text='Onboard' Style="font-weight: bold;"></asp:Label>
                                </td>
                                <td style="width: 1%"><b>:</b></td>
                                <td>
                                    <asp:Label ID="lblOnboardDays" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDAYSONBOARD")+" (Days)" %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>

                                <td>
                                    <asp:Label ID="RadLabel9" runat="server" Text='Contract End' Style="font-weight: bold;"></asp:Label>
                                </td>
                                <td style="width: 1%"><b>:</b></td>
                                <td>
                                    <asp:Label ID="lblReliefDue" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDRELIEFDUEDATE")) %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="RadLabel11" runat="server" Text='Max Tour' Style="font-weight: bold;"></asp:Label>
                                </td>
                                <td style="width: 1%"><b>:</b></td>
                                <td>
                                    <asp:Label ID="lbl90day" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDOFFSIGNER90DAYSRELIEF")) %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="RadLabel2" runat="server" Text='Addl. Cert.' Style="font-weight: bold;"></asp:Label>
                                </td>
                                <td style="width: 1%"><b>:</b></td>
                                <td>
                                    <asp:Label Visible="false" ID="lblOffSignerCertificate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOFFSIGNERCERTIFICATE").ToString().Length>20 ? HttpContext.Current.Server.HtmlDecode(DataBinder.Eval(Container, "DataItem.FLDOFFSIGNERCERTIFICATE").ToString()).ToString().Substring(0, 20)+ "..." : HttpContext.Current.Server.HtmlDecode(DataBinder.Eval(Container, "DataItem.FLDOFFSIGNERCERTIFICATE").ToString()) %>'></asp:Label>
                                    <eluc:ToolTip Visible="false" ID="ucToolTipOffSignerCertificate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOFFSIGNERCERTIFICATE") %>' />
                                    <img id="imgAddCertOffSigner" runat="server" style="cursor: hand" src="<%$ PhoenixTheme:images/annexure.png %>"
                                        alt="AddCert" /></td>
                            </tr>

                            <tr>
                            </tr>
                            <tr>
                                <td rowspan="2">
                                    <asp:Label ID="RadLabel10" runat="server" Text="Reliever" Style="font-weight: bold; text-decoration: underline; font-style: italic"></asp:Label>
                                </td>

                                <td>
                                    <asp:Label ID="RadLabel8" runat="server" Text='Nationality' Style="font-weight: bold;"></asp:Label>
                                </td>
                                <td style="width: 1%"><b>:</b></td>
                                <td>
                                    <asp:Label ID="lblRelieverNationality" runat="server" Text=' <%# DataBinder.Eval(Container,"DataItem.FLDRELIEVERNATIONALITY") %>'></asp:Label>
                                </td>
                                <td>

                                    <asp:Label ID="RadLabel16" runat="server" Text='Daily Rate' Style="font-weight: bold;"></asp:Label>
                                </td>
                                <td style="width: 1%"><b>:</b></td>
                                <td>
                                    <asp:Label ID="lblcurrency" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCURRENCYCODE")%>'></asp:Label>
                                    <asp:Label ID="lblDailyRateReliever" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDRELIEVERDAILYRATE")%>'></asp:Label>
                                    <eluc:ToolTip ID="ucToolTipNW" runat="server" />
                                </td>
                                <td>
                                    <asp:Label ID="RadLabel6" runat="server" Text='Addl. Cert.' Style="font-weight: bold;"></asp:Label>
                                </td>
                                <td style="width: 1%"><b>:</b></td>
                                <td>
                                    <asp:Label ID="lblOnSignerCertificate" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDONSIGNERCERTIFICATE").ToString().Length>20 ? HttpContext.Current.Server.HtmlDecode(DataBinder.Eval(Container, "DataItem.FLDONSIGNERCERTIFICATE").ToString()).ToString().Substring(0, 20)+ "..." : HttpContext.Current.Server.HtmlDecode(DataBinder.Eval(Container, "DataItem.FLDONSIGNERCERTIFICATE").ToString()) %>'></asp:Label>
                                    <eluc:ToolTip ID="ucToolTipOnSignerCertificate" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDONSIGNERCERTIFICATE")%>' />
                                    <img id="imgAddCertOnSigner" runat="server" style="cursor: hand" src="<%$ PhoenixTheme:images/annexure.png %>"
                                        alt="AddCert" /></td>
                                <td>
                                    <asp:Label ID="RadLabel19" runat="server" Text='Stage/Status' Style="font-weight: bold;"></asp:Label>
                                </td>
                                <td style="width: 1%"><b>:</b></td>
                                <td>
                                    <asp:Label ID="lblPDStatus" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPDSTATUS")%>'></asp:Label>

                                </td>

                            </tr>

                            <tr>

                                <td>
                                    <asp:Label ID="lblRelieverId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRELIEVERID") %>'
                                        Visible="false">
                                    </asp:Label>
                                    <asp:Label ID="lblTrainingmatrixid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAININGMATRIXID") %>'></asp:Label>
                                    <asp:Label ID="RadLabel13" runat="server" Text='Proposal Stage' Style="font-weight: bold;"></asp:Label>
                                </td>
                                <td style="width: 1%"><b>:</b></td>
                                <td>
                                    <asp:Image ID="ImgFlagP" runat="server" alt="" ImageUrl="<%$ PhoenixTheme:images/Red.png %>"
                                        Visible="false" />
                                </td>
                                <td>
                                    <asp:Label ID="RadLabel17" runat="server" Text='Approval Stage' Style="font-weight: bold;"></asp:Label>
                                </td>
                                <td style="width: 1%"><b>:</b></td>
                                <td>
                                    <asp:Image ID="ImgFlagA" runat="server" alt="" ImageUrl="<%$ PhoenixTheme:images/Red.png %>"
                                        Visible="false" />
                                </td>
                                <td>
                                    <asp:Label ID="RadLabel20" runat="server" Text='Travel Stage' Style="font-weight: bold;"></asp:Label>
                                </td>
                                <td style="width: 1%"><b>:</b></td>
                                <td>
                                    <asp:Image ID="ImgFlagT" runat="server" alt="" ImageUrl="<%$ PhoenixTheme:images/Red.png %>"
                                        Visible="false" />
                                </td>
                                <td>
                                    <asp:Label ID="RadLabel22" runat="server" Text='SignOn Stage' Style="font-weight: bold;"></asp:Label>
                                </td>
                                <td style="width: 1%"><b>:</b></td>
                                <td>
                                    <asp:Image ID="ImgFlagS" runat="server" alt="" ImageUrl="<%$ PhoenixTheme:images/Red.png %>"
                                        Visible="false" />
                                </td>

                            </tr>

                        </table>
                    </NestedViewTemplate>--%>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="false" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>



            <table cellpadding="1" cellspacing="1">

                <tr>
                    <td>
                        <table>
                            <tr class="rowblue">
                                <td width="5px" height="10px"></td>
                            </tr>
                        </table>
                    </td>
                    <td>* OffSigners Planned
                    </td>
                    <td>
                        <asp:Label ID="lblWD" runat="server" Text=" * WD - Waived Documents"></asp:Label>
                    </td>
                    <td>
                        <img id="lblRed" runat="server" src="<%$ PhoenixTheme:images/Red.png %>" />
                    </td>
                    <td>
                        <asp:Label ID="lblOverDue" runat="server" Text=" * Overdue / Missing"></asp:Label>
                    </td>
                    <td>
                        <img id="lblYellow" runat="server" src="<%$ PhoenixTheme:images/Yellow.png %>" />
                    </td>
                    <td>
                        <asp:Label ID="lblDueWithin30days" runat="server" Text=" * Due within 30 days"></asp:Label>
                    </td>
                    <td>
                        <img id="lblGreen" runat="server" src="<%$ PhoenixTheme:images/Green.png %>" />
                    </td>
                    <td>
                        <asp:Label ID="lblDueWithin60Days" runat="server" Text=" * Due within 60 days"></asp:Label>
                    </td>
                </tr>
            </table>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
