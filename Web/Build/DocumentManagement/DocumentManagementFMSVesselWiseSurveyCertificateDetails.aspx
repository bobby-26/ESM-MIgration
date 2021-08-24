<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementFMSVesselWiseSurveyCertificateDetails.aspx.cs"
    Inherits="DocumentManagementFMSVesselWiseSurveyCertificateDetails" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersSurvey" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div>
                <table width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblCertificateName" runat="server" Text="Certificate"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtCertificateName" runat="server" CssClass="readonlytextbox" Width="250px"
                                ReadOnly="true">
                            </telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblCertificateCategory" runat="server" Text="Type"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtCertificateCategory" runat="server" CssClass="readonlytextbox"
                                Width="250px" ReadOnly="true">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <eluc:TabStrip ID="MenuSurveySchedule" runat="server" OnTabStripCommand="MenuSurveySchedule_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvSurvey" runat="server" AutoGenerateColumns="False" OnItemDataBound="gvSurvey_ItemDataBound"
                Width="100%"  ShowHeader="true" OnNeedDataSource="gvSurvey_NeedDataSource" AllowCustomPaging="true" Height="100%"
                EnableViewState="true" AllowSorting="true" AllowPaging="true" EnableHeaderContextMenu="true">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <SortingSettings SortedBackColor="#CCE5FF" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Last Audit/Survey" Name="LastSurvey" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Next Audit/Survey" Name="NextSurvey" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Planned" Name="Planned" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Vessel" UniqueName="Data">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="12%"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblVesselHeader" runat="server" Text="Vessel"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVessel" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Fleet">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="5%"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblFleetHeader" runat="server" Text="Fleet"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFleet" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFLEETDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Issue Date">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="5%"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblIssueDateHeader" runat="server" Text="Issued"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIssueDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDATEOFISSUE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Expiry Date">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="5%"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblExpiryDateHeader" runat="server" Text="Expiry"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDNOEXPIRY").ToString().Equals("1") ? "Permanent" : General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDATEOFEXPIRY"))%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Issued By">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="15%"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblIssuedByHeader" runat="server" Text="Issued By"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIssuedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUINGAUTHORITYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type" ColumnGroupName="LastSurvey">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="7%"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblNextSurveyTypeHeader" runat="server" Text="Type"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDTEMPLATENAME").ToString().Equals("") || DataBinder.Eval(Container, "DataItem.FLDNOEXPIRY").ToString().Equals("1") ? "N/A" : DataBinder.Eval(Container, "DataItem.FLDLASTSURVEYTYPENAME")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date" ColumnGroupName="LastSurvey">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="5%"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblDoneDateHeader" runat="server" Text="Date"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDTEMPLATENAME").ToString().Equals("") || DataBinder.Eval(Container, "DataItem.FLDNOEXPIRY").ToString().Equals("1") ? "N/A" : General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDLASTSURVEYDATE"))%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn ColumnGroupName="NextSurvey">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="7%"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblSurveyTypeHeader" runat="server" Text="Type"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDTEMPLATENAME").ToString().Equals("") || DataBinder.Eval(Container, "DataItem.FLDNOEXPIRY").ToString().Equals("1") ? "N/A" : DataBinder.Eval(Container, "DataItem.FLDSURVEYTYPENAME")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn ColumnGroupName="NextSurvey">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="5%"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblDueDate" runat="server" Text="Due"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblDueDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDNEXTDUEDATE")) %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn ColumnGroupName="NextSurvey" UniqueName="WinRange">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="6%"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblWindoweHeader" runat="server" Text="Window"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%-- <asp:Label ID="lblWindow" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWINDOWPERIOD") %>'></asp:Label>--%>
                                <%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDWINDOWPERIODBEFORE"))%> - 
                                    <%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDWINDOWPERIODAFTER"))%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn ColumnGroupName="Planned">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="5%"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblPlannedDateHeader" runat="server" Text="Date"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPlannedDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDPLANDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn ColumnGroupName="Planned">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="8%"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblPortHeader" runat="server" Text="Port"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPort" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="7%"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblAnniversaryDateHeader" runat="server" Text="Ini. Audit / Ann. Date "></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblAnniversaryDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDANNIVERSARYDATE")) %>'></asp:Label>
                                <eluc:ToolTip ID="ucRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFULLREMARKS") %>'
                                    Width="250px" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="15%"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblRemarksHeader" runat="server" Text="Remarks"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATEREMARKS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">
                                    Action
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                            <asp:ImageButton runat="server" ID="cmdSvyAtt" ToolTip="Certificate" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" />
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="false" SaveScrollPosition="true" UseStaticHeaders="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <eluc:Status runat="server" ID="ucStatus" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
