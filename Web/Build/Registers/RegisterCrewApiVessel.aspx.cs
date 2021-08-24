using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class RegisterCrewApiVessel : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegisterCrewApiVessel.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvAPI')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegisterCrewApiVessel.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegisterCrewApiVessel.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuRegisters.AccessRights = this.ViewState;
            MenuRegisters.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvAPI.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
   
    protected void MenuRegisters_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvAPI.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtIMONumber.Text = "";
                ViewState["PAGENUMBER"] = 1;
                gvAPI.Rebind();
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
        string[] alColumns = { "FLDVESSELNAME", "FLDIMONUMBER", "FLDACTIVEYESNO" };
        string[] alCaptions = { "Vessel", "IMO Number", "ActiveYN" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersCrewApiVessel.APIVesselSearch(txtIMONumber.Text.Trim(), sortexpression, sortdirection,
                                                            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                            gvAPI.PageSize,
                                                            ref iRowCount,
                                                            ref iTotalPageCount);
        if (ds.Tables.Count > 0)
            General.ShowExcel("Api Vessels", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }


    protected void gvAPI_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAPI.CurrentPageIndex + 1;
            BindData();
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
        string[] alColumns = { "FLDVESSELNAME", "FLDIMONUMBER", "FLDACTIVEYESNO" };
        string[] alCaptions = { "Vessel", "IMO Number", "ActiveYN" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        DataSet ds = new DataSet();
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixRegistersCrewApiVessel.APIVesselSearch(txtIMONumber.Text.Trim(), sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            gvAPI.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvAPI", "Api Vessels", alCaptions, alColumns, ds);

        gvAPI.DataSource = ds;
        gvAPI.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void gvAPI_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
            
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }

    private bool IsValidVessel(string Vessel)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        RadGrid _gridView = gvAPI;

        if (Vessel.Trim() == string.Empty || Vessel.Trim().ToUpper() == "DUMMY")
            ucError.ErrorMessage = "Vessel is required";

        return (!ucError.IsError);
    }

    protected void gvAPI_SortCommand(object sender, GridSortCommandEventArgs e)
    {

        ViewState["SORTEXPRESSION"] = e.SortExpression.Replace(" ASC", "").Replace(" DESC", "");
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

    protected void gvAPI_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string vesselid = ((UserControlCommonVessel)e.Item.FindControl("ddlVesselAdd")).SelectedVessel;
                int isactive = (((RadCheckBox)e.Item.FindControl("chkActiveYNAdd")).Checked.Equals(true)) ? 1 : 0;

                if (!IsValidVessel(vesselid))                                        
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegistersCrewApiVessel.InsertAPIVessel(int.Parse(vesselid), isactive);

                gvAPI.Rebind();

                ucStatus.Text = "Added Successfully";

            }
            
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAPI_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {




            string imonumber = ((RadTextBox)e.Item.FindControl("txtIMONumberEdit")).Text;
            int id = Int32.Parse(((RadLabel)e.Item.FindControl("lblidEdit")).Text);
            int isactive = (((RadCheckBox)e.Item.FindControl("chkActiveYNEdit")).Checked.Equals(true)) ? 1 : 0;

            if (!IsValidVessel(imonumber))
            {
                e.Canceled = true;
                ucError.Visible = true;
                return;
            }

            PhoenixRegistersCrewApiVessel.UpdateAPIVessel(id,imonumber, isactive);

            gvAPI.Rebind();

            ucStatus.Text = "Updated Successfully";

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAPI_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            int id = Int32.Parse(((RadLabel)e.Item.FindControl("lblid")).Text);

            PhoenixRegistersCrewApiVessel.DeleteAPIVessel(id);

            gvAPI.Rebind();

            ucStatus.Text = "Deleted Successfully";

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}