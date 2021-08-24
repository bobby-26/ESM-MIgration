<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewCourseMoveParticipantList.aspx.cs"
    Inherits="CrewCourseMoveParticipantList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="../UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewCourseMoveParticipantList" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlCourseMoveParticipantList">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader">
                    <eluc:Title runat="server" ID="ttlAboutUsBy" Text="Move To Participant" ShowMenu="false">
                    </eluc:Title>
                    <br />
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="CrewMenu" runat="server" OnTabStripCommand="CrewMenu_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                </div>
                <div class="subHeader" style="position: relative;">
                    <span class="navSelect" style="margin-top: 0px; float: right; width: auto;">
                        <eluc:TabStrip ID="CrewCourseMoveParticipant" runat="server" OnTabStripCommand="CrewCourseMoveParticipantList_TabStripCommand">
                        </eluc:TabStrip>
                    </span>
                </div>
                <table cellspacing="1" cellpadding="1" width="100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblName" Text="Move to Batch No" runat="server"></asp:Literal>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlBatch" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true">
                                <asp:ListItem Value="Dummy" Text="--Select--"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
    
                <asp:GridView ID="gvParticipantList" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    OnRowDataBound="gvParticipantList_RowDataBound" Width="100%" CellPadding="3"
                    ShowHeader="true" EnableViewState="false">
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                    <RowStyle Height="10px" />
                    <Columns>
                        <asp:TemplateField HeaderText="Select">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="25px"></ItemStyle>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelect" runat="server"  />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblempName" Text="Employee Name" runat="server"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblEmployeeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></asp:Label>
                                <asp:Label ID="lblEnrollmentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENROLLMENTID") %>'></asp:Label>
                                <asp:Label ID="lblEmpName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblRankNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDRANKNAME"
                                    ForeColor="White">Rank &nbsp;</asp:LinkButton>
                                <img id="FLDRANKNAME" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblQueueDateHeader" runat="server" CommandName="Sort" CommandArgument="FLDQUEUEDATE"
                                    ForeColor="White">Enrolled On&nbsp;</asp:LinkButton>
                                <img id="FLDQUEUEDATE" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblApprovedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVEDDATE", "{0:dd/MMM/yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <br />
                <eluc:Status ID="ucStatus" runat="server" />
                <eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="InitiateMovement" OKText="Ok"
                    CancelText="Cancel" Visible="false" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
