<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefectTrackerPatchByVessel.aspx.cs"
    Inherits="DefectTrackerPatchByVessel" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPVessel" Src="~/UserControls/UserControlVessel.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Patch List</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

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
                        <eluc:Title runat="server" ID="ucTitle" Text="Patch Report" ShowMenu="true"></eluc:Title>
                    </div>
                </div>
                <table width="100%">
                    <tr>
                        <td>
                            Vessel Name
                        </td>
                        <td>
                            <eluc:SEPVessel ID="ucVessel" runat="server" AppendDataBoundItems="true" CssClass="input" />
                        </td>
                        <td>
                            Patch Type
                        </td>
                        <td>
                            <asp:CheckBoxList ID="chklstPatchType" runat="server" RepeatDirection="Horizontal"
                                OnTextChanged="chklst_TextChanged">
                                <asp:ListItem Value="DATAEXTRACT" Text="Data Extract"></asp:ListItem>
                                <asp:ListItem Value="HOTFIX" Text="Hot Fix"></asp:ListItem>
                                <asp:ListItem Value="PATCH" Text="Patch"></asp:ListItem>
                            </asp:CheckBoxList>
                        </td>
                        <td>
                            Not Acknowledged
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsAcknowledged" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            File name
                        </td>
                        <td>
                            <asp:TextBox ID="txtFilename" runat="server" CssClass="input" />
                        </td>
                        <td>
                            Sent between
                        </td>
                        <td>
                            <eluc:Date ID="ucFromSentDate" runat="server" CssClass="input" />
                            <eluc:Date ID="ucToSentDate" runat="server" CssClass="input" />
                        </td>
                        <td>
                            Created by
                        </td>
                        <td>
                            <asp:TextBox ID="txtCreatedBy" runat="server" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td>                                                    
                            Subject
                        </td>
                        <td>
                            <asp:TextBox ID="txtSubject" runat="server" CssClass="input">
                            </asp:TextBox>
                        </td>
                        <td>
                            Acknowledge between
                        </td>
                        <td>
                            <eluc:Date ID="ucFromAcknowledge" runat="server" CssClass="input" />
                            <eluc:Date ID="ucToAcknowledge" runat="server" CssClass="input" />
                        </td>
                       <td>
                            Project Title
                        </td>
                        <td>
                            <asp:TextBox ID="txtProjectTitle" runat="server" CssClass="input" MaxLength=200>
                            </asp:TextBox>                            
                        </td>                        
                    </tr>
                </table>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuDefectTracker" runat="server" OnTabStripCommand="MenuPatchReport_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0">
                    <asp:GridView ID="gvPatch" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        OnRowCreated="gvPatch_RowCreated" Width="100%" CellPadding="1" OnRowCommand="gvPatch_RowCommand"
                        OnRowDataBound="gvPatch_ItemDataBound" OnRowDeleting="gvPatch_RowDeleting" OnRowUpdating="gvPatch_RowUpdating"
                        OnRowEditing="gvPatch_RowEditing" ShowHeader="true" EnableViewState="false" AllowSorting="true"
                        OnSorting="gvPatch_Sorting">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField HeaderText="Vessel Name">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Vessel Name
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label id="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></asp:Label> 
                                    <asp:Label ID="lblVesselID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></asp:Label>
                                    <asp:Label ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="File Name">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    File Name
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkfilename" Target="_blank" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENAME") %>'
                                        runat="server" ToolTip="Download File">
                                    </asp:HyperLink>
                                    <asp:Label ID="lblFileName" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Subject">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Subject
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lnkSubject" runat="server" CommandName="SELECT" CommandArgument='<%# Container.DataItemIndex %>'
                                        Text='<%# DataBinder.Eval(Container, "DataItem.FLDSUBJECT").ToString().Length > 50 ? DataBinder.Eval(Container, "DataItem.FLDSUBJECT").ToString().Substring(0, 50) + "..." : DataBinder.Eval(Container, "DataItem.FLDSUBJECT").ToString()%>'>
                                    </asp:Label>
                                    <eluc:Tooltip ID="uclblSubject" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECT") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Created By">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Created by
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCreatedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPATCHCREATEDBY") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Created Date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Created date
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCreatedDate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCREATEDDATE", "{0:dd/MM/yyyy}")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date Sent On">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Sent on
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDateSenton" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSENTDATE", "{0:dd/MM/yyyy}")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Subject">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Acknowledged on
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDateAcknowledged" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDACKNOWLEDGEDON", "{0:dd/MM/yyyy}")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <HeaderTemplate>
                                    Action
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Sent Mail." ImageUrl="<%$ PhoenixTheme:images/48.png %>"
                                        CommandName="SENDEMAIL" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSendemail"
                                        ToolTip="Send Reminder"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <table width="100%" border="0" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap="nowrap" align="center">
                                <asp:Label ID="lblPagenumber" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblPages" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblRecords" runat="server">
                                </asp:Label>&nbsp;&nbsp;
                            </td>
                            <td nowrap="nowrap" align="left" width="50px">
                                <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap="nowrap" align="right" width="50px">
                                <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                            </td>
                            <td nowrap="nowrap" align="center">
                                <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                </asp:TextBox>
                                <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                    Width="40px"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
