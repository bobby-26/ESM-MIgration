<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewAvailableEmployeeList.aspx.cs" Inherits="Crew_CrewAvailableEmployeeList" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Batch" Src="~/UserControls/UserControlInstituteBatchList.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Available Employees</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuTitle" runat="server" TabStrip="true" OnTabStripCommand="MenuTitle_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />            
            <eluc:TabStrip ID="MenuHeader" runat="server" TabStrip="false" Title="Add Participants"></eluc:TabStrip>

            <table id="tblBatchEnrollment" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCourse" runat="server" Text="Course"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtCourse" CssClass="readonlytextbox" ReadOnly="true" Width="300px"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtcourseId" runat="server" Visible="false"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtcourseInstitute" runat="server" Visible="false"></telerik:RadTextBox>
                        <asp:HiddenField ID="hdnBatchVenue" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblInstitute" runat="server" Text="Institute"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtInstitute" CssClass="readonlytextbox" ReadOnly="true" Width="300px"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtInstituteId" runat="server" Visible="false"></telerik:RadTextBox>
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBatchVenue" runat="server" Text="Venue"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtBatchVenue" CssClass="readonlytextbox" ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAvailableBetween" runat="server" Text="Available"></telerik:RadLabel>
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="lblFrom" runat="server" Text="From"></telerik:RadLabel>
                                </td>
                                <td></td>
                                <td align="center">
                                    <telerik:RadLabel ID="lblTo" runat="server" Text="To"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <eluc:Date ID="txtAvailableFrom" runat="server" CssClass="input" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblDash" runat="server" Text="-"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Date ID="txtAvailableTo" runat="server" CssClass="input" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="txtBatchNo" runat="server" Text="Batch No"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Batch ID="ddlbatch" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" />
                    </td>
                </tr>
            </table>

            <br />
            <b>
                <telerik:RadLabel Text="Available Participants" runat="server" ID="lblTitle"></telerik:RadLabel>
            </b>

            <eluc:TabStrip ID="MenuGrid" runat="server" OnTabStripCommand="MenuGrid_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvAvailableEmployee" runat="server" AllowCustomPaging="true" AllowSorting="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvAvailableEmployee_ItemCommand" OnNeedDataSource="gvAvailableEmployee_NeedDataSource"
                OnItemDataBound="gvAvailableEmployee_ItemDataBound" EnableViewState="false" Height="99%" AllowPaging="true"
                EnableHeaderContextMenu="true">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center">
                    <GroupByExpressions>
                        <telerik:GridGroupByExpression>
                            <SelectFields>
                                <telerik:GridGroupByField FieldName="FLDGROUPBY" SortOrder="Ascending" FieldAlias="Details" />
                                <%--<telerik:GridGroupByField FieldName="FLDNAME" SortOrder="Ascending" FieldAlias="Details"  />--%>
                            </SelectFields>
                            <GroupByFields>
                                <telerik:GridGroupByField FieldName="FLDGROUPBY" />
                            </GroupByFields>
                        </telerik:GridGroupByExpression>
                    </GroupByExpressions>
                    <HeaderStyle Width="102px" />
                      <NoRecordsTemplate>
                      <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn Visible="false" UniqueName="Data" DataField="FLDGROUPBY">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblGroup" runat="server"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO")%>'>
                                </telerik:RadLabel>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Image ID="imgFlag" runat="server" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="" HeaderStyle-Width="110px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <img id="imgAvailability" alt="" src="<%$ PhoenixTheme:images/yellow.png%>" runat="server" visible="false" />
                                <img id="Img4" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png%>" />
                                <img id="imgPreferredLocation" alt="" src="<%$ PhoenixTheme:images/red.png%>" runat="server" visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="File No." UniqueName="FileNO" DataField="FLDFILENO">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>
                                <asp:HiddenField runat="server" ID="hdnreferencedtkey" Value='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCEDTKEY") %>' />
                                <telerik:RadLabel ID="lblPreferredLocationyn" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPREFERREDLOCATIONYN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Availability">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:HiddenField runat="server" ID="hdnempavailabilityDateId" Value='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEAVAILABILITYDATESID") %>' />
                                <asp:HiddenField runat="server" ID="hdnEmployeeId" Value='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>' />
                                <asp:HiddenField runat="server" ID="hdnenrollmentId" Value='<%# DataBinder.Eval(Container,"DataItem.FLDCREWBATCHENROLLMENTID") %>' />
                                <telerik:RadLabel ID="lblAvailableStartDate" runat="server"
                                    Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDAVAILABLESTARTDATE","{0:dd/MMM/yyyy}")) %>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblAvailableEndDate" runat="server"
                                    Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDAVAILABLEENDDATE","{0:dd/MMM/yyyy}")) %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Preferred Location">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCITYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="110px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Add To List"
                                    CommandName="ADDTOBATCH" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAddToBatch"
                                    ToolTip="Add To List" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-check-circle-aptl"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Availability Add" Visible="false"
                                    CommandName="AVAILABILITY" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAvailabilityAdd"
                                    ToolTip="Availability Add" Width="20PX" Height="20PX">
                                <span class="icon"><i class="far fa-calendar-plus"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <table cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <img id="Img1" src="<%$ PhoenixTheme:images/yellow.png%>" runat="server" />
                    </td>
                    <td>- Available date not given
                    </td>
                    <td>
                        <img id="Img2" src="<%$ PhoenixTheme:images/red.png%>" runat="server" />
                    </td>
                    <td>- Preferred location not given
                    </td>
                    <%--<td>
                                <img id="Img5" src="<%$ PhoenixTheme:images/purple.png%>" runat="server" />
                            </td>
                            <td>- Enrolled into another batch
                            </td>--%>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
