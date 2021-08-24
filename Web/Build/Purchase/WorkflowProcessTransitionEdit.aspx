<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkflowProcessTransitionEdit.aspx.cs" Inherits="WorkflowProcessTransitionEdit" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ProcessTarget" Src="~/UserControls/UserControlWFProcessTarget.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ProcessGroup" Src="~/UserControls/UserControlWFProcessGroup.ascx" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Workflow - Process Transition Edit</title>

    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:TabStrip ID="MenuWFProcessTransitionEdit" runat="server" OnTabStripCommand="MenuWFProcessTransitionEdit_TabStripCommand" TabStrip="true" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table style="margin-left: 20px">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblProcess" runat="server" Text="Process"></telerik:RadLabel>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <telerik:RadLabel ID="txtProcess" runat="server" Text=""></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel4" runat="server" Text="Current State"></telerik:RadLabel>
                    </td>

                    <td>
                        <telerik:RadLabel ID="CurrentState" runat="server" Text="" Width="120px"></telerik:RadLabel>
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel5" runat="server" Text="Next State"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="NextState" runat="server" Width="120px"></telerik:RadLabel>
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblShortCode" runat="server" Text="Short Code"></telerik:RadLabel>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <telerik:RadLabel ID="txtShortCode" runat="server" Text=""></telerik:RadLabel>
                        &nbsp;&nbsp;
                    </td>
                </tr>


                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel3" runat="server" Text="Group"></telerik:RadLabel>
                    </td>

                    <td>
                        <eluc:ProcessGroup ID="UcProcessGroup" runat="server" AutoPostBack="True" Width="150px" CssClass="input_mandatory" />
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel7" runat="server" Text="Target"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:ProcessTarget ID="UcProcessTarget" runat="server" AutoPostBack="True" Width="150px" CssClass="input_mandatory" />
                    </td>
                </tr>




                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="Name"></telerik:RadLabel>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtName" runat="server" Text="" CssClass="input_mandatory" Width="160px"></telerik:RadTextBox>&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel2" runat="server" Text="Description"></telerik:RadLabel>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDescription" runat="server" Text="" TextMode="MultiLine" Width="240px" Rows="6" CssClass="input_mandatory"></telerik:RadTextBox>&nbsp;&nbsp;
                    </td>
                </tr>





            </table>
            <hr />

            <eluc:TabStrip ID="MenuWFTransitionCheck" runat="server" Title="Transition Check"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvTransitionCheck" runat="server" CellSpacing="0" GridLines="None"
                EnableHeaderContextMenu="true" EnableViewState="false" GroupingEnabled="false" OnNeedDataSource="gvTransitionCheck_NeedDataSource"
                OnItemDataBound="gvTransitionCheck_ItemDataBound" OnItemCommand="gvTransitionCheck_ItemCommand">
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
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
                        <telerik:GridTemplateColumn HeaderText="Short Code" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblProcessTransitionCheckId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCESSTRANSITIONCHECK") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCheckShortCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCHECKSHORTCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCheckName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCHECKNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                             <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdCheckDelete" ToolTip="Delete">
                                            <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>

            <hr />

            <eluc:TabStrip ID="MenuWFTransitionActivity" runat="server" Title="Activity"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvTransitionActivity" runat="server" CellSpacing="0" GridLines="None"
                EnableHeaderContextMenu="true" EnableViewState="false" GroupingEnabled="false" OnNeedDataSource="gvTransitionActivity_NeedDataSource"
                OnItemCommand="gvTransitionActivity_ItemCommand" OnItemDataBound="gvTransitionActivity_ItemDataBound">
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
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
                        <telerik:GridTemplateColumn HeaderText="Activity" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTransitionActivityId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCESSTRANSITIONACTIVITYID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lblActivityName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.ACTIVITYNAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Description" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lbldtkey" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>' Visible="false"></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                             <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdActivityDelete" ToolTip="Delete">
                                            <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
        <div>
        </div>
    </form>
</body>
</html>
