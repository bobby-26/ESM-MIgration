<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceWorkOrderReschedule.aspx.cs"
    Inherits="PlannedMaintenanceWorkOrderReschedule" ValidateRequest="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Custom" Src="~/UserControls/UserControlEditor.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>
</div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmWorkOrderReschedule" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlWorkOrderReschedule">
        <ContentTemplate>
            <div style="top: 100px; margin-left: auto; margin-right: auto; width: 100%;">
                <div style="top: 100px; margin-left: auto; margin-right: auto; vertical-align: middle;">
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <div class="subHeader" style="position: relative; right: 0px">
                        <eluc:Title runat="server" ID="Title1" Text="Work Order Reschedule" ShowMenu="<%# Title1.ShowMenu %>">
                        </eluc:Title>
                         <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" Visible="false" OnClick="cmdHiddenSubmit_Click" />
                    </div>
                    <div class="navSelect" style="width: auto; float: right; margin-top: -26px">
                        <eluc:TabStrip ID="MenuWorkOrderRescheduleMain" runat="server" TabStrip="true" OnTabStripCommand="MenuWorkOrderRescheduleMain_TabStripCommand">
                        </eluc:TabStrip>
                    </div>
                </div>
                <div style="position: relative; overflow: hidden; clear: right;">
                    <iframe runat="server" id="ifMoreInfo" scrolling="no" style="min-height: 300px; width: 100%">
                    </iframe>
                </div>
                <div class="navSelect*" style="position: relative; clear: both; width: 15px">
                    <eluc:TabStrip ID="MenuWorkOrderReschedule" runat="server" OnTabStripCommand="MenuWorkOrderReschedule_TabStripCommand"></eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                    <asp:GridView ID="gvWorkOrderReschedule" runat="server" AutoGenerateColumns="False"
                        EnableViewState="False" Font-Size="11px" Width="100%" CellPadding="3" OnRowCommand="gvWorkOrderReschedule_RowCommand"
                        AllowSorting="true" OnSorting="gvWorkOrderReschedule_Sorting" OnRowDataBound="gvWorkOrderReschedule_RowDataBound"
                        DataKeyNames="FLDWORKORDERID" OnSelectedIndexChanging="gvWorkOrderRescheduler_SelectedIndexChanging"
                        OnRowCreated="gvWorkOrderReschedule_RowCreated" OnRowEditing="gvWorkOrderReschedule_RowEditing"
                        >
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblWorkorderNumberHeader" runat="server" CommandName="Sort" CommandArgument="FLDWORKORDERNUMBER"
                                        ForeColor="White">Work Order Number</asp:LinkButton>
                                    <img id="FLDWORKORDERNUMBER" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblWorkorderNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNUMBER") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblWorkorderNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDWORKORDERNAME"
                                        ForeColor="White">Work Order Title</asp:LinkButton>
                                    <img id="FLDWORKORDERNAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblComponentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>'></asp:Label>
                                    <asp:Label ID="lblWorkOrderId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERID") %>'></asp:Label>
                                    <asp:Label ID="lblJobID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTJOBID") %>'></asp:Label>
                                    <asp:Label ID="lbldtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></asp:Label>
                                    <asp:LinkButton ID="lnkWorkorderName" runat="server" CommandName="Select" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDWorkOrderID") %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNAME") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblComponentNumberHeader" runat="server" CommandName="Sort" CommandArgument="FLDCOMPONENTNUMBER"
                                        ForeColor="White">Component Number</asp:LinkButton>
                                    <img id="FLDCOMPONENTNUMBER" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblComponentNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNUMBER") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblComponentNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDCOMPONENTNAME"
                                        ForeColor="White">Component Name</asp:LinkButton>
                                    <img id="FLDCOMPONENTNAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblComponentName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblFrequencyHeader" runat="server" CommandName="Sort" CommandArgument="FLDFREQUENCY"
                                        ForeColor="White">Frequency</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblFrequency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFREQUENCY") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblPriorityHeader" runat="server" CommandName="Sort" CommandArgument="FLDPRIORITY"
                                        ForeColor="White">Priority</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPriority" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLANINGPRIORITY") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtPriorityEdit" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLANINGPRIORITY") %>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblResponsibleDisciplineHeader" runat="server" CommandName="Sort"
                                        CommandArgument="FLDPLANNINGDISCIPLINE" ForeColor="White">Responsible Discipline</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblResponsibleDiscipline" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Rank ID="ucRankEdit" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"
                                        RankList='<%#PhoenixRegistersRank.ListRank() %>' SelectedRank='<%# DataBinder.Eval(Container, "DataItem.FLDRANKID")%>' />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblDurationHeader" runat="server" CommandName="Sort" CommandArgument="FLDPLANNINGESTIMETDURATION"
                                        ForeColor="White">Estimation Duration</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDuration" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLANNINGESTIMETDURATION") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDurationEdit" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLANNINGESTIMETDURATION") %>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblDueDateHeader" runat="server" CommandName="Sort" CommandArgument="FLDPLANNINGDUEDATE"
                                        ForeColor="White">Due Date</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDueDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPLANNINGDUEDATE")) %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                     <asp:TextBox ID="ucDueDateEdit" runat="server" CssClass="input" MaxLength="20" Width="90px" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPLANNINGDUEDATE")) %>'></asp:TextBox>
                                    <ajaxToolkit:CalendarExtender ID="cetxtDueDate" runat="server" Format="dd/MMM/yyyy"
                                        Enabled="True" TargetControlID="ucDueDateEdit" PopupPosition="TopLeft">
                                    </ajaxToolkit:CalendarExtender>
                        
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblReason" runat="server" CommandName="Sort" CommandArgument="FLDPLANNINGDUECHANGEREASON"
                                        ForeColor="White">Reason</asp:Label>
                                </HeaderTemplate>
                                <EditItemTemplate>
                                    <span id="spnPickReason">
                                        <asp:TextBox ID="txtReason" runat="server" Width="1px"></asp:TextBox>
                                        <asp:ImageButton runat="server" ID="cmdShowReason" ImageUrl="<%$ PhoenixTheme:images/te_view.png %>"
                                             ImageAlign="AbsMiddle" CommandArgument="<%# Container.DataItemIndex %>"
                                            Text=".." />
                                    </span>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            
             <div id="divPage" style="position: relative;">
                    <table width="100%" border="0" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap="nowrap" align="center">
                                <asp:Label ID="lblPagenumber" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblPages" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblRecords" runat="server">
                                </asp:Label>
                            </td>
                            <td nowrap="nowrap" align="left" width="50px">
                                <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap="nowrap" align="right" width="50px">
                                <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                            </td>
                            <td nowrap="nowrap" align="center">
                                <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                </asp:TextBox>
                                <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                    Width="40px"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="gvWorkOrderReschedule" />
        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
