<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefectTrackerIssueScriptAttachment.aspx.cs"
    Inherits="DefectTracker_DefectTrackerIssueScriptAttachment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Module" Src="~/UserControls/UserControlSEPModuleList.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Bug Edit</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div runat="server" id="DivHeader">
        <link rel="stylesheet" type="text/css" href="<%= Session["sitepath"]%>/css/<%= Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%= Session["sitepath"]%>/css/<%= Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%= Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%= Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%= Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlMailManager">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="subHeader">
                <div id="divHeading" class="divFloatLeft">
                    <eluc:Title runat="server" ID="ucTitle" Text="Script Tracker" Visible="false" ShowMenu="false"></eluc:Title>
                </div>
            </div>
            <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                <asp:GridView ID="gvAttachment" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" OnRowDataBound="gvAttachment_RowDataBound"
                    OnRowCreated="gvAttachment_RowCreated">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" BorderColor="#FF0066" />
                    <RowStyle Height="10px" />
                    <Columns>
                        <asp:TemplateField HeaderText="File Name">
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblFileNameHeader" runat="server">File Name</asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblDTKey" runat="server" Visible="false" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDDTKEY").ToString() %>'></asp:Label>
                                <asp:Label ID="lblFileName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFILENAME").ToString() %>'> </asp:Label>
                                <asp:Label ID="lblFilePath" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFILEPATH").ToString() %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="File Name">
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblModuleHeader" runat="server">Module Name</asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblModule" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMODULENAME").ToString()%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="File Name">
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblSubjectHeader" runat="server">Subject</asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblSubject" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSUBJECT").ToString().Length > 60 ? DataBinder.Eval(Container, "DataItem.FLDSUBJECT").ToString().Substring(0, 60) + "..." : DataBinder.Eval(Container, "DataItem.FLDSUBJECT").ToString()%>'>
                                        Visible="true"></asp:Label>
                                <eluc:Tooltip ID="uclblSubject" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECT") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="File Name">
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblCreatedByHeader" runat="server">Created By</asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCreatedBy" runat="server" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDSCRIPTCREATEDBY").ToString() %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="File Name">
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblCreatedDateHeader" runat="server">Created Date</asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCreatedDate" runat="server" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE").ToString() %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="File Name">
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblDeployedOnHeader" runat="server">Deployed on</asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblDeployedon" runat="server" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDDEPLOYON").ToString().Length > 15 ? DataBinder.Eval(Container, "DataItem.FLDDEPLOYON").ToString().Substring(0, 15) + "..." : DataBinder.Eval(Container, "DataItem.FLDDEPLOYON").ToString() %>'></asp:Label>
                                <eluc:Tooltip ID="uclbldeployedon" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPLOYON") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="File Name">
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblOriginalFilenameHeader" runat="server">Original filename</asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblOriginalfilename" runat="server" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDORGINALFILENAME").ToString() %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblActionHeader" runat="server">Action</asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:HyperLink ID="lnkfilename" Target="_blank" Text="View" runat="server" Width="14px"
                                    Height="14px">
                                </asp:HyperLink>
                                <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
