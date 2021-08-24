using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewOffshore;

public partial class CrewOffshoreDMRROBInitialization : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdHiddenPick.Attributes.Add("style", "visibility:hidden;");
            txtVoyageId.Attributes.Add("style", "visibility:hidden");

            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE");
            MenuRobMain.AccessRights = this.ViewState;
            MenuRobMain.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                BindMainData();
                btnShowVoyage.Attributes.Add("onclick", "return showPickList('spnPickListVoyage', 'codehelp1', '', '../Common/CommonPickListDMRVoyage.aspx?vesselid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() + "', true); ");
                txtVessel.Text = PhoenixSecurityContext.CurrentSecurityContext.VesselName;
            }
            BindOilData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuRobMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                Guid iInitialVoyageId = new Guid();
                if (!IsValidMain(txtVoyageId.Text, txtDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                string dttime = (txtTime.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtTime.Text;

                PhoenixCrewOffshoreDMRVoyageData.VoyageInitializeInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(txtVoyageId.Text),
                    PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                    Convert.ToDateTime(txtDate.Text + " " + dttime),
                    General.GetNullableInteger(""),
                    General.GetNullableGuid(""),
                    "",
                    ref iInitialVoyageId);

                ucStatus.Text = "Initial Project information updated";

                BindMainData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvOil_ItemDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            ImageButton cmdEdit = (ImageButton)e.Row.FindControl("cmdEdit");
            if (cmdEdit != null) cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);

            ImageButton cmdSave = (ImageButton)e.Row.FindControl("cmdSave");
            if (cmdSave != null) cmdSave.Visible = SessionUtil.CanAccess(this.ViewState, cmdSave.CommandName);

            ImageButton cmdCancel = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cmdCancel != null) cmdCancel.Visible = SessionUtil.CanAccess(this.ViewState, cmdCancel.CommandName);

        }
    }

    protected void gvOil_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        
    }

    protected void gvOil_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = e.NewEditIndex;
        _gridView.SelectedIndex = e.NewEditIndex;

        BindOilData();
        ((UserControlMaskNumber)_gridView.Rows[e.NewEditIndex].FindControl("txtOilROBEdit")).Focus();
    }

    protected void gvOil_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindOilData();
    }

    protected void gvOil_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;


            if (!IsValidData(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtOilROBEdit")).Text, ViewState["VOYAGEID"].ToString()))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixCrewOffshoreDMRVoyageData.InsertInitialRob(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                    new Guid(ViewState["VOYAGEID"].ToString()),
                    Convert.ToDecimal(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtOilROBEdit")).Text),
                    new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblOilTypeCode")).Text));

            //ucStatus.Text = "Oil information updated";

            _gridView.EditIndex = -1;
            BindOilData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindMainData()
    {
        DataSet ds = PhoenixCrewOffshoreDMRVoyageData.VoyageInitializeEdit(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtVoyageName.Text = dr["FLDVOYAGENO"].ToString();
            txtVoyageId.Text = dr["FLDVOYAGEID"].ToString();
            txtDate.Text = String.Format("{0:dd-MM-yyyy}", dr["FLDCOMMENCEDDATETIME"]);
            txtTime.Text = String.Format("{0:HH:mm}", dr["FLDCOMMENCEDDATETIME"]);
            ViewState["VOYAGEID"] = dr["FLDVOYAGEID"].ToString();
        }
        else
        {
            ViewState["VOYAGEID"] = "";
        }
    }

    private void BindOilData()
    {
        DataSet ds = PhoenixCrewOffshoreDMRVoyageData.ListInitialRob(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvOil.DataSource = ds.Tables[0];
            gvOil.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvOil);
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

    private bool IsValidData(string rob, string voyageNo)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (rob.Trim().Equals(""))
            ucError.ErrorMessage = "ROB is required.";

        if (voyageNo.Trim().Equals(""))
            ucError.ErrorMessage = "Project name is required.";

        return (!ucError.IsError);
    }

    private bool IsValidMain(string voyageNo, string date)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (voyageNo.Trim().Equals(""))
            ucError.ErrorMessage = "Project name is required.";

        if (General.GetNullableDateTime(date) == null)
            ucError.ErrorMessage = "Date is required.";

        return (!ucError.IsError);
    }

    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
    }
}
