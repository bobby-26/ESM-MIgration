using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.PreSea;
using System.Text;

public partial class Presea_PreSeaFacultyFeedback : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            if (!IsPostBack)
            {
                if (Request.QueryString["feedbackid"] != null)
                    ViewState["feedbackid"] = Request.QueryString["feedbackid"].ToString();
                else
                    ViewState["feedbackid"] = "0";

                if (Request.QueryString["batchid"] != null)
                    ViewState["batchid"] = Request.QueryString["batchid"].ToString();
                else
                    ViewState["batchid"] = "0";

                BindFaculty();
            }
            BindQuestion();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void BindFaculty()
    {
        ddlFaculty.DataSource = PhoenixPreSeaCandidateFeedback.FacultySearch(General.GetNullableInteger(ViewState["batchid"].ToString()));
        ddlFaculty.DataTextField = "FLDNAME";
        ddlFaculty.DataValueField = "FLDUSERID";
        //ddlFaculty.Items.Insert(0, new ListItem("--Select--", "DUMMY"));
        ddlFaculty.DataBind();
    }

    protected void BindQuestion()
    {
        DataSet ds = new DataSet();

        ds = PhoenixPreSeaCandidateFeedback.FacultyFeedbackSearch(int.Parse(ViewState["feedbackid"].ToString()), General.GetNullableInteger(ddlFaculty.SelectedValue));

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvFacultyEvaluation.DataSource = ds;
            gvFacultyEvaluation.DataBind();
            ViewState["ROWSINGRIDVIEW"] = ds.Tables[0].Rows.Count - 1;
        }

    }

    protected void FacultyEvaluation_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        

    }

    protected void gvFacultyEvaluation_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = -1;
            BindQuestion();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvFacultyEvaluation_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (ddlFaculty.SelectedValue.ToUpper().Equals("DUMMY"))
                {
                    ucError.ErrorMessage = "Please Select a Faculty";
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    if (!string.IsNullOrEmpty(((Label)_gridView.Rows[nCurrentRow].FindControl("lblFacultyEvaluationIdEdit")).Text))
                    {
                        PhoenixPreSeaCandidateFeedback.FacultyFeedbackUpdate(General.GetNullableInteger(((Label)_gridView.Rows[nCurrentRow].FindControl("lblFacultyEvaluationIdEdit")).Text),
                                                                             General.GetNullableInteger(Filter.CurrentPreSeaNewApplicantSelection),
                                                                             General.GetNullableDecimal(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtMarksEdit")).Text)
                                                                             ,General.GetNullableString(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtComments")).Text));
                    }
                    else
                    {
                        PhoenixPreSeaCandidateFeedback.FacultyFeedbackInsert(General.GetNullableInteger(Filter.CurrentPreSeaNewApplicantSelection),
                                                                             General.GetNullableInteger(ViewState["feedbackid"].ToString()),
                                                                             General.GetNullableInteger(((Label)_gridView.Rows[nCurrentRow].FindControl("lblQuestionIdEdit")).Text),
                                                                             General.GetNullableInteger(((Label)_gridView.Rows[nCurrentRow].FindControl("lblFacultyid")).Text),
                                                                             General.GetNullableDecimal(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtMarksEdit")).Text),
                                                                             General.GetNullableInteger(Filter.CurrentPreSeaNewApplicantBatch),
                                                                             General.GetNullableString(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtComments")).Text) );
                    }
                }

                BindQuestion();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('codehelp1','ifMoreInfo','null');", true);
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void gvFacultyEvaluation_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.SelectedIndex = e.NewEditIndex;

            _gridView.EditIndex = e.NewEditIndex;

            BindQuestion();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvFacultyEvaluation_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            _gridView.EditIndex = -1;

            BindQuestion();

            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('codehelp1','ifMoreInfo','null');", true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFacultyEvaluation_ItemDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;

        if (e.Row.RowType == DataControlRowType.DataRow)
        {

        }

    }

    protected void gvFacultyEvaluation_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            BindQuestion();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
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
        gv.Rows[0].Attributes["onclick"] = "";
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
