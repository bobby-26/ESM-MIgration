<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectronicLogAnnexVIODS.aspx.cs" Inherits="Log_ElectronicLogAnnexVIODS" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <style>
            .user-info {
                float: right;
            }
        </style>

        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvODS.ClientID %>"), null, 110);
               }, 200);
           }
           window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
               Resize();
           }

           function GridResize(sender, args) {
               var tab = args.get_tab();
               var selectedindex = tab.get_index();
               if (selectedindex == 0) {
                   setTimeout(function () {
                       TelerikGridResize($find("<%= gvODS.ClientID %>"), null, 110);
               }, 200);
           }
           if (selectedindex == 1) {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvODSM.ClientID %>"), null, 110);
                }, 200);
            }
            else {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvODSSD.ClientID %>"), null, 110);
               }, 200);
           }


       }

        </script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <table class="user-info">
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" ID="lblUsername"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFromDate" runat="server" Text="From"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtFromDate" runat="server" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblToDate" runat="server" Text="To"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtToDate" runat="server" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>

                        <telerik:RadDropDownList runat="server" AutoPostBack="true" ID="ddlStatus" OnItemSelected="ddl_TextChanged"></telerik:RadDropDownList>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuMain" runat="server" OnTabStripCommand="MenuMain_TabStripCommand"></eluc:TabStrip>
            <telerik:RadTabStrip runat="server" ID="RadTabStrip1" MultiPageID="RadMultiPage1" SelectedIndex="0" OnClientTabSelecting="GridResize">
                <Tabs>
                    <telerik:RadTab Text="ODS Summary" Width="200px"></telerik:RadTab>
                    <telerik:RadTab Text="ODS Maintenance" Width="200px"></telerik:RadTab>
                    <telerik:RadTab Text="ODS Supply & Discharge" Width="200px"></telerik:RadTab>
                </Tabs>
            </telerik:RadTabStrip>

            <telerik:RadMultiPage runat="server" ID="RadMultiPage1" SelectedIndex="0">
                <telerik:RadPageView runat="server" ID="RadPageView1">
                    <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="gvODS" AutoGenerateColumns="false" AllowCustomPaging="true" 
                        AllowPaging="true" OnNeedDataSource="gvODS_NeedDataSource" OnItemCommand="gvODS_ItemCommand"
                        OnItemDataBound="gvODS_ItemDataBound">
                        <MasterTableView EditMode="InPlace" DataKeyNames="FLDRECORDID" AutoGenerateColumns="false" EnableViewState="true"
                            EnableColumnsViewState="false" TableLayout="Fixed" ShowFooter="true" HeaderStyle-Font-Bold="true"
                            ShowHeadersWhenNoRecords="true" InsertItemPageIndexAction="ShowItemOnCurrentPage" CommandItemDisplay="None">
                            <ColumnGroups>
                                <telerik:GridColumnGroup Name="ods" HeaderText="Ozone Depleting Substances Record" HeaderStyle-HorizontalAlign="Left"></telerik:GridColumnGroup>

                            </ColumnGroups>
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
                                <telerik:GridTemplateColumn HeaderText="" ColumnGroupName="ods" HeaderStyle-Width="6%">

                                    <ItemStyle HorizontalAlign="Center" />

                                    <ItemTemplate>

                                        <telerik:RadLabel ID="radrid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDRECORDID")%>'>
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="radstatus" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSTATUS")%>'>
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="radisdeleted" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDISDELETED")%>'>
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="raddtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDTKEY")%>'>
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="radisbackdatedentry" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDISBACKDATEDENTRY")%>'>
                                        </telerik:RadLabel>
                                        <asp:LinkButton runat="server" ID="imgFlag" Enabled="false" Width="15px" Height="15px" Visible="false" >
                                         <span class="icon" id="imgFlagcolor"  ><i class="fas fa-star-yellow"></i></span>      
                                            </asp:LinkButton>
                                         <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                        <asp:LinkButton runat="server" ID="btngreenlock" ToolTip="Entry UnLocked" Visible="false" Enabled="false">
                                            <span class="icon"><img src="../css/Theme1/images/unlock_3.png" /></span>
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" ID="btnredlock" ToolTip="Entry Locked" Visible="false" CommandName="UnLock">
                                            <span class="icon"><img src="../css/Theme1/images/lock_2.png" /></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <span class="icon" style="vertical-align:middle"><i class="fas fa-star-yellow"></i> </span> <b style="vertical-align:middle">* Missed Entry</b>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Date" ColumnGroupName="ods" HeaderStyle-Width="10%">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />

                                    <ItemTemplate>

                                        <telerik:RadLabel ID="raddate" runat="server" Visible="true">
                                        </telerik:RadLabel>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Name of Equipment" ColumnGroupName="ods" HeaderStyle-Width="13%">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />

                                    <ItemTemplate>

                                        <telerik:RadLabel ID="radequipment" runat="server" Visible="true">
                                        </telerik:RadLabel>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Maker" ColumnGroupName="ods" HeaderStyle-Width="6%">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />

                                    <ItemTemplate>

                                        <telerik:RadLabel ID="radmaker" runat="server" Visible="true">
                                        </telerik:RadLabel>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Model" ColumnGroupName="ods" HeaderStyle-Width="6%">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />

                                    <ItemTemplate>

                                        <telerik:RadLabel ID="radmodel" runat="server" Visible="true">
                                        </telerik:RadLabel>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Location" ColumnGroupName="ods" HeaderStyle-Width="6%">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />

                                    <ItemTemplate>

                                        <telerik:RadLabel ID="radlocation" runat="server" Visible="true">
                                        </telerik:RadLabel>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Name of the Ozone Depleting Substance" ColumnGroupName="ods" HeaderStyle-Width="17%">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />

                                    <ItemTemplate>

                                        <telerik:RadLabel ID="radodsname" runat="server" Visible="true">
                                        </telerik:RadLabel>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Mass(Kg)" ColumnGroupName="ods" HeaderStyle-Width="5%">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />

                                    <ItemTemplate>

                                        <telerik:RadLabel ID="radmass" runat="server" Visible="true">
                                        </telerik:RadLabel>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Officer in Charge Sign" ColumnGroupName="ods" HeaderStyle-Width="12%">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />

                                    <ItemTemplate>

                                        <telerik:RadLabel ID="radoicsign" runat="server" Visible="false" Text="OIC-Signed">
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="radoicsignid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOICSIGN")%>'>
                                        </telerik:RadLabel>
                                        <asp:LinkButton runat="server" Text="Sign" Visible="false" ID="radoicsignlink" CommandName="sign"></asp:LinkButton>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Chief Engineer Sign" ColumnGroupName="ods" HeaderStyle-Width="12%">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />

                                    <ItemTemplate>

                                        <telerik:RadLabel ID="radcesign" runat="server" Visible="false" Text="Verified">
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="radcereverfiedsign" runat="server" Visible="false" Text=" Re-Verified">
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="radcesignid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCESIGN")%>'>
                                        </telerik:RadLabel>
                                        <asp:LinkButton runat="server" Text="Verify" Visible="false" ID="radcesignveifylink" CommandName="CHIEFENGGSIGN"></asp:LinkButton>
                                        <asp:LinkButton runat="server" Text="Re-Verify" Visible="false" ID="radcesignreverifylink" CommandName="CHIEFENGGSIGN"></asp:LinkButton>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Action" ColumnGroupName="ods" HeaderStyle-Width="8%">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />

                                    <ItemTemplate>

                                        <asp:LinkButton runat="server" ID="btnedit" ToolTip="Edit" CommandName="AMEND">
                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" ID="btndelete" ToolTip="Delete" CommandName="Delete">
                                            <span class="icon"><i class="fas fa-times-circle"></i></span>
                                        </asp:LinkButton>
                                        <asp:ImageButton ID="btnattachments" runat="server" AlternateText="Attachment" CommandName="ATTACHMENT" ToolTip="Attachment" CommandArgument='<%# Container.DataSetIndex %>' ImageUrl='Session["images"]/no-attachment.png'></asp:ImageButton>

                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>

                            </Columns>

                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria" 
                                PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox"
                                AlwaysVisible="true" />

                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder" >
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>


                </telerik:RadPageView>
                <telerik:RadPageView runat="server" ID="RadPageView2">
                    <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="gvODSM" AutoGenerateColumns="false" AllowCustomPaging="true"
                        AllowPaging="true" OnNeedDataSource="gvODSM_NeedDataSource" OnItemCommand="gvODSM_ItemCommand"
                        OnItemDataBound="gvODSM_ItemDataBound">
                        <MasterTableView EditMode="InPlace" DataKeyNames="FLDRECORDID" AutoGenerateColumns="false" EnableViewState="true"
                            EnableColumnsViewState="false" TableLayout="Fixed" ShowFooter="true" HeaderStyle-Font-Bold="true"
                            ShowHeadersWhenNoRecords="true" InsertItemPageIndexAction="ShowItemOnCurrentPage" CommandItemDisplay="None">
                            <ColumnGroups>
                                <telerik:GridColumnGroup Name="ods" HeaderText="Ozone Depleting Substances - Maintenance Record" HeaderStyle-HorizontalAlign="Left"></telerik:GridColumnGroup>

                            </ColumnGroups>
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
                                <telerik:GridTemplateColumn HeaderText="" ColumnGroupName="ods" HeaderStyle-Width="6%">

                                    <ItemStyle HorizontalAlign="Center" />

                                    <ItemTemplate>

                                        <telerik:RadLabel ID="radmrid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDRECORDID")%>'>
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="radmstatus" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSTATUS")%>'>
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="mraddtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDTKEY")%>'>
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="radmisdeleted" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDISDELETED")%>'>
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="radmisbackdatedentry" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDISBACKDATEDENTRY")%>'>
                                        </telerik:RadLabel>
                                        <asp:LinkButton runat="server" ID="imgmFlag" Enabled="false" Width="15px" Height="15px" Visible="false" >
                                         <span class="icon" id="imgmFlagcolor"  ><i class="fas fa-star-yellow"></i></span>      
                                            </asp:LinkButton>
                                         <img id="Imgm1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                        <asp:LinkButton runat="server" ID="btnmgreenlock" ToolTip="Entry UnLocked" Visible="false" Enabled="false">
                                            <span class="icon"><img src="../css/Theme1/images/unlock_3.png" /></span>
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" ID="btnmredlock" ToolTip="Entry Locked" Visible="false" CommandName="UnLock">
                                            <span class="icon"><img src="../css/Theme1/images/lock_2.png" /></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                     <FooterTemplate>
                                        <span class="icon" style="vertical-align:middle"><i class="fas fa-star-yellow"></i> </span> <b style="vertical-align:middle">* Missed Entry</b>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Date" ColumnGroupName="ods" HeaderStyle-Width="7%">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />

                                    <ItemTemplate>

                                        <telerik:RadLabel ID="radmdate" runat="server" Visible="true">
                                        </telerik:RadLabel>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Location Port/At Sea" ColumnGroupName="ods" HeaderStyle-Width="10%">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />

                                    <ItemTemplate>

                                        <telerik:RadLabel ID="radmlocation" runat="server" Visible="true">
                                        </telerik:RadLabel>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Name of Gas" ColumnGroupName="ods" HeaderStyle-Width="9%">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />

                                    <ItemTemplate>

                                        <telerik:RadLabel ID="radmgas" runat="server" Visible="true">
                                        </telerik:RadLabel>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Quantity used/ Equipment" ColumnGroupName="ods" HeaderStyle-Width="9%">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />

                                    <ItemTemplate>

                                        <telerik:RadLabel ID="radmquantity" runat="server" Visible="true">
                                        </telerik:RadLabel>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Reason for charging the system/equipment" ColumnGroupName="ods" HeaderStyle-Width="17%">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />

                                    <ItemTemplate>

                                        <telerik:RadLabel ID="radmreason" runat="server" Visible="true">
                                        </telerik:RadLabel>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Briefly describe if any maintenance carried out on the system/ equipment" ColumnGroupName="ods" HeaderStyle-Width="17%">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />

                                    <ItemTemplate>

                                        <telerik:RadLabel ID="radmexplain" runat="server" Visible="true">
                                        </telerik:RadLabel>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Officer in Charge Sign" ColumnGroupName="ods" HeaderStyle-Width="12%">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />

                                    <ItemTemplate>

                                        <telerik:RadLabel ID="radmoicsign" runat="server" Visible="false" Text="OIC-Signed">
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="radmoicsignid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOICSIGN")%>'>
                                        </telerik:RadLabel>
                                        <asp:LinkButton runat="server" Text="Sign" Visible="false" ID="radmoicsignlink" CommandName="sign"></asp:LinkButton>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Chief Engineer Sign" ColumnGroupName="ods" HeaderStyle-Width="12%">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />

                                    <ItemTemplate>

                                        <telerik:RadLabel ID="radmcesign" runat="server" Visible="false" Text="Verified">
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="radmcereverfiedsign" runat="server" Visible="false" Text=" Re-Verified">
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="radmcesignid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCESIGN")%>'>
                                        </telerik:RadLabel>
                                        <asp:LinkButton runat="server" Text="Verify" Visible="false" ID="radmcesignveifylink" CommandName="CHIEFENGGSIGN"></asp:LinkButton>
                                        <asp:LinkButton runat="server" Text="Re-Verify" Visible="false" ID="radmcesignreverifylink" CommandName="CHIEFENGGSIGN"></asp:LinkButton>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Action" ColumnGroupName="ods" HeaderStyle-Width="8%">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />

                                    <ItemTemplate>

                                        <asp:LinkButton runat="server" ID="btnmedit" ToolTip="Edit" CommandName="AMEND">
                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" ID="btnmdelete" ToolTip="Delete" CommandName="Delete">
                                            <span class="icon"><i class="fas fa-times-circle"></i></span>
                                        </asp:LinkButton>
                                        <asp:ImageButton ID="btnmattachments" runat="server" AlternateText="Attachment" CommandName="ATTACHMENT" ToolTip="Attachment" CommandArgument='<%# Container.DataSetIndex %>' ImageUrl='Session["images"]/no-attachment.png'></asp:ImageButton>



                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>

                            </Columns>

                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox"
                                AlwaysVisible="true" />

                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder" EnablePostBackOnRowClick="true">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </telerik:RadPageView>
                <telerik:RadPageView runat="server" ID="RadPageView3">
                    <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="gvODSSD" AutoGenerateColumns="false"
                        AllowPaging="true" OnNeedDataSource="gvODSSD_NeedDataSource" OnItemCommand="gvODSSD_ItemCommand" AllowCustomPaging="true"
                        OnItemDataBound="gvODSSD_ItemDataBound">
                        <MasterTableView EditMode="InPlace" DataKeyNames="FLDRECORDID" AutoGenerateColumns="false" EnableViewState="true"
                            EnableColumnsViewState="false" TableLayout="Fixed" ShowFooter="true" HeaderStyle-Font-Bold="true"
                            ShowHeadersWhenNoRecords="true" InsertItemPageIndexAction="ShowItemOnCurrentPage" CommandItemDisplay="None">
                            <ColumnGroups>
                                <telerik:GridColumnGroup Name="ods" HeaderText="Ozone Depleting Substances (ODS) Record - Supply & Discharge" HeaderStyle-HorizontalAlign="Left"></telerik:GridColumnGroup>
                                <telerik:GridColumnGroup Name="dis" ParentGroupName="ods" HeaderText="Discharge of ODS to the Atmosphere (Kgs)" HeaderStyle-HorizontalAlign="Left"></telerik:GridColumnGroup>

                            </ColumnGroups>
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
                                <telerik:GridTemplateColumn HeaderText="" ColumnGroupName="ods" HeaderStyle-Width="6%">

                                    <ItemStyle HorizontalAlign="Center" />

                                    <ItemTemplate>

                                        <telerik:RadLabel ID="radssdrid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDRECORDID")%>'>
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="radssdstatus" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSTATUS")%>'>
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="radsisdeleted" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDISDELETED")%>'>
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="sraddtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDTKEY")%>'>
                                        </telerik:RadLabel>
                                         <telerik:RadLabel ID="radsdisbackdatedentry" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDISBACKDATEDENTRY")%>'>
                                        </telerik:RadLabel>
                                        <asp:LinkButton runat="server" ID="imgsdFlag" Enabled="false" Width="15px" Height="15px" Visible="false" >
                                         <span class="icon" id="imgsdFlagcolor"  ><i class="fas fa-star-yellow"></i></span>      
                                            </asp:LinkButton>
                                         <img id="Imgsd1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                        <asp:LinkButton runat="server" ID="btnssdgreenlock" ToolTip="Entry UnLocked" Visible="false" Enabled="false">
                                            <span class="icon"><img src="../css/Theme1/images/unlock_3.png" /></span>
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" ID="btnssdredlock" ToolTip="Entry Locked" Visible="false" CommandName="UnLock">
                                            <span class="icon"><img src="../css/Theme1/images/lock_2.png" /></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <span class="icon" style="vertical-align:middle"><i class="fas fa-star-yellow"></i> </span> <b style="vertical-align:middle">* Missed Entry</b>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Date" ColumnGroupName="ods" HeaderStyle-Width="10%">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />

                                    <ItemTemplate>

                                        <telerik:RadLabel ID="radssddate" runat="server" Visible="true">
                                        </telerik:RadLabel>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="System Type" ColumnGroupName="ods" HeaderStyle-Width="10%">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />

                                    <ItemTemplate>

                                        <telerik:RadLabel ID="radsystemtype" runat="server" Visible="true">
                                        </telerik:RadLabel>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Supply to Ship (Kgs)" ColumnGroupName="ods" HeaderStyle-Width="10%">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />

                                    <ItemTemplate>

                                        <telerik:RadLabel ID="radsupply" runat="server" Visible="true">
                                        </telerik:RadLabel>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Deliberate" ColumnGroupName="dis" HeaderStyle-Width="14%">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />

                                    <ItemTemplate>

                                        <telerik:RadLabel ID="raddeli" runat="server" Visible="true">
                                        </telerik:RadLabel>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Non-Deliberate" ColumnGroupName="dis" HeaderStyle-Width="14%">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />

                                    <ItemTemplate>

                                        <telerik:RadLabel ID="radnd" runat="server" Visible="true">
                                        </telerik:RadLabel>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Discharge to Shore Reception facilities (Kgs)" ColumnGroupName="ods" HeaderStyle-Width="10%">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />

                                    <ItemTemplate>

                                        <telerik:RadLabel ID="raddobs" runat="server" Visible="true">
                                        </telerik:RadLabel>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Remarks" ColumnGroupName="ods" HeaderStyle-Width="15%">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />

                                    <ItemTemplate>

                                        <telerik:RadLabel ID="radsdremarks" runat="server" Visible="true">
                                        </telerik:RadLabel>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Officer in Charge Sign" ColumnGroupName="ods" HeaderStyle-Width="12%">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />

                                    <ItemTemplate>

                                        <telerik:RadLabel ID="radsdoicsign" runat="server" Visible="false" Text="OIC-Signed">
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="radsdoicsignid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOICSIGN")%>'>
                                        </telerik:RadLabel>
                                        <asp:LinkButton runat="server" Text="Sign" Visible="false" ID="radsdoicsignlink" CommandName="sign"></asp:LinkButton>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Chief Engineer Sign" ColumnGroupName="ods" HeaderStyle-Width="12%">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />

                                    <ItemTemplate>

                                        <telerik:RadLabel ID="radsdcesign" runat="server" Visible="false" Text="Verified">
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="radsdcereverfiedsign" runat="server" Visible="false" Text=" Re-Verified">
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="radsdcesignid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCESIGN")%>'>
                                        </telerik:RadLabel>
                                        <asp:LinkButton runat="server" Text="Verify" Visible="false" ID="radsdcesignveifylink" CommandName="CHIEFENGGSIGN"></asp:LinkButton>
                                        <asp:LinkButton runat="server" Text="Re-Verify" Visible="false" ID="radsdcesignreverifylink" CommandName="CHIEFENGGSIGN"></asp:LinkButton>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Action" ColumnGroupName="ods" HeaderStyle-Width="8%">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />

                                    <ItemTemplate>

                                        <asp:LinkButton runat="server" ID="btnsdedit" ToolTip="Edit" CommandName="AMEND">
                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" ID="btnsddelete" ToolTip="Delete" CommandName="Delete">
                                            <span class="icon"><i class="fas fa-times-circle"></i></span>
                                        </asp:LinkButton>
                                        <asp:ImageButton ID="btnsdattachments" runat="server" AlternateText="Attachment" CommandName="ATTACHMENT" ToolTip="Attachment" CommandArgument='<%# Container.DataSetIndex %>' ImageUrl='Session["images"]/no-attachment.png'></asp:ImageButton>


                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>

                            </Columns>

                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox"
                                AlwaysVisible="true" />

                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder" EnablePostBackOnRowClick="true">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </telerik:RadPageView>
            </telerik:RadMultiPage>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
