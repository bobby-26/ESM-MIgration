<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionIncidentCauseAnalysis.aspx.cs" Inherits="InspectionIncidentCauseAnalysis" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="RootCause" Src="~/UserControls/UserControlImmediateMainCause.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SubRootCause" Src="~/UserControls/UserControlImmediateSubCause.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BasicCause" Src="~/UserControls/UserControlBasicMainCause.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SubBasicCause" Src="~/UserControls/UserControlBasicSubCause.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Inscident CAR</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="divInspectionNonConformity" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmInspectionNonConformity" runat="server">
    <ajaxtoolkit:toolkitscriptmanager id="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxtoolkit:toolkitscriptmanager>
    <asp:UpdatePanel runat="server" ID="pnlInspectionNC">
        <ContentTemplate>
            <eluc:error id="ucError" runat="server" text="" visible="false"></eluc:error>
            <eluc:status id="ucStatus" runat="server" text="" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <br />
                <b><asp:Literal ID="lblImmediateCause" runat="server" Text="Immediate Cause"></asp:Literal></b>
                <br />
                <div style="position: relative; width: 15px">
                    <eluc:tabstrip id="MenuRootCause" runat="server" ontabstripcommand="MenuRootCause_TabStripCommand">
                    </eluc:tabstrip>
                </div>
                <div id="divGridRootCause" style="position: relative; z-index: 0">
                    <asp:GridView ID="gvImmediateCause" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvImmediateCause_RowCommand" OnRowDataBound="gvImmediateCause_ItemDataBound"
                        OnRowCancelingEdit="gvImmediateCause_RowCancelingEdit" OnRowDeleting="gvImmediateCause_RowDeleting"
                        OnRowCreated="gvImmediateCause_RowCreated" OnRowEditing="gvImmediateCause_RowEditing"
                        ShowFooter="true" ShowHeader="true" EnableViewState="false" AllowSorting="true"
                        OnSelectedIndexChanging="gvImmediateCause_SelectedIndexChanging" OnRowUpdating="gvImmediateCause_RowUpdating">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblRootCauseHeader" runat="server">
                                        Main Cause
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblInsRootcauseid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONINCIDENTIMMEDIATECAUSEID") %>'></asp:Label>
                                    <asp:Label ID="lblRootCauseId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIMMEDIATECAUSEID") %>'></asp:Label>
                                    <asp:Label ID="lblRootCauseName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIMMEDIATECAUSENAME") %>'
                                        Width="200px"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblInsRootcauseidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONINCIDENTIMMEDIATECAUSEID") %>'></asp:Label>
                                    <asp:Label ID="lblRootCauseIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIMMEDIATECAUSEID") %>'></asp:Label>
                                    <eluc:rootcause id="ucRootCauseEdit" runat="server" appenddatabounditems="true" autopostback="true"
                                        Width="200px" cssclass="dropdown_mandatory" maincauselist='<%# PhoenixInspectionImmediateMainCause.ListMainCause(1, null) %>'
                                        selectedmaincause='<%# DataBinder.Eval(Container,"DataItem.FLDIMMEDIATECAUSEID") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:rootcause id="ucRootCauseAdd" runat="server" appenddatabounditems="true" cssclass="dropdown_mandatory"
                                        Width="200px" autopostback="true" maincauselist='<%# PhoenixInspectionImmediateMainCause.ListMainCause(1, null) %>' />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblSubRootCauseHeader" runat="server">
                                    Sub Cause
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSubRootCauseId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBIMMEDIATECAUSEID") %>'></asp:Label>
                                    <asp:Label ID="lblSubRootCause" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBCAUSENAME") %>'
                                        Width="300px"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblSubRootCauseIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBIMMEDIATECAUSEID") %>'></asp:Label>
                                    <eluc:subrootcause id="ucSubRootCauseEdit" runat="server" appenddatabounditems="true"
                                        Width="300px" cssclass="dropdown_mandatory" subcauselist='<%# PhoenixInspectionImmediateSubCause.ListSubCause(1,null) %>'
                                        selectedsubcause='<%# DataBinder.Eval(Container,"DataItem.FLDSUBIMMEDIATECAUSEID") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:subrootcause id="ucSubRootCauseAdd" runat="server" appenddatabounditems="true"
                                        Width="300px" cssclass="dropdown_mandatory" subcauselist='<%# PhoenixInspectionImmediateSubCause.ListSubCause(1,null) %>' />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblReasonHeader" runat="server">
                                    Reason
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblReason" runat="server" Width="200px" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREASON").ToString().Length>100 ? DataBinder.Eval(Container, "DataItem.FLDREASON").ToString().Substring(0, 100)+ "..." : DataBinder.Eval(Container, "DataItem.FLDREASON").ToString() %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtReasonEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREASON") %>'
                                        CssClass="gridinput_mandatory" Width="200px"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtReasonAdd" runat="server" CssClass="gridinput_mandatory" Width="200px"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="UPDATE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                        CommandName="ADD" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAdd"
                                        ToolTip="Add New"></asp:ImageButton>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <br />
                <b><asp:Literal ID="lblRootCause" runat="server" Text="Root Cause"></asp:Literal></b>
                <br />
                <div style="position: relative; width: 15px">
                    <eluc:tabstrip id="MenuBasicCause" runat="server" ontabstripcommand="MenuBasicCause_TabStripCommand">
                    </eluc:tabstrip>
                </div>
                <div id="divGridBasicCause" style="position: relative; z-index: 0">
                    <asp:GridView ID="gvBasicCause" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvBasicCause_RowCommand" OnRowDataBound="gvBasicCause_ItemDataBound"
                        OnRowCancelingEdit="gvBasicCause_RowCancelingEdit" OnRowDeleting="gvBasicCause_RowDeleting"
                        OnRowCreated="gvBasicCause_RowCreated" OnRowEditing="gvBasicCause_RowEditing"
                        ShowFooter="true" ShowHeader="true" EnableViewState="false" AllowSorting="true"
                        OnSelectedIndexChanging="gvBasicCause_SelectedIndexChanging" OnRowUpdating="gvBasicCause_RowUpdating">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblBasicCauseHeader" runat="server">
                                        Main Cause
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblInsBasiccauseid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONINCIDENTBASICCAUSEID") %>'></asp:Label>
                                    <asp:Label ID="lblBasicCauseId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBASICCAUSEID") %>'></asp:Label>
                                    <asp:Label ID="lblBasicCauseName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBASICCAUSENAME") %>'
                                        Width="200px"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblInsBasiccauseidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONINCIDENTBASICCAUSEID") %>'></asp:Label>
                                    <asp:Label ID="lblBasicCauseIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBASICCAUSEID") %>'
                                        Width="200px"></asp:Label>
                                    <eluc:basiccause id="ucBasicCauseEdit" runat="server" appenddatabounditems="true"
                                        Width="200px" autopostback="true" cssclass="dropdown_mandatory" maincauselist='<%# PhoenixInspectionBasicMainCause.ListMainCause(1, null)%>'
                                        selectedmaincause='<%# DataBinder.Eval(Container,"DataItem.FLDBASICCAUSEID") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:basiccause id="ucBasicCauseAdd" runat="server" appenddatabounditems="true"
                                        Width="200px" cssclass="dropdown_mandatory" autopostback="true" maincauselist='<%# PhoenixInspectionBasicMainCause.ListMainCause(1, null) %>' />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblSubBasicCauseHeader" runat="server">
                                    Sub Cause
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSubBasicCauseId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBBASICCAUSEID") %>'></asp:Label>
                                    <asp:Label ID="lblSubBasicCause" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBCAUSENAME") %>'
                                        Width="300px"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblSubBasicCauseIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBBASICCAUSEID") %>'></asp:Label>
                                    <eluc:subbasiccause id="ucSubBasicCauseEdit" runat="server" appenddatabounditems="true"
                                        Width="300px" cssclass="dropdown_mandatory" subcauselist='<%# PhoenixInspectionBasicSubCause.ListSubCause(1, null) %>'
                                        selectedsubcause='<%# DataBinder.Eval(Container,"DataItem.FLDSUBBASICCAUSEID") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:subbasiccause id="ucSubBasicCauseAdd" runat="server" appenddatabounditems="true"
                                        Width="300px" cssclass="dropdown_mandatory" subcauselist='<%# PhoenixInspectionBasicSubCause.ListSubCause(1, null) %>' />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblBasicReasonHeader" runat="server">
                                    Reason
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblBasicReason" runat="server" Width="200px" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREASON").ToString().Length>100 ? DataBinder.Eval(Container, "DataItem.FLDREASON").ToString().Substring(0, 100)+ "..." : DataBinder.Eval(Container, "DataItem.FLDREASON").ToString() %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtBasicReasonEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREASON") %>'
                                        CssClass="gridinput_mandatory" Width="200px"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtBasicReasonAdd" runat="server" CssClass="gridinput_mandatory"
                                        Width="200px"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="UPDATE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                        CommandName="ADD" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAdd"
                                        ToolTip="Add New"></asp:ImageButton>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <eluc:split runat="server" id="ucSplit" targetcontrolid="ifMoreInfo" />
    </form>
</body>
</html>
