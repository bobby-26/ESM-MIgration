using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class CrewAppraisalInCompetent : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvCrewInCompetentAppraisal.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvCrewInCompetentAppraisal.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 0;

            //Filter.CurrentAppraisalSelection = Request.QueryString["appraisalid"].ToString();
        }
        BindData();
    }

    protected void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        try
        {
            DataSet ds = PhoenixCrewAppraisalInCompetent.AppraisalInCompetentSearch(
                                                                new Guid(Filter.CurrentAppraisalSelection));

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvCrewInCompetentAppraisal.DataSource = ds.Tables[0];
                gvCrewInCompetentAppraisal.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvCrewInCompetentAppraisal);
            }
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewInCompetentAppraisal_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            DataRowView drv = (DataRowView)e.Row.DataItem;

            UserControlQuick ucEvaluationItemEdit = (UserControlQuick)e.Row.FindControl("ucEvaluationItemEdit");

            if (ucEvaluationItemEdit != null)
                ucEvaluationItemEdit.SelectedQuick = drv["FLDINCOMPETENTQUESTIONID"].ToString();
        }
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }
    }

    protected void gvCrewInCompetentAppraisal_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        BindData();
        
    }

    protected void gvCrewInCompetentAppraisal_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = e.NewEditIndex;
        _gridView.SelectedIndex = e.NewEditIndex;
        BindData();
        
    }

    

    private Boolean IsPreviousEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
        {
            return true;
        }

        return false;
    }

    private Boolean IsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }

   
    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        try
        {
            gvCrewInCompetentAppraisal.SelectedIndex = -1;
            gvCrewInCompetentAppraisal.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
            else
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
        {
            dt.Rows.Add(dt.NewRow());
            gv.DataSource = dt;
            gv.DataBind();

            int colcount = gv.Columns.Count;
            gv.Rows[0].Cells.Clear();
            gv.Rows[0].Cells.Add(new TableCell());
            gv.Rows[0].Cells[0].ColumnSpan = colcount;
            gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
            gv.Rows[0].Cells[0].Font.Bold = true;
            gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewInCompetentAppraisal_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                if (General.GetNullableGuid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblAppraisalInCompetentId")).Text) != null)
                {
                    PhoenixCrewAppraisalInCompetent.DeleteAppraisalInCompetent(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                           , new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblAppraisalInCompetentId")).Text)
                           );
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewInCompetentAppraisal_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;
        try
        {
            
            if (!IsValidaAppraisalInCompetent(                    
                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtRemarksEdit")).Text))
            {
                ucError.Visible = true;
                return;
            }
            if (General.GetNullableGuid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblAppraisalInCompetentId")).Text) == null)
            {
                InsertAppraisalInCompetent(
                    int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblQuestionId")).Text),
                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtRemarksEdit")).Text);
            }
            else
            {
                PhoenixCrewAppraisalInCompetent.UpdateAppraisalInCompetent(
                                         PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                        , new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblAppraisalInCompetentId")).Text)
                                        , int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblQuestionId")).Text)
                                        , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtRemarksEdit")).Text
                                        );
            }
            _gridView.EditIndex = -1;
            BindData();
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private bool IsValidaAppraisalInCompetent(string remarks)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (remarks == "")
            ucError.ErrorMessage = "Remarks is required.";

        return (!ucError.IsError);
    }

    private void InsertAppraisalInCompetent(int evaluationitem, string remarks)
    {
        PhoenixCrewAppraisalInCompetent.InsertAppraisalInCompetent(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , new Guid(Filter.CurrentAppraisalSelection)
            , evaluationitem
            , remarks
        );
    }

    protected void gvCrewInCompetentAppraisal_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;

        BindData();
        
    }

    protected void GvPersonalInCompetent_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        BindData();
        
    }
}
