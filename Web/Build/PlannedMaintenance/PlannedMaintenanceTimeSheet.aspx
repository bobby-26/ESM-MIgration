<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceTimeSheet.aspx.cs" Inherits="PlannedMaintenance_PlannedMaintenanceTimeSheet" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function setWO(args) {
                var id = args.split("~")[0];
                var name = args.split("~")[1];
               <%-- document.getElementById('<%=txtMaintenanceId.ClientID%>').value = id;
                $find('<%=txtMaintenace.ClientID%>').set_value(name);--%>
            }
            function CloseUrlModelWindow() {
                var wnd = $find('<%=modelPopup.ClientID %>');
                wnd.close();
            }
            function refresh() {
                var wnd = getRadWindow('maint');
                if (wnd == null)
                    wnd = getRadWindow('dsd');
                if (wnd != null) {
                    var button = wnd.GetContentFrame().contentWindow.document.getElementById("cmdHiddenSubmit");
                    if (button != null)
                        button.click();
                    setRadWindowZIndex(wnd);
                    wnd.setActive(true);
                }
                parent.frames[1].$find("RadAjaxManager1").ajaxRequest("TIMESHEET");
                wnd = getRadWindow('wo');
                if (wnd != null) {
                    wnd['close']();
                }
            }
            function onItemChecking(sender, args) {
                var text = args.get_item().get_text();
                var detail = document.getElementById('txtDetail');
                if (args.get_item().get_checked())
                    detail.value += text + "\n";
                else {
                    detail.value = detail.value.split(text + "\n").join('');
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
       <telerik:RadAjaxPanel runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuWorkOrder" runat="server" OnTabStripCommand="MenuWorkOrder_TabStripCommand"></eluc:TabStrip>
            <telerik:RadFormDecorator ID="FormDecorator1" runat="server" DecoratedControls="all"></telerik:RadFormDecorator>
           <table width="100%">
               <tr>
                   <td valign="top" width="50%">
                       <table border="0">
                           <tr>
                               <td>
                                   <telerik:RadLabel ID="lblVesselStatus" runat="server" Text="Vessel Status"></telerik:RadLabel>
                               </td>
                               <td>
                                   <telerik:RadComboBox ID="ddlVesselStatus" runat="server" Width="200px" Filter="Contains" MarkFirstMatch="true">
                                   </telerik:RadComboBox>
                               </td>

                           </tr>
                           <tr>
                               <td>
                                   <telerik:RadLabel ID="lblStart" runat="server" Text="Time"></telerik:RadLabel>
                               </td>
                               <td>
                                   <telerik:RadDatePicker ID="txtDateTime" runat="server" AutoPostBack="true" OnSelectedDateChanged="txtDateTime_SelectedDateChanged">
                                   </telerik:RadDatePicker>
                                   <telerik:RadTimePicker ID="txttimepicker" runat="server" DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm" Width="60px">
                                   </telerik:RadTimePicker>
                               </td>

                           </tr>
                           <%--<tr runat="server" id="trMaintenance" style="display:none">
                    <td>
                        <telerik:RadLabel ID="lblMaintenance" runat="server" Text="Maintenance"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadTextBox ID="txtMaintenace" runat="server" ReadOnly="true" Width="200px"></telerik:RadTextBox>
                        <asp:LinkButton ID="lnkMaintenance" runat="server" ImageAlign="AbsMiddle" Text=".." OnClientClick="showDialog('Add');return false;">
                            <span class="icon"><i class="fas fas fa-tasks"></i></span>
                        </asp:LinkButton>
                        <asp:HiddenField ID="txtMaintenanceId" runat="server" />
                    </td>
                </tr>--%>
                           <tr runat="server" id="trOperation">
                               <td>
                                   <telerik:RadLabel ID="lblOperation" runat="server" Text="Operation"></telerik:RadLabel>
                               </td>
                               <td>
                                   <telerik:RadTextBox ID="txtOperation" runat="server" Enabled="false" Width="200px"></telerik:RadTextBox>
                                   <telerik:RadComboBox ID="ddlOperation" runat="server" Width="200px" Filter="Contains" MarkFirstMatch="true" AutoPostBack="true" OnSelectedIndexChanged="ddlOperation_SelectedIndexChanged">
                                   </telerik:RadComboBox>
                               </td>
                           </tr>
                           <tr runat="server" id="trstatus" visible="false">
                               <td>
                                   <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                               </td>
                               <td>
                                   <telerik:RadRadioButtonList ID="rblStatus" runat="server" Layout="Flow" Direction="Horizontal">
                                   </telerik:RadRadioButtonList>
                               </td>
                           </tr>
                           <tr runat="server" id="trStart" visible="false">
                               <td>
                                   <telerik:RadLabel ID="lblStartTime" runat="server" Text="Start Time"></telerik:RadLabel>
                               </td>
                               <td>
                                   <telerik:RadDateTimePicker ID="txtStartTime" runat="server" Width="200px" DateInput-AutoPostBack="true">
                                   </telerik:RadDateTimePicker>
                               </td>
                           </tr>
                           <tr runat="server" id="trEnd" visible="false">
                               <td>
                                   <telerik:RadLabel ID="lblEndTime" runat="server" Text="End Time"></telerik:RadLabel>
                               </td>
                               <td>
                                   <telerik:RadDateTimePicker ID="txtEndTime" runat="server" Width="200px" DateInput-AutoPostBack="true">
                                   </telerik:RadDateTimePicker>
                               </td>
                           </tr>
                           <tr>
                               <td>
                                   <telerik:RadLabel ID="lblDetail" runat="server" Text="Details"></telerik:RadLabel>
                               </td>
                               <td>
                                   <telerik:RadTextBox ID="txtDetail" runat="server" TextMode="MultiLine" Width="400px" Height="180px"></telerik:RadTextBox>
                               </td>
                           </tr>
                           <tr>
                               <td>
                                   <telerik:RadLabel ID="lblEvent" runat="server" Text="Event"></telerik:RadLabel>
                               </td>
                               <td>
                                   <telerik:RadListBox ID="chkEventList" runat="server" DataTextField="FLDEVENTNAME" DataValueField="FLDSTANDARDEVENTID"
                                       CheckBoxes="true" Height="180px" Width="400px" OnClientItemChecked="onItemChecking">
                                   </telerik:RadListBox>
                               </td>
                           </tr>
                       </table>
                   </td>
                   <td id="gvtable" runat="server" visible="false" valign="top">
                       <telerik:RadGrid ID="gvMembers" runat="server"
                           OnNeedDataSource="gvMembers_NeedDataSource" OnItemDataBound="gvMembers_ItemDataBound"
                           ShowFooter="false" AllowCustomPaging="true" AllowPaging="true" EnableHeaderContextMenu="true">
                           <MasterTableView EditMode="InPlace" AutoGenerateColumns="false" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                               CommandItemDisplay="None"
                               DataKeyNames="FLDMEMBERID"
                               HeaderStyle-HorizontalAlign="Center">
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
                                   <telerik:GridTemplateColumn HeaderText="Name">
                                       <ItemStyle HorizontalAlign="Left" />
                                       <ItemTemplate>
                                           <telerik:RadLabel ID="lblname" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDEMPLOYEENAME")%>'>
                                           </telerik:RadLabel>
                                           <telerik:RadLabel ID="lblmemberid" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMEMBERID")%>' Visible="false">
                                           </telerik:RadLabel>
                                           <telerik:RadLabel ID="lblactivityid" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDACTIVITYID")%>' Visible="false">
                                           </telerik:RadLabel>
                                       </ItemTemplate>
                                   </telerik:GridTemplateColumn>

                                   <telerik:GridTemplateColumn HeaderText="Schedule Category">
                                       <HeaderStyle Width="150px" />
                                       <ItemStyle HorizontalAlign="Left" />
                                       <ItemTemplate>
                                           <telerik:RadLabel ID="lblschedulecategory" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CATNAME")%>'>
                                           </telerik:RadLabel>
                                           <telerik:RadLabel ID="lblcatid" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDUTYCATEGORYID")%>' Visible="false">
                                           </telerik:RadLabel>
                                       </ItemTemplate>

                                   </telerik:GridTemplateColumn>

                                   <telerik:GridTemplateColumn HeaderText="Action">
                                       <ItemStyle HorizontalAlign="Center" />
                                       <ItemTemplate>

                                           <asp:LinkButton runat="server" AlternateText="Timesheet" ID="cmdTimesheet" CommandName="TIME" ToolTip="Timesheet" Visible="false">
                                            <span class="icon"><i class="far fa-calendar-alt"></i></span>
                                           </asp:LinkButton>
                                       </ItemTemplate>

                                   </telerik:GridTemplateColumn>

                               </Columns>
                           </MasterTableView>
                           <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                               PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox"
                               AlwaysVisible="true" />
                           <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                               <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                               <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="415px" />
                               <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                           </ClientSettings>
                       </telerik:RadGrid>
                   </td>
               </tr>
           </table>
        </telerik:RadAjaxPanel>
        <telerik:RadWindow runat="server" ID="modelPopup" Width="900px" Height="365px"
            Modal="true" OffsetElementID="main" VisibleStatusbar="false" KeepInScreenBounds="true">
        </telerik:RadWindow>
        <telerik:RadCodeBlock runat="server" ID="rdbScripts">
            <script type="text/javascript">
                $modalWindow.modalWindowID = "<%=modelPopup.ClientID %>";
            </script>
        </telerik:RadCodeBlock>
    </form>
</body>
</html>
