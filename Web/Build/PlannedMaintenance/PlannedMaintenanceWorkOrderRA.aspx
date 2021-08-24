<%@ Page Language="C#" AutoEventWireup="True" CodeFile="PlannedMaintenanceWorkOrderRA.aspx.cs"
    Inherits="PlannedMaintenanceWorkOrderRA" ValidateRequest="true" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head2" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
         <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function CloseUrlModelWindow() {
                var wnd = $find('<%=RadWindow_NavigateUrl.ClientID %>');                
                wnd.close();
                var btn = document.getElementById("<%=cmdHiddenSubmit.ClientID %>");
                btn.click();
            }
        </script>
    </telerik:RadCodeBlock>
    <style type="text/css">
        .hidden {
            display: none;
        }
    </style>
</head>
<body>
    <form id="frmRA" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server"></telerik:RadWindowManager>

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <asp:Button Text="Click" runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <eluc:TabStrip ID="MenuWorkOrderRA" runat="server" OnTabStripCommand="MenuWorkOrderRA_TabStripCommand"></eluc:TabStrip>
        <table cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDueDate" runat="server" Text="Due Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDueDate" runat="server" CssClass="readonly" ReadOnly="true" Enabled="false" />
                    </td>
                </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblPostponeDate" runat="server" Text="Postponed to Date"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadDatePicker ID="txtPostponeDate" runat="server" Width="120px" CssClass="input_mandatory" AutoPostBack="true" OnSelectedDateChanged="txtPostponeDate_SelectedDateChanged"  />                    
                    &nbsp; <span id="spnPickReason">
                        <asp:ImageButton runat="server" ID="cmdShowReason" ImageUrl="<%$ PhoenixTheme:images/reschedule-remark.png %>"
                            ImageAlign="AbsMiddle" Text=".." ToolTip="Postponement Remarks" Visible="false" />
                        <asp:ImageButton runat="server" ID="cmdShowHistory" ImageUrl="<%$ PhoenixTheme:images/annexure.png %>"
                            ImageAlign="AbsMiddle" Text=".." ToolTip="Postponement History" />
                    </span>
                </td>
            </tr>
           <%-- <tr>
                <td>
                    <telerik:RadLabel ID="lblRHMaintenanceDue" runat="server" Text="Running Hours at which maintenance is due"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadLabel ID="lblRHMaintenanceDueValue" runat="server" Text=""></telerik:RadLabel>
                </td>
            </tr>--%>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblCurrentRunningHr" runat="server" Text="Current running Hrs"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadLabel ID="lblCurrentRunningHrValue" runat="server" Text=""></telerik:RadLabel>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblEstRunHr" runat="server" Text="Running Hours to which maintenance is to be postponed"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadLabel ID="lblEstRunHrValue" runat="server" Text=""></telerik:RadLabel>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblEstDueDate" runat="server" Text="Due date"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadLabel ID="lblEstDueDateValue" runat="server" Text=""></telerik:RadLabel>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblPosponeDate" runat="server" Text="Postponed to date"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadLabel ID="lblPosponeDateValue" runat="server" Text=""></telerik:RadLabel>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblPostponeReason" runat="server" Text="Postponement Reason"></telerik:RadLabel>
                </td>
                <td colspan="2">
                    <eluc:Quick ID="ddlRescheduleReason" runat="server" QuickTypeCode="120" AppendDataBoundItems="true"
                        CssClass="input" Width="306px" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblExpCriteria" runat="server" Text="Exceptional Circumstances"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadCheckBox ID="chkExpectionalCircumstance" runat="server" Enabled="false"></telerik:RadCheckBox>
                </td>
            </tr>
            <tr runat="server" id="divRA" visible="false">                
                <td>
                    <telerik:RadLabel ID="lblRA" runat="server" Text="RA"></telerik:RadLabel>
                </td>
                <td>
                    <span id="spnRA">
                        <telerik:RadTextBox ID="txtRANumber" runat="server" Enabled="false"
                            MaxLength="50" Width="100px" Text="">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtRA" runat="server" Enabled="false"
                            MaxLength="50" Width="200px" Text="" >
                        </telerik:RadTextBox>
                        <asp:ImageButton ID="imgShowRA" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                            ImageAlign="AbsMiddle" Text=".."  CssClass="hidden"/>
                        <telerik:RadTextBox ID="txtRAId" runat="server" CssClass="hidden" MaxLength="20" Width="0px"
                            Text="">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtRaType" runat="server" CssClass="hidden" MaxLength="2" Width="0px"
                            Text=''>
                        </telerik:RadTextBox>
                    </span>
                    &nbsp;
                    <asp:ImageButton runat="server" AlternateText="Show RA Details" ImageUrl="<%$ PhoenixTheme:images/BarChart.png %>"
                        ID="cmdRA" ToolTip="Show PDF" OnClick="cmdRA_Click"></asp:ImageButton>
                    <asp:LinkButton ID="lnkCreateRA" runat="server" Text="Create Machinery RA" OnClick="lnkCreateRA_OnClick" Visible="false"></asp:LinkButton>
                </td>                                          
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblReportedBy" Visible="false" runat="server" Text="Reported By"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtReportedBy" Visible="false" runat="server" CssClass="input"></telerik:RadTextBox>
                </td>
                <td>
                    <asp:LinkButton ID="lnkDownloadRA" Visible="false" runat="server" Text="Download RA" OnClick="lnkDownloadRA_Click"></asp:LinkButton></td>

            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblPostponementRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtReason" runat="server" TextMode="MultiLine" Rows="3" Width="306px" CssClass="input_mandatory"></telerik:RadTextBox>
                </td>
                <td>
                    <a href="GuidanceDocument.doc" target="_blank">Guidance Document</a>
                </td>
            </tr>
            <tr>
                <td colspan="3">                   
                    <asp:HiddenField ID="hdnComp" runat="server" />
                </td>
            </tr>
        </table>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvPostponematrix" runat="server"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvPostponematrix_NeedDataSource"
                ShowHeader="true" EnableViewState="true" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <NoRecordsTemplate>
                        <table id="Table1" runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Priority" AllowSorting="false">     
                            <HeaderStyle Width="50px" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDPRIORITY") %>
                            </ItemTemplate>                           
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="1st Extension Approval By" AllowSorting="false">                            
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                               <%# DataBinder.Eval(Container,"DataItem.FLDFIRSTEXTNOFFICEAPPROVAL") %>
                                <%# DataBinder.Eval(Container,"DataItem.FLDFIRSTEXTNSHIPAPPROVAL") %>
                            </ItemTemplate>                            
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="1st Extension Maximum" AllowSorting="false" >                            
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                              <%# DataBinder.Eval(Container,"DataItem.FLDFIRSTEXTNDAYS") %> or
                              <%# DataBinder.Eval(Container,"DataItem.FLDFIRSTEXTNRUNHRPCT") %>
                            </ItemTemplate>                          
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="2nd Extension Approval By" AllowSorting="false">                            
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                              <%# DataBinder.Eval(Container,"DataItem.FLDSECONDEXTNOFFICEAPPROVAL") %>
                                <%# DataBinder.Eval(Container,"DataItem.FLDSECONDEXTNSHIPAPPROVAL") %>
                            </ItemTemplate>                          
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="2nd Extension Maximum" AllowSorting="false">                            
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDSECONDEXTNDAYS") %>
                                    or
                                 <%# DataBinder.Eval(Container,"DataItem.FLDSECONDEXTNRUNHRPCT") %>
                            </ItemTemplate>                           

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="RA Req. for each Extension" AllowSorting="false">                           
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDRAREQUIREMENT") %>
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
        <telerik:RadWindow runat="server" ID="RadWindow_NavigateUrl" Width="900px" Height="365px"
            Modal="true" OffsetElementID="main" VisibleStatusbar="false" KeepInScreenBounds="true" ReloadOnShow="true" ShowContentDuringLoad="false">
        </telerik:RadWindow>
        <telerik:RadCodeBlock runat="server" ID="rdbScripts">
            <script type="text/javascript">
                $modalWindow.modalWindowID = "<%=RadWindow_NavigateUrl.ClientID %>";
            </script>
        </telerik:RadCodeBlock>
    </form>
</body>
</html>
