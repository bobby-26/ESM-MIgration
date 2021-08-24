﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewQuickReportEmployeeSearch.aspx.cs"
    Inherits="Crew_CrewQuickReportEmployeeSearch" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
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
                      
                <table cellpadding="0" cellspacing="0" >
                    <tr valign="top">
                        <td style="padding-bottom:10px;padding-top:10px">
                            <table >
                                <tr >
                                    <td>
                                        <telerik:RadLabel ForeColor="Black"   ID="lblname" runat="server" Text="Name"></telerik:RadLabel>
                                    </td>
                                    <td style="padding-right:20px">
                                        <telerik:RadTextBox ID="txtname" runat="server" CssClass="input" Width="150px"></telerik:RadTextBox>
                                    </td>
                               
                                    <td>
                                        <telerik:RadLabel  ForeColor="Black"  ID="lblFileNo" runat="server" Text="File No."></telerik:RadLabel>
                                    </td>
                                    <td style="padding-right:20px">
                                        <telerik:RadTextBox ID="txtFileNo" runat="server" CssClass="input" Width="150px"></telerik:RadTextBox>
                                    </td>
                               
                                    <td>
                                        <telerik:RadLabel  ForeColor="Black"  ID="lblSEAMANBOOKNO" runat="server" Text="Seaman Book No."></telerik:RadLabel>
                                    </td>
                                    <td style="padding-right:20px">
                                        <telerik:RadTextBox ID="txtSeamanBookNo" runat="server" CssClass="input" Width="150px"></telerik:RadTextBox>
                                    </td>
                               
                                    <td>
                                        <telerik:RadLabel  ForeColor="Black"  ID="lblpassportno" runat="server" Text="Passport No."></telerik:RadLabel>
                                    </td>
                                    <td style="padding-right:20px">
                                        <telerik:RadTextBox ID="txtpassportno" runat="server" CssClass="input" Width="150px"></telerik:RadTextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>

                <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand"></eluc:TabStrip>
                <telerik:RadGrid ID="gvCrew" runat="server" AutoGenerateColumns="False" 
                    CellPadding="2" RowStyle-Wrap="false" ShowHeader="true" EnableViewState="false" OnItemCommand="gvCrew_ItemCommand"
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

                            <telerik:GridTemplateColumn  HeaderText="S.No."  >
                                
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container,"DataItem.FLDROW") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn  HeaderText="File No."  >
                               
                                <ItemTemplate>
                                    <telerik:RadLabel  ForeColor="Black"  ID="lblEmpFileNo" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn  HeaderText="Name"  >
                              
                                <ItemTemplate>
                                    <telerik:RadLabel  ForeColor="Black"  ID="lblEmpNo" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>' />
                                    <asp:LinkButton ID="lnkName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn  HeaderText="Passport No."  >
                               
                                <ItemTemplate>
                                    <telerik:RadLabel  ForeColor="Black"  ID="lblPassPort" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSPORTNO") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn  HeaderText="Seaman Book No"  >
                               <HeaderStyle Width="7%" />
                                <ItemTemplate>
                                    <telerik:RadLabel ForeColor="Black"   ID="lblSeamanBookNo" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAMANBOOKNO") %>' />

                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn  HeaderText="Rank"  >
                                
                                <ItemTemplate>
                                    <telerik:RadLabel  ForeColor="Black"  ID="lblRank" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn  HeaderText="Batch"  >
                              
                                <ItemTemplate>
                                    <telerik:RadLabel  ForeColor="Black"  ID="lblBatch" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCH") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn  HeaderText="Zone"  >
                                
                                <ItemTemplate>
                                    <telerik:RadLabel  ForeColor="Black"  ID="lblZONE" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDZONENAME") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn  HeaderText="Last Vessel"  >
                                <HeaderStyle Width="7%" />
                                <ItemTemplate>
                                    <telerik:RadLabel  ForeColor="Black"  ID="lblLastVesselname" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTVESSELNAME") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn  HeaderText="Sign-Off"  >
                                
                                <ItemTemplate>
                                    <telerik:RadLabel  ForeColor="Black"  ID="lblSignOffDate" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTSIGNOFFDATE") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn  HeaderText="Present Vessel"  >
                               <HeaderStyle Width="7%" />
                                <ItemTemplate>
                                    <telerik:RadLabel  ForeColor="Black"  ID="lblPresentVessel" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRESENTVESSELNAME") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Sign-On"  > 
                               
                                <ItemTemplate>
                                    <telerik:RadLabel  ForeColor="Black"  ID="lblSignon" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRESENTSIGNONDATE") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn  HeaderText="Remarks"  >
                              
                                <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Remarks" ID="imgToolTip" >
                                         <span class="icon"><i class="fas fa-list-alt-picklist"></i></span>
                                </asp:LinkButton>
                            <%--<telerik:RadLabel ID="lblremarks" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTNOTESDESCRIPTION") %>' Width="50px"/>--%>
                                <eluc:ToolTip ID="ucToolTip" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTNOTESDESCRIPTION") %>' TargetControlId="imgToolTip" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn  HeaderText="Availability"  >
                                
                                <ItemTemplate>
                                    <telerik:RadLabel  ForeColor="Black"  ID="lblDOA" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOA") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn  HeaderText="Last Contact"  >
                               <ItemTemplate>
                                    <telerik:RadLabel  ForeColor="Black"  ID="lbllastcontact" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTCONTACTDATE") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn  HeaderText="Status"  >
                                
                                <ItemTemplate>
                                    <telerik:RadLabel  ForeColor="Black"  ID="lblActive" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn  HeaderText="Requirement"  >
                                
                                <ItemTemplate>
                                    <telerik:RadLabel  ForeColor="Black"  ID="lblRequirement" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAFARERREQUIREMENT") %>' />
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
