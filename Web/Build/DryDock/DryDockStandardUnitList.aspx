<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DryDockStandardUnitList.aspx.cs"
    Inherits="DryDockStandardUnitList" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <style>
        .imgbtn-height {
            height: 20px;
        }
    </style>
    <telerik:radcodeblock id="Radcodeblock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
      
         <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvStandardUnit.ClientID %>"));
                }, 200);
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
        <script type="text/javascript" language="javascript">
            function checkSelectedYN(btn) {
                document.getElementById(btn).click();
            }
            function selectJob(jobid, obj) {
                AjxPost("functionname=selecttariff|jobid=" + jobid + "|checked=" + obj.checked + "|jobregister=4", SitePath + "PhoenixWebFunctions.aspx", null, false);
            }
        </script>

    </telerik:radcodeblock>

</head>
<body>
    <form id="frmStandardUnitList" runat="server" autocomplete="off">
        <telerik:radscriptmanager runat="server" id="RadScriptManager1" />
        <telerik:radskinmanager id="RadSkinManager1" runat="server" />
        
                <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <telerik:radbutton id="cmdHiddenSubmit" runat="server" text="cmdHiddenSubmit" onclick="cmdHiddenSubmit_Click" />
        <eluc:TabStrip ID="MenuStandardStandardUnit" runat="server" TabStrip="true" OnTabStripCommand="StandardStandardUnit_TabStripCommand" ></eluc:TabStrip>
        <table cellpadding="1" cellspacing="1">
            <tr>
                <td>
                    <telerik:radlabel id="lblTitle" runat="server" text="Title"></telerik:radlabel>
                </td>
                <td style="padding-right: 30px">
                    <telerik:radtextbox rendermode="Lightweight" id="txtJobTitle" runat="server" width="220px"></telerik:radtextbox>
                </td>
                <td>
                    <telerik:radlabel id="lblDescription" runat="server" text="Description"></telerik:radlabel>

                </td>
                <td>
                    <telerik:radtextbox rendermode="Lightweight" id="txtJobDesc" runat="server" width="220px"></telerik:radtextbox>
                </td>
            </tr>
        </table>
        <eluc:TabStrip ID="MenuStandardUnit" runat="server" OnTabStripCommand="StandardUnit_TabStripCommand"></eluc:TabStrip>
        <telerik:radgrid rendermode="Lightweight" id="gvStandardUnit" runat="server" allowcustompaging="true" allowsorting="true" allowpaging="true" cellspacing="0" gridlines="None" groupingenabled="false"
            onneeddatasource="gvStandardUnit_NeedDataSource" onsorting="gvStandardUnit_Sorting" onitemdatabound="gvStandardUnit_ItemDataBound" onitemcommand="gvStandardUnit_ItemCommand"
            autogeneratecolumns="false" enableheadercontextmenu="true">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" AllowPaging="true" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDJOBID" TableLayout="Fixed"  >
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
                <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Number" HeaderStyle-Width="10%" AllowSorting="true" SortExpression="FLDNUMBER">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                       <HeaderStyle Wrap="true"  />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblJobid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lbldtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lnkNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Title" HeaderStyle-Width="20%" AllowSorting="true" SortExpression="FLDTITLE">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"  Width="20%"></ItemStyle>
                        
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkTitle" runat="server" CommandName="Select" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTITLE") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    
                    <telerik:GridTemplateColumn HeaderText="Job Description" HeaderStyle-Width="40%"   >
                        <ItemStyle Wrap="true" HorizontalAlign="Left"  Width="40%"></ItemStyle>
                       
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblJobDescription" runat="server" Text='Description' ClientIDMode="AutoID"></telerik:RadLabel>
                            <eluc:ToolTip ID="ucJobDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBDESCRIPTION") %>' TargetControlId="lblJobDescription" />

                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Include" HeaderStyle-Width="5%" >
                        <ItemStyle Wrap="true" HorizontalAlign="Left" Width="5%"    VerticalALign="Middle"></ItemStyle>                        
                        <ItemTemplate>
                            <telerik:RadCheckBox runat="server" ID="chkSelectedYN" Text="" BackColor="Transparent" />
                            <telerik:RadButton runat="server" ID="cmdSelectedYN" Visible="true" Text="<%# Container.DataSetIndex %>" CommandName="SELECTJOB"  Width="0px" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderStyle-Width="5%" HeaderText="Action" >
                        <HeaderStyle HorizontalAlign="center" VerticalAlign="Middle"  Width="5%"></HeaderStyle>
                        
                        <ItemTemplate>
                            <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Edit" CssClass="imgbtn-height"
                                CommandName="EDIT"  ID="cmdEdit"
                                ToolTip="Edit">
                             <span class="icon"><i class="fas fa-edit"></i></span>
                            </asp:LinkButton>

                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">

                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="420px" SaveScrollPosition="true" FrozenColumnsCount="2" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>

        </telerik:radgrid>
                    </telerik:RadAjaxPanel>
    </form>
</body>
</html>