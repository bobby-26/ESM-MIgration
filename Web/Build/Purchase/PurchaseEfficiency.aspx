<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseEfficiency.aspx.cs" Inherits="Purchase_PurchaseEfficiency" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCheckBoxList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleetList.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
      <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
      <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
</telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel runat="server" ID="pnlCrewReportEntry" Height="100%" >
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            
                            <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                                TabStrip="false"></eluc:TabStrip>
                   <%-- <table width="100%" border="1" style="border-collapse: collapse;">
                        <tr valign="top">
                            <td width="25%">
                                <table>
                        </tr>
                    </table>--%>
                    <table cellpadding="2" cellspacing="2">
                        <tr align="top">
                            <td style="overflow-y: hidden">
                                <telerik:RadLabel ID="lblYear" runat="server" Text="Year"></telerik:RadLabel>
                                <br />
                                <telerik:RadListBox ID="ddlYear" SelectionMode="Multiple" AppendDataBoundItems="true" CssClass="input" Width="100px" Height="80px" runat="server"></telerik:RadListBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="litQuarter" runat="server" Text="Quarter"></telerik:RadLabel>
                                <br />
                                <telerik:RadListBox ID="lstQuarter" SelectionMode="Multiple" AppendDataBoundItems="true" CssClass="input" Width="100px" Height="80px" runat="server">
                                    <Items>
                                    <telerik:RadListBoxItem Value="" Text="--Select--"></telerik:RadListBoxItem>
                                    <telerik:RadListBoxItem Value="1" Text="Q1"></telerik:RadListBoxItem>
                                    <telerik:RadListBoxItem Value="2" Text="Q2"></telerik:RadListBoxItem>
                                    <telerik:RadListBoxItem Value="3" Text="Q3"></telerik:RadListBoxItem>
                                    <telerik:RadListBoxItem Value="4" Text="Q4"></telerik:RadListBoxItem>
                                        </Items>
                                </telerik:RadListBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="litMonth" runat="server" Text="Month"></telerik:RadLabel>
                                <br />
                                <telerik:RadListBox ID="lstMonth" SelectionMode="Multiple" AppendDataBoundItems="true" CssClass="input" Width="100px" Height="80px" runat="server">
                                    <Items>
                                    <telerik:RadListBoxItem Value="" Text="--Select--"></telerik:RadListBoxItem>
                                    <telerik:RadListBoxItem Value="1" Text="Jan"></telerik:RadListBoxItem>
                                    <telerik:RadListBoxItem Value="2" Text="Feb"></telerik:RadListBoxItem>
                                    <telerik:RadListBoxItem Value="3" Text="Mar"></telerik:RadListBoxItem>
                                    <telerik:RadListBoxItem Value="4" Text="Apr"></telerik:RadListBoxItem>
                                    <telerik:RadListBoxItem Value="5" Text="May"></telerik:RadListBoxItem>
                                    <telerik:RadListBoxItem Value="6" Text="Jun"></telerik:RadListBoxItem>
                                    <telerik:RadListBoxItem Value="7" Text="July"></telerik:RadListBoxItem>
                                    <telerik:RadListBoxItem Value="8" Text="Aug"></telerik:RadListBoxItem>
                                    <telerik:RadListBoxItem Value="9" Text="Sep"></telerik:RadListBoxItem>
                                    <telerik:RadListBoxItem Value="10" Text="Oct"></telerik:RadListBoxItem>
                                    <telerik:RadListBoxItem Value="11" Text="Nov"></telerik:RadListBoxItem>
                                    <telerik:RadListBoxItem Value="12" Text="Dec"></telerik:RadListBoxItem>
                                        </Items>
                                </telerik:RadListBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblGroup" runat="server" Text="Group"></telerik:RadLabel>
                                <br />

                                <div runat="server" id="divgroup" class="input" style="overflow: auto; width: 98%; height: 80px"
                                    onscroll="javascript:setgroupscroll();">
                                    <asp:hiddenfield id="hdnscrollgroup" runat="server" />
                                    <telerik:RadCheckBoxList ID="chkGroupList" runat="server" AutoPostBack="false" Height="100%"
                                         RepeatColumns="1" RepeatDirection="Horizontal" RepeatLayout="Flow" > 
                                    </telerik:RadCheckBoxList>
                                    </div>
                            </td>
                            <td>
                                <telerik:RadLabel ID="litLocation" runat="server" Text="Location"></telerik:RadLabel>
                                <div runat="server" id="divLocation" class="input" style="overflow-y: auto; overflow-x: hidden; height: 80px" onscroll="javascript:setGroupScroll();">
                                    <telerik:RadCheckBoxList ID="lstPurchaseLocation" runat="server" DataTextField="FLDLOCATIONNAME" AutoPostBack="false"
                                        DataValueField="FLDLOCATIONID" RepeatDirection="Vertical" RepeatColumns="1" OnDataBound="DataBound">
                                    </telerik:RadCheckBoxList>
                                </div>
                            </td>
                            <td style="display: inline-block;">
                                <telerik:RadLabel ID="litType" runat="server" Text="Stock Type :"></telerik:RadLabel>
                                <telerik:RadComboBox ID="ddlType" runat="server"  AutoPostBack="false" >
                                    <Items>
                                    <telerik:RadComboBoxItem Value="STORE" Selected="True" Text="Store"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Value="SPARE" Text="Spare"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Value="SERVICE" Text="Service"></telerik:RadComboBoxItem>
                                        </Items>
                                </telerik:RadComboBox>
                         
                            </td>
                        </tr>
                    </table>

  
                        <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand"></eluc:TabStrip>

                        <telerik:RadGrid RenderMode="Lightweight" ID="gvCrew" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowPaging="true" AllowCustomPaging="true"
                            OnItemDataBound="gvCrew_RowDataBound" OnItemCommand="gvCrew_RowCommand" Width="100%"  Height="70%" EnableHeaderContextMenu="true" GroupingEnabled="false"
                            CellPadding="3" ShowHeader="true" EnableViewState="false" AllowSorting="true" OnNeedDataSource="gvCrew_NeedDataSource"
                            OnSortCommand="gvCrew_Sorting">
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                              <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                               <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="">
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
                                <telerik:GridTemplateColumn HeaderText="Vendor" AllowSorting="true" SortExpression="FLDVENDORNAME">
                                    
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container,"DataItem.FLDVENDORNAME") %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Form No."  AllowSorting="true" SortExpression="FLDFORMNO">
                                     <ItemTemplate>
                                        <%# DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Vessel" AllowSorting="true" SortExpression="FLDVESSELNAME">
                                     <ItemTemplate>
                                        <%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Stock Type" AllowSorting="true" SortExpression="FLDSTOCKTYPE">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container,"DataItem.FLDSTOCKTYPE") %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Purchase Location" AllowSorting="true" SortExpression="FLDLOCATIONNAME">
                                     <ItemTemplate>
                                        <%# DataBinder.Eval(Container,"DataItem.FLDLOCATIONNAME") %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Purchase Group" AllowSorting="true" SortExpression="FLDGROUPNAME">
                                    
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container,"DataItem.FLDGROUPNAME") %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Purchaser" AllowSorting="true" SortExpression="FLDPURCHASER">
                                    
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container,"DataItem.FLDPURCHASER") %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="PO Total(USD)" AllowSorting="true" SortExpression="FLDPOAMOUNTTOTAL">
                                      <ItemTemplate>
                                        <%# DataBinder.Eval(Container,"DataItem.FLDPOAMOUNTTOTAL") %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                            </Columns>
                                       <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                                   </MasterTableView>
                              <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true"  />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
                        </telerik:RadGrid>
<%--                    </div>--%>
                  
<%--                <br />--%>
             <%--   <div id="div2" runat="server">
                    <div class="navSelect" style="position: relative; width: 15px">--%>
<%--                        <eluc:TabStrip ID="MenuExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand"></eluc:TabStrip>--%>
<%--                    </div>--%>
                <%--    <table width="100%">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="ltGrid" runat="server" Text=""></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>--%>
          <%--      </div>
                </div>
            </ContentTemplate>--%>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>









