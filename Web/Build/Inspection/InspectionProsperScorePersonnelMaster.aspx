<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionProsperScorePersonnelMaster.aspx.cs" Inherits="InspectionProsperScorePersonnelMaster" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCRank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCVesseltype" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCCompany" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCPool" Src="~/UserControls/UserControlPool.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCLQuick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCOfficerlist" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Score</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuProgress" runat="server" OnTabStripCommand="QualityProgress_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%">
            <table cellpadding="1" cellspacing="1" width="99%">
                <tr align="left">
                    <td>
                        <telerik:RadLabel ID="lblfirstname" runat="server" Text="First Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFirstName" runat="server" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtMiddleName" runat="server" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtLastName" runat="server" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRANKname" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="lblrname" runat="server" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblfromdate" runat="server" Text="From"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtfromdate" runat="server" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lbltodate" runat="server" Text="To"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txttodate" runat="server" ReadOnly="true"></telerik:RadTextBox></td>
                    <td>
                        <telerik:RadLabel ID="lblcycle" runat="server" Text="Cycle Period"></telerik:RadLabel>
                    </td>
                    <td>                     
                        <telerik:RadComboBox ID="ddlcycle" runat="server" DataTextField="FLDCYCLEDATE" DataValueField="FLDCYCLEID" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlcycle_SelectedIndexChanged"
                            AutoPostBack="true" Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lbltenure" runat="server" Text="Tenure Period"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddltenurecycle" DataTextField="FLDCYCLEDATE" AutoPostBack="true"  DataValueField="FLDTENURECYCLEID" Width="170px" runat="server" OnSelectedIndexChanged="ddltenurecycle_SelectedIndexChanged" Filter="Contains">
                        </telerik:RadComboBox>
                    </td>
                </tr>
            </table>
            <hr />
            <br />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvProsperemplist" runat="server" Height="45%" EnableViewState="false"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvProsperemplist_NeedDataSource" EnableHeaderContextMenu="true"
                OnItemCommand="gvProsperemplist_ItemCommand" ShowFooter="true" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle Width="102px" />
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="PSC" Name="PSC" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Vetting" Name="Vetting" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Incident" Name="Incident" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Health & Safety" Name="Health & Safety" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Feedback" Name="Feedback" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Third Party" Name="Third Party" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="External Nav & Env" Name="External Nav & Env" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>

                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Date" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbldate" CommandName="VIEWDETAILS" CommandArgument="<%# Container.DataSetIndex %>" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATE")) %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblsource" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSOURCEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblEVENT" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEVENT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblinsname" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <telerik:RadLabel ID="lbltotal" runat="server" Visible="true" Text='Total:'></telerik:RadLabel>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="TOTAL" HeaderText="Vessel" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblvesselname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel Type" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblvesselType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Right" HeaderText="DEF" UniqueName="PSCDEF" AllowSorting="false" ColumnGroupName="PSC" ShowSortIcon="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPSCDEF" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPSCDEF") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Right" HeaderText="DET" UniqueName="PSCDET" AllowSorting="false" ColumnGroupName="PSC" ShowSortIcon="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPSCDET" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPSCDET") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Right" HeaderText="DEF" UniqueName="VETTINGDEF" AllowSorting="false" ColumnGroupName="Vetting" ShowSortIcon="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVETTINGDEF" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVETTINGDEF") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Right" HeaderText="REJ" UniqueName="VETTINGREJ" AllowSorting="false" ColumnGroupName="Vetting" ShowSortIcon="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblREJ" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVETTINGREJ") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Right" HeaderText="A" UniqueName="INCIDENTACATEGORY" ColumnGroupName="Incident" AllowSorting="false" ShowSortIcon="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIncidentACategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINCIDENTACATEGORY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Right" HeaderText="B" UniqueName="INCIDENTBCATEGORY" ColumnGroupName="Incident" AllowSorting="false" ShowSortIcon="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIncidentBCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINCIDENTBCATEGORY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Right" HeaderText="C" UniqueName="INCIDENTCCATEGORY" ColumnGroupName="Incident" AllowSorting="false" ShowSortIcon="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIncidentCCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINCIDENTCCATEGORY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Right" HeaderText="A" UniqueName="HNSACATEGORY" ColumnGroupName="Health & Safety" AllowSorting="false" ShowSortIcon="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblHNSACategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHNSACATEGORY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Right" HeaderText="B" UniqueName="HNSBCATEGORY" ColumnGroupName="Health & Safety" AllowSorting="false" ShowSortIcon="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblHNSBCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHNSBCATEGORY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Right" HeaderText="C" UniqueName="HNSCCATEGORY" ColumnGroupName="Health & Safety" AllowSorting="false" ShowSortIcon="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblHNSCCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHNSCCATEGORY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Right" HeaderText="POS" UniqueName="FEEDBACKPOSITIVE" ColumnGroupName="Feedback" AllowSorting="false" ShowSortIcon="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPositiveFeedback" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFEEDBACKPOSITIVE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Right" HeaderText="NEG" UniqueName="FEEDBACKNEGATIVE" ColumnGroupName="Feedback" AllowSorting="false" ShowSortIcon="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNegativeFeedback" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFEEDBACKNEGATIVE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Right" HeaderText="WRN" UniqueName="FEEDBACKWARNING" ColumnGroupName="Feedback" AllowSorting="false" ShowSortIcon="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWarningFeedback" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFEEDBACKWARNING") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Right" HeaderText="DEF" UniqueName="TPIDEF" ColumnGroupName="Third Party" AllowSorting="false" ShowSortIcon="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTPIDEF" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTPIDEF") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Right" HeaderText="REJ" UniqueName="TPIDET" ColumnGroupName="Third Party" AllowSorting="false" ShowSortIcon="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTPIDET" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTPIDET") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Right" HeaderText="DEF" UniqueName="EXTDEF" ColumnGroupName="External Nav & Env" AllowSorting="false" ShowSortIcon="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEXTDEF" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXTDEF") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Right" HeaderText="REJ" UniqueName="EXTDET" ColumnGroupName="External Nav & Env" AllowSorting="false" ShowSortIcon="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEXTDET" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXTDET") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true" AllowResizeToFit="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <br />
            <br />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvEmployeeProsper" runat="server" Height="45%" EnableViewState="false"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvEmployeeProsper_NeedDataSource" EnableHeaderContextMenu="true"
                OnItemCommand="gvEmployeeProsper_ItemCommand" ShowFooter="true"
                AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle Width="102px" HorizontalAlign="Center" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Left" HeaderText="Category" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCategory" runat="server"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Left" HeaderText="Measure" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Right" HeaderText="Count" AllowSorting="false" ShowSortIcon="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Right" HeaderText="Actual" UniqueName="ACTUAL" AllowSorting="false" ShowSortIcon="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblACTUAL" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTUAL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Right" HeaderText="Final Score" UniqueName="CALCULATED" AllowSorting="false" ShowSortIcon="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblScore" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCALCULATED") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="false" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

            <table cellpadding="1" cellspacing="1">
                <tr>
                    <td></td>
                    <td>
                        <asp:Literal ID="Literal1" Text="NOTE: DEF -- Deficiency, DET -- Detention, REJ -- Rejection, POS -- Positive, NEG -- Negative, WRN -- Warning" runat="server">
                        </asp:Literal>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
