<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRecordList.aspx.cs"
    Inherits="InspectionRecordList" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Inspection Record List</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmInspectionRecordList" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlInspectionRecordListEntry">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Vetting List"></eluc:Title>
                        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                    </div>
                </div>
                <table id="tblGuidance" runat="server">
                    <tr>
                        <td>
                            <asp:Label ID="lblNote" runat="server" Text="Note: <br>1. Click on the 'Vetting Name' to navigate to record screen. <br>2. Record the responses for the checklist in the record screen. <br>3. If the response is recorded as 'No/Not Seen' then record the 'Observation' details in the observation screen. <br>4. To complete, click on the 'Complete' button in the action column.<br>5. To verify, give the verification details and click on the 'Verify' button in the observation screen.<br>6. To close, click on the 'Close' button in the action column."
                                ForeColor="Blue" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                </table>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuInspectionRecordList" runat="server" OnTabStripCommand="InspectionRecordList_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                    <asp:GridView ID="gvInspectionRecordList" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvInspectionRecordList_RowCommand" OnRowDataBound="gvInspectionRecordList_ItemDataBound"
                        OnRowDeleting="gvInspectionRecordList_RowDeleting" OnSorting="gvInspectionRecordList_Sorting" OnRowUpdating="gvInspectionRecordList_RowUpdating"
                        AllowSorting="true" OnRowEditing="gvInspectionRecordList_RowEditing" OnSelectedIndexChanging="gvInspectionRecordList_SelectedIndexChanging"
                        DataKeyNames="FLDINSPECTIONSCHEDULEID" ShowFooter="false" ShowHeader="true" EnableViewState="false">
                        
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        
                        <Columns>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>                                
                                <ItemTemplate>
                                    <asp:Image ID="imgFlag" runat="server" Visible="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Ref.No.">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkInspectionRefNoHeader" runat="server" CommandName="Sort" CommandArgument="FLDREFERENCENUMBER"
                                        ForeColor="White">Reference Number&nbsp;</asp:LinkButton>
                                    <img id="FLDREFERENCENUMBER" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblInspectionRefNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENUMBER") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Inspection Type" Visible="false">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkInspectionTypeHeader" runat="server" CommandName="Sort" CommandArgument="FLDINSPECTIONTYPEID"
                                        ForeColor="White">Type&nbsp;</asp:LinkButton>
                                    <img id="FLDINSPECTIONTYPEID" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblInspectionName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONTYPENAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Inspection Name">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkInspectionNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDINSPECTIONNAME"
                                        ForeColor="White">Vetting Name &nbsp;</asp:LinkButton>
                                    <img id="FLDINSPECTIONNAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblInspectionScheduleId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONSCHEDULEID") %>'></asp:Label>
                                    <asp:Label ID="lblInspectionDtKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></asp:Label>
                                    <asp:LinkButton ID="lnkInspection" runat="server" CommandName="NAVIGATE" CommandArgument="<%# Container.DataItemIndex %>"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONNAME") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Vessel Code">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkVesselCodeHeader" runat="server" CommandName="Sort" CommandArgument="FLDVESSELSHORTCODE"
                                        ForeColor="White">Vessel Code&nbsp;</asp:LinkButton>
                                    <img id="FLDVESSELSHORTCODE" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblVesselCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELSHORTCODE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Inspection Date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkInspectionDateHeader" runat="server" CommandName="Sort" CommandArgument="FLDRANGEFROMDATE"
                                        ForeColor="White">Planned Date &nbsp;</asp:LinkButton>
                                    <img id="FLDRANGEFROMDATE" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblOverdueyn" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVERDUEYN") %>'></asp:Label>
                                    <asp:Label ID="lblInspectionDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDRANGEFROMDATE"))%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Name of Inspector">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblNameOfInspectorHeader" runat="server" CommandName="Sort" CommandArgument="FLDNAMEOFINSPECTOR"
                                        ForeColor="White">Name of Inspector&nbsp;</asp:LinkButton>
                                    <img id="FLDNAMEOFINSPECTOR" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblNameOfInspector" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFINSPECTOR") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Port of Vetting">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkPortOfInspectionHeader" runat="server" CommandName="Sort"
                                        CommandArgument="FLDSEAPORTNAME" ForeColor="White">Port of Vetting&nbsp;</asp:LinkButton>
                                    <img id="FLDSEAPORTNAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPortOfInspection" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkStatusNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDSTATUSNAME"
                                        ForeColor="White">Status&nbsp;</asp:LinkButton>
                                    <img id="FLDSTATUSNAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">
                                                                    Action
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Complete Inspection" ImageUrl="<%$ PhoenixTheme:images/audit_complete.png %>"
                                        CommandName="Update" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdComplete"
                                        ToolTip="Complete Vetting"></asp:ImageButton>                                    
                                    <asp:ImageButton runat="server" AlternateText="Close Inspection" ImageUrl="<%$ PhoenixTheme:images/audit_close.png %>"
                                        CommandName="CLOSEINSPECTION" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdClose"
                                        ToolTip="Close Vetting"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="divPage" style="position: relative;">
                    <table width="100%" border="0" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap align="center">
                                <asp:Label ID="lblPagenumber" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblPages" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblRecords" runat="server">
                                </asp:Label>&nbsp;&nbsp;
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
                                <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                </asp:TextBox>
                                <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                    Width="40px"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
                <table cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                       <img id="Img1" src="<%$ PhoenixTheme:images/red-symbol.png%>" runat="server" />
                    </td>
                    <td>
                       <asp:Literal ID="lblOverdue" runat="server" Text="* Overdue"></asp:Literal>
                    </td>
                    <td>
                        <img id="Img2" src="<%$ PhoenixTheme:images/yellow-symbol.png%>" runat="server" />
                    </td>
                    <td>
                       <asp:Literal ID="lblDue" runat="server" Text=" * Due"></asp:Literal>
                    </td>                    
                </tr>                
             </table>
            </div>
            <eluc:Confirm ID="ucConfirmComplete" runat="server" OnConfirmMesage="btnComplete_Click"
                OKText="Yes" CancelText="No" />  
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
