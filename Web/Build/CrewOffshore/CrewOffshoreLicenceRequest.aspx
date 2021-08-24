<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreLicenceRequest.aspx.cs" Inherits="CrewOffshoreLicenceRequest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Suitability Check</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="CrewSuitabilitylink" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmCrewSuitabilityCheck" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlCrewSuitability">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />
            <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%;
                position: absolute;">
                <div class="subHeader">
                    <eluc:Title runat="server" ID="ucTitle" Text="Licence Request" ShowMenu="false" />
                    <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="CrewMenuGeneral" runat="server" OnTabStripCommand="CrewMenuGeneral_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <table runat="server" id="tblPersonalMaster" width="100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblFirstName" runat="server" Text="First Name"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:literal ID="lblMiddleName" runat="server" Text="Middle Name"></asp:literal>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtMiddleName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblLastName" runat="server" Text="Last Name"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtLastName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblEmployeeNo" runat="server" Text="Employee Number"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblRank" runat="server" Text="Rank"></asp:Literal>
                        </td>
                        <td colspan="3">
                            <asp:TextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />                            
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Vessel runat="server" ID="ucVessel" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                                VesselsOnly="true" AssignedVessels="true" AutoPostBack="true" OnTextChangedEvent="SetVesselType" />
                           <%-- <asp:DropDownList ID="ucVessel" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                                AutoPostBack="true" OnTextChanged="SetVesselType" DataTextField="FLDVESSELNAME"
                                DataValueField="FLDVESSELID">
                            </asp:DropDownList>--%>
                        </td>
                        <td>
                            <asp:Literal ID="lblVesselType" runat="server" Text="Vessel Type"></asp:Literal>
                        </td>
                        <td>
                            <eluc:VesselType ID="ucVesselType" runat="server" AppendDataBoundItems="true" CssClass="readonlytextbox"
                                Enabled="false" />
                        </td>
                        <td>
                           <asp:Literal id="lblRank1" runat="server" Text=" Rank"></asp:Literal>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlRank" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                               DataTextField="FLDRANKNAME" DataValueField="FLDRANKID" AutoPostBack="true" OnTextChanged="ddlRank_Changed">
                                <asp:ListItem Value="Dummy" Text="--Select--"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblExpectedJoiningDate" runat="server" Text="Expected Joining Date"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="ucDate" runat="server" CssClass="input_mandatory" />
                        </td>
                        <td>
                            <asp:Literal ID="lblMatrix" runat="server" Text="Training Matrix"></asp:Literal>
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="ddlTrainingMatrix" runat="server" Width="255px" CssClass="input_mandatory" AppendDataBoundItems="true"></asp:DropDownList>
                        </td>                       
                    </tr> 
                    <tr>
                        <td>
                            <asp:Literal ID="lblPaymentBy" runat="server" Text="Payment By"></asp:Literal>
                        </td>
                        <td colspan="5">
                            <asp:RadioButtonList runat="server" ID="rblPaymentBy" RepeatDirection="Horizontal">
                            </asp:RadioButtonList>
                        </td>                                             
                    </tr>                  
                </table>     
                 <div style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuCrewLicence" runat="server" OnTabStripCommand="CrewLicence_TabStripCommand">
                    </eluc:TabStrip>                 
                </div>
                 <asp:GridView ID="gvMissingLicence" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" ShowHeader="true"
                    EnableViewState="false">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                    <RowStyle Height="10px" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="25px"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label runat="server" ID="lblHeaderSelect" Text="Select"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelect" runat="server" Checked="true" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                             <HeaderTemplate>
                                <asp:Label runat="server" ID="lblHeaderLicence" Text="Licence"></asp:Label>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblDocumentId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDDOCUMENTID") %>'></asp:Label>                                
                                <%#DataBinder.Eval(Container, "DataItem.FLDLICENCE")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label runat="server" ID="lblHeaderMissing" Text="Missing/Expired"></asp:Label>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblType" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDTYPE") %>'></asp:Label>
                                <%#DataBinder.Eval(Container, "DataItem.FLDMISSINGYN").ToString() == "1" ? "Missing" : "Expired"%>
                                <asp:Label ID="lblMissingYN" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDMISSINGYN")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>  
                         <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label runat="server" ID="lblHeaderFlag" Text="Flag"></asp:Label>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDFLAGNAME")%>
                            </ItemTemplate>
                        </asp:TemplateField>                       
                    </Columns>
                </asp:GridView> 
                <br />
                <b><asp:Literal ID="lblLicencerequests" runat="server" Text="Licence Requests"></asp:Literal></b>         
                <asp:GridView ID="gvLicReq" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" ShowHeader="true" OnRowEditing="gvLicReq_RowEditing"
                    OnRowCancelingEdit="gvLicReq_RowCancelingEdit" OnRowDataBound="gvLicReq_RowDataBound"
                    EnableViewState="false" AllowSorting="true" OnRowCommand="gvLicReq_RowCommand">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No">
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDROWNUMBER") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblFlagHeader" runat="server" CommandName="Sort" CommandArgument="FLDFLAG"
                                    ForeColor="White">Flag</asp:LinkButton>
                                <img id="FLDFLAG" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblProcessId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPROCESSID")%>'></asp:Label>
                                <asp:Label ID="lblFlagId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFLAGID") %>'></asp:Label>
                                <asp:Label ID="lblFlag" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFLAGNAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Vessel Name">
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDVESSELNAME").ToString().TrimEnd(',') %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblLicenceHeader" runat="server" CommandName="Sort" CommandArgument="FLDLICENCE"
                                    ForeColor="White">Licence Requested</asp:LinkButton>
                                <img id="FLDLICENCE" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDLICENCE").ToString().TrimEnd(',')%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Literal ID="lblCrewChangeDate" runat="server" Text="Crew Change Date"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDCREWCHANGEDATE", "{0:dd/MMM/yyyy}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Literal ID="lblRequestedDate" runat="server" Text="Requested Date"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDCREATEDDATE", "{0:dd/MMM/yyyy}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Literal ID="lblRequestedBy" runat="server" Text="Requested By"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDCREATEDBY")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Literal ID="lblStatus" runat="server" Text="Status"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDCRASTATUS")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblAction" runat="server" Text="Action"></asp:Literal>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/red-symbol.png %>"
                                    CommandName="CANCEL" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                                <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>                
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
