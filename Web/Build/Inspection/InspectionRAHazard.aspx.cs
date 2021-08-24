using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionRAHazard : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionRAHazard.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRAHazard')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionRAHazard.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
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
                    ucTitle.Text = "Health and Safety Hazard";
                }
                else if (ViewState["TYPE"].ToString() == "2")
                {
                    ucTitle.Text = "Environmental Hazard";
                }
                else if (ViewState["TYPE"].ToString() == "3")
                {
                    ucTitle.Text = "Others";
                }
                else if (ViewState["TYPE"].ToString() == "4")
                {
                    ucTitle.Text = "Economic Hazard";
                }

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
            alColumns = new string[] { "FLDNAME", "FLDSEVERITY", "FLDSCORE" };
            alCaptions = new string[] { "Name", "Severity", "Score" };
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

        ds = PhoenixInspectionRiskAssessmentHazard.RiskAssessmentHazardSearch(General.GetNullableString(txtName.Text),
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

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
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
            alColumns = new string[] { "FLDNAME", "FLDSEVERITY", "FLDSCORE" };
            alCaptions = new string[] { "Name", "Severity", "Score" };
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

        if (ViewState["TYPE"].ToString() == "3")
        {
            gvRAHazard.Columns[1].Visible = true;
            gvRAHazard.Columns[2].Visible = true;
        }
        else
        {
            gvRAHazard.Columns[1].Visible = false;
            gvRAHazard.Columns[2].Visible = false;
        }
        if (ViewState["TYPE"].ToString() == "4")
        {
            gvRAHazard.Columns[3].Visible = true;
        }
        else
        {
            gvRAHazard.Columns[3].Visible = false;
        }

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        DataSet ds = new DataSet();
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixInspectionRiskAssessmentHazard.RiskAssessmentHazardSearch(General.GetNullableString(txtName.Text),
            General.GetNullableInteger(ddlHazardType.SelectedValue),
            sortexpression, sortdirection,
            gvRAHazard.CurrentPageIndex + 1,
           gvRAHazard.PageSize,
            ref iRowCount,
            ref iTotalPageCount);


        General.SetPrintOptions("gvRAHazard", "Health and Safety Hazard", alCaptions, alColumns, ds);

        gvRAHazard.DataSource = ds;
        gvRAHazard.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        LinkButton ib = (LinkButton)sender;
        BindData();
    }

    protected void gvRAHazard_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridFooterItem)
            {
                if (e.CommandName.ToUpper().Equals("ADD"))
                {
                    if (!IsValidRAHazard(((RadTextBox)e.Item.FindControl("txtNameAdd")).Text))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    InsertRAHazard(
                        ((RadTextBox)e.Item.FindControl("txtNameAdd")).Text,
                        ((UserControlRASeverity)e.Item.FindControl("ucSeverityAdd")).SelectedSeverity,
                        ((UserControlHard)e.Item.FindControl("ucIncidentConsequenceAdd")).SelectedHard
                    );
                    ((RadTextBox)e.Item.FindControl("txtNameAdd")).Focus();
                }
            }
            if (e.Item is GridEditableItem)
            {
                if (e.CommandName.ToUpper().Equals("DELETE"))
                {
                    DeleteDesignation(Int32.Parse(((RadLabel)e.Item.FindControl("lblHazardId")).Text));
                }

                if (e.CommandName.ToUpper().Equals("UPDATE"))
                {
                    if (!IsValidRAHazard(((RadTextBox)e.Item.FindControl("txtNameEdit")).Text))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    UpdateRAHazard(
                            Int32.Parse(((RadLabel)e.Item.FindControl("lblHazardIdEdit")).Text),
                             ((RadTextBox)e.Item.FindControl("txtNameEdit")).Text,
                             ((UserControlRASeverity)e.Item.FindControl("ucSeverityEdit")).SelectedSeverity,
                             ((UserControlHard)e.Item.FindControl("ucIncidentConsequenceEdit")).SelectedHard
                         );
                }
            }
            BindData();
            gvRAHazard.Rebind();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRAHazard_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            UserControlRASeverity ucSeverity = (UserControlRASeverity)e.Item.FindControl("ucSeverityEdit");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (ucSeverity != null) ucSeverity.SelectedSeverity = drv["FLDSEVERITYID"].ToString();

            UserControlHard ucIncidentConsequenceEdit = (UserControlHard)e.Item.FindControl("ucIncidentConsequenceEdit");
            if (ucIncidentConsequenceEdit != null)
            {
                ucIncidentConsequenceEdit.bind();
                ucIncidentConsequenceEdit.SelectedHard = drv["FLDINCIDENTCONSEQUENCE"].ToString();
            }
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
        }
    }

    private void InsertRAHazard(string name, string severityid, string incidentconsequence)
    {

        PhoenixInspectionRiskAssessmentHazard.InsertRiskAssessmentHazard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
              name, Convert.ToInt32(ddlHazardType.SelectedValue), General.GetNullableInteger(severityid), General.GetNullableInteger(incidentconsequence));
    }

    private void UpdateRAHazard(int Hazardid, string name, string severityid, string incidentconsequence)
    {

        PhoenixInspectionRiskAssessmentHazard.UpdateRiskAssessmentHazard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
             Hazardid, name, Convert.ToInt32(ddlHazardType.SelectedValue), General.GetNullableInteger(severityid), General.GetNullableInteger(incidentconsequence));
        ucStatus.Text = "Information updated";
    }

    private bool IsValidRAHazard(string name)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        //	GridView _gridView = gvRAHazard;

        if (name.Trim().Equals(""))
            ucError.ErrorMessage = "Name is required.";

        return (!ucError.IsError);
    }

    private void DeleteDesignation(int Hazardid)
    {
        PhoenixInspectionRiskAssessmentHazard.DeleteRiskAssessmentHazard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Hazardid);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvRAHazard_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
