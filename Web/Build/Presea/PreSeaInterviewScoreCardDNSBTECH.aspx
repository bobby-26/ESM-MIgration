<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaInterviewScoreCardDNSBTECH.aspx.cs"
    Inherits="PreSeaInterviewScoreCardDNSBTECH" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Venue" Src="~/UserControls/UserControlPreSeaExamVenue.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>AE Performance Report</title>
    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<% =Session["sitepath"]%>/css/<% =Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<% =Session["sitepath"]%>/css/<% =Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<% =Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<% =Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<% =Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript">
            function confirm(args) {
                if (args) {
                    __doPostBack("<%=confirm.UniqueID %>", "");
            }
        }
        </script>
        <script language="javascript" type="text/javascript">
            function cmdPrint_Click() {
                document.getElementById('cmdPrint').style.visibility = "hidden";
                window.print();
            }
        </script>
    </telerik:RadCodeBlock>



</head>
<body>
    <form id="frmAePerformane" runat="server" autocomplete="off">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="InterviewScoreCard_TabStripCommand" />
            <asp:Button ID="cmdHiddenConfirm" runat="server" Text="cmdHiddenConfirm" OnClick="InterviewScoreCardConfirm_TabStripCommand" />
            <asp:Button ID="confirm" runat="server" Text="confirm" OnClick="btnFinalize_Click" />
            <%-- <asp:UpdatePanel runat="server" ID="pnlPreSeaInterviewScoreCard">
                <ContentTemplate>--%>
            <input type="button" id="cmdPrint" value="Print" onclick="cmdPrint_Click();" runat="server"
                visible="false" />
            <table width="100%" cellpadding="1" cellspacing="0" style="font-family: Arial; font-size: small;">
                <tr style="font-weight: bold;">
                    <td colspan="10" align="center">SAMUNDRA INSTITUTE OF MARITIME STUDIES<br />
                        <br />
                        Interview Score Card (DNS & B.TECH Batch)
                    </td>
                </tr>
            </table>
            <hr />
            <table width="100%">
                <tr>
                    <td>Name:</td>
                    <td>
                        <asp:TextBox ID="txtCandidateName" runat="server" ReadOnly="true" Width="250px" ToolTip="Name"
                            CssClass="readonlytextbox"></asp:TextBox>
                    </td>
                    <td>DOB:</td>
                    <td>
                        <eluc:Date ID="txtDOB" runat="server" CssClass="readonlytextbox" Tooltip="DOB" Width="57px"
                            ReadOnly="true" />
                    </td>
                    <td>Entrance Roll No:</td>
                    <td>
                        <asp:TextBox ID="txtRollNo" runat="server" Width="150px" ToolTip="Roll_No" CssClass="readonlytextbox"
                            ReadOnly="true"></asp:TextBox>
                    </td>
                    <td>Examanation Center:</td>
                    <td>
                        <asp:TextBox ID="txtVenueName" runat="server" ReadOnly="true" Width="150px" ToolTip="Venue"
                            CssClass="readonlytextbox"></asp:TextBox>
                    </td>
                </tr>
            </table>

            <%-- Evaluation Item start here --%>
            <%-- Section 1 Acadamic Weightage --%>
            <b>
                <asp:Label ID="lblAcademics" runat="server" Text="Academic"></asp:Label></b>
            <asp:GridView ID="gvAcademicSection" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnRowDataBound="gvScore_RowDataBound" OnRowDeleting="gvScore_RowDeleting"
                OnRowEditing="gvScore_RowEditing" Style="margin-bottom: 0px" EnableViewState="false"
                OnRowCancelingEdit="gvScore_RowCancelingEdit" OnRowUpdating="gvScore_RowUpdating"
                OnRowCreated="gvScore_RowCreated">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                <Columns>
                    <asp:TemplateField HeaderText="S.No">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                        <ItemStyle Wrap="true" />
                        <ItemTemplate>
                            <asp:Label ID="lblFieldId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIELDID") %>'></asp:Label>
                            <asp:Label ID="lblSNo" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDFIELDSERIALNO")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Subject">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="40%"></HeaderStyle>
                        <ItemTemplate>
                            <asp:Label ID="lblFieldDesc" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDFIELDDESCRIPION")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Allot" ItemStyle-HorizontalAlign="Right">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="11%"></HeaderStyle>
                        <ItemTemplate>
                            <asp:Label ID="lblAlloted" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDALLOTEDSCORE")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:MaskNumber runat="server" ID="ucAlotted" CssClass="input_mandatory" MaxLength="4" OnTextChangedEvent="ucMax_TextChangedEvent"
                                IsInteger="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALLOTEDSCORE") %>' AutoPostBack="true" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Max" ItemStyle-HorizontalAlign="Right">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="11%"></HeaderStyle>
                        <ItemTemplate>
                            <asp:Label ID="lblMax" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDMAXLIMITFORALLOT")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:MaskNumber runat="server" ID="ucMax" CssClass="input_mandatory" MaxLength="4" OnTextChangedEvent="ucMax_TextChangedEvent"
                                IsInteger="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAXLIMITFORALLOT") %>' AutoPostBack="true" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Percentage" ItemStyle-HorizontalAlign="Right">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="11%"></HeaderStyle>
                        <ItemTemplate>
                            <asp:Label ID="lblScoredPercentage" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDSCOREDPERCENTAGE")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lblIntervieDataEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINTERVIEWDATAID") %>'></asp:Label>
                            <asp:Label ID="lblFieldIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIELDID") %>'></asp:Label>
                            <asp:Label ID="lblPercentage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCOREDPERCENTAGE") %>'></asp:Label>
                            <%-- <eluc:MaskNumber runat="server" ID="ucScredPercent" CssClass="input_mandatory" MaxLength="6" DecimalPlace="2"
                                IsInteger="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCOREDPERCENTAGE") %>' />--%>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="7%"></HeaderStyle>
                        <HeaderTemplate>
                            <asp:Label ID="lblActionHeader" runat="server"> 
                                                    Action                                           
                            </asp:Label>
                        </HeaderTemplate>
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                ToolTip="Edit"></asp:ImageButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:ImageButton runat="server" AlternateText="Save" ImageUrl='<%$ PhoenixTheme:images/save.png%>'
                                CommandName="UPDATE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                ToolTip="Save"></asp:ImageButton>
                            <img id="Img11" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png%>"
                                width="3" />
                            <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl='<%$ PhoenixTheme:images/te_del.png%>'
                                CommandName="CANCEL" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                ToolTip="Cancel"></asp:ImageButton>
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br style="clear: both;" />
            <div style="text-align: right;">
                <b>Total : </b>&nbsp;
                                <asp:Label ID="lblAcademicTotal" runat="server" Text=""></asp:Label>
                <asp:Label ID="lblAcademicMaxTotal" runat="server" Text="" Visible="false"></asp:Label>
            </div>

            <%-- Section 2 WRITTEN TEST --%>
            <b>
                <asp:Label ID="lblWritten" runat="server" Text="Written"></asp:Label></b>
            <asp:GridView ID="gvWrittenSection" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnRowDataBound="gvScore_RowDataBound" OnRowDeleting="gvScore_RowDeleting"
                OnRowEditing="gvScore_RowEditing" Style="margin-bottom: 0px" EnableViewState="false"
                OnRowCancelingEdit="gvScore_RowCancelingEdit" OnRowUpdating="gvScore_RowUpdating"
                OnRowCreated="gvScore_RowCreated">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                <Columns>
                    <asp:TemplateField HeaderText="S.No">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                        <ItemStyle Wrap="true" />
                        <ItemTemplate>
                            <asp:Label ID="lblFieldId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIELDID") %>'></asp:Label>
                            <asp:Label ID="lblSNo" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDFIELDSERIALNO")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Subject">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="40%"></HeaderStyle>
                        <ItemTemplate>
                            <asp:Label ID="lblFieldDesc" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDFIELDDESCRIPION")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Allot" ItemStyle-HorizontalAlign="Right">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20%"></HeaderStyle>
                        <ItemTemplate>
                            <asp:Label ID="lblAlloted" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDALLOTEDSCORE")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:MaskNumber runat="server" ID="ucAlotted" CssClass="input_mandatory" MaxLength="3"
                                IsInteger="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALLOTEDSCORE") %>' />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Max" ItemStyle-HorizontalAlign="Right">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20%"></HeaderStyle>
                        <ItemTemplate>
                            <asp:Label ID="lblMax" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDMAXLIMITFORALLOT")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lblIntervieDataEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINTERVIEWDATAID") %>'></asp:Label>
                            <asp:Label ID="lblFieldIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIELDID") %>'></asp:Label>
                            <eluc:MaskNumber runat="server" ID="ucMax" CssClass="input_mandatory" MaxLength="3"
                                IsInteger="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAXLIMITFORALLOT") %>' />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                        <HeaderTemplate>
                            <asp:Label ID="lblActionHeader" runat="server">    Action                                           
                            </asp:Label>
                        </HeaderTemplate>
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl='<%$ PhoenixTheme:images/te_edit.png%>'
                                CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                ToolTip="Edit"></asp:ImageButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:ImageButton runat="server" AlternateText="Save" ImageUrl='<%$ PhoenixTheme:images/save.png%>'
                                CommandName="Update" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                ToolTip="Save"></asp:ImageButton>
                            <img id="Img11" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png%>"
                                width="3" />
                            <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl='<%$ PhoenixTheme:images/te_del.png%>'
                                CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                ToolTip="Cancel"></asp:ImageButton>
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br style="clear: both;" />
            <div style="text-align: right;">
                <b>Total : </b>&nbsp;
                                <asp:Label ID="lblWrittenTotal" runat="server" Text=""></asp:Label>
                <asp:Label ID="lblWrittenMaxTotal" runat="server" Text="" Visible="false"></asp:Label>
            </div>

            <%--Personality, Proficiency & Behaviour section--%>
            <b>
                <asp:Label ID="lblPersonality" runat="server" Text="Personality & Proficiency"></asp:Label></b>
            <asp:GridView ID="gvGeneralSection" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnRowDataBound="gvScore_RowDataBound" OnRowDeleting="gvScore_RowDeleting"
                OnRowEditing="gvScore_RowEditing" Style="margin-bottom: 0px" EnableViewState="false"
                OnRowCancelingEdit="gvScore_RowCancelingEdit" OnRowUpdating="gvScore_RowUpdating"
                OnRowCreated="gvScore_RowCreated">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                <Columns>
                    <asp:TemplateField HeaderText="S.No">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                        <ItemStyle Wrap="true" />
                        <ItemTemplate>
                            <asp:Label ID="lblFieldId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIELDID") %>'></asp:Label>
                            <asp:Label ID="lblSNo" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDFIELDSERIALNO")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Criteria">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="40%"></HeaderStyle>
                        <ItemTemplate>
                            <asp:Label ID="lblFieldDesc" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDFIELDDESCRIPION")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Allot" ItemStyle-HorizontalAlign="Right">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20%"></HeaderStyle>
                        <ItemTemplate>
                            <asp:Label ID="lblAlloted" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDALLOTEDSCORE")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:MaskNumber runat="server" ID="ucAlotted" CssClass="input_mandatory" MaxLength="3"
                                IsInteger="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALLOTEDSCORE") %>' />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Max" ItemStyle-HorizontalAlign="Right">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20%"></HeaderStyle>
                        <ItemTemplate>
                            <asp:Label ID="lblMax" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDMAXLIMITFORALLOT")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lblIntervieDataEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINTERVIEWDATAID") %>'></asp:Label>
                            <asp:Label ID="lblFieldIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIELDID") %>'></asp:Label>
                            <eluc:MaskNumber runat="server" ID="ucMax" CssClass="input_mandatory" MaxLength="3"
                                IsInteger="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAXLIMITFORALLOT") %>' />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                        <HeaderTemplate>
                            <asp:Label ID="lblActionHeader" runat="server"> 
                                                    Action                                           
                            </asp:Label>
                        </HeaderTemplate>
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl='<%$ PhoenixTheme:images/te_edit.png%>'
                                CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                ToolTip="Edit"></asp:ImageButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:ImageButton runat="server" AlternateText="Save" ImageUrl='<%$ PhoenixTheme:images/save.png%>'
                                CommandName="Update" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                ToolTip="Save"></asp:ImageButton>
                            <img id="Img11" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png%>"
                                width="3" />
                            <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl='<%$ PhoenixTheme:images/te_del.png%>'
                                CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                ToolTip="Cancel"></asp:ImageButton>
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br style="clear: both;" />
            <div style="text-align: right;">
                <b>Total : </b>&nbsp;
                                <asp:Label ID="lblGeneralTotal" runat="server" Text=""></asp:Label>
                <asp:Label ID="lblGeneralMaxTotal" runat="server" Text="" Visible="false"></asp:Label>
            </div>
            <br />

            <table cellspacing="1" width="100%">
                <tr>
                    <td>Total / Out Off</td>
                    <td>
                        <asp:TextBox ID="txtFinalTotal" runat="server" CssClass="readonlytextbox" Width="100px"></asp:TextBox>
                        <asp:Literal ID="ltrdash" Text="/" runat="server"></asp:Literal>
                        <asp:TextBox ID="txtOverallTotal" runat="server" Text="" CssClass="readonlytextbox" Width="100px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Literal ID="Literal1" runat="server" Text="Percentage"></asp:Literal></td>
                    <td>
                        <asp:TextBox ID="txtAvg" runat="server" CssClass="readonlytextbox" Width="200px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>16 PF Results
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl16PFResult" runat="server" DataTextField="FLDQUICKNAME"
                            DataValueField="FLDQUICKCODE" CssClass="dropdown_mandatory" Width="200px">
                        </asp:DropDownList>
                    </td>
                    <td>16 PF Remarks</td>
                    <td>
                        <asp:TextBox ID="txt16PFRemarks" runat="server" CssClass="input" TextMode="MultiLine"
                            Width="200px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Marks Compiled by
                    </td>
                    <td>
                        <asp:TextBox ID="txtMarkCompiledBy" runat="server" CssClass="input_mandatory" Width="200px"></asp:TextBox>
                    </td>
                    <td>Interviewed by 1
                    </td>
                    <td>
                        <asp:TextBox ID="txtInterviewBy1" runat="server" CssClass="input_mandatory" Width="200px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Interviewed by 2
                    </td>
                    <td>
                        <asp:TextBox ID="txtInterviewBy2" runat="server" CssClass="input" Width="200px"></asp:TextBox>
                    </td>
                    <td colspan="2">
                        <asp:RadioButtonList ID="rdoRecommend" runat="server" RepeatColumns="2" TextAlign="Right"
                            ToolTip="Result">
                            <asp:ListItem Value="1" Text="&nbsp;Recommended&nbsp;&nbsp;"></asp:ListItem>
                            <asp:ListItem Value="0" Text="&nbsp;Not Recommended&nbsp;"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
            </table>

            <table width="100%" style="color: Blue; font-size: small; font-weight: 100;">
                <tr>
                    <td>Note :</td>
                </tr>
                <tr>
                    <td>&nbsp;&nbsp;                      
                        1.The candidate should be a graduate in one of the following streams  PHYSICS, CHEMISTRY &
                            MATHEMATICS,
                            <br />
                        &nbsp;&nbsp;&nbsp;if he is a graduate in any other stream then consider his percentage marks in HSC .Check
                            if he meets the other criteria.
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;&nbsp;2.Checked documents :- 10th / 12th / PCM / DOB / Photographs / Roll No-12th Exam / Exam Call Letter/ Medical Fitness Certificate / 16 PF / ………………………
                    </td>
                </tr>
            </table>
            <br />
            <eluc:Status ID="ucStatus" runat="server" Visible="false" />
            <eluc:Confirm ID="ucFinalize" runat="server" OnConfirmMesage="btnFinalize_Click"
                OKText="Yes" CancelText="No" Visible="false" />
        </div>
    </form>
</body>
</html>
