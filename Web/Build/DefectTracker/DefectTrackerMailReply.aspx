<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefectTrackerMailReply.aspx.cs"
    EnableEventValidation="false" ValidateRequest="false" Inherits="DefectTracker_DefectTrackerMailReply" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="SEPModule" Src="~/UserControls/UserControlSEPbugModuleList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPIncident" Src="~/UserControls/UserControlSEPIncidentList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SearchPatch" Src="../UserControls/UserControlSearchPatch.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>MailManager</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript">
            //<!--
            function HandleKeyDown(obj, e) {
                var KeyCode = (e.which) ? e.which : e.keyCode
                var target = (e.srcElement) ? e.srcElement : e.target;
                var tabKeyCode = 9;

                if (KeyCode == tabKeyCode && target == obj) {
                    if (e.which) {
                        var l = obj.value.substring(obj.selectionStart, 1) + '      '
                        obj.value = obj.value.substring(obj.selectionStart, 1) + '      ' + obj.value.substring(obj.selectionEnd);
                        o.setSelectionRange(l.length, l.length);
                        obj.focus();
                        e.preventDefault();
                    }
                    else {
                        obj.selection = document.selection.createRange();
                        obj.selection.text = String.fromCharCode(tabKeyCode);
                        event.returnValue = false;
                    }
                    return false;
                }
                return true;
            }
            function SelectSingleRadiobutton(rdbtnid) {
                var rdBtn = document.getElementById(rdbtnid);
                var rdBtnList = document.getElementsByTagName("input");
                for (i = 0; i < rdBtnList.length; i++) {
                    if (rdBtnList[i].type == "radio" && rdBtnList[i].id != rdBtn.id) {
                        rdBtnList[i].checked = false;
                    }
                }
            }
            //-->
        </script>
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="subHeader">
        <div id="divHeading" class="divFloatLeft">
            <eluc:Title runat="server" ID="ucTitle" Text="Reply" ShowMenu="false"></eluc:Title>
        </div>
        <div style="position: absolute; right: 0px">
            <eluc:TabStrip ID="MenuMailReply" runat="server" OnTabStripCommand="MenuMailReply_TabStripCommand">
            </eluc:TabStrip>
        </div>
    </div>    
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
    <table width="100%" border="1">
        <tr>
            <td width="40%">
                <table width="100%">
                    <tr>
                        <td width="10%">
                            From
                        </td>
                        <td width="90%">
                            <asp:TextBox ID="txtFrom" runat="server" ReadOnly="true" CssClass="input" Width="90%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            To
                        </td>
                        <td>
                            <asp:TextBox ID="txtTo" runat="server" CssClass="input" Width="90%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Cc
                        </td>
                        <td>
                            <asp:TextBox ID="txtCc" runat="server" CssClass="input" Width="90%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Subject
                        </td>
                        <td>
                            <asp:TextBox ID="txtSubject" runat="server" CssClass="input" Width="90%"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
            <td width="40%">
                Incident List<br />
                <div id="divIncidentGrid" style="position: relative; z-index: 0; width: 100%; height: 200px;
                    overflow: auto;">
                    <asp:GridView ID="gvSEPIncidentList" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        AllowSorting="true" Width="100%" CellPadding="3" ShowHeader="false" EnableViewState="false"
                        OnRowDataBound="gvSEPIncidentList_RowDataBound" RowStyle-VerticalAlign="Bottom"
                        OnRowCreated="gvSEPIncidentList_RowCreated" TabIndex="1">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:RadioButton ID="rbsepincident" runat="server" OnClick="javascript:SelectSingleRadiobutton(this.id)"
                                        OnCheckedChanged="Incident_Changed" AutoPostBack="true" />
                                    <asp:Label ID="lblincidentid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'
                                        Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="lblincident" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINCIDENT") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td>
                <font color="blue" size="0"><b>Attachments</b>
                    <li>Browse and select the attachment and click on upload</li>
                </font>
            </td>
            <td>
                <asp:FileUpload runat="server" ID="txtBugAttachment" CssClass="input" />
                <asp:ImageButton runat="server" ID="cmdUpload" ImageUrl="<%$ PhoenixTheme:images/upload.png %>"
                    ToolTip="Upload" OnClick="cmdUpload_Click" />
                <br />
                <eluc:SearchPatch ID="ucSearchPatch" OnTextChangedEvent="ddl_TextChanged" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:GridView ID="gvAttachment" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" OnRowDataBound="gvAttachment_RowDataBound"
                    OnRowCommand="gvAttachment_RowCommand" OnRowDeleting="gvAttachment_RowDeleting">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />
                    <Columns>
                        <asp:TemplateField HeaderText="File Name">
                            <ItemStyle HorizontalAlign="Left" Width="30%"></ItemStyle>
                            <HeaderTemplate>
                                File Name
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblDTKey" runat="server" Visible="false" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDDTKEY").ToString() %>'></asp:Label>
                                <asp:Label ID="lblFileName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFILENAME").ToString() %>'> </asp:Label>
                                <asp:Label ID="lblFilePath" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDMESSAGEID").ToString() %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:HyperLink ID="lnkfilename" Target="_blank" Text="View" runat="server" Width="14px"
                                    Height="14px" ToolTip="Download File">
                                </asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td colspan="2" width="100%">
                <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" Rows="18" Width="100%"
                    CssClass="input" Font-Size="Small" onkeydown="HandleKeyDown(this, event)"></asp:TextBox>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
