<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewLicenceRequestCRADetails.aspx.cs"
    Inherits="CrewLicenceRequestCRADetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CRA Details</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewCRADetails" runat="server">
    <ajaxtoolkit:toolkitscriptmanager id="ToolkitScriptManager1" runat="server" combinescripts="false">
    </ajaxtoolkit:toolkitscriptmanager>
    <asp:UpdatePanel runat="server" ID="pnlCRA">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:error id="ucError" runat="server" text="" visible="false"></eluc:error>
                <eluc:status id="ucStatus" runat="server" />
                <div class="subHeader" style="position: relative">
                    <div id="div2" style="vertical-align: top">
                        <eluc:title runat="server" id="Title1" text="CRA Details" showmenu="false"></eluc:title>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:tabstrip id="CrewLicReq" runat="server" ontabstripcommand="CrewLicReq_TabStripCommand"
                        tabstrip="true">
                    </eluc:tabstrip>
                </div>
                <div class="subHeader">
                    <div class="divFloat" style="clear: right">
                        <eluc:tabstrip id="MenuCRA" runat="server" ontabstripcommand="MenuCRA_TabStripCommand">
                    </eluc:tabstrip>
                    </div>
                </div>
                <div>
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Literal ID="lblEmployeeName" runat="server" Text="Name"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox runat="server" Width="220px" ID="txtEmployeeName" CssClass="readonlytextbox"
                                    ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblEmployeeNumber" runat="server" Text="File No"></asp:Literal>
                            </td>
                            <td style="width: 30px">
                                <asp:TextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblLicence" runat="server" Text="Licence"></asp:Literal>
                            </td>
                            <td colspan="2">
                                <asp:TextBox runat="server" ID="txtLicence" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="220px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblIssueDateofCRA" runat="server" Text="Issued"></asp:Literal>
                            </td>
                            <td>
                                <eluc:date id="txtFlagStateProcessDate" runat="server" cssclass="input_mandatory" />
                            </td>
                            <td>
                                <asp:Literal ID="lblCRANumber" runat="server" Text="CRA No"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCRANumber" runat="server" CssClass="input_mandatory"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblExpiryDateofCRA" runat="server" Text="Expiry"></asp:Literal>
                            </td>
                            <td>
                                <eluc:date id="txtCRAExpiryDate" runat="server" cssclass="input_mandatory" />
                            </td>
                            <td>
                                <asp:Literal ID="lblHandOverComments" runat="server" Text="Hand Over Comments"></asp:Literal>
                            </td>
                            <td align="left" style="vertical-align: top;">
                                <asp:TextBox ID="txtHandOverComments" runat="server" CssClass="input_mandatory" Height="49px"
                                    TextMode="MultiLine" Width="250px"></asp:TextBox>
                            </td>
                        </tr>
                         <tr>
                            <td>
                                <asp:Literal ID="lblRemarks" runat="server" Text="Remarks"></asp:Literal>
                            </td>
                            <td align="left" style="vertical-align: top;">
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="input_mandatory" Height="49px"
                                    TextMode="MultiLine" Width="250px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="navSelect" id="divTab1" runat="server" style="position: relative; width: 15px">
                    <eluc:tabstrip id="MenuShowExcel" runat="server" ontabstripcommand="CrewShowExcel_TabStripCommand">
                    </eluc:tabstrip>
                </div>
                <asp:GridView runat="server" ID="gvCRA" AutoGenerateColumns="false" Width="100%" GridLines="None"
                    OnRowCommand="gvCRA_RowCommand" ShowFooter="false" ShowHeader="true" EnableViewState="false"
                    OnRowCancelingEdit="gvCRA_RowCancelingEdit" OnRowEditing="gvCRA_RowEditing" OnRowUpdating="gvCRA_RowUpdating"
                    OnRowDataBound="gvCRA_RowDataBound">
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                    <RowStyle Height="10px" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="100px"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblCRAHandedOverby" runat="server" Text="Handed Over by"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDCRAHANDEDOVERBYNAME")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="350px"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblComments" runat="server" Text="Comments"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblHandOverComments" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDHANDOVERREMARKS").ToString().Length>30 ? DataBinder.Eval(Container, "DataItem.FLDHANDOVERREMARKS").ToString().Substring(0, 30) + "..." : DataBinder.Eval(Container, "DataItem.FLDHANDOVERREMARKS").ToString() %>'></asp:Label>
                                <eluc:tooltip id="ucHandOverCommentsTT" runat="server" text='<%# DataBinder.Eval(Container,"DataItem.FLDHANDOVERREMARKS") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="80px"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblDate" runat="server" Text="Date"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDCRAHANDOVERDATE")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="true" HorizontalAlign="center" Width="50px"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblReceived" runat="server" Text="Received"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDISCRARECEIVEDNAME")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkReceivedYN" runat="server" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="350px"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblComments" runat="server" Text="Comments"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblReceivedComments" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCRARECEIVEDBYCOMMENTS").ToString().Length>30 ? DataBinder.Eval(Container, "DataItem.FLDCRARECEIVEDBYCOMMENTS").ToString().Substring(0, 30) + "..." : DataBinder.Eval(Container, "DataItem.FLDCRARECEIVEDBYCOMMENTS").ToString() %>'></asp:Label>
                                <eluc:tooltip id="ucReceivedCommentsTT" runat="server" text='<%# DataBinder.Eval(Container,"DataItem.FLDCRARECEIVEDBYCOMMENTS") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtReceivedComments" runat="server" Width="350px" Height="30px"
                                    CssClass="gridinput_mandatory" TextMode="MultiLine">
                                </asp:TextBox>
                                <asp:Label ID="lblRequestStatusId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTSTATUSID") %>'></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="100px"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblCRAReceivedby" runat="server" Text="Received by"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDCRARECEIVEDBYNAME")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="80px"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblDate" runat="server" Text="Date"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDCRARECEIVEDDATE")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="70px"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblAction" runat="server" Text="Action"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="Update" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
               <%-- <eluc:split runat="server" id="ucSplit" targetcontrolid="ifMoreInfo" />--%>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
