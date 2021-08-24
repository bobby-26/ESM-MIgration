using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using SouthNests.Phoenix.PlannedMaintenance;

public partial class InventoryComponentTypeVesselMapping : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in dgComponentTypeVesselMapping.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl00");
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl01");
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('dgComponentTypeVesselMapping')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("javascript:return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListVessel.aspx?mode=custom', true);", "Vessel List", "vessel.png", "ADDVESSEL");
            MenuGridComponentType.AccessRights = this.ViewState;   
            MenuGridComponentType.MenuList = toolbargrid.Show();            

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["VESSELID"] = null;
            }

            BindData();
            SetPageNavigator();
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
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDVESSELCODE", "FLDVESSELNAME" };
            string[] alCaptions = { "Vessel Code", "Vessel Name" };


            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            DataSet ds = PhoenixInventoryComponentTypeVesselMapping.ComponentTypeVesselMappingSearch(Request.QueryString["COMPONENTTYPEID"].ToString(),
                 sortexpression, sortdirection,
                (int)ViewState["PAGENUMBER"], General.ShowRecords(null),
                ref iRowCount, ref iTotalPageCount);


            General.SetPrintOptions("dgComponentTypeVesselMapping", "Components Type Vessel", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                dgComponentTypeVesselMapping.DataSource = ds;
                dgComponentTypeVesselMapping.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, dgComponentTypeVesselMapping);
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

    protected void cmdSort_Click(object sender, EventArgs e)
    {

        try
        {
            ImageButton ib = (ImageButton)sender;

            ViewState["SORTEXPRESSION"] = ib.CommandName;
            ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void dgComponentTypeVesselMapping_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = -1;
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void dgComponentTypeVesselMapping_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
               DeleteComponenttypeVesselMapping(((Label)_gridView.Rows[nCurrentRow].FindControl("lblVesselId")).Text);
            }
            else if (e.CommandName.ToUpper().Equals("ASSIGN"))
            {
                ViewState["VESSELID"] = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblVesselId")).Text;
                CheckBoxList cbl = (CheckBoxList)ucMessage.FindControl("chkOPtion");
                cbl.Items[0].Selected = true;
                ucMessage.Visible = true;
            }
            else if (e.CommandName.ToUpper().Equals("COPY"))
            {
                string vesselid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblVesselId")).Text;
                PhoenixPlannedMaintenanceComponentTypeJob.CopyComponentTypeJob(new Guid(Request.QueryString["COMPONENTTYPEID"].ToString()), int.Parse(vesselid));
                ucStatus.Text = "Component Jobs Copied";
            }
            else
            {
                _gridView.EditIndex = -1;
                BindData();
                SetPageNavigator();
            }
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void DeleteComponenttypeVesselMapping(string vesselid)
    {
        try
        {
            PhoenixInventoryComponentTypeVesselMapping.DeleteComponentTypeVesselMapping(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , Request.QueryString["COMPONENTTYPEID"].ToString() 
                , Convert.ToInt32(vesselid));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
   
    protected void dgComponentTypeVesselMapping_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        try
        {
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void dgComponentTypeVesselMapping_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
        SetPageNavigator();
    }

    protected void dgComponentTypeVesselMapping_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void dgComponentTypeVesselMapping_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton _doubleClickButton = (LinkButton)e.Row.Cells[0].Controls[0];
                string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
                e.Row.Attributes["ondblclick"] = _jsDouble;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        try
        {
            int result;
            if (Int32.TryParse(txtnopage.Text, out result))
            {
                ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

                if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                    ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];


                if (0 >= Int32.Parse(txtnopage.Text))
                    ViewState["PAGENUMBER"] = 1;

                if ((int)ViewState["PAGENUMBER"] == 0)
                    ViewState["PAGENUMBER"] = 1;

                txtnopage.Text = ViewState["PAGENUMBER"].ToString();
            }
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        try
        {

            dgComponentTypeVesselMapping.SelectedIndex = -1;
            dgComponentTypeVesselMapping.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
            else
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetPageNavigator()
    {
        try
        {
            cmdPrevious.Enabled = IsPreviousEnabled();
            cmdNext.Enabled = IsNextEnabled();
            lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
            lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
            lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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

    private bool IsValidComponentStockItemEdit(string quantityincomponent)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        GridView _gridView = dgComponentTypeVesselMapping;
        decimal result;

        if (quantityincomponent.Trim() != "")
        {
            if (decimal.TryParse(quantityincomponent, out result) == false)
                ucError.ErrorMessage = "Enter numeric decimal";
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
            gv.Rows[0].Attributes["onclick"] = "";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            NameValueCollection nvc = Filter.CurrentPickListSelection;
            string vesselid = nvc.Get("lblVesselId").ToString();

            if ((Request.QueryString["COMPONENTTYPEID"] != null) && (Request.QueryString["COMPONENTTYPEID"] != ""))
            {
                InsertComponentTypeVesselMapping(new Guid(Request.QueryString["COMPONENTTYPEID"].ToString())
                    , int.Parse(vesselid), null
                   );
                ucStatus.Text = "Component Registered.";
            }

            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void InsertComponentTypeVesselMapping(Guid ComponentTypeId, int VesselId, string csvCopyList)
    {
        PhoenixInventoryComponentTypeVesselMapping.InsertComponentTypeVesselMapping(ComponentTypeId, VesselId, csvCopyList);
    }
    protected void ucMessage_Confirm(object sender, EventArgs e)
    {
        try
        {
            if (ucMessage.confirmboxvalue == 1)
            {
                InsertComponentTypeVesselMapping(new Guid(Request.QueryString["COMPONENTTYPEID"].ToString())
                     , int.Parse(ViewState["VESSELID"].ToString()), ucMessage.SelectedList
                    );
                ucStatus.Text = "Component Registered.";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
