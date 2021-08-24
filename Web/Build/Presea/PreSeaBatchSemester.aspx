<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaBatchSemester.aspx.cs"
    Inherits="PreSeaBatchSemester" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.PreSea" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PreSeaBatch" Src="~/UserControls/UserControlPreSeaBatch.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlPreSeaCourse.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PreSea Semester</title>
    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlNTBRManager">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false">
                </eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="Title1" Text="Batch" ShowMenu="false">
                    </eluc:Title>
                </div>
                  <div class="subHeader" style="position: relative">
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                        <eluc:TabStrip ID="MenuPreSea" runat="server" OnTabStripCommand="MenuPreSea_TabStripCommand">
                        </eluc:TabStrip>
                    </div>
                </div>
                <div style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuBatchPlanner" runat="server" OnTabStripCommand="BatchPlanner_TabStripCommand"
                        TabStrip="true">
                    </eluc:TabStrip>
                </div>
                <br style="clear: both;" />
                <div id="divMain" runat="server" style="width: 100%;">
                    <table width="80%" cellpadding="1" cellspacing="1">
                        <tr valign="top">
                            <td>
                                Course Name
                            </td>
                            <td>
                                <eluc:Course ID="ddlCourse" runat="server" AppendDataBoundItems="true" CssClass="readonlytextbox" />
                            </td>
                            <td>
                                Batch
                            </td>
                            <td>
                                <eluc:PreSeaBatch ID="ucBatch" runat="server" CssClass="readonlytextbox" AppendDataBoundItems="true" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0">
                    <asp:GridView ID="gvBatchSemesters" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCancelingEdit="gvBatchSemesters_RowCancelingEdit"
                        OnRowCommand="gvBatchSemesters_RowCommand" OnRowDataBound="gvBatchSemesters_RowDataBound"
                        OnRowDeleting="gvBatchSemesters_RowDeleting" OnRowEditing="gvBatchSemesters_RowEditing"     
                        nSelectedIndexChanging="gvBatchSemesters_SelectedIndexChanging"                   
                        OnRowUpdating="gvBatchSemesters_RowUpdating" ShowFooter="true" EnableViewState="true">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                             <asp:TemplateField HeaderText="Semester Short Code">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Semester Code
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSemesterCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEMESTERCODE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtSemesterCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEMESTERCODE") %>'
                                        MaxLength="10" CssClass="input_mandatory"> </asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtSemesterCodeAdd" runat="server" CssClass="input_mandatory" MaxLength="10"> </asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Semester Name
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblBatchSemId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCHSEMID") %>'></asp:Label>
                                    <asp:Label ID="lblSemesterId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEMESTERID") %>'></asp:Label>
                                    <asp:Label ID="lblSemesterName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEMESTERNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblBatchSemIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCHSEMID") %>'></asp:Label>
                                    <asp:Label ID="lblSemesterIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEMESTERID") %>'></asp:Label>
                                    <asp:TextBox ID="txtSemesterNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEMESTERNAME") %>'
                                        CssClass="input_mandatory" MaxLength="200" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtSemesterNameAdd" runat="server" CssClass="input_mandatory" MaxLength="200" />                                     
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    No of weeks
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblNoofWeeks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOOFWEEKS") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="txtNoofWeeksEdit" runat="server" CssClass="input_mandatory" Mask="99" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOOFWEEKS") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Number ID="txtNoofWeeksAdd" runat="server" CssClass="input_mandatory" Mask="99" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Start date
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblStartdate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTARTDATE","{0:dd/MMM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date ID="txtStartdateEdit" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTARTDATE","{0:dd/MMM/yyyy}") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Date ID="txtStartdateAdd" runat="server" CssClass="input_mandatory" />
                                </FooterTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Order Sequence">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    Order Sequence
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblOrderSequence" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERSEQUENCE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtOrderSequenceEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERSEQUENCE") %>'
                                        CssClass="gridinput" MaxLength="200" Style="text-align: right;"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender3" runat="server" TargetControlID="txtOrderSequenceEdit"
                                        Mask="99" MaskType="Number" InputDirection="RightToLeft">
                                    </ajaxToolkit:MaskedEditExtender>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtOrderSequenceAdd" runat="server" CssClass="gridinput" MaxLength="200"
                                        Style="text-align: right;"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender4" runat="server" TargetControlID="txtOrderSequenceAdd"
                                        Mask="99" MaskType="Number" InputDirection="RightToLeft">
                                    </ajaxToolkit:MaskedEditExtender>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    FeedBack Available
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblFeedBackAvailable" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFEEDBACKAVAILABLEYESNO") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkFeedbackAvaialableEdit" runat="server" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:CheckBox ID="chkFeedbackAvailableAdd" runat="server" />
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
    <%--<eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />--%>
    </form>
</body>
</html>
