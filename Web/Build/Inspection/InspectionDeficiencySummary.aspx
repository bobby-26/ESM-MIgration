<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionDeficiencySummary.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="InspectionDeficiencySummary" %>

<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Deficiency Summary</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <div id="divInspectionIncidentCriticalFactor" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        </div>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInspectionIncidentCriticalFactor" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status ID="ucStatus" runat="server" Text="" />
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <telerik:RadLabel ID="lblDeficiencySummary" runat="server" Text="Deficiency Summary" Visible="false"></telerik:RadLabel>
            <b>
                <telerik:RadLabel ID="lblDeficiencies" runat="server" Text="Deficiencies"></telerik:RadLabel>
            </b>
            <div id="divDeficiency" style="position: relative; z-index: 3; position: static">
                <telerik:RadGrid RenderMode="Lightweight" ID="gvDeficiency" runat="server" AutoGenerateColumns="False" OnItemCommand="gvDeficiency_ItemCommand" OnNeedDataSource="gvDeficiency_NeedDataSource"
                    Font-Size="11px" Width="100%" CellPadding="3" GroupingEnabled="false" EnableHeaderContextMenu="true" AllowCustomPaging="false" AllowPaging="false" AllowSorting="true" EnableViewState="true">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDDEFICIENCYID" TableLayout="Fixed">
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
                        <CommandItemSettings RefreshText="Search" ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Reference Number">
                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                <FooterStyle Wrap="true" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblDeficiencyid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDEFICIENCYID") %>'></telerik:RadLabel>
                                    <asp:LinkButton ID="lnkRefNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENUMBER") %>' CommandName="SELECT" CommandArgument='<%# Container.DataItem %>'></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Type">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="60px"></ItemStyle>
                                <FooterStyle Wrap="False" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDTYPE")%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Category">
                                <ItemStyle Wrap="True" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                <FooterStyle Wrap="True" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDDEFICIENCYCATEGORY")%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Item">
                                <ItemStyle Wrap="True" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                <FooterStyle Wrap="True" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDITEMNAME")%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Description">
                                <ItemStyle Wrap="True" HorizontalAlign="Left" Width="300px"></ItemStyle>
                                <FooterStyle Wrap="True" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDDESCRIPTION")%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Issued Date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="80px"></ItemStyle>
                                <FooterStyle Wrap="False" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <%#  General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDISSUEDDATE"))%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Status">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                                <FooterStyle Wrap="False" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDSTATUS")%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
            <br />
            <b>RCA</b>
            <br />
            <div id="divGridTC" style="position: relative; z-index: 0; width: 100%;">
                <telerik:RadGrid RenderMode="Lightweight" ID="gvMSCAT" runat="server" AutoGenerateColumns="False" Font-Size="11px" OnPreRender="gvMSCAT_PreRender" OnNeedDataSource="gvMSCAT_NeedDataSource"
                    ShowFooter="false" Width="100%" CellPadding="3" AllowSorting="true" EnableHeaderContextMenu="true" AllowCustomPaging="false" AllowPaging="false"
                    EnableViewState="true">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDMSCATID" TableLayout="Fixed">
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
                        <CommandItemSettings RefreshText="Search" ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="CAR">
                                <ItemStyle Wrap="True" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCAR" runat="server" Width="150px" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDEFICIENCYDETAILS")%>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Immediate Cause">
                                <ItemStyle Wrap="True" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblMscatid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMSCATID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblIC" runat="server" Width="150px" Text='<%# DataBinder.Eval(Container, "DataItem.FLDIMMEDIATECAUSE")%>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Remarks">
                                <ItemStyle Wrap="True" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblICRemarks" runat="server" Width="150px" Text='<%# DataBinder.Eval(Container, "DataItem.FLDICREMARKS")%>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Basic Cause">
                                <ItemStyle Wrap="True" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblBC" runat="server" Width="150px" Text='<%# DataBinder.Eval(Container, "DataItem.FLDBASICSUBCAUSE")%>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Remarks">
                                <ItemStyle Wrap="True" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblBCRemarks" runat="server" Width="150px" Text='<%# DataBinder.Eval(Container, "DataItem.FLDBCREMARKS")%>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Control Action Needs">
                                <ItemStyle Wrap="True" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCAN" runat="server" Width="150px" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSUBCONTROLACTIONNEEDED")%>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
            <br />
            <b>
                <telerik:RadLabel ID="lblCorrectiveAction" runat="server" Text="Corrective Action"></telerik:RadLabel>
            </b>
            <br />
            <div id="divGridCorrectiveAction" style="position: relative; z-index: 3; position: static">
                <telerik:RadGrid RenderMode="Lightweight" ID="gvCorrectiveAction" runat="server" AutoGenerateColumns="False"
                    Font-Size="11px" Width="100%" CellPadding="3" OnItemDataBound="gvCorrectiveAction_ItemDataBound" OnNeedDataSource="gvCorrectiveAction_NeedDataSource" EnableViewState="true">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
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
                        <CommandItemSettings RefreshText="Search" ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Checklist Ref No.">
                                <ItemStyle Wrap="true" HorizontalAlign="Center" Width="50px"></ItemStyle>
                                <FooterStyle Wrap="true" HorizontalAlign="Center" Width="50px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblChecklistRefNo" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCACHECKLISTREFNUMBER") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Deficiency Details">
                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="200px"></ItemStyle>
                                <FooterStyle Wrap="true" HorizontalAlign="Left" Width="200px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblDefDetails" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDEFICIENCYDETAILS") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Corrective Action">
                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="200px"></ItemStyle>
                                <FooterStyle Wrap="true" HorizontalAlign="Left" Width="200px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblInsCorrectActid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONCORRECTIVEACTIONID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblInsCorrectActDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblInsCorrectActISAttachment" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDATTACHMENTYN") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblCorrectiveAction" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCORRECTIVEACTION") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                                <FooterStyle Wrap="False" HorizontalAlign="Left" Width="90px" />
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblDepartmentHeader" runat="server">
                                        Department
                                    <br />
                                        (Assigned to)
                                    </telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblDept" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENTNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="80px"></ItemStyle>
                                <FooterStyle Wrap="true" HorizontalAlign="Left" Width="80px" />
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblTargetDateHeader" runat="server">
                                        Target<br />
                                        Date
                                    </telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblTargetDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDTARGETDATE")) %>'
                                        Width="80px">
                                    </telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="80px"></ItemStyle>
                                <FooterStyle Wrap="true" HorizontalAlign="Left" Width="80px" />
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblCompletionDateHeader1" runat="server">
                                        Completion
                                        <br />
                                        Date
                                    </telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCompletionDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOMPLETIONDATE")) %>'
                                        Width="80px">
                                    </telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="200px"></ItemStyle>
                                <FooterStyle Wrap="False" HorizontalAlign="Left" Width="200px" />
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblCompletionDateHeader" runat="server">
                                        Verification
                                        <br />
                                        Level
                                    </telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblVerificationLevel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="25px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Attachment" CommandName="ATTACHMENT" ID="imgEvidence" ToolTip="Uploaded Evidence">
                                     <span class="icon"> <i class="fa fa-paperclip"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
            <br />
            <b>
                <telerik:RadLabel ID="lblPreventiveAction" runat="Server" Text="Preventive Action"></telerik:RadLabel>
            </b>
            <br />
            <div id="divGridPreventiveAction" style="position: relative; z-index: 3; position: static">
                <telerik:RadGrid RenderMode="Lightweight" ID="gvPreventiveAction" runat="server" AutoGenerateColumns="False"
                    OnItemDataBound="gvPreventiveAction_ItemDataBound" OnNeedDataSource="gvPreventiveAction_NeedDataSource"
                    Font-Size="11px" Width="100%" CellPadding="3" EnableViewState="true">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
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
                        <CommandItemSettings RefreshText="Search" ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                        <Columns>
                            <telerik:GridTemplateColumn>
                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                <FooterStyle Wrap="true" HorizontalAlign="Left" />
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblControlActionHeader" runat="server">
                                        Control Action Needs
                                    </telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblControlAction" Width="190px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCONTROLACTIONNEEDS") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                <FooterStyle Wrap="true" HorizontalAlign="Left" />
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblPreventiveActionHeader" runat="server">
                                        Preventive Action
                                    </telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblInsPreventActid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONPREVENTIVEACTIONID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblPreventiveAction" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPREVENTIVEACTION") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <FooterStyle Wrap="False" HorizontalAlign="Left" />
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblDepartmentAssignedToHeader" runat="server">
                                        Department
                                    <br />
                                        (Assigned to)
                                    </telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblDept" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENTNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn Visible="false">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <FooterStyle Wrap="False" HorizontalAlign="Left" />
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblPICHeader" runat="server">
                                        PIC
                                    </telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblPIC" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDASSIGNEDPERSONNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <FooterStyle Wrap="true" HorizontalAlign="Left" />
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblPATargetDateHeader" runat="server">
                                        Target
                                        <br />
                                        Date
                                    </telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblPATargetDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDTARGETDATE")) %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <FooterStyle Wrap="true" HorizontalAlign="Left" />
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblPACompletionDateHeader" runat="server">
                                        Completion
                                        <br />
                                        Date
                                    </telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblPACompletionDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOMPLETIONDATE")) %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <FooterStyle Wrap="true" HorizontalAlign="Left" />
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblCategoryHeader" runat="server">Category</telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <FooterStyle Wrap="true" HorizontalAlign="Left" />
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblSubCategoryHeader" runat="Server">Subcategory</telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblSubCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBCATEGORYNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn Visible="false">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <FooterStyle Wrap="False" HorizontalAlign="Left" />
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblPAExtensionRequiredHeader" runat="server">
                                        Reschedule
                                        <br />
                                        Required
                                    </telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkPAExtensionRequired" runat="server" Checked="false" Enabled="false" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn Visible="false">
                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                <FooterStyle Wrap="true" HorizontalAlign="Left" />
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblPAExtensionReasonHeader" runat="server">
                                        Reschedule Reason
                                    </telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblPAExtensionReason" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXTENSIONREASON") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                <FooterStyle Wrap="true" HorizontalAlign="Left" />
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblWorkOrderHeader" runat="server">Work Order</telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkWorkOrderNumber" runat="server" CommandName="SHOWWORKORDER" CommandArgument='<%# Container.DataItem %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDWONUMBER") %>'></asp:LinkButton>
                                    <telerik:RadLabel ID="lblWorkOrderId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERID") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                    <telerik:RadLabel ID="lblWorkOrderNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWONUMBER") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                <FooterStyle Wrap="true" HorizontalAlign="Left" />
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblWoStatusHeader" runat="Server">Work Order Status</telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblWorkOrderStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
        </div>
    </form>
</body>
</html>
