<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogGRB2OperationList.aspx.cs" Inherits="Log_ElectricLogGRB2OperationList" %>


<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <title>Entries in Garbage Record Book Part 2 </title>
    <telerik:radcodeblock id="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" src="<%=Session["sitepath"]%>/js/jquery-1.12.4.min.js"></script>
    </telerik:radcodeblock>
    <style>
        .bold {
            font-weight: bold;
            font-size: large;
            text-align: center;
        }
        .strike-through {
            text-decoration:line-through;
        }
        .signature {
            float: left;
            text-decoration: underline;
            font-size: 16px;
            font-weight: bold;
        }
        .displayNone {
            display: none;
        }

        .fa-unlock {
            background-color: red;
        }

        .fa-lock {
            background-color: green;
        }

        .not-signed {
            background-color: #ffc200;
            width: 250px;
            display: inline-block;
        }
        .user-info {
            float:right;
        }
    </style>
    <script>

        document.addEventListener("DOMContentLoaded", function () {
            pageOnLoad();
        });


        document.addEventListener("load", function () {
            pageOnLoad();
        });

        function pageOnUnload() {

        }

        function pageOnLoad() {
            $('.rgPagerTextBox').attr('readonly', true);
            $('.rgPagerButton').css('display', 'none');
        }
    </script>
</head>
<body>
    <form id="frmgvCounterUpdate" runat="server" submitdisabledcontrols="true">
        <telerik:radscriptmanager runat="server" id="RadScriptManager1" />
        <telerik:radskinmanager id="RadSkinManager1" runat="server" />

        <telerik:radwindowmanager rendermode="Lightweight" id="RadWindowManager1" runat="server" enableshadow="true">
        </telerik:radwindowmanager>

        <telerik:radajaxpanel id="RadAjaxPanel1" runat="server" height="95%" ClientEvents-OnRequestStart="pageOnUnload" ClientEvents-OnResponseEnd="pageOnLoad">

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <table class="user-info">
                <tr>
                    <td><telerik:RadLabel runat="server" ID="lblUsername"></telerik:RadLabel></td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFromDate" runat="server" Text="From"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtFromDate" runat="server" DatePicker="true" />
                    </td>  <td>
                        <telerik:RadLabel ID="lblToDate" runat="server" Text="To"></telerik:RadLabel>
                    </td>
                     <td>
                        <eluc:Date ID="txtToDate" runat="server" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <%--<telerik:RadComboBox DropDownPosition="Static" style="width:auto" AutoPostBack="true" ID="ddlStatus" runat="server"  EnableLoadOnDemand="True"
                            OnTextChanged="ddl_TextChanged"  EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                        </telerik:RadComboBox>--%>
                        <telerik:RadDropDownList runat="server" AutoPostBack="true" ID="ddlStatus" OnItemSelected="ddl_TextChanged" ></telerik:RadDropDownList>
                    </td>
                </tr>
            </table>
            <br />
