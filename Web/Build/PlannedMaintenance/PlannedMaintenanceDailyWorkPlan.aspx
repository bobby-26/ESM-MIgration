<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceDailyWorkPlan.aspx.cs" Inherits="PlannedMaintenance_PlannedMaintenanceDailyWorkPlan" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Daily Work Plan</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function CloseWindow() {                
                <%--document.getElementById('<%=DateFromFieldValidator.ClientID%>').innerHTML = "";                
                document.getElementById('<%=ValidationSummary1.ClientID%>').style.display = 'none';            --%>    
            }
            function CloseModelWindow() {
                var radwindow = $find('<%=modalPopup.ClientID %>');
                radwindow.close();
                var masterTable = $find("<%= gvDailyWorkPlan.ClientID %>").get_masterTableView();
                masterTable.rebind();
            }
        </script>
    </telerik:RadCodeBlock>
    <style type="text/css">
        .alignleft{
            text-align:left;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />        
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>          
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">        
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuDailyWorkPlan" runat="server" OnTabStripCommand="MenuDailyWorkPlan_TabStripCommand"></eluc:TabStrip>
        <telerik:RadGrid ID="gvDailyWorkPlan" runat="server" Height="94%" OnItemDataBound="gvDailyWorkPlan_ItemDataBound" MasterTableView-ShowFooter="false"
            OnNeedDataSource="gvDailyWorkPlan_NeedDataSource" OnItemCommand="gvDailyWorkPlan_ItemCommand" OnSortCommand="gvDailyWorkPlan_SortCommand"
            EnableViewState="false" ShowFooter="true" AllowCustomPaging="true" AllowPaging="true" EnableHeaderContextMenu="true">
            <MasterTableView EditMode="InPlace" AutoGenerateColumns="false" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" DataKeyNames="FLDDAILYWORKPLANID">
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Daily Work Plan No.">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDetail" runat="server"  CommandName="SELECT" CommandArgument='<%# ((DataRowView)Container.DataItem)["FLDDAILYWORKPLANID"]%>'
                                Text='<%#((DataRowView)Container.DataItem)["FLDPLANNO"]%>'></asp:LinkButton>
                        </ItemTemplate>
                         <%--<EditItemTemplate>
                            <telerik:RadTextBox ID="txtPlanNumber" ToolTip="No." runat="server" Width="100%" MaxLength="10" CssClass="gridinput_mandatory"
                                Text='<%#((DataRowView)Container.DataItem)["FLDPLANNO"]%>'>
                            </telerik:RadTextBox>
                        </EditItemTemplate>--%>
                         <FooterTemplate>
                            <telerik:RadTextBox ID="txtPlanNumberAdd" ToolTip="No." runat="server" Width="100%" MaxLength="10" CssClass="gridinput_mandatory">
                            </telerik:RadTextBox>
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>                                       
                    <telerik:GridTemplateColumn HeaderText="Date">
                        <ItemTemplate>
                            <%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDDATE"])%>
                        </ItemTemplate>
                         <%--<EditItemTemplate>
                            <eluc:Date ID="txtDate" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDDATE"].ToString()%>' />
                        </EditItemTemplate>--%>
                        <FooterTemplate>
                            <eluc:Date ID="txtDateAdd" runat="server" Width="100%" CssClass="input_mandatory" />
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Vessel Status">
                        <ItemTemplate>
                            <%# ((DataRowView)Container.DataItem)["FLDVESSELSTATUSNAME"]%>
                        </ItemTemplate>                        
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Change Time">
                        <ItemTemplate>
                            <%# ((DataRowView)Container.DataItem)["FLDCHANGETIME"]%>
                        </ItemTemplate>                        
                    </telerik:GridTemplateColumn>
                    <%--<telerik:GridTemplateColumn HeaderText="Status">
                        <ItemTemplate>
                           <%#((DataRowView)Container.DataItem)["FLDSTATUS"]%>
                        </ItemTemplate>                         
                    </telerik:GridTemplateColumn>     --%>              
                    <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                        <HeaderStyle />
                        <ItemStyle Wrap="false" />
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDITR" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                            </asp:LinkButton>                           
                        </ItemTemplate>
                      <%--  <EditItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                            </asp:LinkButton>
                        </EditItemTemplate>--%>
                        <FooterStyle HorizontalAlign="Center" />
                        <FooterTemplate>
                            <asp:LinkButton runat="server" AlternateText="Save" CommandName="Add" ID="cmdAdd" ToolTip="Add New">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                            </asp:LinkButton>
                        </FooterTemplate>
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
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
             <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="415px" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
        </telerik:RadAjaxPanel>        
        <telerik:RadWindow ID="modalPopup" runat="server" Width="500px" Height="365px" Modal="true" OnClientClose="CloseWindow" OffsetElementID="main"
             VisibleStatusbar="false" KeepInScreenBounds="true" NavigateUrl="PlannedMaintenanceDailyWorkPlanAdd.aspx">            
        </telerik:RadWindow>
        <telerik:RadCodeBlock runat="server" ID="rdbScripts">
            <script type="text/javascript">
                $modalWindow.modalWindowID = "<%=modalPopup.ClientID %>";
            </script>
        </telerik:RadCodeBlock>
    </form>
</body>
</html>
