<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefectTrackerPatchMailMap.aspx.cs"
    Inherits="DefectTrackerPatchMailMap" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPVessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPModule" Src="~/UserControls/UserControlSEPbugModuleList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPMailUsername" Src="~/UserControls/UserControlSEPMailUsername.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPIncident" Src="~/UserControls/UserControlSEPIncidentList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>MailCallLog</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body onresize="ResizeGridViewHeader('MailCallLog');">
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="True">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlMailCallLog">
        <ContentTemplate>
            <div class="subHeader">
                <div id="div1" style="vertical-align: top">
                    <eluc:Title runat="server" ID="Title1" Text="Mail Logs" ShowMenu="false"></eluc:Title>
                </div>
                <div style="position: absolute; top: 0px; right: 0px">
                    <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                </div>
            </div>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div id="divFind" style="margin-top: 0px;">
                <table width="75%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblCallNumber" runat="server">Call Number</asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCallNumber" runat="server" CssClass="input" />
                        </td>
                         <td>
                            <asp:Literal ID="lblvesselmailsearch" runat="server">Mail Search</asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtvesselmailsearch" runat="server" CssClass="input" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="navSelect" style="position: relative; width: 15px">
                <eluc:TabStrip ID="MenuMailCallLog" runat="server" OnTabStripCommand="MenuMailCallLog_TabStripCommand">
                </eluc:TabStrip>
            </div>
            <div id="divGrid" class="navigation" style="top: 75px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <asp:GridView ID="MailCallLog" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" OnRowDataBound="MailCallLog_ItemDataBound" OnRowCommand="MailCallLog_RowCommand"
                    OnRowDeleting="MailCallLog_RowDeleting"
                    ShowFooter="True" EnableViewState="False" AllowSorting="True" OnRowCreated="MailCallLog_RowCreated">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <Columns>
                        <asp:TemplateField HeaderText="From">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="15%"></ItemStyle>
                            <HeaderTemplate>
                               <asp:Label ID="lblFromHeader" runat="server">
                                    From
                               </asp:Label> 
                            </HeaderTemplate>
                            <ItemTemplate> 
                                <asp:Label ID="lblUniqueID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAILINOUTID")  %>'
                                    Visible="false"></asp:Label>
                                <asp:Label ID="lblMessageId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMESSAGEID") %>'
                                    Visible="false"></asp:Label>
                                <asp:Label ID="lblitemfrom" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFROM").ToString().Length>30 ? DataBinder.Eval(Container, "DataItem.FLDFROM").ToString().Substring(0, 30)+ "..." : DataBinder.Eval(Container, "DataItem.FLDFROM").ToString() %>'></asp:Label>
                                     <eluc:ToolTip ID="ucitemfrom" runat="server"  Text='<%# DataBinder.Eval(Container, "DataItem.FLDFROM")%>'  /> 
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="From">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="10%"></ItemStyle>
                            <HeaderTemplate>
                                 <asp:Label ID="lblToHeader" runat="server" >
                                  To
                                 </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblitemto" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTO").ToString().Length>20 ? DataBinder.Eval(Container, "DataItem.FLDTO").ToString().Substring(0, 20)+ "..." : DataBinder.Eval(Container, "DataItem.FLDTO").ToString()%>'></asp:Label>
                                    <eluc:ToolTip ID="ucitemto" runat="server"  Text='<%# DataBinder.Eval(Container, "DataItem.FLDTO")%>'  />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Type">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" ></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblSubjecHeader" runat="server">
                                Subject
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkSubject" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECT") %>'></asp:LinkButton>
                                <eluc:Tooltip runat="server" ID="ucSubject" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECT") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Module">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblDateHeader" runat="server">
                                Date
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDSENTRECEIVEDON")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Module">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblsrheader" runat="server">
                                S/R
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDMAPPED")%>
                            </ItemTemplate>
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
                                <asp:ImageButton ID="cmdAttachment" runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                    CommandName="ATTACHMENT" CommandArgument='<%# Container.DataItemIndex %>' ToolTip="Attachment">
                                </asp:ImageButton>
                                <img id="Attachment" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img id="Img3" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Ship Ack." ImageUrl="<%$ PhoenixTheme:images/annexure.png %>"
                                    CommandName="MAPMAIL" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdMapMail"
                                    ToolTip="Map Mail"></asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
