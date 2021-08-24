<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewChangePlanFilter.aspx.cs"
    Inherits="CrewChangePlanFilter" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabstelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlCommonVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Change Plan</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="form1" DecoratedControls="All" EnableRoundedCorners="true" />
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="93%">
            <eluc:TabStrip ID="CCPSubMenu" runat="server" OnTabStripCommand="CCPSubMenu_TabStripCommand"></eluc:TabStrip>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <table cellspacing="1" cellpadding="1" width="100%">
                <tr>
                    <td style="width: 49%">
                        <asp:Panel ID="pnlSearchFilters" runat="server" GroupingText="Search" Width="100%">
                            <table>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblPlannedDays" runat="server">
                                            Crew change planned in next
                                                    <eluc:Number ID="lblNoofdays" runat="server" CssClass="input" IsPositive="true" IsInteger="true"
                                                        Width="60px" MaxLength="3" />
                                            days
                                        </telerik:RadLabel>
                                    </td>
                                    <td style="width: 20px"></td>
                                    <td>
                                        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                                        <eluc:Status ID="ucStatus" runat="server" Text="" Visible="false" />
                                        <asp:Literal ID="lblVesselName" runat="server" Text="Vessel"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Vessel ID="ddlVessel" runat="server" CssClass="dropdown_mandatory" VesselsOnly="true"
                                            AppendDataBoundItems="true" AssignedVessels="true" EntityType="VSL" ActiveVessels="true" Width="180px" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                    <td style="width: 10%">
                        <asp:Literal ID="lblDateofCrewChange" runat="server" Text="Date of Crew Change"></asp:Literal>
                    </td>
                    <td style="width: 10%">
                        <eluc:Date ID="txtTentativeDate" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td style="width: 5%">
                        <asp:Literal ID="lblPort" runat="server" Text="Port"></asp:Literal>
                    </td>
                    <td style="width: 25%">
                        <eluc:Port ID="ddlPort" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" Width="180px" />
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuCrewChangePlan" runat="server" OnTabStripCommand="ChangePlan_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCCPlan" Height="77%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemDataBound="gvCCPlan_ItemDataBound" EnableHeaderContextMenu="true"
                ShowFooter="false" ShowHeader="true" EnableViewState="false" OnNeedDataSource="gvCCPlan_NeedDataSource" OnItemCommand="gvCCPlan_ItemCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center">

                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="">
                            <HeaderStyle Width="76%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <table width="100%">
                                    <tr>
                                        <td rowspan="3" style="width: 7%">
                                            <telerik:RadLabel ID="lblonsign" runat="server" Text="On Signer" Style="font-weight: bold; text-decoration: underline; font-style: italic"></telerik:RadLabel>
                                        </td>

                                        <td style="width: 7%">
                                            <telerik:RadLabel ID="lblonname" runat="server" Text="Name" Style="font-weight: bold;"></telerik:RadLabel>

                                        </td>
                                        <td style="width: 1%"><b>:</b></td>
                                        <td style="width: 24%">
                                            <asp:LinkButton ID="lnkEmployee" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'
                                                Style="vertical-align: top; font-weight: bold;"></asp:LinkButton>
                                            <telerik:RadLabel ID="lblCrewPlanId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWPLANID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblOnSignerId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERID") %>'
                                                Visible="false">
                                            </telerik:RadLabel>
                                        </td>
                                        <td style="width: 7%">
                                            <telerik:RadLabel ID="Label1" runat="server" Text="Rank" Style="font-weight: bold;"></telerik:RadLabel>
                                        </td>
                                        <td style="width: 1%"><b>:</b></td>
                                        <td style="width: 15%">
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
                                            <telerik:RadLabel ID="lblonsignerrank" runat="server" ToolTip="On Signer" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></telerik:RadLabel>
                                        </td>
                                        <td style="width: 7%">
                                            <telerik:RadLabel ID="RadLabel3" runat="server" Text="PD Status" Style="font-weight: bold;"></telerik:RadLabel>
                                        </td>
                                        <td style="width: 1%"><b>:</b></td>
                                        <td style="width: 15%">
                                            <telerik:RadLabel ID="lblPDStatus" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPDSTATUS")%>'></telerik:RadLabel>
                                            <eluc:ToolTip ID="ucToolTipAddress" runat="server" Text='<%#HttpContext.Current.Server.HtmlDecode(DataBinder.Eval(Container, "DataItem.FLDAPPROVALREMARKS").ToString()) %>' />
                                        </td>


                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="RadLabel2" runat="server" Text="Planned Port" Style="font-weight: bold;"></telerik:RadLabel>
                                        </td>
                                        <td><b>:</b></td>
                                        <td>
                                            <telerik:RadLabel ID="lblplanedport" runat="server" Text='  <%# DataBinder.Eval(Container, "DataItem.FLDSEAPORTNAME")%>'></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="RadLabel1" runat="server" Text="Planned Relief" Style="font-weight: bold;"></telerik:RadLabel>
                                        </td>
                                        <td><b>:</b></td>
                                        <td>
                                            <%# DataBinder.Eval(Container, "DataItem.FLDEXPECTEDJOINDATE", "{0:dd/MMM/yyyy}") %>   
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="RadLabel4" runat="server" Text="Proceed Remarks" Style="font-weight: bold;"></telerik:RadLabel>
                                        </td>
                                        <td><b>:</b></td>
                                        <td>
                                            <telerik:RadLabel ID="lblProceedRemarks" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPLANNEDREMARKS").ToString().Length>30 ? DataBinder.Eval(Container, "DataItem.FLDPLANNEDREMARKS").ToString().Substring(0, 10) + "..." : DataBinder.Eval(Container, "DataItem.FLDPLANNEDREMARKS").ToString() %>'></telerik:RadLabel>
                                            <eluc:ToolTip ID="ucToolTipProceedRemarks" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPLANNEDREMARKS")%>' />
                                        </td>
                                    </tr>
                                    <tr>

                                        <td>
                                            <telerik:RadLabel ID="RadLabel5" runat="server" Text="Actual Port" Style="font-weight: bold;"></telerik:RadLabel>
                                        </td>
                                        <td><b>:</b></td>
                                        <td><%# DataBinder.Eval(Container, "DataItem.FLDCREWCHANGEPORTNAME")%></td>

                                        <td>
                                            <telerik:RadLabel ID="RadLabel6" runat="server" Text="Actual Date" Style="font-weight: bold;"></telerik:RadLabel>
                                        </td>
                                        <td><b>:</b></td>
                                        <td><%# DataBinder.Eval(Container, "DataItem.FLDCREWCHANGEDATE", "{0:dd/MMM/yyyy}")%></td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblOffsignerDesc" runat="server" Text="Off Signer" Style="font-weight: bold; text-decoration: underline; font-style: italic"></telerik:RadLabel>
                                        </td>

                                        <td>
                                            <telerik:RadLabel ID="RadLabel7" runat="server" Text="Name" Style="font-weight: bold;"></telerik:RadLabel>
                                        </td>
                                        <td><b>:</b></td>
                                        <td>
                                            <asp:LinkButton ID="lnkOffSigner" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERNAME") %>'
                                                Style="font-style: italic"></asp:LinkButton>

                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="RadLabel8" runat="server" Text="Rank" Style="font-weight: bold;"></telerik:RadLabel>
                                        </td>
                                        <td><b>:</b></td>
                                        <td>
                                            <telerik:RadLabel ID="lblOffSignerRank" runat="server" ToolTip="Off Signer" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERRANK") %>'
                                                Style="font-style: italic">
                                            </telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="RadLabel9" runat="server" Text="Relief Due" Style="font-weight: bold;"></telerik:RadLabel>
                                        </td>
                                        <td><b>:</b></td>
                                        <td>
                                            <telerik:RadLabel ID="lblRelifDueDate" Text='<%# DataBinder.Eval(Container, "DataItem.FLDRELIEFDUEDATE", "{0:dd/MMM/yyyy}")%>'
                                                runat="server" Style="font-style: italic">
                                            </telerik:RadLabel>
                                            <eluc:ToolTip ID="ucToolTipRelifdueDate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDRELIEFDUEDATE").ToString()!="" ? "Relif Due Date" : "" %>' />
                                        </td>
                                    </tr>


                                </table>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>                    
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="23%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="NOK"
                                    CommandName="cmdNok" CommandArgument="<%# Container.DataSetIndex %>" ID="imgNok"
                                    ToolTip="Family NOK">
                                <span class="icon"><i class="fas fa-users-vendor"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Suitability Check"
                                    CommandName="SUITABILITYCHECK" CommandArgument="<%# Container.DataSetIndex %>" ID="imgSuitableCheck"
                                    ToolTip="Suitability Check">
                                <span class="icon"><i class="fas  fa-user-astronaut"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Joining Letters"
                                    CommandName="JOININGLETTER" CommandArgument="<%# Container.DataSetIndex %>" ID="imgJoiningLetter"
                                    ToolTip="Joining Letters">
                                <span class="icon"><i class="fa fa-file-signature-jl"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Activities"
                                    CommandName="ACTIVITY" CommandArgument="<%# Container.DataSetIndex %>" ID="imgActivity"
                                    ToolTip="Activities">
                                <span class="icon"><i class="fa fa-pencil-ruler"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Licence Request"
                                    CommandName="LICENCEREQUEST" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                    ToolTip="Initiate Licence Request">
                                <span class="icon"><i class="fas fa-passport"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Initiate Course Request"
                                    CommandName="COURSEREQUEST" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCourse"
                                    ToolTip="Initiate Course Request"><span class="icon"><i class="fas fa-book"></i></span>
                                </asp:LinkButton>                          
                                <asp:LinkButton runat="server" AlternateText="Medical Request"
                                    CommandName="MEDICALREQUEST" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdMedical"
                                    ToolTip="Initiate Medical Request"><span class="icon"><i class="fas fa-briefcase-medical"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Working Gear Issue/Request"
                                    CommandName="WORKGEARISSUE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdWorkGear"
                                    ToolTip="Working Gear Issue/Request"><span class="icon"><i class="fas fa-tshirt"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Generate Contract" CommandName="CONTRACT" ID="cmdGenContract" ToolTip="Generate Contract">
                                    <span class="icon"><i class="fas fa-file-contract"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Unallocated Vessel Expense"
                                    CommandName="UNALLOCATEDVSLEXP" CommandArgument="<%# Container.DataSetIndex %>" ID="imgUnAllocatedVslExp"
                                    ToolTip="Unallocated Vessel Expense">
                                <span class="icon"> <i class="fa fa-coins-ei"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Checklist" CommandName="CHECKLIST" ID="cmdChkList" ToolTip="Checklist">
                                    <span class="icon"><i class="fas fa-list-ul"></i></span>
                                </asp:LinkButton>

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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" AllowExpandCollapse="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>


            <table cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <table>
                            <tr class="rowred">
                                <td width="5px" height="10px"></td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <asp:Literal ID="lblDocumentsnotmappedforthevessel" runat="server" Text="* Documents not mapped for the vessel"></asp:Literal>
                    </td>
                </tr>
            </table>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
