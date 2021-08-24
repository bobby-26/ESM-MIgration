<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionVesselCheckItemEdit.aspx.cs" Inherits="InspectionVesselCheckItemEdit" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselByOwner" Src="~/UserControls/UserControlVesselByOwner.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="RadAjaxPanel1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text="" />
            <eluc:TabStrip ID="MenuAddCheckItem" runat="server" OnTabStripCommand="MenuAddCheckItem_TabStripCommand"></eluc:TabStrip>
            <table width="100%" valign="top" id="lblCheckitem" runat="server" cellspacing="3">
                <tr>
                    <td Valign="top" Width="10%">
                        <telerik:RadLabel ID="lblParent" runat="server" Text="Parent Item">
                        </telerik:RadLabel>
                    </td>
                    <td Valign="top" width="40%">
                        <telerik:RadComboBox ID="ddlParentitem" runat="server" AutoPostBack="true" Filter="Contains" Width="98%" EmptyMessage="Type to select "></telerik:RadComboBox>
                    </td>
                    <td Valign="top" Width="10%">
                        <telerik:RadLabel ID="lblDefeciency" runat="server" Text="Deficiency Category"></telerik:RadLabel>
                    </td>
                    <td Valign="top" width="40%">
                        <eluc:Quick ID="ucDefeciencyCategory" runat="server" AppendDataBoundItems="true" Width="98%"
                         Visible="true" QuickTypeCode="47" CssClass="input_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td valign="top" width="10%">
                        <telerik:RadLabel ID="lblReference" runat="server" Text="Reference Number"></telerik:RadLabel>
                    </td>
                    <td valign="top" width="40%">
                        <telerik:RadLabel ID="txtReferencenumber" runat="server" Text="" Width="98%"></telerik:RadLabel>
                    </td>
                    <td valign="top" width="10%">
                        <telerik:RadLabel ID="lblvessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td valign="top" width="40%">
                        <eluc:VesselByOwner runat="server" ID="ucVessel" AppendDataBoundItems="true"
                                VesselsOnly="true" Width="98%" CssClass="input_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td valign="top" width="10%">
                        <telerik:RadLabel ID="lblItem" runat="server" Text="Item"></telerik:RadLabel>
                    </td>
                    <td valign="top" width="40%">
                        <telerik:RadTextBox ID="txtItem" runat="server" TextMode="MultiLine" Resize="Both" Width="98%" Height="100px" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>                    
                    <td valign="top">
                        <telerik:RadLabel ID="lblDescription" runat="server" Text="Description"></telerik:RadLabel>
                    </td>
                    <td valign="top">
                        <telerik:RadTextBox ID="txtDescription" runat="server" Text="" TextMode="MultiLine" Resize="Both" Width="98%" Height="100px" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblGuidenceNote" runat="server" Text="Guidence Note"></telerik:RadLabel>
                    </td>
                    <td valign="top">
                        <telerik:RadTextBox ID="txtGuidence" runat="server" Text="" TextMode="MultiLine" Resize="Both" Width="98%" Height="100px" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>
                    <td valign="top" width="10%">

                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="Audit / Inspection / Chapter Code"></telerik:RadLabel>
                    </td>
                    <td valign="top" width="40%">

                        <asp:LinkButton ID="btnChaperAdd" runat="server" ToolTip="Add">
                                        <span class="icon"><i class="fas fa-plus-circle"></i></span>
                        </asp:LinkButton>
                        <div id="divChapterCode" runat="server" style="height: 100px; overflow-y: auto; overflow-x: auto; width: 98%; border-width: 1px; border-style: solid; border: 1px solid #c3cedd">
                            <table id="tblChapter" runat="server">
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblPSC" runat="server" Text="PSC Code" ></telerik:RadLabel>
                    </td>
                    <td valign="top">
                        <telerik:RadComboBox ID="ddlPSC" runat="server" AutoPostBack="true" DataTextField="FLDCHAPTERNAME" DataValueField="FLDCHAPTERID"
                            Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select Chapter" Width="70%" Visible="false">
                        </telerik:RadComboBox>
                        <telerik:RadTextBox ID="txtPSC" runat="server" Text="" MaxLength="50" Width="28%" ></telerik:RadTextBox>

                    </td>
                    <td valign="top">
                        <telerik:RadLabel ID="lblVIQ" runat="server" Text="VIQ Code" ></telerik:RadLabel>
                    </td>
                    <td valign="top">
                        <telerik:RadComboBox ID="ddlVIQ" runat="server" AutoPostBack="true" DataTextField="FLDCHAPTERNAME" DataValueField="FLDCHAPTERID"
                            Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select Chapter" Width="70%" Visible="false">
                        </telerik:RadComboBox>
                        <telerik:RadTextBox ID="txtVIQ" runat="server" Text="" MaxLength="50" Width="28%" ></telerik:RadTextBox>
                    </td>                    
                </tr>
                <tr>                    
                    <td valign="top">
                        <telerik:RadLabel ID="lblCDI" runat="server" Text="CDI Code" ></telerik:RadLabel>
                    </td>
                    <td valign="top">
                        <telerik:RadComboBox ID="ddlCDI" runat="server" AutoPostBack="true" DataTextField="FLDCHAPTERNAME" DataValueField="FLDCHAPTERID"
                            Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select Chapter" Width="70%" Visible="false">
                        </telerik:RadComboBox>
                        <telerik:RadTextBox ID="txtCDI" runat="server" Text="" MaxLength="50" Width="28%" ></telerik:RadTextBox>
                    </td>
                    <td valign="top">
                        <telerik:RadLabel ID="lblVir" runat="server" Text="USCG Code" ></telerik:RadLabel>
                    </td>
                    <td valign="top">
                        <telerik:RadTextBox ID="txtVIR" runat="server" Text="" MaxLength="50" Width="28%" ></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblComponents" runat="server" Text="Components Code"></telerik:RadLabel>
                    </td>
                    <td valign="top">
                        <span id="spnPickListComponent">
                            <telerik:RadTextBox ID="txtComponentCode" runat="server" CssClass="input"
                                Width="20%">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtComponentName" runat="server" CssClass="input"
                                Width="70%">
                            </telerik:RadTextBox>
                            <asp:LinkButton ID="btnShowComponents" runat="server" ToolTip="Select Equipment">
                                        <span class="icon"><i class="fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton ID="lnkComponentAdd" runat="server" OnClick="lnkComponentAdd_Click"
                                ToolTip="Add">
                                        <span class="icon"><i class="fas fa-plus-circle"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtComponentId" runat="server" CssClass="hidden" Width="0px"></telerik:RadTextBox>
                        </span>
                        <br />
                        <div id="divComponents" runat="server" style="height: 120px; overflow-y: auto; overflow-x: auto; width: 98%; border-width: 1px; border-style: solid; border: 1px solid #c3cedd">
                            <table id="tblComponents" runat="server">
                            </table>
                        </div>
                    </td>
                    <td valign="top">
                        <telerik:RadLabel ID="lblLocation" runat="server" Text="Location"></telerik:RadLabel>
                    </td>
                    <td valign="top">
                        <span id="spnPickListLocation">
                            <telerik:RadTextBox ID="txtLocationCode" runat="server" CssClass="input"
                                Width="20%">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtLocationName" runat="server" CssClass="input"
                                Width="70%">
                            </telerik:RadTextBox>
                            <asp:LinkButton ID="btnShowLocation" runat="server" ToolTip="Select Equipment">
                                        <span class="icon"><i class="fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton ID="lnkLocationAdd" runat="server" OnClick="lnkLocationAdd_Click"
                                ToolTip="Add">
                                        <span class="icon"><i class="fas fa-plus-circle"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtLocationId" runat="server" CssClass="hidden" Width="0px"></telerik:RadTextBox>
                        </span>
                        <br />
                        <div id="divLocation" runat="server" style="height: 120px; overflow-y: auto; overflow-x: auto; width: 98%; border-width: 1px; border-style: solid; border: 1px solid #c3cedd">
                            <table id="tblLocation" runat="server">
                            </table>
                        </div>
                    </td>


                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblProcedureform" runat="server" Text="Procedures Forms  Checklists"></telerik:RadLabel>
                    </td>
                    <td valign="top">
                        <span id="spnPickListdocumentprocedure">
                            <telerik:RadTextBox ID="txtdocumentProcedureName" runat="server" Width="90%" Enabled="False" Style="font-weight: bold">
                            </telerik:RadTextBox>
                            <asp:LinkButton ID="btnShowdocumentProcedure" runat="server" ToolTip="Select Procedures">
                                 <span class="icon"><i class="fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton ID="lnkdocumentProcedureAdd" runat="server" OnClick="lnkDocumentAdd_Click" ToolTip="Add">
                                                <span class="icon"><i class="fas fa-plus-circle"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtdocumentProcedureId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                        </span>
                        <br></br>
                        <div id="divdocumentProcedure" runat="server" style="height: 100px; overflow-y: auto; overflow-x: auto; width: 98%; border-width: 1px; border-style: solid; border: 1px solid #c3cedd">
                            <table id="tbldocumentprocedure" runat="server">
                            </table>
                        </div>
                    </td>
                    <td valign="top">
                        <telerik:RadLabel ID="lblJob" runat="server" Text="Job Mapping"></telerik:RadLabel>
                    </td>
                    <td valign="top">
                        <div id="divjobs" runat="server" style="height: 120px; overflow-y: auto; overflow-x: auto; width: 98%; border-width: 1px; border-style: solid; border: 1px solid #c3cedd">
                            <asp:CheckBoxList ID="cbJobs" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" RepeatColumns="2" AutoPostBack="false">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblFiling" runat="server" Text="Filing System"></telerik:RadLabel>
                    </td>
                    <td valign="top">
                        <span id="spnPickListFMS">
                            <telerik:RadTextBox ID="txtReportName" runat="server" Width="90%" Enabled="False" Style="font-weight: bold"
                                CssClass="input">
                            </telerik:RadTextBox>
                            <asp:LinkButton ID="btnShowFMS" runat="server" Text="..">
                                 <span class="icon"><i class="fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton ID="lnkReportAdd" runat="server" OnClick="lnkReportAdd_Click">
                                <span class="icon"><i class="fas fa-plus-circle"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtReportId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtFormId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                        </span>
                        <br></br>
                        <div id="divReports" runat="server" style="height: 120px; width: 98%; overflow: auto; border-width: 1px; border-style: solid; border: 1px solid #9f9fff">
                            <table id="tblReports" runat="server">
                            </table>
                        </div>
                    </td>
                    <td valign="top">
                        <telerik:RadLabel ID="lblResponsibility" runat="server" Text="Responsibility"></telerik:RadLabel>
                    </td>
                    <td valign="top">
                        <div id="divpersonsinvolved" runat="server" style="height: 150px; overflow-y: auto; overflow-x: auto; width: 98%; border-width: 1px; border-style: solid; border: 1px solid #c3cedd">
                            <asp:CheckBoxList ID="ChkgroupMem" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" RepeatColumns="2" AutoPostBack="false">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblProcess" runat="server" Text="Process"></telerik:RadLabel>
                    </td>
                    <td valign="top">
                        <div id="divProcess" runat="server" style="height: 150px; overflow-y: auto; overflow-x: auto; width: 98%; border-width: 1px; border-style: solid; border: 1px solid #c3cedd">
                            <telerik:RadCheckBoxList runat="server" ID="ChkProcess" Columns="2" Layout="Flow" AutoPostBack="true"
                                OnSelectedIndexChanged="ChkProcess_SelectedIndexChanged" CausesValidation="false">
                            </telerik:RadCheckBoxList>
                        </div>
                    </td>
                    <td valign="top">
                        <telerik:RadLabel ID="lblActivity" runat="server" Text="Activity"></telerik:RadLabel>
                    </td>
                    <td valign="top">
                        <div id="divActivity" runat="server" style="height: 150px; overflow-y: auto; overflow-x: auto; width: 98%; border-width: 1px; border-style: solid; border: 1px solid #c3cedd">
                            <telerik:RadCheckBoxList runat="server" ID="ChkActivity" Columns="2" Layout="Flow"
                                DataBindings-DataTextField="FLDNAME" DataBindings-DataValueField="FLDACTIVITYID" AppendDataBoundItems="true">
                            </telerik:RadCheckBoxList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblOwner" runat="server" Text="Owner"></telerik:RadLabel>
                    </td>
                    <td valign="top">
                        <div id="divOwner" runat="server" style="height: 150px; overflow-y: auto; overflow-x: auto; width: 98%; border-width: 1px; border-style: solid; border: 1px solid #c3cedd">
                            <asp:CheckBoxList ID="chkOwner" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" RepeatColumns="2" AutoPostBack="false">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                    <td valign="top">
                        <telerik:RadLabel ID="lblClient" runat="server" Text="Client"></telerik:RadLabel>
                    </td>
                    <td valign="top">
                        <div id="divClient" runat="server" style="height: 150px; overflow-y: auto; overflow-x: auto; width: 98%; border-width: 1px; border-style: solid; border: 1px solid #c3cedd">
                            <asp:CheckBoxList ID="chkClient" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" RepeatColumns="2" AutoPostBack="false">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblVesselType" runat="server" Text="Vessel Type"></telerik:RadLabel>
                    </td>
                    <td valign="top">
                        <div id="divVesselType" runat="server" style="height: 150px; overflow-y: auto; overflow-x: auto; width: 98%; border-width: 1px; border-style: solid; border: 1px solid #c3cedd">
                            <asp:CheckBoxList ID="CbVesselType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" RepeatColumns="2" AutoPostBack="false">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                    <td valign="top">
                        <telerik:RadLabel ID="lblActiveyn" runat="server" Text="Active Y/N"></telerik:RadLabel>
                    </td>
                    <td valign="top">
                        <telerik:RadCheckBox ID="cbActive" runat="server"></telerik:RadCheckBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

