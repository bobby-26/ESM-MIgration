<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewFlagCourseDone.aspx.cs" Inherits="Crew_CrewFlagCourseDone" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Flag" Src="~/UserControls/UserControlFlag.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
         <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvFlagCourseDone.ClientID %>"));
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
            <br />
            <table id="tblFlagCourseSearch" >
                <tr>
                    <td style="padding-right:30px">
                        &nbsp&nbsp&nbsp&nbsp<telerik:RadLabel  ForeColor="Black"  ID="lblcourse" runat="server" Text="Course" ></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlCourse" runat="server" DataTextField="FLDDOCUMENTNAME" DataValueField="FLDDOCUMENTID" OnDataBound="ddlCourse_DataBound"
                            CssClass="dropdown_mandatory" Width="63%" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select Course">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td style="padding-right:30px">
                        &nbsp&nbsp&nbsp&nbsp<telerik:RadLabel  ForeColor="Black"  ID="lblFlag" runat="server" Text="Flag"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Flag ID="ddlFlag" runat="server" AppendDataBoundItems="true" AutoPostBack="true" CssClass="input_mandatory" Width="63%" />
                    </td>
                </tr>
                <tr>

                    <td style="padding-right:30px">
                        &nbsp&nbsp&nbsp&nbsp<telerik:RadLabel ForeColor="Black"   ID="lblCrewChangeDate" runat="server" Text="Course Done Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtFromDate" runat="server" CssClass="input_mandatory" Width="30%" />
                        -
                        <eluc:Date ID="txtToDate" runat="server" CssClass="input_mandatory" Width="30%" />
                    </td>
                </tr>
            </table>

            <br />

            <eluc:TabStrip ID="MenuFlagCourseDone" runat="server" OnTabStripCommand="MenuFlagCourseDone_TabStripCommand" TabStrip="false"></eluc:TabStrip>

            <telerik:RadGrid ID="gvFlagCourseDone" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                CellPadding="3" RowStyle-Wrap="false" ShowHeader="true" EnableViewState="false" OnItemCommand="gvFlagCourseDone_ItemCommand"
                OnItemDataBound="gvFlagCourseDone_ItemDataBound" CellSpacing="0" GridLines="None" GroupingEnabled="true" EnableHeaderContextMenu="true"
                OnNeedDataSource="gvFlagCourseDone_NeedDataSource" RenderMode="Lightweight" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ForeColor="Black"   ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                   
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="" Name="A" HeaderStyle-HorizontalAlign="Center"    />
                        <telerik:GridColumnGroup HeaderText="Date" Name="Date" HeaderStyle-HorizontalAlign="Center" />
                    </ColumnGroups>

                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="S.No" ColumnGroupName="A">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel  ForeColor="Black"  ID="lblRowNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="File No"     ColumnGroupName="A">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel  ForeColor="Black"  ID="lblFileNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Name" ColumnGroupName="A">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <asp:LinkButton ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:LinkButton>
                                <telerik:RadLabel  ForeColor="Black"  ID="lblEmpId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>' Visible="false"></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Rank" ColumnGroupName="A">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel  ForeColor="Black"  ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="D.O.B" ColumnGroupName="A">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel  ForeColor="Black"  ID="lblDOB" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATEOFBIRTH","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="CDC NO" ColumnGroupName="A">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel  ForeColor="Black"  ID="lblCDC" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAMANBOOKNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Status" ColumnGroupName="A">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ForeColor="Black"   ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Institute" ColumnGroupName="A">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel  ForeColor="Black"  ID="lblInstitute" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSTITUTENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Place of Issue" ColumnGroupName="A">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel  ForeColor="Black"  ID="lblPlaceOfIssue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Issued On" ColumnGroupName="Date">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel  ForeColor="Black"  ID="lbldateofIssue" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATEOFISSUE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Sign On" ColumnGroupName="Date">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel  ForeColor="Black"  ID="lblSignondate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDSIGNONDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Vessel" ColumnGroupName="Date">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel  ForeColor="Black"  ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
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
