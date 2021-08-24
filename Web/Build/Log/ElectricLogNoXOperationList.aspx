<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogNoXOperationList.aspx.cs" Inherits="Log_ElectricLogNoXOperationList" %>


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
    <title>Entries in NoX Operation List</title>
    <telerik:radcodeblock id="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
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
    </style>
</head>
<body>
    <form id="frmgvCounterUpdate" runat="server" submitdisabledcontrols="true">
        <telerik:radscriptmanager runat="server" id="RadScriptManager1" />
        <telerik:radskinmanager id="RadSkinManager1" runat="server" />

        <telerik:radwindowmanager rendermode="Lightweight" id="RadWindowManager1" runat="server" enableshadow="true">
        </telerik:radwindowmanager>

        <telerik:radajaxpanel id="RadAjaxPanel1" runat="server" height="95%">

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="displayNone" />
            <br />
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
                        <telerik:RadDropDownList runat="server" AutoPostBack="true" ID="ddlStatus" OnItemSelected="ddl_TextChanged" ></telerik:RadDropDownList>
                    </td>
                </tr>
            </table>
            <br />
            <div style="width:100%; text-align:right" >
                <asp:HyperLink ID="lnkfilename" Target="_blank" Text="View" Visible="false" runat="server" ImageUrl="<%$ PhoenixTheme:images/54.png %>" Width="14px" 
                    Height="14px" ToolTip="User Guide">
                </asp:HyperLink>
            </div>

            <eluc:TabStrip ID="MenugvCounterUpdate" runat="server" OnTabStripCommand="gvCounterUpdate_TabStripCommand"></eluc:TabStrip>
       

            <telerik:RadGrid RenderMode="Lightweight" ID="gvElogTransaction" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" EnableViewState="true"
                CellSpacing="0" GridLines="None" Height="80%" Width="100%" OnGridExporting="gvElogTransaction_GridExporting"
                OnNeedDataSource="gvElogTransaction_NeedDataSource" 
                OnItemCommand="gvElogTransaction_ItemCommand" 
                OnItemDataBound="gvElogTransaction_ItemDataBound"
                OnPreRender="gvElogTransaction_PreRender"
                ShowFooter="true"
                GroupingEnabled="false" EnableHeaderContextMenu="true">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" >
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <HeaderStyle Width="102px" />

                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Disesl Engine" HeaderStyle-Width="150px" Name="diseslengine" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Main Engine 1" HeaderStyle-Width="150px" ParentGroupName="diseslengine" Name="mainengine1" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Main Engine 2" HeaderStyle-Width="150px" ParentGroupName="diseslengine" Name="mainengine2" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="AE 1" HeaderStyle-Width="150px" Name="ae1" ParentGroupName="diseslengine" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="AE 2" HeaderStyle-Width="150px" Name="ae2" ParentGroupName="diseslengine" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="AE 3" HeaderStyle-Width="150px" Name="ae3" ParentGroupName="diseslengine" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="AE 4" HeaderStyle-Width="150px" Name="ae4" ParentGroupName="diseslengine" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Harbour Gen." HeaderStyle-Width="150px" ParentGroupName="diseslengine" Name="harbourgen" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>

                         <telerik:GridTemplateColumn HeaderStyle-Width="30px" HeaderStyle-HorizontalAlign="Center" AllowSorting="true" ShowSortIcon="true" UniqueName="status">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="radisbackdatedentry" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDISMISSEDENTRY")%>'>
                                </telerik:RadLabel>
                                <asp:LinkButton runat="server" ID="imgFlag" Enabled="false" Width="15px" Height="15px" Visible="false">
                                         <span class="icon" id="imgFlagcolor"  ><i class="fas fa-star-yellow"></i></span>      
                                </asp:LinkButton>
                            </ItemTemplate>
                            <FooterTemplate>
                                 <span class="icon" style="vertical-align:middle"><i class="fas fa-star-yellow"></i> </span> <b style="vertical-align:middle">* Missed Entry</b>
                             </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderStyle-Width="30px" HeaderStyle-HorizontalAlign="Center" HeaderText="Id" AllowSorting="true" ShowSortIcon="true" UniqueName="id">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblId" runat="server"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDENTRYNO") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblLogBookId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOGBOOKID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDtKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderStyle-Width="80px" HeaderText="Status Change" HeaderStyle-HorizontalAlign="Center" AllowSorting="true" ShowSortIcon="true" UniqueName="statuschange">
                            <ItemStyle Wrap="true" HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblstatus" runat="server" Visible="True" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSCHANGE") %>' ></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Date / Time" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDCODE" UniqueName="date">
                            <ItemStyle Wrap="true" HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDate" runat="server" Visible="True" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Ship Posistion" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" UniqueName="shipposistion">
                            <ItemStyle Wrap="true" VerticalAlign="Top" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td>
                                             <telerik:RadLabel ID="lblShipPosistion" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHIPPOSITION") %>' ></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                         <telerik:GridTemplateColumn HeaderText="Tier" HeaderStyle-Width="70px" HeaderStyle-HorizontalAlign="Center" Display="false" UniqueName="meTier1" ColumnGroupName="mainengine1">
                            <ItemStyle Wrap="true" VerticalAlign="Top" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td>
                                             <telerik:RadLabel ID="lblME1Tier" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAINENGINE1TIER") %>' ></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                         <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center" Display="false" UniqueName="meStatus1" ColumnGroupName="mainengine1">
                            <ItemStyle Wrap="true" VerticalAlign="Top" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td>
                                             <telerik:RadLabel ID="lblME1Status" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAINENGINE1STATUS") %>' ></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Tier" HeaderStyle-Width="70px" HeaderStyle-HorizontalAlign="Center" Display="false" UniqueName="meTier2" ColumnGroupName="mainengine2">
                            <ItemStyle Wrap="true" VerticalAlign="Top" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td>
                                             <telerik:RadLabel ID="lblME2Tier" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAINENGINE2TIER") %>'  ></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                         <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center" Display="false" UniqueName="meStatus2" ColumnGroupName="mainengine2">
                            <ItemStyle Wrap="true" VerticalAlign="Top" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td>
                                             <telerik:RadLabel ID="lblME2Status" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAINENGINE1STATUS") %>'  ></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText="Tier" HeaderStyle-Width="70px" HeaderStyle-HorizontalAlign="Center" Display="false" UniqueName="aeTier1" ColumnGroupName="ae1">
                            <ItemStyle Wrap="true" VerticalAlign="Top" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td>
                                             <telerik:RadLabel ID="lblAE1Tier" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAE1TIER") %>' ></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                         <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center" Display="false" UniqueName="aeStatus1" ColumnGroupName="ae1">
                            <ItemStyle Wrap="true" VerticalAlign="Top" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td>
                                             <telerik:RadLabel ID="lblAE1Status" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAE1STATUS") %>' ></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Tier" HeaderStyle-Width="70px" HeaderStyle-HorizontalAlign="Center" Display="false" UniqueName="aeTier2" ColumnGroupName="ae2">
                            <ItemStyle Wrap="true" VerticalAlign="Top" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td>
                                             <telerik:RadLabel ID="lblAE2Tier" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAE1TIER") %>'  ></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                         <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center" Display="false" UniqueName="aeStatus2" ColumnGroupName="ae2">
                            <ItemStyle Wrap="true" VerticalAlign="Top" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td>
                                             <telerik:RadLabel ID="lblAE2Status" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAE2STATUS") %>' ></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                          <telerik:GridTemplateColumn HeaderText="Tier" HeaderStyle-Width="70px" HeaderStyle-HorizontalAlign="Center" Display="false" UniqueName="aeTier3" ColumnGroupName="ae3">
                            <ItemStyle Wrap="true" VerticalAlign="Top" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td>
                                             <telerik:RadLabel ID="lblAE3Tier" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAE3TIER") %>'  ></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                         <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center" Display="false" UniqueName="aeStatus3" ColumnGroupName="ae3">
                            <ItemStyle Wrap="true" VerticalAlign="Top" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td>
                                             <telerik:RadLabel ID="lblAE3Status" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAE3STATUS") %>' ></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                         <telerik:GridTemplateColumn HeaderText="Tier" HeaderStyle-Width="70px" HeaderStyle-HorizontalAlign="Center" Display="false" UniqueName="aeTier4" ColumnGroupName="ae4">
                            <ItemStyle Wrap="true" VerticalAlign="Top" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td>
                                             <telerik:RadLabel ID="lblAE4Tier" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAE4TIER") %>' ></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                         <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center" Display="false" UniqueName="aeStatus4" ColumnGroupName="ae4">
                            <ItemStyle Wrap="true" VerticalAlign="Top" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td>
                                             <telerik:RadLabel ID="lblAE4Status" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAE4STATUS") %>' ></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Tier" HeaderStyle-Width="70px" HeaderStyle-HorizontalAlign="Center" Display="false" UniqueName="harbourgenTier1" ColumnGroupName="harbourgen">
                            <ItemStyle Wrap="true" VerticalAlign="Top" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td>
                                             <telerik:RadLabel ID="lblHBTier" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARBOURGENTIER") %>' ></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                         <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center" Display="false" UniqueName="harbourgenStatus1" ColumnGroupName="harbourgen">
                            <ItemStyle Wrap="true" VerticalAlign="Top" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td>
                                             <telerik:RadLabel ID="lblHBStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARBOURGENSTATUS") %>' ></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="80px" AllowSorting="true" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Bottom" ShowSortIcon="true" SortExpression="FLDSTATUS" UniqueName="status">
                            <ItemStyle Wrap="true" HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNoxStatus" runat="server" Visible="True" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                         <telerik:GridTemplateColumn HeaderText="Officer in Charge Signature*" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" UniqueName="officerIncharge">
                            <ItemStyle Wrap="true" VerticalAlign="Top" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td>
                                             <telerik:RadLabel ID="lblOfficerInchargeSign" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCHIEFOFFICERNAME") %>' ></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                         
        
                         <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="75px" HeaderStyle-HorizontalAlign="Center" UniqueName="action">
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
                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true"  />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" ResizeGridOnColumnResize="false" />
            </ClientSettings>
            </telerik:RadGrid>
            <table>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" ID="txtChiefEnggSign" CssClass="signature"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" ID="lblChiefEnggSign" Text="Signature of CE"></telerik:RadLabel>
                    </td>
                </tr>
            </table>

        </telerik:radajaxpanel>
    </form>
</body>
</html>