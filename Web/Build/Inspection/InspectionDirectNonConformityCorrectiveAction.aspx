<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionDirectNonConformityCorrectiveAction.aspx.cs" Inherits="InspectionDirectNonConformityCorrectiveAction" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Incident CAR</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="divInspectionNonConformity" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmDirectNonConformityCA" runat="server">
    
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    
    <asp:UpdatePanel runat="server" ID="pnlInspectionDirectNonConformityCA">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text="" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <asp:Literal ID="lblCorrectiveAction" runat="server" Text="Corrective Action"></asp:Literal>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuCARGeneral" runat="server" OnTabStripCommand="MenuCARGeneral_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <br />
                <asp:GridView ID="gvCorrectiveAction" runat="server" AutoGenerateColumns="False"
                    Font-Size="11px" Width="100%" CellPadding="3" OnRowCommand="gvCorrectiveAction_RowCommand"
                    OnRowDataBound="gvCorrectiveAction_ItemDataBound" OnRowCancelingEdit="gvCorrectiveAction_RowCancelingEdit"
                    OnRowDeleting="gvCorrectiveAction_RowDeleting" OnRowCreated="gvCorrectiveAction_RowCreated"
                    OnRowEditing="gvCorrectiveAction_RowEditing" ShowFooter="true" ShowHeader="true"
                    EnableViewState="false" AllowSorting="true" OnSelectedIndexChanging="gvCorrectiveAction_SelectedIndexChanging"
                    OnRowUpdating="gvCorrectiveAction_RowUpdating">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                    <RowStyle Height="10px" />
                    <Columns>
                        <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                        <asp:TemplateField>
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="350px"></ItemStyle>
                            <FooterStyle Wrap="true" HorizontalAlign="Left" Width="350px" />
                            <HeaderTemplate>
                                <asp:Label ID="lblCorrectiveActionHeader" runat="server">
                                    Corrective Action
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblInsCorrectActid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONCORRECTIVEACTIONID") %>'></asp:Label>
                                <asp:Label ID="lblCorrectiveAction" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCORRECTIVEACTION").ToString().Length>320 ? DataBinder.Eval(Container, "DataItem.FLDCORRECTIVEACTION").ToString().Substring(0, 160) +"</BR>"+ DataBinder.Eval(Container, "DataItem.FLDCORRECTIVEACTION").ToString().Substring(161, 160)+ "..." : DataBinder.Eval(Container, "DataItem.FLDCORRECTIVEACTION").ToString() %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblInsCorrectActidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONCORRECTIVEACTIONID") %>'></asp:Label>
                                <asp:TextBox ID="txtCorrectActEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCORRECTIVEACTION") %>'
                                    CssClass="gridinput_mandatory" MaxLength="500" TextMode="MultiLine" Rows="2"></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtCorrectActAdd" runat="server" CssClass="gridinput_mandatory"
                                    MaxLength="500" TextMode="MultiLine" Rows="2"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="120px"></ItemStyle>
                            <FooterStyle Wrap="true" HorizontalAlign="Left" Width="120px" />
                            <HeaderTemplate>
                                <asp:Label ID="lblTargetDateHeader" runat="server">
                                                    Target Date
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblTargetDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDTARGETDATE")) %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="ucTargetDateEdit" CssClass="input" runat="server" Enabled="false"
                                    Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDTARGETDATE")) %>' DatePicker="true" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date ID="ucTargetDateAdd" CssClass="input_mandatory" runat="server" 
                                    DatePicker="true" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="120px"></ItemStyle>
                            <FooterStyle Wrap="true" HorizontalAlign="Left" Width="120px" />
                            <HeaderTemplate>
                                <asp:Label ID="lblCompletionDateHeader" runat="server">
                                                    Completion Date
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCompletionDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOMPLETIONDATE")) %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="ucCompletionDateEdit" runat="server" CssClass="input" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOMPLETIONDATE")) %>'
                                    DatePicker="true" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="70px"></ItemStyle>
                            <FooterStyle Wrap="true" HorizontalAlign="Left" Width="70px" />
                            <HeaderTemplate>
                                <asp:Label ID="lblExtensionRequiredHeader" runat="server">
                                                    Reschedule Required
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkExtensionRequired" runat="server" Checked="false" Enabled="false" />
                            </ItemTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkExtensionRequiredEdit" runat="server" AutoPostBack="true" OnCheckedChanged="chkExtensionRequiredEdit_checkedChanged" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="300px"></ItemStyle>
                            <FooterStyle Wrap="true" HorizontalAlign="Left" Width="200px" />
                            <HeaderTemplate>
                                <asp:Label ID="lblExtensionReasonHeader" runat="server">
                                                    Reschedule Reason
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblExtensionReason" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXTENSIONREASON") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtExtensionReasonEdit" Width="300px" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXTENSIONREASON") %>'
                                    CssClass="input" TextMode="MultiLine" Enabled="false" MaxLength="500"></asp:TextBox>
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
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                                <img id="Img3" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Reschedule Reason" ImageUrl="<%$ PhoenixTheme:images/te_view.png %>"
                                    CommandName="RescheduleReason" CommandArgument='<%# Container.DataItemIndex %>'
                                    ID="cmdReschedule" ToolTip="Reschedule Reason"></asp:ImageButton>
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
                <br />
                <asp:GridView ID="gvPreventiveAction" runat="server" AutoGenerateColumns="False"
                    Font-Size="11px" Width="100%" CellPadding="3" OnRowCommand="gvPreventiveAction_RowCommand"
                    OnRowDataBound="gvPreventiveAction_ItemDataBound" OnRowCancelingEdit="gvPreventiveAction_RowCancelingEdit"
                    OnRowDeleting="gvPreventiveAction_RowDeleting" OnRowCreated="gvPreventiveAction_RowCreated"
                    OnRowEditing="gvPreventiveAction_RowEditing" ShowFooter="true" ShowHeader="true"
                    EnableViewState="false" AllowSorting="true" OnSelectedIndexChanging="gvPreventiveAction_SelectedIndexChanging"
                    OnRowUpdating="gvPreventiveAction_RowUpdating">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                    <RowStyle Height="10px" />
                    <Columns>
                        <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                        <asp:TemplateField>
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="350px"></ItemStyle>
                            <FooterStyle Wrap="true" HorizontalAlign="Left" Width="350px" />
                            <HeaderTemplate>
                                <asp:Label ID="lblPreventiveActionHeader" runat="server">
                                                    Preventive Action
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblInsPreventActid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONPREVENTIVEACTIONID") %>'></asp:Label>
                                <asp:Label ID="lblPreventiveAction" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPREVENTIVEACTION")%>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblInsPreventActidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONPREVENTIVEACTIONID") %>'></asp:Label>
                                <asp:TextBox ID="txtPreventActEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPREVENTIVEACTION") %>'
                                    CssClass="gridinput_mandatory"  MaxLength="500" TextMode="MultiLine" Rows="2"></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtPreventActAdd" runat="server" CssClass="gridinput_mandatory" 
                                    MaxLength="500" TextMode="MultiLine" Rows="2"></asp:TextBox>                                    
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="120px"></ItemStyle>
                                <FooterStyle Wrap="true" HorizontalAlign="Left" Width="120px" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblPATargetDateHeader" runat="server">
                                                    Target Date
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPATargetDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDTARGETDATE")) %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date ID="ucPATargetDateEdit" CssClass="input" runat="server" Enabled="false"
                                        Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDTARGETDATE")) %>' DatePicker="true" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Date ID="ucPATargetDateAdd" CssClass="input_mandatory" runat="server" 
                                        DatePicker="true" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="120px"></ItemStyle>
                                <FooterStyle Wrap="true" HorizontalAlign="Left" Width="120px" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblPACompletionDateHeader" runat="server">
                                                    Completion Date
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPACompletionDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOMPLETIONDATE")) %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date ID="ucPACompletionDateEdit" runat="server" CssClass="input" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOMPLETIONDATE")) %>'
                                        DatePicker="true" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="70px"></ItemStyle>
                                <FooterStyle Wrap="False" HorizontalAlign="Left" Width="70px" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblPAExtensionRequiredHeader" runat="server">
                                                    Reschedule Required
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkPAExtensionRequired" runat="server" Checked="false" Enabled="false" />
                                </ItemTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkPAExtensionRequiredEdit" runat="server" AutoPostBack="true" OnCheckedChanged="chkPAExtensionRequiredEdit_checkedChanged" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="300px"></ItemStyle>
                                <FooterStyle Wrap="true" HorizontalAlign="Left" Width="200px" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblPAExtensionReasonHeader" runat="server">
                                                    Reschedule Reason
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPAExtensionReason" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXTENSIONREASON") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtPAExtensionReasonEdit" runat="server" Width="300px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXTENSIONREASON") %>'
                                        CssClass="input" TextMode="MultiLine" Enabled="false" MaxLength="500"></asp:TextBox>
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
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                                <img id="Img3" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Reschedule Reason" ImageUrl="<%$ PhoenixTheme:images/te_view.png %>"
                                    CommandName="RescheduleReason" CommandArgument='<%# Container.DataItemIndex %>'
                                    ID="cmdReschedule" ToolTip="Reschedule Reason"></asp:ImageButton>
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
                <br />
                <asp:GridView ID="gvFollowUpRemarks" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" OnRowCommand="gvFollowUpRemarks_RowCommand" OnRowDataBound="gvFollowUpRemarks_ItemDataBound"
                    OnRowCancelingEdit="gvFollowUpRemarks_RowCancelingEdit" OnRowDeleting="gvFollowUpRemarks_RowDeleting"
                    OnRowCreated="gvFollowUpRemarks_RowCreated" OnRowEditing="gvFollowUpRemarks_RowEditing"
                    ShowFooter="true" ShowHeader="true" EnableViewState="false" AllowSorting="true"
                    OnSelectedIndexChanging="gvFollowUpRemarks_SelectedIndexChanging" OnRowUpdating="gvFollowUpRemarks_RowUpdating">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                    <RowStyle Height="10px" />
                    <Columns>
                        <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                        <asp:TemplateField>
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="980px"></ItemStyle>
                                <FooterStyle Wrap="true" HorizontalAlign="Left" Width="980px" />
                            <HeaderTemplate>
                                <asp:Label ID="lblFollowUpHeader" runat="server">
                                                        Follow Up Remarks
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblInsFollowupid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONFOLLOWUPID") %>'></asp:Label>
                                <asp:Label ID="lblFollowUpRemarks" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFOLLOWUPREMARKS")%>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblInsFollowupidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONFOLLOWUPID") %>'></asp:Label>
                                <asp:TextBox ID="txtFollowUpEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFOLLOWUPREMARKS") %>'
                                        CssClass="gridinput_mandatory" MaxLength="500" TextMode="MultiLine" Rows="2"></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtFollowUpAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="500"
                                        TextMode="MultiLine" Rows="2"></asp:TextBox>
                            </FooterTemplate>
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
                <br />
                <div id="verification" runat="server">
                <table id="tblInspectionVerification" width="100%">
                    <tr>
                        <td valign="baseline" style="width: 12%">
                            <b><asp:literal ID="lblVerificationLevel" runat="server" Text="Verification Level"></asp:literal></b>
                        </td>
                        <td style="width: 25%" valign="baseline">
                            <eluc:hard id="ucVerficationLevel" runat="server" appenddatabounditems="true" cssclass="input_mandatory"
                                hardtypecode="195" />
                        </td>
                        <td valign="baseline" style="width: 20%">
                            <b><asp:Literal ID="lblVerificationbySuperintendent" runat="server" Text="Verification by Superintendent"></asp:Literal></b>
                        </td>
                        <td valign="baseline" style="width: 30%">
                            <span id="spnPickListSupt">
                                <asp:TextBox ID="txtSuptName" runat="server" CssClass="input_mandatory" Enabled="false" MaxLength="200"
                                    Width="35%"></asp:TextBox>
                                <asp:TextBox ID="txtSuptDesignation" runat="server" CssClass="input_mandatory" Enabled="false"
                                    MaxLength="50" Width="25%"></asp:TextBox>
                                <asp:ImageButton runat="server" ID="imgShowSupt" Style="cursor: pointer; vertical-align: top"
                                    ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" />
                                <asp:TextBox runat="server" ID="txtSupt" Text="" CssClass="input"></asp:TextBox>
                                <asp:TextBox runat="server" ID="txtSuptEmailHidden" Text="" CssClass="input" Width="10px"></asp:TextBox>
                            </span>
                        </td>
                        <td valign="baseline" style="width: 20%">
                            <b><asp:Literal ID="lblVerificationDate" runat="server" Text="Verification Date"></asp:Literal></b>
                        </td>
                        <td valign="baseline" style="width: 30%">
                            <eluc:Date runat="server" ID="txtVerificationDate" CssClass="input_mandatory" DatePicker="true" />
                        </td>
                    </tr>
                </table>
               </div>
            </div> 
            <eluc:Confirm ID="ucConfirmComplete" runat="server" OnConfirmMesage="btnComplete_Click"
                OKText="Yes" CancelText="No" />          
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gvCorrectiveAction" />
        </Triggers>
    </asp:UpdatePanel>         
    <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />  
    </form>
</body>
</html>
