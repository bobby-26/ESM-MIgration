using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselAccounts;


public partial class VesselAccountsCTMDenominationNew : PhoenixBasePage
{
    private decimal d = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("General", "GENERAL");
            toolbarmain.AddButton("BOW", "BOW");
            toolbarmain.AddButton("CTM", "CTMCAL");
            toolbarmain.AddButton("Denomination", "DENOMINATOIN");
            toolbarmain.AddButton("List", "LIST");
            MenuCTMMain.AccessRights = this.ViewState;
            MenuCTMMain.MenuList = toolbarmain.Show();
            MenuCTMMain.SelectedMenuIndex = 3;
            if (!IsPostBack)
            {

                ViewState["CTMID"] = Request.QueryString["CTMID"];
                ViewState["ACTIVEYN"] = Request.QueryString["a"];
                if (ViewState["CTMID"] != null)
                    EditCTM(new Guid(ViewState["CTMID"].ToString()));
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void EditCTM(Guid CTMId)
    {
        DataTable dt = PhoenixVesselAccountsCTMNew.EditCTMRequest(CTMId);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            txtAmount.Text = dr["FLDAMOUNT"].ToString();
        }
    }
    protected void MenuCTMMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("GENERAL") || ViewState["CTMID"] == null)
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsCTMGeneralNew.aspx";
            }
            else if (dce.CommandName.ToUpper().Equals("BOW"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsCTMBOW.aspx";
            }
            else if (dce.CommandName.ToUpper().Equals("DENOMINATOIN"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsCTMDenominationNew.aspx";
            }
            else if (dce.CommandName.ToUpper().Equals("CTMCAL"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsCTMCalculationNew.aspx";
            }
            if (dce.CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsCTMNew.aspx", false);
            }
            else
                Response.Redirect(ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + "?CTMID=" + ViewState["CTMID"] + "&a=" + ViewState["ACTIVEYN"], false);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void BindData()
    {
        try
        {
            DataTable dt = PhoenixVesselAccountsCTMNew.ListDenomination(new Guid(ViewState["CTMID"].ToString()), ref d);

            if (dt.Rows.Count > 0)
            {
                gvDenomination.DataSource = dt;
                gvDenomination.DataBind();
            }
            else
            {
                ShowNoRecordsFound(dt, gvDenomination);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvDenomination_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;
        try
        {
            Guid? id = General.GetNullableGuid(_gridView.DataKeys[nCurrentRow].Value.ToString());
            PhoenixVesselAccountsCTMNew.DeleteDenomination(id.Value);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvDenomination_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton ed = (ImageButton)e.Row.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            if (db != null && drv["FLDDENOMINATIONID"].ToString() == string.Empty) db.Visible = false;
            if (ViewState["ACTIVEYN"].ToString() != "1")
            {
                if (db != null) db.Visible = false;
                if (ed != null) ed.Visible = false;
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[2].Text = d.ToString();
        }
    }
    protected void gvDenomination_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }
    protected void gvDenomination_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = e.NewEditIndex;
        BindData();
        ((TextBox)((UserControlMaskNumber)_gridView.Rows[e.NewEditIndex].FindControl("txtNotes")).FindControl("txtNumber")).Focus();
    }
    protected void gvDenomination_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;
        try
        {
            Guid? id = General.GetNullableGuid(_gridView.DataKeys[nCurrentRow].Value.ToString());
            string denomination = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblDenomination")).Text;
            string notes = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtNotes")).Text;
            if (!IsValidDenomination(notes))
            {
                ucError.Visible = true;
                return;
            }
            if (!id.HasValue)
            {
                PhoenixVesselAccountsCTMNew.InsertDenomination(PhoenixSecurityContext.CurrentSecurityContext.VesselID, new Guid(ViewState["CTMID"].ToString())
                                                    , int.Parse(denomination), int.Parse(notes));
            }
            else
            {
                PhoenixVesselAccountsCTMNew.UpdateDenomination(id.Value, int.Parse(notes));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        _gridView.EditIndex = -1;
        BindData();
    }

    private bool IsValidDenomination(string notes)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (!General.GetNullableInteger(notes).HasValue)
        {
            ucError.ErrorMessage = "Notes is required.";
        }
        return (!ucError.IsError);
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

}