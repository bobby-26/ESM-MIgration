<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreDMRROBInitialization.aspx.cs" Inherits="CrewOffshoreDMRROBInitialization" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Multiport" Src="~/UserControls/UserControlMultiColumnDMRVoyagePort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ROB Initialization</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
        <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
            runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlCourseRegister">
            <ContentTemplate>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                    width: 100%">
                    <div class="subHeader" style="position: relative">
                        <div id="divHeading">
                            <eluc:Title runat="server" ID="ucTitle" Text="Voyage Initialization"></eluc:Title>
                            <asp:Button ID="cmdHiddenPick" runat="server" OnClick="cmdHiddenPick_Click" />
                        </div>
                    </div>
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                        <eluc:TabStrip ID="MenuRobMain" runat="server" OnTabStripCommand="MenuRobMain_TabStripCommand"
                            TabStrip="true"></eluc:TabStrip>
                    </div>
                    <div id="divMain" style="position: relative; z-index: 15">
                        <table id="tblRob">
                            <tr>
                                <td>
                                    <asp:Literal ID="lblVessel" runat="server" Text="Vessel Name"></asp:Literal>  
                                </td>
                                <td>
                                    <asp:TextBox ID="txtVessel" runat="server" Width="180px" CssClass="readonlytextbox" Enabled="False"></asp:TextBox>    
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="lblVoyageNo" runat="server" Text="Initial Project Name"></asp:Literal>
                                </td>
                                <td>
                                    <span id="spnPickListVoyage">
                                        <asp:TextBox ID="txtVoyageName" runat="server" Width="180px" CssClass="input_mandatory" Enabled="False"></asp:TextBox>
                                        <asp:ImageButton ID="btnShowVoyage" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                            ImageAlign="AbsMiddle" Text=".." />
                                        <asp:TextBox ID="txtVoyageId" runat="server" Width="0px" CssClass="input"></asp:TextBox>
                                    </span>
                                </td>
                            </tr>
                            <tr>         
                                <td>
                                    <asp:Literal ID="lblDate" runat="server" Text="Date"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:UserControlDate ID="txtDate" runat="server" CssClass="input_mandatory" ReadOnly="false"
                                        DatePicker="true" />
                                    &nbsp;
                                <asp:TextBox ID="txtTime" runat="server" CssClass="input_mandatory" Width="50px" />
                                <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" AcceptAMPM="false"
                                    ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99" MaskType="Time"
                                    TargetControlID="txtTime" UserTimeFormat="TwentyFourHour" />
                                </td>                      
                            </tr>
                        </table>
                    </div>
                    <hr />
                    <div id="divRob">
                        <asp:Literal ID="lblROBonCommencingVoyage" runat="server" Text="ROB on Commencing Project"></asp:Literal>
                        <asp:GridView ID="gvOil" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" OnRowCommand="gvOil_RowCommand" OnRowDataBound="gvOil_ItemDataBound"
                            OnRowCancelingEdit="gvOil_RowCancelingEdit" OnRowUpdating="gvOil_RowUpdating"
                            OnRowEditing="gvOil_RowEditing" ShowFooter="false" ShowHeader="true" EnableViewState="false"
                            AllowSorting="true">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <Columns>
                                <asp:TemplateField HeaderText="Oil Type">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblOilOil" runat="server" Text="Oil Type"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblOilTypeCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPECODE") %>'></asp:Label>
                                        <asp:Label ID="lblOilShortName" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTNAME") %>'></asp:Label>
                                        <asp:Label ID="lblOilTypeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPENAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ROB">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblROB" runat="server" Text="ROB"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblOilROB" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROB") %>'
                                            DecimalPlace="2"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Number ID="txtOilROBEdit" runat="server" CssClass="input_mandatory txtNumber" DecimalPlace="2"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDROB") %>' IsPositive="true" MaxLength="9" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblActionHeader" runat="server" Text="Action"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                            CommandName="Edit" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                            ToolTip="Edit"></asp:ImageButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                            CommandName="Update" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                            ToolTip="Save"></asp:ImageButton>
                                        <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                            ToolTip="Cancel"></asp:ImageButton>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <br />
                    </div>
                    <eluc:Status runat="server" ID="ucStatus" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
