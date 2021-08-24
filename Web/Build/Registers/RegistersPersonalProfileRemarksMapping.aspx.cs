using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using SouthNests.Phoenix.Registers;

public partial class RegistersPersonalProfileRemarksMapping : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Done", "DONE");
        toolbarmain.AddButton("Cancel", "CANCEL");

        MenuRemarks.MenuList = toolbarmain.Show();
        MenuRemarks.SetTrigger(pnlRemarks);

        if (!IsPostBack)
        {
            ViewState["PROFILECATEGORYID"] = "";
            ViewState["RANKID"] = "";
            ViewState["PROFILEQUESTIONID"] = "";

            ViewState["PROFILECATEGORYID"] = Request.QueryString["profilecategoryid"].ToString();
            ViewState["RANKID"] = Request.QueryString["profilerankgroup"].ToString();
            ViewState["PROFILEQUESTIONID"] = Request.QueryString["profilequestionid"].ToString();
        }
        BindSelectedRemarks();
        BindData();
    }

    private void BindSelectedRemarks()
    {
        try
        {
            DataSet ds = PhoenixRegistersAppraisalProfileQuestion.ListSelectedAppraisalRemarks( General.GetNullableInteger(ViewState["PROFILEQUESTIONID"].ToString())
                                                                                                ,General.GetNullableInteger(ViewState["RANKID"].ToString())
                                                                                                ,General.GetNullableInteger(ViewState["PROFILECATEGORYID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvSelectedRemarks.DataSource = ds;
                gvSelectedRemarks.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvSelectedRemarks);
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
        try
        {
            DataTable dt = PhoenixRegistersAppraisalProfileQuestion.ListAppraisalRemarks( General.GetNullableInteger(ViewState["PROFILECATEGORYID"].ToString())
                                                                                                    , General.GetNullableInteger(ViewState["RANKID"].ToString())
                                                                                                    , General.GetNullableInteger(ViewState["PROFILEQUESTIONID"].ToString()));

            if (dt.Rows.Count > 0)
            {
                gvRemarks.DataSource = dt;
                gvRemarks.DataBind();
            }
            else
            {
                ShowNoRecordsFound(dt, gvRemarks);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuRemarks_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("DONE"))
            {
                if (gvRemarks.Rows.Count > 0)
                {
                    StringBuilder strRemarksId = new StringBuilder();


                    foreach (GridViewRow gr in gvRemarks.Rows)
                    {
                        CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                        Label lblAppraisalRemarksId = (Label)gr.FindControl("lblAppraisalRemarksId");

                        if (chkSelect.Checked)
                        {
                            PhoenixRegistersAppraisalProfileQuestion.UpdateAppraisalQuestionRemarks(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                                    , General.GetNullableInteger(ViewState["PROFILECATEGORYID"].ToString())
                                                                                                    , General.GetNullableInteger(ViewState["RANKID"].ToString())
                                                                                                    , General.GetNullableInteger(ViewState["PROFILEQUESTIONID"].ToString())
                                                                                                    , new Guid(lblAppraisalRemarksId.Text));
                        }
                    }

                    //if (strRemarksId.Length > 1)
                    //{
                    //    strRemarksId.Remove(strRemarksId.Length - 1, 1);
                    //}
                    //if (ViewState["REMARKSIDLIST"].ToString() != string.Empty)
                    //{
                    //    strRemarksId.Append(ViewState["REMARKSIDLIST"].ToString());
                    //}

                    //ViewState["REMARKSIDLIST"]="";

                }
            }
            else if (dce.CommandName.ToUpper().Equals("CANCEL"))
            {
             
            }
            BindSelectedRemarks();
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvRemarks_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
    }

    protected void gvRemarks_RowEditing(object sender, GridViewEditEventArgs e)
    {
    }

    protected void gvRemarks_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {

    }

    protected void gvSelectedRemarks_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
        if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
        }
    }

    protected void gvRemarks_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
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
    }

    protected void gvSelectedRemarks_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            string appraisalremarksId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblAppraisalRemarksMappingId")).Text;
            PhoenixRegistersAppraisalProfileQuestion.DeleteAppraisalQuestionRemarksMapping(new Guid(appraisalremarksId));
            //ViewState["REMARKSIDLIST"] = null;
            BindSelectedRemarks();
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
