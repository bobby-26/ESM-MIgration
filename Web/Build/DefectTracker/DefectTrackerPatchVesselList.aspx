<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefectTrackerPatchVesselList.aspx.cs" EnableEventValidation ="false"
    EnableViewState="true" Inherits="DefectTrackerPatchVesselList" ValidateRequest ="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Patch List</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlPatchEntry">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Patch Acknowledgement" ShowMenu="false">
                        </eluc:Title>
                        <div style="position: absolute; top: 0px; right: 0px">
                            <eluc:TabStrip ID="MenuPatchDetail" runat="server" TabStrip="true" OnTabStripCommand="MenuPatchDetail_TabStripCommand">
                            </eluc:TabStrip>
                        </div>
                    </div>
                </div>
                <table width="60%">
                    <tr>
                        <td>
                            <asp:Label ID="lblTotalPatchSent" Text="Total Patch Sent" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtTotalPatchSent" MaxLength="3" Width="50%" CssClass="input" Enabled="false"
                                runat="server" />
                        </td>
                        <td>
                            <asp:Label ID="lblTotalAcknowledgement" Text="Total Patch Acknowledged" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtTotalAcknowledgement" MaxLength="3" Width="50%" CssClass="input"
                                Enabled="false" runat="server" />
                        </td>
                    </tr>
                </table>
                <div id="divGrid" style="position: relative; z-index: 0">
                    <asp:GridView ID="gvPatch" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        OnRowCreated="gvPatch_RowCreated" Width="100%" CellPadding="3" OnRowCommand="gvPatch_RowCommand"
                        OnRowDataBound="gvPatch_ItemDataBound" OnRowCancelingEdit="gvPatch_RowCancelingEdit"
                        OnRowDeleting="gvPatch_RowDeleting" OnRowUpdating="gvPatch_RowUpdating" OnRowEditing="gvPatch_RowEditing"
                        ShowHeader="true" EnableViewState="false" AllowSorting="true" OnSorting="gvPatch_Sorting">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField HeaderText="Patch">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Vessel Name
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblVesselID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></asp:Label>
                                    <asp:Label ID="lbldtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></asp:Label>
                                    <asp:LinkButton ID="lblVessel" runat="server" CommandName="SELECT" CommandArgument='<%# Container.DataItemIndex %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblVesselID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></asp:Label>
                                    <asp:Label ID="lblVesselName" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <HeaderTemplate>
                                    Sent Y/N
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="chkSelect" Enabled="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox runat="server" ID="chkSelectEdit" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Subject">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Date Sent on
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDateSenton" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSENTDATE") %>'
                                        runat="server"></asp:Label>
                                    <asp:LinkButton ID="lnkDateSenton" Visible="false" runat="server" CommandName="MAILSENT"
                                        CommandArgument='<%# Container.DataItemIndex %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDSENTDATE") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date ID="ucDateSenton" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSENTDATE") %>' />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Subject">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Date Acknowledged
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDateAcknowledged" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACKNOWLEDGEDON") %>'></asp:Label>
                                    <asp:LinkButton ID="lnkDateAcknowledged" Visible="false" runat="server" CommandName="MAILSENT"
                                        CommandArgument='<%# Container.DataItemIndex %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDACKNOWLEDGEDON") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date ID="ucDateAcknowledged" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACKNOWLEDGEDON") %>' />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Subject">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Remarks
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblRemarksItem" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDBODY").ToString().Length > 60 ? DataBinder.Eval(Container, "DataItem.FLDBODY").ToString().Substring(0, 60) + "..." : DataBinder.Eval(Container, "DataItem.FLDBODY").ToString() %> '
                                        Visible="true" ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDBODY") %>'></asp:Label>                                    
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="lblRemarksEdit" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>' />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Sent Mail." ImageUrl="<%$ PhoenixTheme:images/48.png %>"
                                        CommandName="SENDEMAIL" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSendemail"
                                        ToolTip="Send Email"></asp:ImageButton>
                                    <img id="Img3" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/24.png %>"
                                        CommandName="MAP" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdMap"
                                        ToolTip="Map to Sent Mail"></asp:ImageButton>
                                    <img id="Img4" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
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
                </div>
                <eluc:Status runat="server" ID="ucStatus" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
