using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.PreSea;
using System.Web.UI.HtmlControls;

public partial class PreSeaBatchEligiblityDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SessionUtil.PageAccessRights(this.ViewState);

                PhoenixToolbar Maintoolbar = new PhoenixToolbar();
                Maintoolbar.AddButton("Course", "COURSE");
                Maintoolbar.AddButton("Batch", "BATCH");
                Maintoolbar.AddButton("Eligibility", "ELIGIBILITY");
                Maintoolbar.AddButton("Batch Contact", "CONTACT");
                Maintoolbar.AddButton("Fees", "FEES");
                Maintoolbar.AddButton("Semester", "SEMESTER");
                Maintoolbar.AddButton("Subjects", "SUBJECTS");
                Maintoolbar.AddButton("Exam", "EXAM");
                MenuBatchManager.AccessRights = this.ViewState;
                MenuBatchManager.MenuList = Maintoolbar.Show();

                MenuBatchManager.SelectedMenuIndex = 2;

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ucBatch.SelectedBatch = Filter.CurrentPreSeaCourseMasterBatchSelection;
                ucBatch.Enabled = false;
                BindElgibilty();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }


    }

    protected void BatchPlanner_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("COURSE"))
            {
                Response.Redirect("../PreSea/PreSeaCourseMaster.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("BATCH"))
            {
                Response.Redirect("../PreSea/PreSeaBatch.aspx");
            }
            
            if (String.IsNullOrEmpty(Filter.CurrentPreSeaCourseMasterBatchSelection))
            {
                ucError.ErrorMessage = "Please select batch to view/modify plan details.";
                ucError.Visible = true;
                return;
            }
            else
            {           
         
            if (dce.CommandName.ToUpper().Equals("ELIGIBILITY"))
            {
                Response.Redirect("../PreSea/PreSeaBatchEligiblityDetails.aspx");
            }          
            else if (dce.CommandName.ToUpper().Equals("CONTACT"))
            {
                Response.Redirect("../PreSea/PreSeaBatchAdmissionContact.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("SUBJECTS"))
            {
                Response.Redirect("../PreSea/PreSeaBatchAdmissionSubjects.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("SEMESTER"))
            {
                Response.Redirect("../PreSea/PreSeaBatchAdmissionSemester.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("FEES"))
            {
                Response.Redirect("../PreSea/PreSeaBatchFees.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("EXAM"))
            {
                Response.Redirect("../PreSea/PreSeaBatchAdmissionExam.aspx");
            }
           }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindElgibilty()
    {
        DataTable dt = PhoenixPreSeaBatchPlanner.ListBatchElegibility(int.Parse(Filter.CurrentPreSeaCourseMasterBatchSelection), null);
        if (dt.Rows.Count > 0)
        {
            gvPreSea.DataSource = dt;
            gvPreSea.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvPreSea);
        }
    }

    protected void gvPreSea_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

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

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
                if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

                ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
                if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

                ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
                if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

                ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
                if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

                if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
                {
                    ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                    if (db != null)
                    {
                        db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    }

                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }


    protected void gvPreSea_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridview = (GridView)sender;

            int nCurrentRow = int.Parse(e.CommandArgument.ToString());
          

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidPreSeaBatchEligiblity(((UserControlQuick)_gridview.FooterRow.FindControl("ucEligiblityAdd")).SelectedQuick))
                {
                    ucError.Visible = true;
                    return;
                }
                TextBox txtDetailsAdd = (TextBox)_gridview.FooterRow.FindControl("txtElgDtlsAdd");
                UserControlQuick ucElig = ((UserControlQuick)_gridview.FooterRow.FindControl("ucEligiblityAdd"));

                PhoenixPreSeaBatchPlanner.InsertBatchEligibility(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                  int.Parse(Filter.CurrentPreSeaCourseMasterSelection),
                                                                  int.Parse(Filter.CurrentPreSeaCourseMasterBatchSelection),
                                                                  int.Parse(ucElig.SelectedQuick),
                                                                  txtDetailsAdd.Text);

            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                Label lblcode = (Label)_gridview.Rows[nCurrentRow].FindControl("lblQuickIdEdit");
                Label lblBatchEligiblityId = (Label)_gridview.Rows[nCurrentRow].FindControl("lblBatchEligiblityIdEdit");
                if (lblcode != null)
                {
                    TextBox txtDetails = (TextBox)_gridview.Rows[nCurrentRow].FindControl("txtElgDtlsEdit");

                    PhoenixPreSeaBatchPlanner.UpdateBatchEligibilty(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                    int.Parse(Filter.CurrentPreSeaCourseMasterBatchSelection),
                                                                    int.Parse(lblcode.Text),
                                                                    txtDetails.Text, int.Parse(lblBatchEligiblityId.Text));
                    ucStatus.Text = "Eligibility Details saved successfully.";
                    _gridview.EditIndex = -1;
                }
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                Label lblBatchEligId = (Label)_gridview.Rows[nCurrentRow].FindControl("lblBatchEligiblityId");

                PhoenixPreSeaBatchPlanner.DeleteBatchEligibility(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                 int.Parse(Filter.CurrentPreSeaCourseMasterBatchSelection),
                                                                 int.Parse(lblBatchEligId.Text));

                ucStatus.Text = "Eligibility Details deleted successfully.";
                _gridview.EditIndex = -1;
            }

            BindElgibilty();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvPreSeaExam_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = e.NewEditIndex;
        _gridView.SelectedIndex = e.NewEditIndex;

        BindElgibilty();
    }
    protected void gvPreSeaExam_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }
    protected void gvPreSeaExam_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

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
            gv.Rows[0].Attributes["onclick"] = "";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    private bool IsValidPreSeaBatchEligiblity(string EligiblityID)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(EligiblityID) == null)
        {
            ucError.ErrorMessage = "Eligiblity Criteria is required.";
        }

        return (!ucError.IsError);
    }

}

