using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


public partial class InspectionRAFrequencyExtn : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionRAFrequencyExtn.aspx?" + Request.QueryString["type"], "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRAFrequency')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionRAFrequencyExtn.aspx?type=" + Request.QueryString["type"], "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuRAFrequency.AccessRights = this.ViewState;
            MenuRAFrequency.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvRAFrequency.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                ViewState["TYPE"] = Request.QueryString["type"];
                ddlType.SelectedValue = Request.QueryString["type"];
                ViewState["TYPENAME"] = "";
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

        DataSet ds = new DataSet();
        string[] alColumns = new string[3];
        string[] alCaptions = new string[3];

        int? type = General.GetNullableInteger(ViewState["TYPE"].ToString());

        if (type == 1)
        { 

            alColumns[0] = "FLDSHORTCODE";
            alColumns[1] = "FLDNAME";
            alColumns[2] = "FLDSCORE";

            alCaptions[0] = "Short Code";
            alCaptions[1] = "Name";
            alCaptions[2] = "Score";

            ViewState["TYPENAME"] = "Activity Duration";
        }
        else if (type == 2)
        {
            alColumns[0] = "FLDSHORTCODE";
            alColumns[1] = "FLDNAME";
            alColumns[2] = "FLDSCORE";

            alCaptions[0] = "Short Code";
            alCaptions[1] = "Name";
            alCaptions[2] = "Score";

            ViewState["TYPENAME"] = "Activity Frequency";
        }
        else if (type == 3)
        {
            alColumns = new string[2];
            alCaptions = new string[2];

            alColumns[0] = "FLDNAME";
            alColumns[1] = "FLDSCORE";

            alCaptions[0] = "Name";
            alCaptions[1] = "Score";

            ViewState["TYPENAME"] = "No of People";
        }
        else if (type == 4)
        {
            alColumns[0] = "FLDSHORTCODE";
            alColumns[1] = "FLDNAME";
            alColumns[2] = "FLDSCORE";

            alCaptions[0] = "Short Code";
            alCaptions[1] = "Name";
            alCaptions[2] = "Score";

            ViewState["TYPENAME"] = "Controls";
        }
        else if (type == 5)
        {
            alColumns = new string[2];
            alCaptions = new string[2];

            alColumns[0] = "FLDNAME";
            alColumns[1] = "FLDSCORE";

            alCaptions[0] = "Name";
            alCaptions[1] = "Serial Number";

            ViewState["TYPENAME"] = "TIME TAKEN TO CARRY OUT REPAIRS";
        }

        else if (type == 6)
        {
            alColumns[0] = "FLDSHORTCODE";
            alColumns[1] = "FLDNAME";
            alColumns[2] = "FLDSCORE";

            alCaptions[0] = "Short Code";
            alCaptions[1] = "Name";
            alCaptions[2] = "Score";

            ViewState["TYPENAME"] = "Controls";
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

        ds = PhoenixInspectionRiskAssessmentFrequencyExtn.RiskAssessmentFrequencySearch(General.GetNullableString(txtName.Text),
            General.GetNullableInteger(ddlType.SelectedValue),
            sortexpression, sortdirection,
            1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);
        Response.AddHeader("Content-Disposition", "attachment; filename=\"" + ViewState["TYPENAME"].ToString() + ".xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>" + ViewState["TYPENAME"].ToString() + "</h3></td>");
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

    protected void RAFrequency_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                gvRAFrequency.CurrentPageIndex = 0;
                gvRAFrequency.Rebind();
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
        string[] alColumns = new string[3];
        string[] alCaptions = new string[3];

        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRAFrequency.CurrentPageIndex + 1;

        int? type = General.GetNullableInteger(ViewState["TYPE"].ToString());

        if (type == 1)
        {

            alColumns[0] = "FLDSHORTCODE";
            alColumns[1] = "FLDNAME";
            alColumns[2] = "FLDSCORE";

            alCaptions[0] = "Short Code";
            alCaptions[1] = "Name";
            alCaptions[2] = "Score";
            gvRAFrequency.Columns[3].Visible = false;

            ViewState["TYPENAME"] = "Activity Duration";
        }
        else if (type == 2)
        {
            alColumns[0] = "FLDSHORTCODE";
            alColumns[1] = "FLDNAME";
            alColumns[2] = "FLDSCORE";

            alCaptions[0] = "Short Code";
            alCaptions[1] = "Name";
            alCaptions[2] = "Score";

            gvRAFrequency.Columns[3].Visible = false;

            ViewState["TYPENAME"] = "Activity Frequency";
        }
        else if (type == 3)
        {
            alColumns = new string[2];
            alCaptions = new string[2];

            alColumns[0] = "FLDNAME";
            alColumns[1] = "FLDSCORE";

            alCaptions[0] = "Name";
            alCaptions[1] = "Score";

            gvRAFrequency.Columns[0].Visible = false;
            gvRAFrequency.Columns[3].Visible = false;

            ViewState["TYPENAME"] = "No Of People";
        }
        else if (type == 4)
        {
            alColumns[0] = "FLDSHORTCODE";
            alColumns[1] = "FLDNAME";
            alColumns[2] = "FLDSCORE";

            alCaptions[0] = "Short Code";
            alCaptions[1] = "Name";
            alCaptions[2] = "Score";

            gvRAFrequency.Columns[3].Visible = false;

            ViewState["TYPENAME"] = "Controls";
        }
        else if (type == 5)
        {
            alColumns = new string[2];
            alCaptions = new string[2];

            alColumns[0] = "FLDNAME";
            alColumns[1] = "FLDSCORE";

            alCaptions[0] = "Name";
            alCaptions[1] = "Serial Number";

            gvRAFrequency.Columns[0].Visible = false;
            gvRAFrequency.Columns[3].Visible = false;

            ViewState["TYPENAME"] = "TIME TAKEN TO CARRY OUT REPAIRS";
        }

        else if (type == 6)
        {
            alColumns[0] = "FLDSHORTCODE";
            alColumns[1] = "FLDNAME";
            alColumns[2] = "FLDSCORE";

            alCaptions[0] = "Short Code";
            alCaptions[1] = "Name";
            alCaptions[2] = "Score";

            gvRAFrequency.Columns[3].Visible = false;

            ViewState["TYPENAME"] = "Controls";
        }

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        DataSet ds = new DataSet();
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixInspectionRiskAssessmentFrequencyExtn.RiskAssessmentFrequencySearch(General.GetNullableString(txtName.Text),
            General.GetNullableInteger(ddlType.SelectedValue),
            sortexpression, sortdirection,
            gvRAFrequency.CurrentPageIndex + 1,
            gvRAFrequency.PageSize,
            ref iRowCount,
            ref iTotalPageCount);


        General.SetPrintOptions("gvRAFrequency", ViewState["TYPENAME"].ToString(), alCaptions, alColumns, ds);

        gvRAFrequency.DataSource = ds;
        gvRAFrequency.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }


    protected void gvRAFrequency_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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

    protected void gvRAFrequency_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
    protected void gvRAFrequency_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                var editableItem = ((GridEditableItem)e.Item);

                GridEditableItem eeditedItem = e.Item as GridEditableItem;

                if (e.CommandName == RadGrid.InitInsertCommandName)
                {
                    gvRAFrequency.EditIndexes.Clear();
                }
                if (e.CommandName == RadGrid.EditCommandName)
                {
                    e.Item.OwnerTableView.IsItemInserted = false;
                }

                if (e.CommandName.ToUpper().Equals("UPDATE"))
                {

                    if (!IsValidRAFrequency(((RadTextBox)editableItem.FindControl("txtNameEdit")).Text,
                        ((UserControlMaskNumber)editableItem.FindControl("ucScoreEdit")).Text,
                        ((RadTextBox)editableItem.FindControl("txtShortCodeEdit")).Text))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    UpdateRAFrequency(
                            Int32.Parse(((RadLabel)editableItem.FindControl("lblFrequencyIdEdit")).Text),
                             ((RadTextBox)editableItem.FindControl("txtNameEdit")).Text,
                             ((UserControlMaskNumber)editableItem.FindControl("ucScoreEdit")).Text,
                             ddlType.SelectedValue,
                              (((RadCheckBox)editableItem.FindControl("chkByVesselEdit")).Checked.Equals(true)) ? 1 : 0,
                              ((RadTextBox)editableItem.FindControl("txtShortCodeEdit")).Text
                         );

                    gvRAFrequency.Rebind();
                }

                if (e.CommandName.ToUpper().Equals("DELETE"))
                {
                    DeleteDesignation(Int32.Parse(((RadLabel)editableItem.FindControl("lblFrequencyId")).Text));
                    gvRAFrequency.Rebind();
                }
            }

            if (e.Item is GridFooterItem)
            {
                var FooterItem = ((GridFooterItem)e.Item);

                if (e.CommandName.ToUpper().Equals("ADD"))
                {
                    GridFooterItem item = (GridFooterItem)gvRAFrequency.MasterTableView.GetItems(GridItemType.Footer)[0];

                    if (!IsValidRAFrequency(((RadTextBox)FooterItem.FindControl("txtNameAdd")).Text,
                        ((UserControlMaskNumber)FooterItem.FindControl("ucScoreAdd")).Text,
                        ((RadTextBox)FooterItem.FindControl("txtShortCodeAdd")).Text))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    InsertRAFrequency(
                        ((RadTextBox)FooterItem.FindControl("txtNameAdd")).Text,
                        ((UserControlMaskNumber)FooterItem.FindControl("ucScoreAdd")).Text,
                        ddlType.SelectedValue,
                        (((RadCheckBox)FooterItem.FindControl("chkByVesselAdd")).Checked.Equals(true)) ? 1 : 0,
                        ((RadTextBox)FooterItem.FindControl("txtShortCodeAdd")).Text
                    );
                    gvRAFrequency.Rebind();
                    ((RadTextBox)FooterItem.FindControl("txtNameAdd")).Focus();
                }
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRAFrequency_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridHeaderItem)
            {
                int? type = General.GetNullableInteger(ViewState["TYPE"].ToString());


            }
            if (e.Item is GridEditableItem)
            {
                GridEditableItem item = e.Item as GridEditableItem;

                LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
                if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

                LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
                if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

                LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
                if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

                LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
                if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

                if (e.Item is GridEditableItem)
                {
                    LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                    if (db != null)
                    {
                        db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    }
                }

                int? type = General.GetNullableInteger(ViewState["TYPE"].ToString());



            }

            if (e.Item is GridFooterItem)
            {
                LinkButton db1 = (LinkButton)e.Item.FindControl("cmdAdd");
                if (db1 != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, db1.CommandName))
                        db1.Visible = false;
                }
                int? type = General.GetNullableInteger(ViewState["TYPE"].ToString());


            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void InsertRAFrequency(string Frequency, string score, string type, int? byvessel, string shortcode)
    {

        PhoenixInspectionRiskAssessmentFrequencyExtn.InsertRiskAssessmentFrequency(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
              Frequency, General.GetNullableInteger(score), Convert.ToInt32(type), byvessel, shortcode);
    }

    private void UpdateRAFrequency(int Frequencyid, string Frequency, string score, string type, int? byvessel, string shortcode)
    {

        PhoenixInspectionRiskAssessmentFrequencyExtn.UpdateRiskAssessmentFrequency(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
             Frequencyid, Frequency, General.GetNullableInteger(score), Convert.ToInt32(type), byvessel, shortcode);
        ucStatus.Text = "Information updated";
    }

    private bool IsValidRAFrequency(string Frequency, string score, string shortcode)
    {
        ucError.HeaderMessage = "Please provide the following required information";


        if (ViewState["TYPE"].ToString() == "1" || ViewState["TYPE"].ToString() == "4" || ViewState["TYPE"].ToString() == "2")
        {
            if (shortcode.Trim().Equals(""))
                ucError.ErrorMessage = "Short Code is required.";
        }

        if (Frequency.Trim().Equals(""))
            ucError.ErrorMessage = "Name is required.";


        if (General.GetNullableInteger(score) == null)
            ucError.ErrorMessage = "Score is required.";


        return (!ucError.IsError);
    }

    private void DeleteDesignation(int Frequencyid)
    {
        PhoenixInspectionRiskAssessmentFrequencyExtn.DeleteRiskAssessmentFrequency(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Frequencyid);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}