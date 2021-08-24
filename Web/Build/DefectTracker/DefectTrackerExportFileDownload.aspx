<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefectTrackerExportFileDownload.aspx.cs"
    Inherits="DefectTrackerExportFileDownload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Patch Release</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%= Session["sitepath"]%>/css/<%= Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%= Session["sitepath"]%>/css/<%= Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%= Session["sitepath"]%>/js/js_globals.aspx"></script>

    <script type="text/javascript" language="javascript" src="<%= Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%= Session["sitepath"]%>/js/phoenixPopup.js"></script>

</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <div>
        <div class="subHeader">
            <div id="divHeading" class="divFloatLeft">
                <eluc:Title runat="server" ID="ucTitle" Text="Documents"></eluc:Title>
            </div>
            <div style="position: absolute; top: 0px; right: 0px">
                <eluc:TabStrip ID="MenuSubmit" runat="server" OnTabStripCommand="MenuPatchRelease_TabStripCommand">
                </eluc:TabStrip>
            </div>
        </div>
        <table width="50%">
            <tr>
                <td>
                    <b>Notes :</b>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <font color="blue">
                        <li>&nbsp; 1.Click the button &nbsp;<img id="Img5" runat="server" src="<%$ PhoenixTheme:images/download_1.png%>"
                            style="vertical-align: top" />&nbsp; to download. </li>
                        <li>&nbsp; 2.Download the file to C:\.</li>
                        <li>&nbsp; 3.Extract the content of zip file and see filename with .exe extension.</li>
                        <li>&nbsp; 4.Double click on .exe file and click Unzip to import.</li>
                    </font>
                    <br />
                </td>
            </tr>
        </table>
        <eluc:Error ID="ucError" runat="server" Visible="false" />
        <asp:UpdatePanel runat="server" ID="pnlVoyageData">
            <ContentTemplate>
                <b>Data </b>
                <div id="divGrid" style="position: relative; z-index: 1">
                    <asp:GridView ID="gvDataExport" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        OnRowCommand="gvDataExport_RowCommand" Width="100%" CellPadding="3" ShowHeader="true"
                        OnRowDataBound="gvDataExport_RowDataBound" EnableViewState="false">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField HeaderText="Patch Name">
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    File Name
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDTKey" runat="server" Visible="false" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDDTKEY").ToString() %>'></asp:Label>
                                    <asp:LinkButton ID="lblFileName" CommandName="SELECT" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDEXPORTFILENAME").ToString() %>'> </asp:LinkButton>
                                    <asp:Label ID="lblhttppath" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDHTTPPATH").ToString() %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Patch Name">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                     File Size (in KB)
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblfileSize" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFILESIZE").ToString() %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="File Name">
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Uploaded Date
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblUploadeddate" runat="server" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE").ToString() %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="File Name">
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Downloaded By
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCreatedBy" runat="server" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDLASTDOWNLOADBY").ToString() %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="File Name">
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Downloaded date
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCreatedDate" runat="server" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDLASTDOWNLOAD").ToString() %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="File Name">
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Remarks
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblRemarks" runat="server" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDREMARKS").ToString() %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
                                <HeaderTemplate>
                                    Action
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkfilename" Target="_blank" runat="server" Visible="false" Height="14px">
                                    </asp:HyperLink>
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/download_1.png %>"
                                        CommandName="DOWNLOAD" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                        ToolTip="Download"></asp:ImageButton>
                                    <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" />
                                    <asp:ImageButton runat="server" AlternateText="Ship Ack." ImageUrl="<%$ PhoenixTheme:images/annexure.png %>"
                                        CommandName="HISTORY" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDownloadHistory"
                                        ToolTip="Download History"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <b>Attachment</b>
                    <asp:GridView ID="gvAttachmentExport" runat="server" AutoGenerateColumns="False"
                        Font-Size="11px" OnRowCommand="gvAttachmentExport_RowCommand" Width="100%" CellPadding="3"
                        ShowHeader="true" OnRowDataBound="gvAttachmentExport_RowDataBound" EnableViewState="false">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField HeaderText="Patch Name">
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    File Name
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDTKey" runat="server" Visible="false" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDDTKEY").ToString() %>'></asp:Label>
                                    <asp:LinkButton ID="lblFileName" CommandName="SELECT" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDEXPORTFILENAME").ToString() %>'> </asp:LinkButton>
                                    <asp:Label ID="lblhttppath" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDHTTPPATH").ToString() %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Patch Name">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    File Size (in KB)
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblfileSize" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFILESIZE").ToString() %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="File Name">
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Uploaded Date
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblUploadeddate" runat="server" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE").ToString() %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="File Name">
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Downloaded By
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCreatedBy" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDLASTDOWNLOADBY").ToString() %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="File Name">
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Downloaded date
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCreatedDate" runat="server" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDLASTDOWNLOAD").ToString() %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="File Name">
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Remarks
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblRemarks" runat="server" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDREMARKS").ToString() %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
                                <HeaderTemplate>
                                    Action
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkfilename" Target="_blank" runat="server" Visible="false" Height="14px">
                                    </asp:HyperLink>
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/download_1.png %>"
                                        CommandName="DOWNLOAD" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                        ToolTip="Download"></asp:ImageButton>
                                    <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" />
                                    <asp:ImageButton runat="server" AlternateText="Ship Ack." ImageUrl="<%$ PhoenixTheme:images/annexure.png %>"
                                        CommandName="HISTORY" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDownloadHistory"
                                        ToolTip="Download History"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <table width="50%">
            <tr>
                <td colspan="2">
                    <font color="blue">
                        <li>&nbsp; * After download the file, please specify who hand carried the CD in the
                            Download History >> Remarks column.</li>
                    </font>
                    <br />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
