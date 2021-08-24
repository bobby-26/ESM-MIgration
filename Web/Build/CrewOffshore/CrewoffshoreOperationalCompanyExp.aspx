<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewoffshoreOperationalCompanyExp.aspx.cs"
    Inherits="CrewOffshore_CrewoffshoreOperationalCompanyExp" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmCrewOtherExperienceList" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>



            <eluc:TabStrip ID="MenuCrewOtherExperienceList" Title="Crew Company Experience" runat="server">
                <%--OnTabStripCommand="CrewOtherExperienceList_TabStripCommand"--%>
            </eluc:TabStrip>


            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtMiddleName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtLastName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeNumber" runat="server" Text="File Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>



            <eluc:TabStrip ID="MenuCrewCompanyExperience" runat="server" OnTabStripCommand="CrewCompanyExperience_TabStripCommand"></eluc:TabStrip>


            <%--   <asp:GridView ID="gvCrewCompanyExperience" runat="server" AutoGenerateColumns="False"
                Font-Size="11px" CellPadding="3" ShowFooter="false" ShowHeader="true" EnableViewState="false"
                AllowSorting="true" OnRowDataBound="gvCrewCompanyExperience_RowDataBound">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" Wrap="False" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <RowStyle Height="10px" />
                <RowStyle Height="10px" />--%>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewCompanyExperience" runat="server" Height="400px" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvCrewCompanyExperience_NeedDataSource"
                OnItemDataBound="gvCrewCompanyExperience_ItemDataBound"
                OnSortCommand="gvCrewCompanyExperience_SortCommand"
                OnItemCommand="gvCrewCompanyExperience_ItemCommand"
                GroupingEnabled="false" EnableHeaderContextMenu="true"
                AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" >
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
                                <telerik:RadLabel ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="150px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sign on">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSignonDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDSIGNONDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Dry Dockings Attended">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="150px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDockings" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDRYDOCKYN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="No. of">
                            <ItemStyle Wrap="False" HorizontalAlign="left"></ItemStyle>
                            <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblOvidheadcount" runat="server" Text='OVID : '></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblOVIDNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVID") %>'></telerik:RadLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblOsvisheadcount" runat="server" Text='OSVIS : '></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblOSVISNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOSVISCOUTN") %>'></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Class of ROV">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblROV" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCLASSOFROV").ToString().Length>15 ? DataBinder.Eval(Container, "DataItem.FLDCLASSOFROV").ToString().Substring(0, 15) + "..." : DataBinder.Eval(Container, "DataItem.FLDCLASSOFROV").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucROV" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLASSOFROV") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Types of Anchors Handled">
                            <HeaderStyle Width="150px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTypesofAnchors" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTYPESOFANCHORSHANDLED").ToString().Length>15 ? DataBinder.Eval(Container, "DataItem.FLDTYPESOFANCHORSHANDLED").ToString().Substring(0, 15) + "..." : DataBinder.Eval(Container, "DataItem.FLDTYPESOFANCHORSHANDLED").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucTypesofAnchors" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPESOFANCHORSHANDLED") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="No. of">
                            <ItemStyle Wrap="False" HorizontalAlign="left"></ItemStyle>
                            <HeaderStyle Width="150px" />
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblOceanheadcount" runat="server" Text='Ocean Tows : '></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblOceanTow" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDAYINOCEANTOWING") %>'></telerik:RadLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblTowheadcount" runat="server" Text='Infield Moves : '></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblTow" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDAYINTOWING") %>'></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="FSI/PSC">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFSIPSC" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFSIPSCYN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="FMEA">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFMEA" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFMEAYN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="DP Annuals">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDPAnnuals" runat="server" CommandArgument='<%# Container.DataSetIndex %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDDPANNUALSYN") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText=" New Delivery Take over">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNewDeliveryTakeover" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDNEWDELIVERYTAKEOVERYN")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Heavy Lift Project cargoes">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblHeavyLift" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDHEAVYLIFTCARGOESYN")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="DKD Mud">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDKDMud" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDKDMUDYN")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Methanol">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMethanol" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMETHANOLYN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Glycol">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblGlycol" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGLYCOLYN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Experience in Grappling for Anchors">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblGrappling" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDGRAPPLINGANCHORSYN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Experience in Chain Handled">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblchain" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDLENGTHCHAINHANDLEDYN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Maximum Anchor<br /> Handling Depth">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMaximumAnchors" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDMAXANCHORHANDLINGYN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="false" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" EnableNextPrevFrozenColumns="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>



        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
