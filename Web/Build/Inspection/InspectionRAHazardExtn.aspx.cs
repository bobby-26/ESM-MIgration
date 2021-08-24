using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionRAHazardExtn : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionRAHazardExtn.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRAHazard')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionRAHazardExtn.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            if (Request.QueryString["type"] == "1" || Request.QueryString["type"] == "2")
            {
                toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAHazardExtnAdd.aspx?TYPE=" + Request.QueryString["type"] + "')", "Add New", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            }
            MenuRAHazard.AccessRights = this.ViewState;
            MenuRAHazard.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["TYPE"] = Request.QueryString["type"];
                ddlHazardType.SelectedValue = ViewState["TYPE"].ToString();
                if (ViewState["TYPE"].ToString() == "1")
                {
                    ucTitle.Text = "Health and Safety Category";
                }
                else if (ViewState["TYPE"].ToString() == "2")
                {
                    ucTitle.Text = "Environmental Category";
                }
                else if (ViewState["TYPE"].ToString() == "3")
                {
                    ucTitle.Text = "Others";
                }
                else if (ViewState["TYPE"].ToString() == "4")
                {
                    ucTitle.Text = "Economic Category";
                }

                gvRAHazard.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns;
        string[] alCaptions;

        DataSet ds = new DataSet();
        if (ViewState["TYPE"].ToString() == "3")
        {
            alColumns = new string[] { "FLDNAME", "FLDIMPACTNAME", "FLDSEVERITY", "FLDSCORE", "FLDCONSCATEGORY" };
            alCaptions = new string[] { "Name", "Impact", "Severity", "Score", "Consequence" };
        }
        else if (ViewState["TYPE"].ToString() == "4")
        {
            alColumns = new string[] { "FLDNAME", "FLDINCIDENTCONSEQUENCENAME" };
            alCaptions = new string[] { "Name", "Incident Consequence Classification" };
        }
        else
        {
            alColumns = new string[] { "FLDNAME" };
            alCaptions = new string[] { "Name" };
        }
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionRiskAssessmentHazardExtn.RiskAssessmentHazardSearch(General.GetNullableString(txtName.Text),
            General.GetNullableInteger(ddlHazardType.SelectedValue),
            sortexpression, sortdirection,
            1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);
        Response.AddHeader("Content-Disposition", "attachment; filename=" + ucTitle.Text + ".xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3> " + ucTitle.Text + "</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();

    }

    protected void RAHazard_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns;
        string[] alCaptions;
        if (ViewState["TYPE"].ToString() == "3")
        {
            alColumns = new string[] { "FLDNAME", "FLDIMPACTNAME", "FLDSEVERITY", "FLDSCORE", "FLDCONSCATEGORY" };
            alCaptions = new string[] { "Name", "Impact", "Severity", "Score", "Consequence" };
        }
        else if (ViewState["TYPE"].ToString() == "4")
        {
            alColumns = new string[] { "FLDNAME", "FLDINCIDENTCONSEQUENCENAME" };
            alCaptions = new string[] { "Name", "Incident Consequence Classification" };
        }
        else
        {
            alColumns = new string[] { "FLDNAME" };
            alCaptions = new string[] { "Name" };
        }

        gvRAHazard.Columns[6].Visible = true;

        if (ViewState["TYPE"].ToString() == "3")
        {
            gvRAHazard.Columns[1].Visible = true;
            gvRAHazard.Columns[2].Visible = true;
            gvRAHazard.Columns[3].Visible = true;
            gvRAHazard.Columns[5].Visible = true;
            gvRAHazard.Columns[6].Visible = false;
        }
        else
        {
            gvRAHazard.Columns[1].Visible = false;
            gvRAHazard.Columns[2].Visible = false;
            gvRAHazard.Columns[3].Visible = false;
            gvRAHazard.Columns[5].Visible = false;            
        }

        if (ViewState["TYPE"].ToString() == "4")
        {
            gvRAHazard.Columns[4].Visible = true;
            gvRAHazard.Columns[6].Visible = false;
        }
        else
        {
            gvRAHazard.Columns[4].Visible = false;
        }

        if (Request.QueryString["type"] == "1" || Request.QueryString["type"] == "2")
        {
            gvRAHazard.ShowFooter = false;
        }

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        DataSet ds = new DataSet();
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixInspectionRiskAssessmentHazardExtn.RiskAssessmentHazardSearch(General.GetNullableString(txtName.Text),
            General.GetNullableInteger(ddlHazardType.SelectedValue),
            sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
           gvRAHazard.PageSize,
            ref iRowCount,
            ref iTotalPageCount);


        General.SetPrintOptions("gvRAHazard", "Health and Safety Hazard", alCaptions, alColumns, ds);

        gvRAHazard.DataSource = ds;
        gvRAHazard.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvRAHazard_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
                if (e.CommandName.ToUpper().Equals("ADD"))
                {
                    if (!IsValidRAHazard(((RadTextBox)e.Item.FindControl("txtNameAdd")).Text
                                            ,((RadComboBox)e.Item.FindControl("ddlImpactAdd")).SelectedValue))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    InsertRAHazard(
                        ((RadTextBox)e.Item.FindControl("txtNameAdd")).Text,
                        ((UserControlRASeverityExtn)e.Item.FindControl("ucSeverityAdd")).SelectedSeverity,
                        ((RadComboBox)e.Item.FindControl("ddlImpactAdd")).SelectedValue,
                        ((UserControlHard)e.Item.FindControl("ucIncidentConsequenceAdd")).SelectedHard
                    );
                    ((RadTextBox)e.Item.FindControl("txtNameAdd")).Focus();
                }

                if (e.CommandName.ToUpper().Equals("DELETE"))
                {
                    DeleteDesignation(Int32.Parse(((RadLabel)e.Item.FindControl("lblHazardId")).Text));
                }

                if (e.CommandName.ToUpper().Equals("UPDATE"))
                {
                    if (!IsValidRAHazard(((RadTextBox)e.Item.FindControl("txtNameEdit")).Text
                                        , ((RadComboBox)e.Item.FindControl("ddlImpactEdit")).SelectedValue))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    UpdateRAHazard(
                            Int32.Parse(((RadLabel)e.Item.FindControl("lblHazardIdEdit")).Text),
                             ((RadTextBox)e.Item.FindControl("txtNameEdit")).Text,
                             ((UserControlRASeverityExtn)e.Item.FindControl("ucSeverityEdit")).SelectedSeverity,
                             ((RadComboBox)e.Item.FindControl("ddlImpactEdit")).SelectedValue,
                             ((UserControlHard)e.Item.FindControl("ucIncidentConsequenceEdit")).SelectedHard
                         );
                }
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRAHazard_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            if ((Request.QueryString["type"] != null) && ((Request.QueryString["type"] == "1")|| (Request.QueryString["type"] == "2")))
            {
                eb.Visible = false;
            }

            LinkButton edit = (LinkButton)e.Item.FindControl("cmdimgedit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            RadLabel lblHazardId = (RadLabel)e.Item.FindControl("lblHazardId");

            if ((edit != null) && ((ViewState["TYPE"].ToString() == "1") || (ViewState["TYPE"].ToString() == "2")))
                edit.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAHazardExtnAdd.aspx?HAZARDID=" + lblHazardId.Text + "&TYPE="+ Request.QueryString["type"] + "'); return true;");

            if ((edit != null) && ((ViewState["TYPE"].ToString() == "3") || (ViewState["TYPE"].ToString() == "4")))
            {
                edit.Visible = false;
            }

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            UserControlRASeverityExtn ucSeverity = (UserControlRASeverityExtn)e.Item.FindControl("ucSeverityEdit");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (ucSeverity != null) ucSeverity.SelectedSeverity = drv["FLDSEVERITYID"].ToString();

            UserControlHard ucIncidentConsequenceEdit = (UserControlHard)e.Item.FindControl("ucIncidentConsequenceEdit");
            if (ucIncidentConsequenceEdit != null)
            {
                ucIncidentConsequenceEdit.bind();
                ucIncidentConsequenceEdit.SelectedHard = drv["FLDINCIDENTCONSEQUENCE"].ToString();
            }

            RadComboBox ImpactEdit = (RadComboBox)e.Item.FindControl("ddlImpactEdit");
            RadLabel ImpactId = (RadLabel)e.Item.FindControl("lblImpactIdEdit");
            if (ImpactEdit != null && ImpactId != null)
            {
                ImpactEdit.DataSource = PhoenixInspectionOperationalRiskControls.ListRiskAssessmentImpact(null);
                ImpactEdit.DataBind();
                // category.Items.Insert(0, new ListItem("--Select--","Dummy"));
                ImpactEdit.SelectedValue = ImpactId.Text.ToString();
            }

            Image img = (Image)e.Item.FindControl("imgPhoto");
            img.Attributes.Add("src", drv["FLDIMAGE"].ToString());

        }
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
            UserControlHard ucIncidentConsequenceAdd = (UserControlHard)e.Item.FindControl("ucIncidentConsequenceAdd");
            if (ucIncidentConsequenceAdd != null) ucIncidentConsequenceAdd.bind();

            RadComboBox Impact = (RadComboBox)e.Item.FindControl("ddlImpactAdd");
            if (Impact != null)
            {
                Impact.DataSource = PhoenixInspectionOperationalRiskControls.ListRiskAssessmentImpact(null);
                Impact.DataBind();
                //  category.Items.Insert(0, new ListItem("--Select--", "Dummy"));
            }
        }
    }

    private void InsertRAHazard(string name, string severityid, string impactid, string incidentconsequence)
    {

        PhoenixInspectionRiskAssessmentHazardExtn.InsertRiskAssessmentHazard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
              name, Convert.ToInt32(ddlHazardType.SelectedValue), General.GetNullableInteger(severityid), General.GetNullableGuid(impactid), General.GetNullableInteger(incidentconsequence));
        Rebind();
    }

    private void UpdateRAHazard(int Hazardid, string name, string severityid, string impactid,string incidentconsequence)
    {

        PhoenixInspectionRiskAssessmentHazardExtn.UpdateRiskAssessmentHazard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
             Hazardid, name, Convert.ToInt32(ddlHazardType.SelectedValue), General.GetNullableInteger(severityid), General.GetNullableGuid(impactid), General.GetNullableInteger(incidentconsequence));
        ucStatus.Text = "Information updated";
        Rebind();
    }

    private bool IsValidRAHazard(string name, string impact)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        //	GridView _gridView = gvRAHazard;

        if (name.Trim().Equals(""))
            ucError.ErrorMessage = "Name is required.";

        return (!ucError.IsError);
    }

    private void DeleteDesignation(int Hazardid)
    {
        PhoenixInspectionRiskAssessmentHazardExtn.DeleteRiskAssessmentHazard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Hazardid);
    }

    protected void Rebind()
    {
        gvRAHazard.SelectedIndexes.Clear();
        gvRAHazard.EditIndexes.Clear();
        gvRAHazard.DataSource = null;
        gvRAHazard.Rebind();
    }

    protected void gvRAHazard_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRAHazard.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}