<%@ Page Language="C#" AutoEventWireup="True" CodeFile="PreSeaTraineeMapping.aspx.cs"
    Inherits="PreSeaTraineeMapping" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PopupMenu" Src="~/UserControls/UserControlPopupMenu.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="../UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CurrentBatch" Src="../UserControls/UserControlPreSeaCurrentBatch.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Batch</title>
   <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
  </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPreSeaBatch" runat="server" autocomplete="off">
    <div id='divMoveable' style="position: absolute; visibility: hidden; border-color: Black;
        border-style: solid;">
    </div>
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlBatchEntry">
        <ContentTemplate>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Roll No Allocation" />
                    </div>
                </div>
                 <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="PreSeaQuery" runat="server" TabStrip="true" OnTabStripCommand="PreSeaQuery_TabStripCommand">
                    </eluc:TabStrip>
                </div>   
                 <div class="subHeader" style="position: relative;">
                    <span class="navSelect" style="margin-top: 0px; float: right; width: auto;">
                        <eluc:TabStrip ID="PreSeaSub" runat="server" OnTabStripCommand="PreSeaSub_TabStripCommand">
                        </eluc:TabStrip>
                    </span>
                </div>
                     
                <div id="divFind" style="position: relative; z-index: 2">
                    <table id="tblConfigureBatch" width="100%">
                        <tr>
                            <td>
                                Batch
                            </td>
                            <td>
                            <eluc:CurrentBatch ID="ddlBatch" runat="server" CssClass="input"  AppendDataBoundItems="true" AutoPostBack="true" OnTextChangedEvent="OnBatchChanged" />
                              
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuPreSeaBatch" runat="server" OnTabStripCommand="PreSeaBatch_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0">
                    <asp:GridView ID="gvCB" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowDataBound="gvCB_RowDataBound" OnRowUpdating="gvCB_RowUpdating"
                        EnableViewState="false" OnRowCancelingEdit="gvCB_RowCancelingEdit" 
                        OnRowDeleting="gvCB_RowDeleting" OnRowEditing="gvCB_RowEditing" OnRowCommand="gvCB_RowCommand"
                        ShowHeader="true" ShowFooter="false">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Sl no
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDROWID")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Roll Number
                                </HeaderTemplate>
                                <ItemTemplate>
                                  <asp:Label ID="lblTraineeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAINEEID") %>'></asp:Label>
                                    <asp:Label ID="lblPreSeaTraineeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRESEATRAINEEID") %>'></asp:Label>
                                    <asp:Label ID="lblBatch" runat="server"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDROLLNUMBER") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Trainee Name
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAINEENAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                  <asp:Label ID="lblIMUrollno" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIMUENTRANCEROLLNO") %>'></asp:Label>
                                 <asp:Label ID="lblIsIMUApplicable" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISIMUAPPLICABLE") %>'></asp:Label>
                                 <asp:Label ID="lblRollnumberEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROLLNUMBER") %>'></asp:Label>
                                 <asp:Label ID="lblPreSeaTraineeIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRESEATRAINEEID") %>'></asp:Label>
                                    <span id="spnPickListEmployeeAdd" runat="server">
                                        <asp:TextBox ID="txtEmployeeNameAdd" runat="server" CssClass="input_mandatory" Width="80%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAINEENAME") %>'></asp:TextBox>
                                        <asp:ImageButton ID="btnShowEmployeeAdd" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                            ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataItemIndex %>" />
                                        <asp:TextBox ID="txtEmployeeIdAdd" runat="server" Width="0px" CssClass="hidden"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAINEEID") %>' ></asp:TextBox>
                                    </span>
                                </EditItemTemplate>
                            </asp:TemplateField>
                               <asp:TemplateField>
                                <HeaderTemplate>
                                    IMU Entrance roll no
                                </HeaderTemplate>
                                   <ItemTemplate>
                                       <%# DataBinder.Eval(Container, "DataItem.FLDIMUENTRANCEROLLNO")%>
                                   </ItemTemplate>
                                   <EditItemTemplate>
                                       <asp:TextBox ID="txtIMURollnoEdit" runat="server" CssClass="gridinput" MaxLength="50" Text='<%# DataBinder.Eval(Container, "DataItem.FLDIMUENTRANCEROLLNO")%>'></asp:TextBox>
                                   </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Created date
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# string.Format("{0:dd/MMM/yyyy}", DataBinder.Eval(Container, "DataItem.FLDCREATEDDATE"))%>
                               
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    Action
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl='<%$ PhoenixTheme:images/te_del.png%>'
                                        CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                    <img id="Img3" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Handover" ImageUrl="<%$ PhoenixTheme:images/checklist.png %>"
                                        CommandName="HANDOVER" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdHandover"
                                        ToolTip="Admission Hand Over"></asp:ImageButton>
                                </ItemTemplate>
                             <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="Update" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
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
              <eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="ConfirmBatch" OKText="Ok"
                CancelText="Cancel" Visible="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
