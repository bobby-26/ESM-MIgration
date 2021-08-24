<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseRequisitionNotSubmittedForQuery.aspx.cs" Inherits="PurchaseRequisitionNotSubmittedForQuery" %>

<!DOCTYPE html>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZone.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>RequisitionNotSubmittedForQuery</title>
     <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
      <script type="text/javascript">
        
          function PaneResized(sender, args) {
              var browserHeight = $telerik.$(window).height();
              var grid = $find("gvRequisition");
              grid._gridDataDiv.style.height = (browserHeight - 120) + "px";
            //console.log(grid._gridDataDiv.style.height, contentPane._contentElement.offsetHeight);
        }
        function pageLoad() {
            PaneResized();
        }
    </script>
    </telerik:RadCodeBlock>
</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="form1" runat="server">
   
       
      <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <table id="tblRequisitionNotSubmittedForQuery" width="80%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblvesselsearch" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>

                        <eluc:Vessel ID="UcVessel" runat="server" OnTextChangedEvent="UcVessel_TextChangedEvent" AutoPostBack="true" VesselsOnly="true" Width="100%" AppendDataBoundItems="true" />

                    </td>
                    <td>
                        <telerik:RadLabel ID="lblfromdate" runat="server" Text="From Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucfromdatesearch" runat="server" CssClass="input_mandatory" Width="100%" />

                    </td>
                    <td>
                        <telerik:RadLabel ID="lbltodate" runat="server" Text="To Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="uctodatesearch" runat="server" CssClass="input_mandatory" Width="100%" />

                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblfromno" runat="server" Text="From No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtfromnosearch" runat="server" Width="100%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblzone" runat="server" Text="Zone"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Zone ID="ucZonesearch" runat="server" AppendDataBoundItems="true" OnTextChangedEvent="ucZone_Changed" Width="100%" />

                    </td>
                    <td>
                        <telerik:RadLabel ID="lbllimit" runat="server" Text="Limit"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="uclimit" runat="server" Defaultvalue="3" Width="100%" />

                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuRegistersholiday" runat="server" OnTabStripCommand="Registersholiday_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvRequisition" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                CellSpacing="0" GridLines="None"   OnNeedDataSource="gvRequisition_NeedDataSource"
                ShowFooter="false" ShowHeader="true" EnableViewState="false"  EnableHeaderContextMenu="true" GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" >
                       <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Form No" >
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" Width="15%" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                             <telerik:RadLabel ID="lblvesselid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                             <telerik:RadLabel ID="lblorderid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>'></telerik:RadLabel>

                                <telerik:RadLabel ID="lblfromno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Title" >
                            <HeaderStyle Width="25%" />
                            <ItemStyle Width="25%" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbltitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTITLE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            </telerik:GridTemplateColumn>

                         <telerik:GridTemplateColumn HeaderText="Created On" >
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblcreateddate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Active On" >
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblactiveon" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUISITIONACTIVEON","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            </telerik:GridTemplateColumn>

                         <telerik:GridTemplateColumn HeaderText="RFQ Sent On" >
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblquerysubmission" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUERYSUBMITION","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Days" >
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDays" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEDIFF") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            </telerik:GridTemplateColumn>

                        </Columns>
                    
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
