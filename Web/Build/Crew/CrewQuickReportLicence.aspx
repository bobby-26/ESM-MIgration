<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewQuickReportLicence.aspx.cs"
    Inherits="Crew_CrewQuickReportLicence" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationalityList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Documents" Src="~/UserControls/UserControlDocuments.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
         <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvCrew.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
            }
            </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                TabStrip="false"></eluc:TabStrip>
            <br />
            <table cellpadding="0" cellspacing="0" border="1">
                <tr>
                    <td style="padding-left:10px">
                        <table>
                            <tr>
                                <td>
                                    <telerik:RadLabel ForeColor="Black"   ID="lblFromDate1" runat="server" Text="Licence"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Date ID="ucSignOnFromDate" runat="server" CssClass="input_mandatory"  Width="32%" />
                                    <eluc:Date ID="ucSignOnToDate" runat="server" CssClass="input_mandatory"  Width="32%" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel  ForeColor="Black"  ID="lblname" runat="server" Text="Name"   ></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtname" runat="server"  Width="66%" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel  ForeColor="Black"  ID="lblFileno" runat="server" Text="File No."></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtFileNo" runat="server"   Width="66%" />
                                </td>
                            </tr>
                        </table>
                    </td>
                   <td style="padding-left:10px">
                        <telerik:RadLabel  ForeColor="Black"  ID="lblLicence" Text="Licence" runat="server"></telerik:RadLabel>
                       <br />
                        <telerik:RadListBox ID="lstLicence" runat="server" SelectionMode="Multiple" Height="80px"
                            DataTextField="FLDDOCUMENTNAME" DataValueField="FLDDOCUMENTID"   Width="300px" />
                    </td>
                    <td style="padding-left:10px">
                        <telerik:RadLabel ForeColor="Black"   ID="lblLicenceNationality" runat="server" Text="Licence Nationality"></telerik:RadLabel>
                       
                        <eluc:Nationality ID="ucNationality1" runat="server" AppendDataBoundItems="true" height="80px"   Width="300px" />
                    </td>
                </tr>
            </table>
            <br />

            <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid ID="gvCrew" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                CellPadding="3" RowStyle-Wrap="false" ShowHeader="true" EnableViewState="false" OnItemCommand="gvCrew_ItemCommand"
                OnItemDataBound="gvCrew_ItemDataBound" CellSpacing="0" GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnNeedDataSource="gvCrew_NeedDataSource" RenderMode="Lightweight" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel  ForeColor="Black"  ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>

                        <telerik:GridTemplateColumn HeaderText="Licence No.">
                            <ItemTemplate>
                                <telerik:RadLabel  ForeColor="Black"  ID="lblEmpFileNo" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>' />
                                <telerik:RadLabel  ForeColor="Black"  ID="lblEmpNo" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>' />
                                <telerik:RadLabel  ForeColor="Black"  ID="lnkName" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>' />
                                <telerik:RadLabel  ForeColor="Black"  ID="lblRank" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>' />
                                <telerik:RadLabel  ForeColor="Black"  ID="lblBatch" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCH") %>' />
                                <telerik:RadLabel  ForeColor="Black"  ID="lblZONE" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDZONENAME") %>' />
                                <telerik:RadLabel  ForeColor="Black"  ID="lblLastVesselname" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTVESSELNAME") %>' />
                                <telerik:RadLabel  ForeColor="Black"  ID="lblSignOffDate" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTSIGNOFFDATE") %>' />
                                <telerik:RadLabel  ForeColor="Black"  ID="lblPresentVessel" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRESENTVESSELNAME") %>' />
                                <telerik:RadLabel  ForeColor="Black"  ID="lblSignon" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRESENTSIGNONDATE") %>' />
                                <telerik:RadLabel  ForeColor="Black"  ID="lblDOA" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOA") %>' />
                                <telerik:RadLabel  ForeColor="Black"  ID="lbllastcontact" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTCONTACTDATE") %>' />
                                <telerik:RadLabel  ForeColor="Black"  ID="lblActive" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>' />
                                <telerik:RadLabel  ForeColor="Black"  ID="lblRequirement" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAFARERREQUIREMENT") %>' />
                                <telerik:RadLabel  ForeColor="Black"  ID="lblLicenceNo" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLICENCENUMBER") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Licence">

                            <ItemTemplate>
                                <telerik:RadLabel  ForeColor="Black"  ID="lblLicence" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLICENCE") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date of Issue">

                            <ItemTemplate>
                                <telerik:RadLabel  ForeColor="Black"  ID="lblIssueDate" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFISSUE") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date of Expire">

                            <ItemTemplate>
                                <telerik:RadLabel ForeColor="Black"   ID="lblExpireDate" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFEXPIRY") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Place of Issue">

                            <ItemTemplate>
                                <telerik:RadLabel  ForeColor="Black"  ID="lblPlaceOfIssue" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Nationality">

                            <ItemTemplate>
                                <telerik:RadLabel  ForeColor="Black"  ID="lblNationality" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNATIONALITY") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>

                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
