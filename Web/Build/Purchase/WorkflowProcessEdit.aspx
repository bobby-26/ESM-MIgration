<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkflowProcessEdit.aspx.cs" Inherits="WorkflowProcessEdit" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Workflow - Process Edit</title>
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
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuWorkflowProcessEdit" runat="server" OnTabStripCommand="MenuWorkflowProcessEdit_TabStripCommand" TabStrip="true"></eluc:TabStrip>

            <table style="margin-left: 20px">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAdministrator" runat="server" Text="Administrator" Width="50px"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="txtAdministrator" runat="server" Text=""></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblUniqueName" runat="server" Text="Unique Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="txtUniqueName" runat="server" Text=""></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="Process Name"></telerik:RadLabel>
                    </td>
                   <td>
                        <telerik:RadTextBox ID="txtName" runat="server" Text="" Width="160px"></telerik:RadTextBox>&nbsp;&nbsp;
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel4" runat="server" Text="Procedure Name"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadLabel ID="txtprocedureName" runat="server" Text=""></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel3" runat="server" Text="Created By/Date & Time"></telerik:RadLabel>
                    </td>
                    <td >
                        <telerik:RadLabel ID="txtdate" runat="server" Text=""></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel2" runat="server" Text="Description"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDescription" runat="server" Text="" Resize="Both" Width="360px" TextMode="MultiLine" Rows="3"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenuWFProcess" runat="server" OnTabStripCommand="MenuWFProcess_TabStripCommand" TabStrip="true" />

            <table width="100%" cellpadding="1" cellspacing="1">

                <tr>
                    <td>
                        <eluc:TabStrip ID="MenuWorkProcessTransition" runat="server" Title="Transition"></eluc:TabStrip>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvProcessTransition" runat="server" CellSpacing="0" GridLines="None"
                            EnableHeaderContextMenu="true" EnableViewState="false" GroupingEnabled="false" OnNeedDataSource="gvProcessTransition_NeedDataSource"
                            OnItemCommand="gvProcessTransition_ItemCommand" OnItemDataBound="gvProcessTransition_ItemDataBound">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDTRANSITIONID">
                                <HeaderStyle Width="102px" />
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

                                    <telerik:GridTemplateColumn HeaderText="Short Code">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblTransition" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRANSITIONID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblShortCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Name">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Current State">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="CurrentStateID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENTSTATEID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblCurrentState" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.CURRENTSTATENAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Next State">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="NextStateId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEXTSTATEID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblNextState" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.NEXTSTATENAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Group">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="GroupId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUPID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblGroupName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.GROUPNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Target">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="TargetId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTARGETID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblTargetNane" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.TARGETNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Description">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle Width="10%" />
                                        <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                            </asp:LinkButton>

                                            <asp:LinkButton runat="server" AlternateText="TransitionDelete" CommandName="TRANSITIONDELETE" ID="cmdTransitionDelete" ToolTip="TransitionDelete">
                                            <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                            </asp:LinkButton>


                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>

                <tr>
                    <td>
                        <eluc:TabStrip ID="MenuWorkProcessState" runat="server" Title="State"></eluc:TabStrip>

                        <telerik:RadGrid RenderMode="Lightweight" ID="gvProcessState" runat="server" CellSpacing="0" GridLines="None"
                            EnableHeaderContextMenu="true" EnableViewState="false" GroupingEnabled="false" OnNeedDataSource="gvProcessState_NeedDataSource"
                            OnItemCommand="gvProcessState_ItemCommand" OnItemDataBound="gvProcessState_ItemDataBound">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDSTATEID">
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
                                    <telerik:GridTemplateColumn HeaderText="State Type" HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblStateType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.STATETYPE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Short Code" HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblShortCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="State" HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <telerik:RadLabel runat="server" ID="lblStateId" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDSTATEID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Description" HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="StateDelete" CommandName="STATEDELETE" ID="cmdStateDelete" ToolTip="StateDelete">
                                            <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>

                <tr>
                    <td>
                        <eluc:TabStrip ID="MenuWorkProcessAction" runat="server" Title="Action"></eluc:TabStrip>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvProcessAction" runat="server" CellSpacing="0" GridLines="None"
                            EnableHeaderContextMenu="true" EnableViewState="false" GroupingEnabled="false" OnNeedDataSource="gvProcessAction_NeedDataSource"
                            OnItemCommand="gvProcessAction_ItemCommand" OnItemDataBound="gvProcessAction_ItemDataBound">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" TableLayout="Fixed">
                                <HeaderStyle Width="102px" />
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
                                    <telerik:GridTemplateColumn HeaderText="Action Type">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblActionType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.ACTIONTYPE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Short Code">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblActionShortCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Action">
                                        <ItemTemplate>
                                            <telerik:RadLabel runat="server" ID="lblActionId" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDACTIONID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblActionname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Description">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblActionDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle Width="10%" />
                                        <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="ActionDelete" CommandName="ACTIONDELETE" ID="cmdActionDelete" ToolTip="ActionDelete">
                                            <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>

                <tr>
                    <td>
                        <eluc:TabStrip ID="MenuWorkProcessActivity" runat="server" Title="Activity"></eluc:TabStrip>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvProcessActivity" runat="server" CellSpacing="0" GridLines="None"
                            EnableHeaderContextMenu="true" EnableViewState="false" GroupingEnabled="false" OnNeedDataSource="gvProcessActivity_NeedDataSource"
                            OnItemCommand="gvProcessActivity_ItemCommand" OnItemDataBound="gvProcessActivity_ItemDataBound">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" TableLayout="Fixed">
                                <HeaderStyle Width="102px" />
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
                                    <telerik:GridTemplateColumn HeaderText="Activity Type">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblActivityType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.ACTIVITYNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Short Code">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblActivityShortCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Activity">
                                        <ItemTemplate>
                                            <telerik:RadLabel runat="server" ID="lblActivityId" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDACTIVITYID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblActivityName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Description">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblActivityDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle Width="10%" />
                                        <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="ActivityDelete" CommandName="ACTIVITYDELETE" ID="cmdActivityDelete" ToolTip="ActivityDelete">
                                            <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>

                <tr>
                    <td>
                        <eluc:TabStrip ID="MenuWorkProcessGroup" runat="server" Title="Group"></eluc:TabStrip>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvProcessGroup" runat="server" CellSpacing="0" GridLines="None"
                            EnableHeaderContextMenu="true" EnableViewState="false" GroupingEnabled="false" OnNeedDataSource="gvProcessGroup_NeedDataSource"
                            OnItemCommand="gvProcessGroup_ItemCommand" OnItemDataBound="gvProcessGroup_ItemDataBound">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" TableLayout="Fixed">
                                <HeaderStyle Width="102px" />
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
                                    <telerik:GridTemplateColumn HeaderText="Short Code">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblGroupid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUPID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblGroupShortCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Name">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblGroupName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Description">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblGroupDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle Width="10%" />
                                        <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" AlternateText="GroupDelete" CommandName="GROUPDELETE" ID="cmdGroupDelete" ToolTip="GroupDelete">
                                            <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>

                <tr>
                    <td>
                        <eluc:TabStrip ID="MenuWorkProcessTarget" runat="server" Title="Target"></eluc:TabStrip>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvProcessTarget" runat="server" CellSpacing="0" GridLines="None"
                            EnableHeaderContextMenu="true" EnableViewState="false" GroupingEnabled="false" OnNeedDataSource="gvProcessTarget_NeedDataSource"
                            OnItemCommand="gvProcessTarget_ItemCommand" OnItemDataBound="gvProcessTarget_ItemDataBound">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" TableLayout="Fixed">
                                <HeaderStyle Width="102px" />
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
                                    <telerik:GridTemplateColumn HeaderText="Short Code">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblTargetid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTARGETID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblTargetShortCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Name">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblTargetName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Description">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblTargetDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle Width="10%" />
                                        <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="TargetDelete" CommandName="TARGETDELETE" ID="cmdTargetDelete" ToolTip="TargetDelete">
                                            <span class="icon"><i class="fas fa-trash-alt"></i></span>
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
