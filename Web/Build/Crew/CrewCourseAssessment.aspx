<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewCourseAssessment.aspx.cs"
    Inherits="CrewCourseAssessment" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Faculty" Src="~/UserControls/UserControlFaculty.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Course Planner</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCourseAssessment" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlInspectionRecordAndResponseEntry">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:Status runat="server" ID="ucStatus" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <asp:Literal ID="lblAssessment" runat="server" Text="Assessment"></asp:Literal>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="CrewMenuCourseAssessment" runat="server" OnTabStripCommand="CrewMenuCourseAssessment_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divFind" style="position: relative; z-index: 2">
                    <table id="tblConfigureAssessment" width="100%">
                        <tr>
                            <td>
                                <asp:Literal ID="lblEmployeeName" runat="server" Text="Employee Name"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmployeeName" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="250px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblCourse" runat="server" Text="Course"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Course ID="ucCourse" runat="server" AppendDataBoundItems="true" CssClass="readonlytextbox"
                                    Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblCourseType" runat="server" Text="Course Type"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Hard runat="server" ID="ucCourseType" CssClass="readonlytextbox" AppendDataBoundItems="true"
                                    Enabled="false" HardTypeCode="103"  />
                            </td>
                            <td>
                                <asp:Literal ID="lblBatchNo" runat="server" Text="Batch No"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBatchNo" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="360px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblNoofAttempts" runat="server" Text="No of Attempts"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlNoofattempts" runat="server" AppendDataBoundItems="true"
                                    CssClass="input" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td colspan="2">
                                <asp:Literal ID="lblLastupdatedbyDate" runat="server" Text="Last updated by/Date"></asp:Literal>

                                <asp:TextBox ID="txtLastUpdatedBy" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="220px"></asp:TextBox>
                                <asp:TextBox ID="txtLastUpdatedDate" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblResult" runat="server" Text="Result"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtResult" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblRemarks" runat="server" Text="Remarks"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="input" TextMode="MultiLine"
                                    Width="360px"></asp:TextBox>
                                <asp:Image ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" ID="imgPPClip" runat="server" />
                            </td>
                        </tr>
                    </table>
                    <div class="navSelect" style="position: relative; width: 15px">
                        <eluc:TabStrip ID="MenuCrewCourseAssessment" runat="server" OnTabStripCommand="CrewCourseAssessment_TabStripCommand">
                        </eluc:TabStrip>
                    </div>
                    <div id="divGrid" style="position: relative; z-index: +1">
                        <b><asp:Literal ID="lblAcademic" runat="server" Text="Academic"></asp:Literal></b>
                        <asp:GridView ID="gvCourseAssessment" runat="server" AutoGenerateColumns="False"
                            Font-Size="11px" Width="100%" CellPadding="3" OnRowCommand="gvCourseAssessment_RowCommand"
                            OnRowCreated="gvCourseAssessment_RowCreated" OnRowDataBound="gvCourseAssessment_ItemDataBound" OnRowEditing="gvCourseAssessment_RowEditing"
                            OnRowCancelingEdit="gvCourseAssessment_RowCancelingEdit" ShowFooter="true" OnRowUpdating="gvCourseAssessment_RowUpdating"
                            ShowHeader="true" OnRowDeleting="gvCourseAssessment_RowDeleting" EnableViewState="false">
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                            <RowStyle Height="10px" />
                            <Columns>
                             <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20%"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblCode" runat="server" Text="Code"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblAssessmentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDASSESSMENTID") %>'></asp:Label>
                                        <asp:Label ID="lblCourseAssessmentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSEASSESSMENTID") %>'></asp:Label>
                                        <asp:Label ID="lblCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCODE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle HorizontalAlign="Left" Width="40%"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblTopic" runat="server" Text="Topic"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblTopic" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                  <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20%"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblMaxMarks" runat="server" Text="Max Marks"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblMaxMarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAXMARKS") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblMarks" runat="server" Text="Marks"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblMarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALUE") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Number ID="txtMarksEdit" runat="server" CssClass="input_mandatory txtNumber"
                                            MaxLength="5" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALUE") %>' IsInteger="true">
                                        </eluc:Number>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblActionHeader" runat="server">
                                                        Action
                                        </asp:Label>
                                    </HeaderTemplate> 
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="80px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                            CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                            ToolTip="Edit"></asp:ImageButton>
                                        <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                            ToolTip="Delete"></asp:ImageButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                            CommandName="Update" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                            ToolTip="Save"></asp:ImageButton>
                                        <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                            ToolTip="Cancel"></asp:ImageButton>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <br />
                    <div id="div1" style="position: relative; z-index: 0">
                        <b><asp:Literal ID="lblBehavioralAspects" runat="server" Text="Behavioral Aspects"></asp:Literal></b>
                        <asp:GridView ID="gvCrewBehavioralAspects" runat="server" AutoGenerateColumns="False"
                            Font-Size="11px" Width="100%" CellPadding="3" OnRowCommand="gvCrewBehavioralAspects_RowCommand"
                            OnRowCreated="gvCrewBehavioralAspects_RowCreated" OnRowEditing="gvCrewBehavioralAspects_RowEditing"
                            OnRowUpdating="gvCrewBehavioralAspects_RowUpdating"
                            OnRowCancelingEdit="gvCrewBehavioralAspects_RowCancelingEdit" 
                            OnRowDeleting="gvCrewBehavioralAspects_RowDeleting"
                            OnRowDataBound="gvCrewBehavioralAspects_ItemDataBound" ShowFooter="false" ShowHeader="true"
                            EnableViewState="false">
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                            <RowStyle Height="10px" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20%"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblCode" runat="server" Text="Code"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblBehavioralId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBEHAVIORALID") %>'></asp:Label>
                                        <asp:Label ID="lblBehavioralAspectsId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBEHAVIORALASPECTSID") %>'></asp:Label>
                                        <asp:Label ID="lblBehavioralCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCODE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle HorizontalAlign="Left" Width="40%"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblDescription" runat="server" Text="Description"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblBehavioralTopic" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblRating" runat="server" Text="Rating"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblBehavioralMarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALUE") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblBehavioralIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBEHAVIORALID") %>'></asp:Label>
                                        <asp:Label ID="lblBehavioralAspectsIdEdit" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBEHAVIORALASPECTSID") %>'></asp:Label>
                                        <eluc:Number ID="txtBehavioralMarksEdit" runat="server" CssClass="input_mandatory txtNumber"
                                            MaxLength="5" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALUE") %>' IsInteger="true">
                                        </eluc:Number>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblActionHeader" runat="server">
                                                        Action
                                        </asp:Label>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="80px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                            CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                            ToolTip="Edit"></asp:ImageButton>
                                        <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                            ToolTip="Delete"></asp:ImageButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                            CommandName="UPDATE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                            ToolTip="Save"></asp:ImageButton>
                                        <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="CANCEL" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                            ToolTip="Cancel"></asp:ImageButton>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="btnSignOn_Click" OKText="Yes"
                    CancelText="No" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
