<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DryDockJobList.aspx.cs" Inherits="DryDockJobList" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvJob.ClientID %>"));
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
    <form id="frmJobList" runat="server" autocomplete="off">

        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <telerik:RadAjaxPanel ID="Panel" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <telerik:RadButton ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

         
                <eluc:TabStrip ID="MenuJobGeneral" runat="server" OnTabStripCommand="JobGeneral_TabStripCommand"
                    TabStrip="true"></eluc:TabStrip>

          
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblJobNumber" runat="server" Text="Job Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtJobNumber" runat="server" CssClass="input" Width="220px" MaxLength="50"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblJobTitle" runat="server" Text="Job Title"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtJobTitle" runat="server" CssClass="input" Width="220px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblJobType" runat="server" Text="Job Type"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadDropDownList ID="ddlJobType" runat="server" CssClass="input" DefaultMessage="--Select--">
                        </telerik:RadDropDownList>
                    </td>
                </tr>
            </table>


            <eluc:TabStrip ID="MenuJob" runat="server" OnTabStripCommand="Job_TabStripCommand"></eluc:TabStrip>



            <telerik:RadGrid RenderMode="Lightweight" ID="gvJob" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None"
                OnNeedDataSource="gvJob_NeedDataSource"
                OnItemDataBound="gvJob_ItemDataBound1"
                OnItemCommand="gvJob_ItemCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDJOBID" TableLayout="Fixed" CommandItemDisplay="Top" Height="10px">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="RadLabel1" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                    <Columns>
                        <telerik:GridTemplateColumn>
                            <ItemTemplate>
                                <asp:ImageButton ID="cmdAttachments" runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                    CommandName="ATTACHMENT" CommandArgument='<%# Container.DataSetIndex %>' ToolTip="Attachment"></asp:ImageButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Number">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <asp:Label ID="lblJobid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBID") %>'></asp:Label>
                                <asp:Label ID="lbldtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></asp:Label>
                                <asp:LinkButton ID="lnkNumber" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Title">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTITLE").ToString().Length>10 ? DataBinder.Eval(Container, "DataItem.FLDTITLE").ToString().Substring(0, 10)+ "..." : DataBinder.Eval(Container, "DataItem.FLDTITLE").ToString() %>'></asp:Label>
                                <eluc:ToolTip ID="ucToolTipTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTITLE") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Job Type">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDJOBTYPE")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Job Description">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="50%"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblJobDescription" runat="server" Text='Details' ClientIDMode="AutoID"></telerik:RadLabel>
                                <eluc:ToolTip ID="ucJobDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBDESCRIPTION") %>' TargetControlId="lblJobDescription" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDSTATUSNAME")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Completed Satisfactorily">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDCOMPSATISFACTORILYNAME")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Edit"
                                    CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                    ToolTip="Edit">
                                         <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>


                    </Columns>
                    <PagerStyle AlwaysVisible="true" Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox" />
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
