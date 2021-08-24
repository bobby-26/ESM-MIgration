<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefectTrackerScriptAttachment.aspx.cs" EnableEventValidation="false"
    Inherits="DefectTrackerScriptAttachment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
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
    <div>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader">
            <div id="div1" style="vertical-align: top">
                <eluc:Title runat="server" ID="Title1" Text="Script Tracker"></eluc:Title>
            </div>
            <div style="position: absolute; top: 0px; right: 0px">
                <eluc:TabStrip ID="MenuBugAttachment" runat="server"></eluc:TabStrip>
                 <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            </div>
        </div>
       <asp:UpdatePanel runat="server" ID="pnlScriptAttachment">
            <ContentTemplate>
            <eluc:Error ID="Error1" runat="server" Text="" Visible="false"></eluc:Error>
            <br />
            <table width="75%">
                <tr>
                    <td>
                       <asp:Literal ID="lblSubject" runat="server" Text="Subject" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtSubjectSearch" Width="360px" CssClass="input" runat="server">
                        </asp:TextBox>
                    </td>
                    <td>
                        <asp:Literal ID="lblModule" runat="server" Text="Module" />
                    </td>
                    <td>
                        <eluc:Module ID="ucModuleSearch" runat="server" CssClass="input" AppendDataBoundItems="true" />
                    </td>
                 </tr>
             </table>
             <br />
             <div class="navSelect" style="position: relative; width: 15px">
                <eluc:TabStrip ID="DefectTrackerScriptAdd" OnTabStripCommand="DefectTrackerScriptSearch_TabStripCommand" runat="server"></eluc:TabStrip>
            </div>
            <div id="divGrid" style="position: relative; z-index: 0;  width: 100%;">
                    <asp:GridView ID="gvAttachment" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" OnRowDataBound="gvAttachment_RowDataBound" OnRowCreated="gvAttachment_RowCreated">
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
                                <ItemStyle HorizontalAlign="Left" ></ItemStyle>
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
                                    <asp:label ID="lblOriginalFilenameHeader" runat="server">Original filename</asp:label>
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
                                        Height="14px" ToolTip="Download File">
                                    </asp:HyperLink>
                                    <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
