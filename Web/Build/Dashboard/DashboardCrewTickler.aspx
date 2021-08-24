<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardCrewTickler.aspx.cs"
    Inherits="DashboardCrewTickler" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="DashboardMenu" Src="~/UserControls/UserControlDashboardMenu.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonToolTip" Src="~/UserControls/UserControlCommonToolTip.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div2" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixDashboard.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div>
                <eluc:DashboardMenu runat="server" ID="ucDashboardMenu" ShowMenu="true" />
                <div class="subHeader">
                    <div class="divFloatLeft">
                        <eluc:Title runat="server" ID="Title2" Text="Tickler" ShowMenu="false"></eluc:Title>
                    </div>
                    <div style="position: absolute; right: 0px">
                        <eluc:TabStrip ID="MenuPhoenixBroadcast" runat="server" OnTabStripCommand="PhoenixBroadcast_TabStripCommand">
                        </eluc:TabStrip>
                    </div>
                </div>
                <div>
                    <table width="100%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                                <asp:Literal ID="lblUser" runat="server" Text="User"></asp:Literal>
                            </td>
                            <td>
                                <span id="spnPickListUsers">
                                <asp:TextBox CssClass="input_mandatory" runat="server" ID="txtUser" MaxLength="200" Width="80%"></asp:TextBox>
                                <asp:TextBox ID="lblUserCode" runat="server"  Width="0px"></asp:TextBox>
                            </span>
                                <asp:ImageButton ID="cmdShowUsers" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                    ImageAlign="AbsMiddle" Text=".." />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblRemindOn" runat="server" Text="Remind On"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Date ID="txtReminderDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                                &nbsp;
                                <asp:Literal ID="lblTime" runat="server" Text="Time"></asp:Literal>
                                &nbsp;
                                <asp:TextBox ID="txtTime" runat="server" CssClass="input_mandatory" Width="50px" />
                                <ajaxToolkit:MaskedEditExtender ID="txtTimeMask" runat="server" AcceptAMPM="true"
                                    ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99" MaskType="Time"
                                    TargetControlID="txtTime" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblMessage" runat="server" Text="Message"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox CssClass="input_mandatory" runat="server" ID="txtMessage" TextMode="MultiLine"
                                    Rows="5" Columns="80" MaxLength="500" Width="80%"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <hr />
                    <br />
                    <table width="80%">
                        <tr>
                            <td>
                                <asp:Literal ID="lblReminderFromDate" runat="server" Text="Reminder From Date"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Date ID="txtFromDate" runat="server" CssClass="input" DatePicker="true" />
                            </td>
                            <td>
                                <asp:Literal ID="lblReminderToDate" runat="server" Text="Reminder To Date"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Date ID="txtToDate" runat="server" CssClass="input" DatePicker="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblSentRecieved" runat="server" Text="Sent / Recieved"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="input">
                                    <asp:ListItem Value="">--Select--</asp:ListItem>
                                    <asp:ListItem Value="1">Sent</asp:ListItem>
                                    <asp:ListItem Value="0">Received</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Literal ID="lblCompleted" runat="server" Text="Completed"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCompleted" runat="server" CssClass="input">
                                    <asp:ListItem Value="">--Select--</asp:ListItem>
                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                    <asp:ListItem Value="0">No</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <div class="navSelect" id="divTab1" runat="server" style="position: relative; width: 15px">
                        <eluc:TabStrip ID="MenuTickler" runat="server" OnTabStripCommand="MenuTickler_TabStripCommand">
                        </eluc:TabStrip>
                    </div>
                    <asp:GridView GridLines="None" runat="server" ID="gvTickler" AutoGenerateColumns="false" Width="100%"
                        ShowFooter="false" ShowHeader="true" EnableViewState="false" OnRowCancelingEdit="gvTickler_RowCancelingEdit"
                        OnRowEditing="gvTickler_RowEditing" OnRowUpdating="gvTickler_RowUpdating" OnRowDataBound="gvTickler_RowDataBound"
                        OnRowCommand="gvTickler_RowCommand" DataKeyNames="FLDTICKLERID">
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField HeaderText="Posted By">
                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDPOSTEDBYNAME")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Reminder Date">
                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDREMINDERDATE").ToString() != string.Empty ? 
                                        DateTime.Parse(DataBinder.Eval(Container, "DataItem.FLDREMINDERDATE").ToString()).ToLocalTime().ToString("g")
                                        : ""%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblReminderDate" runat="server" Text=' <%#DataBinder.Eval(Container, "DataItem.FLDREMINDERDATE").ToString() != string.Empty ? 
                                        DateTime.Parse(DataBinder.Eval(Container, "DataItem.FLDREMINDERDATE").ToString()).ToLocalTime().ToString("g")
                                        : ""%>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Comments">
                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTASKREMARKS").ToString().Length>30 ? DataBinder.Eval(Container, "DataItem.FLDTASKREMARKS").ToString().Substring(0, 30) + "..." : DataBinder.Eval(Container, "DataItem.FLDTASKREMARKS").ToString() %>'></asp:Label>
                                    <eluc:ToolTip ID="ucRemarksTT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTASKREMARKS") %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblRemarksEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTASKREMARKS").ToString().Length>30 ? DataBinder.Eval(Container, "DataItem.FLDTASKREMARKS").ToString().Substring(0, 30) + "..." : DataBinder.Eval(Container, "DataItem.FLDTASKREMARKS").ToString() %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sent To">
                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDTOUSER")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ReScheduled Date">
                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDSCHEDULEDATE").ToString() != string.Empty ?
                                                                                DateTime.Parse(DataBinder.Eval(Container, "DataItem.FLDSCHEDULEDATE").ToString()).ToLocalTime().ToString("g")
                                        : ""%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date ID="txtRescheduleDate" runat="server" CssClass="input_mandatory" DatePicker="true"
                                        Text='<%#DataBinder.Eval(Container, "DataItem.FLDSCHEDULEDATE")%>' />
                                    <asp:TextBox ID="txtReScheduleTime" runat="server" CssClass="input_mandatory" Width="50px"
                                        Text='<%#DataBinder.Eval(Container, "DataItem.FLDSCHEDULEDATE").ToString() != string.Empty ?
                                                                                DateTime.Parse(DataBinder.Eval(Container, "DataItem.FLDSCHEDULEDATE").ToString()).ToLocalTime().ToString("t")
                                        : ""%>' />
                                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" AcceptAMPM="true"
                                        ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99" MaskType="Time"
                                        TargetControlID="txtReScheduleTime" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Completed">
                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="80px"></ItemStyle>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDCOMPLETEDYN").ToString() == "0" ? "" : "Yes"%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkDoneYN" runat="server" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action Taken">
                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblDoneRemarks" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOMPLETEDREMARKS").ToString().Length>30 ? DataBinder.Eval(Container, "DataItem.FLDCOMPLETEDREMARKS").ToString().Substring(0, 30) + "..." : DataBinder.Eval(Container, "DataItem.FLDCOMPLETEDREMARKS").ToString() %>'></asp:Label>
                                    <eluc:ToolTip ID="ucDoneRemarksTT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPLETEDREMARKS") %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDoneRemarks" runat="server" Width="350px" Height="30px" CssClass="gridinput_mandatory"
                                        TextMode="MultiLine">
                                    </asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionByHeader" runat="server">Action By &nbsp;                                        
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# Eval("FLDCOMPLETEDBYNAME")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionDateHeader" runat="server">Action Date&nbsp;                                        
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDCOMPLETEDDATE", "{0:dd/MM/yyyy}")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblSentReceivedHeader" runat="server" Text="Sent / Received"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDSENTYN").ToString() == "1" ? "Sent" : "Received"%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="true" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <HeaderStyle HorizontalAlign="Center" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblAction" runat="server" Text="Action"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Snooze" ImageUrl="<%$ PhoenixTheme:images/dashboard.png %>"
                                        CommandName="SNOOZE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSnooze"
                                        ToolTip="Snooze"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Update" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <table width="100%" border="0" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap align="center">
                                <asp:Label ID="lblPagenumber" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblPages" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblRecords" runat="server">
                                </asp:Label>&nbsp;&nbsp;
                            </td>
                            <td nowrap align="left" width="50px">
                                <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap align="right" width="50px">
                                <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                            </td>
                            <td nowrap align="center">
                                <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                </asp:TextBox>
                                <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" CssClass="input"
                                    Width="40px"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
