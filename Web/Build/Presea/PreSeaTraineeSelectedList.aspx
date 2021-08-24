<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaTraineeSelectedList.aspx.cs"
    Inherits="PreSeaTraineeSelectedList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="../UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head" runat="server">
    <title></title>
     <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
</telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPreSeaTraineeList" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlTraineeList">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader">
                    <eluc:Title runat="server" ID="ttl" Text="Trainee List" ShowMenu="false"></eluc:Title>
                    <br />
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="PreSeaTraineeMenu" runat="server" OnTabStripCommand="PreSeaTraineeMenu_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <asp:GridView ID="gvTraineeList" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    OnRowDataBound="gvTraineeList_RowDataBound" Width="100%" CellPadding="3" ShowHeader="true"
                    EnableViewState="false">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                    <RowStyle Height="10px" />
                    <Columns>
                        <asp:TemplateField HeaderText="Select">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="25px"></ItemStyle>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelect" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Roll Number
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDBATCHROLLNUMBER") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Trainee Name
                            </HeaderTemplate>
                            <ItemTemplate>
                            <asp:Label ID="lblTraineeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAINEEID") %>'></asp:Label>
                                <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAINEENAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Date of birth
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# string.Format("{0:dd/MMM/yyyy}", DataBinder.Eval(Container, "DataItem.FLDDATEOFBIRTH"))%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                State
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDSTATENAME")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Nationality
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDNATIONALITY")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
