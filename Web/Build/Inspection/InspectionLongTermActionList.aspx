<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionLongTermActionList.aspx.cs"
    Inherits="InspectionLongTermActionList" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Department" Src="~/UserControls/UserControlDepartment.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Long Term Action List</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div runat="server" id="DivHeader">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmRegistersBudgetBillingList" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlBudgetBillingListEntry">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="Title1" Text="Office Preventive Task List" />
                    <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuOrderFormMain" runat="server" OnTabStripCommand="MenuOrderFormMain_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                </div>
                <div class="subHeader" style="position: relative;">
                    <div class="divFloat" style="clear: right">
                        <eluc:TabStrip ID="MenuBulkPO" runat="server" OnTabStripCommand="MenuBulkPO_TabStripCommand">
                        </eluc:TabStrip>
                    </div>
                </div>
                <%--<iframe runat="server" id="ifMoreInfo" scrolling="yes" style="min-height: 250px; width: 100%">
                </iframe>  --%>
                <div id="divInspectionType" style="position: relative; z-index: 2">
                    <table width="100%">
                        <tr>
                            <td>
                                <font color="blue" size="0"><b>
                                    <asp:Literal ID="lblTaskStatusChange" runat="server" Text="Task Status Change:"></asp:Literal></b>
                                    <li>Open >> Accepted >> Completed.</li>
                                    <li>Open >> Accepted >> Cancelled.</li>
                                    <li>Cancelled >> Open >> Accepted >> Completed.</li>
                                </font>
                            </td>
                        </tr>
                    </table>
                    <%--<table id="tblBudgetGroupAllocationSearch" width="100%">
                        <tr>
                            <td>
                                Vessel
                            </td>
                            <td>
                                  <eluc:Vessel ID="ucVessel" runat="server" OnTextChangedEvent="selection_Changed"
                                    CssClass="input" VesselsOnly="true" AutoPostBack="true" AppendDataBoundItems="true" />
                            </td>
                            <td>
                                Task Status
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlAcceptance" runat="server" AppendDataBoundItems="true" CssClass="input"
                                    AutoPostBack="true" OnTextChanged="selection_Changed">
                                    <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                    <asp:ListItem Text="Open" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Accepted" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Completed" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                Department
                            </td>
                            <td>
                                <eluc:Department ID="ucDepartment" runat="server" AppendDataBoundItems="true" AutoPostBack="true" CssClass="input" OnTextChanged="selection_Changed" /> 
                            </td>
                        </tr>
                    </table>--%>
                </div>
                <div style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuLongTermAction" runat="server" OnTabStripCommand="MenuLongTermAction_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: +1; position: static">
                    <asp:GridView ID="gvLongTermAction" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        OnSorting="gvLongTermAction_Sorting" Width="100%" CellPadding="3" OnRowCommand="gvLongTermAction_RowCommand"
                        OnRowDataBound="gvLongTermAction_ItemDataBound" OnRowCancelingEdit="gvLongTermAction_RowCancelingEdit"
                        AllowSorting="true" OnRowEditing="gvLongTermAction_RowEditing" ShowFooter="false"
                        ShowHeader="true" EnableViewState="false" DataKeyNames="FLDINSPECTIONPREVENTIVEACTIONID"
                        OnRowCreated="gvLongTermAction_RowCreated" GridLines="None">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkAllRemittance" runat="server" Text="Check All" AutoPostBack="true"
                                        OnPreRender="CheckAll" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" runat="server" EnableViewState="true" OnCheckedChanged="SaveCheckedValues" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Vessel">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkVesselHeader" runat="server" CommandName="Sort" CommandArgument="FLDVESSELNAME"
                                        ForeColor="White">Vessel</asp:LinkButton>
                                    <img id="FLDVESSELNAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></asp:Label>
                                    <asp:Label ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                                    <asp:Label ID="lblSourceId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONNONCONFORMITYID") %>'></asp:Label>
                                    <asp:Label ID="lblSourceType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSOURCETYPE") %>'></asp:Label>
                                    <asp:Label ID="lblManualTaskYN" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMANUALTASKYN") %>'></asp:Label>
                                    <asp:Label ID="lblGenerateTaskYN" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGENERATETASKSYN") %>'></asp:Label>
                                    <asp:Label ID="lblManualGenerateTaskYN" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMANUALGENERATETASKSYN") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Created From">
                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblSourceHeader" runat="server">Source</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDFROM") %>'
                                        Visible="false"></asp:Label>
                                    <asp:LinkButton ID="lnkTaskSource" runat="server" CommandName="SHOWSOURCE" CommandArgument='<%# Container.DataItemIndex %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDFROMNAME") %>'></asp:LinkButton>
                                    <asp:Label ID="lblCreatedFrom" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDFROMNAME") %>'
                                        Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblTypeHeader" runat="server"> Type</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Task">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="250"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblTastHeader" runat="server">Task</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblLongTermActionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONPREVENTIVEACTIONID") %>'></asp:Label>
                                    <asp:CheckBox ID="chkmanualtask" runat="server"  Visible = "false" Checked='<%# DataBinder.Eval(Container,"DataItem.FLDMANUALGENERATETASKSYN").ToString().Equals("1")?true:false %>' />
                                    <%--<asp:LinkButton ID="lnkTask" runat="server" CommandName="SELECT" CommandArgument='<%# Container.DataItemIndex %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDPREVENTIVEACTION").ToString().Length>40 ? DataBinder.Eval(Container, "DataItem.FLDPREVENTIVEACTION").ToString().Substring(0, 40)+ "..." : DataBinder.Eval(Container, "DataItem.FLDPREVENTIVEACTION").ToString() %>'></asp:LinkButton>--%>
                                    <asp:Label ID="lblTask" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPREVENTIVEACTION").ToString().Length>40 ? DataBinder.Eval(Container, "DataItem.FLDPREVENTIVEACTION").ToString().Substring(0, 40)+ "..." : DataBinder.Eval(Container, "DataItem.FLDPREVENTIVEACTION").ToString() %>'></asp:Label>
                                    <asp:LinkButton ID="lnkManualTaskName" runat="server" Visible="false" CommandName="MANUALTASK"
                                        CommandArgument='<%# Container.DataItemIndex %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDPREVENTIVEACTION").ToString().Length>40 ? DataBinder.Eval(Container, "DataItem.FLDPREVENTIVEACTION").ToString().Substring(0, 40)+ "..." : DataBinder.Eval(Container, "DataItem.FLDPREVENTIVEACTION").ToString() %>'></asp:LinkButton>
                                    <eluc:ToolTip ID="ucToolTip" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPREVENTIVEACTION")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Category">
                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="100"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkCategoryHeader" runat="server" CommandName="Sort" CommandArgument="FLDCATEGORY"
                                        ForeColor="White">Category</asp:LinkButton>
                                    <img id="FLDCATEGORY" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCategory" runat="server" Width="100" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTASKCATEGORYNAME") %>'></asp:Label>
                                    <asp:Label ID="lblCategoryShortCode" Visible="false" runat="server" Width="100" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTASKCATEGORYSHORTCODE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sub Category">
                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="120"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblSubCategory" runat="server">Sub Category</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSubCategory" Width="120" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTASKSUBCATEGORYNAME") %>'></asp:Label>
                                    <asp:LinkButton ID="lnkSubCategory" Width="120" runat="server" Visible="false" CommandName="FILEEDIT" CommandArgument='<%# Container.DataItemIndex %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDTASKSUBCATEGORYNAME") %>'></asp:LinkButton>
                                    <asp:Label ID="lblsectionno" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSECTIONNUMBER")%>'
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="lblsectionid" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSECTIONID")%>'
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="lblDocumentId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDOCUMENTID")%>'
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="lblformid" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFORMID")%>'
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="lblformno" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFORMNO")%>'
                                    Visible="false"></asp:Label> 
                                    <asp:Label ID="lblRevisionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISONID") %>'></asp:Label>
                                    <asp:Label ID="lblRevisionStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSID") %>'
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="lblRevisionNumber" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEWREVISIONNUMBER") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Task Status">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblTaskStatusHeader" runat="server">Task Status</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Assigned Department">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkDepartmentHeader" runat="server" CommandName="Sort" CommandArgument="FLDDEPARTMENTNAME"
                                        ForeColor="White">Assigned Department</asp:LinkButton>
                                    <img id="FLDDEPARTMENTNAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAssignedDepartment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENTNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblSubDepartmentHeader" runat="server">Sub Department</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSubDepartment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBDEPARTMENTNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PIC">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblAcceptedByHeader" runat="server">Accepted by</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAcceptedby" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCEPTEDBYNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Acceptance Status" Visible="false">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblAcceptancestatusHeader" runat="server"> Acceptance <br /> Status</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAcceptedYNName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCEPTEDSTATUS") %>'></asp:Label>
                                    <asp:Label ID="lblAcceptedYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCEPTEDBY") %>'
                                        Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Target Date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblTargetdateHeader" runat="server">Target Date</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblTargetDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDTARGETDATE")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Completed Date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblCompletedDateHeader" runat="server"> Completed  <br /> Date</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCompletedDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOMPLETIONDATE")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Work Order Number">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblWorkOrderNumberHeader" runat="server"> Work <br /> Order Number</asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkWorkOrderNumber" runat="server" CommandName="SHOWWORKORDER"
                                        CommandArgument='<%# Container.DataItemIndex %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDWONUMBER") %>'></asp:LinkButton>
                                    <asp:Label ID="lblWorkOrderNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWONUMBER") %>'
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="lblWorkOrderId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERID") %>'
                                        Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">
                                                                    Action
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="2%"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Show Pending Tasks in Vessels" ImageUrl="<%$ PhoenixTheme:images/showall.png %>"
                                        CommandName="SHOWSTATUS" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdShowStatus"
                                        ToolTip="Show Pending Tasks in Vessels"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="divPage" style="position: relative;">
                    <table width="100%" border="0" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap align="center">
                                <asp:Label ID="lblPagenumber" runat="server"> </asp:Label>
                                <asp:Label ID="lblPages" runat="server"> </asp:Label>
                                <asp:Label ID="lblRecords" runat="server"> </asp:Label>&nbsp;&nbsp;
                            </td>
                            <td nowrap align="left" width="50px">
                                <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap align="right" width="50px">
                                <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                            </td>
                            <td nowrap align="center">
                                <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input"> </asp:TextBox>
                                <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                    Width="40px"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="gvLongTermAction" />
        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
