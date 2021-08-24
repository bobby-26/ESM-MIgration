<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsUserAccessList.aspx.cs" Inherits="AccountsUserAccessList" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="User" Src="~/UserControls/UserControlUserName.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
     <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
             <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvUserAccessList.ClientID %>"));
                }, 200);
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <div style="top: 100px; margin-left: auto; margin-right: auto; vertical-align: middle;">
         <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel runat="server" ID="pnlUserAccessEntry" UpdateMode="Conditional">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
<%--                         <eluc:Title runat="server" ID="ucTitle" Text="User Account Access Mapping" ShowMenu="false" />--%>
                         <asp:Button runat="server" ID="cmdHiddenPick" OnClick="cmdHiddenPick_Click" />
                <br />
                <tr>
                    <td width="15%">
                        <telerik:RadLabel ID="lblAccountUserId" runat="server" Text="Account User Name"></telerik:RadLabel>
                    </td>
                    <td width="15%" colspan="2">
                        <span id="spnPickListMaker">
                            <telerik:RadTextBox ID="txtDesignation" runat="server" CssClass="input" ReadOnly="false"
                                Width="180px"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="lblEmail" runat="server" CssClass="input" ReadOnly="false" Width="1px"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtUserCode" runat="server" CssClass="input" ReadOnly="false" Width="1px"></telerik:RadTextBox>
                            <img id="ImgAccountsUserIdPickList" runat="server" style="cursor: pointer; vertical-align: top"
                                src="<%$ PhoenixTheme:images/picklist.png %>" />
                            <telerik:RadTextBox ID="txtusercodes" runat="server" CssClass="input" ReadOnly="false" MaxLength="20"
                                Width="10px"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <br />
                <br />
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvUserAccessList" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3"  OnNeedDataSource="gvUserAccessList_NeedDataSource" 
                        OnItemDataBound="gvUserAccessList_RowDataBound"  GroupingEnabled="false" EnableHeaderContextMenu="true"
                        OnItemCommand="gvUserAccessList_Rowcommand" EnableViewState="false" >
                         <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" >
                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                      <Columns>
<%--                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />--%>
                            <telerik:GridTemplateColumn HeaderText="Company">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                 <ItemTemplate>
                                    <telerik:RadLabel ID="lblID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Access">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadRadioButtonList ID="rblAccess" runat="server" Direction="Horizontal" AutoPostBack="true"                                        
                                               OnSelectedIndexChanged="rblAccess_SelectedIndexChanged" > 
                                     </telerik:RadRadioButtonList>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                        </Columns>
                    </MasterTableView>
                    </telerik:RadGrid>
              </telerik:RadAjaxPanel>
    </form>
</body>
</html>
