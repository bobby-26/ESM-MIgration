using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class RegistersAddressOwnerMapping : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarAddressRelation = new PhoenixToolbar();
            toolbarAddressRelation.AddButton("Supplier", "SUPPLIER");
            toolbarAddressRelation.AddButton("Map", "MAP");
            MenuAddressRelation.AccessRights = this.ViewState;
            MenuAddressRelation.MenuList = toolbarAddressRelation.Show();
            MenuAddressRelation.SelectedMenuIndex = 0;
            if (!IsPostBack)
            {


                if (Request.QueryString["ADDRESSCODE"] != null)
                {
                    ViewState["ADDRESSCODE"] = Request.QueryString["ADDRESSCODE"];
                }
            }
            BindData();


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        
    }

    protected void BindData()
    {
        try
        {
            DataSet ds = PhoenixRegistersAddressRelation.ListAddressOwnerMapping(
                int.Parse(ViewState["ADDRESSCODE"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvAddressRelaion.DataSource = ds;
                gvAddressRelaion.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvAddressRelaion);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void AddressRelation_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("MAP"))
            {
                Response.Redirect("../Registers/RegistersAddressOwnerMappingInsert.aspx?addresscode=" + ViewState["ADDRESSCODE"]);
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAddressRelation_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixRegistersAddressRelation.DeleteAddressOwnerMapping(
                    int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblID")).Text)
                    );
                ucStatus.Text = "Address mapping is removed";
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAddressRelation_SelectedIndexChanged(object sender, EventArgs e)
    {
        
    }

    protected void gvAddressRelation_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
        if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
    }

    protected void gvAddressRelation_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
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
        gv.Rows[0].Attributes["onclick"] = "";
    }
}
