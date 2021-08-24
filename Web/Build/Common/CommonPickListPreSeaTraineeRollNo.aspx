<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonPickListPreSeaTraineeRollNo.aspx.cs"
    Inherits="CommonPickListPreSeaTraineeRollNo" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PreSeaCourse" Src="~/UserControls/UserControlPreSeaCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PreSeaBatch" Src="~/UserControls/UserControlPreSeaBatch.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Presea Trainee</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div runat="server" id="DivHeader">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmPreSeaTraineeList" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">
            <asp:Literal ID="lblTraineeList" runat="server" Text="Trainee List"></asp:Literal>
        </div>
    </div>
    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuTraineeList" runat="server" OnTabStripCommand="MenuTraineeList_TabStripCommand">
        </eluc:TabStrip>
    </div>
    <br clear="all" />
    <asp:UpdatePanel runat="server" ID="pnlTraineeList">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div id="search">
                <table cellpadding="2" cellspacing="2" width="100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblName" runat="server" Text="Name"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtName" runat="server" CssClass="input" Width="200px" MaxLength="200"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblBatchRollNo" runat="server" Text="Batch RollNo"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRollNo" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblCourse" runat="server" Text="Course"></asp:Literal>
                        </td>
                        <td>
                            <eluc:PreSeaCourse ID="ucPreSeaCourse" runat="server" CssClass="input" AppendDataBoundItems="true" />
                        </td>
                        <td>
                            <asp:Literal ID="lblBatch" runat="server" Text="Batch"></asp:Literal>
                        </td>
                        <td>
                            <eluc:PreSeaBatch ID="ucBatch" runat="server" CssClass="input" AppendDataBoundItems="true"
                                IsOutside="false" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divGrid" style="position: relative;">
                 <asp:GridView ID="gvPreSeaTrainee" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" OnRowCommand="gvPreSeaTrainee_RowCommand" OnRowDataBound="gvPreSeaTrainee_RowDataBound"
                    OnRowEditing="gvPreSeaTrainee_RowEditing" 
                    ShowHeader="true" EnableViewState="false" AllowSorting="true" OnSorting="gvPreSeaTrainee_Sorting">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Literal ID="lblBatchRollNo" runat="server" Text="Batch Roll No"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblTraineeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></asp:Label>
                                <asp:Label ID="lblRollNo" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCHROLLNUMBER") %>'></asp:Label>
                                
                                <asp:LinkButton ID="lnkRollNo" runat="server" CommandArgument="<%#Container.DataItemIndex%>"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCHROLLNUMBER") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Name">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblFirstnameHeader" runat="server">Name                                
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date Of Birth">
                            <HeaderTemplate>
                                <asp:Literal ID="lblDateofBirth" runat="server" Text="Date of Birth"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblDateOfBirth" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFBIRTH","{0:dd/MM/yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Batch Name">
                            <HeaderTemplate>
                                <asp:Literal ID="lblBatchName" runat="server" Text="Batch Name"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblBatchName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCHNAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div id="divPage" style="position: relative;">
                <table width="100%" border="0" cellpadding="1" cellspacing="1" class="datagrid_pagestyle">
                    <tr>
                        <td nowrap="nowrap" align="center">
                            <asp:Label ID="lblPagenumber" runat="server">
                            </asp:Label>
                            <asp:Label ID="lblPages" runat="server">
                            </asp:Label>
                            <asp:Label ID="lblRecords" runat="server">
                            </asp:Label>&nbsp;&nbsp;
                        </td>
                        <td nowrap="nowrap" align="left" width="50px">
                            <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev"><< Prev </asp:LinkButton>
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
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="gvPreSeaTrainee" />
        </Triggers>
    </asp:UpdatePanel>    
    </form>
</body>
</html>
