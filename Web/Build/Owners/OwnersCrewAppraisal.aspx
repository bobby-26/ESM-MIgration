<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnersCrewAppraisal.aspx.cs"
    Inherits="CrewAppraisalMidtenureactivity" ValidateRequest="false" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Occassion" Src="~/UserControls/UserControlOccassionForReport.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Apprasial Form</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style type="text/css">
            .hidden {
                display: none;
            }

            .center {
                background: fixed !important;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmCrewAppraisalactivity" DecoratedControls="All" EnableRoundedCorners="true" />
    <form id="frmCrewAppraisalactivity" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="AppraisalTabs" runat="server" OnTabStripCommand="AppraisalTabs_TabStripCommand"
            TabStrip="true"></eluc:TabStrip>
        <eluc:TabStrip ID="CrewAppraisal" runat="server" OnTabStripCommand="CrewAppraisal_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="80%" EnableAJAX="false">
            <div id="divPrimarySection" runat="server">
                <b>
                    <telerik:RadLabel ID="lblPrimaryDetails" runat="server" Text="Primary Details"></telerik:RadLabel>
                </b>
                <table width="99%" cellpadding="1" cellspacing="1">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblName" runat="server" Text="Officer's Name:"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtEmployeeName" runat="server" ReadOnly="true" Enabled="false" Width="180px" CssClass="readonlytextbox"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblEmployeeNumber" runat="server" Text="File No."></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true" Enabled="false" Width="180px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblRank" runat="server" Text="Rank:"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true" Enabled="false" Width="180px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblVesselName" runat="server" Text="Vessel:"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Vessel ID="ddlVessel" runat="server" AppendDataBoundItems="true" VesselsOnly="true"
                                CssClass="input_mandatory" VesselList="<%#PhoenixRegistersVessel.ListVessel() %>" Width="180px" />
                        </td>
                        <td colspan="2">
                            <asp:Panel ID="pnlDate" GroupingText="Period of appraisal" runat="server">
                                <telerik:RadLabel ID="labelFrom" runat="server" Text="From"></telerik:RadLabel>
                                <eluc:Date ID="txtFromDate" runat="server" CssClass="input_mandatory" />
                                <telerik:RadLabel ID="lblToDate" runat="server" Text="To"></telerik:RadLabel>
                                <eluc:Date ID="txtToDate" runat="server" CssClass="input_mandatory" />
                            </asp:Panel>
                            <td>
                                <telerik:RadLabel ID="lblAppraisalDate" runat="server" Text="Appraisal Date:"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date runat="server" ID="txtdate" CssClass="input_mandatory" />
                            </td>
                    </tr>
                    <tr>

                        <td>
                            <telerik:RadLabel ID="lblOccassionForReport" runat="server" Text="Occasion For Report:"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Occassion ID="ddlOccassion" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" ActiveYN="1" Enabled="false" />

                        </td>
                        <div id="divSignondate" runat="server">
                            <td>
                                <telerik:RadLabel ID="lblSignOnDate" runat="server" Text="Sign On:"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="txtSignOnDate" runat="server" CssClass="readonly_textbox" ReadOnly="true" Enabled="false" />
                            </td>
                        </div>
                    </tr>
                </table>
            </div>
            <div id="divOtherSection" runat="server">
                <hr />
                <table cellspacing="0" cellpadding="1" border="1" style="width: 99%" rules="all">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblos" runat="server" Text="(OS)"></telerik:RadLabel>
                        </td>
                        <td align="center">
                            <telerik:RadLabel ID="lblOutstanding" runat="server" Text=" <b>Outstanding -</b> A standard rarely achieved by others. Exceptional Performance"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblep" runat="server" Text="(EE)"></telerik:RadLabel>
                        </td>
                        <td align="center">
                            <telerik:RadLabel ID="lblExceeds" runat="server" Text=" <b>Exceeds Expectations –</b> A standard exceeding the job requirements."></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadLabel ID="Literal1" runat="server" Text="(ME)"></telerik:RadLabel>
                        </td>
                        <td align="center">
                            <telerik:RadLabel ID="Literal2" runat="server" Text=" <b>Meets Expectations -</b> A standard fully meeting all the requirements of the job."></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadLabel ID="Literal3" runat="server" Text="(BE)"></telerik:RadLabel>
                        </td>
                        <td align="center">
                            <telerik:RadLabel ID="Literal4" runat="server" Text=" <b>Below Expectations -</b> A standard of limited effectiveness and needs improvement."></telerik:RadLabel>
                        </td>
                    </tr>
                </table>
                <br />
                <table width="99%" cellpadding="1" cellspacing="1">
                    <tr>
                        <td valign="top">
                            <telerik:RadGrid RenderMode="Lightweight" ID="gvmidturn" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false" CellSpacing="0" GridLines="None" OnItemDataBound="gvmidturn_ItemDataBound" EnableViewState="false"
                                ShowFooter="false" ShowHeader="true" OnNeedDataSource="gvmidturn_NeedDataSource">
                                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true" AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center">
                                    <GroupByExpressions>
                                        <telerik:GridGroupByExpression>
                                            <SelectFields>
                                                <telerik:GridGroupByField FieldName="FLDCATEGORYDESC" FieldAlias="Category" SortOrder="Ascending" />
                                            </SelectFields>
                                            <GroupByFields>
                                                <telerik:GridGroupByField FieldName="FLDCATEGORYID" SortOrder="Ascending" />
                                            </GroupByFields>
                                        </telerik:GridGroupByExpression>
                                    </GroupByExpressions>
                                    <Columns>
                                        <telerik:GridTemplateColumn HeaderText="Job role">
                                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="60%"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblcategoryDesc" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDCATEGORYDESC")%>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblcategory" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDCATEGORYID")%>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblJobrole" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROFILEQUESTION") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Rating">
                                            <HeaderStyle Width="20%" />
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblAppraisalQuestionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPRAISALQUESTIONID") %>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblAppraisalScoreid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPRAISALSCOREID") %>'></telerik:RadLabel>
                                                <telerik:RadDropDownList ID="ddlscore" runat="server" CssClass="input_mandatory">
                                                    <Items>
                                                        <telerik:DropDownListItem Text="--Select--" Value=""></telerik:DropDownListItem>
                                                        <telerik:DropDownListItem Text="EE" Value="1"></telerik:DropDownListItem>
                                                        <telerik:DropDownListItem Text="ME" Value="2"></telerik:DropDownListItem>
                                                        <telerik:DropDownListItem Text="BE" Value="3"></telerik:DropDownListItem>
                                                        <telerik:DropDownListItem Text="OS" Value="4"></telerik:DropDownListItem>
                                                    </Items>
                                                </telerik:RadDropDownList>
                                                <telerik:RadLabel ID="ddlScoreId" runat="server" Visible="false"></telerik:RadLabel>
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
                                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" ResizeGridOnColumnResize="false" />
                                </ClientSettings>
                            </telerik:RadGrid>
                        </td>
                    </tr>
                </table>
                <hr />

                <table width="99%" cellpadding="1" cellspacing="1">
                    <tr>
                        <td><b>
                            <telerik:RadLabel ID="Literal5" runat="server" Text="Comments"></telerik:RadLabel>
                        </b></td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblHeadOfDept" runat="server" Text="Any other comments: (especially for areas that are below expectations)"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtHeadOfDeptComment" CssClass="input" Width="300px"
                                TextMode="MultiLine">
                            </telerik:RadTextBox>
                        </td>
                        <td colspan="2">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" style="color: Blue; font-weight: bold;">
                            <telerik:RadLabel ID="lblSaveChangesComment" runat="server" Text="1. Please click ‘save changes’ button on top of the screen for save your changes."></telerik:RadLabel>
                            <br />
                            <telerik:RadLabel ID="lblConfirmChangesComment" runat="server" Text="2. Please click &quot;Complete&quot; button to complete the appraisal."></telerik:RadLabel>
                        </td>
                    </tr>
                </table>
            </div>
            <eluc:Status runat="server" ID="ucStatus" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