<%--            <div style="width:100%; text-align:right" >
                <asp:HyperLink ID="lnkfilename" Target="_blank" Text="View" runat="server" ImageUrl="<%$ PhoenixTheme:images/54.png %>" Width="14px" 
                    Height="14px" ToolTip="User Guide">
                </asp:HyperLink>
            </div>--%>

            <table width="100%"> 
                <tr>
                    <td>
                        <b>Garbage Categories:</b> J - Cargo residues (non-HME) ; K - Cargo residues (HME)
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenugvCounterUpdate" runat="server" OnTabStripCommand="gvCounterUpdate_TabStripCommand"></eluc:TabStrip>      

            <telerik:RadGrid RenderMode="Lightweight" ID="gvElogTransaction" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" EnableViewState="true"
                CellSpacing="0" GridLines="None" Height="70%" OnGridExporting="gvElogTransaction_GridExporting"
                OnNeedDataSource="gvElogTransaction_NeedDataSource" 
                OnItemCommand="gvElogTransaction_ItemCommand" 
                OnItemDataBound="gvElogTransaction_ItemDataBound"
                OnPreRender="gvElogTransaction_PreRender"
                GroupingEnabled="false" EnableHeaderContextMenu="true">
                <ExportSettings IgnorePaging="true" OpenInNewWindow="true" FileName="MarpolLog" ExportOnlyData="true">
                    <Pdf PageHeight="210mm" PageWidth="297mm" DefaultFontFamily="Arial Unicode MS" PageTopMargin="45mm"
                        BorderStyle="Medium" BorderColor="#666666">
                    </Pdf>
                </ExportSettings>
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ShowFooter="true" >
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <HeaderStyle Width="102px" />
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Estimated Amount <br/> Discharged" HeaderStyle-Width="40%" Name="Discharged" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Estimated Amount Incinerated" HeaderStyle-Width="40%" Name="Incinerated" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText=""  HeaderStyle-Width="15PX">

                                    <ItemStyle HorizontalAlign="Center" />

                                    <ItemTemplate>

                                        <telerik:RadLabel ID="radisbackdatedentry" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDISMISSEDENTRY")%>'>
                                        </telerik:RadLabel>
                                        <asp:LinkButton runat="server" ID="imgFlag" Enabled="false" Width="15px" Height="15px" Visible="false" >
                                         <span class="icon" id="imgFlagcolor"  ><i class="fas fa-star-yellow"></i></span>      
                                            </asp:LinkButton>
                                         
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <span class="icon" style="vertical-align:middle"><i class="fas fa-star-yellow"></i> </span> <b style="vertical-align:middle">* Missed Entry</b>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="35px" HeaderText="Date/Time" AllowSorting="true" HeaderStyle-HorizontalAlign="Left" HeaderStyle-VerticalAlign="Bottom" ShowSortIcon="true" SortExpression="FLDDATE" UniqueName="date">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                            <ItemTemplate>
                                <%--<telerik:RadLabel ID="RadLabel1" runat="server" Visible="True" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDDATE",  "{0:dd-MMM-yyyy}") %>'></telerik:RadLabel>--%>
                                <telerik:RadLabel ID="lblDate" runat="server" Visible="True" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDDATE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblLogBookId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOGBOOKID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDtKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblLogId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOGID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblLogName" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOGNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDURL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Position / Port or Vessel Name" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Bottom" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDPORTPOSITION" UniqueName="position">
                            <ItemStyle Wrap="true" HorizontalAlign="left" VerticalAlign="Top"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPosition" runat="server" Visible="True"></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Category" HeaderStyle-Width="30px" AllowSorting="true" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Bottom" ShowSortIcon="true" SortExpression="FLDCATEGORY" UniqueName="category">
                            <ItemStyle Wrap="true" HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCategory" runat="server" Visible="True"></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Into Sea (m3)" HeaderStyle-Width="50px" AllowSorting="true" ColumnGroupName="Discharged" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Bottom" ShowSortIcon="true" SortExpression="FLDDISCHARGEINTOSEA" UniqueName="intosea">
                            <ItemStyle Wrap="true" HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblintosea" runat="server" Visible="True"></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="To Reception Facility (m3)" HeaderStyle-Width="50px" AllowSorting="true" ColumnGroupName="Discharged" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Bottom" ShowSortIcon="true" SortExpression="FLDDISCHARGEINTORF" UniqueName="facility">
                            <ItemStyle Wrap="true" HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblfacility" runat="server" Visible="True"></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        
                        <telerik:GridTemplateColumn HeaderText="Remarks" HeaderStyle-Width="50px" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDREMARKS" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Bottom" UniqueName="remarks">
                            <ItemStyle Wrap="true" HorizontalAlign="left" VerticalAlign="Top"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblremarks" runat="server" Visible="True"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDeleted" runat="server" Visible="false" ></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Signature" HeaderStyle-Width="50px" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDSIGNATURE" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Bottom" UniqueName="sign">
                            <ItemStyle Wrap="true" HorizontalAlign="left" VerticalAlign="Top"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblsign" runat="server" Visible="True" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNATURE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn> 

                        <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="35px" AllowSorting="true" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Bottom" ShowSortIcon="true" SortExpression="FLDSTATUS" UniqueName="status">
                            <ItemStyle Wrap="true" HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Visible="True" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
        
                         <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="35px" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Bottom" UniqueName="action">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                 <asp:LinkButton runat="server" AlternateText="Delete" Visible="false"
                                    CommandName="DELETE" ID="cmdDelete"
                                    ToolTip="Delete" Width="20PX" Height="20PX">
                                        <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Edit" Visible="false"
                                    CommandName="VIEW" ID="CmdView"
                                    ToolTip="Edit Log" Width="20PX" Height="20PX">
                                        <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Attachment" Visible="false"
                                    CommandName="ATTACHMENT" ID="cmdAttachment"
                                    ToolTip="Attachment" Width="20PX" Height="20PX">
                                        <span class="icon"><i runat="server" id="attachmentIcon" class="fas fa-paperclip-na"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Signature" Visible="false"
                                    CommandName="2NDENGINEERSIGNATURE" ID="cmd2esignature"
                                    ToolTip="2E Signature" Width="20PX" Height="20PX">
                                       <span class="icon"> <i class="fas fa-file-signature"></i></span>
                                </asp:LinkButton>


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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="3" EnableColumnClientFreeze="true" ScrollHeight="350px" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" ResizeGridOnColumnResize="false" />
            </ClientSettings>
            </telerik:RadGrid>

            <table>
                <tr>
                    <td>
                        <b>Exceptional Discharge or Loss of Garbage under Regulation 7 (Exceptions)</b>
                    </td>
                </tr>
            </table>

             <telerik:RadGrid RenderMode="Lightweight" ID="gvElogExceptional" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" EnableViewState="true"
                CellSpacing="0" GridLines="None" Height="70%" OnGridExporting="gvElogExceptional_GridExporting"
                OnNeedDataSource="gvElogExceptional_NeedDataSource" 
                OnItemCommand="gvElogExceptional_ItemCommand" 
                OnItemDataBound="gvElogExceptional_ItemDataBound"
                OnPreRender="gvElogExceptional_PreRender"
                GroupingEnabled="false" EnableHeaderContextMenu="true">
                <ExportSettings IgnorePaging="true" OpenInNewWindow="true" FileName="MarpolLog" ExportOnlyData="true">
                    <Pdf PageHeight="210mm" PageWidth="297mm" DefaultFontFamily="Arial Unicode MS" PageTopMargin="45mm"
                        BorderStyle="Medium" BorderColor="#666666">
                    </Pdf>
                </ExportSettings>
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ShowFooter="true">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <HeaderStyle Width="102px" />
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Estimated Amount Discharged" HeaderStyle-Width="40%" Name="Discharged" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText=""  HeaderStyle-Width="15PX">

                                    <ItemStyle HorizontalAlign="Center" />

                                    <ItemTemplate>

                                        <telerik:RadLabel ID="radisbackdatedentry" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDISMISSEDENTRY")%>'>
                                        </telerik:RadLabel>
                                        <asp:LinkButton runat="server" ID="imgFlag" Enabled="false" Width="15px" Height="15px" Visible="false" >
                                         <span class="icon" id="imgFlagcolor"  ><i class="fas fa-star-yellow"></i></span>      
                                            </asp:LinkButton>
                                         
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <span class="icon" style="vertical-align:middle"><i class="fas fa-star-yellow"></i> </span> <b style="vertical-align:middle">* Missed Entry</b>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="30px" HeaderText="Date/Time" AllowSorting="true" HeaderStyle-HorizontalAlign="Left" HeaderStyle-VerticalAlign="Bottom" ShowSortIcon="true" SortExpression="FLDDATE" UniqueName="date">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                            <ItemTemplate>
                                <%--<telerik:RadLabel ID="RadLabel1" runat="server" Visible="True" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDDATE",  "{0:dd-MMM-yyyy}") %>'></telerik:RadLabel>--%>
                                <telerik:RadLabel ID="lblDate" runat="server" Visible="True" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDDATE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblLogBookId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOGBOOKID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDtKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblLogId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOGID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblLogName" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOGNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDURL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Position / Port or Vessel Name" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Bottom" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDPORTPOSITION" UniqueName="position">
                            <ItemStyle Wrap="true" HorizontalAlign="left" VerticalAlign="Top"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPosition" runat="server" Visible="True"></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Category" HeaderStyle-Width="30px" AllowSorting="true" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Bottom" ShowSortIcon="true" SortExpression="FLDCATEGORY" UniqueName="category">
                            <ItemStyle Wrap="true" HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCategory" runat="server" Visible="True"></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="(m3)" HeaderStyle-Width="50px" AllowSorting="true" ColumnGroupName="Discharged" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Bottom" ShowSortIcon="true" SortExpression="FLDDISCHARGEINTORF" UniqueName="facility">
                            <ItemStyle Wrap="true" HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDischarge" runat="server" Visible="True"></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Remarks on the reason for the discharge or loss and general remarks (e.g. reasonable precautions taken to prevent or minimize such discharge or accidental loss and general remarks)" HeaderStyle-Width="110px" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDREMARKS" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Bottom" UniqueName="remarks">
                            <ItemStyle Wrap="true" HorizontalAlign="left" VerticalAlign="Top"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblremarks" runat="server" Visible="True"></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Signature" HeaderStyle-Width="40px" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDSIGNATURE" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Bottom" UniqueName="sign">
                            <ItemStyle Wrap="true" HorizontalAlign="left" VerticalAlign="Top"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblsign" runat="server" Visible="True" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNATURE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn> 

                        <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="35px" AllowSorting="true" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Bottom" ShowSortIcon="true" SortExpression="FLDSTATUS" UniqueName="status">
                            <ItemStyle Wrap="true" HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Visible="True" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
        
                         <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="35px" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Bottom" UniqueName="action">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                 <asp:LinkButton runat="server" AlternateText="Delete" Visible="false"
                                    CommandName="DELETE" ID="cmdDelete"
                                    ToolTip="Delete" Width="20PX" Height="20PX">
                                        <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Edit" Visible="false"
                                    CommandName="VIEW" ID="CmdView"
                                    ToolTip="Edit Log" Width="20PX" Height="20PX">
                                        <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Attachment" Visible="false"
                                    CommandName="ATTACHMENT" ID="cmdAttachment"
                                    ToolTip="Attachment" Width="20PX" Height="20PX">
                                        <span class="icon"><i runat="server" id="attachmentIcon" class="fas fa-paperclip-na"></i></span>
                                </asp:LinkButton>

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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="3" EnableColumnClientFreeze="true" ScrollHeight="350px" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" ResizeGridOnColumnResize="false" />
            </ClientSettings>
            </telerik:RadGrid>

            <table>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" ID="txtMasterSign" CssClass="signature"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" ID="lblMasterSign" Text="Signature of Master"></telerik:RadLabel>
                    </td>
                </tr>
            </table>

        </telerik:radajaxpanel>
    </form>
</body>
</html>