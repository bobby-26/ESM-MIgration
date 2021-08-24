using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Integration;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class InspectionHazardControlMapping : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbartab = new PhoenixToolbar();
            toolbartab.AddButton("Back", "LIST", ToolBarDirection.Right);
            toolbartab.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuMapping.AccessRights = this.ViewState;
            MenuMapping.MenuList = toolbartab.Show();

            if (!IsPostBack)
            {

                ViewState["UNIQUEID"] = "";
                if ((Request.QueryString["uniqueid"] != null) && (Request.QueryString["uniqueid"] != ""))
                {
                    ViewState["UNIQUEID"] = Request.QueryString["uniqueid"].ToString();
                }

                ViewState["CONTACTTYPEID"] = "";
                if ((Request.QueryString["contacttypeid"] != null) && (Request.QueryString["contacttypeid"] != ""))
                {
                    ViewState["CONTACTTYPEID"] = Request.QueryString["contacttypeid"].ToString();
                }

                ViewState["COMPANYID"] = "0";

                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                    ViewState["COMPANYID"] = nvc.Get("QMS");

                int companyid = int.Parse(ViewState["COMPANYID"].ToString());

                BindRecomendedPPE();
                ProcedureDetailEdit();
                btnShowDocuments.Attributes.Add("onclick", "return showPickList('spnPickListDocument', 'codehelp1', '', '../Common/CommonPickListDocumentManagementTree.aspx?iframignore=true&companyid=" + ViewState["COMPANYID"].ToString() + "', true); ");
                btnShowComponents.Attributes.Add("onclick", "return showPickList('spnPickListComponent', 'codehelp1', '', '../Common/CommonPickListGlobalComponent.aspx'); ");
            }
            BindFormPosters();
            BindComponents();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void BindFormPosters()
    {
        DataSet dss = PhoenixInspectionOperationalRiskControls.UndesirableFormPosterList(ViewState["UNIQUEID"] == null ? null : General.GetNullableGuid(ViewState["UNIQUEID"].ToString()));
        int number = 1;
        if (dss.Tables[0].Rows.Count > 0)
        {
            tblForms.Rows.Clear();
            foreach (DataRow dr in dss.Tables[0].Rows)
            {
                RadCheckBox cb = new RadCheckBox();
                cb.ID = dr["FLDFORMPOSTERID"].ToString();
                cb.Text = "";
                cb.Checked = true;
                cb.AutoPostBack = true;
                cb.CheckedChanged += new EventHandler(cb_CheckedChanged);
                HyperLink hl = new HyperLink();
                hl.Text = dr["FLDNAME"].ToString();
                hl.ID = "hlink" + number.ToString();
                hl.Attributes.Add("style", "text-decoration:underline; cursor: pointer;color:Blue;");

                int type = 0;
                PhoenixIntegrationQuality.GetSelectedeTreeNodeType(General.GetNullableGuid(dr["FLDFORMPOSTERID"].ToString()), ref type);
                if (type == 2)
                    hl.Attributes.Add("onclick", "Openpopup('codehelp1', '', '../DocumentManagement/DocumentManagementDocumentGeneral.aspx?showall=0&DOCUMENTID=" + dr["FLDFORMPOSTERID"].ToString() + "');return false;");
                else if (type == 3)
                    hl.Attributes.Add("onclick", "Openpopup('codehelp1', '', '../DocumentManagement/DocumentManagementDocumentSectionContentView.aspx?callfrom=documenttree&SECTIONID=" + dr["FLDFORMPOSTERID"].ToString() + "');return false;");
                else if (type == 5)
                {
                    DataSet ds = PhoenixIntegrationQuality.FormEdit(
                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                            , new Guid(hl.ID.ToString()));

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow drr = ds.Tables[0].Rows[0];
                        hl.Attributes.Add("onclick", "Openpopup('codehelp1', '', '../StandardForm/StandardFormFBformView.aspx?FORMTYPE=DMSForm&FORMID=" + dr["FLDFORMPOSTERID"].ToString() + "&FORMREVISIONID=" + drr["FLDFORMREVISIONID"].ToString() + "');return false;");
                    }
                }
                else if (type == 6)
                {
                    DataSet ds = PhoenixIntegrationQuality.FormEdit(
                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                            , new Guid(dr["FLDFORMPOSTERID"].ToString()));

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow drr = ds.Tables[0].Rows[0];
                        if (drr["FLDFORMREVISIONDTKEY"] != null && General.GetNullableGuid(drr["FLDFORMREVISIONDTKEY"].ToString()) != null)
                        {
                            DataTable dt = PhoenixCommonFileAttachment.AttachmentList(new Guid(drr["FLDFORMREVISIONDTKEY"].ToString()));
                            if (dt.Rows.Count > 0)
                            {
                                DataRow drRow = dt.Rows[0];                                          
                                hl.Target = "_blank";
                                hl.NavigateUrl = "../Common/download.aspx?dtkey=" + drRow["FLDDTKEY"].ToString();
                            }
                        }
                    }
                }

                HtmlTableRow tr = new HtmlTableRow();
                HtmlTableCell tc = new HtmlTableCell();
                tc.Controls.Add(cb);
                tr.Cells.Add(tc);
                tc = new HtmlTableCell();
                tc.Controls.Add(hl);
                tr.Cells.Add(tc);
                tblForms.Rows.Add(tr);
                tc = new HtmlTableCell();
                tc.InnerHtml = "<br/>";
                tr.Cells.Add(tc);
                tblForms.Rows.Add(tr);
                number = number + 1;
            }
            divForms.Visible = true;
        }
        else
            divForms.Visible = false;
    }

    protected void lnkFormAdd_Click(object sender, EventArgs e)
    {
        if (General.GetNullableGuid(txtDocumentId.Text) != null)
        {

            PhoenixInspectionOperationalRiskControls.UpdateUndesirableForms(new Guid(ViewState["UNIQUEID"].ToString()), new Guid(txtDocumentId.Text));
            ucStatus.Text = "Forms/Posters/Checklists added.";
            txtDocumentId.Text = "";
            txtDocumentName.Text = "";
            BindFormPosters();        
        }
    }

    void cb_CheckedChanged(object sender, EventArgs e)
    {
        RadCheckBox c = (RadCheckBox)sender;
        if (c.Checked == false)
        {
            PhoenixInspectionOperationalRiskControls.UndesirableeventFormsDelete(new Guid(ViewState["UNIQUEID"].ToString()), new Guid(c.ID));

            string txt = "";

            ucStatus.Text = txt + "deleted.";
            BindFormPosters();
        }
    }

    protected void MenuMapping_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;

            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string recommendedppe = General.ReadCheckBoxList(cblRecomendedPPE);

               if (!IsValidRAHazard())
               {
                   ucError.Visible = true;
                   return;
               }

                PhoenixInspectionOperationalRiskControls.InsertundesirableeventMapping(new Guid(ViewState["UNIQUEID"].ToString()), General.GetNullableString(txtRiskofEscalation.Text), General.GetNullableString(recommendedppe));

               ucStatus.Text = "Information are mapped successfully.";

               ProcedureDetailEdit();
            }

            if (CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../Inspection/InspectionUndesirableEventWorstCase.aspx?contacttypeid=" + ViewState["CONTACTTYPEID"].ToString() + "&contacttype=" + txtevent.Text);
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidRAHazard()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtRiskofEscalation.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Risk of Escalation is required.";

        return (!ucError.IsError);
    }

    protected void BindRecomendedPPE()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        ds = PhoenixInspectionRiskAssessmentMiscellaneousExtn.RiskAssessmentMiscellaneousSearch(null,
            5,
            null, null,
            1,
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            cblRecomendedPPE.DataSource = ds.Tables[0];
            cblRecomendedPPE.DataBind();
        }
    }
    private void ProcedureDetailEdit()
    {
        DataSet ds = PhoenixInspectionOperationalRiskControls.Undesirableeventedit(new Guid(ViewState["UNIQUEID"].ToString()));
        DataTable dt = ds.Tables[0];
        if (dt.Rows.Count > 0)
        {
            txtevent.Text = dt.Rows[0]["FLDEVENTNAME"].ToString();
            txthazard.Text = dt.Rows[0]["FLDWORSTCASENAME"].ToString();
            General.BindCheckBoxList(cblRecomendedPPE, dt.Rows[0]["FLDPPE"].ToString());
            txtRiskofEscalation.Text = dt.Rows[0]["FLDRISKOFESCALATION"].ToString();            
        }
    }

    private void BindCheckBoxList(RadCheckBoxList cbl, string list)
    {
        foreach (string item in list.Split(','))
        {
            if (item.Trim() != "")
            {
                cbl.SelectedValue = item;
            }
        }
    }

    private string ReadCheckBoxList(RadCheckBoxList cbl)
    {
        string list = string.Empty;

        foreach (ButtonListItem item in cbl.Items)
        {
            if (item.Selected == true)
            {
                list = list + item.Value.ToString() + ",";
            }
        }
        list = list.TrimEnd(',');
        return list;
    }

    protected void lnkComponentAdd_Click(object sender, EventArgs e)
    {
        if (General.GetNullableGuid(txtComponentId.Text) != null)
        {

            PhoenixInspectionOperationalRiskControls.UpdateUndesirableEventComponents(new Guid(ViewState["UNIQUEID"].ToString()), new Guid(txtComponentId.Text));
            ucStatus.Text = "Component added.";
            txtComponentId.Text = "";
            txtComponentCode.Text = "";
            txtComponentName.Text = "";
            BindComponents();
        }
    }

    protected void BindComponents()
    {
        DataSet dss = PhoenixInspectionOperationalRiskControls.UndesirableEventComponentList(ViewState["UNIQUEID"] == null ? null : General.GetNullableGuid(ViewState["UNIQUEID"].ToString()));
        int number = 1;
        if (dss.Tables[0].Rows.Count > 0)
        {
            tblcomponents.Rows.Clear();
            foreach (DataRow dr in dss.Tables[0].Rows)
            {
                RadCheckBox cb = new RadCheckBox();
                cb.ID = dr["FLDCOMPONENTID"].ToString();
                cb.Text = "";
                cb.Checked = true;
                cb.AutoPostBack = true;
                cb.CheckedChanged += new EventHandler(com_CheckedChanged);
                HyperLink hl = new HyperLink();
                hl.Text = dr["FLDCOMPONENTNAME"].ToString();
                hl.ID = "hlink2" + number.ToString();
                hl.Attributes.Add("style", "text-decoration:underline; cursor: pointer;color:Blue;");
                HtmlTableRow tr = new HtmlTableRow();
                HtmlTableCell tc = new HtmlTableCell();
                tc.Controls.Add(cb);
                tr.Cells.Add(tc);
                tc = new HtmlTableCell();
                tc.Controls.Add(hl);
                tr.Cells.Add(tc);
                tblcomponents.Rows.Add(tr);
                tc = new HtmlTableCell();
                tc.InnerHtml = "<br/>";
                tr.Cells.Add(tc);
                tblcomponents.Rows.Add(tr);
                number = number + 1;
            }
            divComponents.Visible = true;
        }
        else
            divComponents.Visible = false;
    }

    void com_CheckedChanged(object sender, EventArgs e)
    {
        RadCheckBox c = (RadCheckBox)sender;
        if (c.Checked == false)
        {
            PhoenixInspectionOperationalRiskControls.UndesirableEventComponentDelete(new Guid(ViewState["UNIQUEID"].ToString()), new Guid(c.ID));

            string txt = "";

            ucStatus.Text = txt + "deleted.";
            BindComponents();
        }
    }
}
