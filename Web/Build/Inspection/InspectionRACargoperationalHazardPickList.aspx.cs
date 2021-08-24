using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;

public partial class InspectionRACargoperationalHazardPickList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Close", "CLOSE");
            MenuMOC.AccessRights = this.ViewState;
            MenuMOC.MenuList = toolbarmain.Show();
            MenuMOC.SetTrigger(pnlMOC);
            ViewState["JHAID"] = Request.QueryString["JhaId"].ToString();
        }
        BindGrid();
    }

    private void BindGrid()
    {
        //DataSet ds = new DataSet();
        //ds = PhoenixInspectionRiskAssessmentCargoExtn.JHAOperationalHazardPendingList();
        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    gvOperationalHazard.DataSource = ds;
        //    gvOperationalHazard.DataBind();
        //}
        //else
        //{
        //    DataTable dt = ds.Tables[0];
        //    ShowNoRecordsFound(dt, gvOperationalHazard);
        //}
    }
    protected void gvMOCQuestion_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = e.NewEditIndex;
        _gridView.SelectedIndex = e.NewEditIndex;
        BindGrid();
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
    public void chkOptions_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox chkOptions = (CheckBox)sender;

            GridViewRow gvrow = (GridViewRow)chkOptions.Parent.Parent;

            Label lblOperationalid = (Label)gvrow.FindControl("lblOperationalid");

            CheckBox chkRequiredYNEdit = (((CheckBox)gvrow.FindControl("chkRequiredYNEdit")));

            if ((chkRequiredYNEdit.Checked = true) && (chkRequiredYNEdit.Enabled = true))
            {

                PhoenixInspectionRiskAssessmentCargoExtn.RAOperationalHazardMapping(General.GetNullableGuid((ViewState["JHAID"]).ToString())
                                              , General.GetNullableGuid(((Label)gvrow.FindControl("lblOperationalid")).Text));

                chkRequiredYNEdit.Enabled = false;

                BindGrid();
                String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo','true');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Operational_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("CLOSE"))
            {
                String script = String.Format("javascript:fnReloadList('codehelp1',null,null);");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
            else
            {
                ucError.Visible = true;
                return;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
