<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaBatchTimeSlot.aspx.cs"
    Inherits="PreSeaBatchTimeSlot" %>

<%@ Import Namespace="SouthNests.Phoenix.PreSea" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PreSeaBatch" Src="~/UserControls/UserControlPreSeaBatch.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlPreSeaCourse.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Pre Sea Time Slot</title>
    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPreSeaExam" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlPreSeaRoom">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Time slot" ShowMenu="false"></eluc:Title>
                    </div>
                </div>
                <div style="top: 0px; right: 0px; position: absolute">                   
                </div>
                <div id="divSearch">
                    <table id="tblSearch" cellpadding="2" cellspacing="2" width="50%">
                        <tr>
                            <td>
                                <asp:Literal ID="lblCourse" runat="server" Text="Course"></asp:Literal> 
                            </td>
                            <td>
                                <eluc:Course ID="ddlCourse" runat="server" AppendDataBoundItems="true" CssClass="readonlytextbox"  />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblBatch" runat="server" Text="Batch"></asp:Literal>                            
                            </td>
                            <td>
                                <eluc:PreSeaBatch ID="ucBatch" runat="server" CssClass="readonlytextbox" AppendDataBoundItems="true"  />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblSemester" runat="server" Text="Semester"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSemester" runat="server" AppendDataBoundItems="true" Width="120px" AutoPostBack="true"
                                    CssClass="input">                                    
                                    <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                    <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                    <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                    <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                    <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuPreSeaExam" runat="server" OnTabStripCommand="PreSeaExam_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <asp:GridView ID="gvExamDetails" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" OnRowCancelingEdit="gvExamDetails_RowCancelingEdit"
                    OnRowCommand="gvExamDetails_RowCommand" OnRowDataBound="gvExamDetails_RowDataBound"
                    OnRowDeleting="gvExamDetails_RowDeleting" OnRowEditing="gvExamDetails_RowEditing"
                    OnRowUpdating="gvExamDetails_RowUpdating" ShowFooter="true" EnableViewState="false">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <Columns>
                        <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                        <asp:TemplateField HeaderText="S.No.">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                S.No.
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblSerialNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></asp:Label>
                                <asp:Label ID="lblTimeSlotId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTIMESLOTID") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>                       
                        <asp:TemplateField HeaderText="Start Time">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Start Time
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblStartTime" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTARTTIME") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                            <eluc:Number ID="txtStartTimeEdit" runat="server" CssClass="input_mandatory" 
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTARTTIME") %>' />
                                <%--<asp:TextBox ID="txtStartTimeEdit" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTARTTIME") %>'
                                    Width="50px" />
                                <ajaxToolkit:MaskedEditExtender ID="txtStartTimeMask" runat="server" AcceptAMPM="false"
                                    ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99" MaskType="Time"
                                    TargetControlID="txtStartTimeEdit" UserTimeFormat="TwentyFourHour" />--%>
                            </EditItemTemplate>
                            <FooterTemplate>
                            <eluc:Number ID="txtStartTimeAdd" runat="server" CssClass="input_mandatory" Mask="99.99" />
                                <%--<asp:TextBox ID="txtStartTimeAdd" runat="server" CssClass="input_mandatory" Width="50px" />
                                <ajaxToolkit:MaskedEditExtender ID="txtStartTimeAddMask" runat="server" AcceptAMPM="false"
                                    ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99" MaskType="Time"
                                    TargetControlID="txtStartTimeAdd" UserTimeFormat="TwentyFourHour" />--%>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="End Time">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                End Time
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblEndTime" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENDTIME") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                             <eluc:Number ID="txtEndTimeEdit" runat="server" CssClass="input_mandatory" Mask="99.99"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDENDTIME") %>' />
                               <%-- <asp:TextBox ID="txtEndTimeEdit" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENDTIME") %>'
                                    Width="50px" />--%>
                               <%-- <ajaxToolkit:MaskedEditExtender ID="txtEndTimeMask" runat="server" AcceptAMPM="false"
                                    ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99" MaskType="Time"
                                    TargetControlID="txtEndTimeEdit" UserTimeFormat="TwentyFourHour" />--%>
                            </EditItemTemplate>
                            <FooterTemplate>
                              <eluc:Number ID="txtEndTimeAdd" runat="server" CssClass="input_mandatory" Mask="99.99" />
                               <%-- <asp:TextBox ID="txtEndTimeAdd" runat="server" CssClass="input_mandatory" Width="50px" />
                                <ajaxToolkit:MaskedEditExtender ID="txtEndTimeAddMask" runat="server" AcceptAMPM="false"
                                    ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99" MaskType="Time"
                                    TargetControlID="txtEndTimeAdd" UserTimeFormat="TwentyFourHour" />--%>
                            </FooterTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Break
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblIsBrkYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBREAK") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkIsBrkEdit" runat="server" Checked='<%# DataBinder.Eval(Container,"DataItem.FLDISBRKYN").ToString().Equals("1")? true: false %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:CheckBox ID="chkBrkAdd" runat="server" Checked="false" />
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
                                    ToolTip="Edit" Visible="false"></asp:ImageButton>
                                <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" visible="false" />
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>                                
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="SAVE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                    ToolTip="Save" Visible="false"></asp:ImageButton>
                                <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="CANCEL" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                    ToolTip="Cancel" Visible="false"></asp:ImageButton>
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
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
