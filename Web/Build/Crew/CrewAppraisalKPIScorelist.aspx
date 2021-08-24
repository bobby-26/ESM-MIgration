<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewAppraisalKPIScorelist.aspx.cs" Inherits="Crew_CrewAppraisalKPIScorelist" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="ajaxToolkit" Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Appraisal KPI Score</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmCrewAppraisalTraining" DecoratedControls="All" EnableRoundedCorners="true" />
    <form id="frmCrewAppraisalTraining" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuPersonalPromotion" runat="server"></eluc:TabStrip>
            <b>
                <asp:Literal ID="lblinspection" runat="server" Text="Inspection"></asp:Literal>
            </b>
            <telerik:RadGrid ID="gvCrewAppraisalkpi" Width="100%" runat="server"
                OnNeedDataSource="gvCrewAppraisalkpi_NeedDataSource" ShowFooter="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                    AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Inspection/Vetting">
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <asp:Label ID="lblCourseName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYSHORTCODE") %>'></asp:Label>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Is DET/REJ">
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <asp:Label ID="lblDocType" runat="server" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDISDET").ToString() == "1" ? "Yes" : "No" %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="No.of Inspection">
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <asp:Label ID="lblIssueDate" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALINSPECTION") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="No. of Deficiency">
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <asp:Label ID="lblIssueDate" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEFCOUNT") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Average">
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <asp:Label ID="lblInstitution" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAVG","{0:.####}").ToString() %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Score">
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <asp:Label ID="lblInstitution" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCORE") %>'></asp:Label>
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
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <b>
                <asp:Literal ID="lblincident" runat="server" Text="Incident"></asp:Literal>
            </b>
            <telerik:RadGrid ID="gvkpiincident" Width="100%" runat="server"
                OnNeedDataSource="gvCrewAppraisalkpi_NeedDataSource" ShowFooter="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                    AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Category">
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <asp:Label ID="lblDocType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYSHORTCODE") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Count">
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                 <asp:Label ID="lblIssueDate" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEFCOUNT") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="No.of Inspection">
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <asp:Label ID="lblIssueDate" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALINSPECTION") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                       
                        <telerik:GridTemplateColumn HeaderText="Score">
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <asp:Label ID="lblInstitution" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCORE") %>'></asp:Label>
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
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
          
        </telerik:RadAjaxPanel>
    </form>
</body>
