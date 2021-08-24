<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceJobParameter.aspx.cs" Inherits="PlannedMaintenance_PlannedMaintenanceJobParameter" %>

<%@ Import Namespace="System.Data" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Job Parameter</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />        
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">        
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuJobParameter" runat="server" OnTabStripCommand="MenuJobParameter_TabStripCommand"></eluc:TabStrip>
        <telerik:RadGrid ID="gvJobParameter" runat="server" Height="94%" OnItemDataBound="gvJobParameter_ItemDataBound"
            OnNeedDataSource="gvJobParameter_NeedDataSource" OnItemCommand="gvJobParameter_ItemCommand" OnSortCommand="gvJobParameter_SortCommand"
            EnableViewState="false" ShowFooter="true" AllowCustomPaging="true" AllowPaging="true" EnableHeaderContextMenu="true">
            <MasterTableView EditMode="InPlace" AutoGenerateColumns="false" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" DataKeyNames="FLDPARAMETERID">
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Code">
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDPARAMETERCODE"]%>
                        </ItemTemplate>
                         <EditItemTemplate>
                            <telerik:RadTextBox ID="txtParameterCodeEdit" ToolTip="Code" runat="server" Width="100%" MaxLength="6" CssClass="gridinput_mandatory"
                                Text='<%#((DataRowView)Container.DataItem)["FLDPARAMETERCODE"]%>'>
                            </telerik:RadTextBox>
                        </EditItemTemplate>
                         <FooterTemplate>
                            <telerik:RadTextBox ID="txtParameterCode" ToolTip="Code" runat="server" Width="100%" MaxLength="6" CssClass="gridinput_mandatory">
                            </telerik:RadTextBox>
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Name">
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDPARAMETERNAME"]%>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadTextBox ID="txtParameterNameEdit" ToolTip="Name" runat="server" Width="100%" CssClass="gridinput_mandatory"
                                MaxLength="100" Text='<%#((DataRowView)Container.DataItem)["FLDPARAMETERNAME"]%>'>
                            </telerik:RadTextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <telerik:RadTextBox ID="txtParameterName" ToolTip="Name" runat="server" Width="100%" MaxLength="100" CssClass="gridinput_mandatory">
                            </telerik:RadTextBox>
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Type">
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDPARAMETERTYPENAME"]%>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadDropDownList runat="server" ID="ddlParameterTypeEdit"  DataTextField="FLDPARAMETERTYPENAME"
                                DataValueField="FLDPARAMETERTYPEID" CssClass="gridinput_mandatory">
                            </telerik:RadDropDownList>
                        </EditItemTemplate>
                         <FooterTemplate>
                            <telerik:RadDropDownList runat="server" ID="ddlParameterTypeAdd"  DataTextField="FLDPARAMETERTYPENAME"
                                DataValueField="FLDPARAMETERTYPEID" CssClass="gridinput_mandatory" EnableDirectionDetection="true">
                            </telerik:RadDropDownList>
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn HeaderText="Is Active">
                        <ItemTemplate>
                            <telerik:RadCheckBox ID="chkIsActive" runat="server" Enabled="false"
                                Checked='<%#((DataRowView)Container.DataItem)["FLDISACTIVE"].ToString() =="1"? true : false%>'></telerik:RadCheckBox>
                        </ItemTemplate>
                         <EditItemTemplate>
                             <telerik:RadCheckBox ID="chkIsActiveEdit" runat="server"
                                Checked='<%#((DataRowView)Container.DataItem)["FLDISACTIVE"].ToString() =="1"? true : false%>'></telerik:RadCheckBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                        <HeaderStyle />
                        <ItemStyle Wrap="false" />
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                            </asp:LinkButton> <asp:LinkButton runat="server" AlternateText="Options" CommandName="OPTIONS" ID="cmdoptions" ToolTip="Options">
                                    <span class="icon"><i class="fas fa-list-alt"></i></span>
                            </asp:LinkButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                            </asp:LinkButton>
                        </EditItemTemplate>
                        <FooterStyle HorizontalAlign="Center" />
                        <FooterTemplate>
                            <asp:LinkButton runat="server" AlternateText="Save" CommandName="Add" ID="cmdAdd" ToolTip="Add New">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                            </asp:LinkButton>
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
             <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" EnableNextPrevFrozenColumns="true" ScrollHeight="415px" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
