<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaNewApplicantSendCallLetter.aspx.cs"
    Inherits="PreSeaNewApplicantSendCallLetter" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PreSeaBatch" Src="~/UserControls/UserControlPreSeaBatch.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PreSeaExamVenue" Src="~/UserControls/UserControlPreSeaExamVenue.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmMessage" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PreSea New Applicant Query Activity</title>
    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

        <script type="text/javascript">
         function CheckAll(chkAll)
         {
            var gv = document.getElementById("<%=gvPreSeaSearch.ClientID %>");
            for(i = 1;i < gv.rows.length; i++)
            {
                gv.rows[i].cells[5].getElementsByTagName("INPUT")[0].checked = chkAll.checked;
            }
         }
         function CheckItem(chkChoose2nd)
         {
            var gv = document.getElementById("<%=gvPreSeaSearch.ClientID %>");
            
            if(!chkChoose2nd.checked)
            {
              gv.rows[0].cells[5].getElementsByTagName("INPUT")[0].checked =false;
            }
         }

        </script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPreSeaNewApplicant" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlPreSeaNewApplicant">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:UserControlStatus ID="ucStatus" runat="server" />
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="Title1" Text="Send Call letter to Appilcants" ShowMenu="<%# Title1.ShowMenu %>">
                    </eluc:Title>
                    <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="PreSeaQuery" runat="server" OnTabStripCommand="PreSeaQuery_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <table style="width: 100%">
                    <tr>
                        <td colspan="6" style="color: Blue;">
                            1. By default call letter will be generated for First Choice exam venue.
                            <br />
                            2. For Choosing Second choice check the check box next to "Venue choice 2"
                            <br />
                            3. While Generating call letter, choose 'yes' if the call letter finalised one,
                            otherwise select 'no' (preview purpose).
                            <br />
                            If yes, records will be saved based on venue choice, which selected.
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" style="font-weight: bold;">
                            Applied Exam Venue
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Batch Applied
                        </td>
                        <td>
                            <eluc:PreSeaBatch ID="ucBatch" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" IsOutside="false" />
                        </td>
                        <td>
                            1st Choice
                        </td>
                        <td>
                            <eluc:PreSeaExamVenue ID="ucExamVenue1" runat="server" AppendDataBoundItems="true"
                                CssClass="input" />
                        </td>
                        <td>
                            2nd Choice
                        </td>
                        <td>
                            <eluc:PreSeaExamVenue ID="ucExamVenue2" runat="server" AppendDataBoundItems="true"
                                CssClass="input" />
                        </td>
                    </tr>
                </table>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="PreSeaQueryMenu" runat="server" OnTabStripCommand="PreSeaQueryMenu_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                    <asp:GridView ID="gvPreSeaSearch" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvPreSeaSearch_RowCommand" OnRowDataBound="gvPreSeaSearch_RowDataBound"
                        ShowHeader="true" EnableViewState="false" AllowSorting="true" OnSorting="gvPreSeaSearch_Sorting">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField HeaderText="Name">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderStyle Width="20%" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblFirstnameHeader" runat="server">Name
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblEmployeeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></asp:Label>
                                    <asp:LinkButton ID="lnkEployeeName" runat="server" CommandArgument="<%#Container.DataItemIndex%>"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIRSTNAME").ToString()+ " "+ DataBinder.Eval(Container,"DataItem.FLDMIDDLENAME").ToString()+" "+ DataBinder.Eval(Container,"DataItem.FLDLASTNAME").ToString()%>'
                                        CommandName="GETEMPLOYEE"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date Of Birth">
                                <HeaderStyle Width="10%" />
                                <HeaderTemplate>
                                    Date of Birth
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDateOfBirth" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFBIRTH","{0:dd/MM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Course Name">
                                <HeaderStyle Width="20%" />
                                <HeaderTemplate>
                                    Course Name
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCourseName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSENAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Batch Name">
                                <HeaderStyle Width="20%" />
                                <HeaderTemplate>
                                    Batch Name
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblBatchName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCHNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle Width="12%" />
                                <HeaderTemplate>
                                    Venue Choice 1
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblVenueId1" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMVENUE1") %>'></asp:Label>
                                    <asp:Label ID="lblBaAppliedVenue1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMVENUENAME1") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle Width="12%" VerticalAlign="Middle" />
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkAll" runat="server" Text="Venue Choice 2&nbsp;&nbsp;" TextAlign="left" onclick="CheckAll(this)" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblVenueId2" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMVENUE2") %>'></asp:Label>
                                    <asp:Label ID="lblBaAppliedVenue2" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMVENUENAME2") %>'></asp:Label>
                                    <asp:CheckBox ID="chkChoose2nd" Text="&nbsp;&nbsp;" TextAlign="Right" runat="server" onclick="CheckItem(this)" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="6%"></HeaderStyle>
                                <HeaderTemplate>
                                    Action
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <table width="100%" border="0" class="datagrid_pagestyle">
                    <tr>
                        <td nowrap align="center">
                            <asp:Label ID="lblPagenumber" runat="server">
                            </asp:Label>
                            <asp:Label ID="lblPages" runat="server">
                            </asp:Label>
                            <asp:Label ID="lblRecords" runat="server">
                            </asp:Label>&nbsp;&nbsp;
                        </td>
                        <td nowrap align="left" width="50px">
                            <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                        </td>
                        <td width="20px">
                            &nbsp;
                        </td>
                        <td nowrap align="right" width="50px">
                            <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                        </td>
                        <td nowrap align="center">
                            <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                            </asp:TextBox>
                            <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                Width="40px"></asp:Button>
                        </td>
                    </tr>
                </table>
            </div>
            <eluc:ConfirmMessage ID="ucConfirm" runat="server" Text="" OnConfirmMesage="ucConfirm_OnClick"
                Visible="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
