<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaNewApplicantQueryActivity.aspx.cs"
    Inherits="PreSeaNewApplicantQueryActivity" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmMessage" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Applicant</title>
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
            gv.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = chkAll.checked;
        }
     }
     function chkAllVenue(chkAllVenue)
     {
        var gv = document.getElementById("<%=gvPreSeaSearch.ClientID %>");
        for(i = 1;i < gv.rows.length; i++)
        {
            gv.rows[i].cells[7].getElementsByTagName("INPUT")[0].checked = chkAll.checked;
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
                    <eluc:Title runat="server" ID="Title1" Text="Applicant" ShowMenu="<%# Title1.ShowMenu %>">
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
                            Note : To send letter to applicants please select Course in Filter. </br>
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
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Center" Wrap="False" Width="60px" />
                                <HeaderStyle HorizontalAlign="Left" Wrap="true" />
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkAll" runat="server" Text="All&nbsp;&nbsp;" TextAlign="Left"
                                        onclick="CheckAll(this)" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkItem" runat="server" />
                                    <asp:Label ID="lblEmpId" runat="server" Text='<%# Bind("FLDEMPLOYEEID") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblscorecardid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCORECARDID") %>'></asp:Label>
                                    <asp:Label ID="lblExamPlanId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENTRANCEEXAMPLANID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Name">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblFirstnameHeader" runat="server">Name                           
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblEmployeeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></asp:Label>
                                    <asp:Label ID="lblBatchId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPLIEDBATCH") %>'></asp:Label>
                                    <asp:LinkButton ID="lnkEployeeName" runat="server" CommandArgument="<%#Container.DataItemIndex%>"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIRSTNAME").ToString()+ " "+ DataBinder.Eval(Container,"DataItem.FLDMIDDLENAME").ToString()+" "+ DataBinder.Eval(Container,"DataItem.FLDLASTNAME").ToString()%>'
                                        CommandName="GETEMPLOYEE"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date Of Birth">
                                <HeaderTemplate>
                                    Roll No (Entrance)
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblRollNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENTRANCEROLLNO") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date Of Birth">
                                <HeaderTemplate>
                                    Date of Birth
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDateOfBirth" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFBIRTH","{0:dd/MM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Course Name">
                                <HeaderStyle Width="15%" />
                                <HeaderTemplate>
                                    Course
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCourseName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSENAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Batch Name">      
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>                          
                                <HeaderTemplate>
                                    Batch
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblBatchName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCHNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>    
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>                            
                                <HeaderTemplate>
                                    Venue Choice 1
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblVenueId1" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMVENUE1") %>'></asp:Label>
                                    <asp:Label ID="lblBaAppliedVenue1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMVENUENAME1") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField> 
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>                               
                                <HeaderTemplate>
                                    <asp:Label ID="lblVenueChoice2" runat="server" Text="Venue Choice 2"></asp:Label>
                                    <asp:CheckBox ID="chkAllVenue" runat="server" Text="Venue Choice 2&nbsp;&nbsp;" TextAlign="left"
                                        onclick="chkAllVenue(this)" Visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblVenueId2" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMVENUE2") %>'></asp:Label>
                                    <asp:CheckBox ID="chkChoose2nd" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMVENUENAME2") %>'
                                        TextAlign="Right" runat="server" />
                                    <asp:Label ID="lblBaAppliedVenue2" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMVENUENAME2") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>  
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>                            
                                <HeaderTemplate>
                                    Called Venue
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCalledVenueId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVENUEID") %>'></asp:Label>
                                    <asp:Label ID="lblCalledVenueName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMVENUENAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>                                
                                <HeaderTemplate>
                                    Status
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    Action
                                </HeaderTemplate>
                                 <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="GETEMPLOYEE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="txt" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="2" />
                                    <asp:ImageButton ID="imgReceipt" Visible="false" runat="server" ImageUrl="<%$ PhoenixTheme:images/requisition.png %>"
                                        CommandArgument="<%#Container.DataItemIndex%>" ToolTip="Receipt" CommandName="RECEIPT" />
                                    <img id="Img4" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="2" />
                                    <asp:ImageButton ID="imgInterview" runat="server" ImageUrl="<%$ PhoenixTheme:images/72.png %>"
                                        CommandArgument="<%#Container.DataItemIndex%>" ToolTip="Entrance Exam Interviews" CommandName="INTERVIEW" />
                                    <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton ID="imgConfirm" runat="server" ImageUrl="<%$ PhoenixTheme:images/crew-suitability-check.png %>"
                                        CommandArgument="<%#Container.DataItemIndex%>" ToolTip="Confirm/Reject" CommandName="CONFIRM" />
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
