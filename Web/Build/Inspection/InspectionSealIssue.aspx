<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionSealIssue.aspx.cs"
    Inherits="InspectionSealIssue" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <telerik:RadCodeBlock runat="server">
        <title>Seal Issue</title>

        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

  
         <script type="text/javascript">
        function confirm(args) {
            if (args) {
                __doPostBack("<%=confirm.UniqueID %>", "");
            }
        }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSealIssue" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
       <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                    <telerik:AjaxSetting AjaxControlID="ucConfirm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvSeal" />
                        <telerik:AjaxUpdatedControl ControlID="ucConfirm" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                         <telerik:AjaxUpdatedControl ControlID="ddlFromSealNo" />
                         <telerik:AjaxUpdatedControl ControlID="ddlToSealNo" />
                    </UpdatedControls> 
                </telerik:AjaxSetting>
                </AjaxSettings>
            <AjaxSettings>
                    <telerik:AjaxSetting AjaxControlID="ddlToSealNo">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="txtSelectedSeals" />
                          <telerik:AjaxUpdatedControl ControlID="ddlFromSealNo" />
                         <telerik:AjaxUpdatedControl ControlID="ddlToSealNo" />
                    </UpdatedControls> 
                </telerik:AjaxSetting>
                </AjaxSettings>
           </telerik:RadAjaxManager>
        <div style="top: 100px; margin-left: auto; margin-right: auto; width: 100%;">
            <div style="top: 100px; margin-left: auto; margin-right: auto; vertical-align: middle;">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <telerik:RadButton ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                <telerik:RadButton ID="confirm" runat="server" Text="cmdHiddenSubmit" OnClick="btnConfirm_Click" />
                <div style="font-weight: 600; font-size: 12px;" runat="server">
                    <eluc:TabStrip ID="MenuSealIssue" runat="server" OnTabStripCommand="MenuSealIssue_TabStripCommand"></eluc:TabStrip>
                </div>
            </div>
            <div id="divFind" style="position: relative; z-index: 2;">
                <table id="tblFilter" width="100%">
                    <tr>
                        <td colspan="4">
                            <b><font color="blue" size="0">
                                <telerik:RadLabel ID="lblNote" runat="server" Text="Note: To issue or return the seals, you can either select
                                    seals from the list or specify a range of seal numbers.">
                                </telerik:RadLabel>
                            </font></b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblSelectaRangeTo" runat="server" Text="Select a range to"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadDropDownList ID="ddlType" runat="server"  AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged" >
                                <Items>
                                    <telerik:DropDownListItem Text="Issue" Value="1" Selected="True" />
                                    <telerik:DropDownListItem Text="Return" Value="2" />
                                </Items>

                            </telerik:RadDropDownList>
                            <telerik:RadLabel ID="lblSeals" runat="Server" Text="seals"></telerik:RadLabel>
                        </td>
                        <td colspan="2"></td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblFromSealNo" runat="server" Text="From Seal Number"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadDropDownList ID="ddlFromSealNo" DefaultMessage="--Select--" runat="server"  AutoPostBack="true" 
                            OnSelectedIndexChanged="ddlFromSealNo_SelectedIndexChanged" DataTextField="FLDSEALNO" DataValueField="FLDSEALID">
                            </telerik:RadDropDownList>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblToSealNo" runat="server" Text="To Seal Number"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadDropDownList ID="ddlToSealNo" DefaultMessage="--Select" runat="server"  AutoPostBack="true" OnSelectedIndexChanged="ddlToSealNo_SelectedIndexChanged"
                                 DataTextField="FLDSEALNO" DataValueField="FLDSEALID">
                            </telerik:RadDropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblNoOfSealsSelected" runat="server" Text="No. of seals selected"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtSelectedSeals" runat="server"  ReadOnly="true"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <b>
                                <telerik:RadLabel ID="lblFilter" runat="server" Text="Filter"></telerik:RadLabel>
                            </b>
                        </td>
                    </tr>
                    <tr>
                        <td width="20%">
                            <telerik:RadLabel ID="lblSealType" runat="server" Text="Seal Type"></telerik:RadLabel>
                        </td>
                        <td width="30%">
                            <eluc:Quick ID="ucSealType" runat="server" Width="200px" AutoPostBack="true" AppendDataBoundItems="true"
                                QuickTypeCode="87"  OnTextChangedEvent="ucSealType_Changed" />
                        </td>
                        <td width="20%">
                            <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                        </td>
                        <td width="30%">
                            <eluc:Hard ID="ucSealStatus" runat="server" Width="100px" AutoPostBack="true" HardTypeCode="197"
                                CssClass="input_mandatory" ShortNameFilter="WMS,ISS" OnTextChangedEvent="SealStatus_changed" />
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td width="20%">
                            <telerik:RadLabel ID="lblSealNo" runat="server" Text="Seal Number"></telerik:RadLabel>
                        </td>
                        <td width="30%">
                            <telerik:RadTextBox ID="txtSealNumber" runat="server"  Width="100px"></telerik:RadTextBox>
                        </td>
                        <td width="20%">
                            <telerik:RadLabel ID="lblROB" runat="server" Text="ROB"></telerik:RadLabel>
                        </td>
                        <td width="30%">
                            <telerik:RadTextBox ID="txtROB" runat="server"  Width="50px" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="font-weight: 600; font-size: 12px;" runat="server">
                <eluc:TabStrip ID="MenuSealExcel" runat="server" OnTabStripCommand="MenuSealExcel_TabStripCommand"></eluc:TabStrip>
            </div>
            <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                <%-- <asp:GridView ID="gvSeal" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" OnRowDataBound="gvSeal_RowDataBound" ShowHeader="true"
                    EnableViewState="false" AllowSorting="true" ShowFooter="false" OnSorting="gvSeal_Sorting"
                    DataKeyNames="FLDSEALID" OnRowCommand="gvSeal_RowCommand">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />
                    <Columns>--%>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvSeal" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None"
                   GroupingEnabled="false" EnableHeaderContextMenu="true"   
                    OnNeedDataSource="gvSeal_NeedDataSource"
                    OnItemCommand="gvSeal_ItemCommand"
                    OnSortCommand="gvSeal_SortCommand">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDSEALID" TableLayout="Fixed"  Height="10px">
                        <HeaderStyle Width="102px" />
                        <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
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
                            <telerik:GridTemplateColumn HeaderText="Select All">
                                <HeaderStyle Width="30px" />
                                <itemstyle wrap="false" horizontalalign="Center" width="80px" />
                                <headertemplate>
                                <telerik:RadCheckBox ID="chkAllSeal" runat="server" Text="Select All" AutoPostBack="true"
                                    OnPreRender="CheckAll" />
                            </headertemplate>
                                <itemstyle wrap="false" horizontalalign="Center" width="80px" />
                                <itemtemplate>
                                <telerik:RadCheckBox ID="chkSelect" runat="server" EnableViewState="true" OnCheckedChanged="SaveCheckedValues" />
                            </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Seal Number">
                             
                                <itemtemplate>
                                <telerik:RadLabel ID="lblSealID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEALID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="Label1" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDSEALNO"]%>'></telerik:RadLabel>
                            </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Seal Type">
                          
                                <itemtemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDSEALTYPENAME"]%>
                            </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Status">
                           
                                <itemtemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDSTATUSNAME"]%>
                            </itemtemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle AlwaysVisible="true" Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                   <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                </telerik:RadGrid>
            </div>

        </div>
      <%--  <eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="btnConfirm_Click" OKText="Yes"
            CancelText="No" />--%>
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
    </form>
</body>
</html>
