<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreDMRVoyage.aspx.cs" Inherits="CrewOffshoreDMRVoyage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPStatus" Src="~/UserControls/UserControlSEPStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlOffshoreVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Voyage</title>
    
 <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
      <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmVoyage" runat="server">

        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
       <telerik:RadSkinManager ID="skin1" runat="server"></telerik:RadSkinManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
            <div>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <%--<div class="subHeader" style="position: relative">
                    <div id="divHeading" class="divFloatLeft">
                        <eluc:Title runat="server" ID="Title1" Text="Charter" ShowMenu="true"/> 
                    </div>
                </div>--%>
                <div class="Header">
                    <div style="font-weight: 600; font-size: 12px;" runat="server">
                        <eluc:TabStrip ID="MenuVoyageTap" TabStrip="true" runat="server" OnTabStripCommand="VoyageTap_TabStripCommand">
                        </eluc:TabStrip>
                        <telerik:RadButton runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                    </div>
                </div>
                 
                <table cellpadding="1" cellspacing="1">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblmVesselName" runat="server" Text="Vessel Name"></telerik:RadLabel>
                            &nbsp;&nbsp;
                        </td>
                        <td>
                            <eluc:Vessel ID="UcVessel" runat="server" CssClass="input" AppendDataBoundItems="true" OnTextChangedEvent="UcVessel_TextChangedEvent" AutoPostBack="true" VesselsOnly="true" />
                            &nbsp;&nbsp;
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblCharterer" runat="server" Text="Charter ID"></telerik:RadLabel>
                            &nbsp;&nbsp;
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtCharterer" CssClass="input" MaxLength="1000"></telerik:RadTextBox>
                             &nbsp;&nbsp;
                        </td>
                         <td>
                            <telerik:RadLabel ID="lblProposalFlag" runat="server" Text="Display Proposed Charters"></telerik:RadLabel>
                            &nbsp;&nbsp;
                        </td>
                        <td>
                            <telerik:RadCheckBox ID="chkProposalflag" runat="server" AutoPostBack="true" OnCheckedChanged="chkProposalflag_OnCheckedChanged" />
                        </td>
                    </tr>
                </table>
             
            </div>
              <eluc:TabStrip ID="MenuVoyageList" runat="server" OnTabStripCommand="VoyageList_TabStripCommand">
                    </eluc:TabStrip>
         <div id="divGrid" style="position: relative; z-index: 1" >
       
           
                <telerik:RadGrid ID="gvVoyage" RenderMode="Lightweight" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                       CellSpacing="0" GridLines="None"   
                       OnNeedDataSource="gvVoyage_NeedDataSource" OnItemCommand="gvVoyage_ItemCommand" OnInsertCommand="gvVoyage_InsertCommand" OnItemDataBound ="gvVoyage_ItemDataBound">
                       <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                       <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                       AutoGenerateColumns="false" DataKeyNames="FLDVOYAGEID" TableLayout="Fixed" >
                       <HeaderStyle Width="102px" />
                       <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true"  ShowAddNewRecordButton="true" ShowExportToPdfButton="false"/>
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
                            <telerik:GridEditCommandColumn Exportable="false">
                                    <%--<ItemStyle Width="40px" />--%>
                                    <HeaderStyle Width="40px" />
                                </telerik:GridEditCommandColumn>     
                             <telerik:GridTemplateColumn HeaderText="Vessel Name">
                                 <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                                 <ItemTemplate>
                                     <telerik:RadLabel ID="lblVesselName" runat="server" CommandName="Redirect" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                 </ItemTemplate>
                             </telerik:GridTemplateColumn>
                             
                             <telerik:GridTemplateColumn HeaderText="Voyage No">
                                 <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                                 <HeaderTemplate>
                                     <telerik:RadLabel ID="lblVoyageNoHeader" runat="server" Text="Charter ID"></telerik:RadLabel>
                                 </HeaderTemplate>
                                 <ItemTemplate>
                                     <telerik:RadLabel ID="lblVoyageId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOYAGEID") %>'></telerik:RadLabel>
                                     <telerik:RadLabel  ID="lnkVoyageId" runat="server" CommandName="EDIT" 
                                         Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOYAGENO") %>'></telerik:RadLabel>
                                 </ItemTemplate>
                             </telerik:GridTemplateColumn>
                             
                             <telerik:GridTemplateColumn HeaderText="Commenced Date">
                                 <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                                 <ItemTemplate>
                                     <telerik:RadLabel ID="lblCommencedDate" runat="server" Width="120px" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOMMENCEDDATETIME")) + " " + DataBinder.Eval(Container,"DataItem.FLDCOMMENCEDDATETIME", "{0:HH:mm}") %>'></telerik:RadLabel>
                                 </ItemTemplate>
                             </telerik:GridTemplateColumn>
                             
                             <telerik:GridTemplateColumn HeaderText="Commenced Port">
                                 <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                                 <ItemTemplate>
                                     <telerik:RadLabel ID="lblCommencedPort" runat="server" Text=' <%# DataBinder.Eval(Container, "DataItem.FLDCOMMENCEDPORTANME")%>'></telerik:RadLabel>
                                 </ItemTemplate>
                             </telerik:GridTemplateColumn>

                             <telerik:GridTemplateColumn HeaderText="Commenced At Sea </br> In Location">
                                 <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                                 <ItemTemplate>
                                     <telerik:RadLabel ID="lblCommencedLocationAtSea" runat="server" Text=' <%# DataBinder.Eval(Container, "DataItem.FLDCOMMENCEMENTLOCATIONATSEA")%>'></telerik:RadLabel>
                                 </ItemTemplate>
                             </telerik:GridTemplateColumn>
                             
                             <telerik:GridTemplateColumn HeaderText="Completed Date">
                                 <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                                 <ItemTemplate>
                                     <telerik:RadLabel ID="lblCompletedDate" runat="server" Width="120px" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOMPLETEDDATE")) + " " + DataBinder.Eval(Container,"DataItem.FLDCOMPLETEDDATE", "{0:HH:mm}") %>'></telerik:RadLabel>
                                 </ItemTemplate>
                             </telerik:GridTemplateColumn>
                             
                             <telerik:GridTemplateColumn HeaderText="Completed Port">
                                 <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                                 <ItemTemplate>
                                     <telerik:RadLabel ID="lblCompletedPort" runat="server" Text=' <%# DataBinder.Eval(Container, "DataItem.FLDCOMPLETEDPORTNAME")%>'></telerik:RadLabel>
                                 </ItemTemplate>
                             </telerik:GridTemplateColumn>

                              <telerik:GridTemplateColumn HeaderText="Completed At Sea </br> In Location">
                                 <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                                 <ItemTemplate>
                                     <telerik:RadLabel ID="lblCommpletedLocationAtSea" runat="server" Text=' <%# DataBinder.Eval(Container, "DataItem.FLDCOMPLETIOLOCATIONATSEA")%>'></telerik:RadLabel>
                                 </ItemTemplate>
                             </telerik:GridTemplateColumn>

                             <telerik:GridTemplateColumn HeaderText="Estimated <br/> Start of Charter">
                                 <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                                 <ItemTemplate>
                                     <telerik:RadLabel ID="lblEstimatedCommencedDate" runat="server" Width="120px" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDESTIMATEDCOMMENCEDATE")) + " " + DataBinder.Eval(Container,"DataItem.FLDESTIMATEDCOMMENCEDATE", "{0:HH:mm}") %>'></telerik:RadLabel>
                                 </ItemTemplate>
                             </telerik:GridTemplateColumn>
                             
                              <telerik:GridTemplateColumn HeaderText="Charterer">
                                 <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                                 <ItemTemplate>
                                     <telerik:RadLabel ID="lblCharter" runat="server" Text=' <%# DataBinder.Eval(Container, "DataItem.FLDCHARTERERNAME")%>'></telerik:RadLabel>
                                 </ItemTemplate>
                             </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action">
                                 <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                                 <ItemTemplate>
                                     <asp:LinkButton ID="btndelete" CommandName="Delete" Width="20px" Height="20px" runat="server" >
                                          <span class="icon"><i class="fa fa-trash"></i></span>
                                     </asp:LinkButton>
                                 </ItemTemplate>
                             </telerik:GridTemplateColumn>
                         <%--  <telerik:GridButtonColumn CommandName="Delete" Text="Delete" ButtonType="ImageButton" UniqueName="DeleteColumn" ConfirmText="Delete this City?" ConfirmDialogType="RadWindow" Exportable="false" ConfirmDialogHeight="125px" ConfirmDialogWidth="200px">
                                    <HeaderStyle Width="40px" />
                                  
                                </telerik:GridButtonColumn>--%>

                   </Columns>
                            <PagerStyle AlwaysVisible="true" Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Charter matching your search criteria"
                                PageSizeLabelText="Charter per page:"  CssClass="RadGrid_Default rgPagerTextBox"/>
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                  </div>
                </telerik:RadAjaxPanel>
             
       
                <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
                </telerik:RadAjaxLoadingPanel>
           
         

    </form>
</body>
</html>
