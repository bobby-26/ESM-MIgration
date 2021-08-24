<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersSeaPortCountryVisa.aspx.cs" Inherits="RegistersSeaPortCountryVisa" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Country Visa</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCountryVisa" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="90%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:TabStrip ID="MenuRegistersCountryVisaMain" runat="server" OnTabStripCommand="MenuRegistersCountryVisaMain_TabStripCommand"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuRegistersCountryVisa" runat="server" OnTabStripCommand="RegistersCountryVisa_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvCountryVisa" runat="server" AutoGenerateColumns="False" EnableViewState="false" Height="90%"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="true" ShowFooter="False"
                OnNeedDataSource="gvCountryVisa_NeedDataSource" EnableHeaderContextMenu="true" OnItemCommand="gvCountryVisa_ItemCommand"
                OnItemDataBound="gvCountryVisa_ItemDataBound1" OnSortCommand="gvCountryVisa_SortCommand">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                    AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Physical Presence" Name="Presence" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                    </ColumnGroups>
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Last Modified" Name="Modified" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Country Name" HeaderStyle-Width="12%" AllowSorting="true" ShowFilterIcon="true" SortExpression="FLDCOUNTRYNAME">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVisaID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVISAID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCountryName" runat="server" Visible="True" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Visa Type" HeaderStyle-Width="6%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVisaType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVISATYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Locations For Submission">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTimeTaken" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTIMETAKEN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Days Required">
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDaysRequried" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDAYSREQUIREDFORVISA") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Y/N" ColumnGroupName="Presence" HeaderStyle-Width="5%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="chkPhysicalPresenceYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPHYSICALPRESENCEYESNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Specification" ColumnGroupName="Presence" HeaderStyle-Width="8%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPhysicalPresenceSpecification" runat="server" ClientIDMode="AutoID" CssClass="tooltip" 
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDPHYSICALPRESENCESPECIFICATION").ToString().Length>10 ? DataBinder.Eval(Container, "DataItem.FLDPHYSICALPRESENCESPECIFICATION").ToString().Substring(0, 10) + "..." : DataBinder.Eval(Container, "DataItem.FLDPHYSICALPRESENCESPECIFICATION").ToString() %>'>
                                </telerik:RadLabel>
                                <eluc:ToolTip ID="ucPhyPresenceTT" runat="server" ClientIDMode="AutoID"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDPHYSICALPRESENCESPECIFICATION") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Urgent Procedure" HeaderStyle-Width="10%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblUrgentProcedure" runat="server" ClientIDMode="AutoID" CssClass="tooltip" 
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDURGENTPROCEDURE").ToString().Length>10 ? DataBinder.Eval(Container, "DataItem.FLDURGENTPROCEDURE").ToString().Substring(0, 10) + "..." : DataBinder.Eval(Container, "DataItem.FLDURGENTPROCEDURE").ToString() %>'>
                                </telerik:RadLabel>
                                <eluc:ToolTip ID="ucUrgentProcTT" runat="server"  ClientIDMode="AutoID"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDURGENTPROCEDURE") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks" HeaderStyle-Width="7%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Remarks"
                                    CommandName="REMARKS" CommandArgument="<%# Container.DataSetIndex %>" ID="imgRemarks"
                                    ToolTip="Add Remarks">
                                <span class="icon"><i class="fas fa-glasses"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="User" ColumnGroupName="Modified" HeaderStyle-Width="7%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblModifiedBy" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMODIFIEDBYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText=" Date" ColumnGroupName="Modified" HeaderStyle-Width="7%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblModifiedDate" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMODIFIEDDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
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
