<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkflowRequestList.aspx.cs" Inherits="WorkflowRequestList" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ProcessTarget" Src="~/UserControls/UserControlWFProcessTarget.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ProcessGroup" Src="~/UserControls/UserControlWFProcessGroup.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Workflow - Request</title>
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

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuWFReuestList" runat="server" TabStrip="true" />
     
            <table style="margin-left: 20px">

                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel2" runat="server" Text="Title"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="txtRequestName" runat="server"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>                     
                         <telerik:RadLabel ID="RadLabel10" runat="server" Text="Process"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="txtProcessName" runat="server"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel3" runat="server" Text="Request By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="txtRequestBy" runat="server"></telerik:RadLabel>
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel4" runat="server" Text="Request Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="txtRequestDate" runat="server"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel5" runat="server" Text="State"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="txtState" runat="server"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel6" runat="server" Text="User Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="txtUserCode" runat="server"></telerik:RadLabel>
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel7" runat="server" Text="User Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="txtUserName" runat="server"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
            <table style="margin-left: 20px">
                <tr>
                    <td>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvWorkflowRequest" runat="server" CellSpacing="0" GridLines="None"
                            EnableHeaderContextMenu="true" EnableViewState="false" GroupingEnabled="false" OnNeedDataSource="gvWorkflowRequest_NeedDataSource">

                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
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
                                    <telerik:GridTemplateColumn HeaderText="Key" HeaderStyle-HorizontalAlign="Left">
                                        <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Value" HeaderStyle-HorizontalAlign="Left">
                                        <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblvalue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALUE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                </Columns>

                            </MasterTableView>
                        </telerik:RadGrid>

                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
            </table>

            <table style="margin-left: 20px">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCurrentState" runat="server" Text="Current State"></telerik:RadLabel>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <telerik:RadLabel runat="server" ID="txtCurrentState" Text="" Width="120px"></telerik:RadLabel>

                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblNextState" runat="server" Text="Next State"></telerik:RadLabel>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <telerik:RadLabel ID="txtNextState" runat="server" Text="" Width="120px"></telerik:RadLabel>
                        &nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="Group"></telerik:RadLabel>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <eluc:ProcessGroup ID="UcProcessGroupAdd" runat="server" AutoPostBack="True" Width="150px" OnTextChangedEvent="UcProcessGroupAdd_TextChangedEvent" />
                        <telerik:RadLabel ID="lblGroupId" runat="server" Text="" Width="120px" Visible="false"></telerik:RadLabel>
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel8" runat="server" Text="Target"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:ProcessTarget ID="UcProcessTargetAdd" runat="server" AutoPostBack="True" Width="150px" OnTextChangedEvent="UcProcessTargetAdd_TextChangedEvent" />
                        <telerik:RadLabel ID="lblTargetId" runat="server" Text="" Width="120px" Visible="false"></telerik:RadLabel>
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel9" runat="server" Text="User"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDropDownList ID="ddluser" runat="server" AutoPostBack="true" DataTextField="FLDFIRSTNAME" DataValueField="FLDUSERCODE"></telerik:RadDropDownList>

                    </td>
                </tr>
            </table>


            <table style="margin-left: 20px">
                <tr>
                    <td>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvTransition" runat="server" CellSpacing="0" GridLines="None"
                            EnableHeaderContextMenu="true" EnableViewState="false" GroupingEnabled="false" OnNeedDataSource="gvTransition_NeedDataSource"
                            OnItemDataBound="gvTransition_ItemDataBound" OnItemCommand="gvTransition_ItemCommand">

                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>

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

                                    <telerik:GridTemplateColumn HeaderText="Current State" HeaderStyle-HorizontalAlign="Left">
                                        <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblProcessId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCESSID") %>' Visible="false"></telerik:RadLabel>
                                            <telerik:RadLabel ID="lbltransitionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.TRANSITIONID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblFrom" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.CURRENTNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Next State" HeaderStyle-HorizontalAlign="Left">
                                        <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblTo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.NEXTNAME") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblComplete" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISCOMPLETE") %>' Visible="false"></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Group" HeaderStyle-HorizontalAlign="Left">
                                        <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblGroupName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.GROUPNAME") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblGroupid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUPID") %>' Visible="false"></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Target" HeaderStyle-HorizontalAlign="Left">
                                        <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblTargetName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.TARGETNAME") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblTargetId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTARGETID") %>' Visible="false"></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Action" FooterStyle-HorizontalAlign="Center">
                                        <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                                <span class="icon"><i class="fas fa-edit"></i></span>
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" AlternateText="Save" CommandName="SAVE" ID="cmdSave" ToolTip="Save">
                                                <span class="icon"><i class="fas fa-save"></i></span>
                                            </asp:LinkButton>

                                            <asp:LinkButton runat="server" CommandName="ACTIVITY" ID="cmdActivity" ToolTip="Activity">
                                                <span class="icon"><i class="fa fa-pencil-ruler"></i></span>
                                            </asp:LinkButton>

                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>

                            </MasterTableView>
                        </telerik:RadGrid>

                    </td>
                </tr>

            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
