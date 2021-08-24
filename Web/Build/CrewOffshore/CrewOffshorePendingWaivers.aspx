<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshorePendingWaivers.aspx.cs" Inherits="CrewOffshorePendingWaivers" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
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
    <title>Pending Waivers</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="CrewSuitabilitylink" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmPendingWaivers" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlCrewPendingWaivers">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />
            <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%;
                position: absolute;">
                <div class="subHeader">
                    <eluc:Title runat="server" ID="ucTitle" Text="Pending Waivers" ShowMenu="false" />
                    <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="CrewMenuGeneral" runat="server" OnTabStripCommand="CrewMenuGeneral_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divInput" runat="server">
                <table runat="server" id="tblPersonalMaster" width="100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblFirstName" runat="server" Text="First Name"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblMiddleName" runat="server" Text="Middle Name"></asp:Literal>
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
                        <td runat="server" id="tdempno">
                            <asp:Literal ID="lblEmployeeNo" runat="server" Text="Employee Number"></asp:Literal>
                        </td>
                        <td runat="server" id="tdempnum">
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
                    </table>
                </div> 
                <br />               
                   <table width="100%">                 
                    <tr>
                        <td>
                            <asp:Literal ID="lblRank1" runat="server" Text=" Rank"></asp:Literal>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlRank" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" Enabled="false"
                                DataTextField="FLDRANKNAME" DataValueField="FLDRANKID" AutoPostBack="true" Width="150px" OnTextChanged="ddlRank_Changed">
                                <asp:ListItem Value="Dummy" Text="--Select--"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Literal ID="lblMatrix" runat="server" Text="Training Matrix"></asp:Literal>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlTrainingMatrix" runat="server" Width="300px" CssClass="input_mandatory" Enabled="false"
                                AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlTrainingMatrix_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>                     
                        <td>
                            <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Vessel runat="server" ID="ucVessel" CssClass="input" AppendDataBoundItems="true" Enabled="false"
                                VesselsOnly="true" AssignedVessels="true" AutoPostBack="true" OnTextChangedEvent="SetVesselType" />
                            <%-- <asp:DropDownList ID="ucVessel" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                                AutoPostBack="true" OnTextChanged="SetVesselType" DataTextField="FLDVESSELNAME"
                                DataValueField="FLDVESSELID">
                            </asp:DropDownList>--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblExpectedJoiningDate" runat="server" Text=" Expected Joining Date"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="ucDate" runat="server" CssClass="input_mandatory" Enabled="false" />
                        </td>                        
                        <td>
                            <asp:Literal ID="lblOffsigner" runat="server" Text="Off-signer"></asp:Literal>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlOffSigner" runat="server" Width="242px" CssClass="input" Enabled="false"
                                AppendDataBoundItems="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal Visible="false" ID="lblVesselType" runat="server" Text="Vessel Type"></asp:Literal>
                        </td>
                        <td>
                            <eluc:VesselType Visible="false" ID="ucVesselType" runat="server" AppendDataBoundItems="true" CssClass="readonlytextbox"
                                Enabled="false" />
                        </td>                        
                    </tr>
                </table>
                <asp:Label ID="lblNote" runat="server" CssClass="guideline_text">
                            Note: Please note validity of document checked for "contract + 1 month" starting from the expected joining date.
                </asp:Label>
                <div id="divGrid" style="position: relative; z-index: +1">
                    <asp:GridView ID="gvSuitability" runat="server" AutoGenerateColumns="False" Font-Size="11px" OnRowCommand="gvSuitability_RowCommand"
                        Width="100%" CellPadding="3" OnRowDataBound="gvSuitability_RowDataBound" OnRowEditing="gvSuitability_RowEditing"
                        ShowHeader="true" EnableViewState="true" OnRowCancelingEdit="gvSuitability_RowCancelingEdit">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField HeaderText="Category">
                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="250px" VerticalAlign="Top"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblCategoryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORY") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Type">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Stage">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblStage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTAGENAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Required Document">
                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="130px"></ItemStyle>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblReqDocumentName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUIREDDOC") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Available Document">
                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="130px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblDocumentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTID") %>'></asp:Label>
                                    <asp:Label ID="lblMissingYN" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMISSINGYN") %>'></asp:Label>
                                    <asp:Label ID="lblExpiredYN" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPIREDYN") %>'></asp:Label>
                                    <asp:Label ID="lblVerifiedYN" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVERIFIEDYN") %>'></asp:Label>
                                    <asp:Label ID="lblAuthenticatedYN" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAUTHENTICATEDYN") %>'></asp:Label>
                                    <asp:Label ID="lblFlagId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLAGID") %>'></asp:Label>
                                    <asp:Label ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></asp:Label>
                                    <asp:Label ID="lblAttachmenttype" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDATTACHMENTTYPE") %>'></asp:Label>
                                    <%--<asp:LinkButton ID="lnkVessel" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>'
                                        Text=' <%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:LinkButton>--%>
                                    <asp:Label ID="lblDocumentName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>'></asp:Label>
                                    <asp:Label ID="lblDocumentType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPE") %>'></asp:Label>
                                    <asp:LinkButton ID="lnkName" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>'></asp:LinkButton>
                                    <asp:Label ID="lblWaivedDocumentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAIVEDDOCID") %>'></asp:Label>
                                    <asp:Label ID="lblStageid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTAGEID") %>'></asp:Label>
                                    <asp:Label ID="lblPlanStatus" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLANSTATUS") %>'></asp:Label>
                                    <asp:Label ID="lblProposalStageid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPOSALSTAGEID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Document Status">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Can be waived later">
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="90px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkCanbeWaivedYN" runat="server" Enabled="false" AutoPostBack="false" 
                                        OnCheckedChanged="chkCanbeWaivedYN_CheckedChanged" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDCANBEWAIVED")).ToString().Equals("1")?true:false %>' />                                   
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Waived Y/N">
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="90px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkWaivedYN" runat="server" Enabled="false" AutoPostBack="true" 
                                        OnCheckedChanged="chkWaivedYN_CheckedChanged" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDISWAIVED")).ToString().Equals("1")?true:false %>' />                                   
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Reason">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblReason" runat="server" Text='<%# (DataBinder.Eval(Container, "DataItem.FLDWAIVINGREASON")) %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtReason" runat="server" CssClass="input" Text='<%# (DataBinder.Eval(Container, "DataItem.FLDWAIVINGREASON")) %>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Waived by">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblWaiveBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAIVEDUSERNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Waived Date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblWaivedDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDWAIVEDDATE")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Expiry Date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblExpiryDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATEOFEXPIRY")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Issuing Authority">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="120px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblNationality" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNATIONALITY") %>'></asp:Label>
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
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="SAVE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <table cellpadding="1" cellspacing="1">
                    <tr>
                        <td>
                            <img id="Img1" src="<%$ PhoenixTheme:images/red.png%>" runat="server" />
                        </td>
                        <td>
                            <asp:Label ID="lblDocsExpired" runat="server" CssClass="guideline_text">* Documents Expired & Missing</asp:Label>
                        </td>
                        <%--<td>
                            <img id="Img2" src="<%$ PhoenixTheme:images/blue.png%>" runat="server" />
                        </td>
                        <td>
                            <asp:Label ID="lblDocsMissing" runat="server" CssClass="guideline_text">* Documents Missing</asp:Label>
                        </td>--%>
                    </tr>
                </table>
                <br />
                <b>
                    <asp:Label ID="lblRankExpNote" runat="server" Text="Rank experience required in ANY of the below ranks"></asp:Label></b>
                <div id="divRank" style="position: relative; z-index: +1">
                    <asp:GridView ID="gvRankExp" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowDataBound="gvRankExp_RowDataBound" ShowHeader="true"
                        EnableViewState="true" OnRowCommand="gvRankExp_RowCommand">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField HeaderText="Stage">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblStage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTAGENAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rank">
                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></asp:Label>
                                    <asp:Label ID="lblShortfall" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTFALL") %>'></asp:Label>
                                    <asp:Label ID="lblDocumentType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPE") %>'></asp:Label>
                                    <asp:Label ID="lblWaivedDocumentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAIVEDDOCID") %>'></asp:Label>
                                    <asp:Label ID="lblStageid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTAGEID") %>'></asp:Label>
                                    <asp:Label ID="lblPlanStatus" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLANSTATUS") %>'></asp:Label>
                                    <asp:Label ID="lblProposalStageid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPOSALSTAGEID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Can be waived later">
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="90px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkCanbeWaivedYN" runat="server" Enabled="false" AutoPostBack="false" 
                                        OnCheckedChanged="chkCanbeWaivedYNRankExp_CheckedChanged" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDCANBEWAIVED")).ToString().Equals("1")?true:false %>' />                                   
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Waived Y/N">
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkWaivedYN" runat="server" Enabled="false" AutoPostBack="true"
                                        OnCheckedChanged="chkWaivedYNRankExp_CheckedChanged" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDISWAIVED")).ToString().Equals("1")?true:false %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Reason">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblReason" runat="server" Text='<%# (DataBinder.Eval(Container, "DataItem.FLDWAIVINGREASON")) %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtReason" runat="server" CssClass="input" Text='<%# (DataBinder.Eval(Container, "DataItem.FLDWAIVINGREASON")) %>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Waived by">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblWaiveBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAIVEDUSERNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Waived Date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblWaivedDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDWAIVEDDATE")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Experience Required (in months)">
                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="80px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblRankExpReqd" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKEXPREQUIRED") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Combined Experience held (in months)">
                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="80px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblRankExp" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPERIENCE") %>'></asp:Label>
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
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDITRANKEXP" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="SAVERANKEXP" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="CANCELRANKEXP" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <br />
                <b>
                    <asp:Label ID="lblVesselTypeExpNote" runat="server" Text="Vessel Type experience required in ANY of the below vessel types"></asp:Label></b>
                <div id="divVT" style="position: relative; z-index: +1">
                    <asp:GridView ID="gvVesselTypeExp" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowDataBound="gvVesselTypeExp_RowDataBound" ShowHeader="true"
                        EnableViewState="true" OnRowCommand="gvVesselTypeExp_RowCommand">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField HeaderText="Stage">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblStage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTAGENAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Vessel Type">
                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblVesselType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEDESCRIPTION") %>'></asp:Label>
                                    <asp:Label ID="lblShortfall" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTFALL") %>'></asp:Label>
                                    <asp:Label ID="lblDocumentType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPE") %>'></asp:Label>
                                    <asp:Label ID="lblWaivedDocumentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAIVEDDOCID") %>'></asp:Label>
                                    <asp:Label ID="lblStageid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTAGEID") %>'></asp:Label>
                                    <asp:Label ID="lblPlanStatus" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLANSTATUS") %>'></asp:Label>
                                    <asp:Label ID="lblProposalStageid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPOSALSTAGEID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Can be waived later">
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="90px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkCanbeWaivedYN" runat="server" Enabled="false" AutoPostBack="false" 
                                        OnCheckedChanged="chkCanbeWaivedYNVTExp_CheckedChanged" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDCANBEWAIVED")).ToString().Equals("1")?true:false %>' />                                   
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Waived Y/N">
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkWaivedYN" runat="server" Enabled="false" AutoPostBack="true"
                                        OnCheckedChanged="chkWaivedYNVTExp_CheckedChanged" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDISWAIVED")).ToString().Equals("1")?true:false %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Reason">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblReason" runat="server" Text='<%# (DataBinder.Eval(Container, "DataItem.FLDWAIVINGREASON")) %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtReason" runat="server" CssClass="input" Text='<%# (DataBinder.Eval(Container, "DataItem.FLDWAIVINGREASON")) %>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Waived by">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblWaiveBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAIVEDUSERNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Waived Date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblWaivedDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDWAIVEDDATE")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Experience Required (in months)">
                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="80px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblVTExpReqd" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELTYPEEXPREQUIRED") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Combined Experience held (in months)">
                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="80px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblVesselTypeExp" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPERIENCE") %>'></asp:Label>
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
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDITVTEXP" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="SAVEVTEXP" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="CANCELVTEXP" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <br />
                 <b>
                    <asp:Label ID="lblProposalCheckList" runat="server" Text="Proposal Checklist" Visible="false"></asp:Label></b>
                <div id="div1" style="position: relative; z-index: +1; visibility: hidden;">
                    <asp:GridView ID="gvProposalCheckList" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowDataBound="gvProposalCheckList_RowDataBound" ShowHeader="true"
                        EnableViewState="true" OnRowCommand="gvProposalCheckList_RowCommand">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField HeaderText="Stage">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblStage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTAGENAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Question">
                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblQuestion" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>'></asp:Label>
                                    <asp:Label ID="lblShortfall" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTFALL") %>'></asp:Label>
                                    <asp:Label ID="lblDocumentType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPE") %>'></asp:Label>
                                    <asp:Label ID="lblWaivedDocumentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAIVEDDOCID") %>'></asp:Label>
                                    <asp:Label ID="lblStageid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTAGEID") %>'></asp:Label>
                                    <asp:Label ID="lblPlanStatus" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLANSTATUS") %>'></asp:Label>
                                    <asp:Label ID="lblApprovalStageid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVALSTAGEID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>                          
                            <asp:TemplateField HeaderText="Waived Y/N">
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkWaivedYN" runat="server"  AutoPostBack="true"
                                        OnCheckedChanged="chkWaivedYNProposal_CheckedChanged" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDISWAIVED")).ToString().Equals("1")?true:false %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Reason">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblReason" runat="server" Text='<%# (DataBinder.Eval(Container, "DataItem.FLDWAIVINGREASON")) %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtReason" runat="server" CssClass="input" Text='<%# (DataBinder.Eval(Container, "DataItem.FLDWAIVINGREASON")) %>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Waived by">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblWaiveBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAIVEDUSERNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Waived Date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblWaivedDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDWAIVEDDATE")) %>'></asp:Label>
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
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDITPROPOSAL" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="SAVEPROPOSAL" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="CANCELPROPOSAL" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
