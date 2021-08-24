<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsRHDesignation.aspx.cs"
    Inherits="VesselAccountsRHDesignation" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="WorkSheet" Src="~/UserControls/UserControlRHTimeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Scheduled Work Hours</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style type="text/css">
            table.Hazard {
                border-collapse: collapse;
            }

                table.Hazard td, table.Hazard th {
                    border: 1px solid black;
                    padding: 5px;
                }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRestHourDesignation" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="98%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="MenuRHGeneral" runat="server" OnTabStripCommand="RHGeneral_TabStripCommand"></eluc:TabStrip>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td width="20%">
                        <telerik:RadLabel ID="RadlblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td width="80%">
                        <telerik:RadTextBox ID="lblRank" runat="server" ReadOnly="true" CssClass="readonlytextbox" Width="270px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td width="20%">
                        <telerik:RadLabel ID="RadlblEmployee" runat="server" Text="Crew"></telerik:RadLabel>
                    </td>
                    <td width="80%">
                        <telerik:RadTextBox ID="lblEmployee" runat="server" ReadOnly="true" CssClass="readonlytextbox" Width="270px"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>

            <table runat="server" width="100%">
                <tr runat="server" valign="center">
                    <td>
                        <p style="text-align:center">
                            <telerik:RadLabel ID="lblsea" runat="server" Font-Bold="true" Text="Scheduled Daily Work Hours at Sea"></telerik:RadLabel>
                        </p>
                    </td>
                </tr>
                <tr valign="center">
                    <td width="100%">
                        <telerik:RadGrid RenderMode="Lightweight" Width="100%" ID="gvWorkHours" Height="50%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                            CellSpacing="0" GridLines="None" EnableHeaderContextMenu="true" GroupingEnabled="false" AutoGenerateColumns="false"
                            OnNeedDataSource="gvWorkHours_NeedDataSource" OnItemDataBound="gvWorkHours_ItemDataBound" OnItemCommand="gvWorkHours_ItemCommand"
                            ShowHeader="true" EnableViewState="true">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" DataKeyNames="FLDRESTHOURSDEFAULTWORKHOURSID">
                                <NoRecordsTemplate>
                                    <table runat="server" width="100%" border="0">
                                        <tr>
                                            <td align="center">
                                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </NoRecordsTemplate>
                                <ColumnGroups>
                                    <telerik:GridColumnGroup HeaderText="WatchKeeping" HeaderStyle-Width="40%" Name="WatchKeeping" HeaderStyle-HorizontalAlign="Center">
                                    </telerik:GridColumnGroup>
                                </ColumnGroups>
                                <ColumnGroups>
                                    <telerik:GridColumnGroup HeaderText="Non WatchKeeping" HeaderStyle-Width="40%" Name="NonWatchKeeping" HeaderStyle-HorizontalAlign="Center">
                                    </telerik:GridColumnGroup>
                                </ColumnGroups>
                                <ColumnGroups>
                                    <telerik:GridColumnGroup HeaderText="Period 1" ParentGroupName="WatchKeeping" HeaderStyle-Width="20%" Name="WatchKeepingPeriod1" HeaderStyle-HorizontalAlign="Center">
                                    </telerik:GridColumnGroup>
                                </ColumnGroups>
                                <ColumnGroups>
                                    <telerik:GridColumnGroup HeaderText="Period 2" ParentGroupName="WatchKeeping" HeaderStyle-Width="20%" Name="WatchKeepingPeriod2" HeaderStyle-HorizontalAlign="Center">
                                    </telerik:GridColumnGroup>
                                </ColumnGroups>
                                <ColumnGroups>
                                    <telerik:GridColumnGroup HeaderText="Period 1" ParentGroupName="NonWatchKeeping" HeaderStyle-Width="20%" Name="NonWatchKeepingPeriod1" HeaderStyle-HorizontalAlign="Center">
                                    </telerik:GridColumnGroup>
                                </ColumnGroups>
                                <ColumnGroups>
                                    <telerik:GridColumnGroup HeaderText="Period 2" ParentGroupName="NonWatchKeeping" HeaderStyle-Width="20%" Name="NonWatchKeepingPeriod2" HeaderStyle-HorizontalAlign="Center">
                                    </telerik:GridColumnGroup>
                                </ColumnGroups>
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="From" ColumnGroupName="WatchKeepingPeriod1" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblresthrdefaultworkhrs" Visible="false" Width="0%" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRESTHOURSDEFAULTWORKHOURSID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblWKFrom1" Width="10%" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWATCHKEEPINGFROM1") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadLabel ID="lblresthrdefaultworkhrs" Visible="false" Width="10%" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRESTHOURSDEFAULTWORKHOURSID") %>'></telerik:RadLabel>
                                            <telerik:RadComboBox runat="server" ID="ddlWKPeriod1From" Width="100%"></telerik:RadComboBox>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="To" ColumnGroupName="WatchKeepingPeriod1" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblwk1" Width="10%" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWATCHKEEPINGTO1") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadComboBox runat="server" ID="ddlWKPeriod1To" Width="100%"></telerik:RadComboBox>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="From" ColumnGroupName="WatchKeepingPeriod2" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblwk2" Width="10%" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWATCHKEEPINGFROM2") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadComboBox runat="server" ID="ddlWKPeriod2From" Width="100%"></telerik:RadComboBox>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="To" ColumnGroupName="WatchKeepingPeriod2" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblwk3" Width="10%" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWATCHKEEPINGTO2") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadComboBox runat="server" ID="ddlWKPeriod2To" Width="100%"></telerik:RadComboBox>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="From" ColumnGroupName="NonWatchKeepingPeriod1" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblwk4" Width="10%" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNONWATCHKEEPINGFROM3") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadComboBox runat="server" ID="ddlNWKPeriod1From" Width="100%"></telerik:RadComboBox>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="To" ColumnGroupName="NonWatchKeepingPeriod1" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblwk5" Width="10%" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNONWATCHKEEPINGTO3") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadComboBox runat="server" ID="ddlNWKPeriod1To" Width="100%"></telerik:RadComboBox>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="From" ColumnGroupName="NonWatchKeepingPeriod2" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblwk6" Width="10%" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNONWATCHKEEPINGFROM4") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadComboBox runat="server" ID="ddlNWKPeriod2From" Width="100%"></telerik:RadComboBox>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="To" ColumnGroupName="NonWatchKeepingPeriod2" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblwk7" Width="10%" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNONWATCHKEEPINGTO4") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadComboBox runat="server" ID="ddlNWKPeriod2To" Width="100%"></telerik:RadComboBox>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Action">
                                        <HeaderStyle HorizontalAlign="Center" Width="10%" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%" />
                                        <FooterStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%" />
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" Width="10%" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                                <span class="icon"><i class="fas fa-edit"></i></span>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Save" CommandName="Save" ID="cmdSave" ToolTip="Save">
                                                <span class="icon"><i class="fas fa-save"></i></span>
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                                <span class="icon"><i class="fas fa-times-circle"></i></span>
                                            </asp:LinkButton>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>
            </table>
            <br />
            <table runat="server" width="100%">
                <tr runat="server">
                    <td>
                        <p style="text-align:center">
                            <telerik:RadLabel ID="lblport" runat="server" Font-Bold="true" Text="Scheduled Daily Work Hours in Port"></telerik:RadLabel>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadGrid RenderMode="Lightweight" Width="100%" Height="50%" ID="gvWorkHoursPort" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                            CellSpacing="0" GridLines="None" EnableHeaderContextMenu="true" GroupingEnabled="false" AutoGenerateColumns="false"
                            OnNeedDataSource="gvWorkHoursPort_NeedDataSource" OnItemDataBound="gvWorkHoursPort_ItemDataBound" OnItemCommand="gvWorkHoursPort_ItemCommand"
                            ShowHeader="true" EnableViewState="true">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" DataKeyNames="FLDRESTHOURSDEFAULTWORKHOURSID">
                                <NoRecordsTemplate>
                                    <table runat="server" width="100%" border="0">
                                        <tr>
                                            <td align="center">
                                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </NoRecordsTemplate>
                                <ColumnGroups>
                                    <telerik:GridColumnGroup HeaderText="WatchKeeping" HeaderStyle-Width="40%" Name="WatchKeeping" HeaderStyle-HorizontalAlign="Center">
                                    </telerik:GridColumnGroup>
                                </ColumnGroups>
                                <ColumnGroups>
                                    <telerik:GridColumnGroup HeaderText="Non WatchKeeping" HeaderStyle-Width="40%" Name="NonWatchKeeping" HeaderStyle-HorizontalAlign="Center">
                                    </telerik:GridColumnGroup>
                                </ColumnGroups>
                                <ColumnGroups>
                                    <telerik:GridColumnGroup HeaderText="Period 1" ParentGroupName="WatchKeeping" HeaderStyle-Width="20%" Name="WatchKeepingPeriod1" HeaderStyle-HorizontalAlign="Center">
                                    </telerik:GridColumnGroup>
                                </ColumnGroups>
                                <ColumnGroups>
                                    <telerik:GridColumnGroup HeaderText="Period 2" ParentGroupName="WatchKeeping" HeaderStyle-Width="20%" Name="WatchKeepingPeriod2" HeaderStyle-HorizontalAlign="Center">
                                    </telerik:GridColumnGroup>
                                </ColumnGroups>
                                <ColumnGroups>
                                    <telerik:GridColumnGroup HeaderText="Period 1" ParentGroupName="NonWatchKeeping" HeaderStyle-Width="20%" Name="NonWatchKeepingPeriod1" HeaderStyle-HorizontalAlign="Center">
                                    </telerik:GridColumnGroup>
                                </ColumnGroups>
                                <ColumnGroups>
                                    <telerik:GridColumnGroup HeaderText="Period 2" ParentGroupName="NonWatchKeeping" HeaderStyle-Width="20%" Name="NonWatchKeepingPeriod2" HeaderStyle-HorizontalAlign="Center">
                                    </telerik:GridColumnGroup>
                                </ColumnGroups>
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="From" ColumnGroupName="WatchKeepingPeriod1" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblresthrdefaultworkhrsport" Visible="false" Width="0%" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRESTHOURSDEFAULTWORKHOURSID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblWKFrom1port" Width="10%" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWATCHKEEPINGFROM1") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadLabel ID="lblresthrdefaultworkhrsport" Visible="false" Width="10%" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRESTHOURSDEFAULTWORKHOURSID") %>'></telerik:RadLabel>
                                            <telerik:RadComboBox runat="server" Width="100%" ID="ddlWKPeriod1Fromport"></telerik:RadComboBox>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="To" ColumnGroupName="WatchKeepingPeriod1" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblwk1port" Width="10%" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWATCHKEEPINGTO1") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadComboBox runat="server" Width="100%" ID="ddlWKPeriod1Toport"></telerik:RadComboBox>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="From" ColumnGroupName="WatchKeepingPeriod2" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblwk2port" Width="10%" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWATCHKEEPINGFROM2") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadComboBox runat="server" Width="100%" ID="ddlWKPeriod2Fromport"></telerik:RadComboBox>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="To" ColumnGroupName="WatchKeepingPeriod2" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblwk3port" Width="10%" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWATCHKEEPINGTO2") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadComboBox runat="server" Width="100%" ID="ddlWKPeriod2Toport"></telerik:RadComboBox>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="From" ColumnGroupName="NonWatchKeepingPeriod1" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblwk4port" Width="10%" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNONWATCHKEEPINGFROM3") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadComboBox runat="server" Width="100%" ID="ddlNWKPeriod1Fromport"></telerik:RadComboBox>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="To" ColumnGroupName="NonWatchKeepingPeriod1" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblwk5port" Width="10%" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNONWATCHKEEPINGTO3") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadComboBox runat="server" Width="100%" ID="ddlNWKPeriod1Toport"></telerik:RadComboBox>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="From" ColumnGroupName="NonWatchKeepingPeriod2" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblwk6port" Width="10%" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNONWATCHKEEPINGFROM4") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadComboBox runat="server" Width="100%" ID="ddlNWKPeriod2Fromport"></telerik:RadComboBox>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="To" ColumnGroupName="NonWatchKeepingPeriod2" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblwk7port" Width="10%" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNONWATCHKEEPINGTO4") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadComboBox runat="server" Width="100%" ID="ddlNWKPeriod2Toport"></telerik:RadComboBox>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Action">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%" />
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%" />
                                        <FooterStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%" />
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" Width="10%" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                                <span class="icon"><i class="fas fa-edit"></i></span>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Save" CommandName="Save" ID="cmdSave" ToolTip="Save">
                                                <span class="icon"><i class="fas fa-save"></i></span>
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                                <span class="icon"><i class="fas fa-times-circle"></i></span>
                                            </asp:LinkButton>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <br />
                        <telerik:RadLabel ID="lblWorkHoursAtSea" runat="server" Text="Work Hours At Sea" Font-Bold="true" Visible="false"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <eluc:WorkSheet Id="DesignationWSheetAtSea" runat="server" OnTimeStripCommand="DesignationWSheetAtSea_OnTimeStripCommand"
                            CssClass="input txtNumber" ShowTotalHours="false" Enable="true" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                        <telerik:RadLabel ID="lblWorkHoursAtPort" runat="server" Text="Work Hours At Port" Font-Bold="true" Visible="false"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <eluc:WorkSheet Id="DesignationWSheetAtPort" runat="server" OnTimeStripCommand="DesignationWSheetAtPort_OnTimeStripCommand"
                            CssClass="input txtNumber" ShowTotalHours="false" Enable="true" Visible="false" />
                    </td>
                </tr>
            </table>
            <br />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
