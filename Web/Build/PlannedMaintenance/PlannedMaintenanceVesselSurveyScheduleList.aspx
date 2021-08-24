<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceVesselSurveyScheduleList.aspx.cs"
    Inherits="PlannedMaintenanceVesselSurveyScheduleList" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Category" Src="~/UserControls/UserControlVesselCertificateCategoryList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselApplication.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonToolTip" Src="~/UserControls/UserControlCommonToolTip.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Survey Schedule List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersSurvey" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="radSkinManager1" runat="server"></telerik:RadSkinManager>
        <telerik:RadWindowManager ID="radWindowManager1" runat="server"></telerik:RadWindowManager>
        <telerik:RadToolTipManager runat="server" ID="RadToolTipManager1" ShowEvent="OnClick" HideEvent="LeaveTargetAndToolTip" Height="200"
            Width="300" Position="Center" RelativeTo="Mouse" OnAjaxUpdate="RadToolTipManager1_AjaxUpdate">
        </telerik:RadToolTipManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:Status runat="server" ID="ucStatus" Visible="false" />
            <eluc:TabStrip ID="MainMenuSurveySchedule" runat="server" OnTabStripCommand="MainMenuSurveySchedule_TabStripCommand"></eluc:TabStrip>
            <table width="100%" cellspacing="2">
                <tr>
                    <td colspan="8"></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="lblVesselName" runat="server" Text="Vessel" Enabled="false" CssClass="input"
                            Width="150px">
                        </telerik:RadTextBox>
                        <eluc:Vessel ID="ddlVessel" runat="server" AutoPostBack="true" Module="CERTIFICATESANDSURVEYS"
                            VesselsOnly="true" OnTextChangedEvent="ddlCategory_TextChanged" Width="200px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCertificatename" runat="server" Text="Certificate"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCertificatename" runat="server" CssClass="input"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCategory" runat="server" Text="Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlCategory" runat="server" Width="250px" AutoPostBack="true" OnTextChanged="ddlCategory_TextChanged">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkShowAll" runat="server" Text="Show All"></telerik:RadCheckBox>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkShowNotVerified" runat="server" Text="Show Not Verified"></telerik:RadCheckBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="8"></td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuSurveySchedule" runat="server" OnTabStripCommand="MenuSurveySchedule_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvSurvey" runat="server" AutoGenerateColumns="False" AllowPaging="true" EnableHeaderContextMenu="true"
                Width="100%" AllowCustomPaging="true" EnableViewState="true" AllowSorting="true" Height="80%" CellSpacing="0"
                OnItemCommand="gvSurvey_ItemCommand" ShowHeader="true" OnItemDataBound="gvSurvey_ItemDataBound" 
                OnNeedDataSource="gvSurvey_NeedDataSource" OnSortCommand="gvSurvey_SortCommand" AllowMultiRowSelection="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                    AutoGenerateColumns="false" TableLayout="Fixed" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client">
                    <GroupByExpressions>
                        <telerik:GridGroupByExpression>
                            <SelectFields>
                                <telerik:GridGroupByField FieldName="FLDCATEGORYNAME" FieldAlias="Category" />
                            </SelectFields>
                            <GroupByFields>
                                <telerik:GridGroupByField FieldName="FLDORDER" FieldAlias="FLDORDER" />
                            </GroupByFields>                            
                        </telerik:GridGroupByExpression>
                    </GroupByExpressions>
                   
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Last Audit/Survey" Name="LastSurvey" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Next Audit/Survey" Name="NextSurvey" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Planned" Name="Planned" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                    </ColumnGroups>
                    <ExpandCollapseColumn Resizable="False" Visible="False">
                        <HeaderStyle Width="20px" />
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridClientSelectColumn>
                        <telerik:GridTemplateColumn Visible="false" UniqueName="Data" DataField="FLDCATEGORYNAME">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Certificate" HeaderStyle-Width="250px">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkCertificate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATENAME") %>'
                                    CommandName="CMDSELECT"></asp:LinkButton>
                                <telerik:RadLabel ID="lblCertificateId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSheduleId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCHEDULEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRowDisable" runat="server" Visible="false"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDTEMPLATENAME").ToString().Equals("") || DataBinder.Eval(Container, "DataItem.FLDNOEXPIRY").ToString().Equals("1") ? "1" : "0"%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Issued">
                            <ItemTemplate>
                                <%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDATEOFISSUE")) %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Expiry" UniqueName="ExpiryDate" AllowSorting="true" SortExpression="FLDDATEOFEXPIRY">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDNOEXPIRY").ToString().Equals("1") ? "Permanent" : General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDATEOFEXPIRY"))%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Issued By">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDISSUINGAUTHORITYNAME") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type" ColumnGroupName="LastSurvey">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDTEMPLATENAME").ToString().Equals("") || DataBinder.Eval(Container, "DataItem.FLDNOEXPIRY").ToString().Equals("1") ? "N/A" : DataBinder.Eval(Container, "DataItem.FLDLASTSURVEYTYPENAME")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date" ColumnGroupName="LastSurvey">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDTEMPLATENAME").ToString().Equals("") || DataBinder.Eval(Container, "DataItem.FLDNOEXPIRY").ToString().Equals("1") ? "N/A" : General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDLASTSURVEYDATE"))%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type" ColumnGroupName="NextSurvey">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDTEMPLATENAME").ToString().Equals("") || DataBinder.Eval(Container, "DataItem.FLDNOEXPIRY").ToString().Equals("1") ? "N/A" : DataBinder.Eval(Container, "DataItem.FLDSURVEYTYPENAME")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Due" ColumnGroupName="NextSurvey" UniqueName="NextDue"
                            AllowSorting="true" SortExpression="FLDNEXTDUEDATE">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDTEMPLATENAME").ToString().Equals("") || DataBinder.Eval(Container, "DataItem.FLDNOEXPIRY").ToString().Equals("1") ? "N/A" : General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDNEXTDUEDATE"))%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Window(Range)" ColumnGroupName="NextSurvey">
                            <ItemTemplate>
                                <%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDWINDOWPERIODBEFORE"))%>
                                -
                                <%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDWINDOWPERIODAFTER"))%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date" ColumnGroupName="Planned" AllowSorting="true" SortExpression="FLDPLANDATE">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDPLANDATE")) %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Port" ColumnGroupName="Planned">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Verified?">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDVERIFIEDYN")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" UniqueName="Action" HeaderStyle-Width="180px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Justify"></ItemStyle>
                            <ItemTemplate>
                                <eluc:CommonToolTip runat="server" ID="ucCommonToolTip"
                                    Screen='<%# "PlannedMaintenance/PlannedMaintenanceToolTipSurveyRemark.aspx?dtkey=" + DataBinder.Eval(Container,"DataItem.FLDDTKEY").ToString()+"&cid="+DataBinder.Eval(Container,"DataItem.FLDCERTIFICATEID").ToString()+"&vslid="+DataBinder.Eval(Container,"DataItem.FLDVESSELID").ToString() %>' />
                                <asp:LinkButton runat="server" CommandName="EDIT" ID="cmdEdit"
                                    ToolTip="Edit"><span class="icon"><i class="fas fa-edit"></i></span></asp:LinkButton>
                                <asp:LinkButton runat="server" CommandName="CHANGESURVEY/AUDIT" ID="cmdChangeSurvey" ToolTip="Change Survey/Audit"
                                    Style="display: none"><span class="icon"><i class="fas fa-exchange-alt"></i></span></asp:LinkButton>
                                <asp:LinkButton runat="server" ID="cmdCancelSurveyChange" ToolTip="Cancel Survey/Audit Change"
                                    CommandName="CANCELSURVEY/AUDIT" Style="display: none">
                                     <span class="icon"><i class="fas fa-window-close"></i></span></asp:LinkButton>
                                <asp:LinkButton runat="server" CommandName="DEPLAN" ID="cmdDePlan"
                                    ToolTip="De Plan"><span class="icon"><i class="fas fa-calendar-times"></i></span></asp:LinkButton>
                                <asp:ImageButton runat="server" ID="cmdSvyAtt" ToolTip="Certificate" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" />
                                <asp:LinkButton runat="server" CommandName="RESTART" ID="cmdRestart"
                                    ToolTip="Restart"><span class="icon"><i class="fas fa-sync-alt"></i></span></asp:LinkButton>
                                <asp:LinkButton runat="server" CommandName="CERTIFICATECOC" ID="cmdCOC"
                                    ToolTip="Log Survey Completion"><span class="icon"><i class="fas fa-tasks"></i></span></asp:LinkButton>
                                <asp:LinkButton runat="server" CommandName="VERIFY" ID="cmdVerify"
                                    ToolTip="Verify"><span class="icon"><i class="fas fa-check-circle"></i></span></asp:LinkButton>
                                <asp:LinkButton runat="server" CommandName="EXTNUPDATE" ID="lnkExtension"
                                    ToolTip="Extension"><span class="icon"><i class="fas fa-calendar-plus"></i></span></asp:LinkButton>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                    ColumnsReorderMethod="Reorder" AllowDragToGroup="true">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" FrozenColumnsCount="3" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

            <table cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <img id="Img7" src="<%$ PhoenixTheme:images/green-symbol.png%>" runat="server" />
                    </td>
                    <td>
                        <asp:Literal ID="lblDue" runat="server" Text="* Due"></asp:Literal>
                    </td>
                    <td>
                        <img id="Img2" src="<%$ PhoenixTheme:images/yellow-symbol.png%>" runat="server" />
                    </td>
                    <td>
                        <asp:Literal ID="lblDue60" runat="server" Text="* Due in 60 Days"></asp:Literal>
                    </td>
                    <td>
                        <img id="Img4" src="<%$ PhoenixTheme:images/Orange-symbol.png%>" runat="server" />
                    </td>
                    <td>
                        <asp:Literal ID="lblDue30" runat="server" Text="* Due in 30 Days"></asp:Literal>
                    </td>

                    <td>
                        <img id="Img1" src="<%$ PhoenixTheme:images/red-symbol.png%>" runat="server" />
                    </td>
                    <td>
                        <asp:Literal ID="lblOverdue" runat="server" Text="* Overdue"></asp:Literal>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>

    </form>
</body>
</html>
