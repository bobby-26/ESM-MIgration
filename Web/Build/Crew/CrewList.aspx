<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewList.aspx.cs" Inherits="CrewList" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    <style type="text/css">
        .w3-circle {
            border-radius: 50%;
            box-shadow: 3px 3px 3px grey;
            transition: transform .2s;
            transform: scale(1.5);
        }

            .w3-circle:hover {
                transform: scale(2); /* (200% zoom - Note: if the zoom is too large, it will go outside of the viewport) */
            }

        .dot {
            height: 10px;
            width: 10px;
            border-radius: 50%;
            display: inline-block;
        }
    </style>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmCrewList" DecoratedControls="All" EnableRoundedCorners="true" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server"></telerik:RadSkinManager>
    <form id="frmCrewList" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
<%--        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">--%>
            <eluc:TabStrip ID="MenuCrewOCIMF" runat="server" TabStrip="false" OnTabStripCommand="MenuCrew_TabStripCommand"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td style="width: 20%">
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                        <%--</td>
                    <td style="width: 25%">--%>
                        <eluc:Vessel ID="ddlVessel" runat="server" CssClass="input_mandatory" VesselsOnly="true" AppendDataBoundItems="true"
                            AssignedVessels="true" AutoPostBack="true" Entitytype="VSL" ActiveVesselsOnly="true" Width="180px"
                            OnTextChangedEvent="SetVessel" />
                        <telerik:RadTextBox ID="txtVesselName" runat="server" CssClass="input" ReadOnly="true" Enabled="false" Visible="false"></telerik:RadTextBox>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblLifeboatCapacity" runat="server" Text="Lifeboat Capacity"></telerik:RadLabel>
                        <%--   </td>
                    <td style="width: 20%">--%>
                        <telerik:RadTextBox ID="txtLBCapacity" runat="server" CssClass="input" ReadOnly="true" Enabled="false" Width="60px"></telerik:RadTextBox>
                    </td>
                    <td style="width: 16%">
                        <telerik:RadLabel ID="lblTotalCrewonboard" runat="server" Text="Total Crew onboard"></telerik:RadLabel>
                        <%-- </td>
                    <td style="width: 20%">--%>
                        <telerik:RadTextBox ID="txtTotalCrew" runat="server" CssClass="input" ReadOnly="true" Enabled="false" Width="60px"></telerik:RadTextBox>
                    </td>
                    <td style="width: 20%">
                        <telerik:RadCheckBox ID="chkonboard" runat="server" Text="On-board only" OnCheckedChanged="SetVessel" AutoPostBack="true"></telerik:RadCheckBox>
                        <asp:Label ID="lblBelowsafe" runat="server" Visible="false" Text="* Below Safe Scale"
                            ForeColor="Maroon"></asp:Label>
                        <span id="Span1" class="icon" runat="server"><i class="fas fa-info-circle" style="align-content: center; color: maroon;"></i></span>

                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="RadToolTip1" Width="450px" ShowEvent="onmouseover"
                            RelativeTo="Element" Animation="Fade" TargetControlID="Span1" IsClientID="true" VisibleOnPageLoad="false"
                            HideEvent="ManualClose" Position="TopCenter" EnableRoundedCorners="true" ContentScrolling="Auto">
                            <telerik:RadGrid RenderMode="Lightweight" ID="gvBelowSafeScale" runat="server" AllowCustomPaging="false" AllowSorting="false" AllowPaging="false"
                                CellSpacing="0" GridLines="None" ShowFooter="false" ShowHeader="true" EnableViewState="true" OnNeedDataSource="gvBelowSafeScale_NeedDataSource">
                                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-CssClass="center">
                                    <Columns>
                                        <telerik:GridTemplateColumn HeaderText="Sr.No">
                                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblRow" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSRNO") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Rank">
                                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblRankName" runat="server" ForeColor="Maroon" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Safe Scale">
                                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblSafeScale" ForeColor="Maroon" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSAFESCALE")%>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="OnBoard">
                                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblonScale" ForeColor="Maroon" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDONBOARD")%>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Below Safe Scale">
                                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblBelowSafeScale" ForeColor="Maroon" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDBELOWSAFESCALE")%>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" AllowExpandCollapse="true" ColumnsReorderMethod="Reorder">
                                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                </ClientSettings>
                            </telerik:RadGrid>
                        </telerik:RadToolTip>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuCrewList" runat="server" OnTabStripCommand="CrewList_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewList" Height="78%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvCrewList_ItemCommand" OnItemDataBound="gvCrewList_ItemDataBound" EnableHeaderContextMenu="true"
                ShowFooter="false" ShowHeader="true" EnableViewState="false" OnNeedDataSource="gvCrewList_NeedDataSource" OnSortCommand="gvCrewList_SortCommand" GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true" HierarchyDefaultExpanded="false"
                    AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center">
                    <NestedViewSettings>
                        <ParentTableRelation>
                            <telerik:GridRelationFields MasterKeyField="FLDDTKEY" DetailKeyField="FLDDTKEY" />
                        </ParentTableRelation>
                    </NestedViewSettings>
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Sr.No">
                            <HeaderStyle Width="5%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblsrno" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDROWNUMBER")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" ShowSortIcon="true" AllowSorting="true" SortExpression="FLDEMPLOYEENAME">
                            <HeaderStyle Width="25%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCrewId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblConId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTRACTID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblreflieprior" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFLIEPRIORDATE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblexhandyn" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXHAND") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblNewApp" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEWAPP") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblFamilyId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFAMILYID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselName" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCrewPlanYN" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWPLANYN") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkCrew" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblCrewName" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblTTO" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRANSFERTOOFFICE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDue" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDUE")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblOverDue" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOVERDUE")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblEmployeeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDEMPLOYEEID")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank" ShowSortIcon="true" AllowSorting="true" SortExpression="FLDRANKNAME">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblrankid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblranknmae" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDRANKNAME")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDtkey" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblFilePath" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILEPATH") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Nationality" ShowSortIcon="true" AllowSorting="true" SortExpression="FLDNATIONALITYNAME">
                            <HeaderStyle Width="8%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblnatinality" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDNATIONALITYNAME")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status">
                            <HeaderStyle Width="8%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblType" runat="server" Visible="false" Text=' <%# DataBinder.Eval(Container, "DataItem.FLDTYPE")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblstatsname" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSTATUS")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="(Exp.) Join" AllowSorting="true" SortExpression="FLDSIGNONDATE" ShowSortIcon="true">
                            <HeaderStyle Width="8%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblsignondate" runat="server" Text='<%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDSIGNONDATE", "{0:dd/MMM/yyyy}"))%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="PD Status">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPDStatusid" runat="server" Visible="false" Text=' <%# DataBinder.Eval(Container, "DataItem.FLDPDSTATUSID")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblPDStatus" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPDSTATUS")%>' ToolTip='<%#HttpContext.Current.Server.HtmlDecode(DataBinder.Eval(Container, "DataItem.FLDAPPROVALREMARKS").ToString()) %>'></telerik:RadLabel>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Relief Due" ShowSortIcon="true" AllowSorting="true" SortExpression="FLDRELIEFDUEDATE">
                            <HeaderStyle Width="8%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReliefdate" runat="server" Text='<%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDRELIEFDUEDATE", "{0:dd/MMM/yyyy}"))%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CDC No." ShowSortIcon="true" AllowSorting="true" SortExpression="FLDSEAMANBOOKNO">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCDC" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSEAMANBOOKNO")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Suitability Check"
                                    CommandName="SUITABILITYCHECK" CommandArgument="<%# Container.DataSetIndex %>" ID="imgSuitableCheck"
                                    ToolTip="Suitability Check">
                                <span class="icon"><i class="fas  fa-user-astronaut"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Activities"
                                    CommandName="ACTIVITY" CommandArgument="<%# Container.DataSetIndex %>" ID="imgActivity"
                                    ToolTip="Activities">
                                <span class="icon"><i class="fa fa-pencil-ruler"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Send Data to Vessel"
                                    CommandName="SEND" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSync"
                                    ToolTip="Send Data to Vessel"><span class="icon"><i class="fas fa-share"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Generate Contract" CommandName="CONTRACT" ID="cmdGenContract" ToolTip="Generate Contract">
                                    <span class="icon"><i class="fas fa-file-contract"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Crew Change Request" CommandName="CREWTRAVELREQUEST" ID="cmdIniTravel" ToolTip="Crew Travel Request">
                                    <span class="icon"><i class="fas fa-plane-departure-it"></i></span>
                                </asp:LinkButton>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <NestedViewTemplate>
                        <table style="font-size: 11px; width: 80%">
                            <tr>
                                <td rowspan="3" style="width: 15%;">
                                    <asp:Image ID="imgPhoto" runat="server" Height="30px" CssClass="w3-circle" Width="30px" /></a>
                                </td>
                                <td style="font-weight: 700; width: 20%;">
                                    <telerik:RadLabel ID="RadLabel1" runat="server" Text="Birth Date"></telerik:RadLabel>
                                </td>
                                <td>:</td>
                                <td style="width: 20%;">
                                    <telerik:RadLabel ID="lblDOB" runat="server" Text='<%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDATEOFBIRTH", "{0:dd/MMM/yyyy}"))%>'></telerik:RadLabel>
                                </td>
                                <td style="font-weight: 700; width: 20%;">
                                    <telerik:RadLabel ID="lblpassport" runat="server" Text="PassPort No."></telerik:RadLabel>
                                </td>
                                <td>:</td>
                                <td style="width: 20%;">
                                    <telerik:RadLabel ID="lblPPNo" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPASSPORTNO")%>'></telerik:RadLabel>
                                </td>

                            </tr>
                            <tr>
                                <td style="font-weight: 700;">
                                    <telerik:RadLabel ID="lblExtended" runat="server" Text="Extended/Reduced"></telerik:RadLabel>
                                </td>
                                <td>:</td>
                                <td>
                                    <telerik:RadLabel ID="lblER" runat="server" Text=' <%# DataBinder.Eval(Container, "DataItem.FLDEXTRED")%>'></telerik:RadLabel>
                                </td>
                                <td style="font-weight: 700">
                                    <telerik:RadLabel ID="lblexhandhead" runat="server" Text="Exhand"></telerik:RadLabel>
                                </td>
                                <td>:</td>
                                <td>
                                    <telerik:RadLabel ID="lblExhand" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDEXHAND").ToString() == "1" ? "Yes" : "No"%>'></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr style="font-weight: 700">

                                <td>
                                    <telerik:RadLabel ID="lblexpx" runat="server" Text="Exp(M)"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblexpc" runat="server" Text=":"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblExp" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDECIMALEXPERIENCE")%>'
                                        ToolTip='<%# "Total Exp(M): " + DataBinder.Eval(Container.DataItem,"FLDTOATLEXPERIENCE") %>'>
                                    </telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NestedViewTemplate>
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
                        <telerik:RadLabel ID="lblOverdue" runat="server" Text="* Overdue"></telerik:RadLabel>
                    </td>
                    <td>
                        <table>
                            <tr class="rowblue">
                                <td width="5px" height="10px"></td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPlannedforthevessel" runat="server" Text="* Planned for the vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <table>
                            <tr class="rowgreen">
                                <td width="5px" height="10px"></td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAboveOwnerScale" runat="server" Text="* Above Owner Scale"></telerik:RadLabel>
                    </td>
                    <td>
                        <table>
                            <tr class="rowmaroon">
                                <td width="5px" height="10px"></td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAboveSafeScale" runat="server" Text="* Above Safe Scale"></telerik:RadLabel>

                    </td>
                </tr>
            </table>
            <eluc:Status ID="ucStatus" runat="server" />
        <%--</telerik:RadAjaxPanel>--%>
    </form>
</body>
</html>
